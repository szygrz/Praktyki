using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    public class UploadController : Controller
    {
        // GET: /<controller>/
        private IHostingEnvironment _hostingEnvironment;
        public UploadController (IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpPost]

        public IActionResult Index(IList <IFormFile> files)
        {
          
            foreach (IFormFile item in files){
                string filename = ContentDispositionHeaderValue.Parse(item.ContentDisposition).FileName.Trim('"');
                filename = this.EnsureFilename(filename);
                using (FileStream filestream = System.IO.File.Create(this.Getpath(filename)))
                {

                }

            }
            
            return this.Content("Dodano plik na serwer");
           
        }

        private string Getpath(string filename)
        {
            //throw new NotImplementedException();
            string path = _hostingEnvironment.WebRootPath + "\\Upload\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path + filename;
        }

        private string EnsureFilename(string filename)
        {
            // throw new NotImplementedException();
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);
            return filename;
                    
        }
    }
}
 