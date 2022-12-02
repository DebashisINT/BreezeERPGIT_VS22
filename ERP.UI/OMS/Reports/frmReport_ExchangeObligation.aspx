<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frmReport_ExchangeObligation" Codebehind="frmReport_ExchangeObligation.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <!--___________________These files are for List Items__________________________-->

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list1_rootfile.js"></script>

    <!--___________________________________________________________________________-->
    <style type="text/css">
	
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

        /////////////////////  Show Filter/All Records ////////////////////////////////
        function SignOff() {
            window.parent.SignOff();
        }
        function height() {
            if (document.body.scrollHeight >= 500)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '500px';
            window.frameElement.Width = document.body.scrollWidth;
        }
        function ShowHideFilter(obj) {
            height();
            Settelment_Grid.PerformCallback(obj);
            Obligation_date.PerformCallback(obj);
        }

        function CallList(obj1, obj2, obj3) {
            ajax_showOptions(obj1, obj2, obj3);
        }
        function PageLoad() {
            FieldName = 'TRObligation_Set';
            document.getElementById('txtset_hidden').style.display = "none";
            ShowISettelmentFilterForm('S');
            hide('filter_export');
            document.getElementById('TableShow').style.display = 'none';
            document.getElementById('showFilter1').style.display = 'none';
            cCmbexcel.SetSelectedIndex(0);

        }
        function ShowHeight(obj) {
            height();
        }
        ///////////////////// Settelment wise/Date wise   ////////////////////////////////

        function ShowISettelmentFilterForm(obj) {
            //alert(obj);
            document.getElementById('txtset_hidden').value = "";
            if (obj == 'A') {
                //alert('aaa');

                document.getElementById("TRObligation_Set").style.display = "none";
                document.getElementById("TRObligation_date").style.display = "inline";
                // Obligation_date.PerformCallback('empty');

                hide('tdsetValue');
                hide('tdset');
                show('tddatefromValue');



                show('tddateToValue');
                show('tddateTo');

                document.getElementById('txtset_hidden').style.display = "none";
                document.getElementById('txtset_hidden').value = "";


                height();

            }
            if (obj == 'S') {
                //alert('vvv');
                document.getElementById("TRObligation_date").style.display = "none";
                document.getElementById("TRObligation_Set").style.display = "inline";
                show('tdsetValue');
                show('tdset');

                hide('tddatefromValue');
                hide('tddateToValue');

                document.getElementById('txtset_hidden').style.display = "none";
                // Settelment_Grid.PerformCallback('empty');


                height();
            }

        }
        ///////////////////// Call funtion in Button(Show) click  ////////////////////////////////

        function bindrid() {


            if (rbbtnset.GetValue() == "S") {
                document.getElementById("filter_export").style.display = "inline";
                Settelment_Grid.PerformCallback('grid');
                height();
            }


            if (rbbtnset.GetValue() == "A") {

                document.getElementById("filter_export").style.display = "inline";
                Obligation_date.PerformCallback('dategrid');
                height();
            }

        }


        function show(obj1) {
            //alert(obj1);
            document.getElementById(obj1).style.display = 'inline';
        }
        function hide(obj1) {
            //alert(obj1);
            document.getElementById(obj1).style.display = 'none';
        }
        //THIS IS FOR EMAIL

        function btnAddsubscriptionlist_click() {

            var cmb = document.getElementById('cmbsearchOption');

            var userid = document.getElementById('txtSelectionID');
            if (userid.value != '') {
                var ids = document.getElementById('txtSelectionID_hidden');
                var listBox = document.getElementById('lstSlection');
                var tLength = listBox.length;


                var no = new Option();
                no.value = ids.value;
                no.text = userid.value;
                listBox[tLength] = no;
                var recipient = document.getElementById('txtSelectionID');
                recipient.value = '';
            }
            else
                alert('Please search name and then Add!')
            var s = document.getElementById('txtSelectionID');
            s.focus();
            s.select();

        }


        function callAjax1(obj1, obj2, obj3) {
            var combo = document.getElementById("cmbsearchOption");
            var set_value = combo.value
            if (set_value == '16') {
                ajax_showOptions(obj1, 'GetLeadId', obj3, set_value)
            }
            else {
                ajax_showOptions(obj1, obj2, obj3, set_value)
            }
        }

        function clientselectionfinal() {
            var listBoxSubs = document.getElementById('lstSlection');

            var cmb = document.getElementById('cmbsearchOption');

            var listIDs = '';
            var i;

            if (listBoxSubs.length > 0) {

                for (i = 0; i < listBoxSubs.length; i++) {

                    if (listIDs == '')

                        listIDs = listBoxSubs.options[i].value + ';' + listBoxSubs.options[i].text;
                    else
                        listIDs += ',' + listBoxSubs.options[i].value + ';' + listBoxSubs.options[i].text;

                }
                var sendData = cmb.value + '~' + listIDs;

                CallServer(sendData, "");

                document.getElementById('TableShow').style.display = 'none';
                document.getElementById('showFilter1').style.display = 'inline';
            }
            else {
                alert("Please select email from list.")
            }

            var i;
            for (i = listBoxSubs.options.length - 1; i >= 0; i--) {
                listBoxSubs.remove(i);
            }

            height();
        }

        function btnRemovefromsubscriptionlist_click() {

            var listBox = document.getElementById('lstSlection');
            var tLength = listBox.length;

            var arrTbox = new Array();
            var arrLookup = new Array();
            var i;
            var j = 0;
            for (i = 0; i < listBox.options.length; i++) {
                if (listBox.options[i].selected && listBox.options[i].value != "") {

                }
                else {
                    arrLookup[listBox.options[i].text] = listBox.options[i].value;
                    arrTbox[j] = listBox.options[i].text;
                    j++;
                }
            }
            listBox.length = 0;
            for (i = 0; i < j; i++) {
                var no = new Option();
                no.value = arrLookup[arrTbox[i]];
                no.text = arrTbox[i];
                listBox[i] = no;
            }
        }

        function ReceiveServerData(rValue) {
            var Data = rValue.split('~');
            if (Data[0] == 'Clients') {
            }
        }

        function Sendmail() {
            //            document.getElementById('TrAll1').style.display='none';
            //            document.getElementById('TrAll').style.display='none';
            //            document.getElementById('displayAll').style.display='inline';
            //            document.getElementById('filter').style.display='inline';
            document.getElementById('TableShow').style.display = 'inline';
            document.getElementById('showFilter1').style.display = 'none';
            height();

        }

        function MailsendT() {
            alert("Mail Sent Successfully");
        }
        function MailsendF() {
            alert("Error on sending!Try again..");
        }
        function MailsendFT() {
            alert("Email id could not found!Try again...");
        }

        function SndComplete() {
            document.getElementById('TableShow').style.display = 'none';
            document.getElementById('showFilter1').style.display = 'none';
        }
        //     function fn_Cmbexcel(obj)
        //       {

        //                
        //                cCmbexcel.PerformCallback("Excel");
        //       }
        function FnddlGeneration(obj) {
            // cCmbexcel.SetSelectedIndex(0);

            if (obj == "1") {
                document.getElementById('DdlGeneRationType').Setvalue = "0";
                document.getElementById('BtnForExportEvent').click();
            }

        }

        FieldName = 'btnmail';
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <table class="TableMain100">
            <tr>
                <td class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099"></span>Exchange Obligation</strong>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table width="100%">
                        <tr>
                            <td colspan="5" align="left">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="gridcellleft">
                                            <dxe:ASPxRadioButtonList ID="rbSettlement" ClientInstanceName="rbbtnset" runat="server"
                                                Font-Size="12px" SelectedIndex="0" TextWrap="False" RepeatDirection="Horizontal"
                                                ItemSpacing="10px">
                                                <items>
                                                <dxe:ListEditItem Text="Settelment Wise" Value="S" />
                                                <dxe:ListEditItem Text="Date Wise" Value="A" />
                                            </items>
                                                <clientsideevents valuechanged="function(s, e) {ShowISettelmentFilterForm(s.GetValue());}" />
                                                <border borderwidth="0px" />
                                            </dxe:ASPxRadioButtonList>
                                        </td>
                                        <td id="tdset">
                                            <span class="Ecoheadtxt" style="color: blue"><strong>Settelment No: </strong></span>
                                        </td>
                                        <td id="tdsetValue" style="width: 94px" class="gridcellleft">
                                            &nbsp;<asp:TextBox ID="txtset" runat="server" Width="80px" Font-Size="12px" Height="21px"></asp:TextBox>
                                            <asp:TextBox ID="txtset_hidden" runat="server" Width="5px"></asp:TextBox>
                                        </td>
                                        <td id="tddatefromValue" class="gridcellleft">
                                            <dxe:ASPxDateEdit ID="txtfromDate" runat="server" ClientInstanceName="e1" EditFormat="Custom"
                                                EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" AllowNull="False" Font-Size="12px">
                                                <buttonstyle width="80px">
                                            </buttonstyle>
                                                <dropdownbutton text="From Date">
                                            </dropdownbutton>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td id="tddateToValue" class="gridcellleft">
                                            <dxe:ASPxDateEdit ID="txtToDate" runat="server" ClientInstanceName="e1" EditFormat="Custom"
                                                EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" AllowNull="False" Font-Size="12px">
                                                <buttonstyle width="80px">
                                            </buttonstyle>
                                                <dropdownbutton text="To Date">
                                            </dropdownbutton>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td style="width: 2px">
                                        </td>
                                        <td class="gridcellleft">
                                            <dxe:ASPxComboBox ID="ASPxComboBox1" runat="server" Font-Size="12px" ValueType="System.String"
                                                Font-Bold="False" SelectedIndex="0">
                                                <items>
                                                <dxe:ListEditItem Text="Normal" Value="Normal"></dxe:ListEditItem>
                                                <dxe:ListEditItem Text="Custodial" Value="Custodial"></dxe:ListEditItem>
                                                <dxe:ListEditItem Text="Only ND" Value="Only Nd"></dxe:ListEditItem>
                                            </items>
                                                <buttonstyle width="80px">
                                            </buttonstyle>
                                                <dropdownbutton text="Obligation Type">
                                            </dropdownbutton>
                                                <clientsideevents selectedindexchanged="OnClientTypeChanged" />
                                                <paddings paddingbottom="0px"></paddings>
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td class="gridcellleft">
                                            <dxe:ASPxButton ID="btnShow" runat="server" Text="Show" ValidationGroup="a" AutoPostBack="false">
                                                <clientsideevents click="function(s, e) {
