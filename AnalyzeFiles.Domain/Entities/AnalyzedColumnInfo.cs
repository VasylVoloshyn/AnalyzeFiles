using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzeFiles.Domain.Entities
{
	public class AnalyzedColumnInfo
	{
		public int Id { get; set; }
		public int AnalyzedFileInfoId { get; set; }
		public virtual AnalyzedFileInfo AnalyzedFileInfo { get; set; }		
		public int UniqueValues { get; set; }
		public string MaxFoundItems { get; set; }
		public AnalyzedColumnInfo()
		{

		}
		public AnalyzedColumnInfo(/*int analyzedFileInfoId, */int uniqueValues, string maxFoundInems)
		{
			//AnalyzedFileInfoId = analyzedFileInfoId;
			UniqueValues = uniqueValues;
			MaxFoundItems = maxFoundInems;
		}
	}
}
