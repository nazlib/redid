using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InMemoryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            //if (string.IsNullOrEmpty(_memoryCache.Get<string>("time")))
            //{
            //    _memoryCache.Set<string>("time", DateTime.Now.ToString());
            //}
            //if(_memoryCache.TryGetValue("time", out string timecache))
            //{
                MemoryCacheEntryOptions options = new();
                options.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
                options.SlidingExpiration = TimeSpan.FromSeconds(10);
            options.Priority = CacheItemPriority.High;
            options.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _memoryCache.Set("callback", $"{key}->{value} => sebep:{reason}");
            });
            _memoryCache.Set<string>("time", DateTime.Now.ToString(),options);
            //}
            Product p = new Product { Id = 1, Name = "Kalem", Price = 200 };
            _memoryCache.Set<Product>("product:1", p);
            return View();
        }
        public IActionResult Show()
        {
            //_memoryCache.Remove("time");
            //_memoryCache.GetOrCreate<string>("time",x =>
            //{
            //    return DateTime.Now.ToString();
            //});
            //ViewBag.time = _memoryCache.Get<string>("time");
            _memoryCache.TryGetValue("time", out string timecache);
            _memoryCache.TryGetValue("callback", out string callback);
            ViewBag.time = timecache;
            ViewBag.callback = callback;

            ViewBag.product = _memoryCache.Get<Product>("product:1");
            return View();
        }
    }
}

