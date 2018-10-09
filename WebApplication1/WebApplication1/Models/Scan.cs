using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Scan
    {
        public int ID { get; set; }
        public string FilesNum { get; set; }
        public string FilesInf { get; set; }
        public string Size { get; set; }
        public string TimeRunn { get; set; }
        public string Recognition { get; set; }
        public string Antyvirus { get; set; }
    }
}
