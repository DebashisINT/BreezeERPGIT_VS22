<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="CreateModuleList.aspx.cs" Inherits="ERP.OMS.Management.UserForm.CreateModuleList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .ModuleSetting {
            font-size: 19px;
            -webkit-transform: translateY(3px);
            -ms-transform: translateY(3px);
            -moz-transform: translateY(3px);
            transform: translateY(3px);
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="JS/CreateModuleList.js?v=0.6"></script>
    <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>Module Creation</h3>
        </div>

    </div>
    <div class="form_main">
        <a href="javascript:void(0);" onclick="OnAddNewClick()" class="btn btn-primary"><span>Add New</span> </a>


        <dxe:ASPxGridView ID="Grid" runat="server" KeyFieldName="id"
            Width="100%" ClientInstanceName="cGrid"
            OnDataBinding="gridAttendance_DataBinding"
            SettingsBehavior-AllowFocusedRow="true">
            <Columns>

                <dxe:GridViewDataTextColumn Caption="Name" FieldName="Name" Width="70%"
                    VisibleIndex="0">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Userwise" FieldName="Userwise" Width="10%"
                    VisibleIndex="0">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center"
                    VisibleIndex="17" Width="20%">
                    <DataItemTemplate>

                        <a href="javascript:void(0);" class="pad" title="Control Design" onclick="onEditClick('<%#Container.KeyValue %>')">
                            <img src="../../../assests/images/info.png" /></a>
                        </a>
                         
                         <a href="javascript:void(0);" class="pad" title="Module Settings" onclick="ModSettings('<%#Container.KeyValue %>')">
                             <span><i class="fa fa-cog ModuleSetting"></i></span></a>
                        </a>

                           <a href="javascript:void(0);" class="pad" title="Map user to view all entries." onclick="ShowModUser('<%#Container.KeyValue %>')">
                             <span><i class="fa fa-users ModuleSetting"></i></span></a>
                        </a>

                         <a href="javascript:void(0);" class="pad" title="Menu Creation" onclick="onMenu('<%#Container.VisibleIndex %>')">
                             <img src="../../../assests/images/go.gif" /></a>
                        </a>
                        
                        <a href="javascript:void(0);" class="pad" title="Userwise" onclick="onUserwise('<%#Container.KeyValue %>')">
                            <img src="../../../assests/images/sales.png" /></a>
                        </a>

                         <a href="javascript:void(0);" class="pad" title="Document Desgin" onclick="DocDesign('<%#Container.KeyValue %>')">
                             <img src="../../../assests/images/print.png" /></a>
                        </a>

                         <a href="javascript:void(0);" class="pad" title="Module Desgin" onclick="ModDesign('<%#Container.KeyValue %>')">
                             <img src="../../../assests/images/picture.png" /></a>
                        </a>

                                         
                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <HeaderTemplate><span>Actions</span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>

                </dxe:GridViewDataTextColumn>
            </Columns>
        </dxe:ASPxGridView>





        <dxe:ASPxPopupControl ID="ASPxPopupControl2" runat="server" ClientInstanceName="cAddNew"
            Width="500px" HeaderText="Add New" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <HeaderTemplate>
                <span>Add New</span>
                <dxe:ASPxImage ID="ASPxImage5" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                    <ClientSideEvents Click="function(s, e){  
                                cAddNew.Hide();
                            }" />
                </dxe:ASPxImage>
            </HeaderTemplate>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">

                    <label>Module Name</label>
                    <input type="text" id="txtModName" />
                    <input type="button" value="Save" class="btn brn-sm btn-primary" onclick="SaveNew()" />
                    <input type="button" value="Cancel" class="btn btn-sm btn-danger" onclick="closePopup()" />


                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>








        <dxe:ASPxPopupControl ID="ASPxPopupControl1" runat="server" ClientInstanceName="SettingsPopup"
            Width="500px" HeaderText="Settings" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">

                    <table>
                        <tr>
                            <label>From Date - To Date Filter(Entry Module Grid)</label>
                        </tr>
                        <tr>
                            <dxe:ASPxComboBox ID="FiledType" runat="server" ValueType="System.String" ClientInstanceName="cFiledType" Width="100%"
                                OnCallback="FiledType_Callback" ClearButton-DisplayMode="Always">
                            </dxe:ASPxComboBox>
                        </tr>
                        <tr>
                            <input type="button" class="btn btn-primary mTop5" value="Save" onclick="SaveDateFilter()" />
                        </tr>
                    </table>
                    <asp:HiddenField ID="hdModId" runat="server" />

                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>





        <dxe:ASPxPopupControl ID="Userpopup" runat="server" ClientInstanceName="cUserpopup"
            Width="500px" HeaderText="User List" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" 
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">


                    <dxe:ASPxGridView ID="userGrid" runat="server" KeyFieldName="user_id"
                        Width="100%" ClientInstanceName="cuserGrid"  DataSourceID="userDataControl"  
                        SettingsBehavior-AllowFocusedRow="true" OnCustomCallback="userGrid_CustomCallback">
                           <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
            <SettingsBehavior EnableCustomizationWindow="true"   />
                            <Settings ShowFooter="true"  /> 
                            <SettingsContextMenu Enabled="true" />    
                        <Columns>
                            <dxe:GridViewCommandColumn Caption="Select" ShowSelectCheckbox="true" Width="10%">
                                
                            </dxe:GridViewCommandColumn>

                            <dxe:GridViewDataTextColumn Caption="User Name" FieldName="user_name" Width="90%"
                                VisibleIndex="0">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                        </Columns>
                        <ClientSideEvents EndCallback="userGridEndCallBack" />
                    </dxe:ASPxGridView>

                    <div style="padding-top:10px" class="pull-right">
                    <input type="button" value="Save" class="btn btn-sm btn-primary" onclick="SaveUserModuleWise()" />
                    <input type="button" value="Cancel" class="btn btn-sm btn-danger" onclick="CloseUserPopup()"/>
                    </div> 
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>
        <asp:SqlDataSource ID="userDataControl" runat="server"  SelectCommand="select user_id,user_name+' ('+user_loginId +')'user_name  from tbl_master_user where user_inactive='N'"></asp:SqlDataSource>

    </div>




</asp:Content>
