using DataAccessLayer;
using DevExpress.Web;
//using ImportModuleBusinessLayer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Resource
{
    public partial class MarketingResouce : System.Web.UI.Page
    {
        //Documentattachment objattachment = new Documentattachment();
        DataTable dt = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["Document_Attachment"] = null;
                //  BindGrid();

                if (Request.QueryString["Key"] != "Add")
                {
                    string DocId = Request.QueryString["Key"];
                    DataTable EditedDataDetails = new DataTable();
                    if (EditedDataDetails.Rows.Count > 0)
                    {
                        drp_templatetype.SelectedValue = Convert.ToString(EditedDataDetails.Rows[0]["Contenttype"]);
                        taggingList.Text = Convert.ToString(EditedDataDetails.Rows[0]["Sourcedocnumber"]);
                        taggingList.ClientEnabled = false;
                        lblDocument.Text = Convert.ToString(EditedDataDetails.Rows[0]["Document_name"]);
                    }
                }
                if (Request.QueryString["req"] != null && Request.QueryString["req"] == "V")
                {
                    lblHeading.Text = "View Document Attachment";
                    UploadButton.Visible = false;
                    Div_FileUpload.Visible = false;

                }
            }
        }

        //protected void ComponentDocument_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{
        //    string Action = e.Parameter.Split('~')[0];
        //    if (Action == "BindDocumentNumber")
        //    {
        //        string doctype = e.Parameter.Split('~')[1];
        //        if (doctype != "0")
        //        {
        //            DataTable dt = new DataTable();
        //            dt = objattachment.DocumentfetchbyType(doctype);
        //            Session["Document_Attachment"] = dt;


        //            lookup_Document.DataSource = dt;
        //            lookup_Document.DataBind();
        //        }
        //        else
        //        {
        //            Session["Document_Attachment"] = null;
        //            lookup_Document.DataSource = null;
        //            lookup_Document.DataBind();

        //        }
        //    }
        //    else
        //    {

        //        lookup_Document.DataSource = null;
        //        lookup_Document.DataBind();

        //    }

        //}

        //protected void lookup_Document_DataBinding(object sender, EventArgs e)
        //{
        //    if (Session["Document_Attachment"] != null)
        //    {
        //        lookup_Document.DataSource = (DataTable)Session["Document_Attachment"];
        //    }
        //    else
        //    {

        //        lookup_Document.DataSource = null;
        //    }
        //}


        protected void UploadButton_Click(object sender, EventArgs e)
        {
            string User = Convert.ToString(HttpContext.Current.Session["userid"]).Trim();
            DataTable dtdocumentdetails = CreateTempTable("Documents");
            string DocumentID = "";


            // string DocumentID = lookup_Document.Value.ToString();
            /// string DocumentNumber = lookup_Document.GridView.GetRowValues(lookup_Document.GridView.FocusedRowIndex, "Document").ToString();

            string DocumentNumber = "";

            for (int i = 0; i < taggingGrid.GetSelectedFieldValues("ID").Count; i++)
            {
                DocumentID += "," + Convert.ToString(taggingGrid.GetSelectedFieldValues("ID")[i]);
                DocumentNumber += "," + Convert.ToString(taggingGrid.GetSelectedFieldValues("Document")[i]);
            }

            DocumentID = DocumentID.TrimStart(',');
            DocumentNumber = DocumentNumber.TrimStart(',');

            if (!string.IsNullOrEmpty(DocumentID))
            {
                foreach (HttpPostedFile uploadedFile in file_product.PostedFiles)
                {
                    if (uploadedFile.ContentLength <= 5242880)
                    {
                        string strpathextn = System.IO.Path.GetExtension(uploadedFile.FileName);

                        if (strpathextn == ".pdf" || strpathextn == ".xls" || strpathextn == ".xlsx" || strpathextn == ".docx" || strpathextn == ".doc")
                        {

                            bool exists = System.IO.Directory.Exists(Server.MapPath("~/CommonFolder/ImportDocuments/"));

                            if (!exists)
                                System.IO.Directory.CreateDirectory(Server.MapPath("~/CommonFolder/ImportDocuments/"));


                            uploadedFile.SaveAs(Server.MapPath("~/CommonFolder/ImportDocuments/") +
                                                uploadedFile.FileName);

                            FileUploadedList.Text += "File name: " +
                               uploadedFile.FileName + "<br>" +
                               uploadedFile.ContentLength + " kb<br>" +
                               "Content type: " + uploadedFile.ContentType + "<br><br>";

                            string FileName = DocumentID + Guid.NewGuid() + uploadedFile.FileName;


                            var filePath = Server.MapPath("~/CommonFolder/ImportDocuments/" + uploadedFile.FileName);

                            if (File.Exists(filePath))
                            {
                                File.Delete(filePath);
                            }

                            uploadedFile.SaveAs(Server.MapPath("~/CommonFolder/ImportDocuments/" + FileName));
                            dtdocumentdetails.Rows.Add(DocumentID, DocumentNumber, null, drp_templatetype.SelectedValue, uploadedFile.FileName, FileName);
                        }

                        else
                        {
                            FileUploadedList.Text = "Only pdf,word and excel files are allowed.";
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>Productimagepopulate( '" + FileUploadedList.Text + "');</script>", false);


                        }


                    }

                    else
                    {
                        FileUploadedList.Text = "Maximum 5 Mb file is allowed to upload.";
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>Productimagepopulate( '" + FileUploadedList.Text + "');</script>", false);


                    }

                }
            }
            else
            {
                FileUploadedList.Text = "Document mandatory";
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>Productimagepopulate( '" + FileUploadedList.Text + "');</script>", false);


            }
            if (dtdocumentdetails.Rows.Count > 0)
            {
                //objattachment.DocumentMultipleImage(dtdocumentdetails, User, DocumentID, DocumentNumber, DocumentNumber, "");
                FileUploadedList.Text = "Attachment Saved Successfully.";
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>Productimagepopulate( '" + FileUploadedList.Text + "');</script>", false);

            }
        }


        //protected void UploadButton_Click(object sender, EventArgs e)
        //{

        //    string User = Convert.ToString(HttpContext.Current.Session["userid"]).Trim();
        //    DataTable dtdocumentdetails = CreateTempTable("Multipledocuments");

        //    if (drp_templatetype.SelectedValue != "0")
        //    {
        //        if (lookup_Document.Value != null)
        //        {
        //            string DocumentID = lookup_Document.Value.ToString();
        //            string strpathextn = "";

        //            string DocumentNumber = lookup_Document.GridView.GetRowValues(lookup_Document.GridView.FocusedRowIndex, "Document").ToString();
        //            if (file_product.HasFile)
        //            {
        //                if (file_product.FileBytes.Length <= 5120)
        //                {

        //                    strpathextn = System.IO.Path.GetExtension(file_product.FileName);

        //                    if (strpathextn == ".pdf" || strpathextn == ".xls" || strpathextn == ".xlsx" || strpathextn == ".docx" || strpathextn == ".doc")
        //                    {

        //                        //  string prodid = hdnprodID.Value;
        //                        foreach (HttpPostedFile uploadedFile in file_product.PostedFiles)
        //                            try
        //                            {
        //                                string filename = Path.GetFileName(uploadedFile.FileName);
        //                                string contentType = uploadedFile.ContentType;

        //                                using (Stream fs = uploadedFile.InputStream)
        //                                {
        //                                    using (BinaryReader br = new BinaryReader(fs))
        //                                    {
        //                                        byte[] bytes = br.ReadBytes((Int32)fs.Length);
        //                                        dtdocumentdetails.Rows.Add("1", DocumentNumber, bytes, drp_templatetype.SelectedValue, filename,"");
        //                                    }
        //                                }

        //                                objattachment.DocumentMultipleImage(dtdocumentdetails, User, DocumentID, DocumentNumber, filename, contentType);

        //                            }

        //                            catch (Exception ex)
        //                            {
        //                                FileUploadedList.Text = "ERROR: " + ex.Message.ToString();
        //                            }
        //                        // FileUploadedList.Visible = false;
        //                        FileUploadedList.Text = "Attachment Saved Successfully.";
        //                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>Productimagepopulate( '" + FileUploadedList.Text + "');</script>", false);

        //                    }

        //                    else
        //                    {

        //                        FileUploadedList.Text = "Only pdf,word and excel files are allowed.";
        //                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>Productimagepopulate( '" + FileUploadedList.Text + "');</script>", false);

        //                    }
        //                }
        //                else
        //                {

        //                    FileUploadedList.Text = "Maximum 2 Mb file is allowed to upload.";
        //                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>Productimagepopulate( '" + FileUploadedList.Text + "');</script>", false);

        //                }

        //            }
        //            else
        //            {
        //                //  FileUploadedList.Visible = true;
        //                FileUploadedList.Text = "You have not specified a file.";
        //                ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>Productimagepopulate( '" + FileUploadedList.Text + "');</script>", false);

        //            }

        //        }
        //        else
        //        {
        //            //  FileUploadedList.Visible = true;
        //            FileUploadedList.Text = "Document Number mandatory.";
        //            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>Productimagepopulate( '" + FileUploadedList.Text + "');</script>", false);

        //        }
        //    }
        //    else
        //    {

        //        FileUploadedList.Text = "Document type mandatory.";
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>Productimagepopulate( '" + FileUploadedList.Text + "');</script>", false);

        //    }




        //}

        public DataTable CreateTempTable(string Type)
        {

            DataTable dt = new DataTable();

            if (Type == "Multipledocuments")
            {
                dt.Columns.Add("DocId", typeof(string));
                dt.Columns.Add("DocNummber", typeof(string));
                dt.Columns.Add("Doc_File", typeof(byte[]));
                dt.Columns.Add("Contenttype", typeof(string));
                dt.Columns.Add("Doc_Name", typeof(string));
                dt.Columns.Add("File_content", typeof(string));
            }
            if (Type == "Documents")
            {
                dt.Columns.Add("DocId", typeof(string));
                dt.Columns.Add("DocNummber", typeof(string));
                dt.Columns.Add("Doc_File", typeof(byte[]));
                dt.Columns.Add("Contenttype", typeof(string));
                dt.Columns.Add("Doc_Name", typeof(string));
                dt.Columns.Add("File_content", typeof(string));
            }
            return dt;
        }

        protected void taggingGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {

            string strSplitCommand = Convert.ToString(e.Parameters.Split('~')[0]);
            string doctype = e.Parameters.Split('~')[1];

            if (strSplitCommand == "BindComponentGrid")
            {

                DataTable dt = new DataTable();
                //dt = objattachment.DocumentfetchbyType(doctype);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Session["Document_Attachment"] = dt;
                    taggingGrid.DataSource = dt;
                    taggingGrid.DataBind();
                }
                else
                {
                    Session["Document_Attachment"] = null;
                    taggingGrid.DataSource = null;
                    taggingGrid.DataBind();
                }
            }

            else if (strSplitCommand == "CloseDocumentGrid")
            {
                string DocumentNumber = "";

                for (int i = 0; i < taggingGrid.GetSelectedFieldValues("ID").Count; i++)
                {
                    DocumentNumber += "," + Convert.ToString(taggingGrid.GetSelectedFieldValues("Document")[i]);
                }

                DocumentNumber = DocumentNumber.TrimStart(',');

                taggingGrid.JSProperties["cpDocment"] = DocumentNumber;

            }

        }


        protected void taggingGrid_DataBinding(object sender, EventArgs e)
        {
            if (Session["Document_Attachment"] != null)
            {
                taggingGrid.DataSource = (DataTable)Session["Document_Attachment"];
            }
            else
            {
                taggingGrid.DataSource = null;
            }
        }
    }
}