<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.DailyTask.management_DailyTask_frmBankStatementIndividual" CodeBehind="frmBankStatementIndividual.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register assembly="DevExpress.Web.v15.1" namespace="DevExpress.Web" tagprefix="dx" %>--%>
    <script language="javascript" type="text/javascript">


        function OnMoreInfoClick(obj1) {


            //                       document.getElementById('HiddenField1').value=obj1;
            //                        document.getElementById('Button1').click;
            var hdnID = document.getElementById('hdnID').value;
            var hdnbankdate = document.getElementById('hdnbankdate').value;
            var hdnValueDate = document.getElementById('hdnValueDate').value;
            grid.PerformCallback(obj1 + '~' + hdnbankdate + '~' + hdnValueDate + '~' + hdnID);

            //                      var url='frmBankStatementIndividual.aspx?Id='+obj1+'&TransactionDate='+obj2+'&ValueDate='+obj3+'&InstrumentNumber='+obj4+'&Transactionamount='+obj5+'&Description='+obj6+'&RunningAmount='+obj7+'&Receipt='+obj8;
            ////              OnMoreInfoClick(url,"Rectify Summary",'940px','450px',"Y");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100" style="width: 191px; text-align: center;">
            <tr>
                <td style="text-align: center">
                    <asp:Label ID="Label1" runat="server" Text="Label" Style="text-align: center; vertical-align: middle;" Width="663px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: center">
                    <asp:Label ID="lbltag" runat="server" Text="Transaction Amount" Width="139px"></asp:Label>
                    <asp:Label ID="lblAmount" runat="server" Style="text-align: center" Text="Label"
                        Width="242px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: center">
                    <%--OnCustomCallback="grid_CustomCallback"--%>
                    <dxe:ASPxGridView ID="gridSummary" runat="server" Width="100%" ClientInstanceName="grid"
                        KeyFieldName="CASHBANKDETAIL_ID" AutoGenerateColumns="False" OnCustomCallback="gridSummary_CustomCallback">
                        <Columns>
                            <dxe:GridViewDataTextColumn Caption="Instrument Date" FieldName="CashBankDetail_InstrumentDate" VisibleIndex="0">
                                <Settings AutoFilterCondition="Contains"></Settings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Main Account" FieldName="CashBankDetail_MainAccountId" VisibleIndex="1">
                                <Settings AutoFilterCondition="Contains"></Settings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Sub Account" FieldName="CashBankDetail_SubAccountId" VisibleIndex="2">
                                <Settings AutoFilterCondition="Contains"></Settings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Payment Amount" FieldName="CashBankDetail_PaymentAmount" VisibleIndex="3">
                                <Settings AutoFilterCondition="Contains"></Settings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Receipt Amount" FieldName="CashBankDetail_ReceiptAmount" VisibleIndex="4">
                                <Settings AutoFilterCondition="Contains"></Settings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Instrument Number" FieldName="CashBankDetail_InstrumentNumber" VisibleIndex="5">
                                <Settings AutoFilterCondition="Contains"></Settings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption=" " VisibleIndex="6">
                                <DataItemTemplate>
                                    <asp:CheckBox ID="CheckBox1" runat="server" />
                                </DataItemTemplate>

                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption=" " VisibleIndex="7">
                                <DataItemTemplate>
                                    <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')">
                                        <span style="color: blue; text-decoration: underline">update</span></a>
                                </DataItemTemplate>
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <ClientSideEvents EndCallback="function(s, e) {seek(grid.cpseek);}" />
                    </dxe:ASPxGridView>
                    <asp:HiddenField ID="HiddenField1" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hdnbankdate" runat="server" />
                    <asp:HiddenField ID="hdnValueDate" runat="server" />
                    <asp:HiddenField ID="hdnID" runat="server" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

