using System;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using BusinessLogicLayer;


namespace ERP.OMS.Management
{
    public partial class management_Contact_SpeakWriteLanguage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindLanguage();
                LanGuage();


            }
            //TabPage TABpage = ASPxPageControl1.TabPages.FindByName("Documents");
            //TABpage.Enabled = false;
            //BindLanguage();
            //LanGuage();
        }
        public void BindLanguage()
        {
            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
            DataSet ds = new DataSet();
            ds = objEngine.PopulateData("*", "tbl_master_language", null);
            LanguageGrid.DataSource = ds;
            LanguageGrid.DataBind();
            WriteGrid.DataSource = ds;
            WriteGrid.DataBind();
        }
        protected void BtnLanguage_Click(object sender, EventArgs e)
        {
            lblmessage.Text = "";
            ListLanguage.Items.Clear();
            foreach (GridViewRow row in LanguageGrid.Rows)
            {
                string lngId = (string)LanguageGrid.DataKeys[row.RowIndex].Value.ToString();
                CheckBox GridChk = (CheckBox)row.FindControl("ChkLanguage");
                Label Language = (Label)row.FindControl("LblLanguage");
                if (GridChk.Checked == true)
                {
                    ListLanguage.Items.Add(Language.Text);
                }
            }
            LanguagePanel.Visible = false;
            LinkButton1.Visible = true;
        }
        protected void BtnList_Click(object sender, EventArgs e)
        {
            //BusinessLogicLayer.DBEngine objEngine1 = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine objEngine1 = new BusinessLogicLayer.DBEngine();
            string InternalId = HttpContext.Current.Session["KeyVal_InternalID"].ToString();//"EMA0000003";
            string Id = "";
            StringBuilder addItem = new StringBuilder();
            foreach (ListItem item in ListLanguage.Items)
            {
                //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
                lblmessage.Text = "";
                string[,] ListlngId = objEngine.GetFieldValue("tbl_master_language", "lng_id", "lng_language= '" + item + "'", 1);
                Id = ListlngId[0, 0];
                string add = Id + ",";

                addItem.Append(add);
            }
            string FItem = addItem.ToString();
            if (FItem != "")
            {
                int Fiitem = Convert.ToInt32(FItem.LastIndexOf(","));
                string Finalitem = FItem.Substring(0, Fiitem);
                objEngine1.SetFieldValue("tbl_master_contact", "cnt_speakLanguage='" + Finalitem + "'", " cnt_internalId='" + InternalId + "'");
            }
            else
            {
                lblmessage.Text = "Please Select language then save!";
            }


        }
        public void LanGuage()
        {
            string InternalId = HttpContext.Current.Session["KeyVal_InternalID"].ToString();//"EMA0000003";
            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
            //BusinessLogicLayer.DBEngine objEngine1 = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
            BusinessLogicLayer.DBEngine objEngine1 = new BusinessLogicLayer.DBEngine();
            string[,] ListlngId = objEngine.GetFieldValue("tbl_master_contact", "cnt_speakLanguage,cnt_writeLanguage", "cnt_internalId='" + InternalId + "'", 2);
            string speak = ListlngId[0, 0];
            string write = ListlngId[0, 1];
            if (speak != "")
            {
                string[] st = speak.Split(',');
                for (int i = 0; i <= st.GetUpperBound(0); i++)
                {
                    string[,] ListlngId1 = objEngine.GetFieldValue("tbl_master_language", "lng_language", "lng_id= '" + st[i] + "'", 1);
                    string Id = ListlngId1[0, 0];
                    ListLanguage.Items.Add(Id);
                }
            }
            if (write != "")
            {
                string[] wrte = write.Split(',');
                for (int i = 0; i <= wrte.GetUpperBound(0); i++)
                {
                    string[,] ListlngId1 = objEngine1.GetFieldValue("tbl_master_language", "lng_language", "lng_id= '" + wrte[i] + "'", 1);
                    string Id = ListlngId1[0, 0];
                    ListWrite.Items.Add(Id);
                }
            }

        }
        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            lblmessage.Text = "";
            ListWrite.Items.Clear();
            foreach (GridViewRow row in WriteGrid.Rows)
            {
                string lngId = (string)WriteGrid.DataKeys[row.RowIndex].Value.ToString();
                CheckBox GridChk = (CheckBox)row.FindControl("ChkWrite");
                Label Language = (Label)row.FindControl("LblWrite");
                if (GridChk.Checked == true)
                {
                    ListWrite.Items.Add(Language.Text);
                }
            }
            WritePanel.Visible = false;
            LinkButton2.Visible = true;
        }
        protected void BtnWriteSave_Click(object sender, EventArgs e)
        {
            //BusinessLogicLayer.DBEngine objEngine1 = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine objEngine1 = new BusinessLogicLayer.DBEngine();
            string InternalId = HttpContext.Current.Session["KeyVal_InternalID"].ToString();//"EMA0000003";
            string Id = "";
            StringBuilder addItem = new StringBuilder();
            foreach (ListItem item in ListWrite.Items)
            {
                //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
                lblmessage.Text = "";
                string[,] ListlngId = objEngine.GetFieldValue("tbl_master_language", "lng_id", "lng_language= '" + item + "'", 1);
                Id = ListlngId[0, 0];
                string add = Id + ",";

                addItem.Append(add);
            }
            string FItem = addItem.ToString();
            if (FItem != "")
            {
                int Fiitem = Convert.ToInt32(FItem.LastIndexOf(","));
                string Finalitem = FItem.Substring(0, Fiitem);
                objEngine1.SetFieldValue("tbl_master_contact", "cnt_writeLanguage='" + Finalitem + "'", " cnt_internalId='" + InternalId + "'");
            }
            else
            {
                lblmessage.Text = "Please Select language then save!";
            }
        }
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            lblmessage.Text = "";
            LanguagePanel.Visible = true;
            LinkButton1.Visible = false;
        }
        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            lblmessage.Text = "";
            WritePanel.Visible = true;
            LinkButton2.Visible = false;
        }
    }
}