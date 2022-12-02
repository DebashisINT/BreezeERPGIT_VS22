using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Mail;
using System.IO;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Runtime.Serialization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using Excel;

namespace Reports.Model
{
    public class ExcelFile
    {
        public ExcelFile()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #region Customize Excel
        //public void ExcelImport(DataTable DtTable, string name)
        //{
        //    // Create an Excel object and add workbook...
        //    //Excel.ApplicationClass excel = new Excel.ApplicationClass();
        //    //Excel.Workbook workbook = excel.Application.Workbooks.Add(true); // true for object template???
        //    //Excel.Worksheet xlWorkSheet = (Excel.Worksheet)workbook.Worksheets.get_Item(1);
        //    //Excel.Range chartRange;

        //    chartRange = xlWorkSheet.get_Range("a1", "u1");
        //    chartRange.Font.Bold = true;
        //    chartRange.Interior.Color = System.Drawing.Color.Bisque.ToArgb();
        //    chartRange.RowHeight = 20;
        //    chartRange.Columns.AutoFit();
        //    chartRange.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlMedium, Excel.XlColorIndex.xlColorIndexAutomatic, Excel.XlColorIndex.xlColorIndexAutomatic);
        //    //excel.Application.Cells.Font.Bold = true;
        //    //excel.ActiveWorkbook.Sheets.Application.Cells.Font.Color = System.Drawing.Color.Blue.ToArgb();
        //    // Add column headings...
        //    //chartRange = xlWorkSheet.get_Range("a2", "u4");
        //    //chartRange.Font.Size = 10;  
        //    int iCol = 0;
        //    foreach (DataColumn c in DtTable.Columns)
        //    {
        //        iCol++;
        //        excel.Cells[1, iCol] = c.ColumnName;
        //    }
        //    // for each row of data...
        //    int iRow = 0;
        //    int count = 0;
        //    int countrow = 0;
        //    foreach (DataRow dr in DtTable.Rows)
        //    {
        //        iRow++;
        //        if (dr.IsNull(0))
        //        {
        //            count += 1;
        //            countrow = 1;
        //        }
        //        if (countrow == 0)
        //        {
        //            chartRange = xlWorkSheet.get_Range("a2", "u" + (Convert.ToInt32(iRow) + 1) + "");
        //            chartRange.Font.Size = 10;
        //        }
        //        else if (countrow == 1)
        //        {
        //            if (count % 2 != 0)
        //            {
        //                chartRange = xlWorkSheet.get_Range("a" + (Convert.ToInt32(iRow) + 1) + "", "u" + (Convert.ToInt32(iRow) + 1) + "");
        //                chartRange.Font.Bold = true;
        //                chartRange.Font.Color = System.Drawing.Color.Red.ToArgb();
        //                //chartRange.Cells.Merge(true);                   
        //            }
        //            else if (count % 2 == 0)
        //            {
        //                chartRange = xlWorkSheet.get_Range("a" + (Convert.ToInt32(iRow) + 1) + "", "u" + (Convert.ToInt32(iRow) + 1) + "");
        //                chartRange.Font.Size = 10;
        //            }
        //        }
        //    }
        //    iRow = 0;
        //    foreach (DataRow r in DtTable.Rows)
        //    {
        //        iRow++;
        //        // add each row's cell data...
        //        iCol = 0;
        //        for (int i = 0; i < DtTable.Columns.Count; i++)
        //        {
        //            iCol++;
        //            excel.Cells[iRow + 1, iCol] = r[i];
        //        }
        //    }

        //    // Global missing reference for objects we are not defining...
        //    object missing = System.Reflection.Missing.Value;

        //    HttpContext.Current.Response.Clear();
        //    HttpContext.Current.Response.ClearHeaders();
        //    HttpContext.Current.Response.ContentType = "application/vnd.xls";
        //    HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + name + ".xls");
        //    HttpContext.Current.Response.Write(workbook);
        //    HttpContext.Current.Response.End();


        //    workbook.Close(true, missing, missing);
        //    excel.Quit();


        //    releaseObject(excel);
        //    releaseObject(workbook);
        //    releaseObject(xlWorkSheet);

        //    HttpContext.Current.Response.Write("File created");


        //}
        public void ExportToExcelforExcel(DataTable datatable, string fname, string compareString, string compareString1, DataTable dtHeader, DataTable dtFooter)
        {
            //Create a dummy GridView 
            int colNo = 0;
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;
            //foreach (DataColumn item in datatable.Columns)
            //{
            //    BoundField bfield = new BoundField();
            //    bfield.HeaderText = item.ColumnName;
            //    bfield.DataField = item.ColumnName;
            //    GridView1.Columns.Add(bfield);
            //}
            GridView1.DataSource = datatable;
            GridView1.DataBind();
            GridView1.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            GridView1.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#E8E6E7");
            GridView1.HeaderStyle.Font.Bold = true;
            GridView1.HeaderStyle.Font.Name = "Tahoma";
            GridView1.HeaderStyle.Font.Size = 10;// new FontUnit("10px");
            GridView1.GridLines = GridLines.Both;
            GridView1.BorderWidth = 0;
            GridView1.HeaderStyle.Wrap = false;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            //HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + fname + ".xlsx");
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + fname + ".xls");
            HttpContext.Current.Response.Charset = "";
            //HttpContext.Current.Response.Charset = "UTF-8";
            //HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            //foreach (GridViewRow row in GridView1.Rows)
            //{
            //    if (row.RowType == DataControlRowType.DataRow)
            //        for (int idxColumn = 0; idxColumn < row.Cells.Count; idxColumn++)
            //            row.Cells[idxColumn].Attributes.Add("class", "xlText");
            //}
            //HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel"; //(for .xls)
            //string filePath = "";
            //filePath = HttpContext.Current.Server.MapPath(fname + ".xlsx");
            //HttpContext.Current.Response.TransmitFile(filePath);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            HtmlTextWriter hw1 = new HtmlTextWriter(sw);
            GridView GridView2 = new GridView();
            GridView2.DataSource = dtHeader;
            GridView2.DataBind();
            GridView2.HeaderRow.Visible = false;
            GridView2.BorderWidth = 0;
            for (int j = 0; j < GridView2.Rows.Count; j++)
            {
                GridView2.Rows[j].Font.Bold = true;
                GridView2.Rows[j].Font.Name = "Tahoma";
                GridView2.Rows[j].Font.Size = 10;// new FontUnit("10px");
                //GridView2.Rows[j].HorizontalAlign = HorizontalAlign.Center;
                GridView2.Rows[j].ForeColor = System.Drawing.Color.Blue;
                //GridView2.Rows[j].Cells[0].ColumnSpan = GridView1.Rows[0].Cells.Count;
                //GridView2.Rows[j].Cells[0].BackColor = System.Drawing.Color.LightGray;
                GridView2.Rows[j].Cells[0].Wrap = false;
                if (GridView2.Rows[j].Cells[0].Text == "&nbsp;")
                {
                    GridView2.Rows[j].Visible = false;
                }
            }

            GridView2.RenderControl(hw1);

            int gridvalue = 0;
            int evenOdd = 0;
            for (int i = 1; i <= GridView1.Rows.Count; i++)
            {
                //DataView dv = datatable.DefaultView;
                //dv.RowFilter = "FirstField LIKE 'whatever%'"

                //Apply text style to each Row 
                if (gridvalue != i)
                {
                    if ((GridView1.Rows[i - 1].Cells[0].Text == "&nbsp;" && GridView1.Rows[i - 1].Cells[1].Text == "&nbsp;") || (GridView1.Rows[i - 1].Cells[0].Text == "Total :"))
                    {
                        evenOdd += 1;
                        gridvalue = i + 1;
                        if (evenOdd % 2 != 0)
                        {
                            for (int k1 = 0; k1 < GridView1.Rows[i - 1].Cells.Count; k1++)
                            {
                                bool val = IsNumeric(GridView1.Rows[i - 1].Cells[k1].Text.ToString());
                                var strdatatype = datatable.Columns[k1].DataType.Name;
                                if (val == false)
                                {
                                    GridView1.Rows[i - 1].Cells[k1].HorizontalAlign = HorizontalAlign.Left;
                                    GridView1.Rows[i - 1].Cells[k1].Wrap = false;
                                }
                                else
                                {
                                    GridView1.Rows[i - 1].Cells[k1].HorizontalAlign = HorizontalAlign.Right;
                                    GridView1.Rows[i - 1].Cells[k1].Wrap = false;
                                    if (strdatatype == "Decimal")
                                    {
                                        if (GridView1.Rows[i - 1].Cells[k1].Text != "&nbsp;")
                                        {
                                            string[] datalength = GridView1.Rows[i - 1].Cells[k1].Text.Split(new char[] { '.' });
                                            int decimals = datalength[1].Length;
                                            if (decimals == 2)
                                            {
                                                //GridView1.Rows[i - 1].Cells[k1].Attributes.Add("style", "mso-number-format:0\\.00");
                                                GridView1.Rows[i - 1].Cells[k1].Attributes.Add("style", "mso-number-format:\\#\\,\\#\\#0\\.00");
                                            }
                                            else if (decimals == 3)
                                            {
                                                //GridView1.Rows[i - 1].Cells[k1].Attributes.Add("style", "mso-number-format:0\\.000");
                                                GridView1.Rows[i - 1].Cells[k1].Attributes.Add("style", "mso-number-format:\\#\\,\\#\\#0\\.000");
                                            }
                                            else if (decimals == 4)
                                            {
                                                //GridView1.Rows[i - 1].Cells[k1].Attributes.Add("style", "mso-number-format:0\\.0000");
                                                GridView1.Rows[i - 1].Cells[k1].Attributes.Add("style", "mso-number-format:\\#\\,\\#\\#0\\.0000");
                                            }
                                        }
                                    }
                                    if (val == true)
                                    {
                                        if (Convert.ToDecimal(GridView1.Rows[i - 1].Cells[k1].Text.ToString()) < 0)
                                            GridView1.Rows[i - 1].Cells[k1].ForeColor = System.Drawing.Color.Red;
                                    }
                                }
                            }
                            GridView1.Rows[i - 1].Font.Bold = true;
                            GridView1.Rows[i - 1].Font.Name = "Tahoma";
                            GridView1.Rows[i - 1].Font.Size = 10;//new FontUnit("12px");
                            GridView1.Rows[i - 1].ForeColor = System.Drawing.Color.Blue;
                        }
                        else
                        {
                            for (int k2 = 0; k2 < GridView1.Rows[i - 1].Cells.Count; k2++)
                            {
                                bool val = IsNumeric(GridView1.Rows[i - 1].Cells[k2].Text.ToString());
                                var strdatatype = datatable.Columns[k2].DataType.Name;
                                if (val == false)
                                {
                                    GridView1.Rows[i - 1].Cells[k2].HorizontalAlign = HorizontalAlign.Left;
                                    GridView1.Rows[i - 1].Cells[k2].Wrap = false;
                                }
                                else
                                {
                                    GridView1.Rows[i - 1].Cells[k2].HorizontalAlign = HorizontalAlign.Right;
                                    GridView1.Rows[i - 1].Cells[k2].Wrap = false;
                                    if (strdatatype == "Decimal")
                                    {
                                        if (GridView1.Rows[i - 1].Cells[k2].Text != "&nbsp;")
                                        {
                                            string[] datalength = GridView1.Rows[i - 1].Cells[k2].Text.Split(new char[] { '.' });
                                            int decimals = datalength[1].Length;
                                            if (decimals == 2)
                                            {
                                                //GridView1.Rows[i - 1].Cells[k2].Attributes.Add("style", "mso-number-format:0\\.00");
                                                GridView1.Rows[i - 1].Cells[k2].Attributes.Add("style", "mso-number-format:\\#\\,\\#\\#0\\.00");
                                            }
                                            else if (decimals == 3)
                                            {
                                                //GridView1.Rows[i - 1].Cells[k2].Attributes.Add("style", "mso-number-format:0\\.000");
                                                GridView1.Rows[i - 1].Cells[k2].Attributes.Add("style", "mso-number-format:\\#\\,\\#\\#0\\.000");
                                            }
                                            else if (decimals == 4)
                                            {
                                                //GridView1.Rows[i - 1].Cells[k2].Attributes.Add("style", "mso-number-format:0\\.0000");
                                                GridView1.Rows[i - 1].Cells[k2].Attributes.Add("style", "mso-number-format:\\#\\,\\#\\#0\\.0000");
                                            }
                                        }
                                    }
                                    if (val == true)
                                    {
                                        if (Convert.ToDecimal(GridView1.Rows[i - 1].Cells[k2].Text.ToString()) < 0)
                                            GridView1.Rows[i - 1].Cells[k2].ForeColor = System.Drawing.Color.Red;
                                    }
                                }
                            }
                            GridView1.Rows[i - 1].Font.Bold = true;
                            GridView1.Rows[i - 1].Font.Name = "Tahoma";
                            GridView1.Rows[i - 1].Font.Size = 10;//new FontUnit("10px");
                            GridView1.Rows[i - 1].ForeColor = System.Drawing.Color.Blue;
                        }
                    }
                    //else if (compareString.Contains(GridView1.Rows[i - 1].Cells[0].Text) || compareString.Contains(GridView1.Rows[i - 1].Cells[1].Text) || compareString.Contains(GridView1.Rows[i - 1].Cells[2].Text) || compareString.Contains(GridView1.Rows[i - 1].Cells[3].Text)
                    //    || Convert.ToString(GridView1.Rows[i - 1].Cells[0].Text).Contains(compareString) || Convert.ToString(GridView1.Rows[i - 1].Cells[1].Text).Contains(compareString) || Convert.ToString(GridView1.Rows[i - 1].Cells[2].Text).Contains(compareString) || Convert.ToString(GridView1.Rows[i - 1].Cells[3].Text).Contains(compareString)
                    //    || compareString1.Contains(GridView1.Rows[i - 1].Cells[0].Text) || compareString1.Contains(GridView1.Rows[i - 1].Cells[1].Text) || compareString1.Contains(GridView1.Rows[i - 1].Cells[2].Text) || compareString1.Contains(GridView1.Rows[i - 1].Cells[3].Text)
                    //    || Convert.ToString(GridView1.Rows[i - 1].Cells[0].Text).Contains(compareString1) || Convert.ToString(GridView1.Rows[i - 1].Cells[1].Text).Contains(compareString1) || Convert.ToString(GridView1.Rows[i - 1].Cells[2].Text).Contains(compareString1) || Convert.ToString(GridView1.Rows[i - 1].Cells[3].Text).Contains(compareString1))
                    else if (compareString.Contains(GridView1.Rows[i - 1].Cells[0].Text) || compareString.Contains(GridView1.Rows[i - 1].Cells[1].Text) || compareString.Contains(GridView1.Rows[i - 1].Cells[2].Text) || compareString.Contains(GridView1.Rows[i - 1].Cells[3].Text)
                        || Convert.ToString(GridView1.Rows[i - 1].Cells[0].Text).Contains(compareString) || Convert.ToString(GridView1.Rows[i - 1].Cells[1].Text).Contains(compareString) || Convert.ToString(GridView1.Rows[i - 1].Cells[2].Text).Contains(compareString) || Convert.ToString(GridView1.Rows[i - 1].Cells[3].Text).Contains(compareString)
                        || compareString1.Contains(GridView1.Rows[i - 1].Cells[0].Text) || compareString1.Contains(GridView1.Rows[i - 1].Cells[1].Text) || compareString1.Contains(GridView1.Rows[i - 1].Cells[2].Text) || compareString1.Contains(GridView1.Rows[i - 1].Cells[3].Text)
                        || Convert.ToString(GridView1.Rows[i - 1].Cells[0].Text).Contains(compareString1) || Convert.ToString(GridView1.Rows[i - 1].Cells[1].Text).Contains(compareString1) || Convert.ToString(GridView1.Rows[i - 1].Cells[2].Text).Contains(compareString1) || Convert.ToString(GridView1.Rows[i - 1].Cells[3].Text).Contains(compareString1))
                    {
                        for (int k3 = 0; k3 < GridView1.Rows[i - 1].Cells.Count; k3++)
                        {
                            bool val = IsNumeric(GridView1.Rows[i - 1].Cells[k3].Text.ToString());
                            var strdatatype = datatable.Columns[k3].DataType.Name;
                            //if (val == false)
                            if (strdatatype == "String")
                            {
                                GridView1.Rows[i - 1].Cells[k3].HorizontalAlign = HorizontalAlign.Left;
                                GridView1.Rows[i - 1].Cells[k3].Wrap = false;
                            }
                            else
                            {
                                GridView1.Rows[i - 1].Cells[k3].HorizontalAlign = HorizontalAlign.Right;
                                GridView1.Rows[i - 1].Cells[k3].Wrap = false;
                                if (strdatatype == "Decimal")
                                {
                                    if (GridView1.Rows[i - 1].Cells[k3].Text != "&nbsp;")
                                    {
                                        string[] datalength = GridView1.Rows[i - 1].Cells[k3].Text.Split(new char[] { '.' });
                                        int decimals = datalength[1].Length;
                                        if (decimals == 2)
                                        {
                                            //GridView1.Rows[i - 1].Cells[k3].Attributes.Add("style", "mso-number-format:0\\.00");
                                            GridView1.Rows[i - 1].Cells[k3].Attributes.Add("style", "mso-number-format:\\#\\,\\#\\#0\\.00");
                                        }
                                        else if (decimals == 3)
                                        {
                                            //GridView1.Rows[i - 1].Cells[k3].Attributes.Add("style", "mso-number-format:0\\.000");
                                            GridView1.Rows[i - 1].Cells[k3].Attributes.Add("style", "mso-number-format:\\#\\,\\#\\#0\\.000");
                                        }
                                        else if (decimals == 4)
                                        {
                                            //GridView1.Rows[i - 1].Cells[k3].Attributes.Add("style", "mso-number-format:0\\.0000");
                                            GridView1.Rows[i - 1].Cells[k3].Attributes.Add("style", "mso-number-format:\\#\\,\\#\\#0\\.0000");
                                        }
                                    }
                                }
                                if (val == true)
                                {
                                    if (Convert.ToDecimal(GridView1.Rows[i - 1].Cells[k3].Text.ToString()) < 0)
                                        GridView1.Rows[i - 1].Cells[k3].ForeColor = System.Drawing.Color.Red;
                                }
                            }
                        }
                        GridView1.Rows[i - 1].ForeColor = System.Drawing.Color.Maroon;
                        GridView1.Rows[i - 1].Font.Bold = true;
                        GridView1.Rows[i - 1].Font.Name = "Tahoma";
                        GridView1.Rows[i - 1].Font.Size = 10;//new FontUnit("10px");
                    }
                    else if (GridView1.Rows[i - 1].Cells[0].Text != "&nbsp;" && GridView1.Rows[i - 1].Cells[1].Text == "Test" && GridView1.Rows[i - 1].Cells[2].Text == "&nbsp;" && GridView1.Rows[i - 1].Cells[3].Text == "&nbsp;")
                    {
                        GridView1.Rows[i - 1].Font.Bold = true;
                        GridView1.Rows[i - 1].Font.Name = "Tahoma";
                        GridView1.Rows[i - 1].Font.Size = 10;// new FontUnit("10px");
                        GridView1.Rows[i - 1].Cells[0].Wrap = false;
                        GridView1.Rows[i - 1].Cells[1].Wrap = false;
                        GridView1.Rows[i - 1].Cells[2].Wrap = false;
                        GridView1.Rows[i - 1].Cells[3].Wrap = false;
                        GridView1.Rows[i - 1].HorizontalAlign = HorizontalAlign.Left;
                        GridView1.Rows[i - 1].ForeColor = System.Drawing.Color.Blue;
                        //GridView1.Rows[i - 1].Cells[0].ColumnSpan = GridView1.Rows[i - 1].Cells.Count;
                        //GridView1.Rows[i - 1].Cells[0].BackColor = System.Drawing.Color.LightYellow;
                        for (int cellNum = GridView1.Rows[i - 1].Cells.Count - 1; cellNum > 0; cellNum--)
                        {
                            GridView1.Rows[i - 1].Cells.RemoveAt(cellNum);
                        }
                    }
                    else
                    {
                        for (int k = 0; k < GridView1.Rows[i - 1].Cells.Count; k++)
                        {
                            bool val = IsNumeric(GridView1.Rows[i - 1].Cells[k].Text.ToString());
                            //var strdatatype = GridView1.Rows[i - 1].Cells[k].Text.GetType().Name;
                            var strdatatype = datatable.Columns[k].DataType.Name;
                            //if (val == false)
                            if (strdatatype == "String")
                            {
                                GridView1.Rows[i - 1].Cells[k].HorizontalAlign = HorizontalAlign.Left;
                                GridView1.Rows[i - 1].Cells[k].Wrap = false;
                                if (val == true)
                                {
                                    GridView1.Rows[i - 1].Cells[k].Attributes.Add("style", "mso-number-format:\\#\\#\\#0\\");
                                }
                            }
                            else
                            {
                                //BoundField col = (BoundField)GridView1.Columns[k];
                                //col.DataFormatString = "0.00";
                                //GridView1.Rows[i - 1].Cells[k].Text = String.Format("{0:0.00}", GridView1.Rows[i - 1].Cells[k].Text);
                                //decimal d = Convert.ToDecimal(GridView1.Rows[i - 1].Cells[k].Text);
                                //d = Math.Round(d, 2);
                                //GridView1.Rows[i - 1].Cells[k].Text=Convert.ToString(d);
                                GridView1.Rows[i - 1].Cells[k].HorizontalAlign = HorizontalAlign.Right;
                                GridView1.Rows[i - 1].Cells[k].Wrap = false;
                                if (strdatatype == "Decimal")
                                {
                                    //var datalength = GridView1.Rows[i - 1].Cells[k].Text.Length;
                                    if (GridView1.Rows[i - 1].Cells[k].Text != "&nbsp;")
                                    {
                                        string[] datalength = GridView1.Rows[i - 1].Cells[k].Text.Split(new char[] { '.' });
                                        int decimals = datalength[1].Length;
                                        if (decimals == 2)
                                        {
                                            //GridView1.Rows[i - 1].Cells[k].Attributes.Add("style", "mso-number-format:0\\.00");
                                            GridView1.Rows[i - 1].Cells[k].Attributes.Add("style", "mso-number-format:\\#\\,\\#\\#0\\.00");
                                        }
                                        else if (decimals == 3)
                                        {
                                            //GridView1.Rows[i - 1].Cells[k].Attributes.Add("style", "mso-number-format:0\\.000");
                                            GridView1.Rows[i - 1].Cells[k].Attributes.Add("style", "mso-number-format:\\#\\,\\#\\#0\\.000");
                                        }
                                        else if (decimals == 4)
                                        {
                                            //GridView1.Rows[i - 1].Cells[k].Attributes.Add("style", "mso-number-format:0\\.0000");
                                            GridView1.Rows[i - 1].Cells[k].Attributes.Add("style", "mso-number-format:\\#\\,\\#\\#0\\.0000");
                                        }
                                    }
                                }
                                if (val == true)
                                {
                                    if (Convert.ToDecimal(GridView1.Rows[i - 1].Cells[k].Text.ToString()) < 0)
                                        GridView1.Rows[i - 1].Cells[k].ForeColor = System.Drawing.Color.Red;
                                }
                            }
                        }
                        GridView1.Rows[i - 1].Font.Bold = false;
                        GridView1.Rows[i - 1].Font.Name = "Tahoma";
                        GridView1.Rows[i - 1].Font.Size = 10;// new FontUnit("10px");

                    }
                    colNo = GridView1.Rows[i - 1].Cells.Count;
                }

            }
            GridView1.RenderControl(hw);

