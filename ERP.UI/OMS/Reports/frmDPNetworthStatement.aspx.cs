using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class management_frmDPNetworthStatement : System.Web.UI.Page, ICallbackEventHandler
    {
        string data;
        static string User;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        ExcelFile objExcel = new ExcelFile();

        DailyReports dailyreport = new DailyReports();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                chkTradingCode.Checked = true;
                chkPhoneNo.Checked = true;
                txtRecordNo.Text = "99999";
                //dtFrom.Date = oDBEngine.GetDate();
                dtFrom.Date = oDBEngine.GetDate();
                txtNetworthPercentage.Text = "5";
                rbAll.Checked = true;
                Page.ClientScript.RegisterStartupScript(GetType(), "hidaccounts", "<script>PageLoad();</script>");
                ViewState["AllData"] = null;
                //ddlExport.Visible = false;
                // chkConsiderAccounts.Checked = true;
            }

            if (rbAll.Checked == true)
                Page.ClientScript.RegisterStartupScript(GetType(), "hidaccounttype", "<script>HideAccounts();</script>");
            //else
            //    Page.ClientScript.RegisterStartupScript(GetType(), "hidaccounttype", "<script>ShowAccounts();</script>");

            String cbReference1 = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveSvrData", "context");
            String callbackScript1 = "function CallServer1(arg, context){ " + cbReference1 + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer1", callbackScript1, true);
            //BindGrid();
            if (IsCallback)
            {
                BindGrid();

            }
            Page.ClientScript.RegisterStartupScript(GetType(), "pageheight", "<script>height();</script>");
        }


        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string[] cl = idlist[1].Split(',');
            string str = "";
            string str1 = "";
            for (int i = 0; i < cl.Length; i++)
            {

                string[] val = cl[i].Split(';');
                if (str == "")
                {
                    str = "'" + val[0] + "'";

                }
                else
                {
                    str += ",'" + val[0] + "'";

                }

            }


            data = "Clients~" + str;


        }

        protected void BindGrid()
        {
            DataSet objDs = new DataSet();
            using (SqlConnection objCon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]))
            {
                //using (SqlCommand objCmd = new SqlCommand("Fetch_DPNetworthStatement", objCon))
                //{
                string NoOfRows;
                if (txtRecordNo.Text != "")
                    NoOfRows = txtRecordNo.Text;
                else
                    NoOfRows = "99999";
                //objCmd.CommandType = CommandType.StoredProcedure;
                //objCmd.Parameters.AddWithValue("@NoOfRow", NoOfRows);
                //if (rbSpecific.Checked == true)
                //{
                //    if (hidClients.Value != "")
                //        objCmd.Parameters.AddWithValue("@AccountTypes", hidClients.Value);
                //}
                //objCmd.Parameters.AddWithValue("@SelDate", dtFrom.Text.Split('-')[2] + '-' + dtFrom.Text.Split('-')[1] + '-' + dtFrom.Text.Split('-')[0]);
                //if (chkConsiderAccounts.Checked == true)
                //{
                //    objCmd.Parameters.AddWithValue("@ConsiderPercent", "Y");
                //    string PercentValue;
                //    if (txtNetworthPercentage.Text != "")
                //        PercentValue = txtNetworthPercentage.Text;
                //    else
                //        PercentValue = "5";
                //    objCmd.Parameters.AddWithValue("@PercentValue", PercentValue);

                //}

                //objCmd.Parameters.AddWithValue("@Segment", Convert.ToString(Request.QueryString["type"]).ToUpper());
                //SqlDataAdapter objDa = new SqlDataAdapter(objCmd);
                //objDa.Fill(objDs);
                objDs = dailyreport.Fetch_DPNetworthStatement(Convert.ToInt32(NoOfRows), rbSpecific.Checked, hidClients.Value.ToString(),
                    dtFrom.Text.Split('-')[2] + '-' + dtFrom.Text.Split('-')[1] + '-' + dtFrom.Text.Split('-')[0], chkConsiderAccounts.Checked,
                    txtNetworthPercentage.Text, Convert.ToString(Request.QueryString["type"]).ToUpper());

                gridStatement.DataSource = objDs;
                gridStatement.DataBind();
                if (objDs.Tables[0].Rows.Count > 0)
                {
                    // ddlExport.Visible = true;
                    lblDate.Text = dtFrom.Date.ToString("dd MMM yyyy");
                    Page.ClientScript.RegisterStartupScript(GetType(), "showexpfilt", "<script>ShowObj('btnfilter');</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "hidfilter", "<script>HidFilter();</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "showhead", "<script>ShowObj('trheadline');</script>");
                }
                else
                {
                    // ddlExport.Visible = false;
                    Page.ClientScript.RegisterStartupScript(GetType(), "hidexpfilt", "<script>HidObj('btnfilter');</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "showhead", "<script>HidObj('trheadline');</script>");
                }
                ViewState["AllData"] = objDs.Tables[0];
                if (chkTradingCode.Checked == false)
                    gridStatement.Columns[2].Visible = false;
                else
                    gridStatement.Columns[2].Visible = true;


                if (chkPhoneNo.Checked == false)
                    gridStatement.Columns[4].Visible = false;
                else
                    gridStatement.Columns[4].Visible = true;

                //  }

            }


        }

        protected void btnShow_Click(Object sender, EventArgs e)
        {
            BindGrid();



        }
        //protected void gridStatement_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        //{


        //}
        //protected void gridStatement_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        //{

        //}
        protected void ddlExport_SelectedIndexChanged1(object sender, EventArgs e)
        {
            export();
        }

        void export()
        {
            DataTable dtEx = new DataTable();
            DataTable dtTemp = new DataTable();
            dtEx.Columns.Add("Name");
            dtEx.Columns.Add("AccountNo");
            dtEx.Columns.Add("TradingUcc");
            dtEx.Columns.Add("AccountType");
            dtEx.Columns.Add("PhoneNo");
            dtEx.Columns.Add("CurrentBalance");
            dtEx.Columns.Add("Value");

            System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("hi-IN").NumberFormat;
            currencyFormat.CurrencySymbol = "";
            currencyFormat.CurrencyNegativePattern = 2;
            currencyFormat.CurrencyDecimalDigits = 3;



            dtTemp = (DataTable)ViewState["AllData"];
            for (int i = 0; i <= dtTemp.Rows.Count - 1; i++)
            {
                DataRow dr = dtEx.NewRow();
                dr["Name"] = Convert.ToString(dtTemp.Rows[i][0]);
                dr["AccountNo"] = Convert.ToString(dtTemp.Rows[i][1]);
                dr["TradingUcc"] = Convert.ToString(dtTemp.Rows[i][2]);
                dr["AccountType"] = Convert.ToString(dtTemp.Rows[i][3]);
                dr["PhoneNo"] = Convert.ToString(dtTemp.Rows[i][4]);
                dr["CurrentBalance"] = Convert.ToString(dtTemp.Rows[i][5]);
                //dr["CurrentBalance"] = String.Format("{0:N}", Convert.ToDecimal(dtTemp.Rows[i][5]));
                //dr["CurrentBalance"] = Convert.ToDecimal(dtTemp.Rows[i][5]).ToString("c", currencyFormat);
                // dr["CurrentBalance"] =String.Format("{0:0,0.00}", Convert.ToDecimal(dtTemp.Rows[i][5]));
                // dr["CurrentBalance"] = String.Format("{0:0.00}", Convert.ToDecimal(dtTemp.Rows[i][5]));
                // dr["CurrentBalance"] = String.Format("{0:c}", Convert.ToDecimal(dtTemp.Rows[i][5]));
                // dr["Value"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtTemp.Rows[i][6]));
                dr["Value"] = Convert.ToString(dtTemp.Rows[i][6]);
                dtEx.Rows.Add(dr);

            }

            ////for (int i = 0; i < dtEmail.Rows.Count; i++)
            ////{
            ////    dtMail.Clear();
            ////    CurrentPage = i;
            ////    bindGrid();
            ////    DataRow row3 = dtEx.NewRow();
            ////    row3[0] = "Beneficiary AccountID: " + dtEmail.Rows[i][0].ToString() + "   ||  " + "Beneficiary Name:  " + dtEmail.Rows[i][1].ToString() + "  ||  " + "Second Holder:  " + dtEmail.Rows[i][2].ToString() + "  ||  " + "Third Holder:  " + dtEmail.Rows[i][3].ToString();
            ////    row3[1] = "Test";
            ////    dtEx.Rows.Add(row3);
            ////    ViewState["ISINVAlue"] = null;
            ////    VALUE_Sum = 0;
            ////    for (int k = 0; k < dtMail.Rows.Count; k++)
            ////    {
            ////        if (dtMail.Rows[k]["ISINVAlue"].ToString().Trim() != "" || dtMail.Rows[k]["ISINVAlue"].ToString().Trim().Length > 0)
            ////        {
            ////            VALUE_Sum += Convert.ToDecimal(dtMail.Rows[k]["ISINVAlue"].ToString());
            ////            ViewState["ISINVAlue"] = VALUE_Sum;
            ////        }
            ////    }
            ////    int colCount = dtMail.Columns.Count;
            ////    foreach (DataRow dr1 in dtMail.Rows)
            ////    {
            ////        DataRow row2 = dtEx.NewRow();
            ////        for (int j = 0; j < colCount; j++)
            ////        {
            ////            row2[j] = dr1[j];
            ////        }
            ////        dtEx.Rows.Add(row2);
            ////    }

            ////    if (ViewState["ISINVAlue"] != DBNull.Value)
            ////    {
            ////        if (ViewState["ISINVAlue"] != null)
            ////        {
            ////            DataRow row9 = dtEx.NewRow();
            ////            row9[0] = "Total Value of Holding:  " + objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["ISINVAlue"].ToString()));
            ////            row9[1] = "Test";
            ////            dtEx.Rows.Add(row9);
            ////            if (ddlExport.SelectedItem.Value == "PM")
            ////            {
            ////                DataRow row10 = dtEx.NewRow();
            ////                row10[0] = "";
            ////                row10[1] = "Break";
            ////                dtEx.Rows.Add(row10);
            ////            }
            ////        }
            ////    }


            ////}
            //dt1.Clear();
            //// CurrentPage = 0;
            //// bindGrid();

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            DataTable dtReportHeader = new DataTable();
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString() + "[" + HttpContext.Current.Session["usersegid"] + "]";
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = "DPNetworth Statement For " + oconverter.ArrangeDate2(dtFrom.Value.ToString());
            dtReportHeader.Rows.Add(DrRowR1);

            DataTable dtReportFooter = new DataTable();
            dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
            DataRow FooterRow = dtReportFooter.NewRow();
            FooterRow[0] = "* * *  End Of Report * * *   ";
            dtReportFooter.Rows.Add(FooterRow);



            if (ddlExport.SelectedItem.Value == "E")
            {
                // objExcel.ExportToExcelforExcel(dtEx, "DPNetworth Statement", "Branch/Group Total", dtReportHeader, dtReportFooter);
                ExcelExport(dtEx, "DPNetworth Statement", "Branch/Group Total", dtReportHeader, dtReportFooter);

            }
            else if (ddlExport.SelectedItem.Value == "P")
            {
                objExcel.ExportToPDF(dtEx, "DPNetworth Statement", "Branch/Group Total", dtReportHeader, dtReportFooter);
            }
            //else if (ddlExport.SelectedItem.Value == "PM")
            //{
            //    objExcel.ExportToPDFPageBreak(dtEx, "CDSL DPNetworth Statement", "Branch/Group Total", dtReportHeader, dtReportFooter);
            //}



        }

        protected void ExcelExport(DataTable datatable, string fname, string compareString, DataTable dtHeader, DataTable dtFooter)
        {
            //Create a dummy GridView 
            int colNo = 0;
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;
            GridView1.DataSource = datatable;
            GridView1.DataBind();
            GridView1.HeaderStyle.ForeColor = System.Drawing.Color.Blue;
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

            //hw.WriteLine(@"<style>.number2 {mso-number-format:0\.00; } </style>");
            // hw.WriteLine(@"<style>.number2 {mso-number-format:\@; } </style>");
            hw.WriteLine(@"<style>.number2 {mso-number-format:\#\,\#\#0\.00; } </style>");
            hw.WriteLine(@"<style>.number3 {mso-number-format:\#\,\#\#0\.000; } </style>");

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
                GridView2.Rows[j].Cells[0].ColumnSpan = GridView1.Rows[0].Cells.Count;
                GridView2.Rows[j].Cells[0].BackColor = System.Drawing.Color.LightYellow;
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
                            GridView1.Rows[i].Cells[6].Attributes.Add("class", "number2");
                            GridView1.Rows[i].Cells[5].Attributes.Add("class", "number3");
                            GridView1.Rows[i].Cells[4].HorizontalAlign = HorizontalAlign.Right;
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
                            GridView1.Rows[i].Cells[6].Attributes.Add("class", "number2");
                            GridView1.Rows[i].Cells[5].Attributes.Add("class", "number3");
                            GridView1.Rows[i].Cells[4].HorizontalAlign = HorizontalAlign.Right;
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
                        GridView1.Rows[i - 1].Cells[6].Attributes.Add("class", "number2");
                        GridView1.Rows[i - 1].Cells[5].Attributes.Add("class", "number3");
                        GridView1.Rows[i - 1].Cells[4].HorizontalAlign = HorizontalAlign.Right;
                        GridView1.Rows[i - 1].ForeColor = System.Drawing.Color.Blue;
                        GridView1.Rows[i - 1].Font.Bold = true;
                        GridView1.Rows[i - 1].Font.Size = new FontUnit("10px");
                    }
                    else if (GridView1.Rows[i - 1].Cells[0].Text != "&nbsp;" && GridView1.Rows[i - 1].Cells[1].Text == "Test" && GridView1.Rows[i - 1].Cells[2].Text == "&nbsp;" && GridView1.Rows[i - 1].Cells[3].Text == "&nbsp;")
                    {
                        GridView1.Rows[i - 1].Font.Bold = true;
                        GridView1.Rows[i - 1].Font.Size = new FontUnit("10px");
                        GridView1.Rows[i - 1].HorizontalAlign = HorizontalAlign.Left;
                        GridView1.Rows[i - 1].ForeColor = System.Drawing.Color.Blue;
                        GridView1.Rows[i - 1].Cells[0].ColumnSpan = GridView1.Rows[i - 1].Cells.Count;
                        GridView1.Rows[i - 1].Cells[0].BackColor = System.Drawing.Color.LightYellow;
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
                        GridView1.Rows[i - 1].Cells[6].Attributes.Add("class", "number2");
                        GridView1.Rows[i - 1].Cells[5].Attributes.Add("class", "number3");
                        GridView1.Rows[i - 1].Cells[4].HorizontalAlign = HorizontalAlign.Right;
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
                GridView3.Rows[j1].HorizontalAlign = HorizontalAlign.Center;
                GridView3.Rows[j1].ForeColor = System.Drawing.Color.Blue;
                GridView3.Rows[j1].Cells[0].ColumnSpan = colNo;
                GridView3.Rows[j1].Cells[0].BackColor = System.Drawing.Color.LightYellow;
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
    }
}