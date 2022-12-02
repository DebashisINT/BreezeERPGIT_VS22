<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="TaxExceptionRules.aspx.cs" Inherits="ERP.OMS.Management.Master.TaxExceptionRules" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Js/TaxExceptionrules.js"></script>
    <style>
        .backBox{
            border: 1px solid #dcdada;
            margin: 10px 0;
        }
        .backBox .backHead{
            padding:8px 15px;
            margin:0;
            font-weight:600;
            background:#e2e6ea;
        }
        .backBoxC {
            padding: 15px;
            clear:both
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            setTimeout(function () {
                console.log('setTime');
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    grid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    grid.SetWidth(cntWidth);
                }
            },500);
           
            $('.navbar-minimalize').click(function () {
                console.log('Click');
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    grid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    grid.SetWidth(cntWidth);
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Tax Exception Add/Edit</h3>
            
        </div>
        <div id="divcross" class="crossBtn"><a href="TaxEcceptionrulesList.aspx"><i class="fa fa-times"></i></a></div>
    </div>
    <div class="form_main clearfix">
        <div class="row clearfix">
                <div class="col-md-2 relative">
                    <div class="profDiv">
                        HSN Code 
                    </div>
                    <div style="position: relative">

                        <asp:TextBox ID="txtHsnSacCode" Text="990000" runat="server" MaxLength="50" Enabled="false"></asp:TextBox>

                    </div>
                </div>
            </div>
            <div class="row clearfix">
                <div class="col-md-2">
                    <label>Entity Type</label>
                    <div>
                        <select id="dllEntityType" runat="server" class="form-control">
                            <option value="G">Govt.</option>
                            <option value="NG">Non Govt.</option>
                            <option value="O">Others</option>

                        </select>
                    </div>
                    </div>
                    <div class="col-md-2">
                        <label>Based On</label>
                        <div>
                            <select  id="dllBasedOn" runat="server" class="form-control">
                                <option value="S">Select</option>
                                <option value="A">Amount</option>
                                <option value="Q">Quantity</option>
                            </select>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <label>Operator</label>
                        <div>
                            <select id="ddlOperator" runat="server" class="form-control">
                                <option value="">Select</option>
                                <option value=">">></option>
                                <option value="<"><</option>
                                <option value="=">=</option>

                            </select>
                        </div>
                   </div>
                    <div class="col-md-2">
                        <label>Criteria</label>
                        <div>
                    
                             <dxe:ASPxTextBox ID="txtVoucherAmount" runat="server" ClientInstanceName="ctxtVoucherAmount" Width="100%" CssClass="pull-left">

                                 <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />

                             </dxe:ASPxTextBox>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <label>From Date</label>
                        <div>
                    
                            <dxe:ASPxDateEdit ID="dt_Fromdate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdtFromdate" Width="100%">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxDateEdit>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <label>To Date</label>
                        <div>
                    
                            <dxe:ASPxDateEdit ID="dt_ToDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdtTodate" Width="100%">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxDateEdit>
                        </div>
                    </div>
            </div>
        <div class="row">
        
        <div class="Top clearfix">
            <div class="col-md-6">
                <div class="backBox">
                    <div class="backHead">Input GST</div>
                    <div class="backBoxC">
                        <div class="row">
                            <div class="col-md-6 relative">
                                <label>CGST</label>
                                <div>
                                    <dxe:ASPxComboBox ID="cmbCGST" runat="server" ClientInstanceName="CcmbCGST"
                                        ValueType="System.String" Width="100%">
                                    </dxe:ASPxComboBox>

                                </div>
                            </div>
                            <div class="col-md-6 relative">
                                <label>SGST</label>
                                <div>
                                    <dxe:ASPxComboBox ID="cmbSGST" runat="server" ClientInstanceName="CcmbSGST"
                                        ValueType="System.String" Width="100%">
                                    </dxe:ASPxComboBox>

                                </div>
                            </div>
                        </div>
                        <div class="row mTop5">
                        <div class="col-md-6 relative">
                            <label>UTGST</label>
                            <div>
                                <dxe:ASPxComboBox ID="cmbUTGST" runat="server" ClientInstanceName="CcmbUTGST"
                                    ValueType="System.String" Width="100%">
                                </dxe:ASPxComboBox>

                            </div>
                        </div>
                        <div class="col-md-6 relative">
                            <label>IGST</label>
                            <div>
                                <dxe:ASPxComboBox ID="cmbIGST" runat="server" ClientInstanceName="CcmbIGST"
                                    ValueType="System.String" Width="100%">
                                </dxe:ASPxComboBox>

                            </div>
                        </div>
                    </div>
                    </div>
                </div>     
            </div>
            <div class="col-md-6">
                <div class="backBox">
                    <div class="backHead">Output GST</div>
                    <div class="backBoxC">
                        <div class="row ">
                            <div class="col-md-6 relative">
                                <label>CGST</label>
                                <div>
                                    <dxe:ASPxComboBox ID="cmbsaleCGST" runat="server" ClientInstanceName="CcmbSaleCGST"
                                        ValueType="System.String" Width="100%">
                                    </dxe:ASPxComboBox>

                                </div>
                            </div>
                            <div class="col-md-6 relative">
                                <label>SGST</label>
                                <div>
                                    <dxe:ASPxComboBox ID="cmbSaleSGST" runat="server" ClientInstanceName="CcmbSaleSGST"
                                        ValueType="System.String" Width="100%">
                                    </dxe:ASPxComboBox>

                                </div>
                            </div>
                        </div>
                        <div class="row mTop5">
                        <div class="col-md-6 relative">
                            <label>UTGST</label>
                            <div>
                                <dxe:ASPxComboBox ID="cmbSaleUTGST" runat="server" ClientInstanceName="CcmbSaleUTGST"
                                    ValueType="System.String" Width="100%">
                                </dxe:ASPxComboBox>

                            </div>
                        </div>
                        <div class="col-md-6 relative">
                            <label>IGST</label>
                            <div>
                                <dxe:ASPxComboBox ID="cmbSaleIGST" runat="server" ClientInstanceName="CcmbSaleIGST"
                                    ValueType="System.String" Width="100%">
                                </dxe:ASPxComboBox>

                            </div>
                        </div>
                    </div>
                    </div>
                </div>  
            </div>
        </div>
        <div class="col-md-12">
            <button type="button" class="btn btn-success" onclick="SaveClick()">Save & New</button>
        </div>

        <div class="col-md-12 relative">

            <dxe:ASPxGridView ID="grid" runat="server" ClientInstanceName="grid" AutoGenerateColumns="False"
                Width="100%" Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Auto"
                SettingsDataSecurity-AllowDelete="false" SettingsDataSecurity-AllowEdit="false" OnDataBinding="Grid_DataBinding"
                SettingsDataSecurity-AllowInsert="false" OnCustomCallback="grid_CustomCallback" Settings-HorizontalScrollBarMode="Auto">

                <SettingsPager PageSize="10">
                    <FirstPageButton Visible="True"></FirstPageButton>

                    <LastPageButton Visible="True"></LastPageButton>

                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                </SettingsPager>
                <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />

                <ClientSideEvents EndCallback="function(s, e) {
	                                LastCall(s.cpHeight);
                                }" />

                <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" AlwaysShowPager="True">
                    <FirstPageButton Visible="True">
                    </FirstPageButton>
                    <LastPageButton Visible="True">
                    </LastPageButton>

                    <PageSizeItemSettings Items="10,50, 100, 150, 200" Visible="True"></PageSizeItemSettings>
                </SettingsPager>

                <SettingsEditing Mode="PopupEditForm" PopupEditFormHeight="200px" PopupEditFormHorizontalAlign="Center"
                    PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="600px"
                    EditFormColumnCount="1" />
                <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />

                <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn" />

                <SettingsCommandButton>

                    <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                        <Image AlternateText="Edit" Url="../../../assests/images/Edit.png"></Image>
                    </EditButton>
                    <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete" Styles-Style-CssClass="pad">
                        <Image AlternateText="Delete" Url="../../../assests/images/Delete.png"></Image>
                    </DeleteButton>
                    <UpdateButton Text="Update" ButtonType="Button" Styles-Style-CssClass="btn btn-primary" Image-Width>
                        <Styles>
                            <Style CssClass="btn btn-primary"></Style>
                        </Styles>
                    </UpdateButton>
                    <CancelButton Text="Cancel" ButtonType="Button"></CancelButton>
                </SettingsCommandButton>
                <SettingsSearchPanel Visible="True" Delay="7000" />
                <SettingsText PopupEditFormCaption="Add/Modify Category" ConfirmDelete="Confirm delete?" />
                <StylesEditors>
                    <ProgressBar Height="25px">
                    </ProgressBar>
                </StylesEditors>

                <Columns>
                    <dxe:GridViewDataTextColumn FieldName="SlrNo" ReadOnly="True" Visible="False" VisibleIndex="0">
                        <EditFormSettings Visible="False" />
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>


                    <dxe:GridViewDataTextColumn FieldName="Entitytype" Caption="Entity Type" Width="200"
                        VisibleIndex="1" ShowInCustomizationForm="True">
                        <EditCellStyle Wrap="True">
                        </EditCellStyle>
                        <CellStyle CssClass="gridcellleft" Wrap="True">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn FieldName="BasedOn" Caption="Based On" Width="200"
                        VisibleIndex="2" ShowInCustomizationForm="True">
                        <EditCellStyle Wrap="True">
                        </EditCellStyle>
                        <CellStyle CssClass="gridcellleft" Wrap="True">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn FieldName="Operator" Caption="Operator" Width="200"
                        VisibleIndex="3" ShowInCustomizationForm="True">
                        <EditCellStyle Wrap="True">
                        </EditCellStyle>
                        <CellStyle CssClass="gridcellleft" Wrap="True">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn FieldName="Criteria" Caption="Criteria" Width="200"
                        VisibleIndex="4" ShowInCustomizationForm="True">
                        <EditCellStyle Wrap="True">
                        </EditCellStyle>
                        <CellStyle CssClass="gridcellleft" Wrap="True">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>



                    <dxe:GridViewDataTextColumn FieldName="fromdate" Caption="From Date" Width="200"
                        VisibleIndex="5" ShowInCustomizationForm="True">
                        <EditCellStyle Wrap="True">
                        </EditCellStyle>
                        <CellStyle CssClass="gridcellleft" Wrap="True">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>

                   <dxe:GridViewDataTextColumn FieldName="todate" Caption="To Date" Width="200"
                        VisibleIndex="6" ShowInCustomizationForm="True">
                        <EditCellStyle Wrap="True">
                        </EditCellStyle>
                        <CellStyle CssClass="gridcellleft" Wrap="True">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="" VisibleIndex="7" Width="100" >
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                        <HeaderStyle HorizontalAlign="Center" />
                        <HeaderTemplate>
                        </HeaderTemplate>
                        <DataItemTemplate>
                            <div class='floatedBtnArea'>

                                <a href="javascript:void(0);" onclick="OnEdit('<%#Eval("SlrNo") %>')">
                                    <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>
                                <a href="javascript:void(0);" onclick="DeleteRow('<%#Eval("SlrNo") %>')">
                                    <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                            </div>
                        </DataItemTemplate>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                </Columns>
                <ClientSideEvents RowClick="gridRowclick" />
                <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                <Styles>
                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                    </Header>
                    <LoadingPanel ImageSpacing="10px">
                    </LoadingPanel>
                </Styles>
            </dxe:ASPxGridView>

        </div>
    </div>
    </div>

    

    

    
    <asp:HiddenField ID="hdnAction"  runat="server" />
    <asp:HiddenField ID="hdnHSNSACType" runat="server" />
    <asp:HiddenField ID="hdnID" runat="server" />


</asp:Content>
