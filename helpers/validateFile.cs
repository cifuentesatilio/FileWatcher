using System;
using System.IO;

namespace folderWatcher.helpers
{
	public  class validateFile
	{
		readSettings readSettings;
		public validateFile(readSettings _options)
		{
			readSettings = _options;
		}

		/// <summary>
		/// Validate if the file has excel extension or not
		/// </summary>
		/// <param name="url">filename url</param>
		/// <returns>bool</returns>
		public bool isExcelFile(string url)
		{
			var file_extension = Path.GetExtension(url);
			return readSettings._extensions.Contains(file_extension);
		}

		/// <summary>
		/// According with excel validation returns the folder to move the first excel.
		/// </summary>
		/// <param name="isValidExcelFile">Value validation</param>
		/// <returns>string</returns>
		public string whereToMove(bool isValidExcelFile)
		{
			return isValidExcelFile == true ? readSettings.processedFolder : readSettings.rejectedFolder;
		}

		/// <summary>
		/// Checks if the rejected folder exists, if not exists then its created otherwise just show the message
		/// </summary>
		/// <param name="url">folderToWatch</param>
		public void validateCreateRejectingFolder(string url)
		{
			try
			{
				var newRejectFolder = Path.Combine(url, readSettings.rejectedFolder);

				if (!Directory.Exists(newRejectFolder))
				{
					Console.WriteLine("Directory does not exists, starting to create the directory");
					Directory.CreateDirectory(newRejectFolder);
					Console.WriteLine("Directory created");
				}
				else
					Console.WriteLine("Directory exists");
			}
			catch (IOException ex)
			{
				Console.WriteLine("Error creating reject folder: {error}", ex.Message.ToString());
			}
			
		}

		/// <summary>
		/// Checks if the processed folder exists, if not exists then its created otherwise just show the message
		/// </summary>
		/// <param name="url">folderToWatch</param>
		public  void validateCreateProcessingFolder(string url)
		{
			try
			{
				var newProcessedFolder = Path.Combine(url, readSettings.processedFolder);

				if (!Directory.Exists(newProcessedFolder))
				{
					Console.WriteLine("Directory does not exists, starting to create the directory");
					Directory.CreateDirectory(newProcessedFolder);
					Console.WriteLine("Directory created");
				}
				else
					Console.WriteLine("Directory exists");
			}
			catch(IOException ex)
			{
				Console.WriteLine("Error creating processed folder: {error}", ex.Message.ToString());
			}
		}

		/// <summary>
		/// Convert the sheet's name adding the timestamp to create a unique value
		/// </summary>
		/// <param name="value">Name or filename's url to convert</param>
		/// <returns>String</returns>
		public string getFileNameToMove(string value)
		{
			return string.Concat(Path.GetFileNameWithoutExtension(value), "_", DateTime.Now.ToString("yyyyMMddHHmmss"), Path.GetExtension(value));
		}
	}
}
