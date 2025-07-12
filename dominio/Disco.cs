using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Disco
    {
        public string Titulo { get; set; }
        public string UrlImagen { get; set; }
        public Estilo Estilo { get; set; }

        [DisplayName("Tipo Edición")]
        public TipoEdicion TipoEdicion { get; set; }
        public string Prueba{ get; set; }
    }
}
