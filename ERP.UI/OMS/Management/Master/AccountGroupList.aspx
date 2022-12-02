<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="AccountGroupList.aspx.cs" Inherits="ERP.OMS.Management.Master.AccountGroupList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function AddNewRow() {
            cAddEditPopUp.Show();
        }
        function CloseWindow() {
            cAddEditPopUp.Hide();
        }
        function PerformCallToGridBind() {
            cSelectPanel.PerformCallback();
        }
        function SelectPanel_EndCallBack() {
            if (cSelectPanel.cpAutoID == "Success") {
                jAlert("Data Saved Successfully!");
                clearPopup();
                cAddEditPopUp.Hide();
                //cAccountGroupLayout.Refresh();
            }
        }
        function clearPopup() {
            ctxtLayoutName.SetValue("");
            ctxtLayoutDescription.SetValue("");
        }
        function EditForm(key) {
            var url = 'LayoutDetailsAdd.aspx?key=' + key + '&Type=Edit';
            window.location.href = url;
        }
        function ViewForm(key) {
            var url = 'LayoutDetailsAdd.aspx?key=' + key + '&Type=View';
            window.location.href = url;
        }
        function OnClickDelete(key) {
            jConfirm('Confirm Delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    $.ajax({
                        type: "POST",
                        url: "AccountGroupList.aspx/DoDelete",
                        data: JSON.stringify({ Key: key }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            CheckUniqueCodee = msg.d;
                            if (CheckUniqueCodee)
                                jAlert('Layout is Deleted.');
                            else
                                jAlert('Please try after sometime');


                        }
                    });
                }
            });
        }
        function ActivateForm(key) {
            jConfirm('Confirm Activate?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    $.ajax({
                        type: "POST",
                        url: "AccountGroupList.aspx/DoActivate",
                        data: JSON.stringify({ Key: key }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            CheckUniqueCodee = msg.d;
                            if (CheckUniqueCodee)
                                jAlert('Layout is Activated Now.');
                            else
                                jAlert('Please try after sometime');


                        }
                    });
                }
            });
        }
    </script>


    <style>
        .padding {
            width: 100%;
        }

            .padding > tbody > tr > td {
                padding: 5px 0px;
            }

        .cnt {
            width: 70%;
            margin: 0 auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Profit and Loss Layout</h3>
        </div>
    </div>
    <div class="form_main">
        <div class="SearchArea">
            <div class="FilterSide">
                <div style="float: left;  padding-right: 5px;">
                    <% if (false)
                       { %>
                    <a href="javascript:void(0);" onclick="AddNewRow()" class="btn btn-primary"><span>Add New</span> </a>

                    <% } %>
                </div>

                <div class="pull-left">
                    <% if (false)
                       { %>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnChange="if(!AvailableExportOption()){return false;}" AutoPostBack="true">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>

                    </asp:DropDownList>
                    <% } %>
                </div>
            </div>

        </div>
        <div>
            <dxe:ASPxGridView ID="AccountGroupLayout" runat="server" ClientInstanceName="cAccountGroupLayout" AutoGenerateColumns="False" Width="100%" KeyFieldName="LAYOUT_ID" DataSourceID="LayoutDbSource">
                <ClientSideEvents EndCallback="function(s, e) {
	  EndCall(s.cpInsertError);
}" />


                <Columns>
                    <dxe:GridViewDataTextColumn Caption="Layout Id" FieldName="LAYOUT_ID" Visible="false"></dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Layout Name" FieldName="LAYOUT_NAME"></dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Layout Description" FieldName="LAYOUT_DESCRIPTION"></dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Created By" Visible="false" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy hh:mm:ss" FieldName="LAYOUT_CREATEDBY"></dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Created On" Visible="false" FieldName="LAYOUT_CREATEDON"></dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Modified By" Visible="false" FieldName="LAYOUT_MODIFIEDBY"></dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Modified On" Visible="false" FieldName="LAYOUT_MODIFIEDON"></dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="260px">
                        <DataItemTemplate>
                            <%  
                                if (rights.CanEdit)
                               { %>

                            
                          <a class="edit-button" href="javascript:void(0);" onclick="EditForm('<%# Container.KeyValue %>');"><span style="font-size: 13px;margin-top: 8px;display: inline-block;margin-bottom: 0; <%#Eval("EditDeleteDispaly")%>" class="label label-info">Define Layout</span></a>

                            <%--<a href="javascript:void(0);" onclick="EditForm('<%# Container.KeyValue %>');" class="pad" title="Edit">
                                <span>Define Layout</span></a>--%>
                            <% } %>
                             <%  
                            if (rights.CanView)
                           { %>
                            <a class="edit-button" href="javascript:void(0);" onclick="ViewForm('<%# Container.KeyValue %>');"><span style="font-size: 13px;margin-top: 8px;display: inline-block;margin-bottom: 0; <%#Eval("ViewDispaly")%>" class="label label-info">View Layout</span></a>

                            <%--<a href="javascript:void(0);" onclick="EditForm('<%# Container.KeyValue %>');" class="pad" title="Edit">
                                <span>Define Layout</span></a>--%>
                            <% } %>

                            <% if (false)
                               { %>
                            <a href="javascript:void(0);"  onclick="ActivateForm('<%# Container.KeyValue %>');" class="pad" title="Activate" >
                               <span> Layout</span> </a>
                            <% } %>

                            <% if (false)
                               { %>
                            <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="pad" title="Delete"  >
                                <img src="../../../assests/images/Delete.png" /></a><%} %>
                        </DataItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <CellStyle HorizontalAlign="Center"></CellStyle>
                        <HeaderTemplate><span>Actions</span></HeaderTemplate>
                        <EditFormSettings Visible="False"></EditFormSettings>
                    </dxe:GridViewDataTextColumn>

                </Columns>


            </dxe:ASPxGridView>
        </div>
        <asp:SqlDataSource ID="LayoutDbSource" runat="server" ConflictDetection="CompareAllValues"
            SelectCommand="SELECT LAYOUT_ID,LAYOUT_NAME,LAYOUT_DESCRIPTION,TBBU.user_name LAYOUT_CREATEDBY,LAYOUT_CREATEDON,TBBU.user_name LAYOUT_MODIFIEDBY,LAYOUT_MODIFIEDON,LAYOUT_ISACTIVE
