using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Data;
using System.Configuration;
using BusinessLogicLayer;
//using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxHtmlEditor;
using System.Data;
using System.Data.SqlClient;
using EntityLayer;
using EntityLayer.CommonELS;
//using DevExpress.Web.ASPxGridView;


namespace ERP.OMS.Management.Master
{
    public partial class AssetDetail : ERP.OMS.ViewState_class.VSPage
	{
        //BusinessLogicLayer.DBEngine dbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine dbEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        GenericMethod objGenericMethod = new GenericMethod();
        AssetDetailBL objAssetDetailbl = new AssetDetailBL();
        string strMainAccountCode = "";
        string strSubAccountCode = "";
        string Scompany = "";
        string FinYears = "";
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Init(object sender, EventArgs e)
        {
            FinYear.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);//added for connection purpose
        }
		protected void Page_Load(object sender, EventArgs e)
		{
             // .............................Code Commented and Added by Sam on 02122016. to call this page through Account Haeds and checking the rights . ..................................... 
                if(!IsPostBack)
                {
                    

                    if (Session["querystring"] != null)
                    {
                        Session["redirct"] = "frm_subledger.aspx" + Convert.ToString(Session["querystring"]);
                    }
                }
                if (Convert.ToString(Session["requesttype"]).Trim() == "Account Heads" || Convert.ToString(Session["requesttype"]).Trim() == "Sub Ledger")
                {
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/AccountGroup.aspx");
                }

           // .............................Code Above Commented and Added by Sam on 02122016. ......................................
            
            AssetDetailGrid.JSProperties["cpinsert"] = null;
            AssetDetailGrid.JSProperties["cpEdit"] = null;
            AssetDetailGrid.JSProperties["cpUpdate"] = null;
            AssetDetailGrid.JSProperties["cpDelete"] = null;
            AssetDetailGrid.JSProperties["cpExists"] = null;
            AssetDetailGrid.JSProperties["cpUpdateValid"] = null;
            //Company.SelectCommand = "Select cmp_Name as CompanyName,cmp_internalid as ID from tbl_master_company";
            if (!IsPostBack)
            {
                Scompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                int cindex = 0;
                foreach (ListItem li in cmbCompany.Items)
                {
                    if (li.Value == Scompany)
                    {
                        break;
                    }
                    else
                    {
                        cindex = cindex + 1;
                    }
                }
                if (cmbCompany.Items.Count > 0)
                {
                    cmbCompany.SelectedIndex = cindex;
                }
                BindAllDropDownDtl();
                if (Session["Name"] != null)
                {
                    //lblName.Text = Session["Name"].ToString();
                }
            }
            bindgrid();
        //    if (Request.QueryString["id"] != null)
        //{
        //    if (Request.QueryString["id"].ToString() != "")
        //    {
        //        strMainAccountCode = Request.QueryString["id"].ToString();
        //        AssetDetaildata.SelectCommand = "select [AssetDetail_ID],[AssetDetail_CompanyID],[AssetDetail_BroughtForward],[AssetDetail_DepreciationIT],[AssetDetail_FinYear],[AssetDetail_MainAccountCode],[AssetDetail_PurchaseDate],[AssetDetail_Category],[AssetDetail_CostPrice],([AssetDetail_BroughtForward]+[AssetDetail_CostPrice]+[AssetDetail_Additions]-[AssetDetail_Disposals])-[AssetDetail_Depreciation] as NetValue,[AssetDetail_Additions],[AssetDetail_Disposals],[AssetDetail_Depreciation],[AssetDetail_Vendor],[AssetDetail_ServiceProvider],[AssetDetail_AMCExpiryDate],[AssetDetail_Insurer],[AssetDetail_PolicyExpiryDate],[AssetDetail_PremiumDueDate],[AssetDetail_Location],[AssetDetail_User] From [Master_AssetDetail] where AssetDetail_MainAccountCode='" + strMainAccountCode + "'Order By [AssetDetail_FinYear]";
        //        AssetDetailGrid.DataBind();
        //        hdnMainAccountCode.Value = strMainAccountCode;
        //    }
        //}

        //if ((Request.QueryString["kval"] != null && Request.QueryString["kcode"] != null))
        //{
        //    if ((Request.QueryString["kval"].ToString() != "" && Request.QueryString["kcode"].ToString() != ""))
        //    {
        //        strSubAccountCode = Request.QueryString["kval"].ToString();
        //        strMainAccountCode = Request.QueryString["kcode"].ToString();
        //        AssetDetaildata.SelectCommand = "select [AssetDetail_ID],[AssetDetail_CompanyID],[AssetDetail_BroughtForward],[AssetDetail_DepreciationIT],[AssetDetail_FinYear],[AssetDetail_MainAccountCode],[AssetDetail_PurchaseDate],[AssetDetail_Category],[AssetDetail_CostPrice],([AssetDetail_BroughtForward]+[AssetDetail_CostPrice]+[AssetDetail_Additions]-[AssetDetail_Disposals])-[AssetDetail_Depreciation] as NetValue,[AssetDetail_Additions],[AssetDetail_Disposals],[AssetDetail_Depreciation],[AssetDetail_Vendor],[AssetDetail_ServiceProvider],[AssetDetail_AMCExpiryDate],[AssetDetail_Insurer],[AssetDetail_PolicyExpiryDate],[AssetDetail_PremiumDueDate],[AssetDetail_Location],[AssetDetail_User] From [Master_AssetDetail] where AssetDetail_MainAccountCode='" + strMainAccountCode + "' and AssetDetail_SubAccountCode='" + strSubAccountCode + "'Order By [AssetDetail_FinYear]";
        //        AssetDetailGrid.DataBind();
        //        hdnMainAccountCode.Value = strMainAccountCode;
        //        hdnSubAccountCode.Value = strSubAccountCode;
        //        Session["KeyVal"] = strSubAccountCode;
        //    }
        //}

        
        if (Request.QueryString["formtype"] != null)
        {
            //string ID = Session["InternalId"].ToString();
            //Session["KeyVal_InternalID_New"] = ID.ToString();
            //DisabledTabPage();
        }
        else
        {
            if (Session["KeyVal_InternalID"] != null)
            {
                //Session["KeyVal_InternalID_New"] = Session["KeyVal_InternalID"].ToString();
            }
        }
    }

