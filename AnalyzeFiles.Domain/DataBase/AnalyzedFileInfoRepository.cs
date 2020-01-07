using AnalyzeFiles.Domain.Abstract;
using AnalyzeFiles.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzeFiles.Domain.DataBase
{
	public class AnalyzedFileInfoRepository : IAnalyzedFileInfoRepository
	{
		private AnalyzedFilesDBContext context = new AnalyzedFilesDBContext();
		public IEnumerable<AnalyzedFileInfo> AnalyzedFilesInfo { get { return context.AnalyzedFilesInfo; } }

		public void SaveAnalyzedFileInfo(AnalyzedFileInfo columnInfo)
		{
			throw new NotImplementedException();
		}
	}
}
