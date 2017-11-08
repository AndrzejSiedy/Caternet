using Caternet.Models.Caternet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caternet.Models.ViewModels
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            Events = new List<Event>();
            Seats = new List<Seat>();
        }
        public List<Event> Events { get; set; }
        public List<Seat> Seats { get; set; }
    }
}
