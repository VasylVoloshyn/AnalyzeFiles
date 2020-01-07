using AnalyzeFiles.Domain.Entities;
using AnalyzeFiles.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace AnalyzeFiles.Controllers
{
    public class AnalyzedFilesController : Controller
    {
        private readonly string filesLocation;
        private readonly char listSeparator;
        private IAnalyzedFileInfoRepository repository;
        public AnalyzedFilesController(IAnalyzedFileInfoRepository filesRepository )
        {
            repository = filesRepository;
            filesLocation = ConfigurationManager.AppSettings["filesLocationFolder"];
            listSeparator = CultureInfo.CurrentCulture.TextInfo.ListSeparator.ToCharArray().First();
        }
        // GET: AnalyzedFiles
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Analyze()
        {            
            var model = AnalyzeFilesInFolder();
            return View(model);
        }

        public ActionResult AnalyzedFileList()
        {                       
            return View(repository.AnalyzedFilesInfo);
        }

        private List<AnalyzedFileInfo> AnalyzeFilesInFolder()
        {
            List<AnalyzedFileInfo> filesInfo = new List<AnalyzedFileInfo>();
            if (Directory.Exists(filesLocation))
            {                
                var files = Directory.GetFiles(filesLocation);
                foreach(var file in files)
                {
                    filesInfo.Add(AnalyzeFile(file));
                }
            }
            return filesInfo;
        }

        private AnalyzedFileInfo AnalyzeFile(string file)
        {
            AnalyzedFileInfo analyzedFileInfo = new AnalyzedFileInfo();
            var listSeparator = CultureInfo.CurrentCulture.TextInfo.ListSeparator.ToCharArray().First();
            if (IsFileCSV(file))
            {                
                var lines = System.IO.File.ReadAllLines(file);
                analyzedFileInfo.Name = file;
                analyzedFileInfo.IsFileCSV = true;
                analyzedFileInfo.Rows = lines.Count();

                List<Dictionary<string, int>> list = new List<Dictionary<string, int>>();

                for (int l = 0; l < lines.Count(); l++)
                {
                    var columns = lines[l].Split(listSeparator);
                    for (int i = 0; i < columns.Count(); i++)
                    {
                        if (l == 0)
                        {
                            list.Add(new Dictionary<string, int>() { { columns[i], 1 } });
                        }
                        else
                        {
                            if (!list[i].ContainsKey(columns[i]))
                            {
                                list[i].Add(columns[i], 1);
                            }
                            else
                            {
                                list[i][columns[i]]++;
                            }
                        }
                    }
                }

                for (int i = 0; i < list.Count(); i++)
                {
                    analyzedFileInfo.Columns.Add(new AnalyzedColumnInfo(list[i].Count(), list[i].First(l => l.Value == list[i].Max(m => m.Value)).Key));
                }                                

            }
            else
            {
                analyzedFileInfo.Name = file;
                analyzedFileInfo.IsFileCSV = false;                
            }
            if(repository.AnalyzedFilesInfo.Any(f=>f.Name==file))
            {
                analyzedFileInfo.Id = repository.AnalyzedFilesInfo.First(f => f.Name == file).Id;
                foreach(var c in analyzedFileInfo.Columns)
                {
                    c.AnalyzedFileInfoId = analyzedFileInfo.Id;
                }
            }
            repository.SaveAnalyzedFileInfo(analyzedFileInfo);
            return analyzedFileInfo;
        }

        private bool IsFileCSV(string file)
        {
            var listSeparator = CultureInfo.CurrentCulture.TextInfo.ListSeparator.ToCharArray().First(); 
            var lines  = System.IO.File.ReadAllLines(file);
            var firstLineColumnsCount = lines.First().Split(listSeparator).Count();
            foreach (var line in lines.Skip(1))
            {
                var columnsCount = line.Split(listSeparator).Count();
                if (firstLineColumnsCount != columnsCount)
                {
                    return false;
                }
            }
            return true;
        }
    }
}