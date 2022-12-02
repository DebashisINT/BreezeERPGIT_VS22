using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
namespace ERP.OMS.Management.DailyTask
{
    public partial class management_DailyTask_OpeningPortFolio : ERP.OMS.ViewState_class.VSPage, System.Web.UI.ICallbackEventHandler
    {
        DataSet ds = new DataSet();
       // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        string data;
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DailyTaskOther oDailyTaskOther = new BusinessLogicLayer.DailyTaskOther();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                dtValuationDate.EditFormatString = oconverter.GetDateFormat("Date");
                dtValuationDate.Value = Convert.ToDateTime(DateTime.Today.ToShortDateString());
            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//

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

            if (idlist[0] == "Clients")
            {
                data = "Clients~" + str;
            }

        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;

        }
        protected void btnshow_Click(object sender, EventArgs e)
        {
            if (ddlType.SelectedItem.Value.ToString().Trim() == "1")
            {
                if (txtClient_hidden.Text.ToString().Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "FnGenerate", "FnGenerate('2');", true);

                }
                else
                {
                    FnSegment();
                }
            }
            else
            {
                if (HiddenField_Client.Value.ToString().Trim() == "" && rdbClientSelected.Checked)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "FnGenerate", "alert('Please Select Atleast One Client.');", true);

                }
                else
                {
                    FnSegment();
                }
            }

        }
        void FnSegment()
        {
            DataTable DtSeg = new DataTable();
            string Segment = "";
            DtSeg = oDBEngine.GetDataTable("tbl_master_companyexchange", "case when exch_exchid='EXN0000002' and exch_segmentid='CM' then 'NSE-CM' WHEN exch_exchid='EXB0000001' and exch_segmentid='CM' THEN 'BSE-CM' ELSE NULL END,EXCH_INTERNALID", "exch_segmentid like 'CM%' and exch_compid='" + Session["LastCompany"].ToString() + "'", null);
            if (DtSeg.Rows.Count > 0)
            {
                for (int i = 0; i < DtSeg.Rows.Count; i++)
                {
                    if (Segment.ToString().Trim() == "")
                        Segment = DtSeg.Rows[i][1].ToString();
                    else
                        Segment += "," + DtSeg.Rows[i][1].ToString();
                }
                FnClient(Segment);
            }
            else
            {
                Segment = Session["usersegid"].ToString();
                FnClient(Segment);
            }
        }
        void FnClient(string Segment)
        {
            DataTable DtClient = new DataTable();
            string Client = "";

            if (ddlType.SelectedItem.Value.ToString().Trim() == "1")
            {
                DtClient = oDBEngine.GetDataTable("tbl_master_contact", "DISTINCT cnt_internalID", " cnt_internalId like 'CL%' and isnull(cnt_clienttype,'R') like 'Pro%'");
                if (DtClient.Rows.Count > 0)
                {
                    for (int i = 0; i < DtClient.Rows.Count; i++)
                    {
                        if (Client.ToString().Trim() == "")
                            Client = "'" + DtClient.Rows[i][0].ToString() + "'";
                        else
                            Client += "," + "'" + DtClient.Rows[i][0].ToString() + "'";
                    }
                    Procedure(Segment, Client);

                }
            }
            if (ddlType.SelectedItem.Value.ToString().Trim() == "2")
            {
                if (rdbClientALL.Checked)
                {
                    Procedure(Segment, Client);
                }
                else
                {
                    Procedure(Segment, HiddenField_Client.Value.ToString().Trim());
                }
            }

        }
        void Procedure(string Segment, string Client)
        {
            string ClientType = string.Empty;
            string PostClient = string.Empty;
            string CloseMethod = string.Empty;
            string CreatUser = string.Empty;
            CreatUser = (chkConsBillDate.Checked) ? HttpContext.Current.Session["userid"].ToString().Trim() + "~CALLBILL" : HttpContext.Current.Session["userid"].ToString().Trim();
            if (ddlType.SelectedItem.Value.ToString().Trim() == "1")
            {
                ClientType = "cnt_clienttype='Pro Trading'";
                PostClient = txtClient_hidden.Text.ToString().Trim();
                CloseMethod = ddlclosmethod.SelectedItem.Value.ToString().Trim();
            }
            else
            {
                ClientType = "(cnt_clienttype not in ('Pro Investment','Pro Trading') or cnt_clienttype is null)";
                PostClient = (rdbClientALL.Checked) ? "ALLClient" : "SelectedClient";
                CloseMethod = "0";
            }
            ds = oDailyTaskOther.YearEndOpeningStocks(
                Convert.ToString(Session["LastCompany"]),
                Segment.ToString().Trim(),
               HttpContext.Current.Session["LastFinYear"].ToString().Trim(),
                 Convert.ToString(ClientType),
                 Convert.ToString(PostClient),
                 Convert.ToString(CloseMethod),
                 Convert.ToString(Client.ToString().Trim()),
                  Convert.ToString(Session["usersegid"]),
                 Convert.ToString(CreatUser),
                 Convert.ToString(dtValuationDate.Value));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString()) > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "FnGenerate", "alert('Carry Forward of Portfolio Stock Completed')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "FnGenerate", "alert('Carry Forward Of Portfolio Stock Encountered Some Problem...Call Influx Support Desk')", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "FnGenerate", "alert('There is an Error Ocurred/Please Try Again.')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "FnGenerate", "alert('There is an Error Ocurred/Please Try Again.')", true);
            }


        }

    }

}