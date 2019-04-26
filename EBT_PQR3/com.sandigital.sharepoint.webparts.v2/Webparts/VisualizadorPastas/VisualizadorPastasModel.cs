using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.sandigital.sharepoint.webparts.v2.Webparts.VisualizadorPastas
{
    public class VisualizadorPastasModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string docIcon { get; set; }
        public string url { get; set; }
        public int tp { get; set; }
        public string folder { get; set; }
    }

    public class VisualizadorPastaBreadcrumbModel
    {
        public string name { get; set; }
        public string url { get; set; }        
    }

}
