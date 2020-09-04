using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WTAgent
{
    public class Self
    {
        public string href { get; set; }
    }

    public class HttpLocalhostRestRelationFileImageUid
    {
        public string href { get; set; }
    }

    public class Fid
    {
        public int value { get; set; }
    }

    public class Uuid
    {
        public string value { get; set; }
    }

    public class Langcode
    {
        public string value { get; set; }
    }

    public class Self2
    {
        public string href { get; set; }
    }

    public class Type3
    {
        public string href { get; set; }
    }

    public class Links2
    {
        public Self2 self { get; set; }
        public Type3 type { get; set; }
    }

    public class Uuid2
    {
        public string value { get; set; }
    }

    public class HttpLocalhostRestRelationFileImageUid2
    {
        public Links2 _links { get; set; }
        public List<Uuid2> uuid { get; set; }
    }

    public class Filesize
    {
        public int value { get; set; }
    }

    public class Created
    {
        public DateTime value { get; set; }
        public string format { get; set; }
    }

    public class Changed
    {
        public DateTime value { get; set; }
        public string format { get; set; }
    }

    public class ResFileClass
    {
        public Links _links { get; set; }
        public List<Fid> fid { get; set; }
        public List<Uuid> uuid { get; set; }
        public List<Langcode> langcode { get; set; }
        public List<Type2> type { get; set; }
        public Embedded _embedded { get; set; }
        public List<Filename> filename { get; set; }
        public List<Uri> uri { get; set; }
        public List<Filemime> filemime { get; set; }
        public List<Filesize> filesize { get; set; }
        public List<Created> created { get; set; }
        public List<Changed> changed { get; set; }
        public List<Data> data { get; set; }
    }


}
