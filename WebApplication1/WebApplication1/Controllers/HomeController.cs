using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            System.Diagnostics.Process.Start("C:\\Program Files\\Oracle\\VirtualBox\\VBoxManage.exe", @"startvm Windows10");

            return View();

        }
    }
}
