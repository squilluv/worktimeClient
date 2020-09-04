using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WTAgent
{
    class FileClass
    {
        public Links _links { get; set; }
        public List<Type2> type { get; set; }
        public List<Filename> filename { get; set; }
        public List<Filemime> filemime { get; set; }
        public List<Data> data { get; set; }
        public List<Uri> uri { get; set; }
    }

    public class Uri
    {
        public string value { get; set; }
    }

    public class Filename
    {
        public string value { get; set; }
    }

    public class Filemime
    {
        public string value { get; set; }
    }

    public class Data
    {
        public string value { get; set; }
    }
}
