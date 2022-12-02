using ImportModuleBusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Import.Import
{

    public partial class DownloadAttachment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                Downloadattachment();
                //if (Request.QueryString["D"] != null)
                //{
                //    string contentType = "";
                //    string fileName = "";
                //    byte[] bytes;
                //    byte bytedata;
                //    //bytes = (byte[])bytedata;

                //    DataTable dt = new DataTable();
                //    dt = Documentattachment.DocumentAttachmentfetchByID(Request.QueryString["D"]);
                //    if (dt.Rows.Count > 0)
                //    {
                //        bytes = (byte[])(dt.Rows[0]["Document_File"]);

                //        contentType = Convert.ToString(dt.Rows[0]["Contenttype"]);
                //        fileName = Convert.ToString(dt.Rows[0]["Document_name"]);


                //        HttpContext.Current.Response.Clear();
                //        HttpContext.Current.Response.Buffer = true;
                //        HttpContext.Current.Response.Charset = "";
                //        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //        HttpContext.Current.Response.ContentType = contentType;
                //        HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                //        HttpContext.Current.Response.BinaryWrite(bytes);
                //        HttpContext.Current.Response.Flush();
                //        HttpContext.Current.Response.End();
                //    }
                //}
            }
        }

        public void Downloadattachment()
        {

            if (Request.QueryString["D"] != null)
            {
                string contentType = "";
                string fileName = "";
                byte[] bytes;
                byte bytedata;
                //bytes = (byte[])bytedata;

                DataTable dt = new DataTable();
                dt = Documentattachment.DocumentAttachmentfetchByID(Request.QueryString["D"]);
                if (dt.Rows.Count > 0)
                {
                   // bytes = (byte[])(dt.Rows[0]["Document_File"]);

                    contentType = Convert.ToString(dt.Rows[0]["Contenttype"]);
                    fileName = Convert.ToString(dt.Rows[0]["Document_name"]);

                    string filePath = "~\\CommonFolder\\ImportDocuments\\" +  Convert.ToString(dt.Rows[0]["Document_Content"]);

                    var filePath2 = Server.MapPath("~/CommonFolder/ImportDocuments/" + Convert.ToString(dt.Rows[0]["Document_Content"]));

                    if (File.Exists(filePath2))
                    {
                        Response.ContentType = "image/jpg";
                        Response.AddHeader("Content-Disposition", "attachment;filename=\"" + filePath + "\"");
                        Response.TransmitFile(Server.MapPath(filePath));
                        Response.End();  
                    }

                }
            }
        }

    }

}