,case when isnull(LAYOUT_ISACTIVE,0)=1 then 'visibility: hidden;' else 'visibility:visible;' end EditDeleteDispaly,case when isnull(LAYOUT_ISACTIVE,0)=0 then 'visibility: hidden;' else 'visibility:visible;' end ViewDispaly
 FROM [TBL_TRANS_LAYOUT] LEFT JOIN TBL_MASTER_USER TBU ON TBU.user_id=LAYOUT_CREATEDBY LEFT JOIN TBL_MASTER_USER TBBU ON TBBU.user_id=LAYOUT_MODIFIEDBY  where  LAYOUT_FOR='BS'"></asp:SqlDataSource>

        <div class="PopUpArea">
            <dxe:ASPxPopupControl ID="AddEditPopUp" runat="server" ClientInstanceName="cAddEditPopUp"
                Width="600px" HeaderText="Add/Edit Account Group" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">

                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <dxe:ASPxCallbackPanel runat="server" ID="SelectPanel" OnCallback="SelectPanel_Callback" ClientInstanceName="cSelectPanel" CssClass="cnt">
                            <ClientSideEvents EndCallback="function(s, e) {SelectPanel_EndCallBack();}" />
                            <PanelCollection>
                                <dxe:PanelContent runat="server">
                                    <table class="padding">

                                        <tr>
                                            <td>
                                                <div>Layout Name</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <dxe:ASPxTextBox ID="txtLayoutName" ClientInstanceName="ctxtLayoutName" runat="server" ValueType="System.String" Width="100%">

                                                        <ValidationSettings RequiredField-IsRequired="true" Display="None"></ValidationSettings>
                                                    </dxe:ASPxTextBox>
                                                </div>
                                                <span id="MandatoryLayoutName" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div>Layout Description</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <dxe:ASPxMemo ID="txtLayoutDescription" ClientInstanceName="ctxtLayoutDescription" Height="75px" TextMode="MultiLine" runat="server" Width="100%">
                                                        <ValidationSettings RequiredField-IsRequired="true" Display="None"></ValidationSettings>
                                                    </dxe:ASPxMemo>
                                                </div>
                                                <span id="MandatoryLayoutDesc" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <div>
                                                    <dxe:ASPxButton ID="btnSave" ClientInstanceName="cbtnSave" runat="server" AutoPostBack="False" Text="Save" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                        <ClientSideEvents Click="function(s, e) {return PerformCallToGridBind(); }" />

                                                    </dxe:ASPxButton>
                                                    <dxe:ASPxButton ID="btnCancel" ClientInstanceName="cbtnCancel" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                        <ClientSideEvents Click="function(s, e) {return CloseWindow(); }" />

                                                    </dxe:ASPxButton>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>


                                    <div style="padding-top: 15px;">
                                    </div>

                                </dxe:PanelContent>
                            </PanelCollection>
                        </dxe:ASPxCallbackPanel>
                    </dxe:PopupControlContentControl>
                </ContentCollection>

            </dxe:ASPxPopupControl>
        </div>

    </div>
</asp:Content>