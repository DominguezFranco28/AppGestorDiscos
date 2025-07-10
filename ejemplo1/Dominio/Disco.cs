using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ejemplo1.Dominio
{
    class Disco
    {
        public int Id{ get; set; }
        public string Titulo { get; set; }
        public string UrlImagen { get; set; }
        public Estilo Estilo { get; set; }
        public TipoEdicion TipoEdicion { get; set; }
    }
}
