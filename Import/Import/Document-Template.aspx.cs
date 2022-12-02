using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Web.Services;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using BusinessLogicLayer.EmailTemplate;
using Newtonsoft.Json;
using DataAccessLayer;
using System.Web.Script.Serialization;
using ImportModuleBusinessLayer;

namespace Import.Import
{
    public partial class Document_Template : System.Web.UI.Page
    {


        Documenttemplate objemployeebal = new Documenttemplate();

        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public EntityLayer.CommonELS.UserRightsForPage rights = new EntityLayer.CommonELS.UserRightsForPage();
        string data = "";
        public string pageAccess = "";

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Document-Template.aspx");

            if (!IsPostBack)
            {
                Session["KeyVal"] = null;
                //txt_ajax.Attributes.Add("onkeyup", "ajax_showOptions(this,'userdetails',event,'drpProducttype')");
              //  BindGrid();
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                drp_templatetype.Attributes.Add("onchange", "drpChange()");
                //BindDiv();
            }
        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static object FetchEmailTagsByStage(string Id)
        {
            DataTable dt1 = new DataTable();
            Documenttemplate obj = new Documenttemplate();

            List<ImportTags> emailTags = new List<ImportTags>();
            try
            {
                dt1 = Documenttemplate.GetEmailTags("1");

                emailTags = DbHelpers.ToModelList<ImportTags>(dt1);

                if (emailTags != null && emailTags.Count > 0)
                {
                    var serializer = new JavaScriptSerializer(); var serializedResult = serializer.Serialize(emailTags);
                }

                return JsonConvert.SerializeObject(emailTags);
            }
            catch (Exception ex)
            {
                return new { status = "Ok1" };
            }
        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static string Getupdatedata(string Id)
        {
            DataTable dt1 = new DataTable();
            Documenttemplate obj = new Documenttemplate();

            List<ImportTags> emailTags = new List<ImportTags>();
            try
            {
                dt1 = Documenttemplate.GetEmailTags(Id);

                emailTags = DbHelpers.ToModelList<ImportTags>(dt1);
                if (emailTags != null && emailTags.Count > 0)
                {
                    var serializer = new JavaScriptSerializer(); var serializedResult = serializer.Serialize(emailTags);
                }

                return JsonConvert.SerializeObject(emailTags);

            }
            catch (Exception ex)
            {
                return "Error occured";
            }
        }

        public void BindGrid()
        {

            DataTable dt = new DataTable();

            #region
            try
            {
                dt = objemployeebal.GetImportDetails();

                if (dt.Rows.Count > 0)
                {
                    GrdTemplate.DataSource = dt;
                    GrdTemplate.DataBind();

                }
                else
                {
                    GrdTemplate.DataSource = null;
                    GrdTemplate.DataBind();

                }
            }
            catch (Exception ex)
            {

            }

            #endregion
        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static object AddEmailTemplateInfo(string data, string templatename, string bodyhtml, string remrks, string type, Boolean Defaultcheck, string Id = null)
        {
            bool isUpdated = false;
            Documenttemplate objemployeebal = new Documenttemplate();

            DataTable dt = new DataTable();

            if (string.IsNullOrEmpty(Id))
            {
                dt = objemployeebal.DocumenttemplateManage(data, templatename, bodyhtml, remrks, type, Defaultcheck);

            }
            else
            {
                data = "Update";
                dt = objemployeebal.DocumenttemplateManage(data, templatename, bodyhtml, remrks, type, Defaultcheck, Id);
            }


            if (dt.Rows.Count > 0)
            {
                return new { status = "success" };
            }
            else
            {
                return new { status = "failure" };
            }
        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static object ModifyTemplateInfo(string data, string templatename, string bodyhtml, string remrks, string type, Boolean Defaultcheck, string Id = null)
        {
            bool isUpdated = false;
            Documenttemplate objemployeebal = new Documenttemplate();

            DataTable dt = new DataTable();


            dt = objemployeebal.DocumenttemplateManage(data, templatename, bodyhtml, remrks, type, Defaultcheck, Id);



            if (dt.Rows.Count > 0)
            {
                return new { status = "success" };
            }
            else
            {
                return new { status = "failure" };
            }
        }


        protected void GrdTemplate_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            BindGrid();
        }


        public void BindDiv()
        {
            string[] list = new String[] { "receipent", "sender" }; ;
            //Converter oConverter = new Converter();     //____This is to call recipient variable with the predefined values.
            BusinessLogicLayer.Converter oConverter = new BusinessLogicLayer.Converter();
            string UIstring = "<div>";
            if (list[0] == "receipent")
            {
                string[,] recipient = oConverter.ReservedWord_recipient();
                string mess = "window.opener.document.aspnetForm.txt_msg.value";
                for (int i = 0; i < recipient.Length / 2; i++)
                {
                    UIstring += "<input style='border-right: 1px groove; border-top: 1px groove; font-size: 8pt;  border-left: 1px groove; color: black; border-bottom: 1px groove; font-style: normal; font-family: Verdana; width: 125px' onclick='PostReservedWord(this.value);'  type='button' id='chk' name='chk' value='" + recipient[i, 0] + "'>";
                }
            }
            if (list.Length > 1)
            {
                if (list[1] == "sender")
                {
                    string[,] sender1 = oConverter.ReservedWord_sender();
                    for (int i = 0; i < sender1.Length / 2; i++)
                    {
                        UIstring += "<input style='border-right: 1px groove; border-top: 1px groove; font-size: 8pt;  border-left: 1px groove; color: black; border-bottom: 1px groove; font-style: normal; font-family: Verdana; width: 125px' onclick='PostReservedWord(this.value);'  type='button' id='chk' name='chk' value='" + sender1[i, 0] + "'>";
                    }
                }
            }
            UIstring += "</div>";
            myDiv.InnerHtml = UIstring;
        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static object ModifyListByID(string Id)
        {
            DataTable dt1 = new DataTable();
            Documenttemplate obj = new Documenttemplate();

            ImportTemplate Template = new ImportTemplate();
            try
            {
                dt1 = Documenttemplate.GetImportDetailsByID(Id);

                Template = DbHelpers.ToModel<ImportTemplate>(dt1);

                if (Template != null)
                {
                    // var serializer = new JavaScriptSerializer();
                    // var serializedResult = serializer.Serialize(Template);

                    return new { Body = Template.Body, Template = Template.Template, DocType = Template.DocType, IsDefault = Template.IsDefault, Remarks = Template.Remarks };
                }

                return JsonConvert.SerializeObject(Template);

            }
            catch (Exception ex)
            {
                return "Error occured";
            }
        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static object DeleteTemplate(string Id)
        {
            DataTable dt1 = new DataTable();
            Documenttemplate obj = new Documenttemplate();

            ImportTemplate Template = new ImportTemplate();
            try
            {
                dt1 = Documenttemplate.DeleteTemplate(Id);

                if (dt1 != null)
                {
                    return new { success = "success" };
                }

                return JsonConvert.SerializeObject(Template);

            }
            catch (Exception ex)
            {
                return "Error occured";
            }
        }



        protected void GrdTemplate_DataBinding(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = objemployeebal.GetImportDetails();
            if (dt.Rows.Count > 0)
            {
                GrdTemplate.DataSource = dt;
            }
            else
            {
                GrdTemplate.DataSource = null;
               

            }
          
        }


        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                bindexport(Filter);
            }
        }



        public void bindexport(int Filter)
        {
            //GrdReplacement.Columns[6].Visible = false;

           GrdTemplate.Columns[2].Visible = false;

            string filename = "Document Template";
            exporter.FileName = filename;
            exporter.FileName = "Document Template";

            exporter.PageHeader.Left = "Document Template";
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


    }
}