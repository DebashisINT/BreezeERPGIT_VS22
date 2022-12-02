using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.UI;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_ToolsUtilities_EOD_Process : System.Web.UI.Page
    {
        //GenericLogSystem oGenericLogSystem = new GenericLogSystem();
        //Converter oconverter;
        // BusinessLogicLayer. GenericLogSystem oGenericLogSystem = new BusinessLogicLayer. GenericLogSystem();
        BusinessLogicLayer.Converter oconverter;
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void CbpEOD_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            CbpEOD.JSProperties["cpLogBackupInfo"] = null;
            CbpEOD.JSProperties["cpRecycleExcelInfo"] = null;
            oconverter = new BusinessLogicLayer.Converter();
            string StrAutoGeneration = "R" + oconverter.GetAutoGenerateNo();
            string Command = e.Parameter.Split('~')[0];
            bool IsLogRelatedFile = false;
            if (Command == "EOD_LogBackUp")
            {
                try
                {
                    //   oGenericLogSystem.EODProcess_ForLog(BusinessLogicLayer. GenericLogSystem.LogType.ALL);
                    string[] filePaths = Directory.GetFiles(ConfigurationManager.AppSettings["SaveCSVsql"] + @"\LogFiles\");
                    int FileCount = filePaths.Length;
                    int FileIndex = 0;
                    string FileName = String.Empty;
                    if (FileCount == 0)
                    {
                        CbpEOD.JSProperties["cpLogBackupInfo"] = "Success";
                    }
                    else
                    {
                        while (FileCount > 0)
                        {
                            FileIndex = FileCount - 1;//For Indexing Purpose Index Start With 0.
                            FileName = Path.GetFileName(filePaths[FileIndex].ToString());
                            if (FileName == "Log_IndexTable" || FileName == "Log_MasterTable")
                            {
                                IsLogRelatedFile = true;
                            }
                            else
                            {
                                if (FileName.Contains("Trans"))
                                {
                                    IsLogRelatedFile = true;
                                }
                            }
                            FileCount = FileCount - 1;
                        }
                        if (!IsLogRelatedFile)
                            CbpEOD.JSProperties["cpLogBackupInfo"] = "Success";
                    }
                }
                catch
                {
                    CbpEOD.JSProperties["cpLogBackupInfo"] = "Failure";
                }
            }
            if (Command == "EOD_RecycleExcel")
            {
                try
                {
                    string excelPath = "~/Documents/ExcelContent/";
                    DirectoryInfo excelDirectory = new DirectoryInfo(Server.MapPath(excelPath));

                    if (excelDirectory.Exists)
                    {
                        FileInfo[] excelFiles = excelDirectory.GetFiles();
                        if (excelFiles.Length == 0)
                            CbpEOD.JSProperties["cpRecycleExcelInfo"] = "Recycled";
                        else
                        {
                            foreach (FileInfo excelFile in excelFiles)
                            {
                                File.Delete(excelDirectory.ToString() + "\\" + excelFile.ToString());
                            }
                            FileInfo[] deletedExcelFiles = excelDirectory.GetFiles();
                            if (deletedExcelFiles.Length == 0)
                            {
                                CbpEOD.JSProperties["cpRecycleExcelInfo"] = "Recycled";
                            }
                        }
                    }
                    else
                    {
                        CbpEOD.JSProperties["cpRecycleExcelInfo"] = "Directory";
                    }
                }
                catch
                {
                    CbpEOD.JSProperties["cpRecycleExcelInfo"] = "Failure";
                }
            }
        }
    }
}