using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Caternet.Models.Caternet
{
    public class Seat
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Row { get; set; }
        [Required]
        public int Col { get; set; }
        public string SeatNumber { get; set; }
        public string Email { get; set; }
        public string AttendeeName { get; set; }
        public virtual Event Event {get;set;}
        public virtual ApplicationUser User { get; set; }
    }
}
