<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.SettingsOptions.management_SettingsOptions_EmailSetup" CodeBehind="EmailSetup.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function DeleteRow(keyValue) {
            doIt = confirm('Confirm delete?');
            if (doIt) {
                grid.PerformCallback('Delete~' + keyValue);
               // height();
            }
            else {

            }


        }
        //   function OnMoreInfoClick(keyValue)
        //    {
        //      var dat=document.getElementById('dtTo_hidden').value;
        //       var url='frmClosingRatesAdd.aspx?id='+ keyValue +'&dtfor='+ dat;
        //       OnMoreInfoClick(url,"Edit Closing Rates",'820px','400px',"Y");
        //   
        //    }
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }


        function OnEditButtonClick(keyValue) {
            var url = '../EmailSetupAddEdit.aspx?id=' + keyValue;
            //OnMoreInfoClick(url, "Add New Accout", '940px', '500px', "Y");
            window.location.href = url;
        }

        function OnAddButtonClick() {
            var url = '../EmailSetupAddEdit.aspx?id=ADD';
            //OnMoreInfoClick(url, "Add New Accout", '940px', '500px', "Y");
            window.location.href = url;
        }
        function callback() {
            grid.PerformCallback();
            //height();
        }

        function OnMoreAccessCick(keyValue) {
            grid.PerformCallback('Access~' + keyValue);
            //height();
            //       var url='AddCandidateForOLetter.aspx?id='+ keyValue+'&mode=edit' ;
            //       OnMoreInfoClick(url,"Edit Candidate",'960px','550px',"Y");

        }
    </script>
    <style>
        .dxgvTable {
            table-layout: auto !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Email Accounts Setup</h3>
        </div>

    </div>
    <div class="form_main">
        <table class="TableMain100">

            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td id="Td1" align="left">
                                <% if (rights.CanAdd)
                                   { %><a href="javascript:void(0);" onclick="javascript:OnAddButtonClick();" class="btn btn-primary"><span>Add New</span> </a><%} %>
                                <%--<a href="javascript:ShowHideFilter('s');" class="btn btn-success"><span>Show Filter</span></a>--%>
                                <%--<a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>--%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxGridView ID="gridStatus" ClientInstanceName="grid" Width="100%" KeyFieldName="EmailAccounts_ID"
                        DataSourceID="gridStatusDataSource" runat="server" AutoGenerateColumns="False"
                        OnCustomCallback="gridStatus_CustomCallback">
                        <SettingsBehavior AllowFocusedRow="false" ConfirmDelete="True" />

                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />


                        <%--<Styles>
                            <FocusedRow CssClass="gridselectrow" BackColor="#FCA977">
                            </FocusedRow>
                            <FocusedGroupRow CssClass="gridselectrow" BackColor="#FCA977">
                            </FocusedGroupRow>
                        </Styles>--%>
                        <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <Columns>
                            <dxe:GridViewDataTextColumn Visible="False" FieldName="EmailAccounts_ID" Caption="ID">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="Company"
                                Caption="Company">
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="Segment"
                                Caption="Segment">
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="EmailType"
                                Caption="Email Type">
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="EmailAccounts_EmailID"
                                Caption="From Email" Visible="true">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>



                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="EmailAccounts_Password"
                                Caption="Password" Visible="False">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="EmailAccounts_SMTP"
                                Caption="SMTP Host" Visible="true">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="EmailAccounts_SMTPPort"
                                Caption="SMTP Port" Visible="true">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="EmailAccounts_POP"
                                Caption="SMTP Pop" Visible="false">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="EmailAccounts_POPPort"
                                Caption="Pop Prot" Visible="false">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>

                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="EmailAccounts_ReplyToAccount"
                                Caption="ReplyTo Email" Visible="false">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>

                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="11" FieldName="EmailAccounts_Disclaimer"
                                Caption="Disclaimer" Visible="False">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>

                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="ActiveInd"
                                Caption="Status" Width="50px">
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>

                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn VisibleIndex="10" Width="100px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <DataItemTemplate>

                                    <% if (rights.CanDelete)
                                       { %><a href="javascript:void(0);" onclick="DeleteRow('<%# Container.KeyValue %>')" class="pad"><img src="../../../assests/images/Delete.png" /></a><%} %>
                                     <% if (rights.CanEdit)
                                        { %><a href="javascript:void(0);" onclick="OnMoreAccessCick('<%# Container.KeyValue %>')" class="pad"><img src="../../../assests/images/activity.png" /></a><%} %>
                                     <% if (rights.CanEdit)
                                        { %><a href="javascript:void(0);" onclick="OnEditButtonClick('<%# Container.KeyValue %>')" class="pad">
                                            <img src="../../../assests/images/Edit.png" /></a><%} %>
                                </DataItemTemplate>
                                <HeaderTemplate>
                                    Actions
                                    <%-- <a href="javascript:void(0);" onclick="javascript:OnAddButtonClick();"><span style="color: #000099; text-decoration: underline">Add New</span> </a>--%>
                                </HeaderTemplate>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                        </Columns>
                        <SettingsText ConfirmDelete="Confirm delete?" />
                        <StylesEditors>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </StylesEditors>
                        <%--<Settings ShowHorizontalScrollBar="True" />--%>
                    </dxe:ASPxGridView>
                    <asp:SqlDataSource ID="gridStatusDataSource" runat="server"
                        SelectCommand="">
                        <SelectParameters>
                            <asp:SessionParameter Name="userlist" SessionField="userchildHierarchy" Type="string" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
            </tr>
        </table>
        <table align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <%-- <dxeCharts:RichTextEditor ID="richeditor" Theme="Blue"  runat="server" />  --%>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
