using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
using System.Web.UI;

namespace ERP.OMS.Management.Master
{
    public partial class management_Master_frmAddDocuments : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        public static string filesrc = string.Empty;
        public static string filedoc = string.Empty;
        public static string fileSize = string.Empty;
        public static string fileSizeinMB = string.Empty;

        Converter objConverter = new Converter();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        clsDropDownList OclsDropDownList = new clsDropDownList();
        string DocID = "";
        string DocementTypeId = "";
        string strXmlKeyDocCB = "";
        BusinessLogicLayer.MasterSettings objmaster = new BusinessLogicLayer.MasterSettings();

        CommonBL ComBL = new CommonBL();

        protected void Page_Load(object sender, EventArgs e)
        {

            // Code  Added and Commented By Priti on 12122016 to use Convert.ToString instead of ToString()
            SlectBuilding.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            fileSize = Convert.ToString(ConfigurationManager.AppSettings["fileSize"]);

            fileSizeinMB = Convert.ToString(ConfigurationManager.AppSettings["fileSizeinMB"]);
            int i = Request.QueryString.Count;
            string strUrl = Request.RawUrl;

            LinkButton1.Attributes.Add("Onclick", "javascript:ShowFileUpload();");
            btnCancel.Attributes.Add("Onclick", "javascript:OnCloseButtonClick();");

            //Tanmoy 15-05-2020
            string mastersettings = objmaster.GetSettings("isServiceManagementRequred");
            hdnIsServiceManagemnt.Value = mastersettings;

            string masterSTBsettings = ComBL.GetSystemSettingsResult("IsSTBManagementRequired");
            if (masterSTBsettings == "Yes")
            {
                hdnIsServiceManagemnt.Value = "1";
            }

            //Tanmoy 15-05-2020


            if (Request.QueryString["type"] != null)
            {

                DocementTypeId = Convert.ToString(Request.QueryString["type"]);

            }
            if (Request.QueryString["id1"] != null)
            {

                DocementTypeId = Convert.ToString(Request.QueryString["id1"]);
            }

            if (DocementTypeId == "Franchisee")
            {
                DocementTypeId = "Franchisees";
            }
            else if (DocementTypeId == "Salesman/Agents")
            {
                DocementTypeId = "Agents";
            }

            if (DocementTypeId == "SrvSearch")
            {
                DocementTypeId = "SrvDelivery";
            }

            if (Request.QueryString["docid"] != null)
            {

                DocID = Convert.ToString(Request.QueryString["docid"]);
            }
            if (!IsPostBack)
            {
                string previousPageUrl = string.Empty;
                if (Request.UrlReferrer != null)
                    previousPageUrl = Request.UrlReferrer.AbsoluteUri;
                else
                    previousPageUrl = Page.ResolveUrl("~/OMS/Management/ProjectMainPage.aspx");

                ViewState["previousPageUrl"] = previousPageUrl;


                if (Request.QueryString["AcType"] != null && Convert.ToString(Request.QueryString["AcType"]).Trim() != "Add")
                {

                    ShowForm();

                    FileUpload1.Visible = false;
                    LinkButton1.Visible = true;

                }
                else
                {

                    FileUpload1.Visible = true;
                    LinkButton1.Visible = false;
                    dtReceived.EditFormatString = objConverter.GetDateFormat("Date");

                    dtReceived.Value = oDBEngine.GetDate();
                    dtRenew.EditFormatString = objConverter.GetDateFormat("Date");


                    if (Request.QueryString["SpecialType"] == null)
                    {

                        string[,] DocyType = oDBEngine.GetFieldValue("tbl_master_documentType", "dty_id,dty_documentType", " dty_applicableFor='" + DocementTypeId + "'", 2, "dty_documentType");
                        if (DocyType[0, 0] != "n")
                        {

                            OclsDropDownList.AddDataToDropDownList(DocyType, DTYpe);
                        }
                    }

                    else if (Convert.ToString(Request.QueryString["SpecialType"]) == "CashBank")
                    {
                        string strSpclDocumentType = "CashBankJV";
                        string[,] DocyType = oDBEngine.GetFieldValue("tbl_master_documentType", "dty_id,dty_documentType", " dty_applicableFor='" + strSpclDocumentType + "'", 2, "dty_documentType");
                        if (DocyType[0, 0] != "n")
                        {

                            OclsDropDownList.AddDataToDropDownList(DocyType, DTYpe);
                        }
                    }
                    string[,] Building1 = oDBEngine.GetFieldValue("tbl_master_building", "bui_id,bui_Name", null, 2, "bui_Name");
                    if (Building1[0, 0] != "n")
                    {

                        OclsDropDownList.AddDataToDropDownList(Building1, Building, true);
                    }
                }
            }
            if (IsPostBack == false && Request.QueryString["SpecialType"] != null && Request.QueryString["Mode"] != null && Convert.ToString(Request.QueryString["SpecialType"]) == "CashBank" && Request.QueryString["Mode"] == "Edit")
            {
                DataRow dr = (Session["DataRow"] as DataRow);
                ViewState["strXmlKeyDocCB"] = Session["XmlKeyDocCB"];


            }
        }
        public void LoadEditableDataForCashBank(DataRow dr)
        {

            TxtName.Text = Convert.ToString(dr["doc_documentName"]);
            dtReceived.Value = Convert.ToString(dr["doc_receivedate"]);
            txt_note1.Text = Convert.ToString(dr["doc_Note1"]);
            txt_note2.Text = Convert.ToString(dr["doc_Note2"]);
            TxtCellNo.Text = Convert.ToString(dr["doc_CellNo"]);
            TxtRoomNo.Text = Convert.ToString(dr["doc_RoomNo"]);
            TxtFloorNo.Text = Convert.ToString(dr["doc_Floor"]);

            if (Convert.ToString(dr["doc_buildingId"]) != string.Empty)
            {

                Building.SelectedValue = Convert.ToString(dr["doc_buildingId"]);
            }


        }

