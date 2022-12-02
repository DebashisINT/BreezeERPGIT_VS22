//====================================================================
// This file is generated as part of Web project conversion.
// The extra class 'FileList' in the code behind file in 'WebForm1.aspx.cs' is moved to this file.
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

	public class FileList
	{
		protected string[] _directories = new string[0];
		protected string[] _files = new string[0];
		protected string _basedirectory = null;
		protected const string httpTag = "http://";
		
		/// <summary>
		/// Path to the directory that is being browsed
		/// </summary>
		public string BaseDirectory
		{
			get { return _basedirectory; }
		}
		
		/// <summary>
		/// Array of the directories that were found
		/// </summary>
		public string[] Directories
		{
			get { return _directories; }
		}
		
		/// <summary>
		/// Array of the files that matched the specified filter(s)
		/// </summary>
		public string[] Files
		{
			get { return _files; }
		}
		
		public static string[] ExtractHrefsFromPage (byte[] pagedata)
		{
			string htmlpage = ASCIIEncoding.ASCII.GetString(pagedata);
			Regex hrefparser = new Regex("href\\s*=\\s*(?:\"(?<1>[^\"]*)\"|(?<1>\\S+))",
				RegexOptions.IgnoreCase | RegexOptions.Compiled);
			
			ArrayList results = new ArrayList();
			for ( Match found = hrefparser.Match(htmlpage); found.Success; found = found.NextMatch())
			{
				results.Add( HttpUtility.UrlDecode(found.Groups[1].Value));
			}
		
			string[] hrefs = new string[results.Count];
			results.CopyTo(hrefs);
			return hrefs;
		}
		
		protected string ExtractUrifromUrl ( string urlstring)
		{
			int delimiter = urlstring.IndexOf("/", httpTag.Length);
			if (-1 == delimiter)
				return "/";
				
			return urlstring.Substring(delimiter);
		}
		
		static public string GetPath (string file)
		{
			int delimiter = file.LastIndexOfAny(new char[] { '\\', '/' });
			return file.Substring(0, delimiter + 1);
		}

		protected static string GetContentType(string url)
		{
			HttpWebRequest headRequest = WebRequest.Create(url) as HttpWebRequest;
			headRequest.Method = "HEAD";
				
			HttpWebResponse response = headRequest.GetResponse() as HttpWebResponse;
			return response.ContentType;
		}

		public static bool IsHTMLContent(string url)
		{
			string content = GetContentType(url);
			
			if (null == content || -1 == content.ToLower().IndexOf("html"))
				return false;
				
			return true;	
		}
		
		protected void ParseHTTPPage ( string directory, string filter)
		{
			try
			{
				string[] filterlist = filter.Split(';');
				if (!FileList.IsHTMLContent(directory))
					directory = GetPath(directory);

				string uristring = ExtractUrifromUrl(directory);
		
				WebClient client = new WebClient();
				byte[] pagedata = client.DownloadData(directory);
				string[] hrefs = ExtractHrefsFromPage(pagedata);
				
				ArrayList dirs = new ArrayList();
				ArrayList files = new ArrayList();
				foreach (string uri in hrefs)
				{
					if (uri.EndsWith("/"))
					{
						//	handle the directory
						if (uri.StartsWith(uristring))
							dirs.Add(uri.Substring(uristring.Length).Trim('/'));
					}
					else
					{
						string file = Path.GetFileName(uri);
						foreach (string query in filterlist)
						{
							if (System.Text.RegularExpressions.Regex.IsMatch(file, "." + query.Replace(".", "\\."), RegexOptions.IgnoreCase))
							{
								files.Add(file);
								break;
							}
						}
					}
				}
				
				_directories = new string[dirs.Count];
				dirs.CopyTo(_directories);
				System.Array.Sort(_directories);
				
				_files = new string[files.Count];
				files.CopyTo(_files);
				System.Array.Sort(_files);
				
				_basedirectory = directory;
				if (!_basedirectory.EndsWith("/"))
					_basedirectory += "/";
			}
			catch(Exception except)
			{
				System.Diagnostics.Trace.WriteLine("Exception parsing URL: " + except.Message);
			}
			return;
		}
		
		protected void PasreUNCPage ( string directory, string filter)
		{
			try
			{
				if (FileAttributes.Directory != (FileAttributes.Directory & File.GetAttributes(directory)))
					directory = GetPath(directory);
			}
			catch(Exception)
			{
				return;
			}
			
			try
			{
				_directories = RelativePaths( Directory.GetDirectories(directory, "*.*"));
				System.Array.Sort(_directories);
			}
			catch(Exception except)
			{
				System.Diagnostics.Trace.WriteLine("Exception parsing directory: " + except.Message);
			}
			
			try
			{
				_files = new string[0];
				string[] extensions = filter.Split(';');
				foreach (string ext in extensions)
				{ 
					string[] foundfiles = RelativePaths( Directory.GetFiles(directory, ext));
					if (foundfiles.Length > 0)
					{
						string[] newlist = new string[_files.Length + foundfiles.Length];
						_files.CopyTo(newlist, 0);
						foundfiles.CopyTo(newlist, _files.Length);
						_files = newlist;
						System.Array.Sort(_files);
					}
				}
			}
			catch(Exception except)
			{
				System.Diagnostics.Trace.WriteLine("Exception parsing files: " + except.Message);
			}

			_basedirectory = directory;
			if (!_basedirectory.EndsWith("\\"))
				_basedirectory += "\\";
			return;
		}
		
		public FileList ( string directory, string filter)
		{
			if (null == directory || 0 == directory.Length)
			{
				_files = new string[0];
				_directories = new string[0];
				_basedirectory = "";
				return;
			}
			
			if (PathHelper.IsHTTPPath(directory))
			{
				ParseHTTPPage(directory, filter);
			}
			else
			{
				PasreUNCPage(directory, filter);
			}
			return;
		}
		
		protected string[] RelativePaths (string[] filelist)
		{
			if (null == filelist)
				return new string[0];
				
			string[] result = new string[filelist.Length];
			for (int index = 0; index < filelist.Length; index++)
				result[index] = Path.GetFileName(filelist[index]);
				
			return result;
		}
	}

}