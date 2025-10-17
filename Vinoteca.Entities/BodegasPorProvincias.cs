using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinoteca.Abstractions;

namespace Vinoteca.Entities
{
    public class BodegasPorProvincias : IEntidad
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Provincia))]
        public int ProvinciaId { get; set; }
        [ForeignKey(nameof(Vinoteca))]
        public int VinotecaId { get; set; }
        public virtual Provincia Provincia { get; set; }
        public virtual Bodega Bodega { get; set; }


    }
}
