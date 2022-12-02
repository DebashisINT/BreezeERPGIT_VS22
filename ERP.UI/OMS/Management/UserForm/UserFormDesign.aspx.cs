using DataAccessLayer;
using DevExpress.Web;
using ERP.OMS.Management.UserForm.UserControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.UserForm
{
    public partial class UserFormDesign : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadControl(); 
            }
        }

        private void LoadControl()
        {

            hdMainId.Value = Request.QueryString["id"];

            ProcedureExecute proc = new ProcedureExecute("prc_UserDefineFormDetails");
            proc.AddVarcharPara("@Action", 100, "GetControlsForDesigner");
            proc.AddVarcharPara("@Name", 500, Request.QueryString["ModName"]);
            proc.AddVarcharPara("@tagId", 500, Request.QueryString["id"]); 
            DataSet ds = proc.GetDataSet();
            DataTable Header = ds.Tables[0];
            DataTable LayoutTable = ds.Tables[1];
            DataTable PanelTable = ds.Tables[2];


            Label divlb = new Label();
            LiteralControl literal = new LiteralControl();
            int divCount = 0;


            if (LayoutTable.Rows.Count > 0)
            {
                #region MainDiv

                foreach (DataRow lrow in LayoutTable.Rows)
                {
                    DataRow dr = LayoutTable.Rows[0];
                    if (Convert.ToString(lrow["ControlId"]).Trim() != "")
                        dr = Header.Select("id=" + lrow["ControlId"])[0];


                    if (Convert.ToString(lrow["ControlId"]).Trim() == "")
                    {

                        HtmlGenericControl htmlDiv = new HtmlGenericControl("div");
                        htmlDiv.Attributes["class"] = Convert.ToString(lrow["ClassName"]);
                        htmlDiv.InnerHtml = "<span onclick=removeDiv(event) style='left: 50%; position: absolute;'><i class='fa fa-times-circle'></i></span>";
                        divdropid.Controls.Add(htmlDiv);
                        

                    }
                    else if (Convert.ToString(dr["VissibleText"]) == "Text Field" || Convert.ToString(dr["VissibleText"]) == "Formula")
                    {
                        divCount += 2;
                        LiteralControl literalControl = new LiteralControl();
                        literalControl.Text = "<div  draggable='true' ondragstart='dragControl(event)' id='div" + Convert.ToString(dr["id"]) + "' class='" + Convert.ToString(lrow["ClassName"]) + "'><label>" + Convert.ToString(lrow["DisplayText"]) + "</label>";
                        divdropid.Controls.Add(literalControl);

                        ASPxTextBox DyTextBox = new ASPxTextBox();
                        DyTextBox.ClientInstanceName = "cid" + Convert.ToString(dr["id"]);
                        DyTextBox.ID = "id" + Convert.ToString(dr["id"]);
                         
                        DyTextBox.MaxLength = 99;
                        DyTextBox.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                        DyTextBox.ClientSideEvents.GotFocus = "function(){ storeControlDiv(" + Convert.ToString(dr["id"]) + ") }";
                        divdropid.Controls.Add(DyTextBox);

                        literalControl = new LiteralControl();
                        literalControl.Text = "</div>";
                        divdropid.Controls.Add(literalControl);
                    }
                    else if (Convert.ToString(dr["VissibleText"]) == "Date")
                    {
                        divCount += 2;
                        literal = new LiteralControl();
                        literal.Text = "<div  draggable='true' ondragstart='dragControl(event)' id='div" + Convert.ToString(dr["id"]) + "' class='" + Convert.ToString(lrow["ClassName"]) + "'><label>" + Convert.ToString(lrow["DisplayText"]) + "</label>";
                        divdropid.Controls.Add(literal);

                        ASPxDateEdit DydateEddit = new ASPxDateEdit();
                        DydateEddit.ClientInstanceName = "cid" + Convert.ToString(dr["id"]);
                        DydateEddit.ID = "id" + Convert.ToString(dr["id"]);

                        DydateEddit.ClientSideEvents.GotFocus = "function(){ storeControlDiv(" + Convert.ToString(dr["id"]) + ") }";
                        DydateEddit.DisplayFormatString = "dd/MM/yyyy";
                        DydateEddit.EditFormatString = "dd/MM/yyyy";
                        DydateEddit.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                        divdropid.Controls.Add(DydateEddit);

                        literal = new LiteralControl();
                        literal.Text = "</div>";
                        divdropid.Controls.Add(literal);
                    }

                    else if (Convert.ToString(dr["VissibleText"]) == "Numeric")
                    {
                        divCount += 2;
                        literal = new LiteralControl();
                        literal.Text = "<div   draggable='true' ondragstart='dragControl(event)' id='div" + Convert.ToString(dr["id"]) + "' class='" + Convert.ToString(lrow["ClassName"]) + "'><label>" + Convert.ToString(lrow["DisplayText"]) + "</label>";
                        divdropid.Controls.Add(literal);

                        ASPxTextBox DyTextBox = new ASPxTextBox();
                        DyTextBox.ClientInstanceName = "cid" + Convert.ToString(dr["id"]);
                        DyTextBox.ID = "id" + Convert.ToString(dr["id"]);
                        DyTextBox.MaskSettings.Mask = "<0..999999999>.<00..99>";
                        DyTextBox.MaskSettings.AllowMouseWheel = false;
                        DyTextBox.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                        DyTextBox.ClientSideEvents.GotFocus = "function(){ storeControlDiv(" + Convert.ToString(dr["id"]) + ") }";
                        DyTextBox.ValidationSettings.Display = DevExpress.Web.Display.Dynamic;
                        divdropid.Controls.Add(DyTextBox);

                        literal = new LiteralControl();
                        literal.Text = "</div>";
                        divdropid.Controls.Add(literal);
                    }


                    else if (Convert.ToString(dr["VissibleText"]) == "Drop Down")
                    {
                        divCount += 2;
                        literal = new LiteralControl();
                        literal.Text = "<div  draggable='true' ondragstart='dragControl(event)' id='div" + Convert.ToString(dr["id"]) + "' class='" + Convert.ToString(lrow["ClassName"]) + "'><label>" + Convert.ToString(lrow["DisplayText"]) + "</label>";
                        divdropid.Controls.Add(literal);

                        ASPxComboBox dyCombo = new ASPxComboBox();
                        dyCombo.ClientInstanceName = "cid" + Convert.ToString(dr["id"]);
                        dyCombo.ID = "id" + Convert.ToString(dr["id"]);

                        string[] ListOfValues = Convert.ToString(dr["Value"]).Split(',');
                        foreach (string val in ListOfValues)
                        {
                            if (val != "")
                            {
                                dyCombo.Items.Add(val);
                            }
                        }
                        dyCombo.ClientSideEvents.GotFocus = "function(){ storeControlDiv(" + Convert.ToString(dr["id"]) + ") }";
                        dyCombo.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                        divdropid.Controls.Add(dyCombo);

                        literal = new LiteralControl();
                        literal.Text = "</div>";
                        divdropid.Controls.Add(literal);
                    }





                    else if (Convert.ToString(dr["VissibleText"]) == "Customer Master")
                    {
                        divCount += 2;
                        literal = new LiteralControl();
                        literal.Text = "<div  draggable='true' ondragstart='dragControl(event)' id='div" + Convert.ToString(dr["id"]) + "' class='" + Convert.ToString(lrow["ClassName"]) + "'><label>" + Convert.ToString(lrow["DisplayText"]) + "</label>";
                        divdropid.Controls.Add(literal);

                        CustomerSelection ucSimpleControl = LoadControl("~/OMS/Management/UserForm/UserControl/CustomerSelection.ascx") as CustomerSelection;
                        ucSimpleControl.ID = "id" + Convert.ToString(dr["id"]);
                        ucSimpleControl.SetClientsideEventGotFocus("function(){ storeControlDiv(" + Convert.ToString(dr["id"]) + ") }");
                        divdropid.Controls.Add(ucSimpleControl); 
                        literal = new LiteralControl();
                        literal.Text = "</div>";
                        divdropid.Controls.Add(literal);
                    }


                    else if (Convert.ToString(dr["VissibleText"]) == "Employee Master")
                    {
                        divCount += 2;
                        literal = new LiteralControl();
                        literal.Text = "<div  draggable='true' ondragstart='dragControl(event)' id='div" + Convert.ToString(dr["id"]) + "' class='" + Convert.ToString(lrow["ClassName"]) + "'><label>" + Convert.ToString(lrow["DisplayText"]) + "</label>";
                        divdropid.Controls.Add(literal);

                        EmployeeSelection ucSimpleControl = LoadControl("~/OMS/Management/UserForm/UserControl/EmployeeSelection.ascx") as EmployeeSelection;
                        ucSimpleControl.ID = "id" + Convert.ToString(dr["id"]);
                        ucSimpleControl.SetClientsideEventGotFocus("function(){ storeControlDiv(" + Convert.ToString(dr["id"]) + ") }");
                        divdropid.Controls.Add(ucSimpleControl);

                        literal = new LiteralControl();
                        literal.Text = "</div>";
                        divdropid.Controls.Add(literal);
                    }



                    else if (Convert.ToString(dr["VissibleText"]) == "Memo Field")
                    {
                        divCount += 6;
                        literal = new LiteralControl();
                        literal.Text = "<div  draggable='true' ondragstart='dragControl(event)' id='div" + Convert.ToString(dr["id"]) + "' class='" + Convert.ToString(lrow["ClassName"]) + "'><label>" + Convert.ToString(lrow["DisplayText"]) + "</label>";
                        divdropid.Controls.Add(literal);

                        ASPxMemo DyTextBox = new ASPxMemo();
                        DyTextBox.ClientInstanceName = "cid" + Convert.ToString(dr["id"]);
                        DyTextBox.ID = "id" + Convert.ToString(dr["id"]);

                        DyTextBox.ClientSideEvents.GotFocus = "function(){ storeControlDiv(" + Convert.ToString(dr["id"]) + ") }";
                        DyTextBox.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                        divdropid.Controls.Add(DyTextBox);

                        literal = new LiteralControl();
                        literal.Text = "</div>";
                        divdropid.Controls.Add(literal);


                        divCount = 0;
                    }





                }

                #endregion

            }




            #region Panel Layout


            foreach (DataRow dr in PanelTable.Rows)
            {

                if (Convert.ToString(dr["VissibleText"]) == "Text Field" || Convert.ToString(dr["VissibleText"]) == "Formula")
                {
                    divCount += 2;
                    divlb = new Label();
                    divlb.Text = "<div class='col-md-2'  draggable='true'  ondragstart='dragControl(event)' id='div" + Convert.ToString(dr["id"]) + "'><label>" + Convert.ToString(dr["FieldName"]) + "</label>";
                    HeaderControlDetails.Controls.Add(divlb);

                    ASPxTextBox DyTextBox = new ASPxTextBox();
                    DyTextBox.ClientInstanceName = "cid" + Convert.ToString(dr["id"]);
                    DyTextBox.ID = "id" + Convert.ToString(dr["id"]);

                     

                    DyTextBox.MaxLength = 99;
                    DyTextBox.ClientSideEvents.GotFocus = "function(){ storeControlDiv(" + Convert.ToString(dr["id"]) + ") }";
                    DyTextBox.Width = System.Web.UI.WebControls.Unit.Percentage(100); 
                    HeaderControlDetails.Controls.Add(DyTextBox);

                    divlb = new Label();
                    divlb.Text = "</div>";
                    HeaderControlDetails.Controls.Add(divlb);
                }
                else if (Convert.ToString(dr["VissibleText"]) == "Date")
                {
                    divCount += 2;
                    divlb = new Label();
                    divlb.Text = "<div class='col-md-2'   draggable='true' ondragstart='dragControl(event)' id='div" + Convert.ToString(dr["id"]) + "'><label>" + Convert.ToString(dr["FieldName"]) + "</label>";
                    HeaderControlDetails.Controls.Add(divlb);

                    ASPxDateEdit DydateEddit = new ASPxDateEdit();
                    DydateEddit.ClientInstanceName = "cid" + Convert.ToString(dr["id"]);
                    DydateEddit.ID = "id" + Convert.ToString(dr["id"]);
                     
                    DydateEddit.DisplayFormatString = "dd/MM/yyyy";
                    DydateEddit.EditFormatString = "dd/MM/yyyy";
                    DydateEddit.ClientSideEvents.GotFocus = "function(){ storeControlDiv(" + Convert.ToString(dr["id"]) + ") }";
                    DydateEddit.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                    HeaderControlDetails.Controls.Add(DydateEddit);

                    divlb = new Label();
                    divlb.Text = "</div>";
                    HeaderControlDetails.Controls.Add(divlb);
                }

                else if (Convert.ToString(dr["VissibleText"]) == "Numeric")
                {
                    divCount += 2;
                    divlb = new Label();
                    divlb.Text = "<div class='col-md-2'   draggable='true' ondragstart='dragControl(event)' id='div" + Convert.ToString(dr["id"]) + "'><label>" + Convert.ToString(dr["FieldName"]) + "</label>";
                    HeaderControlDetails.Controls.Add(divlb);

                    ASPxTextBox DyTextBox = new ASPxTextBox();
                    DyTextBox.ClientInstanceName = "cid" + Convert.ToString(dr["id"]);
                    DyTextBox.ID = "id" + Convert.ToString(dr["id"]);
                    DyTextBox.MaskSettings.Mask = "<0..999999999>.<00..99>";
                    DyTextBox.MaskSettings.AllowMouseWheel = false;
                    DyTextBox.ClientSideEvents.GotFocus = "function(){ storeControlDiv(" + Convert.ToString(dr["id"]) + ") }";
                    DyTextBox.ValidationSettings.Display = DevExpress.Web.Display.Dynamic;
                    DyTextBox.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                    HeaderControlDetails.Controls.Add(DyTextBox);

                    divlb = new Label();
                    divlb.Text = "</div>";
                    HeaderControlDetails.Controls.Add(divlb);
                }


                else if (Convert.ToString(dr["VissibleText"]) == "Drop Down")
                {
                    divCount += 2;
                    divlb = new Label();
                    divlb.Text = "<div class='col-md-2'   draggable='true' ondragstart='dragControl(event)' id='div" + Convert.ToString(dr["id"]) + "'><label>" + Convert.ToString(dr["FieldName"]) + "</label>";
                    HeaderControlDetails.Controls.Add(divlb);

                    ASPxComboBox dyCombo = new ASPxComboBox();
                    dyCombo.ClientInstanceName = "cid" + Convert.ToString(dr["id"]);
                    dyCombo.ID = "id" + Convert.ToString(dr["id"]);

                    string[] ListOfValues = Convert.ToString(dr["Value"]).Split(',');
                    foreach (string val in ListOfValues)
                    {
                        if (val != "")
                        {
                            dyCombo.Items.Add(val);
                        }
                    }

                    dyCombo.ClientSideEvents.GotFocus = "function(){ storeControlDiv(" + Convert.ToString(dr["id"]) + ") }";
                    dyCombo.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                    HeaderControlDetails.Controls.Add(dyCombo);

                    divlb = new Label();
                    divlb.Text = "</div>";
                    HeaderControlDetails.Controls.Add(divlb);
                }





                else if (Convert.ToString(dr["VissibleText"]) == "Customer Master")
                {
                    divCount += 2;
                    divlb = new Label();
                    divlb.Text = "<div class='col-md-2'   draggable='true' ondragstart='dragControl(event)' id='div" + Convert.ToString(dr["id"]) + "' ><label>" + Convert.ToString(dr["FieldName"]) + "</label>";
                    HeaderControlDetails.Controls.Add(divlb);

                    CustomerSelection ucSimpleControl = LoadControl("~/OMS/Management/UserForm/UserControl/CustomerSelection.ascx") as CustomerSelection;
                    ucSimpleControl.ID = "id" + Convert.ToString(dr["id"]);

                    ucSimpleControl.SetClientsideEventGotFocus("function(){ storeControlDiv(" + Convert.ToString(dr["id"]) + ") }");

                    HeaderControlDetails.Controls.Add(ucSimpleControl);

                    divlb = new Label();
                    divlb.Text = "</div>";
                    HeaderControlDetails.Controls.Add(divlb);
                }


                else if (Convert.ToString(dr["VissibleText"]) == "Employee Master")
                {
                    divCount += 2;
                    divlb = new Label();
                    divlb.Text = "<div class='col-md-2'   draggable='true' ondragstart='dragControl(event)' id='div" + Convert.ToString(dr["id"]) + "' ><label>" + Convert.ToString(dr["FieldName"]) + "</label>";
                    HeaderControlDetails.Controls.Add(divlb);

                    EmployeeSelection ucSimpleControl = LoadControl("~/OMS/Management/UserForm/UserControl/EmployeeSelection.ascx") as EmployeeSelection;
                    ucSimpleControl.ID = "id" + Convert.ToString(dr["id"]);

                    ucSimpleControl.SetClientsideEventGotFocus("function(){ storeControlDiv(" + Convert.ToString(dr["id"]) + ") }");

                    HeaderControlDetails.Controls.Add(ucSimpleControl);

                    divlb = new Label();
                    divlb.Text = "</div>";
                    HeaderControlDetails.Controls.Add(divlb);
                }




                else if (Convert.ToString(dr["VissibleText"]) == "Memo Field")
                {
                    divCount += 6; 

                    divlb = new Label();
                    divlb.Text = "<div class='col-md-2'   draggable='true' ondragstart='dragControl(event)' id='div" + Convert.ToString(dr["id"]) + "'><label>" + Convert.ToString(dr["FieldName"]) + "</label>";
                    HeaderControlDetails.Controls.Add(divlb);

                    ASPxMemo DyTextBox = new ASPxMemo();
                    DyTextBox.ClientInstanceName = "cid" + Convert.ToString(dr["id"]);
                    DyTextBox.ID = "id" + Convert.ToString(dr["id"]);
                     
                    DyTextBox.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                    DyTextBox.ClientSideEvents.GotFocus = "function(){ storeControlDiv(" + Convert.ToString(dr["id"]) + ") }";
                    HeaderControlDetails.Controls.Add(DyTextBox);

                    divlb = new Label();
                    divlb.Text = "</div>";
                    HeaderControlDetails.Controls.Add(divlb);


                    divCount = 0;
                }

                divlb = new Label();
                divlb.Text = "<div class='clear'></div>";
                HeaderControlDetails.Controls.Add(divlb);
                 


            }

            #endregion


        }


        [WebMethod]
        public static string UpdateDesgin(List<FormDesigner> formDesigner,string MainId)
        {
            string RetMsg = "-1~Error";

            try
            {
                DataTable layout = new DataTable();
                layout.Columns.Add("id", typeof(System.String));
                layout.Columns.Add("className", typeof(System.String));
                layout.Columns.Add("DisplayName", typeof(System.String));


                foreach (var obj in formDesigner)
                {
                    DataRow dr = layout.NewRow();
                    if (obj.id != null)
                    {
                        dr["id"] = Convert.ToString(obj.id);
                        dr["DisplayName"] = Convert.ToString(obj.DisplayName);
                    }
                    dr["className"] = Convert.ToString(obj.className);

                    layout.Rows.Add(dr);
                }


                ProcedureExecute proc = new ProcedureExecute("prc_UserDefineFormDetails");
                proc.AddVarcharPara("@Action", 100, "SaveLayout");
                proc.AddPara("@ModuleId", MainId);
                proc.AddPara("@layoutTbl", layout);

                proc.AddVarcharPara("@outputMsg", 200, "", QueryParameterDirection.Output);
                proc.AddIntegerPara("@status", null, QueryParameterDirection.Output); 
                proc.RunActionQuery();

                RetMsg = proc.GetParaValue("@status").ToString() + "~" + proc.GetParaValue("@outputMsg").ToString();
            
            }
            catch (Exception ex)
            {
                RetMsg = "-1~" + ex.Message;
            }

            return RetMsg;
        }
    }

    public class FormDesigner 
    {
        public string id { get; set; }
        public string className { get; set; }
        public string DisplayName { get; set; }
    
    }
}