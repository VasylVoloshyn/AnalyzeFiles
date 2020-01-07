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

		public void SaveAnalyzedFileInfo(AnalyzedFileInfo fileInfo)
		{
			if (fileInfo.Id == 0)
			{
				context.AnalyzedFilesInfo.Add(fileInfo);
				foreach(var column in fileInfo.Columns)
				{
					context.AnalyzedColumnInfo.Add(column);					
				}
			}
			else
			{
				var entity = context.AnalyzedFilesInfo.Find(fileInfo.Id);
				if (entity != null)
				{
					entity.IsFileCSV = fileInfo.IsFileCSV;
					entity.Name = fileInfo.Name;
					entity.Rows = fileInfo.Rows;
					var columnsToDelete = context.AnalyzedColumnInfo.Where(c => c.AnalyzedFileInfoId == fileInfo.Id).ToArray();
					foreach(var col in columnsToDelete)
					{
						context.AnalyzedColumnInfo.Remove(col);
					}
				}
				foreach (var column in fileInfo.Columns)
				{
					if (column.Id == 0)
					{
						context.AnalyzedColumnInfo.Add(column);
					}

					else
					{
						var columnEntity = context.AnalyzedColumnInfo.Find(column.Id);
						if (columnEntity != null)
						{
							columnEntity.AnalyzedFileInfoId = column.AnalyzedFileInfoId;
							columnEntity.MaxFoundItems = column.MaxFoundItems;
							columnEntity.UniqueValues = column.UniqueValues;
						}
					}
				}
			}
			context.SaveChanges();
		}
	}
}
