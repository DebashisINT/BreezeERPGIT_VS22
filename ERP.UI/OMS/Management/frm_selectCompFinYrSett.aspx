<%@ Page Title="Change Company and Financial Year" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"
    Inherits="ERP.OMS.Management.management_frm_selectCompFinYrSett" CodeBehind="frm_selectCompFinYrSett.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web" TagPrefix="dx" %>--%>

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <%--    <script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>--%>
    <script type="text/javascript" src="/assests/js/ajaxList_wofolder.js"></script>

    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />

    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>

    <link rel="stylesheet" href="../modalfiles/modal.css" type="text/css" />

    <script type="text/javascript" src="../modalfiles/modal.js"></script>


     
        <script type="text/javascript" src="/assests/js/main.js"></script>
    <%--<script type="text/javascript" src="/assests/js/jquery-1.3.2.min.js"></script>--%>

    <script type="text/javascript" language="javascript">
        //function BtnCancel_Click() {          
        //    var parentWindow = window.parent;
        //    parentWindow.popup11.Hide();
        //}
        function visibility() {
            document.getElementById("setid").style.display = 'none'
        }
        function showlist(obj1, obj2, obj3, obj4) {
          //  FieldName = 'BtnCancel';
            var comp = document.getElementById("cmbCompany");
            var finy = document.getElementById("cmbFinYear");
            var obj5 = obj4 + '~' + finy.value;
            ajax_showOptions(obj1, obj2, obj3, obj5, 'Main');
        }
       
        function BtnDone_ClientClick() {
           
          //  FieldName = 'BtnCancel';
            var sett;
            var comp = document.getElementById("cmbCompany");
            var selectedOption = comp.options[comp.selectedIndex];
            var finy = document.getElementById("cmbFinYear");

            sett = document.getElementById("txtSettNo_hidden");
            if (sett.value == "No Record Found") {
                alert("Please Select Sett. No.!.")
            }
            else {
                var data = 'Save~' + comp.value + '~' + finy.value + '~' + selectedOption.text + '~' + sett.value;

            }
          
            $('#<%=hdncompany.ClientID %>').val(data);
        }
        function keyVal(obj) {
            if (obj != '') {
                if (document.getElementById('txtSettNo_hidden').value != '' &&
                    document.getElementById('txtSettNo_hidden').value == "No Record Found") {

                    document.getElementById('btnDone').disabled = true;
                    ShowOpenSettlement();
                    //                    Tr_OpenSettlement.style.display="none";
                }
                else {

                    document.getElementById('btnDone').disabled = false;

                    //                   Tr_OpenSettlement.style.display="inline";
                }

            }
            else {
                document.getElementById('btnDone').disabled = true;
            }
        }
        function PageLoadForButton() {
            document.getElementById('btnDone').disabled = true;
        }

        function PageLoadForButtonV() {
            document.getElementById('btnDone').disabled = false;
        }


      
        function ShowOpenSettlement() {

            document.getElementById('txtSettNo').value = '';
            var SelectedFinYear = document.getElementById('cmbFinYear').value;

            var url = "calendar.aspx?id=ADD/" + '<%=Session["CmbSegmentValue"]%>' + '&OpenSettlementForYear=' + SelectedFinYear;
            //parent.editwinS.close();
            //window.showModalDialog(url,"Dialog Box Arguments # 2","dialogHeight: 350px; dialogWidth: 700px; dialogTop: 250px; dialogLeft: 300px; edge: Raised; center: Yes; resizable: No; status: No;");

            if ('<%=Session["ExchangeSegmentID"]%>' == '1' || '<%=Session["ExchangeSegmentID"]%>' == '4' || '<%=Session["ExchangeSegmentID"]%>' == '15' || '<%=Session["ExchangeSegmentID"]%>' == '19') {

                window.open(url, 'name', 'height=350,width=700,toolbar=no,directories=no,status=no,menubar=no,scrollbars=no,resizable=no ,modal=yes');
            }
            else {

                CallServer("CreateSettlement~" + SelectedFinYear, "");
            }
        }


        function height() {
            if (document.body.scrollHeight >= 500)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '500px';
            window.frameElement.Width = document.body.scrollWidth;
        }
        function blinkIt() {
            if (!document.all) return;
            else {
                for (i = 0; i < document.all.tags('blink').length; i++) {
                    s = document.all.tags('blink')[i];
                    s.style.visibility = (s.style.visibility == 'visible') ? 'hidden' : 'visible';
                }
            }
        }



    </script>

    <script type="text/ecmascript">

        function ReceiveServerData(rValue) {
            var DATA = rValue.split('~');
            if (DATA[0] == "Save") {
                if (DATA[1] == "Y") {
                    var parentWindow = window.parent;
                    parentWindow.popup11.Hide();
                   
                }
                else {
                    alert(DATA[1] + ' Please Relogin and try!');
                }
            }

            if (DATA[0] == "CreateSettlement") {
                alert(DATA[1]);
            }

        }
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <div class="panel-heading">
        <div class="panel-title">
            <h3>Change Company and Financial Year</h3>
            <div class="crossBtn"><a href="ProjectMainPage.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>

    <div class="form_main">
        <table width="100%" border="0" cellspacing="0" cellpadding="5" class="TableMain100">
            <tr>
                <td colspan="2" class="">
                    <table width="100%" border="0" cellpadding="0">
                        <tr>
                            <td class="headfont" style="text-align: left;">
                               <%-- <strong>Select Company and Financial Year</strong>--%></td>
                        </tr>
                    </table>
                </td>

            </tr>
            <tr class="maintablerow">
                <td style="text-align: left; width: 57px;">Company :
                </td>
                <td style="text-align: left; width: 175px;">
                    <asp:DropDownList ID="cmbCompany" runat="server" CssClass="EcoheadCon" Width="270px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr class="maintablerow">
                <td style="text-align: left; width: 57px;">Fin. Year:
                </td>
                <td style="text-align: left; width: 175px;">
                    <asp:DropDownList ID="cmbFinYear" runat="server" CssClass="EcoheadCon" Width="270px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr class="maintablerow" id="setid" runat="server" style="display:none;">
                <td style="text-align: left; width: 57px;">Sett. No. :
                </td>
                <td style="text-align: left; width: 175px;">
                    <asp:TextBox ID="txtSettNo" runat="server" CssClass="EcoheadCon" Width="270px"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="2" style="display: none;">
                    <asp:TextBox ID="txtSettNo_hidden" runat="server"></asp:TextBox>
                </td>
            </tr>
         
            <tr>
                <td></td>
                <td style="text-align: left;">
                       <asp:Button ID="btnDone" runat="server" Text="Done" class="btnUpdate btn btn-primary"  OnClick="btnDone_Click"  OnClientClick="BtnDone_ClientClick()"/>
                  
                    <input id="BtnCancel" type="button" value="Cancel" style="display: none" class="btnUpdate btn btn-danger" onclick="BtnCancel_Click()" />
                </td>
            </tr>
            <tr style="display: none;">
                <td colspan="2" id="Tr_OpenSettlement" style="display: none">
                    <a id="LinkOpenSettlement" onclick="ShowOpenSettlement()" style="cursor: hand;">No Settlement Exists For This FinYear/Date.
                                                                        Click Here To Create a New Settlement.</a>
                </td>
            </tr>
        </table>
        <asp:HiddenField id="hdncompany" runat="server"/>
        <%--<dxe:ASPxPopupControl ID="OpenSettlementPopUp" runat="server" ClientInstanceName="cOpenSettlementPopUp"
            Width="750px" Height="350px" CloseAction="CloseButton" HeaderText="Open Settlement" Modal="True">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                    </dxe:PopupControlContentControl>
                </ContentCollection>
            </dxe:ASPxPopupControl>--%>
    </div>
</asp:Content>


