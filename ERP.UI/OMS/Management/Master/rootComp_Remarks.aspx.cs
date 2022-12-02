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

namespace ERP.OMS.Management.Master
{
    public partial class management_master_rootComp_Remarks : ERP.OMS.ViewState_class.VSPage
    {
        string[,] FieldName;
        public string pageAccess = "";
        //DBEngine oDBEngine = new DBEngine(string.Empty);

        AjaxControlToolkit.TabContainer tabDynamic;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
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
                Session["rea_internalId"] = Session["KeyVal_InternalID"];
            }

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/root_Companies.aspx"); 
             
            if (HttpContext.Current.Session["userid"] == null)
            {
                // Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }


            Session["KeyVal1"] = "Cmp";
            
            if (Session["Name"] != null)
            {
                try
                {
                    sqlCategory.SelectParameters["KeyVal1"].DefaultValue = Session["KeyVal1"].ToString();
                }
                catch (Exception ex)
                {
                    sqlCategory.SelectParameters["KeyVal1"].DefaultValue = "";
                }
                lblName.Text = Session["Name"].ToString();
            }
            if (Session["requesttype"].ToString() == "OtherEntity")
            {
               
                TabPage page = ASPxPageControl1.TabPages.FindByName("CorresPondence");
                page.Visible = true;
                  
                page = ASPxPageControl1.TabPages.FindByName("Documents");
                page.Visible = true;   

            }
            //Bellow function will load all the control with res[ect to there module , Module Id pass by the session variable  session[keval1]
           // loadControls(); 
            loadTab();

        }
        protected void btnSearch(object sender, EventArgs e)
        {
            GridRemarks.Settings.ShowFilterRow = true;
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
                GridRemarks.ClearSort();
                GridRemarks.DataBind();
                //debjyoti 13-12-2016
                string[] CallVal = e.Parameters.ToString().Split('~');
                GridRemarks.JSProperties["cpReturnMsg"] = null;
                //debjyoti 13-12-2016 end

                if (e.Parameters == "s")
                    GridRemarks.Settings.ShowFilterRow = true;

                if (e.Parameters == "All")
                {
                    GridRemarks.FilterExpression = string.Empty;
                }


                if (e.Parameters == "Lead")
                {

                }
                if (CallVal[0].ToString() == "SAVETABDATA")
                {
                    SaveTabData();
                }
                //debjyoti 13-12-2016
                if (CallVal[0].ToString() == "BEFORE_EDIT")
                {
                    string[,] Field_Value;
                    Field_Value = oDBEngine.GetFieldValue("tbl_master_contactRemarks r inner join tbl_master_remarksCategory c on r.cat_id=c.id ", "r.id,r.rea_internalId,r.cat_id,r.rea_Remarks,c.cat_description,c.cat_field_type,c.cat_max_date,c.cat_group_id", "r.id=" + Convert.ToString(CallVal[1]), 8);

                    //replace the \n with "~" so that proper json can be created
                    Field_Value[0, 3] = Convert.ToString(Field_Value[0, 3]).Replace("\r\n", "~");
                    Field_Value[0, 3] = Convert.ToString(Field_Value[0, 3]).Replace("\n", "~");
                    GridRemarks.JSProperties["cpEditJson"] = @"{""id"":" + @"""" + Field_Value[0, 0].ToString() +
                                                            @""",""rea_internalId"":""" + Field_Value[0, 1].ToString() +
                                                            @""",""cat_id"":""" + Field_Value[0, 2].ToString() +
                                                            @""",""rea_Remarks"":""" + Field_Value[0, 3].ToString() +
                                                            @""",""cat_description"":""" + Field_Value[0, 4].ToString() +
                                                            @""",""cat_field_type"":""" + Field_Value[0, 5].ToString() +
                                                            @""",""cat_max_date"":""" + Field_Value[0, 6].ToString() +
                                                             @""",""cat_group_id"":""" + Field_Value[0, 7].ToString() +
                                                            @"""}";
                }

