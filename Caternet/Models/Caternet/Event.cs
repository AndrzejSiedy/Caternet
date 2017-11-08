using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caternet.Models.Caternet
{
    public class Event
    {
        public int Id { get; set; }
        /// <summary>
        /// Event date
        /// </summary>
        public DateTime Date { get; set; }

        public string Name { get; set; }
        /// <summary>
        /// number of rows in venue
        /// </summary>
        public int Rows { get; set; }
        /// <summary>
        /// number of seats in a row
        /// </summary>
        public int Cols { get; set; }

        public virtual ICollection<Seat> Seats { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}
