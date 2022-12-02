using System;
using System.Data;
using System.Configuration;

namespace ERP.OMS.Management
{
    public partial class management_UpdateOfferedProduct : System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Data();
                TxtProduct.Attributes.Add("onkeyup", "call_ajax(this,'getProductByLetters',event)");
                TxtCompName.Attributes.Add("onkeyup", "call_ajax1(this,'getCompanyByLetters',event)");
                //if (TxtProduct.Text != Request.QueryString["ProductAmount"].ToString())
                //{
                //    TxtProduct.Attributes.Add("onkeyup", "call_ajax(this,'getProductByLetters',event)");
                //    TxtCompName.Attributes.Add("onkeyup", "call_ajax1(this,'getCompanyByLetters',event)");
                //}
                //else if (TxtProduct.Text == Request.QueryString["ProductAmount"].ToString())
                //{
                //    TxtProduct_hidden.Text = Request.QueryString["prod"].ToString();
                //    TxtCompName_hidden.Text = Request.QueryString["CompID"].ToString();
                //}

            }
        }
        public void Data()
        {

            DDLProductype.SelectedItem.Text = Request.QueryString["ProductType"].ToString();
            TxtAmount.Text = Request.QueryString["ProductAmount"].ToString();
            TxtProduct.Text = Request.QueryString["prod"].ToString();
            TxtCompName.Text = Request.QueryString["Comp"].ToString();
            TxtCompName_hidden.Text = Request.QueryString["CompID"].ToString();
            TxtProduct_hidden.Text = Request.QueryString["PID"].ToString();
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            string id = Request.QueryString["id"].ToString();
            DataTable dt = new DataTable();
            string Product_Id = "";
            string CreateDate = oDBEngine.GetDate().ToShortDateString();
            string CreateUser = Session["userid"].ToString();
            //string ProductType = DDLProductype.SelectedItem.Value;
            string ProductType = Request.QueryString["ProductType"].ToString();
            Product_Id = TxtProduct_hidden.Text;
            if (Product_Id == "")
            {
                TxtCompName_hidden.Text = Request.QueryString["CompID"].ToString();
                Product_Id = Request.QueryString["PID"].ToString();

            }
            if (Product_Id != "")
            {
                try
                {
                    string TxtAmount1 = "";
                    if (TxtAmount.Text != "")
                    {
                        int.Parse(TxtAmount.Text);
                        TxtAmount1 = TxtAmount.Text;
                    }
                    else
                    {
                        TxtAmount1 = "0";
                    }
                    switch (ProductType)
                    {
                        case "Mutual Fund":
                            Product_Id = TxtProduct_hidden.Text;
                            dt = oDBEngine.GetDataTable("tbl_master_products", "top 1 prds_internalId as ID", " prds_productType = 'MF' and prds_description='" + TxtProduct.Text + "'");
                            oDBEngine.SetFieldValue("tbl_trans_offeredProduct", "ofp_productTypeId='Mutual Fund',ofp_probableAmount='" + TxtAmount.Text + "',ofp_productId='" + Product_Id + "'", " ofp_id=" + id);
                            break;
                        case "Insurance-Life":
                            Product_Id = TxtProduct_hidden.Text;
                            //if (Product_Id != "")
                            //{
                            dt = oDBEngine.GetDataTable("tbl_master_products", "top 1 prds_internalId as ID", " prds_productType = 'IN' and prds_description='" + TxtProduct.Text + "'");
                            oDBEngine.SetFieldValue("tbl_trans_offeredProduct", "ofp_productTypeId='Insurance-Life',ofp_probableAmount='" + TxtAmount.Text + "',ofp_productId='" + Product_Id + "'", " ofp_id=" + id);


                            //}
                            //else
                            //lblError.Text = "Please select a product from list.";
                            //lblError.Visible = true;

                            break;
                        case "Insurance-General":
                            Product_Id = TxtProduct_hidden.Text;
                            dt = oDBEngine.GetDataTable("tbl_master_products", "top 1 prds_internalId as ID", " prds_productType = 'IG' and prds_description='" + TxtProduct.Text + "'");
                            oDBEngine.SetFieldValue("tbl_trans_offeredProduct", "ofp_productTypeId='Insurance-Life',ofp_probableAmount='" + TxtAmount.Text + "',ofp_productId='" + Product_Id + "'", " ofp_id=" + id);
                            break;
                        case "HLO":
                            Product_Id = TxtProduct_hidden.Text;
                            dt = oDBEngine.GetDataTable("tbl_master_CFProducts", "top 1 cf_pcode as ID", " cf_ptype = 'Housing Loan' and cf_pname='" + TxtProduct.Text + "'");
                            oDBEngine.SetFieldValue("tbl_trans_offeredProduct", "ofp_productTypeId='Housing Loan',ofp_probableAmount='" + TxtAmount.Text + "',ofp_productId='" + Product_Id + "'", " ofp_id=" + id);
                            break;
                        case "LAP":
                            Product_Id = TxtProduct_hidden.Text;
                            dt = oDBEngine.GetDataTable("tbl_master_CFProducts", "top 1 cf_pcode as ID", " cf_ptype = 'Loan Against Property' and cf_pname='" + TxtProduct.Text + "'");
                            oDBEngine.SetFieldValue("tbl_trans_offeredProduct", "ofp_productTypeId='Loan Against Property',ofp_probableAmount='" + TxtAmount.Text + "',ofp_productId='" + Product_Id + "'", " ofp_id=" + id);
                            break;
                        case "PLO":
                            Product_Id = TxtProduct_hidden.Text;
                            dt = oDBEngine.GetDataTable("tbl_master_CFProducts", "top 1 cf_pcode as ID", " cf_ptype = 'Personal Loan' and cf_pname='" + TxtProduct.Text + "'");
                            oDBEngine.SetFieldValue("tbl_trans_offeredProduct", "ofp_productTypeId='Personal Loan',ofp_probableAmount='" + TxtAmount.Text + "',ofp_productId='" + Product_Id + "'", " ofp_id=" + id);
                            break;
                        case "TLO":
                            Product_Id = TxtProduct_hidden.Text;
                            dt = oDBEngine.GetDataTable("tbl_master_CFProducts", "top 1 cf_pcode as ID", " cf_ptype = 'Travel Loan' and cf_pname='" + TxtProduct.Text + "'");
                            oDBEngine.SetFieldValue("tbl_trans_offeredProduct", "ofp_productTypeId='Travel Loan',ofp_probableAmount='" + TxtAmount.Text + "',ofp_productId='" + Product_Id + "'", " ofp_id=" + id);
                            break;
                        case "BLO":
                            Product_Id = TxtProduct_hidden.Text;
                            dt = oDBEngine.GetDataTable("tbl_master_CFProducts", "top 1 cf_pcode as ID", " cf_ptype = 'Business Loan' and cf_pname='" + TxtProduct.Text + "'");
                            oDBEngine.SetFieldValue("tbl_trans_offeredProduct", "ofp_productTypeId='Business Loan',ofp_probableAmount='" + TxtAmount.Text + "',ofp_productId='" + Product_Id + "'", " ofp_id=" + id);
                            break;
                        case "ELO":
                            Product_Id = TxtProduct_hidden.Text;
                            dt = oDBEngine.GetDataTable("tbl_master_CFProducts", "top 1 cf_pcode as ID", " cf_ptype = 'Education Loan' and cf_pname='" + TxtProduct.Text + "'");
                            oDBEngine.SetFieldValue("tbl_trans_offeredProduct", "ofp_productTypeId='Education Loan',ofp_probableAmount='" + TxtAmount.Text + "',ofp_productId='" + Product_Id + "'", " ofp_id=" + id);
                            break;
                        case "ALO":
                            Product_Id = TxtProduct_hidden.Text;
                            dt = oDBEngine.GetDataTable("tbl_master_CFProducts", "top 1 cf_pcode as ID", " cf_ptype = 'Auto Loan' and cf_pname='" + TxtProduct.Text + "'");
                            oDBEngine.SetFieldValue("tbl_trans_offeredProduct", "ofp_productTypeId='Auto Loan',ofp_probableAmount='" + TxtAmount.Text + "',ofp_productId='" + Product_Id + "'", " ofp_id=" + id);
                            break;
                        case "SLO":
                            Product_Id = TxtProduct_hidden.Text;
                            dt = oDBEngine.GetDataTable("tbl_master_CFProducts", "top 1 cf_pcode as ID", " cf_ptype = 'SME Loan' and cf_pname='" + TxtProduct.Text + "'");
                            oDBEngine.SetFieldValue("tbl_trans_offeredProduct", "ofp_productTypeId='SME Loan',ofp_probableAmount='" + TxtAmount.Text + "',ofp_productId='" + Product_Id + "'", " ofp_id=" + id);
                            break;
                        case "LAS":
                            Product_Id = TxtProduct_hidden.Text;
                            dt = oDBEngine.GetDataTable("tbl_master_CFProducts", "top 1 cf_pcode as ID", " cf_ptype = 'Loan Against Securities' and cf_pname='" + TxtProduct.Text + "'");
                            oDBEngine.SetFieldValue("tbl_trans_offeredProduct", "ofp_productTypeId='Loan Against Securities',ofp_probableAmount='" + TxtAmount.Text + "',ofp_productId='" + Product_Id + "'", " ofp_id=" + id);
                            break;
                        case "CRD":
                            Product_Id = TxtProduct_hidden.Text;
                            dt = oDBEngine.GetDataTable("tbl_master_CFProducts", "top 1 cf_pcode as ID", " cf_ptype = 'Credit Cards' and cf_pname='" + TxtProduct.Text + "'");
                            oDBEngine.SetFieldValue("tbl_trans_offeredProduct", "ofp_productTypeId='Credit Cards',ofp_probableAmount='" + TxtAmount.Text + "',ofp_productId='" + Product_Id + "'", " ofp_id=" + id);
                            break;
                        default:
                            oDBEngine.SetFieldValue("tbl_trans_offeredProduct", "ofp_productTypeId='" + DDLProductype.SelectedItem.Value + "',ofp_probableAmount='" + TxtAmount.Text + "'", " ofp_id=" + id);
                            break;
                    }
                    string popupScript = "";
                    popupScript = "<script language='javascript'>" + "parent.editwin.close();</script>";
                    ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
                }
                catch (Exception ex)
                {
                    lblError.Text = "Amount must be numeric.";
                    lblError.Visible = true;
                    //string n = ex.Message;
                }
            }
            else
            {
                lblError1.Text = "Please select a product from list.";
                lblError1.Visible = true;
            }

        }
        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            string popupScript = "";
            popupScript = "<script language='javascript'>" + "window.opener.location.href=window.opener.location.href;window.close();</script>";
            ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
        }
    }
}