using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using DevExpress.Web;
using DataAccessLayer;

namespace ERP.OMS.Management.Master
{
    public partial class Bank_general : System.Web.UI.Page
    {
        public BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        Int32 ID;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if(!IsPostBack)
            {
                if (Request.QueryString["id"]!="ADD")
                {
                    if (Request.QueryString["id"] != null) // lead edit
                    {
                        ID = Int32.Parse(Request.QueryString["id"]);
                        HttpContext.Current.Session["KeyVal"] = ID;
                        string[,] InternalId;
                        InternalId = oDBEngine.GetFieldValue("tbl_master_Bank", "bnk_internalId,bnk_bankName", "bnk_id=" + ID, 1);
                        //InternalId = oDBEngine.GetFieldValue("tbl_master_lead", "cnt_internalId", "cnt_id=" + ID, 1);
                        HttpContext.Current.Session["KeyVal_InternalID"] = InternalId[0, 0];
                        
                    }
                    //Int32 bnkId = Convert.ToInt32(Request.QueryString["id"]);
                  //  FillDataByID(bnkId);
                    if (HttpContext.Current.Session["KeyVal"] != null)
                    {
                        Int32 bnkId = Convert.ToInt32(HttpContext.Current.Session["KeyVal"]);
                        FillDataByID(bnkId);
                        fillBranchGridByBankId(Convert.ToString(HttpContext.Current.Session["KeyVal"]));
                    }
                }
                else if (Request.QueryString["id"] == "ADD")
                {
                    HttpContext.Current.Session["KeyVal"] = null;
                    fillBranchGrid();
                    //Mantis Issue 0023983
                    chkActive.Checked = true;
                    //End of Mantis Issue 0023983
                }
                DisabledTabPage();
            }
           
        }
        private void FillDataByID(Int32 bnkId)
        {
            DataTable dt = new DataTable();
            dt = oDBEngine.GetDataTable("exec BankSelect @actiontype='BankSelectById',@bnk_id=" + bnkId + "");
            if (dt.Rows.Count > 0)
            {
                txtbnk_internalId_hidden.Value = Convert.ToString(dt.Rows[0]["bnk_internalId"]);
                txtBankName.Text = Convert.ToString(dt.Rows[0]["bnk_bankName"]);
                txtBranch.Text = Convert.ToString(dt.Rows[0]["bnk_branchName"]);
                txtMICRcode.Text = Convert.ToString(dt.Rows[0]["bnk_micrno"]);
                txtIFSCCode.Text = Convert.ToString(dt.Rows[0]["bnk_IFSCCode"]);
                txtRTGScode.Text = Convert.ToString(dt.Rows[0]["bnk_RTGSCode"]);
                txtNEFTcode.Text = Convert.ToString(dt.Rows[0]["bnk_NEFTCode"]);
                txtAcNo.Text = Convert.ToString(dt.Rows[0]["bnk_AcNo"]);
                txtSftCode.Text = Convert.ToString(dt.Rows[0]["bnk_SwiftCode"]);
                txtRemrks.Text = Convert.ToString(dt.Rows[0]["bnk_Remarks"]);
                // Mantis Issue 0023983
                chkActive.Checked = Convert.ToBoolean(dt.Rows[0]["bnk_Active"]);
                // End of Mantis Issue 0023983
            }
        }
        public void DisabledTabPage()
        {
            if (Request.QueryString["id"] =="ADD")
            {
                TabPage page = pageControl.TabPages.FindByName("tabCorrespondence");
                page.Visible = false;
            }
            else
            {
                TabPage page = pageControl.TabPages.FindByName("tabCorrespondence");
                page.Visible = true;
            }
           
        }
        protected void Clear()
        {
            txtBankName.Text = "";
            txtBranch.Text = "";
            txtMICRcode.Text = "";
            txtIFSCCode.Text = "";
            txtNEFTcode.Text = "";
            txtRTGScode.Text = "";
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] == "ADD")
            { 
                //comment by sanjib due to impelmentation has been changed on 1312017

                //DataSet dsInst = new DataSet();
                //String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                //SqlConnection con = new SqlConnection(conn);
                //SqlCommand cmd = new SqlCommand("BankInsert", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@bnk_bankName", txtBankName.Text.Trim());
                //cmd.Parameters.AddWithValue("@bnk_branchName", txtBranch.Text.Trim());
                //cmd.Parameters.AddWithValue("@bnk_micrno", txtMICRcode.Text.Trim());
                //cmd.Parameters.AddWithValue("@bnk_IFSCCode", txtIFSCCode.Text.Trim());
                //cmd.Parameters.AddWithValue("@bnk_NEFTCode", txtNEFTcode.Text.Trim());
                //cmd.Parameters.AddWithValue("@bnk_RTGSCode", txtRTGScode.Text.Trim());
                //cmd.Parameters.AddWithValue("@CreateUser", Convert.ToString(HttpContext.Current.Session["userid"]));
                //cmd.CommandTimeout = 0;
                //SqlDataAdapter Adap = new SqlDataAdapter();
                //Adap.SelectCommand = cmd;
                //Adap.Fill(dsInst);
                //cmd.Dispose();
                //con.Dispose();
                //Clear();

                ProcedureExecute proc;
                string rtrnvalue = "";
                try
                {

                    string ListOfBranch = "";
                    List<object> ComponentList = BranchGridLookup.GridView.GetSelectedFieldValues("branch_id");
                    foreach (object Pobj in ComponentList)
                    { 
                        ListOfBranch += "," + Pobj; 
                    }
                    ListOfBranch = ListOfBranch.TrimStart(',');


                    using (proc = new ProcedureExecute("BankInsert"))
                    {

                        proc.AddVarcharPara("@bnk_bankName", 200, txtBankName.Text.Trim());
                        proc.AddVarcharPara("@bnk_micrno", 11, txtMICRcode.Text.Trim());
                        proc.AddVarcharPara("@bnk_branchName", 500, txtBranch.Text.Trim());
                        proc.AddVarcharPara("@bnk_IFSCCode", 11, txtIFSCCode.Text.Trim());
                        proc.AddVarcharPara("@bnk_NEFTCode", 11, txtNEFTcode.Text.Trim());
                        proc.AddVarcharPara("@bnk_RTGSCode", 11, txtRTGScode.Text.Trim());

                        // Mantis Issue #16918
                        proc.AddVarcharPara("@bnk_AcNo", 20, txtAcNo.Text.Trim());
                        proc.AddVarcharPara("@bnk_SwiftCode", 20, txtSftCode.Text.Trim());
                        proc.AddVarcharPara("@bnk_Remarks", 8000, txtRemrks.Text.Trim());
                        // End Mantis Issue #16918
                        proc.AddVarcharPara("@BranchList", 4000, ListOfBranch);
                        // Mantis Issue 0023983
                        proc.AddVarcharPara("@bnk_Active", 10, Convert.ToString(chkActive.Checked));
                        // End of Mantis Issue 0023983

                        proc.AddVarcharPara("@CreateUser", 50, Convert.ToString(HttpContext.Current.Session["userid"]));
                        proc.AddVarcharPara("@result", 50, "", QueryParameterDirection.Output);
                        int i = proc.RunActionQuery();
                        rtrnvalue = proc.GetParaValue("@result").ToString();
                        if (i >= 1)
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Saved Succesfully')</script>");
                            Response.Redirect("Bank_general.aspx?id=" + rtrnvalue + "");
                        }
                        else {
                            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Sorry there are some error, Please try again.')</script>");
                        }

                    }
                }

                catch (Exception ex)
                {
                    
                }

                finally
                {
                    proc = null;
                }

                
            }
            else
            {

                string ListOfBranch = "";
                List<object> ComponentList = BranchGridLookup.GridView.GetSelectedFieldValues("branch_id");
                foreach (object Pobj in ComponentList)
                {
                    ListOfBranch += "," + Pobj;
                }
                ListOfBranch = ListOfBranch.TrimStart(',');
                
                DataSet dsInst = new DataSet();
                //String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"]; MULTI
                String conn = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                SqlConnection con = new SqlConnection(conn);
                SqlCommand cmd = new SqlCommand("BankUpdate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@bnk_bankName", txtBankName.Text.Trim());
                cmd.Parameters.AddWithValue("@bnk_branchName", txtBranch.Text.Trim());
                cmd.Parameters.AddWithValue("@bnk_micrno", txtMICRcode.Text.Trim());
                cmd.Parameters.AddWithValue("@bnk_IFSCCode", txtIFSCCode.Text.Trim());
                cmd.Parameters.AddWithValue("@bnk_NEFTCode", txtNEFTcode.Text.Trim());
                cmd.Parameters.AddWithValue("@bnk_RTGSCode", txtRTGScode.Text.Trim());
                cmd.Parameters.AddWithValue("@BranchList", ListOfBranch);

                // Mantis Issue #16918
                cmd.Parameters.AddWithValue("@bnk_AcNo", txtAcNo.Text.Trim());
                cmd.Parameters.AddWithValue("@bnk_SwiftCode", txtSftCode.Text.Trim());
                cmd.Parameters.AddWithValue("@bnk_Remarks", txtRemrks.Text.Trim());
                // End Mantis Issue #16918

                // Mantis Issue 0023983 
                cmd.Parameters.AddWithValue("@bnk_Active", Convert.ToString( chkActive.Checked));
                // End of Mantis Issue 0023983

                cmd.Parameters.AddWithValue("@CreateUser", Convert.ToString(HttpContext.Current.Session["userid"]));
                cmd.Parameters.AddWithValue("@bnk_internalId", txtbnk_internalId_hidden.Value);
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();
            }
        }


        #region BranchDetails
        protected void Component_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            DataTable bankBranchTable = (DataTable)Session["bankBranchTable"];
            if (bankBranchTable != null)
                BranchGridLookup.DataSource = bankBranchTable;
        }

        public void fillBranchGrid()
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_bank_branchMap");
            proc.AddVarcharPara("@Action", 50, "GetAllBranch"); 
            ds = proc.GetTable();
            Session["bankBranchTable"] = ds;
            BranchGridLookup.DataBind();
        
        }

        public void fillBranchGridByBankId(string bankId)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_bank_branchMap");
            proc.AddVarcharPara("@Action", 50, "GetAllBranchById");
            proc.AddVarcharPara("@bankId", 10, bankId);
            ds = proc.GetTable();
            Session["bankBranchTable"] = ds;
            BranchGridLookup.DataBind();


            ProcedureExecute proc1 = new ProcedureExecute("prc_bank_branchMap");
            proc1.AddVarcharPara("@Action", 50, "GetBranchList");
            proc1.AddVarcharPara("@bankId", 10, bankId);
            ds = proc1.GetTable();

            foreach (DataRow dr in ds.Rows)
            {
                BranchGridLookup.GridView.Selection.SelectRowByKey(Convert.ToString(dr["BranchId"]));            
            }
        
        }

        #endregion

    }
}