                if (CallVal[0].ToString() == "EDIT")
                {
                    Dictionary<string, string> JsonValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(Convert.ToString(CallVal[1]));
                    //Get the datatable
                    DataTable remarksDt = oDBEngine.GetDataTable("select id,cat_description,cat_applicablefor,cat_field_type,isMandatory,cat_max_value,cat_max_length,cat_max_date from tbl_master_remarksCategory where id=" + Convert.ToString(JsonValues["cat_id"]));
                    if (remarksDt.Rows.Count > 0)
                    {
                        DataRow dr = remarksDt.Rows[0];
                        if (Convert.ToInt32(dr["cat_field_type"]) == 1 || Convert.ToInt32(dr["cat_field_type"]) == 2 || Convert.ToInt32(dr["cat_field_type"]) == 4 || Convert.ToInt32(dr["cat_field_type"]) == 5)
                        {
                            if (Convert.ToBoolean(dr["isMandatory"]) && Convert.ToString(JsonValues["rea_Remarks"]).Trim() == "")
                            {
                                GridRemarks.JSProperties["cpErrorMsg"] = "Value must be entered.";
                                return;
                            }
                        }
                        if (Convert.ToInt32(dr["cat_field_type"]) == 5 && Convert.ToString(JsonValues["rea_Remarks"]).Trim() != "" && !IsValidEmail(Convert.ToString(JsonValues["rea_Remarks"])))
                        {
                            GridRemarks.JSProperties["cpErrorMsg"] = "Please provide a valid E-mail.";
                            return;
                        }
                        //debjyoti 21-12-2016
                        //if (Convert.ToInt32(dr["cat_field_type"]) == 4 && Convert.ToDouble(dr["cat_max_value"]) < Convert.ToDouble(JsonValues["rea_Remarks"]))
                        //{
                        //    GridRemarks.JSProperties["cpErrorMsg"] = "Value must be less than " + Convert.ToString(dr["cat_max_value"]) + ".";
                        //    return;
                        //}

                        if (Convert.ToInt32(dr["cat_field_type"]) == 2 && Convert.ToString(JsonValues["rea_Remarks"]).Length > 500)
                        {
                            GridRemarks.JSProperties["cpErrorMsg"] = "Memo field can not accept more than 500 characters.";
                            return;
                        }
                    }
                    oDBEngine.SetFieldValue("tbl_master_contactRemarks", "rea_Remarks='" + Convert.ToString(JsonValues["rea_Remarks"]) + "'", "id='" + Convert.ToString(JsonValues["id"]) + "'");
                    GridRemarks.DataBind();
                    GridRemarks.JSProperties["cpReturnMsg"] = "Saved Successfully.";
                    //reCat.insertRemarkCategory(Convert.ToString(Session["KeyVal_InternalID"]),Convert.ToInt32(JsonValues["cat_id"]), Convert.ToString(JsonValues["rea_Remarks"]), Convert.ToString(Session["userid"]));
                }
                if (CallVal[0].ToString() == "Delete")
                {
                    string Remarkcategorycode = Convert.ToString(CallVal[1].ToString());
                    int i = 0;
                    i = oDBEngine.DeleteValue("tbl_master_contactRemarks", "id ='" + Remarkcategorycode.ToString() + "'");
                    GridRemarks.DataBind();
                    GridRemarks.JSProperties["cpReturnMsg"] = "Succesfully Deleted.";
                }

                if (CallVal[0].ToString() == "UPDATE_DATE")
                {
                    Dictionary<string, string> JsonValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(Convert.ToString(CallVal[1]));
                    DataTable remarksDt = oDBEngine.GetDataTable("select id,cat_description,cat_applicablefor,cat_field_type,isMandatory,cat_max_value,cat_max_length,cat_max_date from tbl_master_remarksCategory where id=" + Convert.ToString(JsonValues["cat_id"]));
                    if (remarksDt.Rows.Count > 0)
                    {
                        DataRow dr = remarksDt.Rows[0];
                        if (Convert.ToBoolean(dr["isMandatory"]) && dtEditDate.Value == null)
                        {
                            GridRemarks.JSProperties["cpErrorMsg"] = "Value must be entered.";
                            return;
                        }

                        if (Convert.ToBoolean(dr["isMandatory"]) && dtEditDate.Value == null)
                        {
                            GridRemarks.JSProperties["cpErrorMsg"] = "Value must be entered.";
                            return;
                        }
                        oDBEngine.SetFieldValue("tbl_master_contactRemarks", "rea_Remarks='" + Convert.ToString(dtEditDate.Value) + "'", "id='" + Convert.ToString(JsonValues["id"]) + "'");
                        GridRemarks.DataBind();
                        GridRemarks.JSProperties["cpReturnMsg"] = "Saved Successfully.";
                    }
                }


