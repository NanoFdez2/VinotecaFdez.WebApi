using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinoteca.Abstractions;

namespace Vinoteca.Entities
{
    public class VinosPorBodegas : IEntidad
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Bodega))]
        public int BodegaId { get; set; }
        [ForeignKey(nameof(Vino))]
        public int VinoId { get; set; }
        public virtual Bodega Bodega { get; set; }
        public virtual Vino Vino { get; set; }

    }
}
