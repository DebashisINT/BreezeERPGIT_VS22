using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Configuration;
using ClsDropDownlistNameSpace;
using EntityLayer.CommonELS;
using System.Web.Services;
using System.Collections.Generic;
using BusinessLogicLayer;

namespace ERP.OMS.Management.Master
{
    public partial class ServiceCenter : ERP.OMS.ViewState_class.VSPage
    {
       public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
       BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
        public string pageAccess = "";

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        string checking = "a";

        BusinessLogicLayer.Converter oConverter = new BusinessLogicLayer.Converter();

        BusinessLogicLayer.GenericStoreProcedure oGenericStoreProcedure;
        clsDropDownList clsdropdown = new clsDropDownList();
        BusinessLogicLayer.Contact oContactGeneralBL = new BusinessLogicLayer.Contact();
        string[] strSpParam;
        DataSet dsbranchhrchy = new DataSet();
        BusinessLogicLayer.DriverMasterBL drvBl = new BusinessLogicLayer.DriverMasterBL();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            // Code  Added and Commented By Priti on 21122016 to add Convert.ToString instead of ToString()
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/BranchAddEdit.aspx");
                Session["requesttype"] = "Driver";
                Session["ContactType"] = "Driver";
               
                TabPage page = PageControl1.TabPages.FindByName("Documents");
                page.Enabled = true;
                page = PageControl1.TabPages.FindByName("Correspondence");
                page.Enabled = true;
                page = PageControl1.TabPages.FindByName("UDF");
                page.Enabled = true;
                btnUdf.Attributes.Remove("disabled");
                btnUdf.Enabled = true;
               
              
                if (!IsPostBack)
                {

                   // IsUdfpresent.Value = Convert.ToString(getUdfCount());
                    ShowForm();
                    cmbParentBranch.SelectedValue = Convert.ToString(HttpContext.Current.Session["userbranchID"]);
                }
                if (Convert.ToString(Request.QueryString["id"]) == "ADD")
                {
                    lblAddEdit.Text = "Add Service Center";
                }
                else
                {
                    lblAddEdit.Text = "Modify Service Center";
                }
              
              
            }
            catch { }
        }
        [WebMethod]
        public static bool CheckUniqueName(string ShortName, string divId)
        {
            string ShortCode = "0";

            if (HttpContext.Current.Session["KeyVal_InternalID"] != null)
            {
                ShortCode = Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]);
            }
            if (divId=="0")
            {
                ShortCode = "0";
            }
            MShortNameCheckingBL objMShortNameCheckingBL = new MShortNameCheckingBL();
            DataTable dt = new DataTable();
            Boolean status = false;
            BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();
            if (ShortCode != "" && Convert.ToString(ShortName).Trim() != "")
            {
                status = objMShortNameCheckingBL.CheckUnique(ShortName, ShortCode, "ServiceCeneter");
            }
            return status;
        }
        //  protected int getUdfCount()
        //{
        //    DataTable udfCount = oDBEngine.GetDataTable("select 1 from tbl_master_remarksCategory rc where cat_applicablefor='Br'   and ( exists (select * from tbl_master_udfGroup where id=rc.cat_group_id and grp_isVisible=1) or rc.cat_group_id=0)");
        //    return udfCount.Rows.Count;
        //}
        
          protected void btnSave_Click(object sender, EventArgs e)
          {
              try
              {

                  if (Convert.ToString(Request.QueryString["id"]) == "ADD")
                  {
                      string name = "";
                      string Code = "";
                      string PBranch = "";
                      string Remarks = "";
                    
                      if (txtCode.Text.Trim() != "")
                      {
                          Code = Convert.ToString(txtCode.Text);
                      }
                      else
                      {
                          
                      }


                      if (txtCentername.Text.Trim() != "")
                      {
                          name = Convert.ToString(txtCentername.Text);
                      }
                      else
                      {

                          
                      }

                      if (cmbParentBranch.SelectedItem.Value != "")
                      {
                          PBranch = cmbParentBranch.SelectedItem.Value;
                      }
                      else
                      {
                          PBranch = Convert.ToString(DBNull.Value);
                      }


                      if (txtRemarks.Text != "")
                      {
                          Remarks = Convert.ToString(txtRemarks.Text);
                      }
                      else
                      {
                          Remarks = Convert.ToString(DBNull.Value);
                      }

                  

                        string retData=drvBl.InsertServiceCenter("ServiceCenter",Code.Trim(),name.Trim(),Convert.ToInt32(PBranch),Remarks.Trim(),chkIsActive.Checked,Convert.ToString(HttpContext.Current.Session["userid"]),"SC","ADD");

                        string[,] cnt_id = oDBEngine.GetFieldValue(" tbl_master_contact", " cnt_id", " cnt_internalId='" + retData + "'", 1);
                        if (Convert.ToString(cnt_id[0, 0]) != "n")
                        {
                            
                            Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script>popUpRedirect('DriverAddEdit.aspx?id=" + Convert.ToString(cnt_id[0, 0]) + "');</script>");

                        }

                  }
                  else
                  {
                      string name = "";
                      string Code = "";
                      string PBranch = "";
                      string Remarks = "";
                    

                      if (Request.QueryString["id"] != null)
                      {
                          if (txtCode.Text.Trim() != "")
                          {
                              Code = Convert.ToString(txtCode.Text);
                          }
                          else
                          {

                              ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "jAlert('Please enter service center name');", true);
                              return;
                              //Code = DBNull.Value.ToString();
                          }


                          if (txtCentername.Text.Trim() != "")
                          {
                              name = Convert.ToString(txtCentername.Text);
                          }
                          else
                          {

                              ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "jAlert('Please enter service center name');", true);
                              return;
                              //Code = DBNull.Value.ToString();
                          }

                          if (cmbParentBranch.SelectedItem.Value != "")
                          {
                              PBranch = cmbParentBranch.SelectedItem.Value;
                          }
                          else
                          {
                              PBranch = Convert.ToString(DBNull.Value);
                          }


                          if (txtRemarks.Text != "")
                          {
                              Remarks = Convert.ToString(txtRemarks.Text);
                          }
                          else
                          {
                              Remarks = Convert.ToString(DBNull.Value);
                          }
                          Boolean retData = drvBl.UpdateServiceCenter("Driver", Code, name, Convert.ToInt32(PBranch), Remarks, chkIsActive.Checked, Convert.ToString(HttpContext.Current.Session["userid"]), "DV",Convert.ToInt32(Request.QueryString["id"]), "Edit");
                          Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script>jAlert('Saved Successfully..');</script>");
                      }
                  }
              }
              catch { }
          }
          protected void ShowForm()
           {

            if (Convert.ToString(Request.QueryString["id"]) == "ADD")
            {
                Session["requesttype"] = "";
                Session["ContactType"] = "";

                string[,] Data = oDBEngine.GetFieldValue("tbl_master_branch ", "branch_id as parentId, branch_description as ParentBranch", null, 2, "branch_description");
                clsdropdown.AddDataToDropDownList(Data, cmbParentBranch);
                
               
                cmbParentBranch.Items.Insert(0, new ListItem("None", "0"));
                TabPage page = PageControl1.TabPages.FindByName("Documents");
                page.Enabled = false;
                page = PageControl1.TabPages.FindByName("Correspondence");
                page.Enabled = false;
                page = PageControl1.TabPages.FindByName("UDF");
                page.Enabled = false;
                btnUdf.Attributes.Add("disabled", "disabled");
                btnUdf.Enabled = false;
                hiddenedit.Value = "";
            }
            else
            {
                if (Request.QueryString["id"] != null)
                {
                    Session["cnt_id"] = Convert.ToString(Request.QueryString["id"]);
                    string[,] DT = oDBEngine.GetFieldValue("tbl_master_contact ", "tbl_master_contact.cnt_UCC as driver_no,tbl_master_contact.cnt_firstName as driver_name,tbl_master_contact.cnt_branchid,tbl_master_contact.cnt_VerifcationRemarks,tbl_master_contact.Is_Active,tbl_master_contact.cnt_internalId", "cnt_id=" + Request.QueryString["id"], 6);
                    if (DT[0, 0] != "n")
                    HttpContext.Current.Session["KeyVal"] = Convert.ToString(Request.QueryString["id"]);
                    
                    hiddenedit.Value = Convert.ToString(Request.QueryString["id"]);
                    strSpParam = new string[1];
                    strSpParam[0] = "branchid|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Int + "|10|" + Request.QueryString["id"] + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;

                    oGenericStoreProcedure = new BusinessLogicLayer.GenericStoreProcedure();

                    dsbranchhrchy = oGenericStoreProcedure.Procedure_DataSet(strSpParam, "Hr_GetBranchSubTree");
                    string[,] Data = oDBEngine.GetFieldValue("tbl_master_branch ", "branch_id as parentId, branch_description as ParentBranch", null, 2, "branch_description");
                    {
                        if (DT[0, 2] != "")
                        {
                            clsdropdown.AddDataToDropDownList(Data, cmbParentBranch, int.Parse(Convert.ToString(DT[0, 2])));
                            cmbParentBranch.Items.Insert(0, new ListItem("None", "0"));
                            cmbParentBranch.SelectedValue = DT[0, 3];
                        }
                        else
                        {

                            clsdropdown.AddDataToDropDownList(Data, cmbParentBranch, 0);
                        }

                        txtCode.Text = DT[0, 0];
                        txtCentername.Text = DT[0, 1];
                        txtRemarks.Text = DT[0, 3];
                        chkIsActive.Checked = Convert.ToBoolean(DT[0, 4]);
                        Session["KeyVal_InternalID"] = DT[0, 5];
                    }
               
                }
                
            }



        }

          protected void Button2_Click(object sender, EventArgs e)
          {
              Response.Redirect("ServiceCenterList.aspx");
          }
        
      

    }
}
