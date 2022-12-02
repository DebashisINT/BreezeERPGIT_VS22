using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Drawing;
using BusinessLogicLayer;

/// <summary>
/// Summary description for GenericExcelExport
/// </summary>
public class GenericExcelExport
{
    DBEngine oDBEngine;
    string strExcelConn = String.Empty;

    public GenericExcelExport()
    {
        //oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"].ToString());  MULTI
        oDBEngine = new DBEngine();
    }
    public void ExportToExcel(string[] ColumnType, string[] ColumnSize, string[] ColumnWidthSize, System.Data.DataTable Dt, string DirectoryPath, string Version, string FileName, string[] HeaderText, string[] FindWordChangeRowColor)
    {
        DirectoryPath = DirectoryPath + "ExcelContent/";
        DirectoryInfo directoryInfo = new DirectoryInfo(DirectoryPath);
        if (!directoryInfo.Exists)
        {
            directoryInfo.Create();
        }
        string FileExtension = String.Empty;
        if (Version == "2003") FileExtension = ".xls";
        if (Version == "2007") FileExtension = ".xlsx";
        string TempPath = DirectoryPath + FileName + "_Temp" + FileExtension;
        string strExcelConn = String.Empty;

        string[] OriginalColumnName = new string[Dt.Columns.Count];
        string[] ColumnName = new string[Dt.Columns.Count];
        int i = 0;
        foreach (DataColumn c in Dt.Columns)
        {
            OriginalColumnName[i] = Dt.Columns[i].ColumnName;
            ColumnName[i] = "Column" + i;
            Dt.Columns[c.ColumnName].ColumnName = "Column" + Convert.ToString(i++);
        }

        if (Version == "2003")
        {
            // Excel 97-2003
            strExcelConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + TempPath + ";Extended Properties='Excel 8.0;HDR=Yes'";
        }
        if (Version == "2007")
        {
            //Excel 97
            strExcelConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + TempPath + ";Extended Properties='Excel 12.0 Xml;HDR=Yes'";
        }
        if (Version != "2003" || Version != "2007")
        {

            int LoopCnt;
            string strCreateTable = "Create Table ExcelExport (";
            string strInsertTable = "Insert into ExcelExport (";
            string strParameters = String.Empty;
            string strCompleteCommandText = String.Empty;
            for (LoopCnt = 0; LoopCnt < ColumnName.Length; LoopCnt++)
            {
                if (ColumnType[LoopCnt] == "C")
                {
                    strCreateTable = strCreateTable + ColumnName[LoopCnt] + " Char(" + ColumnSize[LoopCnt] + "),";
                }
                if (ColumnType[LoopCnt] == "I")
                {
                    strCreateTable = strCreateTable + ColumnName[LoopCnt] + " Int,";
                }
                else if (ColumnType[LoopCnt] == "V")
                {
                    strCreateTable = strCreateTable + ColumnName[LoopCnt] + " Varchar(" + ColumnSize[LoopCnt] + "),";
                }
                else if (ColumnType[LoopCnt] == "T")
                {
                    strCreateTable = strCreateTable + ColumnName[LoopCnt] + " Text,";
                }

                else if (ColumnType[LoopCnt] == "D")
                {
                    strCreateTable = strCreateTable + ColumnName[LoopCnt] + " Decimal,";
                }
                else if (ColumnType[LoopCnt] == "N")
                {
                    strCreateTable = strCreateTable + ColumnName[LoopCnt] + " Numeric(" + ColumnSize[LoopCnt].Split(',')[0] + "," + ColumnSize[LoopCnt].Split(',')[1] + "),";
                }
                else if (ColumnType[LoopCnt] == "Dt")
                {
                    strCreateTable = strCreateTable + ColumnName[LoopCnt] + " DateTime,";
                }
            }
            strCreateTable = strCreateTable.Substring(0, strCreateTable.Length - 1) + ")";

            for (LoopCnt = 0; LoopCnt < ColumnName.Length; LoopCnt++)
            {
                strInsertTable = strInsertTable + ColumnName[LoopCnt] + ",";
                strParameters = strParameters + "?,";
            }
            strInsertTable = strInsertTable.Substring(0, strInsertTable.Length - 1) + ") ";
            strParameters = "Values(" + strParameters.Substring(0, strParameters.Length - 1) + ")";
            strCompleteCommandText = strInsertTable + strParameters;
            OleDbParameter parameter;
            using (OleDbConnection conn = new OleDbConnection(strExcelConn))
            {
                // Create a new sheet in the Excel spreadsheet.
                OleDbCommand cmd = new OleDbCommand(strCreateTable, conn);
                // Open the connection.

                if (conn.State == ConnectionState.Open) conn.Close();

                conn.Open();

                // Execute the OleDbCommand.
                cmd.ExecuteNonQuery();

                cmd.CommandText = strCompleteCommandText;

                // Add the parameters.
                for (LoopCnt = 0; LoopCnt < ColumnName.Length; LoopCnt++)
                {
                    if (ColumnType[LoopCnt] == "C")
                    {
                        cmd.Parameters.Add(ColumnName[LoopCnt], OleDbType.Char, Convert.ToInt32(ColumnSize[LoopCnt]), ColumnName[LoopCnt]);
                    }
                    if (ColumnType[LoopCnt] == "I")
                    {
                        cmd.Parameters.Add(ColumnName[LoopCnt], OleDbType.Integer, Convert.ToInt32(ColumnSize[LoopCnt]), ColumnName[LoopCnt]);
                    }
                    else if (ColumnType[LoopCnt] == "V")
                    {
                        cmd.Parameters.Add(ColumnName[LoopCnt], OleDbType.VarChar, Convert.ToInt32(ColumnSize[LoopCnt]), ColumnName[LoopCnt]);
                    }
                    else if (ColumnType[LoopCnt] == "T")
                    {
                        cmd.Parameters.Add(ColumnName[LoopCnt], OleDbType.VarChar, Convert.ToInt32(ColumnSize[LoopCnt]), ColumnName[LoopCnt]);
                    }

                    else if (ColumnType[LoopCnt] == "D")
                    {
                        parameter = new OleDbParameter(ColumnName[LoopCnt], OleDbType.Decimal);
                        parameter.Precision = Convert.ToByte(ColumnSize[LoopCnt].Split(',')[0]);
                        parameter.Scale = Convert.ToByte(ColumnSize[LoopCnt].Split(',')[1]);
                        parameter.SourceColumn = ColumnName[LoopCnt];
                        cmd.Parameters.Add(parameter);
                    }
                    else if (ColumnType[LoopCnt] == "N")
                    {
                        parameter = new OleDbParameter(ColumnName[LoopCnt], OleDbType.Numeric);
                        parameter.Precision = Convert.ToByte(ColumnSize[LoopCnt].Split(',')[0]);
                        parameter.Scale = Convert.ToByte(ColumnSize[LoopCnt].Split(',')[1]);
                        parameter.SourceColumn = ColumnName[LoopCnt];
                        cmd.Parameters.Add(parameter);

                    }
                    else if (ColumnType[LoopCnt] == "Dt")
                    {
                        cmd.Parameters.Add(ColumnName[LoopCnt], OleDbType.Date, Convert.ToInt32(ColumnSize[LoopCnt]), ColumnName[LoopCnt]);
                    }
                }
                // Initialize an OleDBDataAdapter object.
                OleDbDataAdapter da = new OleDbDataAdapter("select * from ExcelExport", conn);

                // Set the InsertCommand of OleDbDataAdapter, 
                // which is used to insert data.
                da.InsertCommand = cmd;

                // Changes the Rowstate()of each DataRow to Added,
                // so that OleDbDataAdapter will insert the rows.
                foreach (DataRow dr in Dt.Rows)
                {
                    dr.SetAdded();
                }

                // Insert the data into the Excel spreadsheet.
                da.Update(Dt);
                da.Dispose();
            }
        }
        DesignExcel(OriginalColumnName, ColumnWidthSize, FileName, DirectoryPath, Version, ColumnType, HeaderText, FindWordChangeRowColor);
    }
    protected void DesignExcel(string[] ColName, string[] ColWidthSize, string FileName, string DirectoryPath, string Version, string[] ColumnType, string[] HeaderText, string[] FindWordChangeRowColor)
    {
        string FileExtension = String.Empty;
        int CharInt = 97; // For Work Sheet Col Ref
        string ColRef = String.Empty;
        bool IsColZCrossed = false;
        int CellRef = 97; //For Work Sheet Cell Ref
        string EndColumnRef = String.Empty;
        if (Version == "2003") FileExtension = ".xls";
        if (Version == "2007") FileExtension = ".xlsx";
        string FilePath_Delete = DirectoryPath + FileName + "_Temp" + FileExtension;
        string FilePath_Save = DirectoryPath + FileName + FileExtension;
        Workbook xlWorkBook = null;
        Worksheet xlWorkSheet = null;
        object misValue = Missing.Value;
        //try
        //{
        Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
        xlWorkBook = xlApp.Workbooks.Open(FilePath_Delete, misValue, misValue, misValue, misValue, misValue, misValue, misValue, misValue, misValue, misValue, misValue, misValue, misValue, misValue);
        xlWorkSheet = (Worksheet)xlWorkBook.ActiveSheet;


        int i = 0;
        foreach (string col in ColName)
        {
            xlWorkSheet.Cells[1, ++i] = col;
            SetColumnWidth(xlWorkSheet, i, Convert.ToInt32(ColWidthSize[i - 1]));
            if (ColumnType[i - 1] == "N" || ColumnType[i - 1] == "D")
            {
                ((Range)xlWorkSheet.Cells[1, i]).EntireColumn.NumberFormat = "\"\"#,##0.00######_);[RED]\\(#,##0.00######)";
            }
            else if (ColumnType[i - 1] == "I")
            {
                ((Range)xlWorkSheet.Cells[1, i]).EntireColumn.NumberFormat = "\"\"#,##0_);[RED]\\(#,##0)";
            }
            else
            {
                ((Range)xlWorkSheet.Cells[1, i]).EntireColumn.HorizontalAlignment = HorizontalAlign.Left;
            }

            CharInt++;
            CellRef++;
            if (CharInt == 122)
            {
                ColRef = ColRef + "" + "A";
                IsColZCrossed = true;
                CharInt = 97;
            }
        }
        if (IsColZCrossed)
        {
            ColRef = ColRef + "" + Convert.ToChar(CharInt);
        }
        else
            ColRef = "" + Convert.ToChar(CharInt);
        //Comman Feature
        xlWorkSheet.Columns.EntireColumn.WrapText = true;
        xlWorkSheet.Columns.EntireColumn.Font.Name = "Arial";
        xlWorkSheet.Columns.EntireColumn.Font.Size = 8;
        // Fix first row
        xlWorkSheet.Activate();
        xlWorkSheet.Application.ActiveWindow.SplitRow = 1;
        xlWorkSheet.Application.ActiveWindow.FreezePanes = true;

        //insert Report Header (Parameter HeaderText)
        ReportHeader(xlWorkSheet, CellRef, ColRef, HeaderText);

        //Preparing Col Header
        EndColumnRef = ColRef + Convert.ToString(HeaderText.Length + 1);
        RTemplate_SimpleHeader(xlWorkSheet, "A" + Convert.ToString(HeaderText.Length + 1), EndColumnRef);

        //Change Your Total,GroupTotal and Distinguish any other Row..Very Time Consuming 
        //For Large DataSet So Please Use This parameter When RecordSet is Not Very Large
        if (FindWordChangeRowColor != null)
        {
            DistinguishRow(xlWorkSheet, ColName, FindWordChangeRowColor);
        }

        //SetCellColor(xlWorkSheet, 10, 2, System.Drawing.Color.BlueViolet);
        //SetCellSize(xlWorkSheet, 10, 2, 18.5);
        //SetCellBoldItalic(xlWorkSheet, 1, 2, true, true);
        //SetCellRangeBoldItalic(xlWorkSheet, "A2", "B2", true, true);
        //SetCellHAlign(xlWorkSheet, 1, 2, "C");
        //SetCellRangeHAlign(xlWorkSheet, "A3", "B3", "L");
        //SetCellRangeUnderLine(xlWorkSheet, "A2", "B2", true);
        //SetCellUnderLine(xlWorkSheet, 1, 2, true);
        //SetCellVAlign(xlWorkSheet, 1, 2, "M");
        //SetCellRangeVAlign(xlWorkSheet, "A2", "B2", "M");

        xlWorkBook.Close(true, FilePath_Save, misValue);
        xlApp.Quit();

        Marshal.ReleaseComObject(xlWorkSheet);
        Marshal.ReleaseComObject(xlWorkBook);
        Marshal.ReleaseComObject(xlApp);

        xlWorkSheet = null;
        xlWorkBook = null;
        xlApp = null;

        GC.GetTotalMemory(false);
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        GC.GetTotalMemory(true);

        File.Delete(FilePath_Delete);
        Download(FilePath_Save);

        //}
        //catch { }
        //finally { }


    }
    void ReportHeader(Worksheet ws, int CellRef, string ColRef, string[] HeaderText)
    {
        //Insert Report Info
        Range rng, row;
        string LastColCharValue = ColRef;
        string StartRange, LastRange;
        for (int shiftrow = 1; shiftrow <= HeaderText.Length; shiftrow++)
        {
            rng = (Range)ws.Cells[1, CellRef - 1];
            row = rng.EntireRow;
            row.Insert(XlInsertShiftDirection.xlShiftDown, false);
            row.Cells[0, 1] = HeaderText[shiftrow - 1];

            //Design Report Info
            StartRange = "A1";
            LastRange = LastColCharValue + "1";
            rng = (Range)ws.Cells[1, CellRef - 1];
            row = rng.EntireRow;
            row.EntireColumn.WrapText = false;
            SetCellRangeBackColor(ws, StartRange, LastRange, Color.FromArgb(228, 248, 255));
            SetCellRangeBoldItalic(ws, StartRange, LastRange, true, true);
            SetCellRangeSize(ws, StartRange, LastRange, 10);
        }
    }
    public void SetColumnWidth(Worksheet ws, int col, int width)
    {
        ((Range)ws.Cells[1, col]).EntireColumn.ColumnWidth = width;
    }
    public void SetCellBoldItalic(Worksheet ws, int row, int col, bool BoldTrueFalse, bool ItalicTrueFalse)
    {
        ((Range)ws.Cells[row, col]).Font.Bold = BoldTrueFalse;
        ((Range)ws.Cells[row, col]).Font.Italic = ItalicTrueFalse;
    }
    public void SetCellRangeBoldItalic(Worksheet ws, string StartRange, string EndRange, Boolean BoldTrueFalse, bool ItalicTrueFalse)
    {
        ws.get_Range(StartRange, EndRange).Font.Bold = BoldTrueFalse;
        ws.get_Range(StartRange, EndRange).Font.Italic = ItalicTrueFalse;
    }
    public void SetCellSize(Worksheet ws, int row, int col, double SizeValue)
    {
        ((Range)ws.Cells[row, col]).Font.Size = SizeValue;
    }
    public void SetCellRangeSize(Worksheet ws, string StartRange, string EndRange, double SizeValue)
    {
        ws.get_Range(StartRange, EndRange).Font.Size = SizeValue;
    }
    public void SetCellColor(Worksheet ws, int row, int col, System.Drawing.Color Color)
    {
        ((Range)ws.Cells[row, col]).Font.Color = System.Drawing.ColorTranslator.ToOle(Color); ;
    }
    public void SetCellRangeColor(Worksheet ws, string StartRange, string EndRange, System.Drawing.Color Color)
    {
        ws.get_Range(StartRange, EndRange).Font.Color = System.Drawing.ColorTranslator.ToOle(Color);
    }
    public void SetCellBackColor(Worksheet ws, int row, int col, System.Drawing.Color Color)
    {
        ((Range)ws.Cells[row, col]).Font.Background = System.Drawing.ColorTranslator.ToOle(Color);
    }
    public void SetCellRangeBackColor(Worksheet ws, string StartRange, string EndRange, System.Drawing.Color Color)
    {
        ws.get_Range(StartRange, EndRange).Interior.Color = System.Drawing.ColorTranslator.ToOle(Color);
    }
    public void SetCellUnderLine(Worksheet ws, int row, int col, bool UnderLineTrueFalse)
    {
        ((Range)ws.Cells[row, col]).Font.Underline = UnderLineTrueFalse;
    }
    public void SetCellRangeUnderLine(Worksheet ws, string StartRange, string EndRange, bool UnderLineTrueFalse)
    {
        ws.get_Range(StartRange, EndRange).Font.Underline = UnderLineTrueFalse;
    }
    public void SetCellHAlign(Worksheet ws, int row, int col, string strHAlign)
    {
        if (strHAlign == "C")
            ((Range)ws.Cells[row, col]).HorizontalAlignment = HorizontalAlign.Center;
        if (strHAlign == "R")
            ((Range)ws.Cells[row, col]).HorizontalAlignment = HorizontalAlign.Right;
        if (strHAlign == "L")
            ((Range)ws.Cells[row, col]).HorizontalAlignment = HorizontalAlign.Left;
        if (strHAlign == "J")
            ((Range)ws.Cells[row, col]).HorizontalAlignment = HorizontalAlign.Justify;
    }
    public void SetCellRangeHAlign(Worksheet ws, string StartRange, string EndRange, string strHAlign)
    {
        if (strHAlign == "C")
            ws.get_Range(StartRange, EndRange).HorizontalAlignment = HorizontalAlign.Center;
        if (strHAlign == "R")
            ws.get_Range(StartRange, EndRange).HorizontalAlignment = HorizontalAlign.Right;
        if (strHAlign == "L")
            ws.get_Range(StartRange, EndRange).HorizontalAlignment = HorizontalAlign.Left;
        if (strHAlign == "J")
            ws.get_Range(StartRange, EndRange).HorizontalAlignment = HorizontalAlign.Justify;
    }
    public void SetCellVAlign(Worksheet ws, int row, int col, string strHAlign)
    {
        if (strHAlign == "B")
            ((Range)ws.Cells[row, col]).VerticalAlignment = VerticalAlign.Bottom;
        if (strHAlign == "M")
            ((Range)ws.Cells[row, col]).VerticalAlignment = VerticalAlign.Middle;
        if (strHAlign == "T")
            ((Range)ws.Cells[row, col]).VerticalAlignment = VerticalAlign.Top;
    }
    public void SetCellRangeVAlign(Worksheet ws, string StartRange, string EndRange, string strHAlign)
    {
        if (strHAlign == "B")
            ws.get_Range(StartRange, EndRange).VerticalAlignment = VerticalAlign.Bottom;
        if (strHAlign == "M")
            ws.get_Range(StartRange, EndRange).VerticalAlignment = VerticalAlign.Middle;
        if (strHAlign == "T")
            ws.get_Range(StartRange, EndRange).VerticalAlignment = VerticalAlign.Top;
    }
    public void CTemplate_SimpleHeader(Worksheet ws, int row, int col)
    {
        ((Range)ws.Cells[row, col]).Font.Bold = true;
        ((Range)ws.Cells[row, col]).Font.Size = 14;
        ((Range)ws.Cells[row, col]).Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.SteelBlue);
        ((Range)ws.Cells[row, col]).HorizontalAlignment = HorizontalAlign.Center;
        ((Range)ws.Cells[row, col]).VerticalAlignment = VerticalAlign.Middle;

    }
    public void RTemplate_SimpleHeader(Worksheet ws, string StartRange, string EndRange)
    {
        ws.get_Range(StartRange, EndRange).Font.Bold = true;
        ws.get_Range(StartRange, EndRange).Font.Size = 8;
        ws.get_Range(StartRange, EndRange).Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightGray);
        ws.get_Range(StartRange, EndRange).HorizontalAlignment = HorizontalAlign.Center;
        ws.get_Range(StartRange, EndRange).VerticalAlignment = VerticalAlign.Middle;
    }
    protected void Download(string FilePath)
    {
        try
        {
            string filename = FilePath;
            FileInfo fileInfo = new FileInfo(filename);
            if (fileInfo.Exists)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + fileInfo.Name);
                HttpContext.Current.Response.Charset = "";
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.TransmitFile(fileInfo.FullName);
                HttpContext.Current.Response.End();
                //fileInfo.Delete();
            }
        }
        catch { }
    }
    private void DistinguishRow(Worksheet ws, string[] ColName, string[] FindWordChangeRowColor)
    {
        Range currentFind = null;
        Range firstFind = null;
        int i;
        foreach (string DR in FindWordChangeRowColor)
        {
            for (i = 1; i < ColName.Length; i++)
            {
                Range Rng = ((Range)ws.Cells[1, i]).EntireColumn;
                currentFind = Rng.Find(DR, Missing.Value,
                XlFindLookIn.xlValues, XlLookAt.xlPart,
                XlSearchOrder.xlByRows, XlSearchDirection.xlNext, false,
                Missing.Value, Missing.Value);

                while (currentFind != null)
                {
                    // Keep track of the first range you find. 
                    if (firstFind == null)
                    {
                        firstFind = currentFind;
                    }

                    // If you didn't move to a new range, you are done.
                    else if (currentFind.get_Address(Missing.Value, Missing.Value, XlReferenceStyle.xlA1, Missing.Value, Missing.Value)
                          == firstFind.get_Address(Missing.Value, Missing.Value, XlReferenceStyle.xlA1, Missing.Value, Missing.Value))
                    {
                        break;
                    }
                    currentFind.EntireRow.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Maroon);
                    currentFind.EntireRow.Font.Bold = true;
                    currentFind = Rng.FindNext(currentFind);
                }
            }
            currentFind = null;
            firstFind = null;
        }
    }

    #region Generate HTML
    public string ExportToHTML(string[] ColumnType, string[] ColumnSize, string[] ColumnWidthSize, System.Data.DataTable Dt, string DirectoryPath, string Version, string FileName, string[] HeaderText, string[] FindWordChangeRowColor)
    {
        DirectoryPath = DirectoryPath + "ExcelContent/";
        DirectoryInfo directoryInfo = new DirectoryInfo(DirectoryPath);
        if (!directoryInfo.Exists)
        {
            directoryInfo.Create();
        }
        string FileExtension = String.Empty;
        if (Version == "2003") FileExtension = ".xls";
        if (Version == "2007") FileExtension = ".xlsx";
        string TempPath = DirectoryPath + FileName + "_Temp" + FileExtension;
        string strExcelConn = String.Empty;

        string[] OriginalColumnName = new string[Dt.Columns.Count];
        string[] ColumnName = new string[Dt.Columns.Count];
        int i = 0;
        foreach (DataColumn c in Dt.Columns)
        {
            OriginalColumnName[i] = Dt.Columns[i].ColumnName;
            ColumnName[i] = "Column" + i;
            Dt.Columns[c.ColumnName].ColumnName = "Column" + Convert.ToString(i++);
        }

        if (Version == "2003")
        {
            // Excel 97-2003
            strExcelConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + TempPath + ";Extended Properties='Excel 8.0;HDR=Yes'";
        }
        if (Version == "2007")
        {
            //Excel 97
            strExcelConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + TempPath + ";Extended Properties='Excel 12.0 Xml;HDR=Yes'";
        }
        if (Version != "2003" || Version != "2007")
        {

            int LoopCnt;
            string strCreateTable = "Create Table ExcelExport (";
            string strInsertTable = "Insert into ExcelExport (";
            string strParameters = String.Empty;
            string strCompleteCommandText = String.Empty;
            for (LoopCnt = 0; LoopCnt < ColumnName.Length; LoopCnt++)
            {
                if (ColumnType[LoopCnt] == "C")
                {
                    strCreateTable = strCreateTable + ColumnName[LoopCnt] + " Char(" + ColumnSize[LoopCnt] + "),";
                }
                if (ColumnType[LoopCnt] == "I")
                {
                    strCreateTable = strCreateTable + ColumnName[LoopCnt] + " Int,";
                }
                else if (ColumnType[LoopCnt] == "V")
                {
                    strCreateTable = strCreateTable + ColumnName[LoopCnt] + " Varchar(" + ColumnSize[LoopCnt] + "),";
                }
                else if (ColumnType[LoopCnt] == "T")
                {
                    strCreateTable = strCreateTable + ColumnName[LoopCnt] + " Text,";
                }

                else if (ColumnType[LoopCnt] == "D")
                {
                    strCreateTable = strCreateTable + ColumnName[LoopCnt] + " Decimal,";
                }
                else if (ColumnType[LoopCnt] == "N")
                {
                    strCreateTable = strCreateTable + ColumnName[LoopCnt] + " Numeric(" + ColumnSize[LoopCnt].Split(',')[0] + "," + ColumnSize[LoopCnt].Split(',')[1] + "),";
                }
                else if (ColumnType[LoopCnt] == "Dt")
                {
                    strCreateTable = strCreateTable + ColumnName[LoopCnt] + " DateTime,";
                }
            }
            strCreateTable = strCreateTable.Substring(0, strCreateTable.Length - 1) + ")";

            for (LoopCnt = 0; LoopCnt < ColumnName.Length; LoopCnt++)
            {
                strInsertTable = strInsertTable + ColumnName[LoopCnt] + ",";
                strParameters = strParameters + "?,";
            }
            strInsertTable = strInsertTable.Substring(0, strInsertTable.Length - 1) + ") ";
            strParameters = "Values(" + strParameters.Substring(0, strParameters.Length - 1) + ")";
            strCompleteCommandText = strInsertTable + strParameters;
            OleDbParameter parameter;
            using (OleDbConnection conn = new OleDbConnection(strExcelConn))
            {
                // Create a new sheet in the Excel spreadsheet.
                OleDbCommand cmd = new OleDbCommand(strCreateTable, conn);
                // Open the connection.

                if (conn.State == ConnectionState.Open) conn.Close();

                conn.Open();

                // Execute the OleDbCommand.
                cmd.ExecuteNonQuery();

                cmd.CommandText = strCompleteCommandText;

                // Add the parameters.
                for (LoopCnt = 0; LoopCnt < ColumnName.Length; LoopCnt++)
                {
                    if (ColumnType[LoopCnt] == "C")
                    {
                        cmd.Parameters.Add(ColumnName[LoopCnt], OleDbType.Char, Convert.ToInt32(ColumnSize[LoopCnt]), ColumnName[LoopCnt]);
                    }
                    if (ColumnType[LoopCnt] == "I")
                    {
                        cmd.Parameters.Add(ColumnName[LoopCnt], OleDbType.Integer, Convert.ToInt32(ColumnSize[LoopCnt]), ColumnName[LoopCnt]);
                    }
                    else if (ColumnType[LoopCnt] == "V")
                    {
                        cmd.Parameters.Add(ColumnName[LoopCnt], OleDbType.VarChar, Convert.ToInt32(ColumnSize[LoopCnt]), ColumnName[LoopCnt]);
                    }
                    else if (ColumnType[LoopCnt] == "T")
                    {
                        cmd.Parameters.Add(ColumnName[LoopCnt], OleDbType.VarChar, Convert.ToInt32(ColumnSize[LoopCnt]), ColumnName[LoopCnt]);
                    }

                    else if (ColumnType[LoopCnt] == "D")
                    {
                        parameter = new OleDbParameter(ColumnName[LoopCnt], OleDbType.Decimal);
                        parameter.Precision = Convert.ToByte(ColumnSize[LoopCnt].Split(',')[0]);
                        parameter.Scale = Convert.ToByte(ColumnSize[LoopCnt].Split(',')[1]);
                        parameter.SourceColumn = ColumnName[LoopCnt];
                        cmd.Parameters.Add(parameter);
                    }
                    else if (ColumnType[LoopCnt] == "N")
                    {
                        parameter = new OleDbParameter(ColumnName[LoopCnt], OleDbType.Numeric);
                        parameter.Precision = Convert.ToByte(ColumnSize[LoopCnt].Split(',')[0]);
                        parameter.Scale = Convert.ToByte(ColumnSize[LoopCnt].Split(',')[1]);
                        parameter.SourceColumn = ColumnName[LoopCnt];
                        cmd.Parameters.Add(parameter);

                    }
                    else if (ColumnType[LoopCnt] == "Dt")
                    {
                        cmd.Parameters.Add(ColumnName[LoopCnt], OleDbType.Date, Convert.ToInt32(ColumnSize[LoopCnt]), ColumnName[LoopCnt]);
                    }
                }
                // Initialize an OleDBDataAdapter object.
                OleDbDataAdapter da = new OleDbDataAdapter("select * from ExcelExport", conn);

                // Set the InsertCommand of OleDbDataAdapter, 
                // which is used to insert data.
                da.InsertCommand = cmd;

                // Changes the Rowstate()of each DataRow to Added,
                // so that OleDbDataAdapter will insert the rows.
                foreach (DataRow dr in Dt.Rows)
                {
                    dr.SetAdded();
                }

                // Insert the data into the Excel spreadsheet.
                da.Update(Dt);
                da.Dispose();
            }
        }
        return DesignHTML(OriginalColumnName, ColumnWidthSize, FileName, DirectoryPath, Version, ColumnType, HeaderText, FindWordChangeRowColor);
    }
    protected string DesignHTML(string[] ColName, string[] ColWidthSize, string FileName, string DirectoryPath, string Version, string[] ColumnType, string[] HeaderText, string[] FindWordChangeRowColor)
    {
        string FileExtension = String.Empty;
        int HtmlLastCell = 0;
        int CharInt = 97; // For Work Sheet Col Ref
        string ColRef = String.Empty;
        bool IsColZCrossed = false;
        int CellRef = 97; //For Work Sheet Cell Ref
        string EndColumnRef = String.Empty;
        if (Version == "2003") FileExtension = ".xls";
        if (Version == "2007") FileExtension = ".xlsx";
        string FilePath_Delete = DirectoryPath + FileName + "_Temp" + FileExtension;
        string FilePath_Save = DirectoryPath + FileName + FileExtension;
        Workbook xlWorkBook = null;
        Worksheet xlWorkSheet = null;
        object misValue = Missing.Value;
        //try
        //{
        Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
        xlWorkBook = xlApp.Workbooks.Open(FilePath_Delete, misValue, misValue, misValue, misValue, misValue, misValue, misValue, misValue, misValue, misValue, misValue, misValue, misValue, misValue);
        xlWorkSheet = (Worksheet)xlWorkBook.ActiveSheet;


        int i = 0;
        foreach (string col in ColName)
        {
            xlWorkSheet.Cells[1, ++i] = col;
            SetColumnWidth(xlWorkSheet, i, Convert.ToInt32(ColWidthSize[i - 1]));
            if (ColumnType[i - 1] == "N" || ColumnType[i - 1] == "D")
            {
                ((Range)xlWorkSheet.Cells[1, i]).EntireColumn.NumberFormat = "\"\"#,##0.00######_);[RED]\\(#,##0.00######)";
            }
            else if (ColumnType[i - 1] == "I")
            {
                ((Range)xlWorkSheet.Cells[1, i]).EntireColumn.NumberFormat = "\"\"#,##0_);[RED]\\(#,##0)";
            }
            else
            {
                ((Range)xlWorkSheet.Cells[1, i]).EntireColumn.HorizontalAlignment = HorizontalAlign.Left;
            }

            CharInt++;
            CellRef++;
            if (CharInt == 122)
            {
                ColRef = ColRef + "" + "A";
                IsColZCrossed = true;
                CharInt = 97;
            }
            //To Find Out LastColumn integer Value
            ++HtmlLastCell;
        }
        if (IsColZCrossed)
        {
            ColRef = ColRef + "" + Convert.ToChar(CharInt);
        }
        else
            ColRef = "" + Convert.ToChar(CharInt);
        //Comman Feature
        xlWorkSheet.Columns.EntireColumn.WrapText = true;
        xlWorkSheet.Columns.EntireColumn.Font.Name = "Arial";
        xlWorkSheet.Columns.EntireColumn.Font.Size = 8;
        // Fix first row
        xlWorkSheet.Activate();
        xlWorkSheet.Application.ActiveWindow.SplitRow = 1;
        xlWorkSheet.Application.ActiveWindow.FreezePanes = true;

        //Add Extra Col In Case Header is Longer Then Detail Record
        string IncrementalCellAfterZpostion = "A";

        int ExtraCharInt = 97 + HtmlLastCell - 1;

        for (int k = 0; k < 10; k++)
        {
            if (ExtraCharInt + k == 122)
            {
                ExtraCharInt = 97;
                IsColZCrossed = true;
                if (IncrementalCellAfterZpostion != "A")
                    IncrementalCellAfterZpostion = IncrementalCellAfterZpostion + Convert.ToChar(1);
                ColRef = IncrementalCellAfterZpostion + "A";
            }
            else if (ExtraCharInt + k > 122)
            {
                ExtraCharInt = 97 + ExtraCharInt - 123;
                IsColZCrossed = true;
                ColRef = IncrementalCellAfterZpostion + "" + Convert.ToChar(ExtraCharInt);
            }
            else
            {
                if (IsColZCrossed)
                {
                    ColRef = IncrementalCellAfterZpostion + "" + Convert.ToChar(ExtraCharInt + k);
                }
                else
                    ColRef = "" + Convert.ToChar(ExtraCharInt + k);
            }
        }
        
      
       
        //insert Report Header (Parameter HeaderText)
        ReportHeader(xlWorkSheet, CellRef, ColRef, HeaderText);

        //ReAssigned EndColumnRef With Customized Range
        EndColumnRef = ColRef + Convert.ToString(HeaderText.Length + 1);

       

        RTemplate_SimpleHeader(xlWorkSheet, "A" + Convert.ToString(HeaderText.Length + 1), EndColumnRef);

        //Change Your Total,GroupTotal and Distinguish any other Row..Very Time Consuming 
        //For Large DataSet So Please Use This parameter When RecordSet is Not Very Large
        if (FindWordChangeRowColor != null)
        {
            DistinguishRow(xlWorkSheet, ColName, FindWordChangeRowColor);
        }

        xlWorkBook.Close(true, FilePath_Save, misValue);
        xlApp.Quit();

        Marshal.ReleaseComObject(xlWorkSheet);
        Marshal.ReleaseComObject(xlWorkBook);
        Marshal.ReleaseComObject(xlApp);

        xlWorkSheet = null;
        xlWorkBook = null;
        xlApp = null;

        GC.GetTotalMemory(false);
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        GC.GetTotalMemory(true);

        File.Delete(FilePath_Delete);

        return GenerateHTML(FilePath_Save, EndColumnRef);
    }
    protected string GenerateHTML(string FilePath, string EndRange)
    {
        string MailBody = string.Empty;   //-------EmailContent";
        try
        {
            string xlFilename = FilePath;
            string htmlFilename = null;
            string htmlDirectory = null;

            FileInfo xlFileInfo = new FileInfo(xlFilename);
            if (xlFileInfo.Exists)
            {
                //-----Create Directory and Filename to Save Html File---------------------------------
                htmlDirectory = xlFilename.Substring(0, xlFilename.LastIndexOf("/") + 1);
                htmlDirectory = htmlDirectory + "EmailContent/";
                htmlFilename = xlFilename.Substring(xlFilename.LastIndexOf("/") + 1).Replace(".xlsx", ".htm");

                DirectoryInfo htmlDirectoryInfo = new DirectoryInfo(htmlDirectory);
                if (!htmlDirectoryInfo.Exists)
                {
                    htmlDirectoryInfo.Create();
                }
                //--------Get Worksheet, excel Column Range to get Content from Excel to Html------------
                FileInfo htmlFileInfo = new FileInfo(htmlDirectory + htmlFilename);
                Application excel = null;
                Workbook xls = null;
                String excelRange = "$A1:$" + EndRange.ToUpper();//"$" + StartRange + ":$" + LastRange;

                excel = new Application();
                object missing = Type.Missing;
                object trueObject = true;
                excel.Visible = false;
                excel.DisplayAlerts = false;

                xls = excel.Workbooks.Open(xlFileInfo.FullName, missing, trueObject, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
                Worksheet ws = (Worksheet)xls.ActiveSheet;
                xls.PublishObjects.Add(XlSourceType.xlSourceSheet, htmlFileInfo.FullName, ws.Name, excelRange, missing, missing, missing).Publish(true);

                //--Get Content and Return-------                                   
                if (htmlFileInfo.Exists)
                {
                    StreamReader reader = new StreamReader(htmlFileInfo.FullName);
                    MailBody = reader.ReadToEnd();
                    reader.Close();
                    htmlFileInfo.Delete();
                }
                xlFileInfo.Delete();
            }
        }
        catch { }
        return MailBody;
    }
    #endregion

}

