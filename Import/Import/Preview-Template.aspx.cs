using DataAccessLayer;
using ImportModuleBusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Import.Import
{

    public partial class Preview_Template : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["T"] != null)
                {

                    GetPreviewDetails(Request.QueryString["T"]);
                }

            }
        }


        #region Purchase Order Mail
        public void GetPreviewDetails(string Keyvalue)
        {
            int stat = 0;


            Templatesettings objtemplatebal = new Templatesettings();

            DataTable dt_EmailConfig = new DataTable();
            DataTable dt_EmailConfigpurchase = new DataTable();

            TemplateDocumentsTags fetchModel = new TemplateDocumentsTags();

            DataTable dt_Emailbodysubject = new DataTable();



            string Subject = "";
            string Body = "";
            string emailTo = "";
            int MailStatus = 0;



            dt_Emailbodysubject = objtemplatebal.GetTemplates(Keyvalue);
            if (dt_Emailbodysubject.Rows.Count > 0)
            {
                Body = Convert.ToString(dt_Emailbodysubject.Rows[0]["body"]);
                Subject = Convert.ToString(dt_Emailbodysubject.Rows[0]["subjct"]);

                dt_EmailConfigpurchase = objtemplatebal.GetTemplatetags(Keyvalue, "DefaultTemplateTags");


                if (dt_EmailConfigpurchase.Rows.Count > 0)
                {

                    fetchModel = DbHelpers.ToModel<TemplateDocumentsTags>(dt_EmailConfigpurchase);
                    Body = Templatesettings.GetFormattedString<TemplateDocumentsTags>(fetchModel, Body);
                    Subject = Templatesettings.GetFormattedString<TemplateDocumentsTags>(fetchModel, Subject);

                }

                lblheading.InnerHtml = Subject;
                lblbody.InnerHtml = Body;

            }

        }
       #endregion


    }
     


          

    #region  ModelClass

    public class TemplateDocumentsTags
    {

        public string Name { get; set; }
        public string Email { get; set; }
        public string Date { get; set; }
        public string Signature { get; set; }
        public string Finyear { get; set; }
        public string Company { get; set; }

        public string CompanyregistrationNo { get; set; }
        public string ServiceTax { get; set; }
        public string PAN { get; set; }
        public string CIN { get; set; }
        public string VatNo { get; set; }

        public string NatureBusiness { get; set; }
        public string B_Address1 { get; set; }
        public string B_Address2 { get; set; }
        public string B_Address3 { get; set; }
        public string B_landmark { get; set; }

        public string S_Address1 { get; set; }
        public string S_Address2 { get; set; }
        public string S_Address3 { get; set; }
        public string S_landmark { get; set; }
        public string O_Address1 { get; set; }
        public string O_Address2 { get; set; }
        public string O_Address3 { get; set; }
        public string O_landmark { get; set; }

    }

    #endregion

}
