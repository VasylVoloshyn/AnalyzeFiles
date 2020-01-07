using AnalyzeFiles.Domain.Entities;
using System.Collections.Generic;

namespace AnalyzeFiles.Domain.Abstract
{
	public interface IAnalyzedFileInfoRepository
	{
		IEnumerable<AnalyzedFileInfo> AnalyzedFilesInfo { get; }
		void SaveAnalyzedFileInfo(AnalyzedFileInfo columnInfo);
	}
}
