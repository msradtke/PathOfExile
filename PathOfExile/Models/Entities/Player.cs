using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathOfExile.Models.Entities
{
    public class Player
    {
        public string Name { get; set; }
        public string Notes { get; set; }
        public DateTime LastContact { get; set; }
        public DateTime FirstContact { get; set; }
        public bool Contacted { get; set; }

    }
}
