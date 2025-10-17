using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinoteca.Abstractions;

namespace Vinoteca.Entities
{
    public class Bodega : IEntidad
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Provincia { get; set; }
        public virtual ICollection<VinosPorBodegas> vinosPorBodegas { get; set; }
        public Bodega()
        {
            vinosPorBodegas = new HashSet<VinosPorBodegas>();
        }
    }
}
