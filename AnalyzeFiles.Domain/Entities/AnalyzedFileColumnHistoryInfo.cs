using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzeFiles.Domain.Entities
{
	public class AnalyzedFileColumnHistoryInfo
	{
		public int Id { get; set; }		
		public int AnalyzedFileHistoryInfoId { get; set; }
		public virtual AnalyzedFileHistoryInfo AnalyzedFileHistoryInfo { get; set; }
		public int Position { get; set; }
		public int Row { get; set; }
		public string Value { get; set; }
	}
}
