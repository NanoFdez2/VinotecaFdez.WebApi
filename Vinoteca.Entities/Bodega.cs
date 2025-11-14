using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinoteca.Abstractions;

namespace Vinoteca.Entities
{
    public class Bodega : IEntidad, IClassMethods
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Provincia { get; set; }
        public virtual ICollection<VinosPorBodegas> vinosPorBodegas { get; set; }
        public Bodega()
        {
            vinosPorBodegas = new HashSet<VinosPorBodegas>();
        }

        public void SetNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre de la bodega no puede estar vacío.");
            Nombre = nombre;
        }
        public string GetClassName()
        {
            return string.Join(": ", this.GetType().BaseType.Name, Nombre);
        }
    }
}
