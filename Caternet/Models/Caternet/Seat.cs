using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caternet.Models.Caternet
{
    public class Seat
    {
        public int Id { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public string SeatNumber { get; set; }
        public virtual Event Event {get;set;}
        public virtual ApplicationUser User { get; set; }
    }
}
