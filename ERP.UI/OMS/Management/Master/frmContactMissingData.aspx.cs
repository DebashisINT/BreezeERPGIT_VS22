using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using BusinessLogicLayer;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_frmContactMissingData : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        protected string createUser = null;
        protected string createDate = null;
        protected string modifyuser = null;
        protected string modifydate = null;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        string ContactID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            ContactID = Request.QueryString["id"].ToString();
            DataTable dtuser = oDBEngine.GetDataTable("select convert(varchar(19),ISNULL(createdate,''),100) as createdate,(select top 1 user_name +' [ '+user_loginId +' ] ' from tbl_master_user,tbl_master_contact where user_id=tbl_master_contact.CreateUser and cnt_internalId='" + ContactID + "') as user_name,(case when isnull(lastmodifydate,'1900-01-01 00:00:00.000')=''then'' else convert(varchar(19),ISNULL(lastmodifydate,''),100)end) as modifydate,(select top 1 user_name +' [ '+user_loginId +' ] ' from tbl_master_user,tbl_master_contact where user_id=tbl_master_contact.lastmodifyuser and cnt_internalId='" + ContactID + "') as user_name from tbl_master_contact where cnt_internalId='" + ContactID + "'");
            fillHTML();
            if (dtuser.Rows[0] != null)
            {
                createDate = dtuser.Rows[0][0].ToString();
                createUser = dtuser.Rows[0][1].ToString();
                modifydate = dtuser.Rows[0][2].ToString();
                modifyuser = dtuser.Rows[0][3].ToString();
                if ((modifyuser == "") && (modifydate == ""))
                {
                    Label2.Visible = false;
                    Label3.Visible = false;
                }
            }
        }
        protected void fillHTML()
        {
            DataSet ds = new DataSet();
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))MULTI
            using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
            {
                using (SqlDataAdapter da = new SqlDataAdapter("Fetch_ContactMissingDetails", con))
                {
                    da.SelectCommand.Parameters.Add("@ContactID", ContactID);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 0;
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    ds.Reset();
                    da.Fill(ds);
                    // Mantis Issue 24802
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                    // End of Mantis Issue 24802
                }

            }
            DataTable dtCnt = ds.Tables[0];

            int j = 0;
            int p = 0;
            int n = 0;
            int dp = 0;
            int bn = 0;
            int dc = 0;
            int did = 0;
            string DispTbl = "";
            DispTbl = "<table cellspacing=\"1\"  cellpadding=\"2\" style=\"width:550px;background-color:#F2F5A9;border:solid 1px #000000\" border=\"1\">";
            DispTbl = DispTbl + "<tr><td colspan=\"2\" aligin=\"left\">Following Records are not available.<td></tr>";
            if (dtCnt.Rows.Count > 0)
            {

                foreach (DataRow dr in dtCnt.Rows)
                {
                    for (int i = 0; i < dtCnt.Columns.Count; i++)
                    {
                        if (i != 0)
                        {
                            if (dr[i].ToString() != "Y")
                            {
                                if (dr[i].ToString() == "Address Details")
                                {
                                    p = p + 1;

                                    if (p == 1)
                                    {
                                        j = j + 1;
                                        DispTbl = DispTbl + "<tr><td align=\"left\">" + "[" + j + "]" + "</td><td align=\"left\">" + dr[i].ToString() + "</td></tr>";
                                    }
                                }
                                //else if (dr[i].ToString().Trim() == "Office Address Details")
                                //{
                                //    n = n + 1;

                                //    if (n == 1)
                                //    {
                                //        j = j + 1;
                                //        DispTbl = DispTbl + "<tr><td align=\"left\">" + "[" + j + "]" + "</td><td align=\"left\">" + dr[i].ToString() + "</td></tr>";
                                //    }

                                //}
                                else if (dr[i].ToString().Trim() == "DP Detail")
                                {
                                    dp = dp + 1;

                                    if (dp == 1)
                                    {
                                        j = j + 1;
                                        DispTbl = DispTbl + "<tr><td align=\"left\">" + "[" + j + "]" + "</td><td align=\"left\">" + dr[i].ToString() + "</td></tr>";
                                    }

                                }
                                else if (dr[i].ToString().Trim() == "DP Detail")
                                {
                                    dp = dp + 1;

                                    if (dp == 1)
                                    {
                                        j = j + 1;
                                        DispTbl = DispTbl + "<tr><td align=\"left\">" + "[" + j + "]" + "</td><td align=\"left\">" + dr[i].ToString() + "</td></tr>";
                                    }

                                }
                                else if (dr[i].ToString().Trim() == "Bank Details")
                                {
                                    bn = bn + 1;

                                    if (bn == 1)
                                    {
                                        j = j + 1;
                                        DispTbl = DispTbl + "<tr><td align=\"left\">" + "[" + j + "]" + "</td><td align=\"left\">" + dr[i].ToString() + "</td></tr>";
                                    }

                                }

                                else if (dr[i].ToString().Trim() == "Address Proof")
                                {
                                    dc = dc + 1;

                                    if (dc == 1)
                                    {
                                        j = j + 1;
                                        DispTbl = DispTbl + "<tr><td align=\"left\">" + "[" + j + "]" + "</td><td align=\"left\">" + dr[i].ToString() + "</td></tr>";
                                    }

                                }
                                else if (dr[i].ToString().Trim() == "ID Proof")
                                {
                                    did = did + 1;

                                    if (did == 1)
                                    {
                                        j = j + 1;
                                        DispTbl = DispTbl + "<tr><td align=\"left\">" + "[" + j + "]" + "</td><td align=\"left\">" + dr[i].ToString() + "</td></tr>";
                                    }

                                }
                                else
                                {

                                    j = j + 1;
                                    DispTbl = DispTbl + "<tr><td align=\"left\">" + "[" + j + "]" + "</td><td align=\"left\">" + dr[i].ToString() + "</td></tr>";
                                }
                            }
                        }
                    }

                }

            }

            DataTable dtDocType = oDBEngine.GetDataTable("TBL_MASTER_DOCUMENTTYPE ", " dty_id,dty_documentType ", "DTY_APPLICABLEFOR='Customer/Client' AND DTY_MANDATORY=1 ");
            for (int k = 0; k < dtDocType.Rows.Count; k++)
            {
                DataTable dtDoc = oDBEngine.GetDataTable("TBL_MASTER_DOCUMENT ", " * ", " DOC_CONTACTID='" + ContactID + "' and DOC_DOCUMENTTYPEID ='" + dtDocType.Rows[k]["dty_id"].ToString() + "'");
                if (dtDoc.Rows.Count == 0)
                {
                    j = j + 1;
                    DispTbl = DispTbl + "<tr><td align=\"left\">" + "[" + j + "]" + "</td><td align=\"left\">" + dtDocType.Rows[k]["dty_documentType"].ToString() + "</td></tr>";
                }
            }
            DispTbl = DispTbl + "</table>";
            display.InnerHtml = DispTbl;
        }
    }
}