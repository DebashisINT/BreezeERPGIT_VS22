<%@ Page Language="C#" Debug="true" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frmReport_NoTransOnlyHolding" Codebehind="~/reports/frmreport_notransonlyholding.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_rootfile.js"></script>

    <style type="text/css">
	 .dxgvTable{table-layout:auto !important;}
	/* Big box with list of options */
	#ajax_listOfOptions{
		position:absolute;	/* Never change this one */
		width:50px;	/* Width of box */
		height:auto;	/* Height of box */
		overflow:auto;	/* Scrolling features */
		border:1px solid Blue;	/* Blue border */
		background-color:#FFF;	/* White background color */
		text-align:left;
		font-size:0.9em;
		z-index:32767;
	}
	#ajax_listOfOptions div{	/* General rule for both .optionDiv and .optionDivSelected */
		margin:1px;		
		padding:1px;
		cursor:pointer;
		font-size:0.9em;
	}
	#ajax_listOfOptions .optionDiv{	/* Div for each item in list */
		
	}
	#ajax_listOfOptions .optionDivSelected{ /* Selected item in the list */
		background-color:#DDECFE;
		color:Blue;
	}
	#ajax_listOfOptions_iframe{
		background-color:#F00;
		position:absolute;
		z-index:3000;
	}
	
	form{
		display:inline;
	}
	
	</style>

    <script type="text/javascript" language="javascript">
        function PageLoad() {
            LoadingOnOff('hide');
            FieldName = 'btnPrint';
            document.getElementById('txtEmpName_hidden').style.display = 'none';
            document.getElementById('tdHeader').style.display = 'none';
            document.getElementById('tdfooter').style.display = 'none';
            document.getElementById('tr_header').style.display = 'none';
            document.getElementById('tr_footer').style.display = 'none';
            document.getElementById('tdvalue').style.display = 'none';
            document.getElementById('tdempname').style.display = 'none';
            hide('trGpmType');
            hide('tr_output');
            FnddlGroupBy('S');
        }
        function show(obj1) {
            //alert(obj1);
            document.getElementById(obj1).style.display = 'inline';
        }
        function hide(obj1) {
            //alert(obj1);
            document.getElementById(obj1).style.display = 'none';
        }
        function openGroupPopUp(obj) {
            document.getElementById('groupid').value = '';
            if (obj == 'G') {
                cmbgpm.SelectIndex(0, true);
                show('trGpmType');
            }
            else if (obj == 'GroupCode') {
                OnMoreInfoClick();
                hide('trGpmType');
            }
            else {
                hide('trGpmType');
            }
        }
        function fnGroups(s) {
            if (s == 'S') {

                OnMoreInfoClick();
            }
            else {
                document.getElementById('groupid').value = "";
            }

            height();
        }
        function OnMoreInfoClick() {
            var URL = 'cdslBill_GroupSelection.aspx';
            ParentWindowShow(URL, 'Charge Groups');
        }
        function bindChkBox(obj) {

            if (obj != 'NA') {
                var URL = 'CdslBill_GroupMaster.aspx?type=' + obj;
                ParentWindowShow(URL, 'Groups');
            }
        }
        function ParentWindowShow(objURL, title) {
            OnMoreInfoClick(objURL, title, '940px', '450px', "Y");
        }
        function ChkCheckProperty(obj, objChk) {
            if (objChk == true) {
                if (obj == 'H')
                    document.getElementById('tdHeader').style.display = 'inline';
                else if (obj == 'F')
                    document.getElementById('tdfooter').style.display = 'inline';
            }
            else {
                if (obj == 'H')
                    document.getElementById('tdHeader').style.display = 'none';
                else if (obj == 'F')
                    document.getElementById('tdfooter').style.display = 'none';
            }
        }
        function FunHeaderFooter(objID, objListFun, objEvent, objParam) {
            ajax_showOptions(objID, objListFun, objEvent, objParam);
        }
        function popupClose2() {
            document.getElementById('groupid').value = "";
            rbgrp.SetValue('A');
            parent.editwin.close();
        }
        function FillValues(s) {
            document.getElementById('groupid').value = s;
            parent.editwin.close();
        }

        function FillValuesgpm(s) {
            document.getElementById('groupid').value = s;
            parent.editwin.close();
        }
        function SignOff() {
            window.parent.SignOff();
        }
        function CallAjax(obj1, obj2, obj3) {
            ajax_showOptions(obj1, obj2, obj3);
        }
        function LoadingOnOff(obj) {
            if (obj == 'show')
                document.getElementById('Div1').style.display = 'inline';
            else if (obj == 'hide')
                document.getElementById('Div1').style.display = 'none';
        }

        function height() {
            if (document.body.scrollHeight >= 700)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '700px';

            window.frameElement.width = document.body.scrollWidth;
        }
        function OnDateChanged() {
            if (e2.GetDate() < e1.GetDate()) {
                alert('Invalid. DateFrom must be less or equal to DateTo.');
            }
        }
        function Check() {
            if (e2.GetDate() < e1.GetDate()) {
                alert('Invalid. DateFrom must be less or equal to DateTo.');
                return false;
            }
            else {
                return true;
            }
        }
        function checkChange() {
            var checkbox = document.getElementById('chkSignature')
            if (checkbox.checked) {
                document.getElementById('tdvalue').style.display = 'inline';
                document.getElementById('tdempname').style.display = 'inline';
                document.getElementById('txtEmpName').focus();

            }
            else {
                document.getElementById('tdvalue').style.display = 'none';
                document.getElementById('tdempname').style.display = 'none';

            }

        }
        function FnddlGroupBy(obj) {
            if (obj == 'S') {
                document.getElementById('tr_header').style.display = 'none';
                document.getElementById('tr_footer').style.display = 'none';
                document.getElementById('tr_print').style.display = 'none';

                document.getElementById('tr_orderby').style.display = 'none';
                document.getElementById('tr_employee').style.display = 'none';
                document.getElementById('tr_duplex').style.display = 'none';
                document.getElementById('tr_email').style.display = 'none';
                document.getElementById('Tr_DigitalSign').style.display = 'none';
                document.getElementById('tr_check').style.display = 'none';
                document.getElementById('tr_screen').style.display = 'none';
                hide('tr_output');

                //document.getElementById('tr_export').style.display='none';


            }
            if (obj == 'P') {
                document.getElementById('tr_print').style.display = 'inline';

                document.getElementById('tr_orderby').style.display = 'inline';
                document.getElementById('tr_employee').style.display = 'inline';
                document.getElementById('tr_duplex').style.display = 'inline';
                document.getElementById('tr_email').style.display = 'none';
                document.getElementById('Tr_DigitalSign').style.display = 'none';
                document.getElementById('tr_check').style.display = 'none';
                document.getElementById('tr_screen').style.display = 'none';
                document.getElementById('tr_grid').style.display = 'none';
                document.getElementById('tr_header').style.display = 'inline';
                document.getElementById('tr_footer').style.display = 'inline';
                show('tr_output');
                //document.getElementById('tr_export').style.display='none';

            }
            if (obj == 'M') {
                document.getElementById('tr_header').style.display = 'inline';
                document.getElementById('tr_footer').style.display = 'inline';
                document.getElementById('tr_print').style.display = 'none';

                document.getElementById('tr_orderby').style.display = 'none';
                document.getElementById('tr_employee').style.display = 'none';
                document.getElementById('tr_duplex').style.display = 'none';
                document.getElementById('tr_email').style.display = 'inline';
                //document.getElementById('Tr_DigitalSign').style.display='inline';
                document.getElementById('tr_check').style.display = 'inline';
                document.getElementById('tr_screen').style.display = 'none';
                document.getElementById('tr_grid').style.display = 'none';
                show('tr_output');
                //document.getElementById('tr_export').style.display='none';


            }
            if (obj == 'E') {
                document.getElementById('tr_print').style.display = 'none';
                document.getElementById('tr_header').style.display = 'none';
                document.getElementById('tr_footer').style.display = 'none';
                document.getElementById('tr_orderby').style.display = 'none';
                document.getElementById('tr_employee').style.display = 'none';
                document.getElementById('tr_duplex').style.display = 'none';
                document.getElementById('tr_email').style.display = 'none';
                //document.getElementById('Tr_DigitalSign').style.display='inline';
                document.getElementById('tr_check').style.display = 'none';
                document.getElementById('tr_screen').style.display = 'inline';
                document.getElementById('tr_grid').style.display = 'inline';
                show('tr_output');
                //document.getElementById('tr_export').style.display='inline';
            }



        }

        function CallAjax(obj1, obj2, obj3) {
            ajax_showOptions(obj1, obj2, obj3);
        }
        function checkChange1() {
            var checkbox = document.getElementById('CheckBox1')
            if (checkbox.checked) {
                document.getElementById('Tr_DigitalSign').style.display = 'inline';
                //document.getElementById('txtEmpName').focus();
            }
            else {
                document.getElementById('Tr_DigitalSign').style.display = 'none';
            }



        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div>
            <asp:Panel ID="panel1" BackColor="#DDECFE" runat="server" Height="500px" Width="100%">
                <table class="TableMain100">
                    <tr>
                        <td class="EHEADER" colspan="7" style="text-align: center; height: 27px;">
                            <strong><span style="color: #000099">Quarterly Statement Of Holding ( No Transaction
                                Clients )</span></strong>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <table>
                                <tr>
                                    <td class="gridcellleft" bgcolor="#B7CEEC">
                                        Date From :
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="txtDateFrom" runat="server" ClientInstanceName="e1" Width="130px"
                                            EditFormat="Custom" EditFormatString="dd MMMM yyyy" UseMaskBehavior="True" AllowNull="False">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                            <ClientSideEvents ValueChanged="function(s, e) {OnDateChanged();}" />
                                        </dxe:ASPxDateEdit>
                                    </td>
                                    <td class="gridcellleft" bgcolor="#B7CEEC">
                                        Date To :
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="txtDateTo" runat="server" ClientInstanceName="e2" Width="130px"
                                            EditFormat="Custom" EditFormatString="dd MMMM yyyy" UseMaskBehavior="True" AllowNull="False">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                            <ClientSideEvents ValueChanged="function(s, e) {OnDateChanged();}" />
                                        </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td class="gridcellleft" bgcolor="#B7CEEC">
                                        Generation Type :
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlgeneration" Width="120px" Font-Size="12px"
                                            onchange="FnddlGroupBy(this.value)">
                                            <asp:ListItem Value="S">--Select--</asp:ListItem>
                                            <asp:ListItem Value="P">Print</asp:ListItem>
                                            <asp:ListItem Value="M">Send Email</asp:ListItem>
                                            <asp:ListItem Value="E">Screen</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="tr_output">
                        <td>
                            <table>
                                <tr>
                                    <td class="gridcellleft" bgcolor="#B7CEEC">
                                        Output Type :
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddloutput" Width="220px" Font-Size="12px"
                                            onchange="FnddlGroupBy(this.value)">
                                            <asp:ListItem Value="1">No Transaction Only Holding</asp:ListItem>
                                            <asp:ListItem Value="2">No Transaction No Holding</asp:ListItem>
                                            
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="tr_header">
                        <td colspan="2">
                            <table>
                                <tr>
                                    <td class="gridcellleft" bgcolor="#B7CEEC">
                                        Use Header :
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkHeader" runat="server" onclick="ChkCheckProperty('H',this.checked);" />
                                    </td>
                                    <td id="tdHeader">
                                        <asp:TextBox ID="txtHeader" runat="server" Width="279px" Font-Size="12px" onkeyup="FunHeaderFooter(this,'GetHeaderFooter',event,'H')"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="tr_footer">
                        <td colspan="2">
                            <table>
                                <tr>
                                    <td class="gridcellleft" bgcolor="#B7CEEC">
                                        Use Footer :
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkFooter" runat="server" onclick="ChkCheckProperty('F',this.checked);" />
                                    </td>
                                    <td id="tdfooter">
                                        <asp:TextBox ID="txtFooter" runat="server" Width="279px" Font-Size="12px" onkeyup="FunHeaderFooter(this,'GetHeaderFooter',event,'F')"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="tr_duplex">
                        <td>
                            <table>
                                <tr>
                                    <td class="gridcellleft" bgcolor="#B7CEEC">
                                        Duplex Printing ( Back-To-Back )
                                    </td>
                                    <td colspan="2">
                                        <asp:CheckBox ID="chbDuplex" TextAlign="Right" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="tr_employee">
                        <td colspan="4">
                            <table>
                                <tr class="gridcellleft">
                                    <td class="gridcellleft" bgcolor="#B7CEEC">
                                        <span class="Ecoheadtxt" style="color: blue;">Add Signatory:</span></td>
                                    <td style="width: 73px">
                                        <asp:CheckBox ID="chkSignature" runat="server" onclick="checkChange();" />
                                    </td>
                                    <td class="gridcellleft" bgcolor="#B7CEEC" id="tdempname">
                                        <span class="Ecoheadtxt" style="color: blue;">Employee Name:</span>
                                    </td>
                                    <td id="tdvalue">
                                        <asp:TextBox ID="txtEmpName" onkeyup="CallAjax(this,'SearchByEmployees',event)" runat="server"></asp:TextBox>
                                        <asp:TextBox ID="txtEmpName_hidden" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="tr_orderby">
                        <td>
                            <table>
                                <tr>
                                    <td class="gridcellleft" bgcolor="#B7CEEC">
                                        Order Type:
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cmbBillGenerationOrder" Width="160px" runat="server" Font-Size="12px"
                                            ValueType="System.String" Font-Bold="False" SelectedIndex="0" TabIndex="4">
                                            <Items>
                                                <dxe:ListEditItem Value="PinCode" Text="Area Wise"></dxe:ListEditItem>
                                                <dxe:ListEditItem Value="BranchID" Text="Branch Wise"></dxe:ListEditItem>
                                                <dxe:ListEditItem Value="G" Text="Group Wise"></dxe:ListEditItem>
                                            </Items>
                                            <clientsideevents valuechanged="function(s, e) {
	                                        openGroupPopUp(s.GetValue());
                                        }"></clientsideevents>
                                            <ButtonStyle Width="60px">
                                            </ButtonStyle>
                                            <DropDownButton>
                                            </DropDownButton>
                                        </dxe:ASPxComboBox>
                                    </td>
                                    <td colspan="3">
                                        <div id="trGpmType">
                                            <dxe:ASPxComboBox ID="cmbBillGroupType" Width="170px" runat="server" Font-Size="12px"
                                                ValueType="System.String" Font-Bold="False" SelectedIndex="0" ClientInstanceName="cmbgpm"
                                                TabIndex="5">
                                                <ClientSideEvents ValueChanged="function(s, e) {
	                    bindChkBox(s.GetValue());
                    }"></ClientSideEvents>
                                                <ButtonStyle Width="60px">
                                                </ButtonStyle>
                                                <DropDownButton Text="Group Type">
                                                </DropDownButton>
                                            </dxe:ASPxComboBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="tr_check">
                        <td>
                            <table>
                                <tr>
                                    <td class="gridcellleft" bgcolor="#B7CEEC">
                                        Digital Signatory:
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="CheckBox1" runat="server" onclick="checkChange1();" Checked="false" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="Tr_DigitalSign">
                        <td>
                            <table>
                                <tr>
                                    <td class="gridcellleft" bgcolor="#B7CEEC">
                                        Select Signature :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtdigitalName" runat="server" Width="250px" onkeyup="FunCallAjaxList(this,event,'Digital')"></asp:TextBox>
                                        <asp:TextBox ID="txtdigitalName_hidden" runat="server" TabIndex="11" Width="100px"
                                            Style="display: none"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="tr_print">
                        <td style="width: 6px">
                            <asp:Button ID="btnPrint" class="btnUpdate" runat="server" Text="Print" OnClientClick="return Check();"
                                OnClick="btnPrint_Click" Font-Size="smaller" Width="97px" />
                        </td>
                    </tr>
                    <tr id="tr_email">
                        <td style="width: 6px">
                            <asp:Button ID="btnshow" runat="server" CssClass="btnUpdate" Font-Size="smaller"
                                Width="101px" Text="Send Email" OnClick="btnshow_Click" /></td>
                    </tr>
                    <tr id="tr_screen">
                        <td style="width: 6px">
                            <asp:Button ID="btnscreen" runat="server" CssClass="btnUpdate" Font-Size="smaller"
                                Width="101px" Text="Screen" OnClick="btnscreen_Click" /></td>
                    </tr>
                    <tr id="tr_export" runat="server">
                        <td class="gridcellright" align="right" id="td_export" runat="server">
                            <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true" BackColor="Navy"
                                Font-Bold="False" ForeColor="White" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                ValueType="System.Int32" Width="100px">
                                <Items>
                                    <dxe:ListEditItem Text="Select" Value="0" />
                                    <dxe:ListEditItem Text="PDF" Value="1" />
                                    <dxe:ListEditItem Text="XLS" Value="2" />
                                    <dxe:ListEditItem Text="RTF" Value="3" />
                                    <dxe:ListEditItem Text="CSV" Value="4" />
                                </Items>
                                <ButtonStyle BackColor="#C0C0FF" ForeColor="Black">
                                </ButtonStyle>
                                <ItemStyle BackColor="Navy" ForeColor="White">
                                    <HoverStyle BackColor="#8080FF" ForeColor="White">
                                    </HoverStyle>
                                </ItemStyle>
                                <Border BorderColor="White" />
                                <DropDownButton Text="Export">
                                </DropDownButton>
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr id="tr_grid">
                        <td style="text-align: left; width: 995px; vertical-align: top;" id="FormGrid">
                            <dxe:ASPxGridView ID="GridReminder" ClientInstanceName="grid" runat="server" KeyFieldName="BenAccountNumber"
                                AutoGenerateColumns="False" OnPageIndexChanged="GridReminder_PageIndexChanging"
                                Width="100%">
                                <Styles>
                                    <LoadingPanel ImageSpacing="10px">
                                    </LoadingPanel>
                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                    </Header>
                                </Styles>
                                <Settings ShowHorizontalScrollBar="true" />
                                <SettingsPager AlwaysShowPager="True" NumericButtonCount="20" ShowSeparators="True">
                                    <FirstPageButton Visible="True">
                                    </FirstPageButton>
                                    <LastPageButton Visible="True">
                                    </LastPageButton>
                                </SettingsPager>
                                <SettingsBehavior AllowFocusedRow="True" AllowSort="False" />
                                <%--<ClientSideEvents FocusedRowChanged="function(s, e) { OnGridFocusedRowChanged(); }" />--%>
                                <ClientSideEvents SelectionChanged="function(s, e) { OnGridFocusedRowChanged(); }" />
                                <Columns>
                                    <dxe:GridViewDataTextColumn Caption="AccountNumber" FieldName="BenAccountNumber"
                                        ReadOnly="True" VisibleIndex="1">
                                        <EditFormSettings Visible="False" />
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Name" FieldName="name1" ReadOnly="True" VisibleIndex="2">
                                        <EditFormSettings Visible="False" />
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Address" FieldName="Address1" ReadOnly="True"
                                        VisibleIndex="3">
                                        <EditFormSettings Visible="False" />
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Pin No" FieldName="PinNo" ReadOnly="True"
                                        VisibleIndex="4" Width="10%">
                                        <EditFormSettings Visible="False" />
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Phone No" FieldName="PhoneNo" ReadOnly="True"
                                        VisibleIndex="5">
                                        <EditFormSettings Visible="False" />
                                    </dxe:GridViewDataTextColumn>
                                    <%-- <dxe:GridViewDataTextColumn Caption="Subject" FieldName="shortname" ReadOnly="True"
                                                            VisibleIndex="6">
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataTextColumn>--%>
                                </Columns>
                            </dxe:ASPxGridView>
                        </td>
                    </tr>
                </table>
                <div id='Div1' style='position: absolute; font-family: arial; font-size: 30; left: 50%;
                    top: 50%; = background-color: white; layer-background-color: white; height: 80;
                    width: 150;'>
                    <table width='100' height='35' border='1' cellpadding='0' cellspacing='0' bordercolor='#C0D6E4'>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td height='25' align='center' bgcolor='#FFFFFF'>
                                            <img src='/assests/images/progress.gif' width='18' height='18'></td>
                                        <td height='10' width='100%' align='center' bgcolor='#FFFFFF'>
                                            <font size='2' face='Tahoma'><strong align='center'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Loading..</strong></font></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <dxe:ASPxGridViewExporter ID="exporter" runat="server">
                        </dxe:ASPxGridViewExporter>
                        <asp:HiddenField ID="txtHeader_hidden" runat="server" />
                        <asp:HiddenField ID="txtFooter_hidden" runat="server" />
                        <asp:HiddenField ID="groupid" runat="server" />
                        <asp:HiddenField ID="gpm" runat="server" />
                    </table>
                </div>
            </asp:Panel>
        </div>
</asp:Content>
