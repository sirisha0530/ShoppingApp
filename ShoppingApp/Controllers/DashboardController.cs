using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using Services.Dashboard;
using Domain.Model.Dashboard;
using ShoppingApp.Models.Dashboard;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using ShoppingApp.Models;
using System.Linq;

namespace ShoppingApp.Controllers
{
    [Route("[controller]")]
    //[ApiController]
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IWebHostEnvironment _hostEnvironment;



        public DashboardController(IDashboardRepository dashboardRepository, IWebHostEnvironment hostEnvironment)
        {
            _dashboardRepository = dashboardRepository;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet("GetProduct")]
        public IActionResult GetProduct()
        {

            try
            {
                List<Productlist> productlist = new List<Productlist>();
                IEnumerable<Productlist> products = productlist;
                products = _dashboardRepository.GetAllProduct();

                return View("~/Views/Dashboard/GetProduct.cshtml",products);


            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }


        [HttpGet("CreateProductpage")]
        public IActionResult CreateProductpage()
        {
            try
            {
                return View();
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }

        }

        //[HttpPost("CreateProduct")]
        //public IActionResult CreateProduct(Productlistservice model)
        //{
        //    try
        //    {

        //        _dashboardRepository.InsertProduct(model);
        //        return View("", true);
        //    }
        //    catch (Exception exception)
        //    {
        //        return BadRequest(new { success = false, exception.Message });
        //    }

        //}

        [HttpDelete("DeleteProduct")]
        public void DeleteProduct(long id)
        {
            try
            {
                _dashboardRepository.DeleteProduct(id);
                GetProduct();
            }
            catch (Exception exception)
            {

            }
        }


        [HttpPost("CreateProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(ProductlistDTO model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);

                Productlistservice employee = new Productlistservice
                {
                    ProductName = model.ProductName.ToUpper(),
                    ProductDiscription = model.ProductDiscription.ToUpper(),

                    ProductPrice = model.ProductPrice,
                    ProductImage = uniqueFileName,
                };
                _dashboardRepository.InsertProduct(employee);
                GetProduct();
                //        return View("", true);
            }
            return View("~/Views/Dashboard/GetProduct.cshtml");
        }

        private string UploadedFile(ProductlistDTO model)
        {
            string uniqueFileName = null;

            if (model.ProductImage != null)
            {
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProductImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ProductImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        [HttpGet]
        public IActionResult SearchProduct(string search)
        {
            string Searchproduct = search.ToUpper();
            List<Productlist> productlist = new List<Productlist>();
            List<Productlist> Showproductlist = new List<Productlist>();

            IEnumerable<Productlist> products = productlist;

            products = _dashboardRepository.GetAllProduct();
            foreach (Productlist product in products)
            {

                if (product.ProductName.ToUpper().Contains(Searchproduct) || product.ProductDiscription.ToUpper().Contains(Searchproduct))
                {
                    Showproductlist.Add(product);
                }
            }
            IEnumerable<Productlist> product2 = Showproductlist;

            return View("~/Views/Dashboard/GetProduct.cshtml", product2);
        }

    }
}
