﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace PathOfExile.Models.Entities
{
    public class HistoryEntry
    {
        
        [XmlElement]
        public string PlayerName { get; set; }
        [XmlElement]
        public DateTime TimeStamp { get; set; }
        [XmlElement]
        public string Item { get; set; }
        [XmlElement]
        public int Quantity { get; set; }
        [XmlElement]
        public int TimesContacted { get; set; }

    }
}