        public void ShowForm()
        {
            // Code  Added and Commented By Priti on 12122016 to use Convert.ToString instead of ToString()
            string[,] dt = oDBEngine.GetFieldValue("tbl_master_document", "doc_contactId,doc_documentTypeId,doc_documentName,doc_source,doc_buildingId,doc_Floor,doc_RoomNo,doc_CellNo,doc_FileNo,doc_receivedate,doc_renewdate,doc_Note1,Doc_Note2 ", "doc_id='" + DocID + "'", 13);
            txt_note1.Text = Convert.ToString(dt[0, 11]);
            txt_note2.Text = Convert.ToString(dt[0, 12]);
            DTYpe.SelectedValue = dt[0, 1];
            TxtName.Text = Convert.ToString(dt[0, 2]);

            TxtfileNo.Text = Convert.ToString(dt[0, 8]);
            TxtCellNo.Text = Convert.ToString(dt[0, 7]);
            TxtRoomNo.Text = Convert.ToString(dt[0, 6]);
            TxtFloorNo.Text = Convert.ToString(dt[0, 5]);

            hidFilename.Value = Convert.ToString(dt[0, 3]);

            filedoc = dt[0, 1];

            filesrc = Convert.ToString(dt[0, 3]);
            if (filesrc != "")
            {
                divfile.Visible = true;
            }



            if (Convert.ToString(dt[0, 9]).Length > 0)
            {
                dtReceived.Value = Convert.ToDateTime(Convert.ToString(dt[0, 9]));
            }

            if (Convert.ToString(dt[0, 10]).Length > 0)
            {

                dtRenew.Value = Convert.ToDateTime(dt[0, 10].ToString());
            }

            dtReceived.EditFormatString = objConverter.GetDateFormat("Date");
            dtRenew.EditFormatString = objConverter.GetDateFormat("Date");

            string[,] DocyType = oDBEngine.GetFieldValue("tbl_master_documentType", "dty_id,dty_documentType", " dty_applicableFor='" + DocementTypeId + "'", 2, "dty_documentType");
            if (dt[0, 1] != "")
            {
                OclsDropDownList.AddDataToDropDownList(DocyType, DTYpe, int.Parse(Convert.ToString(dt[0, 1])));

            }
            else
            {

                OclsDropDownList.AddDataToDropDownList(DocyType, DTYpe, 0);
            }

            string[,] Building1 = oDBEngine.GetFieldValue("tbl_master_building", "bui_id,bui_Name", null, 2, "bui_Name");
            if (dt[0, 4] != "")
            {
                OclsDropDownList.AddDataToDropDownList(Building1, Building, int.Parse(Convert.ToString(dt[0, 4])));

            }
            else
            {

                OclsDropDownList.AddDataToDropDownList(Building1, Building, 0);
            }


        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            // Code  Added and Commented By Priti on 12122016 to use Convert.ToString instead of ToString()

            string folderid = Convert.ToString(DTYpe.SelectedItem.Value);


            int year = oDBEngine.GetDate().Year;
            string FolderName = String.Empty;
            string path = String.Empty;
            string FormFolderName = String.Empty;

            if (Convert.ToString(Request.QueryString["id"]) == "RegistrationForm.aspx")
            {
                path = HttpContext.Current.Server.MapPath(@"..\KRA\KRADoc\");
            }
            else if (Request.QueryString["SpecialType"] != null && Convert.ToString(Request.QueryString["SpecialType"]) == "CashBank")
            {
                path = HttpContext.Current.Server.MapPath(@"..\Documents");
            }
            else
            {
                path = HttpContext.Current.Server.MapPath(@"..\Documents\");
            }
            //string fulpath = path + "\\" + folderid;
            string fulpath = path + folderid;
            if (!System.IO.Directory.Exists(fulpath))
            {
                Directory.CreateDirectory(fulpath);

            }
            // FolderName = path + "\\" + folderid + "\\" + year;
            FolderName = path + folderid + "\\" + year;
            if (!System.IO.Directory.Exists(FolderName))
            {
                Directory.CreateDirectory(FolderName);
            }

            if (DocID != "")
            {
                string DocumentID = "";

                if (Request.QueryString["formtype"] != null)
                    DocumentID = Convert.ToString(Session["InternalId"]);//session
                else
                {
                    if (Session["InternalId"] != null)
                    {
                        if (Session["InternalId"] != "Edit")
                        {
                            DocumentID = Convert.ToString(Session["InternalId"]);
                        }
                        else
                        {
                            if (Convert.ToString(HttpContext.Current.Session["userlastsegment"]) == "10")
                            {
                                DocumentID = Convert.ToString(HttpContext.Current.Session["CdslClients_BOID"]);//session
                            }
                            else
                            {
                                DocumentID = Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]);//session
                            }
                        }
                    }
                    else
                    {
                        if (Convert.ToString(HttpContext.Current.Session["userlastsegment"]) == "10")
                        {
                            DocumentID = Convert.ToString(HttpContext.Current.Session["CdslClients_BOID"]);//session
                        }
                        else
                        {
                            DocumentID = Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]);//session
                        }

                    }
                }
                string building1 = "";
                if (Building.SelectedItem.Text == "Select")
                {
                    //..................................... Code added and modifed by Sam on 12102016.....................................
                    //building1 = "0";
                    building1 = null;
                    //..................................... Code Above added and modifed by Sam on 12102016.....................................
                }
                else
                {
                    building1 = Building.SelectedItem.Value;
                }
                Int32 CreateUser = Int32.Parse(Convert.ToString(HttpContext.Current.Session["userid"]));//Session UserID
                string CreateDate = Convert.ToDateTime(oDBEngine.GetDate().ToString()).ToString("yyyy-MM-dd hh:mmm:ss");
                //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

