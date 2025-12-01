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

        private readonly IAseguramientosApplication _asegurmiento;

        public CargarExcel(ICargaExcel upexcel, ISegurosApplication seguros, IAseguramientosApplication asegurmiento, IAseguradosApplication asegurados, ICargaTXT cargatxt)
        {
            cargaexcel = upexcel;
            _seguros = seguros;
            _asegurmiento = asegurmiento;
            _asegurados = asegurados;
            this.cargatxt = cargatxt;
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
                    var seguroAsignado = seguros
                        .FirstOrDefault(s =>
                            usuario.EDAD >= s.EDADMIN &&
                            usuario.EDAD <= s.EDADMAX
                        );

                    if (seguroAsignado == null)
                    {
                        // Si no se encuentra seguro según la edad
                        response.Messagemultiple.Add($"usuario {usuario.NMBRCOMPLETO} no aplica a seguros");
                        continue; // lo saltamos o puedes marcarlo como error
                    }

                    var aseguramiento = new AseguramientoRequestDTO()
                    {
                        CEDULA = usuario.CEDULA,
                        CODSEGURO = seguroAsignado.CODSEGURO,

                    };

                    var resp = await _asegurados.RegistrarAsegurado(usuario);
                    if (!resp.IsSucces)
                    {

                        response.Messagemultiple.Add(resp.Message);

                    }

                    var respaseg = await _asegurmiento.RegistrarAseguramiento(aseguramiento);
                    if (!resp.IsSucces)
                    {

                        respaseg.Messagemultiple.Add(resp.Message);

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
