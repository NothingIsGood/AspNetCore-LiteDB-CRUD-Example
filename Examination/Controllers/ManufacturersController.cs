using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Examination.Models;
using LiteDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Examination.Controllers
{
    public class ManufacturersController : Controller
    {
        private IConfiguration _configuration;
        private LiteDatabase _db;

        public ManufacturersController(IConfiguration configuration)
        {
            _configuration = configuration;
            _db = new LiteDatabase(_configuration["DbName"]);
        }

        public IActionResult Index()
        {
            return View(_db.GetCollection<Manufacturer>(_configuration["ManufacturerCollection"]).FindAll());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Manufacturer product)
        {
            if (ModelState.IsValid)
            {
                _db.GetCollection<Manufacturer>(_configuration["ManufacturerCollection"]).Insert(product);
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Update(int id)
        {
            var manufacturer = _db.GetCollection<Manufacturer>(_configuration["ManufacturerCollection"]).FindOne(p => p.Id == id);
            if (manufacturer == null)
            {
                return NotFound();
            }
            return View(manufacturer);
        }

        [HttpPost]
        public IActionResult Update(Manufacturer manufacturer)
        {
            if (ModelState.IsValid)
            {
                _db.GetCollection<Manufacturer>(_configuration["ManufacturerCollection"]).Update(manufacturer);
                return RedirectToAction("Index");
            }
            return View(manufacturer);
        }

        public IActionResult Delete(int id)
        {
            var manufacturer = _db.GetCollection<Manufacturer>(_configuration["ManufacturerCollection"]).FindOne(m => m.Id == id);
            if (manufacturer == null)
            {
                return NotFound();
            }
            return View(manufacturer);
        }

        [HttpPost]
        public IActionResult Delete(Manufacturer manufacturer)
        {
            try
            {
                _db.GetCollection<Manufacturer>(_configuration["ManufacturerCollection"]).Delete(m => m.Id == manufacturer.Id);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(manufacturer);
            }
        }
    }
}