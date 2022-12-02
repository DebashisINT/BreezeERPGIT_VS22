using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.IO.Compression;

/// <summary>
/// Summary description for PersistViewStateToFileSystem
/// </summary>
public class PersistViewStateToFileSystem:Page
{
	public PersistViewStateToFileSystem()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    protected override void SavePageStateToPersistenceMedium(object state)
    {
        LosFormatter los = new LosFormatter();
        StringWriter sw = new StringWriter();
        los.Serialize(sw, state);

        StreamWriter w = File.CreateText(ViewStateFilePath);
        w.Write(sw.ToString());
        w.Close();
        sw.Close();
    
    }

    protected override object LoadPageStateFromPersistenceMedium()
    {
        // determine the file to access
        if (!File.Exists(ViewStateFilePath))
            return null;
        else
        {
            // open the file
            StreamReader sr = File.OpenText(ViewStateFilePath);
            string viewStateString = sr.ReadToEnd();
            sr.Close();

            // deserialize the string
            LosFormatter los = new LosFormatter();
            return los.Deserialize(viewStateString);
        }
    }


    public string ViewStateFilePath
    {

        get
        {
            bool isSessionId = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["isSessionId"]);
            string folderName = Path.Combine(Request.PhysicalApplicationPath, "PersistedViewState");
            string fileName = string.Empty;
            string filepath = string.Empty;
            if (!isSessionId)
            {
                HiddenField hfVSFileName = null;
                string VSFileName = "";

                // Get the HiddenField Key from the page
                hfVSFileName = FindControl(this, "hfVSFileName") as HiddenField;

                // Get the HiddenField value from the page
                string hfVSFileNameVal = GetValue(hfVSFileName.UniqueID.ToString());
                if (!string.IsNullOrEmpty(hfVSFileNameVal))
                {
                    VSFileName = hfVSFileNameVal;
                }

                if (!Page.IsPostBack)
                {
                    VSFileName = GenerateGUID();
                     hfVSFileName.Value = VSFileName;

                    //Removing files from Server
                    RemoveFilesfromServer();
                }

                fileName = VSFileName + "-" + Path.GetFileNameWithoutExtension(Request.Path).Replace("/", "-") + ".vs";
                filepath = Path.Combine(folderName, fileName);

                return filepath;
            }
            else
            {
                if (Session["viewstateFilPath"] == null)
                {

                    fileName = Session.SessionID + "-" + Path.GetFileNameWithoutExtension(Request.Path).Replace("/", "-") + ".vs";
                    filepath = Path.Combine(folderName, fileName);
                    Session["viewstateFilPath"] = filepath;
                }
                return Session["viewstateFilPath"].ToString();
            }
        }
    }

    public static Control FindControl(Control root, string controlId)
    {
        if (root.ID == controlId)
        {
            return root;
        }

        foreach (Control c in root.Controls)
        {
            Control t = FindControl(c, controlId);
            if (t != null)
            {
                return t;
            }
        }

        return null;
    }
    public string GetValue(string uniqueId)
    {
        return System.Web.HttpContext.Current.Request.Form[uniqueId];
    }

    private void RemoveFilesfromServer()
    {
        try
        {
            string folderName = Path.Combine(Request.PhysicalApplicationPath, "PersistedViewState");
            DirectoryInfo _Directory = new DirectoryInfo(folderName);
            FileInfo[] files = _Directory.GetFiles();
            DateTime threshold = DateTime.Now.AddDays(-3);
            foreach (FileInfo file in files)
            {
                if (file.CreationTime <= threshold)
                    file.Delete();
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Removing Files from Server");
        }
    }
    /// <summary>
    /// A GUID is created to store the file names
    /// </summary>
    private string GenerateGUID()
    {
        return System.Guid.NewGuid().ToString("");
    }

}
