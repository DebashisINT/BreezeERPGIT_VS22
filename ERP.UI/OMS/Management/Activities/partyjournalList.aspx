<%--================================================== Revision History =============================================
1.0   Pallab    V2.0.38      23-05-2023          0026200: Party Journal - Auto module design modification & check in small device
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="partyjournalList.aspx.cs" Inherits="ERP.OMS.Management.Activities.partyjournalList" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/partyJournalAuto.css" rel="stylesheet" />
    <script>
        function OnAddButtonClick() {
            window.location.href = '/OMS/management/Activities/partyjournalAdd.aspx'
        }

        function updateGridByDate() {
            if (cFormDate.GetDate() == null) {
                jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
            }
            else if (ctoDate.GetDate() == null) {
                jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
            }

            else {
                localStorage.setItem("FromDateCustomerPartyJournal", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("ToDateCustomerPartyJournal", ctoDate.GetDate().format('yyyy-MM-dd'));

                $("#hfIsFilter").val("Y");

                cGvJvSearch.Refresh();
            }
        }

        function CustomBtnView(key) {
            window.location.href = '/OMS/management/Activities/partyjournalAdd.aspx?IsView=Y&key=' + key;
        }
        function ViewJournal(key) {
            cGvJvSearchFullInfo.PerformCallback('FilterGridByDate~' + key);
            cJournalPopup.Show();
        }



        function gridRowclick(s, e) {
            //alert('hi');
            $('#GvJvSearch').find('tr').removeClass('rowActive');
            $('.floatedBtnArea').removeClass('insideGrid');
            //$('.floatedBtnArea a .ico').css({ 'opacity': '0' });
            $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
            $(s.GetRow(e.visibleIndex)).addClass('rowActive');
            setTimeout(function () {
                //alert('delay');
                var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a .ico').css({'opacity': '1'});
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a').each(function (e) {
                //    setTimeout(function () {
                //        $(this).fadeIn();
                //    }, 100);
                //});    
                $.each(lists, function (index, value) {
                    //console.log(index);
                    //console.log(value);
                    setTimeout(function () {
                        console.log(value);
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }

        var isFirstTime = true;
        function AllControlInitilize() {
            ///  document.getElementById('AddButton').style.display = 'inline-block';
            if (isFirstTime) {

                if (localStorage.getItem('FromDateCustomerPartyJournal')) {
                    var fromdatearray = localStorage.getItem('FromDateCustomerPartyJournal').split('-');
                    var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                    cFormDate.SetDate(fromdate);
                }

                if (localStorage.getItem('ToDateCustomerPartyJournal')) {
                    var todatearray = localStorage.getItem('ToDateCustomerPartyJournal').split('-');
                    var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                    ctoDate.SetDate(todate);
                }

                //updateGridByDate();

                isFirstTime = false;
            }
        }
    </script>
    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    
    <style>
        select
        {
            z-index: 0;
        }

        #gridAdvanceAdj {
            max-width: 99% !important;
        }
        #FormDate, #toDate, #dtTDate, #dt_PLQuote, #dt_PlQuoteExpiry {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        select
        {
            -webkit-appearance: auto;
        }

        .calendar-icon
        {
            right: 20px;
        }

        .panel-title h3
        {
            padding-top: 0px !important;
        }

        .fakeInput
        {
                min-height: 30px;
    border-radius: 4px;
        }
        
    </style>
    <%--Rev end 1.0--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>Party Journal - Auto</h3>
        </div>
        <table class="padTab pull-right" style="margin-top: 7px">
            <tr>
                <td>
                    <label>From Date</label></td>
                <%--Rev 1.0: "for-cust-icon" class add --%>
                <td class="for-cust-icon">
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                    <%--Rev 1.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 1.0--%>
                </td>
                <td>
                    <label>To Date</label>
                </td>
                <%--Rev 1.0: "for-cust-icon" class add --%>
                <td class="for-cust-icon">
                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                    <%--Rev 1.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 1.0--%>
                </td>

                <td>
                    <input type="button" value="Show" class="btn btn-primary" onclick="updateGridByDate()" />
                </td>

            </tr>

        </table>
    </div>
        <div class="form_main">
        <div class="mb-10">
            <% if (rights.CanAdd)
               { %>
            <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-success"><span class="btn-icon"><i class="fa fa-plus"></i></span><span><u>A</u>dd New</span> </a>
            <% } %>

            <% if (rights.CanExport)
               { %>
            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>
            <% } %>
        </div>

        <div class="clearfix relative">
            <dxe:ASPxGridView ID="GvJvSearch" runat="server" AutoGenerateColumns="False" SettingsBehavior-AllowSort="true"
                ClientInstanceName="cGvJvSearch" KeyFieldName="ID" Width="100%" Settings-HorizontalScrollBarMode="Auto" DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false">
                <SettingsSearchPanel Visible="true" Delay="5000" />
                <ClientSideEvents RowClick="gridRowclick" />
                <SettingsBehavior ConfirmDelete="True" />
                <Styles>
                    <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>
                    <FocusedRow HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridselectrow"></FocusedRow>
                    <LoadingPanel ImageSpacing="10px"></LoadingPanel>
                    <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>
                    <Footer CssClass="gridfooter"></Footer>
                </Styles>
                <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" AlwaysShowPager="True">
                    <FirstPageButton Visible="True">
                    </FirstPageButton>
                    <LastPageButton Visible="True">
                    </LastPageButton>
                </SettingsPager>

                <Columns>

                    <dxe:GridViewDataTextColumn Width="20%" Visible="False" ShowInCustomizationForm="false" VisibleIndex="0" FieldName="ID" Caption="ID" SortOrder="Descending">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <%-- End of Rev Sayantani--%>
                    <dxe:GridViewDataTextColumn Width="18%" Caption="Doc. Number" FieldName="DOCUMENT_NUMBER" VisibleIndex="1">
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Width="18%" VisibleIndex="2" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" FieldName="DOCUMENT_DATE" Caption="Doc. Date">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Width="12.5%" VisibleIndex="3" FieldName="Total_Debit" Caption="Total Debit">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Width="12.5%" VisibleIndex="4" FieldName="Total_Credit" Caption="Total Credit">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Width="12.5%" VisibleIndex="5" FieldName="user_name" Caption="Created By" Settings-AllowAutoFilter="False">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Width="12.5%" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" FieldName="CREATED_DATE" Caption="Created On">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Width="12.5%" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="19">
                        <DataItemTemplate>
                            <div class='floatedBtnArea'>
                            <% if (rights.CanView)
                               { %>
                            <a href="javascript:void(0);" onclick="CustomBtnView('<%# Container.KeyValue %>')" class="" title="View">
                                <span class='ico ColorThree'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span></a></a>
                            <a href="javascript:void(0);" onclick="ViewJournal('<%# Container.KeyValue %>')" class="" title="View">
                                <span class='ico ColorFour'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View Journal</span></a></a>
                            <% } %>
                            </div>
                            

                        </DataItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <CellStyle HorizontalAlign="Center"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <HeaderTemplate><span>Actions</span></HeaderTemplate>
                        <EditFormSettings Visible="False"></EditFormSettings>
                    </dxe:GridViewDataTextColumn>
                </Columns>
                <%-- --Rev Sayantani--%>
                <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
                <SettingsCookies Enabled="true" StorePaging="true" StoreColumnsWidth="false" StoreColumnsVisiblePosition="false" />
                <%-- -- End of Rev Sayantani --%>
                <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" HorizontalScrollBarMode="Visible" ShowFooter="true" />
                <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                    <FirstPageButton Visible="True">
                    </FirstPageButton>
                    <LastPageButton Visible="True">
                    </LastPageButton>
                </SettingsPager>
                <TotalSummary>
                    <dxe:ASPxSummaryItem FieldName="Total_Debit" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="Total_Credit" SummaryType="Sum" />
                </TotalSummary>
            </dxe:ASPxGridView>
            <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                ContextTypeName="ERPDataClassesDataContext" TableName="V_PARTYJOURAL" />
        </div>
    </div>
    </div>
    <asp:HiddenField ID="hfIsFilter" runat="server" />
    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>

    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="JournalPopup" runat="server" ClientInstanceName="cJournalPopup"
            Width="900px" HeaderText="Generated Journals" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentStyle VerticalAlign="Top"></ContentStyle>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxGridView ID="GridFullInfo" runat="server" AutoGenerateColumns="False" SettingsBehavior-AllowSort="true"
                        ClientInstanceName="cGvJvSearchFullInfo" KeyFieldName="JvID" Width="100%"
                        OnCustomCallback="GridFullInfo_CustomCallback" OnDataBinding="GridFullInfo_DataBinding">
                        <ClientSideEvents EndCallback="function(s, e) {GridFullInfo_EndCallBack();}" />
                        <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn" />

                        <Styles>
                            <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>
                            <FocusedRow HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridselectrow"></FocusedRow>
                            <LoadingPanel ImageSpacing="10px"></LoadingPanel>
                            <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>
                            <Footer CssClass="gridfooter"></Footer>
                        </Styles>
                        <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <Columns>

                            <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="JV_DATE" Caption="Posting Date">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="JV_NO" Width="150px" Caption="Document No.">
                                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="MainAccount" Width="150px" Caption="Ledger Desc.">
                                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="SubAccount" Width="150px" Caption="Subledger Desc.">
                                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="JV_NARRATION" Width="300px" Caption="NARRATION">
                                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Proj_Name" Width="300px" Caption="Project Name" Settings-AllowAutoFilter="True">
                                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="true" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="JV_DR_AMT" Caption="Debit Amount">
                                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="JV_CR_AMT" Caption="Credit Amount">
                                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="8" Visible="true" FieldName="CGSTRate" Caption="CGST Rate">
                                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Visible="true" VisibleIndex="9" FieldName="CGSTRate" Caption="CGST Rate">
                                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Visible="true" VisibleIndex="10" FieldName="IGSTRate" Caption="IGST Rate">
                                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Visible="true" VisibleIndex="11" FieldName="UTGSTRate" Caption="UTGST Rate">
                                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>



                            <dxe:GridViewDataTextColumn VisibleIndex="12" FieldName="CGSTAmount" Caption="CGST Amt">
                                <CellStyle CssClass="gridcellleft"></CellStyle>
                                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="13" FieldName="SGSTAmount" Caption="SGST Amt">
                                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="14" FieldName="IGSTAmount" Caption="IGST Amt">
                                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="15" FieldName="UTGSTAmount" Caption="UTGST Amt">
                                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="16" FieldName="RCM" Caption="RCM">
                                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="17" FieldName="ITC" Caption="ITC">
                                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                        </Columns>
                        <Settings ShowFooter="true" ShowColumnHeaders="true" ShowFilterRow="true" ShowGroupFooter="VisibleIfExpanded" />
                        <TotalSummary>
                            <dxe:ASPxSummaryItem FieldName="JV_DR_AMT" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="JV_CR_AMT" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="CGSTAmount" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="SGSTAmount" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="IGSTAmount" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="UTGSTAmount" SummaryType="Sum" />
                        </TotalSummary>
                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" HorizontalScrollBarMode="Visible" />
                        <SettingsSearchPanel Visible="True" />
                    </dxe:ASPxGridView>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
    </div>
</asp:Content>
