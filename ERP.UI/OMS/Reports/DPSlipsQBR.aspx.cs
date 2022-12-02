using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Reports
{
    public partial class Reports_DPSlipsQBR : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.Reports oReports = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        ExcelFile objExcel = new ExcelFile();
        static DataSet ds = new DataSet();
        int pageindex = 0;
        int pagecount = 0;
        int pageSize = 0;
        int rowcount = 0;
        public string dp = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //________This script is for firing javascript when page load first___//
            if (!ClientScript.IsStartupScriptRegistered("Today"))
                ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>PageLoad();</script>");
            // ______________________________End Script____________________________//

            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            if (HttpContext.Current.Session["Segmentname"] != null)
            {
                string strSegmentName = HttpContext.Current.Session["Segmentname"].ToString();
                dp = strSegmentName;
                Session["dp"] = strSegmentName;

            }
            else
            {
                dp = HttpContext.Current.Session["userlastsegment"].ToString();
                Session["dp"] = HttpContext.Current.Session["userlastsegment"].ToString();
            }



        }
        protected void NavigationLink_Click1(Object sender, CommandEventArgs e)
        {
            //RememberOldValues();
            //switch (e.CommandName)
            //{
            //    case "First":
            //        pageindex = 0;
            //        break;
            //    case "Next":
            //        pageindex = int.Parse(CurrentPage.Value) + 1;
            //        break;
            //    case "Prev":
            //        if (int.Parse(CurrentPage.Value) == 0)
            //            pageindex = 0;
            //        else
            //            pageindex = int.Parse(CurrentPage.Value) - 1;
            //        break;
            //    case "Last":
            //        pageindex = int.Parse(TotalPages.Value);
            //        break;
            //    default:
            //        pageindex = int.Parse(e.CommandName.ToString());
            //        break;
            //}

            //bindverify();
            //RePopulateValues();

        }
        private void RememberOldValues()
        {
            ArrayList categoryIDList = new ArrayList();
            Int64 index = -1;
            foreach (GridViewRow row in offlineGrid.Rows)
            {
                index = (Int64)offlineGrid.DataKeys[row.RowIndex].Value;
                bool result = ((CheckBox)row.FindControl("CheckBox3")).Checked;

                // Check in the Session
                if (Session["CHECKED_ITEMS"] != null)
                    categoryIDList = (ArrayList)Session["CHECKED_ITEMS"];
                if (result)
                {
                    if (!categoryIDList.Contains(index))
                    {
                        categoryIDList.Add(index);

                    }
                }
                else
                    categoryIDList.Remove(index);
            }
            if (categoryIDList != null && categoryIDList.Count > 0)
                Session["CHECKED_ITEMS"] = categoryIDList;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string segment = string.Empty;
            if (dp == "NSDL" || dp == "9")
            {
                segment = "9";
            }
            else
            {
                segment = "10";
            }
            // using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            // {
            ds = oReports.Sp_Fetch_SlipsQBR_Data(
                Convert.ToString(ddlSlipType.SelectedItem.Value),
                 Convert.ToString(txtsliipno.Text),
                 Convert.ToString(segment),
                 Convert.ToString(HttpContext.Current.Session["usersegid"])
                );
            //using (SqlCommand com = new SqlCommand("Sp_Fetch_SlipsQBR_Data", con))
            //{
            //    com.CommandType = CommandType.StoredProcedure;
            //    com.Parameters.AddWithValue("@sliptype", Convert.ToInt32(ddlSlipType.SelectedItem.Value));
            //    com.Parameters.AddWithValue("@slipno", txtsliipno.Text);
            //    if (dp == "NSDL" || dp == "9")
            //    {
            //        com.Parameters.AddWithValue("@segment", 9);
            //    }
            //    else
            //    {
            //        com.Parameters.AddWithValue("@segment", 10);
            //    }
            //    com.Parameters.AddWithValue("@Dpid", HttpContext.Current.Session["usersegid"].ToString());
            //    using (SqlDataAdapter da = new SqlDataAdapter(com))
            //    {
            //        ds.Clear();
            //        da.Fill(ds);
            //    }
            //}
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0][0].ToString() == "2")
                {
                    if (ds.Tables[0].Rows[0]["DpSlipsUsage_Status"].ToString() == "2")
                    {
                        string stralert = ds.Tables[0].Rows[0]["DpSlipsUsage_SlipNumber"].ToString().Trim() + " has Been Canceled due to " +
                            ds.Tables[0].Rows[0]["DpSlipsUsage_Reason"].ToString().Trim() + " Reason On " + ds.Tables[0].Rows[0]["DpSlipsUsage_CancelDate"].ToString().Trim() + " Date";
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + stralert + " ');", true);
                    }
                    if (ds.Tables[0].Rows[0]["DpSlipsUsage_Status"].ToString() == "0")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Slip yet to be Entered');", true);

                    }
                    offlineGrid.DataSource = null;
                    offlineGrid.DataBind();

                }
                else if (ds.Tables[0].Rows[0][0].ToString() == "3")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Record Not Exist');", true);
                    offlineGrid.DataSource = null;
                    offlineGrid.DataBind();
                }
                else
                {
                    offlineGrid.DataSource = ds;
                    offlineGrid.DataBind();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Record Not Exist');", true);
                offlineGrid.DataSource = null;
                offlineGrid.DataBind();
            }

            // }
        }

        protected void ddlExport_SelectedIndexChanged1(object sender, EventArgs e)
        {
            if (ddlExport.SelectedItem.Text != "Export")
            {
                if (ds.Tables.Count > 0 && offlineGrid.Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() != "2")
                    {
                        string querystr = "dp=" + HttpContext.Current.Session["usersegid"].ToString();
                        querystr += "&sliptype=" + (ddlSlipType.SelectedItem.Value.ToString());
                        querystr += "&slipno=" + (txtsliipno.Text);
                        if (dp == "NSDL" || dp == "9")
                        {
                            querystr += "&segment=" + ("9");
                        }
                        else
                        {
                            querystr += "&segment=" + ("10");
                        }

                        if (ddlExport.SelectedItem.Text != "Export")
                        {
                            querystr += "&ExportTo=" + ddlExport.SelectedValue.ToString();
                        }

                        string url = @"../management/DPSlipsQBRExortToExcelFrame.aspx?" + querystr;
                        string fullURL = "window.open('" + url + "', '_blank', 'height=200,width=400,left=400,top=400,screenX=400,screenY=400,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
                        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('There is No Record to Export');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('There is No Record to Export');", true);
                }
            }
        }
        protected void offlineGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string abc = e.Row.RowIndex.ToString();
                e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
                e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.offlineGrid, "Select$" + e.Row.RowIndex);
            }
        }
    }
}