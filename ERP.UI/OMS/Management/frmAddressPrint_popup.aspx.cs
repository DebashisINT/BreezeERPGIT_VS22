using System;
using System.Data;
using System.Web.UI;

namespace ERP.OMS.Management
{
    public partial class management_frmAddressPrint_popup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "height", "<script>height();</script>");
            populateAddressGrid();

        }
        protected void populateAddressGrid()
        {
            DataSet dsnew = (DataSet)Session["DatasetMain"];
            //DataSet dsnew = (DataSet)number;
            //string whereClause = " add_cntid <> ''"; //and add_cntId=cnt_internalId and cou_id=add_country and id=add_state and add_area=area_id and cnt_branchid=branch_id and c1.city_id=add_city and cnt_contacttype=cnt_prefix ";
            //if (Request.QueryString["ClientName"].ToString() != "All")
            //{
            //    whereClause = whereClause + " and cnt_firstname like '" + Request.QueryString["client"].Trim().ToString() + "%'";
            //}
            //if (Request.QueryString["client"].ToString() != "All")
            //{
            //    whereClause = whereClause + " and  cnttpy_contacttype='" + Request.QueryString["contype"].Trim().ToString() + "'";
            //}
            //if (Request.QueryString["addtype"].ToString() != "All")
            //{
            //    whereClause = whereClause + " and add_addressType='" + Request.QueryString["addtype"].Trim().ToString() + "'";
            //}
            //if (Request.QueryString["state"].ToString() != "All")
            //{
            //    whereClause = whereClause + " and state='" + Request.QueryString["state"].Trim().ToString() + "'";
            //}
            //if (Request.QueryString["city"].ToString() != "All")
            //{
            //    whereClause = whereClause + " and city_name='" + Request.QueryString["city"].Trim().ToString() + "'";
            //}
            //if (Request.QueryString["area"].ToString() != "All")
            //{
            //    whereClause = whereClause + " and area_name='" + Request.QueryString["area"].Trim().ToString() + "'";
            //}
            //if (Request.QueryString["branch"].ToString() != "All")
            //{
            //    whereClause = whereClause + " and branch_description='" + Request.QueryString["branch"].Trim().ToString() + "'";
            //}

            //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            //DataTable dt = oDBEngine.GetDataTable("tbl_master_contactstatus,tbl_master_address,tbl_master_contact,tbl_master_country,tbl_master_state,tbl_master_area,tbl_master_branch,tbl_master_contacttype,tbl_master_city c1", "Distinct branch_id,cnttpy_contactType,(select top 1 phf_phonenumber from tbl_master_phonefax where phf_cntid=add_cntid) as phf_phonenumber,cnt_lastname,branch_description,area_name,state,cnt_middlename,cnt_firstname, add_id, add_cntId, cnt_ucc, add_addressType, add_address1, add_address2, add_address3, add_landMark, cnt_internalId,cou_country,add_pin,city_name= c1.city_name", whereClause, "branch_id");
            DataTable dtAddress = new DataTable();
            dtAddress.Columns.Add("Add1", String.Empty.GetType());
            dtAddress.Columns.Add("Add2", String.Empty.GetType());
            dtAddress.Columns.Add("Add3", String.Empty.GetType());
            int count = 0;
            for (int i = 0; i < dsnew.Tables[0].Rows.Count; i++)
            {
                string Address = dsnew.Tables[0].Rows[i]["ClientName"].ToString().Trim() + " [" + dsnew.Tables[0].Rows[i]["Ucc"].ToString().Trim() + "]" +
                                "</br>" + dsnew.Tables[0].Rows[i]["AddRess1"].ToString().Trim() + "-" + dsnew.Tables[0].Rows[i]["AddRess2"].ToString().Trim() +
                                "</br>" + dsnew.Tables[0].Rows[i]["AddRess3"].ToString().Trim() +
                                "</br>" + dsnew.Tables[0].Rows[i]["CityName"].ToString().Trim() + "-" + dsnew.Tables[0].Rows[i]["pin"].ToString().Trim() +
                                "</br>" + dsnew.Tables[0].Rows[i]["StateName"].ToString().Trim() + "-" + dsnew.Tables[0].Rows[i]["CountryName"].ToString().Trim() +
                               "</br>" + dsnew.Tables[0].Rows[i]["phone"].ToString().Trim();

                dtAddress.Rows.Add(Address);
            }
            ASPxDataView1.DataSource = dtAddress;
            ASPxDataView1.DataBind();
            //Session["DatasetMain"] = null;
        }

    }
}