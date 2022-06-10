using bizLandTask.DAL;
using bizLandTask.Models;
using bizLandTask.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace bizLandTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext db;
        public HomeController(AppDbContext _db) 
        {
            db = _db;
        }
        public IActionResult Index()
        {
            PortfolioViewModels pvm = new PortfolioViewModels
            {
                portfolios=db.Portfolios.ToList(),
                categories=db.Categories.ToList()
            };
            return View(pvm);
        }
    }
}
