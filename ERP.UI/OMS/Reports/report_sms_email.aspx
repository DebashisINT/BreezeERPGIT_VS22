<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_report_sms_email" CodeBehind="report_sms_email.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>

<%@ Register Assembly="DevExpress.Web.v10.2.Export, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.Export" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>--%>


    <script language="javascript" type="text/javascript">
     
        //    function OnMoreInfoClick(keyValue) {
        //        
        //       var url='frmReport_EmailLogReportsDetails.aspx?id='+keyValue;
        //        OnMoreInfoClick(url,"Email Details",'980px','520px',"Y");
        //   
        //    }

        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function callback() {
            grid.PerformCallback();
        }
        function callheight(obj) {
            height();
           //// parent.CallMessage();
        }
        //    function OnContactInfoClick(keyValue,CompName)
        //    {
        //        var url='insurance_contactPerson.aspx?id='+keyValue;
        //        OnMoreInfoClick(url,"Lead Name : "+CompName,'940px','450px','Y');
        //    } 
        function CallAjax(obj1, obj2, obj3) {
            FieldName = 'cmbExport';
            var seg = cSegment.GetValue();
            //alert(seg);
            ajax_showOptions(obj1, obj2, obj3, seg);
        }
        FieldName = 'cmbExport';

        function ShowEmployeeFilterForm(obj) {
            document.getElementById('txtName_hidden').value = "";
            if (obj == 'A') {
                document.getElementById('tdName').style.display = "none";
                document.getElementById('txtName_hidden').style.display = "none";
                document.getElementById('txtName').style.display = "none";
                document.getElementById('txtName').value = "";
                document.getElementById('txtName_hidden').value = "";

            }
            if (obj == 'S') {
                document.getElementById('txtName_hidden').style.display = "inline";
                document.getElementById('txtName').style.display = "inline";
                document.getElementById('tdName').style.display = "inline";
            }
        }
        ////    function ShowEmployeeFilterForm1(obj)
        ////    {
        ////            document.getElementById('txtName_hidden').value="";       
        ////            document.getElementById('tdName').style.display="none";
        ////            document.getElementById('txtName_hidden').style.display="none";
        ////            document.getElementById('txtName').style.display="none";
        ////            document.getElementById('txtName').value="";
        ////            //document.getElementById('txtName_hidden').value="";
        ////    }
        function PageLoad() {
            ShowEmployeeFilterForm('A');
        }
        //-->
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Email Log Reports</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <%--            <tr>
                <td class="EHEADER" style="text-align: center; height: 20px;">
                    <strong><span style="color: #000099">Email Log Reports</span></strong>
                </td>
            </tr>--%>
            <tr>
                <td style="height: 63px">
                    <table>
                        <tr>
                            <td class="gridcellleft">
                                <dxe:ASPxDateEdit ID="txtFromDate" runat="server" EditFormat="Custom" UseMaskBehavior="True">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                    <DropDownButton Text="From Date">
                                    </DropDownButton>
                                </dxe:ASPxDateEdit>
                            </td>
                            <td class="gridcellleft">
                                <dxe:ASPxDateEdit ID="txtToDate" runat="server" EditFormat="Custom" UseMaskBehavior="True">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                    <DropDownButton Text="To Date">
                                    </DropDownButton>
                                </dxe:ASPxDateEdit>
                            </td>
                            <td>Recipient:</td>
                            <td valign="bottom">
                                <dxe:ASPxRadioButtonList ID="rbsegment" ClientInstanceName="cSegment" runat="server"
                                    Font-Size="12px" ItemSpacing="10px" RepeatDirection="Horizontal" SelectedIndex="0"
                                    TextWrap="False">
                                    <Border BorderWidth="0px"></Border>
                                    <ClientSideEvents ValueChanged="function(s, e) {ShowEmployeeFilterForm(s.GetValue());}"></ClientSideEvents>
                                    <Items>
                                        <dxe:ListEditItem Text="Customer" Value="C"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Dpclients" Value="D"></dxe:ListEditItem>
                                    </Items>
                                </dxe:ASPxRadioButtonList>
                                
                            </td>
                            <td><dxe:ASPxRadioButtonList ID="rbUser" runat="server" SelectedIndex="0" ItemSpacing="10px"
                                    RepeatDirection="Horizontal" TextWrap="False" Font-Size="12px">
                                    <Items>
                                        <dxe:ListEditItem Text="All" Value="A" />
                                        <dxe:ListEditItem Text="Specific" Value="S" />
                                    </Items>
                                    <ClientSideEvents ValueChanged="function(s, e) {ShowEmployeeFilterForm(s.GetValue());}" />
                                    <Border BorderWidth="0px" />
                                </dxe:ASPxRadioButtonList></td>
                            <td id="tdName">&nbsp;Name:
                            </td>
                            <td>
                                <asp:TextBox ID="txtName" runat="server" Width="220px" Font-Size="11px" Height="20px"></asp:TextBox>
                            </td>
                            <td style="display: none;">
                                <asp:TextBox ID="txtName_hidden" runat="server" Width="0px" Visible="true"></asp:TextBox>
                            </td>
                            <td class="gridcellleft">
                                <asp:Button ID="btnGetReport" runat="server" Text="Get Report" CssClass="btn btn-primary"
                                     OnClick="btnGetReport_Click" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table class="TableMain100">
                        <tr>
                            <td style="text-align: left; vertical-align: top">
                                <table>
                                    <tr>
                                        <td id="ShowFilter">
                                            <a href="javascript:ShowHideFilter('s');" class="btn btn-success"><span>Show Filter</span></a>
                                        </td>
                                        <td id="Td1">
                                            <a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="gridcellright">
                                <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true" BackColor="Navy"
                                    Font-Bold="False" ForeColor="White" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                    ValueType="System.Int32" Width="130px" Visible="False">
                                    <Items>
                                        <dxe:ListEditItem Text="Select" Value="0"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="PDF" Value="1"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="XLS" Value="2"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="RTF" Value="3"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="CSV" Value="4"></dxe:ListEditItem>
                                    </Items>
                                    <ButtonStyle BackColor="#C0C0FF" ForeColor="Black"></ButtonStyle>
                                    <Border BorderColor="White" />
                                    <ItemStyle BackColor="Navy" ForeColor="White">
                                        <HoverStyle BackColor="#8080FF" ForeColor="White"></HoverStyle>
                                    </ItemStyle>
                                    <DropDownButton Text="Export"></DropDownButton>
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                    </table>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxGridView ID="LeadGrid" runat="server" AutoGenerateColumns="False" KeyFieldName="NotificationRequest_ID"
                        Width="100%" ClientInstanceName="grid" OnCustomCallback="LeadGrid_CustomCallback1"
                        OnCustomJSProperties="LeadGrid_CustomJSProperties">
                        <SettingsBehavior AllowFocusedRow="True" ColumnResizeMode="NextColumn" />
                        <Styles>
                            <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>

                            <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>

                            <FocusedRow CssClass="gridselectrow"></FocusedRow>

                            <LoadingPanel ImageSpacing="10px"></LoadingPanel>
                        </Styles>
                        <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                            <FirstPageButton Visible="True"></FirstPageButton>

                            <LastPageButton Visible="True"></LastPageButton>
                        </SettingsPager>
                        <ClientSideEvents EndCallback="function(s, e) {
	callheight(s.cpHeight);
}" />
                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="NotificationRequest_phoneemail" Caption="PhoneEmail" VisibleIndex="0"></dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="name" ReadOnly="True" Caption="Name" VisibleIndex="1">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                <CellStyle HorizontalAlign="Left"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="NotificationRequest_accesscode" ReadOnly="True" Caption="Subject Line" VisibleIndex="2">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                <CellStyle HorizontalAlign="Left"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="topics_description" Caption="Topic Description" VisibleIndex="3">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                <CellStyle HorizontalAlign="Left"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="NotificationRequest_deliverystatus" ReadOnly="True" Caption="DeliveryStatus" VisibleIndex="4">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                <CellStyle HorizontalAlign="Left"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="NotificationRequest_rejectreason" ReadOnly="True" Caption="RejectReason" VisibleIndex="5">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                <CellStyle HorizontalAlign="Left"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="NotificationRequest_phoneemail" ReadOnly="True" Caption="PhonEmail" Visible="False" VisibleIndex="3">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                <CellStyle HorizontalAlign="Left"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="NotificationRequest_receivedatetime" ReadOnly="True" Caption="ReceiveDatetime" Visible="False" VisibleIndex="3">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                <CellStyle HorizontalAlign="Left"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="NotificationRequest_deliverydatetime" ReadOnly="True" Caption="DeliveryDatetime" VisibleIndex="6">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                <CellStyle HorizontalAlign="Left"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <Settings ShowGroupPanel="True" />
                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="LeadGridDataSource" runat="server"
            SelectCommand="">
            <SelectParameters>
                <asp:SessionParameter Name="userlist" SessionField="userchildHierarchy" Type="string" />
            </SelectParameters>
        </asp:SqlDataSource>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server">
        </dxe:ASPxGridViewExporter>
    </div>
    <br />
    <br />
    <br />
    <br />
</asp:Content>


