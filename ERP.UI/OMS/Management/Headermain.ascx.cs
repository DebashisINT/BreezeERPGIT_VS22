using System;
using System.Configuration;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
public partial class Headermain : System.Web.UI.UserControl
{
    clsDropDownList cls = new clsDropDownList();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //Response.Write("<script language='javaScript'> alert('sssss !!') </script>");
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

            lblCompHead.Text = oDBEngine.GetFieldValue("tbl_master_company", "cmp_name", "cmp_id=45", 1)[0, 0].ToString();
            
            
            if (Session["username"] != null)
            {
                Label2.Visible = true;
                Label1.Visible = true;
                //lblName.Text = oDBEngine.GetFieldValue("tbl_master_user", "user_name", "user_id=58", 1)[0].ToString();
                lblName.Text = oDBEngine.GetFieldValue("tbl_master_user", " (user_name + '['+ (select cnt_shortName from tbl_master_contact where cnt_internalId=tbl_master_user.user_contactId )+']') as name ", "user_id = " + Session["userid"].ToString(), 1)[0, 0].ToString();
                lblLastTime.Text = oDBEngine.GetFieldValue(" tbl_master_user", "last_login_date", "user_id = " + Session["userid"].ToString(), 1)[0, 0].ToString();
                string[,] segmnet = oDBEngine.GetFieldValue(" tbl_master_userGroup", " grp_segmentid", " grp_id in (" + HttpContext.Current.Session["usergoup"] + ")", 1);
                string segmentList = "";
                if (segmnet.Length > 0)
                {
                    for (int i = 0; i < segmnet.Length; i++)
                    {
                        segmentList += segmnet[i,0].ToString() + ",";
                    }
                }
                segmentList = segmentList.Substring(0, segmentList.Length - 1);
                string[,] ValidUserSegment = oDBEngine.GetFieldValue(" tbl_master_segment",
                                                               " seg_id,seg_name",
                                                               " seg_id in(" + segmentList + ")", 2);

                // Calling a function to add values to a dropdownlist
                // First argument is data array and secont is comboboxID
                cls.AddDataToDropDownList(ValidUserSegment, cmbSegment, int.Parse(HttpContext.Current.Session["userlastsegment"].ToString()));
                if (cmbSegment.SelectedItem.Value != "")
                {
                    oDBEngine.PopulateMenu(Menumain, cmbSegment.SelectedItem.Value);
                }

                if (cmbSegment.SelectedItem.Value.Trim() == "7" || cmbSegment.SelectedItem.Value.Trim() == "8" || cmbSegment.SelectedItem.Value.Trim() == "9" || cmbSegment.SelectedItem.Value.Trim() == "10" || cmbSegment.SelectedItem.Value.Trim() == "3" || cmbSegment.SelectedItem.Value.Trim() == "5" || cmbSegment.SelectedItem.Value.Trim() == "11" || cmbSegment.SelectedItem.Value.Trim() == "12")
                {
                    tblSegment.Visible = true;

                    //string[,] data = oDBEngine.GetFieldValue(" tbl_master_company ", " cmp_Name ,(select convert(varchar(12),Settlements_StartDateTime,113) from Master_Settlements where (RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix))='" + HttpContext.Current.Session["LastSettNo"].ToString() + "') ,(select convert(varchar(12),Settlements_FundsPayin,113) from Master_Settlements where (RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix))='" + HttpContext.Current.Session["LastSettNo"].ToString() + "') ", " cmp_internalid ='" + HttpContext.Current.Session["LastCompany"].ToString() + "'", 3);
                    string[,] data = oDBEngine.GetFieldValue(" tbl_trans_LastSegment ", " (select top 1 cmp_Name from tbl_master_company where cmp_internalid=ls_lastCompany) as comp," +
                        " ls_lastSettlementNo+ls_lastSettlementType as sett," +
                        " (select top 1 convert(varchar(12),Settlements_StartDateTime,113) from Master_Settlements where (RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix))=(ls_lastSettlementNo+ls_lastSettlementType)) ," +
                        " (select top 1 convert(varchar(12),Settlements_FundsPayin,113) from Master_Settlements where (RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix))=(ls_lastSettlementNo+ls_lastSettlementType)) ,ls_lastFinYear,ls_lastdpcoid as dpid " ,
                        " ls_cntId='" + HttpContext.Current.Session["usercontactID"].ToString() + "' and ls_lastSegment='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'", 6);
                    if (data[0, 0] != "n")
                    {
                        lblSCompName.Text = data[0, 0];
                        if (data[0, 1] != "")
                        {
                            lblSettNo.Text = data[0, 1];
                            lblStartDate.Text = data[0, 2];
                            lblfundPayeeDate.Text = data[0, 3];
                            lblFinYear.Text = data[0, 4];
                            HttpContext.Current.Session["LastFinYear"] = data[0, 4];
                            HttpContext.Current.Session["LastSettNo"] = data[0, 1];
                        }
                        else
                        {
                            if (cmbSegment.SelectedItem.Value.Trim() != "3")
                            {
                                lblSettNo.Text = data[0, 5];
                            }
                            else
                            {
                                lblSettNo.Text = "";
                            }
                            lblStartDate.Text = data[0, 4];

                        }
                        //lblStartDate.Text = data[0, 2];
                        //lblfundPayeeDate.Text = data[0, 3];
                        //lblFinYear.Text = data[0, 4];
                        HttpContext.Current.Session["LastFinYear"] = data[0, 4];
                        if (cmbSegment.SelectedItem.Value.Trim() != "3")
                        {
                            HttpContext.Current.Session["LastSettNo"] = data[0, 1];
                        }
                        else
                        {
                            HttpContext.Current.Session["LastSettNo"] = string.Empty;

                        }
             
                    }
                    else
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "Segment", "<script language='javascript'>OnSegmentChange('" + cmbSegment.SelectedItem.Value + "');</script>");
                }
                else
                    tblSegment.Visible = false;
                }
            else
            {
                Label2.Visible = false;
                Label1.Visible = false;
                //PanelSetting.Visible = false;

            }

        }
        lnkSelectCompanySettFinYear.HRef = "javascript:showpage('" + HttpContext.Current.Session["userlastsegment"].ToString() + "')";
    }
    protected void cmbSegment_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbSegment.SelectedItem.Value != "")
        {
            //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            DBEngine oDBEngine = new DBEngine();

            //-------Update tbl_master_user according to the segment selected------//
            oDBEngine.SetFieldValue("tbl_master_user", "user_lastsegement=" + cmbSegment.SelectedItem.Value, " user_id = '" + HttpContext.Current.Session["userid"] + "'");
            HttpContext.Current.Session["userlastsegment"] = cmbSegment.SelectedItem.Value;
            string[,] segId = oDBEngine.GetFieldValue("tbl_trans_LastSegment", "ls_lastdpcoid,ls_lastCompany,ls_lastFinYear,ls_lastSettlementNo", "ls_lastSegment='" + cmbSegment.SelectedItem.Value + "' and ls_cntId='" + HttpContext.Current.Session["usercontactID"] + "'", 4);
            if (segId[0, 0] != "n")
            {
                HttpContext.Current.Session["usersegid"] = segId[0, 0].ToString().Trim();
                HttpContext.Current.Session["LastCompany"] = segId[0, 1].ToString();
                HttpContext.Current.Session["LastFinYear"] = segId[0, 2].ToString();
                HttpContext.Current.Session["LastSettNo"] = segId[0, 3].ToString();
            }
            string segmentname = cmbSegment.SelectedItem.Text.ToString();
            string[] sname = segmentname.Split('-');
            if (sname.Length > 1)
            {
                string[] ExchangeSegmentID = oDBEngine.GetFieldValue1("Master_ExchangeSegments MES,Master_Exchange ME", "MES.ExchangeSegment_ID", "MES.ExchangeSegment_Code='" + sname[1] + "'And MES.ExchangeSegment_ExchangeID=ME.Exchange_ID AND ME.Exchange_ShortName='" + sname[0] + "'", 1);
                HttpContext.Current.Session["ExchangeSegmentID"] = ExchangeSegmentID[0].ToString();
            }

            //-------End-----------------------------------------------------------//
            //-------Populating menue According to the Segment---------------------//

            //oDBEngine.PopulateMenu(Menumain, cmbSegment.SelectedItem.Value);
            //-------End-----------------------------------------------------------//
            //__Now Check wether segment is of type CM,FO and populate page to select company,Fin.Year,Sett. No.
            //if (cmbSegment.SelectedItem.Value.Trim() == "7" || cmbSegment.SelectedItem.Value.Trim() == "8")
            //{
            //    if (HttpContext.Current.Session["LastSettNo"].ToString() == "")
            //        Page.ClientScript.RegisterStartupScript(this.GetType(), "Segment", "<script language='javascript'>OnSegmentChange('" + cmbSegment.SelectedItem.Value + "');</script>");
            //    tblSegment.Visible = true;
            //}
            //else
            Response.Redirect("../management/welcome.aspx", false);
        }
    }
}
