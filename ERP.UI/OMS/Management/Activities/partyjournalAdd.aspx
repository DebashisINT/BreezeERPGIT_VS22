<%--================================================== Revision History =============================================
1.0   Pallab    V2.0.38      23-05-2023          0026201: Add Party Journal - Auto module design modification & check in small device
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="partyjournalAdd.aspx.cs" Inherits="ERP.OMS.Management.Activities.partyjournalAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/partyJournalAuto.css" rel="stylesheet" />
    <script src="https://cdn3.devexpress.com/jslib/20.2.3/js/dx.all.js"></script>
    <link href="https://cdn3.devexpress.com/jslib/20.2.3/css/dx.common.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/20.2.3/css/dx.common.css" />
    <link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/20.2.3/css/dx.light.css" />
    <script src="JS/partyjournalAdd.js"></script>

    <style>
        #data-grid-demo {
            min-height: 700px;
        }

        .options {
            margin-top: 20px;
            padding: 20px;
            background: #f5f5f5;
        }

            .options .caption {
                font-size: 18px;
                font-weight: 500;
            }

        .option {
            margin-top: 10px;
        }

            .option > span {
                width: 120px;
                display: inline-block;
            }

            .option > .dx-widget {
                display: inline-block;
                vertical-align: middle;
                width: 100%;
                max-width: 350px;
            }
            .dxgvControl_PlasticBlue td.dxgvBatchEditModifiedCell_PlasticBlue {
                background:#fff
            }
    </style>
    <script>
        function OnAddButtonClick() {
            window.location.href = '/OMS/management/Activities/partyjournalAdd.aspx'
        }
        function back() {
            window.location.href = '/OMS/management/Activities/partyjournallist.aspx'
        }


    </script>

    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    
    <style>
        select
        {
            z-index: 1;
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

        .boxedView
        {
            background: none;
            border: none;
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
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>Add Party Journal - Auto</h3>
        </div>
        <div id="btncross" class="crossBtn" style="display: block; margin-left: 50px;" onclick="back()"><a href="javascript:ReloadPage()"><i class="fa fa-times"></i></a></div>
    </div>
        <div class="form_main">
        <div class="boxedView">
            <div class="row">
                <div class="col-md-3" id="divNumbering" runat="server">
                    <label>Numbering Scheme <span style="color: red">*</span></label>
                    <div>
                        <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                             <ClientSideEvents SelectedIndexChanged="CmbScheme_ValueChange"></ClientSideEvents>
                        </dxe:ASPxComboBox>
                    </div>
                </div>
                <div class="col-md-3">
                    <label>Document Number</label>
                    <div>
                       <dxe:ASPxTextBox runat="server" ID="txtVoucherNo" ClientInstanceName="ctxtVoucherNo" MaxLength="16" Text="Auto" ClientEnabled="false" Width="100%">
                       </dxe:ASPxTextBox>
                    </div>
                </div>
                <div class="col-md-3">
                    <label>Date <span style="color: red">*</span></label>
                    <div>
                        <dxe:ASPxDateEdit ID="dtTDate" runat="server" ClientInstanceName="cdtTDate" EditFormat="Custom" AllowNull="false"
                            Font-Size="12px" UseMaskBehavior="True" Width="100%" EditFormatString="dd-MM-yyyy" CssClass="pull-left">
                            <ButtonStyle Width="13px"></ButtonStyle>
                            
                        </dxe:ASPxDateEdit>
                        <%--Rev 1.0--%>
                        <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                        <%--Rev end 1.0--%>
                    </div>
                </div>
            </div>
        </div>

        <div class="clearfix">
            <h5 style="margin-top: 0;">Party (Credit)</h5>
            <dxe:ASPxGridView runat="server" ClientInstanceName="grid" ID="grid"
                Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" Settings-ShowFooter="false"
                SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto"
                OnCellEditorInitialize="grid_CellEditorInitialize" OnCustomCallback="grid_CustomCallback"
                OnRowInserting="grid_RowInserting"
                OnRowUpdating="grid_RowUpdating"
                OnRowDeleting="grid_RowDeleting"
                OnBatchUpdate="grid_BatchUpdate"
                OnDataBinding="grid_DataBinding"
                KeyFieldName="SrlNo"
                
                Settings-VerticalScrollableHeight="100" CommandButtonInitialize="false" Settings-ShowStatusBar="Hidden" Settings-HorizontalScrollBarMode="Auto">
                <SettingsPager Visible="false"></SettingsPager>
                <Styles>
                    <StatusBar CssClass="statusBar">
                    </StatusBar>
                </Styles>
                <Columns>
                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="50" VisibleIndex="0" Caption=" ">
                        <CustomButtons>
                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                            </dxe:GridViewCommandColumnCustomButton>
                        </CustomButtons>
                    </dxe:GridViewCommandColumn>

                    <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl#" ReadOnly="true" VisibleIndex="1" Width="5%">
                        <PropertiesTextEdit>
                        </PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataButtonEditColumn ReadOnly="true" FieldName="Unit" Caption="Unit" VisibleIndex="2">

                        <PropertiesButtonEdit>

                            <ClientSideEvents ButtonClick="UnitButnClick" KeyDown="UnitKeyDown" />
                            <Buttons>

                                <dxe:EditButton Text="..." Width="20px">
                                </dxe:EditButton>
                            </Buttons>
                        </PropertiesButtonEdit>
                    </dxe:GridViewDataButtonEditColumn>

                    <dxe:GridViewDataButtonEditColumn ReadOnly="true" FieldName="DocumentNumbering" Caption="Document Numbering" VisibleIndex="3" Width="200px">

                        <PropertiesButtonEdit>

                            <ClientSideEvents ButtonClick="DocumentNumberingButnClick" KeyDown="DocumentNumberingKeyDown" />
                            <Buttons>

                                <dxe:EditButton Text="..." Width="20px">
                                </dxe:EditButton>
                            </Buttons>
                        </PropertiesButtonEdit>
                    </dxe:GridViewDataButtonEditColumn>

                    <dxe:GridViewDataButtonEditColumn ReadOnly="true" FieldName="Customer" Caption="Party" VisibleIndex="4">

                        <PropertiesButtonEdit>

                            <ClientSideEvents ButtonClick="CustomerButnClick" KeyDown="CustomerKeyDown" />
                            <Buttons>

                                <dxe:EditButton Text="..." Width="20px">
                                </dxe:EditButton>
                            </Buttons>
                        </PropertiesButtonEdit>
                    </dxe:GridViewDataButtonEditColumn>

                    <dxe:GridViewDataButtonEditColumn ReadOnly="true" FieldName="Project" Caption="Project" VisibleIndex="5">

                        <PropertiesButtonEdit>

                            <ClientSideEvents ButtonClick="ProjectClick" KeyDown="ProjectKeyDown" />
                            <Buttons>

                                <dxe:EditButton Text="..." Width="20px">
                                </dxe:EditButton>
                            </Buttons>
                        </PropertiesButtonEdit>
                    </dxe:GridViewDataButtonEditColumn>

                    <dxe:GridViewDataButtonEditColumn ReadOnly="true" FieldName="RefDoc" Caption="Ref. Doc." VisibleIndex="6">

                        <PropertiesButtonEdit>

                            <ClientSideEvents ButtonClick="RefDocClick" KeyDown="RefDocKeyDown" />
                            <Buttons>

                                <dxe:EditButton Text="..." Width="20px">
                                </dxe:EditButton>
                            </Buttons>
                        </PropertiesButtonEdit>
                    </dxe:GridViewDataButtonEditColumn>

                    <dxe:GridViewDataTextColumn VisibleIndex="7" Caption="Amount" ReadOnly="true" FieldName="Amount" Width="130">
                        <PropertiesTextEdit Style-HorizontalAlign="Right">
                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                            <ValidationSettings RequiredField-IsRequired="false" Display="None"></ValidationSettings>

                        </PropertiesTextEdit>
                        <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="BalAmount" ReadOnly="true" Caption="Bal. Amount" Width="130">
                        <PropertiesTextEdit Style-HorizontalAlign="Right">
                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                            <ValidationSettings RequiredField-IsRequired="false" Display="None"></ValidationSettings>

                        </PropertiesTextEdit>
                        <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn VisibleIndex="9" Caption="Adj. Amount" FieldName="AdjAmount" Width="130">
                        <PropertiesTextEdit Style-HorizontalAlign="Right">
                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                            <ValidationSettings RequiredField-IsRequired="false" Display="None"></ValidationSettings>
                             <ClientSideEvents LostFocus="AdjLostfocus" />
                        </PropertiesTextEdit>
                        <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn VisibleIndex="10" Caption="Remarks" ReadOnly="false" FieldName="Remarks" Width="200">
                        <PropertiesTextEdit>
                        </PropertiesTextEdit>
                        <CellStyle Wrap="False" HorizontalAlign="Left" CssClass="gridcellleft"></CellStyle>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn FieldName="UnitID" ReadOnly="True" Width="0"
                        EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide"
                        PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn FieldName="ProjectId" Caption="ProjectId" VisibleIndex="16" ReadOnly="True" Width="0"
                        EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide"
                        PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn FieldName="SchemaID" Caption="hidden Field Id" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn FieldName="CustomerID" Caption="hidden Field Id" VisibleIndex="17" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn FieldName="DocId" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide">
                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn FieldName="UpdateEdit" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" Width="0">
                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="100" VisibleIndex="11" Caption=" ">
                        <CustomButtons>
                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="AddNew" Image-Url="/assests/images/add.png">
                            </dxe:GridViewCommandColumnCustomButton>
                        </CustomButtons>
                    </dxe:GridViewCommandColumn>


                </Columns>



                <ClientSideEvents EndCallback="OnEndCallback" CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex" BatchEditStartEditing="GetVisibleIndex" />
                <SettingsDataSecurity AllowEdit="true" />
                <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                    <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                </SettingsEditing>
            </dxe:ASPxGridView>
        </div>
        

        <div class="clearfix">
            <h5>Party (Debit)</h5>

            <dxe:ASPxGridView runat="server" ClientInstanceName="vendorgrid" ID="vendorgrid"
                Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" Settings-ShowFooter="false"
                SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto"
                OnCellEditorInitialize="vendorgrid_CellEditorInitialize"
                OnCustomCallback="vendorgrid_CustomCallback"
                OnRowInserting="vendorgrid_RowInserting"
                OnRowUpdating="vendorgrid_RowUpdating"
                OnRowDeleting="vendorgrid_RowDeleting"
                OnBatchUpdate="vendorgrid_BatchUpdate"
                OnDataBinding="vendorgrid_DataBinding"
                KeyFieldName="SrlNo"
                Settings-VerticalScrollableHeight="100" CommandButtonInitialize="false" Settings-ShowStatusBar="Hidden"  Settings-HorizontalScrollBarMode="Auto">
                <SettingsPager Visible="false"></SettingsPager>
                <Styles>
                    <StatusBar CssClass="statusBar">
                    </StatusBar>
                </Styles>
                <Columns>
                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="50" VisibleIndex="0" Caption=" ">
                        <CustomButtons>
                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="VendorCustomDelete" Image-Url="/assests/images/crs.png">
                            </dxe:GridViewCommandColumnCustomButton>
                        </CustomButtons>
                    </dxe:GridViewCommandColumn>

                    <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl#" ReadOnly="true" VisibleIndex="1" Width="5%">
                        <PropertiesTextEdit>
                        </PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataButtonEditColumn ReadOnly="true" FieldName="Unit" Caption="Unit" VisibleIndex="2">

                        <PropertiesButtonEdit>

                            <ClientSideEvents ButtonClick="VendorUnitButnClick" KeyDown="VendorUnitKeyDown" />
                            <Buttons>

                                <dxe:EditButton Text="..." Width="20px">
                                </dxe:EditButton>
                            </Buttons>
                        </PropertiesButtonEdit>
                    </dxe:GridViewDataButtonEditColumn>

                    <dxe:GridViewDataButtonEditColumn ReadOnly="true" FieldName="DocumentNumbering" Caption="Document Numbering" VisibleIndex="3" Width="200px">

                        <PropertiesButtonEdit>

                            <ClientSideEvents ButtonClick="VendorDocumentNumberingButnClick" KeyDown="VendorDocumentNumberingKeyDown" />
                            <Buttons>

                                <dxe:EditButton Text="..." Width="20px">
                                </dxe:EditButton>
                            </Buttons>
                        </PropertiesButtonEdit>
                    </dxe:GridViewDataButtonEditColumn>

                    <dxe:GridViewDataButtonEditColumn ReadOnly="true" FieldName="Vendor" Caption="Party" VisibleIndex="4">

                        <PropertiesButtonEdit>

                            <ClientSideEvents ButtonClick="VendorButnClick" KeyDown="VendorKeyDown" />
                            <Buttons>

                                <dxe:EditButton Text="..." Width="20px">
                                </dxe:EditButton>
                            </Buttons>
                        </PropertiesButtonEdit>
                    </dxe:GridViewDataButtonEditColumn>

                    <dxe:GridViewDataButtonEditColumn ReadOnly="true" FieldName="Project" Caption="Project" VisibleIndex="5">

                        <PropertiesButtonEdit>

                            <ClientSideEvents ButtonClick="VendorProjectClick" KeyDown="VendorProjectKeyDown" />
                            <Buttons>

                                <dxe:EditButton Text="..." Width="20px">
                                </dxe:EditButton>
                            </Buttons>
                        </PropertiesButtonEdit>
                    </dxe:GridViewDataButtonEditColumn>

                    <dxe:GridViewDataButtonEditColumn ReadOnly="true" FieldName="RefDoc" Caption="Ref. Doc." VisibleIndex="6">

                        <PropertiesButtonEdit>

                            <ClientSideEvents ButtonClick="VendorRefDocClick" KeyDown="VendorRefDocKeyDown" />
                            <Buttons>

                                <dxe:EditButton Text="..." Width="20px">
                                </dxe:EditButton>
                            </Buttons>
                        </PropertiesButtonEdit>
                    </dxe:GridViewDataButtonEditColumn>

                    <dxe:GridViewDataTextColumn VisibleIndex="7" Caption="Amount" ReadOnly="true" FieldName="Amount" Width="130">
                        <PropertiesTextEdit Style-HorizontalAlign="Right">
                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                            <ValidationSettings RequiredField-IsRequired="false" Display="None"></ValidationSettings>

                        </PropertiesTextEdit>
                        <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="BalAmount" ReadOnly="true" Caption="Bal. Amount" Width="130">
                        <PropertiesTextEdit Style-HorizontalAlign="Right">
                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                            <ValidationSettings RequiredField-IsRequired="false" Display="None"></ValidationSettings>

                        </PropertiesTextEdit>
                        <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn VisibleIndex="9" Caption="Adj. Amount" FieldName="AdjAmount" Width="130">
                        <PropertiesTextEdit Style-HorizontalAlign="Right">
                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                            <ValidationSettings RequiredField-IsRequired="false" Display="None"></ValidationSettings>
                            <ClientSideEvents LostFocus="AdjLostfocusVendor" />
                        </PropertiesTextEdit>
                        <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn VisibleIndex="10" Caption="Remarks" ReadOnly="false" FieldName="Remarks" Width="200">
                        <PropertiesTextEdit>
                        </PropertiesTextEdit>
                        <CellStyle Wrap="False" HorizontalAlign="Left" CssClass="gridcellleft"></CellStyle>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn FieldName="UnitID" ReadOnly="True" Width="0"
                        EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide"
                        PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn FieldName="ProjectId" Caption="ProjectId" VisibleIndex="16" ReadOnly="True" Width="0"
                        EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide"
                        PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn FieldName="SchemaID" Caption="hidden Field Id" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn FieldName="VendorID" Caption="hidden Field Id" VisibleIndex="17" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn FieldName="DocId" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide">
                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn FieldName="UpdateEdit" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" Width="0">
                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="100" VisibleIndex="11" Caption=" ">
                        <CustomButtons>
                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="VendorAddNew" Image-Url="/assests/images/add.png">
                            </dxe:GridViewCommandColumnCustomButton>
                        </CustomButtons>
                    </dxe:GridViewCommandColumn>


                </Columns>



                <ClientSideEvents EndCallback="OnEndCallbackVendor" CustomButtonClick="OnCustomButtonClickVendor" RowClick="GetVisibleIndexVendor" BatchEditStartEditing="GetVisibleIndexVendor" />
                <SettingsDataSecurity AllowEdit="true" />
                <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                    <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                </SettingsEditing>
            </dxe:ASPxGridView>


        </div>
        <div class="content reverse horizontal-images clearfix" style="width: 100%; margin-right: 0; padding: 8px 0; height: auto; border-top: 1px solid #ccc; border-bottom: 1px solid #ccc; border-radius: 0;">
            <ul>
                <li class="clsbnrLblTotalQty">
                    <div class="horizontallblHolder">
                        <table>
                            <tbody>
                                <tr>
                                    <td>
                                        <span class="dxeBase_PlasticBlue" id="ASPxLabel10">Total Credit</span>
                                    </td>
                                    <td>
                                        <span class="dxeBase_PlasticBlue" id="totCr">0.00</span>
                                    </td>
                                </tr>

                            </tbody>
                        </table>
                    </div>
                </li>
                <li class="clsbnrLblTotalQty">
                    <div class="horizontallblHolder">
                        <table>
                            <tbody>
                                <tr>
                                    <td>
                                        <span class="dxeBase_PlasticBlue" id="ASPxLabel10">Total Debit</span>
                                    </td>
                                    <td>
                                        <span class="dxeBase_PlasticBlue" id="totDr">0.00</span>
                                    </td>
                                </tr>

                            </tbody>
                        </table>
                    </div>
                </li>
                <li class="clsbnrLblTotalQty">
                    <div class="horizontallblHolder">
                        <table>
                            <tbody>
                                <tr>
                                    <td>
                                        <span class="dxeBase_PlasticBlue" id="ASPxLabel10">Balance</span>
                                    </td>
                                    <td>
                                        <span class="dxeBase_PlasticBlue" id="RunningTot">0.00</span>
                                    </td>
                                </tr>

                            </tbody>
                        </table>
                    </div>
                </li>
            </ul>
        </div>

        <div class="mTop5">
            <button type="button" class="btn btn-primary hide" onclick="SaveNew();">Save & New</button>
            <button type="button" id="divSave" runat="server" class="btn btn-primary" onclick="SaveExit();">Save & Exit</button>
        </div>
    </div>
    </div>
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>



    <%--Customer Modal--%>


    <div class="modal fade" id="UnitNumberingnidal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Select Branch</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div id="gridContainer"></div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="Numberingnidal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Numbering for Journal</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div id="gridContainerNumbering"></div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="Customermodal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Party (Credit)</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div id="gridContainerCustomer"></div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="Projectmodal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Project</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div id="gridContainerProject"></div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="RefDocmodal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Document</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div id="RefDocProject"></div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
                </div>
            </div>
        </div>
    </div>


    <%--end customer modal--%>



    <%--Vendor Modal--%>


    <div class="modal fade" id="UnitNumberingnidalVendor" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabelVendor">Select Branch</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div id="gridContainerVendor"></div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="NumberingnidalVendor" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Numbering for Journal</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div id="gridContainerNumberingVendor"></div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="CustomermodalVendor" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Party (Debit)</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div id="gridContainerCustomerVendor"></div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="ProjectmodalVendor" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Project</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div id="gridContainerProjectVendor"></div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="RefDocmodalVendor" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Document</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div id="RefDocProjectVendor"></div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
                </div>
            </div>
        </div>
    </div>


    <%--end Vendor modal--%>

    <asp:HiddenField ID="hdnKey" runat="server" />
</asp:Content>
