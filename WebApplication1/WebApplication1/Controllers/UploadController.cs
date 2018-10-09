
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WebApplication1.Models.Home;
using Renci.SshNet;
using System.Text.RegularExpressions;
using WebApplication1.Models;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

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
           
            SftpClient client_sftp = new SftpClient("192.168.56.103", "Szymon", "qwerty");
            SshClient client_ssh = new SshClient("192.168.56.103", "Szymon", "qwerty");
            SftpClient client_sftp2 = new SftpClient("192.168.56.102", "Szymon", "qwerty");
            SshClient client_ssh2 = new SshClient("192.168.56.102", "Szymon", "qwerty");
            string localDirectoryUpload = @"C:\Users\szymo\Desktop\WebApplication1\WebApplication1\upload";
            string localDirectoryDownload = @"C:\Users\szymo\Desktop\WebApplication1\tmp\Scan1.txt";
            string localDirectoryDownload2 = @"C:\Users\szymo\Desktop\WebApplication1\tmp\Scan2.txt";
            string localPatternUpload = "*.zip";
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
            
            return  RedirectToAction("Index", "AddData");
           // return RedirectToAction("Files");
        }
        public string Files()
        {
            System.IO.File.Delete(@"C:\Users\szymo\Desktop\WebApplication1\tmp\Scan3.txt");
            System.IO.File.Delete(@"C:\Users\szymo\Desktop\WebApplication1\tmp\Scan4.txt");
            string readText = System.IO.File.ReadAllText(@"C:\Users\szymo\Desktop\WebApplication1\tmp\Scan1.txt");
            string readText2 = System.IO.File.ReadAllText(@"C:\Users\szymo\Desktop\WebApplication1\tmp\Scan2.txt");
            string[] pattern = { @"(?<=Scanned files: )\d+", @"(?<=Infected files: )\d+", @"(?<=Data read: )\d+.{6}", @"(?<=Time: )\d+.{8}", @".{1,200}FOUND"};
            string[] pattern2 = { @"(?<=Wszystkie pliki: )\d+", @"(?<=Pliki zar.{5}: )\d+", @"(?<=Rozmiar: )\d+.{6}", @"(?<=wykonywania: )\d+.{8}", @".{1,200}FOUND"};
            string input = readText;
            string input2 = readText2;
            for (int i = 0; i < pattern.Length; i++)
            {
              //  foreach (Match match in Regex.Matches(input, pattern[i])) --> więcej wystąpień
                    System.Text.RegularExpressions.Match x = Regex.Match(input, pattern[i]);
                System.IO.File.AppendAllText(@"C:\Users\szymo\Desktop\WebApplication1\tmp\Scan3.txt","'"+ x.Value +"'"+",");
            }
            System.IO.File.AppendAllText(@"C:\Users\szymo\Desktop\WebApplication1\tmp\Scan3.txt","'ClamAV'");
            for (int i = 0; i < pattern2.Length; i++)
            {
                //  foreach (Match match in Regex.Matches(input2, pattern2[i])) --> więcej wystąpień
                System.Text.RegularExpressions.Match x = Regex.Match(input2, pattern2[i]);
                System.IO.File.AppendAllText(@"C:\Users\szymo\Desktop\WebApplication1\tmp\Scan4.txt", "'" + x.Value + "'" + ",");
            }
            System.IO.File.AppendAllText(@"C:\Users\szymo\Desktop\WebApplication1\tmp\Scan4.txt", "'Avast'");
            return (readText) + (readText2);
          

        }
    }
}

     

