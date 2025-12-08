using AutoMapper;
using Azure.Core;
using Chubbseg.Application.DTOS;
using Chubbseg.Application.Interfaces;
using Chubbseg.Domain.Entidades;
using Chubbseg.Infrastructure.FileExcel;
using Chubbseg.Infrastructure.FileUpload;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
namespace Chubbseg.Application.Services
{
    public class CargarExcel : ICargarExcel
    {
        private readonly ICargaExcel cargaexcel;
        private readonly ICargaTXT cargatxt;
        private readonly ISegurosApplication _seguros;
        private readonly IAseguradosApplication _asegurados;
        private readonly IMapper _mapper;
        private readonly IAseguramientosApplication _asegurmiento;

        public CargarExcel(ICargaExcel upexcel, ISegurosApplication seguros, IAseguramientosApplication asegurmiento, IAseguradosApplication asegurados, ICargaTXT cargatxt, IMapper mapper)
        {
            cargaexcel = upexcel;
            _seguros = seguros;
            _asegurmiento = asegurmiento;
            _asegurados = asegurados;
            this.cargatxt = cargatxt;
            _mapper = mapper;
        }

        public async Task<BaseResponse<bool>> ProcesarArchivo<T>(IFormFile archivo) where T : class, new()
        {
            BaseResponse<bool> response = new BaseResponse<bool>();
            try
            {
                string extension = Path.GetExtension(archivo.FileName).ToLower();
                List<T> usuarios = new List<T>();

                if (extension == ".xlsx")
                {
                    usuarios = cargaexcel.SubirExcel<T>(archivo);
                }
                else if (extension == ".txt")
                {
                    usuarios = cargatxt.ImportarTxt<T>(archivo);
                }
                else
                {
                    response.Message = "Formato no soportado. Use .xlsx o .txt";
                    return response;
                }

                List<AseguradosRequestDTO> usuariosMapeados = usuarios.Cast<AseguradosRequestDTO>().ToList();

                BaseResponse<IEnumerable<SegurosResponseDTO>> 
                segurosResponse = await _seguros.ListaSeguros();
                IEnumerable<SegurosResponseDTO> seguros = segurosResponse.Data;

                foreach (AseguradosRequestDTO usuario in usuariosMapeados)
                {
                    if (string.IsNullOrWhiteSpace(usuario.CODSEGURO))
                    {
                        response.Messagemultiple.Add($"Usuario {usuario.NMBRCOMPLETO} no tiene códigos de seguro asignados");
                        continue;
                    }

                    // Registrar el usuario (Asegurado)
                    BaseResponse<bool> resp = await _asegurados.RegistrarAsegurado(usuario);
                    if (!resp.IsSucces)
                    {
                        response.Messagemultiple.Add(resp.Message);
                        continue; // Si falla el registro del asegurado, no intentamos asignarle seguros
                    }

                    // Separar los códigos de seguro por coma
                    List<string> codigos = usuario.CODSEGURO.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                            .Select(c => c.Trim())
                                                            .ToList();

                    foreach (string codigo in codigos)
                    {
                        // Buscar si el código existe en la lista de seguros cargada
                        SegurosResponseDTO seguroAsignado = seguros.FirstOrDefault(s => s.CODSEGURO.ToLower() == codigo.ToLower());

                        if (seguroAsignado == null)
                        {
                            response.Messagemultiple.Add($"Usuario {usuario.NMBRCOMPLETO} no tiene un seguro válido con CODSEGURO {codigo}");
                            continue;
                        }

                        AseguramientoRequestDTO aseguramiento = new AseguramientoRequestDTO()
                        {
                            CEDULA = usuario.CEDULA,
                            CODSEGURO = seguroAsignado.CODSEGURO
                        };

                        BaseResponse<bool> respaseg = await _asegurmiento.RegistrarAseguramiento(aseguramiento);
                        if (!respaseg.IsSucces)
                        {
                            response.Messagemultiple.Add(respaseg.Message);
                        }
                    }
                }

                response.Data = true;
                response.IsSucces = true;
                response.Message = "Proceso de carga masiva finalizado. Verifique los mensajes de error si los hubo.";
            }
            catch (SqlException ex)
            {
                throw new Exception("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Excepcion Error: " + ex.Message);
            }
            return response;
        }

        public async Task<BaseResponse<bool>> RegistMasvSeguros<T>(IFormFile archivo) where T : class, new()
        {
            BaseResponse<bool> response = new BaseResponse<bool>();
            try
            {
                string extension = Path.GetExtension(archivo.FileName).ToLower();
                List<T> listaCargada = new List<T>();

                if (extension == ".xlsx")
                {
                    listaCargada = cargaexcel.SubirExcel<T>(archivo);
                }
                else if (extension == ".txt")
                {
                    listaCargada = cargatxt.ImportarTxt<T>(archivo);
                }
                else
                {
                    response.Message = "Formato no soportado. Use .xlsx o .txt";
                    return response;
                }

                List<SegurosRequestDTO> segurosMapeados = listaCargada.Cast<SegurosRequestDTO>().ToList();

                foreach (SegurosRequestDTO seguroDto in segurosMapeados)
                {
                    // Nota: Aquí se asume que _seguros.RegistrarSeguro espera un SegurosRequestDTO
                    BaseResponse<bool> resp = await _seguros.RegistrarSeguro(seguroDto);

                    if (!resp.IsSucces)
                    {
                        response.Messagemultiple.Add($"Error al registrar seguro {seguroDto.CODSEGURO}: {resp.Message}");
                    }
                }

                response.Data = true;
                response.IsSucces = true;
                response.Message = "Carga masiva de seguros finalizada.";
            }
            catch (SqlException ex)
            {
                throw new Exception("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.IsSucces = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
    }
