
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions options = new ();
            options.AbsoluteExpiration = DateTime.Now.AddMinutes(30);
            _distributedCache.SetString("name", "shizuka", options);
            await _distributedCache.SetStringAsync("surname", "bal", options);//await işlem bitene kadar alt satıra geçmez
            Product product = new Product
            {
                Id = 1,
                Name = "kalem",
                Price = 10
            };
            var jsonproduct = JsonSerializer.Serialize(product);
            //await _distributedCache.SetStringAsync("product:1", jsonproduct, options);
            Byte[] byteproduct = Encoding.UTF8.GetBytes(jsonproduct);
            _distributedCache.Set("product:1", byteproduct);
            return View();
        }
        public IActionResult SHow()
        {
            Byte[] byteproduct = _distributedCache.Get("product:1");
          
            var name = _distributedCache.GetString("name");
            var productjson = Encoding.UTF8.GetString(byteproduct);//_distributedCache.GetString("product:1");
            var p = JsonSerializer.Deserialize<Product>(productjson);
            ViewBag.product = p;
            return View();
        }
        public IActionResult Remove()
        {
            _distributedCache.Remove("name");
            return View();
        }
        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/test.png");
            byte[] images = System.IO.File.ReadAllBytes(path);
            _distributedCache.Set("image", images);
            return View();
        }
        public IActionResult ImageUrl()
        {
            byte[] bytepc = _distributedCache.Get("image");
            return File(bytepc, "image/png");
        }
    }
}

