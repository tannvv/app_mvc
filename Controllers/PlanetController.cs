using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Models;
using App.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace App.Controllers
{
    public class PlanetController : Controller
    {
        private readonly PlanetService _planetService;
        private readonly ILogger<PlanetController> _logger;

        // [BindProperty(SupportsGet =true, Name ="action")]
        // public string Name {set;get;}

        public PlanetController(ILogger<PlanetController> logger, PlanetService planetService)
        {
            _planetService = planetService;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PlanetInfo(int id){
            PlanetModel planet = _planetService.Where(p => p.ID==id).FirstOrDefault();
            return View("Detail", planet); // truyen model sang view
        }

    }
}