        public void bindgrid()
        {
            

            if (Request.QueryString["id"] != null)
            {
                if (Request.QueryString["id"].ToString() != "")
                {
                    strMainAccountCode = Request.QueryString["id"].ToString();
                    //AssetDetaildata.SelectCommand = "select [AssetDetail_ID],[AssetDetail_CompanyID],[AssetDetail_BroughtForward],[AssetDetail_DepreciationIT],[AssetDetail_FinYear],[AssetDetail_MainAccountCode],[AssetDetail_PurchaseDate],[AssetDetail_Category],[AssetDetail_CostPrice],([AssetDetail_BroughtForward]+[AssetDetail_CostPrice]+[AssetDetail_Additions]-[AssetDetail_Disposals])-[AssetDetail_Depreciation] as NetValue,[AssetDetail_Additions],[AssetDetail_Disposals],[AssetDetail_Depreciation],[AssetDetail_Vendor],[AssetDetail_ServiceProvider],[AssetDetail_AMCExpiryDate],[AssetDetail_Insurer],[AssetDetail_PolicyExpiryDate],[AssetDetail_PremiumDueDate],[AssetDetail_Location],[AssetDetail_User] From [Master_AssetDetail] where AssetDetail_MainAccountCode='" + strMainAccountCode + "'Order By [AssetDetail_FinYear]";
                    //AssetDetailGrid.DataBind();
                    //hdnMainAccountCode.Value = strMainAccountCode;
                }
            }

            if ((Request.QueryString["kval"] != null && Request.QueryString["kcode"] != null))
            {
                if ((Request.QueryString["kval"].ToString() != "" && Request.QueryString["kcode"].ToString() != ""))
                {
                    strSubAccountCode = Request.QueryString["kval"].ToString();
                    strMainAccountCode = Request.QueryString["kcode"].ToString();
                    //AssetDetaildata.SelectCommand = "select [AssetDetail_ID],[AssetDetail_CompanyID],[AssetDetail_BroughtForward],[AssetDetail_DepreciationIT],[AssetDetail_FinYear],[AssetDetail_MainAccountCode],[AssetDetail_PurchaseDate],[AssetDetail_Category],[AssetDetail_CostPrice],([AssetDetail_BroughtForward]+[AssetDetail_CostPrice]+[AssetDetail_Additions]-[AssetDetail_Disposals])-[AssetDetail_Depreciation] as NetValue,[AssetDetail_Additions],[AssetDetail_Disposals],[AssetDetail_Depreciation],[AssetDetail_Vendor],[AssetDetail_ServiceProvider],[AssetDetail_AMCExpiryDate],[AssetDetail_Insurer],[AssetDetail_PolicyExpiryDate],[AssetDetail_PremiumDueDate],[AssetDetail_Location],[AssetDetail_User] From [Master_AssetDetail] where AssetDetail_MainAccountCode='" + strMainAccountCode + "' and AssetDetail_SubAccountCode='" + strSubAccountCode + "'Order By [AssetDetail_FinYear]";
                    //AssetDetailGrid.DataBind();
                    //hdnMainAccountCode.Value = strMainAccountCode;
                    //hdnSubAccountCode.Value = strSubAccountCode;
                    Session["KeyVal"] = strSubAccountCode;
                }
            }
            DataTable assetdtldt = objAssetDetailbl.PopulateGridForAssetDetail(strMainAccountCode, strSubAccountCode);
            if (assetdtldt.Rows.Count > 0)
            {
                AssetDetailGrid.DataSource = assetdtldt;
                AssetDetailGrid.DataBind();
            }
            else
            {
                AssetDetailGrid.DataSource = null;
                AssetDetailGrid.DataBind();
            }
        }
        public void BindAllDropDownDtl()
        {
            DataSet dst = new DataSet();
            dst = objAssetDetailbl.PopulateAllDropDownDataForAssetDetail();
            if(dst.Tables[0]!=null && dst.Tables[0].Rows.Count>0)
            {
                lstVendor.DataTextField = "VendorName";
                lstVendor.DataValueField = "cnt_internalid";
                lstVendor.DataSource = dst.Tables[0];
                lstVendor.DataBind();
                lstVendor.Items.Insert(0, new ListItem("Select", "0"));
            }
            if (dst.Tables[1] != null && dst.Tables[1].Rows.Count > 0)
            {
                lstServicepro.DataTextField = "ServiceProviderName";
                lstServicepro.DataValueField = "cnt_internalid";
                lstServicepro.DataSource = dst.Tables[1];
                lstServicepro.DataBind();
                lstServicepro.Items.Insert(0, new ListItem("Select", "0"));
            }
            if (dst.Tables[2] != null && dst.Tables[2].Rows.Count > 0)
            {
                lstinsurer.DataTextField = "InsurerName";
                lstinsurer.DataValueField = "cnt_internalid";
                lstinsurer.DataSource = dst.Tables[2];
                lstinsurer.DataBind();
                lstinsurer.Items.Insert(0, new ListItem("Select", "0"));
            }
            if (dst.Tables[3] != null && dst.Tables[3].Rows.Count > 0)
            {
                lstlocation.DataTextField = "BranchName";
                lstlocation.DataValueField = "branch_id";
                lstlocation.DataSource = dst.Tables[3];
                lstlocation.DataBind();
                lstlocation.Items.Insert(0, new ListItem("Select", "0"));

            }
            if (dst.Tables[4] != null && dst.Tables[4].Rows.Count > 0)
            {
                lstusedby.DataTextField = "UsedBy";
                lstusedby.DataValueField = "cnt_internalid";
                lstusedby.DataSource = dst.Tables[4];
                lstusedby.DataBind();
                lstusedby.Items.Insert(0, new ListItem("Select", "0"));
            }

            if (dst.Tables[5] != null && dst.Tables[5].Rows.Count > 0)
            {
                cmbFinYear.TextField = "FinancialYear";
                cmbFinYear.ValueField= "ID";                  
                cmbFinYear.DataSource = dst.Tables[5];
                cmbFinYear.DataBind();
            }
            if (dst.Tables[6] != null && dst.Tables[6].Rows.Count > 0)
            {
                cmbCompany.TextField = "CompanyName";
                cmbCompany.ValueField = "ID";
                cmbCompany.DataSource = dst.Tables[6];
                cmbCompany.DataBind();
            }
        }

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filename = "Asset Detail"; 
            Int32 Filter = int.Parse(drdExport.SelectedItem.Value.ToString());
            exporter.FileName = filename;
            exporter.PageHeader.Left = "Asset Detail"; 
            //exporter.PageHeader.Left = Convert.ToString(Session["Contactrequesttype"]);
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";
             
