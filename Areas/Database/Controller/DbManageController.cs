using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Areas.Database.Controllers
{
    [Area("Database")]
    [Route("/database-manage/[action]")]
    public class DbManageController : Controller
    {
        private readonly AppDbContext _dbContext;
        public DbManageController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [TempData]
        public string StatusMessage { get; set; }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DeleteDb(){
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> DeleteDbAsync(){
            var success = await _dbContext.Database.EnsureDeletedAsync();
            StatusMessage = success ? "Delete database success" : "Delete database failed";
            return RedirectToAction("Index");
        } 

        [HttpPost]

        public async Task<IActionResult> Migrate(){
            await _dbContext.Database.MigrateAsync();
            StatusMessage = "Create database success";
            return RedirectToAction("Index");
        } 
    }
}