using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Configuration;

namespace ERP.OMS.Management
{
    public partial class management_frmShowReminder : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        DataTable DT = new DataTable();
        string strScrolling;
        //string callbackResult;
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            string innerHTML = showReminder();
            SpnResultId.InnerHtml = innerHTML;
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "GetTempFromServer", "context");
            String callbackScript = "function CallServer(arg, context){" + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);

        }

        private string showReminder()
        {
            try
            {
                HtmlTableCell cellScrolling = new HtmlTableCell();

                DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " rem_id AS Rid, rem_createUser AS CreaterId, (SELECT tbl_master_user.user_name FROM tbl_master_user WHERE user_id = rem_createUser) AS CreateBy, rem_createDate AS CreateDate, rem_targetUser AS TargetId,(SELECT tbl_master_user.user_name FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, rem_startDate AS StartDate, rem_endDate AS EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'NotAttened' ELSE 'Attened' END AS Status ", " CAST(rem_startDate AS DateTime) <= getdate()  AND CAST(rem_endDate AS DateTime) >= getdate()  AND ((rem_targetUser = " + HttpContext.Current.Session["userid"] + ") or (rem_inchargetargetUser = " + HttpContext.Current.Session["userid"] + ")) and (rem_displayTricker = 1) and (rem_actiontaken <> 1)");
                if (DT.Rows.Count != 0)
                {
                    strScrolling = "<Marquee onMouseover='this.stop();' OnMouseOut='this.start();' direction='horizental' scrollamount='5' border='1px #D2E8FF solid' bgcolor='' width='100%'> <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width: 100%\"><tr>";
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        if (i % 2 == 0)
                            strScrolling = strScrolling + "  <td style='color: blue;' nowrap='nowrap' id=" + DT.Rows[i][0].ToString() + " onclick='openNewPath147(" + DT.Rows[i][0].ToString() + ");' style=\"vertical-align: top;  text-align: left; font-size: 9pt; text-transform: capitalize; color: maroon; font-style: normal; font-family: Verdana, Arial; cursor: hand; \" >[" + (i + 1) + "] \" " + DT.Rows[i][8].ToString() + "  !!  </td>";
                        else
                            strScrolling = strScrolling + "  <td style='color: red;' nowrap='nowrap' id=" + DT.Rows[i][0].ToString() + " onclick='openNewPath147(" + DT.Rows[i][0].ToString() + ");' style=\"vertical-align: top;  text-align: left; font-size: 9pt; text-transform: capitalize; color: maroon; font-style: normal; font-family: Verdana, Arial; cursor: hand; \" >[" + (i + 1) + "] \" " + DT.Rows[i][8].ToString() + "  !!  </td>";
                    }
                    strScrolling = strScrolling + "</tr></table></Marquee>";
                    //SpnResultId.InnerHtml = strScrolling;

                }
                else
                    strScrolling = "";
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());

            }
            return strScrolling;
        }
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            if (eventArgument != "Parent")
            {

                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
                int NoOfRowsEffected = oDBEngine.SetFieldValue(" tbl_trans_reminder ", " rem_displayTricker=0,rem_actionTaken=2 ,rem_attendDate='" + oDBEngine.GetDate() + "'", " rem_id=" + eventArgument);
                //int noinsert = 0;
                //noinsert = oDBEngine.InsurtFieldValue("trans_Reminderremarks", "Reminderremarks_mainid,Reminderremarks_content,Reminderremarks_createuser,Reminderremarks_createdatetime", "'"+ eventArgument +"','Attend From Header Please leave Your suggestion','" + HttpContext.Current.Session["userid"].ToString() + "','" + oDBEngine.GetDate().ToString() + "'");

                strScrolling = showReminder();// +'~' + Session["mode"];
            }
            else
            {
                showReminder();
                //strScrolling = strScrolling + '~' + Session["mode"];
            }
        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return strScrolling;

        }

    }
}