                if (FileUpload1.PostedFile != null)
                {
                    string FName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                    string sd = objConverter.GetAutoGenerateNo();

                    string filename = Convert.ToString(HttpContext.Current.Session["userid"]) + sd + FName;
                    string FLocation = Server.MapPath("../Documents/") + filename;

                    FolderName = path + "\\" + folderid + "\\" + year;

                    string total = FolderName + "\\" + filename;

                    FileUpload1.PostedFile.SaveAs(total);
                    if (dtRenew.Value == null)
                    {
                        objEngine.SetFieldValue("tbl_master_document", "doc_verifydatetime=null,doc_Note1='" + Convert.ToString(txt_note1.Text).Trim() + "',doc_Note2='" + Convert.ToString(txt_note2.Text).Trim() + "',doc_documentTypeId='" + DTYpe.SelectedItem.Value + "',doc_documentName='" + TxtName.Text + "',doc_source='" + Convert.ToString(DTYpe.SelectedItem.Value) + "/" + year + "/" + filename + "',doc_buildingId='" + building1 + "',doc_Floor='" + TxtFloorNo.Text + "',doc_RoomNo='" + TxtRoomNo.Text + "',doc_CellNo='" + TxtCellNo.Text + "',doc_FileNo='" + TxtfileNo.Text + "',LastModifyDate='" + CreateDate + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "',doc_receivedate='" + Convert.ToDateTime(dtReceived.Value.ToString()).ToString("yyyy-MM-dd") + "'", " doc_id='" + DocID + "'");
                    }
                    else
                    {
                        objEngine.SetFieldValue("tbl_master_document", "doc_verifydatetime=null,doc_Note1='" + Convert.ToString(txt_note1.Text).Trim() + "',doc_Note2='" + Convert.ToString(txt_note2.Text).Trim() + "',doc_documentTypeId='" + DTYpe.SelectedItem.Value + "',doc_documentName='" + TxtName.Text + "',doc_source='" + Convert.ToString(DTYpe.SelectedItem.Value) + "/" + year + "~" + Convert.ToString(DTYpe.SelectedItem.Value) + "/" + year + "/" + filename + "',doc_buildingId='" + building1 + "',doc_Floor='" + TxtFloorNo.Text + "',doc_RoomNo='" + TxtRoomNo.Text + "',doc_CellNo='" + TxtCellNo.Text + "',doc_FileNo='" + TxtfileNo.Text + "',LastModifyDate='" + oDBEngine.GetDate() + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "',doc_receivedate='" + Convert.ToDateTime(dtReceived.Value.ToString()).ToString("yyyy-MM-dd") + "',doc_renewdate='" + Convert.ToDateTime(dtRenew.Value.ToString()).ToString("yyyy-MM-dd") + "'", " doc_id='" + DocID + "'");
                    }

                }
                else
                {

                    if (dtRenew.Value == null)
                    {
                        oDBEngine.SetFieldValue("tbl_master_document", "doc_verifydatetime=null,doc_Note1='" + Convert.ToString(txt_note1.Text).Trim() + "',doc_Note2='" + Convert.ToString(txt_note2.Text).Trim() + "',doc_documentTypeId='" + DTYpe.SelectedItem.Value + "',doc_documentName='" + TxtName.Text + "',doc_buildingId='" + building1 + "',doc_Floor='" + TxtFloorNo.Text + "',doc_RoomNo='" + TxtRoomNo.Text + "',doc_CellNo='" + TxtCellNo.Text + "',doc_FileNo='" + TxtfileNo.Text + "',LastModifyDate='" + CreateDate + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "',doc_receivedate='" + Convert.ToDateTime(dtReceived.Value.ToString()).ToString("yyyy-MM-dd") + "'", " doc_id='" + DocID + "'");
                    }
                    else
                    {
                        oDBEngine.SetFieldValue("tbl_master_document", "doc_verifydatetime=null,doc_Note1='" + Convert.ToString(txt_note1.Text).Trim() + "',doc_Note2='" + Convert.ToString(txt_note2.Text).Trim() + "',doc_documentTypeId='" + DTYpe.SelectedItem.Value + "',doc_documentName='" + TxtName.Text + "',doc_buildingId='" + building1 + "',doc_Floor='" + TxtFloorNo.Text + "',doc_RoomNo='" + TxtRoomNo.Text + "',doc_CellNo='" + TxtCellNo.Text + "',doc_FileNo='" + TxtfileNo.Text + "',LastModifyDate='" + CreateDate + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "',doc_receivedate='" + Convert.ToDateTime(dtReceived.Value.ToString()).ToString("yyyy-MM-dd") + "',doc_renewdate='" + Convert.ToDateTime(dtRenew.Value.ToString()).ToString("yyyy-MM-dd") + "'", " doc_id='" + DocID + "'");
                    }
                }

