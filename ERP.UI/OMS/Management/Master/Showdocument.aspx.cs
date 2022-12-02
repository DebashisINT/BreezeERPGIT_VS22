using System;
using System.Configuration;
using System.Data;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;


namespace ERP.OMS.Management.Master
{
    public partial class management_master_Showdocument : ERP.OMS.ViewState_class.VSPage
    {
        string[,] _id;
        string finalId = "";
        public string pageAccess = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        clsDropDownList oclsDropDownList = new clsDropDownList();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fillDropDownlist();
            }
            pnlDocuments.Visible = false;
            txtName.Attributes.Add("onkeyup", "CallList(this,'searchdocument',event)");
        }
        #region Second dropdown Fill
        protected void fillDropDownlist()
        {
            drpDocumentType.Items.Clear();
            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
            string[,] Data = objEngine.GetFieldValue(" tbl_master_documentType", " dty_id,dty_documentType ", " dty_applicableFor='" + drpDocumentEntity.SelectedValue.ToString() + "'", 2, "dty_documentType");
            if (Data[0, 0] != "n")
                //objEngine.AddDataToDropDownList(Data, drpDocumentType);
                oclsDropDownList.AddDataToDropDownList(Data, drpDocumentType);

        }
        #endregion

        #region Passing selected value of First dropdpwn
        protected void drpDocumentEntity_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillDropDownlist();
        }
        #endregion

        #region Search button Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {

            if (txtName.Text != "")
            {
                //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
                switch (drpDocumentEntity.SelectedItem.Text)
                {
                    case "Products MF":
                        _id = objEngine.GetFieldValue("tbl_master_products", "prds_internalid", "prds_productType=12 and prds_description='" + txtName.Text + "'", 1);
                        finalId = _id[0, 0].ToString();
                        break;
                    case "Products Insurance":
                        _id = objEngine.GetFieldValue("tbl_master_products", "prds_internalid", "prds_productType=13 and prds_description='" + txtName.Text + "'", 1);
                        finalId = _id[0, 0].ToString();
                        break;
                    case "Products IPOs":
                        _id = objEngine.GetFieldValue("tbl_master_products", "prds_internalid", "prds_productType=15 and prds_description='" + txtName.Text + "'", 1);
                        finalId = _id[0, 0].ToString();
                        break;
                    case "Customer/Client":
                        _id = objEngine.GetFieldValue("tbl_master_contact", "cnt_internalid", "cnt_contactType=1 and cnt_firstName='" + txtName.Text + "'", 1);
                        finalId = _id[0, 0].ToString();
                        break;
                    case "Lead":
                        _id = objEngine.GetFieldValue("tbl_master_lead", "cnt_internalid", "cnt_firstName='" + txtName.Text + "'", 1);
                        finalId = _id[0, 0].ToString();
                        break;
                    case "Employee":
                        _id = objEngine.GetFieldValue("tbl_master_contact", "cnt_internalid", "cnt_contactType='EM' and cnt_firstName='" + txtName.Text + "'", 1);
                        finalId = _id[0, 0].ToString();
                        break;
                    case "Sub Brokers":
                        _id = objEngine.GetFieldValue("tbl_master_contact", "cnt_internalid", "cnt_contactType=2 and cnt_firstName='" + txtName.Text + "'", 1);
                        finalId = _id[0, 0].ToString();
                        break;
                    case "Franchisees":
                        _id = objEngine.GetFieldValue("tbl_master_contact", "cnt_internalid", "cnt_contactType=4 and cnt_firstName='" + txtName.Text + "'", 1);
                        finalId = _id[0, 0].ToString();
                        break;
                    case "Data Vendors":
                        _id = objEngine.GetFieldValue("tbl_master_contact", "cnt_internalid", "cnt_contactType=7 and cnt_firstName='" + txtName.Text + "'", 1);
                        finalId = _id[0, 0].ToString();
                        break;
                    case "Referral Agents":
                        _id = objEngine.GetFieldValue("tbl_master_contact", "cnt_internalid", "cnt_contactType=8 and cnt_firstName='" + txtName.Text + "'", 1);
                        finalId = _id[0, 0].ToString();
                        break;
                    case "Recruitment Agents":
                        _id = objEngine.GetFieldValue("tbl_master_contact", "cnt_internalid", "cnt_contactType=9 and cnt_firstName='" + txtName.Text + "'", 1);
                        finalId = _id[0, 0].ToString();
                        break;
                    case "AMCs":
                        _id = objEngine.GetFieldValue("tbl_master_AssetsManagementCompanies", "amc_amcCode", "amc_nameOfMutualFund='" + txtName.Text + "'", 1);
                        finalId = _id[0, 0].ToString();
                        break;
                    case "Insurance Companies":
                        _id = objEngine.GetFieldValue("tbl_master_insurerName", "insu_internalid", "insu_nameOfCompany='" + txtName.Text + "'", 1);
                        finalId = _id[0, 0].ToString();
                        break;
                    case "RTAs":
                        _id = objEngine.GetFieldValue("tbl_registrarTransferAgent", "rta_rtaCode", "rta_name='" + txtName.Text + "'", 1);
                        finalId = _id[0, 0].ToString();
                        break;
                    case "Branches":
                        _id = objEngine.GetFieldValue("tbl_master_branch", "branch_internalid", "branch_description='" + txtName.Text + "'", 1);
                        finalId = _id[0, 0].ToString();
                        break;
                    case "Companies":
                        _id = objEngine.GetFieldValue("tbl_master_company", "cmp_internalId", "cmp_Name='" + txtName.Text + "'", 1);
                        finalId = _id[0, 0].ToString();
                        break;
                    default:
                        finalId = "";
                        break;

                }

            }
            if (finalId != "" && drpDocumentType.SelectedValue != "")
            {
                //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
                DataSet dsDocument = new DataSet();
                dsDocument = objEngine.PopulateData("tbl_master_document.doc_id AS Id, tbl_master_documentType.dty_documentType AS Type, tbl_master_document.doc_documentName AS FileName, tbl_master_document.doc_source AS Src,COALESCE ('Building- ' + tbl_master_building.bui_Name + CHAR(13) + CHAR(10), '') + COALESCE ('/ Floor No : ' + tbl_master_document.doc_Floor + CHAR(13) + CHAR(10), '')  + '/ Room No-' + tbl_master_document.doc_RoomNo + '/ Cabinet No-' + tbl_master_document.doc_CellNo + '/ File No-' + tbl_master_document.doc_FileNo AS FilePath ", "tbl_master_document,tbl_master_documentType,tbl_master_building", "(tbl_master_document.doc_contactId = '" + finalId + "') AND (tbl_master_document.doc_documentTypeId = '" + drpDocumentType.SelectedValue.ToString() + "')AND (tbl_master_document.doc_documentTypeId = tbl_master_documentType.dty_id) AND (tbl_master_document.doc_buildingId = tbl_master_building.bui_id)");
                if (dsDocument.Tables["TableName"].Rows.Count > 0)
                {
                    pnlDocuments.Visible = true;
                    grdDocuments.DataSource = dsDocument.Tables["TableName"];
                    grdDocuments.DataBind();
                }
                else
                {
                    lblError.Text = "There is No Document Found";
                }

            }
            else if (chkSearch.Checked == true && drpDocumentType.SelectedValue != "")
            {
                //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
                DataSet dsDocument = new DataSet();
                dsDocument = objEngine.PopulateData("tbl_master_document.doc_id AS Id, tbl_master_documentType.dty_documentType AS Type, tbl_master_document.doc_documentName AS FileName, tbl_master_document.doc_source AS Src,COALESCE ('Building- ' + tbl_master_building.bui_Name + CHAR(13) + CHAR(10), '') + COALESCE ('/ Floor No : ' + tbl_master_document.doc_Floor + CHAR(13) + CHAR(10), '')  + '/ Room No-' + tbl_master_document.doc_RoomNo + '/ Cabinet No-' + tbl_master_document.doc_CellNo + '/ File No-' + tbl_master_document.doc_FileNo AS FilePath ", "tbl_master_document,tbl_master_documentType,tbl_master_building", "(tbl_master_document.doc_documentName = '" + txtName.Text + "') AND (tbl_master_document.doc_documentTypeId = '" + drpDocumentType.SelectedValue.ToString() + "')AND (tbl_master_document.doc_documentTypeId = tbl_master_documentType.dty_id) AND (tbl_master_document.doc_buildingId = tbl_master_building.bui_id)");
                if (dsDocument.Tables["TableName"].Rows.Count > 0)
                {
                    pnlDocuments.Visible = true;
                    grdDocuments.DataSource = dsDocument.Tables["TableName"];
                    grdDocuments.DataBind();
                }
                else
                {
                    lblError.Text = "There is No Document Found";
                }

            }
            else
            {
                lblError.Text = "First Choose the File";
            }
            this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL1", "<script>height();</script>");
        }
        #endregion
    }
}