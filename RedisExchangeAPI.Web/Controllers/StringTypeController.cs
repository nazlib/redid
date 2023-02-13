using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;

        private readonly IDatabase db;

        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(0);
        }

        public IActionResult Index()
        {
            db.StringSet("name", "nazlı bal");
            db.StringSet("visiter", 100);
            return View();
        }
        public IActionResult Show()
        {
            var namerange = db.StringGetRange("name", 0, 3);
            var name = db.StringGet("name");
            if (name.HasValue)
            {
                ViewBag.value = name.ToString();
            }
            var value = db.StringLength("name");
            //byte[] pic = default(byte[]);
            //db.StringSet("pic", pic);

            // db.StringIncrement("visiter", 10);

            // var count = db.StringDecrementAsync("visiter", 1).Result;

            db.StringDecrementAsync("ziyaretci", 10).Wait();//değişkene atamak istermiyorsak result istemiyorsak

            ViewBag.value = value.ToString();

            return View();
        }
    }
}