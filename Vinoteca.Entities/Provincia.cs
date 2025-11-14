using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinoteca.Abstractions;
using Vinoteca.Applications.Dtos.Provincia;

namespace Vinoteca.Entities
{
    public class Provincia : IEntidad, IClassMethods
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public Provincia devolverProvincia(ProvinciaRequestDto prov)
        {
            Provincia nuevaProv = new Provincia();
            nuevaProv.Nombre = prov.Nombre;
            nuevaProv.Id = prov.Id;
            return nuevaProv;

        }

        public void SetNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre de la provincia no puede estar vacío.");
            Nombre = nombre;
        }
        public string GetClassName()
        {
            return string.Join(": ", this.GetType().BaseType.Name, Nombre);
        }
    }
}
