using System;
using BusinessLogicLayer;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_utilities_frm_LeaveApplication_popup : System.Web.UI.Page
    {
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);

        protected void Page_Load(object sender, EventArgs e)
        {
            string ID = Request.QueryString["ID"];
            if (ID != "")
            {
                String pdCount = string.Empty;
                String PLcount = string.Empty;
                String CLcount = string.Empty;
                String SLcount = string.Empty;
                String HCcount = string.Empty;
                String HScount = string.Empty;
                for (int i = 1; i <= 3; i++)
                {
                    if (pdCount == string.Empty)
                    {
                        pdCount = " SELECT (case when ATD_STATUSDAY" + i.ToString() + "='Pd' then 1 else 0 end)";
                        PLcount = " SELECT (case when ATD_STATUSDAY" + i.ToString() + "='PL' then 1 else 0 end)";
                        CLcount = " SELECT (case when ATD_STATUSDAY" + i.ToString() + "='CL' then 1 else 0 end)";
                        SLcount = " SELECT (case when ATD_STATUSDAY" + i.ToString() + "='SL' then 1 else 0 end)";
                        HCcount = " SELECT (case when ATD_STATUSDAY" + i.ToString() + "='HC' then 1 else 0 end)";
                        HScount = " SELECT (case when ATD_STATUSDAY" + i.ToString() + "='HS' then 1 else 0 end)";

                    }
                    else
                    {
                        pdCount += " +(case when ATD_STATUSDAY" + i.ToString() + "='Pd' then 1 else 0 end)";
                        PLcount += " +(case when ATD_STATUSDAY" + i.ToString() + "='PL' then 1 else 0 end)";
                        CLcount += " +(case when ATD_STATUSDAY" + i.ToString() + "='CL' then 1 else 0 end)";
                        SLcount += " +(case when ATD_STATUSDAY" + i.ToString() + "='SL' then 1 else 0 end)";
                        HCcount += " +(case when ATD_STATUSDAY" + i.ToString() + "='HC' then 1 else 0 end)";
                        HScount += " +(case when ATD_STATUSDAY" + i.ToString() + "='HS' then 1 else 0 end)";
                    }
                }
                string TableWcond = " FROM TBL_TRANS_ATTENDANCE WHERE ATD_CNTID='" + ID + "'  and atd_month=month(getdate()) and atd_year=year(getdate())";
                pdCount += TableWcond;
                PLcount += TableWcond;
                CLcount += TableWcond;
                SLcount += TableWcond;
                HCcount += TableWcond;
                HScount += TableWcond;


                string[,] data = oDBEngine.GetFieldValue(" tbl_trans_LeaveAccountBalance", " cast(lab_PLcurrentYear-lab_PLtotalAvailedThisYear-lab_PLtotalEncashedThisYear-(" + PLcount + ") as decimal(18,2))  as PL,cast((lab_CLcurrentYear-lab_CLtotalAvailedThisYear-lab_cLtotalEncashedThisYear-(" + CLcount + ")-(" + HCcount + ")) as decimal(18,2)) as CL,cast((lab_SLcurrentYear-lab_SLtotalAvailedThisYear-lab_SLtotalEncashedThisYear-(" + SLcount + ")-(" + HScount + ")) as decimal(18,2)) as SL ", " lab_contactId='" + ID + "' ", 3);
                if (data[0, 0] != "n")
                {
                    lblPL.Text = data[0, 0];
                    lblCL.Text = data[0, 1];
                    lblSL.Text = data[0, 2];
                }
            }
        }
    }
}