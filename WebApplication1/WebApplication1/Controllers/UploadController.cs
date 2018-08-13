
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WebApplication1.Models.Home;

namespace WebApplication1
{
    public class UploadController : Controller
    {
        private readonly IFileProvider fileProvider;

        public UploadController(IFileProvider fileProvider)
        {
            this.fileProvider = fileProvider;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                return Content("files not selected");

            foreach (var file in files)
            {

                var path = Path.Combine(
                        Directory.GetCurrentDirectory(),"upload",
                        file.GetFilename());

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                
            }
      
            return RedirectToAction("Files");
        }
        public string Files()
        {
            return "This is the Welcome action method...";
        }


    }

    }

     

