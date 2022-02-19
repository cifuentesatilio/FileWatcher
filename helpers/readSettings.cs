using System;
using System.Collections.Generic;

namespace folderWatcher.helpers
{
	public class readSettings
	{
		private string _pathToWatch;
		private string _masterFileName;
		private string _processedFolder;
		private string _rejectedFolder;

		public string rejectedFolder
		{
			get
			{
				return (_rejectedFolder == "") ? "Not Applicable" : _rejectedFolder;
			}
			set
			{
				_rejectedFolder = value;
			}
		}

		public string processedFolder
		{
			get
			{
				return (_processedFolder == "") ? "Processed" : _processedFolder;
			}
			set
			{
				_processedFolder = value;
			}
		}


		public string masterFileName
		{
			get
			{
				return (_masterFileName == "") ? "master.xlsx" : _masterFileName;
			}
			set
			{
				_masterFileName = value;
			}
		}

		public string pathToWatch
		{
			get
			{
				return (_pathToWatch == "") ? (AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "storage") : _pathToWatch;
			}
			set
			{
				_pathToWatch = value;
			}
		}

		public List<string> _extensions
		{
			get
			{
				var exts = new List<string>();
				exts.Add(".xlsx");
				exts.Add(".xls");
				exts.Add(".xlsm");
				exts.Add(".xlsb");

				return exts;
			}
		}
	}
}
