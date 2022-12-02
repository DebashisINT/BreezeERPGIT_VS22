using System;
using System.Data;
using System.Configuration;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Management
{
    public partial class management_frmAddProductBrochures : System.Web.UI.Page
    {
        clsDropDownList cls = new clsDropDownList();
        //DBEngine odbEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        DBEngine odbEngine = new DBEngine();
        string[,] id;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                FillDocumentType();
                txtName.Attributes.Add("onkeyup", "call_ajax(this,'searchdocument1',event)");
            }
        }
        public void FillDocumentType()
        {
            string[,] DType = odbEngine.GetFieldValue("tbl_master_documentType", "dty_id as Id, dty_documentType as Name", " dty_applicableFor='" + drpDocumentEntity.SelectedItem.Text + "'", 2, "dty_documentType");
            if (DType[0, 0] != "n")
            {
                cls.AddDataToDropDownList(DType, drpDocumentType);
            }
        }
        protected void drpDocumentEntity_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDocumentType();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ShowButton.Visible = false;
            if (txtName.Text != "")
            {

                switch (drpDocumentEntity.SelectedItem.Text)
                {
                    case "Products MF":
                        id = odbEngine.GetFieldValue("tbl_master_products", "prds_internalid", " prds_productType='MF' and prds_description='" + txtName.Text + "'", 1);
                        break;
                    case "Products Insurance":
                        id = odbEngine.GetFieldValue("tbl_master_products", "prds_internalid", " prds_productType=13 and prds_description='" + txtName.Text + "'", 1);
                        break;
                    case "Products IPOs":
                        id = odbEngine.GetFieldValue("tbl_master_products", "prds_internalid", " prds_productType=15 and prds_description='" + txtName.Text + "'", 1);
                        break;
                    case "Customer":
                        id = odbEngine.GetFieldValue("tbl_master_contact", "cnt_internalid", " cnt_contactType=1 and cnt_firstName='" + txtName.Text + "'", 1);
                        break;
                    case "Lead":
                        id = odbEngine.GetFieldValue("tbl_master_lead", "cnt_internalid", " cnt_firstName='" + txtName.Text + "'", 1);
                        break;
                    case "Employee":
                        id = odbEngine.GetFieldValue("tbl_master_contact", "cnt_internalid", " cnt_contactType=3 and cnt_firstName='" + txtName.Text + "'", 1);
                        break;
                    case "Sub Brokers":
                        id = odbEngine.GetFieldValue("tbl_master_contact", "cnt_internalid", " cnt_contactType=2 and cnt_firstName='" + txtName.Text + "'", 1);
                        break;
                    case "Franchisees":
                        id = odbEngine.GetFieldValue("tbl_master_contact", "cnt_internalid", " cnt_contactType=4 and cnt_firstName='" + txtName.Text + "'", 1);
                        break;
                    case "Data Vendors":
                        id = odbEngine.GetFieldValue("tbl_master_contact", "cnt_internalid", " cnt_contactType=7 and cnt_firstName='" + txtName.Text + "'", 1);
                        break;
                    case "Referral Agents":
                        id = odbEngine.GetFieldValue("tbl_master_contact", "cnt_internalid", " cnt_contactType=8 and cnt_firstName='" + txtName.Text + "'", 1);
                        break;
                    case "Recruitment Agents":
                        id = odbEngine.GetFieldValue("tbl_master_contact", "cnt_internalid", " cnt_contactType=9 and cnt_firstName='" + txtName.Text + "'", 1);
                        break;
                    case "AMCs":
                        id = odbEngine.GetFieldValue("tbl_master_AssetsManagementCompanies", "amc_amcCode", " amc_nameOfMutualFund='" + txtName.Text + "'", 1);
                        break;
                    case "Insurance Companies":
                        id = odbEngine.GetFieldValue("tbl_master_insurerName", "insu_internalid", " insu_nameOfCompany='" + txtName.Text + "'", 1);
                        break;
                    case "RTAs":
                        id = odbEngine.GetFieldValue("tbl_registrarTransferAgent", "rta_rtaCode", " rta_name='" + txtName.Text + "'", 1);
                        break;
                    case "Branches":
                        id = odbEngine.GetFieldValue("tbl_master_branch", "branch_internalid", " branch_description='" + txtName.Text + "'", 1);
                        break;
                    case "Companies":
                        id = odbEngine.GetFieldValue("tbl_master_company", "cmp_internalId", " cmp_Name='" + txtName.Text + "'", 1);
                        break;
                    case "Building":
                        id = odbEngine.GetFieldValue("tbl_master_building", "bui_id", " bui_Name='" + txtName.Text + "'", 1);
                        break;
                }
                if (id[0, 0] != "n")
                {
                    DataTable dt = new DataTable();
                    dt = odbEngine.GetDataTable("tbl_master_document INNER JOIN tbl_master_documentType ON tbl_master_document.doc_documentTypeId = tbl_master_documentType.dty_id INNER JOIN tbl_master_building ON tbl_master_document.doc_buildingId = tbl_master_building.bui_id", "tbl_master_document.doc_id AS Id, tbl_master_documentType.dty_documentType AS Type, tbl_master_document.doc_documentName AS FileName, tbl_master_document.doc_source AS Src, COALESCE ('Building- ' + tbl_master_building.bui_Name + CHAR(13) + CHAR(10), '') + COALESCE ('/ Floor No : ' + tbl_master_document.doc_Floor + CHAR(13) + CHAR(10), '')  + '/ Room No-' + tbl_master_document.doc_RoomNo + '/ Cabinet No-' + tbl_master_document.doc_CellNo + '/ File No-' + tbl_master_document.doc_FileNo AS FilePath", " (doc_contactId = '" + id[0, 0].ToString() + "') AND (doc_documentTypeId = " + drpDocumentType.SelectedItem.Value + ")");
                    if (dt.Rows.Count != 0)
                    {
                        grdDocuments.DataSource = dt.DefaultView;
                        grdDocuments.DataBind();
                        ShowButton.Visible = true;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i]["src"].ToString() == "")
                            {
                                grdDocuments.Rows[i].Cells[4].Controls.Clear();
                                grdDocuments.Rows[i].Cells[4].Text = "N/A";
                            }
                        }
                    }
                    else
                    {
                        ShowButton.Visible = false;
                        ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('There is No Document Found')</script>");
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('There is No Product')</script>");
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('First Choose the File Name')</script>");
            }
        }
    }
}