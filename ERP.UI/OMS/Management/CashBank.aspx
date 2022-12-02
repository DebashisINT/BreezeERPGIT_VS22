<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.management_CashBank" CodeBehind="CashBank.aspx.cs" %>


<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register assembly="DevExpress.Web.v15.1" namespace="DevExpress.Web" tagprefix="dx" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    

    <script type="text/javascript">
        $(document).ready(function () {

            $(".water").each(function () {
                if ($(this).val() == this.title) {
                    $(this).addClass("opaque");
                }
            });

            $(".water").focus(function () {
                if ($(this).val() == this.title) {
                    $(this).val("");
                    $(this).removeClass("opaque");
                }
            });

            $(".water").blur(function () {
                if ($.trim($(this).val()) == "") {
                    $(this).val(this.title);
                    $(this).addClass("opaque");
                }
                else {
                    $(this).removeClass("opaque");
                }
            });
        });

    </script>

    <script language="javascript" type="text/javascript">
        //function SignOff() {
        //    window.parent.SignOff();
        //}
        //function height() {
        //    if (document.body.scrollHeight >= 500)
        //        window.frameElement.height = document.body.scrollHeight;
        //    else
        //        window.frameElement.height = '500px';
        //    window.frameElement.Width = document.body.scrollWidth;
        //}
        function OnMoreInfoClick(KeyValue) {
            <%Session["cashJournal"] = "2";%>
            var url = "cashbankPopup.aspx?id=" + KeyValue;
            OnMoreInfoClick(url, "Modify CashBank Entries", '940px', '450px', "Y");

        }
        function OnAddButtonClick() {
            <%Session["cashJournal"] = "2";%>
            var url = 'cashbankPopup.aspx';
            OnMoreInfoClick(url, "Add CashBank Entries", '985px', '450px', "Y");

        }
        function RefreshGrid() {
            editwin.close();
        }
        FieldName = 'ctl00_ContentPlaceHolder1_Headermain1_Menumain_SkipLink';
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function EndCall(obj) {
            if (obj == "2")
                alert('Deleted Successfully');
            else if (obj == "3")
                alert('Entry Already Tagged,Deletion DisAllowed');
            height();
        }
        function callback() {
            grid.PerformCallback();
        }
        function ShowHideFilter(obj) {
            InitialTextVal();
            if (obj == "s")
                document.getElementById('TrFilter').style.display = "inline";
            else {
                document.getElementById('TrFilter').style.display = "none";
                grid.PerformCallback(obj);
            }
        }
        function btnSearch_click() {
            document.getElementById('TrFilter').style.display = "none";
            grid.PerformCallback('s');
        }
        function InitialTextVal() {
            document.getElementById('txtRefNo').value = "Ref. No.";
            document.getElementById('txtAcNo').value = "A/C No";
            document.getElementById('txtInstNo').value = "Inst. Number";
            document.getElementById('txtPayment').value = "Payment";
            document.getElementById('txtReceipt').value = "Receipt";
            //dtTrDate.SetDate('01-01-1900');
            dtTrDate.SetDate(new Date('01-01-0100'));
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100">
            <tr>
                <td class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099">Cash Bank</span></strong>
                </td>
            </tr>
            <tr>
                <td style="text-align: left; vertical-align: top">
                    <table>
                        <tr>
                            <td id="ShowFilter">
                                <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">Show Filter</span></a>
                            </td>
                            <td id="Td1">
                                <a href="javascript:ShowHideFilter('All');"><span style="color: #000099; text-decoration: underline">All Records</span></a>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="TrFilter" style="display: none">
                <td>
                    <table>
                        <tr>
                            <td>
                                <dxe:ASPxDateEdit ID="dtTrDate" runat="server" EditFormat="Custom" TabIndex="17"
                                    ClientInstanceName="dtTrDate" UseMaskBehavior="True" Font-Size="12px" Width="120px"
                                    EditFormatString="dd-MM-yyyy">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRefNo" runat="server" CssClass="water" Text="Ref. No." ToolTip="Ref. No."
                                    Font-Size="12px" Width="100px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAcNo" runat="server" CssClass="water" Text="A/C No" ToolTip="A/C No"
                                    Font-Size="12px" Width="120px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtInstNo" runat="server" CssClass="water" Text="Inst. Number" ToolTip="Inst. Number"
                                    Font-Size="12px" Width="85px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPayment" runat="server" CssClass="water" Text="Payment" ToolTip="Payment"
                                    Font-Size="12px" Width="92px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReceipt" runat="server" CssClass="water" Text="Receipt" ToolTip="Receipt"
                                    Font-Size="12px" Width="90px"></asp:TextBox>
                            </td>
                            <td>
                                <input id="btnSearch" type="button" value="Search" class="btnUpdate" style="height: 21px"
                                    onclick="btnSearch_click()" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxGridView ID="grdCashbank" runat="server" ClientInstanceName="grid" KeyFieldName="CashBank_ID"
                        Width="100%" DataSourceID="dsCashBank" OnCustomCallback="grdCashbank_CustomCallback"
                        AutoGenerateColumns="False" OnRowDeleting="grdCashbank_RowDeleting" OnCustomJSProperties="grdCashbank_CustomJSProperties">
                        <SettingsBehavior ConfirmDelete="True" AllowFocusedRow="True" />
                        <Styles>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </Styles>
                        <SettingsPager NumericButtonCount="20" Visible="False">
                        </SettingsPager>
                        <Columns>
                            <dxe:GridViewDataTextColumn Caption="CashBank_ID" FieldName="CashBank_ID" Visible="False"
                                VisibleIndex="0">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" VisibleIndex="0" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataDateColumn Caption="Tr.Date" FieldName="cashbank_TransactionDate"
                                VisibleIndex="0">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <PropertiesDateEdit EditFormat="Custom" EditFormatString="dd-MM-yyyy" DisplayFormatString="{0:dd MMM yyyy}"
                                    UseMaskBehavior="True">
                                </PropertiesDateEdit>
                            </dxe:GridViewDataDateColumn>
                            <dxe:GridViewDataTextColumn Caption="Ref. No." FieldName="VoucherNo" VisibleIndex="1">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" VisibleIndex="0" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="A/C No" FieldName="AccountNumber" VisibleIndex="2">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" VisibleIndex="0" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Payment" FieldName="PaymentAmount" VisibleIndex="3">
                                <CellStyle HorizontalAlign="Right" CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" VisibleIndex="0" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Receipt" FieldName="ReceiptAmount" VisibleIndex="4">
                                <CellStyle HorizontalAlign="Right" CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" VisibleIndex="0" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Date" VisibleIndex="5" FieldName="cashbank_TransactionDate"
                                Visible="False">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" VisibleIndex="16" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Segment" VisibleIndex="6" FieldName="CashBank_ExchangeSegmentID"
                                Visible="False">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" VisibleIndex="16" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Branch" VisibleIndex="7" FieldName="CashBank_BranchID"
                                Visible="False">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" VisibleIndex="16" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Details" Width="5%" VisibleIndex="8">
                                <DataItemTemplate>
                                    <a href="#" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')">Edit</a>
                                </DataItemTemplate>
                                <HeaderTemplate>
                                    <a href="javascript:void(0);" onclick="OnAddButtonClick()"><span style="color: #000099; text-decoration: underline">Add New</span></a>
                                </HeaderTemplate>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewCommandColumn VisibleIndex="9" Width="5%" ShowDeleteButton="True"/>
                        </Columns>
                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center" PopupEditFormHeight="600px"
                            PopupEditFormVerticalAlign="TopSides" PopupEditFormWidth="900px" />
                        <SettingsText PopupEditFormCaption="Add/Modify Cash/Bank" ConfirmDelete="Are you sure to Delete this Record!" />
                        <Settings ShowGroupPanel="True" />
                        <StylesEditors>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </StylesEditors>
                        <ClientSideEvents EndCallback="function(s, e) {
	  EndCall(s.cpEND);
}" />
                    </dxe:ASPxGridView>
                </td>
            </tr>
            <tr>
                <td></td>
            </tr>
        </table>
        <asp:SqlDataSource ID="dsCashBank" runat="server" ConflictDetection="CompareAllValues"
            SelectCommand=""
            InsertCommand="insert into table1 (temp123) values('11')" UpdateCommand="update table1 set temp123='123'"></asp:SqlDataSource>
        <asp:SqlDataSource ID="dsCompany" runat="server"
            SelectCommand="SELECT COMP.CMP_INTERNALID AS CashBank_CompanyID , COMP.CMP_NAME AS CashBank_CompanyName  FROM TBL_MASTER_COMPANY AS COMP"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SelectSegment" runat="server" 
            SelectCommand="SELECT LTRIM(RTRIM(A.EXCH_INTERNALID)) AS CashBank_ExchangeSegmentID ,TME.EXH_ShortName + '--' + A.EXCH_SEGMENTID AS EXCHANGENAME FROM (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID=@COMPANYID) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID">
            <SelectParameters>
                <asp:Parameter Name="COMPANYID" Type="string" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="dsBranch" runat="server"
            SelectCommand="SELECT BRANCH_id AS BANKBRANCH_ID , BRANCH_DESCRIPTION AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH"></asp:SqlDataSource>
        <asp:SqlDataSource ID="MainAccount" runat="server" 
            SelectCommand="Select MainAccount_ReferenceID as CashBank_MainAccountID, MainAccount_Name as MainAccount_Name from Master_MainAccount where MainAccount_BankCashType='Bank' or MainAccount_BankCashType='Cash'"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SelectSubaccount" runat="server" 
            SelectCommand="Select SubAccount_ReferenceID as SubAccountID, SubAccount_Name as SubAccountName from Master_SubAccount where SubAccount_MainAcReferenceID=@ID">
            <SelectParameters>
                <asp:Parameter Name="ID" Type="string" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="dsgrdClientbank" runat="server" ConflictDetection="CompareAllValues"
            InsertCommand="insert into table1 (temp123) values('11')"
            SelectCommand="select A.* , MB.bnk_id,MB.bnk_bankName,MB.bnk_BranchName,MB.bnk_micrno from (Select TCBD.cbd_id,TCBD.cbd_cntId,TCBD.cbd_bankCode, TCBD.cbd_Accountcategory,TCBD.cbd_Accountcategory as AccountType,TCBD.cbd_accountNumber,TCBD.cbd_accountType,cbd_accountName from tbl_trans_contactBankDetails as  TCBD where TCBD.cbd_cntId=@SubAccountCode) as A inner  join tbl_master_Bank as MB on MB.bnk_id=a.cbd_id">
            <SelectParameters>
                <asp:Parameter Name="SubAccountCode" Type="string" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
</asp:Content>
