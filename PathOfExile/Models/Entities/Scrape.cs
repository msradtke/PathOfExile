using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathOfExile.Models.Entities
{
    public class Scrape
    {
        public string PlayerName { get; set; }
        public string Item { get; set; }
        public int Quantity { get; set; }
        public bool Attempted { get; set; }


    }
}
