using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
namespace ERP.OMS.Management.Activities
{
    public partial class management_activities_frmbulkemail_attachment : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        string data = "";
        DataTable DT = new DataTable();
        public string pageAccess = "";
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        clsDropDownList clsdrp = new clsDropDownList();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            if (!IsPostBack)
            {
                //________This script is for firing javascript when page load first___//
                if (!ClientScript.IsStartupScriptRegistered("Today"))
                    ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>AtTheTimePageLoad();</script>");
                //______________________________End Script____________________________//
                chkAttachment.Attributes.Add("onclick", "checkAttachmentclick(chkAttachment.checked);");
                cmbBulkEmailTemplate.Attributes.Add("onchange", "SendingMailOption(cmbBulkEmailTemplate.value);");

               // DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]); MULTI
                DBEngine oDBEngine = new DBEngine();


                string[,] listitems = oDBEngine.GetFieldValue(" tbl_master_bulkmail ", " bem_id,bem_description ", " bem_useuntil is null ", 2, "bem_description");
                if (listitems[0, 0] != "n")
                    // oDBEngine.AddDataToDropDownList(listitems, cmbBulkEmailTemplate, true);
                    clsdrp.AddDataToDropDownList(listitems, cmbBulkEmailTemplate, true);
                Session["table"] = null;
                lblMessage.Text = "";
            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }

        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }

        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
           // DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]); MULTI
            DBEngine oDBEngine = new DBEngine();

            string id = eventArgument.ToString();
            string[] FieldWvalue = id.Split('~');
            #region Bulk Email TEmplate
            if (FieldWvalue[0] == "option")
            {
                string[,] listitems = oDBEngine.GetFieldValue(" tbl_master_bulkmail ", " bem_bodysource ", " bem_id=" + FieldWvalue[1], 1);
                Session["KeyVal"] = listitems[0, 0];
                data = "option~" + listitems[0, 0];
            }
            #endregion

            #region optionnull --> To make session null at select selected!
            if (FieldWvalue[0] == "optionnull")
            {
                Session["KeyVal"] = null;
            }
            #endregion
            #region sendingmails
            if (FieldWvalue[0] == "send")
            {
                if (Session["KeyVal"] != null)
                {
                    if (Session["table"] != null)
                    {
                        string retrn = oDBEngine.SendBulkMail(cmbBulkEmailTemplate.SelectedValue, (DataTable)Session["table"]);
                        if (retrn == "done")
                        {
                            data = "send~Y";
                            Session["table"] = null;
                            DataTable dtt = new DataTable();
                            GridAttachment.DataSource = dtt;
                            GridAttachment.DataBind();
                        }
                        else
                            data = "send~" + retrn;
                    }
                    else
                        data = "send~Attach file To Email!";
                }
                else
                    data = "send~Select Template For Sending Mails!";
            }
            #endregion
        }

        protected void GridAttachment_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string datalist = e.Parameters.ToString();
            string[] FieldWvalue = datalist.Split('~');

            if (FieldWvalue[0] == "remvloc")        //___Remove Upload
            {
                string[] FileLocationIDs = FieldWvalue[1].ToString().Split(',');
                DataTable DTLocalFiles = (DataTable)Session["table"];
                for (int i = 0; i < DTLocalFiles.Rows.Count; i++)
                {
                    for (int j = 0; j < FileLocationIDs.Length; j++)
                    {
                        if (DTLocalFiles.Rows[i]["filepathServer"].ToString().Trim() == FileLocationIDs[j].ToString().Trim())
                        {
                            File.Delete(FileLocationIDs[j].ToString().Trim());
                            DTLocalFiles.Rows[i].Delete();
                        }
                    }
                }
                GridAttachment.DataSource = DTLocalFiles.DefaultView;
                GridAttachment.DataBind();
            }
        }
        protected void btnUpload_ServerClick(object sender, EventArgs e)
        {
            string paths = "";
            string path = Server.MapPath("../Documents") + "\\Bulk_Email_Attacthments";
            if (System.IO.Directory.Exists(path))
            {
                if (Session["KeyVal"].ToString() == "1")
                {
                    if (UploadBody.PostedFile.FileName != "")
                    {
                        paths = uploadDocument(UploadBody.PostedFile.FileName, UploadBody);
                        fillGridLocal(paths, UploadBody);
                        lblMessage.Text = "";
                    }
                    else
                    {
                        lblMessage.Text = "Please Browse File for upload!!";
                        return;
                    }
                    if (UploadAttachment.PostedFile.FileName != "")
                    {
                        paths = uploadDocument(UploadAttachment.PostedFile.FileName, UploadAttachment);
                        fillGridLocal(paths, UploadAttachment);
                    }
                }
                else
                {
                    lblMessage.Text = "";
                    if (UploadAttachment.PostedFile.FileName != "")
                    {
                        paths = uploadDocument(UploadAttachment.PostedFile.FileName, UploadAttachment);
                        fillGridLocal(paths, UploadAttachment);
                    }
                }
            }
            else
            {
                System.IO.Directory.CreateDirectory(path);
                if (UploadBody.PostedFile.FileName != "")
                {
                    paths = uploadDocument(UploadBody.PostedFile.FileName, UploadBody);
                    fillGridLocal(paths, UploadBody);
                }
                if (UploadAttachment.PostedFile.FileName != "")
                {
                    paths = uploadDocument(UploadAttachment.PostedFile.FileName, UploadAttachment);
                    fillGridLocal(paths, UploadAttachment);
                }
            }
            string sendmesg = Session["KeyVal"].ToString() + "," + chkAttachment.Checked.ToString();

            //________This script is for firing javascript ___//
            if (!ClientScript.IsStartupScriptRegistered("Today"))
                ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>UploadFuction('" + sendmesg + "');</script>");
            //______________________________End Script____________________________//
        }

        private string uploadDocument(string path, FileUpload uploader)
        {
            string FLocation1 = "";
            string FName = Path.GetFileName(path);
            if (FName != "")
            {
                string FLocation = Server.MapPath("../Documents/Bulk_Email_Attacthments/") + Session.SessionID + "_" + FName;
                FLocation1 = FLocation;
                uploader.PostedFile.SaveAs(FLocation);
            }
            return FLocation1;
        }

        private void fillGridLocal(string path, FileUpload uploader)
        {
            if (Session["table"] != null)
            {
                //DataTable DT = new DataTable();
                DT = (DataTable)Session["table"];
                int lenghth = DT.Rows.Count;
                DataRow DR = DT.NewRow();
                DR["fileid"] = lenghth + 1;
                DR["filename"] = uploader.FileName;
                DR["filepath"] = uploader.PostedFile.FileName;
                DR["filepathServer"] = path;
                DR["bodyFile"] = Session["KeyVal"].ToString();

                DT.Rows.Add(DR);
            }
            else
            {
                ///DataTable DT = new DataTable();
                DataColumn DC1 = new DataColumn("fileid");
                DataColumn DC2 = new DataColumn("filename");
                DataColumn DC3 = new DataColumn("filepath");
                DataColumn DC4 = new DataColumn("filepathServer");
                DataColumn DC5 = new DataColumn("bodyFile");
                DT.Columns.Add(DC1);
                DT.Columns.Add(DC2);
                DT.Columns.Add(DC3);
                DT.Columns.Add(DC4);
                DT.Columns.Add(DC5);

                DataRow DR = DT.NewRow();
                DR["fileid"] = 1;
                DR["filename"] = uploader.FileName;
                DR["filepath"] = uploader.PostedFile.FileName;
                DR["filepathServer"] = path;
                DR["bodyFile"] = Session["KeyVal"].ToString();

                DT.Rows.Add(DR);
            }
            Session["table"] = DT;

            GridAttachment.DataSource = DT;
            GridAttachment.DataBind();
        }

    }
}