            HtmlTextWriter hw2 = new HtmlTextWriter(sw);
            GridView GridView3 = new GridView();
            GridView3.DataSource = dtFooter;
            GridView3.DataBind();
            GridView3.HeaderRow.Visible = false;
            GridView3.BorderWidth = 0;
            GridView3.GridLines = GridLines.Both;
            for (int j1 = 0; j1 < GridView3.Rows.Count; j1++)
            {
                GridView3.Rows[j1].Font.Bold = true;
                GridView3.Rows[j1].Font.Name = "Tahoma";
                GridView3.Rows[j1].Font.Size = 10;// new FontUnit("10px");
                //GridView3.Rows[j1].HorizontalAlign = HorizontalAlign.Center;
                GridView3.Rows[j1].ForeColor = System.Drawing.Color.Blue;
                //GridView3.Rows[j1].Cells[0].ColumnSpan = colNo;
                //GridView3.Rows[j1].Cells[0].BackColor = System.Drawing.Color.LightYellow;
                if (GridView3.Rows[j1].Cells[0].Text == "&nbsp;")
                {
                    GridView3.Rows[j1].Visible = false;
                }
            }

            GridView3.RenderControl(hw2);

            //style to format numbers to string 
            //string style = @"<style> .textmode { mso-number-format:\#\,\#\#0\.00_ \;\[Red\]\-\#\,\#\#0\.00\ ; } </style>";
            //string style = @"<style> .textmode {mso-number-format:Number} </style>";
            //string style = @"<style> TD { mso-number-format:\@; } </style>";
            //string style = @"<style> TD { mso-number-format:\#\,\#\#0\.00; } </style>";
            //HttpContext.Current.Response.Write(style);
            //string style = @"<style> .textmode { } </style>";
            //HttpContext.Current.Response.Write(style);
            HttpContext.Current.Response.Output.Write(sw.ToString());
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        //For Cell BackColor
        public void ExportToExcelforCellBackColor(DataTable datatable, string fname, string compareString, string compareString1, DataTable dtHeader, DataTable dtFooter)
        {
            //Create a dummy GridView 
            int colNo = 0;
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;
            GridView1.DataSource = datatable;
            GridView1.DataBind();
            GridView1.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            GridView1.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#E8E6E7");
            GridView1.HeaderStyle.Font.Bold = true;
            GridView1.HeaderStyle.Font.Name = "Tahoma";
            GridView1.HeaderStyle.Font.Size = 10;// new FontUnit("10px");
            GridView1.GridLines = GridLines.Both;
            GridView1.BorderWidth = 0;
            GridView1.HeaderStyle.Wrap = false;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + fname + ".xls");
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            HtmlTextWriter hw1 = new HtmlTextWriter(sw);
            GridView GridView2 = new GridView();
            GridView2.DataSource = dtHeader;
            GridView2.DataBind();
            GridView2.HeaderRow.Visible = false;
            GridView2.BorderWidth = 0;
            for (int j = 0; j < GridView2.Rows.Count; j++)
            {
                GridView2.Rows[j].Font.Bold = true;
                GridView2.Rows[j].Font.Name = "Tahoma";
                GridView2.Rows[j].Font.Size = 10;
                GridView2.Rows[j].ForeColor = System.Drawing.Color.Blue;
                GridView2.Rows[j].Cells[0].Wrap = false;
                if (GridView2.Rows[j].Cells[0].Text == "&nbsp;")
                {
                    GridView2.Rows[j].Visible = false;
                }
                if (fname == "TechnicianProductivity")
                {
                    if (GridView2.Rows[j].Cells[0].Text == "Daily No. of Box Checked: LEVEL A+&gt;35.00; LEVEL A&lt;=35.00; LEVEL B&lt;=25.00; LEVEL C&lt;20.00")
                    {
                        GridView2.Rows[j].Cells[0].Wrap = true;
                        GridView2.Rows[j].BackColor = System.Drawing.Color.Orange;
                        GridView2.Rows[j].ForeColor = System.Drawing.Color.Black;
                    }
                    if (GridView2.Rows[j].Cells[0].Text == "Performance Grade by Daily No. of Box Repaired: LEVEL A+&gt;20.00; LEVEL A&lt;=20.00; LEVEL B&lt;=15.00; LEVEL C&lt;10.00")
                    {
                        GridView2.Rows[j].Cells[0].Wrap = true;
                        GridView2.Rows[j].BackColor = System.Drawing.Color.Chocolate;
                        GridView2.Rows[j].ForeColor = System.Drawing.Color.Black;
                    }
                }
            }

            GridView2.RenderControl(hw1);