                string popupScript = "";

                string query = Convert.ToString(Request.QueryString["id"]);

                if (Request.QueryString["mode"] != null)
                {

                    query = query + "&mode=" + Convert.ToString(Request.QueryString["mode"]);
                }


                if (Session["KeyVal2"] != null)
                {

                    popupScript = "<script language='javascript'>" + "alert('Saved Successfully');window.location ='Contact_Document.aspx';</script>";


                    ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
                }
                else
                {

                    string previousPageUrl = Convert.ToString(ViewState["previousPageUrl"]);

                    popupScript = "<script language='javascript'>" + "alert('Saved Successfully');window.parent.location.href='" + previousPageUrl + "';window.parent.popup.Hide();</script>";
                    ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
                }



            }
            else
            {
                string DocumentID = "";
                if (Request.QueryString["formtype"] != null)
                    DocumentID = Convert.ToString(Session["InternalId"]);
                else
                {
                    if (Session["InternalId"] != null)
                    {
                        if (Session["InternalId"] != "Edit")
                        {
                            DocumentID = Convert.ToString(Session["InternalId"]);
                        }
                        else
                        {
                            if (Convert.ToString(HttpContext.Current.Session["userlastsegment"]) == "10")
                            {
                                DocumentID = Convert.ToString(HttpContext.Current.Session["CdslClients_BOID"]);//session
                            }
                            else
                            {
                                DocumentID = Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]);//session
                            }
                        }
                    }
                    else
                    {
                        if (Request.QueryString["FormNo"] != null)
                        {
                            DocumentID = Convert.ToString(Request.QueryString["FormNo"]);
                        }
                        else
                        {
                            if (Request.QueryString["Page"] != null)
                            {
                                if (Convert.ToString(Request.QueryString["Page"]) == "branch")
                                {
                                    if (Session["branch_InternalId"] != null)
                                        DocumentID = Convert.ToString(Session["branch_InternalId"]);
                                }

                            }
                            else if (HttpContext.Current.Session["KeyVal_InternalID"] != null)
                            {
                                DocumentID = Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]);//session
                            }

                        }
                    }
                }
                string building1 = "";
                if (Building.SelectedItem.Text == "Select")
                {
                    building1 = "0";

                }
                else
                {
                    building1 = Building.SelectedItem.Value;
                }
                Int32 CreateUser = Int32.Parse(Convert.ToString(HttpContext.Current.Session["userid"]));


                string CreateDate = Convert.ToDateTime(oDBEngine.GetDate().ToString()).ToString("yyyy-MM-dd hh:mmm:ss");
                //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
                string FName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                if (FName != "")
                {

                    string sd;// = objConverter.GetAutoGenerateNoForFileUpload();

                    string datetd = Convert.ToString(Convert.ToDateTime(DateTime.Now));
                    if (datetd.Contains("/"))
                    { sd = objConverter.GetAutoGenerateNo(); }
                    else
                        sd = objConverter.GetAutoGenerateNoForFileUpload();

                    string filename = Convert.ToString(HttpContext.Current.Session["userid"]) + sd + FName;
                    string FLocation = Server.MapPath("../Documents/") + filename;

                    FolderName = path + "\\" + folderid + "\\" + year;

                    string total = FolderName + "\\" + filename;

                    FileUpload1.PostedFile.SaveAs(total);


                    if (Request.QueryString["FormNo"] == null && Request.QueryString["SpecialType"] == null)
                    {
                        if (dtRenew.Value == null)
                        {
                            if (Building.SelectedIndex == 0)
                            {
                                objEngine.InsurtFieldValue("tbl_master_document", "doc_contactId,doc_documentTypeId,doc_documentName,doc_source,doc_Floor,doc_RoomNo,doc_CellNo,doc_FileNo,CreateDate,CreateUser,doc_receivedate,doc_Note1,doc_Note2", "'" + DocumentID + "','" + DTYpe.SelectedItem.Value + "','" + TxtName.Text + "','" + DTYpe.SelectedItem.Value.ToString() + "/" + year + "/" + filename + "','" + TxtFloorNo.Text + "','" + TxtRoomNo.Text + "','" + TxtCellNo.Text + "','" + TxtfileNo.Text + "','" + CreateDate.ToString() + "','" + CreateUser + "','" + Convert.ToDateTime(dtReceived.Value.ToString()).ToString("yyyy-MM-dd") + "','" + txt_note1.Text.ToString().Trim() + "','" + txt_note2.Text.ToString().Trim() + "'");

                            }
                            else
                            {
                                objEngine.InsurtFieldValue("tbl_master_document", "doc_contactId,doc_documentTypeId,doc_documentName,doc_source,doc_buildingId,doc_Floor,doc_RoomNo,doc_CellNo,doc_FileNo,CreateDate,CreateUser,doc_receivedate,doc_Note1,doc_Note2", "'" + DocumentID + "','" + DTYpe.SelectedItem.Value + "','" + TxtName.Text + "','" + DTYpe.SelectedItem.Value.ToString() + "/" + year + "/" + filename + "','" + building1 + "','" + TxtFloorNo.Text + "','" + TxtRoomNo.Text + "','" + TxtCellNo.Text + "','" + TxtfileNo.Text + "','" + CreateDate.ToString() + "','" + CreateUser + "','" + Convert.ToDateTime(dtReceived.Value.ToString()).ToString("yyyy-MM-dd") + "','" + txt_note1.Text.ToString().Trim() + "','" + txt_note2.Text.ToString().Trim() + "'");
                            }
                        }
                        else
                            objEngine.InsurtFieldValue("tbl_master_document", "doc_contactId,doc_documentTypeId,doc_documentName,doc_source,doc_buildingId,doc_Floor,doc_RoomNo,doc_CellNo,doc_FileNo,CreateDate,CreateUser,doc_receivedate,doc_renewdate,doc_Note1,doc_Note2", "'" + DocumentID + "','" + DTYpe.SelectedItem.Value + "','" + TxtName.Text + "','" + DTYpe.SelectedItem.Value.ToString() + "/" + DocumentID + "~" + filename + "','" + building1 + "','" + TxtFloorNo.Text + "','" + TxtRoomNo.Text + "','" + TxtCellNo.Text + "','" + TxtfileNo.Text + "','" + CreateDate.ToString() + "','" + CreateUser + "','" + Convert.ToDateTime(dtReceived.Value.ToString()).ToString("yyyy-MM-dd") + "','" + Convert.ToDateTime(dtRenew.Value.ToString()).ToString("yyyy-MM-dd") + "','" + txt_note1.Text.ToString().Trim() + "','" + txt_note2.Text.ToString().Trim() + "'");
                    }
                    else if (Request.QueryString["SpecialType"] != null && Convert.ToString(Request.QueryString["SpecialType"]) == "CashBank")
                    {

                        BusinessLogicLayer.GenericXML oGenericXML = new BusinessLogicLayer.GenericXML();
                        string[] ColumnName = new string[] { "doc_contactId", "doc_documentTypeId", "doc_documentName", "doc_source", "doc_buildingId", "doc_Floor", "doc_RoomNo", "doc_CellNo", "doc_FileNo", "CreateDate", "CreateUser", "doc_receivedate", "doc_Note1", "doc_Note2", "ApplicableFor" };
                        string[] ColumnType = new string[] { "S", "S", "S", "S", "S", "S", "S", "S", "S", "S", "S", "S", "S", "S", "S" };
                        string[] ColumnValue = new string[] { DocumentID, DTYpe.SelectedItem.Value.ToString(), TxtName.Text, Convert.ToString(DTYpe.SelectedItem.Value.ToString() + "/" + year + "~" + DTYpe.SelectedItem.Value.ToString() + "/" + year + "/" + filename).Split('~')[1], building1, TxtFloorNo.Text, TxtRoomNo.Text, TxtCellNo.Text, TxtfileNo.Text, CreateDate.ToString(), CreateUser.ToString(), dtReceived.Value.ToString(), txt_note1.Text.ToString().Trim(), txt_note2.Text.ToString().Trim(), Request.QueryString["id1"].ToString() };
                        string TableName = "KRADocRecord";
                        Guid obj = Guid.NewGuid();
                        string DocXMLPath = "";
                        if (Request.QueryString["Mode"] == null)
                        {
                            if (Session["NewCBDocPath"] == null)
                            {
                                DocXMLPath = "../Documents/" + obj.ToString() + "_" + DateTime.Now.ToString("dd/MM/yyyy").Replace("/", "");

                                Session["NewCBDocPath"] = DocXMLPath;

                            }
                            else
                            {
                                DocXMLPath = Convert.ToString(Session["NewCBDocPath"]);

                            }

                            oGenericXML.Add_XML(DocXMLPath, ColumnName, ColumnType, ColumnValue, TableName);
                            Session["CashBankDocumentsPaths"] = DocXMLPath;
                        }
                        else
                        {
                            DocXMLPath = Convert.ToString(Session["NewCBDocPath"]);

                            try
                            {
                                string[] ss = DocXMLPath.Split('\\');
                                DocXMLPath = "../Documents/" + ss[ss.Length - 1];
                                oGenericXML.Update_XML(DocXMLPath, ColumnName, ColumnType, ColumnValue, TableName, "RowID", Convert.ToString(ViewState["strXmlKeyDocCB"]));
                            }
                            catch (Exception ex)
                            {
                                oGenericXML.Add_XML(DocXMLPath, ColumnName, ColumnType, ColumnValue, TableName);
                            }

                        }



                        string popupScrpt = "<script language='javascript'>" + "alert('Saved Successfully');window.parent.popup.Hide();</script>";
                        ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScrpt);
                    }
                    else
                    {

                        BusinessLogicLayer.GenericXML oGenericXML = new BusinessLogicLayer.GenericXML();
                        string[] ColumnName = new string[] { "doc_contactId", "doc_documentTypeId", "doc_documentName", "doc_source", "doc_buildingId", "doc_Floor", "doc_RoomNo", "doc_CellNo", "doc_FileNo", "CreateDate", "CreateUser", "doc_receivedate", "doc_Note1", "doc_Note2", "ApplicableFor" };
                        string[] ColumnType = new string[] { "S", "S", "S", "S", "S", "S", "S", "S", "S", "S", "S", "S", "S", "S", "S" };
                        string[] ColumnValue = new string[] { DocumentID, DTYpe.SelectedItem.Value.ToString(), TxtName.Text, DTYpe.SelectedItem.Value.ToString() + "/" + year + "~" + DTYpe.SelectedItem.Value.ToString() + "/" + year + "/" + filename, building1, TxtFloorNo.Text, TxtRoomNo.Text, TxtCellNo.Text, TxtfileNo.Text, CreateDate.ToString(), CreateUser.ToString(), dtReceived.Value.ToString(), txt_note1.Text.ToString().Trim(), txt_note2.Text.ToString().Trim(), Request.QueryString["id1"].ToString() };
                        string TableName = "KRADocRecord";
                        string DocXMLPath = "../KRA/KRADoc/" + "KRADocRecords_" + DocumentID;
                        oGenericXML.Add_XML(DocXMLPath, ColumnName, ColumnType, ColumnValue, TableName);
                        string popupScrpt = "<script language='javascript'>" + "alert('Successfully Uploaded');window.parent.popup.Hide();</script>";
                        ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScrpt);
                    }

                }
                else
                {
                    if (dtRenew.Value == null)
                        objEngine.InsurtFieldValue("tbl_master_document", "doc_contactId,doc_documentTypeId,doc_documentName,doc_buildingId,doc_Floor,doc_RoomNo,doc_CellNo,doc_FileNo,CreateDate,CreateUser,doc_receivedate,doc_Note1,doc_Note2", "'" + DocumentID + "','" + DTYpe.SelectedItem.Value + "','" + TxtName.Text + "','" + building1 + "','" + TxtFloorNo.Text + "','" + TxtRoomNo.Text + "','" + TxtCellNo.Text + "','" + TxtfileNo.Text + "','" + Convert.ToDateTime(CreateDate).ToString("yyyy-MM-dd") + "','" + CreateUser + "','" + Convert.ToDateTime(dtReceived.Value).ToString("yyyy-MM-dd") + "','" + txt_note1.Text.ToString().Trim() + "','" + txt_note2.Text.ToString().Trim() + "'");
                    else
                        objEngine.InsurtFieldValue("tbl_master_document", "doc_contactId,doc_documentTypeId,doc_documentName,doc_buildingId,doc_Floor,doc_RoomNo,doc_CellNo,doc_FileNo,CreateDate,CreateUser,doc_receivedate,doc_renewdate,doc_Note1,doc_Note2", "'" + DocumentID + "','" + DTYpe.SelectedItem.Value + "','" + TxtName.Text + "','" + building1 + "','" + TxtFloorNo.Text + "','" + TxtRoomNo.Text + "','" + TxtCellNo.Text + "','" + TxtfileNo.Text + "','" + Convert.ToDateTime(CreateDate).ToString("yyyy-MM-dd") + "','" + CreateUser + "','" + Convert.ToDateTime(dtReceived.Value).ToString("yyyy-MM-dd") + "','" + Convert.ToDateTime(dtRenew.Value).ToString("yyyy-MM-dd") + "','" + txt_note1.Text.ToString().Trim() + "','" + txt_note2.Text.ToString().Trim() + "'");

                }
                string popupScript = "";
                string query = Convert.ToString(Request.QueryString["id"]);
                if (Request.QueryString["mode"] != null)
                {
                    query = query + "&mode=" + Convert.ToString(Request.QueryString["mode"]);
                }


                if (Session["KeyVal2"] != null)
                {
                    popupScript = "<script language='javascript'>" + "alert('Saved Successfully');window.parent.Getvalue();window.parent.popup.Hide();</script>";
                    ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
                }
                else
                {
                    string previousPageUrl = Convert.ToString(ViewState["previousPageUrl"]);
                    popupScript = "<script language='javascript'>" + "alert('Saved Successfully');window.parent.location.href='" + previousPageUrl + "';window.parent.popup.Hide();</script>";
                    ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
                }
            }
        }
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            divfile.Visible = false;
            FileUpload1.Visible = true;
            LinkButton1.Visible = false;
            IsFileUpload.Value = "Y";

        }
    }
}