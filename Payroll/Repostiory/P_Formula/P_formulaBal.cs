using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Payroll.Models;
using DataAccessLayer;
using DataAccessLayer.Model;

namespace Payroll.Repostiory.P_Formula
{
    public class P_formulaBal : IPayrole_formula
    {
        public void save(FormulaApply apply, ref string tblformulaid, ref int ReturnCode, ref string ReturnMsg)
        {

            string action = string.Empty;
            DataTable formula_dtls = new DataTable();
            DataSet dsInst = new DataSet();

            try
            {

                formula_dtls.Columns.Add("low", typeof(decimal));
                formula_dtls.Columns.Add("high", typeof(decimal));
                formula_dtls.Columns.Add("value", typeof(decimal));

                foreach (P_formula_dtls dtls in apply.dtls)
                {
                    DataRow dr = formula_dtls.NewRow();
                    dr["low"] = dtls.low;
                    dr["high"] = dtls.high;
                    dr["value"] = dtls.value;
                    formula_dtls.Rows.Add(dr);
                }
                
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ExecProcedure execProc = new ExecProcedure();
                    List<KeyObj> paramList = new List<KeyObj>();
                    execProc.ProcedureName = "proll_FormulaADDModify";
                    if (apply.header.TableBreakUpId!=null)
                    {
                        paramList.Add(new KeyObj("@Action", "EditFormula"));
                        paramList.Add(new KeyObj("@TableBreakUpId", apply.header.TableBreakUpId));
                    }
                    else
                    {
                        paramList.Add(new KeyObj("@Action", "AddFormula"));
                    }
                   
                    paramList.Add(new KeyObj("@user_id", user_id));
                    paramList.Add(new KeyObj("@TableName", apply.header.table));
                    paramList.Add(new KeyObj("@TableFormulaCode", apply.header.tableFormulaCode));
                    paramList.Add(new KeyObj("@TableCode", apply.header.short_nm));
                    paramList.Add(new KeyObj("@Applicable_frm", apply.header.applicbl_frm));
                    paramList.Add(new KeyObj("@Applicable_to", apply.header.applicbl_to));
                    paramList.Add(new KeyObj("@PARAMTABLE", formula_dtls));
                    paramList.Add(new KeyObj("@ReturnMessage", ReturnMsg, true));
                    paramList.Add(new KeyObj("@ReturnCode", ReturnCode, true));
                    paramList.Add(new KeyObj("@TableFormulaId", tblformulaid, true));
                    execProc.param = paramList;
                    execProc.ExecuteProcedureNonQuery();
                    paramList.Clear();
                    ReturnMsg = Convert.ToString(execProc.outputPara[0].value);
                    ReturnCode = Convert.ToInt32(execProc.outputPara[1].value);
                    tblformulaid = Convert.ToString(execProc.outputPara[2].value);
                }


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string Delete(string ActionType, string id, ref int ReturnCode)
        {
            string output = string.Empty;
            string action = string.Empty;
            int NoOfRowEffected = 0;

            DataTable formula_dtls = new DataTable();

            try
            {
                if (ActionType == "Delete")
                {
                    action = "DeleteFormula";
                }

                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ExecProcedure execProc = new ExecProcedure();
                    List<KeyObj> paramList = new List<KeyObj>();
                    execProc.ProcedureName = "proll_FormulaADDModify";
                    paramList.Add(new KeyObj("@Action", action));
                    paramList.Add(new KeyObj("@user_id", user_id));//ADDEDITCATEGORYIMPORT
                    paramList.Add(new KeyObj("@TableFormulaCode", id));

                    paramList.Add(new KeyObj("@ReturnMessage", output, true));
                    paramList.Add(new KeyObj("@ReturnCode", ReturnCode, true));
                    execProc.param = paramList;
                    execProc.ExecuteProcedureNonQuery();
                    paramList.Clear();
                    NoOfRowEffected = execProc.NoOfRows;
                    output = Convert.ToString(execProc.outputPara[0].value);
                    ReturnCode = Convert.ToInt32(execProc.outputPara[1].value);
                }


            }
            catch (Exception ex)
            {
                throw;
            }



            return output;

        }

        public FormulaApply getFormulaDetailsById(string _formulaCode, string EditFlag, int TableBreakup_ID)
        {
            DataSet _getFormulaDtls = new DataSet();
            FormulaApply _apply = new FormulaApply();
            List<P_formula_dtls> items = new List<P_formula_dtls>();
            P_formula_header _header = new P_formula_header();
            try
            {
                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "proll_TableFormulaDetails";
                if (EditFlag=="I")
                {
                    paramList.Add(new KeyObj("@Action", "PopulateBreakUpDetails"));
                    paramList.Add(new KeyObj("@TableBreakUpId", TableBreakup_ID));
                }

                else
                {
                    paramList.Add(new KeyObj("@Action", "PopulateFormulaDetails"));
                }
               
                paramList.Add(new KeyObj("@TableFormulaCode", _formulaCode));
               
                execProc.param = paramList;
                _getFormulaDtls = execProc.ExecuteProcedureGetDataSet();

                if (_getFormulaDtls.Tables[0].Rows.Count > 0)
                {
                    _header.table = _getFormulaDtls.Tables[0].Rows[0]["TableName"].ToString();
                    _header.short_nm = _getFormulaDtls.Tables[0].Rows[0]["TableCode"].ToString();
                    _header.tableFormulaCode = _getFormulaDtls.Tables[0].Rows[0]["TableFormulaCode"].ToString();


                    if (EditFlag == "I")
                    {
                        _header.applicbl_frm = Convert.ToDateTime(_getFormulaDtls.Tables[0].Rows[0]["ApplicatedFrom"]);
                        _header.applicbl_to = Convert.ToDateTime(_getFormulaDtls.Tables[0].Rows[0]["ApplicatedTo"]);
                        _header.TableBreakUpId = Convert.ToInt32(_getFormulaDtls.Tables[0].Rows[0]["TableBreakup_ID"]);
                    
                        foreach (DataRow dr in _getFormulaDtls.Tables[1].Rows)
                        {

                            items.Add(new P_formula_dtls
                            {
                                low = dr["LowValue"].ToString(),
                                high = dr["HighValue"].ToString(),
                                value = dr["ResultValue"].ToString(),
                                TableFormulaDetail_ID = dr["TableFormulaDetail_ID"].ToString()

                            });

                        }

                        _apply.dtls = items;
                    }
                    _apply.header = _header;


                }

            }
            catch (Exception ex)
            {

            }

            return _apply;
        }
    }
}