using System;
using System.Data;
using System.Web;
using System.Configuration;
using BusinessLogicLayer;
using EntityLayer.CommonELS;

namespace ERP.OMS.Management.Master
{
    public partial class Vehicle_Document : ERP.OMS.ViewState_class.VSPage
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/Vehicle.aspx");
            if (!IsPostBack)
            {
                string[,] RegistrationNumber = oDBEngine.GetFieldValue(" tbl_master_vehicle ", "vehicle_regNo", " vehicle_Id='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'", 1);
                if (RegistrationNumber[0, 0] != "n")
                {
                    lblHeader.Text = RegistrationNumber[0, 0].ToUpper();
                }
            }


            BindGrid();
        }

        public void BindGrid()
        {
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataColumn col1 = new DataColumn("Id");
            DataColumn col2 = new DataColumn("Type");
            DataColumn col3 = new DataColumn("FileName");
            DataColumn col4 = new DataColumn("Src");
            DataColumn col5 = new DataColumn("FilePath");
            DataColumn col6 = new DataColumn("doc");
            dt.Columns.Add(col1);
            dt.Columns.Add(col2);
            dt.Columns.Add(col3);
            dt.Columns.Add(col4);
            dt.Columns.Add(col5);
            dt.Columns.Add(col6);
            string keyval = Session["KeyVal_InternalID"].ToString();

            if (keyval != null && keyval != "")
                dt1 = oDBEngine.GetDataTable("tbl_master_document INNER JOIN tbl_master_documentType ON tbl_master_document.doc_documentTypeId = tbl_master_documentType.dty_id", "tbl_master_document.doc_id AS Id, tbl_master_documentType.dty_documentType AS Type,tbl_master_document.doc_documentName AS FileName, tbl_master_document.doc_source AS Src,doc_buildingid,doc_Floor,doc_RoomNo,doc_CellNo,doc_FileNo,tbl_master_document.doc_documentTypeId as doc", "doc_contactId='" + Session["KeyVal_InternalID"] + "'");
            else
            {
                dt1 = null;
                Session["KeyVal"] = "ADD";
            }
            if(dt1 != null)
            {
            if (dt1.Rows.Count != 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    if (dt1.Rows[i][4].ToString() == "0")
                    {
                        DataRow RowNew = dt.NewRow();
                        RowNew["Id"] = dt1.Rows[i][0].ToString();
                        RowNew["Type"] = dt1.Rows[i][1].ToString();
                        RowNew["FileName"] = dt1.Rows[i][2].ToString();
                        RowNew["Src"] = dt1.Rows[i][3].ToString();
                        RowNew["doc"] = dt1.Rows[i][9].ToString();
                        string BName = "N/A";
                        RowNew["FilePath"] = BName;
                        dt.Rows.Add(RowNew);
                    }
                    else
                    {
                        DataRow RowNew = dt.NewRow();
                        RowNew["Id"] = dt1.Rows[i][0].ToString();
                        RowNew["Type"] = dt1.Rows[i][1].ToString();
                        RowNew["FileName"] = dt1.Rows[i][2].ToString();
                        RowNew["Src"] = dt1.Rows[i][3].ToString();
                        RowNew["doc"] = dt1.Rows[i][9].ToString();
                        string BuildingName = "";
                        string[,] bname1 = oDBEngine.GetFieldValue("tbl_master_building", "bui_name", " bui_id='" + dt1.Rows[i][4].ToString() + "'", 1);
                        if (bname1[0, 0] != "n")
                        {
                            BuildingName = bname1[0, 0];
                        }
                        string BName = "Building-  " + BuildingName + " " + "/ Floor No : " + dt1.Rows[i][5].ToString() + " " + "/ Room No-" + dt1.Rows[i][6].ToString() + " " + "/ Cabinet No-" + dt1.Rows[i][7].ToString() + " " + "/ File No-" + dt1.Rows[i][8].ToString();
                        RowNew["FilePath"] = BName;
                        dt.Rows.Add(RowNew);
                    }
                }
            }
        }
            EmployeeDocumentGrid.DataSource = dt.DefaultView;
            EmployeeDocumentGrid.DataBind();
        }

        protected void EmployeeDocumentGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string[] CallVal = e.Parameters.ToString().Split('~');
            if (CallVal[0].ToString() == "Delete")
            {
                oDBEngine.DeleteValue("tbl_master_document", " doc_id='" + CallVal[1].ToString() + "'");
                BindGrid();
            }
        }
        
    }
}