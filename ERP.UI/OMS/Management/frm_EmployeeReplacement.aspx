<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.management_frm_EmployeeReplacement" Codebehind="frm_EmployeeReplacement.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />
     <!--Ajax List Section-->
    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_wofolder.js"></script>
    <script type="text/javascript" src="/assests/js/init.js"></script>
    <link href="../CSS/AjaxStyle.css" rel="stylesheet" type="text/css" />
    <!--End Ajax List Section-->
    <script type="text/javascript" language="javascript">
    function closeWindow()
    {
        window.close();
        parent.editwin.close();
        return false;
    }
    function SetValue()
    {
        window.opener.ClearDate();
    }
    </script>
    <script type="text/ecmascript">
        function updateAttendanceNull(obj)
        {
            //alert(obj);
            CallServer('Edit~'+obj , "");
        }
        function ReceiveServerData(rValue)
        {   
            //alert(rValue);
//            var DATA=rValue.split('~');  
//            if(DATA[0]=="Edit")
//            {
//            }
        }
        
        //---Ajax Section---
    FieldName="";
    function CallAjax(obj1,obj2,obj3,Query)
    {
        var CombinedQuery=new String(Query);
        ajax_showOptions(obj1,obj2,obj3,replaceChars(CombinedQuery),'Main');
    }
    function replaceChars(entry) {
            out = "+"; // replace this
            add = "--"; // with this
            temp = "" + entry; // temporary holder

            while (temp.indexOf(out)>-1) {
            pos= temp.indexOf(out);
            temp = "" + (temp.substring(0, pos) + add + 
            temp.substring((pos + out.length), temp.length));
            }
            return temp;
        }
    //---End Ajax Section---
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div style="padding: 5px 5px 5px 5px">
            <div style="border: solid 1px blue; padding: 5px 5px 5px 5px; text-align:center; vertical-align:middle;">
                <table>
                    <tr>
                        <td class="gridcellleft" style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                            Replacing Employee:
                        </td>
                        <td class="gridcellleft" style="vertical-align: top">
                            &nbsp;<asp:TextBox ID="txtReplacement" runat="server" Width="236px"></asp:TextBox><asp:HiddenField
                                ID="txtReplacement_hidden" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="gridcellleft" style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                            Effective Date:
                        </td>
                        <td class="gridcellleft" style="vertical-align: top">
                            <dxe:ASPxDateEdit Width="225px" ID="cmbDOL" ClientInstanceName="date" runat="server"
                                EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top">
                            <asp:HiddenField ID="childemployees" runat="server" />
                        </td>
                        <td class="gridcellright" style="vertical-align: top">
                            <asp:Button ID="btnupdate" runat="server" Text="Save" CssClass="btnUpdate" Height="23px" OnClick="btnupdate_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
</asp:Content>
