using System;
using System.Configuration;
using System.IO;
using System.Web;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class management_frmAddDocumentCandidate : System.Web.UI.Page
    {
        ClsDropDownlistNameSpace.clsDropDownList cls = new ClsDropDownlistNameSpace.clsDropDownList();
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        DBEngine oDBEngine = new DBEngine();
        protected void Page_Init(object sender, EventArgs e)
        {
            SlectBuilding.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Button1.Attributes.Add("onclick", "javascript:return Validation();");
            string DocementTypeId = "";
            if (Request.QueryString["type"] != null)
            {
                DocementTypeId = Request.QueryString["type"].ToString();
                //DocementTypeId = "'" + DocementTypeId + "'";
            }
            if (Request.QueryString["id1"] != null)
            {
                DocementTypeId = Request.QueryString["id1"].ToString();
            }
            if (!IsPostBack)
            {
                string[,] DocyType = oDBEngine.GetFieldValue("tbl_master_documentType", "dty_id,dty_documentType", " dty_applicableFor='" + DocementTypeId + "'", 2, "dty_documentType");
                if (DocyType[0, 0] != "n")
                {
                    cls.AddDataToDropDownList(DocyType, DTYpe);
                }
                string[,] Building1 = oDBEngine.GetFieldValue("tbl_master_building", "bui_id,bui_Name", null, 2, "bui_Name");
                if (Building1[0, 0] != "n")
                {
                    cls.AddDataToDropDownList(Building1, Building, true);
                }
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            string DocumentID = "";
            DocumentID = Request.QueryString["id2"].ToString();
            //if (Request.QueryString["formtype"] != null)
            //    DocumentID = Session["InternalId"].ToString();//session
            //else
            //{
            //    if (Session["InternalId"] != null)
            //    {
            //        DocumentID = Session["InternalId"].ToString();
            //    }
            //    else
            //    {
            //        if (HttpContext.Current.Session["userlastsegment"].ToString() == "10")
            //        {
            //            DocumentID = HttpContext.Current.Session["CdslClients_BOID"].ToString();//session
            //        }
            //        else
            //        {
            //            DocumentID = HttpContext.Current.Session["KeyVal_InternalID"].ToString();//session
            //        }
            //    }
            //}
            string building1 = "";
            if (Building.SelectedItem.Text == "Select")
            {
                building1 = "0";
            }
            else
            {
                building1 = Building.SelectedItem.Value;
            }
            Int32 CreateUser = Int32.Parse(HttpContext.Current.Session["userid"].ToString());//Session UserID
            DateTime CreateDate = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
            string FName = Path.GetFileName(FileUpload1.PostedFile.FileName);
            if (FName != "")
            {
                string sd = objConverter.GetAutoGenerateNo();
                //  string filename = Session.SessionID + FName;
                string filename = HttpContext.Current.Session["userid"].ToString() + sd + FName;
                string FLocation = Server.MapPath("../Documents/") + filename;
                FileUpload1.PostedFile.SaveAs(FLocation);
                objEngine.InsurtFieldValue("tbl_master_document", "doc_contactId,doc_documentTypeId,doc_documentName,doc_source,doc_buildingId,doc_Floor,doc_RoomNo,doc_CellNo,doc_FileNo,CreateDate,CreateUser", "'" + DocumentID + "','" + DTYpe.SelectedItem.Value + "','" + TxtName.Text + "','" + DTYpe.SelectedItem.Value.ToString() + "/" + DocumentID + "~" + filename + "','" + building1 + "','" + TxtFloorNo.Text + "','" + TxtRoomNo.Text + "','" + TxtCellNo.Text + "','" + TxtfileNo.Text + "','" + CreateDate.ToString() + "','" + CreateUser + "'");
            }
            else
            {
                objEngine.InsurtFieldValue("tbl_master_document", "doc_contactId,doc_documentTypeId,doc_documentName,doc_buildingId,doc_Floor,doc_RoomNo,doc_CellNo,doc_FileNo,CreateDate,CreateUser", "'" + DocumentID + "','" + DTYpe.SelectedItem.Value + "','" + TxtName.Text + "','" + building1 + "','" + TxtFloorNo.Text + "','" + TxtRoomNo.Text + "','" + TxtCellNo.Text + "','" + TxtfileNo.Text + "','" + CreateDate.ToString() + "','" + CreateUser + "'");
            }
            string popupScript = "";
            string query = Request.QueryString["id"].ToString();
            if (Request.QueryString["mode"].ToString() != "")
            {
                query = query + "&mode=" + Request.QueryString["mode"].ToString();
            }
            if (Session["KeyVal2"] != null)
            {
                popupScript = "<script language='javascript'>" + "alert('Successfully Uploaded');window.parent.Getvalue();window.parent.popup.Hide();</script>";
                ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
            }
            else
            {
                popupScript = "<script language='javascript'>" + "alert('Successfully Uploaded');window.parent.location.href='" + query + "';window.parent.popup.Hide();</script>";
                ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
            }

        }
    }
}