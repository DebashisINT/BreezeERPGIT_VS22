using ClsDropDownlistNameSpace;
using DataAccessLayer;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
	public partial class rootcompany_deductorinfo : System.Web.UI.Page
	{
        Int32 ID;
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.GenericStoreProcedure oGenericStoreProcedure;
        BusinessLogicLayer.Company ORootCompaniesGeneralBL = new BusinessLogicLayer.Company();
        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();
        clsDropDownList OclsDropDownList = new clsDropDownList();

        public string pageAccess = "";

        protected void Page_Init(object sender, EventArgs e)
        {

         
            drTDSState.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            drdministry.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
           
           

        }

		protected void Page_Load(object sender, EventArgs e)
		{
            string strCmpInternalid = "";
            if (!IsPostBack)
            {
                strCmpInternalid = Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]);


                DataTable dttbl = GetDetailsofTdsData();
                if (dttbl != null && dttbl.Rows.Count > 0)
                {
                    string assmntYr = Convert.ToString(dttbl.Rows[0]["Assessment_Year"]);
                    string FinYear = Convert.ToString(dttbl.Rows[0]["FinYear"]);
                    string Deductor_Name = Convert.ToString(dttbl.Rows[0]["Deductor_Name"]);
                    string Deductor_Branch = Convert.ToString(dttbl.Rows[0]["Deductor_Branch"]);
                    string Deductor_Addr1 = Convert.ToString(dttbl.Rows[0]["Deductor_Addr1"]);
                    string Deductor_Addr2 = Convert.ToString(dttbl.Rows[0]["Deductor_Addr2"]);
                    string Deductor_Addr3 = Convert.ToString(dttbl.Rows[0]["Deductor_Addr3"]);
                    string Deductor_Addr4 = Convert.ToString(dttbl.Rows[0]["Deductor_Addr4"]);
                    string Deductor_Addr5 = Convert.ToString(dttbl.Rows[0]["Deductor_Addr5"]);
                    string StateName = Convert.ToString(dttbl.Rows[0]["StateName"]);
                    string Pincode = Convert.ToString(dttbl.Rows[0]["Pincode"]);
                    string Deductor_Mail = Convert.ToString(dttbl.Rows[0]["Deductor_Mail"]);
                    string Deductor_STD = Convert.ToString(dttbl.Rows[0]["Deductor_STD"]);
                    string Deductor_Telephone = Convert.ToString(dttbl.Rows[0]["Deductor_Telephone"]);
                    Boolean Deductor_Change_Addr = Convert.ToBoolean(dttbl.Rows[0]["Deductor_Change_Addr"]);
                    string Deductor_resp_name = Convert.ToString(dttbl.Rows[0]["Deductor_resp_name"]);
                    string Deductor_resp_designation = Convert.ToString(dttbl.Rows[0]["Deductor_resp_designation"]);
                    string Deductor_resp_addr1 = Convert.ToString(dttbl.Rows[0]["Deductor_resp_addr1"]);
                    string Deductor_resp_addr2 = Convert.ToString(dttbl.Rows[0]["Deductor_resp_addr2"]);
                    string Deductor_resp_addr3 = Convert.ToString(dttbl.Rows[0]["Deductor_resp_addr3"]);
                    string Deductor_resp_addr4 = Convert.ToString(dttbl.Rows[0]["Deductor_resp_addr4"]);
                    string Deductor_resp_addr5 = Convert.ToString(dttbl.Rows[0]["Deductor_resp_addr5"]);
                    string Deductor_resp_state = Convert.ToString(dttbl.Rows[0]["Deductor_resp_state"]);
                    string Deductor_resp_pin = Convert.ToString(dttbl.Rows[0]["Deductor_resp_pin"]);
                    string Deductor_resp_mail = Convert.ToString(dttbl.Rows[0]["Deductor_resp_mail"]);
                    string Deductor_mobile = Convert.ToString(dttbl.Rows[0]["Deductor_mobile"]);
                    string Deductor_resp_STD = Convert.ToString(dttbl.Rows[0]["Deductor_resp_STD"]);
                    string Deductor_resp_telephhone = Convert.ToString(dttbl.Rows[0]["Deductor_resp_telephhone"]);
                    Boolean Deductor_resp_change_addr = Convert.ToBoolean(dttbl.Rows[0]["Deductor_resp_change_addr"]);
                    string Deductor_TDSState = Convert.ToString(dttbl.Rows[0]["Deductor_TDSState"]);
                    string Deductor_PAO = Convert.ToString(dttbl.Rows[0]["Deductor_PAO"]);
                    string Deductor_DDO = Convert.ToString(dttbl.Rows[0]["Deductor_DDO"]);
                    string Deductor_ministry = Convert.ToString(dttbl.Rows[0]["Deductor_ministry"]);
                    string Deductor_ministry_other = Convert.ToString(dttbl.Rows[0]["Deductor_ministry_other"]);
                    string Deductor_resp_PAN = Convert.ToString(dttbl.Rows[0]["Deductor_resp_PAN"]);
                    string Deductor_PAO_registration = Convert.ToString(dttbl.Rows[0]["Deductor_PAO_registration"]);
                    string Deductor_DDO_registration = Convert.ToString(dttbl.Rows[0]["Deductor_DDO_registration"]);
                    string Deductor_emp_STD_Alt = Convert.ToString(dttbl.Rows[0]["Deductor_emp_STD_Alt"]);
                    string Deductor_emp_Tel_Alt = Convert.ToString(dttbl.Rows[0]["Deductor_emp_Tel_Alt"]);
                    string Deductor_emp_mail_ALt = Convert.ToString(dttbl.Rows[0]["Deductor_emp_mail_ALt"]);
                    string Deductor_resp_STD_Alt = Convert.ToString(dttbl.Rows[0]["Deductor_resp_STD_Alt"]);
                    string Deductor_resp_Tel_Alt = Convert.ToString(dttbl.Rows[0]["Deductor_resp_Tel_Alt"]);
                    string Deductor_resp_mail_Alt = Convert.ToString(dttbl.Rows[0]["Deductor_resp_mail_Alt"]);
                    string Deductor_AIN = Convert.ToString(dttbl.Rows[0]["Deductor_AIN"]);
                    string Deductor_GSTIN = Convert.ToString(dttbl.Rows[0]["Deductor_GSTIN"]);
                    string Deductor_State = Convert.ToString(dttbl.Rows[0]["Deductor_State"]);
                    string Deductor_Pin = Convert.ToString(dttbl.Rows[0]["Deductor_Pin"]);
                    string Deductor_resp_stateId = Convert.ToString(dttbl.Rows[0]["Deductor_resp_stateId"]);
                    string Deductor_resp_pinId = Convert.ToString(dttbl.Rows[0]["Deductor_resp_pinId"]);
                    string Deductor_TDSStateId=Convert.ToString(dttbl.Rows[0]["Deductor_TDSStateId"]);

                    txtAssyear.Text = assmntYr;
                    txtfinyr.Text = FinYear;
                    txtNamedeductor.Text = Deductor_Name;
                    txtBranchdeduct.Text = Deductor_Branch;
                    txtDeductaddr1.Text = Deductor_Addr1;
                    txtDeductaddr2.Text = Deductor_Addr2;
                    txtDeductaddr3.Text = Deductor_Addr3;
                    txtDeductaddr4.Text = Deductor_Addr4;
                    txtDeductaddr5.Text = Deductor_Addr5;
                    txtDeductpin.Text = Pincode;
                    txtDeductState.Text = StateName;
                    txtDeductEmail.Text = Deductor_Mail;
                    txtdeductSTD.Text = Deductor_STD;
                    txtDeductTelNo.Text = Deductor_Telephone;
                    ChkdeductAddrReturn.Checked = Deductor_Change_Addr;
                    txtResponsibleDeduct.Text = Deductor_resp_name;
                    txtdeductdesig.Text = Deductor_resp_designation;
                    txtPersaddr1.Text = Deductor_resp_addr1;
                    txtPersaddr2.Text = Deductor_resp_addr2;
                    txtPersaddr3.Text = Deductor_resp_addr3;
                    txtPersaddr4.Text = Deductor_resp_addr4;
                    txtPersaddr5.Text = Deductor_resp_addr5;
                    txtpersPin.Text = Deductor_resp_pin;
                    txtPersState.Text = Deductor_resp_state;
                    txtPersemail.Text = Deductor_resp_mail;
                    txtMobile.Text = Deductor_mobile;
                    txtPersSTD.Text = Deductor_resp_STD;
                    txtPersTel.Text = Deductor_resp_telephhone;
                    chkResPersaddr.Checked = Deductor_resp_change_addr;
                    drdTDSState.Text = Deductor_TDSState;
                    drdpao.Text = Deductor_PAO;
                    drdDDO.Text = Deductor_DDO;
                    drdMinstryName.Text = Deductor_ministry;
                    txtOtherMinstryName.Text = Deductor_ministry_other;
                    txtRePanPers.Text = Deductor_resp_PAN;
                    txtPaoNo.Text = Deductor_PAO_registration;
                    txtDdoNo.Text = Deductor_DDO_registration;
                    txtEmpaltSTD.Text = Deductor_emp_STD_Alt;
                    txtEmpAltTel.Text = Deductor_emp_Tel_Alt;
                    txtEmpAltEmail.Text = Deductor_emp_mail_ALt;
                    txtPersAltSTD.Text = Deductor_resp_STD_Alt;
                    txtPersAltTel.Text = Deductor_resp_Tel_Alt;
                    txtResPersEmail.Text = Deductor_resp_mail_Alt;
                    txtAcctAIN.Text = Deductor_AIN;
                    txtGST.Text = Deductor_GSTIN;
                    HdndeductPin.Value = Deductor_Pin;
                    hdnDeductStateid.Value = Deductor_State;
                    hdnPersPinId.Value = Deductor_resp_pinId;
                    hdnPersStateId.Value = Deductor_resp_stateId;
                    drdTDSState.Value = Deductor_TDSStateId;
                }
            }
		}



       

        protected void Save_deductorInfo(object sender, EventArgs e)
        {
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("Prc_TDSDeductor");
            proc.AddVarcharPara("@Action", 200, "InsertTDSDeductorInfo");
            proc.AddIntegerPara("@AssessmentYear", Convert.ToInt32(txtAssyear.Text));
            proc.AddIntegerPara("@FinYear", Convert.ToInt32(txtfinyr.Text));
            proc.AddVarcharPara("@DeductorName", 150, txtNamedeductor.Text);
            proc.AddVarcharPara("@DeductorBranch", 150, txtBranchdeduct.Text);
            proc.AddVarcharPara("@DeductorAddr1", 150, txtDeductaddr1.Text);
            proc.AddVarcharPara("@DeductorAddr2", 150, txtDeductaddr2.Text);
            proc.AddVarcharPara("@DeductorAddr3", 150, txtDeductaddr3.Text);
            proc.AddVarcharPara("@DeductorAddr4", 150, txtDeductaddr4.Text);
            proc.AddVarcharPara("@DeductorAddr5", 150, txtDeductaddr5.Text);
            proc.AddIntegerPara("@DeductorPin", Convert.ToInt32(HdndeductPin.Value));
            proc.AddIntegerPara("@DeductorState", Convert.ToInt32(hdnDeductStateid.Value));
            proc.AddVarcharPara("@DeductorMail", 150, txtDeductEmail.Text);
            if (!string.IsNullOrEmpty(txtdeductSTD.Text))
            {
                proc.AddVarcharPara("@DeductorSTD", 150,Convert.ToString(txtdeductSTD.Text));
            }
            else
            {
                proc.AddVarcharPara("@DeductorSTD",150,Convert.ToString("") );
            }

            if (!string.IsNullOrEmpty(txtDeductTelNo.Text))
            {
                proc.AddIntegerPara("@DeductorTelephone", Convert.ToInt32(txtDeductTelNo.Text));
            }
            else
            {
                proc.AddIntegerPara("@DeductorTelephone", Convert.ToInt32(0));
            }
            
            proc.AddBooleanPara("@DeductorChangeAddr", ChkdeductAddrReturn.Checked);
            proc.AddVarcharPara("@Deductorrespname", 150, txtResponsibleDeduct.Text);
            proc.AddVarcharPara("@Deductorrespdesignation", 100, txtdeductdesig.Text);
            proc.AddVarcharPara("@Deductorrespaddr1", 150, txtPersaddr1.Text);
            proc.AddVarcharPara("@Deductorrespaddr2", 150, txtPersaddr2.Text);
            proc.AddVarcharPara("@Deductorrespaddr3", 150, txtPersaddr3.Text);
            proc.AddVarcharPara("@Deductorrespaddr4", 150, txtPersaddr4.Text);
            proc.AddVarcharPara("@Deductorrespaddr5", 150, txtPersaddr5.Text);
            proc.AddIntegerPara("@Deductorresppin", Convert.ToInt32(hdnPersPinId.Value));
            proc.AddIntegerPara("@Deductorrespstate",Convert.ToInt32(hdnPersStateId.Value));
            proc.AddVarcharPara("@Deductorrespmail", 150, txtPersemail.Text);
            proc.AddVarcharPara("@Deductormobile", 150, txtMobile.Text);
            if (!string.IsNullOrEmpty(txtPersSTD.Text))
            {
                proc.AddIntegerPara("@DeductorrespSTD", Convert.ToInt32(txtPersSTD.Text));
            }
            else
            {
                proc.AddIntegerPara("@DeductorrespSTD", Convert.ToInt32(0));
            }
            if (!string.IsNullOrEmpty(txtPersTel.Text))
            {
                proc.AddIntegerPara("@Deductorresptelephhone", Convert.ToInt32(txtPersTel.Text));
            }
            else
            {
                proc.AddIntegerPara("@Deductorresptelephhone", Convert.ToInt32(0));
            }
           
            proc.AddBooleanPara("@Deductorrespchangeaddr", chkResPersaddr.Checked);
            proc.AddVarcharPara("@DeductorTDSState", 20, Convert.ToString(drdTDSState.Value));
            proc.AddVarcharPara("@DeductorPAO", 100, Convert.ToString(drdpao.Text));
            proc.AddVarcharPara("@DeductorDDO", 100, Convert.ToString(drdDDO.Text));
            proc.AddVarcharPara("@Deductorministry", 50, Convert.ToString(drdMinstryName.Value));
            proc.AddVarcharPara("@Deductorministryother", 150, txtOtherMinstryName.Text);
            proc.AddVarcharPara("@DeductorrespPAN", 150, txtRePanPers.Text);
            if (!string.IsNullOrEmpty(txtPaoNo.Text))
            {
                proc.AddIntegerPara("@DeductorPAOregistration", Convert.ToInt32(txtPaoNo.Text));
            }
            else
            {
                proc.AddIntegerPara("@DeductorPAOregistration", Convert.ToInt32(0));
            }
           
            proc.AddVarcharPara("@DeductorDDOregistration", 150, txtDdoNo.Text);
            if (!string.IsNullOrEmpty(txtEmpaltSTD.Text))
            {
                proc.AddIntegerPara("@DeductorempSTDAlt", Convert.ToInt32(txtEmpaltSTD.Text));
            }
            else
            {
                proc.AddIntegerPara("@DeductorempSTDAlt", Convert.ToInt32(0));
            }
            if (!string.IsNullOrEmpty(txtEmpAltTel.Text))
            {
                proc.AddIntegerPara("@DeductorempTelAlt", Convert.ToInt32(txtEmpAltTel.Text));
            }
            else
            {
                proc.AddIntegerPara("@DeductorempTelAlt", Convert.ToInt32(0));
            }
            proc.AddVarcharPara("@DeductorempmailALt", 150, txtEmpAltEmail.Text);
            if (!string.IsNullOrEmpty(txtPersAltSTD.Text))
            {
                proc.AddIntegerPara("@DeductorrespSTDAlt", Convert.ToInt32(txtPersAltSTD.Text));
            }
            else
            {
                proc.AddIntegerPara("@DeductorrespSTDAlt", Convert.ToInt32(0));
            }
            if (!string.IsNullOrEmpty(txtPersAltTel.Text))
            {
                proc.AddIntegerPara("@DeductorrespTelAlt", Convert.ToInt32(txtPersAltTel.Text));
            }
            else
            {
                proc.AddIntegerPara("@DeductorrespTelAlt", Convert.ToInt32(0));
            }
            proc.AddVarcharPara("@DeductorrespmailAlt", 150, txtResPersEmail.Text);
            proc.AddVarcharPara("@DeductorAIN", 10, txtAcctAIN.Text);
            proc.AddVarcharPara("@DeductorGSTIN", 20, txtGST.Text);
            proc.AddVarcharPara("@deductCompany_id", 150, Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]));
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            proc.GetScalar();
             rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));


             Page.Response.Redirect(Page.Request.Url.ToString(), true);

        }
        public DataTable GetDetailsofTdsData()
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_TDSDeductor");
            proc.AddVarcharPara("@Action", 500, "SelectTDSDeductorInfo");
            proc.AddVarcharPara("@deductCompany_id", 150, Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]));
            dt = proc.GetTable();
            return dt;
        }

	}
}