//====================================================================
// This file is generated as part of Web project conversion.
// The extra class 'PathHelper' in the code behind file in 'WebForm1.aspx.cs' is moved to this file.
//====================================================================




namespace Browsing
 {

	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Data;
	using System.Drawing;
	using System.IO;
	using System.Net;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Web;
	using System.Web.SessionState;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	public class PathHelper
	{
		protected const string httpTag = "http://";
		protected const int minUNCBackSlashCount = 4;	//	The UNC path "\\server\path\" has 4 back slashes

		static public string GetParentPath (string path)
		{
			int delimiter = 0;
			string parent = "";
			string current = path.TrimEnd(new char[] { '\\', '/'});
			if (IsHTTPPath(current))
			{
				delimiter = current.LastIndexOfAny(new char[] { '/' });
				if (-1 == delimiter)
					return path;
				
				parent = current.Substring(0, delimiter + 1);
				if (parent.Length < httpTag.Length || httpTag == parent.ToLower())
					return current + '/';
					
				return parent;
			}
			
			//	UNC Path handling
			delimiter = current.LastIndexOfAny(new char[] { '\\' });
			if (-1 == delimiter)
				return path;
				
			parent = current.Substring(0, delimiter + 1);
			string[] parts = parent.Split('\\');
			if (parts.Length <= minUNCBackSlashCount)
				return current + '\\';
				
			return parent;
		}
		
		public static bool IsHTTPPath ( string directory)
		{
			if (null == directory || directory.Length < httpTag.Length)
				return false;
				
			return httpTag == directory.Substring(0, httpTag.Length).ToLower();
		}
	}

}