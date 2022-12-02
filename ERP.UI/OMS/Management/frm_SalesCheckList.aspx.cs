using System;
using System.Configuration;

namespace ERP.OMS.Management
{
    public partial class management_frm_SalesCheckList : System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string id = "";
            id = Request.QueryString["leadid"].ToString();
            string FieldName = "";
            if (Session["LastId"] == null)
            {
                string branch_id = oDBEngine.GetFieldValue("tbl_master_lead", "cnt_branchid", " cnt_internalid='" + id + "'", 1)[0, 0];
                FieldName = "sad_salesid,sad_cntid,sad_branch,sad_productapplicationform,sad_photoidproof,sad_addressproof,sad_ageproof,sad_signatureproof,sad_kycdocument,sad_tripartiteagreement,sad_poaagreement,sad_medicalreports";
                int NoOfRowsEffected = oDBEngine.InsurtFieldValue("tbl_trans_salesDetails", FieldName, " '" + Request.QueryString["Salesid"].ToString() + "','" + id + "','" + branch_id.ToString() + "','" + drpProductApplicationForm.SelectedItem.Text + "','" + drpPhotoIdProof.SelectedItem.Text + "','" + drpAddressProof.SelectedItem.Text + "','" + drpAgeProof.SelectedItem.Text + "','" + drpSignatureProof.SelectedItem.Text + "','" + drpKYCDocument.SelectedItem.Text + "','" + drpTripartiteAgreement.SelectedItem.Text + "','" + drpPOAAgreement.SelectedItem.Text + "','" + drpMedicalReports.SelectedItem.Text + "'");
            }
            else
            {
                oDBEngine.SetFieldValue("tbl_trans_salesDetails", "sad_salesid='" + Request.QueryString["Salesid"].ToString() + "',sad_cntid='" + id + "',sad_productapplicationform='" + drpProductApplicationForm.SelectedItem.Text + "',sad_photoidproof='" + drpPhotoIdProof.SelectedItem.Text + "',sad_addressproof='" + drpAddressProof.SelectedItem.Text + "',sad_ageproof='" + drpAgeProof.SelectedItem.Text + "',sad_signatureproof='" + drpSignatureProof.SelectedItem.Text + "',sad_kycdocument='" + drpKYCDocument.SelectedItem.Text + "',sad_tripartiteagreement='" + drpTripartiteAgreement.SelectedItem.Text + "',sad_poaagreement='" + drpPOAAgreement.SelectedItem.Text + "'sad_medicalreports='" + drpMedicalReports.SelectedItem.Text + "'", "sad_cntid='" + id + "' order by sad_id desc");
            }
        }
    }
}