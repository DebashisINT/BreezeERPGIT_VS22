<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                30-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

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

    <style>
        /*Rev 1.0*/

        select
        {
            height: 30px !important;
            border-radius: 4px;
           /* -webkit-appearance: none;
            position: relative;
            z-index: 1;*/
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue , .dxeTextBox_PlasticBlue
        {
            height: 30px;
            border-radius: 4px;
        }

        .dxeButtonEditButton_PlasticBlue
        {
            background: #094e8c !important;
            border-radius: 4px !important;
            padding: 0 4px !important;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
        }

        .simple-select::after {
            /*content: '<';*/
            content: url(../../../assests/images/left-arw.png);
            position: absolute;
            top: 41px;
            right: 13px;
            font-size: 16px;
            transform: rotate(269deg);
            font-weight: 500;
            background: #094e8c;
            color: #fff;
            height: 18px;
            display: block;
            width: 26px;
            /* padding: 10px 0; */
            border-radius: 4px;
            text-align: center;
            line-height: 19px;
            z-index: 1;
        }
        .simple-select {
            position: relative;
        }
        .simple-select:disabled::after
        {
            background: #1111113b;
        }
        select.btn
        {
            padding-right: 10px !important;
        }

        /*select.btn
        {
            padding-right: 10px !important;
        }*/

        /*.panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }*/

        .dxpLite_PlasticBlue .dxp-current
        {
            background-color: #1b5ea4;
            padding: 3px 5px;
            border-radius: 2px;
        }

        #accordion {
            margin-bottom: 20px;
            margin-top: 10px;
        }

        .dxgvHeader_PlasticBlue {
    background: #1b5ea4 !important;
    color: #fff !important;
}
        #ShowGrid
        {
            margin-top: 10px;
        }

        .pt-25{
                padding-top: 25px !important;
        }

        .dxgvEditFormDisplayRow_PlasticBlue td.dxgv, .dxgvDataRow_PlasticBlue td.dxgv, .dxgvDataRowAlt_PlasticBlue td.dxgv, .dxgvSelectedRow_PlasticBlue td.dxgv, .dxgvFocusedRow_PlasticBlue td.dxgv
        {
            padding: 6px 6px 6px !important;
        }

        #lookupCardBank_DDD_PW-1
        {
                left: -182px !important;
        }
        .plhead a>i
        {
                top: 9px;
        }

        .clsTo
        {
            display: flex;
    align-items: flex-start;
        }

        input[type="radio"], input[type="checkbox"]
        {
            margin-right: 5px;
        }
        .dxeCalendarDay_PlasticBlue
        {
                padding: 6px 6px;
        }

        .modal-dialog
        {
            width: 50%;
        }

        .modal-header
        {
            padding: 8px 4px 8px 10px;
            background: #094e8c !important;
        }

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #EmployeeGrid , .TableMain100 #GrdEmployee,
        .TableMain100 #SalesDetailsGrid, #ShowGrid
        {
            max-width: 98% !important;
        }

        /*div.dxtcSys > .dxtc-content > div, div.dxtcSys > .dxtc-content > div > div
        {
            width: 95% !important;
        }*/

        .btn-info
        {
                background-color: #1da8d1 !important;
                background-image: none;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxeDisabled_PlasticBlue, .aspNetDisabled
        {
            background: #f3f3f3 !important;
        }

        .dxeButtonDisabled_PlasticBlue
        {
            background: #b5b5b5 !important;
            border-color: #b5b5b5 !important;
        }

        #ddlValTech
        {
            width: 100% !important;
            margin-bottom: 0 !important;
        }

        .dis-flex
        {
            display: flex;
            align-items: baseline;
        }

        input + label
        {
            line-height: 1;
                margin-top: 7px;
        }

        .dxtlHeader_PlasticBlue
        {
            background: #094e8c !important;
        }

        .dxeBase_PlasticBlue .dxichCellSys
        {
            padding-top: 2px !important;
        }

        .pBackDiv
        {
            border-radius: 10px;
            box-shadow: 1px 1px 10px #1111112e;
        }
        .HeaderStyle th
        {
            padding: 5px;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-stripContainer
        {
            padding-top: 15px;
        }

        .pt-2
        {
            padding-top: 5px;
        }
        .pt-10
        {
            padding-top: 10px;
        }

        .pt-15
        {
            padding-top: 15px;
        }

        .pb-10
        {
            padding-bottom: 10px;
        }

        .pTop10 {
    padding-top: 20px;
}
        .custom-padd
        {
            padding-top: 4px;
    padding-bottom: 10px;
        }

        input + label
        {
                margin-right: 10px;
        }

        .btn
        {
            margin-bottom: 0;
        }

        .pl-10
        {
            padding-left: 10px;
        }

        .col-md-3>label, .col-md-3>span
        {
            margin-top: 0 !important;
        }

        .devCheck
        {
            margin-top: 5px;
        }

        .mtc-5
        {
            margin-top: 5px;
        }

        .mtc-10
        {
            margin-top: 10px;
        }

        /*select.btn
        {
           position: relative;
           z-index: 0;
        }*/

        select
        {
            margin-bottom: 0;
        }

        .form-control
        {
            background-color: transparent;
        }

        select.btn-radius {
    padding: 4px 8px 6px 11px !important;
}
        .mt-30{
            margin-top: 30px;
        }

        .panel-title h3
        {
            padding-top: 0;
            padding-bottom: 0;
        }

        .btn-radius
        {
            padding: 4px 11px !important;
            border-radius: 4px !important;
        }

        .crossBtn
        {
             right: 30px;
             top: 25px;
        }

        .mb-10
        {
            margin-bottom: 10px;
        }

        .btn-cust
        {
            background-color: #108b47 !important;
            color: #fff;
        }

        .btn-cust:hover
        {
            background-color: #097439 !important;
            color: #fff;
        }

        .gHesder
        {
            background: #1b5ca0 !important;
            color: #ffffff !important;
            padding: 6px 0 6px !important;
        }

        .close
        {
             color: #fff;
             opacity: .5;
             font-weight: 400;
        }

        .mt-27
        {
            margin-top: 27px !important;
        }

        .col-md-3 , .col-md-2
        {
            margin-top: 8px;
        }

        #upldBigLogo , #upldSmallLogo
        {
            width: 100%;
        }

        #DivSetAsDefault
        {
            margin-top: 25px;
        }

        .btn.btn-xs
        {
                font-size: 14px !important;
        }

        .dxpc-content table
        {
             width: 100%;
        }

        input[type="text"], input[type="password"], textarea
        {
            margin-bottom: 0 !important;
        }
        #FromDate , #ToDate , #ASPxFromDate , #ASPxToDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #FromDate_B-1 , #ToDate_B-1 , #ASPxFromDate_B-1 , #ASPxToDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FromDate_B-1 #FromDate_B-1Img , #ToDate_B-1 #ToDate_B-1Img , #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img
        {
            display: none;
        }

        #lblToDate
        {
            padding-left: 10px;
        }

        .dxtc-activeTab:after {
            content: '';
            width: 0;
            height: 0;
            border-left: 8px solid transparent;
            border-right: 8px solid transparent;
            border-top: 9px solid #3e5395;
            position: absolute;
            /* left: 50%; */
            z-index: 3;
            /* bottom: -15px; */
            margin-left: -9px;
        }

        .calendar-icon
        {
            right: 14px;
        }
        /*Rev end 1.0*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="JS/CreateModuleList.js?v=0.6"></script>
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>Module Creation</h3>
        </div>

    </div>
        <div class="form_main">
        <a href="javascript:void(0);" onclick="OnAddNewClick()" class="btn btn-primary mb-10"><span>Add New</span> </a>


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
                    <input type="text" id="txtModName" class="mb-10"/>
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
    </div>


</asp:Content>
