using DevExpress.Web;
using DevExpress.Web.Data;
using EntityLayer.CommonELS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class SalesdiscountApproval : ERP.OMS.ViewState_class.VSPage
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public static EntityLayer.CommonELS.UserRightsForPage rights;

        #region page event
        protected void Page_Init(object sender, EventArgs e)
        {
            //((GridViewDataComboBoxColumn)SalesapprovalGrid.Columns["branched"]).PropertiesComboBox.DataSource = Cmbbranch();


            if (!IsPostBack)
            {
                SalesapprovalGrid.DataBind();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            SalesapprovalGrid.SettingsEditing.BatchEditSettings.StartEditAction = (GridViewBatchStartEditAction)Enum.Parse(typeof(GridViewBatchStartEditAction), "click", true);

            if (HttpContext.Current.Session["userid"] != null)
            {
                if (!Page.IsPostBack)
                {
                    Sqluserlist.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    //To check User Rights 
                    Session["exportval"] = null;
                    rights = new UserRightsForPage();
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/SalesdiscountApproval.aspx");
                    // SalesapprovalGrid.AddNewRow();
                }
            }
        }

        #endregion

        #region Export event

        public void bindexport(int Filter)
        {


            string filename = "Sales discount Approval";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Sales discount Approval";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";

            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsToResponse();
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {

            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
            }
        }
        #endregion

        #region Grid Event
        protected void glCategory_ValueChanged(object sender, EventArgs e)
        {
            ASPxGridLookup lookup = (ASPxGridLookup)sender;
            ASPxGridView gridview = (ASPxGridView)lookup.GridView;

            string[] fieldNames = new string[] { "userID", "userName" };
            List<object> rowValues = gridview.GetSelectedFieldValues(fieldNames);

            for (int index = 0; index < rowValues.Count; index++)
            {
                object[] values = (object[])rowValues[index];

                string val_0 = values[0].ToString();
                string val_1 = values[1].ToString();

                if (!gridview.JSProperties.ContainsKey(string.Format("cp{0}", fieldNames[0])))
                {
                    gridview.JSProperties[string.Format("cp{0}", fieldNames[0])] = val_0;
                }
                else
                {
                    gridview.JSProperties[string.Format("cp{0}", fieldNames[0])] += "," + val_0;
                }

                if (!gridview.JSProperties.ContainsKey(string.Format("cp{0}", fieldNames[1])))
                {
                    gridview.JSProperties[string.Format("cp{0}", fieldNames[1])] = val_1;
                }
                else
                {
                    gridview.JSProperties[string.Format("cp{0}", fieldNames[1])] += "," + val_1;
                }

            }

            gridview.JSProperties["cpValueChanged"] = true;
        }
        protected void glManager_ValueChanged(object sender, EventArgs e)
        {
            ASPxGridLookup lookup = (ASPxGridLookup)sender;
            ASPxGridView gridview = (ASPxGridView)lookup.GridView;

            string[] fieldNames = new string[] { "userID", "userName" };
            List<object> rowValues = gridview.GetSelectedFieldValues(fieldNames);

            for (int index = 0; index < rowValues.Count; index++)
            {
                object[] values = (object[])rowValues[index];

                string val_0 = values[0].ToString();
                string val_1 = values[1].ToString();

                if (!gridview.JSProperties.ContainsKey(string.Format("cp{0}", fieldNames[0])))
                {
                    gridview.JSProperties[string.Format("cp{0}", fieldNames[0])] = val_0;
                }
                else
                {
                    gridview.JSProperties[string.Format("cp{0}", fieldNames[0])] += "," + val_0;
                }

                if (!gridview.JSProperties.ContainsKey(string.Format("cp{0}", fieldNames[1])))
                {
                    gridview.JSProperties[string.Format("cp{0}", fieldNames[1])] = val_1;
                }
                else
                {
                    gridview.JSProperties[string.Format("cp{0}", fieldNames[1])] += "," + val_1;
                }

            }

            gridview.JSProperties["cpValueChanged"] = true;
        }
        protected void glFinancer_ValueChanged(object sender, EventArgs e)
        {
            ASPxGridLookup lookup = (ASPxGridLookup)sender;
            ASPxGridView gridview = (ASPxGridView)lookup.GridView;

            string[] fieldNames = new string[] { "userID", "userName" };
            List<object> rowValues = gridview.GetSelectedFieldValues(fieldNames);

            for (int index = 0; index < rowValues.Count; index++)
            {
                object[] values = (object[])rowValues[index];

                string val_0 = values[0].ToString();
                string val_1 = values[1].ToString();

                if (!gridview.JSProperties.ContainsKey(string.Format("cp{0}", fieldNames[0])))
                {
                    gridview.JSProperties[string.Format("cp{0}", fieldNames[0])] = val_0;
                }
                else
                {
                    gridview.JSProperties[string.Format("cp{0}", fieldNames[0])] += "," + val_0;
                }

                if (!gridview.JSProperties.ContainsKey(string.Format("cp{0}", fieldNames[1])))
                {
                    gridview.JSProperties[string.Format("cp{0}", fieldNames[1])] = val_1;
                }
                else
                {
                    gridview.JSProperties[string.Format("cp{0}", fieldNames[1])] += "," + val_1;
                }

            }

            gridview.JSProperties["cpValueChanged"] = true;
        }
        protected void glCategory_Load(object sender, EventArgs e)
        {
            (sender as ASPxGridLookup).GridView.Width = new Unit(500, UnitType.Pixel);
        }

        protected void SalesapprovalGrid_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.Cancel = true;
        }

        protected void SalesapprovalGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = true;
        }

        protected void SalesapprovalGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            e.Cancel = true;
        }

        protected void SalesapprovalGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            SalesapprovalGrid.DataSource = GetGriddata();
            SalesapprovalGrid.DataBind();
        }

        protected void SalesapprovalGrid_DataBinding(object sender, EventArgs e)
        {
            SalesapprovalGrid.DataSource = GetGriddata();
        }
        //private static object GetValueFromTemplateControl(object sender)
        //{
        //    GridViewDataColumn c = ((ASPxGridView)sender).Columns["Approvers"] as GridViewDataColumn;
        //    ASPxGridLookup lc = ((ASPxGridView)sender).FindEditRowCellTemplateControl(c, "glCategory")
        //        as ASPxGridLookup;
        //    return lc.Value;
        //}
        protected void SalesapprovalGrid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            try
            {
                
                foreach (var args in e.InsertValues)
                {
                    //string CashReportID = Convert.ToString(args.Keys["CashReportID"]);
                    string isactive = Convert.ToString(args.NewValues["active"]);
                    string branchedid = Convert.ToString(args.NewValues["branched"]);
                    //string approvers = Convert.ToString(args.NewValues["Textdata"]);
                    string salesman = Convert.ToString(args.NewValues["Textdata"]);
                    string manager = Convert.ToString(args.NewValues["Textdatamanager"]);
                    string financer = Convert.ToString(args.NewValues["Textdatafinancer"]);

                    if (isactive == "True" && !string.IsNullOrEmpty(branchedid) && !string.IsNullOrEmpty(salesman))
                    {
                        //approvers = "'" + approvers.Trim() + "'";
                        DataSet dsEmail = new DataSet();
                        //String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                        String conn = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                        SqlConnection con = new SqlConnection(conn);
                        SqlCommand cmd3 = new SqlCommand("prc_SalesdiscountApproval", con);
                        cmd3.CommandType = CommandType.StoredProcedure;
                        if (isactive == "True")
                        { cmd3.Parameters.AddWithValue("@active", 1); }
                        else
                        {
                            cmd3.Parameters.AddWithValue("@active", 0);
                        
                        }
                       
                        cmd3.Parameters.AddWithValue("@branchid", branchedid);
                        cmd3.Parameters.AddWithValue("@userlist", salesman);
                        cmd3.Parameters.AddWithValue("@managerlist", manager);
                        cmd3.Parameters.AddWithValue("@financerlist", financer);


                        cmd3.Parameters.AddWithValue("@createdbyuser", Convert.ToInt32(HttpContext.Current.Session["userid"]));

                        cmd3.Parameters.AddWithValue("@actiontype", 2);

                        cmd3.CommandTimeout = 0;
                        SqlDataAdapter Adap = new SqlDataAdapter();
                        Adap.SelectCommand = cmd3;
                        Adap.Fill(dsEmail);
                        dsEmail.Clear();
                        cmd3.Dispose();
                        con.Dispose();
                        GC.Collect();
                    }
                }

                foreach (var args in e.UpdateValues)
                {

                    string ID = Convert.ToString(args.Keys["id"]);
                    string isactive = Convert.ToString(args.NewValues["active"]);
                    string branchedid = Convert.ToString(args.NewValues["branched"]);
                    //string approvers = Convert.ToString(args.NewValues["Textdata"]);
                    string salesman = Convert.ToString(args.NewValues["Textdata"]);
                    string manager = Convert.ToString(args.NewValues["Textdatamanager"]);
                    string financer = Convert.ToString(args.NewValues["Textdatafinancer"]);


                    if (!string.IsNullOrEmpty(ID) && isactive == "True" && !string.IsNullOrEmpty(branchedid) && !string.IsNullOrEmpty(salesman))
                    {
                        // approvers = "'" + approvers.Trim()+"'";
                        DataSet dsEmail = new DataSet();
                        //String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                        String conn = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                        SqlConnection con = new SqlConnection(conn);
                        SqlCommand cmd3 = new SqlCommand("prc_SalesdiscountApproval", con);
                        cmd3.CommandType = CommandType.StoredProcedure;

                        if (isactive == "True")
                        { cmd3.Parameters.AddWithValue("@active", 1); }
                        else
                        {
                            cmd3.Parameters.AddWithValue("@active", 0);

                        }
                        cmd3.Parameters.AddWithValue("@ids", ID);
                        cmd3.Parameters.AddWithValue("@branchid", branchedid);
                        cmd3.Parameters.AddWithValue("@userlist", salesman);
                        cmd3.Parameters.AddWithValue("@managerlist", manager);
                        cmd3.Parameters.AddWithValue("@financerlist", financer);

                        cmd3.Parameters.AddWithValue("@lastmodifyuser", Convert.ToInt32(HttpContext.Current.Session["userid"]));

                        cmd3.Parameters.AddWithValue("@actiontype", 1);

                        cmd3.CommandTimeout = 0;
                        SqlDataAdapter Adap = new SqlDataAdapter();
                        Adap.SelectCommand = cmd3;
                        Adap.Fill(dsEmail);
                        dsEmail.Clear();
                        cmd3.Dispose();
                        con.Dispose();
                        GC.Collect();

                    }
                    else {
                        //SalesapprovalGrid.JSProperties["cperroremssg"] = Convert.ToString("The Branch and Approvers are mandatory.");
                        //return;
                    }
                   
                }

                SalesapprovalGrid.JSProperties["cpupdatemssg"] = Convert.ToString("Saved Successfully.");
            }
            catch (Exception ex)
            {
                SalesapprovalGrid.JSProperties["cperroremssg"] = Convert.ToString("The Branch and Approvers are mandatory.");
            }
        }

        protected void Grid_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            //if (e.Column.FieldName == "Approvers")
            //{
            //    var tagIDs = (int[])e.Value;
            //    string text = "";
            //    if (tagIDs != null)
            //    {
            //        text = DataProvider.Getusers().Where(t => tagIDs.Contains(t.userID)).
            //           Select(t => t.userName).DefaultIfEmpty().Aggregate((a, b) => a + ", " + b);
            //    }
            //    e.DisplayText = text ?? string.Empty;
            //}
            if (e.Column.FieldName == "Salesman")
            {
                var tagIDs = (int[])e.Value;
                string text = "";
                if (tagIDs != null)
                {
                    text = DataProvider.Getusers().Where(t => tagIDs.Contains(t.userID)).
                       Select(t => t.userName).DefaultIfEmpty().Aggregate((a, b) => a + ", " + b);
                }
                e.DisplayText = text ?? string.Empty;
            }
            else if (e.Column.FieldName == "Manager")
            {
                var tagIDs = (int[])e.Value;
                string text = "";
                if (tagIDs != null)
                {
                    text = DataProvider.Getusers().Where(t => tagIDs.Contains(t.userID)).
                       Select(t => t.userName).DefaultIfEmpty().Aggregate((a, b) => a + ", " + b);
                }
                e.DisplayText = text ?? string.Empty;
            }
            else if (e.Column.FieldName == "Financer")
            {
                var tagIDs = (int[])e.Value;
                string text = "";
                if (tagIDs != null)
                {
                    text = DataProvider.Getusers().Where(t => tagIDs.Contains(t.userID)).
                       Select(t => t.userName).DefaultIfEmpty().Aggregate((a, b) => a + ", " + b);
                }
                e.DisplayText = text ?? string.Empty;
            }
        }
        protected void Grid_ParseValue(object sender, ASPxParseValueEventArgs e)
        {
            //if (e.FieldName == "Approvers")
            //{
            //    string valueString = (string)e.Value;
            //    if (!string.IsNullOrEmpty(valueString))
            //    {
            //        string[] parts = valueString.Split(',');
            //        int[] array = new int[parts.Length];
            //        for (int i = 0; i < parts.Length; i++)
            //        {
            //            array[i] = int.Parse(parts[i]);
            //        }
            //        e.Value = array;

            //    }
            //}
            if (e.FieldName == "Salesman")
            {
                string valueString = (string)e.Value;
                if (!string.IsNullOrEmpty(valueString))
                {
                    string[] parts = valueString.Split(',');
                    int[] array = new int[parts.Length];
                    for (int i = 0; i < parts.Length; i++)
                    {
                        array[i] = int.Parse(parts[i]);
                    }
                    e.Value = array;

                }
            }
            else if (e.FieldName == "Manager")
            {
                string valueString = (string)e.Value;
                if (!string.IsNullOrEmpty(valueString))
                {
                    string[] parts = valueString.Split(',');
                    int[] array = new int[parts.Length];
                    for (int i = 0; i < parts.Length; i++)
                    {
                        array[i] = int.Parse(parts[i]);
                    }
                    e.Value = array;

                }
            }
            else if (e.FieldName == "Financer")
            {
                string valueString = (string)e.Value;
                if (!string.IsNullOrEmpty(valueString))
                {
                    string[] parts = valueString.Split(',');
                    int[] array = new int[parts.Length];
                    for (int i = 0; i < parts.Length; i++)
                    {
                        array[i] = int.Parse(parts[i]);
                    }
                    e.Value = array;

                }
            }
        }

        #endregion

        #region private event
        private IEnumerable GetGriddata()
         {

            List<SalesdiscountApprovals> approvallist = new List<SalesdiscountApprovals>();


            DataTable dt = new DataTable();

            dt = oDBEngine.GetDataTable("exec prc_SalesdiscountApproval @actiontype=0");//it is use for action like 0/1/2/3/4 ->select/UPDATE/insert/delete/selectbyid

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SalesdiscountApprovals salesapproval = new SalesdiscountApprovals();
                salesapproval.id = Convert.ToInt32(dt.Rows[i]["Id"]);
                salesapproval.active = Convert.ToBoolean(dt.Rows[i]["Actived"]);
                salesapproval.branched = Convert.ToString(dt.Rows[i]["branch_Id"]);
                salesapproval.Textdata = Convert.ToString(dt.Rows[i]["textdata"]);
                salesapproval.Textdatamanager = Convert.ToString(dt.Rows[i]["textdatamanager"]);
                salesapproval.Textdatafinancer = Convert.ToString(dt.Rows[i]["textdatafinancer"]);
                salesapproval.branch_description = Convert.ToString(dt.Rows[i]["branch_description"]);
                //= Convert.ToString(dt.Rows[i]["Approvers"]);


                //if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Approvers"])))
                //{
                //    int[] array = Convert.ToString(dt.Rows[i]["Approvers"]).Split(',').Select(str => int.Parse(str)).ToArray();

                //    salesapproval.Approvers = array;
                //}
                //else
                //{
                //    salesapproval.Approvers = new int[] { };
                //}

                //--------salesman
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Salesman"])))
                {
                    int[] array = Convert.ToString(dt.Rows[i]["Salesman"]).Split(',').Select(str => int.Parse(str)).ToArray();

                    salesapproval.Salesman = array;
                }
                else
                {
                    salesapproval.Salesman = new int[] { };
                }
                //------------manager-------------------
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Manager"])))
                {
                    int[] array = Convert.ToString(dt.Rows[i]["Manager"]).Split(',').Select(str => int.Parse(str)).ToArray();

                    salesapproval.Manager = array;
                }
                else
                {
                    salesapproval.Manager = new int[] { };
                }
               //--------------------financer----------------------------
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Financer"])))
                {
                    int[] array = Convert.ToString(dt.Rows[i]["Financer"]).Split(',').Select(str => int.Parse(str)).ToArray();

                    salesapproval.Financer = array;
                }
                else
                {
                    salesapproval.Financer = new int[] { };
                }





                approvallist.Add(salesapproval);
            }


            return approvallist;

        }

        public IEnumerable Cmbbranch()
        {
            List<branches> LevelList = new List<branches>();

            DataTable DT = oDBEngine.GetDataTable("select branch_description,branch_id from dbo.tbl_master_branch  order by branch_description");

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                branches Levels = new branches();
                Levels.branchID = Convert.ToString(DT.Rows[i]["branch_id"]);
                Levels.branchName = Convert.ToString(DT.Rows[i]["branch_description"]);
                LevelList.Add(Levels);
            }

            return LevelList;
        }
        public IEnumerable CmbUser()
        {
            List<users> LevelList = new List<users>();

            DataTable DT = oDBEngine.GetDataTable("select user_id ,user_name from tbl_master_user  order by user_name asc");

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                users Levels = new users();
                Levels.userID = Convert.ToInt32(DT.Rows[i]["user_id"]);
                Levels.userName = Convert.ToString(DT.Rows[i]["user_name"]);
                LevelList.Add(Levels);
            }

            return LevelList;
        }
        #endregion

        protected void glCategory_Init(object sender, EventArgs e)
        {
            //int itemindex = ((ASPxGridLookup)sender).NamingContainer as GridViewDataItemTemplateContainer).ItemIndex;
            ASPxGridLookup cb = sender as ASPxGridLookup;
            GridViewDataItemTemplateContainer container = cb.NamingContainer as GridViewDataItemTemplateContainer;

        }

        protected void glManager_Init(object sender, EventArgs e)
        {

        }

   

     
       

        
       

        
    }
    #region class
    public class SalesdiscountApprovals
    {
        public int id { get; set; }
        public bool active { get; set; }

        public string Textdata { get; set; }
        public string Textdatamanager { get; set; }
        public string Textdatafinancer { get; set; }
        public string branched { get; set; }
        //public int[] Approvers { get; set; }
        public int[] Salesman { get; set; }
        public int[] Manager { get; set; }
        public int[] Financer { get; set; }
        public string branch_description { get; set; }
    }

    public class users
    {
        public int userID { get; set; }
        public string userName { get; set; }
    }

    public class branches
    {
        public string branchID { get; set; }
        public string branchName { get; set; }
    }


    #endregion

    public static class DataProvider
    {

        static HttpSessionState Session { get { return HttpContext.Current.Session; } }
        static BusinessLogicLayer.DBEngine oDBEngines = new BusinessLogicLayer.DBEngine(string.Empty);
        public static IList<users> Getusers()
        {

            List<users> LevelList = new List<users>();

            DataTable DT = oDBEngines.GetDataTable("select user_id ,user_name from tbl_master_user  order by user_name asc");

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                users Levels = new users();
                Levels.userID = Convert.ToInt32(DT.Rows[i]["user_id"]);
                Levels.userName = Convert.ToString(DT.Rows[i]["user_name"]);
                LevelList.Add(Levels);
            }

            return LevelList;
        }

    }
}