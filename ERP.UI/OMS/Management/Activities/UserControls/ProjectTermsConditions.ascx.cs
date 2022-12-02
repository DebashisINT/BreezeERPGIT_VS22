using BusinessLogicLayer;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities.UserControls
{
    public partial class ProjectTermsConditions : System.Web.UI.UserControl
    {



        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Request.QueryString["key"] == "ADD")
                Session["BGDetails"] = null;


            }

        }
        protected void GrdBGDetails_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

            string strSplitCommand = e.Parameters.Split('~')[0];
            if (HttpContext.Current.Session["BGDetails"] != null)
            {
                GrdBGDetails.DataBind();
            }

             if (strSplitCommand == "BindTermsAndCondition")
            {
                string strquoteId = e.Parameters.Split('~')[1];
                string Type = e.Parameters.Split('~')[2];

                SetEditTermsCoditionData(strquoteId, Type);

             }

           

        }
        protected void GrdBGDetails_DataBinding(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["BGDetails"] != null)
            {
                GrdBGDetails.DataSource = (DataTable)HttpContext.Current.Session["BGDetails"];
                //GrdBGDetails.DataBind();
            }
        }

       public DateTime DefectliabilityPeriod()
        {

            return dtDefectPerid.Date;
        }
        public string  DefectliabilityPeriodRemarks()
       {
           return txtDefectPerid.Text;
       }
        public string LiquidatedDamage()
        {
            return txtLiquiDamage.Text;
        }
        public  DateTime LiqDamageApplicableDt()
        {
            return dtLiqDmgAppliDt.Date;
        }

        public string PaymentTerms()
        {
            return txtPaymentTerms.Text;
        }

        public string OrderType()
        {
            return txtOrderType.Text;
        }
        public string NatureofWork()
        {
            return txtNatureWork.Text;
        }

          public void Save_ProjectTerms(string Doc_id,string Doc_Type)
          {

              DataTable dt = (DataTable)HttpContext.Current.Session["BGDetails"];
              
              int i;
              int rtrnvalue = 0;
              ProcedureExecute proc = new ProcedureExecute("Prc_Project_TermsConditions");
              proc.AddVarcharPara("@Action", 500, "Add");
              proc.AddBigIntegerPara("@Doc_id", Convert.ToInt64(Doc_id));
              proc.AddVarcharPara("@Doc_Type", 500, Doc_Type);
              // Rev Mantis Issue 24061 [ existing error fixed. dtDefectPerid.Date was comming as "1/1/0001 12:00:00 AM" and wrongly enetered into the if-endif block]
              //if (Convert.ToString(dtDefectPerid.Date) != "1/1/0001 12:00:00 AM")
              if (dtDefectPerid.Date.ToString("dd/MM/yyyy") != "01/01/0001") 
              // End of Rev Mantis Issue 24061
              {
                  proc.AddDateTimePara("@Terms_DefectLibilityPeriodDate", dtDefectPerid.Date);
              }

              // Rev Mantis Issue 24061 [ existing error fixed. dtDefectPerid.Date was comming as "1/1/0001 12:00:00 AM" and wrongly enetered into the if-endif block]
              //if (Convert.ToString(dtDefectPeridToDate.Date) != "1/1/0001 12:00:00 AM")
              if (dtDefectPeridToDate.Date.ToString("dd/MM/yyyy") != "01/01/0001")
              // End of Rev Mantis Issue 24061
              {
                  proc.AddDateTimePara("@Terms_DefectLibilityPeriodTODate", dtDefectPeridToDate.Date);
              }

              proc.AddVarcharPara("@Terms_DefectLibilityPeriodRemarks", 500, txtDefectPerid.Text);
              proc.AddVarcharPara("@Terms_LiqDamage", 100, txtLiquiDamage.Text);

              // Rev Mantis Issue 24061 [ existing error fixed. dtDefectPerid.Date was comming as "1/1/0001 12:00:00 AM" and wrongly enetered into the if-endif block]
              // if (Convert.ToString(dtLiqDmgAppliDt.Date) != "1/1/0001 12:00:00 AM")
              if (dtLiqDmgAppliDt.Date.ToString("dd/MM/yyyy") != "01/01/0001") 
              // End of Rev Mantis Issue 24061
              {
                  proc.AddDateTimePara("@Terms_LiqDamageAppDate", dtLiqDmgAppliDt.Date);
              }
              proc.AddVarcharPara("@Terms_Payment", 100, txtPaymentTerms.Text);
              proc.AddVarcharPara("@Terms_OrderType", 100, txtOrderType.Text);
              proc.AddVarcharPara("@Terms_NatureWork", 100, txtNatureWork.Text);

              proc.AddIntegerPara("@Terms_CreatedBy", Convert.ToInt32(Session["userid"]));
              proc.AddPara("@BGTABLE", dt);
              proc.AddVarcharPara("@ReturnValue", 50, "", QueryParameterDirection.Output);
              i = proc.RunActionQuery();
              rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
             // return rtrnvalue;
              Session["BGDetails"] = null;

          }

          public void SetEditTermsCoditionData(String DocId, String DocType)
          {
              GrdBGDetails.JSProperties["cpTermsBind"] = "";
              Session["BGDetails"] = null;
              DataSet dt=GetTermsDetails(DocId,DocType);
              DataTable BGDetails = dt.Tables[0];
              DataTable TermsDetails = dt.Tables[1];
              if (TermsDetails.Rows.Count > 0)
              {
                  dtDefectPerid.Text = Convert.ToString(TermsDetails.Rows[0]["Terms_DefectLibilityPeriodDate"]);
                  dtDefectPeridToDate.Text = Convert.ToString(TermsDetails.Rows[0]["Terms_DefectLibilityPeriodToDate"]);
                  txtDefectPerid.Text = Convert.ToString(TermsDetails.Rows[0]["Terms_DefectLibilityPeriodRemarks"]);
                  txtLiquiDamage.Text = Convert.ToString(TermsDetails.Rows[0]["Terms_LiqDamage"]);
                  dtLiqDmgAppliDt.Text = Convert.ToString(TermsDetails.Rows[0]["Terms_LiqDamageAppDate"]);
                  txtPaymentTerms.Text = Convert.ToString(TermsDetails.Rows[0]["Terms_Payment"]);
                  txtOrderType.Text = Convert.ToString(TermsDetails.Rows[0]["Terms_OrderType"]);
                  txtNatureWork.Text = Convert.ToString(TermsDetails.Rows[0]["Terms_NatureWork"]);

                  GrdBGDetails.JSProperties["cpTermsBind"] = dtDefectPerid.Text + "~" + txtDefectPerid.Text + "~" + txtLiquiDamage.Text + "~" + dtLiqDmgAppliDt.Text + "~" + txtPaymentTerms.Text + "~" + txtOrderType.Text + "~" + txtNatureWork.Text + "~" + dtDefectPeridToDate.Text;
              }
              HttpContext.Current.Session["BGDetails"] = BGDetails;
              GrdBGDetails.DataSource = (DataTable)HttpContext.Current.Session["BGDetails"];
              GrdBGDetails.DataBind();
          }

          public DataSet GetTermsDetails(string DocId, string DocType)
        {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("Prc_TermsConditionDetails");
          proc.AddBigIntegerPara("@Doc_Id", Convert.ToInt64(DocId));
          proc.AddVarcharPara("@doc_Type", 500, DocType);
          ds = proc.GetDataSet();
          return ds;
        }


         
    }

    
}