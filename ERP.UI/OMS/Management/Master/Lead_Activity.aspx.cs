using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
using System.Web.Services;
using System.Collections.Generic;
using DevExpress.Utils.OAuth.Provider;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_Lead_Activity : ERP.OMS.ViewState_class.VSPage
    {
        //string HttpContext.Current.Session["KeyVal_InternalID"];

        //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        //Converter oConverter = new Converter();
        //Converter OConvert = new Converter();

        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Converter oConverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
        clsDropDownList oclsDropDownList = new clsDropDownList();
        Employee_BL objemployeebal = new Employee_BL();


        protected void Page_Load(object sender, EventArgs e)
        {
            // Code  Added  By Priti on 13122016 to convert.Tostring instead of ToString()
            #region dtime1

            txtStartDate.TimeSectionProperties.Visible = true;
            txtStartDate.UseMaskBehavior = true;
            txtStartDate.EditFormatString = "dd/MM/yyyy hh:mm tt";
            //txtStartDate.DisplayFormatString = "None";
            #endregion

            #region dtime2
            txtEndDate.TimeSectionProperties.Visible = true;
            txtEndDate.UseMaskBehavior = true;
            txtEndDate.EditFormatString = "dd/MM/yyyy hh:mm tt";
            //txtEndDate.DisplayFormatString = "None";
            #endregion

            #region dtime3
            txtNextDate.TimeSectionProperties.Visible = true;
            txtNextDate.UseMaskBehavior = true;
            txtNextDate.EditFormatString = "dd/MM/yyyy hh:mm tt";
            //txtNextDate.DisplayFormatString = "None";
            #endregion

            if (Session["requesttype"] != null)
            {
                //lblHeadTitle.Text = Session["requesttype"].ToString();
                lblHeadTitle.Text = Convert.ToString(Session["requesttype"]) + " - Create Activity";
                bindheadername();
            }


            cmbCategory.Attributes.Add("OnChange", "VisibilityOnOff(this.value)");
            //cmbCategory.SelectedIndex = 1;
            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            ////this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

            //BTNClose.Attributes.Add("onclick", "return ul();"); 
            //ShowPanel.Visible = false;
            if (!IsPostBack)
            {
                string previousPageUrl = string.Empty;
                if (Request.UrlReferrer != null)
                    previousPageUrl = Request.UrlReferrer.AbsoluteUri;
                else
                    previousPageUrl = Page.ResolveUrl("~/OMS/Management/ProjectMainPage.aspx");

                ViewState["previousPageUrl"] = previousPageUrl;
                goBackCrossBtn.NavigateUrl = previousPageUrl;

                //  pageprevurl = Page.PreviousPage.ToString();
                //GridCategory.Visible = false;
                // BtnADD.Attributes.Add("onclick", "return Validation();");
                HttpContext.Current.Session["KeyVal_InternalID"] = Request.QueryString["ID"];

                //string AllUserList = oDBEngine.getChildUser_for_report(HttpContext.Current.Session["userid"].ToString(), "");
                string AllUserList = oDBEngine.getChildUser_for_report(Convert.ToString(HttpContext.Current.Session["userid"]), "");
                //AllUserList += HttpContext.Current.Session["userid"].ToString();
                AllUserList += Convert.ToString(HttpContext.Current.Session["userid"]);
                

                //string[,] Data =new string([]) []{};
                //string[,] Data = oDBEngine.GetFieldValue("tbl_master_activitytype", "aty_id, aty_activityType", null, 2, "aty_activityType");
                //if (Data[0, 0] != "n")
                //{
                //    oDBEngine.AddDataToDropDownList(Data, cmbActType);
                //    cmbActType.SelectedValue = "1";
                //}
                // ............. Code Commented by Sam on 07112016 due to  get user its own cnt_internalid....................
                string owninterid = Convert.ToString(HttpContext.Current.Session["owninternalid"]);
                DataTable dtbl = new DataTable();
                dtbl = objemployeebal.GetAssignedEmployeeDetailByReportingTo(owninterid);
                if (dtbl != null && dtbl.Rows.Count > 0)
                {
                    cmbAssignTo.DataTextField = "name";
                    cmbAssignTo.DataValueField = "cnt_id";
                    cmbAssignTo.DataSource = dtbl;
                    cmbAssignTo.DataBind();
                    
                }
                //string[,] Data = oDBEngine.GetFieldValue("tbl_master_user", "user_id,user_name", "user_id in(" + AllUserList + ")", 2, "user_name");
                //if (Data[0, 0] != "n")
                //{
                //    oclsDropDownList.AddDataToDropDownList(Data, cmbAssignTo);
                //}
                //string[,] Data = oDBEngine.GetFieldValue("tbl_master_user", "user_id,user_name", "user_id in(" + AllUserList + ")", 2, "user_name");
                //if (Data[0, 0] != "n")
                //{ 
                //    oclsDropDownList.AddDataToDropDownList(Data, cmbAssignTo);
                //}
                // ............. Code Above Commented by Sam on 07112016 due to  get user its own cnt_internalid....................
                string[,] Data = oDBEngine.GetFieldValue("tbl_master_address", "add_id,ISNULL(add_address1, '') + ' ' + ISNULL(add_address2, '') + ' ' + ISNULL(add_address3, '') AS ADDRESS", "add_cntId ='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'", 2, "add_address1");
                if (Data[0, 0] != "n")
                {
                    //oDBEngine.AddDataToDropDownList(Data, drpVisitPlace);
                    oclsDropDownList.AddDataToDropDownList(Data, drpVisitPlace);
                    drpVisitPlace.SelectedIndex = -1;
                }
                if (Data[0, 0] != "n")
                {
                    //oDBEngine.AddDataToDropDownList(Data, drpVisitPlace);
                    oclsDropDownList.AddDataToDropDownList(Data, drpVisitPlace);
                    drpVisitPlace.SelectedIndex = -1;
                }
                //___________Creating Session DataTable to hold Category Data______//
                Session["DataTable_Lead_Activity"] = new DataTable();

                Body.Visible = true;
                panelinside.Visible = false;
                //______________________Checking wether Activity has been generated or not___________//
                CheckActivity();
                txtStartDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                txtEndDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                txtNextDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());

                txtStartDate.EditFormatString = OConvert.GetDateFormat("DateTime");
                txtEndDate.EditFormatString = OConvert.GetDateFormat("DateTime");
                txtNextDate.EditFormatString = OConvert.GetDateFormat("DateTime");
                string productType = "";
                //productType = cmbCategory.SelectedItem.Value.ToString();
                productType = Convert.ToString(cmbCategory.SelectedItem.Value);
                Page.ClientScript.RegisterStartupScript(GetType(), "visit", "<script language='JavaScript'>VisibilityOnOff('" + productType + "');</script>");
                txtbranch.Attributes.Add("onkeyup", "SearchByBranchName(this,'BranchName',event)");
                rdClient.Attributes.Add("onclick", "BranchOrClient(this);");
                rdBranch.Attributes.Add("onclick", "BranchOrClient(this);");
            }

            lblmessage1.Text = "";
            lblmessage.Text = "";
            txtProduct.Attributes.Add("onkeyup", "CallList(this,'getProductByLetters',event)");
            txtCompany.Attributes.Add("onkeyup", "CallList1(this,'getCompanyByLetters',event)");
            DateTime dt = oDBEngine.GetDate();

        }

        public void bindheadername()
        {
            //if (Convert.ToString(Request.QueryString["type"]) == "Product")
            //{
                int cntid = Convert.ToInt32(Convert.ToString(Request.QueryString["cnt_id"]));
                string msg = " [ Short Name:";
                // Code  Added and Commented By Priti on 13122016 to add column cnt_UCC instead of cnt_shortname
               // DataTable dt = oDBEngine.GetDataTable(" Select ISNULL(con.cnt_firstName, 'NA') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' +  ISNULL(con.cnt_lastName, '') as name,cnt_shortname=case when cnt_shortname='' then 'NA' when cnt_shortname IS null then 'NA'  else cnt_shortname end  from tbl_master_contact con where cnt_id=" + cntid + " ");
                DataTable dt = oDBEngine.GetDataTable(" Select ISNULL(con.cnt_firstName, 'NA') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' +  ISNULL(con.cnt_lastName, '') as name,cnt_UCC=case when cnt_UCC='' then 'NA' when cnt_UCC IS null then 'NA'  else cnt_UCC end  from tbl_master_contact con where cnt_id=" + cntid + " ");
               //...........end..................
               if (dt != null && dt.Rows.Count > 0)
                {
                    msg = msg + Convert.ToString(dt.Rows[0]["cnt_UCC"]) + ", Name:";
                    msg = msg + Convert.ToString(dt.Rows[0]["name"]) + " ]";
                    string msg1 = lblHeadTitle.Text;
                    lblHeadTitle.Text = msg1 + msg;
                }
                //lblheader
                //HeaderText.InnerText = "Add " + Convert.ToString(Request.QueryString["type"]) + " Document";
                //Session["PrePageRedirect"] = "/OMS/management/store/Master/sProducts.aspx";
             

        }

        private void CheckActivity()
        {
            // Code  Added  By Priti on 13122016 to convert.Tostring instead of ToString()
            //throw new Exception("The method or operation is not implemented.");
            if (cmbActType.SelectedItem.Value != "")
            {
                //  DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                string[,] data;
                //int ActType = int.Parse(cmbActType.SelectedItem.Value.ToString());
                int ActType = int.Parse(Convert.ToString(cmbActType.SelectedItem.Value));
                switch (ActType)
                {
                    case 1:
                        data = oDBEngine.GetFieldValue("tbl_trans_phonecall", "phc_id", "phc_leadcotactid='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'", 1);
                        if (data[0, 0] != "n")
                        {
                            Response.Write("<script language='javaScript'> alert('You have already create the phonecall activity for this lead !!') </script>");
                            Body.Visible = false;
                        }
                        break;
                    case 4:
                        data = oDBEngine.GetFieldValue("tbl_trans_salesvisit", "slv_id", "slv_leadcotactid='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'", 1);
                        if (data[0, 0] != "n")
                        {
                            Response.Write("<script language='javaScript'> alert('You have already create the salesvisit activity for this lead !!') </script>");
                            Body.Visible = false;
                        }
                        break;
                    case 6:
                        data = oDBEngine.GetFieldValue("tbl_trans_sales", "sls_id", "sls_contactlead_id='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'", 1);
                        if (data[0, 0] != "n")
                        {
                            Response.Write("<script language='javaScript'> alert('You have already create the sales activity for this lead !!') </script>");
                            Body.Visible = false;
                        }
                        break;
                    default:

                        break;
                }
            }

        }

        protected void cmbActType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Code  Added  By Priti on 13122016 to convert.Tostring instead of ToString()
            if (cmbActType.SelectedItem.Value != "")
            {
                //int ActType = int.Parse(cmbActType.SelectedItem.Value.ToString());
                int ActType = int.Parse(Convert.ToString(cmbActType.SelectedItem.Value));
                switch (ActType)
                {
                    case 1:
                        Body.Visible = true;
                        panelinside.Visible = false;
                        //______________________Checking wether Activity has been generated or not___________//
                        CheckActivity();
                        break;
                    case 4:
                        Body.Visible = true;
                        panelinside.Visible = true;
                        //______________________Checking wether Activity has been generated or not___________//
                        CheckActivity();
                        break;
                    case 6:
                        Body.Visible = true;
                        panelinside.Visible = true;
                        //______________________Checking wether Activity has been generated or not___________//
                        CheckActivity();
                        break;
                    default:
                        Body.Visible = false;
                        break;
                }
                string productType = "";
                //productType = cmbCategory.SelectedItem.Value.ToString();
                productType = Convert.ToString(cmbCategory.SelectedItem.Value);
                Page.ClientScript.RegisterStartupScript(GetType(), "visit1", "<script language='JavaScript'>VisibilityOnOff('" + productType + "');</script>");

            }
            else
            {
                Body.Visible = false;
                panelinside.Visible = false;
            }
        }
        protected void BTNSave_click(object sender, EventArgs e)
        {
            // Code  Added  By Priti on 13122016 to convert.Tostring instead of ToString()
            string txtStartTime = Convert.ToDateTime(txtStartDate.Value).ToShortTimeString();
            txtStartDate.Value = Convert.ToDateTime(txtStartDate.Value).ToShortDateString();
            string txtEndTime = Convert.ToDateTime(txtEndDate.Value).ToShortTimeString();
            txtEndDate.Value = Convert.ToDateTime(txtEndDate.Value).ToShortDateString();
            txtNextDate.Value = txtNextDate.Value;
            //string LeadID1 = HttpContext.Current.Session["KeyVal_InternalID"].ToString();
            string LeadID1 = Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]);
            string LeadId = LeadID1.Substring(0, 2);
            //int ActType = int.Parse(cmbActType.SelectedItem.Value.ToString());
            int ActType = int.Parse(Convert.ToString(cmbActType.SelectedItem.Value));
            //   DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            string NewID = "";
            switch (ActType)
            {
                case 1:

                    NewID = oDBEngine.GetInternalId("PC", "tbl_trans_Activies", "act_activityNo", "act_activityNo");
                    string Values = HttpContext.Current.Session["userbranchID"] + "," + cmbActType.SelectedItem.Value + ",'" + txtDescription.Text + "'," + HttpContext.Current.Session["userid"] + "," + cmbAssignTo.SelectedItem.Value + ", getdate() ,'" + txtStartDate.Value + "','" + txtEndDate.Value + "','" + NewID + "'," + cmbPriority.SelectedItem.Value + ",'" + txtStartTime + "','" + txtEndTime + "','" + txtInstructionNotes.Text + "','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'";
                    int rowEffected = oDBEngine.InsurtFieldValue("tbl_trans_Activies", "act_branchId,act_activityType,act_description,act_assignedBy,act_assignedTo,act_createDate,act_scheduledDate,act_expectedDate,act_activityno,act_priority,act_scheduledTime,act_expectedTime,act_instruction,CreateDate,CreateUser", Values);
                    string[,] act_id = oDBEngine.GetFieldValue("tbl_trans_Activies", "act_id", "act_activityno='" + NewID + "'", 1);
                    rowEffected = oDBEngine.InsurtFieldValue("tbl_trans_phonecall", "phc_activityId,phc_branchId,phc_leadcotactId,CreateDate,CreateUser", act_id[0, 0] + "," + HttpContext.Current.Session["userbranchID"] + ",'" + HttpContext.Current.Session["KeyVal_InternalID"] + "','" + Convert.ToString(oDBEngine.GetDate()) + "'," + HttpContext.Current.Session["userid"]);
                    if (LeadId == "LD")
                    {
                        rowEffected = oDBEngine.SetFieldValue("tbl_master_lead", "cnt_status='" + NewID + "', cnt_lead_stage=2,cnt_userAccess = cnt_userAccess + '" + cmbAssignTo.SelectedItem.Value + "'", " cnt_internalId ='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'");
                    }
                    else
                    {
                        rowEffected = oDBEngine.SetFieldValue("tbl_master_contact", "cnt_status='" + NewID + "', cnt_lead_stage=2", " cnt_internalId ='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'");
                    }
                    //________add message here___//
                    //oDBEngine.messageTableUpdate(cmbAssignTo.SelectedItem.Value.ToString(), HttpContext.Current.Session["userid"].ToString(), "Phone Call", txtStartDate.Value.ToString(), txtEndDate.Value.ToString(), cmbPriority.SelectedItem.Text, txtInstructionNotes.Text, act_id[0, 0], "activity");
                    oDBEngine.messageTableUpdate(Convert.ToString(cmbAssignTo.SelectedItem.Value), Convert.ToString(HttpContext.Current.Session["userid"]), "Phone Call", Convert.ToString(txtStartDate.Value), Convert.ToString(txtEndDate.Value), cmbPriority.SelectedItem.Text, txtInstructionNotes.Text, act_id[0, 0], "activity");
                    break;
                case 4:
                    DataTable DT = (DataTable)Session["DataTable_Lead_Activity"];
                    //if (cmbCategory.SelectedItem.Value.ToString().Trim() == "Broking & DP Account")
                    //{
                    //}
                    if (DT.Rows.Count != 0)
                    {
                        NewID = oDBEngine.GetInternalId("SW", "tbl_trans_Activies", "act_activityNo", "act_activityNo");
                        Values = HttpContext.Current.Session["userbranchID"] + "," + cmbActType.SelectedItem.Value + ",'" + txtDescription.Text + "'," + HttpContext.Current.Session["userid"] + "," + cmbAssignTo.SelectedItem.Value + ", '" + oDBEngine.GetDate().ToShortDateString() + "' ,'" + txtStartDate.Value + "','" + txtEndDate.Value + "','" + NewID + "'," + cmbPriority.SelectedItem.Value + ",'" + txtStartTime + "','" + txtEndTime + "','" + Convert.ToString(oDBEngine.GetDate()) + "','" + Convert.ToString(Session["userid"]) + "'";
                        rowEffected = oDBEngine.InsurtFieldValue("tbl_trans_Activies", "act_branchId,act_activityType,act_description,act_assignedBy,act_assignedTo,act_createDate,act_scheduledDate,act_expectedDate,act_activityno,act_priority,act_scheduledTime,act_expectedTime,CreateDate,CreateUser", Values);
                        act_id = oDBEngine.GetFieldValue("tbl_trans_Activies", "act_id", "act_activityno='" + NewID + "'", 1);
                        string NextVisitPlace = "";
                        string PlaceType = "";
                        if (rdClient.Checked == true)
                        {
                            PlaceType = "C";
                            //NextVisitPlace = drpVisitPlace.SelectedItem.Value.ToString();
                            if (drpVisitPlace.Items.Count != 0)//code added by priti on 13122016 due to null value avoid
                            {
                                NextVisitPlace = Convert.ToString(drpVisitPlace.SelectedItem.Value);
                            }
                            
                        }
                        else if (rdBranch.Checked == true)
                        {
                            PlaceType = "B";
                            //NextVisitPlace = txtbranch_hidden.Value.ToString();
                            NextVisitPlace = Convert.ToString(txtbranch_hidden.Value);
                        }
                        rowEffected = oDBEngine.InsurtFieldValue("tbl_trans_salesVisit", "slv_activityId,slv_branchid,slv_leadcotactId,slv_salesvisitoutcome,slv_nextvisitdatetime,slv_nextvisitplace,slv_nextplacetype,CreateDate,CreateUser", act_id[0, 0] + "," + HttpContext.Current.Session["userbranchID"] + ",'" + HttpContext.Current.Session["KeyVal_InternalID"] + "','9','" + txtNextDate.Value + "','" + oDBEngine.GetDate().ToString() + "','" + NextVisitPlace.ToString().Trim() + "','" + PlaceType.ToString().Trim() + "','" + HttpContext.Current.Session["userid"] + "'");
                        string[,] act_slv_id = oDBEngine.GetFieldValue("tbl_trans_salesVisit", "slv_id", "slv_activityId='" + act_id[0, 0] + "'", 1);
                        DataTable DTcategory = new DataTable();
                        DTcategory = (DataTable)Session["DataTable_Lead_Activity"];

                        for (int i = 0; i <= DTcategory.Rows.Count - 1; i++)
                        {
                            string product = Convert.ToString(DTcategory.Rows[i][2]);
                            string productid = "";
                            if (product != "")
                            {
                                string[,] pdid = oDBEngine.GetFieldValue("tbl_Master_Products", "prds_internalId", "prds_description='" + product.Trim() + "'", 1);
                                productid = pdid[0, 0];
                            }
                            string PAmount = Convert.ToString(DTcategory.Rows[i][1]);
                            if (PAmount != "")
                            {
                                PAmount = Convert.ToString(DTcategory.Rows[i][1]);
                            }
                            else
                            {
                                PAmount = "0";
                            }
                            rowEffected = oDBEngine.InsurtFieldValue("tbl_trans_offeredProduct", "ofp_leadId, ofp_productTypeId, ofp_productId, ofp_probableAmount,ofp_activityId", "'" + HttpContext.Current.Session["KeyVal_InternalID"] + "','" + Convert.ToString(DTcategory.Rows[i][0]) + "','" + productid + "','" + PAmount + "','" + NewID + "'");
                            if (rdClient.Checked == true)
                            {
                                if (drpVisitPlace.Items.Count != 0)//code added by priti on 13122016 due to null value avoid
                                {
                                    rowEffected = oDBEngine.SetFieldValue(" tbl_master_address ", " add_activityId=" + act_slv_id[0, 0], " add_id= " + drpVisitPlace.SelectedItem.Value);

                                }
                            }
                        }
                        if (LeadId == "LD")
                        {
                            rowEffected = oDBEngine.SetFieldValue("tbl_master_lead", "cnt_status='" + NewID + "',cnt_userAccess = cnt_userAccess + '" + cmbAssignTo.SelectedItem.Value + "', cnt_lead_stage=3", " cnt_internalid='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'");
                        }
                        else
                        {
                            rowEffected = oDBEngine.SetFieldValue("tbl_master_contact", "cnt_status='" + NewID + "', cnt_lead_stage=3", " cnt_internalId ='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'");
                        }
                        //________add message here___//
                        //oDBEngine.messageTableUpdate(cmbAssignTo.SelectedItem.Value, HttpContext.Current.Session["userid"].ToString(), "Sales Visit", txtStartDate.Value.ToString(), txtEndDate.Value.ToString(), cmbPriority.SelectedItem.Text, txtInstructionNotes.Text, act_id[0, 0], "activity");
                        oDBEngine.messageTableUpdate(cmbAssignTo.SelectedItem.Value, Convert.ToString(HttpContext.Current.Session["userid"]), "Sales Visit", Convert.ToString(txtStartDate.Value), Convert.ToString(txtEndDate.Value), cmbPriority.SelectedItem.Text, txtInstructionNotes.Text, act_id[0, 0], "activity");
                    }
                    else
                    {
                        //if (cmbCategory.SelectedItem.Value.ToString().Trim() == "Broking & DP Account" || cmbCategory.SelectedItem.Value.ToString().Trim() == "Sub Broker" || cmbCategory.SelectedItem.Value.ToString().Trim() == "Relationship Partner")
                        if (Convert.ToString(cmbCategory.SelectedItem.Value).Trim() == "Broking & DP Account" || Convert.ToString(cmbCategory.SelectedItem.Value).Trim() == "Sub Broker" || Convert.ToString(cmbCategory.SelectedItem.Value).Trim() == "Relationship Partner")
                        {
                            LoadAllProductCat();
                            NewID = oDBEngine.GetInternalId("SW", "tbl_trans_Activies", "act_activityNo", "act_activityNo");
                            Values = HttpContext.Current.Session["userbranchID"] + "," + cmbActType.SelectedItem.Value + ",'" + txtDescription.Text + "'," + HttpContext.Current.Session["userid"] + "," + cmbAssignTo.SelectedItem.Value + ", '" + oDBEngine.GetDate().ToShortDateString() + "' ,'" + txtStartDate.Value + "','" + txtEndDate.Value + "','" + NewID + "'," + cmbPriority.SelectedItem.Value + ",'" + txtStartTime + "','" + txtEndTime + "','" + Convert.ToString(oDBEngine.GetDate()) + "','" + Convert.ToString(Session["userid"]) + "'";
                            rowEffected = oDBEngine.InsurtFieldValue("tbl_trans_Activies", "act_branchId,act_activityType,act_description,act_assignedBy,act_assignedTo,act_createDate,act_scheduledDate,act_expectedDate,act_activityno,act_priority,act_scheduledTime,act_expectedTime,CreateDate,CreateUser", Values);
                            act_id = oDBEngine.GetFieldValue("tbl_trans_Activies", "act_id", "act_activityno='" + NewID + "'", 1);
                            string NextVisitPlace = "";
                            string PlaceType = "";
                            if (rdClient.Checked == true)
                            {
                                PlaceType = "C";
                                //NextVisitPlace = drpVisitPlace.SelectedItem.Value.ToString();
                                if (drpVisitPlace.Items.Count != 0)
                                {
                                    NextVisitPlace = Convert.ToString(drpVisitPlace.SelectedItem.Value);
                                }
                               
                            }
                            else if (rdBranch.Checked == true)
                            {
                                PlaceType = "B";
                                //NextVisitPlace = txtbranch_hidden.Value.ToString();
                                NextVisitPlace = Convert.ToString(txtbranch_hidden.Value);
                            }
                            rowEffected = oDBEngine.InsurtFieldValue("tbl_trans_salesVisit", "slv_activityId,slv_branchid,slv_leadcotactId,slv_salesvisitoutcome,slv_nextvisitdatetime,slv_nextvisitplace,slv_nextplacetype,CreateDate,CreateUser", act_id[0, 0] + "," + HttpContext.Current.Session["userbranchID"] + ",'" + HttpContext.Current.Session["KeyVal_InternalID"] + "','9','" + txtNextDate.Value + "','" + Convert.ToString(NextVisitPlace).Trim() + "','" + Convert.ToString(PlaceType).Trim() + "','" + Convert.ToString(oDBEngine.GetDate()) + "','" + HttpContext.Current.Session["userid"] + "'");
                            string[,] act_slv_id = oDBEngine.GetFieldValue("tbl_trans_salesVisit", "slv_id", "slv_activityId='" + act_id[0, 0] + "'", 1);
                            DataTable DTcategory = new DataTable();
                            DTcategory = (DataTable)Session["DataTable_Lead_Activity"];

                            for (int i = 0; i <= DTcategory.Rows.Count - 1; i++)
                            {
                                string product = Convert.ToString(DTcategory.Rows[i][2]);
                                string productid = "";
                                if (product != "")
                                {
                                    string[,] pdid = oDBEngine.GetFieldValue("tbl_Master_Products", "prds_internalId", "prds_description='" + product.Trim() + "'", 1);
                                    productid = pdid[0, 0];
                                }
                                string PAmount = Convert.ToString(DTcategory.Rows[i][1]);
                                if (PAmount != "")
                                {
                                    PAmount = Convert.ToString(DTcategory.Rows[i][1]);
                                }
                                else
                                {
                                    PAmount = "0";
                                }
                                rowEffected = oDBEngine.InsurtFieldValue("tbl_trans_offeredProduct", "ofp_leadId, ofp_productTypeId, ofp_productId, ofp_probableAmount,ofp_activityId", "'" + HttpContext.Current.Session["KeyVal_InternalID"] + "','" + Convert.ToString(DTcategory.Rows[i][0]) + "','" + productid + "','" + PAmount + "','" + NewID + "'");
                                if (rdClient.Checked == true)
                                {
                                    rowEffected = oDBEngine.SetFieldValue(" tbl_master_address ", " add_activityId=" + act_slv_id[0, 0], " add_id= " + drpVisitPlace.SelectedItem.Value);
                                }
                            }
                            if (LeadId == "LD")
                            {
                                rowEffected = oDBEngine.SetFieldValue("tbl_master_lead", "cnt_status='" + NewID + "',cnt_userAccess = cnt_userAccess + '" + cmbAssignTo.SelectedItem.Value + "', cnt_lead_stage=3", " cnt_internalid='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'");
                            }
                            else
                            {
                                rowEffected = oDBEngine.SetFieldValue("tbl_master_contact", "cnt_status='" + NewID + "', cnt_lead_stage=3", " cnt_internalId ='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'");
                            }
                            //________add message here___//
                            //oDBEngine.messageTableUpdate(cmbAssignTo.SelectedItem.Value, HttpContext.Current.Session["userid"].ToString(), "Sales Visit", txtStartDate.Value.ToString(), txtEndDate.Value.ToString(), cmbPriority.SelectedItem.Text, txtInstructionNotes.Text, act_id[0, 0], "activity");
                            oDBEngine.messageTableUpdate(cmbAssignTo.SelectedItem.Value, Convert.ToString(HttpContext.Current.Session["userid"]), "Sales Visit", Convert.ToString(txtStartDate.Value), Convert.ToString(txtEndDate.Value), cmbPriority.SelectedItem.Text, txtInstructionNotes.Text, act_id[0, 0], "activity");
                        }
                        else
                        {
                            lblmessage.Text = "Add Product!";
                            return;
                        }
                    }
                    break;
                case 6:
                    DT = (DataTable)Session["DataTable_Lead_Activity"];
                    if (DT.Rows.Count != 0)
                    {
                        NewID = oDBEngine.GetInternalId("SL", "tbl_trans_Activies", "act_activityNo", "act_activityNo");
                        Values = HttpContext.Current.Session["userbranchID"] + "," + cmbActType.SelectedItem.Value + ",'" + txtDescription.Text + "'," + HttpContext.Current.Session["userid"] + "," + cmbAssignTo.SelectedItem.Value + ", getdate() ,'" + txtStartDate.Value + "','" + txtEndDate.Value + "','" + NewID + "'," + cmbPriority.SelectedItem.Value + ",'" + txtStartTime + "','" + txtEndTime + "','" + txtInstructionNotes.Text + "','" + Convert.ToString(oDBEngine.GetDate()) + "','" + Convert.ToString(Session["userid"]) + "'";
                        rowEffected = oDBEngine.InsurtFieldValue("tbl_trans_Activies", "act_branchId,act_activityType,act_description,act_assignedBy,act_assignedTo,act_createDate,act_scheduledDate,act_expectedDate,act_activityno,act_priority,act_scheduledTime,act_expectedTime,act_instruction,CreateDate,CreateUser", Values);
                        act_id = oDBEngine.GetFieldValue("tbl_trans_Activies", "act_id", "act_activityno='" + NewID + "'", 1);
                        DataTable DTcategory = new DataTable();
                        //DTcategory = new DataTable();
                        DTcategory = (DataTable)Session["DataTable_Lead_Activity"];

                        for (int i = 0; i < DTcategory.Rows.Count; i++)
                        {
                            string product = Convert.ToString(DTcategory.Rows[i][2]);
                            string productid = "";
                            if (product != "")
                            {
                                string[,] pdid = oDBEngine.GetFieldValue("tbl_Master_Products", "prds_internalId", "prds_description='" + product.Trim() + "'", 1);
                                productid = pdid[0, 0];
                            }
                            string PAmount = Convert.ToString(DTcategory.Rows[i][1]);
                            if (PAmount != "")
                            {
                                PAmount = Convert.ToString(DTcategory.Rows[i][1]);
                            }
                            else
                            {
                                PAmount = "0";
                            }
                            //  DBEngine oDBEngineLocal = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);

                            //BusinessLogicLayer.DBEngine oDBEngineLocal = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
                            BusinessLogicLayer.DBEngine oDBEngineLocal = new BusinessLogicLayer.DBEngine();

                            rowEffected = oDBEngineLocal.InsurtFieldValue("tbl_trans_offeredProduct", "ofp_leadId, ofp_productTypeId, ofp_productId, ofp_probableAmount,ofp_activityId,CreateDate,CreateUser", "'" + HttpContext.Current.Session["KeyVal_InternalID"] + "','" + Convert.ToString(DTcategory.Rows[i][0]) + "','" + productid + "','" + PAmount + "','" + NewID + "','" + Convert.ToString(oDBEngine.GetDate()) + "','" + Convert.ToString(Session["userid"]) + "'");

                            rowEffected = oDBEngineLocal.InsurtFieldValue("tbl_trans_Sales", "sls_activity_id,sls_contactlead_id,sls_branch_id,sls_sales_status,sls_date_closing,sls_ProductType,sls_product,sls_estimated_value,sls_nextvisitdate,CreateDate,CreateUser", act_id[0, 0] + ",'" + HttpContext.Current.Session["KeyVal_InternalID"] + "'," + HttpContext.Current.Session["userbranchID"] + ",4,'" + txtEndDate.Value + "','" + Convert.ToString(DTcategory.Rows[i][0]) + "','" + productid + "','" + Convert.ToString(DTcategory.Rows[i][1]) + "','" + txtNextDate.Value + "','" + Convert.ToString(oDBEngine.GetDate()) + "','" + Convert.ToString(Session["userid"]) + "'");
                            if (LeadId == "LD")
                            {
                                rowEffected = oDBEngineLocal.SetFieldValue("tbl_master_lead", "cnt_Status='" + NewID + "', cnt_lead_stage=3,cnt_userAccess = cnt_userAccess + '" + cmbAssignTo.SelectedItem.Value + "'", " cnt_internalId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'");
                            }
                            else
                            {
                                rowEffected = oDBEngine.SetFieldValue("tbl_master_contact", "cnt_status='" + NewID + "', cnt_lead_stage=3", " cnt_internalId ='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'");
                            }
                        }
                        //________add message here___//
                        //oDBEngine.messageTableUpdate(cmbAssignTo.SelectedItem.Value, HttpContext.Current.Session["userid"].ToString(), "Sales", txtStartDate.Value.ToString(), txtEndDate.Value.ToString(), cmbPriority.SelectedItem.Text, txtInstructionNotes.Text, act_id[0, 0], "activity");
                        oDBEngine.messageTableUpdate(cmbAssignTo.SelectedItem.Value, Convert.ToString(HttpContext.Current.Session["userid"]), "Sales", Convert.ToString(txtStartDate.Value), Convert.ToString(txtEndDate.Value), cmbPriority.SelectedItem.Text, txtInstructionNotes.Text, act_id[0, 0], "activity");
                    }
                    else
                    {
                        //if (cmbCategory.SelectedItem.Value.ToString().Trim() == "Broking & DP Account" || cmbCategory.SelectedItem.Value.ToString().Trim() == "Sub Broker" || cmbCategory.SelectedItem.Value.ToString().Trim() == "Relationship Partner")
                        if (Convert.ToString(cmbCategory.SelectedItem.Value).Trim() == "Broking & DP Account" || Convert.ToString(cmbCategory.SelectedItem.Value).Trim() == "Sub Broker" || Convert.ToString(cmbCategory.SelectedItem.Value).Trim() == "Relationship Partner")
                        {
                            LoadAllProductCat();
                            NewID = oDBEngine.GetInternalId("SL", "tbl_trans_Activies", "act_activityNo", "act_activityNo");
                            Values = HttpContext.Current.Session["userbranchID"] + "," + cmbActType.SelectedItem.Value + ",'" + txtDescription.Text + "'," + HttpContext.Current.Session["userid"] + "," + cmbAssignTo.SelectedItem.Value + ", getdate() ,'" + txtStartDate.Value + "','" + txtEndDate.Value + "','" + NewID + "'," + cmbPriority.SelectedItem.Value + ",'" + txtStartTime + "','" + txtEndTime + "','" + txtInstructionNotes.Text + "','" + Convert.ToString(oDBEngine.GetDate()) + "','" + Convert.ToString(Session["userid"]) + "'";
                            rowEffected = oDBEngine.InsurtFieldValue("tbl_trans_Activies", "act_branchId,act_activityType,act_description,act_assignedBy,act_assignedTo,act_createDate,act_scheduledDate,act_expectedDate,act_activityno,act_priority,act_scheduledTime,act_expectedTime,act_instruction,CreateDate,CreateUser", Values);
                            act_id = oDBEngine.GetFieldValue("tbl_trans_Activies", "act_id", "act_activityno='" + NewID + "'", 1);
                            DataTable DTcategory = new DataTable();
                            //DTcategory = new DataTable();
                            DTcategory = (DataTable)Session["DataTable_Lead_Activity"];

                            for (int i = 0; i < DTcategory.Rows.Count; i++)
                            {
                                string product = Convert.ToString(DTcategory.Rows[i][2]);
                                string productid = "";
                                if (product != "")
                                {
                                    string[,] pdid = oDBEngine.GetFieldValue("tbl_Master_Products", "prds_internalId", "prds_description='" + product.Trim() + "'", 1);
                                    productid = pdid[0, 0];
                                }
                                string PAmount = Convert.ToString(DTcategory.Rows[i][1]);
                                if (PAmount != "")
                                {
                                    PAmount = Convert.ToString(DTcategory.Rows[i][1]);
                                }
                                else
                                {
                                    PAmount = "0";
                                }
                                //  DBEngine oDBEngineLocal = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);

                                //BusinessLogicLayer.DBEngine oDBEngineLocal = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
                                BusinessLogicLayer.DBEngine oDBEngineLocal = new BusinessLogicLayer.DBEngine();

                                rowEffected = oDBEngineLocal.InsurtFieldValue("tbl_trans_offeredProduct", "ofp_leadId, ofp_productTypeId, ofp_productId, ofp_probableAmount,ofp_activityId,CreateDate,CreateUser", "'" + HttpContext.Current.Session["KeyVal_InternalID"] + "','" + Convert.ToString(DTcategory.Rows[i][0]) + "','" + productid + "','" + PAmount + "','" + NewID + "','" + Convert.ToString(oDBEngine.GetDate()) + "','" + Convert.ToString(Session["userid"]) + "'");
                                rowEffected = oDBEngineLocal.InsurtFieldValue("tbl_trans_Sales", "sls_activity_id,sls_contactlead_id,sls_branch_id,sls_sales_status,sls_date_closing,sls_ProductType,sls_product,sls_estimated_value,sls_nextvisitdate,CreateDate,CreateUser", act_id[0, 0] + ",'" + HttpContext.Current.Session["KeyVal_InternalID"] + "'," + HttpContext.Current.Session["userbranchID"] + ",4,'" + txtEndDate.Value + "','" + Convert.ToString(DTcategory.Rows[i][0]) + "','" + productid + "','" + Convert.ToString(DTcategory.Rows[i][1]) + "','" + txtNextDate.Value + "','" + Convert.ToString(oDBEngine.GetDate()) + "','" + Convert.ToString(Session["userid"]) + "'");
                                if (LeadId == "LD")
                                {
                                    rowEffected = oDBEngineLocal.SetFieldValue("tbl_master_lead", "cnt_Status='" + NewID + "', cnt_lead_stage=3,cnt_userAccess = cnt_userAccess + '" + cmbAssignTo.SelectedItem.Value + "'", " cnt_internalId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'");
                                }
                                else
                                {
                                    rowEffected = oDBEngine.SetFieldValue("tbl_master_contact", "cnt_status='" + NewID + "', cnt_lead_stage=3", " cnt_internalId ='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'");
                                }
                            }
                            //________add message here___//
                            //oDBEngine.messageTableUpdate(cmbAssignTo.SelectedItem.Value, HttpContext.Current.Session["userid"].ToString(), "Sales", txtStartDate.Value.ToString(), txtEndDate.Value.ToString(), cmbPriority.SelectedItem.Text, txtInstructionNotes.Text, act_id[0, 0], "activity");
                            oDBEngine.messageTableUpdate(cmbAssignTo.SelectedItem.Value, Convert.ToString(HttpContext.Current.Session["userid"]), "Sales", Convert.ToString(txtStartDate.Value), Convert.ToString(txtEndDate.Value), cmbPriority.SelectedItem.Text, txtInstructionNotes.Text, act_id[0, 0], "activity");
                        }
                        else
                        {
                            lblmessage.Text = "Add Product!";
                            return;
                        }
                    }
                    break;
                default:

                    break;
            }
            // DBEngine oDBEngineL = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);

            //BusinessLogicLayer.DBEngine oDBEngineL = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngineL = new BusinessLogicLayer.DBEngine();

            //string[,] COLVAL = oDBEngineL.GetFieldValue("tbl_master_lead", "cnt_useraccess", "cnt_internalid='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'", 1);
            //int rowsEffected = oDBEngineL.SetFieldValue("tbl_master_lead", "cnt_useraccess='" + COLVAL[0,0] + "," + cmbAssignTo.SelectedItem.Value + "',cnt_status='" + NewID + "',cnt_lead_stage=3", " cnt_internalid='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'");
            Body.Visible = false;
            lblmessage1.Text = cmbActType.SelectedItem + " Saved Successfully!";
            string popupScript = "";
            popupScript = "<script language='javascript'>" + "parent.editwin.close();</script>";
            ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);

        }



        protected void BTNClose_Click(object sender, EventArgs e)
        {
            //string popupScript = "";
            //popupScript = "<script language='javascript'>" + "parent.editwin.close();</script>";
            //ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
            string previousPageUrl = Convert.ToString(ViewState["previousPageUrl"]);
            //string previousPageUrl = ViewState["previousPageUrl"].ToString();
            Response.Redirect(previousPageUrl);
        }
        //protected void BtnADD_Click(object sender, EventArgs e)
        //{

        //}
        /*Code  Added  By Priti on 13122016 to use jquery Choosen*/
        [WebMethod]
        public static List<string> GetCompany(string reqStr, string param)
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = new DataTable();
            DT.Rows.Clear();
           // DT = oDBEngine.GetDataTable("tbl_master_employee, tbl_master_contact,tbl_trans_employeeCTC", "ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name, tbl_master_employee.emp_id as Id    ", " tbl_master_employee.emp_contactId = tbl_trans_employeeCTC.emp_cntId and  tbl_trans_employeeCTC.emp_cntId = tbl_master_contact.cnt_internalId   and cnt_contactType='EM'  and (cnt_firstName Like '" + reqStr + "%' or cnt_shortName like '" + reqStr + "%')");
            List<string> obj = new List<string>();
            //foreach (DataRow dr in DT.Rows)
            //{

            //    obj.Add(Convert.ToString(dr["Name"]) + "|" + Convert.ToString(dr["Id"]));
            //}
            if (param == "Mutual Fund")
            {
                DT = oDBEngine.GetDataTable("tbl_master_AssetsManagementCompanies", "amc_nameOfMutualFund,amc_amcCode", "amc_nameOfMutualFund Like '" + reqStr + "%'");
                if (DT.Rows.Count != 0)
                {
                    foreach (DataRow dr in DT.Rows)
                    {
                        obj.Add(Convert.ToString(dr["amc_nameOfMutualFund"]) + "|" + Convert.ToString(dr["amc_amcCode"]));
                    }
                }
                //else
                  
                //Response.Write("<script language='javaScript'> alert('No Record Found###No Record Found|') </script>");
            }
            else
            {
                if (param == "Insurance-Life")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_insurerName", "insu_nameOfCompany,insu_internalId", "insu_InsuranceCompType ='Life Insurers' and insu_nameOfCompany Like '" + reqStr + "%'");
                    if (DT.Rows.Count != 0)
                    {
                        foreach (DataRow dr in DT.Rows)
                        {
                            obj.Add(Convert.ToString(dr["insu_nameOfCompany"]) + "|" + Convert.ToString(dr["insu_internalId"]));
                        }
                    }
                    //else
                    //    Response.Write("No Record Found###No Record Found|");
                }
                else
                {
                    if (param == "Insurance-General")
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_insurerName", "insu_nameOfCompany,insu_internalId", "insu_InsuranceCompType ='Non-Life Insurers' and insu_nameOfCompany Like '" + reqStr + "%'");
                        if (DT.Rows.Count != 0)
                        {
                            foreach (DataRow dr in DT.Rows)
                            {
                                obj.Add(Convert.ToString(dr["insu_nameOfCompany"]) + "|" + Convert.ToString(dr["insu_internalId"]));
                            }
                        }
                        //else
                        //    Response.Write("No Record Found###No Record Found|");
                    }
                    //else
                    //    Response.Write("No Record Found###No Record Found|");
                }
            }
            return obj;
        }
        //...............code end........



        private void LoadAllProductCat()
        {

            //ShowPanel.Visible = true;
            TdProduct.Visible = true;
            lblmessage.Text = "";
            //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            string category = cmbCategory.SelectedItem.Value;
            string PRDID = "";
            switch (category)
            {
                case "Mutual Fund":
                    string[,] productId = oDBEngine.GetFieldValue("tbl_master_products", "top 1 prds_internalId as ID", "prds_productType = 'MF' and prds_description='" + txtProduct.Text + "'", 1);
                    if (productId[0, 0] != "")
                    {
                        PRDID = productId[0, 0];
                    }
                    break;
                case "Insurance-Life":
                    productId = oDBEngine.GetFieldValue("tbl_master_products", "top 1 prds_internalId as ID", "prds_productType = 'IN' and prds_description='" + txtProduct.Text + "'", 1);
                    if (productId[0, 0] != "")
                    {
                        PRDID = productId[0, 0];
                    }
                    break;
                case "Insurance-General":
                    productId = oDBEngine.GetFieldValue("tbl_master_products", "top 1 prds_internalId as ID", "prds_productType = 'IG' and prds_description='" + txtProduct.Text + "'", 1);
                    if (productId[0, 0] != "")
                    {
                        PRDID = productId[0, 0];
                    }
                    break;
                default:
                    break;
            }
            DataTable DTCategory = (DataTable)Session["DataTable_Lead_Activity"];
            if (DTCategory.Columns.Count == 0)
            {
                //___________creating Columns Here
                DataColumn productType = new DataColumn("Product Type");
                DataColumn productAmmount = new DataColumn("Product Amount");
                DataColumn Product = new DataColumn("Product");
                //___________Adding to datatable DTCategory
                DTCategory.Columns.Add(productType);
                DTCategory.Columns.Add(productAmmount);
                DTCategory.Columns.Add(Product);
            }

            DataRow newRowForDT = DTCategory.NewRow();
            //newRowForDT["Product Type"] = cmbCategory.SelectedItem.ToString();
            newRowForDT["Product Type"] = Convert.ToString(cmbCategory.SelectedItem);
            newRowForDT["Product Amount"] = txtProductAmmount.Text;
            newRowForDT["Product"] = txtProduct.Text;
            DTCategory.Rows.Add(newRowForDT);
            GridCategory.DataSource = DTCategory;
            GridCategory.DataBind();
            Session["DataTable_Lead_Activity"] = DTCategory;
            txtProduct.Text = "";
            txtProductAmmount.Text = "";
        }
        protected void GridCategory_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            LoadAllProductCat();
        }

    }
}