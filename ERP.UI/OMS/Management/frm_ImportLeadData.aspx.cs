using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Management
{
    public partial class management_frm_ImportLeadData : System.Web.UI.Page
    {
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        DBEngine oDBEngine = new DBEngine();
        DataTable dt = new DataTable();
        clsDropDownList cls = new clsDropDownList();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDropDown();
                BindState();
                BindCity();
                string strConn;
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                "Data Source='" + Request.QueryString["filestr"].ToString() + "';" +
                "Extended Properties=Excel 8.0;";
                //You must use the $ after the object you reference in the spreadsheet
                OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", strConn);
                DataSet myDataSet = new DataSet();
                myCommand.Fill(myDataSet, "ExcelInfo");
                DataColumn colId = new DataColumn("Id");
                DataColumn colName = new DataColumn("Name");
                dt.Columns.Add(colId);
                dt.Columns.Add(colName);
                DataRow row = dt.NewRow();
                for (int j = 0; j < myDataSet.Tables["ExcelInfo"].Columns.Count; j++)
                {
                    DataRow row1 = dt.NewRow();
                    row1[0] = j + 1;
                    row1[1] = myDataSet.Tables["ExcelInfo"].Columns[j].ColumnName;
                    dt.Rows.Add(row1);
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    DDLFirstName.DataValueField = "Id";
                    DDLFirstName.DataTextField = "Name";
                    DDLFirstName.DataSource = dt;
                    DDLFirstName.DataBind();
                    DDLFirstName.Items.Insert(0, "Select");
                    DDLMiddleName.DataValueField = "Id";
                    DDLMiddleName.DataTextField = "Name";
                    DDLMiddleName.DataSource = dt;
                    DDLMiddleName.DataBind();
                    DDLMiddleName.Items.Insert(0, "Select");
                    DDLLastName.DataValueField = "Id";
                    DDLLastName.DataTextField = "Name";
                    DDLLastName.DataSource = dt;
                    DDLLastName.DataBind();
                    DDLLastName.Items.Insert(0, "Select");
                    DDLShortName.DataValueField = "Id";
                    DDLShortName.DataTextField = "Name";
                    DDLShortName.DataSource = dt;
                    DDLShortName.DataBind();
                    DDLShortName.Items.Insert(0, "Select");
                    DDLDob.DataValueField = "Id";
                    DDLDob.DataTextField = "Name";
                    DDLDob.DataSource = dt;
                    DDLDob.DataBind();
                    DDLDob.Items.Insert(0, "Select");
                    DDLResidenceAdd1.DataValueField = "Id";
                    DDLResidenceAdd1.DataTextField = "Name";
                    DDLResidenceAdd1.DataSource = dt;
                    DDLResidenceAdd1.DataBind();
                    DDLResidenceAdd1.Items.Insert(0, "Select");
                    DDLResidenceAdd2.DataValueField = "Id";
                    DDLResidenceAdd2.DataTextField = "Name";
                    DDLResidenceAdd2.DataSource = dt;
                    DDLResidenceAdd2.DataBind();
                    DDLResidenceAdd2.Items.Insert(0, "Select");
                    DDLResidenceAdd3.DataValueField = "Id";
                    DDLResidenceAdd3.DataTextField = "Name";
                    DDLResidenceAdd3.DataSource = dt;
                    DDLResidenceAdd3.DataBind();
                    DDLResidenceAdd3.Items.Insert(0, "Select");
                    DDLResidenceLandmark.DataValueField = "Id";
                    DDLResidenceLandmark.DataTextField = "Name";
                    DDLResidenceLandmark.DataSource = dt;
                    DDLResidenceLandmark.DataBind();
                    DDLResidenceLandmark.Items.Insert(0, "Select");
                    DDLResidencePin.DataValueField = "Id";
                    DDLResidencePin.DataTextField = "Name";
                    DDLResidencePin.DataSource = dt;
                    DDLResidencePin.DataBind();
                    DDLResidencePin.Items.Insert(0, "Select");
                    DDLOfficeAddress1.DataValueField = "Id";
                    DDLOfficeAddress1.DataTextField = "Name";
                    DDLOfficeAddress1.DataSource = dt;
                    DDLOfficeAddress1.DataBind();
                    DDLOfficeAddress1.Items.Insert(0, "Select");
                    DDLOfficeAddress2.DataValueField = "Id";
                    DDLOfficeAddress2.DataTextField = "Name";
                    DDLOfficeAddress2.DataSource = dt;
                    DDLOfficeAddress2.DataBind();
                    DDLOfficeAddress2.Items.Insert(0, "Select");
                    DDLOfficeAddress3.DataValueField = "Id";
                    DDLOfficeAddress3.DataTextField = "Name";
                    DDLOfficeAddress3.DataSource = dt;
                    DDLOfficeAddress3.DataBind();
                    DDLOfficeAddress3.Items.Insert(0, "Select");
                    DDLOfficeLandmark.DataValueField = "Id";
                    DDLOfficeLandmark.DataTextField = "Name";
                    DDLOfficeLandmark.DataSource = dt;
                    DDLOfficeLandmark.DataBind();
                    DDLOfficeLandmark.Items.Insert(0, "Select");
                    DDLOfficePin.DataValueField = "Id";
                    DDLOfficePin.DataTextField = "Name";
                    DDLOfficePin.DataSource = dt;
                    DDLOfficePin.DataBind();
                    DDLOfficePin.Items.Insert(0, "Select");
                    DDLResidencePhoneNo.DataValueField = "Id";
                    DDLResidencePhoneNo.DataTextField = "Name";
                    DDLResidencePhoneNo.DataSource = dt;
                    DDLResidencePhoneNo.DataBind();
                    DDLResidencePhoneNo.Items.Insert(0, "Select");
                    DDLResidenceFaxNo.DataValueField = "Id";
                    DDLResidenceFaxNo.DataTextField = "Name";
                    DDLResidenceFaxNo.DataSource = dt;
                    DDLResidenceFaxNo.DataBind();
                    DDLResidenceFaxNo.Items.Insert(0, "Select");
                    DDLOfficePhoneNo.DataValueField = "Id";
                    DDLOfficePhoneNo.DataTextField = "Name";
                    DDLOfficePhoneNo.DataSource = dt;
                    DDLOfficePhoneNo.DataBind();
                    DDLOfficePhoneNo.Items.Insert(0, "Select");
                    DDLOfficeFaxNo.DataValueField = "Id";
                    DDLOfficeFaxNo.DataTextField = "Name";
                    DDLOfficeFaxNo.DataSource = dt;
                    DDLOfficeFaxNo.DataBind();
                    DDLOfficeFaxNo.Items.Insert(0, "Select");
                    DDLMobileNo.DataValueField = "Id";
                    DDLMobileNo.DataTextField = "Name";
                    DDLMobileNo.DataSource = dt;
                    DDLMobileNo.DataBind();
                    DDLMobileNo.Items.Insert(0, "Select");
                    DDLEmailAddress.DataValueField = "Id";
                    DDLEmailAddress.DataTextField = "Name";
                    DDLEmailAddress.DataSource = dt;
                    DDLEmailAddress.DataBind();
                    DDLEmailAddress.Items.Insert(0, "Select");
                    DDLCCEmail.DataValueField = "Id";
                    DDLCCEmail.DataTextField = "Name";
                    DDLCCEmail.DataSource = dt;
                    DDLCCEmail.DataBind();
                    DDLCCEmail.Items.Insert(0, "Select");

                }
            }
            txtReferedBy.Attributes.Add("onkeyup", "CallList(this,'referedby',event)");
        }
        public void BindDropDown()
        {
            string[,] Country = oDBEngine.GetFieldValue("tbl_master_country", "cou_id,cou_country", null, 2, "cou_country");
            if (Country[0, 0] != "n")
            {
                cls.AddDataToDropDownList(Country, DDLCountry);
            }

            string[,] contactSource = oDBEngine.GetFieldValue("tbl_master_contactSource", "cntsrc_id,cntsrc_sourceType", null, 2, "cntsrc_sourceType");
            if (contactSource[0, 0] != "n")
            {
                cls.AddDataToDropDownList(contactSource, DDLContactSource);
            }

        }
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            int Fname = 0;
            int MiddleName = 0;
            int LastName = 0;
            int Shortname = 0;
            int DOB = 0;
            int ResidenceAdd1 = 0;
            int ResidenceAdd2 = 0;
            int ResidenceAdd3 = 0;
            int ResidenceLandmark = 0;
            int ResidenceCity = 0;
            int ResidencePin = 0;
            int OfficeAddress1 = 0;
            int OfficeAddress2 = 0;
            int OfficeAddress3 = 0;
            int OfficeLandmark = 0;
            int OfficeCity = 0;
            int OfficePin = 0;
            int ResidencePhoneNo = 0;
            int ResidenceFaxNo = 0;
            int OfficePhoneNo = 0;
            int OfficeFaxNo = 0;
            int MobileNo = 0;
            int EmailAddress = 0;
            int CCEmail = 0;

            string Fname1 = "";
            string MiddleName1 = "";
            string LastName1 = "";
            string Shortname1 = "";
            string DOB1 = "";
            string ResidenceAdd11 = "";
            string ResidenceAdd21 = "";
            string ResidenceAdd31 = "";
            string ResidenceLandmark1 = "";
            string ResidenceCity1 = "";
            string ResidencePin1 = "";
            string OfficeAddress11 = "";
            string OfficeAddress21 = "";
            string OfficeAddress31 = "";
            string OfficeLandmark1 = "";
            string OfficeCity1 = "";
            string OfficePin1 = "";
            string ResidencePhoneNo1 = "";
            string ResidenceFaxNo1 = "";
            string OfficePhoneNo1 = "";
            string OfficeFaxNo1 = "";
            string MobileNo1 = "";
            string EmailAddress1 = "";
            string CCEmail1 = "";

            if (DDLFirstName.SelectedValue != "Select")
            {
                Fname = Convert.ToInt32(Convert.ToString(DDLFirstName.SelectedValue) == "" ? "0" : Convert.ToString(DDLFirstName.SelectedValue));
            }
            if (DDLMiddleName.SelectedValue != "Select")
            {
                MiddleName = Convert.ToInt32(Convert.ToString(DDLMiddleName.SelectedValue) == "" ? "0" : Convert.ToString(DDLMiddleName.SelectedValue));
            }
            if (DDLLastName.SelectedValue != "Select")
            {
                LastName = Convert.ToInt32(Convert.ToString(DDLLastName.SelectedValue) == "" ? "0" : Convert.ToString(DDLLastName.SelectedValue));
            }
            if (DDLShortName.SelectedValue != "Select")
            {
                Shortname = Convert.ToInt32(Convert.ToString(DDLShortName.SelectedValue) == "" ? "0" : Convert.ToString(DDLShortName.SelectedValue));
            }
            if (DDLDob.SelectedValue != "Select")
            {
                DOB = Convert.ToInt32(Convert.ToString(DDLDob.SelectedValue) == "" ? "0" : Convert.ToString(DDLDob.SelectedValue));
            }
            if (DDLResidenceAdd1.SelectedValue != "Select")
            {
                ResidenceAdd1 = Convert.ToInt32(Convert.ToString(DDLResidenceAdd1.SelectedValue) == "" ? "0" : Convert.ToString(DDLResidenceAdd1.SelectedValue));
            }
            if (DDLResidenceAdd2.SelectedValue != "Select")
            {
                ResidenceAdd2 = Convert.ToInt32(Convert.ToString(DDLResidenceAdd2.SelectedValue) == "" ? "0" : Convert.ToString(DDLResidenceAdd2.SelectedValue));
            }
            if (DDLResidenceAdd3.SelectedValue != "Select")
            {
                ResidenceAdd3 = Convert.ToInt32(Convert.ToString(DDLResidenceAdd3.SelectedValue) == "" ? "0" : Convert.ToString(DDLResidenceAdd3.SelectedValue));
            }
            if (DDLResidenceLandmark.SelectedValue != "Select")
            {
                ResidenceLandmark = Convert.ToInt32(Convert.ToString(DDLResidenceLandmark.SelectedValue) == "" ? "0" : Convert.ToString(DDLResidenceLandmark.SelectedValue));
            }
            if (DDLResidenceCity.SelectedValue != "Select")
            {
                ResidenceCity = Convert.ToInt32(Convert.ToString(DDLResidenceCity.SelectedValue) == "" ? "0" : Convert.ToString(DDLResidenceCity.SelectedValue));
            }
            if (DDLResidencePin.SelectedValue != "Select")
            {
                ResidencePin = Convert.ToInt32(Convert.ToString(DDLResidencePin.SelectedValue) == "" ? "0" : Convert.ToString(DDLResidencePin.SelectedValue));
            }
            if (DDLOfficeAddress1.SelectedValue != "Select")
            {
                OfficeAddress1 = Convert.ToInt32(Convert.ToString(DDLOfficeAddress1.SelectedValue) == "" ? "0" : Convert.ToString(DDLOfficeAddress1.SelectedValue));
            }
            if (DDLOfficeAddress2.SelectedValue != "Select")
            {
                OfficeAddress2 = Convert.ToInt32(Convert.ToString(DDLOfficeAddress2.SelectedValue) == "" ? "0" : Convert.ToString(DDLOfficeAddress2.SelectedValue));
            }
            if (DDLOfficeAddress3.SelectedValue != "Select")
            {
                OfficeAddress3 = Convert.ToInt32(Convert.ToString(DDLOfficeAddress3.SelectedValue) == "" ? "0" : Convert.ToString(DDLOfficeAddress3.SelectedValue));
            }
            if (DDLOfficeLandmark.SelectedValue != "Select")
            {
                OfficeLandmark = Convert.ToInt32(Convert.ToString(DDLOfficeLandmark.SelectedValue) == "" ? "0" : Convert.ToString(DDLOfficeLandmark.SelectedValue));
            }
            if (DDLOfficePin.SelectedValue != "Select")
            {
                OfficePin = Convert.ToInt32(Convert.ToString(DDLOfficePin.SelectedValue) == "" ? "0" : Convert.ToString(DDLOfficePin.SelectedValue));
            }
            if (DDLResidencePhoneNo.SelectedValue != "Select")
            {
                ResidencePhoneNo = Convert.ToInt32(Convert.ToString(DDLResidencePhoneNo.SelectedValue) == "" ? "0" : Convert.ToString(DDLResidencePhoneNo.SelectedValue));
            }
            if (DDLResidenceFaxNo.SelectedValue != "Select")
            {
                ResidenceFaxNo = Convert.ToInt32(Convert.ToString(DDLResidenceFaxNo.SelectedValue) == "" ? "0" : Convert.ToString(DDLResidenceFaxNo.SelectedValue));
            }
            if (DDLOfficePhoneNo.SelectedValue != "Select")
            {
                OfficePhoneNo = Convert.ToInt32(Convert.ToString(DDLOfficePhoneNo.SelectedValue) == "" ? "0" : Convert.ToString(DDLOfficePhoneNo.SelectedValue));
            }
            if (DDLOfficeFaxNo.SelectedValue != "Select")
            {
                OfficeFaxNo = Convert.ToInt32(Convert.ToString(DDLOfficeFaxNo.SelectedValue) == "" ? "0" : Convert.ToString(DDLOfficeFaxNo.SelectedValue));
            }
            if (DDLMobileNo.SelectedValue != "Select")
            {
                MobileNo = Convert.ToInt32(Convert.ToString(DDLMobileNo.SelectedValue) == "" ? "0" : Convert.ToString(DDLMobileNo.SelectedValue));
            }
            if (DDLEmailAddress.SelectedValue != "Select")
            {
                EmailAddress = Convert.ToInt32(Convert.ToString(DDLEmailAddress.SelectedValue) == "" ? "0" : Convert.ToString(DDLEmailAddress.SelectedValue));
            }
            if (DDLCCEmail.SelectedValue != "Select")
            {
                CCEmail = Convert.ToInt32(Convert.ToString(DDLCCEmail.SelectedValue) == "" ? "0" : Convert.ToString(DDLCCEmail.SelectedValue));
            }
            string strConn;
            strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" +
            "Data Source='" + Request.QueryString["filestr"].ToString() + "';" +
            "Extended Properties=Excel 8.0;";
            //You must use the $ after the object you reference in the spreadsheet
            OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", strConn);
            DataSet dtSet = new DataSet();
            myCommand.Fill(dtSet);
            string str = "";
            string filestr = Request.QueryString["filestr"].ToString();
            string[] tempFileName = filestr.Split('\\');
            int length = tempFileName.Length - 1;
            string[] tempFile = tempFileName[length].Split('.');

            if (!System.IO.Directory.Exists(Server.MapPath("..\\ImportFiles\\")))
                System.IO.Directory.CreateDirectory(Server.MapPath("..\\ImportFiles\\"));

            StreamWriter F2 = new StreamWriter(Server.MapPath("..\\ImportFiles\\" + tempFile[0] + "_" + Session["userid"].ToString() + "_" + oDBEngine.GetDate().Day.ToString() + ".txt"), false);
            string temp123 = dtSet.Tables[0].Columns[Fname - 1].ColumnName;
            DataView dv = dtSet.Tables[0].DefaultView;
            try
            {
                for (int i = 0; i < dtSet.Tables[0].Rows.Count; i++)
                {
                    str = "";
                    if (Fname != 0)
                    {
                        str += dtSet.Tables[0].Rows[i][Fname - 1] + "!";
                        Fname1 = dtSet.Tables[0].Rows[i][Fname - 1].ToString();
                    }
                    else
                    {
                        str += "" + "!";
                    }
                    if (MiddleName != 0)
                    {
                        str += dtSet.Tables[0].Rows[i][MiddleName - 1] + "!";
                        MiddleName1 = dtSet.Tables[0].Rows[i][MiddleName - 1].ToString();
                    }
                    else
                    {
                        str += "" + "!";
                    }
                    if (LastName != 0)
                    {
                        str += dtSet.Tables[0].Rows[i][LastName - 1] + "!";
                        LastName1 = dtSet.Tables[0].Rows[i][LastName - 1].ToString();
                    }
                    else
                    {
                        str += "" + "!";
                    }
                    if (Shortname != 0)
                    {
                        str += dtSet.Tables[0].Rows[i][Shortname - 1] + "!";
                        Shortname1 = dtSet.Tables[0].Rows[i][Shortname - 1].ToString();
                    }
                    else
                    {
                        str += "" + "!";
                    }
                    str += txtReferedBy_hidden.Text + "!";
                    str += DDLContactSource.SelectedValue.ToString() + "!";
                    if (DOB != 0)
                    {
                        str += dtSet.Tables[0].Rows[i][DOB - 1] + "!";
                        DOB1 = dtSet.Tables[0].Rows[i][DOB - 1].ToString();
                    }
                    else
                    {
                        str += "01/01/1900" + "!";
                    }
                    if (ResidenceAdd1 != 0)
                    {
                        str += dtSet.Tables[0].Rows[i][ResidenceAdd1 - 1] + "!";
                        ResidenceAdd11 = dtSet.Tables[0].Rows[i][ResidenceAdd1 - 1].ToString();
                    }
                    else
                    {
                        str += "" + "!";
                    }
                    if (ResidenceAdd2 != 0)
                    {
                        str += dtSet.Tables[0].Rows[i][ResidenceAdd2 - 1] + "!";
                        ResidenceAdd21 = dtSet.Tables[0].Rows[i][ResidenceAdd2 - 1].ToString();
                    }
                    else
                    {
                        str += "" + "!";
                    }
                    if (ResidenceAdd3 != 0)
                    {
                        str += dtSet.Tables[0].Rows[i][ResidenceAdd3 - 1] + "!";
                        ResidenceAdd31 = dtSet.Tables[0].Rows[i][ResidenceAdd3 - 1].ToString();
                    }
                    else
                    {
                        str += "" + "!";
                    }
                    if (ResidenceLandmark != 0)
                    {
                        str += dtSet.Tables[0].Rows[i][ResidenceLandmark - 1] + "!";
                        ResidenceLandmark1 = dtSet.Tables[0].Rows[i][ResidenceLandmark - 1].ToString();
                    }
                    else
                    {
                        str += "" + "!";
                    }
                    str += DDLCountry.SelectedValue.ToString() + "!";
                    str += DDLState.SelectedValue.ToString() + "!";
                    if (ResidenceCity != 0)
                    {
                        str += dtSet.Tables[0].Rows[i][ResidenceCity - 1] + "!";
                        ResidenceCity1 = dtSet.Tables[0].Rows[i][ResidenceCity - 1].ToString();
                    }
                    else
                    {
                        str += "" + "!";
                    }
                    if (ResidencePin != 0)
                    {
                        str += dtSet.Tables[0].Rows[i][ResidencePin - 1] + "!";
                        ResidencePin1 = dtSet.Tables[0].Rows[i][ResidencePin - 1].ToString();
                    }
                    else
                    {
                        str += "" + "!";
                    }
                    if (OfficeAddress1 != 0)
                    {
                        str += dtSet.Tables[0].Rows[i][OfficeAddress1 - 1] + "!";
                        OfficeAddress11 = dtSet.Tables[0].Rows[i][OfficeAddress1 - 1].ToString();
                    }
                    else
                    {
                        str += "" + "!";
                    }
                    if (OfficeAddress2 != 0)
                    {
                        str += dtSet.Tables[0].Rows[i][OfficeAddress2 - 1] + "!";
                        OfficeAddress21 = dtSet.Tables[0].Rows[i][OfficeAddress2 - 1].ToString();
                    }
                    else
                    {
                        str += "" + "!";
                    }
                    if (OfficeAddress3 != 0)
                    {
                        str += dtSet.Tables[0].Rows[i][OfficeAddress3 - 1] + "!";
                        OfficeAddress31 = dtSet.Tables[0].Rows[i][OfficeAddress3 - 1].ToString();
                    }
                    else
                    {
                        str += "" + "!";
                    }
                    if (OfficeLandmark != 0)
                    {
                        str += dtSet.Tables[0].Rows[i][OfficeLandmark - 1] + "!";
                        OfficeLandmark1 = dtSet.Tables[0].Rows[i][OfficeLandmark - 1].ToString();
                    }
                    else
                    {
                        str += "" + "!";
                    }
                    str += DDLCountry.SelectedValue.ToString() + "!";
                    str += DDLState.SelectedValue.ToString() + "!";
                    if (OfficeCity != 0)
                    {
                        str += dtSet.Tables[0].Rows[i][OfficeCity - 1] + "!";
                        OfficeCity1 = dtSet.Tables[0].Rows[i][OfficeCity - 1].ToString();
                    }
                    else
                    {
                        str += "" + "!";
                    }
                    if (OfficePin != 0)
                    {
                        str += dtSet.Tables[0].Rows[i][OfficePin - 1] + "!";
                        OfficePin1 = dtSet.Tables[0].Rows[i][OfficePin - 1].ToString();
                    }
                    else
                    {
                        str += "" + "!";
                    }
                    if (ResidencePhoneNo != 0)
                    {
                        str += dtSet.Tables[0].Rows[i][ResidencePhoneNo - 1] + "!";
                        ResidencePhoneNo1 = dtSet.Tables[0].Rows[i][ResidencePhoneNo - 1].ToString();
                    }
                    else
                    {
                        str += "" + "!";
                        ResidencePhoneNo1 = "";
                    }
                    if (ResidenceFaxNo != 0)
                    {
                        str += dtSet.Tables[0].Rows[i][ResidenceFaxNo - 1] + "!";
                        ResidenceFaxNo1 = dtSet.Tables[0].Rows[i][ResidenceFaxNo - 1].ToString();
                    }
                    else
                    {
                        str += "" + "!";
                        ResidenceFaxNo1 = "";
                    }
                    if (OfficePhoneNo != 0)
                    {
                        str += dtSet.Tables[0].Rows[i][OfficePhoneNo - 1] + "!";
                        OfficePhoneNo1 = dtSet.Tables[0].Rows[i][OfficePhoneNo - 1].ToString();
                    }
                    else
                    {
                        str += "" + "!";
                    }
                    if (OfficeFaxNo != 0)
                    {
                        str += dtSet.Tables[0].Rows[i][OfficeFaxNo - 1] + "!";
                        OfficeFaxNo1 = dtSet.Tables[0].Rows[i][OfficeFaxNo - 1].ToString();
                    }
                    else
                    {
                        str += "" + "!";
                    }
                    if (MobileNo != 0)
                    {
                        str += dtSet.Tables[0].Rows[i][MobileNo - 1] + "!";
                        MobileNo1 = dtSet.Tables[0].Rows[i][MobileNo - 1].ToString();
                    }
                    else
                    {
                        str += "" + "!";
                    }
                    if (EmailAddress != 0)
                    {
                        str += dtSet.Tables[0].Rows[i][EmailAddress - 1] + "!";
                        EmailAddress1 = dtSet.Tables[0].Rows[i][EmailAddress - 1].ToString();
                    }
                    else
                    {
                        str += "" + "!";
                    }
                    if (CCEmail != 0)
                    {
                        str += dtSet.Tables[0].Rows[i][CCEmail - 1] + "!";
                        CCEmail1 = dtSet.Tables[0].Rows[i][CCEmail - 1].ToString();
                    }
                    else
                    {
                        str += "" + "!";
                    }
                    str += HttpContext.Current.Session["userbranchID"].ToString() + "!";
                    str += Session["userid"].ToString() + "!";
                    str += oDBEngine.GetDate();
                    F2.WriteLine(str);
                    string FirstLetter = Fname1.Substring(0, 1).ToUpper();
                    string Prefix = "LD" + FirstLetter;
                    string InternalId = oDBEngine.GetInternalId(Prefix, "tbl_master_lead", "cnt_internalId", " cnt_internalId");
                    string FieldValue = "'" + InternalId + "','" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + Fname1 + "','" + MiddleName1 + "','" + LastName1 + "','" + Shortname1 + "','" + DDLContactSource.SelectedValue.ToString() + "','" + txtReferedBy_hidden.Text + "','" + DOB1 + "','due','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "','LD'";
                    string FieldName = "cnt_internalId,cnt_branchid,cnt_firstName,cnt_middleName,cnt_lastName,cnt_shortName,cnt_contactSource,cnt_referedBy,cnt_dOB,cnt_Status,CreateDate,CreateUser,cnt_contactType";
                    int NoOfRowAffected = oDBEngine.InsurtFieldValue("tbl_master_lead", FieldName, FieldValue);
                    string RFieldName = "add_cntId,add_entity,add_addressType,add_address1,add_address2,add_address3,add_landmark,add_country,add_state,add_city,add_pin,CreateDate,CreateUser";
                    string RFieldValue = "'" + InternalId + "','Lead','Residence','" + ResidenceAdd11 + "','" + ResidenceAdd21 + "','" + ResidenceAdd31 + "','" + ResidenceLandmark1 + "','" + DDLCountry.SelectedValue.ToString() + "','" + DDLState.SelectedValue.ToString() + "','" + DDLResidenceCity.SelectedItem.Value + "','" + ResidencePin1 + "','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'";
                    oDBEngine.InsurtFieldValue("tbl_master_address", RFieldName, RFieldValue);
                    if (OfficeAddress11 != "")
                    {
                        string OFieldName = "add_cntId,add_entity,add_addressType,add_address1,add_address2,add_address3,add_landmark,add_country,add_state,add_city,add_pin,CreateDate,CreateUser";
                        string OFieldValue = "'" + InternalId + "','Lead','Office','" + OfficeAddress11 + "','" + OfficeAddress21 + "','" + OfficeAddress31 + "','" + OfficeLandmark1 + "','" + DDLCountry.SelectedValue.ToString() + "','" + DDLState.SelectedValue.ToString() + "','" + DDLResidenceCity.SelectedItem.Value + "','" + OfficePin1 + "','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'";
                        oDBEngine.InsurtFieldValue("tbl_master_address", OFieldName, OFieldValue);
                    }
                    if (EmailAddress1 != "")
                    {
                        oDBEngine.InsurtFieldValue("tbl_master_email", "eml_entity,eml_cntid,eml_type,eml_email,eml_ccEmail,CreateDate,CreateUser", "'lead','" + InternalId + "','Official','" + EmailAddress1 + "','" + CCEmail1 + "','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'");
                    }
                    if (ResidencePhoneNo1 != "")
                    {
                        oDBEngine.InsurtFieldValue("tbl_master_phonefax", "phf_cntId,phf_entity,phf_type,phf_phoneNumber,phf_faxNumber,CreateDate,CreateUser", "'" + InternalId + "','Lead','Residence','" + ResidencePhoneNo1 + "','" + ResidenceFaxNo1 + "','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'");
                    }
                    if (OfficePhoneNo1 != "")
                    {
                        oDBEngine.InsurtFieldValue("tbl_master_phonefax", "phf_cntId,phf_entity,phf_type,phf_phoneNumber,phf_faxNumber,CreateDate,CreateUser", "'" + InternalId + "','Lead','Office','" + OfficePhoneNo1 + "','" + OfficeFaxNo1 + "','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'");
                    }
                }
                TrMain.Visible = false;
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('Your data import process is completed')</script>");
            }
            catch
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('Your data import cann't be process at this moment please contact to administrator')</script>");
            }
            finally
            {
                F2.Close();
            }
        }
        protected void DDLCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindState();
            BindCity();
        }
        public void BindState()
        {
            try
            {
                string[,] State = oDBEngine.GetFieldValue("tbl_master_state", "id,state", " countryId='" + DDLCountry.SelectedItem.Value + "'", 2, "state");
                if (State[0, 0] != "n")
                {
                    cls.AddDataToDropDownList(State, DDLState);
                }
            }
            catch
            {
            }
        }
        public void BindCity()
        {
            try
            {
                string[,] City = oDBEngine.GetFieldValue("tbl_master_city", "city_id,city_name", " state_id='" + DDLState.SelectedItem.Value + "'", 2, "city_name");
                if (City[0, 0] != "n")
                {
                    cls.AddDataToDropDownList(City, DDLResidenceCity);
                }
            }
            catch
            {
            }
        }
        protected void DDLState_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCity();
        }
    }
}