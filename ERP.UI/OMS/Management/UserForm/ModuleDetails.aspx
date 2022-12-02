<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ModuleDetails.aspx.cs" Inherits="ERP.OMS.Management.UserForm.ModuleDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #formuladiv {
            background: #f3f3f3;
            border: 1px solid #ccc;
            border-radius: 4px;
            padding: 0 10px 10px 10px;
            margin-bottom: 15px;
        }

        #OrderBy_EC {
            display: none;
        }

        .p-19 {
            padding-top: 19px;
        }

        .pt-15 {
            padding-top: 15px;
        }

        .hd {
            font-size: 18px;
            font-weight: 500;
        }

        .popover-content {
            width: 600px !important;
        }

        .popover {
            max-width: 600px !important;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="JS/ModuleDetails.js?v=0.2"></script>
    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">
                <asp:Label ID="ModuleName" runat="server" Text="" />
            </h3>
        </div>

        <div id="divcross" runat="server" class="crossBtn"><a href="createModuleList.aspx"><i class="fa fa-times"></i></a></div>
    </div>

    <div class="form_main clearfix">
        <div class="clearfix">

            <dxe:ASPxCallbackPanel runat="server" ID="ComponentPanel" ClientInstanceName="cComponentPanel"
                OnCallback="ComponentPanel_Callback">
                <PanelCollection>
                    <dxe:PanelContent runat="server">


                        <div class="row">
                            <div class="col-md-2">
                                <label>Field Type</label>
                                <dxe:ASPxComboBox ID="FiledType" runat="server" ValueType="System.String" ClientInstanceName="cFiledType" Width="100%">
                                    <ClientSideEvents ValueChanged="fieldTypeChange" />
                                </dxe:ASPxComboBox>
                            </div>

                            <div class="col-md-2">
                                <label>Name</label>
                                <dxe:ASPxTextBox ID="txtName" ClientInstanceName="ctxtName"
                                    MaxLength="100" runat="server" Width="100%">
                                </dxe:ASPxTextBox>
                            </div>

                            <div class="col-md-2">
                                <label>Grid(Sequence)</label>
                                <dxe:ASPxTextBox ID="OrderBy" ClientInstanceName="cOrderBy" runat="server" Width="100%">
                                    <MaskSettings Mask="<0..99>" />
                                </dxe:ASPxTextBox>

                            </div>


                            <div class="col-md-2 p-19 ">
                                <label>Visible in List</label>
                                <dxe:ASPxCheckBox ID="vissibleinList" runat="server" ClientInstanceName="cvissibleinList"></dxe:ASPxCheckBox>

                            </div>


                            <div class="col-md-2 p-19">
                                <label>Mandatory</label>
                                <dxe:ASPxCheckBox ID="Mandetory" runat="server" ClientInstanceName="cMandetory"></dxe:ASPxCheckBox>
                            </div>
                            <div class="col-md-2 p-19 hide" id="chkFormula">
                                <label>Formula</label>
                                <dxe:ASPxCheckBox ID="chkIsFormula" runat="server" ClientInstanceName="cchkIsFormula">
                                    <ClientSideEvents ValueChanged="cchkIsFormulaChange" />
                                </dxe:ASPxCheckBox>
                            </div>
                            <div class="clear" />

                            <div class="col-md-4" id="commaSepearedtedDiv" style="display: none">
                                <label>Comma Separated Values</label>
                                <dxe:ASPxTextBox ID="txtValues" ClientInstanceName="ctxtValues"
                                    MaxLength="300" runat="server" Width="100%">
                                </dxe:ASPxTextBox>
                            </div>



                        </div>




                        <div class="clear"></div>
                        <div class="col-md-12 pt-15">
                            <div id="formuladiv" style="display: none">

                                <div class="row">
                                    <h4 class="hd col-md-12">Define Formula</h4>
                                    <div class="col-md-2">
                                        <dxe:ASPxComboBox ID="columnList" ClientInstanceName="ccolumnList" Width="100%"
                                            OnCallback="columnList_Callback" runat="server" ValueType="System.String">
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div class="col-md-3">
                                        <input type="button" class="btn btn-xs btn-success" value="Add" onclick="FormulaAdd()" />

                                        <span data-toggle="popover" data-html="true" id="formulapopover"
                                            data-content=""><i class="fa fa-question-circle"></i></span>
                                    </div>

                                    <div class="clear"></div>
                                    <div class="col-md-12">
                                        <dxe:ASPxMemo ID="memoFormula" runat="server" ClientInstanceName="cmemoFormula"
                                            Height="71px" Width="100%">
                                        </dxe:ASPxMemo>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12 ">
                            <input type="button" value="Save" class="btn brn-sm btn-primary" onclick="SaveNew()" />
                            <input type="button" value="Cancel" class="btn brn-sm btn-danger" onclick="Cancel()" />
                        </div>


                        <asp:HiddenField ID="AddEdit" runat="server" />
                    </dxe:PanelContent>
                </PanelCollection>
                <ClientSideEvents EndCallback="PanelEndCallBack" />
            </dxe:ASPxCallbackPanel>


        </div>
    </div>

    <dxe:ASPxGridView ID="Grid" runat="server" KeyFieldName="id"
        Width="100%" ClientInstanceName="cGrid"
        OnDataBinding="Grid_DataBinding"
        SettingsBehavior-AllowFocusedRow="true">
        <Columns>

            <dxe:GridViewDataTextColumn Caption="Name" FieldName="FieldName" Width="50%"
                VisibleIndex="0">
                <CellStyle CssClass="gridcellleft" Wrap="true">
                </CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>


            <dxe:GridViewDataTextColumn Caption="Grid(Sequence)" FieldName="OrderBy" Width="10%"
                VisibleIndex="0">
                <CellStyle CssClass="gridcellleft" Wrap="true">
                </CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>


            <dxe:GridViewDataTextColumn Caption="Mandatory" FieldName="Mandatory" Width="10%"
                VisibleIndex="0">
                <CellStyle CssClass="gridcellleft" Wrap="true">
                </CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>



            <dxe:GridViewDataTextColumn Caption="Field Type" FieldName="VissibleText" Width="20%"
                VisibleIndex="0">
                <CellStyle CssClass="gridcellleft" Wrap="true">
                </CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>

            <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center"
                VisibleIndex="17" Width="20%">
                <DataItemTemplate>

                    <a href="javascript:void(0);" class="pad" title="Edit" onclick="onEditClick('<%#Container.KeyValue %>')">
                        <img src="../../../assests/images/info.png" /></a>
                    </a>

                            <a href="javascript:void(0);" class="pad" title="Edit" onclick="onDeleteClick('<%#Container.KeyValue %>')">
                                <img src="../../../assests/images/Delete.png" /></a>
                    </a>
                                              
                </DataItemTemplate>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                <HeaderTemplate><span>Actions</span></HeaderTemplate>
                <EditFormSettings Visible="False"></EditFormSettings>

            </dxe:GridViewDataTextColumn>
        </Columns>
        <SettingsPager PageSize="10">
            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
        </SettingsPager>
    </dxe:ASPxGridView>
    <asp:HiddenField ID="ModuleId" runat="server" />
</asp:Content>
