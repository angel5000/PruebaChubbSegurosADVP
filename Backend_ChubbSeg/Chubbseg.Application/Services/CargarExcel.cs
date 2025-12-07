using AutoMapper;
using Azure.Core;
using Chubbseg.Application.DTOS;
using Chubbseg.Application.Interfaces;
using Chubbseg.Domain.Entidades;
using Chubbseg.Infrastructure.FileExcel;
using Chubbseg.Infrastructure.FileUpload;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        async public Task<BaseResponse<bool>> ProcesarArchivo<T> (IFormFile archivo) where T : class, new()
        {
            var response = new BaseResponse<bool>();
            try
            {
                var extension = Path.GetExtension(archivo.FileName).ToLower();
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
                var usuariosMapeados = usuarios.Cast<AseguradosRequestDTO>().ToList();
                var segurosResponse = await _seguros.ListaSeguros();
                var seguros = segurosResponse.Data;

                foreach (var usuario in usuariosMapeados)
                {
                    if (string.IsNullOrWhiteSpace(usuario.CODSEGURO))
                    {
                        response.Messagemultiple.Add($"Usuario {usuario.NMBRCOMPLETO} no tiene códigos de seguro asignados");
                        continue;
                    }

                    // Registrar el usuario una sola vez
                    var resp = await _asegurados.RegistrarAsegurado(usuario);
                    if (!resp.IsSucces)
                    {
                        response.Messagemultiple.Add(resp.Message);
                        continue; // Si falla el registro de usuario, saltamos este usuario
                    }

                    // Separar los códigos de seguro por coma
                    var codigos = usuario.CODSEGURO.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                    .Select(c => c.Trim())
                                                    .ToList();

                    foreach (var codigo in codigos)
                    {
                        var seguroAsignado = seguros.FirstOrDefault(s => s.CODSEGURO.ToLower() == codigo.ToLower());

                        if (seguroAsignado == null)
                        {
                            response.Messagemultiple.Add($"Usuario {usuario.NMBRCOMPLETO} no tiene un seguro válido con CODSEGURO {codigo}");
                            continue;
                        }

                        var aseguramiento = new AseguramientoRequestDTO()
                        {
                            CEDULA = usuario.CEDULA,
                            CODSEGURO = seguroAsignado.CODSEGURO
                        };

                        var respaseg = await _asegurmiento.RegistrarAseguramiento(aseguramiento);
                        if (!respaseg.IsSucces)
                        {
                            response.Messagemultiple.Add(respaseg.Message);
                        }
                    }

                    // Si al menos un seguro fue registrado correctamente
                    response.Data = true;
                    response.IsSucces = true;
                    response.Message = "Aseguramientos registrados correctamente.";
                   
            

                }
            }
            catch (Exception ex)
            {
                response.IsSucces = false;
                response.Message = ex.Message;
            }
            return response;
        }

        async public Task<BaseResponse<bool>> RegistMasvSeguros<T>(IFormFile archivo, HttpContext context) where T : class, new()
        {
            var response = new BaseResponse<bool>();
            try
            {
                var extension = Path.GetExtension(archivo.FileName).ToLower();
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
                var SegurosMapeados = usuarios.Cast<SegurosRequestDTO>().ToList();
            

                foreach (var seguros in SegurosMapeados)
                {
                    var seguroAsignado = seguros;

                    var entidad = _mapper.Map<SegurosRequestDTO>(seguroAsignado);

                  

                    var resp = await _seguros.RegistrarSeguro(entidad, context);
                    if (!resp.IsSucces)
                    {

                        response.Messagemultiple.Add(resp.Message);

                    }

                   
                    else
                    {
                        response.Data = true;
                        response.IsSucces = true;
                        response.Message = "Aseguramient registrado correctamente.";
                    }


                }
            }
            catch (Exception ex)
            {
                response.IsSucces = false;
                response.Message = ex.Message;
            }
            return response;
        }


    }
}
