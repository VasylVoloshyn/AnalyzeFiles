using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzeFiles.Domain.Entities
{
	public class AnalyzedFileInfo
	{
		public AnalyzedFileInfo()
		{
			Columns = new List<AnalyzedColumnInfo>();
		}
		public int Id { get; set; }			
		public int Rows { get; set; }
		public string Name { get; set; }
		public bool IsFileCSV { get; set; }
		public virtual ICollection<AnalyzedColumnInfo> Columns { get; set; }
		

	}
}
