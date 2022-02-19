using System;
using System.IO;
using Spire.Xls;
using Spire.Xls.Collections;

namespace folderWatcher.helpers
{
	public class dirMonitor 
	{
		private FileSystemWatcher fileSystemWatcher;
		private validateFile _validate;
		private readSettings _option;
		private string _newPath;

		public dirMonitor(readSettings option)
		{
			_option = option;
			_validate = new validateFile(option);

			fileSystemWatcher = new FileSystemWatcher();
			fileSystemWatcher.Path = _option.pathToWatch;
			fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
			fileSystemWatcher.EnableRaisingEvents = true;

			fileSystemWatcher.Changed += new FileSystemEventHandler(fileChanged);
			fileSystemWatcher.Created += new FileSystemEventHandler(fileCreated);
		}

		private void fileChanged(object sender, FileSystemEventArgs e)
		{
			if (e.ChangeType != WatcherChangeTypes.Changed) return;

			_newPath = e.FullPath;
		}

		private void fileCreated(object sender, FileSystemEventArgs e)
		{
			if (File.Exists(_newPath))
			{
				int worked = (_validate.isExcelFile(_newPath)) ? processingFile(_newPath) : rejectingFile(_newPath);
			}
		}

		private int processingFile(string url)
		{
			try
			{
				Workbook wbTo = new Workbook();
				wbTo.LoadFromFile(Path.Combine(_option.pathToWatch, _option.masterFileName));
				
				var result = GetAllWorksheets(url);
				foreach (Worksheet item in result)
				{
					var name = _validate.getFileNameToMove(item.Name);
					Worksheet targetWorkSheet = wbTo.Worksheets.Add(name);
					targetWorkSheet.CopyFrom(item);

					wbTo.Save();
				}

				wbTo.Dispose();

				return movingFiles(true, url);
			}
			catch(IOException ex)
			{
				Console.WriteLine("Error {error}", ex.Message.ToString());
				return 0;
			}
			catch(Exception ex)
			{
				Console.WriteLine("Error {error}", ex.Message.ToString());
				return 0;
			}
		}

		private int rejectingFile(string url)
		{
			Console.WriteLine("It isn't an Excel file, starting to move the file to the 'Not applicable' folder");
			return movingFiles(false, url);
		}

		private static WorksheetsCollection GetAllWorksheets(string url)
		{
			Workbook wbFrom = new Workbook();
			wbFrom.LoadFromFile(url);

			WorksheetsCollection worksheets = wbFrom.Worksheets;
			return worksheets;
		}

		private int movingFiles(bool isProcessed, string currentUrl)
		{
			try
			{
				var whereToMove = _validate.whereToMove(isProcessed);
				var folderToMove = Path.Combine(_option.pathToWatch, whereToMove);
				var fileToMove = Path.Combine(folderToMove, _validate.getFileNameToMove(currentUrl));

				File.Move(currentUrl, fileToMove, true);

				if (File.Exists(fileToMove))
					Console.WriteLine("The file was moved to the new location");

				return 1;
			}
			catch (IOException e)
			{
				Console.WriteLine("Error {error}", e.Message.ToString());
				return 0;
			}
		}
	}
}
