using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Caternet.Data;
using Caternet.Models.Caternet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Caternet.Models;

namespace Caternet.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private const int seatsPerUser = 4;

        public EventsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            return View(await _context.Events.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .SingleOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Date,Rows,Cols")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();

                // create of seats
                List<Seat> eventSeats = new List<Seat>();
                for (var r = 1; r <= @event.Rows; r++)
                {
                    for (int c = 1; c <= @event.Cols; c++)
                    {
                        var seat = new Seat()
                        {
                            Event = @event,
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


                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.SingleOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Date")] Event @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();

                    



                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .SingleOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.SingleOrDefaultAsync(m => m.Id == id);
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<JsonResult> GetSeats(int eventId)
        {

            var eventSeats = await _context.Seats.Where(s => s.Event.Id == eventId).OrderBy(o => o.SeatNumber).ToListAsync();

            var availableSeats = eventSeats.Where(s => string.IsNullOrEmpty(s.Email)).OrderBy(o => o.SeatNumber).ToList();

            // logged in user
            var user = await _userManager.GetUserAsync(User);
            var userSeats = eventSeats.Where(s => s.User != null && s.User.Id == user.Id).Select(s => s.SeatNumber);
            var output = new
            {
                Seats = availableSeats,
                UserSeats = string.Join(",", userSeats)
            };
            return Json(output);
        }

        [HttpPost]
        public async Task<JsonResult> SaveSeat (int eventId, int seatId, string email, string attendeeName)
        {

            // get seat
            var eventSeats = await _context.Seats.Where(s => s.Event.Id == eventId).OrderBy(o => o.SeatNumber).ToListAsync();

            // get logged in user
            var user = await _userManager.GetUserAsync(User);
            var seat = eventSeats.FirstOrDefault(s => s.Id == seatId);

            // get user seats for event that are not yet assigned
            var userSeats = eventSeats.Where(s => s.User != null && s.User.Id == user.Id).ToList();
            var uSeats = userSeats.Select(s => s.SeatNumber);

            var availableSeats = eventSeats.Where(s => string.IsNullOrEmpty(s.Email)).OrderBy(o => o.SeatNumber).ToList();
            if (seat != null && !string.IsNullOrEmpty(seat.Email))
            {
                // this seat is booked
                return Json(new
                {
                    Seats = availableSeats,
                    success = false,
                    UserSeats = string.Join(",", uSeats) + $" Seat {seat.SeatNumber} is aleady taken, refresh page to get updated list of free seats"
                });
            }

            // if user assigned seats exceeds hardcoded value
            // prevent from booking more seats
            if(userSeats.Count >= seatsPerUser)
            {
                return Json(new
                {
                    Seats = availableSeats,
                    success = false,
                    message = $"Only {seatsPerUser} can be booked by single user",
                    UserSeats = string.Join(",", uSeats) + $" only {seatsPerUser} seats can be booked by user"
                });
            }

            // assign user
            seat.User = user;
            seat.Email = email;
            seat.AttendeeName = attendeeName;
            // save
            await _context.SaveChangesAsync();

            availableSeats = _context.Seats.Where(s => s.Event.Id == eventId && string.IsNullOrEmpty(s.Email)).OrderBy(o => o.SeatNumber).ToList();

            // get booked seats
            var seatsSelected = _context.Seats.Where(s => s.User.Id == user.Id && s.Event.Id == eventId).Select( s => s.SeatNumber);
            return Json(new
            {
                Seats = availableSeats,
                success = true,
                message = $"{seatsSelected.Count()} selected so far",
                UserSeats = string.Join(",", seatsSelected.ToArray())
            });


        }

    }
}
