using System;
using System.Data;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Management
{
    public partial class management_CallUserInformation : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        string strScrolling;
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        clsDropDownList clsDropDownList = new clsDropDownList();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindRating();
                ShowInformation();
                String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
                String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            }

            drpRating.Attributes.Add("onchange", "valueChange()");
            Page.ClientScript.RegisterStartupScript(GetType(), "test", "<script language='javascript'>height();</script>");
        }
        public void BindRating()
        {
            string[,] Rating = oDBEngine.GetFieldValue("tbl_master_LeadRating", "rat_id as Id,rat_LeadRating as Rating", null, 2);
            if (Rating[0, 0] != "n")
            {
                clsDropDownList.AddDataToDropDownList(Rating, drpRating);
            }
        }
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string a = eventArgument.ToString();
            Session["mode"] = a;
        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return strScrolling;

        }
        public void ShowInformation()
        {
            try
            {
                SetValueForLables("N/A");
                string LeadID = "";
                string PhcActiID = "";
                DataSet ds = new DataSet();
                if (Session["SalesVisitID"] != null)
                {
                    string SalesID = Session["SalesVisitID"].ToString();
                    ds = oDBEngine.PopulateData("svo.slv_SalesVisitOutcome as call_dispositions, sv.slv_nextvisitdatetime as phc_nextCall, sv.slv_lastdatevisit as phc_callDate,  sv.slv_leadcotactId as phc_leadcotactId, sv.slv_activityId as phc_activityId", "tbl_trans_salesVisit sv ,tbl_master_SalesVisitOutCome svo", " sv.slv_salesvisitoutcome = svo.slv_Id and sv.slv_id =" + SalesID);
                }
                else
                {
                    if (Session["phonecallid"] != null)
                    {
                        string PhoneId = Session["phonecallid"].ToString();
                        ds = oDBEngine.PopulateData("tbl_master_calldispositions.call_dispositions, tbl_trans_phonecall.phc_nextCall, tbl_trans_phonecall.phc_callDate,phc_leadcotactId,phc_activityId", "tbl_trans_phonecall INNER JOIN tbl_master_calldispositions ON tbl_trans_phonecall.phc_callDispose = tbl_master_calldispositions.call_id", " tbl_trans_phonecall.phc_id = '" + PhoneId + "'");
                    }
                }
                if (ds.Tables[0].Rows.Count != 0)
                {
                    LeadID = ds.Tables[0].Rows[0]["phc_leadcotactId"].ToString();
                    Session["LeadId"] = LeadID.ToString();
                    PhcActiID = ds.Tables[0].Rows[0]["phc_activityId"].ToString();
                    //FillProductDetails(LeadID, PhcActiID);
                }
                DataSet Lead = new DataSet();
                string LID = LeadID.Substring(0, 2);
                if (LID == "LD")
                {
                    Lead = oDBEngine.PopulateData("*", "tbl_master_lead", " cnt_internalid='" + LeadID + "'");
                }
                else
                {
                    Lead = oDBEngine.PopulateData("*", "tbl_master_contact", " cnt_internalid='" + LeadID + "'");
                }
                if (Lead.Tables[0].Rows.Count != 0)
                {
                    string val = Lead.Tables[0].Rows[0]["cnt_rating"].ToString();
                    try
                    {
                        if (val != "")
                        {
                            string[,] ratingg = oDBEngine.GetFieldValue("tbl_master_LeadRating", "rat_LeadRating", " rat_id='" + val + "'", 1);
                            if (ratingg[0, 0] != "n")
                            {
                                drpRating.SelectedItem.Text = ratingg[0, 0];
                                Session["mode"] = val;
                            }
                        }
                    }
                    catch
                    {
                    }
                    lblLeadName.Text = Lead.Tables[0].Rows[0]["cnt_firstName"].ToString() + " " + Lead.Tables[0].Rows[0]["cnt_middleName"].ToString() + " " + Lead.Tables[0].Rows[0]["cnt_lastName"].ToString();
                    string Dob;
                    if (Lead.Tables[0].Rows[0]["cnt_dOB"].ToString() != "1/1/1900 12:00:00 AM")
                    {
                        Dob = Lead.Tables[0].Rows[0]["cnt_dOB"].ToString();
                    }
                    else
                    {
                        Dob = "";

                    }

                    string ProId = Lead.Tables[0].Rows[0]["cnt_profession"].ToString();
                    if (ProId == "")
                    {
                        lblProfession.Text = "N/A";
                    }
                    else
                    {
                        string[,] prof = oDBEngine.GetFieldValue("tbl_master_profession", "pro_professionName", "pro_id=" + ProId, 1);
                        if (prof[0, 0] != "n")
                        {
                            lblProfession.Text = prof[0, 0];
                        }
                    }

                    if (Dob != "")
                    {
                        int date_ofBirth = Dob.LastIndexOf('/');
                        string Dob3 = Dob.Substring(date_ofBirth + 1, 4);
                        int Dob4 = Convert.ToInt32(Dob3);
                        int Dob1 = Convert.ToInt32(oDBEngine.GetDate().Year);
                        int Dob2 = Dob1 - Dob4;
                        lblAge.Text = Dob2.ToString() + " , ";
                    }
                    else if (Dob == "")
                    {
                        lblAge.Text = "N/A , ";
                    }
                }
                DataSet PhoneFax = new DataSet();
                PhoneFax = oDBEngine.PopulateData("*", "tbl_master_phonefax", "phf_cntId='" + LeadID + "'");
                if (PhoneFax.Tables[0].Rows.Count != 0)
                {
                    for (int i = 0; i < PhoneFax.Tables[0].Rows.Count; i++)
                    {
                        string TypePh = PhoneFax.Tables[0].Rows[i]["phf_type"].ToString().ToUpper();
                        switch (TypePh)
                        {
                            case "RESIDENCE":
                                lblRes.Text = PhoneFax.Tables[0].Rows[i]["phf_countryCode"].ToString() + " " + PhoneFax.Tables[0].Rows[i]["phf_areaCode"].ToString() + " " + PhoneFax.Tables[0].Rows[i]["phf_phoneNumber"].ToString() + "-" + PhoneFax.Tables[0].Rows[i]["phf_extension"].ToString();
                                lblFax.Text = PhoneFax.Tables[0].Rows[i]["phf_faxNumber"].ToString();
                                break;
                            case "OFFICE":
                                lblOffice.Text = PhoneFax.Tables[0].Rows[i]["phf_countryCode"].ToString() + " " + PhoneFax.Tables[0].Rows[i]["phf_areaCode"].ToString() + " " + PhoneFax.Tables[0].Rows[i]["phf_phoneNumber"].ToString() + "-" + PhoneFax.Tables[0].Rows[i]["phf_extension"].ToString();
                                lblFax.Text = PhoneFax.Tables[0].Rows[i]["phf_faxNumber"].ToString();
                                break;
                            case "FAX":
                                lblFax.Text = PhoneFax.Tables[0].Rows[i]["phf_countryCode"].ToString() + " " + PhoneFax.Tables[0].Rows[i]["phf_areaCode"].ToString() + " " + PhoneFax.Tables[0].Rows[i]["phf_phoneNumber"].ToString() + "-" + PhoneFax.Tables[0].Rows[i]["phf_extension"].ToString();
                                break;
                            case "MOBILE":
                                lblMobile.Text = PhoneFax.Tables[0].Rows[i]["phf_countryCode"].ToString() + " " + PhoneFax.Tables[0].Rows[i]["phf_phoneNumber"].ToString();
                                break;
                            default:
                                lblRes.Text = PhoneFax.Tables[0].Rows[i]["phf_countryCode"].ToString() + " " + PhoneFax.Tables[0].Rows[i]["phf_areaCode"].ToString() + " " + PhoneFax.Tables[0].Rows[i]["phf_phoneNumber"].ToString() + "-" + PhoneFax.Tables[0].Rows[i]["phf_extension"].ToString();
                                lblFax.Text = PhoneFax.Tables[0].Rows[i]["phf_faxNumber"].ToString();
                                break;
                        }
                    }
                }
                DataSet EMail = new DataSet();
                EMail = oDBEngine.PopulateData("*", "tbl_master_email", "eml_cntId='" + LeadID + "'");
                if (EMail.Tables[0].Rows.Count != 0)
                {
                    lblemailid.Text = EMail.Tables[0].Rows[0]["eml_email"].ToString();
                }
                DataSet Address = new DataSet();
                string strAddress = "";
                Address = oDBEngine.PopulateData("*", "tbl_master_address", "add_cntId='" + LeadID + "'");
                if (Address.Tables[0].Rows.Count != 0)
                {
                    string pscript = "";
                    pscript += "<script language='javascript'>";
                    pscript += "var sel = window.parent.document.getElementById('drpNextVisitPlace');";
                    pscript += "for (i = 0; i < sel.options.length; i++) { sel.options[i] = null;  }";
                    pscript += " var txt; var addOption;";
                    string jScript = "";
                    jScript += "<script language='javascript'>";
                    jScript += "var sel = window.parent.document.getElementById('drpVisitPlace');";
                    jScript += "for (i = 0; i < sel.options.length; i++) { sel.options[i] = null;  }";
                    jScript += " var txt; var addOption;";
                    for (int j = 0; j < Address.Tables[0].Rows.Count; j++)
                    {
                        strAddress += Address.Tables[0].Rows[j]["add_address1"].ToString() + "@@@@" + Address.Tables[0].Rows[j]["add_id"].ToString() + "||";
                        jScript += "addOption = new Option('" + Address.Tables[0].Rows[j]["add_address1"].ToString() + "','" + Address.Tables[0].Rows[j]["add_id"].ToString() + "');";
                        jScript += "sel.options[" + j + "] = addOption;";
                        pscript += "addOption = new Option('" + Address.Tables[0].Rows[j]["add_address1"].ToString() + "','" + Address.Tables[0].Rows[j]["add_id"].ToString() + "');";
                        pscript += "sel.options[" + j + "] = addOption;";
                        string AddType = Address.Tables[0].Rows[j]["add_addresstype"].ToString().ToUpper();
                        if (AddType != "")
                        {
                            switch (AddType)
                            {
                                case "RESIDENCE":
                                    lblRes1.Text = Address.Tables[0].Rows[j]["add_address1"].ToString();
                                    lblRes2.Text = Address.Tables[0].Rows[j]["add_address2"].ToString();
                                    lblRes3.Text = Address.Tables[0].Rows[j]["add_address3"].ToString();
                                    break;
                                case "OFFICE":
                                    lblOffice1.Text = Address.Tables[0].Rows[j]["add_address1"].ToString();
                                    lblOffice2.Text = Address.Tables[0].Rows[j]["add_address2"].ToString();
                                    break;
                                default:
                                    lblRes1.Text = Address.Tables[0].Rows[j]["add_address1"].ToString();
                                    lblRes2.Text = Address.Tables[0].Rows[j]["add_address2"].ToString();
                                    lblRes3.Text = Address.Tables[0].Rows[j]["add_address3"].ToString();
                                    break;
                            }
                        }
                    }
                    jScript += "</script>";
                    pscript += "</script>";
                    if (Session["SalesVisitID"] != null)
                    {
                        string scr = "<script language='javascript'>saveAddressString('" + strAddress + "')</script>";
                        ClientScript.RegisterStartupScript(GetType(), "scr", scr);
                        string scr1 = "<script language='javascript'>saveAddressString123('" + strAddress + "')</script>";
                        ClientScript.RegisterStartupScript(GetType(), "scr1", scr1);
                    }
                    else
                    {
                        string scr = "<script language='javascript'>saveAddressString('" + strAddress + "')</script>";
                        ClientScript.RegisterStartupScript(GetType(), "scr", scr);
                    }
                }
            }
            catch
            {
            }
        }

        private void SetValueForLables(string txt)
        {
            lblLeadName.Text = txt;
            lblMobile.Text = txt;
            lblOffice.Text = txt;
            lblOffice1.Text = txt;
            lblOffice2.Text = txt;
            lblFax.Text = txt;
            lblemailid.Text = txt;
            lblRes.Text = txt;
            lblRes1.Text = txt;
            lblRes2.Text = txt;
            lblRes3.Text = txt;
        }



    }
}