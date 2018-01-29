using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OptionsBindSample.Controllers
{
    public class HomeController:Controller
    {
        private readonly Class _myClass;

        public HomeController(IOptions<Class> classAccesser)
        {
            _myClass = classAccesser.Value;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_myClass);
        }
    }
}
