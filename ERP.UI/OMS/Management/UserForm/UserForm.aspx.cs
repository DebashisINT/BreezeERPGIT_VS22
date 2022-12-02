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
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.UserForm
{
    public partial class UserForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadControl();

                hdtagId.Value = Request.QueryString["id"];
            }

        }

        private void LoadControl()
        {
            titleHeader.Text = Request.QueryString["ModName"];
            hdModuleName.Value = Request.QueryString["ModName"];
            List<JsonforHeader> jsonforHeader = new List<JsonforHeader>();

            ProcedureExecute proc = new ProcedureExecute("prc_UserDefineFormDetails");
            proc.AddVarcharPara("@Action", 100, "GetControls");
            proc.AddVarcharPara("@Name", 500, Request.QueryString["ModName"]);
            proc.AddVarcharPara("@tagId", 500, Request.QueryString["id"]);
            DataSet ds = proc.GetDataSet();
            DataTable Header = ds.Tables[0];
            DataTable HeaderResult = ds.Tables[1];
            DataTable LayoutTable = ds.Tables[2];



            Label divlb = new Label();

            int divCount = 0;
            #region Layout
            if (LayoutTable.Rows.Count > 0)
            {

                foreach (DataRow lrow in LayoutTable.Rows)
                {

                    DataRow dr = Header.Rows[0];
                    if (Convert.ToString(lrow["ControlId"]).Trim() != "")
                        dr = Header.Select("id=" + lrow["ControlId"])[0];

                    if (Convert.ToString(lrow["ControlId"]).Trim() == "")
                    {
                        divlb = new Label();
                        divlb.Text = "<div class='" + Convert.ToString(lrow["ClassName"]) + "'>";
                        HeaderControlDetails.Controls.Add(divlb);
                        divlb = new Label();
                        divlb.Text = "</div>";
                        HeaderControlDetails.Controls.Add(divlb);

                    }
                    else if (Convert.ToString(dr["VissibleText"]) == "Text Field" || Convert.ToString(dr["VissibleText"]) == "Formula")
                    {
                        divCount += 2;
                        divlb = new Label();
                        divlb.Text = "<div class='" + Convert.ToString(lrow["ClassName"]) + "'><label>" + Convert.ToString(lrow["DisplayText"]) + "</label>";
                        HeaderControlDetails.Controls.Add(divlb);

                        ASPxTextBox DyTextBox = new ASPxTextBox();
                        DyTextBox.ClientInstanceName = "cid" + Convert.ToString(dr["id"]);
                        DyTextBox.ID = "id" + Convert.ToString(dr["id"]);


                        var drPaytable = Header.AsEnumerable()
                         .Where(row => row.Field<string>("Formula").Contains(Convert.ToString(dr["FieldName"]) + "]"));


                        if (drPaytable.Count() > 0)
                        {
                            DyTextBox.ClientSideEvents.LostFocus = "SetFormula";
                        }

                        if (Request.QueryString["id"] != "Add")
                        {
                            DyTextBox.Text = Convert.ToString(HeaderResult.Rows[0][Convert.ToString(dr["FieldName"]).Trim()]);
                        }

                        DyTextBox.MaxLength = 99;
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
                        divlb.Text = "<div class='" + Convert.ToString(lrow["ClassName"]) + "'><label>" + Convert.ToString(lrow["DisplayText"]) + "</label>";
                        HeaderControlDetails.Controls.Add(divlb);

                        ASPxDateEdit DydateEddit = new ASPxDateEdit();
                        DydateEddit.ClientInstanceName = "cid" + Convert.ToString(dr["id"]);
                        DydateEddit.ID = "id" + Convert.ToString(dr["id"]);

                        //DataRow[] drPaytable = Header.Select("Formula like '%" + Convert.ToString(dr["FieldName"]) + "]%'");
                        var drPaytable = Header.AsEnumerable()
                        .Where(row => row.Field<string>("Formula").Contains(Convert.ToString(dr["FieldName"]) + "]"));

                        if (drPaytable.Count() > 0)
                        {
                            DydateEddit.ClientSideEvents.LostFocus = "SetFormula";
                        }

                        if (Request.QueryString["id"] != "Add")
                        {
                            DydateEddit.Date = Convert.ToDateTime(HeaderResult.Rows[0][Convert.ToString(dr["FieldName"]).Trim()]);
                        }

                        DydateEddit.DisplayFormatString = "dd/MM/yyyy";
                        DydateEddit.EditFormatString = "dd/MM/yyyy";
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
                        divlb.Text = "<div class='" + Convert.ToString(lrow["ClassName"]) + "'><label>" + Convert.ToString(lrow["DisplayText"]) + "</label>";
                        HeaderControlDetails.Controls.Add(divlb);

                        ASPxTextBox DyTextBox = new ASPxTextBox();
                        DyTextBox.ClientInstanceName = "cid" + Convert.ToString(dr["id"]);
                        DyTextBox.ID = "id" + Convert.ToString(dr["id"]);
                        DyTextBox.MaskSettings.Mask = "<0..999999999>.<00..99>";
                        DyTextBox.MaskSettings.AllowMouseWheel = false;

                        // DataRow[] drPaytable = Header.Select("Formula like '%" + Convert.ToString(dr["FieldName"]) + "]%'");
                        var drPaytable = Header.AsEnumerable()
                        .Where(row => row.Field<string>("Formula").Contains(Convert.ToString(dr["FieldName"]) + "]"));

                        if (drPaytable.Count() > 0)
                        {
                            DyTextBox.ClientSideEvents.LostFocus = "SetFormula";
                        }

                        if (Request.QueryString["id"] != "Add")
                        {
                            DyTextBox.Text = Convert.ToString(HeaderResult.Rows[0][Convert.ToString(dr["FieldName"]).Trim()]);
                        }
                        DyTextBox.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                        DyTextBox.ValidationSettings.Display = DevExpress.Web.Display.Dynamic;
                        HeaderControlDetails.Controls.Add(DyTextBox);

                        divlb = new Label();
                        divlb.Text = "</div>";
                        HeaderControlDetails.Controls.Add(divlb);
                    }


                    else if (Convert.ToString(dr["VissibleText"]) == "Drop Down")
                    {
                        divCount += 2;
                        divlb = new Label();
                        divlb.Text = "<div class='" + Convert.ToString(lrow["ClassName"]) + "'><label>" + Convert.ToString(lrow["DisplayText"]) + "</label>";
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


                        var drPaytable = Header.AsEnumerable()
                       .Where(row => row.Field<string>("Formula").Contains(Convert.ToString(dr["FieldName"]) + "]"));

                        if (drPaytable.Count() > 0)
                        {
                            dyCombo.ClientSideEvents.LostFocus = "SetFormula";
                        }

                        if (Request.QueryString["id"] != "Add")
                        {
                            dyCombo.Text = Convert.ToString(HeaderResult.Rows[0][Convert.ToString(dr["FieldName"]).Trim()]);
                        }
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
                        divlb.Text = "<div class='" + Convert.ToString(lrow["ClassName"]) + "'><label>" + Convert.ToString(lrow["DisplayText"]) + "</label>";
                        HeaderControlDetails.Controls.Add(divlb);

                        CustomerSelection ucSimpleControl = LoadControl("~/OMS/Management/UserForm/UserControl/CustomerSelection.ascx") as CustomerSelection;
                        ucSimpleControl.ID = "id" + Convert.ToString(dr["id"]);

                        // DataRow[] drPaytable = Header.Select("Formula like '%" + Convert.ToString(dr["FieldName"]) + "]%'");
                        var drPaytable = Header.AsEnumerable()
                        .Where(row => row.Field<string>("Formula").Contains(Convert.ToString(dr["FieldName"]) + "]"));

                        if (drPaytable.Count() > 0)
                        {
                            ucSimpleControl.SetClientSideEventChange("SetFormula");
                        }

                        if (Request.QueryString["id"] != "Add")
                        {
                            ucSimpleControl.SetName(Convert.ToString(HeaderResult.Rows[0][Convert.ToString(dr["FieldName"]).Trim()]), Convert.ToString(HeaderResult.Rows[0][Convert.ToString(dr["FieldName"]) + "_Value"]));
                        } 
                        HeaderControlDetails.Controls.Add(ucSimpleControl);

                        divlb = new Label();
                        divlb.Text = "</div>";
                        HeaderControlDetails.Controls.Add(divlb);
                    }

                    else if (Convert.ToString(dr["VissibleText"]) == "Employee Master")
                    {
                        divCount += 2;
                        divlb = new Label();
                        divlb.Text = "<div class='" + Convert.ToString(lrow["ClassName"]) + "'><label>" + Convert.ToString(lrow["DisplayText"]) + "</label>";
                        HeaderControlDetails.Controls.Add(divlb);

                        EmployeeSelection ucSimpleControl = LoadControl("~/OMS/Management/UserForm/UserControl/EmployeeSelection.ascx") as EmployeeSelection;
                        ucSimpleControl.ID = "id" + Convert.ToString(dr["id"]);
                         
                        var drPaytable = Header.AsEnumerable()
                        .Where(row => row.Field<string>("Formula").Contains(Convert.ToString(dr["FieldName"]) + "]"));

                        if (drPaytable.Count() > 0)
                        {
                            ucSimpleControl.SetClientSideEventChange("SetFormula");
                        }

                        if (Request.QueryString["id"] != "Add")
                        {
                            ucSimpleControl.SetName(Convert.ToString(HeaderResult.Rows[0][Convert.ToString(dr["FieldName"]).Trim()]), Convert.ToString(HeaderResult.Rows[0][Convert.ToString(dr["FieldName"]) + "_Value"]));
                        }
                        HeaderControlDetails.Controls.Add(ucSimpleControl);

                        divlb = new Label();
                        divlb.Text = "</div>";
                        HeaderControlDetails.Controls.Add(divlb);
                    }




                    else if (Convert.ToString(dr["VissibleText"]) == "Memo Field")
                    {
                        divCount += 6;

                        divlb = new Label();
                        divlb.Text = "<div class='" + Convert.ToString(lrow["ClassName"]) + "'><label>" + Convert.ToString(lrow["DisplayText"]) + "</label>";
                        HeaderControlDetails.Controls.Add(divlb);

                        ASPxMemo DyTextBox = new ASPxMemo();
                        DyTextBox.ClientInstanceName = "cid" + Convert.ToString(dr["id"]);
                        DyTextBox.ID = "id" + Convert.ToString(dr["id"]);


                        var drPaytable = Header.AsEnumerable()
                         .Where(row => row.Field<string>("Formula").Contains(Convert.ToString(dr["FieldName"]) + "]"));


                        if (drPaytable.Count() > 0)
                        {
                            DyTextBox.ClientSideEvents.LostFocus = "SetFormula";
                        }

                        if (Request.QueryString["id"] != "Add")
                        {
                            DyTextBox.Text = Convert.ToString(HeaderResult.Rows[0][Convert.ToString(dr["FieldName"]).Trim()]);
                        }

                        DyTextBox.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                        HeaderControlDetails.Controls.Add(DyTextBox);

                        divlb = new Label();
                        divlb.Text = "</div>";
                        HeaderControlDetails.Controls.Add(divlb);


                        divCount = 0;
                    }




                    if (Convert.ToString(lrow["ControlId"]).Trim() != "")
                    {
                        JsonforHeader obj = new JsonforHeader();
                        obj.Id = Convert.ToInt32(dr["id"]);
                        obj.Mandatory = Convert.ToBoolean(dr["Mandatory"]);
                        obj.controlType = Convert.ToString(dr["VissibleText"]);
                        obj.FieldName = Convert.ToString(dr["FieldName"]);
                        obj.Formula = Convert.ToString(dr["Formula"]);
                        jsonforHeader.Add(obj);
                    }

                }




            }

            #endregion
            else
            {

                #region Default



                foreach (DataRow dr in Header.Rows)
                {

                    if (Convert.ToString(dr["VissibleText"]) == "Text Field" || Convert.ToString(dr["VissibleText"]) == "Formula")
                    {
                        divCount += 2;
                        divlb = new Label();
                        divlb.Text = "<div class='col-md-2'><label>" + Convert.ToString(dr["FieldName"]) + "</label>";
                        HeaderControlDetails.Controls.Add(divlb);

                        ASPxTextBox DyTextBox = new ASPxTextBox();
                        DyTextBox.ClientInstanceName = "cid" + Convert.ToString(dr["id"]);
                        DyTextBox.ID = "id" + Convert.ToString(dr["id"]);


                        var drPaytable = Header.AsEnumerable()
                         .Where(row => row.Field<string>("Formula").Contains(Convert.ToString(dr["FieldName"]) + "]"));


                        if (drPaytable.Count() > 0)
                        {
                            DyTextBox.ClientSideEvents.LostFocus = "SetFormula";
                        }

                        if (Request.QueryString["id"] != "Add")
                        {
                            DyTextBox.Text = Convert.ToString(HeaderResult.Rows[0][Convert.ToString(dr["FieldName"]).Trim()]);
                        }

                        DyTextBox.MaxLength = 99;
                        HeaderControlDetails.Controls.Add(DyTextBox);

                        divlb = new Label();
                        divlb.Text = "</div>";
                        HeaderControlDetails.Controls.Add(divlb);
                    }
                    else if (Convert.ToString(dr["VissibleText"]) == "Date")
                    {
                        divCount += 2;
                        divlb = new Label();
                        divlb.Text = "<div class='col-md-2'><label>" + Convert.ToString(dr["FieldName"]) + "</label>";
                        HeaderControlDetails.Controls.Add(divlb);

                        ASPxDateEdit DydateEddit = new ASPxDateEdit();
                        DydateEddit.ClientInstanceName = "cid" + Convert.ToString(dr["id"]);
                        DydateEddit.ID = "id" + Convert.ToString(dr["id"]);

                        //DataRow[] drPaytable = Header.Select("Formula like '%" + Convert.ToString(dr["FieldName"]) + "]%'");
                        var drPaytable = Header.AsEnumerable()
                        .Where(row => row.Field<string>("Formula").Contains(Convert.ToString(dr["FieldName"]) + "]"));

                        if (drPaytable.Count() > 0)
                        {
                            DydateEddit.ClientSideEvents.LostFocus = "SetFormula";
                        }

                        if (Request.QueryString["id"] != "Add")
                        {
                            DydateEddit.Date = Convert.ToDateTime(HeaderResult.Rows[0][Convert.ToString(dr["FieldName"]).Trim()]);
                        }

                        DydateEddit.DisplayFormatString = "dd/MM/yyyy";
                        DydateEddit.EditFormatString = "dd/MM/yyyy";

                        HeaderControlDetails.Controls.Add(DydateEddit);

                        divlb = new Label();
                        divlb.Text = "</div>";
                        HeaderControlDetails.Controls.Add(divlb);
                    }

                    else if (Convert.ToString(dr["VissibleText"]) == "Numeric")
                    {
                        divCount += 2;
                        divlb = new Label();
                        divlb.Text = "<div class='col-md-2'><label>" + Convert.ToString(dr["FieldName"]) + "</label>";
                        HeaderControlDetails.Controls.Add(divlb);

                        ASPxTextBox DyTextBox = new ASPxTextBox();
                        DyTextBox.ClientInstanceName = "cid" + Convert.ToString(dr["id"]);
                        DyTextBox.ID = "id" + Convert.ToString(dr["id"]);
                        DyTextBox.MaskSettings.Mask = "<0..999999999>.<00..99>";
                        DyTextBox.MaskSettings.AllowMouseWheel = false;

                        // DataRow[] drPaytable = Header.Select("Formula like '%" + Convert.ToString(dr["FieldName"]) + "]%'");
                        var drPaytable = Header.AsEnumerable()
                        .Where(row => row.Field<string>("Formula").Contains(Convert.ToString(dr["FieldName"]) + "]"));

                        if (drPaytable.Count() > 0)
                        {
                            DyTextBox.ClientSideEvents.LostFocus = "SetFormula";
                        }

                        if (Request.QueryString["id"] != "Add")
                        {
                            DyTextBox.Text = Convert.ToString(HeaderResult.Rows[0][Convert.ToString(dr["FieldName"]).Trim()]);
                        }

                        HeaderControlDetails.Controls.Add(DyTextBox);

                        divlb = new Label();
                        divlb.Text = "</div>";
                        HeaderControlDetails.Controls.Add(divlb);
                    }


                    else if (Convert.ToString(dr["VissibleText"]) == "Drop Down")
                    {
                        divCount += 2;
                        divlb = new Label();
                        divlb.Text = "<div class='col-md-2'><label>" + Convert.ToString(dr["FieldName"]) + "</label>";
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


                        var drPaytable = Header.AsEnumerable()
                       .Where(row => row.Field<string>("Formula").Contains(Convert.ToString(dr["FieldName"]) + "]"));

                        if (drPaytable.Count() > 0)
                        {
                            dyCombo.ClientSideEvents.LostFocus = "SetFormula";
                        }

                        if (Request.QueryString["id"] != "Add")
                        {
                            dyCombo.Text = Convert.ToString(HeaderResult.Rows[0][Convert.ToString(dr["FieldName"]).Trim()]);
                        }

                        HeaderControlDetails.Controls.Add(dyCombo);

                        divlb = new Label();
                        divlb.Text = "</div>";
                        HeaderControlDetails.Controls.Add(divlb);
                    }





                    else if (Convert.ToString(dr["VissibleText"]) == "Customer Master")
                    {
                        divCount += 2;
                        divlb = new Label();
                        divlb.Text = "<div class='col-md-2'><label>" + Convert.ToString(dr["FieldName"]) + "</label>";
                        HeaderControlDetails.Controls.Add(divlb);

                        CustomerSelection ucSimpleControl = LoadControl("~/OMS/Management/UserForm/UserControl/CustomerSelection.ascx") as CustomerSelection;
                        ucSimpleControl.ID = "id" + Convert.ToString(dr["id"]);

                        // DataRow[] drPaytable = Header.Select("Formula like '%" + Convert.ToString(dr["FieldName"]) + "]%'");
                        var drPaytable = Header.AsEnumerable()
                        .Where(row => row.Field<string>("Formula").Contains(Convert.ToString(dr["FieldName"]) + "]"));

                        if (drPaytable.Count() > 0)
                        {
                            ucSimpleControl.SetClientSideEventChange("SetFormula");
                        }

                        if (Request.QueryString["id"] != "Add")
                        {
                            ucSimpleControl.SetName(Convert.ToString(HeaderResult.Rows[0][Convert.ToString(dr["FieldName"]).Trim()]), Convert.ToString(HeaderResult.Rows[0][Convert.ToString(dr["FieldName"]) + "_Value"]));
                        }

                        HeaderControlDetails.Controls.Add(ucSimpleControl);

                        divlb = new Label();
                        divlb.Text = "</div>";
                        HeaderControlDetails.Controls.Add(divlb);
                    }


                    else if (Convert.ToString(dr["VissibleText"]) == "Employee Master")
                    {
                        divCount += 2;
                        divlb = new Label();
                        divlb.Text = "<div class='col-md-2'><label>" + Convert.ToString(dr["FieldName"]) + "</label>";
                        HeaderControlDetails.Controls.Add(divlb);

                        EmployeeSelection ucSimpleControl = LoadControl("~/OMS/Management/UserForm/UserControl/CustomerSelection.ascx") as EmployeeSelection;
                        ucSimpleControl.ID = "id" + Convert.ToString(dr["id"]);

                        // DataRow[] drPaytable = Header.Select("Formula like '%" + Convert.ToString(dr["FieldName"]) + "]%'");
                        var drPaytable = Header.AsEnumerable()
                        .Where(row => row.Field<string>("Formula").Contains(Convert.ToString(dr["FieldName"]) + "]"));

                        if (drPaytable.Count() > 0)
                        {
                            ucSimpleControl.SetClientSideEventChange("SetFormula");
                        }

                        if (Request.QueryString["id"] != "Add")
                        {
                            ucSimpleControl.SetName(Convert.ToString(HeaderResult.Rows[0][Convert.ToString(dr["FieldName"]).Trim()]), Convert.ToString(HeaderResult.Rows[0][Convert.ToString(dr["FieldName"]) + "_Value"]));
                        }

                        HeaderControlDetails.Controls.Add(ucSimpleControl);

                        divlb = new Label();
                        divlb.Text = "</div>";
                        HeaderControlDetails.Controls.Add(divlb);
                    }






                    else if (Convert.ToString(dr["VissibleText"]) == "Memo Field")
                    {
                        divCount += 6;
                        divlb = new Label();
                        divlb.Text = "<div class='clear'></div>";
                        HeaderControlDetails.Controls.Add(divlb);

                        divlb = new Label();
                        divlb.Text = "<div class='col-md-6 lblmTop8'><label>" + Convert.ToString(dr["FieldName"]) + "</label>";
                        HeaderControlDetails.Controls.Add(divlb);

                        ASPxMemo DyTextBox = new ASPxMemo();
                        DyTextBox.ClientInstanceName = "cid" + Convert.ToString(dr["id"]);
                        DyTextBox.ID = "id" + Convert.ToString(dr["id"]);


                        var drPaytable = Header.AsEnumerable()
                         .Where(row => row.Field<string>("Formula").Contains(Convert.ToString(dr["FieldName"]) + "]"));


                        if (drPaytable.Count() > 0)
                        {
                            DyTextBox.ClientSideEvents.LostFocus = "SetFormula";
                        }

                        if (Request.QueryString["id"] != "Add")
                        {
                            DyTextBox.Text = Convert.ToString(HeaderResult.Rows[0][Convert.ToString(dr["FieldName"]).Trim()]);
                        }

                        DyTextBox.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                        HeaderControlDetails.Controls.Add(DyTextBox);

                        divlb = new Label();
                        divlb.Text = "</div>";
                        HeaderControlDetails.Controls.Add(divlb);


                        divCount = 0;
                    }

                    if (divCount % 12 == 0)
                    {
                        divlb = new Label();
                        divlb.Text = "<div class='clear'></div>";
                        HeaderControlDetails.Controls.Add(divlb);
                    }



                    JsonforHeader obj = new JsonforHeader();
                    obj.Id = Convert.ToInt32(dr["id"]);
                    obj.Mandatory = Convert.ToBoolean(dr["Mandatory"]);
                    obj.controlType = Convert.ToString(dr["VissibleText"]);
                    obj.FieldName = Convert.ToString(dr["FieldName"]);
                    obj.Formula = Convert.ToString(dr["Formula"]);
                    jsonforHeader.Add(obj);
                }


                #endregion

            }
            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            HeaderJson.InnerText = oSerializer.Serialize(jsonforHeader);

        }


        [WebMethod]
        public static string AddField(List<SaveData> SaveDetails, string ModuleName, string hdtagId)
        {
            string RetMsg = "-1~Error";
            DataTable HeaderTable = new DataTable();
            HeaderTable.Columns.Add("colName", typeof(System.String));
            HeaderTable.Columns.Add("result", typeof(System.String));
            foreach (var obj in SaveDetails)
            {
                DataRow dr = HeaderTable.NewRow();
                dr["colName"] = obj.ColumnName;
                dr["result"] = obj.Result;
                HeaderTable.Rows.Add(dr);
            }

            try
            {
                ProcedureExecute proc = new ProcedureExecute("prc_addEditUserDefineform");
                if (hdtagId == "Add")
                    proc.AddVarcharPara("@Action", 100, "Add");
                else
                    proc.AddVarcharPara("@Action", 100, "Edit");
                proc.AddPara("@header", HeaderTable);
                proc.AddPara("@hdtagId", hdtagId);
                proc.AddPara("@userid", HttpContext.Current.Session["userid"]);
                proc.AddVarcharPara("@ModName", 500, ModuleName);
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







        public class JsonforHeader
        {
            public int Id { get; set; }
            public string controlType { get; set; }
            public bool Mandatory { get; set; }
            public string FieldName { get; set; }
            public string Formula { get; set; }
        }

        public class SaveData
        {
            public string ColumnName { get; set; }
            public string Result { get; set; }
        }


    }
}