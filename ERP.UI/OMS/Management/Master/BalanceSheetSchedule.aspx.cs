using DataAccessLayer;
using DevExpress.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class BalanceSheetSchedule : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                Session["GridData"] = null;
                bsgrid.DataBind();
                Session["LedgerGridData"] = GetLedgerMap();
                Session["FormulaListData"] = GetFormula();

                DataTable dt = (DataTable)Session["GridData"];



            }
        }

        private DataTable GetFormula()
        {
            DataTable DT = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_BSSchedule_Details");
            proc.AddVarcharPara("@Action", 500, "GetFormula");
            DT = proc.GetTable();
            return DT;
        }

        private DataTable GetLedgerMap()
        {
            DataTable DT = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_BSSchedule_Details");
            proc.AddVarcharPara("@Action", 500, "GetLedgerMap");
            DT = proc.GetTable();
            return DT;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object FilldllData(string Type)
        {
            ListddlData listddl = new ListddlData();


            if (HttpContext.Current.Session["userid"] != null)
            {
                Type = Type.Replace("'", "''");

                DataSet DT = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("prc_BSSchedule_Details");
                proc.AddVarcharPara("@Action", 500, "GetddlDetails");
                proc.AddVarcharPara("@AccountGroup_Type", 500, Type);
                DT = proc.GetDataSet();

                listddl.listSubGroup = (from DataRow dr in DT.Tables[0].Rows
                                        select new dllData()
                                        {
                                            ScheduleGroup_Id = Convert.ToInt32(dr["ScheduleGroup_Id"]),
                                            ScheduleGroup_Name = Convert.ToString(dr["ScheduleGroup_Name"])
                                        }).ToList();

            }

            return listddl;

        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object FilldllChildData(string ParentId, string orderId)
        {
            ListddlData listddl = new ListddlData();


            if (HttpContext.Current.Session["userid"] != null)
            {
                ParentId = ParentId.Replace("'", "''");

                DataSet DT = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("prc_BSSchedule_Details");
                proc.AddVarcharPara("@Action", 500, "GetDllDataByParentId");
                proc.AddVarcharPara("@ParentId", 500, ParentId);
                proc.AddIntegerPara("@orderId", Convert.ToInt32(orderId) + 1);

                DT = proc.GetDataSet();


                listddl.listSubGroup = (from DataRow dr in DT.Tables[0].Rows
                                        select new dllData()
                                        {
                                            ScheduleGroup_Id = Convert.ToInt32(dr["ScheduleGroup_Id"]),
                                            ScheduleGroup_Name = Convert.ToString(dr["ScheduleGroup_Name"])
                                        }).ToList();
            }

            return listddl;

        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object ShowLedger(string id)
        {
            List<LedgerDetails> listddl = new List<LedgerDetails>();


            if (HttpContext.Current.Session["userid"] != null)
            {
                id = id.Replace("'", "''");

                DataTable dsInst = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_BSSchedule_Details");
                proc.AddVarcharPara("@Action", 500, "ViewLedger");
                proc.AddVarcharPara("@id", 500, id);
                dsInst = proc.GetTable();


                listddl = (from DataRow dr in dsInst.Rows
                           select new LedgerDetails()
                           {
                               LedgerCode = Convert.ToString(dr["Ledger_Code"]),
                               LedgerName = Convert.ToString(dr["Ledger_Name"])
                           }).ToList();
            }

            return listddl;

        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object ViewLayout()
        {
            List<ViewLayout> listddl = new List<ViewLayout>();

            string unorderedList = "";
            if (HttpContext.Current.Session["userid"] != null)
            {

                DataTable dsInst = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_BSSchedule_Details");
                proc.AddVarcharPara("@Action", 500, "ViewLayout");
                dsInst = proc.GetTable();


                DataRow[] parentMenus = dsInst.Select("[Parent]=0");

                dynamic sb = new StringBuilder();
                unorderedList = GenerateUL(parentMenus, dsInst, sb);


                //listddl = (from DataRow dr in dsInst.Rows
                //           select new ViewLayout()
                //           {
                //               AccountGroup_Name = Convert.ToString(dr["AccountGroup_Name"]),

                //           }).ToList();
            }

            return unorderedList;

        }



        protected void bsgrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string Data = e.Parameters;
            if (Data == "AddData")
            {
                if (Session["GridData"] != null)
                {
                    try
                    {
                        DataTable dtBs = new DataTable();
                        dtBs = (DataTable)Session["GridData"];




                        string Type = "", CAT_NAME = "", GROUP_NAME = "", SUBGROUP1_NAME = "", SUBGROUP2_NAME = "", SUBGROUP3_NAME = "";

                        int CAT_ID = 0, GROUP_ID = 0, SUBGROUP1_ID = 0, SUBGROUP2_ID = 0, SUBGROUP3_ID = 0, ID = 0, SL = 0;
                        if (dtBs.Rows.Count > 0)
                            ID = Convert.ToInt32(dtBs.Compute("MAX(ID)", "")) + 1;
                        else
                            ID = 1;



                        SL = Convert.ToInt32(txtSl.Text);
                        Type = Convert.ToString(cmbType.Value);
                        CAT_ID = Convert.ToInt32(cmbCategory.Value);
                        if (CAT_ID != 0)
                            CAT_NAME = Convert.ToString(cmbCategory.Text);

                        GROUP_ID = Convert.ToInt32(cmbGroup.Value);
                        if (GROUP_ID != 0)
                            GROUP_NAME = Convert.ToString(cmbGroup.Text);


                        SUBGROUP1_ID = Convert.ToInt32(cmbSubGroup1.Value);
                        if (SUBGROUP1_ID != 0)
                            SUBGROUP1_NAME = Convert.ToString(cmbSubGroup1.Text);

                        SUBGROUP2_ID = Convert.ToInt32(cmbSubGroup2.Value);
                        if (SUBGROUP2_ID != 0)
                            SUBGROUP2_NAME = Convert.ToString(cmbSubGroup2.Text);

                        SUBGROUP3_ID = Convert.ToInt32(cmbSubGroup3.Value);
                        if (SUBGROUP3_ID != 0)
                            SUBGROUP3_NAME = Convert.ToString(cmbSubGroup3.Text);




                        string Query = "";
                        if (SUBGROUP3_ID != 0)
                        {
                            Query = "SUBGROUP1_ID=" + SUBGROUP1_ID + "and SUBGROUP2_ID=" + SUBGROUP2_ID;
                        }
                        else
                        {
                            if (SUBGROUP2_ID != 0)
                            {
                                Query = "GROUP_ID=" + GROUP_ID + " and  SUBGROUP1_ID=" + SUBGROUP1_ID + " and  SUBGROUP2_ID=0";
                            }
                            else
                            {
                                if (SUBGROUP1_ID != 0)
                                {
                                    Query = "CAT_ID=" + CAT_ID + " and  GROUP_ID=" + GROUP_ID + " and  SUBGROUP1_ID=0";
                                }
                                else
                                {
                                    if (GROUP_ID != 0)
                                    {
                                        Query = "Type='" + Type + "' and  CAT_ID=" + CAT_ID + " and  GROUP_ID=0";
                                    }
                                    else
                                    {
                                        if (CAT_ID != 0)
                                        {
                                            Query = "Type='" + Type + "' and  CAT_ID=0";
                                        }
                                    }
                                }
                            }
                        }

                        DataView DVParent = new DataView(dtBs);
                        DVParent.RowFilter = Query;
                        DataTable DTParent = DVParent.ToTable();

                        if (DTParent == null || DTParent.Rows.Count == 0)
                        {
                            bsgrid.JSProperties["cpSuccess"] = "ParentNotFound";
                            return;
                        }


                        DataView DV = new DataView(dtBs);
                        DV.RowFilter = "Type='" + Type + "' and CAT_ID=" + CAT_ID + " and GROUP_ID=" + GROUP_ID + " and  SUBGROUP1_ID=" + SUBGROUP1_ID + " and SUBGROUP2_ID=" + SUBGROUP2_ID + "and SUBGROUP3_ID=" + SUBGROUP3_ID;
                        DataTable DT = DV.ToTable();

                        if (DT != null && DT.Rows.Count > 0)
                        {
                            bsgrid.JSProperties["cpSuccess"] = "DuplicateRow";
                            return;
                        }








                        foreach (DataRow dr in dtBs.Rows)
                        {
                            if (Convert.ToInt32(dr["SL"]) >= SL)
                            {
                                dr["SL"] = Convert.ToInt32(dr["SL"]) + 1;
                            }
                        }

                        dtBs.AcceptChanges();








                        dtBs.Rows.Add(ID, SL, Type, CAT_ID, CAT_NAME, GROUP_ID, GROUP_NAME, SUBGROUP1_ID, SUBGROUP1_NAME, SUBGROUP2_ID, SUBGROUP2_NAME, SUBGROUP3_ID, SUBGROUP3_NAME, "display : none;", "display : none;", "display : none;", "display : none;");
                        DataView dvBs = new DataView(dtBs);
                        dvBs.Sort = "SL ASC";
                        DataTable sortedDT = dvBs.ToTable();
                        Session["GridData"] = sortedDT;
                        bsgrid.JSProperties["cpSuccess"] = "SucsessAdd";
                        bsgrid.DataBind();
                    }
                    catch
                    {
                        bsgrid.JSProperties["cpSuccess"] = "Problem";
                    }


                }
            }
            if (Data == "SaveData")
            {
                DataTable FinalDt = (DataTable)Session["GridData"];
                FinalDt.Columns.Remove("CAT_NAME");
                FinalDt.Columns.Remove("GROUP_NAME");
                FinalDt.Columns.Remove("SUBGROUP1_NAME");
                FinalDt.Columns.Remove("SUBGROUP2_NAME");
                FinalDt.Columns.Remove("SUBGROUP3_NAME");
                FinalDt.Columns.Remove("Ledger_Visible");
                FinalDt.Columns.Remove("isFormula");
                FinalDt.Columns.Remove("Delete_Visible");
                FinalDt.Columns.Remove("ISScheduleVisible");
                if (FinalDt.Rows.Count == 0)
                {
                    bsgrid.JSProperties["cpSuccess"] = "NoDataFound";
                    return;
                }





                DataTable Formuladt = (DataTable)Session["FormulaListData"];

                DataTable NewFormuladt = new DataTable();

                NewFormuladt.Merge(Formuladt);

                NewFormuladt.Columns.Remove("TEXT");

                DataTable DtTable = (DataTable)Session["LedgerGridData"];

                DataTable DT = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_BSSchedule_Details");
                proc.AddVarcharPara("@Action", 500, "SaveData");
                proc.AddPara("@GridDetails", FinalDt);
                proc.AddPara("@LedgerDetails", DtTable);
                proc.AddPara("@FormulaDetails", NewFormuladt);
                DT = proc.GetTable();
                // bsgrid.DataBind();
                Session["GridData"] = null;
                Session["LedgerGridData"] = null;
                //Session["FormulaListData"] = GetFormula();
                //bsgrid.DataBind();
                bsgrid.JSProperties["cpSuccess"] = "DataSaved";
            }

            if (e.Parameters.ToString().Split('~')[0] == "Delete")
            {
                DataTable FinalDt = (DataTable)Session["GridData"];

                DataRow[] drrow = FinalDt.Select("ID=" + Convert.ToString(e.Parameters.ToString().Split('~')[1]));
                int Sl = Convert.ToInt32(drrow[0]["SL"]);

                //foreach (DataRow dr in FinalDt.Rows)
                //{
                //    if (Convert.ToString(dr["ID"]) == Convert.ToString(e.Parameters.ToString().Split('~')[1]))
                //    {
                //        dr.Delete();
                //    }
                //}

                DataTable Formuladt = (DataTable)Session["FormulaListData"];

                foreach (DataRow dr in Formuladt.Rows)
                {
                    if (Convert.ToString(dr["Schedule_id"]) == Convert.ToString(e.Parameters.ToString().Split('~')[1]))
                    {
                        bsgrid.DataBind();
                        bsgrid.JSProperties["cpSuccess"] = "FormulaDetected";
                        return;
                    }
                }





                foreach (DataRow dr in drrow)
                {
                    dr.Delete();

                }


                FinalDt.AcceptChanges();

                foreach (DataRow dr in FinalDt.Rows)
                {
                    if (Convert.ToInt32(dr["SL"]) > Sl)
                    {
                        dr["SL"] = Convert.ToInt32(dr["SL"]) - 1;
                    }
                }

                FinalDt.AcceptChanges();


                DataTable dtt = (DataTable)Session["LedgerGridData"];

                DataRow[] dttdR = dtt.Select("BSSchedule_Id=" + Convert.ToString(e.Parameters.ToString().Split('~')[1]));


                foreach (DataRow dr in dttdR)
                {

                    dr.Delete();

                }
                dtt.AcceptChanges();





                DataRow[] drrowFormula = Formuladt.Select("parent_id=" + Convert.ToString(e.Parameters.ToString().Split('~')[1]));

                foreach (DataRow dr in drrowFormula)
                {
                    dr.Delete();
                }

                DataRow[] drrowFormule = Formuladt.Select("Schedule_id=" + Convert.ToString(e.Parameters.ToString().Split('~')[1]));

                foreach (DataRow dr in drrowFormula)
                {
                    dr.Delete();
                }



                Formuladt.AcceptChanges();



                Session["GridData"] = FinalDt;
                Session["LedgerGridData"] = dtt;
                //Session["FormulaListData"] = Formuladt;
                bsgrid.DataBind();
                bsgrid.JSProperties["cpSuccess"] = "DataDeleted";
            }
        }
        protected void bsgrid_DataBinding(object sender, EventArgs e)
        {
            if (Session["GridData"] != null)
            {
                bsgrid.DataSource = (DataTable)Session["GridData"];
            }
            else
            {
                DataTable DT = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_BSSchedule_Details");
                proc.AddVarcharPara("@Action", 500, "BindGrid");
                DT = proc.GetTable();
                DataView dvBs = new DataView(DT);
                dvBs.Sort = "SL ASC";
                DataTable sortedDT = dvBs.ToTable();
                Session["GridData"] = sortedDT;
                bsgrid.DataSource = (DataTable)Session["GridData"];
            }
        }
        protected void LedgerGrid_DataBinding(object sender, EventArgs e)
        {
            DataTable dtt = new DataTable();
            if (Session["LedgerDetails"] == null)
            {
                BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
                string id = Convert.ToString(GroupId.Value);
                dtt = (DataTable)Session["LedgerDetails"];
                //dtt = oGenericMethod.GetDataTable("select MainAccount_ReferenceID LedgerCode,MainAccount_AccountCode LedgerCodeName,''GroupCode,'' GroupName,MainAccount_Name LedgerName from Master_MainAccount where MainAccount_AccountType in ('Income','Expenses') and MainAccount_ReferenceID not in (select LEDGER_MAP_LEDGERID from TBL_TRANS_LAYOUTDETAILS_PL_LEDGER_MAP where LEDGER_MAP_LAYOUT_ID=" + Convert.ToInt32(Request.QueryString["key"]) + " AND LEDGER_MAP_LAYOUTDETAILS_ID<>" + id + ")");
            }
            else
            {
                dtt = (DataTable)Session["LedgerDetails"];
            }
            LedgerGrid.DataSource = dtt;
            // Session["LedgerDetails"] = null;
        }
        protected void LedgerPanel_Callback(object sender, CallbackEventArgsBase e)
        {

            LedgerPanel.JSProperties["cpAutoID"] = "";
            if (e.Parameter.ToString().Split('~')[0] == "ShowDetails")
            {


                string id = e.Parameter.ToString().Split('~')[1];
                string Type = e.Parameter.ToString().Split('~')[2];

                DataTable dsInst = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_BSSchedule_Details");
                proc.AddVarcharPara("@Action", 500, "GetLedger");
                proc.AddVarcharPara("@AccountGroup_Type", 500, Type);
                proc.AddVarcharPara("@id", 500, id);
                dsInst = proc.GetTable();



                DataTable DocDt = new DataTable();
                DataTable DtTable = new DataTable();
                DtTable = (DataTable)Session["LedgerGridData"];


                var rowsOnlyInDt1 = from r in dsInst.AsEnumerable()
                                    where !DtTable.AsEnumerable().Any(r2 => r["Ledger_Id"].ToString().Trim().ToLower() == r2["Ledger_Id"].ToString().Trim().ToLower() && r2["BSSchedule_Id"].ToString().Trim().ToLower() != id)
                                    select r;
                if (rowsOnlyInDt1.Any())
                    DocDt = rowsOnlyInDt1.CopyToDataTable();

                Session["LedgerDetails"] = DocDt;
                LedgerGrid.DataSource = DocDt;
                LedgerGrid.DataBind();

                DataTable dttKey = (DataTable)Session["LedgerGridData"];
                LedgerGrid.Selection.UnselectAll();
                foreach (DataRow dr in dttKey.Rows)
                {
                    LedgerGrid.Selection.SelectRowByKey(Convert.ToString(dr["Ledger_id"]));
                }

            }

            if (e.Parameter.ToString().Split('~')[0] == "SaveMap")
            {

                string id = e.Parameter.ToString().Split('~')[1];
                string Type = e.Parameter.ToString().Split('~')[2];
                var SelectList = LedgerGrid.GetSelectedFieldValues("Ledger_id");

                DataTable DtTable = new DataTable();
                DtTable = (DataTable)Session["LedgerGridData"];

                DataRow[] drDelete = DtTable.Select("BSSchedule_Id='" + Convert.ToString(id) + "'");

                foreach (DataRow item in drDelete)
                {
                    //if (Convert.ToString(id) == Convert.ToString(item["BSSchedule_Id"]))
                    //{
                    item.Delete();
                    //}
                }
                DtTable.AcceptChanges();
                foreach (var item in SelectList)
                {
                    DtTable.Rows.Add(Convert.ToInt32(item), id);
                }
                Session["LedgerGridData"] = DtTable;
                LedgerPanel.JSProperties["cpAutoID"] = "Sucsess";

            }

        }

        protected void FormulaPopup_Callback(object sender, CallbackEventArgsBase e)
        {
            FormulaPopupCallBack.JSProperties["cpSuccess"] = "";
            if (e.Parameter.ToString().Split('~')[0] == "ShowDetails")
            {


                DataTable Formuladt = (DataTable)Session["FormulaListData"];

                DataView dvFormula = new DataView(Formuladt);
                dvFormula.RowFilter = "parent_id=" + Convert.ToString(e.Parameter.ToString().Split('~')[1]);
                Formuladt = dvFormula.ToTable();



                DataTable FinalDt = (DataTable)Session["GridData"];

                DataTable dtAvailable = new DataTable();
                dtAvailable.Columns.Add("ID", typeof(String));
                dtAvailable.Columns.Add("Text", typeof(String));

                foreach (DataRow dr in FinalDt.Rows)
                {
                    string ID = Convert.ToString(dr["ID"]);
                    if (ID != Convert.ToString(e.Parameter.ToString().Split('~')[1]))
                    {
                        string Text = "";
                        if (Convert.ToString(dr["SUBGROUP3_NAME"]) != "")
                        {
                            Text = Convert.ToString(dr["SUBGROUP3_NAME"]);
                            goto Finish;
                        }
                        else
                        {
                            if (Convert.ToString(dr["SUBGROUP2_NAME"]) != "")
                            {
                                Text = Convert.ToString(dr["SUBGROUP2_NAME"]);
                                goto Finish;
                            }
                            else
                            {
                                if (Convert.ToString(dr["SUBGROUP1_NAME"]) != "")
                                {
                                    Text = Convert.ToString(dr["SUBGROUP1_NAME"]);
                                    goto Finish;
                                }
                                else
                                {
                                    if (Convert.ToString(dr["GROUP_NAME"]) != "")
                                    {
                                        Text = Convert.ToString(dr["GROUP_NAME"]);
                                        goto Finish;
                                    }
                                    else
                                    {
                                        if (Convert.ToString(dr["CAT_NAME"]) != "")
                                        {
                                            Text = Convert.ToString(dr["CAT_NAME"]);
                                            goto Finish;
                                        }
                                        else
                                        {
                                            Text = Convert.ToString(dr["Type"]);
                                            goto Finish;
                                        }
                                    }
                                }
                            }
                        }

                    Finish:
                        dtAvailable.Rows.Add(ID, Text);
                    }
                }




                var rowsOnlyInDt1 = from r in dtAvailable.AsEnumerable()
                                    where !Formuladt.AsEnumerable().Any(r2 => r["ID"].ToString().Trim().ToLower() == r2["Schedule_id"].ToString().Trim().ToLower())
                                    select r;
                if (rowsOnlyInDt1.Any())
                    dtAvailable = rowsOnlyInDt1.CopyToDataTable();





                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                foreach (DataRow dr in dtAvailable.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dtAvailable.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }

                string jsondataAvailable = serializer.Serialize(rows);


                System.Web.Script.Serialization.JavaScriptSerializer serializer1 = new System.Web.Script.Serialization.JavaScriptSerializer();
                List<Dictionary<string, object>> rows1 = new List<Dictionary<string, object>>();
                Dictionary<string, object> row1;
                foreach (DataRow dr in Formuladt.Rows)
                {
                    row1 = new Dictionary<string, object>();
                    foreach (DataColumn col in Formuladt.Columns)
                    {
                        row1.Add(col.ColumnName, dr[col]);
                    }
                    rows1.Add(row1);
                }

                string jsondataChoosen = serializer1.Serialize(rows1);

                FormulaPopupCallBack.JSProperties["cpBind"] = jsondataAvailable + "~" + jsondataChoosen;
                //lbChoosen.TextField = "Text";
                //lbChoosen.ValueField = "Schedule_id";
                //lbChoosen.DataSource = Formuladt;
                //lbChoosen.DataBind();



                //lbAvailable.TextField = "Text";
                //lbAvailable.ValueField = "ID";
                //lbAvailable.DataSource = dtAvailable;
                //lbAvailable.DataBind();
            }
            else if (e.Parameter.ToString().Split('~')[0] == "SaveDetails")
            {
                DataTable dtFormula = new DataTable();
                dtFormula = Session["FormulaListData"] as DataTable;
                string FormulaId = e.Parameter.ToString().Split('~')[1];



                DataRow[] drDelete = dtFormula.Select("parent_id='" + Convert.ToString(FormulaId) + "'");

                foreach (DataRow item in drDelete)
                {
                    //if (Convert.ToString(id) == Convert.ToString(item["BSSchedule_Id"]))
                    //{
                    item.Delete();
                    //}
                }





                //foreach (DataRow dr in dtFormula.Rows)
                //{
                //    if (Convert.ToString(dr["parent_id"]) == FormulaId)
                //    {
                //        dr.Delete();
                //    }
                //}
                dtFormula.AcceptChanges();

                string jsonString = hdnChoosen.Value;
                List<jsonFetch> UserList = JsonConvert.DeserializeObject<List<jsonFetch>>(jsonString);



                foreach (jsonFetch item in UserList)
                {




                    if (Convert.ToString(item.ScheduleGroup_Name).StartsWith("+"))
                    {
                        dtFormula.Rows.Add(FormulaId, "+", "+");
                    }
                    else if (Convert.ToString(item.ScheduleGroup_Name).StartsWith("-"))
                    {
                        dtFormula.Rows.Add(FormulaId, "-", "-");
                    }
                    else
                    {
                        dtFormula.Rows.Add(FormulaId, item.ScheduleGroup_Id, item.ScheduleGroup_Name);
                    }


                    //if (Convert.ToString(item.Value).StartsWith("+"))
                    //{
                    //    Formula = Formula + "+";
                    //}
                    //else if (Convert.ToString(item.Value).StartsWith("-"))
                    //{
                    //    Formula = Formula + "-";
                    //}
                    //else
                    //{
                    //    Formula = Formula + "[" + item.Value +"]";
                    //}

                }
                Session["FormulaListData"] = dtFormula;
                FormulaPopupCallBack.JSProperties["cpSuccess"] = "Sucsess";

            }
        }


        private static string GenerateUL(DataRow[] menu, DataTable table, StringBuilder sb)
        {
            sb.AppendLine("<ul>");

            if (menu.Length > 0)
            {
                foreach (DataRow dr in menu)
                {
                    //Dim handler As String = dr("Handler").ToString()
                    string menuText = dr["Name"].ToString();
                    string line = String.Format("<li>{0}", menuText);
                    sb.Append(line);

                    string pid = dr["Id"].ToString();

                    DataRow[] subMenu = table.Select(String.Format("[Parent] = {0}", pid));
                    if (subMenu.Length > 0)
                    {
                        dynamic subMenuBuilder = new StringBuilder();
                        sb.Append(GenerateUL(subMenu, table, subMenuBuilder));
                    }
                    sb.Append("</li>");
                }
            }

            sb.Append("</ul>");
            return sb.ToString();
        }
        public void bindexport(int Filter)
        {
            exporter.GridViewID = "bsgrid";
            string filename = "Balance Sheet - Schedule VI";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Balance Sheet - Schedule VI";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";

            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
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
                case 5:
                    exporter.WriteXlsxToResponse();
                    break;
            }
        }
        protected void drdCashBankExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            int Filter = 2;
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    bindexport(2);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    bindexport(2);
                }
            }
        }

        protected void SchedulePopUpCallBack_Callback(object sender, CallbackEventArgsBase e)
        {
            SchedulePopUpCallBack.JSProperties["cpSuccess"] = "";
            string Id = Convert.ToString(e.Parameter.ToString().Split('~')[1]);
            if (e.Parameter.ToString().Split('~')[0] == "ShowDetails")
            {
                DataTable Formuladt = (DataTable)Session["GridData"];

                DataView dvFormula = new DataView(Formuladt);
                dvFormula.RowFilter = "ID=" + Convert.ToString(e.Parameter.ToString().Split('~')[1]);
                Formuladt = dvFormula.ToTable();

                if (Formuladt != null && Formuladt.Rows.Count > 0)
                {
                    txtSchedule.Text = Convert.ToString(Formuladt.Rows[0]["Schedule_Number"]);
                }
                else
                {
                    txtSchedule.Text = "";
                }

                SchedulePopUpCallBack.JSProperties["cpSuccess"] = "ShowSuccess";
            }
            if (e.Parameter.ToString().Split('~')[0] == "SaveDetails")
            {
                DataTable DtTable = (DataTable)Session["GridData"];
                DataRow[] drDelete = DtTable.Select("ID='" + Convert.ToString(Id) + "'");

                foreach (DataRow item in drDelete)
                {
                    item["Schedule_Number"] = Convert.ToString(txtSchedule.Text == "" ? "" : txtSchedule.Text);
                }
                DtTable.AcceptChanges();

                Session["GridData"] = DtTable;
                bsgrid.DataBind();
                SchedulePopUpCallBack.JSProperties["cpSuccess"] = "SaveSuccess";

            }

        }
    }


    public class dllData
    {
        public Int32 ScheduleGroup_Id { get; set; }
        public string ScheduleGroup_Name { get; set; }
    }

    public class JsonData
    {
        public List<jsonFetch> data { get; set; }
    }
    public class jsonFetch
    {
        public string ScheduleGroup_Id { get; set; }
        public string ScheduleGroup_Name { get; set; }
    }

    public class ListddlData
    {
        public List<dllData> listSubGroup = new List<dllData>();

    }

    public class LedgerDetails
    {
        public string LedgerCode { get; set; }
        public string LedgerName { get; set; }
    }

    public class ViewLayout
    {
        public string AccountGroup_Name { get; set; }
    }





}