            int gridvalue = 0;
            int evenOdd = 0;
            for (int i = 1; i <= GridView1.Rows.Count; i++)
            {
                //Apply text style to each Row 
                if (gridvalue != i)
                {
                    if ((GridView1.Rows[i - 1].Cells[0].Text == "&nbsp;" && GridView1.Rows[i - 1].Cells[1].Text == "&nbsp;") || (GridView1.Rows[i - 1].Cells[0].Text == "Total :"))
                    {
                        evenOdd += 1;
                        gridvalue = i + 1;
                        if (evenOdd % 2 != 0)
                        {
                            for (int k1 = 0; k1 < GridView1.Rows[i - 1].Cells.Count; k1++)
                            {
                                bool val = IsNumeric(GridView1.Rows[i - 1].Cells[k1].Text.ToString());
                                var strdatatype = datatable.Columns[k1].DataType.Name;
                                if (val == false)
                                {
                                    GridView1.Rows[i - 1].Cells[k1].HorizontalAlign = HorizontalAlign.Left;
                                    GridView1.Rows[i - 1].Cells[k1].Wrap = false;
                                }
                                else
                                {
                                    GridView1.Rows[i - 1].Cells[k1].HorizontalAlign = HorizontalAlign.Right;
                                    GridView1.Rows[i - 1].Cells[k1].Wrap = false;
                                    if (strdatatype == "Decimal")
                                    {
                                        if (GridView1.Rows[i - 1].Cells[k1].Text != "&nbsp;")
                                        {
                                            string[] datalength = GridView1.Rows[i - 1].Cells[k1].Text.Split(new char[] { '.' });
                                            int decimals = datalength[1].Length;
                                            if (decimals == 2)
                                            {
                                                GridView1.Rows[i - 1].Cells[k1].Attributes.Add("style", "mso-number-format:\\#\\,\\#\\#0\\.00");
                                            }
                                            else if (decimals == 3)
                                            {
                                                GridView1.Rows[i - 1].Cells[k1].Attributes.Add("style", "mso-number-format:\\#\\,\\#\\#0\\.000");
                                            }
                                            else if (decimals == 4)
                                            {
                                                GridView1.Rows[i - 1].Cells[k1].Attributes.Add("style", "mso-number-format:\\#\\,\\#\\#0\\.0000");
                                            }
                                        }
                                    }
                                    if (val == true)
                                    {
                                        if (Convert.ToDecimal(GridView1.Rows[i - 1].Cells[k1].Text.ToString()) < 0)
                                            GridView1.Rows[i - 1].Cells[k1].ForeColor = System.Drawing.Color.Red;
                                    }
                                }
                            }
                            GridView1.Rows[i - 1].Font.Bold = true;
                            GridView1.Rows[i - 1].Font.Name = "Tahoma";
                            GridView1.Rows[i - 1].Font.Size = 10;//new FontUnit("12px");
                            GridView1.Rows[i - 1].ForeColor = System.Drawing.Color.Blue;
                        }
                        else
                        {
                            for (int k2 = 0; k2 < GridView1.Rows[i - 1].Cells.Count; k2++)
                            {
                                bool val = IsNumeric(GridView1.Rows[i - 1].Cells[k2].Text.ToString());
                                var strdatatype = datatable.Columns[k2].DataType.Name;
                                if (val == false)
                                {
                                    GridView1.Rows[i - 1].Cells[k2].HorizontalAlign = HorizontalAlign.Left;
                                    GridView1.Rows[i - 1].Cells[k2].Wrap = false;
                                }
                                else
                                {
                                    GridView1.Rows[i - 1].Cells[k2].HorizontalAlign = HorizontalAlign.Right;
                                    GridView1.Rows[i - 1].Cells[k2].Wrap = false;
                                    if (strdatatype == "Decimal")
                                    {
                                        if (GridView1.Rows[i - 1].Cells[k2].Text != "&nbsp;")
                                        {
                                            string[] datalength = GridView1.Rows[i - 1].Cells[k2].Text.Split(new char[] { '.' });
                                            int decimals = datalength[1].Length;
                                            if (decimals == 2)
                                            {
                                                GridView1.Rows[i - 1].Cells[k2].Attributes.Add("style", "mso-number-format:\\#\\,\\#\\#0\\.00");
                                            }
                                            else if (decimals == 3)
                                            {
                                                GridView1.Rows[i - 1].Cells[k2].Attributes.Add("style", "mso-number-format:\\#\\,\\#\\#0\\.000");
                                            }
                                            else if (decimals == 4)
                                            {
                                                GridView1.Rows[i - 1].Cells[k2].Attributes.Add("style", "mso-number-format:\\#\\,\\#\\#0\\.0000");
                                            }
                                        }
                                    }
                                    if (val == true)
                                    {
                                        if (Convert.ToDecimal(GridView1.Rows[i - 1].Cells[k2].Text.ToString()) < 0)
                                            GridView1.Rows[i - 1].Cells[k2].ForeColor = System.Drawing.Color.Red;
                                    }
                                }
                            }
                            GridView1.Rows[i - 1].Font.Bold = true;
                            GridView1.Rows[i - 1].Font.Name = "Tahoma";
                            GridView1.Rows[i - 1].Font.Size = 10;//new FontUnit("10px");
                            GridView1.Rows[i - 1].ForeColor = System.Drawing.Color.Blue;
                        }
                    }
                    else if (compareString.Contains(GridView1.Rows[i - 1].Cells[0].Text) || compareString.Contains(GridView1.Rows[i - 1].Cells[1].Text) || compareString.Contains(GridView1.Rows[i - 1].Cells[2].Text) || compareString.Contains(GridView1.Rows[i - 1].Cells[3].Text)
                        || Convert.ToString(GridView1.Rows[i - 1].Cells[0].Text).Contains(compareString) || Convert.ToString(GridView1.Rows[i - 1].Cells[1].Text).Contains(compareString) || Convert.ToString(GridView1.Rows[i - 1].Cells[2].Text).Contains(compareString) || Convert.ToString(GridView1.Rows[i - 1].Cells[3].Text).Contains(compareString)
                        || compareString1.Contains(GridView1.Rows[i - 1].Cells[0].Text) || compareString1.Contains(GridView1.Rows[i - 1].Cells[1].Text) || compareString1.Contains(GridView1.Rows[i - 1].Cells[2].Text) || compareString1.Contains(GridView1.Rows[i - 1].Cells[3].Text)
                        || Convert.ToString(GridView1.Rows[i - 1].Cells[0].Text).Contains(compareString1) || Convert.ToString(GridView1.Rows[i - 1].Cells[1].Text).Contains(compareString1) || Convert.ToString(GridView1.Rows[i - 1].Cells[2].Text).Contains(compareString1) || Convert.ToString(GridView1.Rows[i - 1].Cells[3].Text).Contains(compareString1))
                    {
                        for (int k3 = 0; k3 < GridView1.Rows[i - 1].Cells.Count; k3++)
                        {
                            bool val = IsNumeric(GridView1.Rows[i - 1].Cells[k3].Text.ToString());
                            var strdatatype = datatable.Columns[k3].DataType.Name;
                            if (strdatatype == "String")
                            {
                                GridView1.Rows[i - 1].Cells[k3].HorizontalAlign = HorizontalAlign.Left;
                                GridView1.Rows[i - 1].Cells[k3].Wrap = false;
                            }
                            else
                            {
                                GridView1.Rows[i - 1].Cells[k3].HorizontalAlign = HorizontalAlign.Right;
                                GridView1.Rows[i - 1].Cells[k3].Wrap = false;
                                if (strdatatype == "Decimal")
                                {
                                    if (GridView1.Rows[i - 1].Cells[k3].Text != "&nbsp;")
                                    {
                                        string[] datalength = GridView1.Rows[i - 1].Cells[k3].Text.Split(new char[] { '.' });
                                        int decimals = datalength[1].Length;
                                        if (decimals == 2)
                                        {
                                            GridView1.Rows[i - 1].Cells[k3].Attributes.Add("style", "mso-number-format:\\#\\,\\#\\#0\\.00");
                                        }
                                        else if (decimals == 3)
                                        {
                                            GridView1.Rows[i - 1].Cells[k3].Attributes.Add("style", "mso-number-format:\\#\\,\\#\\#0\\.000");
                                        }
                                        else if (decimals == 4)
                                        {
                                            GridView1.Rows[i - 1].Cells[k3].Attributes.Add("style", "mso-number-format:\\#\\,\\#\\#0\\.0000");
                                        }
                                    }
                                }
                                if (val == true)
                                {
                                    if (Convert.ToDecimal(GridView1.Rows[i - 1].Cells[k3].Text.ToString()) < 0)
                                        GridView1.Rows[i - 1].Cells[k3].ForeColor = System.Drawing.Color.Red;
                                }
                            }
                        }
                        GridView1.Rows[i - 1].ForeColor = System.Drawing.Color.Maroon;
                        GridView1.Rows[i - 1].Font.Bold = true;
                        GridView1.Rows[i - 1].Font.Name = "Tahoma";
                        GridView1.Rows[i - 1].Font.Size = 10;//new FontUnit("10px");
                    }
                    else if (GridView1.Rows[i - 1].Cells[0].Text != "&nbsp;" && GridView1.Rows[i - 1].Cells[1].Text == "Test" && GridView1.Rows[i - 1].Cells[2].Text == "&nbsp;" && GridView1.Rows[i - 1].Cells[3].Text == "&nbsp;")
                    {
                        GridView1.Rows[i - 1].Font.Bold = true;
                        GridView1.Rows[i - 1].Font.Name = "Tahoma";
                        GridView1.Rows[i - 1].Font.Size = 10;// new FontUnit("10px");
                        GridView1.Rows[i - 1].Cells[0].Wrap = false;
                        GridView1.Rows[i - 1].Cells[1].Wrap = false;
                        GridView1.Rows[i - 1].Cells[2].Wrap = false;
                        GridView1.Rows[i - 1].Cells[3].Wrap = false;
                        GridView1.Rows[i - 1].HorizontalAlign = HorizontalAlign.Left;
                        GridView1.Rows[i - 1].ForeColor = System.Drawing.Color.Blue;
                        for (int cellNum = GridView1.Rows[i - 1].Cells.Count - 1; cellNum > 0; cellNum--)
                        {
                            GridView1.Rows[i - 1].Cells.RemoveAt(cellNum);
                        }
                    }
                    else
                    {
                        for (int k = 0; k < GridView1.Rows[i - 1].Cells.Count; k++)
                        {
                            bool val = IsNumeric(GridView1.Rows[i - 1].Cells[k].Text.ToString());
                            var strdatatype = datatable.Columns[k].DataType.Name;
                            if (strdatatype == "String")
                            {
                                GridView1.Rows[i - 1].Cells[k].HorizontalAlign = HorizontalAlign.Left;
                                GridView1.Rows[i - 1].Cells[k].Wrap = false;
                                if (GridView1.Rows[i - 1].Cells[k].Text.ToString() == "A+")
                                {
                                    GridView1.Rows[i - 1].Cells[k].BackColor = System.Drawing.Color.DarkGreen;
                                }

                                if (GridView1.Rows[i - 1].Cells[k].Text.ToString() == "A")
                                {
                                    GridView1.Rows[i - 1].Cells[k].BackColor = System.Drawing.Color.SeaGreen;
                                }

                                if (GridView1.Rows[i - 1].Cells[k].Text.ToString() == "B")
                                {
                                    GridView1.Rows[i - 1].Cells[k].BackColor = System.Drawing.Color.Yellow;
                                }

                                if (GridView1.Rows[i - 1].Cells[k].Text.ToString() == "C")
                                {
                                    GridView1.Rows[i - 1].Cells[k].BackColor = System.Drawing.Color.Red;
                                }
                            }
                            else
                            {
                                GridView1.Rows[i - 1].Cells[k].HorizontalAlign = HorizontalAlign.Right;
                                GridView1.Rows[i - 1].Cells[k].Wrap = false;
                                if (strdatatype == "Decimal")
                                {
                                    if (GridView1.Rows[i - 1].Cells[k].Text != "&nbsp;")
                                    {
                                        string[] datalength = GridView1.Rows[i - 1].Cells[k].Text.Split(new char[] { '.' });
                                        int decimals = datalength[1].Length;
                                        if (decimals == 2)
                                        {
                                            GridView1.Rows[i - 1].Cells[k].Attributes.Add("style", "mso-number-format:\\#\\,\\#\\#0\\.00");
                                        }
                                        else if (decimals == 3)
                                        {
                                            GridView1.Rows[i - 1].Cells[k].Attributes.Add("style", "mso-number-format:\\#\\,\\#\\#0\\.000");
                                        }
                                        else if (decimals == 4)
                                        {
                                            GridView1.Rows[i - 1].Cells[k].Attributes.Add("style", "mso-number-format:\\#\\,\\#\\#0\\.0000");
                                        }
                                    }
                                }
                                if (val == true)
                                {
                                    if (Convert.ToDecimal(GridView1.Rows[i - 1].Cells[k].Text.ToString()) < 0)
                                        GridView1.Rows[i - 1].Cells[k].ForeColor = System.Drawing.Color.Red;
                                }
                            }
                        }
                        GridView1.Rows[i - 1].Font.Bold = false;
                        GridView1.Rows[i - 1].Font.Name = "Tahoma";
                        GridView1.Rows[i - 1].Font.Size = 10;// new FontUnit("10px");

                    }
                    colNo = GridView1.Rows[i - 1].Cells.Count;
                }

            }
            GridView1.RenderControl(hw);

            HtmlTextWriter hw2 = new HtmlTextWriter(sw);
            GridView GridView3 = new GridView();
            GridView3.DataSource = dtFooter;
            GridView3.DataBind();
            GridView3.HeaderRow.Visible = false;
            GridView3.BorderWidth = 0;
            GridView3.GridLines = GridLines.Both;
            for (int j1 = 0; j1 < GridView3.Rows.Count; j1++)
            {
                GridView3.Rows[j1].Font.Bold = true;
                GridView3.Rows[j1].Font.Name = "Tahoma";
                GridView3.Rows[j1].Font.Size = 10;
                GridView3.Rows[j1].ForeColor = System.Drawing.Color.Blue;
                if (GridView3.Rows[j1].Cells[0].Text == "&nbsp;")
                {
                    GridView3.Rows[j1].Visible = false;
                }
            }

            GridView3.RenderControl(hw2);

            HttpContext.Current.Response.Output.Write(sw.ToString());
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        //For Cell BackColor

        public void ExportToCSV(DataTable datatable, string fname, string compareString)
        {
            //Create a dummy GridView 
            int colNo = 0;
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;
            GridView1.DataSource = datatable;
            GridView1.DataBind();
            GridView1.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            GridView1.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#E8E6E7");
            GridView1.HeaderStyle.Font.Bold = true;
            GridView1.HeaderStyle.Font.Size = new FontUnit("10px");
            GridView1.GridLines = GridLines.Both;
            GridView1.BorderWidth = 1;
            GridView1.HeaderStyle.Wrap = false;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + fname + ".csv");
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/text";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            //HtmlTextWriter hw1 = new HtmlTextWriter(sw);
            //GridView GridView2 = new GridView();
            //GridView2.DataSource = dtHeader;
            //GridView2.DataBind();
            //GridView2.HeaderRow.Visible = false;
            //GridView2.BorderWidth = 0;
            //for (int j = 0; j < GridView2.Rows.Count; j++)
            //{
            //    GridView2.Rows[j].Font.Bold = true;
            //    GridView2.Rows[j].Font.Size = new FontUnit("10px");
            //    //GridView2.Rows[j].HorizontalAlign = HorizontalAlign.Center;
            //    GridView2.Rows[j].ForeColor = System.Drawing.Color.Blue;
            //    //GridView2.Rows[j].Cells[0].ColumnSpan = GridView1.Rows[0].Cells.Count;
            //    //GridView2.Rows[j].Cells[0].BackColor = System.Drawing.Color.LightGray;
            //    GridView2.Rows[j].Cells[0].Wrap = false;
            //    if (GridView2.Rows[j].Cells[0].Text == "&nbsp;")
            //    {
            //        GridView2.Rows[j].Visible = false;
            //    }
            //}
            //GridView2.RenderControl(hw1);

