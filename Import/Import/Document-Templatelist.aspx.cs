using DataAccessLayer;
using ImportModuleBusinessLayer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Import.Import
{
    public partial class Document_Templatelist : System.Web.UI.Page
    {
        Documenttemplate objemployeebal = new Documenttemplate();

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Document-Templatelist.aspx");

            if (!IsPostBack)
            {
                Session["KeyVal"] = null;

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
            }
            catch (Exception ex)
            {

            }

            #endregion
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


            GrdTemplate.DataSource = dt;
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

    }
}