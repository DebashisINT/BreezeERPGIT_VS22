using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;
using DevExpress.Web;


namespace ERP.OMS.Reports
{
    public partial class Reports_AddEmail : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        ClsDropDownlistNameSpace.clsDropDownList cls = new ClsDropDownlistNameSpace.clsDropDownList();
        ExcelFile objExcel = new ExcelFile();
        Converter oconverter = new Converter();
        static DataSet ds = new DataSet();
        //DBEngine oDbEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        DBEngine oDbEngine = new DBEngine();
        DBEngine oDBEngine = new DBEngine(string.Empty);
        string data;
        static decimal debittotal = decimal.Zero;
        static decimal credittotal = decimal.Zero;
        static decimal clientnet = decimal.Zero;
        static decimal clientnetpro = decimal.Zero;
        static decimal exchobligation = decimal.Zero;
        static decimal stttexch = decimal.Zero;
        static decimal brkgprov = decimal.Zero;
        static decimal roundofftrade = decimal.Zero;
        static decimal other = decimal.Zero;
        static decimal diff = decimal.Zero;

        static string segment;
        static string company;
        string datefor;
        static string SubClients;


        protected void Page_Load(object sender, EventArgs e)
        {
            txtUserId.Attributes.Add("onkeyup", "callAjax(this,'GetMailId',event)");
            txtSelectionID.Attributes.Add("onkeyup", "callAjax1(this,'GetMailId',event)");
            FillCombo();

            if (!IsPostBack)
            {
                Session["DataTable_Email"] = new DataTable();
            }
            //  datefor = "7/30/2009 12:00:00 AM";
            string datf = Request.QueryString["dt"].ToString();
            string[] dt = datf.Split('-');
            string dd = dt[0].ToString();
            string mm = dt[1].ToString();
            string yy = dt[2].ToString();
            datefor = mm + "/" + dd + "/" + yy + " 12:00:00 AM";
            string type = Request.QueryString["type"].ToString();

            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);

        }

        private void FillCombo()
        {
            if (HttpContext.Current.Session["userlastsegment"].ToString() == "4")
            {
                string[,] r = new string[1, 2];
                r[0, 0] = "EM";
                r[0, 1] = "Employees";
                cls.AddDataToDropDownList(r, cmbContactType);
            }
            else
            {
                string[,] r = new string[9, 2];
                r[0, 0] = "EM";
                r[0, 1] = "Employees";
                r[1, 0] = "CL";
                r[1, 1] = "Customers";
                r[2, 0] = "LD";
                r[2, 1] = "Lead";
                r[3, 0] = "CD";
                r[3, 1] = "CDSL Client";
                r[4, 0] = "ND";
                r[4, 1] = "NSDL Client";
                r[5, 0] = "BP";
                r[5, 1] = "Business Partne";
                r[6, 0] = "RA";
                r[6, 1] = "Relationship Partner";
                r[7, 0] = "SB";
                r[7, 1] = "Sub Broker";
                r[8, 0] = "FR";
                r[8, 1] = "Franchisees";
                cls.AddDataToDropDownList(r, cmbContactType);
                cls.AddDataToDropDownList(r, cmbsearchOption);

            }

        }


        protected void GridCategory_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            LoadAllProductCat();


        }

        private void LoadAllProductCat()
        {
            string usr = txtUserId.Text;
            string[] nm = usr.ToString().Split('<');
            string name = nm[0].ToString();
            string[] em = nm[1].ToString().Split('>');
            string email = em[0].ToString();
            DataTable DTCategory = (DataTable)Session["DataTable_Email"];
            if (DTCategory.Rows.Count != 0)
            {
                for (int i = 0; i < DTCategory.Rows.Count; i++)
                {
                    if (DTCategory.Rows[i]["ID"].ToString() == txtUserId_hidden.Text)
                    {

                    }
                }
            }

            //if (DTCategory.Rows.Count != 0)
            //{
            if (DTCategory.Columns.Count == 0)
            {
                //___________creating Columns Here
                DataColumn ID = new DataColumn("ID");
                DataColumn Name = new DataColumn("Name");
                DataColumn Email = new DataColumn("Email");
                //___________Adding to datatable DTCategory
                DTCategory.Columns.Add(ID);
                DTCategory.Columns.Add(Name);
                DTCategory.Columns.Add(Email);
            }

            DataRow newRowForDT = DTCategory.NewRow();
            newRowForDT["ID"] = txtUserId_hidden.Text;
            newRowForDT["Name"] = name;
            newRowForDT["Email"] = email;
            DTCategory.Rows.Add(newRowForDT);
            GridCategory.DataSource = DTCategory;
            GridCategory.DataBind();
            GridCategory.Columns[0].Visible = false;
            txtUserId.Text = "";
            txtUserId_hidden.Text = "";
            Session["DataTable_Email"] = DTCategory;

            // }
        }



        protected string GetRowColor(int i)
        {

            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }
        void procedure()
        {

            debittotal = decimal.Zero;
            credittotal = decimal.Zero;
            clientnet = decimal.Zero;
            clientnetpro = decimal.Zero;
            exchobligation = decimal.Zero;
            stttexch = decimal.Zero;
            brkgprov = decimal.Zero;
            roundofftrade = decimal.Zero;
            other = decimal.Zero;
            diff = decimal.Zero;
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))MULTI
            using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "[Sp_SettlementTrialNSEFO]";

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@date", datefor);
                cmd.Parameters.AddWithValue("@segment", Convert.ToInt32(Session["usersegid"].ToString()));
                cmd.Parameters.AddWithValue("@Companyid", Session["LastCompany"].ToString());
                cmd.Parameters.AddWithValue("@MasterSegment", HttpContext.Current.Session["ExchangeSegmentID"].ToString());

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds.Reset();
                da.Fill(ds);

            }
        }

        void htmltableclientwise()
        {
            if (ds.Tables[0].Rows[0]["status"].ToString() == "1")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Rates", "alert('Rates for this date does not exists');", true);

            }
            else
            {
                if (ds.Tables[1].Rows.Count != 0)
                {
                    //////////////////////MAIN TABLE/////////////////////////
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "display", "displayalldata();", true);

                    String strHtmlAllClient = String.Empty;

                    strHtmlAllClient = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                    strHtmlAllClient += "<tr><td align=\"left\" colspan=13 style=\"color:Blue;\"><b>Settelment Trail for " + oconverter.ArrangeDate2(datefor.ToString()) + "</b></td></tr>";

                    strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
                    strHtmlAllClient += "<td align=\"center\" >Client Name</td>";
                    strHtmlAllClient += "<td align=\"center\">Code</td>";
                    strHtmlAllClient += "<td align=\"center\">Branch</td>";
                    strHtmlAllClient += "<td align=\"center\">MTM</td>";
                    strHtmlAllClient += "<td align=\"center\">Premium</td>";
                    strHtmlAllClient += "<td align=\"center\">Fut Final Settlement</td>";
                    strHtmlAllClient += "<td align=\"center\">Exc / Asn Settlement</td>";
                    strHtmlAllClient += "<td align=\"center\">Tran Charges</td>";
                    strHtmlAllClient += "<td align=\"center\">Serv Tax & Cess</td>";
                    strHtmlAllClient += "<td align=\"center\">Sec Tran Charges</td>";
                    strHtmlAllClient += "<td align=\"center\">Stamp Duty</td>";
                    strHtmlAllClient += "<td align=\"center\">Net Receivable (Dr.)</td>";
                    strHtmlAllClient += "<td align=\"center\">Net Payable (Cr.)</td></tr>";

                    int flag = 0;
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        flag = flag + 1;
                        strHtmlAllClient += "<tr style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                        strHtmlAllClient += "<td align=\"left\" >" + ds.Tables[1].Rows[i]["Name"].ToString() + "</td>";
                        strHtmlAllClient += "<td align=\"left\">" + ds.Tables[1].Rows[i]["code"].ToString() + "</td>";
                        strHtmlAllClient += "<td align=\"left\">" + ds.Tables[1].Rows[i]["branchcode"].ToString() + "</td>";

                        if (ds.Tables[1].Rows[i]["mtm"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[1].Rows[i]["mtm"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (ds.Tables[1].Rows[i]["premium"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[1].Rows[i]["premium"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (ds.Tables[1].Rows[i]["futfinal"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[1].Rows[i]["futfinal"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (ds.Tables[1].Rows[i]["asnexc"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[1].Rows[i]["asnexc"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (ds.Tables[1].Rows[i]["trancharge"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[1].Rows[i]["trancharge"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (ds.Tables[1].Rows[i]["srvtaxcess"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[1].Rows[i]["srvtaxcess"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (ds.Tables[1].Rows[i]["Sttax"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[1].Rows[i]["Sttax"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (ds.Tables[1].Rows[i]["StampDuty"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[1].Rows[i]["StampDuty"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (ds.Tables[1].Rows[i]["NetDr"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[1].Rows[i]["NetDr"].ToString()))) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (ds.Tables[1].Rows[i]["NetCr"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[1].Rows[i]["NetCr"].ToString()))) + "</td></tr>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td></tr>";
                        }


                    }


                    strHtmlAllClient += "<tr style=\"background-color: #F0F8FF;\">";
                    strHtmlAllClient += "<td align=\"left\" colspan=3><b>Client Net</b></td>";
                    strHtmlAllClient += "<td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[2].Rows[0]["summtm"].ToString())) + "</b></td>";
                    strHtmlAllClient += "<td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[2].Rows[0]["sumpremium"].ToString()))) + "</b></td>";
                    strHtmlAllClient += "<td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[2].Rows[0]["sumfinset"].ToString()))) + "</b></td>";
                    strHtmlAllClient += "<td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[2].Rows[0]["sumasnexc"].ToString()))) + "</b></td>";
                    strHtmlAllClient += "<td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[2].Rows[0]["sumtrancharge"].ToString())) + "</b></td>";
                    strHtmlAllClient += "<td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[2].Rows[0]["sumsrvtaxcess"].ToString())) + "</b></td>";
                    strHtmlAllClient += "<td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[2].Rows[0]["sumSttax"].ToString())) + "</b></td>";
                    strHtmlAllClient += "<td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[2].Rows[0]["StampDuty"].ToString())) + "</b></td>";


                    clientnet = Convert.ToDecimal(ds.Tables[2].Rows[0]["NetCr"]) - Math.Abs(Convert.ToDecimal(ds.Tables[2].Rows[0]["NetDr"]));
                    if (clientnet < 0)
                    {
                        strHtmlAllClient += "<td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(clientnet))) + "</b></td>";
                        strHtmlAllClient += "<td align=\"right\">&nbsp;</td></tr>";
                        debittotal = debittotal + Math.Abs(clientnet);
                    }
                    else
                    {
                        strHtmlAllClient += "<td align=\"right\">&nbsp;</td>";
                        strHtmlAllClient += "<td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(clientnet))) + "</b></td></tr>";
                        credittotal = credittotal + Math.Abs(clientnet);
                    }
                    ///////////////////////////////////////Pro Client Record Bind////////////////////////////////////
                    strHtmlAllClient += "<tr style=\"background-color: white\">";
                    strHtmlAllClient += "<td align=\"left\" colspan=3>Pro Account Obligation :</td>";
                    if (ds.Tables[3].Rows[0]["summtmPro"] != DBNull.Value)
                    {
                        strHtmlAllClient += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[3].Rows[0]["summtmPro"].ToString())) + "</td>";
                    }
                    else
                    {
                        strHtmlAllClient += "<td align=\"right\">&nbsp;</td>";

                    }
                    if (ds.Tables[3].Rows[0]["sumpremiumPro"] != DBNull.Value)
                    {
                        strHtmlAllClient += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[3].Rows[0]["sumpremiumPro"].ToString()))) + "</td>";
                    }
                    else
                    {
                        strHtmlAllClient += "<td align=\"right\">&nbsp;</td>";

                    }
                    if (ds.Tables[3].Rows[0]["finsetPro"] != DBNull.Value)
                    {
                        strHtmlAllClient += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[3].Rows[0]["finsetPro"].ToString()))) + "</td>";
                    }
                    else
                    {
                        strHtmlAllClient += "<td align=\"right\">&nbsp;</td>";

                    }
                    if (ds.Tables[3].Rows[0]["asnexcPro"] != DBNull.Value)
                    {
                        strHtmlAllClient += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[3].Rows[0]["asnexcPro"].ToString()))) + "</td>";
                    }
                    else
                    {
                        strHtmlAllClient += "<td align=\"right\">&nbsp;</td>";

                    }
                    strHtmlAllClient += "<td align=\"right\">&nbsp;</td>";
                    strHtmlAllClient += "<td align=\"right\">&nbsp;</td>";

                    if (ds.Tables[3].Rows[0]["sumSttaxPro"] != DBNull.Value)
                    {
                        strHtmlAllClient += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[3].Rows[0]["sumSttaxPro"].ToString())) + "</td>";
                    }
                    else
                    {
                        strHtmlAllClient += "<td align=\"right\">&nbsp;</td>";

                    }

                    if (ds.Tables[3].Rows[0]["NetCrPro"] == DBNull.Value)
                    {
                        ds.Tables[3].Rows[0]["NetCrPro"] = 0.0;
                    }
                    if (ds.Tables[3].Rows[0]["NetDrPro"] == DBNull.Value)
                    {
                        ds.Tables[3].Rows[0]["NetDrPro"] = 0.0;
                    }
                    clientnetpro = Convert.ToDecimal(ds.Tables[3].Rows[0]["NetCrPro"]) - Math.Abs(Convert.ToDecimal(ds.Tables[3].Rows[0]["NetDrPro"]));
                    if (clientnetpro < 0)
                    {
                        strHtmlAllClient += "<td align=\"right\">&nbsp;</td>";
                        strHtmlAllClient += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(clientnetpro))) + "</td>";
                        strHtmlAllClient += "<td align=\"right\">&nbsp;</td></tr>";
                        debittotal = debittotal + Math.Abs(clientnetpro);
                    }
                    else
                    {
                        if (clientnetpro == Convert.ToDecimal(0.0))
                        {
                            strHtmlAllClient += "<td align=\"right\">&nbsp;</td>";
                            strHtmlAllClient += "<td align=\"right\">&nbsp;</td>";
                            strHtmlAllClient += "<td align=\"right\">&nbsp;</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td align=\"right\">&nbsp;</td>";
                            strHtmlAllClient += "<td align=\"right\">&nbsp;</td>";
                            strHtmlAllClient += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(clientnetpro))) + "</td></tr>";
                        }
                        credittotal = credittotal + Math.Abs(clientnetpro);
                    }

                    ///////////////////////////////////////END////////////////////////////////////////////

                    /////////////////////////////////////Exchange Obligation////////////////////////////////

                    exchobligation = Convert.ToDecimal(ds.Tables[4].Rows[0]["Exchangeobligation"].ToString());
                    if (exchobligation > 0)
                    {
                        strHtmlAllClient += "<tr ><td colspan=11 align=\"left\"><b>Exchange Obligation :</b></td><td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(exchobligation))) + "</b></td><td></td></tr>";
                        debittotal = debittotal + Math.Abs(exchobligation);
                    }
                    else
                    {
                        strHtmlAllClient += "<tr ><td colspan=11 align=\"left\"><b>Exchange Obligation :</b></td><td></td><td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(exchobligation))) + "</b></td></tr>";
                        credittotal = credittotal + Math.Abs(exchobligation);
                    }
                    ///////////////////////////////////////////END/////////////////////////////////////////////

                    /////////////////////////////////////////STT Payable To Exchange///////////////////////// 
                    if (ds.Tables[5].Rows[0]["exchstttax"] != DBNull.Value)
                    {
                        stttexch = Convert.ToDecimal(ds.Tables[5].Rows[0]["exchstttax"].ToString());
                    }
                    strHtmlAllClient += "<tr style=\"background-color: white\"><td colspan=11 align=\"left\"><b>STT Payable To Exchange :<b></td><td></td><td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(stttexch))) + "</b></td></tr>";
                    ////////////////////////////////////////////END///////////////////////////////////////////

                    /////////////////////////////////////////Brkg For Prov///////////////////////// 

                    brkgprov = Convert.ToDecimal(ds.Tables[2].Rows[0]["totalbrkg"].ToString());
                    ////////////////////////////////////////////END///////////////////////////////////////

                    strHtmlAllClient += "<tr ><td colspan=11 align=\"left\">Brokerage Income :</td><td></td><td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(brkgprov)) + "</td></tr>";
                    strHtmlAllClient += "<tr style=\"background-color: white\"><td colspan=11 align=\"left\">Transaction Charges Collected :</td><td></td><td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[2].Rows[0]["sumtrancharge"].ToString())) + "</td></tr>";
                    strHtmlAllClient += "<tr ><td colspan=11 align=\"left\">Stamp Duty Collected :</td><td></td><td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[2].Rows[0]["StampDuty"].ToString())) + "</td></tr>";
                    strHtmlAllClient += "<tr style=\"background-color: white\"><td colspan=11 align=\"left\">Service Tax Payable:</td><td></td><td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[2].Rows[0]["sumsrvtaxcess"].ToString())) + "</td></tr>";

                    ///////////////////////////////////////////////Service Tax Unrecoverable(STT Tax 'I')////////////////////////
                    if (ds.Tables[7].Rows[0]["unrecover"] != DBNull.Value)
                    {
                        strHtmlAllClient += "<tr ><td colspan=11 align=\"left\">Service Tax Unrecoverable:</td><td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[7].Rows[0]["unrecover"].ToString())) + "</td><td></td></tr>";
                        debittotal = debittotal + Math.Abs(Convert.ToDecimal(ds.Tables[7].Rows[0]["unrecover"].ToString()));

                    }
                    else
                    {
                        strHtmlAllClient += "<tr ><td colspan=11 align=\"left\">Service Tax Unrecoverable:</td><td align=\"right\">" + 0.0 + "</td><td></td></tr>";
                        debittotal = debittotal + Convert.ToDecimal(0.0);

                    }
                    /////////////////////////////////////////////END///////////////////////////////////////////////

                    ////////////////////////////////////////////Trade Average/////////////////////////////////////

                    if (ds.Tables[6].Rows[0]["tradeaveraging"] == DBNull.Value)
                    {
                        ds.Tables[6].Rows[0]["tradeaveraging"] = 0.0;
                    }
                    if (ds.Tables[8].Rows[0]["diffbrkg"] == DBNull.Value)
                    {
                        ds.Tables[8].Rows[0]["diffbrkg"] = 0.0;
                    }
                    roundofftrade = Convert.ToDecimal(ds.Tables[6].Rows[0]["tradeaveraging"].ToString()) + Convert.ToDecimal(ds.Tables[8].Rows[0]["diffbrkg"].ToString());
                    if (roundofftrade < 0)
                    {
                        strHtmlAllClient += "<tr style=\"background-color: white\"><td colspan=11 align=\"left\">Round-Off (Due To Trade Averaging) :</td><td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(roundofftrade))) + "</td><td></td></tr>";
                        debittotal = debittotal + Math.Abs(roundofftrade);
                    }
                    else
                    {
                        strHtmlAllClient += "<tr style=\"background-color: white\"><td colspan=11 align=\"left\">Round-Off (Due To Trade Averaging) :</td><td></td><td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(roundofftrade))) + "</td></tr>";
                        credittotal = credittotal + Math.Abs(roundofftrade);
                    }

                    ////////////////////////////////////////////END/////////////////////////////////////

                    //////////////////////////////////////STT DIFFERENCE////////////////////////////////////////
                    if (ds.Tables[3].Rows[0]["sumSttaxPro"] == DBNull.Value)
                    {
                        ds.Tables[3].Rows[0]["sumSttaxPro"] = 0.0;
                    }
                    if (Math.Abs(stttexch) - (Math.Abs(Convert.ToDecimal(ds.Tables[2].Rows[0]["sumSttax"].ToString())) + Math.Abs(Convert.ToDecimal(ds.Tables[3].Rows[0]["sumSttaxPro"].ToString()))) > 0)
                    {
                        strHtmlAllClient += "<tr ><td colspan=11 align=\"left\">STT Difference :</td><td align=\"right\" style=\"color:maroon;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(stttexch - (Convert.ToDecimal(ds.Tables[2].Rows[0]["sumSttax"].ToString()) + (Convert.ToDecimal(ds.Tables[3].Rows[0]["sumSttaxPro"].ToString()))))) + "</td><td></td></tr>";
                        debittotal = debittotal + Math.Abs(Math.Abs(stttexch) - (Math.Abs(Convert.ToDecimal(ds.Tables[2].Rows[0]["sumSttax"].ToString())) + Math.Abs(Convert.ToDecimal(ds.Tables[3].Rows[0]["sumSttaxPro"].ToString()))));
                    }
                    else
                    {
                        strHtmlAllClient += "<tr ><td colspan=11 align=\"left\">STT Difference :</td><td></td><td align=\"right\" style=\"color:maroon;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(stttexch - (Convert.ToDecimal(ds.Tables[2].Rows[0]["sumSttax"].ToString()) + (Convert.ToDecimal(ds.Tables[3].Rows[0]["sumSttaxPro"].ToString()))))) + "</td></tr>";
                        credittotal = credittotal + Math.Abs(Math.Abs(stttexch) - (Math.Abs(Convert.ToDecimal(ds.Tables[2].Rows[0]["sumSttax"].ToString())) + Math.Abs(Convert.ToDecimal(ds.Tables[3].Rows[0]["sumSttaxPro"].ToString()))));
                    }
                    ///////////////////////////////////////////END///////////////////////////////////////////////////

                    ////////////////////////////////////////////Credit Site Charges/////////////////////////////////////


                    other = stttexch + brkgprov + Convert.ToDecimal(ds.Tables[2].Rows[0]["sumtrancharge"].ToString()) + Convert.ToDecimal(ds.Tables[2].Rows[0]["StampDuty"].ToString()) + Convert.ToDecimal(ds.Tables[2].Rows[0]["sumsrvtaxcess"].ToString());
                    credittotal = credittotal + other;

                    ///////////////////////////////////////////////END//////////////////////////////////////////////////

                    ///////////////////////////////////////////////Credit and Debit Diff//////////////////////////////////////////////////

                    strHtmlAllClient += "<tr style=\"background-color: white\"><td colspan=11 align=\"left\"><b>Total :</b></td><td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(debittotal))) + "</b></td><td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(credittotal))) + "</b></td></tr>";

                    diff = credittotal - debittotal;
                    if (diff < 0)
                    {
                        strHtmlAllClient += "<tr ><td colspan=11 align=\"left\" style=\"color:maroon;\"><b>Diff(If Any):</b></td><td align=\"right\" style=\"color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(diff))) + "</b></td><td></td></tr>";
                    }
                    else
                    {
                        strHtmlAllClient += "<tr ><td colspan=11 align=\"left\" style=\"color:maroon;\"><b>Diff(If Any):</b></td><td></td><td align=\"right\" style=\"color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(diff))) + "</b></td></tr>";
                    }


                    /////////////////////////////////////////END////////////////////////////////////////////////////

                    if (ds.Tables[1].Rows.Count == 0)
                    {
                        strHtmlAllClient = String.Empty;

                    }


                    strHtmlAllClient += "</table>";

                    displayALLCLIENT.InnerHtml = strHtmlAllClient;
                    Session["htmlTable"] = strHtmlAllClient;
                }

                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "norecord1", "NoRecord();", true);
                }
            }

        }

        void htmltablebranchwise()
        {
            if (ds.Tables[0].Rows[0]["status"].ToString() == "1")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Rates", "alert('Rates for this date does not exists');", true);

            }
            else
            {

                if (ds.Tables[9].Rows.Count != 0)
                {
                    //////////////////////MAIN TABLE/////////////////////////
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "display", "displayalldata();", true);

                    String strHtmlAllClient = String.Empty;

                    strHtmlAllClient = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                    strHtmlAllClient += "<tr><td align=\"left\" colspan=12 style=\"color:Blue;\"><b>Settelment Trail for " + oconverter.ArrangeDate2(datefor.ToString()) + "</b></td></tr>";

                    strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
                    strHtmlAllClient += "<td align=\"center\" >Branch Name</td>";
                    strHtmlAllClient += "<td align=\"center\">Code</td>";
                    strHtmlAllClient += "<td align=\"center\">MTM</td>";
                    strHtmlAllClient += "<td align=\"center\">Premium</td>";
                    strHtmlAllClient += "<td align=\"center\">Fut Final Settlement</td>";
                    strHtmlAllClient += "<td align=\"center\">Exc / Asn Settlement</td>";
                    strHtmlAllClient += "<td align=\"center\">Tran Charges</td>";
                    strHtmlAllClient += "<td align=\"center\">Serv Tax & Cess</td>";
                    strHtmlAllClient += "<td align=\"center\">Sec Tran Charges</td>";
                    strHtmlAllClient += "<td align=\"center\">Stamp Duty</td>";
                    strHtmlAllClient += "<td align=\"center\">Net Receivable (Dr.)</td>";
                    strHtmlAllClient += "<td align=\"center\">Net Payable (Cr.)</td></tr>";

                    int flag = 0;
                    for (int i = 0; i < ds.Tables[9].Rows.Count; i++)
                    {
                        flag = flag + 1;
                        strHtmlAllClient += "<tr style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                        strHtmlAllClient += "<td align=\"left\" >" + ds.Tables[9].Rows[i]["branchname"].ToString() + "</td>";
                        strHtmlAllClient += "<td align=\"left\">" + ds.Tables[9].Rows[i]["branchcode"].ToString() + "</td>";
                        if (ds.Tables[9].Rows[i]["mtm"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[9].Rows[i]["mtm"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (ds.Tables[9].Rows[i]["premium"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[9].Rows[i]["premium"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (ds.Tables[9].Rows[i]["futfinal"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[9].Rows[i]["futfinal"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (ds.Tables[9].Rows[i]["asnexc"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[9].Rows[i]["asnexc"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (ds.Tables[9].Rows[i]["trancharge"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[9].Rows[i]["trancharge"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (ds.Tables[9].Rows[i]["srvtaxcess"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[9].Rows[i]["srvtaxcess"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (ds.Tables[9].Rows[i]["Sttax"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[9].Rows[i]["Sttax"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (ds.Tables[9].Rows[i]["StampDuty"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[9].Rows[i]["StampDuty"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (ds.Tables[9].Rows[i]["NetDr"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[9].Rows[i]["NetDr"].ToString()))) + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                        if (ds.Tables[9].Rows[i]["NetCr"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[9].Rows[i]["NetCr"].ToString()))) + "</td></tr>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td></tr>";
                        }
                    }


                    strHtmlAllClient += "<tr style=\"background-color: #F0F8FF;\">";
                    strHtmlAllClient += "<td align=\"left\" colspan=2><b>Branch Net</b></td>";
                    strHtmlAllClient += "<td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[2].Rows[0]["summtm"].ToString())) + "</b></td>";
                    strHtmlAllClient += "<td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[2].Rows[0]["sumpremium"].ToString()))) + "</b></td>";
                    strHtmlAllClient += "<td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[2].Rows[0]["sumfinset"].ToString()))) + "</b></td>";
                    strHtmlAllClient += "<td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[2].Rows[0]["sumasnexc"].ToString()))) + "</b></td>";
                    strHtmlAllClient += "<td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[2].Rows[0]["sumtrancharge"].ToString())) + "</b></td>";
                    strHtmlAllClient += "<td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[2].Rows[0]["sumsrvtaxcess"].ToString())) + "</b></td>";
                    strHtmlAllClient += "<td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[2].Rows[0]["sumSttax"].ToString())) + "</b></td>";
                    strHtmlAllClient += "<td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[2].Rows[0]["StampDuty"].ToString())) + "</b></td>";


                    clientnet = Convert.ToDecimal(ds.Tables[2].Rows[0]["NetCr"]) - Math.Abs(Convert.ToDecimal(ds.Tables[2].Rows[0]["NetDr"]));
                    if (clientnet < 0)
                    {
                        strHtmlAllClient += "<td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(clientnet))) + "</b></td>";
                        strHtmlAllClient += "<td align=\"right\">&nbsp;</td></tr>";
                        debittotal = debittotal + Math.Abs(clientnet);
                    }
                    else
                    {
                        strHtmlAllClient += "<td align=\"right\">&nbsp;</td>";
                        strHtmlAllClient += "<td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(clientnet))) + "</b></td></tr>";
                        credittotal = credittotal + Math.Abs(clientnet);
                    }
                    ///////////////////////////////////////Pro Client Record Bind////////////////////////////////////
                    strHtmlAllClient += "<tr style=\"background-color: white\">";
                    strHtmlAllClient += "<td align=\"left\" colspan=2>Pro Account Obligation :</td>";
                    if (ds.Tables[3].Rows[0]["summtmPro"] != DBNull.Value)
                    {
                        strHtmlAllClient += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[3].Rows[0]["summtmPro"].ToString())) + "</td>";
                    }
                    else
                    {
                        strHtmlAllClient += "<td align=\"right\">&nbsp;</td>";

                    }
                    if (ds.Tables[3].Rows[0]["sumpremiumPro"] != DBNull.Value)
                    {
                        strHtmlAllClient += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[3].Rows[0]["sumpremiumPro"].ToString()))) + "</td>";
                    }
                    else
                    {
                        strHtmlAllClient += "<td align=\"right\">&nbsp;</td>";

                    }
                    if (ds.Tables[3].Rows[0]["finsetPro"] != DBNull.Value)
                    {
                        strHtmlAllClient += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[3].Rows[0]["finsetPro"].ToString()))) + "</td>";
                    }
                    else
                    {
                        strHtmlAllClient += "<td align=\"right\">&nbsp;</td>";

                    }
                    if (ds.Tables[3].Rows[0]["asnexcPro"] != DBNull.Value)
                    {
                        strHtmlAllClient += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[3].Rows[0]["asnexcPro"].ToString()))) + "</td>";
                    }
                    else
                    {
                        strHtmlAllClient += "<td align=\"right\">&nbsp;</td>";

                    }
                    strHtmlAllClient += "<td align=\"right\">&nbsp;</td>";
                    strHtmlAllClient += "<td align=\"right\">&nbsp;</td>";
                    strHtmlAllClient += "<td align=\"right\">&nbsp;</td>";

                    if (ds.Tables[3].Rows[0]["sumSttaxPro"] != DBNull.Value)
                    {
                        strHtmlAllClient += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[3].Rows[0]["sumSttaxPro"].ToString())) + "</td>";
                    }
                    else
                    {
                        strHtmlAllClient += "<td align=\"right\">&nbsp;</td>";

                    }

                    if (ds.Tables[3].Rows[0]["NetCrPro"] == DBNull.Value)
                    {
                        ds.Tables[3].Rows[0]["NetCrPro"] = 0.0;
                    }
                    if (ds.Tables[3].Rows[0]["NetDrPro"] == DBNull.Value)
                    {
                        ds.Tables[3].Rows[0]["NetDrPro"] = 0.0;
                    }
                    clientnetpro = Convert.ToDecimal(ds.Tables[3].Rows[0]["NetCrPro"]) - Math.Abs(Convert.ToDecimal(ds.Tables[3].Rows[0]["NetDrPro"]));
                    if (clientnetpro < 0)
                    {
                        strHtmlAllClient += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(clientnetpro))) + "</td>";
                        strHtmlAllClient += "<td align=\"right\">&nbsp;</td></tr>";
                        debittotal = debittotal + Math.Abs(clientnetpro);
                    }
                    else
                    {
                        if (clientnetpro == Convert.ToDecimal(0.0))
                        {
                            strHtmlAllClient += "<td align=\"right\">&nbsp;</td>";
                            strHtmlAllClient += "<td align=\"right\">&nbsp;</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td align=\"right\">&nbsp;</td>";
                            strHtmlAllClient += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(clientnetpro))) + "</td></tr>";
                        }
                        credittotal = credittotal + Math.Abs(clientnetpro);
                    }

                    ///////////////////////////////////////END////////////////////////////////////////////

                    /////////////////////////////////////Exchange Obligation////////////////////////////////

                    exchobligation = Convert.ToDecimal(ds.Tables[4].Rows[0]["Exchangeobligation"].ToString());
                    if (exchobligation > 0)
                    {
                        strHtmlAllClient += "<tr ><td colspan=10 align=\"left\"><b>Exchange Obligation :</b></td><td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(exchobligation))) + "</b></td><td></td></tr>";
                        debittotal = debittotal + Math.Abs(exchobligation);
                    }
                    else
                    {
                        strHtmlAllClient += "<tr ><td colspan=10 align=\"left\"><b>Exchange Obligation :</b></td><td></td><td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(exchobligation))) + "</b></td></tr>";
                        credittotal = credittotal + Math.Abs(exchobligation);
                    }
                    ///////////////////////////////////////////END/////////////////////////////////////////////

                    /////////////////////////////////////////STT Payable To Exchange///////////////////////// 
                    if (ds.Tables[5].Rows[0]["exchstttax"] != DBNull.Value)
                    {
                        stttexch = Convert.ToDecimal(ds.Tables[5].Rows[0]["exchstttax"].ToString());
                    }
                    strHtmlAllClient += "<tr style=\"background-color: white\"><td colspan=10 align=\"left\"><b>STT Payable To Exchange :<b></td><td></td><td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(stttexch))) + "</b></td></tr>";
                    ////////////////////////////////////////////END///////////////////////////////////////////

                    /////////////////////////////////////////Brkg For Prov///////////////////////// 

                    brkgprov = Convert.ToDecimal(ds.Tables[2].Rows[0]["totalbrkg"].ToString());
                    ////////////////////////////////////////////END///////////////////////////////////////

                    strHtmlAllClient += "<tr ><td colspan=10 align=\"left\">Brokerage Income :</td><td></td><td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(brkgprov)) + "</td></tr>";
                    strHtmlAllClient += "<tr style=\"background-color: white\"><td colspan=10 align=\"left\">Transaction Charges Collected :</td><td></td><td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[2].Rows[0]["sumtrancharge"].ToString())) + "</td></tr>";
                    strHtmlAllClient += "<tr ><td colspan=10 align=\"left\">Stamp Duty Collected :</td><td></td><td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[2].Rows[0]["StampDuty"].ToString())) + "</td></tr>";
                    strHtmlAllClient += "<tr style=\"background-color: white\"><td colspan=10 align=\"left\">Service Tax Payable:</td><td></td><td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[2].Rows[0]["sumsrvtaxcess"].ToString())) + "</td></tr>";

                    ///////////////////////////////////////////////Service Tax Unrecoverable(STT Tax 'I')////////////////////////
                    if (ds.Tables[7].Rows[0]["unrecover"] != DBNull.Value)
                    {
                        strHtmlAllClient += "<tr ><td colspan=10 align=\"left\">Service Tax Unrecoverable:</td><td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[7].Rows[0]["unrecover"].ToString())) + "</td><td></td></tr>";
                        debittotal = debittotal + Math.Abs(Convert.ToDecimal(ds.Tables[7].Rows[0]["unrecover"].ToString()));

                    }
                    else
                    {
                        strHtmlAllClient += "<tr ><td colspan=10 align=\"left\">Service Tax Unrecoverable:</td><td align=\"right\">" + 0.0 + "</td><td></td></tr>";
                        debittotal = debittotal + Convert.ToDecimal(0.0);

                    }
                    /////////////////////////////////////////////END///////////////////////////////////////////////

                    ////////////////////////////////////////////Trade Average/////////////////////////////////////

                    if (ds.Tables[6].Rows[0]["tradeaveraging"] == DBNull.Value)
                    {
                        ds.Tables[6].Rows[0]["tradeaveraging"] = 0.0;
                    }
                    if (ds.Tables[8].Rows[0]["diffbrkg"] == DBNull.Value)
                    {
                        ds.Tables[8].Rows[0]["diffbrkg"] = 0.0;
                    }
                    roundofftrade = Convert.ToDecimal(ds.Tables[6].Rows[0]["tradeaveraging"].ToString()) + Convert.ToDecimal(ds.Tables[8].Rows[0]["diffbrkg"].ToString());
                    if (roundofftrade < 0)
                    {
                        strHtmlAllClient += "<tr style=\"background-color: white\"><td colspan=10 align=\"left\">Round-Off (Due To Trade Averaging) :</td><td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(roundofftrade))) + "</td><td></td></tr>";
                        debittotal = debittotal + Math.Abs(roundofftrade);
                    }
                    else
                    {
                        strHtmlAllClient += "<tr style=\"background-color: white\"><td colspan=10 align=\"left\">Round-Off (Due To Trade Averaging) :</td><td></td><td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(roundofftrade))) + "</td></tr>";
                        credittotal = credittotal + Math.Abs(roundofftrade);
                    }

                    ////////////////////////////////////////////END/////////////////////////////////////

                    //////////////////////////////////////STT DIFFERENCE////////////////////////////////////////
                    if (ds.Tables[3].Rows[0]["sumSttaxPro"] == DBNull.Value)
                    {
                        ds.Tables[3].Rows[0]["sumSttaxPro"] = 0.0;
                    }
                    if (Math.Abs(stttexch) - (Math.Abs(Convert.ToDecimal(ds.Tables[2].Rows[0]["sumSttax"].ToString())) + Math.Abs(Convert.ToDecimal(ds.Tables[3].Rows[0]["sumSttaxPro"].ToString()))) > 0)
                    {
                        strHtmlAllClient += "<tr ><td colspan=10 align=\"left\">STT Difference :</td><td align=\"right\" style=\"color:maroon;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(stttexch - (Convert.ToDecimal(ds.Tables[2].Rows[0]["sumSttax"].ToString()) + (Convert.ToDecimal(ds.Tables[3].Rows[0]["sumSttaxPro"].ToString()))))) + "</td><td></td></tr>";
                        debittotal = debittotal + Math.Abs(Math.Abs(stttexch) - (Math.Abs(Convert.ToDecimal(ds.Tables[2].Rows[0]["sumSttax"].ToString())) + Math.Abs(Convert.ToDecimal(ds.Tables[3].Rows[0]["sumSttaxPro"].ToString()))));
                    }
                    else
                    {
                        strHtmlAllClient += "<tr ><td colspan=10 align=\"left\">STT Difference :</td><td></td><td align=\"right\" style=\"color:maroon;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(stttexch - (Convert.ToDecimal(ds.Tables[2].Rows[0]["sumSttax"].ToString()) + (Convert.ToDecimal(ds.Tables[3].Rows[0]["sumSttaxPro"].ToString()))))) + "</td></tr>";
                        credittotal = credittotal + Math.Abs(Math.Abs(stttexch) - (Math.Abs(Convert.ToDecimal(ds.Tables[2].Rows[0]["sumSttax"].ToString())) + Math.Abs(Convert.ToDecimal(ds.Tables[3].Rows[0]["sumSttaxPro"].ToString()))));
                    }
                    ///////////////////////////////////////////END///////////////////////////////////////////////////

                    ////////////////////////////////////////////Credit Site Charges/////////////////////////////////////


                    other = stttexch + brkgprov + Convert.ToDecimal(ds.Tables[2].Rows[0]["sumtrancharge"].ToString()) + Convert.ToDecimal(ds.Tables[2].Rows[0]["StampDuty"].ToString()) + Convert.ToDecimal(ds.Tables[2].Rows[0]["sumsrvtaxcess"].ToString());
                    credittotal = credittotal + other;

                    ///////////////////////////////////////////////END//////////////////////////////////////////////////

                    ///////////////////////////////////////////////Credit and Debit Diff//////////////////////////////////////////////////

                    strHtmlAllClient += "<tr style=\"background-color: white\"><td colspan=10 align=\"left\"><b>Total :</b></td><td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(debittotal))) + "</b></td><td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(credittotal))) + "</b></td></tr>";

                    diff = credittotal - debittotal;
                    if (diff < 0)
                    {
                        strHtmlAllClient += "<tr ><td colspan=10 align=\"left\" style=\"color:maroon;\"><b>Diff(If Any):</b></td><td align=\"right\" style=\"color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(diff))) + "</b></td><td></td></tr>";
                    }
                    else
                    {
                        strHtmlAllClient += "<tr ><td colspan=10 align=\"left\" style=\"color:maroon;\"><b>Diff(If Any):</b></td><td></td><td align=\"right\" style=\"color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(diff))) + "</b></td></tr>";
                    }


                    /////////////////////////////////////////END////////////////////////////////////////////////////

                    if (ds.Tables[9].Rows.Count == 0)
                    {
                        strHtmlAllClient = String.Empty;

                    }


                    strHtmlAllClient += "</table>";

                    displayALLCLIENT.InnerHtml = strHtmlAllClient;
                    Session["htmlTable"] = strHtmlAllClient;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "norecord2", "NoRecord();", true);
                }
            }

        }

        protected void Button3_Click(object sender, EventArgs e)
        {

            //string cllist = SubClients.ToString();
            //procedure();

            //if (Request.QueryString["type"].ToString() == "0")
            //{
            //    htmltableclientwise();
            //}
            //else
            //{
            //    htmltablebranchwise();
            //}

            //DataTable dtCL = (DataTable)Session["DataTable_Email"];
            //for (int i = 0; i < dtCL.Rows.Count; i++)
            //{
            //     string emailbdy =  Session["htmlTable"].ToString();
            //     string contactid =dtCL.Rows[i]["ID"].ToString();
            //     string billdate = Request.QueryString["dt"].ToString();
            //     string Subject = "Settlement Trial For " + billdate;
            //     if (oDbEngine.SendReport(emailbdy, contactid, billdate, Subject) == true)
            //     {

            //         this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript6", "<script>MailsendT();</script>");
            //         //Session["htmlTable"]="";
            //     }
            //     else
            //     {
            //         if (dtCL.Rows.Count <= 1)
            //         {
            //             this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript9", "<script>MailsendF();</script>");
            //         }
            //        //Session["htmlTable"]="";
            //     }


            //}


        }
        protected void Button2_Click(object sender, EventArgs e)
        {

            Session["DataTable_Email"] = new DataTable();
            this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript9", "<script>parent.editwin.close();;</script>");
        }


        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string str = "";
            string str1 = "";
            if (idlist[0] == "ComboChange")
            {
                //MainAcID = idlist[1];
            }
            else
            {
                string[] cl = idlist[1].Split(',');
                for (int i = 0; i < cl.Length; i++)
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

                    SubClients = str;
                    //                data = "Clients~" + str1;

                    //if (idlist[0] == "Clients")
                    //{
                    //    SubClients = str;
                    //    data = "Clients~" + str1;
                    //}

                }
            }
        }


        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
    }
}