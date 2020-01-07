using AnalyzeFiles.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzeFiles.Domain.Abstract
{
	public interface IAnalyzedColumnInfoRepository
	{
		IEnumerable<AnalyzedColumnInfo> AnalyzedColumnsInfo { get; }
		void SaveAnalyzedColumInfo(AnalyzedColumnInfo columnInfo);
	}
}
