using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WTAgent
{
    class ProcessClass
    {
        public Links _links { get; set; }
        public List<Type2> type { get; set; }
        public List<Title> title { get; set; }
        public List<FieldActiveWindow> field_active_window { get; set; }
        public List<FieldImage> field_image { get; set; }
        public List<FieldMachine> field_machine { get; set; }
        public List<FieldUrl> field_url { get; set; }
        public List<FieldType> field_type { get; set; }
    }
}
