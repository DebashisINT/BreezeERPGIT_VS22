using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace ERP.OMS.Management.Payroll
{
    public partial class PayslipConfiguration : System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Payroll/ViewPayslip.aspx");
            if (!IsPostBack)
            {
                Session["lookup_DesignName"] = null;
                LoadDesign("PageLoad");
                //Session["lookup_DesignName"] = null;
                //DataTable dtDesignName = new DataTable();
                //dtDesignName.Columns.Add("DesignName", typeof(String));
                //dtDesignName.Columns.Add("FullDesignName", typeof(String));


                //string[] filePaths = new string[] { };
                //string DesignPath = "";
                //if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                //{
                //    DesignPath = @"Reports\Reports\RepxReportDesign\PaySlip\DocDesign\Designes";
                //}
                //else
                //{
                //    DesignPath = @"Reports\RepxReportDesign\PaySlip\DocDesign\Designes";
                //}
                //string fullpath = Server.MapPath("~");
                //fullpath = fullpath.Replace("ERP.UI\\", "");
                //string DesignFullPath = fullpath + DesignPath;
                //filePaths = System.IO.Directory.GetFiles(DesignFullPath, "*.repx");

                //foreach (string filename in filePaths)
                //{
                //    string reportname = Path.GetFileNameWithoutExtension(filename);
                //    string name = "";
                //    if (reportname.Split('~').Length > 1)
                //    {
                //        name = reportname.Split('~')[0];
                //    }
                //    else
                //    {
                //        name = reportname;
                //    }
                //    string reportValue = reportname;
                //    //if (reportValue != SavereportValue)
                //    //{
                //    CmbDefaultDesignName.Items.Add(name, reportValue);
                //    //}
                //    dtDesignName.Rows.Add(name, reportValue);
                //}
                //CmbDefaultDesignName.SelectedIndex = 0;

                //Session["lookup_DesignName"] = dtDesignName;
                //Gridlookup_DesignName.DataBind();
                ViewGrid.DataBind();
            }
        }
        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void ViewGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

            var ActionType = Convert.ToString(e.Parameters.Split('|')[0]);

            if (ActionType == "Add")
            {
                var StructureId = Convert.ToString(e.Parameters.Split('|')[1]);
                var DefaultDesign = Convert.ToString(e.Parameters.Split('|')[2]);
                
                if (StructureId.Trim() != "" && DefaultDesign.Trim() != "" && DefaultDesign.Trim() != "null")
                {
                    string strUserID = Convert.ToString(Session["userid"]);

                    DataTable dtCheck = new DataTable();
                    var sqlQuery = "SELECT StructureId FROM proll_PayslipCong WHERE StructureId = '" + StructureId.Trim() + "'";
                    dtCheck = oDBEngine.GetDataTable(sqlQuery);
                    // duplicate manual entry check
                    if (dtCheck.Rows.Count > 0)
                    {
                        ViewGrid.JSProperties["cpInsertSuccessOrFail"] = "duplicate"; ;
                    }
                    else
                    {
                        DataTable dtSelectedDesign = new DataTable();
                        dtSelectedDesign.Columns.Add("DesignName", typeof(String));

                        List<object> DesignList = Gridlookup_DesignName.GridView.GetSelectedFieldValues("FullDesignName");
                        foreach (object DesignN in DesignList)
                        {
                            dtSelectedDesign.Rows.Add(DesignN);
                        }

                        if (dtSelectedDesign.Rows.Count>0)
                        {
                            try
                            {
                                DataSet dsInst = new DataSet();

                                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                                SqlCommand cmd = new SqlCommand("prc_InsertPayrollConfiguration", con);

                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@Action", ActionType);
                                cmd.Parameters.AddWithValue("@StructureId", StructureId);
                                cmd.Parameters.AddWithValue("@DefaultDesign", DefaultDesign);
                                cmd.Parameters.AddWithValue("@SelectedDesign", dtSelectedDesign);
                                cmd.Parameters.AddWithValue("@UserID", strUserID);

                                cmd.CommandTimeout = 0;
                                SqlDataAdapter Adap = new SqlDataAdapter();
                                Adap.SelectCommand = cmd;
                                Adap.Fill(dsInst);
                                cmd.Dispose();
                                con.Dispose();

                                Gridlookup_DesignName.DataBind();
                                ViewGrid.JSProperties["cpInsertSuccessOrFail"] = "successInsert";

                            }
                            catch (Exception ex)
                            {
                                ViewGrid.JSProperties["cpInsertSuccessOrFail"] = "failInsert";
                            }
                        }
                        else
                        {
                            ViewGrid.JSProperties["cpInsertSuccessOrFail"] = "noSelectedDesign";
                        }
                    }
                }
                else
                {
                    if(StructureId.Trim() == ""  )
                    {
                        ViewGrid.JSProperties["cpInsertSuccessOrFail"] = "emptyPayStructure";
                    }
                    else if(DefaultDesign.Trim() =="" || DefaultDesign.Trim() =="null" )
                    {
                        ViewGrid.JSProperties["cpInsertSuccessOrFail"] = "emptyDefaultDesign";
                    }
                }
                
            }
            else if (ActionType == "Edit")
            {
                var payConfig_ID = Convert.ToString(e.Parameters.Split('|')[1]);

                string strUserID = Convert.ToString(Session["userid"]);

                DataTable dtLoad = new DataTable();
                //var sqlQueryFetch = "select PConfg.payConfig_ID,PMast.StructureID,PMast.StructureName,PConfg.DefaultDesign,ISNULL((SELECT  LTRIM(STUFF(((SELECT DISTINCT ', ' + DesignName FROM (SELECT pmap.DesignName FROM proll_PayslipDesignMap pmap INNER JOIN proll_PayslipCong Pmapconfig ON pmap.payConfig_ID=Pmapconfig.payConfig_ID AND Pmapconfig.payConfig_ID=PConfg.payConfig_ID ) AS desnnm FOR XML PATH(''))),1,2,' '))),'') AS SelectedDesign FROM proll_PayslipCong PConfg left outer join proll_PayStructureMaster PMast on PMast.StructureID=PConfg.StructureID where PConfg.payConfig_ID = '" + payConfig_ID + "'";
                //dtLoad = oDBEngine.GetDataTable(sqlQueryFetch);

                DataTable dtPayslipConfig = new DataTable();
                DataAccessLayer.ProcedureExecute proc = new DataAccessLayer.ProcedureExecute("prc_GetPayslipData");
                proc.AddVarcharPara("@Action", 100, "EditPayslipConfigData");
                proc.AddVarcharPara("@payConfig_ID", 100, payConfig_ID);
                dtLoad = proc.GetTable();

                // duplicate manual entry check
                if (dtLoad.Rows.Count > 0)
                {
                    foreach (DataRow row in dtLoad.Rows)
                    {
                        ViewGrid.JSProperties["cpPayStructure"] = Convert.ToString(row["StructureName"]);
                        ViewGrid.JSProperties["cpHdnPayStructureID"] = Convert.ToString(row["StructureID"]);
                        ViewGrid.JSProperties["cpDefaultDesignName"] = Convert.ToString(row["DefaultDesign"]);
                        ViewGrid.JSProperties["cpSelectedDesignName"] = Convert.ToString(row["SelectedDesign"]);
                    }

                   ViewGrid.JSProperties["cpInsertSuccessOrFail"] = "Edit"; ;
                }

                txtPayStructure.Enabled = false;

            }
            else if (ActionType == "Delete")
            {
                var payConfig_ID = Convert.ToString(e.Parameters.Split('|')[1]);
                
                string strUserID = Convert.ToString(Session["userid"]);

                try
                {
                    DataTable dtSelectedDesign = new DataTable();
                    dtSelectedDesign.Columns.Add("DesignName", typeof(String));  // Blank Table sent since in delelte UDT not needed.

                    DataSet dsInst = new DataSet();

                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                    SqlCommand cmd = new SqlCommand("prc_InsertPayrollConfiguration", con);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", ActionType);
                    cmd.Parameters.AddWithValue("@UserID", strUserID);
                    cmd.Parameters.AddWithValue("@payConfig_ID", payConfig_ID);
                    cmd.Parameters.AddWithValue("@SelectedDesign", dtSelectedDesign);

                    cmd.CommandTimeout = 0;
                    SqlDataAdapter Adap = new SqlDataAdapter();
                    Adap.SelectCommand = cmd;
                    Adap.Fill(dsInst);
                    cmd.Dispose();
                    con.Dispose();

                    Gridlookup_DesignName.DataBind();
                    ViewGrid.JSProperties["cpInsertSuccessOrFail"] = "successDelete";

                }
                catch (Exception ex)
                {
                    ViewGrid.JSProperties["cpInsertSuccessOrFail"] = "failDelete";
                }
            }
            if (ActionType == "Update")
            {
                var StructureId = Convert.ToString(e.Parameters.Split('|')[1]);
                var DefaultDesign = Convert.ToString(e.Parameters.Split('|')[2]);
                var PayConfig_ID = Convert.ToString(e.Parameters.Split('|')[3]);


                if (StructureId.Trim() != "" && DefaultDesign.Trim() != "" && DefaultDesign.Trim() != "null")
                {
                    string strUserID = Convert.ToString(Session["userid"]);

                    DataTable dtCheck = new DataTable();
                    var sqlQuery = "SELECT StructureId FROM proll_PayslipCong WHERE PayConfig_ID = '" + PayConfig_ID.Trim() + "'";
                    dtCheck = oDBEngine.GetDataTable(sqlQuery);
                    // duplicate manual entry check
                    if (dtCheck.Rows.Count > 0)
                    {
                        DataTable dtSelectedDesign = new DataTable();
                        dtSelectedDesign.Columns.Add("DesignName", typeof(String));

                        List<object> DesignList = Gridlookup_DesignName.GridView.GetSelectedFieldValues("FullDesignName");
                        foreach (object DesignN in DesignList)
                        {
                            dtSelectedDesign.Rows.Add(DesignN);
                        }

                        if(dtSelectedDesign.Rows.Count>0)
                        { 
                            try
                            {
                                DataSet dsInst = new DataSet();

                                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                                SqlCommand cmd = new SqlCommand("prc_InsertPayrollConfiguration", con);

                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@Action", ActionType);
                                cmd.Parameters.AddWithValue("@StructureId", StructureId);
                                cmd.Parameters.AddWithValue("@DefaultDesign", DefaultDesign);
                                cmd.Parameters.AddWithValue("@SelectedDesign", dtSelectedDesign);
                                cmd.Parameters.AddWithValue("@UserID", strUserID);
                                cmd.Parameters.AddWithValue("@payConfig_ID", PayConfig_ID);

                                cmd.CommandTimeout = 0;
                                SqlDataAdapter Adap = new SqlDataAdapter();
                                Adap.SelectCommand = cmd;
                                Adap.Fill(dsInst);
                                cmd.Dispose();
                                con.Dispose();

                                Gridlookup_DesignName.DataBind();
                                ViewGrid.JSProperties["cpInsertSuccessOrFail"] = "successUpdate";


                            }
                            catch (Exception ex)
                            {
                                ViewGrid.JSProperties["cpInsertSuccessOrFail"] = "failUpdate";
                            }
                        }
                        else
                        {
                            ViewGrid.JSProperties["cpInsertSuccessOrFail"] = "noSelectedDesign";
                        }

                    }
                    else
                    {
                        ViewGrid.JSProperties["cpInsertSuccessOrFail"] = "Pay Structure doesnot exist";
                    }

                    txtPayStructure.Enabled = true;
                }
                else
                {
                    if (StructureId.Trim() == "")
                    {
                        ViewGrid.JSProperties["cpInsertSuccessOrFail"] = "emptyPayStructure";
                    }
                    else if (DefaultDesign.Trim() == "" || DefaultDesign.Trim() == "null")
                    {
                        ViewGrid.JSProperties["cpInsertSuccessOrFail"] = "emptyDefaultDesign";
                    }
                }
            }
            
            
        }

        protected void Gridlookup_DesignName_DataBinding(object sender, EventArgs e)
        {
            if (Session["lookup_DesignName"] != null)
            {
               Gridlookup_DesignName.DataSource = (DataTable)Session["lookup_DesignName"];

            }

        }

        protected void ViewGrid_DataBinding(object sender, EventArgs e)
        {
            //DataTable dtPayslipConfig = oDBEngine.GetDataTable("select PConfg.payConfig_ID,PMast.StructureName,PConfg.DefaultDesign,ISNULL((SELECT  LTRIM(STUFF(((SELECT DISTINCT ', ' + DesignName FROM (SELECT pmap.DesignName FROM proll_PayslipDesignMap pmap INNER JOIN proll_PayslipCong Pmapconfig ON pmap.payConfig_ID=Pmapconfig.payConfig_ID AND Pmapconfig.payConfig_ID=PConfg.payConfig_ID ) AS desnnm FOR XML PATH(''))),1,2,' '))),'') AS SelectedDesign FROM proll_PayslipCong PConfg left outer join proll_PayStructureMaster PMast on PMast.StructureID=PConfg.StructureID ");

            DataTable dtPayslipConfig = new DataTable();
            DataAccessLayer.ProcedureExecute proc = new DataAccessLayer.ProcedureExecute("prc_GetPayslipData");
            proc.AddVarcharPara("@Action", 100, "GetPayslipConfigData");
            dtPayslipConfig = proc.GetTable();
            
            ViewGrid.DataSource = dtPayslipConfig;

        }


        //protected void Panellookup_DesignName_Callback(object sender, CallbackEventArgsBase e)
        //{
        //    var ActionType = Convert.ToString(e.Parameters.Split('|')[0]);

        //    if (ActionType == "Add")
        //    {
        //        Gridlookup_DesignName.GridView.Selection.CancelSelection();
        //    }
        //    else if (ActionType == "Edit")
        //    {
        //        String SelectedDesign_val = Convert.ToString(e.Parameters.Split('|')[1]);
        //        Gridlookup_DesignName.GridView.Selection.CancelSelection();

        //        if (SelectedDesign_val != "")
        //        {
        //            string[] eachDesign = SelectedDesign_val.Split(',');
        //            foreach (string val in eachDesign)
        //            {
        //                if (val != "")
        //                {
        //                    Gridlookup_DesignName.GridView.Selection.SelectRowByKey(Convert.ToString(val));
        //                }

        //            }
        //        }
        //    }

        //}

        protected void Panellookup_DesignName_Callback1(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            var ActionType = Convert.ToString(e.Parameter.Split('|')[0]);

            if (ActionType == "Add")
            {
                LoadDesign("Add");
            }
            else if (ActionType == "Edit")
            {
                LoadDesign("Add");
                String SelectedDesign_val = Convert.ToString(e.Parameter.Split('|')[1]);

                if (SelectedDesign_val != "")
                {
                    string[] eachDesign = SelectedDesign_val.Split(',');
                    foreach (string val in eachDesign)
                    {
                        if (val != "")
                        {
                            Gridlookup_DesignName.GridView.Selection.SelectRowByKey(Convert.ToString(val));
                        }

                    }
                }
            }
        }

        
        
        private void LoadDesign(string action)
        {
            
            DataTable dtDesignName = new DataTable();
            dtDesignName.Columns.Add("DesignName", typeof(String));
            dtDesignName.Columns.Add("FullDesignName", typeof(String));
            Session["lookup_DesignName"] = dtDesignName;
            Gridlookup_DesignName.DataBind();

            string[] filePaths = new string[] { };
            string DesignPath = "";
            if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
            {
                DesignPath = @"Reports\Reports\RepxReportDesign\PaySlip\DocDesign\Designes";
            }
            else
            {
                DesignPath = @"Reports\RepxReportDesign\PaySlip\DocDesign\Designes";
            }
            string fullpath = Server.MapPath("~");
            fullpath = fullpath.Replace("ERP.UI\\", "");
            string DesignFullPath = fullpath + DesignPath;
            filePaths = System.IO.Directory.GetFiles(DesignFullPath, "*.repx");

            foreach (string filename in filePaths)
            {
                string reportname = Path.GetFileNameWithoutExtension(filename);
                string name = "";
                if (reportname.Split('~').Length > 1)
                {
                    name = reportname.Split('~')[0];
                }
                else
                {
                    name = reportname;
                }
                string reportValue = reportname;
                //if (reportValue != SavereportValue)
                //{

                if (action == "PageLoad")
                {
                    CmbDefaultDesignName.Items.Add(name, reportValue);
                }
                //}
                dtDesignName.Rows.Add(name, reportValue);
            }

            if (action == "PageLoad")
            {
                CmbDefaultDesignName.SelectedIndex = 0;
            }

            Session["lookup_DesignName"] = dtDesignName;
            Gridlookup_DesignName.DataBind();
        }

        
    }
   
}