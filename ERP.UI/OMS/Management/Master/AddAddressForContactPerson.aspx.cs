using System;
using System.Data;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_AddAddressForContactPerson : ERP.OMS.ViewState_class.VSPage
    {
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                //if (Session["prePageUrl"] == null)
                //{
                //    string previousPageUrl = string.Empty;
                //    if (Request.UrlReferrer != null)
                //        previousPageUrl = Request.UrlReferrer.AbsoluteUri;
                //    else
                //        previousPageUrl = Page.ResolveUrl("~/OMS/Management/ProjectMainPage.aspx");

                //    Session["prePageUrl"] = previousPageUrl;
                //    goBackCrossBtn.NavigateUrl = previousPageUrl;
                //}
                BindAddress();
                SetCountry();

            }
        }
       
        public void BindAddress()
        {
            // .............................Code Commented and Added by Sam on 12122016 to use Convert.tostring instead of tostring(). ................
            DataTable dtAddress = oDBEngine.GetDataTable("tbl_master_address", "add_addressType,add_address1,add_address2,add_address3,add_landMark,add_country,add_state,add_city,add_area,add_pin,(select top 1 cou_country from tbl_master_country where cou_id=tbl_master_address.add_country) as Country,(select top 1 state from tbl_master_state where id=tbl_master_address.add_state) as State,(select top 1 city_name from tbl_master_city where city_id=tbl_master_address.add_city) as City,(select top 1 area_name from tbl_master_area where area_id=tbl_master_address.add_area) as Area", " add_cntId='" + Convert.ToString(Request.QueryString["id"]) + "'");
            if (dtAddress.Rows.Count > 0)
            {
                ddlAddressType.SelectedValue =Convert.ToString( dtAddress.Rows[0][0]);
                txtAddress1.Text =Convert.ToString(dtAddress.Rows[0][1]);
                txtAddress2.Text = Convert.ToString(dtAddress.Rows[0][2]);
                txtAddress3.Text = Convert.ToString(dtAddress.Rows[0][3]);
                txtLandmark.Text = Convert.ToString(dtAddress.Rows[0][4]);
                txtCountry_hidden.Value = Convert.ToString(dtAddress.Rows[0][5]);
                txtState_hidden.Value = Convert.ToString(dtAddress.Rows[0][6]);
                txtCity_hidden.Value = Convert.ToString(dtAddress.Rows[0][7]);
                txtArea_hidden.Value = Convert.ToString(dtAddress.Rows[0][8]);
                txtPincode_hidden.Value = Convert.ToString(dtAddress.Rows[0][9]);
               // txtPincode.Text = dtAddress.Rows[0][9].ToString();
                //txtCountry.Text = dtAddress.Rows[0][10].ToString();
               // txtState.Text = dtAddress.Rows[0][11].ToString();
                //txtCity.Text = dtAddress.Rows[0][12].ToString();
              //  txtArea.Text = dtAddress.Rows[0][13].ToString();
            }
            else
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
        }
        // .............................Code Above Commented and Added by Sam on 12122016 to use Convert.tostring instead of tostring(). ..................................... 
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //if (  txtAddress1.Text.Trim() == "") 
            //{
            //    if (txtAddress1.Text.Trim() == "")
            //    {
            //        Page.ClientScript.RegisterStartupScript(GetType(), "JScript618", "<script language='javascript'>Page_Load();</script>");
            //        Page.ClientScript.RegisterStartupScript(GetType(), "JScript615", "<script language='javascript'>alert('Please Select Address1 Field');</script>");
            //        return;
            //    }

                //else if (txtCountry.Text.Trim() == "")
                //{
                //    Page.ClientScript.RegisterStartupScript(GetType(), "JScript118", "<script language='javascript'>Page_Load();</script>");
                //    Page.ClientScript.RegisterStartupScript(GetType(), "JScript115", "<script language='javascript'>alert('Please Select Country');</script>");
                //    return;
                //}
                //else if (txtState.Text.Trim() == "")
                //{
                //    Page.ClientScript.RegisterStartupScript(GetType(), "JScript418", "<script language='javascript'>Page_Load();</script>");
                //    Page.ClientScript.RegisterStartupScript(GetType(), "JScript415", "<script language='javascript'>alert('Please Select State');</script>");
                //    return;
                //}

                //else if (txtCity.Text.Trim() == "")
                //{
                //    Page.ClientScript.RegisterStartupScript(GetType(), "JScript18", "<script language='javascript'>Page_Load();</script>");
                //    Page.ClientScript.RegisterStartupScript(GetType(), "JScript15", "<script language='javascript'>alert('Please Select City');</script>");
                //    return;
                //}

                //if (txtArea.Text.Length == 0)
                //{
                //    Page.ClientScript.RegisterStartupScript(GetType(), "JScript218", "<script language='javascript'>Page_Load();</script>");
                //    Page.ClientScript.RegisterStartupScript(GetType(), "JScript215", "<script language='javascript'>alert('Please Select Area');</script>");
                //}

                //else if (txtPincode.Text.Trim() == "")
                //{
                //    Page.ClientScript.RegisterStartupScript(GetType(), "JScript518", "<script language='javascript'>Page_Load();</script>");
                //    Page.ClientScript.RegisterStartupScript(GetType(), "JScript515", "<script language='javascript'>alert('Please Select Pincode');</script>");
                //    return;
                //}
            //}
            //else
            //{

             // .............................Code Commented and Added by Sam on 12122016 to use Convert.tostring instead of tostring(). ................
                //string FieldValue = "add_address1='" + txtAddress1.Text + "',add_address2='" + txtAddress2.Text + "',add_address3='" + txtAddress3.Text + "',add_landMark='" + txtLandmark.Text + "',add_country='" + txtCountry_hidden.Value + "',add_state='" + txtState_hidden.Value + "',add_city='" + txtCity_hidden.Value + "',add_area='" + txtArea_hidden.Value + "',add_pin='" + txtPincode.Text + "',LastModifyUser='" + Session["userid"].ToString() + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "'";
            try
            {
                // .............................Code Commented and Added by Sam on 12122016 to use Convert.tostring instead of tostring(). ................
                string FieldValue = "add_address1='" + txtAddress1.Text + "',add_address2='" + txtAddress2.Text + "',add_address3='" + txtAddress3.Text + "',add_landMark='" + txtLandmark.Text + "',add_country='" + txtCountry_hidden.Value + "',add_state='" + txtState_hidden.Value + "',add_city='" + txtCity_hidden.Value + "',add_area='" + txtArea_hidden.Value + "',add_pin='" + Convert.ToString(txtPincode_hidden.Value) + "',LastModifyUser='" +Convert.ToString( Session["userid"]) + "',LastModifyDate='" + oDBEngine.GetDate() + "'";

                Int32 NoofRowsEffect = oDBEngine.SetFieldValue("tbl_master_address", FieldValue, " add_cntId='" + Convert.ToString(Request.QueryString["id"]) + "' and add_addressType='" + ddlAddressType.SelectedItem.Value + "'");
                if (NoofRowsEffect == 0)
                {
                    string FieldName = "add_cntId,add_addressType,add_address1,add_address2,add_address3,add_landMark,add_country,add_state,add_city,add_area,add_pin,CreateUser,CreateDate";
                    FieldValue = "'" + Convert.ToString(Request.QueryString["id"]) + "','" + ddlAddressType.SelectedItem.Value + "','" + txtAddress1.Text + "','" + txtAddress2.Text + "','" + txtAddress3.Text + "','" + txtLandmark.Text + "','" + txtCountry_hidden.Value + "','" + txtState_hidden.Value + "','" + txtCity_hidden.Value + "','" + txtArea_hidden.Value + "','" + Convert.ToString(txtPincode_hidden.Value) + "','" + Convert.ToString(Session["userid"]) + "','" + oDBEngine.GetDate() + "'";
                    Int32 RowsEffect = oDBEngine.InsurtFieldValue("tbl_master_address", FieldName, FieldValue);
                }
                //Page.ClientScript.RegisterStartupScript(GetType(), "JS", "<script language='javascript'>window.close();</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "JS", "<script language='javascript'>alert('Updated Successfully.')</script>");
            }
            catch (Exception ex) { 
            
            }
           // }
            // .............................Code Above Commented and Added by Sam on 12122016 to use Convert.tostring instead of tostring(). ..................................... 
        }


        public void SetCountry()
        {
            //objEngine
            DataTable DT = new DataTable();
            DT = oDBEngine.GetDataTable("tbl_master_country", "  ltrim(rtrim(cou_country)) Name,ltrim(rtrim(cou_id)) Code ", null);
            lstCountry.DataSource = DT;
            lstCountry.DataMember = "Code";
            lstCountry.DataTextField = "Name";
            lstCountry.DataValueField = "Code";
            lstCountry.DataBind();
        }



    }
}