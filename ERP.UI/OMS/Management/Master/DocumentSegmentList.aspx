<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="DocumentSegmentList.aspx.cs" Inherits="ERP.OMS.Management.Master.DocumentSegmentList" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function popup()
        {
            jAlert('Please check atleast one row.');
        }
        function ShowLogData(haslog) {
            cgridSegment.Refresh();
        }
        function ImportUpdatePopOpenEmployeesTarget(e) {
            $("#modalimport").modal('show');
        }
        function ViewLogData() {
            cGvJvSearch.Refresh();
        }
        function ShowMsgLastCall() {
            if (cgridSegment.cpDelete) {
                jAlert(cgridSegment.cpDelete, "Alert", function () { cgridSegment.Refresh(); });
                cgridSegment.cpDelete = null;
            }
            else if (cgridSegment.cpDeleteMessage)
            {
                jAlert(cgridSegment.cpDeleteMessage);
                cgridSegment.cpDeleteMessage = null;
            }
        }
        function MassDelete() {
            jConfirm("Confirm Delete?", "Alert", function (ret) {
                if (ret)
                { cgridSegment.PerformCallback("MassDelete"); }
            });
           
            <%--var id=0
            var grid = document.getElementById("<%=gridSegment.ClientID%>");            
            var checkBoxes = grid.getElementsByTagName("INPUT");
            for (var i = 0; i < checkBoxes.length; i++) {
                if (checkBoxes[i].checked) {
                    id = 1;
                }
            }
            if (id == 1)
            {
                cgridSegment.PerformCallback("MassDelete");
            }
            else
            {
                jAlert('Please select a minimum one Row');
            }--%>
            
            
        }
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cgridSegment.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cgridSegment.SetWidth(cntWidth);
            }
            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cgridSegment.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cgridSegment.SetWidth(cntWidth);
                }

            });
        });
        function OnAddClick() {
            // window.location.href = 'DocumentSegmentAdd.aspx?CustomerID=' + $("#hdnCustomerID").val();

            var url = 'DocumentSegmentAdd.aspx?CustomerID=' + $("#hdnCustomerID").val();
            cPosView.SetContentUrl(url);
            cPosView.RefreshContentUrl();

            cPosView.Show();
        }
        function onEditClick(id) {

            //window.location.href = 'DocumentSegmentAdd.aspx?Key=' + id;

            var url = 'DocumentSegmentAdd.aspx?Key=' + id;
            cPosView.SetContentUrl(url);
            cPosView.RefreshContentUrl();
            cPosView.Show();
        }
        function OnClickDelete(id) {
            jConfirm("Confirm Delete?", "Alert", function (ret) {
                if (ret)
                { cgridSegment.PerformCallback("Delete~" + id); }
            });
        }
        function OnViewClick(keyValue) {
            var url = 'DocumentSegmentAdd.aspx?key=' + keyValue + '&req=V';
            // window.location.href = url;

            //  var url = 'DocumentSegmentAdd.aspx?Key=' + id;
            cPosView.SetContentUrl(url);
            cPosView.RefreshContentUrl();
            cPosView.Show();
        }
        function gridRowclick(s, e) {
            $('#gridAdvanceAdj').find('tr').removeClass('rowActive');
            $('.floatedBtnArea').removeClass('insideGrid');
            $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
            $(s.GetRow(e.visibleIndex)).addClass('rowActive');
            setTimeout(function () {
                var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
                $.each(lists, function (index, value) {
                    setTimeout(function () {
                        console.log(value);
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">Document Segment</h3>
        </div>
    </div>
    <div id="ApprovalCross" runat="server" class="crossBtn"><a href="CustomerMasterList.aspx"><i class="fa fa-times"></i></a></div>
    <div class="form_main">
        <%--<% if (rights.CanAdd)
           { %>--%>

        <%--<%} %>--%>

        <%-- <% if (rights.CanExport)
           { %>
        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius"  AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
            <asp:ListItem Value="0">Export to</asp:ListItem>
            <asp:ListItem Value="1">PDF</asp:ListItem>
            <asp:ListItem Value="2">XLS</asp:ListItem>
            <asp:ListItem Value="3">RTF</asp:ListItem>
            <asp:ListItem Value="4">CSV</asp:ListItem>
        </asp:DropDownList>
          <% } %>--%>
        <div id="ImportSegment" runat="server">
            <a href="javascript:void(0);" onclick="OnAddClick()" id="AddId" class="btn btn-success" style="border-radius: 15px"><i class="fa fa-plus"></i><span><u>A</u>dd </span></a>
            <asp:LinkButton ID="lnlDownloaderexcel" runat="server" OnClick="lnlDownloaderexcel_Click" CssClass="btn btn-info btn-radius  mBot0">Download Format</asp:LinkButton>
            <button type="button" onclick="ImportUpdatePopOpenEmployeesTarget();" class="btn btn-primary btn-radius">Import(Add/Update)</button>
            <button type="button" class="btn btn-warning btn-radius" data-toggle="modal" data-target="#modalSS" id="btnViewLog" onclick="ViewLogData();">View Log</button>
            <button type="button" class="btn btn-primary btn-radius"  id="btnMassDelete" onclick="MassDelete();">Mass Delete</button>

        </div>

        <div class="GridViewArea relative">

            <dxe:ASPxGridViewExporter ID="exporter" GridViewID="gridAdvanceAdj" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
            </dxe:ASPxGridViewExporter>


            <dxe:ASPxGridView ID="gridSegment" runat="server" KeyFieldName="ID" AutoGenerateColumns="False"
                Width="100%" ClientInstanceName="cgridSegment" SettingsBehavior-AllowFocusedRow="true"
                SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" Settings-HorizontalScrollBarMode="Auto"
                Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto"
                SettingsBehavior-ColumnResizeMode="Control" DataSourceID="EntityServerModeDataSource" OnCustomCallback="gridSegment_CustomCallback"
                SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false">
                <SettingsSearchPanel Visible="True" Delay="5000" />
                <Columns>

                   
                    <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="ID" Caption="ID" SortOrder="Descending">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                     <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="40"  VisibleIndex="1" SelectAllCheckboxMode="Page" />
                    <dxe:GridViewDataTextColumn Caption="Segment" FieldName="SegmentLayoutNAME" Width="200"
                        VisibleIndex="2" >
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Parent Segment" FieldName="PSegmentLayoutNAME" Width="200"
                        VisibleIndex="3" >
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>


                    <dxe:GridViewDataDateColumn Caption="Code" FieldName="Code" Width="200"
                        VisibleIndex="4">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>

                        <Settings AllowAutoFilterTextInputTimer="False" />

                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataDateColumn>
                    <dxe:GridViewDataTextColumn Caption="Name" FieldName="SegmentName" Width="200"
                        VisibleIndex="5">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Address" FieldName="Address" Width="200"
                        VisibleIndex="6">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>

                        <Settings AllowAutoFilterTextInputTimer="False" />

                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="GSTIN" FieldName="GSTIN" Width="200"
                        VisibleIndex="7">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>



                    <dxe:GridViewDataTextColumn Caption="Created By" FieldName="CreatedBy" Width="200"
                        VisibleIndex="8">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>

                        <Settings AllowAutoFilterTextInputTimer="False" />

                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Updated On" FieldName="UpdatedOn" Width="200"
                        VisibleIndex="9">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>

                        <Settings AllowAutoFilterTextInputTimer="False" />

                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Last Updated By" FieldName="LastUpdatedBy" Width="200"
                        VisibleIndex="10">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>

                        <Settings AllowAutoFilterTextInputTimer="False" />

                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="0">
                        <DataItemTemplate>
                            <div class='floatedBtnArea'>
                                <%-- <% if (rights.CanView)
                                   { %>--%>
                                <a href="javascript:void(0);" onclick="OnViewClick('<%# Container.KeyValue %>')" class="" title="">
                                    <span class='ico ColorFive'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span></a>
                                <%--<% } %>
                                <% if (rights.CanEdit)
                                   { %>--%>
                                <a href="javascript:void(0);" class="" title="" onclick="onEditClick('<%# Container.KeyValue %>')">
                                    <span class='ico editColor'><i class='fa fa-question-circle' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span>
                                </a>
                                <%--  <%} %>--%>


                                <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="" title="" id="a_delete">
                                    <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>

                            </div>
                        </DataItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <CellStyle HorizontalAlign="Center"></CellStyle>

                        <Settings AllowAutoFilterTextInputTimer="False" />

                        <HeaderTemplate><span></span></HeaderTemplate>
                        <EditFormSettings Visible="False"></EditFormSettings>

                    </dxe:GridViewDataTextColumn>


                </Columns>
                <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                <ClientSideEvents EndCallback="function(s, e) {
	                                        ShowMsgLastCall();
                                        }" />
                <TotalSummary>
                    <dxe:ASPxSummaryItem FieldName="Adjusted_Amount" SummaryType="Sum" />
                </TotalSummary>

                <SettingsPager NumericButtonCount="10" PageSize="10">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                </SettingsPager>

                <Settings ShowGroupPanel="True" ShowFooter="true" ShowGroupFooter="VisibleIfExpanded" ShowStatusBar="Hidden" ShowHorizontalScrollBar="true" ShowFilterRow="true" ShowFilterRowMenu="true" />
                <SettingsLoadingPanel Text="Please Wait..." />
                <ClientSideEvents RowClick="gridRowclick" />

            </dxe:ASPxGridView>
            <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                ContextTypeName="ERPDataClassesDataContext" TableName="v_Master_Entity_Segment" />
            <%-- <asp:HiddenField ID="hfIsFilter" runat="server" />
            <asp:HiddenField ID="hfFromDate" runat="server" />
            <asp:HiddenField ID="hfToDate" runat="server" />
            <asp:HiddenField ID="hfBranchID" runat="server" />
            <asp:HiddenField ID="hiddenedit" runat="server" />--%>
        </div>
    </div>
    <asp:HiddenField ID="hdnCustomerID" runat="server" />
    <dxe:ASPxPopupControl ID="PosView" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cPosView" Height="580px"
        Width="1100px" HeaderText="Add/Edit Document Segment" Modal="true">

        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>

    <div class="modal fade" id="modalimport" role="dialog">
        <div class="modal-dialog VerySmall">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Select File to Import (Add/Update)</h4>
                </div>
                <div class="modal-body">

                    <div class="col-md-12">
                        <div id="divproduct">

                            <div>
                                <asp:FileUpload ID="OFDBankSelect" accept=".xls,.xlsx" runat="server" Width="100%" />
                            </div>
                            <div style="clear: both;"></div>
                            <div>                                
                                <dxe:ASPxCheckBox ID="ChkAutoUniqueCodes" runat="server"></dxe:ASPxCheckBox>
                                <dxe:ASPxLabel ID="lblAutoUniqueCodes" Width="120px" runat="server" Text="Auto Generate Unique Codes">
                                </dxe:ASPxLabel>                               
                            </div>
                            <table id="Table1" style="width: 90%" class="nbackBtn padTble" runat="server">
                                <tr>
                                    <td style="width: 120px" class="gHesder">Length</td>
                                    <td style="width: 120px" class="gHesder">Prefix</td>
                                    <td style="width: 120px" class="gHesder">No of Digits</td>
                                    <td style="width: 120px" class="gHesder">Start No</td>
                                </tr>
                                <tr>
                                    <td>
                                        <dxe:ASPxTextBox ID="txtLength" runat="server" ClientInstanceName="ctxtLength" Width="100%" onkeypress="return onlyNumbers();">
                                        </dxe:ASPxTextBox>
                                    </td>
                                    <td>
                                        <dxe:ASPxTextBox ID="txtPrefix" runat="server" ClientInstanceName="ctxtPrefix" Width="100%">
                                        </dxe:ASPxTextBox>
                                    </td>
                                    <td>
                                        <dxe:ASPxTextBox ID="txtDigits" runat="server" ClientInstanceName="ctxtDigits" Width="100%" onkeypress="return onlyNumbers();">
                                        </dxe:ASPxTextBox>
                                    </td>
                                    <td>
                                        <dxe:ASPxTextBox ID="txtStartNo" runat="server" ClientInstanceName="ctxtStartNo" Width="100%" onkeypress="return onlyNumbers();">
                                        </dxe:ASPxTextBox>
                                    </td>

                                </tr>
                            </table>

                            <div class="pTop10  mTop5">
                                <asp:Button ID="BtnSaveexcel" runat="server" Text="Import(Add/Update)" OnClick="BtnSaveexcel_Click1" CssClass="btn btn-primary" />
                            </div>


                        </div>

                    </div>
                </div>

            </div>
        </div>
    </div>

    <div class="modal fade" id="modalSS" role="dialog">
        <div class="modal-dialog fullWidth">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Document Segment Log</h4>
                </div>
                <div class="modal-body">

                    <dxe:ASPxGridView ID="GvJvSearch" runat="server" AutoGenerateColumns="False" SettingsBehavior-AllowSort="true"
                        ClientInstanceName="cGvJvSearch" KeyFieldName="EmpLogId" Width="100%" OnDataBinding="GvJvSearch_DataBinding" Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight="400">

                        <SettingsBehavior ConfirmDelete="false" ColumnResizeMode="NextColumn" />
                        <Styles>
                            <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>
                            <FocusedRow HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridselectrow"></FocusedRow>
                            <LoadingPanel ImageSpacing="10px"></LoadingPanel>
                            <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>
                            <Footer CssClass="gridfooter"></Footer>
                        </Styles>
                        <Columns>
                            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="EmpLogId" Caption="LogID" SortOrder="Descending">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="CreatedDatetime" Caption="Date" Width="10%">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                                <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="SegmentCode" Caption="Customer Code" Width="10%">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                                <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="LoopNumber" Caption="Row Number" Width="13%">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="SegmentName" Width="8%" Caption="Name">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="FileName" Width="14%" Caption="File Name">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                                <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Description" Caption="Description" Width="10%" Settings-AllowAutoFilter="False">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Status" Caption="Status" Width="14%" Settings-AllowAutoFilter="False">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <SettingsSearchPanel Visible="false" />
                        <SettingsPager NumericButtonCount="200" PageSize="200" ShowSeparators="True" Mode="ShowPager">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="200,400,600" />
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                    </dxe:ASPxGridView>

                </div>

            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdnSyncCustomertoFSMWhileCreating" runat="server" />
</asp:Content>
