using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WTAgent
{
    class ScreenClass
    {
        public Links _links { get; set; }
        public List<Type2> type { get; set; }
        public List<Title> title { get; set; }
        public List<FieldActiveWindow> field_active_window { get; set; }
        public List<FieldImage> field_image { get; set; }
        public List<FieldMachine> field_machine { get; set; }
        public List<FieldUrl> field_url { get; set; }
        public List<FieldType> field_type { get; set; }
        public Embedded _embedded { get; set; }
    }

    public class Embedded
    {
        [JsonProperty("http://localhost/rest/relation/node/process_and_screen/field_image")]
        public List<NodeFileImage> url { get; set; } 
    }

    public class NodeFileImage
    {
        public Links3 _links { get; set; }
        public List<Uuid3> uuid { get; set; }
        public string display { get; set; }
        public string description { get; set; }
        public string alt { get; set; }
        public string title { get; set; }
    }

    public class Links3
    {
        public Self3 self { get; set; }
        public Type3 type { get; set; }
    }

    public class Uuid3
    {
        public string value { get; set; }
    }

    public class Self3
    {
        public string href { get; set; }
    }

    public class Type
    {
        public string href { get; set; }
    }

    public class FieldImageUrl
    {
        public string href { get; set; }
    }

    public class Links
    {
        public Type type { get; set; }
        public Self self { get; set; }

        [JsonProperty("http://localhost/rest/relation/file/image/uid")]
        public List<FieldImageUrl> url { get; set; }
    }

    public class Type2
    {
        public string target_id { get; set; }
    }

    public class Title
    {
        public string value { get; set; }
    }

    public class FieldActiveWindow
    {
        public string value { get; set; }
    }

    public class FieldImage
    {
        public string value { get; set; }
    }

    public class FieldMachine
    {
        public string value { get; set; }
    }

    public class FieldUrl
    {
        public string value { get; set; }
    }

    public class FieldType
    {
        public string value { get; set; }
    }
}