            int gridvalue = 0;
            int evenOdd = 0;
            for (int i = 1; i <= GridView1.Rows.Count; i++)
            {
                //DataView dv = datatable.DefaultView;
                //dv.RowFilter = "FirstField LIKE 'whatever%'"

                //Apply text style to each Row 
                if (gridvalue != i)
                {
                    if (GridView1.Rows[i - 1].Cells[0].Text == "&nbsp;" && GridView1.Rows[i - 1].Cells[1].Text == "&nbsp;")
                    {
                        evenOdd += 1;
                        gridvalue = i + 1;
                        if (evenOdd % 2 != 0)
                        {
                            for (int k1 = 0; k1 < GridView1.Rows[i - 1].Cells.Count; k1++)
                            {
                                bool val = IsNumeric(GridView1.Rows[i - 1].Cells[k1].Text.ToString());
                                if (val == false)
                                {
                                    GridView1.Rows[i - 1].Cells[k1].HorizontalAlign = HorizontalAlign.Left;
                                    GridView1.Rows[i - 1].Cells[k1].Wrap = false;
                                }
                                else
                                {
                                    GridView1.Rows[i - 1].Cells[k1].HorizontalAlign = HorizontalAlign.Right;
                                    GridView1.Rows[i - 1].Cells[k1].Wrap = false;
                                    if (Convert.ToDecimal(GridView1.Rows[i - 1].Cells[k1].Text.ToString()) < 0)
                                        GridView1.Rows[i - 1].Cells[k1].ForeColor = System.Drawing.Color.Red;
                                }
                            }
                            GridView1.Rows[i - 1].Font.Bold = true;
                            GridView1.Rows[i - 1].Font.Name = "Tahoma";
                            GridView1.Rows[i - 1].Font.Size = 10;//new FontUnit("12px");
                            GridView1.Rows[i - 1].ForeColor = System.Drawing.Color.Blue;
                        }
                        else
                        {
                            for (int k2 = 0; k2 < GridView1.Rows[i - 1].Cells.Count; k2++)
                            {
                                bool val = IsNumeric(GridView1.Rows[i - 1].Cells[k2].Text.ToString());
                                if (val == false)
                                {
                                    GridView1.Rows[i - 1].Cells[k2].HorizontalAlign = HorizontalAlign.Left;
                                    GridView1.Rows[i - 1].Cells[k2].Wrap = false;
                                }
                                else
                                {
                                    GridView1.Rows[i - 1].Cells[k2].HorizontalAlign = HorizontalAlign.Right;
                                    GridView1.Rows[i - 1].Cells[k2].Wrap = false;
                                    if (Convert.ToDecimal(GridView1.Rows[i - 1].Cells[k2].Text.ToString()) < 0)
                                        GridView1.Rows[i - 1].Cells[k2].ForeColor = System.Drawing.Color.Red;
                                }
                            }
                            GridView1.Rows[i - 1].Font.Bold = false;
                            GridView1.Rows[i - 1].Font.Name = "Tahoma";
                            GridView1.Rows[i - 1].Font.Size = 10;//new FontUnit("10px");
                            GridView1.Rows[i - 1].ForeColor = System.Drawing.Color.Blue;
                        }
                    }
                    else if (compareString.Contains(GridView1.Rows[i - 1].Cells[0].Text) || compareString.Contains(GridView1.Rows[i - 1].Cells[1].Text) || compareString.Contains(GridView1.Rows[i - 1].Cells[2].Text) || compareString.Contains(GridView1.Rows[i - 1].Cells[3].Text))
                    {
                        for (int k3 = 0; k3 < GridView1.Rows[i - 1].Cells.Count; k3++)
                        {
                            bool val = IsNumeric(GridView1.Rows[i - 1].Cells[k3].Text.ToString());
                            if (val == false)
                            {
                                GridView1.Rows[i - 1].Cells[k3].HorizontalAlign = HorizontalAlign.Left;
                                GridView1.Rows[i - 1].Cells[k3].Wrap = false;
                            }
                            else
                            {
                                GridView1.Rows[i - 1].Cells[k3].HorizontalAlign = HorizontalAlign.Right;
                                GridView1.Rows[i - 1].Cells[k3].Wrap = false;
                                if (Convert.ToDecimal(GridView1.Rows[i - 1].Cells[k3].Text.ToString()) < 0)
                                    GridView1.Rows[i - 1].Cells[k3].ForeColor = System.Drawing.Color.Red;
                            }
                        }
                        GridView1.Rows[i - 1].ForeColor = System.Drawing.Color.Maroon;
                        GridView1.Rows[i - 1].Font.Bold = true;
                        GridView1.Rows[i - 1].Font.Name = "Tahoma";
                        GridView1.Rows[i - 1].Font.Size = 10;//new FontUnit("10px");
                    }
                    else if (GridView1.Rows[i - 1].Cells[0].Text != "&nbsp;" && GridView1.Rows[i - 1].Cells[1].Text == "Test" && GridView1.Rows[i - 1].Cells[2].Text == "&nbsp;" && GridView1.Rows[i - 1].Cells[3].Text == "&nbsp;")
                    {
                        GridView1.Rows[i - 1].Font.Bold = true;
                        GridView1.Rows[i - 1].Font.Name = "Tahoma";
                        GridView1.Rows[i - 1].Font.Size = 10;// new FontUnit("10px");
                        GridView1.Rows[i - 1].Cells[0].Wrap = false;
                        GridView1.Rows[i - 1].Cells[1].Wrap = false;
                        GridView1.Rows[i - 1].Cells[2].Wrap = false;
                        GridView1.Rows[i - 1].Cells[3].Wrap = false;
                        GridView1.Rows[i - 1].HorizontalAlign = HorizontalAlign.Left;
                        GridView1.Rows[i - 1].ForeColor = System.Drawing.Color.Blue;
                        //GridView1.Rows[i - 1].Cells[0].ColumnSpan = GridView1.Rows[i - 1].Cells.Count;
                        //GridView1.Rows[i - 1].Cells[0].BackColor = System.Drawing.Color.LightYellow;
                        for (int cellNum = GridView1.Rows[i - 1].Cells.Count - 1; cellNum > 0; cellNum--)
                        {
                            GridView1.Rows[i - 1].Cells.RemoveAt(cellNum);
                        }
                    }
                    else
                    {
                        for (int k = 0; k < GridView1.Rows[i - 1].Cells.Count; k++)
                        {
                            bool val = IsNumeric(GridView1.Rows[i - 1].Cells[k].Text.ToString());
                            if (val == false)
                            {
                                GridView1.Rows[i - 1].Cells[k].HorizontalAlign = HorizontalAlign.Left;
                                GridView1.Rows[i - 1].Cells[k].Wrap = false;
                            }
                            else
                            {
                                GridView1.Rows[i - 1].Cells[k].HorizontalAlign = HorizontalAlign.Right;
                                //BoundField col = (BoundField)GridView1.Columns[k];
                                //col.DataFormatString = "0.00";
                                GridView1.Rows[i - 1].Cells[k].Text = String.Format("{0:#.##}", GridView1.Rows[i - 1].Cells[k].Text);
                                GridView1.Rows[i - 1].Cells[k].Wrap = false;
                                if (Convert.ToDecimal(GridView1.Rows[i - 1].Cells[k].Text.ToString()) < 0)
                                    GridView1.Rows[i - 1].Cells[k].ForeColor = System.Drawing.Color.Red;

                            }
                        }
                        GridView1.Rows[i - 1].Font.Bold = false;
                        GridView1.Rows[i - 1].Font.Name = "Tahoma";
                        GridView1.Rows[i - 1].Font.Size = 10;// new FontUnit("10px");

                    }
                    colNo = GridView1.Rows[i - 1].Cells.Count;
                }

            }
            GridView1.RenderControl(hw);

            //HtmlTextWriter hw2 = new HtmlTextWriter(sw);
            //GridView GridView3 = new GridView();
            //GridView3.DataSource = dtFooter;
            //GridView3.DataBind();
            //GridView3.HeaderRow.Visible = false;
            //GridView3.BorderWidth = 0;
            //GridView3.GridLines = GridLines.Both;
            //for (int j1 = 0; j1 < GridView3.Rows.Count; j1++)
            //{
            //    GridView3.Rows[j1].Font.Bold = true;
            //    GridView3.Rows[j1].Font.Size = new FontUnit("10px");
            //    //GridView3.Rows[j1].HorizontalAlign = HorizontalAlign.Center;
            //    GridView3.Rows[j1].ForeColor = System.Drawing.Color.Blue;
            //    //GridView3.Rows[j1].Cells[0].ColumnSpan = colNo;
            //    //GridView3.Rows[j1].Cells[0].BackColor = System.Drawing.Color.LightYellow;
            //    if (GridView3.Rows[j1].Cells[0].Text == "&nbsp;")
            //    {
            //        GridView3.Rows[j1].Visible = false;
            //    }
            //}
            //GridView3.RenderControl(hw2);

            //style to format numbers to string 
            //string style = @"<style> .textmode { mso-number-format:\#\,\#\#0\.00_ \;\[Red\]\-\#\,\#\#0\.00\ ; } </style>";
            //HttpContext.Current.Response.Write(style);
            HttpContext.Current.Response.Output.Write(sw.ToString());
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        public void ExportToExcelforCustomQuery(DataTable datatable, string fname, DataTable dtHeader, DataTable dtFooter)
        {
            //Create a dummy GridView 
            int colNo = 0;
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;
            GridView1.DataSource = datatable;
            GridView1.DataBind();
            GridView1.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            GridView1.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#E8E6E7");
            GridView1.HeaderStyle.Font.Bold = true;
            GridView1.HeaderStyle.Font.Size = new FontUnit("10px");
            GridView1.GridLines = GridLines.Both;
            GridView1.BorderWidth = 0;
            GridView1.HeaderStyle.Wrap = false;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + fname + ".xls");
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            HtmlTextWriter hw1 = new HtmlTextWriter(sw);
            GridView GridView2 = new GridView();
            GridView2.DataSource = dtHeader;
            GridView2.DataBind();
            GridView2.HeaderRow.Visible = false;
            GridView2.BorderWidth = 0;
            for (int j = 0; j < GridView2.Rows.Count; j++)
            {
                GridView2.Rows[j].Font.Bold = true;
                GridView2.Rows[j].Font.Size = new FontUnit("10px");
                //GridView2.Rows[j].HorizontalAlign = HorizontalAlign.Center;
                GridView2.Rows[j].ForeColor = System.Drawing.Color.Blue;
                //GridView2.Rows[j].Cells[0].ColumnSpan = GridView1.Rows[0].Cells.Count;
                //GridView2.Rows[j].Cells[0].BackColor = System.Drawing.Color.LightGray;
                GridView2.Rows[j].Cells[0].Wrap = false;
                if (GridView2.Rows[j].Cells[0].Text == "&nbsp;")
                {
                    GridView2.Rows[j].Visible = false;
                }
            }
            GridView2.RenderControl(hw1);

            int gridvalue = 0;
            int evenOdd = 0;
            for (int i = 1; i <= GridView1.Rows.Count; i++)
            {
                //DataView dv = datatable.DefaultView;
                //dv.RowFilter = "FirstField LIKE 'whatever%'"

                //Apply text style to each Row 
                if (gridvalue != i)
                {
                    if (GridView1.Rows[i - 1].Cells[0].Text == "&nbsp;" && GridView1.Rows[i - 1].Cells[1].Text == "&nbsp;")
                    {
                        evenOdd += 1;
                        gridvalue = i + 1;
                        if (evenOdd % 2 != 0)
                        {
                            for (int k1 = 0; k1 < GridView1.Rows[i].Cells.Count; k1++)
                            {
                                bool val = IsNumeric(GridView1.Rows[i].Cells[k1].Text.ToString());
                                if (val == false)
                                {
                                    GridView1.Rows[i].Cells[k1].HorizontalAlign = HorizontalAlign.Left;
                                    GridView1.Rows[i].Cells[k1].Wrap = false;
                                }
                                else
                                {
                                    GridView1.Rows[i].Cells[k1].HorizontalAlign = HorizontalAlign.Right;
                                    GridView1.Rows[i].Cells[k1].Wrap = false;
                                    if (Convert.ToDecimal(GridView1.Rows[i].Cells[k1].Text.ToString()) < 0)
                                        GridView1.Rows[i].Cells[k1].ForeColor = System.Drawing.Color.Red;
                                }
                            }
                            GridView1.Rows[i].Font.Bold = true;
                            GridView1.Rows[i].Font.Size = new FontUnit("10px");
                            GridView1.Rows[i].ForeColor = System.Drawing.Color.Blue;
                        }
                        else
                        {
                            for (int k2 = 0; k2 < GridView1.Rows[i].Cells.Count; k2++)
                            {
                                bool val = IsNumeric(GridView1.Rows[i].Cells[k2].Text.ToString());
                                if (val == false)
                                {
                                    GridView1.Rows[i].Cells[k2].HorizontalAlign = HorizontalAlign.Left;
                                    GridView1.Rows[i].Cells[k2].Wrap = false;
                                }
                                else
                                {
                                    GridView1.Rows[i].Cells[k2].HorizontalAlign = HorizontalAlign.Right;
                                    GridView1.Rows[i].Cells[k2].Wrap = false;
                                    if (Convert.ToDecimal(GridView1.Rows[i].Cells[k2].Text.ToString()) < 0)
                                        GridView1.Rows[i].Cells[k2].ForeColor = System.Drawing.Color.Red;
                                }
                            }
                            GridView1.Rows[i].Font.Bold = false;
                            GridView1.Rows[i].Font.Size = new FontUnit("10px");
                            GridView1.Rows[i].ForeColor = System.Drawing.Color.Blue;
                        }
                    }
                    else
                    {
                        for (int k = 0; k < GridView1.Rows[i - 1].Cells.Count; k++)
                        {
                            bool val = IsNumeric(GridView1.Rows[i - 1].Cells[k].Text.ToString());
                            if (val == false)
                            {
                                GridView1.Rows[i - 1].Cells[k].HorizontalAlign = HorizontalAlign.Left;
                                GridView1.Rows[i - 1].Cells[k].Wrap = false;
                            }
                            else
                            {
                                GridView1.Rows[i - 1].Cells[k].HorizontalAlign = HorizontalAlign.Right;
                                GridView1.Rows[i - 1].Cells[k].Wrap = false;
                                if (Convert.ToDecimal(GridView1.Rows[i - 1].Cells[k].Text.ToString()) < 0)
                                    GridView1.Rows[i - 1].Cells[k].ForeColor = System.Drawing.Color.Red;

                            }
                        }
                        GridView1.Rows[i - 1].Font.Bold = false;
                        GridView1.Rows[i - 1].Font.Size = new FontUnit("10px");

                    }
                    colNo = GridView1.Rows[i - 1].Cells.Count;
                }

            }
            GridView1.RenderControl(hw);

            HtmlTextWriter hw2 = new HtmlTextWriter(sw);
            GridView GridView3 = new GridView();
            GridView3.DataSource = dtFooter;
            GridView3.DataBind();
            GridView3.HeaderRow.Visible = false;
            GridView3.BorderWidth = 0;
            GridView3.GridLines = GridLines.Both;
            for (int j1 = 0; j1 < GridView3.Rows.Count; j1++)
            {
                GridView3.Rows[j1].Font.Bold = true;
                GridView3.Rows[j1].Font.Size = new FontUnit("10px");
                //GridView3.Rows[j1].HorizontalAlign = HorizontalAlign.Center;
                GridView3.Rows[j1].ForeColor = System.Drawing.Color.Blue;
                //GridView3.Rows[j1].Cells[0].ColumnSpan = colNo;
                //GridView3.Rows[j1].Cells[0].BackColor = System.Drawing.Color.LightYellow;
                if (GridView3.Rows[j1].Cells[0].Text == "&nbsp;")
                {
                    GridView3.Rows[j1].Visible = false;
                }
            }
            GridView3.RenderControl(hw2);

            //style to format numbers to string 
            //string style = @"<style> .textmode { mso-number-format:\#\,\#\#0\.00_ \;\[Red\]\-\#\,\#\#0\.00\ ; } </style>";
            //HttpContext.Current.Response.Write(style);
            HttpContext.Current.Response.Output.Write(sw.ToString());
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        #endregion

        #region
        //public void ExportToPDF(DataTable datatable, string fname, string compareString)
        //{
        //    GridView GridView1 = new GridView();
        //    GridView1.AllowPaging = false;
        //    GridView1.DataSource = datatable;
        //    GridView1.DataBind();
        //    GridView1.HeaderStyle.ForeColor = System.Drawing.Color.Blue;
        //    GridView1.HeaderStyle.Font.Bold = true;
        //    GridView1.HeaderStyle.Font.Size = new FontUnit("7px");
        //    GridView1.HeaderStyle.Wrap = false;        
        //    HttpContext.Current.Response.ContentType = "application/pdf";
        //    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + fname + ".pdf");
        //    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    StringWriter sw = new StringWriter();
        //    HtmlTextWriter hw = new HtmlTextWriter(sw);

        //    int gridvalue = 0;
        //    int evenOdd = 0;
        //    for (int i = 1; i <= GridView1.Rows.Count; i++)
        //    {
        //        //DataView dv = datatable.DefaultView;
        //        //dv.RowFilter = "FirstField LIKE 'whatever%'"

