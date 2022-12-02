using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using System.Runtime.Serialization;
using System.IO;
using System.Drawing.Imaging;
using System.Collections.Generic;
//using System.IO.Compression;
using Ionic.Zip;
using BusinessLogicLayer;
/// <summary>
/// Summary description for GenericCsvExport
/// </summary>
public class GenericCsvExport
{
    #region Enum
    public enum CSVExportType { Dataset, Query }
    #endregion

    #region Properties
    #endregion

    #region Global Variable
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);  MULTI
    DBEngine oDBEngine = new DBEngine();
        string fileServerPath = ConfigurationManager.AppSettings["SaveCSVsql"].ToString();
        ZipFile zip = new ZipFile();
        private string importStatus = "0";
        string compressFileDirectory = null;
        bool FileWritten = false;
    #endregion

	public GenericCsvExport()
	{
       
		//
		// TODO: Add constructor logic here
		//

	}

    public void ExportFile(DataTable DtRecord,String ExportedFilePath,String ExportedFileName)
    {
       FileWritten = WriteFile(DtRecord,ExportedFilePath);
        if (FileWritten)
        {
             DownloadFile(ExportedFilePath,ExportedFileName);
         
        }

    }

    public bool WriteFile(DataTable Dt,string ExportedFilePath)
    {
        try
        {
            using (StreamWriter sw = new StreamWriter(ExportedFilePath, true))
            {
           
                    int colCount = Dt.Columns.Count;

                    foreach (DataRow dr in Dt.Rows)
                    {
                        for (int j = 0; j < colCount; j++)
                        {

                            if (!Convert.IsDBNull(dr[j]))
                                sw.Write(dr[j]);
                               

                        }

                        sw.Write(sw.NewLine);
                    }

 
            }
            return true;
        }
        catch(Exception ex)
        {
            return false;
        }
          

    }
    public bool WriteFile(DataTable Dt,string ExportedFilePath,string Separator)//WriteFile With Separator
    {
        try
        {
            using (StreamWriter sw = new StreamWriter(ExportedFilePath, true))
            {
           
                    int colCount = Dt.Columns.Count;

                    foreach (DataRow dr in Dt.Rows)
                    {
                        for (int j = 0; j < colCount; j++)
                        {

                            if (!Convert.IsDBNull(dr[j]))
                                sw.Write(dr[j]);
                                sw.Write(Separator);


                        }

                        sw.Write(sw.NewLine);
                    }

 
            }
            return true;
        }
        catch(Exception ex)
        {
            return false;
        }
          

    }


     public void DownloadFile(string ExportedFilePath,string FileName)
    {

         try
        {
           FileInfo fileInfo = new FileInfo(ExportedFilePath);
            
            if (fileInfo.Exists)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
                HttpContext.Current.Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                HttpContext.Current.Response.ContentType = "application/unknown";
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.WriteFile(fileInfo.FullName);
                //Response.TransmitFile(fileInfo.FullName);
                HttpContext.Current.Response.End();

            }
        }
        catch { }

    }
    public string ZipFile(string ExportedFilePath, //The path Of the File Which Will Be Exported
                          string FileName,         //The Name of The File With Extension Which Will Be Exported
                          string ZipFileName       //The Name Of the Zip file To be made 
                          )
    {
        string inputFilePath = null;
        string inputFileName = null;
        string inputFileExt = null;  
        string compressFilePath = null; 
 
        inputFileName = Path.GetFileName(FileName);
        inputFilePath = Path.GetDirectoryName(ExportedFilePath);
        inputFileExt = Path.GetExtension(FileName).ToString();

                if (inputFileExt != ".zip" && inputFileExt != ".rar")
                {
                    //*******Uploaded Normal File Save in Server Path With Formatted Compress File********
                     compressFilePath = inputFilePath +"\\"+ ZipFileName + ".zip";
                    //using (ZipFile zip = new ZipFile())
                    //{
                         //// Sample===zip.AddFile("D:\\Reports\\2008-Regional-Sales-Report.pdf", "files");
                        zip.AddFile(ExportedFilePath, ZipFileName);
                        zip.Save(compressFilePath);
                    //}
                    File.Delete(ExportedFilePath);
                }
        return(compressFilePath); //Returning the Path Of the File which has been compressed
    }
    


    
}
