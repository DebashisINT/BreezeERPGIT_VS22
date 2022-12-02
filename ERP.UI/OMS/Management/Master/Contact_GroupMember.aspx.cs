using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using DevExpress.Web.ASPxTabControl;
using DevExpress.Web;
using System.Configuration;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
using EntityLayer.CommonELS;

namespace ERP.OMS.Management.Master
{

    public partial class management_master_Contact_GroupMember : ERP.OMS.ViewState_class.VSPage
    {
        string InterNalId;
        // DBEngine objEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
        public string pageAccess = "";
        //  DBEngine oDBEngine = new DBEngine(string.Empty);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        clsDropDownList oclsDropDownList = new clsDropDownList();

        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);

            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            // Code  Added and Commented By Priti on 12122016 to use Convert.ToString instead of ToString()
            GroupMaster.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            if (Session["requesttype"] != null)
            {
                //lblHeadTitle.Text = Session["requesttype"].ToString() + " Group Member List";
                lblHeadTitle.Text = Convert.ToString(Session["requesttype"]) + " Group Member List";
            }

           // rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/Contact_GroupMember.aspx");
            string cnttype = string.Empty;

           // cnttype = Session["ContactType"] == null ? "" : Session["ContactType"].ToString();
            cnttype = Session["ContactType"] == null ? "" : Convert.ToString(Session["ContactType"]);
            if (HttpContext.Current.Session["userid"] == null)
            {
                // Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            // bellow code added by debjyoti 
            //Reason: Rights of user set from parents 15-11-2016


            if (Session["requesttype"] != null)
            {
                if (Convert.ToString(Session["requesttype"]).Trim() == "Lead")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=Lead");
                }
                else if (Convert.ToString(Session["requesttype"]).Trim() == "Customer/Client")
                {
                    //rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=customer");
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/CustomerMasterList.aspx");
                }
                else if (Convert.ToString(Session["requesttype"]).Trim() == "Franchisee")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=franchisee");
                }
                else if (Convert.ToString(Session["requesttype"]).Trim() == "Partner")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=partner");
                }
                else if (Convert.ToString(Session["requesttype"]).Trim() == "Consultant")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=consultant");
                }
                else if (Convert.ToString(Session["requesttype"]).Trim() == "Share Holder")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=shareholder");
                }
                else if (Convert.ToString(Session["requesttype"]).Trim() == "Debtor")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=debtor");
                }
                else if (Convert.ToString(Session["requesttype"]).Trim() == "Creditors")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=creditor");
                }
                else if (Convert.ToString(Session["requesttype"]).Trim() == "Relationship Manager")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=agent");
                }
                else
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/root_UserGroups.aspx");
                }

            }
            //End Here Debjyoti 15-11-2016
            if (!IsPostBack)
            {
                if (Session["Name"] != null)
                {
                    //lblName.Text = Session["Name"].ToString();
                    lblName.Text = Convert.ToString(Session["Name"]);
                }

            }
            if (Convert.ToString(Session["requesttype"]) == "Lead")
            {
                TabPage pageFM = ASPxPageControl1.TabPages.FindByName("FamilyMembers");
                pageFM.Visible = true;
            }
            if (Request.QueryString["formtype"] != null)
            {
                //InterNalId = Session["InternalId"].ToString();
                InterNalId = Convert.ToString(Session["InternalId"]);
                TabVisibility();
            }
            else { 
                //InterNalId = (string)HttpContext.Current.Session["KeyVal_InternalID"].ToString();
                InterNalId = Convert.ToString((string)HttpContext.Current.Session["KeyVal_InternalID"]);
            }
            Session["KeyVal_InternalID_New"] = InterNalId;
            GroupMasterBind();
        }

         
        void chk_CheckedChanged(object sender, EventArgs e)
        {
            string chkid = (((CheckBox)sender).ClientID);
            for (int i = 0; i < Convert.ToInt32(Counter.Text); i++)
            {
                Table tbl = (Table)TablePanel.FindControl("TableBind");
                Label lblGrouptype = new Label();
                lblGrouptype = (Label)tbl.FindControl("lbl" + Convert.ToString(i));
                CheckBox chk = (CheckBox)tbl.FindControl("chk" + Convert.ToString(i));
                DropDownList combo = (DropDownList)tbl.FindControl("ddl" + Convert.ToString(i));
                if (chk.Checked)
                {
                    combo.Enabled = true;
                }
                else
                {
                    combo.Enabled = false;
                }
                combo.Width.Equals(500);
                string[,] ContactId = objEngine.GetFieldValue("tbl_trans_group", "grp_contactId", " grp_contactId='" + InterNalId + "' and grp_groupType='" + lblGrouptype.Text + "'", 1);

            }
        }

        public void GroupMasterBind()
        {
            int count = 0;
            TableBind.Rows.Clear();
            string[,] Gtype = objEngine.GetFieldValue("tbl_master_groupMaster gpm", "distinct gpm_type,(select grp_groupMaster from tbl_trans_group where tbl_trans_group.grp_groupType = gpm_type and tbl_trans_group.grp_contactId = '" + InterNalId + "') as ID", null, 2);

            for (int i = 0; i < Gtype.Length / 2; i++)
            {
                if (Gtype[i, 0] != null)
                {
                    CheckBox chk = new CheckBox();
                    chk.CheckedChanged += chk_CheckedChanged;
                    chk.AutoPostBack = true;
                    chk.ID = "chk" + Convert.ToString(count);
                    Label lbl = new Label();
                    lbl.ID = "lbl" + Convert.ToString(count);
                    lbl.Text = Gtype[i, 0];

                    DropDownList ddl = new DropDownList();
                    ddl.ID = "ddl" + Convert.ToString(count);
                    ddl.Width = 250;
                    string[,] Description = objEngine.GetFieldValue("V_MultiGroup", "Group_Id,Group_Name", "gpm_Type='" + Gtype[i, 0] + "'", 2, "Group_Name");
                    if (Description[0, 0] != "n")
                    {
                        //objEngine.AddDataToDropDownList(Description, ddl);
                        oclsDropDownList.AddDataToDropDownList(Description, ddl);
                        if (Gtype[i, 1] != "")
                        {
                            chk.Checked = true;
                            ddl.Enabled = true;
                            ddl.SelectedValue = Gtype[i, 1];
                        }
                        else
                        {
                            ddl.Enabled = false;
                        }

                        ////////if (Gtype[1, 1] != "")
                        ////////{
                        ////////    chk.Checked = true;
                        ////////    ddl.SelectedValue = Gtype[1, 1];
                        ////////}
                        TableCell TblCell = new TableCell();
                        TableCell TblCell1 = new TableCell();
                        TableCell TblCell2 = new TableCell();
                        TableRow row = new TableRow();
                        TblCell2.HorizontalAlign.Equals("Right");
                        TblCell2.Controls.Add(chk);
                        row.Cells.Add(TblCell2);
                        TblCell.HorizontalAlign.Equals("Right");
                        TblCell.Controls.Add(lbl);
                        row.Cells.Add(TblCell);
                        TblCell1.HorizontalAlign.Equals("Left");
                        TblCell1.Controls.Add(ddl);
                        row.Cells.Add(TblCell1);
                        TableBind.Rows.Add(row);

                        count += 1;

                    }
                }
            }
            Counter.Text = Convert.ToString(count);

        }

        
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            int CreateUser = Convert.ToInt32(HttpContext.Current.Session["userid"]);//Session UserId
            DateTime CreateDate = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            //objEngine.DeleteValue("tbl_trans_group", " grp_contactId='" + InterNalId + "'");
            for (int i = 0; i < Convert.ToInt32(Counter.Text); i++)
            {
                Table tbl = (Table)TablePanel.FindControl("TableBind");
                Label lblGrouptype = new Label();
                lblGrouptype = (Label)tbl.FindControl("lbl" + Convert.ToString(i));
                CheckBox chk = (CheckBox)tbl.FindControl("chk" + Convert.ToString(i));
                DropDownList combo = (DropDownList)tbl.FindControl("ddl" + Convert.ToString(i));
                combo.Width.Equals(500);
                string[,] ContactId = objEngine.GetFieldValue("tbl_trans_group", "grp_contactId", " grp_contactId='" + InterNalId + "' and grp_groupType='" + lblGrouptype.Text + "'", 1);

                if (ContactId[0, 0] != "n")
                {
                    string a1 = null;
                    //a1 = combo.SelectedItem.Text.ToString().Trim();
                    a1 = Convert.ToString(combo.SelectedItem.Text).Trim();
                    if (chk.Checked == true)
                    {
                        oDBEngine.SetFieldValue("tbl_trans_group", "grp_groupMaster='" + combo.SelectedItem.Value + "',grp_contactId='" + InterNalId + "',grp_groupType='" + lblGrouptype.Text + "',CreateDate='" + CreateDate + "',CreateUser='" + CreateUser + "'", "grp_contactId='" + InterNalId + "' and grp_groupType='" + lblGrouptype.Text + "'");
                    }
                }
                else
                {

                    if (chk.Checked == true)
                    {
                        objEngine.InsurtFieldValue("tbl_trans_group", "grp_groupMaster,grp_contactId,grp_groupType,CreateDate,CreateUser", "'" + combo.SelectedItem.Value + "','" + InterNalId + "','" + lblGrouptype.Text + "','" + CreateDate + "','" + CreateUser + "'");
                    }

                }



            }
            GroupMasterGrid.DataBind();
            TablePanel.Visible = false;
            GridPanel.Visible = true;
            LinkButton1.Visible = true;
            // BtnAdd.Visible = true;


        }
        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            string Contactrequesttype = Convert.ToString(Session["Contactrequesttype"]).Trim();
            string url = "frmContactMain.aspx?requesttype=" + Contactrequesttype;

            Response.Redirect("Contact_GroupMember.aspx");
            
        }
        //protected void BtnAdd_Click(object sender, EventArgs e)
        //{
        //    BtnAdd.Visible = false;
        //    GridPanel.Visible = false;
        //    TablePanel.Visible = true;
        //}
        public void TabVisibility()
        {
            TabPage page = ASPxPageControl1.TabPages.FindByName("Documents");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("General");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("CorresPondence");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("Bank Details");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("DP Details");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("Documents");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("Family Members");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("Registration");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("Deposit");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("Remarks");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("Education");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("Trad. Prof.");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("Other");
            page.Enabled = false;
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            GridPanel.Visible = false;
            //  TablePanel.visible = true;
            TablePanel.Visible = true;
            LinkButton1.Visible = false;

        }

        //Purpose: User rights in  Group member list grid
        //Name : Debjyoti 16-11-2016

        protected void GroupMasterGrid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (!rights.CanDelete)
            {
                if (e.ButtonType == ColumnCommandButtonType.Delete)
                {
                    e.Visible = false;
                }
            }


            if (!rights.CanEdit)
            {
                if (e.ButtonType == ColumnCommandButtonType.Edit)
                {
                    e.Visible = false;
                }
            }

        }

        //End : debjyoti 16-11-2016
    }
}