using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinoteca.Abstractions;

namespace Vinoteca.Entities
{
    public class Variedad : IEntidad
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Detalle { get; set; }
    }
}
