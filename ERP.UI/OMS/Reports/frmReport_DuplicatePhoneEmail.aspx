<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frmReport_DuplicatePhoneEmail" Codebehind="frmReport_DuplicatePhoneEmail.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />
    <%-- <script type="text/javascript" src="/assests/js/loaddata1.js"></script>--%>

    <script language="javascript" type="text/javascript">
        //    function SignOff()
        //    {
        //        window.parent.SignOff();
        //    }
        //    function height()
        //    {
        //        if(document.body.scrollHeight>=500)
        //            window.frameElement.height = document.body.scrollHeight;
        //        else
        //            window.frameElement.height = '500px';
        //        window.frameElement.Width = document.body.scrollWidth;
        //    }

        function ShowHideFilterPhone(obj) {
            Phonegrid.PerformCallback(obj);
        }
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        //    function callback()
        //        {
        //            grid.PerformCallback();
        //             Phonegrid.PerformCallback();
        //        } 
        //    function callheightPhone(obj)
        //    {
        //        height();
        //       //// parent.CallMessage();
        //    }
        //    
        //    function callheightEmail(obj)
        //    {
        //        height();
        //      // // parent.CallMessage();
        //    }
        function PageLoad() {

            //  document.getElementById('txtName_hidden').style.display="none";
            //   document.getElementById('txtName').style.display="none";
            ShowEmployeeFilterForm('A');

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div>
            <table class="TableMain100">
                <tr>
                    <td class="EHEADER" style="text-align: center; height: 20px;">
                        <strong><span style="color: #000099">Duplicate
                            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                            List</span></strong>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        Select :
                        <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True">
                            <asp:ListItem>Email</asp:ListItem>
                            <asp:ListItem>Phone</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel1" runat="server" Width="100%">
                <table class="TableMain100">
                    <tr>
                        <td>
                            <table class="TableMain100">
                                <tr>
                                    <td style="text-align: left; vertical-align: top">
                                        <table>
                                            <tr>
                                                <td id="ShowFilter">
                                                    <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">
                                                        Show Filter</span></a>
                                                </td>
                                                <td id="Td1">
                                                    <a href="javascript:ShowHideFilter('All');"><span style="color: #000099; text-decoration: underline">
                                                        All Records</span></a>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="gridcellright">
                                        <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true" BackColor="Navy"
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
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dxe:ASPxGridView ID="LeadGrid" runat="server" AutoGenerateColumns="False" KeyFieldName="eml_id"
                                Width="100%" ClientInstanceName="grid" OnCustomCallback="LeadGrid_CustomCallback1"
                                OnCustomJSProperties="LeadGrid_CustomJSProperties">
                                <settings showgrouppanel="True" />
                                <columns>
                                <dxe:GridViewDataTextColumn FieldName="eml_id" ReadOnly="True" VisibleIndex="0"
                                    Visible="False">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="eml_entity" ReadOnly="True" VisibleIndex="0" caption="Type">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="eml_email" VisibleIndex="1" caption="Email">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn FieldName="Name" ReadOnly="True" VisibleIndex="3" caption="Name">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dxe:GridViewDataTextColumn>
                         <dxe:GridViewDataTextColumn FieldName="eml_cntid" ReadOnly="True" VisibleIndex="2" caption="Contact ID"  Visible="False">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dxe:GridViewDataTextColumn> 
        
                               
                            </columns>
                                <settingsbehavior allowfocusedrow="True" columnresizemode="NextColumn" />
                                <styles>
                                <LoadingPanel ImageSpacing="10px">
                                </LoadingPanel>
                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                </Header>
                                <FocusedGroupRow CssClass="gridselectrow">
                                </FocusedGroupRow>
                                <FocusedRow CssClass="gridselectrow">
                                </FocusedRow>
                            </styles>
                                <settingspager numericbuttoncount="20" pagesize="20" showseparators="True" alwaysshowpager="True">
                                <FirstPageButton Visible="True">
                                </FirstPageButton>
                                <LastPageButton Visible="True">
                                </LastPageButton>
                            </settingspager>
                                <clientsideevents endcallback="function(s, e) {
	callheightEmail(s.cpHeight);
}" />
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
            </asp:Panel>
            <asp:Panel ID="Panel2" runat="server" Width="100%">
                <table class="TableMain100">
                    <tr>
                        <td>
                            <table class="TableMain100">
                                <tr>
                                    <td style="text-align: left; vertical-align: top">
                                        <table>
                                            <tr>
                                                <td id="Td2">
                                                    <a href="javascript:ShowHideFilterPhone('Sp');"><span style="color: #000099; text-decoration: underline">
                                                        Show Filter</span></a>
                                                </td>
                                                <td id="Td3">
                                                    <a href="javascript:ShowHideFilterPhone('Al');"><span style="color: #000099; text-decoration: underline">
                                                        All Records</span></a>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="gridcellright">
                                        <dxe:ASPxComboBox ID="cmbExportPhone" runat="server" AutoPostBack="true" BackColor="Navy"
                                            Font-Bold="False" ForeColor="White" ValueType="System.Int32" Width="130px" OnSelectedIndexChanged="cmbExportPhone_SelectedIndexChanged">
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
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dxe:ASPxGridView ID="PhoneGrid" runat="server" AutoGenerateColumns="False" KeyFieldName="phf_id"
                                Width="100%" ClientInstanceName="Phonegrid" OnCustomCallback="PhoneGrid_CustomCallback1"
                                OnCustomJSProperties="PhoneGrid_CustomJSProperties">
                                <settings showgrouppanel="True" />
                                <columns>
                                <dxe:GridViewDataTextColumn FieldName="phf_id" ReadOnly="True" VisibleIndex="0"
                                    Visible="False">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="phf_entity" ReadOnly="True" VisibleIndex="0" caption="Type">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="phf_phoneNumber" VisibleIndex="1" caption="Phone Number">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Name" ReadOnly="True" VisibleIndex="3" caption="Name">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dxe:GridViewDataTextColumn>
                                                              <dxe:GridViewDataTextColumn FieldName="phf_cntId" ReadOnly="True" VisibleIndex="2" caption="ContactID" >
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dxe:GridViewDataTextColumn> 
                               
                            </columns>
                                <settingsbehavior allowfocusedrow="True" columnresizemode="NextColumn" />
                                <styles>
                                <LoadingPanel ImageSpacing="10px">
                                </LoadingPanel>
                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                </Header>
                                <FocusedGroupRow CssClass="gridselectrow">
                                </FocusedGroupRow>
                                <FocusedRow CssClass="gridselectrow">
                                </FocusedRow>
                            </styles>
                                <settingspager numericbuttoncount="20" pagesize="20" showseparators="True" alwaysshowpager="True">
                                <FirstPageButton Visible="True">
                                </FirstPageButton>
                                <LastPageButton Visible="True">
                                </LastPageButton>
                            </settingspager>
                                <clientsideevents endcallback="function(s, e) {
	callheightPhone(s.cpHeight);
}" />
                            </dxe:ASPxGridView>
                        </td>
                    </tr>
                </table>
                <asp:SqlDataSource ID="PhoneGridDataSource" runat="server" 
                    SelectCommand=""></asp:SqlDataSource>
                <dxe:ASPxGridViewExporter ID="Exporter1" runat="server">
                </dxe:ASPxGridViewExporter>
            </asp:Panel>
        </div>
</asp:Content>
