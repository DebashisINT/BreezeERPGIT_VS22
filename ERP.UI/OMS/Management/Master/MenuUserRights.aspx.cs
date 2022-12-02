using BusinessLogicLayer;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class MenuUserRights : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["exportval"] = null;
                HdGroupId.Value = Request.QueryString["UserGroup"];
                GridRights.DataBind();


                ProcedureExecute proc = new ProcedureExecute("Prc_UserRightsAddEdit");
                proc.AddVarcharPara("@Action", 100, "GroupName");
                proc.AddVarcharPara("@GroupId", 100, HdGroupId.Value);
                DataTable dt = proc.GetTable();
                lblmodName.Text = Convert.ToString(dt.Rows[0][0]);
            }
        }

        protected void GridRights_DataBinding(object sender, EventArgs e)
        {
            MasterSettings masterbl = new MasterSettings();
            string mastersettings = masterbl.GetSettings("isServiceManagementRequred");

            ProcedureExecute proc = new ProcedureExecute("Prc_UserRightsAddEdit");
            proc.AddVarcharPara("@Action", 100, "FetchData");
            proc.AddVarcharPara("@GroupId", 100, HdGroupId.Value);
            proc.AddPara("@showService", mastersettings);
            DataTable dt = proc.GetTable();

            foreach (DataRow dr in dt.Rows)
            {
                if (Convert.ToString(dr["hasRights"]).Trim() == "")
                    dr["hasRights"] = "No Right(s) Found.";
                else
                    dr["hasRights"] = Convert.ToString(dr["hasRights"]).TrimEnd(',');
            }

            GridRights.DataSource = dt;
            
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


        public void bindexport(int Filter)
        {
            GridRights.Columns[4].Visible = false;
            string filename = "User Rights";
            exporter.FileName = filename;
            exporter.FileName = "UserRights";
            drdExport.SelectedValue = "0";
            exporter.PageHeader.Left = "User Rights";
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

        [WebMethod]
        public static object GetMenuRightsControl(string MenuId, string GroupId)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_UserRightsAddEdit");
            proc.AddVarcharPara("@Action", 100, "GetRightsControl");
            proc.AddVarcharPara("@Id", 100, MenuId);
            proc.AddVarcharPara("@GroupId", 100, GroupId);
            DataSet dt = proc.GetDataSet();


            List<RightsName> rightsNames = new List<RightsName>();
            rightsNames = (from DataRow dr in dt.Tables[0].Rows
                           select new RightsName()
                           {
                               Id = dr["id"].ToString(),
                               Name = dr["Rights"].ToString()
                           }).ToList();
            List<string> hasRights = new List<string>();

            foreach (DataRow dr in dt.Tables[1].Rows)
                hasRights.Add(Convert.ToString(dr["Rights"]));

            OutputResult outputResult = new OutputResult();
            outputResult.rightsName = rightsNames;
            outputResult.hasSelect = hasRights;


            return outputResult;
        }



        [WebMethod]
        public static string SaveRights(string FieldList, string GroupId, string MenuId)
        {
            try
            {
                ProcedureExecute proc = new ProcedureExecute("Prc_UserRightsAddEdit");
                proc.AddVarcharPara("@Action", 100, "SaveRights");
                proc.AddVarcharPara("@Id", 100, MenuId);
                proc.AddVarcharPara("@GroupId", 100, GroupId);
                proc.AddPara("@FieldId", FieldList);
                proc.RunActionQuery();
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
            return "Ok";
        }


        [WebMethod]
        public static string TaggedAll(string GroupId)
        {
            try
            {
                ProcedureExecute proc = new ProcedureExecute("Prc_UserRightsAddEdit");
                proc.AddVarcharPara("@Action", 100, "TaggedAll");
                proc.AddVarcharPara("@GroupId", 100, GroupId);
                proc.RunActionQuery();
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
            return "Ok";

        }

        [WebMethod]
        public static string UnTaggedAll(string GroupId)
        {
            try
            {
                ProcedureExecute proc = new ProcedureExecute("Prc_UserRightsAddEdit");
                proc.AddVarcharPara("@Action", 100, "UnTaggedAll");
                proc.AddVarcharPara("@GroupId", 100, GroupId);
                proc.RunActionQuery();
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
            return "Ok";

        }
        public class OutputResult
        {
            public List<RightsName> rightsName { get; set; }
            public List<string> hasSelect { get; set; }

        }

        public class RightsName
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }


    }
}