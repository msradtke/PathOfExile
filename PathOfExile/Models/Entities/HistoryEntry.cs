using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathOfExile.Models.Entities
{
    public class HistoryEntry
    {
        public int Id { get; set; }
        public string PlayerName { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Item { get; set; }
        public int Quantity { get; set; }
        public bool Contacted { get; set; }

    }
}