            AssetDetailGrid.Columns[19].Visible = false;
           
             
            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    //bindpdf();
                    break;
                case 2:
                    exporter.WriteXlsToResponse();
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }
         
    protected void AssetDetailGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        //AssetDetaildata.SelectCommand = "select [AssetDetail_ID],[AssetDetail_BroughtForward],[AssetDetail_DepreciationIT],[AssetDetail_CompanyID],[AssetDetail_FinYear],[AssetDetail_MainAccountCode],[AssetDetail_PurchaseDate],[AssetDetail_Category],[AssetDetail_CostPrice],([AssetDetail_BroughtForward]+[AssetDetail_CostPrice]+[AssetDetail_Additions]-[AssetDetail_Disposals])-[AssetDetail_Depreciation] as NetValue,[AssetDetail_Additions],[AssetDetail_Disposals],[AssetDetail_Depreciation],[AssetDetail_Vendor],[AssetDetail_ServiceProvider],[AssetDetail_AMCExpiryDate],[AssetDetail_Insurer],[AssetDetail_PolicyExpiryDate],[AssetDetail_PremiumDueDate],[AssetDetail_Location],[AssetDetail_User] From [Master_AssetDetail] Order By AssetDetail_ID Desc";
        //AssetDetaildata.DataBind();

        //if (e.Parameters == "s")
        //{
        //    AssetDetailGrid.Settings.ShowFilterRow = true;

        //}

        //if (e.Parameters == "All")
        //{
        //    AssetDetailGrid.FilterExpression = string.Empty;
        //}

        AssetDetailGrid.JSProperties["cpinsert"] = null;
        AssetDetailGrid.JSProperties["cpEdit"] = null;
        AssetDetailGrid.JSProperties["cpUpdate"] = null;
        AssetDetailGrid.JSProperties["cpDelete"] = null;
        AssetDetailGrid.JSProperties["cpExists"] = null;
        AssetDetailGrid.JSProperties["cpUpdateValid"] = null;

        int insertcount = 0;
        int updtcnt = 0;
        int deletecnt = 0;

        //oGenericMethod = new GenericMethod();
        //oGenericMethod = new BusinessLogicLayer.GenericMethod();

        string WhichCall = e.Parameters.ToString().Split('~')[0];
        string WhichType = null;
        if (e.Parameters.ToString().Contains("~"))
            if (e.Parameters.ToString().Split('~')[1] != "")
                WhichType = e.Parameters.ToString().Split('~')[1];

        if (e.Parameters == "s")
            AssetDetailGrid.Settings.ShowFilterRow = true;
        if (e.Parameters == "All")
            AssetDetailGrid.FilterExpression = string.Empty;

        #region Insertion
        if (WhichCall == "savecity")
        {
            //Scompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            //int cindex = 0;
            //foreach (ListItem li in cmbCompany.Items)
            //{
            //    if (li.Value == Scompany)
            //    {
            //        break;
            //    }
            //    else
            //    {
            //        cindex = cindex + 1;
            //    }
            //}
            //if (cmbCompany.Items.Count > 0)
            //{
            //    cmbCompany.SelectedIndex = cindex;
            //}
            //oGenericMethod = new BusinessLogicLayer.GenericMethod();
            Store_MasterBL oStore_MasterBL = new Store_MasterBL();
            int TradingLot = 0;
            int QuoteLot = 0;
            int DeliveryLot = 0;
            int productSize = 0;
            int ProductColor = 0;
            string strMsg = "fail";
            if (HttpContext.Current.Session["userid"] != null)
            {
                AssetDetailEL objAssetDetailEL = new AssetDetailEL();
                objAssetDetailEL.AssetDetail_CompanyID = Convert.ToString(cmbCompany.SelectedItem.Value).Trim();
                if (lstVendor.SelectedIndex != -1)
                {
                    objAssetDetailEL.AssetDetail_Vendor = Convert.ToString(lstVendor.SelectedItem.Value).Trim();
                }

                if (cmbAssetCategory.SelectedIndex != -1)
                {
                    objAssetDetailEL.AssetDetail_Category = Convert.ToString(cmbAssetCategory.SelectedItem.Value).Trim();
                }

                if (lstServicepro.SelectedIndex != -1)
                {
                    objAssetDetailEL.AssetDetail_ServiceProvider = Convert.ToString(lstServicepro.SelectedItem.Value).Trim();
                }

                objAssetDetailEL.AssetDetail_FinYear = Convert.ToString(cmbFinYear.Text);
                if (dtpExpiryDate.Date != dtpExpiryDate.MinDate)
                {
                    objAssetDetailEL.AssetDetail_AMCExpiryDate = Convert.ToDateTime(dtpExpiryDate.Date);
                }

                if (dtpPurchaseDate.Date != dtpExpiryDate.MinDate)
                {
                    objAssetDetailEL.AssetDetail_PurchaseDate = Convert.ToDateTime(dtpPurchaseDate.Date);
                }

                if (lstinsurer.SelectedIndex != -1)
                {
                    objAssetDetailEL.AssetDetail_Insurer = Convert.ToString(lstinsurer.SelectedItem.Value).Trim();
                }
                if (txtCostPrice.Text != "")
                {
                    objAssetDetailEL.AssetDetail_CostPrice = Convert.ToDecimal(txtCostPrice.Text);
                }
                else
                {
                    objAssetDetailEL.AssetDetail_CostPrice = 0;
                }

                if (dtpPolicyExpiry.Date != dtpExpiryDate.MinDate)
                {
                    objAssetDetailEL.AssetDetail_PolicyExpiryDate = Convert.ToDateTime(dtpPolicyExpiry.Date);
                }
                if (txtAddition.Text != "")
                {
                    objAssetDetailEL.AssetDetail_Additions = Convert.ToDecimal(txtAddition.Text);
                }
                else
                {
                    objAssetDetailEL.AssetDetail_Additions = 0;
                }

                if (dtpPremiumDue.Date != dtpExpiryDate.MinDate)
                {
                    objAssetDetailEL.AssetDetail_PremiumDueDate = Convert.ToDateTime(dtpPremiumDue.Date);
                }
                if (txtDisposals.Text != "")
                {
                    objAssetDetailEL.AssetDetail_Disposals = Convert.ToDecimal(txtDisposals.Text);
                }
                else
                {
                    objAssetDetailEL.AssetDetail_Disposals = 0;
                }

                if (lstlocation.SelectedIndex != -1)
                {
                    objAssetDetailEL.AssetDetail_Location = Convert.ToInt32(lstlocation.SelectedItem.Value);
                }
                if (txtDepreciation.Text != "")
                {
                    objAssetDetailEL.AssetDetail_Depreciation = Convert.ToDecimal(txtDepreciation.Text);
                }
                else
                {
                    objAssetDetailEL.AssetDetail_Depreciation = 0;
                }

                if (txtBroughtForward.Text != "")
                {
                    objAssetDetailEL.AssetDetail_BroughtForward = Convert.ToDecimal(txtBroughtForward.Text);
                }
                else
                {
                    objAssetDetailEL.AssetDetail_BroughtForward = 0;
                }
                if (txtDepreciationIT.Text != "")
                {
                    objAssetDetailEL.AssetDetail_DepreciationIT = Convert.ToDecimal(txtDepreciationIT.Text);
                }
                else
                {
                    objAssetDetailEL.AssetDetail_DepreciationIT = 0;
                }

                if (lstusedby.SelectedIndex != -1)
                {
                    objAssetDetailEL.AssetDetail_User = Convert.ToString(lstusedby.SelectedItem.Value).Trim();
                }
                //////////////////////////////////////////////
                if (Request.QueryString["id"] != null)
                {
                    strMainAccountCode = Request.QueryString["id"].ToString();
                }
                else
                {
                    strMainAccountCode = Convert.ToString(Request.QueryString["kcode"]);
                    strSubAccountCode = Convert.ToString(Request.QueryString["kval"]);
                }
                //////////////////////////////////////////////////
                if (strMainAccountCode != null)
                {
                    objAssetDetailEL.AssetDetail_MainAccountCode = Convert.ToString(strMainAccountCode).Trim();
                }
                if (strSubAccountCode != null)
                {
                    objAssetDetailEL.AssetDetail_SubAccountCode = Convert.ToString(strSubAccountCode).Trim();
                }
                objAssetDetailEL.AssetDetail_CreateUser = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                insertcount = objAssetDetailbl.InsertAssetDetail(objAssetDetailEL);

                strMsg = "Success";
            }
            else
            {
                strMsg = "Your session is end";
            }

            if (insertcount > 0)
            {
                AssetDetailGrid.JSProperties["cpinsert"] = "Success";
                bindgrid();
            }
            else
            {
                AssetDetailGrid.JSProperties["cpinsert"] = strMsg;
            }
        }
        #endregion Insertion

        #region Update Section

        if (WhichCall == "updatecity")
        { 
            string strMsg = "fail";
            if (HttpContext.Current.Session["userid"] != null)
            {
                AssetDetailEL objAssetDetailEL = new AssetDetailEL();
                objAssetDetailEL.AssetDetail_ID = Convert.ToInt32(WhichType);
                objAssetDetailEL.AssetDetail_CompanyID = Convert.ToString(cmbCompany.SelectedItem.Value).Trim();
                if (lstVendor.SelectedIndex != -1)
                {
                    objAssetDetailEL.AssetDetail_Vendor = Convert.ToString(lstVendor.SelectedItem.Value).Trim();
                }

                if (cmbAssetCategory.SelectedIndex != -1)
                {
                    objAssetDetailEL.AssetDetail_Category = Convert.ToString(cmbAssetCategory.SelectedItem.Value).Trim();
                }

                if (lstServicepro.SelectedIndex != -1)
                {
                    objAssetDetailEL.AssetDetail_ServiceProvider = Convert.ToString(lstServicepro.SelectedItem.Value).Trim();
                }

                objAssetDetailEL.AssetDetail_FinYear = Convert.ToString(cmbFinYear.Text); 
                if (dtpExpiryDate.Date != dtpExpiryDate.MinDate)
                {
                    objAssetDetailEL.AssetDetail_AMCExpiryDate = Convert.ToDateTime(dtpExpiryDate.Date);
                } 

                if (dtpPurchaseDate.Date != dtpExpiryDate.MinDate)
                {
                    objAssetDetailEL.AssetDetail_PurchaseDate = Convert.ToDateTime(dtpPurchaseDate.Date);
                }

                if (lstinsurer.SelectedIndex != -1)
                {
                    objAssetDetailEL.AssetDetail_Insurer = Convert.ToString(lstinsurer.SelectedItem.Value).Trim();
                }
                if (txtCostPrice.Text!="")
                {
                    objAssetDetailEL.AssetDetail_CostPrice = Convert.ToDecimal(txtCostPrice.Text);
                }
                else
                {
                    objAssetDetailEL.AssetDetail_CostPrice = 0;
                }
                
                if (dtpPolicyExpiry.Date != dtpExpiryDate.MinDate)
                {
                    objAssetDetailEL.AssetDetail_PolicyExpiryDate = Convert.ToDateTime(dtpPolicyExpiry.Date);
                }
                if (txtAddition.Text != "")
                {
                    objAssetDetailEL.AssetDetail_Additions = Convert.ToDecimal(txtAddition.Text);
                }
                else
                {
                    objAssetDetailEL.AssetDetail_Additions = 0;
                }
                
                if (dtpPremiumDue.Date != dtpExpiryDate.MinDate)
                {
                    objAssetDetailEL.AssetDetail_PremiumDueDate = Convert.ToDateTime(dtpPremiumDue.Date);
                }
                if (txtDisposals.Text!="")
                {
                    objAssetDetailEL.AssetDetail_Disposals = Convert.ToDecimal(txtDisposals.Text);
                }
                else
                {
                    objAssetDetailEL.AssetDetail_Disposals = 0;
                }
                
                if (lstlocation.SelectedIndex != -1)
                {
                    objAssetDetailEL.AssetDetail_Location = Convert.ToInt32(lstlocation.SelectedItem.Value);
                }
                if (txtDepreciation.Text!="")
                {
                    objAssetDetailEL.AssetDetail_Depreciation = Convert.ToDecimal(txtDepreciation.Text);
                }
                else
                {
                    objAssetDetailEL.AssetDetail_Depreciation =0;
                }

                if (txtBroughtForward.Text!="")
                {
                    objAssetDetailEL.AssetDetail_BroughtForward = Convert.ToDecimal(txtBroughtForward.Text);
                }
                else
                {
                    objAssetDetailEL.AssetDetail_BroughtForward = 0;
                }
                if (txtDepreciationIT.Text!="")
                {
                    objAssetDetailEL.AssetDetail_DepreciationIT = Convert.ToDecimal(txtDepreciationIT.Text);
                }
                else
                {
                    objAssetDetailEL.AssetDetail_DepreciationIT = 0;
                }
               
                if (lstusedby.SelectedIndex != -1)
                {
                    objAssetDetailEL.AssetDetail_User = Convert.ToString(lstusedby.SelectedItem.Value).Trim();
                }
                //////////////////////////////////////////////
                if (Request.QueryString["id"] != null)
                {
                    strMainAccountCode = Request.QueryString["id"].ToString();
                }
                else
                {
                    strMainAccountCode = Convert.ToString(Request.QueryString["kcode"]);
                    strSubAccountCode = Convert.ToString(Request.QueryString["kval"]);
                }
                //////////////////////////////////////////////////
                if (strMainAccountCode != null)
                {
                    objAssetDetailEL.AssetDetail_MainAccountCode = Convert.ToString(strMainAccountCode).Trim();
                }
                if (strSubAccountCode != null)
                {
                    objAssetDetailEL.AssetDetail_SubAccountCode = Convert.ToString(strSubAccountCode).Trim();
                } 
                objAssetDetailEL.AssetDetail_CreateUser = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                insertcount = objAssetDetailbl.UpdateAssetDetail(objAssetDetailEL);

                strMsg = "Success";
            }
            else
            {
                strMsg = "Your session is end";
            }
            //Store_MasterBL oStore_MasterBL = new Store_MasterBL();

            //Boolean sizeapplicable = false;
            //Boolean colorapplicable = false;
            

            //updtcnt = oStore_MasterBL.UpdateProduct(Convert.ToInt32(WhichType), txtPro_Code.Text, txtPro_Name.Text, txtPro_Description.Text, Convert.ToString(CmbProType.SelectedItem == null ? 0 : CmbProType.SelectedItem.Value),
            // Convert.ToInt32(CmbProClassCode.SelectedItem == null ? 0 : CmbProClassCode.SelectedItem.Value), txtGlobalCode.Text, Convert.ToInt32(txtTradingLot.Text), Convert.ToInt32(CmbTradingLotUnits.SelectedItem == null ? 0 : CmbTradingLotUnits.SelectedItem.Value),
            // Convert.ToInt32(CmbQuoteCurrency.SelectedItem == null ? 0 : CmbQuoteCurrency.SelectedItem.Value), Convert.ToInt32(txtQuoteLot.Text), Convert.ToInt32(CmbQuoteLotUnit.SelectedItem == null ? 0 : CmbQuoteLotUnit.SelectedItem.Value), Convert.ToInt32(txtDeliveryLot.Text),
            // Convert.ToInt32(CmbDeliveryLotUnit.SelectedItem == null ? 0 : CmbDeliveryLotUnit.SelectedItem.Value),
            // Convert.ToInt32(CmbProductColor.SelectedItem == null ? 0 : CmbProductColor.SelectedItem.Value),
            // Convert.ToInt32(CmbProductSize.SelectedItem == null ? 0 : CmbProductSize.SelectedItem.Value),
            // Convert.ToInt32(HttpContext.Current.Session["userid"]), sizeapplicable, colorapplicable);
            ////updtcnt = oStore_MasterBL.UpdateProduct(Convert.ToInt32(WhichType), txtPro_Code.Text, txtPro_Name.Text, txtPro_Description.Text, Convert.ToString(CmbProType.SelectedItem == null ? 0 : CmbProType.SelectedItem.Value),
            ////   Convert.ToInt32(CmbProClassCode.SelectedItem == null ? 0 : CmbProClassCode.SelectedItem.Value), txtGlobalCode.Text, Convert.ToInt32(txtTradingLot.Text), Convert.ToInt32(CmbTradingLotUnits.SelectedItem == null ? 0 : CmbTradingLotUnits.SelectedItem.Value),
            ////   Convert.ToInt32(CmbQuoteCurrency.SelectedItem == null ? 0 : CmbQuoteCurrency.SelectedItem.Value), Convert.ToInt32(txtQuoteLot.Text), Convert.ToInt32(CmbQuoteLotUnit.SelectedItem == null ? 0 : CmbQuoteLotUnit.SelectedItem.Value), Convert.ToInt32(txtDeliveryLot.Text),
            ////   Convert.ToInt32(CmbDeliveryLotUnit.SelectedItem == null ? 0 : CmbDeliveryLotUnit.SelectedItem.Value), Convert.ToInt32(CmbProductColor.SelectedItem == null ? 0 : CmbProductColor.SelectedItem.Value), Convert.ToInt32(CmbProductSize.SelectedItem == null ? 0 : CmbProductSize.SelectedItem.Value), Convert.ToInt32(HttpContext.Current.Session["userid"]));

            if (insertcount > 0)
            {
                AssetDetailGrid.JSProperties["cpUpdate"] = "Success";
                bindgrid();
            }
            else
            {
                AssetDetailGrid.JSProperties["cpUpdate"] = "fail";
            }


        }

        #endregion Update

        if (WhichCall == "Delete")
        {
            deletecnt = objAssetDetailbl.DeleteAssetDetail(Convert.ToInt32(WhichType));
            //deletecnt = objGenericMethod.Delete_Table("Master_sProducts", "sProducts_ID=" + WhichType + "");
            if (deletecnt > 0)
            {
                AssetDetailGrid.JSProperties["cpDelete"] = "Success";
                bindgrid();
            }
            else
                AssetDetailGrid.JSProperties["cpDelete"] = "Fail";
        }

        #region Edit Section
        if (WhichCall == "Edit")
        {
           
            DataTable dtEdit = objAssetDetailbl.GetAssetDetailById(Convert.ToInt32(WhichType));

            
            if (dtEdit.Rows.Count > 0 && dtEdit != null)
            {
                //AssetDetail_ID, AssetDetail_CompanyID, AssetDetail_FinYear, AssetDetail_MainAccountCode, AssetDetail_SubAccountCode, 
                //AssetDetail_Category, AssetDetail_PurchaseDate, AssetDetail_Vendor, AssetDetail_CostPrice, AssetDetail_BillNumber, 
                //AssetDetail_Additions, AssetDetail_Disposals, AssetDetail_Depreciation, AssetDetail_DepreciationIT, AssetDetail_Location,
                //AssetDetail_User, AssetDetail_Insurer, AssetDetail_Premium, AssetDetail_PolicyExpiryDate, AssetDetail_PremiumDueDate, 
                //AssetDetail_ServiceProvider, AssetDetail_AMCExpiryDate, AssetDetail_BroughtForward,
                //AssetDetail_CreateUser, AssetDetail_CreateDate, AssetDetail_ModifyUser, AssetDetail_ModifyDateTime


                //cmbCompany
                //lstVendor
                //cmbAssetCategory
                //lstServicepro
                //cmbFinYear
                //dtpExpiryDate
                //dtpPurchaseDate
                string cmbCompany = Convert.ToString(dtEdit.Rows[0]["AssetDetail_CompanyID"]);
                string lstVendor = Convert.ToString(dtEdit.Rows[0]["AssetDetail_Vendor"]);
                string cmbAssetCategory = Convert.ToString(dtEdit.Rows[0]["AssetDetail_Category"]);
                string lstServicepro = Convert.ToString(dtEdit.Rows[0]["AssetDetail_ServiceProvider"]);
                string cmbFinYear = Convert.ToString(dtEdit.Rows[0]["AssetDetail_FinYear"]);
                string dtpExpiryDate = Convert.ToString(dtEdit.Rows[0]["AssetDetail_AMCExpiryDate"]);
                //if(dtpExpiryDate!="")
                //{
                //    dtpExpiryDate =Convert.ToDateTime(dtpExpiryDate).ToString("dd-MM-yyyy");
                //}
                string dtpPurchaseDate = Convert.ToString(dtEdit.Rows[0]["AssetDetail_PurchaseDate"]);


                //lstinsurer
                //txtCostPrice
                //dtpPolicyExpiry
                //txtAddition
                //dtpPremiumDue
                //txtDisposals
                //lstlocation
                string lstinsurer = Convert.ToString(dtEdit.Rows[0]["AssetDetail_Insurer"]);
                string txtCostPrice = Convert.ToString(dtEdit.Rows[0]["AssetDetail_CostPrice"]);
                string dtpPolicyExpiry = Convert.ToString(dtEdit.Rows[0]["AssetDetail_PolicyExpiryDate"]);
                string txtAddition = Convert.ToString(dtEdit.Rows[0]["AssetDetail_Additions"]);
                string dtpPremiumDue = Convert.ToString(dtEdit.Rows[0]["AssetDetail_PremiumDueDate"]);
                string txtDisposals = Convert.ToString(dtEdit.Rows[0]["AssetDetail_Disposals"]);
                string lstlocation = Convert.ToString(dtEdit.Rows[0]["AssetDetail_Location"]);


                //txtDepreciation
                //txtBroughtForward
                //txtDepreciationIT
                //lstusedby
                string txtDepreciation = Convert.ToString(dtEdit.Rows[0]["AssetDetail_Depreciation"]);
                string txtBroughtForward = Convert.ToString(dtEdit.Rows[0]["AssetDetail_BroughtForward"]);
                string txtDepreciationIT = Convert.ToString(dtEdit.Rows[0]["AssetDetail_DepreciationIT"]);
                string lstusedby = Convert.ToString(dtEdit.Rows[0]["AssetDetail_User"]);

                AssetDetailGrid.JSProperties["cpEdit"] = cmbCompany + "~"
                                                + lstVendor + "~"
                                                + cmbAssetCategory + "~"
                                                + lstServicepro + "~"
                                                + cmbFinYear + "~"
                                                + dtpExpiryDate + "~"
                                                + dtpPurchaseDate + "~"
                                                + lstinsurer + "~"
                                                + txtCostPrice + "~"
                                                + dtpPolicyExpiry + "~"
                                                + txtAddition + "~"
                                                + dtpPremiumDue + "~"
                                                + txtDisposals + "~"
                                                + lstlocation + "~"
                                                + txtDepreciation + "~"
                                                + txtBroughtForward + "~"
                                                + txtDepreciationIT + "~" 
                                                + WhichType + "~"
                                                + lstusedby; 
            }
        } 
          #endregion Edit Section
    }








    protected void AssetDetailGrid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
    {
        //if (e.RowType == GridViewRowType.Data)
        //{
        //    if (e.GetValue("AssetDetail_BroughtForward") != DBNull.Value)
        //    {
        //        e.Row.Cells[1].Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.GetValue("AssetDetail_BroughtForward")));

        //    }
        //    if (e.GetValue("AssetDetail_CostPrice") != DBNull.Value)
        //    {
        //        e.Row.Cells[2].Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.GetValue("AssetDetail_CostPrice")));

        //    }
        //    if (e.GetValue("AssetDetail_Additions") != DBNull.Value)
        //    {
        //        e.Row.Cells[3].Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.GetValue("AssetDetail_Additions")));
        //    }
        //    if (e.GetValue("AssetDetail_Disposals") != DBNull.Value)
        //    {
        //        e.Row.Cells[4].Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.GetValue("AssetDetail_Disposals")));

        //    }
        //    if (e.GetValue("AssetDetail_Depreciation") != DBNull.Value)
        //    {
        //        e.Row.Cells[5].Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.GetValue("AssetDetail_Depreciation")));

        //    }
        //    if (e.GetValue("NetValue") != DBNull.Value)
        //    {
        //        e.Row.Cells[6].Text = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(e.GetValue("NetValue")));

        //    }

        //}
    }

    protected void AssetDetailGrid_HtmlEditFormCreated(object sender, DevExpress.Web.ASPxGridViewEditFormEventArgs e)
    {
        Scompany = HttpContext.Current.Session["LastCompany"].ToString();
        FinYears = HttpContext.Current.Session["LastFinYear"].ToString();
        

        ASPxComboBox cmbCompany = (ASPxComboBox)AssetDetailGrid.FindEditFormTemplateControl("cmbCompany");
        // ...................................Code Added and Commented By Sam on 21112016.................................
        
        //cmbCompany.SelectedItem.Value = Scompany;
        int sindex=0;
        foreach(ListItem li in cmbCompany.Items)
        {
            if(li.Value==Scompany)
            {
                
                break;
            }
            else
            {
                sindex = sindex + 1;
            }
        }
        if (cmbCompany.Items.Count>0)
        {
            cmbCompany.SelectedIndex = sindex;
        }
        else
        {
            cmbCompany.SelectedIndex = -1;
        }
        

        ASPxComboBox cmbFinYear = (ASPxComboBox)AssetDetailGrid.FindEditFormTemplateControl("cmbFinYear");
        
        //cmbFinYear.SelectedItem.Value = FinYears;
        int fyindex = 0;
        foreach (ListItem li in cmbFinYear.Items)
        {
            if (li.Value == FinYears)
            {

                break;
            }
            else
            {
                fyindex = fyindex + 1;
            }
        }
        if (cmbFinYear.Items.Count > 0)
        {
            cmbFinYear.SelectedIndex = fyindex;
        }
        else
        {
            cmbFinYear.SelectedIndex = -1;
        }

        // ...................................Code Above Added and Commented By Sam on 21112016.................................
        TextBox vendorname = (TextBox)AssetDetailGrid.FindEditFormTemplateControl("txtVendor");
        vendorname.Attributes.Add("onkeyup", "CallList(this,'VendorName',event)");

        TextBox Serviceprovidername = (TextBox)AssetDetailGrid.FindEditFormTemplateControl("txtServiceProvider");
        Serviceprovidername.Attributes.Add("onkeyup", "CallList(this,'ServiceProviderName',event)");

        TextBox Insurername = (TextBox)AssetDetailGrid.FindEditFormTemplateControl("txtInsurer");
        Insurername.Attributes.Add("onkeyup", "CallList(this,'InsurerName',event)");

        TextBox LocationName = (TextBox)AssetDetailGrid.FindEditFormTemplateControl("txtLocation");
        LocationName.Attributes.Add("onkeyup", "CallList(this,'LocationName',event)");

        TextBox UsedBy = (TextBox)AssetDetailGrid.FindEditFormTemplateControl("txtUsedBy");
        UsedBy.Attributes.Add("onkeyup", "CallList(this,'UsedBy',event)");


         
    }
    protected void AssetDetailGrid_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
    {
        AssetDetailGrid.EndUpdate();
        Scompany = HttpContext.Current.Session["LastCompany"].ToString();

        ASPxTextBox txtBroughtForward = (ASPxTextBox)AssetDetailGrid.FindEditFormTemplateControl("txtBroughtForward");
        e.NewValues["AssetDetail_BroughtForward"] = txtBroughtForward.Text;

        ASPxTextBox txtDepreciationIT = (ASPxTextBox)AssetDetailGrid.FindEditFormTemplateControl("txtDepreciationIT");
        e.NewValues["AssetDetail_DepreciationIT"] = txtDepreciationIT.Text;


        //ASPxComboBox cmbCompany = (ASPxComboBox)AssetDetailGrid.FindEditFormTemplateControl("cmbCompany");
        e.NewValues["AssetDetail_CompanyID"] = Scompany;

        ASPxComboBox cmbFinYear = (ASPxComboBox)AssetDetailGrid.FindEditFormTemplateControl("cmbFinYear");
        e.NewValues["AssetDetail_FinYear"] = cmbFinYear.Value;


        ASPxDateEdit PurchaseDate = (ASPxDateEdit)AssetDetailGrid.FindEditFormTemplateControl("dtpPurchaseDate");
        //if (PurchaseDate.Date == Convert.ToDateTime("1/1/0001")) PurchaseDate.Date = oDBEngine.GetDate();
        e.NewValues["AssetDetail_PurchaseDate"] = PurchaseDate.Value;


        ASPxDateEdit ExpiryDate = (ASPxDateEdit)AssetDetailGrid.FindEditFormTemplateControl("dtpExpiryDate");
        //if (ExpiryDatee == Convert.ToDateTime("1/1/0001")) ExpiryDate.Date = oDBEngine.GetDate();

        e.NewValues["AssetDetail_AMCExpiryDate"] = ExpiryDate.Value;

        ASPxDateEdit PolicyExpiryDate = (ASPxDateEdit)AssetDetailGrid.FindEditFormTemplateControl("dtpPolicyExpiry");
        //if (PolicyExpiryDate.Date ==  Convert.ToDateTime("1/1/0001")) PolicyExpiryDate.Date = oDBEngine.GetDate();
        e.NewValues["AssetDetail_PolicyExpiryDate"] = PolicyExpiryDate.Value;

        ASPxDateEdit PremiumDate = (ASPxDateEdit)AssetDetailGrid.FindEditFormTemplateControl("dtpPremiumDue");
        //if (PremiumDate.Date == Convert.ToDateTime("1/1/0001")) PremiumDate.Date = oDBEngine.GetDate();
        e.NewValues["AssetDetail_PremiumDueDate"] = PremiumDate.Value;

        TextBox txtVendor = (TextBox)AssetDetailGrid.FindEditFormTemplateControl("txtVendor_hidden");
        e.NewValues["AssetDetail_Vendor"] = txtVendor.Text;

        TextBox txtServiceProvider = (TextBox)AssetDetailGrid.FindEditFormTemplateControl("txtServiceProvider_hidden");
        e.NewValues["AssetDetail_ServiceProvider"] = txtServiceProvider.Text;

        TextBox txtInsurer = (TextBox)AssetDetailGrid.FindEditFormTemplateControl("txtInsurer_hidden");
        e.NewValues["AssetDetail_Insurer"] = txtInsurer.Text;

        TextBox txtLocation = (TextBox)AssetDetailGrid.FindEditFormTemplateControl("txtLocation");
        e.NewValues["AssetDetail_Location"] = "";

        TextBox txtLoc = (TextBox)AssetDetailGrid.FindEditFormTemplateControl("txtLocation_hidden");
        e.NewValues["AssetDetail_Location"] = txtLoc.Text;

        TextBox txtUsed = (TextBox)AssetDetailGrid.FindEditFormTemplateControl("txtUsedBy_hidden");
        e.NewValues["AssetDetail_User"] = txtUsed.Text;

        ASPxComboBox cmbCategory = (ASPxComboBox)AssetDetailGrid.FindEditFormTemplateControl("cmbAssetCategory");
        e.NewValues["AssetDetail_Category"] = cmbCategory.Value;

        //e.NewValues["AssetDetail_MainAccountCode"] = hdnMainAccountCode.Value.ToString();

        //e.NewValues["AssetDetail_SubAccountCode"] = hdnSubAccountCode.Value.ToString();

        //int rowsaffected = dbEngine.InsurtFieldValue("Master_AssetDetail", "AssetDetail_CompanyID, AssetDetail_FinYear,AssetDetail_MainAccountCode,AssetDetail_PurchaseDate, AssetDetail_Category, AssetDetail_Vendor, AssetDetail_CostPrice, AssetDetail_Additions, AssetDetail_Disposals, AssetDetail_Depreciation, AssetDetail_Location, AssetDetail_User, AssetDetail_Insurer, AssetDetail_Premium, AssetDetail_PolicyExpiryDate, AssetDetail_PremiumDueDate, AssetDetail_ServiceProvider, AssetDetail_AMCExpiryDate", "'" + +"','"+ +"','"+ +"','"+ +"','"+ +"','"+ +"','"+ +"','"+ +"','"+ +"','"+ +"','"+ +"','"+ +"','"+ +"','"+ +"','"+ +"','"+ +"'");

        //if (strSubAccountCode != "")
        //{
        //    e.NewValues["AssetDetail_SubAccountCode"] = strSubAccountCode;
        //}



    }
    protected void AssetDetailGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {
        Scompany = HttpContext.Current.Session["LastCompany"].ToString();

        ASPxTextBox txtBroughtForward = (ASPxTextBox)AssetDetailGrid.FindEditFormTemplateControl("txtBroughtForward");
        e.NewValues["AssetDetail_BroughtForward"] = txtBroughtForward.Text;

        ASPxTextBox txtDepreciationIT = (ASPxTextBox)AssetDetailGrid.FindEditFormTemplateControl("txtDepreciationIT");
        e.NewValues["AssetDetail_DepreciationIT"] = txtDepreciationIT.Text;

        //ASPxComboBox CompanyID = (ASPxComboBox)AssetDetailGrid.FindEditFormTemplateControl("cmbCompany");
        e.NewValues["AssetDetail_CompanyID"] = Scompany;

        ASPxTextBox CostPrice = (ASPxTextBox)AssetDetailGrid.FindEditFormTemplateControl("txtCostPrice");
        e.NewValues["AssetDetail_CostPrice"] = Convert.ToDecimal(CostPrice.Text);

        TextBox txtVendor = (TextBox)AssetDetailGrid.FindEditFormTemplateControl("txtVendor_hidden");
        e.NewValues["AssetDetail_Vendor"] = txtVendor.Text;

        TextBox txtInsurerhidden = (TextBox)AssetDetailGrid.FindEditFormTemplateControl("txtInsurer_hidden");
        e.NewValues["AssetDetail_Insurer"] = txtInsurerhidden.Text;

        TextBox txtServiceProviderhidden = (TextBox)AssetDetailGrid.FindEditFormTemplateControl("txtServiceProvider_hidden");
        e.NewValues["AssetDetail_ServiceProvider"] = txtServiceProviderhidden.Text;


        TextBox txtLocationhidden = (TextBox)AssetDetailGrid.FindEditFormTemplateControl("txtLocation_hidden");
        e.NewValues["AssetDetail_Location"] = txtLocationhidden.Text;

        TextBox txtUsedByhidden = (TextBox)AssetDetailGrid.FindEditFormTemplateControl("txtUsedBy_hidden");
        e.NewValues["AssetDetail_User"] = txtUsedByhidden.Text;


        ASPxDateEdit PurchaseDate = (ASPxDateEdit)AssetDetailGrid.FindEditFormTemplateControl("dtpPurchaseDate");
        e.NewValues["AssetDetail_PurchaseDate"] = PurchaseDate.Value;

        ASPxDateEdit ExpiryDate = (ASPxDateEdit)AssetDetailGrid.FindEditFormTemplateControl("dtpExpiryDate");
        e.NewValues["AssetDetail_AMCExpiryDate"] = ExpiryDate.Value;


        ASPxDateEdit PolicyExpiryDate = (ASPxDateEdit)AssetDetailGrid.FindEditFormTemplateControl("dtpPolicyExpiry");
        e.NewValues["AssetDetail_PolicyExpiryDate"] = PolicyExpiryDate.Value;

        ASPxDateEdit PremiumDate = (ASPxDateEdit)AssetDetailGrid.FindEditFormTemplateControl("dtpPremiumDue");
        e.NewValues["AssetDetail_PremiumDueDate"] = PremiumDate.Value;

        //e.NewValues["AssetDetail_SubAccountCode"] = Session["KeyVal"];

        AssetDetailGrid.DataBind();



    }
    protected void AssetDetailGrid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
    {

        ASPxComboBox cmbFinYear = (ASPxComboBox)AssetDetailGrid.FindEditFormTemplateControl("cmbFinYear");
        string str = cmbFinYear.Text.Trim();



        if (str == "2008-2009")
        {
            string startdate = "1/4/2008";
            string enddate = "3/31/2009";
            string dtstartdate = startdate;
            string dtenddate = enddate;
            DateTime dtpstartdate = Convert.ToDateTime(startdate);

            DateTime dtpenddate = Convert.ToDateTime(enddate);


            if (e.NewValues["AssetDetail_PurchaseDate"] != null)
            {
                string strPurchaseDate = Convert.ToDateTime(e.NewValues["AssetDetail_PurchaseDate"]).ToString("MM/dd/yyyy");
                DateTime dtPurchaseDate = Convert.ToDateTime(strPurchaseDate);
                if ((dtPurchaseDate >= dtpstartdate) && (dtPurchaseDate <= dtpenddate))
                {

                }
                else
                {
                    e.RowError = "Not Valid PurchaseDate";
                    return;
                }
            }
        }
        else if (str == "2009-2010")
        {
            string startdate = "1/4/2009";
            string enddate = "3/31/2010";
            string dtstartdate = startdate;
            string dtenddate = enddate;
            DateTime dtpstartdate = Convert.ToDateTime(startdate);

            DateTime dtpenddate = Convert.ToDateTime(enddate);


            if (e.NewValues["AssetDetail_PurchaseDate"] != null)
            {
                string strPurchaseDate = Convert.ToDateTime(e.NewValues["AssetDetail_PurchaseDate"]).ToString("MM/dd/yyyy");
                DateTime dtPurchaseDate = Convert.ToDateTime(strPurchaseDate);
                if ((dtPurchaseDate >= dtpstartdate) && (dtPurchaseDate <= dtpenddate))
                {

                }
                else
                {
                    e.RowError = "Not Valid Date";
                    return;
                }
            }
        }


        if (AssetDetailGrid.IsNewRowEditing)
        {
            DataTable dtbl = null;
            //dtbl = dbEngine.GetDataTable("Master_Assetdetail", "*", "AssetDetail_SubAccountCode='" + hdnSubAccountCode.Value + "' And AssetDetail_FinYear='" + str + "'");
            if (dtbl.Rows.Count > 0)
            {

                e.RowError = "Already Record Exist!";
                return;

            }
        }

        //if ((e.NewValues["AssetDetail_CompanyID"] == null) || (e.NewValues["AssetDetail_CompanyID"].ToString() == ""))
        //{
        //    e.RowError = "Please select Company Name.";
        //    return;
        //}

    }


    protected void AssetDetailGrid_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
    {
        string id = e.EditingKeyValue.ToString();
        ASPxTextBox txtDepreciationIT = (ASPxTextBox)AssetDetailGrid.FindEditFormTemplateControl("txtDepreciationIT");
        ASPxTextBox txtBroughtForward = (ASPxTextBox)AssetDetailGrid.FindEditFormTemplateControl("txtBroughtForward");
        TextBox txtLocation = (TextBox)AssetDetailGrid.FindEditFormTemplateControl("txtLocation");
        TextBox txtLocationhidden = (TextBox)AssetDetailGrid.FindEditFormTemplateControl("txtLocation_hidden");
        ASPxComboBox CompanyID = (ASPxComboBox)AssetDetailGrid.FindEditFormTemplateControl("cmbCompany");
        ASPxComboBox cmbFinYear = (ASPxComboBox)AssetDetailGrid.FindEditFormTemplateControl("cmbFinYear");
        ASPxDateEdit PurchaseDate = (ASPxDateEdit)AssetDetailGrid.FindEditFormTemplateControl("dtpPurchaseDate");
        ASPxDateEdit ExpiryDate = (ASPxDateEdit)AssetDetailGrid.FindEditFormTemplateControl("dtpExpiryDate");
        ASPxDateEdit PolicyExpiryDate = (ASPxDateEdit)AssetDetailGrid.FindEditFormTemplateControl("dtpPolicyExpiry");
        ASPxDateEdit PremiumDate = (ASPxDateEdit)AssetDetailGrid.FindEditFormTemplateControl("dtpPremiumDue");
        DataTable dtLocation = dbEngine.GetDataTable("Master_AssetDetail A,tbl_master_branch B", "B.Branch_id as BranchID,A.AssetDetail_FinYear as FinYear,A.AssetDetail_CompanyID As CompanyID,A.AssetDetail_Location,B.branch_description+'('+B.branch_code +')' as BranchName", "B.Branch_id=A.AssetDetail_Location and A.AssetDetail_ID=" + id);
        if (dtLocation.Rows.Count > 0)
        {
            txtLocation.Text = dtLocation.Rows[0]["BranchName"].ToString();
            txtLocationhidden.Text = dtLocation.Rows[0]["BranchID"].ToString();
        }
        DataTable dtCompany = dbEngine.GetDataTable("Master_AssetDetail", "AssetDetail_FinYear as FinYear,AssetDetail_CompanyID As CompanyID,AssetDetail_PurchaseDate as PurchaseDate,AssetDetail_PolicyExpiryDate as PolicyDate,AssetDetail_PremiumDueDate as PremiumDate,AssetDetail_AMCExpiryDate as AMCExpiryDate", "AssetDetail_ID=" + id);
        if (dtCompany.Rows.Count > 0)
        {
            cmbFinYear.SelectedItem.Text = dtCompany.Rows[0]["FinYear"].ToString();
            CompanyID.Value = dtCompany.Rows[0]["CompanyID"].ToString();
            if (dtCompany.Rows[0]["PurchaseDate"] != DBNull.Value) PurchaseDate.Value = Convert.ToDateTime(dtCompany.Rows[0]["PurchaseDate"]);
            if (dtCompany.Rows[0]["AMCExpiryDate"] != DBNull.Value) ExpiryDate.Value = Convert.ToDateTime(dtCompany.Rows[0]["AMCExpiryDate"]);
            if (dtCompany.Rows[0]["PolicyDate"] != DBNull.Value) PolicyExpiryDate.Value = Convert.ToDateTime(dtCompany.Rows[0]["PolicyDate"]);
            if (dtCompany.Rows[0]["PremiumDate"] != DBNull.Value) PremiumDate.Value = Convert.ToDateTime(dtCompany.Rows[0]["PremiumDate"]);

        }
        TextBox txtInsurer = (TextBox)AssetDetailGrid.FindEditFormTemplateControl("txtInsurer");
        TextBox txtInsurerhidden = (TextBox)AssetDetailGrid.FindEditFormTemplateControl("txtInsurer_hidden");
        TextBox txtVendor = (TextBox)AssetDetailGrid.FindEditFormTemplateControl("txtVendor");
        TextBox txtVendorhidden = (TextBox)AssetDetailGrid.FindEditFormTemplateControl("txtVendor_hidden");
        TextBox txtServiceProvider = (TextBox)AssetDetailGrid.FindEditFormTemplateControl("txtServiceProvider");
        TextBox txtServiceProviderhidden = (TextBox)AssetDetailGrid.FindEditFormTemplateControl("txtServiceProvider_hidden");
        DataTable dtInsurer = dbEngine.GetDataTable("Master_AssetDetail M,tbl_master_contact C", "(C.cnt_firstName+IsNull(C.cnt_middleName,'')+C.cnt_lastName) as VendorName,M.AssetDetail_Vendor as VendorID", "M.AssetDetail_Vendor=C.cnt_internalId And M.AssetDetail_ID=" + id);
        {
            if (dtInsurer.Rows.Count > 0)
            {
                txtInsurer.Text = dtInsurer.Rows[0]["VendorName"].ToString();
                txtInsurerhidden.Text = dtInsurer.Rows[0]["VendorID"].ToString();
                txtVendor.Text = dtInsurer.Rows[0]["VendorName"].ToString();
                txtVendorhidden.Text = dtInsurer.Rows[0]["VendorID"].ToString();
                txtServiceProvider.Text = dtInsurer.Rows[0]["VendorName"].ToString();
                txtServiceProviderhidden.Text = dtInsurer.Rows[0]["VendorID"].ToString();
            }
        }

        TextBox txtUsedBy = (TextBox)AssetDetailGrid.FindEditFormTemplateControl("txtUsedBy");
        TextBox txtUsedByhidden = (TextBox)AssetDetailGrid.FindEditFormTemplateControl("txtUsedBy_hidden");
        DataTable dtUsedBy = dbEngine.GetDataTable("tbl_master_contact C,Master_AssetDetail M", "C.cnt_internalid as UserID,(C.cnt_firstName+C.cnt_lastName) as UsedBy,M.AssetDetail_User", "C.cnt_internalid=M.AssetDetail_User And M.AssetDetail_ID=" + id);
        {
            if (dtUsedBy.Rows.Count > 0)
            {
                txtUsedBy.Text = dtUsedBy.Rows[0]["UsedBy"].ToString();
                txtUsedByhidden.Text = dtUsedBy.Rows[0]["UserID"].ToString();
            }
        }
    }

		}
	}