                //debjyoti 13-12-2016 end

            }
            catch (Exception ex)
            {

            }
        }





        protected void GridRemarks_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            GridRemarks.SettingsText.PopupEditFormCaption = "Modify UDF";

        }

        protected void GridRemarks_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            GridRemarks.SettingsText.PopupEditFormCaption = "Add UDF";
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
            DataTable dt = oDBEngine.GetDataTable("tbl_master_remarksCategory", "id,cat_description,cat_field_type ,isMandatory", "cat_applicablefor='" + Convert.ToString(Session["KeyVal1"]) + "'");


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
                    if (Session["KeyVal_InternalID"] != null && Session["userid"] != null && RemarksData != "")
                    {
                        reCat.insertRemarkCategory(Convert.ToString(Session["KeyVal_InternalID"]), Convert.ToInt32(cat_id), RemarksData, Convert.ToString(Session["userid"]));
                    }

                }
                if (ob is ASPxDateEdit)
                {
                    ASPxDateEdit datenew = new ASPxDateEdit();
                    datenew = (ASPxDateEdit)ob;
                    DateTime RemarksData = Convert.ToDateTime(datenew.Value);
                    string cat_id = Convert.ToString(datenew.ID).Replace("dt", "");
                    if (Session["KeyVal_InternalID"] != null && Session["userid"] != null)
                    {
                        reCat.insertRemarkCategory(Convert.ToString(Session["KeyVal_InternalID"]), Convert.ToInt32(cat_id), Convert.ToString(RemarksData), Convert.ToString(Session["userid"]));
                    }

                }

                if (ob is ASPxMemo)
                {
                    ASPxMemo MemoNew = new ASPxMemo();
                    MemoNew = (ASPxMemo)ob;
                    string RemarksData = MemoNew.Text.Trim();
                    string cat_id = Convert.ToString(MemoNew.ID).Replace("memo", "");
                    if (Session["KeyVal_InternalID"] != null && Session["userid"] != null)
                    {
                        reCat.insertRemarkCategory(Convert.ToString(Session["KeyVal_InternalID"]), Convert.ToInt32(cat_id), Convert.ToString(RemarksData), Convert.ToString(Session["userid"]));
                    }

                }

                if (ob is DropDownList)
                {
                    DropDownList dropDownNew = new DropDownList();
                    dropDownNew = (DropDownList)ob;
                    string RemarksData = Convert.ToString(dropDownNew.Text);
                    string cat_id = Convert.ToString(dropDownNew.ID).Replace("dd", "");
                    if (Session["KeyVal_InternalID"] != null && Session["userid"] != null)
                    {
                        reCat.insertRemarkCategory(Convert.ToString(Session["KeyVal_InternalID"]), Convert.ToInt32(cat_id), Convert.ToString(RemarksData), Convert.ToString(Session["userid"]));
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
                        if (Session["KeyVal_InternalID"] != null && Session["userid"] != null)
                        {
                            reCat.insertRemarkCategory(Convert.ToString(Session["KeyVal_InternalID"]), Convert.ToInt32(cat_id), Convert.ToString(RemarksData), Convert.ToString(Session["userid"]));
                        }
                    }

                }
                if (ob is RadioButtonList)
                {
                    RadioButtonList radioNew = new RadioButtonList();
                    radioNew = (RadioButtonList)ob;

                    string RemarksData = Convert.ToString(radioNew.Text);
                    string cat_id = Convert.ToString(radioNew.ID).Replace("rd", "");
                    if (Session["KeyVal_InternalID"] != null && Session["userid"] != null)
                    {
                        reCat.insertRemarkCategory(Convert.ToString(Session["KeyVal_InternalID"]), Convert.ToInt32(cat_id), Convert.ToString(RemarksData), Convert.ToString(Session["userid"]));
                    }
                }

            }
            Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "jAlert('Saved Successfully.');", true);
            Popup_Empcitys.ShowOnPageLoad = false;
            GridRemarks.DataBind();
            //ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "Js1", " MakeInVisible();", true);

        }



        public void loadControls()
        {

            //DataTable dt = oDBEngine.GetDataTable("tbl_master_remarksCategory", "id,cat_description,cat_field_type,cat_max_length,isMandatory,cat_max_value,cat_max_date,cat_options ", "cat_applicablefor='Ld'");
            DataTable dt = oDBEngine.GetDataTable("tbl_master_remarksCategory", "id,cat_description,cat_field_type,cat_max_length,isMandatory,cat_max_value,cat_max_date,cat_options ", "cat_applicablefor='" + Convert.ToString(Session["KeyVal1"]) + "'");
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

            ///For Tab control ... tab added here from udf group
            DataTable dtTab = oDBEngine.GetDataTable("tbl_master_udfGroup", " id,grp_description ", "grp_applicablefor='" + Convert.ToString(Session["KeyVal1"]) + "' and grp_isVisible=1");
            foreach (DataRow drtab in dtTab.Rows)
            {
                newTab = new TabPage(Convert.ToString(drtab["grp_description"]), Convert.ToString(drtab["id"]));
                tabControl.TabPages.Add(newTab);
            }

            //debjyoti 21-12-2016
            dtTab = oDBEngine.GetDataTable("tbl_master_remarksCategory", "1", "cat_applicablefor='" + Convert.ToString(Session["KeyVal1"]) + "' and cat_group_id=0");
            if (dtTab.Rows.Count > 0)
            {
                newTab = new TabPage("No Group", "0");
                tabControl.TabPages.Add(newTab);
            }

           //  DataTable dt = oDBEngine.GetDataTable("tbl_master_remarksCategory", "id,cat_description,cat_field_type,cat_max_length,isMandatory,cat_max_value,cat_max_date,cat_options,cat_group_id ", "cat_applicablefor='" + Convert.ToString(Session["KeyVal1"]) + "'  and (select grp_isvisible from tbl_master_udfGroup where id= cat_group_id)=1");
             //debjyoti 21-12-2016
             DataTable dt = oDBEngine.GetDataTable("tbl_master_remarksCategory", "id,cat_description,cat_field_type,cat_max_length,isMandatory,cat_max_value,cat_max_date,cat_options,cat_group_id ", "cat_applicablefor='" + Convert.ToString(Session["KeyVal1"]) + "'  and ((select grp_isvisible from tbl_master_udfGroup where id= cat_group_id)=1 or cat_group_id=0)");
          
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
                    tb.Attributes.Add("style", "width: 263px; height: 28px;");
                    tb.Attributes.Add("class", "controlClass");
                    Label lb = new Label();
                    lb.Text = Convert.ToString(dr["cat_description"]);
                    lb.Attributes.Add("class", "controlClass");
                    lb.ID = "lbl" + Convert.ToString(dr["id"]).Trim();
                    tempTab.Controls.Add(lb);
                    tempTab.Controls.Add(tb);

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
                    memo.Attributes.Add("style", "width: 263px; height: 55px;");
                    memo.Attributes.Add("Class", "controlClass");
                    memo.TextMode = TextBoxMode.MultiLine;
                    Label lb = new Label();
                    lb.Text = Convert.ToString(dr["cat_description"]);
                    lb.Attributes.Add("class", "controlClass");
                    lb.ID = "lbl" + Convert.ToString(dr["id"]).Trim();

                    tempTab.Controls.Add(lb);
                    tempTab.Controls.Add(memo);

                    if (Convert.ToBoolean(dr["isMandatory"]))
                    {
                        //RequiredFieldValidator reqFieldVal = new RequiredFieldValidator();
                        //reqFieldVal.ID = "validator_" + Convert.ToString(dr["id"]).Trim();
                        //reqFieldVal.ControlToValidate = memo.ID;
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
                #endregion forMemo

                #region forNumber
                if (Convert.ToInt32(dr["cat_field_type"]) == 4)
                {
                    TextBox tb = new TextBox();
                    tb.ID = "txt" + Convert.ToString(dr["id"]).Trim();
                    tb.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    tb.MaxLength = 20;
                    tb.Attributes.Add("style", "width: 263px; height: 28px;");
                    tb.TextMode = TextBoxMode.Number;
                    tb.Attributes.Add("class", "controlClass");

                    Label lb = new Label();
                    lb.Text = Convert.ToString(dr["cat_description"]);
                    lb.Attributes.Add("class", "controlClass");
                    lb.ID = "lbl" + Convert.ToString(dr["id"]).Trim();

                    tempTab.Controls.Add(lb);
                    tempTab.Controls.Add(tb);




                    //Debjyoti 21-12-2016
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
                if (Convert.ToInt32(dr["cat_field_type"]) == 3)
                {
                    ASPxDateEdit dateEdit = new ASPxDateEdit();
                    dateEdit.Attributes.Add("style", "width: 263px; height: 28px;");
                    dateEdit.ID = "dt" + Convert.ToString(dr["id"]).Trim();
                    dateEdit.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;

                    dateEdit.TimeSectionProperties.Visible = false;
                    dateEdit.UseMaskBehavior = true;
                    dateEdit.EditFormatString = "dd-MM-yyyy";
                    dateEdit.DisplayFormatString = "dd-MM-yyyy";
                    dateEdit.Date = System.DateTime.Now;
                    dateEdit.Attributes.Add("class", "controlClass");

                   
                    // dateEdit.NullText = "dd-MM-yyyy";
                    // dateEdit.AllowNull=Convert.ToBoolean(dr["isMandatory"]);
                    
                    //debjyoti 21-12-2016
                    //dateEdit.ValidateRequestMode = System.Web.UI.ValidateRequestMode.Enabled;
                    //dateEdit.MinDate = new DateTime(1900, 1, 1);
                    //if (Convert.ToDateTime(dr["cat_max_date"]) != new DateTime(1900, 1, 1))
                    //{
                    //    dateEdit.MaxDate = Convert.ToDateTime(dr["cat_max_date"]);
                    //}

                    Label lb = new Label();
                    lb.Text = Convert.ToString(dr["cat_description"]);
                    lb.Attributes.Add("class", "controlClass");
                    lb.ID = "lbl" + Convert.ToString(dr["id"]).Trim();

                    tempTab.Controls.Add(lb);
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
                if (Convert.ToInt32(dr["cat_field_type"]) == 5)
                {
                    TextBox tb = new TextBox();
                    tb.ID = "txt" + Convert.ToString(dr["id"]).Trim();
                    tb.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    tb.MaxLength = Convert.ToInt32(dr["cat_max_length"]);
                    tb.Attributes.Add("style", "width: 263px; height: 28px;");
                    tb.TextMode = TextBoxMode.Email;
                    tb.Attributes.Add("class", "controlClass");


                    Label lb = new Label();
                    lb.Text = Convert.ToString(dr["cat_description"]);
                    lb.Attributes.Add("class", "controlClass");
                    lb.ID = "lbl" + Convert.ToString(dr["id"]).Trim();

                    tempTab.Controls.Add(lb);
                    tempTab.Controls.Add(tb);

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
                        //reqFieldVal.ForeColor = Color.Red;
                        //tempTab.Controls.Add(reqFieldVal);
                        Label span = new Label();
                        span.Text = " <span id='Mandatory" + Convert.ToString(dr["id"]).Trim() + "' class='pullleftClass fa fa-exclamation-circle iconRed' style='color:red;display:none; position: absolute; right: -8px; top: 19px;' title='Mandatory'></span>";
                        tempTab.Controls.Add(span);
                    }


                }
                #endregion forEmail

                #region combo
                if (Convert.ToInt32(dr["cat_field_type"]) == 6)
                {
                    DropDownList dd = new DropDownList();
                    dd.ID = "dd" + Convert.ToString(dr["id"]).Trim();
                    dd.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    dd.Attributes.Add("style", "width: 263px; height: 28px;");
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

                    tempTab.Controls.Add(lb);
                    tempTab.Controls.Add(dd);

                    if (Convert.ToBoolean(dr["isMandatory"]))
                    {
                        //RequiredFieldValidator reqFieldVal = new RequiredFieldValidator();
                        //reqFieldVal.ID = "validator_" + Convert.ToString(dr["id"]).Trim();
                        //reqFieldVal.ControlToValidate = dd.ID;
                        //reqFieldVal.SetFocusOnError = true;
                        //reqFieldVal.ErrorMessage = "Required*";
                        //reqFieldVal.EnableClientScript = false;
                        //reqFieldVal.CssClass = "required";
                        //reqFieldVal.Enabled = true;
                        //reqFieldVal.EnableClientScript = true;
                        //reqFieldVal.ForeColor = Color.Red;
                        //tempTab.Controls.Add(reqFieldVal);
                        Label span = new Label();
                        span.Text = " <span id='Mandatory" + Convert.ToString(dr["id"]).Trim() + "' class='pullleftClass fa fa-exclamation-circle iconRed' style='color:red;display:none; position: absolute; right: -8px; top: 19px;' title='Mandatory'></span>";
                        tempTab.Controls.Add(span);
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

                    tempTab.Controls.Add(lb);
                    tempTab.Controls.Add(chk);
                }
                #endregion checkBox

                #region RadioButton
                if (Convert.ToInt32(dr["cat_field_type"]) == 8)
                {
                    Label lb = new Label();
                    lb.Text = Convert.ToString(dr["cat_description"]);
                    lb.Attributes.Add("class", "controlClass");
                    lb.ID = "lbl" + Convert.ToString(dr["id"]).Trim();

                    tempTab.Controls.Add(lb);
                    if (Convert.ToBoolean(dr["isMandatory"]))
                    {
                        //RequiredFieldValidator reqFieldVal = new RequiredFieldValidator();
                        //reqFieldVal.ID = "validator_" + Convert.ToString(dr["id"]).Trim();
                        //reqFieldVal.ControlToValidate = rdBttn.ID;
                        //reqFieldVal.SetFocusOnError = true;
                        //reqFieldVal.ErrorMessage = "Required*";
                        //reqFieldVal.EnableClientScript = false;
                        //reqFieldVal.CssClass = "required";
                        //reqFieldVal.Enabled = true;
                        //reqFieldVal.EnableClientScript = true;
                        //reqFieldVal.ForeColor = Color.Red;
                        //tempTab.Controls.Add(reqFieldVal);

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
                    tempTab.Controls.Add(rdBttn);



                }
                #endregion RadioButton

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
            GridRemarks.JSProperties["cpMandatory"] = "";
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
                            dt = oDBEngine.GetDataTable("select isMandatory,cat_max_value,cat_field_type,cat_description  from tbl_master_remarksCategory where id=" + cat_id);
                            if (dt.Rows.Count > 0)
                            {
                                dr = dt.Rows[0];
                                if (Convert.ToBoolean(dr["isMandatory"]) && RemarksData == "")
                                {
                                    if (Convert.ToString(GridRemarks.JSProperties["cpMandatory"]).Trim() == "")
                                        GridRemarks.JSProperties["cpMandatory"] = cat_id;
                                    else
                                        GridRemarks.JSProperties["cpMandatory"] = Convert.ToString(GridRemarks.JSProperties["cpMandatory"]) + "~" + cat_id;
                                    retval = false;
                                    continue;
                                }

                                //debjyoti 21-12-2016
                                //if (Convert.ToInt32(dr["cat_field_type"]) == 4 && Convert.ToDouble(RemarksData) > Convert.ToDouble(dr["cat_max_value"]))
                                //{
                                //    GridRemarks.JSProperties["cpErrorMsg"] = Convert.ToString(dr["cat_description"])+" cannot be greater than "+Convert.ToString(dr["cat_max_value"]);
                                //    retval = false;
                                //    continue;
                                //}
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
                                    if (Convert.ToString(GridRemarks.JSProperties["cpMandatory"]).Trim() == "")
                                        GridRemarks.JSProperties["cpMandatory"] = cat_id;
                                    else
                                        GridRemarks.JSProperties["cpMandatory"] = Convert.ToString(GridRemarks.JSProperties["cpMandatory"]) + "~" + cat_id;
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
                                    if (Convert.ToString(GridRemarks.JSProperties["cpMandatory"]).Trim() == "")
                                        GridRemarks.JSProperties["cpMandatory"] = cat_id;
                                    else
                                        GridRemarks.JSProperties["cpMandatory"] = Convert.ToString(GridRemarks.JSProperties["cpMandatory"]) + "~" + cat_id;
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
                                    if (Convert.ToString(GridRemarks.JSProperties["cpMandatory"]).Trim() == "")
                                        GridRemarks.JSProperties["cpMandatory"] = cat_id;
                                    else
                                        GridRemarks.JSProperties["cpMandatory"] = Convert.ToString(GridRemarks.JSProperties["cpMandatory"]) + "~" + cat_id;
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
                        if (Session["KeyVal_InternalID"] != null && Session["userid"] != null && RemarksData != "")
                        {
                            reCat.insertRemarkCategory(Convert.ToString(Session["KeyVal_InternalID"]), Convert.ToInt32(cat_id), RemarksData, Convert.ToString(Session["userid"]));
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
                            if (Session["KeyVal_InternalID"] != null && Session["userid"] != null)
                            {
                                reCat.insertRemarkCategory(Convert.ToString(Session["KeyVal_InternalID"]), Convert.ToInt32(cat_id), Convert.ToString(RemarksData), Convert.ToString(Session["userid"]));
                            }
                        }
                    }

                    if (ob is ASPxMemo)
                    {
                        ASPxMemo MemoNew = new ASPxMemo();
                        MemoNew = (ASPxMemo)ob;
                        string RemarksData = MemoNew.Text.Trim();
                        string cat_id = Convert.ToString(MemoNew.ID).Replace("memo", "");
                        if (Session["KeyVal_InternalID"] != null && Session["userid"] != null)
                        {
                            reCat.insertRemarkCategory(Convert.ToString(Session["KeyVal_InternalID"]), Convert.ToInt32(cat_id), Convert.ToString(RemarksData), Convert.ToString(Session["userid"]));
                        }

                    }

                    if (ob is DropDownList)
                    {
                        DropDownList dropDownNew = new DropDownList();
                        dropDownNew = (DropDownList)ob;
                        string RemarksData = Convert.ToString(dropDownNew.Text);
                        string cat_id = Convert.ToString(dropDownNew.ID).Replace("dd", "");
                        if (Session["KeyVal_InternalID"] != null && Session["userid"] != null)
                        {
                            reCat.insertRemarkCategory(Convert.ToString(Session["KeyVal_InternalID"]), Convert.ToInt32(cat_id), Convert.ToString(RemarksData), Convert.ToString(Session["userid"]));
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
                            if (Session["KeyVal_InternalID"] != null && Session["userid"] != null)
                            {
                                reCat.insertRemarkCategory(Convert.ToString(Session["KeyVal_InternalID"]), Convert.ToInt32(cat_id), Convert.ToString(RemarksData), Convert.ToString(Session["userid"]));
                            }
                        }

                    }
                    if (ob is RadioButtonList)
                    {
                        RadioButtonList radioNew = new RadioButtonList();
                        radioNew = (RadioButtonList)ob;

                        string RemarksData = Convert.ToString(radioNew.Text);
                        string cat_id = Convert.ToString(radioNew.ID).Replace("rd", "");
                        if (Session["KeyVal_InternalID"] != null && Session["userid"] != null)
                        {
                            reCat.insertRemarkCategory(Convert.ToString(Session["KeyVal_InternalID"]), Convert.ToInt32(cat_id), Convert.ToString(RemarksData), Convert.ToString(Session["userid"]));
                        }
                    }

                }

                GridRemarks.DataBind();
                GridRemarks.JSProperties["cpReturnMsg"] = "Saved Successfully.";


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

    }
}