bindrid();


	
}" />
                                            </dxe:ASPxButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="filter_export">
                            <td align="left" class="gridcellleft">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td id="ShowFilter" class="gridcellleft">
                                            <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">
                                                Show Filter</span></a>
                                        </td>
                                        <td id="Td1" class="gridcellleft">
                                            <a href="javascript:ShowHideFilter('All');"><span style="color: #000099; text-decoration: underline">
                                                All Records</span></a>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="gridcellright">
                                <table>
                                    <tr>
                                        <td valign="top">
                                            <a href="javascript:void(0);" onclick="Sendmail();"><span style="color: Blue; text-decoration: underline;
                                                font-size: 8pt; font-weight: bold">Send Email</span></a> ||
                                        </td>
                                        <td>
                                            <%--<dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true" BackColor="Navy"
                                                Font-Bold="False" ForeColor="White" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                                ValueType="System.Int32" Width="130px">
                                                <items>
                                    <dxe:ListEditItem Text="Select" Value="0" />
                                    <dxe:ListEditItem Text="PDF" Value="1" />
                                    <dxe:ListEditItem Text="XLS" Value="2" />
                                    <dxe:ListEditItem Text="RTF" Value="3" />
                                    <dxe:ListEditItem Text="CSV" Value="4" />
                                </items>
                                                <buttonstyle backcolor="#C0C0FF" forecolor="Black">
                                </buttonstyle>
                                                <itemstyle backcolor="Navy" forecolor="White">
                                    <HoverStyle BackColor="#8080FF" ForeColor="White">
                                    </HoverStyle>
                                </itemstyle>
                                                <border bordercolor="White" />
                                                <dropdownbutton text="Export">
                                </dropdownbutton>
                                            </dxe:ASPxComboBox>--%>
                                            <%-- <dxe:ASPxComboBox ID="Cmbexcel" runat="server" Width="120px" Font-Size="13px"
                                    ClientInstanceName="cCmbexcel"  OnCallback="Cmbexcel_Callback">
                                    <items>
                                    <dxe:ListEditItem Text="Export" Value="0" />
                                    <dxe:ListEditItem Text="Excel" Value="1" />
                                    
                                </items>
                                    <ClientSideEvents ValueChanged="function(s, e) {fn_Cmbexcel(s.GetValue());}"
                                        EndCallback="Cmbexcel_EndCallback" />
                                </dxe:ASPxComboBox>--%>
                               <asp:DropDownList ID="DdlGeneRationType" runat="server" Width="120px" Font-Size="12px"
                                                    onchange="FnddlGeneration(this.value)">
                                                    <asp:ListItem Value="0">Export</asp:ListItem>
                                                    <asp:ListItem Value="1">Excel</asp:ListItem>
                                                    
                                                </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table width="100%" id="TableShow">
                        <tr>
                            <td width="70px" style="text-align: left;">
                                Type:</td>
                            <td class="gridcellleft" style="vertical-align: top; text-align: left" id="TdFilter">
                                <span id="spanall">
                                    <asp:DropDownList ID="cmbsearchOption" runat="server" Width="150px" Font-Size="12px">
                                    </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td width="70px" style="text-align: left;">
                                Select User:</td>
                            <td class="gridcellleft" style="vertical-align: top; text-align: left" id="TdFilter1">
                                <span id="spanal2">
                                    <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="285px"></asp:TextBox></span>
                                <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                    style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span
                                        style="color: #009900; font-size: 8pt;"> </span>
                            </td>
                        </tr>
                        <tr>
                            <td width="70px" style="text-align: left;">
                            </td>
                            <td style="text-align: left; vertical-align: top; height: 134px;">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            &nbsp;&nbsp;<asp:ListBox ID="lstSlection" runat="server" Font-Size="12px" Height="90px"
                                                Width="290px"></asp:ListBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left">
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <a id="A2" href="javascript:void(0);" onclick="clientselectionfinal()"><span style="color: #000099;
                                                            text-decoration: underline; font-size: 10pt;">Done</span></a>&nbsp;&nbsp;
                                                    </td>
                                                    <td>
                                                        <a id="A1" href="javascript:void(0);" onclick="btnRemovefromsubscriptionlist_click()">
                                                            <span style="color: #cc3300; text-decoration: underline; font-size: 8pt;">Remove</span></a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td width="70px" style="text-align: left;">
                            </td>
                            <td style="height: 23px">
                                <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                                <asp:TextBox ID="dtFrom_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                                <asp:TextBox ID="dtTo_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                            </td>
                        </tr>
                        <%--  <tr>
                                    <td style="text-align:left;">
                                        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" CssClass="btnUpdate" Text="Send" /></td>
                                    </tr>--%>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table width="100%" id="showFilter1">
                        <tr>
                            <td>
                                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" CssClass="btnUpdate"
                                    Text="Send" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="TRObligation_Set">
                <td>
                    <dxe:ASPxGridView ID="Settelment_Grid" ClientInstanceName="Settelment_Grid" runat="server"
                        KeyFieldName="productname" AutoGenerateColumns="False" Width="100%" OnCustomCallback="Settelment_Grid_CustomCallback"
                        OnHtmlRowCreated="Settelment_Grid_HtmlRowCreated" OnDataBound="Settelment_Grid_DataBound"
                        OnCustomJSProperties="Settelment_Grid_CustomJSProperties">
                        <columns>
                        <dxe:GridViewDataTextColumn Caption="Product" ReadOnly="True" VisibleIndex="0"
                            FieldName="productname" Width="10%">
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                            <EditFormCaptionStyle HorizontalAlign="Right">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="False" />
                            
                        </dxe:GridViewDataTextColumn>
                        
                          <dxe:GridViewDataTextColumn Caption="Series" ReadOnly="True" VisibleIndex="1"
                            FieldName="series" Width="10%">
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                            <EditFormCaptionStyle HorizontalAlign="Right">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Total Buy" VisibleIndex="2" FieldName="totalbuy"
                            Width="13%">
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                            <EditFormCaptionStyle HorizontalAlign="Right">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Value" VisibleIndex="3" FieldName="buyvalue"
                            Width="13%">
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                            <EditFormCaptionStyle HorizontalAlign="Right">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Total Sell" VisibleIndex="4" FieldName="sell"
                            Width="13%">
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                            <EditFormCaptionStyle HorizontalAlign="Right">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Value" VisibleIndex="5" FieldName="sellvalue"
                            Width="13%">
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                            <EditFormCaptionStyle HorizontalAlign="Right">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Net Quantity" FieldName="netvalue" VisibleIndex="6"
                            Width="13%">
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                            <EditFormCaptionStyle HorizontalAlign="Right">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Obligation" FieldName="obligance" VisibleIndex="7">
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                            <EditFormCaptionStyle HorizontalAlign="Right">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="False" />
                        </dxe:GridViewDataTextColumn>
                    </columns>
                        <templates>
                        <FooterRow>
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                <tr>
                                    <td style="width: 10%">
                                    </td>
                                     <td style="width: 10%">
                                    </td>
                                    <td style="width: 13%">
                                    </td>
                                    <td style="width: 13%">
                                        <asp:Label ID="lblbuyvalue" runat="server" Text="" Font-Bold="true" ForeColor="white"></asp:Label>
                                    </td>
                                    <td style="width: 13%">
                                    </td>
                                    <td style="width: 13%">
                                        <asp:Label ID="lblsellvalue" runat="server" Text="" Font-Bold="true" ForeColor="white"></asp:Label>
                                    </td>
                                    <td style="width: 13%">
                                    </td>
                                    <td style="width: 13%">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td align="right" colspan="7">
                                        <table border="0" cellpadding="2" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <%--<strong><span style="color: #ffffff">Total =</span></strong>--%>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbltoltal" runat="server" Text="" Font-Bold="true" ForeColor="white"></asp:Label></td>
                                                <td>
                                                    <asp:Label ID="lbltotalcriteria" runat="server" Text="" Font-Bold="true" ForeColor="white"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%-- <strong><span style="color: #ffffff">ND Obligation =</span></strong>--%>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblND" runat="server" Text="" Font-Bold="true" ForeColor="white"></asp:Label></td>
                                                <td>
                                                    <asp:Label ID="lblNdCriteria" runat="server" Text="" Font-Bold="true" ForeColor="white"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%--<strong><span style="color: #ffffff">Net Obligation =</span></strong>--%>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblObligance" runat="server" Text="" Font-Bold="true" ForeColor="white"></asp:Label></td>
                                                <td>
                                                    <asp:Label ID="lbltotalnet" runat="server" Text="" Font-Bold="true" ForeColor="white"></asp:Label></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </FooterRow>
                        <TitlePanel>
                            <table style="width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblobligationtitle" runat="server" Text="" Font-Bold="true" ForeColor="white"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </TitlePanel>
                    </templates>
                        <styles>
                        <LoadingPanel ImageSpacing="10px">
                        </LoadingPanel>
                        <Header ImageSpacing="5px" SortingImageSpacing="5px" HorizontalAlign="Center" CssClass="gridheader">
                        </Header>
                        <FocusedRow CssClass="gridselectrow">
                        </FocusedRow>
                        <Footer CssClass="gridfooter">
                        </Footer>
                        <TitlePanel CssClass="gridtitle">
                        </TitlePanel>
                    </styles>
                        <settings showfooter="True" showstatusbar="Visible" showtitlepanel="True" />
                        <settingstext popupeditformcaption="Add/ Modify Employee" />
                        <settingsediting mode="PopupEditForm" popupeditformhorizontalalign="Center" popupeditformmodal="True"
                            popupeditformverticalalign="WindowCenter" popupeditformwidth="900px" editformcolumncount="3" />
                        <settingsbehavior allowfocusedrow="True" columnresizemode="NextColumn" confirmdelete="True" />
                        <settingspager numericbuttoncount="20" pagesize="20" showseparators="True" alwaysshowpager="True">
                        <FirstPageButton Visible="True">
                        </FirstPageButton>
                        <LastPageButton Visible="True">
                        </LastPageButton>
                    </settingspager>
                        <clientsideevents endcallback="function(s, e) {
	ShowHeight(s.cpHeight);
}" />
                    </dxe:ASPxGridView>
                    <%--  <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                SelectCommand="exchange_obligation" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="trades_segment" SessionField="userlastsegment" Type="Int32" />
                    <asp:SessionParameter Name="trades_settlementno" SessionField="LastSettNo" Type="string" />
                </SelectParameters>
            </asp:SqlDataSource>--%>
                    <dxe:ASPxGridViewExporter ID="exporter" runat="server">
                    </dxe:ASPxGridViewExporter>
                </td>
            </tr>
            <tr id="TRObligation_date">
                <td>
                    <dxe:ASPxGridView ID="Obligation_date" ClientInstanceName="Obligation_date" runat="server"
                        KeyFieldName="settelmentno" AutoGenerateColumns="False" Width="100%" OnCustomCallback="Obligation_date_CustomCallback"
                        OnDataBound="Obligation_date_DataBound" OnHtmlRowCreated="Obligation_date_HtmlRowCreated"
                        OnCustomJSProperties="Obligation_date_CustomJSProperties">
                        <columns>
                        <dxe:GridViewDataTextColumn Caption="Settelment No" ReadOnly="True" VisibleIndex="0" Width="10%"
                            FieldName="settelmentno">
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                            <EditFormCaptionStyle HorizontalAlign="Right">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="False" />
                        </dxe:GridViewDataTextColumn>
                         <dxe:GridViewDataTextColumn Caption="Trade Date" ReadOnly="True" VisibleIndex="1" Width="10%"
                            FieldName="tradedate">
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                            <EditFormCaptionStyle HorizontalAlign="Right">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Buy Value" VisibleIndex="2" FieldName="buyvalue" Width="20%">
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                            <EditFormCaptionStyle HorizontalAlign="Right">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Sell Value" VisibleIndex="3" FieldName="sellvalue" Width="20%">
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                            <EditFormCaptionStyle HorizontalAlign="Right">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Nd Obligation" FieldName="NdObligance" VisibleIndex="4" Width="20%">
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                            <EditFormCaptionStyle HorizontalAlign="Right">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Obligation" FieldName="obligance" VisibleIndex="5">
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                            <EditFormCaptionStyle HorizontalAlign="Right">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="False" />
                        </dxe:GridViewDataTextColumn>
                    </columns>
                        <templates>
                    <FooterRow>
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                <tr>
                                    <td style="width: 10%">
                                    
                                    </td>
                                     <td style="width: 10%">
                                    
                                    </td>
                                  
                                  
                                    <td style="width: 20%" align="center">
                                        <asp:Label ID="lblbuyvaluedate" runat="server" Text="" Font-Bold="true" ForeColor="white"></asp:Label>
                                    </td>
                                   
                                    <td style="width: 20%" align="center">
                                        <asp:Label ID="lblsellvaluedate" runat="server" Text="" Font-Bold="true" ForeColor="white"></asp:Label>
                                    </td>
                                    <td style="width: 20%" align="center">
                                    <asp:Label ID="lblNDvaluedate" runat="server" Text="" Font-Bold="true" ForeColor="white"></asp:Label>
                                    </td>
                                   
                                    <td style="width: 20%" align="center">
                                      <asp:Label ID="lblnetvaluedate" runat="server" Text="" Font-Bold="true" ForeColor="white"></asp:Label>
                                    </td>
                                </tr>
                               
                            </table>
                        </FooterRow>
                        <TitlePanel>
                            <table style="width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbldateobligationtitle" runat="server" Text="" Font-Bold="true" ForeColor="white"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </TitlePanel>
                    </templates>
                        <styles>
                        <LoadingPanel ImageSpacing="10px">
                        </LoadingPanel>
                        <Header ImageSpacing="5px" SortingImageSpacing="5px" HorizontalAlign="Center" CssClass="gridheader">
                        </Header>
                        <SelectedRow CssClass="gridselectrow">
                        </SelectedRow>
                        <FocusedRow CssClass="gridselectrow">
                        </FocusedRow>
                        <Footer CssClass="gridfooter">
                        </Footer>
                        <TitlePanel CssClass="gridtitle">
                        </TitlePanel>
                    </styles>
                        <settings showfooter="True" showstatusbar="Visible" showtitlepanel="True" />
                        <settingstext popupeditformcaption="Add/ Modify Employee" />
                        <settingsediting mode="PopupEditForm" popupeditformhorizontalalign="Center" popupeditformmodal="True"
                            popupeditformverticalalign="WindowCenter" popupeditformwidth="900px" editformcolumncount="3" />
                        <settingsbehavior allowfocusedrow="True" columnresizemode="NextColumn" confirmdelete="True" />
                        <settingspager numericbuttoncount="20" pagesize="20" showseparators="True" alwaysshowpager="True">
                        <FirstPageButton Visible="True">
                        </FirstPageButton>
                        <LastPageButton Visible="True">
                        </LastPageButton>
                    </settingspager>
                        <clientsideevents endcallback="function(s, e) {
	ShowHeight(s.cp1Height);
}" />
                    </dxe:ASPxGridView>
                </td>
                <td style="display:none">
                <asp:Button ID="BtnForExportEvent" runat="server" OnClick="cmbExport_SelectedIndexChanged"
                            BackColor="#DDECFE" BorderStyle="None"  /></td>
            </tr>
        </table>
 </asp:Content>
