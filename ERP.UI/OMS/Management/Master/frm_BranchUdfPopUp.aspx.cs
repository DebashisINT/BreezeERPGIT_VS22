using System;
using System.Web;
using System.Web.UI;
//using DevExpress.Web.ASPxEditors;
//using DevExpress.Web.ASPxTabControl;
using DevExpress.Web;
using BusinessLogicLayer;
using EntityLayer.CommonELS;
using System.Data;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Collections.Generic;
using Newtonsoft.Json;
using Org.BouncyCastle.Math;
using AjaxControlToolkit;
using System.Linq;

namespace ERP.OMS.Management.Master
{
    public partial class management_frm_BranchUdfPopUp : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        string[,] FieldName;
        public string pageAccess = "";
        //DBEngine oDBEngine = new DBEngine(string.Empty);

        AjaxControlToolkit.TabContainer tabDynamic;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        BusinessLogicLayer.MasterDataCheckingBL DelClass = new BusinessLogicLayer.MasterDataCheckingBL();
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

            if (!IsPostBack)
            {
                sqlCategory.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                SqlRemarks.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                SetDateFormat();


            }


            if (Request.QueryString["Type"] != null)
                Session["KeyVal1"] = Request.QueryString["Type"];



            loadTab();

        }
        protected void btnSearch(object sender, EventArgs e)
        {
            //  GridRemarks.Settings.ShowFilterRow = true;
        }
        protected void GridRemarks_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "cat_id")
            {
                (e.Editor as ASPxComboBox).DataBound += new EventHandler(editCombo_DataBound);
            }
        }
        private void editCombo_DataBound(object sender, EventArgs e)
        {
            ListEditItem noneItem = new ListEditItem("None", null);
            (sender as ASPxComboBox).Items.Insert(0, noneItem);
        }
        protected void GridRemarks_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }





        protected void GridRemarks_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            //GridRemarks.SettingsText.PopupEditFormCaption = "Modify UDF";

        }

        protected void GridRemarks_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            // GridRemarks.SettingsText.PopupEditFormCaption = "Add UDF";
        }

        protected void GridRemarks_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (!rights.CanDelete)
            {
                if (e.ButtonType == ColumnCommandButtonType.Delete)
                {
                    e.Visible = false;
                }
            }


            if (!rights.CanEdit)
            {
                if (e.ButtonType == ColumnCommandButtonType.Edit)
                {
                    e.Visible = false;
                }
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            DataTable dt = oDBEngine.GetDataTable("tbl_master_remarksCategory", "id,cat_description,cat_field_type ,isMandatory", "cat_applicablefor='" + Convert.ToString(Request.QueryString["Type"]) + "'");


            //loop through tab
            foreach (TabPage page in tabControl.TabPages)
            {
                foreach (object ctrl in page.Controls)
                {
                    if (ctrl is TextBox)
                    {
                        string str = "";
                    }
                }

            }




            foreach (object ob in DynamicControlPanel.Controls)
            {
                if (ob is TextBox)
                {
                    TextBox tbnew = new TextBox();
                    tbnew = (TextBox)ob;
                    string RemarksData = tbnew.Text.Trim();
                    string cat_id = tbnew.ID.ToString().Replace("txt", "");
                    cat_id = cat_id.Replace("memo", "");
                    if (Request.QueryString["KeyVal_InternalID"] != null && Session["userid"] != null && RemarksData != "")
                    {
                        reCat.insertRemarkCategory(Convert.ToString(Request.QueryString["KeyVal_InternalID"]), Convert.ToInt32(cat_id), RemarksData, Convert.ToString(Session["userid"]));
                    }

                }
                if (ob is ASPxDateEdit)
                {
                    ASPxDateEdit datenew = new ASPxDateEdit();
                    datenew = (ASPxDateEdit)ob;
                    DateTime RemarksData = Convert.ToDateTime(datenew.Value);
                    string cat_id = Convert.ToString(datenew.ID).Replace("dt", "");
                    if (Request.QueryString["KeyVal_InternalID"] != null && Session["userid"] != null)
                    {
                        reCat.insertRemarkCategory(Convert.ToString(Request.QueryString["KeyVal_InternalID"]), Convert.ToInt32(cat_id), Convert.ToString(RemarksData), Convert.ToString(Session["userid"]));
                    }

                }

                if (ob is ASPxMemo)
                {
                    ASPxMemo MemoNew = new ASPxMemo();
                    MemoNew = (ASPxMemo)ob;
                    string RemarksData = MemoNew.Text.Trim();
                    string cat_id = Convert.ToString(MemoNew.ID).Replace("memo", "");
                    if (Request.QueryString["KeyVal_InternalID"] != null && Session["userid"] != null)
                    {
                        reCat.insertRemarkCategory(Convert.ToString(Request.QueryString["KeyVal_InternalID"]), Convert.ToInt32(cat_id), Convert.ToString(RemarksData), Convert.ToString(Session["userid"]));
                    }

                }

                if (ob is DropDownList)
                {
                    DropDownList dropDownNew = new DropDownList();
                    dropDownNew = (DropDownList)ob;
                    string RemarksData = Convert.ToString(dropDownNew.Text);
                    string cat_id = Convert.ToString(dropDownNew.ID).Replace("dd", "");
                    if (Request.QueryString["KeyVal_InternalID"] != null && Session["userid"] != null)
                    {
                        reCat.insertRemarkCategory(Convert.ToString(Request.QueryString["KeyVal_InternalID"]), Convert.ToInt32(cat_id), Convert.ToString(RemarksData), Convert.ToString(Session["userid"]));
                    }

                }

                if (ob is CheckBox)
                {
                    CheckBox chekNew = new CheckBox();
                    chekNew = (CheckBox)ob;
                    string RemarksData;
                    if (Convert.ToString(chekNew.ID).Contains("chk"))
                    {
                        if (chekNew.Checked)
                            RemarksData = "1";
                        else
                            RemarksData = "0";
                        string cat_id = Convert.ToString(chekNew.ID).Replace("chk", "");
                        if (Request.QueryString["KeyVal_InternalID"] != null && Session["userid"] != null)
                        {
                            reCat.insertRemarkCategory(Convert.ToString(Request.QueryString["KeyVal_InternalID"]), Convert.ToInt32(cat_id), Convert.ToString(RemarksData), Convert.ToString(Session["userid"]));
                        }
                    }

                }
                if (ob is RadioButtonList)
                {
                    RadioButtonList radioNew = new RadioButtonList();
                    radioNew = (RadioButtonList)ob;

                    string RemarksData = Convert.ToString(radioNew.Text);
                    string cat_id = Convert.ToString(radioNew.ID).Replace("rd", "");
                    if (Request.QueryString["KeyVal_InternalID"] != null && Session["userid"] != null)
                    {
                        reCat.insertRemarkCategory(Convert.ToString(Request.QueryString["KeyVal_InternalID"]), Convert.ToInt32(cat_id), Convert.ToString(RemarksData), Convert.ToString(Session["userid"]));
                    }
                }

            }
            Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "jAlert('Saved Successfully.');", true);
            Popup_Empcitys.ShowOnPageLoad = false;
            //GridRemarks.DataBind();
            //ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "Js1", " MakeInVisible();", true);

        }



        public void loadControls()
        {

            //DataTable dt = oDBEngine.GetDataTable("tbl_master_remarksCategory", "id,cat_description,cat_field_type,cat_max_length,isMandatory,cat_max_value,cat_max_date,cat_options ", "cat_applicablefor='Ld'");
            DataTable dt = oDBEngine.GetDataTable("tbl_master_remarksCategory", "id,cat_description,cat_field_type,cat_max_length,isMandatory,cat_max_value,cat_max_date,cat_options ", "cat_applicablefor='" + Convert.ToString(Request.QueryString["Type"]) + "'");
            foreach (DataRow dr in dt.Rows)
            {


                //div begin
                Label divlb = new Label();
                divlb.Text = "<div class='col-md-6 divControlClass'>";
                DynamicControlPanel.Controls.Add(divlb);


                #region fortextbox
                if (Convert.ToInt32(dr["cat_field_type"]) == 1)
                {




                    TextBox tb = new TextBox();
                    tb.ID = "txt" + Convert.ToString(dr["id"]).Trim();
                    tb.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    tb.MaxLength = Convert.ToInt32(dr["cat_max_length"]);
                    tb.Attributes.Add("style", "width: 270px; height: 28px;");
                    tb.Attributes.Add("class", "controlClass");
                    Label lb = new Label();
                    lb.Text = Convert.ToString(dr["cat_description"]);
                    lb.Attributes.Add("class", "controlClass");
                    lb.ID = "lbl" + Convert.ToString(dr["id"]).Trim();
                    DynamicControlPanel.Controls.Add(lb);
                    DynamicControlPanel.Controls.Add(tb);

                    if (Convert.ToBoolean(dr["isMandatory"]))
                    {
                        RequiredFieldValidator reqFieldVal = new RequiredFieldValidator();
                        reqFieldVal.ID = "validator_" + Convert.ToString(dr["id"]).Trim();
                        reqFieldVal.ControlToValidate = tb.ID;
                        reqFieldVal.SetFocusOnError = true;
                        reqFieldVal.ErrorMessage = "Required*";
                        reqFieldVal.EnableClientScript = false;
                        reqFieldVal.CssClass = "required";
                        reqFieldVal.Enabled = true;
                        reqFieldVal.EnableClientScript = true;
                        reqFieldVal.Attributes.Add("class", "controlClass");
                        reqFieldVal.ForeColor = Color.Red;

                        DynamicControlPanel.Controls.Add(reqFieldVal);
                    }


                }

                #endregion fortextbox

                #region forMemo
                if (Convert.ToInt32(dr["cat_field_type"]) == 2)
                {
                    // ASPxMemo memo = new ASPxMemo();
                    TextBox memo = new TextBox();
                    memo.ID = "memo" + Convert.ToString(dr["id"]).Trim();

                    //memo.ClientInstanceName = "cmemo" + Convert.ToString(dr["id"]).Trim();

                    memo.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    memo.MaxLength = Convert.ToInt32(dr["cat_max_length"]);
                    memo.Attributes.Add("style", "width: 270px; height: 55px;");
                    memo.Attributes.Add("Class", "controlClass");
                    memo.TextMode = TextBoxMode.MultiLine;
                    Label lb = new Label();
                    lb.Text = Convert.ToString(dr["cat_description"]);
                    lb.Attributes.Add("class", "controlClass");
                    lb.ID = "lbl" + Convert.ToString(dr["id"]).Trim();

                    DynamicControlPanel.Controls.Add(lb);
                    DynamicControlPanel.Controls.Add(memo);

                    if (Convert.ToBoolean(dr["isMandatory"]))
                    {
                        RequiredFieldValidator reqFieldVal = new RequiredFieldValidator();
                        reqFieldVal.ID = "validator_" + Convert.ToString(dr["id"]).Trim();
                        reqFieldVal.ControlToValidate = memo.ID;
                        reqFieldVal.SetFocusOnError = true;
                        reqFieldVal.ErrorMessage = "Required*";
                        reqFieldVal.EnableClientScript = false;
                        reqFieldVal.CssClass = "required";
                        reqFieldVal.Enabled = true;
                        reqFieldVal.EnableClientScript = true;
                        reqFieldVal.Attributes.Add("class", "controlClass");
                        reqFieldVal.ForeColor = Color.Red;

                        DynamicControlPanel.Controls.Add(reqFieldVal);
                    }


                }
                #endregion forMemo

                #region forNumber
                if (Convert.ToInt32(dr["cat_field_type"]) == 4)
                {
                    TextBox tb = new TextBox();
                    tb.ID = "txt" + Convert.ToString(dr["id"]).Trim();
                    tb.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    tb.MaxLength = 20;
                    tb.Attributes.Add("style", "width: 270px; height: 28px;");
                    tb.TextMode = TextBoxMode.Number;
                    tb.Attributes.Add("class", "controlClass");

                    Label lb = new Label();
                    lb.Text = Convert.ToString(dr["cat_description"]);
                    lb.Attributes.Add("class", "controlClass");
                    lb.ID = "lbl" + Convert.ToString(dr["id"]).Trim();

                    DynamicControlPanel.Controls.Add(lb);
                    DynamicControlPanel.Controls.Add(tb);




                    //debjyoti 21-12-2016
                    //if (Convert.ToInt32(dr["cat_max_value"]) != 0)
                    //{

                    //    RangeValidator rangeVal = new RangeValidator();
                    //    rangeVal.ID = "validator_maxVal_" + Convert.ToString(dr["id"]).Trim();
                    //    rangeVal.ControlToValidate = tb.ID;
                    //    rangeVal.SetFocusOnError = true;
                    //    rangeVal.ErrorMessage = "maximum value is " + Convert.ToString(dr["cat_max_value"]).Trim();
                    //    rangeVal.ForeColor = Color.Red;
                    //    rangeVal.EnableClientScript = false;
                    //    rangeVal.CssClass = "required";
                    //    rangeVal.Enabled = true;
                    //    rangeVal.EnableClientScript = true;
                    //    rangeVal.MaximumValue = Convert.ToString(dr["cat_max_value"]).Trim();
                    //    rangeVal.MinimumValue = "0";
                    //    rangeVal.Type = ValidationDataType.Integer;
                    //    rangeVal.Attributes.Add("class", "controlClass");

                    //    DynamicControlPanel.Controls.Add(rangeVal);
                    //}

                    if (Convert.ToBoolean(dr["isMandatory"]))
                    {
                        RequiredFieldValidator reqFieldVal = new RequiredFieldValidator();
                        reqFieldVal.ID = "validator_" + Convert.ToString(dr["id"]).Trim();
                        reqFieldVal.ControlToValidate = tb.ID;
                        reqFieldVal.SetFocusOnError = true;
                        reqFieldVal.ErrorMessage = "Required*";
                        reqFieldVal.EnableClientScript = false;
                        reqFieldVal.CssClass = "required";
                        reqFieldVal.Enabled = true;
                        reqFieldVal.EnableClientScript = true;
                        reqFieldVal.Attributes.Add("class", "controlClass");
                        reqFieldVal.ForeColor = Color.Red;

                        DynamicControlPanel.Controls.Add(reqFieldVal);
                    }



                }
                #endregion forNumber

                #region fordate
                if (Convert.ToInt32(dr["cat_field_type"]) == 3)
                {
                    ASPxDateEdit dateEdit = new ASPxDateEdit();
                    dateEdit.Attributes.Add("style", "width: 270px; height: 28px;");
                    dateEdit.ID = "dt" + Convert.ToString(dr["id"]).Trim();
                    dateEdit.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;

                    dateEdit.TimeSectionProperties.Visible = false;
                    dateEdit.UseMaskBehavior = true;
                    dateEdit.EditFormatString = "dd-MM-yyyy";
                    dateEdit.DisplayFormatString = "dd-MM-yyyy";
                    dateEdit.Date = System.DateTime.Now;
                    dateEdit.Attributes.Add("class", "controlClass");

                    dateEdit.ValidateRequestMode = System.Web.UI.ValidateRequestMode.Enabled;
                    // dateEdit.NullText = "dd-MM-yyyy";
                    // dateEdit.AllowNull=Convert.ToBoolean(dr["isMandatory"]);
                    dateEdit.MinDate = new DateTime(1900, 1, 1);
                    if (Convert.ToDateTime(dr["cat_max_date"]) != new DateTime(1900, 1, 1))
                    {
                        dateEdit.MaxDate = Convert.ToDateTime(dr["cat_max_date"]);
                    }

                    Label lb = new Label();
                    lb.Text = Convert.ToString(dr["cat_description"]);
                    lb.Attributes.Add("class", "controlClass");
                    lb.ID = "lbl" + Convert.ToString(dr["id"]).Trim();

                    DynamicControlPanel.Controls.Add(lb);
                    DynamicControlPanel.Controls.Add(dateEdit);
                }

                #endregion fordate

                #region forEmail
                if (Convert.ToInt32(dr["cat_field_type"]) == 5)
                {
                    TextBox tb = new TextBox();
                    tb.ID = "txt" + Convert.ToString(dr["id"]).Trim();
                    tb.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    tb.MaxLength = Convert.ToInt32(dr["cat_max_length"]);
                    tb.Attributes.Add("style", "width: 270px; height: 28px;");
                    tb.TextMode = TextBoxMode.Email;
                    tb.Attributes.Add("class", "controlClass");


                    Label lb = new Label();
                    lb.Text = Convert.ToString(dr["cat_description"]);
                    lb.Attributes.Add("class", "controlClass");
                    lb.ID = "lbl" + Convert.ToString(dr["id"]).Trim();

                    DynamicControlPanel.Controls.Add(lb);
                    DynamicControlPanel.Controls.Add(tb);

                    if (Convert.ToBoolean(dr["isMandatory"]))
                    {
                        RequiredFieldValidator reqFieldVal = new RequiredFieldValidator();
                        reqFieldVal.ID = "validator_" + Convert.ToString(dr["id"]).Trim();
                        reqFieldVal.ControlToValidate = tb.ID;
                        reqFieldVal.SetFocusOnError = true;
                        reqFieldVal.ErrorMessage = "Required*";
                        reqFieldVal.EnableClientScript = false;
                        reqFieldVal.CssClass = "required";
                        reqFieldVal.Enabled = true;
                        reqFieldVal.EnableClientScript = true;
                        reqFieldVal.ForeColor = Color.Red;
                        DynamicControlPanel.Controls.Add(reqFieldVal);
                    }


                }
                #endregion forEmail

                #region combo
                if (Convert.ToInt32(dr["cat_field_type"]) == 6)
                {
                    DropDownList dd = new DropDownList();
                    dd.ID = "dd" + Convert.ToString(dr["id"]).Trim();
                    dd.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    dd.Attributes.Add("style", "width: 270px; height: 28px;");
                    dd.Attributes.Add("class", "controlClass");
                    string[] options = Convert.ToString(dr["cat_options"]).Split(',');
                    foreach (string op in options)
                    {
                        dd.Items.Add(op);
                    }

                    Label lb = new Label();
                    lb.Text = Convert.ToString(dr["cat_description"]);
                    lb.Attributes.Add("class", "controlClass");
                    lb.ID = "lbl" + Convert.ToString(dr["id"]).Trim();

                    DynamicControlPanel.Controls.Add(lb);
                    DynamicControlPanel.Controls.Add(dd);

                    if (Convert.ToBoolean(dr["isMandatory"]))
                    {
                        RequiredFieldValidator reqFieldVal = new RequiredFieldValidator();
                        reqFieldVal.ID = "validator_" + Convert.ToString(dr["id"]).Trim();
                        reqFieldVal.ControlToValidate = dd.ID;
                        reqFieldVal.SetFocusOnError = true;
                        reqFieldVal.ErrorMessage = "Required*";
                        reqFieldVal.EnableClientScript = false;
                        reqFieldVal.CssClass = "required";
                        reqFieldVal.Enabled = true;
                        reqFieldVal.EnableClientScript = true;
                        reqFieldVal.ForeColor = Color.Red;
                        DynamicControlPanel.Controls.Add(reqFieldVal);
                    }


                }
                #endregion combo

                #region checkBox
                if (Convert.ToInt32(dr["cat_field_type"]) == 7)
                {
                    CheckBox chk = new CheckBox();
                    chk.ID = "chk" + Convert.ToString(dr["id"]).Trim();
                    chk.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    chk.Attributes.Add("class", "controlClass");

                    Label lb = new Label();
                    lb.Text = Convert.ToString(dr["cat_description"]);
                    lb.Attributes.Add("class", "controlClass");
                    lb.ID = "lbl" + Convert.ToString(dr["id"]).Trim();

                    DynamicControlPanel.Controls.Add(lb);
                    DynamicControlPanel.Controls.Add(chk);
                }
                #endregion checkBox

                #region RadioButton
                if (Convert.ToInt32(dr["cat_field_type"]) == 8)
                {
                    Label lb = new Label();
                    lb.Text = Convert.ToString(dr["cat_description"]);
                    lb.Attributes.Add("class", "controlClass");
                    lb.ID = "lbl" + Convert.ToString(dr["id"]).Trim();

                    DynamicControlPanel.Controls.Add(lb);

                    RadioButtonList rdBttn = new RadioButtonList();
                    rdBttn.ID = "rd" + Convert.ToString(dr["id"]).Trim();
                    rdBttn.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    rdBttn.Attributes.Add("class", "controlClass");

                    string[] options = Convert.ToString(dr["cat_options"]).Split(',');
                    foreach (string op in options)
                    {
                        if (op.Trim() != "")
                        {
                            rdBttn.Items.Add(op);
                        }
                    }
                    DynamicControlPanel.Controls.Add(rdBttn);

                    if (Convert.ToBoolean(dr["isMandatory"]))
                    {
                        RequiredFieldValidator reqFieldVal = new RequiredFieldValidator();
                        reqFieldVal.ID = "validator_" + Convert.ToString(dr["id"]).Trim();
                        reqFieldVal.ControlToValidate = rdBttn.ID;
                        reqFieldVal.SetFocusOnError = true;
                        reqFieldVal.ErrorMessage = "Required*";
                        reqFieldVal.EnableClientScript = false;
                        reqFieldVal.CssClass = "required";
                        reqFieldVal.Enabled = true;
                        reqFieldVal.EnableClientScript = true;
                        reqFieldVal.ForeColor = Color.Red;
                        DynamicControlPanel.Controls.Add(reqFieldVal);
                    }

                }
                #endregion RadioButton

                //div end
                divlb = new Label();
                divlb.Text = "</div>";
                DynamicControlPanel.Controls.Add(divlb);
            }




        }
        public void loadTab()
        {
            TabPage newTab, tempTab;

            AllControslId.Value = "";

            DataTable existingDtinAdd = (DataTable)Session["UdfDataOnAdd"];

            //GetAllSavedData
            DataTable dtRecord = oDBEngine.GetDataTable("tbl_master_contactRemarks", "id,cat_id,rea_Remarks ", "rea_internalId='" + Convert.ToString(Request.QueryString["KeyVal_InternalID"]) + "'");
            DataColumn[] keys = new DataColumn[1];
            keys[0] = dtRecord.Columns["cat_id"];
            dtRecord.PrimaryKey = keys;
            ///For Tab control ... tab added here from udf group
            DataTable dtTab = oDBEngine.GetDataTable("tbl_master_udfGroup", " id,grp_description ", "grp_applicablefor='" + Convert.ToString(Request.QueryString["Type"]) + "' and grp_isVisible=1");
            foreach (DataRow drtab in dtTab.Rows)
            {
                newTab = new TabPage(Convert.ToString(drtab["grp_description"]), Convert.ToString(drtab["id"]));
                tabControl.TabPages.Add(newTab);
            }

            //debjyoti 21-12-2016
            dtTab = oDBEngine.GetDataTable("tbl_master_remarksCategory", "1", "cat_applicablefor='" + Convert.ToString(Request.QueryString["Type"]) + "' and cat_group_id=0");
            if (dtTab.Rows.Count > 0)
            {
                newTab = new TabPage("No Group", "0");
                tabControl.TabPages.Add(newTab);
            }

            // DataTable dt = oDBEngine.GetDataTable("tbl_master_remarksCategory", "id,cat_description,cat_field_type,cat_max_length,isMandatory,cat_max_value,cat_max_date,cat_options,cat_group_id ", "cat_applicablefor='" + Convert.ToString(Session["KeyVal1"]) + "'  and (select grp_isvisible from tbl_master_udfGroup where id= cat_group_id)=1");
            DataTable dt = oDBEngine.GetDataTable("tbl_master_remarksCategory", "id,cat_description,cat_field_type,cat_max_length,isMandatory,cat_max_value,cat_max_date,cat_options,cat_group_id ", "cat_applicablefor='" + Convert.ToString(Request.QueryString["Type"]) + "'  and ((select grp_isvisible from tbl_master_udfGroup where id= cat_group_id)=1 or cat_group_id=0)", "orderBy");
            foreach (DataRow dr in dt.Rows)
            {


                //div begin
                Label divlb = new Label();
                divlb.Text = "<div class='col-md-6 divControlClass'>";


                tempTab = tabControl.TabPages.FindByName(Convert.ToString(dr["cat_group_id"]));



                tempTab.Controls.Add(divlb);

                //Id Added to hiddenField
                if (Convert.ToString(AllControslId.Value).Trim() == "")
                    AllControslId.Value = Convert.ToString(dr["id"]).Trim() + "/" + Convert.ToString(dr["cat_field_type"]);
                else
                    AllControslId.Value += "~" + Convert.ToString(dr["id"]).Trim() + "/" + Convert.ToString(dr["cat_field_type"]);

                #region fortextbox
                if (Convert.ToInt32(dr["cat_field_type"]) == 1)
                {
                    TextBox tb = new TextBox();
                    tb.ID = "txt" + Convert.ToString(dr["id"]).Trim();
                    tb.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    tb.MaxLength = Convert.ToInt32(dr["cat_max_length"]);
                    tb.Attributes.Add("style", "width: 100%; height: 28px;");
                    tb.Attributes.Add("class", "controlClass");

                    //Set data if present
                    DataRow exsistsvalue = dtRecord.Rows.Find(dr["id"]);

                    if (Request.QueryString["keyval_internalId"] == "Add")
                    {
                        if (existingDtinAdd != null)
                        {
                            DataRow[] addRowData = existingDtinAdd.Select("cat_id=" + Convert.ToString(dr["id"]));
                            if (addRowData.Length > 0)
                            {
                                tb.Text = Convert.ToString(addRowData[0]["RemarksData"]);
                            }
                        }
                    }
                    else
                    {
                        if (exsistsvalue != null)
                        {
                            tb.Text = Convert.ToString(exsistsvalue["rea_Remarks"]);
                        }
                    }

                    Label lb = new Label();
                    lb.Text = Convert.ToString(dr["cat_description"]);
                    lb.Attributes.Add("class", "controlClass");
                    lb.ID = "lbl" + Convert.ToString(dr["id"]).Trim();
                    tempTab.Controls.Add(lb);
                    tempTab.Controls.Add(tb);

                    if (Convert.ToBoolean(dr["isMandatory"]))
                    {
                        Label span = new Label();
                        span.Text = " <span id='Mandatory" + Convert.ToString(dr["id"]).Trim() + "' class='pullleftClass fa fa-exclamation-circle iconRed' style='color:red;display:none; position: absolute; right: -8px; top: 19px;' title='Mandatory'></span>";
                        tempTab.Controls.Add(span);
                    }


                }

                #endregion fortextbox

                #region forMemo
                else if (Convert.ToInt32(dr["cat_field_type"]) == 2)
                {
                    // ASPxMemo memo = new ASPxMemo();
                    TextBox memo = new TextBox();
                    memo.ID = "memo" + Convert.ToString(dr["id"]).Trim();

                    //memo.ClientInstanceName = "cmemo" + Convert.ToString(dr["id"]).Trim();

                    memo.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    memo.MaxLength = Convert.ToInt32(dr["cat_max_length"]);
                    memo.Attributes.Add("style", "width:100%; height: 55px;");
                    memo.Attributes.Add("Class", "controlClass");
                    memo.TextMode = TextBoxMode.MultiLine;

                    //debjyoti 22-12-2016
                    if (Request.QueryString["keyval_internalId"] == "Add")
                    {
                        if (existingDtinAdd != null)
                        {
                            DataRow[] addRowData = existingDtinAdd.Select("cat_id=" + Convert.ToString(dr["id"]));
                            if (addRowData.Length > 0)
                            {
                                memo.Text = Convert.ToString(addRowData[0]["RemarksData"]);
                            }
                        }
                    }
                    else
                    {
                        DataRow exsistsvalue = dtRecord.Rows.Find(dr["id"]);
                        if (exsistsvalue != null)
                        {
                            memo.Text = Convert.ToString(exsistsvalue["rea_Remarks"]);
                        }
                    }
                    Label lb = new Label();
                    lb.Text = Convert.ToString(dr["cat_description"]);
                    lb.Attributes.Add("class", "controlClass");
                    lb.ID = "lbl" + Convert.ToString(dr["id"]).Trim();

                    tempTab.Controls.Add(lb);
                    tempTab.Controls.Add(memo);

                    if (Convert.ToBoolean(dr["isMandatory"]))
                    {
                        Label span = new Label();
                        span.Text = " <span id='Mandatory" + Convert.ToString(dr["id"]).Trim() + "' class='pullleftClass fa fa-exclamation-circle iconRed' style='color:red;display:none; position: absolute; right: -8px; top: 19px;' title='Mandatory'></span>";
                        tempTab.Controls.Add(span);
                    }


                }
                #endregion forMemo

                #region forNumber
                else if (Convert.ToInt32(dr["cat_field_type"]) == 4)
                {
                    TextBox tb = new TextBox();
                    tb.ID = "txt" + Convert.ToString(dr["id"]).Trim();
                    tb.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    tb.MaxLength = 20;
                    tb.Attributes.Add("style", "width: 100%; height: 28px;");
                    tb.TextMode = TextBoxMode.Number;
                    tb.Attributes.Add("class", "controlClass");

                    //debjyoti 22-12-2016
                    if (Request.QueryString["keyval_internalId"] == "Add")
                    {
                        if (existingDtinAdd != null)
                        {
                            DataRow[] addRowData = existingDtinAdd.Select("cat_id=" + Convert.ToString(dr["id"]));
                            if (addRowData.Length > 0)
                            {
                                tb.Text = Convert.ToString(addRowData[0]["RemarksData"]);
                            }
                        }
                    }
                    else
                    {
                        DataRow exsistsvalue = dtRecord.Rows.Find(dr["id"]);
                        if (exsistsvalue != null)
                        {
                            tb.Text = Convert.ToString(exsistsvalue["rea_Remarks"]);
                        }
                    }
                    Label lb = new Label();
                    lb.Text = Convert.ToString(dr["cat_description"]);
                    lb.Attributes.Add("class", "controlClass");
                    lb.ID = "lbl" + Convert.ToString(dr["id"]).Trim();

                    tempTab.Controls.Add(lb);
                    tempTab.Controls.Add(tb);




                    //debjyoti 21-12-2016
                    //if (Convert.ToInt32(dr["cat_max_value"]) != 0)
                    //{

                    //    RangeValidator rangeVal = new RangeValidator();
                    //    rangeVal.ID = "validator_maxVal_" + Convert.ToString(dr["id"]).Trim();
                    //    rangeVal.ControlToValidate = tb.ID;
                    //    rangeVal.SetFocusOnError = true;
                    //    rangeVal.ErrorMessage = "maximum value is " + Convert.ToString(dr["cat_max_value"]).Trim();
                    //    rangeVal.ForeColor = Color.Red;
                    //    rangeVal.EnableClientScript = false;
                    //    rangeVal.CssClass = "required";
                    //    rangeVal.Enabled = true;
                    //    rangeVal.EnableClientScript = true;
                    //    rangeVal.MaximumValue = Convert.ToString(dr["cat_max_value"]).Trim();
                    //    rangeVal.MinimumValue = "0";
                    //    rangeVal.Type = ValidationDataType.Integer;
                    //    rangeVal.Attributes.Add("class", "controlClass");

                    //    tempTab.Controls.Add(rangeVal);
                    //}

                    if (Convert.ToBoolean(dr["isMandatory"]))
                    {
                        //RequiredFieldValidator reqFieldVal = new RequiredFieldValidator();
                        //reqFieldVal.ID = "validator_" + Convert.ToString(dr["id"]).Trim();
                        //reqFieldVal.ControlToValidate = tb.ID;
                        //reqFieldVal.SetFocusOnError = true;
                        //reqFieldVal.ErrorMessage = "Required*";
                        //reqFieldVal.EnableClientScript = false;
                        //reqFieldVal.CssClass = "required";
                        //reqFieldVal.Enabled = true;
                        //reqFieldVal.EnableClientScript = true;
                        //reqFieldVal.Attributes.Add("class", "controlClass");
                        //reqFieldVal.ForeColor = Color.Red;

                        //tempTab.Controls.Add(reqFieldVal);

                        Label span = new Label();
                        span.Text = " <span id='Mandatory" + Convert.ToString(dr["id"]).Trim() + "' class='pullleftClass fa fa-exclamation-circle iconRed' style='color:red;display:none; position: absolute; right: -8px; top: 19px;' title='Mandatory'></span>";
                        tempTab.Controls.Add(span);
                    }



                }
                #endregion forNumber

                #region fordate
                else if (Convert.ToInt32(dr["cat_field_type"]) == 3)
                {
                    ASPxDateEdit dateEdit = new ASPxDateEdit();
                    dateEdit.Attributes.Add("style", "width: 100%; height: 28px;");
                    dateEdit.ID = "dt" + Convert.ToString(dr["id"]).Trim();
                    dateEdit.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;

                    dateEdit.TimeSectionProperties.Visible = false;
                    dateEdit.UseMaskBehavior = true;
                    dateEdit.EditFormatString = "dd-MM-yyyy";
                    dateEdit.DisplayFormatString = "dd-MM-yyyy";
                    // dateEdit.Date = System.DateTime.Now;
                    dateEdit.Attributes.Add("class", "controlClass");

                    //debjyoti 22-12-2016
                    if (Request.QueryString["keyval_internalId"] == "Add")
                    {
                        if (existingDtinAdd != null)
                        {
                            DataRow[] addRowData = existingDtinAdd.Select("cat_id=" + Convert.ToString(dr["id"]));
                            if (addRowData.Length > 0)
                            {
                                string dateadd = Convert.ToString(addRowData[0]["RemarksData"]);
                                if (dateadd.Trim() != "")
                                {
                                    string[] dateSplit = dateadd.Split('/');
                                    int month = Convert.ToInt32(dateSplit[0]);
                                    int day = Convert.ToInt32(dateSplit[1]);
                                    int year = Convert.ToInt32(Convert.ToString(dateSplit[2]).Substring(0, 4));

                                    dateEdit.Date = new DateTime(year, month, day);
                                }
                            }
                        }
                    }
                    else
                    {
                        DataRow exsistsvalue = dtRecord.Rows.Find(dr["id"]);
                        if (exsistsvalue != null)
                        {
                            string date = Convert.ToString(exsistsvalue["rea_Remarks"]);
                            if (date.Trim() != "")
                            {
                                string[] dateSplit = date.Split('/');
                                int month = Convert.ToInt32(dateSplit[0]);
                                int day = Convert.ToInt32(dateSplit[1]);
                                int year = Convert.ToInt32(Convert.ToString(dateSplit[2]).Substring(0, 4));

                                dateEdit.Date = new DateTime(year, month, day);
                            }
                        }
                    }


                    Label lb = new Label();
                    lb.Text = Convert.ToString(dr["cat_description"]);
                    lb.Attributes.Add("class", "controlClass");
                    lb.ID = "lbl" + Convert.ToString(dr["id"]).Trim();

                    tempTab.Controls.Add(lb);


                    ////temp
                    //DateTime temp = new DateTime(1993, 10, 20);
                    //dateEdit.Date = temp;
                    ////temp

                    tempTab.Controls.Add(dateEdit);
                    if (Convert.ToBoolean(dr["isMandatory"]))
                    {
                        Label span = new Label();
                        span.Text = " <span id='Mandatory" + Convert.ToString(dr["id"]).Trim() + "' class='pullleftClass fa fa-exclamation-circle iconRed' style='color:red;display:none; position: absolute; right: -8px; top: 19px;' title='Mandatory'></span>";
                        tempTab.Controls.Add(span);
                    }
                }

                #endregion fordate

                #region forEmail
                else if (Convert.ToInt32(dr["cat_field_type"]) == 5)
                {
                    TextBox tb = new TextBox();
                    tb.ID = "txt" + Convert.ToString(dr["id"]).Trim();
                    tb.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    tb.MaxLength = Convert.ToInt32(dr["cat_max_length"]);
                    tb.Attributes.Add("style", "width: 100%; height: 28px;");
                    tb.TextMode = TextBoxMode.Email;
                    tb.Attributes.Add("class", "controlClass");

                    //Set data if present
                    if (Request.QueryString["keyval_internalId"] == "Add")
                    {
                        if (existingDtinAdd != null)
                        {
                            DataRow[] addRowData = existingDtinAdd.Select("cat_id=" + Convert.ToString(dr["id"]));
                            if (addRowData.Length > 0)
                            {
                                tb.Text = Convert.ToString(addRowData[0]["RemarksData"]);
                            }
                        }
                    }
                    else
                    {
                        DataRow exsistsvalue = dtRecord.Rows.Find(dr["id"]);
                        if (exsistsvalue != null)
                        {
                            tb.Text = Convert.ToString(exsistsvalue["rea_Remarks"]);
                        }
                    }
                    Label lb = new Label();
                    lb.Text = Convert.ToString(dr["cat_description"]);
                    lb.Attributes.Add("class", "controlClass");
                    lb.ID = "lbl" + Convert.ToString(dr["id"]).Trim();

                    tempTab.Controls.Add(lb);
                    tempTab.Controls.Add(tb);

                    if (Convert.ToBoolean(dr["isMandatory"]))
                    {
                        Label span = new Label();
                        span.Text = " <span id='Mandatory" + Convert.ToString(dr["id"]).Trim() + "' class='pullleftClass fa fa-exclamation-circle iconRed' style='color:red;display:none; position: absolute; right: -8px; top: 19px;' title='Mandatory'></span>";
                        tempTab.Controls.Add(span);
                    }


                }
                #endregion forEmail

                #region combo
                else if (Convert.ToInt32(dr["cat_field_type"]) == 6)
                {
                    DropDownList dd = new DropDownList();
                    dd.ID = "dd" + Convert.ToString(dr["id"]).Trim();
                    dd.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    dd.Attributes.Add("style", "width: 100%; height: 28px;");
                    dd.Attributes.Add("class", "controlClass");
                    string[] options = Convert.ToString(dr["cat_options"]).Split(',');
                    foreach (string op in options)
                    {
                        dd.Items.Add(op);
                    }

                    //Set data if present
                    if (Request.QueryString["keyval_internalId"] == "Add")
                    {
                        if (existingDtinAdd != null)
                        {
                            DataRow[] addRowData = existingDtinAdd.Select("cat_id=" + Convert.ToString(dr["id"]));
                            if (addRowData.Length > 0)
                            {
                                string combodata = Convert.ToString(addRowData[0]["RemarksData"]);
                                int selectedIndex = -1;
                                foreach (var str in dd.Items)
                                {
                                    selectedIndex += 1;
                                    if (Convert.ToString(dd.Items[selectedIndex].Value) == Convert.ToString(combodata).Trim())
                                        break;
                                }
                                dd.SelectedIndex = selectedIndex;
                            }
                        }
                    }
                    else
                    {


                        DataRow exsistsvalue = dtRecord.Rows.Find(dr["id"]);
                        if (exsistsvalue != null)
                        {
                            int selectedIndex = -1;
                            foreach (var str in dd.Items)
                            {
                                selectedIndex += 1;
                                if (Convert.ToString(dd.Items[selectedIndex].Value) == Convert.ToString(exsistsvalue["rea_Remarks"]).Trim())
                                    break;
                            }
                            dd.SelectedIndex = selectedIndex;
                        }
                    }
                    Label lb = new Label();
                    lb.Text = Convert.ToString(dr["cat_description"]);
                    lb.Attributes.Add("class", "controlClass");
                    lb.ID = "lbl" + Convert.ToString(dr["id"]).Trim();

                    tempTab.Controls.Add(lb);
                    tempTab.Controls.Add(dd);

                    if (Convert.ToBoolean(dr["isMandatory"]))
                    {

                        Label span = new Label();
                        span.Text = " <span id='Mandatory" + Convert.ToString(dr["id"]).Trim() + "' class='pullleftClass fa fa-exclamation-circle iconRed' style='color:red;display:none; position: absolute; right: -8px; top: 19px;' title='Mandatory'></span>";
                        tempTab.Controls.Add(span);
                    }


                }
                #endregion combo

                #region checkBox
                //if (Convert.ToInt32(dr["cat_field_type"]) == 7)
                //{
                //    CheckBox chk = new CheckBox();
                //    chk.ID = "chk" + Convert.ToString(dr["id"]).Trim();
                //    chk.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                //    chk.Attributes.Add("class", "controlClass");

               //    //Set data if present
                //    DataRow exsistsvalue = dtRecord.Rows.Find(dr["id"]);
                //    if (exsistsvalue != null)
                //    {
                //        chk.Checked = Convert.ToBoolean(exsistsvalue["rea_Remarks"]);
                //    }

               //    Label lb = new Label();
                //    lb.Text = Convert.ToString(dr["cat_description"]);
                //    lb.Attributes.Add("class", "controlClass");
                //    lb.ID = "lbl" + Convert.ToString(dr["id"]).Trim();

               //    tempTab.Controls.Add(lb);
                //    tempTab.Controls.Add(chk);
                //}



                else if (Convert.ToInt32(dr["cat_field_type"]) == 7)
                {
                    CheckBoxList chk = new CheckBoxList();
                    chk.ID = "chk" + Convert.ToString(dr["id"]).Trim();
                    chk.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    chk.Attributes.Add("class", "controlClass");

                    string[] options = Convert.ToString(dr["cat_options"]).Split(',');
                    foreach (string op in options)
                    {
                        chk.Items.Add(op);
                    }

                    //set Data value
                    if (Request.QueryString["keyval_internalId"] == "Add")
                    {
                        if (existingDtinAdd != null)
                        {
                            DataRow[] addRowData = existingDtinAdd.Select("cat_id=" + Convert.ToString(dr["id"]));
                            if (addRowData.Length > 0)
                            {
                                string remarkdata = Convert.ToString(addRowData[0]["RemarksData"]);
                                string[] savedItem = remarkdata.Split('~');
                                foreach (string itm in savedItem)
                                {
                                    if (chk.Items.FindByText(itm) != null)
                                    chk.Items.FindByText(itm).Selected = true;
                                }

                            }
                        }
                    }
                    else
                    {


                        DataRow exsistsvalue = dtRecord.Rows.Find(dr["id"]);
                        if (exsistsvalue != null)
                        {
                            string[] savedItem = Convert.ToString(exsistsvalue["rea_Remarks"]).Split('~');
                            foreach (string itm in savedItem)
                            {
                                if (chk.Items.FindByText(itm) != null)
                                  chk.Items.FindByText(itm).Selected = true;
                            }

                        }

                    }

                    Label lb = new Label();
                    lb.Text = Convert.ToString(dr["cat_description"]);
                    lb.Attributes.Add("class", "controlClass");
                    lb.ID = "lbl" + Convert.ToString(dr["id"]).Trim();

                    tempTab.Controls.Add(lb);
                    tempTab.Controls.Add(chk);
                }
                #endregion checkBox

                #region RadioButton
                else if (Convert.ToInt32(dr["cat_field_type"]) == 8)
                {
                    Label lb = new Label();
                    lb.Text = Convert.ToString(dr["cat_description"]);
                    lb.Attributes.Add("class", "controlClass");
                    lb.ID = "lbl" + Convert.ToString(dr["id"]).Trim();

                    tempTab.Controls.Add(lb);
                    if (Convert.ToBoolean(dr["isMandatory"]))
                    {
                        Label span = new Label();
                        span.Text = " <span id='Mandatory" + Convert.ToString(dr["id"]).Trim() + "' class='pullleftClass fa fa-exclamation-circle iconRed' style='color:red;display:none; position: absolute; right: -8px; top: 19px;' title='Mandatory'></span>";
                        tempTab.Controls.Add(span);
                    }


                    RadioButtonList rdBttn = new RadioButtonList();
                    rdBttn.ID = "rd" + Convert.ToString(dr["id"]).Trim();
                    rdBttn.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    rdBttn.Attributes.Add("class", "controlClass");

                    string[] options = Convert.ToString(dr["cat_options"]).Split(',');
                    foreach (string op in options)
                    {
                        if (op.Trim() != "")
                        {
                            rdBttn.Items.Add(op);
                        }
                    }

                    //Set data if present
                    if (Request.QueryString["keyval_internalId"] == "Add")
                    {
                        if (existingDtinAdd != null)
                        {
                            DataRow[] addRowData = existingDtinAdd.Select("cat_id=" + Convert.ToString(dr["id"]));
                            if (addRowData.Length > 0)
                            {
                                string remarksdata = Convert.ToString(addRowData[0]["RemarksData"]);

                                int selectedIndex = -1;
                                foreach (var str in rdBttn.Items)
                                {
                                    selectedIndex += 1;
                                    if (rdBttn.Items[selectedIndex].Value == remarksdata.Trim())
                                        break;
                                }
                                rdBttn.SelectedIndex = selectedIndex;
                            }
                        }
                    }
                    else
                    {
                        DataRow exsistsvalue = dtRecord.Rows.Find(dr["id"]);
                        if (exsistsvalue != null)
                        {
                            int selectedIndex = -1;
                            foreach (var itm in rdBttn.Items)
                            {
                                
                                selectedIndex += 1;
                                if (rdBttn.Items[selectedIndex].Value == Convert.ToString(exsistsvalue["rea_Remarks"]).Trim())
                                    break;
                            }
                            rdBttn.SelectedIndex = selectedIndex;
                        }
                    }

                    tempTab.Controls.Add(rdBttn);


                }
                #endregion RadioButton

                #region ListBox

                else if (Convert.ToInt32(dr["cat_field_type"]) == 9)
                {
                    ListBox lst = new ListBox();
                    lst.ID = "lstcsn" + Convert.ToString(dr["id"]).Trim();
                    lst.SelectionMode = ListSelectionMode.Multiple;
                    //memo.ClientInstanceName = "cmemo" + Convert.ToString(dr["id"]).Trim();

                    lst.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    lst.Attributes.Add("style", "width:100%; height: 55px;");
                    lst.CssClass = "chsn";
                    string[] options = Convert.ToString(dr["cat_options"]).Split(',');
                    foreach (string op in options)
                    {
                        lst.Items.Add(op);
                    }


                    Label lb = new Label();
                    lb.Text = Convert.ToString(dr["cat_description"]);
                    lb.Attributes.Add("class", "controlClass");
                    lb.ID = "lbl" + Convert.ToString(dr["id"]).Trim();

                    tempTab.Controls.Add(lb);
                    tempTab.Controls.Add(lst);

                    if (Convert.ToBoolean(dr["isMandatory"]))
                    {
                        Label span = new Label();
                        span.Text = " <span id='Mandatory" + Convert.ToString(dr["id"]).Trim() + "' class='pullleftClass fa fa-exclamation-circle iconRed' style='color:red;display:none; position: absolute; right: -8px; top: 19px;' title='Mandatory'></span>";
                        tempTab.Controls.Add(span);
                    }


                }

                #endregion ListBox

                //div end
                divlb = new Label();
                divlb.Text = "</div>";
                tempTab.Controls.Add(divlb);
            }

        }

        public void SetDateFormat()
        {
            dtEditDate.TimeSectionProperties.Visible = false;
            dtEditDate.UseMaskBehavior = true;
            dtEditDate.EditFormatString = "dd-MM-yyyy";
            dtEditDate.DisplayFormatString = "dd-MM-yyyy";

        }

        public bool isValid()
        {
            Boolean retval = true;
            DataRow dr;
            DataTable dt;
            tabControl.JSProperties["cpMandatory"] = "";
            foreach (TabPage page in tabControl.TabPages)
            {
                if (page.IsActive)
                {
                    foreach (object ob in page.Controls)
                    {
                        if (ob is TextBox)
                        {
                            TextBox tbnew = new TextBox();
                            tbnew = (TextBox)ob;
                            string RemarksData = tbnew.Text.Trim();
                            string cat_id = tbnew.ID.ToString().Replace("txt", "");
                            cat_id = cat_id.Replace("memo", "");
                            dt = oDBEngine.GetDataTable("select isMandatory,cat_max_value,cat_field_type  from tbl_master_remarksCategory where id=" + cat_id);
                            if (dt.Rows.Count > 0)
                            {
                                dr = dt.Rows[0];
                                if (Convert.ToBoolean(dr["isMandatory"]) && RemarksData == "")
                                {
                                    if (Convert.ToString(tabControl.JSProperties["cpMandatory"]).Trim() == "")
                                        tabControl.JSProperties["cpMandatory"] = cat_id;
                                    else
                                        tabControl.JSProperties["cpMandatory"] = Convert.ToString(tabControl.JSProperties["cpMandatory"]) + "~" + cat_id;
                                    retval = false;
                                    continue;
                                }

                            }
                        }
                        if (ob is ASPxDateEdit)
                        {
                            ASPxDateEdit datenew = new ASPxDateEdit();
                            datenew = (ASPxDateEdit)ob;
                            string cat_id = Convert.ToString(datenew.ID).Replace("dt", "");
                            dt = oDBEngine.GetDataTable("select isMandatory,cat_max_value  from tbl_master_remarksCategory where id=" + cat_id);
                            if (dt.Rows.Count > 0)
                            {
                                dr = dt.Rows[0];
                                if (Convert.ToBoolean(dr["isMandatory"]) && datenew.Value == null)
                                {
                                    if (Convert.ToString(tabControl.JSProperties["cpMandatory"]).Trim() == "")
                                        tabControl.JSProperties["cpMandatory"] = cat_id;
                                    else
                                        tabControl.JSProperties["cpMandatory"] = Convert.ToString(tabControl.JSProperties["cpMandatory"]) + "~" + cat_id;
                                    retval = false;
                                    continue;
                                }
                            }
                        }
                        if (ob is DropDownList)
                        {
                            DropDownList dropDownNew = new DropDownList();
                            dropDownNew = (DropDownList)ob;
                            string RemarksData = Convert.ToString(dropDownNew.Text);
                            string cat_id = Convert.ToString(dropDownNew.ID).Replace("dd", "");
                            dt = oDBEngine.GetDataTable("select isMandatory,cat_max_value  from tbl_master_remarksCategory where id=" + cat_id);
                            if (dt.Rows.Count > 0)
                            {
                                dr = dt.Rows[0];
                                if (Convert.ToBoolean(dr["isMandatory"]) && RemarksData.Trim() == "")
                                {
                                    if (Convert.ToString(tabControl.JSProperties["cpMandatory"]).Trim() == "")
                                        tabControl.JSProperties["cpMandatory"] = cat_id;
                                    else
                                        tabControl.JSProperties["cpMandatory"] = Convert.ToString(tabControl.JSProperties["cpMandatory"]) + "~" + cat_id;
                                    retval = false;
                                    continue;
                                }
                            }
                        }
                        if (ob is RadioButtonList)
                        {
                            RadioButtonList radioNew = new RadioButtonList();
                            radioNew = (RadioButtonList)ob;

                            string RemarksData = Convert.ToString(radioNew.Text);
                            string cat_id = Convert.ToString(radioNew.ID).Replace("rd", "");
                            dt = oDBEngine.GetDataTable("select isMandatory,cat_max_value  from tbl_master_remarksCategory where id=" + cat_id);
                            if (dt.Rows.Count > 0)
                            {
                                dr = dt.Rows[0];
                                if (Convert.ToBoolean(dr["isMandatory"]) && RemarksData.Trim() == "")
                                {
                                    if (Convert.ToString(tabControl.JSProperties["cpMandatory"]).Trim() == "")
                                        tabControl.JSProperties["cpMandatory"] = cat_id;
                                    else
                                        tabControl.JSProperties["cpMandatory"] = Convert.ToString(tabControl.JSProperties["cpMandatory"]) + "~" + cat_id;
                                    retval = false;
                                    continue;
                                }
                            }
                        }
                    }
                }
            }

            return retval;

        }


        public void SaveTabData()
        {

            //  DataTable dt = oDBEngine.GetDataTable("tbl_master_remarksCategory", "id,cat_description,cat_field_type ,isMandatory", "cat_applicablefor='" + Convert.ToString(Session["KeyVal1"]) + "'");


            //loop through tab
            //foreach (TabPage page in tabControl.TabPages)
            //{
            //    foreach (object ctrl in page.Controls)
            //    {
            //        if (ctrl is TextBox)
            //        {
            //            string str = "";
            //        }
            //    }

            //}
            if (!isValid())
            {
                return;
            }
            loadUdfDataInTable();
            foreach (TabPage page in tabControl.TabPages)
            {
                if (!page.IsActive)
                {
                    continue;
                }

                foreach (object ob in page.Controls)
                {
                    if (ob is TextBox)
                    {
                        TextBox tbnew = new TextBox();
                        tbnew = (TextBox)ob;
                        string RemarksData = tbnew.Text.Trim();
                        string cat_id = tbnew.ID.ToString().Replace("txt", "");
                        cat_id = cat_id.Replace("memo", "");

                        if (RemarksData == "")
                        {
                            DelClass.DeleteUdfData(Convert.ToString(Request.QueryString["KeyVal_InternalID"]), Convert.ToInt32(cat_id));
                        }
                        else
                        {
                            if (Request.QueryString["KeyVal_InternalID"] != null && Session["userid"] != null)
                            {
                                if (Convert.ToString(Request.QueryString["KeyVal_InternalID"]) != "Add")
                                {
                                    reCat.insertRemarkCategory(Convert.ToString(Request.QueryString["KeyVal_InternalID"]), Convert.ToInt32(cat_id), RemarksData, Convert.ToString(Session["userid"]));
                                }
                                else
                                {
                                    addDataToTable(Convert.ToString(Request.QueryString["Type"]), Convert.ToInt32(cat_id), RemarksData);
                                }
                            }
                        }

                    }
                    if (ob is ASPxDateEdit)
                    {
                        ASPxDateEdit datenew = new ASPxDateEdit();
                        datenew = (ASPxDateEdit)ob;
                        if (datenew.Value != null)
                        {
                            DateTime RemarksData = Convert.ToDateTime(datenew.Value);
                            string cat_id = Convert.ToString(datenew.ID).Replace("dt", "");
                            if (Request.QueryString["KeyVal_InternalID"] != null && Session["userid"] != null)
                            {
                                if (Convert.ToString(Request.QueryString["KeyVal_InternalID"]) != "Add")
                                {
                                    reCat.insertRemarkCategory(Convert.ToString(Request.QueryString["KeyVal_InternalID"]), Convert.ToInt32(cat_id), Convert.ToString(RemarksData), Convert.ToString(Session["userid"]));
                                }
                                else
                                {
                                    addDataToTable(Convert.ToString(Request.QueryString["Type"]), Convert.ToInt32(cat_id), Convert.ToString(RemarksData));
                                }
                            }
                        }
                        else
                        {
                            string cat_id = Convert.ToString(datenew.ID).Replace("dt", "");
                            if (Request.QueryString["KeyVal_InternalID"] != null && Session["userid"] != null)
                            {
                                DelClass.DeleteUdfData(Convert.ToString(Request.QueryString["KeyVal_InternalID"]), Convert.ToInt32(cat_id));
                            }
                        }
                    }

                    if (ob is ASPxMemo)
                    {
                        ASPxMemo MemoNew = new ASPxMemo();
                        MemoNew = (ASPxMemo)ob;
                        string RemarksData = MemoNew.Text.Trim();
                        string cat_id = Convert.ToString(MemoNew.ID).Replace("memo", "");
                        if (RemarksData == "")
                        {
                            DelClass.DeleteUdfData(Convert.ToString(Request.QueryString["KeyVal_InternalID"]), Convert.ToInt32(cat_id));
                        }
                        else
                        {
                            if (Request.QueryString["KeyVal_InternalID"] != null && Session["userid"] != null)
                            {
                                if (Convert.ToString(Request.QueryString["KeyVal_InternalID"]) != "Add")
                                {
                                    reCat.insertRemarkCategory(Convert.ToString(Request.QueryString["KeyVal_InternalID"]), Convert.ToInt32(cat_id), Convert.ToString(RemarksData), Convert.ToString(Session["userid"]));
                                }
                                else
                                {
                                    addDataToTable(Convert.ToString(Request.QueryString["Type"]), Convert.ToInt32(cat_id), Convert.ToString(RemarksData));
                                }
                            }
                        }
                    }

                    if (ob is DropDownList)
                    {
                        DropDownList dropDownNew = new DropDownList();
                        dropDownNew = (DropDownList)ob;
                        string RemarksData = Convert.ToString(dropDownNew.Text);
                        string cat_id = Convert.ToString(dropDownNew.ID).Replace("dd", "");

                        if (RemarksData == "")
                        {
                            DelClass.DeleteUdfData(Convert.ToString(Request.QueryString["KeyVal_InternalID"]), Convert.ToInt32(cat_id));
                        }
                        else
                        {
                            if (Request.QueryString["KeyVal_InternalID"] != null && Session["userid"] != null)
                            {
                                if (Convert.ToString(Request.QueryString["KeyVal_InternalID"]) != "Add")
                                {
                                    reCat.insertRemarkCategory(Convert.ToString(Request.QueryString["KeyVal_InternalID"]), Convert.ToInt32(cat_id), Convert.ToString(RemarksData), Convert.ToString(Session["userid"]));
                                }
                                else
                                {
                                    addDataToTable(Convert.ToString(Request.QueryString["Type"]), Convert.ToInt32(cat_id), Convert.ToString(RemarksData));
                                }

                            }
                        }
                    }

                    if (ob is CheckBox)
                    {
                        CheckBox chekNew = new CheckBox();
                        chekNew = (CheckBox)ob;
                        string RemarksData;
                        if (Convert.ToString(chekNew.ID).Contains("chk"))
                        {
                            if (chekNew.Checked)
                                RemarksData = "1";
                            else
                                RemarksData = "0";
                            string cat_id = Convert.ToString(chekNew.ID).Replace("chk", "");
                            if (Request.QueryString["KeyVal_InternalID"] != null && Session["userid"] != null)
                            {
                                if (Convert.ToString(Request.QueryString["KeyVal_InternalID"]) != "Add")
                                {
                                    reCat.insertRemarkCategory(Convert.ToString(Request.QueryString["KeyVal_InternalID"]), Convert.ToInt32(cat_id), Convert.ToString(RemarksData), Convert.ToString(Session["userid"]));
                                }
                                else
                                {
                                    addDataToTable(Convert.ToString(Request.QueryString["Type"]), Convert.ToInt32(cat_id), Convert.ToString(RemarksData));
                                }
                            }
                        }

                    }

                    if (ob is CheckBoxList)
                    {
                        CheckBoxList chekNew = new CheckBoxList();
                        chekNew = (CheckBoxList)ob;
                        string RemarksData = "";
                        if (Convert.ToString(chekNew.ID).Contains("chk"))
                        {
                            foreach (ListItem listItem in chekNew.Items)
                            {
                                if (listItem.Selected)
                                {
                                    if (RemarksData.Trim() == "")
                                        RemarksData = listItem.Text;
                                    else
                                        RemarksData = RemarksData + "~" + listItem.Text;
                                }
                            }

                            string cat_id = Convert.ToString(chekNew.ID).Replace("chk", "");
                            if (Request.QueryString["KeyVal_InternalID"] != null && Session["userid"] != null)
                            {
                                if (Convert.ToString(Request.QueryString["KeyVal_InternalID"]) != "Add")
                                {
                                    reCat.insertRemarkCategory(Convert.ToString(Request.QueryString["KeyVal_InternalID"]), Convert.ToInt32(cat_id), Convert.ToString(RemarksData), Convert.ToString(Session["userid"]));
                                }
                                else
                                {
                                    addDataToTable(Convert.ToString(Request.QueryString["Type"]), Convert.ToInt32(cat_id), Convert.ToString(RemarksData));
                                }
                            }
                        }

                    }

                    if (ob is RadioButtonList)
                    {
                        RadioButtonList radioNew = new RadioButtonList();
                        radioNew = (RadioButtonList)ob;

                        string RemarksData = Convert.ToString(radioNew.Text);
                        string cat_id = Convert.ToString(radioNew.ID).Replace("rd", "");
                        if (Request.QueryString["KeyVal_InternalID"] != null && Session["userid"] != null)
                        {
                            if (Convert.ToString(Request.QueryString["KeyVal_InternalID"]) != "Add")
                            {
                                reCat.insertRemarkCategory(Convert.ToString(Request.QueryString["KeyVal_InternalID"]), Convert.ToInt32(cat_id), Convert.ToString(RemarksData), Convert.ToString(Session["userid"]));
                            }
                            else
                            {
                                addDataToTable(Convert.ToString(Request.QueryString["Type"]), Convert.ToInt32(cat_id), Convert.ToString(RemarksData));
                            }
                        }
                    }

                    if (ob is ListBox)
                    {
                        ListBox lstNew = new ListBox();
                        lstNew = (ListBox)ob;
                        string RemarksData = "";

                        for (int i = lstNew.Items.Count - 1; i >= 0; i--)
                        {
                            if (lstNew.Items[i].Selected == true)
                            {
                                if (RemarksData.Trim() == "")
                                    RemarksData = lstNew.Items[i].Text;
                                else
                                    RemarksData = RemarksData + "~" + lstNew.Items[i].Text;
                            }
                        }


                        string cat_id = lstNew.ID.ToString().Replace("lstcsn", "");
                        if (Request.QueryString["KeyVal_InternalID"] != null && Session["userid"] != null)
                        {
                            if (Convert.ToString(Request.QueryString["KeyVal_InternalID"]) != "Add")
                            {
                                reCat.insertRemarkCategory(Convert.ToString(Request.QueryString["KeyVal_InternalID"]), Convert.ToInt32(cat_id), RemarksData, Convert.ToString(Session["userid"]));
                            }
                            else
                            {
                                addDataToTable(Convert.ToString(Request.QueryString["Type"]), Convert.ToInt32(cat_id), Convert.ToString(RemarksData));
                            }
                        }

                    }

                }

                //GridRemarks.DataBind();
                tabControl.JSProperties["cpReturnMsg"] = "Saved Successfully.";


            }
        }
        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        protected void tabControl_Callback(object sender, CallbackEventArgsBase e)
        {
            SaveTabData();
        }


        protected void loadUdfDataInTable()
        {
            if (Session["UdfDataOnAdd"] == null)
            {
                CreateUdfTable();
            }

        }

        protected void addDataToTable(string type, int cat_id, string remarksData)
        {
            DataTable sessionTable = (DataTable)Session["UdfDataOnAdd"];
            //Delete Existing row
            DataRow[] deleterow = sessionTable.Select("cat_id=" + Convert.ToString(cat_id));
            foreach (DataRow dr in deleterow)
            {
                sessionTable.Rows.Remove(dr);
            }
            //Delete Existing row end here

            DataRow tblRow = sessionTable.NewRow();
            tblRow["Type"] = type;
            tblRow["cat_id"] = cat_id;
            tblRow["RemarksData"] = remarksData;
            sessionTable.Rows.Add(tblRow);
            Session["UdfDataOnAdd"] = sessionTable;
        }

        protected void CreateUdfTable()
        {
            DataTable udfAdd = new DataTable("UdfDataOnAdd");
            udfAdd.Columns.Add("Type", typeof(System.String));
            udfAdd.Columns.Add("cat_id", typeof(System.Int32));
            udfAdd.Columns.Add("RemarksData", typeof(System.String));
            Session["UdfDataOnAdd"] = udfAdd;

        }

    }



}