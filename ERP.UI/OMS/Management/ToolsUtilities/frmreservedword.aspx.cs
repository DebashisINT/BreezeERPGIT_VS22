using System;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_utilities_frmreservedword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] list = Request.QueryString["type"].ToString().Split(',');
            //Converter oConverter = new Converter();     //____This is to call recipient variable with the predefined values.
            BusinessLogicLayer.Converter oConverter = new BusinessLogicLayer.Converter();
            string UIstring = "<table><tr><td><table>";
            if (list[0] == "receipent")
            {
                UIstring += "<tr>";
                string[,] recipient = oConverter.ReservedWord_recipient();
                string mess = Request.QueryString["control"].Trim();
                for (int i = 0; i < recipient.Length / 2; i++)
                {
                    //UIstring += "<td><input style='border-right: 1px groove; border-top: 1px groove; font-size: 8pt;  border-left: 1px groove; color: black; border-bottom: 1px groove; font-style: normal; font-family: Verdana; width: 125px' onclick='" + Request.QueryString["control"].Trim() + "=" + Request.QueryString["control"].Trim() + "+< \"+ this.value +\">" + ";' type='button' id='chk' name='chk' value='" + recipient[i, 0] + "'></td>";
                    UIstring += "<td><input style='border-right: 1px groove; border-top: 1px groove; font-size: 8pt;  border-left: 1px groove; color: black; border-bottom: 1px groove; font-style: normal; font-family: Verdana; width: 125px' onclick='PostReservedWord(this.value);'  type='button' id='chk' name='chk' value='" + recipient[i, 0] + "'></td>";
                }
                UIstring += "</tr>";
            }
            if (list.Length > 1)
            {
                if (list[1] == "sender")
                {
                    string[,] sender1 = oConverter.ReservedWord_sender();
                    UIstring += "<tr></tr><tr>";
                    for (int i = 0; i < sender1.Length / 2; i++)
                    {
                        //    UIstring += "<td><input style='border-right: 1px groove; border-top: 1px groove; font-size: 8pt;  border-left: 1px groove; color: black; border-bottom: 1px groove; font-style: normal; font-family: Verdana; width: 125px' onclick='" + Request.QueryString["control"].Trim() + "=" + Request.QueryString["control"].Trim() + "+\"< \"+this.value+\">\" ;' type='button' id='chk' name='chk' value='" + sender1[i, 0] + "'></td>";
                        UIstring += "<td><input style='border-right: 1px groove; border-top: 1px groove; font-size: 8pt;  border-left: 1px groove; color: black; border-bottom: 1px groove; font-style: normal; font-family: Verdana; width: 125px' onclick='PostReservedWord(this.value);'  type='button' id='chk' name='chk' value='" + sender1[i, 0] + "'></td>";
                    }
                    UIstring += "</tr>";
                }
            }
            UIstring += "<tr></tr><tr><td><input style='border-right: 1px groove; border-top: 1px groove; font-size: 8pt;  border-left: 1px groove; color: black; border-bottom: 1px groove; font-style: normal; font-family: Verdana; width: 125px' onclick='window.close();' type='button' id='close' value='Close' ></td></tr>";
            UIstring += "</table></td></tr></table>";
            Response.Write(UIstring);
        }
    }
}