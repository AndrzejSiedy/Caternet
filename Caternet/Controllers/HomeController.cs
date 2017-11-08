using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Caternet.Models;
using Caternet.Models.ViewModels;
using Caternet.Data;

namespace Caternet.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            HomeViewModel vm = new HomeViewModel();

            vm.Events = _context.Events.ToList();

            // seats will be requested dynamically when user selects event
            //vm.Seats = _context.Seats.ToList();
            //vm.Events.ForEach(e =>
            //{
            //    e.Seats = vm.Seats.Where(s => s.Event.Id == e.Id).ToList();
            //});

            return View(vm);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
