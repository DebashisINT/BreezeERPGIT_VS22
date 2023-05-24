<%--================================================== Revision History =============================================
1.0   Pallab    V2.0.38      24-05-2023          0026212: UOM CONVERSION RATES module design modification & check in small device
====================================================== Revision History =============================================--%>

<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.SettingsOptions.management_SettingsOptions_frm_UOMConversion" Codebehind="frm_UOMConversion.aspx.cs" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    

    

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
    
    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    
    <style>
        select
        {
            z-index: 1;
            margin-bottom: 0;
            -webkit-appearance: auto;
        }

        /*#grid {
            max-width: 98% !important;
        }*/
        #FormDate , #toDate , #dtTDate , #dt_PLQuote , #dt_partyInvDt
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #dt_PLQuote_B-1 , #dt_partyInvDt_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #dt_PLQuote_B-1 #dt_PLQuote_B-1Img ,
        #dt_partyInvDt_B-1 #dt_partyInvDt_B-1Img
        {
            display: none;
        }

        /*select
        {
            -webkit-appearance: auto;
        }*/

        .calendar-icon
        {
                right: 18px;
                bottom: 6px;
        }
        .padTabtype2 > tbody > tr > td
        {
            vertical-align: bottom;
        }
        #rdl_Salesquotation
        {
            margin-top: 0px;
        }

        .lblmTop8>span, .lblmTop8>label
        {
            margin-top: 8px !important;
        }

        .col-md-2, .col-md-4 {
    margin-bottom: 5px;
}

        .simple-select::after
        {
                top: 34px;
            right: 13px;
        }

        .dxeErrorFrameWithoutError_PlasticBlue.dxeControlsCell_PlasticBlue
        {
            padding: 0;
        }

        .aspNetDisabled
        {
            background: #f3f3f3 !important;
        }

        .backSelect {
    background: #42b39e !important;
}

        #ddlInventory
        {
                -webkit-appearance: auto;
        }

        /*.wid-90
        {
            width: 100%;
        }
        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-content
        {
            width: 97%;
        }*/
        .newLbl
        {
                margin: 3px 0 !important;
        }

        .lblBot > span, .lblBot > label
        {
                margin-bottom: 3px !important;
        }

        .col-md-2 > label, .col-md-2 > span, .col-md-1 > label, .col-md-1 > span
        {
            margin-top: 8px !important;
            font-size: 14px;
        }

        .col-md-6 span
        {
            font-size: 14px;
        }

        #gridDEstination
        {
            width:99% !important;
        }

        #txtEntity , #txtCustName
        {
            width: 100%;
        }
        .col-md-6 span
        {
            margin-top: 8px !important;
        }

        .rds
        {
            margin-top: 10px !important;
        }

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue , select
        {
            height: 30px !important;
            
        }
        select
        {
            background-color: transparent;
                padding: 0 20px 0 5px !important;
        }

        .newLbl
        {
            font-size: 14px;
            margin: 3px 0 !important;
            font-weight: 500 !important;
            margin-bottom: 0 !important;
            line-height: 20px;
        }

        .crossBtn {
            top: 25px !important;
            right: 25px !important;
        }

        .wrapHolder
        {
            height: 60px;
        }
        #rdl_SaleInvoice
        {
            margin-top: 12px;
        }

        .dxeRoot_PlasticBlue
        {
            width: 100% !important;
        }

        #ShowFilter {
            padding-bottom: 0px !important;
        }

        @media only screen and (max-width: 1380px) and (min-width: 1300px)
        {
            .col-xs-1, .col-xs-2, .col-xs-3, .col-xs-4, .col-xs-5, .col-xs-6, .col-xs-7, .col-xs-8, .col-xs-9, .col-xs-10, .col-xs-11, .col-xs-12, .col-sm-1, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-sm-10, .col-sm-11, .col-sm-12, .col-md-1, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-md-10, .col-md-11, .col-md-12, .col-lg-1, .col-lg-2, .col-lg-3, .col-lg-4, .col-lg-5, .col-lg-6, .col-lg-7, .col-lg-8, .col-lg-9, .col-lg-10, .col-lg-11, .col-lg-12
            {
                 padding-right: 10px;
                 padding-left: 10px;
            }
            .simple-select::after
        {
                top: 34px;
            right: 8px;
        }
            .calendar-icon
        {
                right: 14px;
                bottom: 6px;
        }
        }

    </style>
    <%--Rev end 1.0--%>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">

        function PopulateGrid(obj) {

            grid.PerformCallback(obj);
        }
        function Changestatus(obj) {
            var URL = "changeuomsetting.aspx?id=" + obj;
            editwin = dhtmlmodal.open("Editbox", "iframe", URL, "Add/Edit Conversion Multiplier", "width=995px,center=0,resize=1,top=-1", "recal");
            editwin.onclose = function () {
                grid.PerformCallback();
            }
        }

    function ChangestatusNew(obj) {
        debugger;
        var url = 'changeuomsetting.aspx?id=' + obj;
        popup.SetContentUrl(url);
        popup.Show();
    }
    function ShowHideFilter(obj) {
        grid.PerformCallback(obj);
    }
    function callback() {
        grid.PerformCallback();
    }
    function callheight(obj) {
        height();
        // parent.CallMessage();
    }
    </script>
     <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>UOM CONVERSION RATES</h3>
        </div>

    </div>
        <div class="form_main">

              <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" ContentUrl="changeuomsetting.aspx"
                                            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"    ClientInstanceName="popup" Height="300px"
                                            Width="500px" HeaderText="Add/Modify UOM" Modal="true" AllowResize="true" ResizingMode="Postponed" >
                                            <ContentCollection>
                                                <dxe:PopupControlContentControl runat="server">
                                                </dxe:PopupControlContentControl>
                                            </ContentCollection>
                                        </dxe:ASPxPopupControl>
            <table class="TableMain100">
               <%-- <tr>
                    <td class="EHEADER" style="text-align: center;">
                        <strong><span style="color: #000099">UOM CONVERSION RATES </span></strong>
                    </td>
                </tr>--%>
                <tr>
                    <td style="text-align: left; vertical-align: top">
                        <table class="mb-10">
                            <tr>
                                <td id="ShowFilter">
                                   <%--<a href="javascript:ShowHideFilter('s');" class="btn btn-success"><span>Shailter</span></a>--%>
                                <a href="javascript:void(0);" onclick="javascript:ChangestatusNew('Add');" class="btn btn-success">Add New</a>
                                </td>
                                <td id="Td1">
                                     <%--<a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>--%>
                                <asp:DropDownList ID="cmbExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true"  >
                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                     <asp:ListItem Value="2">XLS</asp:ListItem>
                                     <asp:ListItem Value="3">RTF</asp:ListItem>
                                     <asp:ListItem Value="4">CSV</asp:ListItem>
                        
                                </asp:DropDownList>
                                </td>
                               
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                    <dxe:ASPxGridView ID="grdDocuments" runat="server" AutoGenerateColumns="False"
                        KeyFieldName="tmpconversion_id" Width="100%" OnRowDeleting="grdDocuments_RowDeleting" ClientInstanceName="grid"
                        OnCustomCallback="grdDocuments_CustomCallback" OnHtmlRowCreated="grdDocuments_HtmlRowCreated">
                        <templates>
                                <TitlePanel>
                                    <table style="width: 100%">
                                        <tr>
                                            <td align="right">
                                                <table width="200px">
                                                    <tr>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </TitlePanel>
                                <EditForm>
                                </EditForm>
                            </templates>
                        <settings showgrouppanel="True" />
                        <settingsbehavior confirmdelete="True" allowfocusedrow="True" />
                        <columns>
                                <dxe:GridViewDataTextColumn FieldName="tmpconversion_id" ReadOnly="True" VisibleIndex="0"
                                    Visible="false">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="left" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="tmpconversion_fromuomname" Caption="From Conversion Name" Width="25%" ReadOnly="True"
                                    VisibleIndex="0">
                                    <CellStyle HorizontalAlign="left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="left" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="tmpconversion_touomname" Caption="To Conversion Name" Width="25%"
                                    VisibleIndex="1" Visible="true">
                                    <CellStyle HorizontalAlign="left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="left" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="tmpconversion_multiplier" Caption="Converted Multipier" Width="25%"
                                    VisibleIndex="2">
                                    <CellStyle HorizontalAlign="left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="left" />
                                </dxe:GridViewDataTextColumn>
                               <%-- <dxe:GridViewDataTextColumn FieldName="tmp_convuomname" Caption="Converted To"
                                    Width="22%" VisibleIndex="3">
                                    <CellStyle HorizontalAlign="left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="left" />
                                </dxe:GridViewDataTextColumn>--%>
                               <dxe:GridViewDataTextColumn FieldName="change" Caption="Action" Width="15%" VisibleIndex="3">
                                    <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                   
                                    <HeaderStyle HorizontalAlign="Center" />
                                     <Settings AllowAutoFilter="False" AllowGroup="False" />
                                      <DataItemTemplate >
                                         <img src="../../../assests/images/changeIcon.png" />
                                      </DataItemTemplate>
                                </dxe:GridViewDataTextColumn>
                                <%--<dxe:GridViewCommandColumn VisibleIndex="4" Width="10%" >--%>
                               <%-- <EditButton Visible="false">
                                </EditButton>
                                <DeleteButton Visible="True">
                                </DeleteButton>--%>
                                <%--<HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                   
                                    <a href="javascript:void(0);" onclick="Changestatus('Add')"><span style="color: #000099;
                                        text-decoration: underline">Add New</span> </a>
                                    
                                </HeaderTemplate>

                            </dxe:GridViewCommandColumn>--%>
                           
                            </columns>

                        <settingssearchpanel visible="True" />
                        <settings showgrouppanel="True" showstatusbar="Hidden" showfilterrow="true" showfilterrowmenu="True" />

                        <SettingsCommandButton>
                <ClearFilterButton Text="Clear">
                </ClearFilterButton>

               
                <DeleteButton Image-Url="/assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                </DeleteButton>

               


            </SettingsCommandButton>
                        <%--<SettingsBehavior AllowFocusedRow="True" ColumnResizeMode="NextColumn" />--%>
                        <settings showgrouppanel="True" showstatusbar="Visible" />
                        <styles>
                            <Header SortingImageSpacing="5px" ImageSpacing="5px" CssClass="gridheader"></Header>
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
                        <settingsediting mode="PopupEditForm" popupeditformheight="200px" popupeditformhorizontalalign="Center"
                            popupeditformmodal="True" popupeditformverticalalign="WindowCenter" popupeditformwidth="600px"
                            editformcolumncount="1" />
                        <settingstext popupeditformcaption="Add/Modify " ConfirmDelete="Confirm delete?" />

                    </dxe:ASPxGridView>
                    <%--   <asp:SqlDataSource ID="grddoc" runat="server"
                            SelectCommand="" DeleteCommand="DELETE FROM [tbl_master_forms] WHERE [frm_id] = @formID">
                            <DeleteParameters>
                                <asp:Parameter Name="formID" Type="int32" />
                            </DeleteParameters>
                            <SelectParameters>
                                <asp:SessionParameter Name="userlist" SessionField="userchildHierarchy" Type="string" />
                            </SelectParameters>
                        </asp:SqlDataSource>--%>
                </td>
                </tr>
                <tr>
                    <td style="display: none;">
                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox></td>
                </tr>
            </table>
            <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
            </dxe:ASPxGridViewExporter>
        </div>
    </div>
</asp:Content>