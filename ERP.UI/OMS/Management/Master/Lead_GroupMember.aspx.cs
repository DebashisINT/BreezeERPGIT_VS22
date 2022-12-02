using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Management.Master
{
    public partial class management_Master_Lead_GroupMember : ERP.OMS.ViewState_class.VSPage
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        clsDropDownList oclsDropDownList = new clsDropDownList();
        string InterNalId;
        protected void Page_Load(object sender, EventArgs e)
        {
            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //GroupMaster.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    GroupMaster.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

                }
                else
                {
                    //GroupMaster.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    GroupMaster.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            //if (HttpContext.Current.Session["userid"] == null)
            //{
            //   //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            //}
            ////this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");


            if (!IsPostBack)
            {

            }
            InterNalId = (string)HttpContext.Current.Session["KeyVal_InternalID"].ToString();// "LDC0000911";
            GroupMasterBind();
        }

        public void GroupMasterBind()
        {
            int count = 0;
            TableBind.Rows.Clear();
            string[,] Gtype = oDBEngine.GetFieldValue("tbl_master_groupMaster gpm", "distinct gpm_type,(select grp_groupMaster from tbl_trans_group where tbl_trans_group.grp_groupType = gpm_type and tbl_trans_group.grp_contactId = '" + InterNalId + "') as ID", null, 2);

            for (int i = 0; i < Gtype.Length / 2; i++)
            {
                if (Gtype[i, 0] != null)
                {
                    CheckBox chk = new CheckBox();
                    chk.ID = "chk" + count.ToString();
                    Label lbl = new Label();
                    lbl.ID = "lbl" + count.ToString();
                    lbl.Text = Gtype[i, 0];

                    DropDownList ddl = new DropDownList();
                    ddl.ID = "ddl" + count.ToString();
                    ddl.Width = 250;
                    string[,] Description = oDBEngine.GetFieldValue("tbl_master_groupMaster", "gpm_id,gpm_description", "gpm_Type='" + Gtype[i, 0] + "'", 2, "gpm_description");
                    if (Description[0, 0] != "n")
                    {
                        //oDBEngine.AddDataToDropDownList(Description, ddl);
                        oclsDropDownList.AddDataToDropDownList(Description, ddl);
                        if (Gtype[i, 1] != "")
                        {
                            chk.Checked = true;
                            ddl.SelectedValue = Gtype[i, 1];
                        }
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
            Counter.Text = count.ToString();

        }
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            int CreateUser = Convert.ToInt32(HttpContext.Current.Session["userid"]);//Session UserId
            DateTime CreateDate = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            //oDBEngine.DeleteValue("tbl_trans_group", " grp_contactId='" + InterNalId + "'");
            for (int i = 0; i < Convert.ToInt32(Counter.Text); i++)
            {
                Table tbl = (Table)TablePanel.FindControl("TableBind");
                Label lblGrouptype = new Label();
                lblGrouptype = (Label)tbl.FindControl("lbl" + i.ToString());
                DropDownList combo = (DropDownList)tbl.FindControl("ddl" + i.ToString());
                CheckBox chk = (CheckBox)tbl.FindControl("chk" + i.ToString());
                string[,] ContactId = oDBEngine.GetFieldValue("tbl_trans_group", "grp_contactId", " grp_contactId='" + InterNalId + "' and grp_groupType='" + lblGrouptype.Text + "'", 1);
                if (ContactId[0, 0] != "n")
                {
                }
                else
                {
                    if (chk.Checked == true)
                    {
                        oDBEngine.InsurtFieldValue("tbl_trans_group", "grp_groupMaster,grp_contactId,grp_groupType,CreateDate,CreateUser", "'" + combo.SelectedItem.Value + "','" + InterNalId + "','" + lblGrouptype.Text + "','" + CreateDate + "','" + CreateUser + "'");
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