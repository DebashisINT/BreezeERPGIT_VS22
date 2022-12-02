using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.SqlClient;
//using DevExpress.Web;
using DevExpress.Web;
using BusinessLogicLayer;
namespace ERP.OMS.Management.DailyTask
{

    public partial class management_DailyTask_display_xml : ERP.OMS.ViewState_class.VSPage
    {
        DataSet datatemp = new DataSet();
       // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        DailyTaskOther oDailyTaskOther = new DailyTaskOther();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ClientScript.IsStartupScriptRegistered("Today"))
                ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>PageLoad();</script>");
            if (!IsPostBack)
            {
                bind();
            }
        }
        protected void dtexec_DateChanged(object sender, EventArgs e)
        {
            //Session["executiondate"] = dtexec.Text;

        }

        public void bind()
        {
            string dp = Request.QueryString["id"].ToString();
            if (dp == "nsdl")
            {
                datatemp.Reset();
                DirectoryInfo dir = new DirectoryInfo(Server.MapPath("../Documents/"));
                FileInfo[] fileInfos = dir.GetFiles("*nsdl");
                for (int leng = 0; leng < fileInfos.Length; leng++)
                {
                    datatemp.ReadXml(((System.IO.FileSystemInfo)(fileInfos[leng])).FullName);

                }
                if (fileInfos.Length == 0)
                {
                    Panel1.Visible = false;
                    btncan.Visible = false;
                    btnsaveset.Visible = false;
                    Label1.Visible = true;
                }
                else
                {
                    try
                    {
                        datatemp.Tables[0].Columns.Add("id", typeof(string));
                        //datatemp.Tables[0].PrimaryKey = datatemp.Tables[0].Columns["id"];
                        datatemp.Tables[0].AcceptChanges();

                        for (int i = 0; i < datatemp.Tables[0].Rows.Count; i++)
                        {
                            datatemp.Tables[0].Rows[i]["id"] = i;
                        }
                        datatemp.Tables[0].AcceptChanges();

                        DetailsGrid.DataSource = datatemp;
                        DetailsGrid.DataBind();
                        Label1.Visible = false;
                    }
                    catch
                    {
                        DetailsGrid.DataSource = datatemp;
                        DetailsGrid.DataBind();
                        Label1.Visible = false;
                    }


                }
                ViewState["DetailsGrid"] = datatemp;


            }
        }
        protected void btncan_Click(object sender, EventArgs e)
        {
            if (Session["dp"].ToString() == "NSDL")
            {
                DirectoryInfo dir = new DirectoryInfo(Server.MapPath("../Documents/"));
                FileInfo[] fileInfos = dir.GetFiles("*nsdl");
                for (int leng = 0; leng < fileInfos.Length; leng++)
                {
                    File.Delete(((System.IO.FileSystemInfo)(fileInfos[leng])).FullName);
                }
            }


            bind();

        }
        protected void btnsaveset_Click(object sender, EventArgs e)
        {
            DataSet ds = (DataSet)ViewState["DetailsGrid"];
            int flag = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (Convert.ToDateTime(ds.Tables[0].Rows[i]["executiondate"].ToString().Split('-')[1] + "-" + ds.Tables[0].Rows[i]["executiondate"].ToString().Split('-')[0] + "-" + ds.Tables[0].Rows[i]["executiondate"].ToString().Split('-')[2]) < Convert.ToDateTime(oDBEngine.GetDate().Date))
                {
                    flag = 1;
                    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "di", "alert('Can not update because Execution date is less than current date.');", true);
                    break;

                }
            }
            if (flag != 1)
            {
                //SqlConnection lConn = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
                //if (lConn.State == ConnectionState.Open)
                //{
                //    lConn.Close();
                //}

                //lConn.Open();
                //SqlCommand cmd = new SqlCommand("sp_insert_settlement", lConn);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@dp", Session["dp"].ToString());
                //cmd.Parameters.AddWithValue("@doc", ds.GetXml());
                if (ds.Tables.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "di", "alert('No record exists.');", true);

                }

                else
                {
                    //cmd.ExecuteNonQuery();
                    //lConn.Close();
                    oDailyTaskOther.InsertSettlement(Session["dp"].ToString(), ds.GetXml());
                    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "di", "alert('Updated Sucessfully.');", true);

                }
                try
                {
                    if (Session["dp"].ToString() == "NSDL")
                    {
                        if (File.Exists(Server.MapPath("../Documents/" + "unsaved" + "nsdl")))
                        {
                            File.Copy(Server.MapPath("../Documents/" + "unsaved" + "nsdl"), Server.MapPath("../Documents/" + "unsaved" + "nsdl" + "v"));
                            File.Delete(Server.MapPath("../Documents/" + "unsaved" + "nsdl"));

                        }
                    }
                }
                catch
                {

                }
            }
        }
        protected void btnsave_Click(object sender, EventArgs e)
        {

        }


        protected void DetailsGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string id = e.CommandArgument.ToString();
            DirectoryInfo dir = new DirectoryInfo(Server.MapPath("../Documents/"));
            FileInfo[] fileInfos = dir.GetFiles("*nsdl");

            for (int leng = 0; leng < fileInfos.Length; leng++)
            {
                File.Delete(((System.IO.FileSystemInfo)(fileInfos[leng])).FullName);
            }
            DataSet ds = (DataSet)ViewState["DetailsGrid"];
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (i == Convert.ToInt32(id))
                {
                    ASPxDateEdit dt = (ASPxDateEdit)DetailsGrid.Rows[i].FindControl("dtexec");
                    string s = Convert.ToDateTime(dt.Date).ToString("dd-MM-yyyy");
                    ds.Tables[0].Rows[i]["executiondate"] = s;
                    ds.Tables[0].AcceptChanges();
                    ds.WriteXml(Server.MapPath("../Documents/" + "unsaved" + "nsdl"));

                }
            }

            bind();

        }
        protected void DetailsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    if (DataBinder.Eval(e.Row.DataItem, "verifyuser") == DBNull.Value)
            //    {
            //    }
            //    else
            //    {
            //        int verified = (int)DataBinder.Eval(e.Row.DataItem, "verifyuser");
            //        e.Row.ForeColor = System.Drawing.Color.Crimson;
            //        e.Row.Font.Italic = true;
            //        e.Row.BackColor = System.Drawing.Color.Lavender;
            //        CheckBox c = (CheckBox)e.Row.Cells[6].FindControl("CheckBox3");
            //        c.Checked = true;
            //    }
            //    if (DataBinder.Eval(e.Row.DataItem, "Rejected") == DBNull.Value)
            //    {
            //        //string rowID = "row" + e.Row.RowIndex;
            //        //ScriptManager.RegisterStartupScript(this, this.GetType(), "JSWE1", "aaa('" + rowID + "');", true);

            //        //e.Row.ForeColor = System.Drawing.Color.White;
            //        //e.Row.Font.Italic = true;
            //        //e.Row.BackColor = System.Drawing.Color.White;
            //        //Button b = (Button)e.Row.Cells[6].FindControl("btnreject");
            //        //b.BackColor = System.Drawing.Color.SteelBlue;
            //    }
            //    else
            //    {
            //        string rejected = (string)DataBinder.Eval(e.Row.DataItem, "Rejected");
            //        if (rejected != null)
            //        {
            //            e.Row.ForeColor = System.Drawing.Color.Crimson;
            //            e.Row.Font.Italic = true;
            //            e.Row.BackColor = System.Drawing.Color.Bisque;
            //            Button b = (Button)e.Row.Cells[6].FindControl("btnreject");
            //            b.BackColor = System.Drawing.Color.DarkRed;
            //            b.ForeColor = System.Drawing.Color.White;
            //            CheckBox c = (CheckBox)e.Row.Cells[6].FindControl("CheckBox3");
            //            c.Checked = false;
            //            c.Enabled = false;
            //        }
            //        else
            //        {

            //        }
            //    }
            //}      
        }
    }
}

