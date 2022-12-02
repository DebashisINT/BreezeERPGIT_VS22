using System;
using System.IO;

namespace ExportCSV.Classes
{
    /// <summary>
    /// Summary description for SetProperties.
    /// </summary>
    public class SetProperties
    {
        private string strTableName = "";
        private string strCsvDirOnServer = "";
        private string strExportCSVasName = "";
        private string strExportTableName = "";
        private string strExportCsvDirOnServer = "";
        private string strExportAsCsvOrText = "";
        private bool blnDropExistingTable = false;
        private bool blnSaveFileOnServer = false;
        private FileInfo fil;

        public string TableName { get { return strTableName; } set { strTableName = value; } }
        public string CsvDirOnServer { get { return strCsvDirOnServer; } set { strCsvDirOnServer = value; } }
        public bool DropExistingTable { get { return blnDropExistingTable; } set { blnDropExistingTable = value; } }
        public bool SaveFileOnServer { get { return blnSaveFileOnServer; } set { blnSaveFileOnServer = value; } }
        public FileInfo FileInformation { get { return fil; } set { fil = value; } }
        public string ExportCSVasName { get { return strExportCSVasName; } set { strExportCSVasName = value; } }
        public string ExportTableName { get { return strExportTableName; } set { strExportTableName = value; } }
        public string ExportCSVDirOnServer { get { return strExportCsvDirOnServer; } set { strExportCsvDirOnServer = value; } }
        public string ExportAsCsvOrText { get { return strExportAsCsvOrText; } set { strExportAsCsvOrText = value; } }
    }
}