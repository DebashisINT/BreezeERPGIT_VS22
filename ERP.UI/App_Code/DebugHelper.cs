using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BusinessLogicLayer;
/// <summary>
/// Summary description for DebugHelper
/// </summary>
public class DebugHelper
{
    //DBEngine oDbEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"].ToString());  MULTI
    DBEngine oDbEngine = new DBEngine();
	public DebugHelper()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public void GetPrepareSpToRun(string SpName, string[] InputName, string[] InputType, string[] InputValue, string SegmentID, string ExchangeSegmentID)
    {
        string[] InputNameClone = (string[])InputName.Clone();
        string[] InputTypeClone = (string[])InputType.Clone();
        string[] InputValueClone = (string[])InputValue.Clone();
        int LoopCnt;
        string PreparedQueryToSp = "Execute " + SpName;

        for (LoopCnt = 0; LoopCnt < InputNameClone.Length; LoopCnt++)
        {
            if (InputTypeClone[LoopCnt].ToString() == "I")
            {
                PreparedQueryToSp = PreparedQueryToSp + "@" + InputNameClone[LoopCnt] + "=" + InputValueClone[LoopCnt] + ",";
            }
            else
            {
                InputValueClone[LoopCnt] = InputValueClone[LoopCnt].Replace("'", "''''");
                PreparedQueryToSp = PreparedQueryToSp + "@" + InputNameClone[LoopCnt] + "=''" + InputValueClone[LoopCnt] + "'',";
            }
        }
        PreparedQueryToSp = PreparedQueryToSp.Substring(0, PreparedQueryToSp.Length - 1);
        oDbEngine.InsurtFieldValue("Config_DebugHelper", "DebugHelper_Type,DebugHelper_QString,DebugHelper_SegmentID,DebugHelper_ExchangeSegmentID,DebugHelper_UserID,DebugHelper_CreateDateTime", "'SP','" + PreparedQueryToSp + "'," + SegmentID + "," + ExchangeSegmentID + "," + HttpContext.Current.Session["UserID"].ToString() + ",'" + oDbEngine.GetDate() + "'");
    }
    public void GetPrepareQueryToRun(string TableName, string FieldName, string WhereCluase, string OrderBy,string GroupBy)
    {

    }
}
