using AnalyzeFiles.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzeFiles.Domain.DataBase
{
	public class AnalyzedFilesDBContext:DbContext
	{
		public AnalyzedFilesDBContext() : base("AnalyzeFilesDB") { }
		public DbSet<AnalyzedFileInfo> AnalyzedFilesInfo { get; set; }
		public DbSet<AnalyzedColumnInfo> AnalyzedColumnInfo { get; set; }
	}
}
