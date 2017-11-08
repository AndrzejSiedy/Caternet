using Caternet.Models;
using Caternet.Models.Caternet;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caternet.Data
{
    public class DbInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _context;


        public DbInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task Seed()
        {
            _context.Database.EnsureCreated();

            if (!_context.Users.Any())
            {
                // create first boss user 
                var user = new ApplicationUser { UserName = "boss@test.com", Email = "boss@test.com" };
                var result = await _userManager.CreateAsync(user, "test");

                if (!result.Succeeded)
                {
                    return;
                }

                // insert initial meetup
                var firstEvent = new Event()
                {
                    Date = DateTime.Parse("2017-12-01"),
                    Name = "First Event",
                    Cols = 10,
                    Rows = 10
                };
                _context.Events.Add(firstEvent);
                await _context.SaveChangesAsync();

                // based on initial settings create 10X10 seats, and assign them with meetup
                List<Seat> eventSeats = new List<Seat>();
                for (var r = 1; r <= firstEvent.Rows; r++)
                {
                    for (int c = 1; c <= firstEvent.Cols; c++)
                    {
                        var seat = new Seat()
                        {
                            Event = firstEvent,
                            Row = r,
                            Col = c,

                            // convert number to ASCII character
                            //  does not provide error detection. Any byte greater than hexadecimal 0x7F is decoded as the Unicode question mark ("?").
                            SeatNumber = ((char)(r + 96) + c.ToString()).ToUpper()
                    };
                        eventSeats.Add(seat);
                    }
                }

                _context.Seats.AddRange(eventSeats);
                await _context.SaveChangesAsync();


            }

        }
    }
}