        //        //Apply text style to each Row 
        //        if (gridvalue != i)
        //        {
        //            if (GridView1.Rows[i - 1].Cells[0].Text == "&nbsp;" && GridView1.Rows[i - 1].Cells[1].Text == "&nbsp;")
        //            {
        //                evenOdd += 1;
        //                gridvalue = i + 1;
        //                if (evenOdd % 2 != 0)
        //                {
        //                    for (int k1 = 0; k1 < GridView1.Rows[i].Cells.Count; k1++)
        //                    {
        //                        bool val = IsNumeric(GridView1.Rows[i].Cells[k1].Text.ToString());
        //                        if (val == false)
        //                        {
        //                            GridView1.Rows[i].Cells[k1].HorizontalAlign = HorizontalAlign.Left;
        //                        }
        //                        else
        //                        {
        //                            GridView1.Rows[i].Cells[k1].HorizontalAlign = HorizontalAlign.Right;
        //                        }
        //                    }
        //                    GridView1.Rows[i].Font.Bold = true;
        //                    GridView1.Rows[i].Font.Size = new FontUnit("6px");
        //                    GridView1.Rows[i].ForeColor = System.Drawing.Color.Blue;
        //                }
        //                else
        //                {
        //                    for (int k2 = 0; k2 < GridView1.Rows[i].Cells.Count; k2++)
        //                    {
        //                        bool val = IsNumeric(GridView1.Rows[i].Cells[k2].Text.ToString());
        //                        if (val == false)
        //                        {
        //                            GridView1.Rows[i].Cells[k2].HorizontalAlign = HorizontalAlign.Left;
        //                        }
        //                        else
        //                        {
        //                            GridView1.Rows[i].Cells[k2].HorizontalAlign = HorizontalAlign.Right;
        //                        }
        //                    }
        //                    GridView1.Rows[i].Font.Bold = false;
        //                    GridView1.Rows[i].Font.Size = new FontUnit("6px");
        //                    GridView1.Rows[i].ForeColor = System.Drawing.Color.Blue;
        //                }
        //            }
        //            if (GridView1.Rows[i - 1].Cells[0].Text == compareString || GridView1.Rows[i - 1].Cells[1].Text == compareString || GridView1.Rows[i - 1].Cells[2].Text == compareString || GridView1.Rows[i - 1].Cells[3].Text == compareString)
        //            {
        //                for (int k3 = 0; k3 < GridView1.Rows[i - 1].Cells.Count; k3++)
        //                {
        //                    bool val = IsNumeric(GridView1.Rows[i - 1].Cells[k3].Text.ToString());
        //                    if (val == false)
        //                    {
        //                        GridView1.Rows[i - 1].Cells[k3].HorizontalAlign = HorizontalAlign.Left;
        //                    }
        //                    else
        //                    {
        //                        GridView1.Rows[i - 1].Cells[k3].HorizontalAlign = HorizontalAlign.Right;
        //                    }
        //                }
        //                GridView1.Rows[i - 1].ForeColor = System.Drawing.Color.Blue;
        //                GridView1.Rows[i - 1].Font.Bold = true;
        //                GridView1.Rows[i - 1].Font.Size = new FontUnit("6px");
        //            }
        //            else if (GridView1.Rows[i - 1].Cells[0].Text != "&nbsp;" && GridView1.Rows[i - 1].Cells[1].Text == "&nbsp;" && GridView1.Rows[i - 1].Cells[2].Text == "&nbsp;" && GridView1.Rows[i - 1].Cells[3].Text == "&nbsp;")
        //            {
        //                GridView1.Rows[i - 1].Font.Bold = true;
        //                GridView1.Rows[i - 1].Font.Size = new FontUnit("6px");
        //                GridView1.Rows[i - 1].HorizontalAlign = HorizontalAlign.Left;
        //                GridView1.Rows[i - 1].ForeColor = System.Drawing.Color.Blue;
        //                GridView1.Rows[i - 1].Cells[0].ColumnSpan = GridView1.Rows[i - 1].Cells.Count;
        //                for (int cellNum = GridView1.Rows[i - 1].Cells.Count - 1; cellNum > 0; cellNum--)
        //                {
        //                    GridView1.Rows[i - 1].Cells.RemoveAt(cellNum);
        //                }
        //            }
        //            else
        //            {
        //                for (int k = 0; k < GridView1.Rows[i - 1].Cells.Count; k++)
        //                {
        //                    bool val = IsNumeric(GridView1.Rows[i - 1].Cells[k].Text.ToString());
        //                    if (val == false)
        //                    {
        //                        GridView1.Rows[i - 1].Cells[k].HorizontalAlign = HorizontalAlign.Left;
        //                    }
        //                    else
        //                    {
        //                        GridView1.Rows[i - 1].Cells[k].HorizontalAlign = HorizontalAlign.Right;
        //                    }
        //                }
        //                GridView1.Rows[i - 1].Font.Bold = false;
        //                GridView1.Rows[i - 1].Font.Size = new FontUnit("6px");
        //            }
        //        }


        //    }       
        //    GridView1.RenderControl(hw);
        //    StringReader sr = new StringReader(sw.ToString());
        //    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
        //    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        //    PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
        //    pdfDoc.Open();
        //    htmlparser.Parse(sr);
        //    pdfDoc.Close();
        //    HttpContext.Current.Response.Write(pdfDoc);
        //    HttpContext.Current.Response.End();

        //}   
        #endregion
        public void ExportToPDF(DataTable datatable, string fname, string compareString, string compareString1, DataTable dtHeader, DataTable dtFooter)
        {
            Document pdfReport = new Document(PageSize.A4, 25, 25, 40, 25);
            System.IO.MemoryStream msReport = new System.IO.MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(pdfReport, msReport);
            pdfReport.Open();
            PdfPTable ptHeader = new PdfPTable(dtHeader.Columns.Count);
            for (int intH = 1; intH <= dtHeader.Rows.Count; intH++)
            {
                PdfPCell cell = new PdfPCell();
                cell.BorderWidth = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                // cell.Colspan = dtHeader.Columns.Count;
                // cell.BackgroundColor = CMYKColor.LIGHT_GRAY;
                cell.Phrase = new Phrase(dtHeader.Rows[intH - 1][0].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 8, Font.BOLD, CMYKColor.BLUE));
                ptHeader.AddCell(cell);
            }
            pdfReport.Add(ptHeader);
            //pdfReport.Close();

            if (datatable.Columns.Count != null && datatable.Columns.Count > 0)
            {
                PdfPTable ptData = new PdfPTable(datatable.Columns.Count);

                float[] headerwidths = new float[datatable.Columns.Count]; // percentage
                for (int intK = 0; intK < datatable.Columns.Count; intK++)
                {
                    if (datatable.Columns[intK].ColumnName.ToString().ToUpper().Contains("DESC"))
                        headerwidths[intK] = (100 / (datatable.Columns.Count + 2)) * 3;
                    else
                        headerwidths[intK] = 100 / (datatable.Columns.Count + 2);
                }
                ptData.SetWidths(headerwidths);
                ptData.WidthPercentage = 100;
                ptData.DefaultCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                ptData.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                //Insert the Table Headers
                for (int intK = 0; intK < datatable.Columns.Count; intK++)
                {
                    PdfPCell cell = new PdfPCell();
                    cell.BorderWidth = 0.001f;
                    cell.Phrase = new Phrase(datatable.Columns[intK].ColumnName.ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 7, Font.BOLD, CMYKColor.BLACK));
                    ptData.AddCell(cell);
                }

                ptData.HeaderRows = 1;  // this is the end of the table header

                //Insert the Table Data
                int gridvalue = 0;
                int evenOdd = 0;
                for (int intJ = 1; intJ <= datatable.Rows.Count; intJ++)
                {

                    if (gridvalue != intJ)
                    {
                        if ((datatable.Rows[intJ - 1][0] == DBNull.Value && datatable.Rows[intJ - 1][1] == DBNull.Value) || (Convert.ToString(datatable.Rows[intJ - 1][0]) == "Total :"))
                        {
                            evenOdd += 1;
                            gridvalue = intJ + 1;
                            if (evenOdd % 2 != 0)
                            {
                                for (int intK = 0; intK < datatable.Columns.Count; intK++)
                                {
                                    bool val = IsNumeric(datatable.Rows[intJ - 1][intK].ToString());
                                    if (val == false)
                                    {
                                        PdfPCell cell = new PdfPCell();
                                        cell.BorderWidth = 0.001f;
                                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //cell.Phrase = new Phrase(datatable.Rows[intJ - 1][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.BLACK));
                                        cell.Phrase = new Phrase(datatable.Rows[intJ - 1][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.BOLD, CMYKColor.BLUE));
                                        ptData.AddCell(cell);
                                    }
                                    else
                                    {
                                        PdfPCell cell = new PdfPCell();
                                        cell.BorderWidth = 0.001f;
                                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        if (Convert.ToDecimal(datatable.Rows[intJ - 1][intK].ToString()) < 0)
                                            //cell.Phrase = new Phrase(datatable.Rows[intJ - 1][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.BLACK));
                                            cell.Phrase = new Phrase(datatable.Rows[intJ - 1][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.RED));
                                        else
                                            //cell.Phrase = new Phrase(datatable.Rows[intJ - 1][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.BLACK));
                                            cell.Phrase = new Phrase(datatable.Rows[intJ - 1][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.BLUE));

                                        ptData.AddCell(cell);
                                    }

                                }
                            }
                            else
                            {
                                for (int intK = 0; intK < datatable.Columns.Count; intK++)
                                {
                                    bool val = IsNumeric(datatable.Rows[intJ - 1][intK].ToString());
                                    if (val == false)
                                    {
                                        PdfPCell cell = new PdfPCell();
                                        cell.BorderWidth = 0.001f;
                                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //cell.Phrase = new Phrase(datatable.Rows[intJ - 1][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.BLACK));
                                        cell.Phrase = new Phrase(datatable.Rows[intJ - 1][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.BLUE));
                                        ptData.AddCell(cell);
                                    }
                                    else
                                    {
                                        PdfPCell cell = new PdfPCell();
                                        cell.BorderWidth = 0.001f;
                                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        if (Convert.ToDecimal(datatable.Rows[intJ - 1][intK].ToString()) < 0)
                                            //cell.Phrase = new Phrase(datatable.Rows[intJ - 1][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.BLACK));
                                            cell.Phrase = new Phrase(datatable.Rows[intJ - 1][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.RED));
                                        else
                                            //cell.Phrase = new Phrase(datatable.Rows[intJ - 1][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.BLACK));
                                            cell.Phrase = new Phrase(datatable.Rows[intJ - 1][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.BLUE));
                                        ptData.AddCell(cell);
                                    }

                                }
                            }
                        }
                        else if (datatable.Rows[intJ - 1][0].ToString() == compareString || datatable.Rows[intJ - 1][1].ToString() == compareString || datatable.Rows[intJ - 1][2].ToString() == compareString || datatable.Rows[intJ - 1][3].ToString() == compareString
                            || Convert.ToString(datatable.Rows[intJ - 1][0]).Contains(compareString) || Convert.ToString(datatable.Rows[intJ - 1][1]).Contains(compareString) || Convert.ToString(datatable.Rows[intJ - 1][2]).Contains(compareString) || Convert.ToString(datatable.Rows[intJ - 1][3]).Contains(compareString)
                            || datatable.Rows[intJ - 1][0].ToString() == compareString1 || datatable.Rows[intJ - 1][1].ToString() == compareString1 || datatable.Rows[intJ - 1][2].ToString() == compareString1 || datatable.Rows[intJ - 1][3].ToString() == compareString1
                            || Convert.ToString(datatable.Rows[intJ - 1][0]).Contains(compareString1) || Convert.ToString(datatable.Rows[intJ - 1][1]).Contains(compareString1) || Convert.ToString(datatable.Rows[intJ - 1][2]).Contains(compareString1) || Convert.ToString(datatable.Rows[intJ - 1][3]).Contains(compareString1))
                        {
                            for (int intK = 0; intK < datatable.Columns.Count; intK++)
                            {
                                bool val = IsNumeric(datatable.Rows[intJ - 1][intK].ToString());
                                var strdatatype = datatable.Columns[intK].DataType.Name;
                                //if (val == false)
                                if (strdatatype == "String")
                                {
                                    PdfPCell cell = new PdfPCell();
                                    cell.BorderWidth = 0.001f;
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //cell.Phrase = new Phrase(datatable.Rows[intJ - 1][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.BLACK));
                                    cell.Phrase = new Phrase(datatable.Rows[intJ - 1][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.BOLD, CMYKColor.MAGENTA));
                                    ptData.AddCell(cell);
                                }
                                else
                                {
                                    PdfPCell cell = new PdfPCell();
                                    cell.BorderWidth = 0.001f;
                                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    if (val == true)
                                    {
                                        if (Convert.ToDecimal(datatable.Rows[intJ - 1][intK].ToString()) < 0)
                                            //cell.Phrase = new Phrase(datatable.Rows[intJ - 1][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.BLACK));
                                            cell.Phrase = new Phrase(datatable.Rows[intJ - 1][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.RED));
                                        else
                                            //cell.Phrase = new Phrase(datatable.Rows[intJ - 1][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.BLACK));
                                            cell.Phrase = new Phrase(datatable.Rows[intJ - 1][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.MAGENTA));
                                    }
                                    ptData.AddCell(cell);
                                }

                            }
                        }
                        else if (datatable.Rows[intJ - 1][0] != DBNull.Value && datatable.Rows[intJ - 1][1] == "Test" && datatable.Rows[intJ - 1][2] == DBNull.Value && datatable.Rows[intJ - 1][3] == DBNull.Value)
                        {
                            PdfPCell cell = new PdfPCell();
                            cell.BorderWidth = 0.001f;
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Colspan = datatable.Columns.Count;
                            // cell.BackgroundColor = CMYKColor.LIGHT_GRAY;
                            cell.Phrase = new Phrase(datatable.Rows[intJ - 1][0].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.BOLD, CMYKColor.BLACK));
                            ptData.AddCell(cell);
                        }
                        else
                        {
                            for (int intK = 0; intK < datatable.Columns.Count; intK++)
                            {
                                bool val = IsNumeric(datatable.Rows[intJ - 1][intK].ToString());
                                var strdatatype = datatable.Columns[intK].DataType.Name;
                                //if (val == false)
                                if (strdatatype=="String")
                                {
                                    PdfPCell cell = new PdfPCell();
                                    cell.BorderWidth = 0.001f;
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Phrase = new Phrase(datatable.Rows[intJ - 1][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6));
                                    ptData.AddCell(cell);
                                }
                                else
                                {
                                    PdfPCell cell = new PdfPCell();
                                    cell.BorderWidth = 0.001f;
                                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    if (val == true)
                                    {
                                        if (Convert.ToDecimal(datatable.Rows[intJ - 1][intK].ToString()) < 0)
                                            //cell.Phrase = new Phrase(datatable.Rows[intJ - 1][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.BLACK));
                                            cell.Phrase = new Phrase(datatable.Rows[intJ - 1][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.RED));
                                        else
                                            cell.Phrase = new Phrase(datatable.Rows[intJ - 1][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6));
                                    }
                                    ptData.AddCell(cell);
                                }

                            }
                        }
                    }

                }

                //Insert the Table

                pdfReport.Add(ptData);

