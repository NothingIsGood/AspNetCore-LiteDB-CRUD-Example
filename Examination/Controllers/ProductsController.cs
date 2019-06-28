using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Examination.Models;
using LiteDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace Examination.Controllers
{
    public class ProductsController : Controller
    {
        private IConfiguration _configuration;
        private LiteDatabase _db;

        public ProductsController(IConfiguration configuration)
        {
            _configuration = configuration;
            _db = new LiteDatabase(_configuration["DbName"]);
        }

        public IActionResult Index()
        {
            return View(_db.GetCollection<Product>(_configuration["ProductCollection"]).FindAll());
        }
        
        public IActionResult Create()
        {
            var manufacturers = _db.GetCollection<Manufacturer>(_configuration["ManufacturerCollection"]).FindAll();
            ViewBag.Manufacturers = new SelectList(manufacturers, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _db.GetCollection<Product>(_configuration["ProductCollection"]).Insert(product);
                return RedirectToAction("Index");
            }
            return View();
        }
        
        public IActionResult Update(int id)
        {
            var manufacturers = _db.GetCollection<Manufacturer>(_configuration["ManufacturerCollection"]).FindAll();
            ViewBag.Manufacturers = new SelectList(manufacturers, "Id", "Name");

            var product = _db.GetCollection<Product>(_configuration["ProductCollection"]).FindOne(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult Update(Product product)
        {
            if (ModelState.IsValid)
            {
                _db.GetCollection<Product>(_configuration["ProductCollection"]).Update(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public IActionResult Delete(int id)
        {
            var product = _db.GetCollection<Product>(_configuration["ProductCollection"]).FindOne(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult Delete(Product product)
        {
            try
            {
                _db.GetCollection<Product>(_configuration["ProductCollection"]).Delete(p => p.Id == product.Id);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(product);
            }
        }
    }
}