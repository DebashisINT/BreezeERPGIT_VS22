using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
using EntityLayer.CommonELS;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_HRrecruitmentagent_GroupMember : ERP.OMS.ViewState_class.VSPage //SSystem.Web.UI.Page
    {
        string InterNalId;
        //DBEngine objEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
        clsDropDownList OclsDropDownList = new clsDropDownList();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage(); 
       
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/HRrecruitmentagent.aspx?requesttype=VendorService");
            GroupMaster.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string cnttype = Session["ContactType"].ToString();
            if (!IsPostBack)
            {
                string[,] EmployeeNameID = objEngine.GetFieldValue(" tbl_master_contact ", " case when cnt_firstName is null then '' else cnt_firstName end + ' '+case when cnt_middleName is null then '' else cnt_middleName end+ ' '+case when cnt_lastName is null then '' else cnt_lastName end+' ['+cnt_shortName+']' as name ", " cnt_internalId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'", 1);
                if (EmployeeNameID[0, 0] != "n")
                {
                    lblHeader.Text = EmployeeNameID[0, 0].ToUpper();
                }
            }
            InterNalId = (string)HttpContext.Current.Session["KeyVal_InternalID"].ToString();// "LDC0000911";
            GroupMasterBind();
        }

        //AG09112016 - All the dropdowns to be remain disbaled. Enabled=false. If we select respective checkboxes then it should get Enabled=true

        void chk_CheckedChanged(object sender, EventArgs e)
        {
            string chkid = (((CheckBox)sender).ClientID);
            for (int i = 0; i < Convert.ToInt32(Counter.Text); i++)
            {
                Table tbl = (Table)TablePanel.FindControl("TableBind");
                Label lblGrouptype = new Label();
                lblGrouptype = (Label)tbl.FindControl("lbl" + i.ToString());
                CheckBox chk = (CheckBox)tbl.FindControl("chk" + i.ToString());
                DropDownList combo = (DropDownList)tbl.FindControl("ddl" + i.ToString());
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
            string[,] Gtype = objEngine.GetFieldValue("tbl_master_groupMaster", "distinct gpm_type", null, 1, "gpm_type");
            int Length = Gtype.GetLength(0);
            int Length1 = Gtype.GetLength(1);

            for (int i = 0; i < Gtype.Length; i++)
            {
                if (Gtype[i, 0] != null)
                {
                    CheckBox chk = new CheckBox();
                    chk.CheckedChanged += chk_CheckedChanged;
                    chk.AutoPostBack = true;
                    chk.ID = "chk" + count.ToString();
                    Label lbl = new Label();
                    lbl.ID = "lbl" + count.ToString();
                    lbl.Text = Gtype[i, 0];

                    DropDownList ddl = new DropDownList();
                    ddl.ID = "ddl" + count.ToString();
                    ddl.Width = 150;
                    string[,] Description = objEngine.GetFieldValue("tbl_master_groupMaster", "gpm_id,gpm_description", "gpm_Type='" + Gtype[i, 0] + "'", 2, "gpm_description");
                    if (Description[0, 0] != "n")
                    {
                        //objEngine.AddDataToDropDownList(Description, ddl);
                        OclsDropDownList.AddDataToDropDownList(Description, ddl);
                        //AG09112016 - All the dropdowns to be remain disbaled. Enabled=false. If we select respective checkboxes then it should get Enabled=true
                        if (Gtype[i, 0] == "")
                        {
                            chk.Checked = true;
                            ddl.Enabled = true;
                            ddl.SelectedValue = Gtype[i, 0];
                        }
                        else
                        {
                            ddl.Enabled = false;
                        }

                        TableCell TblCell = new TableCell();
                        TableCell TblCell1 = new TableCell();
                        TableCell TblCell2 = new TableCell();
                        TableRow row = new TableRow();
                        TblCell2.HorizontalAlign.Equals("Rigrt");
                        TblCell2.Controls.Add(chk);
                        row.Cells.Add(TblCell2);
                        TblCell.HorizontalAlign.Equals("Rigrt");
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
            Counter.Text = count.ToString();

        }
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            int CreateUser = Convert.ToInt32(HttpContext.Current.Session["userid"]);//Session UserId
            DateTime CreateDate = Convert.ToDateTime(objEngine.GetDate().ToShortDateString());
            //objEngine.DeleteValue("tbl_trans_group", " grp_contactId='" + InterNalId + "'");
            for (int i = 0; i < Convert.ToInt32(Counter.Text); i++)
            {
                Table tbl = (Table)TablePanel.FindControl("TableBind");
                Label lblGrouptype = new Label();
                lblGrouptype = (Label)tbl.FindControl("lbl" + i.ToString());
                DropDownList combo = (DropDownList)tbl.FindControl("ddl" + i.ToString());
                CheckBox chk = (CheckBox)tbl.FindControl("chk" + i.ToString());
                string[,] ContactId = objEngine.GetFieldValue("tbl_trans_group", "grp_contactId", " grp_contactId='" + InterNalId + "' and grp_groupType='" + lblGrouptype.Text + "'", 1);
                if (ContactId[0, 0] != "n")
                {

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
            BtnAdd.Visible = true;
        }
        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            BtnAdd.Visible = false;
            GridPanel.Visible = false;
            TablePanel.Visible = true;
        }
    }
}