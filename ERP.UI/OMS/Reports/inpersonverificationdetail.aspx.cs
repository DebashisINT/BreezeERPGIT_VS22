using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_inpersonverificationdetail : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        DataTable dt = new DataTable();
        Audit_Inspection audit_ins = new Audit_Inspection();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        string data;


        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "height1", "<script>SearchOpt();</script>");
            //Page.ClientScript.RegisterStartupScript(GetType(), "height2", "<script>SearchOpt1();</script>");
            Page.ClientScript.RegisterStartupScript(GetType(), "height", "<script>height();</script>");
            btnSave.Attributes.Add("OnClick", "Javascript:return ValidatePage();");
            //txtClientID1.Attributes.Add("onkeyup", "abcd(this,'lgjh',event)");
            txtSelectionID.Attributes.Add("onkeyup", "abcd(this,'Other',event)");

            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            GridBind();

            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
        }

        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string[] cl = idlist[1].Split(',');
            string str = "";
            string str1 = "";
            for (int i = 0; i < cl.Length; i++)
            {
                if (idlist[0].ToString().Trim() == "Clients")
                {
                    string[] val = cl[i].Split(';');
                    string[] AcVal = val[0].Split('-');
                    if (str == "")
                    {

                        str = "'" + AcVal[0] + "'";
                        str1 = "'" + AcVal[0] + "'" + ";" + val[1];
                    }
                    else
                    {

                        str += ",'" + AcVal[0] + "'";
                        str1 += "," + "'" + AcVal[0] + "'" + ";" + val[1];
                    }
                }

                else
                {
                    string[] val = cl[i].Split(';');
                    if (str == "")
                    {
                        str = val[0];
                        str1 = val[0] + ";" + val[1];
                    }
                    else
                    {
                        str += "," + val[0];
                        str1 += "," + val[0] + ";" + val[1];
                    }
                }

            }
            data = idlist[0] + "~" + str;


        }
        public void BindGroup()
        {
            DataTable DtGroup = oDBEngine.GetDataTable("tbl_master_groupmaster", "distinct gpm_Type", null);
            if (DtGroup.Rows.Count > 0)
            {
                ddlGroup.DataSource = DtGroup;
                ddlGroup.DataTextField = "gpm_Type";
                ddlGroup.DataValueField = "gpm_Type";
                ddlGroup.DataBind();
                DtGroup.Dispose();

            }

        }
        protected void BtnGroup_Click(object sender, EventArgs e)
        {
            if (ddlGroupBy.SelectedItem.Value.ToString().Trim() == "Group")
            {
                if (HiddenField_Group.Value.ToString().Trim() == "")
                {
                    BindGroup();
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Procedure();
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            Procedure();
            exporter.WriteXlsToResponse();
        }


        protected void GridBind()
        {

            //if (cmbDuplicate.SelectedItem.Value == "N")
            //{
            //    gridContract.Columns["VerificationDate"].Visible = false;
            //    gridContract.Columns["VerificationName"].Visible = false;
            //    gridContract.Columns["VerificationRemarks"].Visible = false;




            //}

            if (ViewState["DatasetMain"] != null)
            {
                DataSet dsNew = (DataSet)ViewState["DatasetMain"];
                gridContract.DataSource = dsNew.Tables[0];
                gridContract.DataBind();

            }
            //if (cmbType.SelectedItem.Value == "PIN Code")
            //{
            //gridContract.Columns[5].Caption = cmbType.SelectedItem.Text;
            //}
            //else
            //{
            //    gridContract.Columns[5].Caption = "Email";
            //}


        }
        protected void gridContract_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

            if (e.Parameters.ToString() == "s")
            {
                gridContract.Settings.ShowFilterRow = true;
            }
            else if (e.Parameters.ToString() == "All")
            {
                gridContract.FilterExpression = string.Empty;
            }
            //if (cmbDuplicate.SelectedItem.Value != "Y")
            //{
            //    gridContract.Columns["cnt_InPersonVerificationDate"].Visible = false;
            //    gridContract.Columns["cnt_InPersonVerificationBy"].Visible = false;
            //    gridContract.Columns["cnt_VerifcationRemarks"].Visible = false;




            //}
            GridBind();
        }
        protected void cmbDuplicate_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public void Procedure()
        {
            DataSet ds = new DataSet();
            string GRPTYPE = "";
            string Groupby = "";
            string CLIENTS = "";

            if (ddlGroupBy.SelectedItem.Value.ToString().Trim() == "Branch")/////group type branch selection
            {
                if (RadioBtnOtherGroupBySelected.Checked)
                {
                    GRPTYPE = "BRANCH";

                    if (RadioBtnOtherGroupByAll.Checked)
                    {

                        Groupby = "ALL";
                    }
                    else
                    {
                        Groupby = HiddenField_Branch.Value.ToString().Trim();
                        CLIENTS = "ALL";
                    }
                }
                else
                {
                    GRPTYPE = "BRANCH";
                    if (RadioBtnOtherGroupBySelected.Checked == true)
                    {
                        Groupby = HiddenField_Branch.Value.ToString().Trim();
                    }
                    else
                    {
                        Groupby = "All";
                    }
                    CLIENTS = "ALL";
                }
            }
            else if (ddlGroupBy.SelectedItem.Value.ToString().Trim() == "Group")/////group type group selection
            {
                GRPTYPE = ddlGroup.SelectedItem.Text.ToString().Trim();
                if (RadioBtnGroupAll.Checked)
                {
                    Groupby = "All";
                }
                else
                {
                    Groupby = HiddenField_Group.Value.ToString().Trim();
                }
                CLIENTS = "All";
            }
            else                                /////group type client selection
            {
                GRPTYPE = "BRANCH";
                Groupby = "All";

            }

            if (ddlGroupBy.SelectedItem.Value.ToString().Trim() == "Clients")/////group type client selection
            {
                if (RadioBtnOtherGroupByAll.Checked)
                {
                    CLIENTS = "All";
                }
                else if (RadioBtnOtherGroupBySelected.Checked)
                {
                    CLIENTS = HiddenField_Client.Value.ToString().Trim();
                }
                else
                {
                    CLIENTS = "All";
                }
            }

            ds = audit_ins.Inpersonverification(cmbDuplicate.SelectedItem.Value, GRPTYPE, Groupby, CLIENTS);
            ViewState["DatasetMain"] = ds;
            GridBind();
        }

    }
}