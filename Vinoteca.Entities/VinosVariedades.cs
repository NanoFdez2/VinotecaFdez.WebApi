using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinoteca.Abstractions;

namespace Vinoteca.Entities
{
    public class VinosVariedades : IEntidad
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Vino))]
        public int VinoId { get; set; }
        [ForeignKey(nameof(Variedad))]
        public int VariedadId { get; set; }
        public virtual Vino Vino { get; set; }
        public virtual Variedad Variedad { get; set; }
    }
}
