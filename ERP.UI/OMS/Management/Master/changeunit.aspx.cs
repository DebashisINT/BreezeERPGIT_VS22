using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;
using System.Web.Services;
using System.Collections.Generic;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_changeunit : ERP.OMS.ViewState_class.VSPage
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        string id = "";
        string convDateTime = "";
        string convDateTime2 = "";
        string convDateTime3 = "";
        string final = "";
        string substr = "";
        string final2 = "";
        string substr2 = "";
        string final3 = "";
        string substr3 = "";
        string send = "";
        string[] id1 = new string[0];
        DataTable DT = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            if (Request.QueryString["id"] != "Add")
            {

                id = Convert.ToString(Request.QueryString["id"]);
                id1 = Request.QueryString["id"].Split('(');


                DataTable dt2 = oDBEngine.GetDataTable("select  uom_name from Master_UOM where UOM_id='" + id1[0] + "'");
                litSegment.InnerText = Convert.ToString(dt2.Rows[0]["uom_name"]);


                HiddenField1.Value = id1.ToString();
                hdAddEdit.Value = "Edit";
                lblHeading.Text = "Change Unit";
                trExistingUnit.Visible = true;
                trChangeTo.Visible = true;
                trunit.Visible = false;
                trshort.Visible = false;
                truseFor.Visible = false;
            }
            else
            {
                trExistingUnit.Visible = false;
                trChangeTo.Visible = false;
                trunit.Visible = true;
                trshort.Visible = true;
                truseFor.Visible = true;
                hdAddEdit.Value = "Add";
            }
        
           
        }


        [WebMethod]
        public static List<string> GetUOM(string reqStr)
        {
            
                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]); MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable DT = new DataTable();
                DT.Rows.Clear();
                DT = oDBEngine.GetDataTable("select uom_ID, uom_name from Master_UOM where uom_name Like '" + reqStr.Trim() + "%' or UOM_shortName like '" + reqStr.Trim() + "%'");
                List<string> obj = new List<string>();
                foreach (DataRow dr in DT.Rows)
                {

                    obj.Add(Convert.ToString(dr["uom_name"]) + "|" + Convert.ToString(dr["uom_ID"]));
                }
                return obj;
            
        }
        protected void btnYes_Click(object sender, EventArgs e)
        {

            if (hdAddEdit.Value == "Add")
            {
                string StrShortName=Convert.ToString(txtShortUnit.Value);
                string PrintName = Convert.ToString(txtShortUnit.Value).ToUpper();
                DataTable dt2 = oDBEngine.GetDataTable("select  UOM_ShortName from Master_UOM where UOM_ShortName='" + StrShortName.Trim() + "'");
                if(dt2.Rows.Count>0)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script>alert('Short Name already exists..')</script>"); 
                }
                else
                {
                    int noofRows = 0;
                    //Mantis Issue 0024849
                    //noofRows = oDBEngine.InsurtFieldValue("Master_uom", "UOM_Name,UOM_UsedFor,UOM_ShortName", "'" + txtUnit.Value + "','" + txtUseFor.Value + "','" + txtShortUnit.Value + "'");
                    noofRows = oDBEngine.InsurtFieldValue("Master_uom", "UOM_Name,UOM_UsedFor,UOM_ShortName,EInvoice_Print_Name", "'" + txtUnit.Value + "','" + txtUseFor.Value + "','" + txtShortUnit.Value + "','" + PrintName + "'");
                    //End of Mantis Issue 0024849

                    if (noofRows > 0)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script>alert('Saved Successfully..');   window.location.href='/OMS/management/Master/frm_UOM.aspx'</script>");
                    }
                }
                
            }
            else
            {
                string uomvalu = txtconverttounit_hidden.Value;
                if (!string.IsNullOrEmpty(uomvalu))
                {
                    int noofrow = oDBEngine.SetFieldValue("Master_uom", "UOM_ConvUOM='" + uomvalu + "'", "uom_id='" + id1[0] + "'");
                    string p1 = id;
                    string popUpscript = "";
                    popUpscript = "<script language='javascript'>window.opener.PopulateGrid('" + p1 + "');window.close();</script>";
                    ClientScript.RegisterStartupScript(GetType(), "JScript", popUpscript);
                    string popUpscript1 = "";
                    popUpscript1 = "jAlert('Successfully Saved'); window.location ='frm_UOM.aspx'; ";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, popUpscript1, true);

                }
                else
                {

                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript15", "<script language='javascript'>jAlert('Invalid Unit.');</script>");
                }
            }
            // comment by sanjib due to chnaged textbox to choosen 212017
            //if (txtproduct_hidden.Text.Length > 0)
            //{
            //    int noofrow = oDBEngine.SetFieldValue("Master_uom", "UOM_ConvUOM='" + txtproduct_hidden.Text + "'", "uom_id='" + id1[0] + "'");
            //    string p1 = id;// +"/" + "0" + "/" + "1";
            //    string popUpscript = "";
            //    popUpscript = "<script language='javascript'>window.opener.PopulateGrid('" + p1 + "');window.close();</script>";
            //    ClientScript.RegisterStartupScript(GetType(), "JScript", popUpscript);
            //    string popUpscript1 = "";
            //    popUpscript1 = "jAlert('Successfully Saved'); window.location ='frm_UOM.aspx'; ";
            //    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, popUpscript1, true);
            //    //Response.Redirect("welcome.aspx", false);
            //}
            //else
            //{
            //    Page.ClientScript.RegisterStartupScript(GetType(), "JScript15", "<script language='javascript'>jAlert('Invalid Unit.');</script>");
            //}


        }
        protected void btnNo_Click(object sender, EventArgs e)
        {
            //Page.ClientScript.RegisterStartupScript(GetType(), "JScript4", "<script>parent.editwin.close();</script>");
            Response.Redirect("frm_UOM.aspx");
        }
    }
}