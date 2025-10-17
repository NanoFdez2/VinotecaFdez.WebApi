using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinoteca.Applications.Dtos.Vino
{
    public class VinoRequestDto
    {
        public int Id { get; set; }
        [StringLength(30)]
        public string Nombre { get; set; }

        public int Medida { get; set; }
        [StringLength(30)]
        public string Tipo { get; set; }
        [ForeignKey(nameof(Bodega))]
        public int BodegaId { get; set; }
        public int Anio { get; set; }
        public decimal Precio { get; set; }


    }
}
