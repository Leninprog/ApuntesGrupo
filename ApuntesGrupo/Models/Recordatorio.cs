using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApuntesGrupo.Models
{
    public class Recordatorio
    {
        public string Texto { get; set; }
        public DateTime FechaHora { get; set; }
        public bool Activo { get; set; }
    }
}
