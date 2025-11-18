    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinoteca.Abstractions;

namespace Vinoteca.Entities
{
    public class Vino : IEntidad, IClassMethods
    {
        public Vino()
        {
            BodegasPorProvincias = new HashSet<BodegasPorProvincias>();
            VinosPorBodegas = new HashSet<VinosPorBodegas>();
            VinosVariedades = new HashSet<VinosVariedades>();
        }
        public int Id { get; set; }
        [StringLength(50)]
        public string Nombre { get; set; }
        public int Medida { get; set; }
        public string Tipo { get; set; }
        [ForeignKey(nameof(Bodega))]
        public int BodegaId { get; set; }
        public int Anio { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }
        public virtual Bodega Bodegas { get; set; }
        public virtual ICollection<BodegasPorProvincias> BodegasPorProvincias { get; set; }
        public virtual ICollection<VinosPorBodegas> VinosPorBodegas { get; set; }
        public virtual ICollection<VinosVariedades> VinosVariedades { get; set; }

        public void SetNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre del vino no puede estar vacío.");
            Nombre = nombre;
        }
        public string GetClassName()
        {
            return string.Join(": ", this.GetType().BaseType.Name, Nombre);
        }
    }
}
