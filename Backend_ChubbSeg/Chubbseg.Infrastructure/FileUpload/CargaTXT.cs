using Chubbseg.Domain.Entidades;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Infrastructure.FileUpload
{
    public class CargaTXT : ICargaTXT
    {
        public List<T> ImportarTxt<T>(IFormFile file) where T : class, new()
        {
            var lista = new List<T>();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                string? linea;
                bool primeraLinea = true;

                var propiedades = typeof(T).GetProperties();

                while ((linea = reader.ReadLine()) != null)
                {
                    if (primeraLinea)
                    {
                        primeraLinea = false;
                        continue;
                    }

                    var campos = linea.Split('|');

                    T item = new T();

                    for (int i = 0; i < propiedades.Length; i++)
                    {
                        var prop = propiedades[i];

                        object? valorConvertido = Convert.ChangeType(campos[i], prop.PropertyType);

                        prop.SetValue(item, valorConvertido);
                    }

                    lista.Add(item);
                }
            }

            return lista;
        }
    }
}
