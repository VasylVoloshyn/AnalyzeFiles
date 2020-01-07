using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzeFiles.Domain.Entities
{
	public class AnalyzedFileHistoryInfo
	{
		public AnalyzedFileHistoryInfo()
		{
			Columns = new List<AnalyzedFileColumnHistoryInfo>();
		}
		public int Id { get; set; }
		public DateTime CreatedDate { get; set; }
		public int AnalyzedFileInfoId { get; set; }
		public virtual AnalyzedFileInfo AnalyzedFileInfo { get; set; }
		public virtual ICollection<AnalyzedFileColumnHistoryInfo> Columns { get; set; }
	}
}
