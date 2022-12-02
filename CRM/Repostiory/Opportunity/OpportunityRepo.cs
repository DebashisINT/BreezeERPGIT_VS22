using CRM.Models;
using DataAccessLayer;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CRM.Repostiory.Opportunity
{
    public class OpportunityRepo : IOpportunity
    {
        public List<QuotationDetailsList> getVerifyId(string _AccountID)
        {
            DataSet _getOpeverify = new DataSet();
            QuotationDetailsList _applyverify = new QuotationDetailsList();
            List<QuotationDetailsList> _applyverifyList = new List<QuotationDetailsList>();
            //EnquiriesDet _header = new EnquiriesDet();
            try
            {
                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "PRC_CRMQuotationDetails";
                paramList.Add(new KeyObj("@Customer_Id", _AccountID));
               
                execProc.param = paramList;
                _getOpeverify = execProc.ExecuteProcedureGetDataSet();

                if (_getOpeverify.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in _getOpeverify.Tables[0].Rows)
                    {
                        _applyverify = new QuotationDetailsList();
                        _applyverify.Quote_Number = dr["Quote_Number"].ToString();
                        _applyverify.Customer_Id = dr["Customer_Id"].ToString();
                        _applyverify.Quote_Date = Convert.ToDateTime(dr["Quote_Date"]);
                        _applyverify.Quote_Expiry = Convert.ToDateTime(dr["Quote_Expiry"]);
                        _applyverify.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                        _applyverify.Quote_TotalAmount = Convert.ToDecimal(dr["Quote_TotalAmount"]);
                        _applyverify.Quote_Id = dr["Quote_Id"].ToString();
                        _applyverifyList.Add(_applyverify);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return _applyverifyList;
        }

        public List<QuotationDetailsList> getQuotDetails(string _AccountID)
        {
            DataSet _getOpeverify = new DataSet();
            QuotationDetailsList _applyverify = new QuotationDetailsList();
            List<QuotationDetailsList> _applyverifyList = new List<QuotationDetailsList>();
            //EnquiriesDet _header = new EnquiriesDet();
            try
            {
                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "PRC_CRMQuotationDetails";
                paramList.Add(new KeyObj("@Customer_Id", _AccountID));

                execProc.param = paramList;
                _getOpeverify = execProc.ExecuteProcedureGetDataSet();

                if (_getOpeverify.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in _getOpeverify.Tables[0].Rows)
                    {
                        _applyverify = new QuotationDetailsList();
                        _applyverify.Quote_Number = dr["Quote_Number"].ToString();
                        _applyverify.Customer_Id = dr["Customer_Id"].ToString();
                        _applyverify.Quote_Date = Convert.ToDateTime(dr["Quote_Date"]);
                        _applyverify.Quote_Expiry = Convert.ToDateTime(dr["Quote_Expiry"]);
                        _applyverify.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                        _applyverify.Quote_TotalAmount = Convert.ToDecimal(dr["Quote_TotalAmount"]);
                        _applyverify.Quote_Id = dr["Quote_Id"].ToString();
                        _applyverifyList.Add(_applyverify);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return _applyverifyList;
        }
    }
}