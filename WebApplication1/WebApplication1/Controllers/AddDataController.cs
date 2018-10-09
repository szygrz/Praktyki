using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    public class AddDataController : Controller
    {
        private readonly WebApplication1Context _context;

        public AddDataController(WebApplication1Context context)
        {
            _context = context;
           
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            string readText = System.IO.File.ReadAllText(@"C:\Users\szymo\Desktop\WebApplication1\tmp\Scan1.txt");
            string readText2 = System.IO.File.ReadAllText(@"C:\Users\szymo\Desktop\WebApplication1\tmp\Scan2.txt");
            string input = readText;
            string input2 = readText2;
            string[] ScanData = { "FilesNum", "FilesInf", "Size", "TimeRunn", "Recognition" };
            string[] ScanData2 = { "FilesNum", "FilesInf", "Size", "TimeRunn", "Recognition" };
            string[] pattern = { @"(?<=Scanned files: )\d+", @"(?<=Infected files: )\d+", @"(?<=Data read: )\d+.{6}", @"(?<=Time: )\d+.{4}", @".{1,200}FOUND" };
            string[] pattern2 = { @"(?<=Wszystkie pliki: )\d+", @"(?<=Pliki zar.{5}: )\d+", @"(?<=Rozmiar: )\d+.{6}", @"(?<=wykonywania: )\d+.{8}", @".{1,200}FOUND" };
            for (int i = 0; i < 5; i++)
            {
                Match match1 = Regex.Match(input, pattern[i]);
                Match match2 = Regex.Match(input2, pattern2[i]);
                ScanData[i] = (match1.Groups[0].Value);
                ScanData2[i] = (match2.Groups[0].Value);
            }

            Scan Av_1 = new Scan
            {
                FilesNum = ScanData[0],
                FilesInf = ScanData[1],
                Size = ScanData[2],
                TimeRunn = ScanData[3]+ " sekund",
                Recognition = ScanData[4],
                Antyvirus = "ClamAV"
            };

            Scan Av_2 = new Scan
            {
                FilesNum = ScanData2[0],
                FilesInf = ScanData2[1],
                Size = ScanData2[2],
                TimeRunn = ScanData2[3],
                Recognition = ScanData2[4],
                Antyvirus = "Avast"
            };
            _context.Scan.Add(Av_1);
            _context.Scan.Add(Av_2);
            await _context.SaveChangesAsync();
             
            return RedirectToAction("Index", "Scan");
        }
    }
}
