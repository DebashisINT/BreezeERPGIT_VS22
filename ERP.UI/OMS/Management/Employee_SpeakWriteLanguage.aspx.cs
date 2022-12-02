using System;
using System.Web;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;


namespace ERP.OMS.Management
{
    public partial class management_Employee_SpeakWriteLanguage : System.Web.UI.Page
    {
        //DBEngine objEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        DBEngine objEngine = new DBEngine();
        public string WLanguage = "";
        public string SpLanguage = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LanGuage();
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>page_load();</script>");
            }
        }
        public void LanGuage()
        {
            string InternalId = HttpContext.Current.Session["KeyVal_InternalID"].ToString();//"EMA0000003";        
            string[,] ListlngId = objEngine.GetFieldValue("tbl_master_contact", "cnt_speakLanguage,cnt_writeLanguage", "cnt_internalId='" + InternalId + "'", 2);
            string speak = ListlngId[0, 0];
            SpLanguage = speak;
            string write = ListlngId[0, 1];
            WLanguage = write;
            if (speak != "")
            {
                string spk = "";
                string[] st = speak.Split(',');
                for (int i = 0; i <= st.GetUpperBound(0); i++)
                {
                    string[,] ListlngId1 = objEngine.GetFieldValue("tbl_master_language", "lng_language", "lng_id= '" + st[i] + "'", 1);
                    string Id = ListlngId1[0, 0];
                    spk += Id + ", ";
                }
                int spklng = spk.LastIndexOf(',');
                spk = spk.Substring(0, spklng);
                LitSpokenLanguage.Text = spk;
            }
            if (write != "")
            {
                string wrt = "";
                string[] wrte = write.Split(',');
                for (int i = 0; i <= wrte.GetUpperBound(0); i++)
                {
                    string[,] ListlngId1 = objEngine.GetFieldValue("tbl_master_language", "lng_language", "lng_id= '" + wrte[i] + "'", 1);
                    string Id = ListlngId1[0, 0];
                    wrt += Id + ",";
                }
                int wrtlng = wrt.LastIndexOf(',');
                wrt = wrt.Substring(0, wrtlng);
                LitWrittenLanguage.Text = wrt;
            }

        }
        protected void BtnSaveAllLang_Click(object sender, EventArgs e)
        {
            string InternalId = HttpContext.Current.Session["KeyVal_InternalID"].ToString();//"EMA0000003";
            objEngine.SetFieldValue("tbl_master_contact", "cnt_speakLanguage='" + txtSpeakLanguage.Value + "',cnt_writeLanguage='" + txtWriteLanguage.Value + "'", " cnt_internalId='" + InternalId + "'");
            string popUpscript = "";
            popUpscript = "<script language='javascript'>";
            popUpscript += "document.getElementById('TdSpk').style.display = 'none';document.getElementById('TdWrt').style.display = 'none';document.getElementById('TdAll').style.display = 'none';</script>";
            ClientScript.RegisterStartupScript(GetType(), "JScript", popUpscript);
            LanGuage();
        }
    }
}