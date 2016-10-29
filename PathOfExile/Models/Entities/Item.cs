using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
namespace PathOfExile.Models.Entities
{
    public class Item
    {
        [XmlElement]
        public string ItemName { get; set; }
        [XmlElement]
        public string Url { get; set; }
        [XmlElement]
        public string MessageSingle { get; set; }
        [XmlElement]
        public string MessageMulti { get; set; }
    }
}
