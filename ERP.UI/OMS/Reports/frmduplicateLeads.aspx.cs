using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Xml;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmduplicateLeads : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        string data = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        public string pageAccess = "";
        //  DBEngine oDBEngine = new DBEngine(string.Empty);
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
               
            }

            if (!IsPostBack)
            {

                //________This script is for firing javascript when page load first___//
                if (!ClientScript.IsStartupScriptRegistered("Today"))
                    ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>AtTheTimePageLoad();</script>");
                //______________________________End Script____________________________//
            }
            gridbind();
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//
        }

        private void gridbind()
        {
            DataTable DT = oDBEngine.GetDataTable(" tbl_master_phonefax a, tbl_master_phonefax b, tbl_master_lead k ", " DISTINCT a.phf_cntId as id ,k.cnt_firstName as name, a.phf_phoneNumber as phone, case Left(k.cnt_Status,2) when 'PC' Then 'Active' Else 'Due' End  as status ", " k.cnt_internalId = a.phf_cntId and a.phf_phoneNumber = b.phf_phoneNumber AND a.phf_cntId <> b.phf_cntId and a.phf_entity ='Lead'and    a.phf_phoneNumber <> ' ' AND k.cnt_Status = 'Due' ", " a.phf_phoneNumber ");

            GridDuplicateLead.DataSource = DT.DefaultView;
            GridDuplicateLead.DataBind();
        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }

        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {

            string id = eventArgument.ToString();
            string[] FieldWvalue = id.Split('~');
            data = "";
            string Id = FieldWvalue[1];
            string[] FieldID = Id.Split(',');
            #region City name to combo city
            if (FieldWvalue[0] == "Delete")
            {
                for (int i = 0; i < FieldID.Length; i++)
                {
                    DataTable dt_temp = oDBEngine.GetDataTable("tbl_master_lead,tbl_master_phonefax,tbl_master_address,tbl_master_email", "*", " cnt_internalId = '" + FieldID[i] + "' and eml_cntId ='" + FieldID[i] + "' and add_cntid ='" + FieldID[i] + "' and  phf_cntId='" + FieldID[i] + "'");
                    DataSet xmlDS = new DataSet();
                    DataTable dtTemp = oDBEngine.GetDataTable("tbl_master_lead INNER JOIN  tbl_master_address ON tbl_master_lead.cnt_internalId = tbl_master_address.add_cntId INNER JOIN tbl_master_email ON tbl_master_address.add_cntId = tbl_master_email.eml_cntId INNER JOIN   tbl_master_phonefax ON tbl_master_email.eml_cntId = tbl_master_phonefax.phf_cntId", "*", " tbl_master_lead.cnt_internalId = '" + FieldID[i] + "'");
                    xmlDS.Tables.Add(dtTemp);
                    if (Directory.Exists(Server.MapPath("..\\DeletedLead").Trim()))
                    {
                        XmlDocument xml_sender = new XmlDocument();
                        xml_sender.Load(Server.MapPath("..\\DeletedLead\\Lead.xml").Trim());
                        XmlElement rootNode = (XmlElement)xml_sender.SelectSingleNode("//DeleteLead");
                        for (int k = 0; k < xmlDS.Tables[0].Rows.Count; k++)
                        {
                            XmlElement parentNode;
                            parentNode = xml_sender.CreateElement("Leads");
                            rootNode.AppendChild(parentNode);
                            for (int j = 0; j < xmlDS.Tables[0].Columns.Count; j++)
                            {
                                if (dt_temp.Rows[k][j].ToString().Trim() != "")
                                {
                                    XmlElement parentNode1;
                                    parentNode1 = xml_sender.CreateElement(xmlDS.Tables[0].Columns[j].Caption.ToString().Trim());
                                    parentNode1.InnerText = dt_temp.Rows[k][j].ToString();
                                    parentNode.AppendChild(parentNode1);
                                }
                            }
                        }
                        xml_sender.Save(Server.MapPath("..\\DeletedLead\\Lead.xml").Trim());

                    }
                    else
                    {
                        Directory.CreateDirectory(Server.MapPath("..\\DeletedLead"));
                        XmlDocument xmlDoc = new XmlDocument();
                        XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes");
                        XmlElement rootNode = xmlDoc.CreateElement("DeleteLead");
                        xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement);
                        xmlDoc.AppendChild(rootNode);
                        for (int k = 0; k < xmlDS.Tables[0].Rows.Count; k++)
                        {
                            XmlElement parentNode;
                            parentNode = xmlDoc.CreateElement("Leads");
                            rootNode.AppendChild(parentNode);
                            for (int j = 0; j < xmlDS.Tables[0].Columns.Count; j++)
                            {
                                if (dt_temp.Rows[k][j].ToString().Trim() != "")
                                {
                                    XmlElement parentNode1;
                                    parentNode1 = xmlDoc.CreateElement(xmlDS.Tables[0].Columns[j].Caption.ToString().Trim());
                                    parentNode1.InnerText = dt_temp.Rows[k][j].ToString().Trim();
                                    parentNode.AppendChild(parentNode1);
                                }
                            }
                        }
                        xmlDoc.Save(Server.MapPath("..\\DeletedLead\\Lead.xml").Trim());
                    }
                    oDBEngine.DeleteValue("tbl_master_lead", " cnt_internalId='" + FieldID[i] + "'");
                    oDBEngine.DeleteValue("tbl_master_address", " add_cntId='" + FieldID[i] + "'");
                    oDBEngine.DeleteValue("tbl_master_phonefax", " phf_cntId='" + FieldID[i] + "'");
                    oDBEngine.DeleteValue("tbl_master_email", " eml_cntId='" + FieldID[i] + "'");
                    oDBEngine.DeleteValue("tbl_master_contactRegistration", " crg_cntId='" + FieldID[i] + "'");
                    oDBEngine.DeleteValue("tbl_trans_contactBankDetails", " cbd_cntId='" + FieldID[i] + "'");
                    oDBEngine.DeleteValue("tbl_trans_group", " grp_contactId='" + FieldID[i] + "'");
                    oDBEngine.DeleteValue("tbl_master_dPDetails", " dpd_cntId='" + FieldID[i] + "'");
                    oDBEngine.DeleteValue("tbl_trans_salesVisit", " slv_leadcotactId='" + FieldID[i] + "'");
                    oDBEngine.DeleteValue("tbl_trans_Sales", " sls_contactlead_id='" + FieldID[i] + "'");
                    oDBEngine.DeleteValue("tbl_trans_salesDetails", " sad_cntId='" + FieldID[i] + "'");
                    oDBEngine.DeleteValue("tbl_master_document", " doc_contactId='" + FieldID[i] + "'");
                }
                data = "Delete~Y";
            }
            #endregion
            #region Area name to combo area
            if (FieldWvalue[0] == "Area")
            {
            }

            #endregion



        }
        protected void GridDuplicateLead_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            gridbind();
        }
    }
}