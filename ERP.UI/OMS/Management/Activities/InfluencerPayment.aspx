<%--================================================== Revision History =============================================
1.0   Pallab    V2.0.39      07-08-2023          0026687: Influencer Payment module all bootstrap modal outside click event disable
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="InfluencerPayment.aspx.cs" Inherits="ERP.OMS.Management.Activities.InfluencerPayment" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="JS/InfluencerPayment.js?v1.00.000.000.18"></script>
    <link href="CSS/CustomerReceiptPayment.css?v1.00005" rel="stylesheet" />
    <script src="JS/SearchPopup.js"></script>
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <style>
        .mBot4 {
            margin-bottom: 4px;
        }

        .horizontal-images.content li img.dxEditors_edtEllipsis_PlasticBlue {
            width: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>

    <div class="panel-title clearfix" id="myDiv">
        <h3 class="pull-left">
            <label id="TxtHeaded">Add Influencer Payment</label>
        </h3>
    </div>

    <div id="pageheaderContent" class="pull-right reverse wrapHolder content horizontal-images" style="display: none;" runat="server">
        <div class="Top clearfix">
            <ul>
                <li>
                    <div class="lblHolder" id="divContactPhone" style="display: none;" runat="server">
                        <table>
                            <tr>
                                <td>Contact Person's Phone</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblContactPhone" runat="server" Text="N/A" CssClass="classout"></asp:Label></td>
                            </tr>
                        </table>
                    </div>
                </li>
                <li>
                    <div class="lblHolder" id="divGSTIN" style="display: none;" runat="server">
                        <table>
                            <tr>
                                <td>GST Registed?</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblGSTIN" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </li>
            </ul>
        </div>
    </div>



    <div id="ApprovalCross" runat="server" class="crossBtn"><a href="InfluencerPaymentList.aspx"><i class="fa fa-times"></i></a></div>




    <%--        <div class="tab">
            <input type="button" class="tablinks" value="General" onclick="ShowHideTab(event, 'General'); return false" />
            <input type="button" class="tablinks" value="Billing/Shipping" onclick="ShowHideTab(event, 'BS'); return false" />
        </div>--%>

    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ClientInstanceName="page" Width="100%">
        <TabPages>
            <dxe:TabPage Name="General" Text="General">
                <ContentCollection>
                    <dxe:ContentControl runat="server">
                        <div id="General" class="tabcontent" style="display: block;">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="">
                                        <div style="background: #f5f4f3; padding: 5px 0 5px 0; margin-bottom: 10px; border-radius: 4px; border: 1px solid #ccc;">
                                            <div class="col-md-2" id="divNumberingScheme" runat="server">
                                                <label style="margin-top: 8px">Numbering Scheme</label>
                                                <div>
                                                    <dxe:ASPxComboBox ID="CmbScheme" ClientInstanceName="cCmbScheme"
                                                        SelectedIndex="0" EnableCallbackMode="false"
                                                        TextField="SchemaName" ValueField="ID"
                                                        runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                                        <ClientSideEvents SelectedIndexChanged="CmbScheme_ValueChange"></ClientSideEvents>
                                                    </dxe:ASPxComboBox>
                                                    <span id="MandatoryNumberingScheme" class="iconNumberScheme pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                </div>
                                            </div>

                                            <div class="col-md-2">
                                                <label style="margin-top: 8px">Document No.</label>
                                                <div>

                                                    <dxe:ASPxTextBox runat="server" ID="txtVoucherNo" ClientInstanceName="ctxtVoucherNo" MaxLength="16" Text="Auto" ClientEnabled="false">
                                                    </dxe:ASPxTextBox>

                                                    <span id="MandatoryBillNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; margin-top: -21px; margin-left: 169px; display: none"
                                                        title="Mandatory"></span>

                                                </div>
                                            </div>


                                            <div class="col-md-2">
                                                <label style="margin-top: 8px">Posting Date</label>
                                                <div>
                                                    <dxe:ASPxDateEdit ID="dtTDate" runat="server" ClientInstanceName="cdtTDate" EditFormat="Custom" AllowNull="false"
                                                        Font-Size="12px" UseMaskBehavior="True" Width="100%" EditFormatString="dd-MM-yyyy" CssClass="pull-left">
                                                        <ButtonStyle Width="13px"></ButtonStyle>
                                                        <ClientSideEvents></ClientSideEvents>
                                                    </dxe:ASPxDateEdit>
                                                    <span id="MandatoryTransDate" class="iconTransDate  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                </div>
                                            </div>

                                            <div class="col-md-2 lblmTop8">
                                                <label>For Unit <span style="color: red">*</span></label>
                                                <div>
                                                    <dxe:ASPxComboBox ID="ddlBranch" runat="server" ClientInstanceName="cddlBranch"
                                                        Width="100%">
                                                        <ClientSideEvents SelectedIndexChanged="ddlBranch_Change" />
                                                    </dxe:ASPxComboBox>
                                                    <span id="MandatoryBranch" class="iconBranch pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>


                                                </div>
                                            </div>

                                            <div class="col-md-2 lblmTop8 relative">
                                                <label class="btn-block" style="margin-bottom: 0;">
                                                    <dxe:ASPxLabel ID="lbl_Influencer" runat="server" Text="Influencer">
                                                    </dxe:ASPxLabel>


                                                    <span style="color: red">*</span>

                                                </label>



                                                <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName" Width="100%">
                                                    <Buttons>
                                                        <dxe:EditButton>
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                    <ClientSideEvents ButtonClick="function(s,e){InfluencerButnClick();}" KeyDown="function(s,e){Influencer_KeyDown(s,e);}" />
                                                </dxe:ASPxButtonEdit>
                                                <span id="MandatorysInfluencer" class="iconInfluencer pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                            </div>

                                            <div class="col-md-2 lblmTop8">
                                                <label>Contact Person </label>
                                                <div>
                                                    <dxe:ASPxComboBox ID="ddlContactPerson" runat="server" ClientInstanceName="cddlContactPerson" Width="100%">
                                                    </dxe:ASPxComboBox>


                                                </div>
                                            </div>
                                            <div class="clear"></div>

                                            <div class="col-md-2 lblmTop8" id="tdCashBankLabel">
                                                <label>Cash Bank <span style="color: red">*</span></label>
                                                <div>
                                                    <dxe:ASPxComboBox ID="ddlCashBank" runat="server" ClientInstanceName="cddlCashBank" Width="100%">
                                                        <ClientSideEvents SelectedIndexChanged="CashBank_SelectedIndexChanged" />
                                                    </dxe:ASPxComboBox>

                                                    <span id="MandatoryCashBank" class="iconCashBank pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                                                </div>
                                            </div>


                                            <div class="col-md-2 lblmTop8">
                                                <label>Currency <span style="color: red">*</span></label>
                                                <div>
                                                    <dxe:ASPxComboBox ID="ddlCurrency" runat="server" ClientInstanceName="cddlCurrency" Width="100%">
                                                        <ClientSideEvents SelectedIndexChanged="Currency_Rate" />
                                                    </dxe:ASPxComboBox>


                                                </div>
                                            </div>

                                            <div class="col-md-2 lblmTop8">
                                                <label>Rate <span style="color: red">*</span></label>
                                                <div>
                                                    <dxe:ASPxTextBox ID="txtRate" runat="server" ClientInstanceName="ctxtRate" Width="100%" ClientEnabled="false" Text="0.00">
                                                    </dxe:ASPxTextBox>


                                                </div>
                                            </div>


                                            <div id="multipleredio" class="col-md-2 hide" runat="server">
                                                <div style="padding-top: 20px; margin-top: 10px">
                                                    <asp:RadioButtonList ID="rdl_MultipleType" runat="server" Width="160px" RepeatDirection="Horizontal" onchange="return selectValue();">
                                                        <asp:ListItem Text="Single" Value="S" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="Multiple" Value="M"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                            </div>

                                            <div class="clear" id="ClearSingle" style="display: none; !important"></div>
                                            <div id="singletype" runat="server">

                                                <div class="col-md-2 lblmTop8">
                                                    <label style="">Instrument Type</label>
                                                    <div style="">
                                                        <dxe:ASPxComboBox ID="cmbInstrumentType" runat="server" ClientInstanceName="cComboInstrumentTypee" Font-Size="12px"
                                                            ValueType="System.String" Width="100%" EnableIncrementalFiltering="True" Native="true">
                                                            <Items>

                                                                <dxe:ListEditItem Text="Cheque" Value="C" Selected />
                                                                <dxe:ListEditItem Text="Draft" Value="D" />
                                                                <dxe:ListEditItem Text="E.Transfer" Value="E" />
                                                                <dxe:ListEditItem Text="Cash" Value="CH" />
                                                                <dxe:ListEditItem Text="Card" Value="CRD" />
                                                            </Items>
                                                            <ClientSideEvents SelectedIndexChanged="InstrumentTypeSelectedIndexChanged" />
                                                        </dxe:ASPxComboBox>

                                                    </div>
                                                </div>

                                                <div class="col-md-2 lblmTop8" id="divInstrumentNo" style="" runat="server">
                                                    <label id="" style="">Instrument No</label>
                                                    <div id="">
                                                        <dxe:ASPxTextBox runat="server" ID="txtInstNobth" ClientInstanceName="ctxtInstNobth" Width="100%" MaxLength="30">
                                                        </dxe:ASPxTextBox>
                                                    </div>
                                                </div>
                                                <div class="clear"></div>
                                                <div class="col-md-2 lblmTop8" id="tdIDateDiv" style="" runat="server">
                                                    <label id="tdIDateLable" style="">Instrument Date</label>
                                                    <div id="tdIDateValue" style="">
                                                        <dxe:ASPxDateEdit ID="InstDate" runat="server" EditFormat="Custom" ClientInstanceName="cInstDate"
                                                            UseMaskBehavior="True" Font-Size="12px" Width="100%" EditFormatString="dd-MM-yyyy">
                                                            <ClientSideEvents></ClientSideEvents>
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                        </dxe:ASPxDateEdit>
                                                    </div>
                                                </div>
                                                <div class="col-md-2 hide" id="divDrawnOn" style="" runat="server">
                                                    <label id="" style="">Drawn on ( Party banker’s name )</label>
                                                    <div id="">
                                                        <dxe:ASPxTextBox runat="server" ID="txtDrawnOn" ClientInstanceName="ctxtDrawnOn" Width="100%" MaxLength="30">
                                                        </dxe:ASPxTextBox>
                                                    </div>
                                                </div>

                                            </div>


                                            <div class="col-md-4 lblmTop8">
                                                <label>Narration </label>
                                                <div>
                                                    <asp:TextBox ID="txtNarration" runat="server" MaxLength="500"
                                                        TextMode="MultiLine"
                                                        Width="100%"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2 lblmTop8">
                                                <label>Payable Amount<span style="color: red">*</span> </label>
                                                <div>
                                                    <dxe:ASPxTextBox ID="txtVoucherAmount" runat="server" ClientInstanceName="ctxtVoucherAmount" Width="100%" CssClass="pull-left">
                                                        <ClientSideEvents TextChanged="function(s, e) { GetInvoiceMsg(e)}" LostFocus="lostFocusVoucherAmount" />
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                                        <ValidationSettings RequiredField-IsRequired="false" Display="None"></ValidationSettings>
                                                    </dxe:ASPxTextBox>
                                                    <span id="MandatoryVoucherAmount" class="iconVoucherAmount pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                </div>
                                            </div>
                                            <div class="col-md-2 lblmTop8">
                                                <label>Voucher Amount  </label>
                                                <div>
                                                    <dxe:ASPxTextBox ID="txtActualVoucherAmount" runat="server" ClientInstanceName="ctxtActualVoucherAmount" Width="100%" CssClass="pull-left">
                                                        <ClientSideEvents TextChanged="function(s, e) { GetInvoiceMsg(e)}" LostFocus="lostFocusVoucherAmount" />
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                                        <ValidationSettings RequiredField-IsRequired="false" Display="None"></ValidationSettings>
                                                    </dxe:ASPxTextBox>

                                                </div>
                                            </div>
                                            <div class="col-md-2 lblmTop8">
                                                <label>TDS Section  </label>
                                                <div class="relative">
                                                    <dxe:ASPxComboBox runat="server" ID="ddl_tdsSection" ClientInstanceName="ctdsSection" Width="100%">
                                                        <ClientSideEvents SelectedIndexChanged="tdsSectionSelectionChange" />
                                                    </dxe:ASPxComboBox>
                                                </div>
                                            </div>
                                            <div class="clear"></div>
                                            <%--Add Nil Rate TDS Check box Tanmoy --%>
                                             <div class="col-md-2">
                                                <div style="margin-top: 29px;">
                                                    <asp:CheckBox ID="chkNILRateTDS" runat="server" Text="NIL rate TDS?" TextAlign="Right"></asp:CheckBox>
                                                </div>
                                            </div>
                                             <%--Add Nil Rate TDS Check box Tanmoy --%>

                                            <%-- <div id="Multipletype" style="display: none" class="bgrnd" runat="server">
                                                <uc1:ucPaymentDetails runat="server" ID="PaymentDetails" />
                                            </div>--%>
                                            <div class="col-md-2 lblmTop8">
                                                <dxe:ASPxLabel ID="lblProject" runat="server" Text="Project">
                                                </dxe:ASPxLabel>
                                                <%-- <label id="lblProject" runat="server">Project</label>--%>
                                                <dxe:ASPxGridLookup ID="lookup_Project" runat="server" ClientInstanceName="clookup_Project" DataSourceID="EntityServerModeDataStock"
                                                    KeyFieldName="Proj_Id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False">
                                                    <Columns>
                                                        <dxe:GridViewDataColumn FieldName="Proj_Code" Visible="true" VisibleIndex="1" Caption="Project Code" Settings-AutoFilterCondition="Contains" Width="200px">
                                                        </dxe:GridViewDataColumn>
                                                        <dxe:GridViewDataColumn FieldName="Proj_Name" Visible="true" VisibleIndex="2" Caption="Project Name" Settings-AutoFilterCondition="Contains" Width="200px">
                                                        </dxe:GridViewDataColumn>
                                                        <dxe:GridViewDataColumn FieldName="Customer" Visible="true" VisibleIndex="3" Caption="Customer" Settings-AutoFilterCondition="Contains" Width="200px">
                                                        </dxe:GridViewDataColumn>
                                                        <dxe:GridViewDataColumn FieldName="HIERARCHY_NAME" Visible="true" VisibleIndex="4" Caption="Hierarchy" Settings-AutoFilterCondition="Contains" Width="200px">
                                                        </dxe:GridViewDataColumn>
                                                    </Columns>
                                                    <GridViewProperties Settings-VerticalScrollBarMode="Auto">
                                                        <Templates>
                                                            <StatusBar>
                                                                <table class="OptionsTable" style="float: right">
                                                                    <tr>
                                                                        <td></td>
                                                                    </tr>
                                                                </table>
                                                            </StatusBar>
                                                        </Templates>
                                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>
                                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                    </GridViewProperties>
                                                    <ClientSideEvents GotFocus="Project_gotFocus" LostFocus="Project_LostFocus" ValueChanged="ProjectValueChange" />

                                                </dxe:ASPxGridLookup>
                                                <dx:linqservermodedatasource id="EntityServerModeDataStock" runat="server" onselecting="ProjectServerModeDataSource_Selecting"
                                                    contexttypename="ERPDataClassesDataContext" tablename="ProjectCodeBind" />
                                            </div>
                                            <div class="col-md-4 lblmTop8">
                                                <dxe:ASPxLabel ID="lblHierarchy" runat="server" Text="Hierarchy">
                                                </dxe:ASPxLabel>
                                                <asp:DropDownList ID="ddlHierarchy" runat="server" Width="100%" Enabled="false">
                                                </asp:DropDownList>
                                            </div>

                                            <div class="clear"></div>
                                        </div>
                                        <dxe:ASPxGridView runat="server" ClientInstanceName="grid" ID="grid"
                                            Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" Settings-ShowFooter="false"
                                            SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto"
                                            OnCellEditorInitialize="grid_CellEditorInitialize"
                                            OnBatchUpdate="grid_BatchUpdate"
                                            OnDataBinding="grid_DataBinding"
                                            OnRowInserting="Grid_RowInserting"
                                            OnRowUpdating="Grid_RowUpdating"
                                            OnRowDeleting="Grid_RowDeleting"
                                            KeyFieldName="PaymentDetail_ID"
                                            Settings-VerticalScrollableHeight="170" CommandButtonInitialize="false" Settings-ShowStatusBar="Hidden">
                                            <SettingsPager Visible="false"></SettingsPager>
                                            <Styles>
                                                <StatusBar CssClass="statusBar">
                                                </StatusBar>
                                            </Styles>
                                            <Columns>
                                                <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="3%" VisibleIndex="0" Caption=" ">
                                                    <CustomButtons>
                                                        <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                                                        </dxe:GridViewCommandColumnCustomButton>
                                                    </CustomButtons>
                                                </dxe:GridViewCommandColumn>
                                                <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl#" ReadOnly="true" VisibleIndex="1" Width="5%">
                                                    <PropertiesTextEdit>
                                                    </PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataButtonEditColumn ReadOnly="true" FieldName="Type" Caption="Type" VisibleIndex="2">

                                                    <PropertiesButtonEdit>

                                                        <ClientSideEvents ButtonClick="TypeButnClick" KeyDown="TypeKeyDown" />
                                                        <Buttons>

                                                            <dxe:EditButton Text="..." Width="20px">
                                                            </dxe:EditButton>
                                                        </Buttons>
                                                    </PropertiesButtonEdit>
                                                </dxe:GridViewDataButtonEditColumn>

                                                <dxe:GridViewDataButtonEditColumn ReadOnly="true" FieldName="DocumentNo" Caption="Document No." VisibleIndex="3">

                                                    <PropertiesButtonEdit>

                                                        <ClientSideEvents ButtonClick="DocumentNoButnClick" KeyDown="DocumentNoKeyDown" />
                                                        <Buttons>

                                                            <dxe:EditButton Text="..." Width="20px">
                                                            </dxe:EditButton>
                                                        </Buttons>
                                                    </PropertiesButtonEdit>
                                                </dxe:GridViewDataButtonEditColumn>

                                                <dxe:GridViewDataTextColumn VisibleIndex="4" Caption="Payment" ReadOnly="false" FieldName="Payment" Width="130">
                                                    <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                                        <ClientSideEvents LostFocus="Payment_LostFocus"></ClientSideEvents>
                                                        <ValidationSettings RequiredField-IsRequired="false" Display="None"></ValidationSettings>

                                                    </PropertiesTextEdit>
                                                    <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="5" Caption="Remarks" ReadOnly="false" FieldName="Remarks" Width="200">
                                                    <PropertiesTextEdit>
                                                    </PropertiesTextEdit>
                                                    <CellStyle Wrap="False" HorizontalAlign="Left" CssClass="gridcellleft"></CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="PaymentDetail_ID" Caption="Srl No" ReadOnly="true" Width="0">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="IsOpening" Caption="hidden Field Id" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                                    <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="ActualAmount" Caption="hidden Field Id" VisibleIndex="16" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                                    <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="TypeId" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide">
                                                    <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="DocId" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide">
                                                    <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="UpdateEdit" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" Width="0">
                                                    <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="2.5%" VisibleIndex="6" Caption=" ">
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
                                        <div class="clear"></div>
                                        <div class="text-center">
                                            <table style="margin-left: 30%; margin-top: 10px">
                                                <tr>
                                                    <td style="padding-right: 50px"><b>Total Amount</b></td>
                                                    <td style="width: 203px;">
                                                        <dxe:ASPxTextBox ID="txtTotalAmount" runat="server" Width="105px" ClientInstanceName="c_txt_Debit" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">

                                                            <MaskSettings Mask="&lt;0..999999999999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                        </dxe:ASPxTextBox>
                                                    </td>
                                                </tr>
                                                <tr style="display: none">
                                                    <td style="padding-right: 30px">
                                                        <asp:Label ID="lbltaxAmountHeader" runat="server" Text="Total Taxable Amount" Font-Bold="true"></asp:Label></td>
                                                    <td>
                                                        <dxe:ASPxTextBox ID="txtTaxAmount" runat="server" Width="105px" ClientInstanceName="ctxtTaxAmount" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                                            <MaskSettings Mask="&lt;0..999999999999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                        </dxe:ASPxTextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div class="content reverse horizontal-images clearfix" style="margin-top: 8px; width: 100%; margin-right: 0; padding: 8px; height: auto; border-top: 1px solid #ccc; border-bottom: 1px solid #ccc; border-radius: 0;">
                                                <ul>
                                                    <li class="clsbnrLblTaxableAmt" style="float: right;">
                                                        <div class="horizontallblHolder hide">
                                                            <table>
                                                                <tbody>
                                                                    <tr>
                                                                        <td>
                                                                            <dxe:ASPxLabel ID="ASPxLabel11" runat="server" Text="Running Balance" ClientInstanceName="cbnrLblInvVal" />
                                                                        </td>
                                                                        <td>
                                                                            <strong>
                                                                                <dxe:ASPxLabel ID="lblRunningBalanceCapsulCrp" runat="server" Text="0.00" ClientInstanceName="clblRunningBalanceCapsul" />
                                                                            </strong>
                                                                        </td>
                                                                    </tr>

                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </li>
                                                    <li>
                                                        <div class="text-left mBot4">Round Off Account</div>
                                                        <dxe:ASPxButtonEdit ID="btnMARoundOff" runat="server" ReadOnly="true" ClientInstanceName="cbtnMARoundOff" Width="100%">
                                                            <Buttons>
                                                                <dxe:EditButton>
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                            <ClientSideEvents ButtonClick="function(s,e){MainAccountButnClick();}" KeyDown="function(s,e){MainAccountNewkeydown(s,e);}" />
                                                        </dxe:ASPxButtonEdit>
                                                    </li>
                                                    <li>
                                                        <div class="text-left mBot4">Amount</div>
                                                        <dxe:ASPxTextBox ID="txtMainAccountAmount" runat="server" ClientInstanceName="ctxtMainAccountAmount"
                                                            DisplayFormatString="0.00" Width="100%" CssClass="pull-left">
                                                            <ClientSideEvents LostFocus="ShowRunningTotal" />
                                                            <MaskSettings Mask="&lt;-999999999..999999999g&gt;.&lt;00..99&gt;" />
                                                        </dxe:ASPxTextBox>
                                                    </li>
                                                    <li>
                                                        <div class="text-left mBot4">TDS Amount</div>
                                                        <dxe:ASPxTextBox ID="txtTdsAmount" runat="server" ClientInstanceName="ctxtTdsAmount" ClientEnabled="false" DisplayFormatString="0.00" Width="100%" CssClass="pull-left">
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                                        </dxe:ASPxTextBox>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                        <div class="col-md-12 mainAccount">
                                            <div class="col-md-6">
                                            </div>
                                            <div class="col-md-6">
                                            </div>

                                        </div>

                                        <div id="divSubmitButton" runat="server">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="padding: 5px 0px;">
                                                        <span id="tdSaveButtonNew" runat="server">
                                                            <dxe:ASPxButton ID="btnSaveNew" ClientInstanceName="cbtnSaveNew" runat="server"
                                                                AutoPostBack="false" CssClass="btn btn-primary" TabIndex="0" Text="S&#818;ave & New"
                                                                UseSubmitBehavior="False">
                                                                <ClientSideEvents Click="function(s, e) {SaveButtonClickNew();}" />
                                                            </dxe:ASPxButton>
                                                        </span>
                                                        <span id="tdSaveButton" runat="server">
                                                            <dxe:ASPxButton ID="btnSaveRecords" ClientInstanceName="cbtnSaveRecords" runat="server"
                                                                AutoPostBack="false" CssClass="btn btn-primary" TabIndex="0" Text="Save & Ex&#818;it"
                                                                UseSubmitBehavior="False">
                                                                <ClientSideEvents Click="function(s, e) {SaveButtonClick();}" />
                                                            </dxe:ASPxButton>
                                                        </span>
                                                        <dxe:ASPxButton ID="btnSaveUdf" Visible="false" ClientInstanceName="cbtn_SaveUdf" runat="server" AutoPostBack="False" Text="U&#818;DF" UseSubmitBehavior="False"
                                                            CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                                                            <ClientSideEvents Click="function(s, e) {OpenUdf();}" />
                                                        </dxe:ASPxButton>
                                                        <dxe:ASPxButton ID="ASPxButton3" Visible="false" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="T&#818;ax & Charges" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                            <ClientSideEvents Click="function(s, e) {Save_TaxesClick();}" />
                                                        </dxe:ASPxButton>
                                                        <b><span id="tagged" runat="server" style="display: none; color: red">This Influencer Payment is tagged in other modules. Cannot Modify data except UDF</span></b>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>




                                    </div>
                                </div>
                            </div>

                        </div>
                    </dxe:ContentControl>
                </ContentCollection>
            </dxe:TabPage>
            <%--<dxe:TabPage Name="Billing/Shipping" Text="Billing/Shipping">
                <ContentCollection>
                    <dxe:ContentControl runat="server">


                        <ucBS:Sales_BillingShipping runat="server" ID="Sales_BillingShipping" />

                    </dxe:ContentControl>
                </ContentCollection>
            </dxe:TabPage>--%>
        </TabPages>



    </dxe:ASPxPageControl>





    <%--Modal Section Start--%>

    <div class="modal fade" id="TypeModal" role="dialog" data-backdrop="static"
        data-keyboard="false">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" onclick="CloseSubModal();">&times;</button>
                    <h4 class="modal-title">Select Type</h4>
                </div>
                <div class="modal-body">
                    <div id="TypeTable">
                        <table border='1' width="100%">
                            <tr class="HeaderStyle">
                                <th>Type</th>

                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" onclick="CloseSubModal();">Close</button>
                </div>
            </div>

        </div>
    </div>




    <%--Rev 1.0--%>
    <%--<div class="modal fade" id="CustModel" role="dialog">--%>
    <div class="modal fade" id="CustModel" role="dialog" data-backdrop="static" data-keyboard="false">
      <%--Rev end 1.0--%>
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Influencer Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Influencerkeydown(event)" id="txtCustSearch" autofocus width="100%" placeholder="Search by Entity Name or Unique Id" />
                    <div id="InfluencerTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Influencer Name</th>
                                <th>Unique Id</th>
                                <th>Address</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>




    <%--Rev 1.0--%>
    <%--<div class="modal fade" id="DocModel" role="dialog">--%>
    <div class="modal fade" id="DocModel" role="dialog" data-backdrop="static" data-keyboard="false">
      <%--Rev end 1.0--%>
        <div class="modal-dialog" style="height: 400px;">

            <!-- Modal content-->
            <!-- Modal MainAccount-->
            <div class="modal-content">
                <div class="modal-header">

                    <h4 class="modal-title">Select Document</h4>
                </div>
                <div class="modal-body" style="height: 400px; overflow-y: auto;">
                    <input type="text" onkeydown="DocNewkeydown(event)" id="txtDocSearch" autofocus="autofocus" width="100%" placeholder="Search by Document Number" />

                    <div id="DocTable">
                        <table border="1" width="100%" class="table table-hover">
                            <tr class="HeaderStyle">
                                <th>Document Number</th>
                                <th>Document Date</th>
                                <th>Document Unit</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" onclick="closeDocModal();">Close</button>
                </div>
            </div>
        </div>
    </div>
    <%--Rev 1.0--%>
    <%--<div class="modal fade" id="MainAccountModel" role="dialog">--%>
    <div class="modal fade" id="MainAccountModel" role="dialog" data-backdrop="static" data-keyboard="false">
      <%--Rev end 1.0--%>
        <div class="modal-dialog">

            <!-- Modal content-->
            <!-- Modal MainAccount-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" onclick="closeModal();">&times;</button>
                    <h4 class="modal-title">Main Account Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="MainAccountNewkeydown(event)" id="txtMainAccountSearch" autofocus width="100%" placeholder="Search by Main Account Name or Short Name" />

                    <div id="MainAccountTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th>Main Account Name</th>
                                <th>Subledger Type</th>
                                <th>Reverse Applicable</th>
                                <th>HSN/SAC</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" onclick="closeModal();">Close</button>
                </div>
            </div>
        </div>
    </div>


    <asp:HiddenField runat="server" ID="hdnEnterBranch" />
    <asp:HiddenField runat="server" ID="hdnProductId" />
    <asp:HiddenField runat="server" ID="hdtHsnCode" />
    <asp:HiddenField runat="server" ID="hdnInstrumentType" />
    <asp:HiddenField runat="server" ID="hdnInfluencerId" />
    <div runat="server" id="hdnCompany" class="hide"></div>
    <div runat="server" id="hdnLocalCurrency" class="hide"></div>
    <div runat="server" id="PaymentId" class="hide"></div>
    <asp:HiddenField runat="server" ID="hdAddEdit" />
    <asp:HiddenField runat="server" ID="SysSetting" />
    <asp:HiddenField runat="server" ID="hdnRefreshType" />
    <asp:HiddenField ID="hdncWiseProductId" runat="server" />
    <asp:HiddenField ID="hdnHSNId" runat="server" />
    <div runat="server" id="jsonProducts" class="hide"></div>
    <asp:HiddenField runat="server" ID="IsUdfpresent" />
    <asp:HiddenField runat="server" ID="Keyval_internalId" />
    <asp:HiddenField runat="server" ID="DoEdit" />
    <asp:HiddenField runat="server" ID="hdnMainAccountId" />
    <asp:HiddenField runat="server" ID="hdnTDSRate" />
    <asp:HiddenField ID="hdnProjectSelectInEntryModule" runat="server" />


    <dxe:ASPxLoadingPanel ID="LoadingPanelCRP" runat="server" ClientInstanceName="cLoadingPanelCRP"
        Modal="True">
    </dxe:ASPxLoadingPanel>
        <asp:HiddenField ID="hdnProjectMandatory" runat="server" />
</asp:Content>
