using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.DashboardSetting
{
    public partial class DashboardSetting : System.Web.UI.Page
    {
        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindComponentsByRights();
                if (Request.QueryString["id"] != "ADD")
                {
                    PopulateDashBoardDetailsById(Request.QueryString["id"]);

                    Hidden_add_edit.Value = "edit";
                    Hidn_dash_board_id.Value = Request.QueryString["id"];
                }

                else
                {
                    Hidden_add_edit.Value = "add";
                }

            }


        }
        #endregion

        #region CustomMethods
        private void bindComponentsByRights()
        {
            List<Components> _components = new List<Components>();
            DataTable dt = new DataTable();
            string output = string.Empty;
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    string masterdbname = Convert.ToString(ConfigurationSettings.AppSettings["MasterDBName"]);

                    ProcedureExecute proc = new ProcedureExecute("prc_dashboardsetting");
                    proc.AddVarcharPara("@Action", 30, "FETCHCOMPONENTBYRIGHTS");
                    proc.AddVarcharPara("@MASTERDB", 100, masterdbname);
                    proc.AddVarcharPara("@is_success", 500, "", QueryParameterDirection.Output);
                    dt = proc.GetTable();
                    output = Convert.ToString(proc.GetParaValue("@is_success"));
                    if (output == "true")
                    {
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            _components = (from DataRow dr in dt.Rows
                                           select new Components()
                                          {
                                              id = dr["id"].ToString(),
                                              text = dr["text"].ToString(),
                                              parent_id = dr["parent_id"].ToString(),
                                              column_name = dr["column_name"].ToString()
                                          }).ToList();

                        }


                    }
                }

                if (_components.Count != 0)
                {
                    var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    jsonlistdiv.InnerText = oSerializer.Serialize(_components);

                }

            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }

        }



        private void PopulateDashBoardDetailsById(string dashboard_id)
        {
            List<CheckRights> _chkrights = new List<CheckRights>();
            string output = string.Empty;
            DataSet ds = new DataSet();
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    ProcedureExecute proc = new ProcedureExecute("prc_dashboardsetting");
                    proc.AddVarcharPara("@Action", 30, "select");
                    proc.AddIntegerPara("@ID", Convert.ToInt32(dashboard_id.ToString().Trim()));
                    proc.AddVarcharPara("@is_success", 500, "", QueryParameterDirection.Output);
                    ds = proc.GetDataSet();
                    output = Convert.ToString(proc.GetParaValue("@is_success"));
                    if (output == "true")
                    {
                        if (ds != null && ds.Tables.Count>0)
                        {
                            
                            txt_setting_nm.Text = ds.Tables[0].Rows[0]["group_name"].ToString().Trim();
                            txt_setting_nm.ClientEnabled = false;
                            UserId.Value = ds.Tables[0].Rows[0]["group_id"].ToString().Trim();

                            //if(ds.Tables[2]!=null && ds.Tables[2].Rows.Count>0)
                            //{
                            //    list1.DataSource = ds.Tables[2];
                            //    list1.DataTextField = "user_name";
                            //    list1.DataValueField = "user_id";
                            //    list1.DataBind();
                            //}

                            if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                            {
                                list2.DataSource = ds.Tables[1];
                                list2.DataTextField = "user_name";
                                list2.DataValueField = "user_id";
                                list2.DataBind();
                            }


                            DataRow dr = ds.Tables[1].Rows[0];

                            foreach (DataColumn dc in ds.Tables[1].Columns) {
                                string colname = "";
                                bool status = false;

                                if (dc.ColumnName != "id" && dc.ColumnName != "Dashboard_id" && dc.ColumnName != "user_name" && dc.ColumnName != "user_id")
                                {
                                    CheckRights _rights = new CheckRights();
                                    _rights.column_name = dc.ColumnName;
                                    _rights.status=Convert.ToBoolean(dr[dc]);
                                    _chkrights.Add(_rights);
                               }
                            
                            }

                            if (_chkrights.Count != 0)
                            {
                                var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                jsonlisteditdiv.InnerText = oSerializer.Serialize(_chkrights);

                            }


                        }


                    }
                    else
                    {
                       
                    }
                }   
            }
            catch(Exception ex)
            {
                output = ex.Message.ToString();
            }
        }

        #endregion
    }

    #region page class
    public class Components
    {
        public string id { get; set; }
        public string text { get; set; }
        public string parent_id { get; set; }
        public string column_name { get; set; }
    }

    public class CheckRights
    {
        public string column_name { get; set; }

        public bool status { get; set; }
    }
    #endregion
}