using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.sandigital.sharepoint.webparts.v2.Webparts.AgrupadoraPastas
{
    public class AgrupadoraPastasArquivo
    {
        public string id_arquivo { get; set; }
        public string Titulo{ get;set; }
        public string Icone { get; set; }
        public string id_pasta { get; set; }
        public string url { get; set; }
    }

    public class AgrupadoraPastasPasta
    {
        public int nivel { get; set; }
        public string idx_pasta { get; set; }
        public string idx_pai { get; set; }
        public string id_pasta { get; set; }
        public string Pai { get; set; }
        public string id_pai { get; set; }
        public string Pasta { get; set; }
        public List<AgrupadoraPastasArquivo> Arquivos { get; set; }
        public List<AgrupadoraPastasPasta> Pastas { get; set; }
        
    }

}