                //Closes the Report and writes to Memory Stream
                PdfPTable ptFooter = new PdfPTable(dtFooter.Columns.Count);
                for (int intF = 1; intF <= dtFooter.Rows.Count; intF++)
                {
                    if (dtFooter.Rows[intF - 1][0] == DBNull.Value)
                    {
                        PdfPCell cell = new PdfPCell();
                        //  cell.BorderWidth = 0.001f;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        // cell.Colspan = dtFooter.Columns.Count;
                        cell.BackgroundColor = CMYKColor.WHITE;
                        cell.Phrase = new Phrase(dtFooter.Rows[intF - 1][0].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 8, Font.BOLD, CMYKColor.BLUE));
                        ptFooter.AddCell(cell);
                    }
                    else
                    {
                        PdfPCell cell = new PdfPCell();
                        //cell.BorderWidth = 0.001f;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //   cell.Colspan = dtFooter.Columns.Count;
                        cell.BackgroundColor = CMYKColor.WHITE;
                        cell.Phrase = new Phrase(dtFooter.Rows[intF - 1][0].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 8, Font.BOLD, CMYKColor.BLUE));
                        ptFooter.AddCell(cell);
                    }

                }
                pdfReport.Add(ptFooter);
                pdfReport.Close();
            }
            //Writes the Memory Stream Data to Response Object

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AppendHeader("content-disposition", "attachment;filename=" + fname + ".pdf");
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.BinaryWrite(msReport.ToArray());
            HttpContext.Current.Response.End();

        }
        public bool IsNumeric(string Value)
        {
            try
            {
                Convert.ToDecimal(Value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void ExportToExcelforExcel(DataTable datatable, string fname, string compareString, DataTable dtHeader, DataTable dtFooter, string Type)
        {
            //Create a dummy GridView 
            //Type Variable is Only Used For distinguish From Other Method But Not Used AnyWhere
            int colNo = 0;
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;
            GridView1.DataSource = datatable;
            GridView1.DataBind();
            GridView1.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            GridView1.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#E8E6E7");
            GridView1.HeaderStyle.Font.Bold = true;
            GridView1.HeaderStyle.Font.Size = new FontUnit("10px");
            GridView1.HeaderStyle.Wrap = false;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + fname + ".xls");
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            HtmlTextWriter hw1 = new HtmlTextWriter(sw);
            GridView GridView2 = new GridView();
            GridView2.DataSource = dtHeader;
            GridView2.DataBind();
            GridView2.HeaderRow.Visible = false;
            int j = 0;
            for (j = 0; j < GridView2.Rows.Count; j++)
            {
                GridView2.Rows[j].Font.Bold = true;
                GridView2.Rows[j].Font.Size = new FontUnit("10px");
                //GridView2.Rows[j].HorizontalAlign = HorizontalAlign.Center;
                GridView2.Rows[j].ForeColor = System.Drawing.Color.Blue;
                //GridView2.Rows[j].Cells[0].BackColor = System.Drawing.Color.LightGray;
                GridView2.Rows[j].Cells[0].Wrap = false;
                if (GridView2.Rows[j].Cells[0].Text == "&nbsp;")
                {
                    GridView2.Rows[j].Visible = false;
                }
            }

            GridView2.HeaderStyle.Width = Unit.Pixel(200);

            GridView2.RenderControl(hw1);

            int gridvalue = 0;
            int evenOdd = 0;
            for (int i = 1; i <= GridView1.Rows.Count; i++)
            {
                //DataView dv = datatable.DefaultView;
                //dv.RowFilter = "FirstField LIKE 'whatever%'"

                //Apply text style to each Row 
                if (gridvalue != i)
                {
                    if (GridView1.Rows[i - 1].Cells[0].Text == "&nbsp;" && GridView1.Rows[i - 1].Cells[1].Text == "&nbsp;")
                    {
                        evenOdd += 1;
                        gridvalue = i + 1;
                        if (evenOdd % 2 != 0)
                        {
                            for (int k1 = 0; k1 < GridView1.Rows[i].Cells.Count; k1++)
                            {
                                bool val = IsNumeric(GridView1.Rows[i].Cells[k1].Text.ToString());
                                if (val == false)
                                {
                                    //GridView1.Rows[i].Cells[k1].HorizontalAlign = HorizontalAlign.Left;
                                    GridView1.Rows[i].Cells[k1].Wrap = false;
                                }
                                else
                                {
                                    GridView1.Rows[i].Cells[k1].HorizontalAlign = HorizontalAlign.Right;
                                    GridView1.Rows[i].Cells[k1].Wrap = false;
                                    if (Convert.ToDecimal(GridView1.Rows[i].Cells[k1].Text.ToString()) < 0)
                                        GridView1.Rows[i].Cells[k1].ForeColor = System.Drawing.Color.Red;
                                }
                            }
                            GridView1.Rows[i].Font.Bold = true;
                            GridView1.Rows[i].Font.Size = new FontUnit("10px");
                            GridView1.Rows[i].ForeColor = System.Drawing.Color.Blue;
                        }
                        else
                        {
                            for (int k2 = 0; k2 < GridView1.Rows[i].Cells.Count; k2++)
                            {
                                bool val = IsNumeric(GridView1.Rows[i].Cells[k2].Text.ToString());
                                if (val == false)
                                {
                                    GridView1.Rows[i].Cells[k2].HorizontalAlign = HorizontalAlign.Left;
                                    GridView1.Rows[i].Cells[k2].Wrap = false;
                                }
                                else
                                {
                                    GridView1.Rows[i].Cells[k2].HorizontalAlign = HorizontalAlign.Right;
                                    GridView1.Rows[i].Cells[k2].Wrap = false;
                                    if (Convert.ToDecimal(GridView1.Rows[i].Cells[k2].Text.ToString()) < 0)
                                        GridView1.Rows[i].Cells[k2].ForeColor = System.Drawing.Color.Red;
                                }
                            }
                            GridView1.Rows[i].Font.Bold = false;
                            GridView1.Rows[i].Font.Size = new FontUnit("10px");
                            GridView1.Rows[i].ForeColor = System.Drawing.Color.Blue;
                        }
                    }
                    else if (GridView1.Rows[i - 1].Cells[0].Text == compareString || GridView1.Rows[i - 1].Cells[1].Text == compareString || GridView1.Rows[i - 1].Cells[2].Text == compareString || GridView1.Rows[i - 1].Cells[3].Text == compareString)
                    {
                        for (int k3 = 0; k3 < GridView1.Rows[i - 1].Cells.Count; k3++)
                        {
                            bool val = IsNumeric(GridView1.Rows[i - 1].Cells[k3].Text.ToString());
                            if (val == false)
                            {
                                //GridView1.Rows[i - 1].Cells[k3].HorizontalAlign = HorizontalAlign.Left;
                                GridView1.Rows[i - 1].Cells[k3].Wrap = false;
                            }
                            else
                            {
                                GridView1.Rows[i - 1].Cells[k3].HorizontalAlign = HorizontalAlign.Right;
                                GridView1.Rows[i - 1].Cells[k3].Wrap = false;
                                if (Convert.ToDecimal(GridView1.Rows[i - 1].Cells[k3].Text.ToString()) < 0)
                                    GridView1.Rows[i - 1].Cells[k3].ForeColor = System.Drawing.Color.Red;
                            }
                        }
                        GridView1.Rows[i - 1].ForeColor = System.Drawing.Color.Blue;
                        GridView1.Rows[i - 1].Font.Bold = true;
                        GridView1.Rows[i - 1].Font.Size = new FontUnit("10px");
                    }
                    else if (GridView1.Rows[i - 1].Cells[0].Text != "&nbsp;" && GridView1.Rows[i - 1].Cells[1].Text == "Test" && GridView1.Rows[i - 1].Cells[2].Text == "&nbsp;" && GridView1.Rows[i - 1].Cells[3].Text == "&nbsp;")
                    {
                        GridView1.Rows[i - 1].Font.Bold = true;
                        GridView1.Rows[i - 1].Font.Size = new FontUnit("10px");
                        //GridView1.Rows[i - 1].HorizontalAlign = HorizontalAlign.Left;
                        GridView1.Rows[i - 1].ForeColor = System.Drawing.Color.Blue;
                        //GridView1.Rows[i - 1].Cells[0].ColumnSpan = GridView1.Rows[i - 1].Cells.Count;
                        //GridView1.Rows[i - 1].Cells[0].BackColor = System.Drawing.Color.LightYellow;
                        for (int cellNum = GridView1.Rows[i - 1].Cells.Count - 1; cellNum > 0; cellNum--)
                        {
                            GridView1.Rows[i - 1].Cells.RemoveAt(cellNum);
                        }
                    }
                    else
                    {
                        for (int k = 0; k < GridView1.Rows[i - 1].Cells.Count; k++)
                        {
                            bool val = IsNumeric(GridView1.Rows[i - 1].Cells[k].Text.ToString());
                            if (val == false)
                            {
                                GridView1.Rows[i - 1].Cells[k].HorizontalAlign = HorizontalAlign.Left;
                                GridView1.Rows[i - 1].Cells[k].Wrap = false;
                            }
                            else
                            {
                                GridView1.Rows[i - 1].Cells[k].HorizontalAlign = HorizontalAlign.Right;
                                GridView1.Rows[i - 1].Cells[k].Wrap = false;
                                if (Convert.ToDecimal(GridView1.Rows[i - 1].Cells[k].Text.ToString()) < 0)
                                    GridView1.Rows[i - 1].Cells[k].ForeColor = System.Drawing.Color.Red;

                            }
                        }
                        GridView1.Rows[i - 1].Font.Bold = false;
                        GridView1.Rows[i - 1].Font.Size = new FontUnit("10px");

                    }
                    colNo = GridView1.Rows[i - 1].Cells.Count;
                }

            }
            GridView1.RenderControl(hw);

            HtmlTextWriter hw2 = new HtmlTextWriter(sw);
            GridView GridView3 = new GridView();
            GridView3.DataSource = dtFooter;
            GridView3.DataBind();
            GridView3.HeaderRow.Visible = false;
            for (int j1 = 0; j1 < GridView3.Rows.Count; j1++)
            {
                GridView3.Rows[j1].Font.Bold = true;
                GridView3.Rows[j1].Font.Size = new FontUnit("10px");
                //GridView3.Rows[j1].HorizontalAlign = HorizontalAlign.Center;
                GridView3.Rows[j1].ForeColor = System.Drawing.Color.Blue;
                //GridView3.Rows[j1].Cells[0].BackColor = System.Drawing.Color.LightYellow;
                if (GridView3.Rows[j1].Cells[0].Text == "&nbsp;")
                {
                    GridView3.Rows[j1].Visible = false;
                }
            }
            GridView3.RenderControl(hw2);

            //style to format numbers to string 
            //string style = @"<style> .textmode { mso-number-format:\#\,\#\#0\.00_ \;\[Red\]\-\#\,\#\#0\.00\ ; } </style>";
            //HttpContext.Current.Response.Write(style);
            HttpContext.Current.Response.Output.Write(sw.ToString());
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        public void ExportToExcelforExcel1(DataTable datatable, string fname, string compareString, DataTable dtHeader, DataTable dtFooter)
        {
            //Create a dummy GridView 
            int colNo = 0;
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;
            StringWriter sw = new StringWriter();
            DataTable dtTable = new DataTable();
            dtTable = datatable.Clone();
            DataRow newRow = dtTable.NewRow();
            for (int ij = 0; ij < datatable.Rows.Count; ij++)
            {
                newRow = dtTable.NewRow();
                newRow[0] = datatable.Rows[ij][0];
                newRow[1] = datatable.Rows[ij][1];
                newRow[2] = datatable.Rows[ij][2];
                newRow[3] = datatable.Rows[ij][3];
                newRow[4] = datatable.Rows[ij][4];
                newRow[5] = datatable.Rows[ij][5];
                newRow[6] = datatable.Rows[ij][6];
                newRow[7] = datatable.Rows[ij][7];
                newRow[8] = datatable.Rows[ij][8];
                if (datatable.Rows[ij][0].ToString() == "a")
                {
                    //Label lbl_ik = new Label();
                    //lbl_ik.Text = datatable.Rows[ij][3].ToString();
                    //HtmlTextWriter ahw = new HtmlTextWriter(sw);
                    //lbl_ik.RenderControl(ahw);
                    //Table tbid = (Table)datatable.Rows[ij][3];
                    HtmlTable tb = (HtmlTable)datatable.Rows[ij][3]; //table1 is the id of HTML Table                
                    int rowscount = tb.Rows.Count;

                    //for (int i = 0; i < rowscount; i++)
                    //{
                    //    HtmlTableCellCollection tcs = tb.Rows[i].Cells;

                    //    DataRow dr = dt.NewRow();
                    //    dr["a1"] = tcs[0].InnerText;
                    //    dr["a2"] = tcs[1].InnerText;
                    //    dt.Rows.Add(dr);
                    //}


                }
                dtTable.Rows.Add(newRow);
            }
            GridView1.DataSource = dtTable;
            GridView1.DataBind();
            GridView1.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            GridView1.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#E8E6E7");
            GridView1.HeaderStyle.Font.Bold = true;
            GridView1.HeaderStyle.Font.Size = new FontUnit("15px");
            GridView1.HeaderStyle.Wrap = false;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + fname + ".xls");
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";

            HtmlTextWriter hw = new HtmlTextWriter(sw);

            HtmlTextWriter hw1 = new HtmlTextWriter(sw);
            GridView GridView2 = new GridView();
            GridView2.DataSource = dtHeader;
            GridView2.DataBind();
            GridView2.HeaderRow.Visible = false;
            for (int j = 0; j < GridView2.Rows.Count; j++)
            {
                GridView2.Rows[j].Font.Bold = true;
                GridView2.Rows[j].Font.Size = new FontUnit("14px");
                GridView2.Rows[j].HorizontalAlign = HorizontalAlign.Center;
                GridView2.Rows[j].ForeColor = System.Drawing.Color.Blue;
                //GridView2.Rows[j].Cells[0].ColumnSpan = GridView1.Rows[0].Cells.Count;
                //GridView2.Rows[j].Cells[0].BackColor = System.Drawing.Color.LightYellow;
                GridView2.Rows[j].Cells[0].Wrap = false;
                if (GridView2.Rows[j].Cells[0].Text == "&nbsp;")
                {
                    GridView2.Rows[j].Visible = false;
                }
            }
            GridView2.RenderControl(hw1);

            int gridvalue = 0;
            int evenOdd = 0;
            for (int i = 1; i <= GridView1.Rows.Count; i++)
            {
                //DataView dv = datatable.DefaultView;
                //dv.RowFilter = "FirstField LIKE 'whatever%'"

                //Apply text style to each Row 
                if (gridvalue != i)
                {
                    //if (GridView1.Rows[i - 1].Cells[0].Text == "a")
                    //{
                    //    Label lbl_ik = new Label();
                    //    lbl_ik.Text = GridView1.Rows[i - 1].Cells[3].Text;
                    //    HtmlTextWriter ahw = new HtmlTextWriter(sw);
                    //    lbl_ik.RenderControl(ahw);
                    //}
                    if (GridView1.Rows[i - 1].Cells[0].Text == "&nbsp;" && GridView1.Rows[i - 1].Cells[1].Text == "&nbsp;")
                    {
                        evenOdd += 1;
                        gridvalue = i + 1;
                        if (evenOdd % 2 != 0)
                        {
                            for (int k1 = 0; k1 < GridView1.Rows[i].Cells.Count; k1++)
                            {
                                bool val = IsNumeric(GridView1.Rows[i].Cells[k1].Text.ToString());
                                if (val == false)
                                {
                                    GridView1.Rows[i].Cells[k1].HorizontalAlign = HorizontalAlign.Left;
                                    GridView1.Rows[i].Cells[k1].Wrap = false;
                                }
                                else
                                {
                                    GridView1.Rows[i].Cells[k1].HorizontalAlign = HorizontalAlign.Right;
                                    GridView1.Rows[i].Cells[k1].Wrap = false;
                                    if (Convert.ToDecimal(GridView1.Rows[i].Cells[k1].Text.ToString()) < 0)
                                        GridView1.Rows[i].Cells[k1].ForeColor = System.Drawing.Color.Red;
                                }
                            }
                            GridView1.Rows[i].Font.Bold = true;
                            GridView1.Rows[i].Font.Size = new FontUnit("10px");
                            GridView1.Rows[i].ForeColor = System.Drawing.Color.Blue;
                        }
                        else
                        {
                            for (int k2 = 0; k2 < GridView1.Rows[i].Cells.Count; k2++)
                            {
                                bool val = IsNumeric(GridView1.Rows[i].Cells[k2].Text.ToString());
                                if (val == false)
                                {
                                    GridView1.Rows[i].Cells[k2].HorizontalAlign = HorizontalAlign.Left;
                                    GridView1.Rows[i].Cells[k2].Wrap = false;
                                }
                                else
                                {
                                    GridView1.Rows[i].Cells[k2].HorizontalAlign = HorizontalAlign.Right;
                                    GridView1.Rows[i].Cells[k2].Wrap = false;
                                    if (Convert.ToDecimal(GridView1.Rows[i].Cells[k2].Text.ToString()) < 0)
                                        GridView1.Rows[i].Cells[k2].ForeColor = System.Drawing.Color.Red;
                                }
                            }
                            GridView1.Rows[i].Font.Bold = false;
                            GridView1.Rows[i].Font.Size = new FontUnit("10px");
                            GridView1.Rows[i].ForeColor = System.Drawing.Color.Blue;
                        }
                    }
                    else if (GridView1.Rows[i - 1].Cells[0].Text == compareString || GridView1.Rows[i - 1].Cells[1].Text == compareString || GridView1.Rows[i - 1].Cells[2].Text == compareString || GridView1.Rows[i - 1].Cells[3].Text == compareString)
                    {
                        for (int k3 = 0; k3 < GridView1.Rows[i - 1].Cells.Count; k3++)
                        {
                            bool val = IsNumeric(GridView1.Rows[i - 1].Cells[k3].Text.ToString());
                            if (val == false)
                            {
                                GridView1.Rows[i - 1].Cells[k3].HorizontalAlign = HorizontalAlign.Left;
                                GridView1.Rows[i - 1].Cells[k3].Wrap = false;
                            }
                            else
                            {
                                GridView1.Rows[i - 1].Cells[k3].HorizontalAlign = HorizontalAlign.Right;
                                GridView1.Rows[i - 1].Cells[k3].Wrap = false;
                                if (Convert.ToDecimal(GridView1.Rows[i - 1].Cells[k3].Text.ToString()) < 0)
                                    GridView1.Rows[i - 1].Cells[k3].ForeColor = System.Drawing.Color.Red;
                            }
                        }
                        GridView1.Rows[i - 1].ForeColor = System.Drawing.Color.Blue;
                        GridView1.Rows[i - 1].Font.Bold = true;
                        GridView1.Rows[i - 1].Font.Size = new FontUnit("10px");
                    }
                    else if (GridView1.Rows[i - 1].Cells[0].Text != "&nbsp;" && GridView1.Rows[i - 1].Cells[1].Text == "Test" && GridView1.Rows[i - 1].Cells[2].Text == "&nbsp;" && GridView1.Rows[i - 1].Cells[3].Text == "&nbsp;")
                    {
                        GridView1.Rows[i - 1].Font.Bold = true;
                        GridView1.Rows[i - 1].Font.Size = new FontUnit("10px");
                        //GridView1.Rows[i - 1].HorizontalAlign = HorizontalAlign.Left;
                        GridView1.Rows[i - 1].ForeColor = System.Drawing.Color.Blue;
                        //GridView1.Rows[i - 1].Cells[0].ColumnSpan = GridView1.Rows[i - 1].Cells.Count;
                        //GridView1.Rows[i - 1].Cells[0].BackColor = System.Drawing.Color.LightYellow;
                        for (int cellNum = GridView1.Rows[i - 1].Cells.Count - 1; cellNum > 0; cellNum--)
                        {
                            GridView1.Rows[i - 1].Cells.RemoveAt(cellNum);
                        }
                    }
                    else
                    {
                        for (int k = 0; k < GridView1.Rows[i - 1].Cells.Count; k++)
                        {
                            bool val = IsNumeric(GridView1.Rows[i - 1].Cells[k].Text.ToString());
                            if (val == false)
                            {
                                GridView1.Rows[i - 1].Cells[k].HorizontalAlign = HorizontalAlign.Left;
                                GridView1.Rows[i - 1].Cells[k].Wrap = false;
                            }
                            else
                            {
                                GridView1.Rows[i - 1].Cells[k].HorizontalAlign = HorizontalAlign.Right;
                                GridView1.Rows[i - 1].Cells[k].Wrap = false;
                                if (Convert.ToDecimal(GridView1.Rows[i - 1].Cells[k].Text.ToString()) < 0)
                                    GridView1.Rows[i - 1].Cells[k].ForeColor = System.Drawing.Color.Red;

                            }
                        }
                        GridView1.Rows[i - 1].Font.Bold = false;
                        GridView1.Rows[i - 1].Font.Size = new FontUnit("10px");

                    }
                    colNo = GridView1.Rows[i - 1].Cells.Count;
                }

            }
            GridView1.RenderControl(hw);

            HtmlTextWriter hw2 = new HtmlTextWriter(sw);
            GridView GridView3 = new GridView();
            GridView3.DataSource = dtFooter;
            GridView3.DataBind();
            GridView3.HeaderRow.Visible = false;
            for (int j1 = 0; j1 < GridView3.Rows.Count; j1++)
            {
                GridView3.Rows[j1].Font.Bold = true;
                GridView3.Rows[j1].Font.Size = new FontUnit("14px");
                //GridView3.Rows[j1].HorizontalAlign = HorizontalAlign.Center;
                GridView3.Rows[j1].ForeColor = System.Drawing.Color.Blue;
                //GridView3.Rows[j1].Cells[0].ColumnSpan = colNo;
                //GridView3.Rows[j1].Cells[0].BackColor = System.Drawing.Color.LightYellow;
                if (GridView3.Rows[j1].Cells[0].Text == "&nbsp;")
                {
                    GridView3.Rows[j1].Visible = false;
                }
            }
            GridView3.RenderControl(hw2);

            //style to format numbers to string 
            //string style = @"<style> .textmode { mso-number-format:\#\,\#\#0\.00_ \;\[Red\]\-\#\,\#\#0\.00\ ; } </style>";
            //HttpContext.Current.Response.Write(style);
            HttpContext.Current.Response.Output.Write(sw.ToString());
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        public void ExportToPDFPageBreak(DataTable datatable, string fname, string compareString, DataTable dtHeader, DataTable dtFooter)
        {
            Document pdfReport = new Document(PageSize.A4, 25, 25, 40, 25);

            System.IO.MemoryStream msReport = new System.IO.MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(pdfReport, msReport);
            pdfReport.Open();
            PdfPTable ptHeader = new PdfPTable(dtHeader.Columns.Count);
            for (int intH = 1; intH <= dtHeader.Rows.Count; intH++)
            {
                PdfPCell cell = new PdfPCell();
                cell.BorderWidth = 0.001f;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Colspan = dtHeader.Columns.Count;
                cell.BackgroundColor = CMYKColor.LIGHT_GRAY;
                cell.Phrase = new Phrase(dtHeader.Rows[intH - 1][0].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 8, Font.BOLD, CMYKColor.BLUE));
                ptHeader.AddCell(cell);
            }
            // pdfReport.Add(ptHeader);
            //pdfReport.Close();




            PdfPTable ptData = new PdfPTable(datatable.Columns.Count);

            float[] headerwidths = new float[datatable.Columns.Count]; // percentage
            for (int intK = 0; intK < datatable.Columns.Count; intK++)
            {
                if (datatable.Columns[intK].ColumnName.ToString().ToUpper().Contains("DESC"))
                    headerwidths[intK] = (100 / (datatable.Columns.Count + 2)) * 3;
                else
                    headerwidths[intK] = 100 / (datatable.Columns.Count + 2);
            }
            ptData.SetWidths(headerwidths);
            ptData.WidthPercentage = 100;
            ptData.DefaultCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            ptData.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;

            //Insert the Table Headers
            for (int intK = 0; intK < datatable.Columns.Count; intK++)
            {
                PdfPCell cell = new PdfPCell();
                cell.BorderWidth = 0.001f;
                cell.Phrase = new Phrase(datatable.Columns[intK].ColumnName.ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 7, Font.BOLD, CMYKColor.BLUE));
                ptData.AddCell(cell);
            }

            ptData.HeaderRows = 1;  // this is the end of the table header


            // this is the end of the table header



            //------------------------------------ADD FOOTER--------------------------
            PdfPTable ptFooter = new PdfPTable(dtFooter.Columns.Count);
            for (int intF = 1; intF <= dtFooter.Rows.Count; intF++)
            {
                if (dtFooter.Rows[intF - 1][0] == DBNull.Value)
                {
                    PdfPCell cell = new PdfPCell();
                    cell.BorderWidth = 0.001f;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Colspan = dtFooter.Columns.Count;
                    cell.BackgroundColor = CMYKColor.WHITE;
                    cell.Phrase = new Phrase(dtFooter.Rows[intF - 1][0].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 8, Font.BOLD, CMYKColor.BLUE));
                    ptFooter.AddCell(cell);
                }
                else
                {
                    PdfPCell cell = new PdfPCell();
                    cell.BorderWidth = 0.001f;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Colspan = dtFooter.Columns.Count;
                    cell.BackgroundColor = CMYKColor.LIGHT_GRAY;
                    cell.Phrase = new Phrase(dtFooter.Rows[intF - 1][0].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 8, Font.BOLD, CMYKColor.BLUE));
                    ptFooter.AddCell(cell);
                }

            }
            // pdfReport.Add(ptFooter);

            //-----------------------------


            //Insert the Table Data
            int gridvalue = 0;
            int evenOdd = 0;
            for (int intJ = 1; intJ <= datatable.Rows.Count; intJ++)
            {

                if (gridvalue != intJ)
                {
                    if (datatable.Rows[intJ - 1][0] == DBNull.Value && datatable.Rows[intJ - 1][1] == DBNull.Value)
                    {
                        evenOdd += 1;
                        gridvalue = intJ + 1;
                        if (evenOdd % 2 != 0)
                        {
                            for (int intK = 0; intK < datatable.Columns.Count; intK++)
                            {
                                bool val = IsNumeric(datatable.Rows[intJ][intK].ToString());
                                if (val == false)
                                {
                                    PdfPCell cell = new PdfPCell();
                                    cell.BorderWidth = 0.001f;
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Phrase = new Phrase(datatable.Rows[intJ][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.BLUE));
                                    ptData.AddCell(cell);
                                }
                                else
                                {
                                    PdfPCell cell = new PdfPCell();
                                    cell.BorderWidth = 0.001f;
                                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    if (Convert.ToDecimal(datatable.Rows[intJ][intK].ToString()) < 0)
                                        cell.Phrase = new Phrase(datatable.Rows[intJ][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.RED));
                                    else
                                        cell.Phrase = new Phrase(datatable.Rows[intJ][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.BLUE));

                                    ptData.AddCell(cell);
                                }

                            }
                        }
                        else
                        {
                            for (int intK = 0; intK < datatable.Columns.Count; intK++)
                            {
                                bool val = IsNumeric(datatable.Rows[intJ][intK].ToString());
                                if (val == false)
                                {
                                    PdfPCell cell = new PdfPCell();
                                    cell.BorderWidth = 0.001f;
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Phrase = new Phrase(datatable.Rows[intJ][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.BLUE));
                                    ptData.AddCell(cell);
                                }
                                else
                                {
                                    PdfPCell cell = new PdfPCell();
                                    cell.BorderWidth = 0.001f;
                                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    if (Convert.ToDecimal(datatable.Rows[intJ][intK].ToString()) < 0)
                                        cell.Phrase = new Phrase(datatable.Rows[intJ][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.RED));
                                    else
                                        cell.Phrase = new Phrase(datatable.Rows[intJ][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.BLUE));
                                    ptData.AddCell(cell);
                                }

                            }
                        }
                    }
                    else if (datatable.Rows[intJ - 1][0].ToString() == compareString || datatable.Rows[intJ - 1][1].ToString() == compareString || datatable.Rows[intJ - 1][2].ToString() == compareString || datatable.Rows[intJ - 1][3].ToString() == compareString)
                    {
                        for (int intK = 0; intK < datatable.Columns.Count; intK++)
                        {
                            bool val = IsNumeric(datatable.Rows[intJ - 1][intK].ToString());
                            if (val == false)
                            {
                                PdfPCell cell = new PdfPCell();
                                cell.BorderWidth = 0.001f;
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Phrase = new Phrase(datatable.Rows[intJ - 1][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.BLUE));
                                ptData.AddCell(cell);
                            }
                            else
                            {
                                PdfPCell cell = new PdfPCell();
                                cell.BorderWidth = 0.001f;
                                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                if (Convert.ToDecimal(datatable.Rows[intJ - 1][intK].ToString()) < 0)
                                    cell.Phrase = new Phrase(datatable.Rows[intJ - 1][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.RED));
                                else
                                    cell.Phrase = new Phrase(datatable.Rows[intJ - 1][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.BLUE));

                                ptData.AddCell(cell);
                            }

                        }
                    }
                    else if (datatable.Rows[intJ - 1][0] != DBNull.Value && datatable.Rows[intJ - 1][1].ToString() == "Test" && datatable.Rows[intJ - 1][2] == DBNull.Value && datatable.Rows[intJ - 1][3] == DBNull.Value)
                    {
                        PdfPCell cell = new PdfPCell();
                        cell.BorderWidth = 0.001f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Colspan = datatable.Columns.Count;
                        cell.BackgroundColor = CMYKColor.LIGHT_GRAY;
                        cell.Phrase = new Phrase(datatable.Rows[intJ - 1][0].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.BOLD, CMYKColor.BLUE));
                        ptData.AddCell(cell);
                    }
                    //--------------------For page Break
                    else if (datatable.Rows[intJ - 1][0] != DBNull.Value && datatable.Rows[intJ - 1][1].ToString() == "Break" && datatable.Rows[intJ - 1][2] == DBNull.Value && datatable.Rows[intJ - 1][3] == DBNull.Value)
                    {
                        PdfPCell cell = new PdfPCell();
                        cell.BorderWidth = 0.001f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Colspan = datatable.Columns.Count;
                        cell.BackgroundColor = CMYKColor.LIGHT_GRAY;
                        cell.Phrase = new Phrase(datatable.Rows[intJ - 1][0].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.BOLD, CMYKColor.BLUE));
                        ptData.AddCell(cell);


                    }
                    else
                    {
                        for (int intK = 0; intK < datatable.Columns.Count; intK++)
                        {
                            bool val = IsNumeric(datatable.Rows[intJ - 1][intK].ToString());
                            if (val == false)
                            {
                                PdfPCell cell = new PdfPCell();
                                cell.BorderWidth = 0.001f;
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Phrase = new Phrase(datatable.Rows[intJ - 1][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6));
                                ptData.AddCell(cell);
                            }
                            else
                            {
                                PdfPCell cell = new PdfPCell();
                                cell.BorderWidth = 0.001f;
                                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                if (Convert.ToDecimal(datatable.Rows[intJ - 1][intK].ToString()) < 0)
                                    cell.Phrase = new Phrase(datatable.Rows[intJ - 1][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.RED));
                                else
                                    cell.Phrase = new Phrase(datatable.Rows[intJ - 1][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6));
                                ptData.AddCell(cell);
                            }

                        }
                    }
                }



                if (datatable.Rows[intJ - 1][0].ToString() == "" && datatable.Rows[intJ - 1][1].ToString() == "Break" && datatable.Rows[intJ - 1][2] == DBNull.Value && datatable.Rows[intJ - 1][3] == DBNull.Value)
                {
                    pdfReport.Add(ptHeader);

                    PdfPTable frstData = new PdfPTable(datatable.Columns.Count);
                    pdfReport.Add(ptData);
                    pdfReport.Add(ptFooter);
                    pdfReport.NewPage();
                    ptData.DeleteBodyRows();
                }

            }

            //Insert the Table

            //pdfReport.Add(ptData);

            //Closes the Report and writes to Memory Stream
            //PdfPTable ptFooter = new PdfPTable(dtFooter.Columns.Count);
            //for (int intF = 1; intF <= dtFooter.Rows.Count; intF++)
            //{
            //    if (dtFooter.Rows[intF - 1][0] == DBNull.Value)
            //    {
            //        PdfPCell cell = new PdfPCell();
            //        cell.BorderWidth = 0.001f;
            //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //        cell.Colspan = dtFooter.Columns.Count;
            //        cell.BackgroundColor = CMYKColor.WHITE;
            //        cell.Phrase = new Phrase(dtFooter.Rows[intF - 1][0].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 8, Font.BOLD, CMYKColor.BLUE));
            //        ptFooter.AddCell(cell);
            //    }
            //    else
            //    {
            //        PdfPCell cell = new PdfPCell();
            //        cell.BorderWidth = 0.001f;
            //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //        cell.Colspan = dtFooter.Columns.Count;
            //        cell.BackgroundColor = CMYKColor.LIGHT_GRAY;
            //        cell.Phrase = new Phrase(dtFooter.Rows[intF - 1][0].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 8, Font.BOLD, CMYKColor.BLUE));
            //        ptFooter.AddCell(cell);
            //    }

            //}
            //pdfReport.Add(ptFooter);
            pdfReport.Close();

            //Writes the Memory Stream Data to Response Object

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AppendHeader("content-disposition", "attachment;filename=" + fname + ".pdf");
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.BinaryWrite(msReport.ToArray());
            HttpContext.Current.Response.End();

        }
        public void ExportToExcelforExcel2(DataTable datatable, string fname, string compareString, DataTable dtHeader, DataTable dtFooter)
        {
            //Create a dummy GridView 
            int colNo = 0;
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;
            GridView1.DataSource = datatable;
            GridView1.DataBind();
            GridView1.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            GridView1.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#E8E6E7");
            GridView1.HeaderStyle.Font.Bold = true;
            GridView1.HeaderStyle.Font.Size = new FontUnit("15px");
            GridView1.HeaderStyle.Wrap = false;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + fname + ".xls");
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            HtmlTextWriter hw1 = new HtmlTextWriter(sw);
            GridView GridView2 = new GridView();
            GridView2.DataSource = dtHeader;
            GridView2.DataBind();
            GridView2.HeaderRow.Visible = false;
            for (int j = 0; j < GridView2.Rows.Count; j++)
            {
                GridView2.Rows[j].Font.Bold = true;
                GridView2.Rows[j].Font.Size = new FontUnit("14px");
                //GridView2.Rows[j].HorizontalAlign = HorizontalAlign.Center;
                GridView2.Rows[j].ForeColor = System.Drawing.Color.Blue;
                //GridView2.Rows[j].Cells[0].ColumnSpan = GridView1.Rows[0].Cells.Count;
                //GridView2.Rows[j].Cells[0].BackColor = System.Drawing.Color.LightYellow;
                GridView2.Rows[j].Cells[0].Wrap = false;
                if (GridView2.Rows[j].Cells[0].Text == "&nbsp;")
                {
                    GridView2.Rows[j].Visible = false;
                }
            }
            GridView2.RenderControl(hw1);

            int gridvalue = 0;
            int evenOdd = 0;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                //DataView dv = datatable.DefaultView;
                //dv.RowFilter = "FirstField LIKE 'whatever%'"

                //Apply text style to each Row 
                GridView1.Rows[i].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                GridView1.Rows[i].Cells[0].Wrap = false;
                GridView1.Rows[i].Font.Bold = true;
                GridView1.Rows[i].Font.Size = new FontUnit("10px");
                GridView1.Rows[i].ForeColor = System.Drawing.Color.Blue;

            }
            GridView1.RenderControl(hw);

            HtmlTextWriter hw2 = new HtmlTextWriter(sw);
            GridView GridView3 = new GridView();
            GridView3.DataSource = dtFooter;
            GridView3.DataBind();
            GridView3.HeaderRow.Visible = false;
            for (int j1 = 0; j1 < GridView3.Rows.Count; j1++)
            {
                GridView3.Rows[j1].Font.Bold = true;
                GridView3.Rows[j1].Font.Size = new FontUnit("14px");
                //GridView3.Rows[j1].HorizontalAlign = HorizontalAlign.Center;
                GridView3.Rows[j1].ForeColor = System.Drawing.Color.Blue;
                //GridView3.Rows[j1].Cells[0].ColumnSpan = colNo;
                //GridView3.Rows[j1].Cells[0].BackColor = System.Drawing.Color.LightYellow;
                if (GridView3.Rows[j1].Cells[0].Text == "&nbsp;")
                {
                    GridView3.Rows[j1].Visible = false;
                }
            }
            GridView3.RenderControl(hw2);

            //style to format numbers to string 
            //string style = @"<style> .textmode { mso-number-format:\#\,\#\#0\.00_ \;\[Red\]\-\#\,\#\#0\.00\ ; } </style>";
            //HttpContext.Current.Response.Write(style);
            HttpContext.Current.Response.Output.Write(sw.ToString());
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        public void ExportToExcelforExcel3(DataTable datatable, string fname, string compareString, DataTable dtHeader, DataTable dtFooter)
        {
            //Create a dummy GridView 
            int colNo = 0;
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;
            StringWriter sw = new StringWriter();
            DataTable dtTable = new DataTable();
            dtTable = datatable.Clone();
            DataRow newRow = dtTable.NewRow();
            for (int ij = 0; ij < datatable.Rows.Count; ij++)
            {
                newRow = dtTable.NewRow();
                newRow[0] = datatable.Rows[ij][0];
                //newRow[1] = datatable.Rows[ij][1];
                //newRow[2] = datatable.Rows[ij][2];
                //newRow[3] = datatable.Rows[ij][3];
                //newRow[4] = datatable.Rows[ij][4];
                //newRow[5] = datatable.Rows[ij][5];
                //newRow[6] = datatable.Rows[ij][6];
                //newRow[7] = datatable.Rows[ij][7];
                //newRow[8] = datatable.Rows[ij][8];
                if (datatable.Rows[ij][0].ToString() == "a")
                {
                    //Label lbl_ik = new Label();
                    //lbl_ik.Text = datatable.Rows[ij][3].ToString();
                    //HtmlTextWriter ahw = new HtmlTextWriter(sw);
                    //lbl_ik.RenderControl(ahw);
                    //Table tbid = (Table)datatable.Rows[ij][3];
                    HtmlTable tb = (HtmlTable)datatable.Rows[ij][3]; //table1 is the id of HTML Table                
                    int rowscount = tb.Rows.Count;

                    //for (int i = 0; i < rowscount; i++)
                    //{
                    //    HtmlTableCellCollection tcs = tb.Rows[i].Cells;

                    //    DataRow dr = dt.NewRow();
                    //    dr["a1"] = tcs[0].InnerText;
                    //    dr["a2"] = tcs[1].InnerText;
                    //    dt.Rows.Add(dr);
                    //}


                }
                dtTable.Rows.Add(newRow);
            }
            GridView1.DataSource = dtTable;
            GridView1.DataBind();
            GridView1.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            GridView1.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#E8E6E7");
            GridView1.HeaderStyle.Font.Bold = true;
            GridView1.HeaderStyle.Font.Size = new FontUnit("15px");
            GridView1.HeaderStyle.Wrap = false;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + fname + ".xls");
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";

            HtmlTextWriter hw = new HtmlTextWriter(sw);

            HtmlTextWriter hw1 = new HtmlTextWriter(sw);
            GridView GridView2 = new GridView();
            GridView2.DataSource = dtHeader;
            GridView2.DataBind();
            GridView2.HeaderRow.Visible = false;
            for (int j = 0; j < GridView2.Rows.Count; j++)
            {
                GridView2.Rows[j].Font.Bold = true;
                GridView2.Rows[j].Font.Size = new FontUnit("14px");
                //GridView2.Rows[j].HorizontalAlign = HorizontalAlign.Center;
                GridView2.Rows[j].ForeColor = System.Drawing.Color.Blue;
                //GridView2.Rows[j].Cells[0].ColumnSpan = GridView1.Rows[0].Cells.Count;
                //GridView2.Rows[j].Cells[0].BackColor = System.Drawing.Color.LightYellow;
                GridView2.Rows[j].Cells[0].Wrap = false;
                if (GridView2.Rows[j].Cells[0].Text == "&nbsp;")
                {
                    GridView2.Rows[j].Visible = false;
                }
            }
            GridView2.RenderControl(hw1);

            int gridvalue = 0;
            int evenOdd = 0;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                //DataView dv = datatable.DefaultView;
                //dv.RowFilter = "FirstField LIKE 'whatever%'"

                //Apply text style to each Row 
                if (gridvalue != i)
                {
                    //if (GridView1.Rows[i - 1].Cells[0].Text == "a")
                    //{
                    //    Label lbl_ik = new Label();
                    //    lbl_ik.Text = GridView1.Rows[i - 1].Cells[3].Text;
                    //    HtmlTextWriter ahw = new HtmlTextWriter(sw);
                    //    lbl_ik.RenderControl(ahw);
                    //}
                    if (GridView1.Rows[i - 1].Cells[0].Text == "&nbsp;" && GridView1.Rows[i - 1].Cells[0].Text == "&nbsp;")
                    {
                        evenOdd += 1;
                        gridvalue = i + 1;
                        if (evenOdd % 2 != 0)
                        {
                            for (int k1 = 0; k1 < GridView1.Rows[i].Cells.Count; k1++)
                            {
                                bool val = IsNumeric(GridView1.Rows[i].Cells[k1].Text.ToString());
                                if (val == false)
                                {
                                    GridView1.Rows[i].Cells[k1].HorizontalAlign = HorizontalAlign.Left;
                                    GridView1.Rows[i].Cells[k1].Wrap = false;
                                }
                                else
                                {
                                    GridView1.Rows[i].Cells[k1].HorizontalAlign = HorizontalAlign.Right;
                                    GridView1.Rows[i].Cells[k1].Wrap = false;
                                    if (Convert.ToDecimal(GridView1.Rows[i].Cells[k1].Text.ToString()) < 0)
                                        GridView1.Rows[i].Cells[k1].ForeColor = System.Drawing.Color.Red;
                                }
                            }
                            GridView1.Rows[i].Font.Bold = true;
                            GridView1.Rows[i].Font.Size = new FontUnit("10px");
                            GridView1.Rows[i].ForeColor = System.Drawing.Color.Blue;
                        }
                        else
                        {
                            for (int k2 = 0; k2 < GridView1.Rows[i].Cells.Count; k2++)
                            {
                                bool val = IsNumeric(GridView1.Rows[i].Cells[k2].Text.ToString());
                                if (val == false)
                                {
                                    GridView1.Rows[i].Cells[k2].HorizontalAlign = HorizontalAlign.Left;
                                    GridView1.Rows[i].Cells[k2].Wrap = false;
                                }
                                else
                                {
                                    GridView1.Rows[i].Cells[k2].HorizontalAlign = HorizontalAlign.Right;
                                    GridView1.Rows[i].Cells[k2].Wrap = false;
                                    if (Convert.ToDecimal(GridView1.Rows[i].Cells[k2].Text.ToString()) < 0)
                                        GridView1.Rows[i].Cells[k2].ForeColor = System.Drawing.Color.Red;
                                }
                            }
                            GridView1.Rows[i].Font.Bold = false;
                            GridView1.Rows[i].Font.Size = new FontUnit("10px");
                            GridView1.Rows[i].ForeColor = System.Drawing.Color.Blue;
                        }
                    }
                    //else if (GridView1.Rows[i - 1].Cells[0].Text == compareString || GridView1.Rows[i - 1].Cells[1].Text == compareString || GridView1.Rows[i - 1].Cells[2].Text == compareString || GridView1.Rows[i - 1].Cells[3].Text == compareString)
                    //{
                    //    for (int k3 = 0; k3 < GridView1.Rows[i - 1].Cells.Count; k3++)
                    //    {
                    //        bool val = IsNumeric(GridView1.Rows[i - 1].Cells[k3].Text.ToString());
                    //        if (val == false)
                    //        {
                    //            GridView1.Rows[i - 1].Cells[k3].HorizontalAlign = HorizontalAlign.Left;
                    //            GridView1.Rows[i - 1].Cells[k3].Wrap = false;
                    //        }
                    //        else
                    //        {
                    //            GridView1.Rows[i - 1].Cells[k3].HorizontalAlign = HorizontalAlign.Right;
                    //            GridView1.Rows[i - 1].Cells[k3].Wrap = false;
                    //            if (Convert.ToDecimal(GridView1.Rows[i - 1].Cells[k3].Text.ToString()) < 0)
                    //                GridView1.Rows[i - 1].Cells[k3].ForeColor = System.Drawing.Color.Red;
                    //        }
                    //    }
                    //    GridView1.Rows[i - 1].ForeColor = System.Drawing.Color.Blue;
                    //    GridView1.Rows[i - 1].Font.Bold = true;
                    //    GridView1.Rows[i - 1].Font.Size = new FontUnit("10px");
                    //}
                    //else if (GridView1.Rows[i - 1].Cells[0].Text != "&nbsp;" && GridView1.Rows[i - 1].Cells[1].Text == "Test" && GridView1.Rows[i - 1].Cells[2].Text == "&nbsp;" && GridView1.Rows[i - 1].Cells[3].Text == "&nbsp;")
                    //{
                    //    GridView1.Rows[i - 1].Font.Bold = true;
                    //    GridView1.Rows[i - 1].Font.Size = new FontUnit("10px");
                    //    GridView1.Rows[i - 1].HorizontalAlign = HorizontalAlign.Left;
                    //    GridView1.Rows[i - 1].ForeColor = System.Drawing.Color.Blue;
                    //    GridView1.Rows[i - 1].Cells[0].ColumnSpan = GridView1.Rows[i - 1].Cells.Count;
                    //    GridView1.Rows[i - 1].Cells[0].BackColor = System.Drawing.Color.LightYellow;
                    //    for (int cellNum = GridView1.Rows[i - 1].Cells.Count - 1; cellNum > 0; cellNum--)
                    //    {
                    //        GridView1.Rows[i - 1].Cells.RemoveAt(cellNum);
                    //    }
                    //}
                    //else
                    //{
                    //    for (int k = 0; k < GridView1.Rows[i - 1].Cells.Count; k++)
                    //    {
                    //        bool val = IsNumeric(GridView1.Rows[i - 1].Cells[k].Text.ToString());
                    //        if (val == false)
                    //        {
                    //            GridView1.Rows[i - 1].Cells[k].HorizontalAlign = HorizontalAlign.Left;
                    //            GridView1.Rows[i - 1].Cells[k].Wrap = false;
                    //        }
                    //        else
                    //        {
                    //            GridView1.Rows[i - 1].Cells[k].HorizontalAlign = HorizontalAlign.Right;
                    //            GridView1.Rows[i - 1].Cells[k].Wrap = false;
                    //            if (Convert.ToDecimal(GridView1.Rows[i - 1].Cells[k].Text.ToString()) < 0)
                    //                GridView1.Rows[i - 1].Cells[k].ForeColor = System.Drawing.Color.Red;

                    //        }
                    //    }
                    //    GridView1.Rows[i - 1].Font.Bold = false;
                    //    GridView1.Rows[i - 1].Font.Size = new FontUnit("10px");

                    //}
                    colNo = GridView1.Rows[i].Cells.Count;
                }
                else
                {
                    colNo = GridView1.Rows[i].Cells.Count;
                }
            }
            GridView1.RenderControl(hw);

            HtmlTextWriter hw2 = new HtmlTextWriter(sw);
            GridView GridView3 = new GridView();
            GridView3.DataSource = dtFooter;
            GridView3.DataBind();
            GridView3.HeaderRow.Visible = false;
            for (int j1 = 0; j1 < GridView3.Rows.Count; j1++)
            {
                GridView3.Rows[j1].Font.Bold = true;
                GridView3.Rows[j1].Font.Size = new FontUnit("14px");
                //GridView3.Rows[j1].HorizontalAlign = HorizontalAlign.Center;
                GridView3.Rows[j1].ForeColor = System.Drawing.Color.Blue;
                //GridView3.Rows[j1].Cells[0].ColumnSpan = colNo;
                //GridView3.Rows[j1].Cells[0].BackColor = System.Drawing.Color.LightYellow;
                if (GridView3.Rows[j1].Cells[0].Text == "&nbsp;")
                {
                    GridView3.Rows[j1].Visible = false;
                }
            }
            GridView3.RenderControl(hw2);

            //style to format numbers to string 
            //string style = @"<style> .textmode { mso-number-format:\#\,\#\#0\.00_ \;\[Red\]\-\#\,\#\#0\.00\ ; } </style>";
            //HttpContext.Current.Response.Write(style);
            HttpContext.Current.Response.Output.Write(sw.ToString());
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
    }
}