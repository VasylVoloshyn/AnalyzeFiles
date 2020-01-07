using AnalyzeFiles.Domain.Abstract;
using AnalyzeFiles.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzeFiles.Domain.DataBase
{
	public class AnalyzedColumnInfoRepository : IAnalyzedColumnInfoRepository
	{
		private AnalyzedFilesDBContext context = new AnalyzedFilesDBContext();
		public IEnumerable<AnalyzedColumnInfo> AnalyzedColumnsInfo { get { return context.AnalyzedColumnInfo; } }	
		public void SaveAnalyzedColumInfo(AnalyzedColumnInfo columnInfo)
		{
			if(columnInfo.Id ==0)
			{
				context.AnalyzedColumnInfo.Add(columnInfo);				
			}
			else
			{
				var entity = context.AnalyzedColumnInfo.Find(columnInfo.Id);
				if(entity!=null)
				{
					entity.AnalyzedFileInfoId = columnInfo.AnalyzedFileInfoId;
					entity.MaxFoundItems = columnInfo.MaxFoundItems;
					entity.UniqueValues = columnInfo.UniqueValues;
				}
			}
			context.SaveChanges();
		}
	}
}
