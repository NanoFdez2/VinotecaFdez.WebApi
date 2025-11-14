using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinoteca.Abstractions;

namespace Vinoteca.Entities
{
    public class Variedad : IEntidad, IClassMethods
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Detalle { get; set; }

        public void SetNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre de la variedad no puede estar vacío.");
            Nombre = nombre;
        }
        public string GetClassName()
        {
            return string.Join(": ", this.GetType().BaseType.Name, Nombre);
        }
    }
}
