using DataAccessLayer;
using ImportModuleBusinessLayer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Import.Import
{
    public partial class Document_AttachmentList : System.Web.UI.Page
    {


        Documentattachment objattachment = new Documentattachment();
        DataTable dt = new DataTable();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public EntityLayer.CommonELS.UserRightsForPage rights = new EntityLayer.CommonELS.UserRightsForPage();

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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Document-Attachmentlist.aspx");

            if (!IsPostBack)
            {
                Session["Document_Attachment"] = null;
            //    BindGrid();
            }
        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static void DownloadAttachment(string Id)
        {
            string contentType = "";
            string fileName = "";
            byte[] bytes;
            byte bytedata;
            //bytes = (byte[])bytedata;

            DataTable dt = new DataTable();
            dt = Documentattachment.DocumentAttachmentfetchByID(Id);
            if (dt.Rows.Count > 0)
            {
                bytes = (byte[])(dt.Rows[0]["Document_File"]);

                contentType = Convert.ToString(dt.Rows[0]["Contenttype"]);
                fileName = Convert.ToString(dt.Rows[0]["Document_File"]);


                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Charset = "";
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.ContentType = contentType;
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                HttpContext.Current.Response.BinaryWrite(bytes);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }

        }


        public void BindGrid()
        {
            try
            {
                dt = objattachment.DocumentAttachmentfetch();
                if (dt.Rows.Count > 0)
                {
                    gridAttachment.DataSource = dt;
                    gridAttachment.DataBind();
                }
                else
                {
                    gridAttachment.DataSource = null;
                    gridAttachment.DataBind();
                }
            }
            catch (Exception ex)
            {

            }


        }

        protected void Grdattachment_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            BindGrid();
        }

        protected void gridAttachment_DataBinding(object sender, EventArgs e)
        {
            dt = objattachment.DocumentAttachmentfetch();
            if (dt.Rows.Count > 0)
            {
                gridAttachment.DataSource = dt;
            }
            else
            {
                gridAttachment.DataSource =null;
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
            gridAttachment.Columns[5].Visible = false;
            string filename = "Document Attachment";
            exporter.FileName = filename;
            exporter.FileName = "Document Attachment";

            exporter.PageHeader.Left = "Document Attachment";
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


        //[WebMethod]
        //[System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        //public static object DeleteAttachment(string AttchmentID)
        //{
        //    Documentattachment objattachment = new Documentattachment();
        //    DataTable dtattachment = new DataTable();
        //    try
        //    {
        //        dtattachment = objattachment.DelteAttachment(AttchmentID);

        //        if (dtattachment.Rows.Count > 0)
        //        {
        //            return new { status = "success" };
        //        }

        //        return new { status = "failure" };
        //    }
        //    catch
        //    {

        //        return new { status = "failure" };
        //    }

        //}

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static object DeleteAttachment(string AttchmentID)
        {
            Documentattachment objattachment = new Documentattachment();
            DataTable dtattachment = new DataTable();
            try
            {
                dtattachment = objattachment.DelteAttachment(AttchmentID);

                if (dtattachment.Rows.Count > 0)
                {

                    var filePath = HttpContext.Current.Server.MapPath("~/CommonFolder/ImportDocuments/" + Convert.ToString(dtattachment.Rows[0]["Document_Content"]));
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }


                    return new { status = "success" };
                }

                return new { status = "failure" };
            }
            catch
            {

                return new { status = "failure" };
            }

        }


    }
}