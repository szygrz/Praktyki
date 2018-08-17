
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WebApplication1.Models.Home;
using Renci.SshNet;


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

            SftpClient client_sftp = new SftpClient("10.6.218.22", "Szymon", "qwerty");
            SshClient client_ssh= new SshClient("10.6.218.22", "Szymon", "qwerty");
            client_sftp.Connect();
            string localDirectoryUpload = @"C:\Users\szymo\Desktop\WebApplication1\WebApplication1\upload";
            string localDirectoryDownload = @"C:\Users\szymo\Desktop\WebApplication1\tmp\Scan1.txt";
            string localPatternUpload = "*.mp3";
            string uploadDirectory = "/C:/Users/Szymon/Desktop/Upload/";
            string downloadDirectory= "/C:/Users/Szymon/Desktop/Scan/Scan.txt";
            string[] filesUpload = Directory.GetFiles(localDirectoryUpload, localPatternUpload);
            foreach (string file in filesUpload)
            {
                using (Stream inputStream = new FileStream(file, FileMode.Open))
                {
                    client_sftp.UploadFile(inputStream, uploadDirectory + Path.GetFileName(file));
                }
            }
            System.IO.DirectoryInfo di = new DirectoryInfo(localDirectoryUpload);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            client_ssh.Connect();
            client_ssh.RunCommand("C:/Users/Szymon/Desktop/Scan.bat");
            using (Stream fileStream = System.IO.File.OpenWrite(localDirectoryDownload))
            {
                client_sftp.DownloadFile(downloadDirectory, fileStream);
            }
          
            return RedirectToAction("Files");
        }
        public string Files()
        {
            
            string readText = System.IO.File.ReadAllText(@"C:\Users\szymo\Desktop\WebApplication1\tmp\Scan1.txt");
            string readText2 = System.IO.File.ReadAllText(@"C:\Users\szymo\Desktop\WebApplication1\tmp\Scan2.txt");
            return (readText) + (readText2);
          
            
        }
        


    }

    }

     

