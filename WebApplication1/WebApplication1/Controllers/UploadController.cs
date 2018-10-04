
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WebApplication1.Models.Home;
using Renci.SshNet;
using System;
using System.Text.RegularExpressions;

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
                        Directory.GetCurrentDirectory(), "upload",
                        file.GetFilename());

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

            }

            SftpClient client_sftp = new SftpClient("192.168.56.101", "Szymon", "qwerty");
            SshClient client_ssh = new SshClient("192.168.56.101", "Szymon", "qwerty");
            SftpClient client_sftp2 = new SftpClient("192.168.56.102", "Szymon", "qwerty");
            SshClient client_ssh2 = new SshClient("192.168.56.102", "Szymon", "qwerty");
            string localDirectoryUpload = @"C:\Users\szymo\Desktop\WebApplication1\WebApplication1\upload";
            string localDirectoryDownload = @"C:\Users\szymo\Desktop\WebApplication1\tmp\Scan.txt";
            string localDirectoryDownload2 = @"C:\Users\szymo\Desktop\WebApplication1\tmp\Scan2.txt";
            string localPatternUpload = "*.mp3";
            string uploadDirectory = "/C:/Users/Szymon/Desktop/Upload/";
            string downloadDirectory = "/C:/Users/Szymon/Desktop/Scan/Scan.txt";

            string[] filesUpload = Directory.GetFiles(localDirectoryUpload, localPatternUpload);
            client_sftp.Connect();
            foreach (string file in filesUpload)
            {
                using (Stream inputStream = new FileStream(file, FileMode.Open))
                {
                    client_sftp.UploadFile(inputStream, uploadDirectory + Path.GetFileName(file));

                }
            }
            System.IO.DirectoryInfo di = new DirectoryInfo(localDirectoryUpload);
            client_ssh.Connect();
            client_ssh.RunCommand("C:/Users/Szymon/Desktop/Scan.bat");

            using (Stream fileStream = System.IO.File.OpenWrite(localDirectoryDownload))
            {
                client_sftp.DownloadFile(downloadDirectory, fileStream);
            }
            client_ssh.Disconnect();
            client_sftp.Disconnect();

            client_sftp2.Connect();
            client_ssh2.Connect();
            foreach (string file in filesUpload)
            {
                using (Stream inputStream = new FileStream(file, FileMode.Open))
                {
                    client_sftp2.UploadFile(inputStream, uploadDirectory + Path.GetFileName(file));

                }
            }
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }

            client_ssh2.RunCommand("C:/Users/Szymon/Desktop/Scan.bat");
            using (Stream fileStream = System.IO.File.OpenWrite(localDirectoryDownload2))
            {
                client_sftp2.DownloadFile(downloadDirectory, fileStream);
            }
            client_sftp2.Disconnect();
            client_ssh2.Disconnect();
            return RedirectToAction("Files");
        }
        public string Files()
        {
            System.IO.File.Delete(@"C:\Users\szymo\Desktop\WebApplication1\tmp\Scan3.txt");
            string readText = System.IO.File.ReadAllText(@"C:\Users\szymo\Desktop\WebApplication1\tmp\Scan.txt");
            string readText2 = System.IO.File.ReadAllText(@"C:\Users\szymo\Desktop\WebApplication1\tmp\Scan2.txt");
            string pattern = @"\d";
            string input = readText;
            foreach (Match match in Regex.Matches(input, pattern))
                System.IO.File.AppendAllText(@"C:\Users\szymo\Desktop\WebApplication1\tmp\Scan3.txt", match.Value);
            return (readText) + (readText2);
          

        }
    }
}

     

