<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frm_ReportAssetDetail_iframe" CodeBehind="frm_ReportAssetDetail_iframe.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_rootfile.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />

    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>

    <link rel="stylesheet" href="../modalfiles/modal.css" type="text/css" />

    <script type="text/javascript" src="../modalfiles/modal.js"></script>


    <script type="text/javascript" language="javascript">

        function CallList(obj, obj1, obj2, obj3) {

            var o = document.getElementById("txtAssetMain")
            ajax_showOptions(obj, obj1, obj2, o.value);
        }
        FieldName = 'ctl00_ContentPlaceHolder1_Headermain1_cmbSegment';

        //    function CallList(obj,obj1,obj2,ob3)
        //    {
        //        alert(obj);
        //        ajax_showOptions(obj,obj1,obj2,obj3);
        //    }
        //    FieldName='ctl00_ContentPlaceHolder1_Headermain1_cmbSegment'; 

        function ShowHeight(obj) {
           // height();
        }
       
        function OnMainAssetChanged(cmbAssetMain) {

            cmbAssetSub.PerformCallback(cmbAssetMain.GetValue());
        }


        function ShowHide(obj) {
            document.getElementById("tr1").style.display = 'none';
            document.getElementById("tr2").style.display = 'none';
            document.getElementById("tblmain").style.display = 'none';
            document.getElementById("tblsub").style.display = 'none';
            document.getElementById("tr3").style.display = '';
            document.getElementById("tr4").style.display = '';

            height();
        }
        function ForFilter() {


            document.getElementById("tr1").style.display = '';
            document.getElementById("tr2").style.display = '';
            document.getElementById("tblmain").style.display = 'inline';
            document.getElementById("tblsub").style.display = 'inline';
            document.getElementById("tr3").style.display = 'none';
            document.getElementById("tr4").style.display = 'none';


            var val = 0;

            for (i = 0; i < document.form1.All.length; i++) {
                if (document.form1.All[i].checked == true) {
                    val = document.form1.All[i].value;

                }
                if (val == 'All') {
                    document.getElementById("tblsub").style.display = 'none';

                }
                if (val == 'Specific') {
                    document.getElementById("tblsub").style.display = 'inline';

                }

            }
           // height();


        }
        function ForCkeckBox() {
            var i = document.getElementById("rdnAll").value;
            if (i == 'All') {
                document.getElementById("tblsub").style.display = 'none';
            }



        }
        function ForCkeckBoxSpecific() {
            var j = document.getElementById("rdnSpecific").value;
            if (j == 'Specific') {
                document.getElementById("tblsub").style.display = 'inline';
            }



        }


        function OnMoreInfoClick(keyValue, MainValue) {
            //alert(keyValue);
            var url = 'ItemHistroy.aspx?id=' + keyValue;
            OnMoreInfoClick(url, "Show Items Of " + MainValue + "~" + keyValue + "", '960px', '470px')

        }


    </script>

  
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Report Asset Detail</h3>
        </div>
    </div>
    <div class="form_main">
        <div style="text-align: center; vertical-align: top">
            <%--<table class="TableMain100">
                <tr>
                    <td class="EHEADER">
                        <span style="color: Blue"><strong>Report Asset Detail</strong></span>&nbsp;</td>
                </tr>
            </table>--%>
            <table class="TableMain100">
            <tr>
                <td style="">
                    <table cellpadding="0" cellspacing="0">
                        <tr id="tr3">
                            <td style="" id="TrForFilter">
                                <a href="#" style="font-weight: bold; color: Blue" onclick="javascript:ForFilter();">Filter</a>
                                <%--|| <a href="#" style="font-weight: bold; color: Blue" onclick="javascript:search1();">
                                                    Search</a>--%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
                </table>
            <table class="TableMain100" style= "width:500px !important;text-align:left;" >

                
                <tr id="tr1">
                    <td class="gridcellright"
                        id="tdsettlement">
                        <span class="Ecoheadtxt" ><strong>AssetCategory:</strong></span>
                    </td>
                    <td class="mt" >
                        <asp:DropDownList ID="cmbAssetCategory" runat="server" Width="142px">
                            <asp:ListItem Selected="True">ALL</asp:ListItem>
                            <asp:ListItem Value="M">Movable</asp:ListItem>
                            <asp:ListItem Value="I">Immovable</asp:ListItem>
                            <asp:ListItem Value="W">Work In Progress</asp:ListItem>
                        </asp:DropDownList></td>
                    <td >
                        <table id="tblmain" style="width: 200px">
                            <td class="gridcellleft"
                                runat="server" id="Td4">
                                <span class="Ecoheadtxt" style="color: Blue"><strong>Asset Type:</strong></span></td>
                            <td 
                                runat="server" id="tdckeck">All:<input id="rdnAll" name="All" runat="server"  title="All"
                                    type="radio" value="All" onclick="ForCkeckBox();" /></td>
                            <td class="gridcellleft" 
                                runat="server" id="Td8">Specific:<input id="rdnSpecific" dir="ltr" name="All" runat="server"
                                    title="Specific" type="radio" value="Specific" checked="true"
                                    onclick="ForCkeckBoxSpecific();" /></td>
                        </table>
                    </td>
                    
                </tr>
                <tr>
                    <td colspan="3" 
                        id="Td9">
                        <table id="tblsub">
                            <td class="gridcellright">
                                <span style="color: Blue"><strong>AssetMain:</strong></span></td>
                            <td >
                                <asp:TextBox ID="txtAssetMain" runat="server" >ALL</asp:TextBox></td>
                            <td class="gridcellright" >
                                <span style="color: Blue"><strong>AssetSub:</strong></span></td>
                            <td >
                                <asp:TextBox ID="txtAssetSub" runat="server" Width="117px">ALL</asp:TextBox></td>
                        </table>
                    </td>
                    
                </tr>
                <tr id="tr2">
                    <td class="gridcellright" 
                        id="td1">
                        <span class="Ecoheadtxt" style="text-align: left"><strong>Location:<span style="color: #000000"></span></strong><span
                            style="color: #000000"></span></span></td>
                    <td class="lt" >
                        <asp:TextBox ID="txtLocation" runat="server" Width="137px">ALL</asp:TextBox></td>
                    <td  style="width: 195px">
                        <table>
                            <td class="gridcellleft" 
                                runat="server" id="td2">
                                <span class="Ecoheadtxt" style="color: Blue"><strong>UsedBy:</strong></span></td>
                            <td class="gridcellleft" 
                                id="td3">
                                <asp:TextBox ID="txtUsedBy" runat="server" >ALL</asp:TextBox>
                            </td>
                            <td >
                                <dxe:ASPxButton ID="btnShow" runat="server" AutoPostBack="False" Text="Show"
                                     CssClass="btn btn-primary">
                                    <ClientSideEvents Click="function(s, e) {grid.PerformCallback();}"></ClientSideEvents>
                                </dxe:ASPxButton>
                            </td>
                        </table>
                    </td>
                    
                </tr>
                <tr id="tr4">
                    <td colspan="9">
                        <dxe:ASPxGridView ID="AssetDetailGrid" ClientInstanceName="grid" Width="100%" runat="server"
                            KeyFieldName="MainAccountName"
                            AutoGenerateColumns="False" DataSourceID="AssetDetailDatasource"
                            OnCustomCallback="AssetDetailGrid_CustomCallback" OnHtmlEditFormCreated="AssetDetailGrid_HtmlEditFormCreated" OnHtmlRowCreated="AssetDetailGrid_HtmlRowCreated" OnRowInserting="AssetDetailGrid_RowInserting" OnHtmlDataCellPrepared="AssetDetailGrid_HtmlDataCellPrepared">
                            <ClientSideEvents EndCallback="function(s, e) {ShowHide(s.cpheight);
