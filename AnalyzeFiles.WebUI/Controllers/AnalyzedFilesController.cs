using AnalyzeFiles.Domain.Entities;
using AnalyzeFiles.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AnalyzeFiles.Models;

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
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Analyze()
        {
            List<AnalyzedFileInfo> model = new List<AnalyzedFileInfo>();
            try
            {
                model = AnalyzeFilesInFolder();
            }
            catch (Exception ex)
            {
                var errorModel = new ErrorModel();
                errorModel.ErrorText = "ERROR:" + ex.Message.ToString();
                return RedirectToAction("Index", "Error", errorModel);
            }
            
            return View(model);
        }

        public ActionResult AnalyzedFileList()
        {                       
            return View(repository.AnalyzedFilesInfo);
        }

        /// <summary>
        /// Analyze new added or changed files in the folder.
        /// </summary>
        /// <returns></returns>
        private List<AnalyzedFileInfo> AnalyzeFilesInFolder()
        {            
            List<AnalyzedFileInfo> filesInfo = new List<AnalyzedFileInfo>();
            try
            {
                if (Directory.Exists(filesLocation))
                {
                    var files = Directory.GetFiles(filesLocation);
                    foreach (var file in files)
                    {
                        if (IsFileChanged(file))
                        {
                            filesInfo.Add(AnalyzeFile(file));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return filesInfo;
        }
        /// <summary>
        /// Analyze file and save it to database
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private AnalyzedFileInfo AnalyzeFile(string file)
        {
            AnalyzedFileInfo analyzedFileInfo = new AnalyzedFileInfo();
            try
            {
                //If file with same name exists in database than file was already analyzed. 
                if (repository.AnalyzedFilesInfo.Any(f => f.Name == file))
                {
                    analyzedFileInfo = repository.AnalyzedFilesInfo.First(f => f.Name == file);
                }

                if (IsFileCSV(file))
                {
                    //Add history
                    var history = new AnalyzedFileHistoryInfo();
                    history.CreatedDate = DateTime.Now;

                    var lines = System.IO.File.ReadAllLines(file);
                    analyzedFileInfo.Name = file;
                    analyzedFileInfo.IsFileCSV = true;
                    analyzedFileInfo.Rows = lines.Count();
                    analyzedFileInfo.FileHistory.Add(history);

                    List<Dictionary<string, int>> list = new List<Dictionary<string, int>>();
                    for (int l = 0; l < lines.Count(); l++)
                    {

                        var columns = lines[l].Split(listSeparator);
                        for (int i = 0; i < columns.Count(); i++)
                        {
                            //Add columns info to the history.
                            AnalyzedFileColumnHistoryInfo columnHistoryInfo = new AnalyzedFileColumnHistoryInfo();
                            columnHistoryInfo.Row = l;
                            columnHistoryInfo.Position = i;
                            columnHistoryInfo.Value = columns[i];
                            analyzedFileInfo.FileHistory.Last().Columns.Add(columnHistoryInfo);
                            
                            //Add unique values of the column into the list
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
                    //Analyzr info with unique values and add it to Columns Table
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

                repository.SaveAnalyzedFileInfo(analyzedFileInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return analyzedFileInfo;
        }

        /// <summary>
        /// Check if file is in CSV format
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private bool IsFileCSV(string file)
        {
            try
            {
                var lines = System.IO.File.ReadAllLines(file);
                var firstLineColumnsCount = lines.First().Split(listSeparator).Count();
                foreach (var line in lines.Skip(1))
                {
                    var columnsCount = line.Split(listSeparator).Count();
                    //IF file have differnt column amount in different rows then file is not csv
                    if (firstLineColumnsCount != columnsCount)
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
                
            }
            return true;
        }

        /// <summary>
        /// Check if File was chaged. If previosly analyzed file columns amount or rows amount is not equal to current file rows amount and columns amiunt then file was changed.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private bool IsFileChanged(string file)
        {
            try
            {
                if (IsFileCSV(file))
                {
                    if (repository.AnalyzedFilesInfo.Any(f => f.Name == file))
                    {
                        var lastFile = repository.AnalyzedFilesInfo.First(f => f.Name == file).FileHistory.OrderByDescending(f => f.Id).First().Columns.GroupBy(c => c.Position);
                        var lastFileColumnsCount = lastFile.Count();
                        var lastFileRowsCount = lastFile.First().Select(f => f.Position).Count();

                        var newFileLines = System.IO.File.ReadAllLines(file);
                        var newFileColumnsCount = newFileLines.First().Split(listSeparator).Count();
                        var newFileRowsCount = newFileLines.Count();
                        if (newFileColumnsCount == lastFileColumnsCount && newFileRowsCount == lastFileRowsCount)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }
    }
}