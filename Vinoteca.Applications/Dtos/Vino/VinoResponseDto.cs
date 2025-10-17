using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinoteca.Applications.Dtos.Vino
{
    public class VinoResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Medida { get; set; }
        public string Tipo { get; set; }
        public int BodegaId { get; set; }
        public int Anio { get; set; }
        public decimal Precio { get; set; }
    }
}