ShowHeight(s.cpHeight);}" />
                            <Templates>
                                <DetailRow>
                                    <table class="TableMain100">
                                        <tr>
                                            <td class="gridcellleft">
                                                <dxe:ASPxGridView ID="DetailAssetGrid" runat="server"
                                                    Width="100%" KeyFieldName="AssetDetail_ID" DataSourceID="DataSourceDetailAssetGrid"
                                                    OnBeforePerformDataSelect="AssetDetailGrid_DataSelect" AutoGenerateColumns="False" OnHtmlRowCreated="DetailAssetGrid_HtmlRowCreated" OnHtmlDataCellPrepared="DetailAssetGrid_HtmlDataCellPrepared">
                                                    <Styles>
                                                    </Styles>
                                                    <Columns>
                                                        <dxe:GridViewDataTextColumn FieldName="SubAccountCode" Caption="SubItems" VisibleIndex="0">
                                                            <CellStyle CssClass="gridcellleft">
                                                            </CellStyle>
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn FieldName="OpeningBalance" Caption="Opening Balance"
                                                            VisibleIndex="1">
                                                            <CellStyle CssClass="gridcellright">
                                                            </CellStyle>
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn FieldName="Additions" Caption="Additions" VisibleIndex="2">
                                                            <CellStyle CssClass="gridcellright">
                                                            </CellStyle>
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn FieldName="Disposals" Caption="Disposals" VisibleIndex="3">
                                                            <CellStyle CssClass="gridcellright">
                                                            </CellStyle>
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn FieldName="Depreciation" Caption="Depreciation" VisibleIndex="4">
                                                            <CellStyle CssClass="gridcellright">
                                                            </CellStyle>
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn FieldName="CostPrice" Caption="Cost Price" VisibleIndex="5">
                                                            <CellStyle CssClass="gridcellright">
                                                            </CellStyle>
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn FieldName="NetValue" Caption="NetValue" VisibleIndex="6">
                                                            <CellStyle CssClass="gridcellright">
                                                            </CellStyle>
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn FieldName="CustomerName" Caption="User" VisibleIndex="7" Visible="False">
                                                            <CellStyle CssClass="gridcellright">
                                                            </CellStyle>
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn FieldName="Desciption" Caption="Location" VisibleIndex="7" Visible="False">
                                                            <CellStyle CssClass="gridcellright">
                                                            </CellStyle>
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn Caption="#" VisibleIndex="7">
                                                            <DataItemTemplate>
                                                                <a href="javascript:void(0);">History</a>
                                                                <%--<a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')">
                                                    History</a>--%>
                                                            </DataItemTemplate>
                                                            <EditFormSettings Visible="False" />
                                                            <CellStyle Wrap="False">
                                                            </CellStyle>

                                                        </dxe:GridViewDataTextColumn>

                                                    </Columns>
                                                </dxe:ASPxGridView>
                                    </table>
                                </DetailRow>
                            </Templates>
                            <SettingsBehavior ConfirmDelete="True" />
                            <Styles>
                            </Styles>
                            <Columns>
                                <dxe:GridViewDataTextColumn Caption="Asset A/C" FieldName="MainAccountName" VisibleIndex="0"
                                    Width="40%">
                                    <CellStyle CssClass="gridcellleft">
                                    </CellStyle>
                                    <PropertiesTextEdit Width="90px">
                                    </PropertiesTextEdit>
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="B/F Value" FieldName="OpeningBalance" UnboundType="Decimal"
                                    VisibleIndex="1" Width="15%">
                                    <CellStyle CssClass="gridcellleft">
                                    </CellStyle>
                                    <PropertiesTextEdit DisplayFormatString="{0:N2}" Width="90px">
                                        <MaskSettings Mask="0,00,000..9,99,999" />
                                    </PropertiesTextEdit>
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Addition" FieldName="Additions" VisibleIndex="2"
                                    Width="15%">
                                    <CellStyle CssClass="gridcellright">
                                    </CellStyle>
                                    <PropertiesTextEdit DisplayFormatString="{0:N2}" Width="90px">
                                        <MaskSettings IncludeLiterals="DecimalSymbol" Mask="$&lt;0..99999g&gt;.&lt;00..99&gt;" />
                                    </PropertiesTextEdit>
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Disposal" FieldName="Disposals" UnboundType="Decimal"
                                    VisibleIndex="4" Width="15%">
                                    <CellStyle CssClass="gridcellright">
                                    </CellStyle>
                                    <PropertiesTextEdit DisplayFormatString="{0:N2}" EnableClientSideAPI="True" Width="90px">
                                        <MaskSettings IncludeLiterals="DecimalSymbol" />
                                    </PropertiesTextEdit>
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Depreciation" FieldName="Depreciation" VisibleIndex="3"
                                    Width="15%">
                                    <CellStyle CssClass="gridcellright">
                                    </CellStyle>
                                    <PropertiesTextEdit DisplayFormatString="{0:N2}" Width="90px">
                                    </PropertiesTextEdit>
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Net Value" FieldName="NetValue" VisibleIndex="5">
                                    <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                        <MaskSettings Mask="$&lt;0..99999g&gt;.&lt;00..99&gt;" />
                                    </PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="OriginalCost" FieldName="CostPrice" VisibleIndex="6">
                                    <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                    </PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <Settings ShowFooter="True" />
                            <SettingsDetail ShowDetailRow="True" />
                        </dxe:ASPxGridView>
                        <asp:SqlDataSource ID="DatasourceAssetSub" runat="server" ConflictDetection="CompareAllValues"
                            SelectCommand=""></asp:SqlDataSource>
                        <asp:SqlDataSource ID="DatasourceAssetMain" runat="server" 
                            SelectCommand="Select M.MainAccount_SubLedgerType,S.SubAccount_MainAcReferenceID,S.SubAccount_Code from Master_SubAccount S,Master_MainAccount M where M.MainAccount_SubLedgerType='Custom' And S.SubAccount_MainAcReferenceID=M.MainAccount_AccountCode"></asp:SqlDataSource>
                        <asp:SqlDataSource ID="DatasourceFinancialYear" runat="server" 
                            SelectCommand="SELECT [FinYear_ID], [FinYear_Code] FROM [Master_FinYear] Order By FinYear_Code Desc "></asp:SqlDataSource>
                        <asp:SqlDataSource ID="AssetDetailDatasource" runat="server"></asp:SqlDataSource>
                        <asp:SqlDataSource ID="DataSourceDetailAssetGrid" runat="server"
                            SelectCommand=""></asp:SqlDataSource>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="display: none">
                        <asp:TextBox ID="txtLocation_hidden" runat="server" Visible="true"></asp:TextBox>
                        <asp:TextBox ID="txtUsedBy_hidden" runat="server" Visible="true"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <div style="text-align: right">
                &nbsp;
            </div>
        </div>
    </div>
</asp:Content>
