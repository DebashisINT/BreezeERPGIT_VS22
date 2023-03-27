<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                27-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="Group Master" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.management_master_GroupMaster" CodeBehind="GroupMaster.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     
    <style>
       

        .labelReq {
            content: "*";
            color: red;
        }

        .hide {
            display: none;
        }

        
        


    </style>
    
    <script type="text/javascript">
        $(document).ready(function () {
           
           

            $("#GroupMasterGrid_DXPEForm_efnew_DXCBtn30").on('click', function () {
                var GroupName = document.getElementById("GroupMasterGrid_DXPEForm_efnew_GroupName").value;
                var GroupCode = document.getElementById("GroupMasterGrid_DXPEForm_efnew_GroupCode").value;
                if (GroupName == "") {
                    $('#GroupMasterGrid_DXPEForm_efnew_GroupName').addClass('hide');
                    return false;
                }
                else {
                    $('#GroupMasterGrid_DXPEForm_efnew_GroupName').removeClass('hide');
                    return true;
                }
                if (GroupCode == "") {

                    $('#GroupMasterGrid_DXPEForm_efnew_GroupCode').addClass('hide');
                    return false;
                }
                else {
                    $('#GroupMasterGrid_DXPEForm_efnew_GroupCode').removeClass('hide');
                    return true;
                }
            });
        });
       

        function InsertMessage()
        {
            jAlert("Save Successfully.");
        }

        function RequiredValidationSaveAndUpdate() {
            var GroupName = document.getElementById("GroupMasterGrid_DXPEForm_efnew_GroupName").value;
            var GroupCode = document.getElementById("GroupMasterGrid_DXPEForm_efnew_GroupCode").value;
            if (GroupName == "") {
                $('#GroupMasterGrid_DXPEForm_efnew_GroupName').addClass('hide');
                return false;
            }
            else {
                $('#GroupMasterGrid_DXPEForm_efnew_GroupName').removeClass('hide');
                return true;
            }
            if (GroupCode == "") {

                $('#GroupMasterGrid_DXPEForm_efnew_GroupCode').addClass('hide');
                return false;
            }
            else {
                $('#GroupMasterGrid_DXPEForm_efnew_GroupCode').removeClass('hide');
                return true;
            }

        }

        var name = "";
        var type = "";
        function OnGetRowValues(values) {
            name = values;
            //alert('Row value group name ' + values);
        }

        function OnGetRowValuesx(values) {
            type = values;
            var url1 = "";
            url1 = 'groupmasterPopUp.aspx?id=' + values[0] + '&type=' + values[1] + '&name=' + values[1];
            //alert(url1);
            window.location.assign(url1);
        }

        function DeleteRow(keyValue) {
            doIt = confirm('Confirm delete?');
            if (doIt) {
                var obj1 = "Delete" + "~" + keyValue;
                grid.PerformCallback(obj1);
            }
            else {

            }
        }
        function HideGroupType(obj) {
            if (obj == 'a') {
                GroupType.SetEnabled(true);
            }
            else if (obj == 'b') {
                GroupType.SetEnabled(false);
            }
            else if (obj == 'c') {
                jAlert("Member already Exists in  this group.Deletion is not allowed.");
            }
        }

        function EndCall(obj) {
            if (grid.cpDelmsg != null)
                jAlert(grid.cpDelmsg);
            if(grid.cpInsertmsg != "")
            {
                jAlert(grid.cpInsertmsg);
            }
            //if (($('#aftersavecnt').val())-($('#hdnbeforesave').val())=="1")
            //{
            //    jAlert("Save Successfully.");
            //}
        }
        //function is called on changing country
        function OnOwnerChanged(cmbCountry) {
            //alert();
            grid.GetEditor("gpm_Description").PerformCallback(cmbCountry.GetValue().toString());
        }
        function OnTypeChanged(cmbType) {          
            
            grid.GetEditor("GroupOwner").PerformCallback(cmbType.GetValue().toString());
        }
        function OnMoreInfoClick(keyValue, name, type) {
            //  frmOpenNewWindow1('groupmasterPopUp.aspx?id='+keyValue+'&type='+type+'&name='+name,400,500);

            //               var url='Lead_general.aspx?id=' + 'ADD';
            //             OnMoreInfoClick(url,"Add New Lead Details",'940px','450px',"Y");

            //alert();
            var url1 = "";
            var url = 'groupmasterPopUp.aspx?id=' + keyValue + '&type=' + type + '&name=' + name;
            //  OnMoreInfoClick(url, "Add Member", '940px', '450px', "Y");

            // window.open(url, "Add Member", "width=450px,height=720px");
            window.location.assign(url);

        }
        function frmOpenNewWindow1(location, v_height, v_weight) {
            var y = (screen.availHeight - v_height) / 2;
            var x = (screen.availWidth - v_weight) / 2;
            window.open(location, "Search_Conformation_Box", "height=" + v_height + ",width=" + v_weight + ",top=" + y + ",left=" + x + ",location=no,directories=no,menubar=no,toolbar=no,status=yes,scrollbars=no,resizable=no,dependent=no");
        }
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function CallList(obj1, obj2, obj3) {
            var obj4 = GroupType.GetValue();
            ajax_showOptions(obj1, obj2, obj3, obj4);
        }
        function CheckUnique() {
            var id = '<%= Convert.ToString(Session["id"]) %>';
            var GroupMasterShortCode = GroupCodeShortName.GetText();
            GroupMasterCode = 0;
            if ((id != null) && (id != '')) {
                GroupMasterCode = id;
                '<% Session["id"] = null; %>'
            }
            //$.ajax({
            //    type: "POST",
            //    url: "GroupMaster.aspx/CheckUniqueCode",
            //    data: JSON.stringify({ CategoriesShortCode: GroupMasterShortCode, Code: GroupMasterCode }),
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    success: function (msg) {
            //        CheckUniqueCode = msg.d;
            //        if (CheckUniqueCode != true) {
            //            alert('Please enter unique short name');

            //        }
            //    }
            //  });
        }
        // Code Added By Priti on 21122016 to check Unique Short Name
        function fn_ctxtPro_Name_TextChanged() {
      
            var ShortName = GroupCodeShortName.GetText();
           
            //var qString = window.location.href.split("=")[1];
            $.ajax({
                type: "POST",
                url: "GroupMaster.aspx/CheckUniqueName",
                data: JSON.stringify({ ShortName: ShortName}),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = msg.d;

                    if (data == true) {
                        jAlert("Please enter unique Short Name");
                        GroupCodeShortName.SetText("");
                        //document.getElementById("txtCode").focus();
                        return false;
                    }
                }
            });
        }
        FieldName = 'UpdateButton';
    </script>
    <style>
        .padTbl>tbody>tr:not(:last-child)> td {
            padding-bottom:20px;
        }

        /*Rev 1.0*/

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
        .TableMain100 #GrdHolidays , #gridudfGroup
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

        .col-md-3
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
        /*Rev end 1.0*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>Group Master</h3>
        </div>
    </div>
        <div class="form_main">
        <table class="TableMain100">
            <%--<tr>
            <td class="EHEADER" style="text-align: center;">
                <strong><span style="color: #000099">Group Master</span></strong>
            </td>
        </tr>--%>
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td style="text-align: left; vertical-align: top">
                                <table>
                                    <tr>
                                        <td id="ShowFilter">
                                            <% if (rights.CanAdd)
                                               { %>
                                            <a href="javascript:void(0);" onclick="grid.AddNewRow()" class="btn btn-success btn-radius "><span class="btn-icon"><i class="fa fa-plus" ></i></span><span>Add New</span> </a>
                                            
                                            <%} %>

                                            <%--<a href="javascript:ShowHideFilter('s');" class="btn btn-primary"><span>Show Filter</span></a>--%>
                                           <% if (rights.CanExport)
                                               { %>
                                             <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius " OnChange="if(!AvailableExportOption()){return false;}"
                                                OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Value="0">Export to</asp:ListItem>
                                                <asp:ListItem Value="1">PDF</asp:ListItem>
                                                <asp:ListItem Value="2">XLS</asp:ListItem>
                                                <asp:ListItem Value="3">RTF</asp:ListItem>
                                                <asp:ListItem Value="4">CSV</asp:ListItem>
                                            </asp:DropDownList>
                                             <%} %>
                                        </td>
                                        <td id="Td1">
                                            <%--<a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>--%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <%--<td></td>
                            <td style="float: right; vertical-align: top">
                                <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true"
                                    Font-Bold="False" ForeColor="black" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                    ValueType="System.Int32" Width="130px">
                                    <items>
                                        <dxe:ListEditItem Text="Select" Value="0" />
                                        <dxe:ListEditItem Text="PDF" Value="1" />
                                        <dxe:ListEditItem Text="XLS" Value="2" />
                                        <dxe:ListEditItem Text="RTF" Value="3" />
                                        <dxe:ListEditItem Text="CSV" Value="4" />
                                    </items>
                                    <buttonstyle>
                                    </buttonstyle>
                                    <itemstyle>
                                        <HoverStyle>
                                        </HoverStyle>
                                    </itemstyle>
                                    <border bordercolor="black" />
                                    <dropdownbutton text="Export">
                                    </dropdownbutton>
                                </dxe:ASPxComboBox>
                            </td>--%>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="relative">
                     
                    <dxe:ASPxGridView ID="GroupMasterGrid" runat="server" ClientInstanceName="grid"
                        AutoGenerateColumns="False" Width="100%" KeyFieldName="Id" OnAutoFilterCellEditorInitialize="GroupMasterGrid_AutoFilterCellEditorInitialize"
                        DataSourceID="GroupMaster"  OnHtmlEditFormCreated ="GroupMasterGrid_HtmlEditFormCreated"
                        OnHtmlRowCreated="GroupMasterGrid_HtmlRowCreated" OnCellEditorInitialize="GroupMasterGrid_CellEditorInitialize"
                        OnInitNewRow="GroupMasterGrid_InitNewRow" OnCustomCallback="GroupMasterGrid_CustomCallback"
                        OnRowValidating="GroupMasterGrid_RowValidating" OnRowUpdating="GroupMasterGrid_RowUpdating"
                        OnRowInserting="GroupMasterGrid_RowInserting" OnCustomJSProperties="GroupMasterGrid_CustomJSProperties"
                        OnStartRowEditing="GroupMasterGrid_StartRowEditing" OnRowDeleting="GroupMasterGrid_RowDeleting" OnCommandButtonInitialize="GroupMasterGrid_CommandButtonInitialize" OnCustomButtonInitialize="GroupMasterGrid_CustomButtonInitialize">
                         <SettingsSearchPanel Visible="True" Delay="5000" />
                        <ClientSideEvents EndCallback="function(s, e) {
                            EndCall(s.cpEND);
	HideGroupType(s.cpvarHide);
}"
                            CustomButtonClick="function(s, e) {
	                        if(e.buttonID == 'myButton'){
                            var url1='x';
                            var keyValue = grid.GetRowKey(e.visibleIndex);
                       
                             s.GetRowValues(e.visibleIndex, 'GroupName', OnGetRowValues);
                             s.GetRowValues(e.visibleIndex, 'Id;GroupName;MemberType', OnGetRowValuesx);
                          
                                          
                           
                             }
        }"></ClientSideEvents>
                        <Templates>
                            <editform>
                                <table style="width: 100%;">
                                    <tr>
                                        <td>

                                            <table width="100%" style="margin:10px;" class="padTbl">
                                                <tr>
                                                    <td class="gridcellright">Group Type<span style="color: red;">*</span></td>
                                                    <td>
                                                        <%-- <dxe:ASPxComboBox ID="GroupType" Text='<%#Bind("GroupType") %>' runat="server" ValueType="System.String"
                                                            SelectedIndex="0" ClientInstanceName="GroupType" Width="250px">--%>
                                                        <dxe:ASPxComboBox ID="GroupType" Value='<%#Bind("GroupType") %>' runat="server" ValueType="System.String" 
                                                            SelectedIndex="0" ClientInstanceName="GroupType" Width="250px">
                                                            <Items>

                                                                <%--  #AGMSGRP121016    <dxe:ListEditItem Text="Sub Broker" Value="Sub Broker" />
                                                                <dxe:ListEditItem Text="Broker" Value="Broker" />--%>
                                                                <dxe:ListEditItem Text="Customers" Value="Customers" />
                                                                <dxe:ListEditItem Text="Vendors" Value="Vendors" />
                                                                <dxe:ListEditItem Text="DriverTransporter" Value="DriverTransporter" />
                                                                <dxe:ListEditItem Text="Relationship Partner" Value="RelationshipPartner" />
                                                                



                                                            </Items>
                                                             <ClientSideEvents SelectedIndexChanged="function(s,e){
                                                                      var indexr = s.GetValue();                                                                      
                                                                      GroupOwnerCombo.PerformCallback(indexr);
                                                                
                                                                 }" />
                                                             
                                                        </dxe:ASPxComboBox>
                                                          
                                                       
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="gridcellright">Group Name <span style="color: red;">*</span></td>
                                                    <td>
                                                        <dxe:ASPxTextBox ID="GroupName" runat="server" MaxLength="50" Text='<%#Bind("GroupName") %>' Width="250px" ValidationSettings-ValidationGroup="<%# Container.ValidationGroup %>">
                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="Mandatory" ErrorTextPosition="right"
                                                                SetFocusOnError="True" ValidateOnLeave="False">
                                                                <RequiredField ErrorText="" IsRequired="True" />
                                                            </ValidationSettings>
                                                        </dxe:ASPxTextBox>
                                                        <span class="errortttxt hide" style="color: red;">Mandatory</span>
                                                        <%--<dxe:ASPxTextBox ID="GroupName" runat="server" Text='<%# Bind("GroupName") %>' Width="400px">

                                                            <ValidationSettings>
                                                                <RequiredField IsRequired="true"  ErrorText="Required"  />
                                                           </ValidationSettings>
                                                    </dxe:ASPxTextBox>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="gridcellright">Short Name <span style="color: red;">*</span></td>
                                                    <td>
                                                        <dxe:ASPxTextBox ID="GroupCode" runat="server" Text='<%#Bind("GroupCode") %>' Width="250px" MaxLength="50" 
                                                            ValidationSettings-ValidationGroup="<%# Container.ValidationGroup %>" 
                                                            ClientInstanceName="GroupCodeShortName">
                                                           <%-- <ClientSideEvents TextChanged="function(s, e) {  CheckUnique(); }" />--%>
                                                             <ClientSideEvents TextChanged="function(s, e) {  fn_ctxtPro_Name_TextChanged(); }" />
                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="Mandatory" ErrorTextPosition="right"
                                                                SetFocusOnError="True" ValidateOnLeave="False">
                                                                <RequiredField ErrorText="" IsRequired="True" />
                                                            </ValidationSettings>
                                                        </dxe:ASPxTextBox>
                                                        <%--<span class="errortttxt hide" style="color:red;"> Mandatory</span>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="gridcellright">Parent Group</td>
                                                    <td>
                                                        <%--<dxe:ASPxComboBox ID="PrincipalGroup"  DataSourceID="PrinciPleaGroup" ValueField="gpm_id" TextField="PrincipalGroup" Value='<%#Bind("PrincipalGroup") %>' EnableIncrementalFiltering="True" EnableSynchronization="False"  runat="server" ValueType="System.String" SelectedIndex="0" ClientInstanceName="PrincipalGroup1" Width="400px">
                                    
                                     </dxe:ASPxComboBox>--%>
                                                        <dxe:ASPxComboBox ID="PrincipalGroup" runat="server" EnableSynchronization="False"
                                                            EnableIncrementalFiltering="True" DataSourceID="PrincipleGroup" TextField="PrincipalGroup"
                                                            ValueField="gpm_id" ClientInstanceName="combo" Width="250px" ValueType="System.String"
                                                            Value='<%#Bind("PrincipalGroup") %>'>
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                </tr>
                                                <tr class="hide">
                                                    <td class="gridcellright hide">Group Owner</td>
                                                    <td>                                                        
                                                      <%--  <asp:TextBox ID="GroupOwner" MaxLength="50" runat="server" Width="250px" Text='<%#Bind("GroupOwner1") %>'></asp:TextBox>--%>
                                                        <%--<asp:TextBox ID="GroupOwner_hidden" runat="server" Visible="true"></asp:TextBox>--%>
                                                        <asp:HiddenField runat="server" ID="GroupOwner_hidden" Visible="true" />
                                                       <dxe:ASPxComboBox ID="GroupOwner" runat="server" EnableSynchronization="False"  ClientInstanceName="GroupOwnerCombo"
                                                            EnableIncrementalFiltering="True"  TextField="GroupOwner"  OnCallback="GroupOwnerCombo_CustomCallback"
                                                            ValueField="cnt_internalId"  Width="250px" ValueType="System.String" EnableCallbackMode="true" CallbackPageSize="8"
                                                            Value='<%#Bind("GroupOwner") %>'>
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                             <ClientSideEvents init="function(s,e){
                                                                      
                                                                GroupOwnerCombo.PerformCallback('');
                                                                 }" />
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                </tr>
                                                <tr class="hide">
                                                    <td class="gridcellright hide">Member Type</td>
                                                    <td>
                                                        <dxe:ASPxComboBox ID="MemberType" runat="server" Value='<%#Bind("MemberType") %>' TextField=""
                                                            ValueType="System.String" SelectedIndex="0" ClientInstanceName="MemberType" Width="250px">
                                                            <Items>
                                                                <dxe:ListEditItem Text="Contacts" Value="Contacts" />
                                                                <dxe:ListEditItem Text="Addresses" Value="Addresses" />
                                                                <dxe:ListEditItem Text="Emails" Value="Emails" />
                                                                <dxe:ListEditItem Text="Phones" Value="Phones" />
                                                                <%--  #AGMSGRP121016  <dxe:ListEditItem Text="CDSL Accounts" Value="CDSL Accounts" />
                                                                <dxe:ListEditItem Text="NSDL Accounts" Value="NSDL Accounts" />--%>
                                                            </Items>
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                </tr>
                                                <%--    For Email Editing--%>
                                                <tr class="hide">
                                                    <td class="gridcellright hide">Email</td>
                                                    <td>
                                                        <dxe:ASPxTextBox ID="TextBox1" MaxLength="50" runat="server" Text='<%#Bind("gpm_emailID") %>' Width="250px" ValidationSettings-ValidationGroup="<%# Container.ValidationGroup %>">
                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="Invalid Email." ErrorTextPosition="right"
                                                                SetFocusOnError="True" ValidateOnLeave="False">
                                                                <RegularExpression ErrorText="Invalid Email Format" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                            </ValidationSettings>

                                                        </dxe:ASPxTextBox>
                                                    </td>
                                                </tr>
                                                <tr class="hide">
                                                    <td class="gridcellright hide">CC Email</td>
                                                    <td>
                                                        <dxe:ASPxTextBox ID="TextBox2" MaxLength="50" runat="server" Text='<%#Bind("gpm_ccemailID") %>' Width="250px" ValidationSettings-ValidationGroup="<%# Container.ValidationGroup %>">
                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="Invalid Email." ErrorTextPosition="right"
                                                                SetFocusOnError="True" ValidateOnLeave="False">
                                                                <RegularExpression ErrorText="Invalid Email Format" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                            </ValidationSettings>
                                                        </dxe:ASPxTextBox>
                                                    </td>
                                                </tr>
                                                <%--    For Email Editing--%>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%-- <controls>
                                <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors">
                                </dxe:ASPxGridViewTemplateReplacement>                                                           
                            </controls> --%>
                                            <div style="padding: 2px 2px 2px 102px">
                                                <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                    runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                    runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </editform>
                        </Templates>
                        <%-- <Styles>
                            <Header CssClass="gridheader" SortingImageSpacing="5px" ImageSpacing="5px">
                            </Header>
                            <FocusedRow CssClass="gridselectrow">
                            </FocusedRow>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <FocusedGroupRow CssClass="gridselectrow">
                            </FocusedGroupRow>
                        </Styles>--%>
                        <%--<SettingsPager NumericButtonCount="20" PageSize="10" AlwaysShowPager="True" ShowSeparators="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>--%>
                        <Columns>
                            <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="0" FieldName="Id">
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="GroupName">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <PropertiesTextEdit>

                                    <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom">

                                        <RequiredField IsRequired="True" ErrorText="Group Name Can Not Be Blank"></RequiredField>

                                    </ValidationSettings>

                                </PropertiesTextEdit>
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="True" VisibleIndex="1"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="GroupType">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="GroupCode" Caption="Short Name">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="True" VisibleIndex="2"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataComboBoxColumn Visible="False" FieldName="PrincipalGroup" Caption="Principal Group"
                                VisibleIndex="4">
                                <PropertiesComboBox ValueType="System.String" TextField="PrincipalGroup" ValueField="gpm_id"
                                    EnableIncrementalFiltering="True" EnableSynchronization="False" DataSourceID="PrincipleGroup">

                                    <ClientSideEvents SelectedIndexChanged="function(s,e){OnOwnerChanged(s);}"></ClientSideEvents>

                                </PropertiesComboBox>
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="True" VisibleIndex="3"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="6" FieldName="PrincipalGroup1"
                                Caption="Principal Group">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="GroupOwner" Caption="Group Owner">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataComboBoxColumn Visible="False" FieldName="GroupOwner" Caption="Group Owner">
                                <PropertiesComboBox ValueType="System.String" TextField="GroupOwner" ValueField="cnt_internalId"
                                    EnableIncrementalFiltering="True" EnableSynchronization="False" DataSourceID="SelectOwner">

                                    <ClientSideEvents SelectedIndexChanged="function(s,e){OnOwnerChanged(s);}"></ClientSideEvents>

                                </PropertiesComboBox>
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="True" VisibleIndex="4"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewDataComboBoxColumn Visible="False" FieldName="GroupType" Caption="Group Type"
                                VisibleIndex="8">
                                <PropertiesComboBox ValueType="System.String">


                                    <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom">

                                        <RequiredField IsRequired="True" ErrorText="Select Group Type"></RequiredField>

                                    </ValidationSettings>

                                    <ClientSideEvents SelectedIndexChanged="function(s,e){OnTypeChanged(s);}"></ClientSideEvents>

                                    <Items>

                                        <dxe:ListEditItem Value="Sub Broker" Text="Sub Broker"></dxe:ListEditItem>

                                        <dxe:ListEditItem Value="Broker" Text="Broker"></dxe:ListEditItem>

                                        <dxe:ListEditItem Value="Relationship Partner" Text="Relationship Partner"></dxe:ListEditItem>

                                        <dxe:ListEditItem Value="Franchisee" Text="Franchisee"></dxe:ListEditItem>

                                        <dxe:ListEditItem Value="Relationship Manager" Text="Relationship Manager"></dxe:ListEditItem>

                                        <dxe:ListEditItem Value="Relationship Officer" Text="Relationship Officer"></dxe:ListEditItem>

                                        <dxe:ListEditItem Value="Dealers" Text="Dealers"></dxe:ListEditItem>

                                        <dxe:ListEditItem Value="Family" Text="Family"></dxe:ListEditItem>

                                        <dxe:ListEditItem Value="Business Partner" Text="Business Partner"></dxe:ListEditItem>

                                        <dxe:ListEditItem Value="Agents" Text="Agents"></dxe:ListEditItem>

                                        <dxe:ListEditItem Value="Virtual DP" Text="Virtual DP"></dxe:ListEditItem>

                                        <dxe:ListEditItem Value="DeliveryGroup" Text="DeliveryGroup"></dxe:ListEditItem>

                                    </Items>

                                </PropertiesComboBox>
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                </EditFormCaptionStyle>
                                <EditFormSettings Caption="Group Type" Visible="True" VisibleIndex="0"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewDataComboBoxColumn FieldName="MemberType" Caption="Member Type" VisibleIndex="7">
                                <PropertiesComboBox ValueType="System.String">


                                    <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom">

                                        <RequiredField IsRequired="True" ErrorText="Select Member Type"></RequiredField>

                                    </ValidationSettings>
                                    <Items>

                                        <dxe:ListEditItem Value="Contacts" Text="Contacts"></dxe:ListEditItem>

                                        <dxe:ListEditItem Value="Addresses" Text="Addresses"></dxe:ListEditItem>

                                        <dxe:ListEditItem Value="Emails" Text="Emails"></dxe:ListEditItem>

                                        <dxe:ListEditItem Value="Phones" Text="Phones"></dxe:ListEditItem>
                                        <%--     #AGMSGRP121016     <dxe:ListEditItem Value="CDSL Accounts" Text="CDSL Accounts"></dxe:ListEditItem>
                                        <dxe:ListEditItem Value="NSDL Accounts" Text="NSDL Accounts"></dxe:ListEditItem>--%>
                                    </Items>

                                </PropertiesComboBox>
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                </EditFormCaptionStyle>
                                <EditFormSettings Caption="Member Type" Visible="True"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="gpm_emailID" Caption="Email">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <PropertiesTextEdit>

                                    <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom">

                                        <RegularExpression ErrorText="Enetr Valid E-Mail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></RegularExpression>

                                    </ValidationSettings>

                                </PropertiesTextEdit>
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="True" VisibleIndex="1"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="10" FieldName="gpm_ccemailID" Caption="CC Email ID">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <PropertiesTextEdit>

                                    <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom">

                                        <RegularExpression ErrorText="Enetr Valid E-Mail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></RegularExpression>

                                    </ValidationSettings>

                                </PropertiesTextEdit>
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Top">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="True" VisibleIndex="1"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <%--<dxe:GridViewCommandColumn VisibleIndex="9" ShowEditButton="true">
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    Actions                                   
                                </HeaderTemplate>                              
                            </dxe:GridViewCommandColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="10" Caption="Details">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                <DataItemTemplate>                                   
                                    <a href="javascript:void(0);" onclick="DeleteRow('<%# Container.KeyValue %>')">
                                        <img src="/assests/images/Delete.png" alt="Delete"></a>
                                </DataItemTemplate>
                                <CellStyle Wrap="False">
                                </CellStyle>
                                <HeaderTemplate>
                                    Actions
                                </HeaderTemplate>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>--%>
                            <dxe:GridViewCommandColumn Caption="Actions" VisibleIndex="11" ShowEditButton="true" ShowDeleteButton="true" Width="9%" ButtonType="Image">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <HeaderTemplate>
                                    Actions
                                </HeaderTemplate>

                                <CustomButtons>

                                    <dxe:GridViewCommandColumnCustomButton ID="myButton">
                                        <Image AlternateText="Add member" ToolTip="Add member" Url="../../../assests/images/addmember.png" />
                                    </dxe:GridViewCommandColumnCustomButton>
                                </CustomButtons>

                                <CellStyle HorizontalAlign="Center"></CellStyle>
                            
                            </dxe:GridViewCommandColumn>

                            <%--   <dxe:GridViewCommandColumn ShowNewButtonInHeader="true" ShowEditButton="true" VisibleIndex="3">
                        <CustomButtons>
                            <dxe:GridViewCommandColumnCustomButton ID="deleteButton" Text="Delete"  />
                        </CustomButtons>
                    </dxe:GridViewCommandColumn>--%>
                        </Columns>
                        
                        <SettingsCommandButton>
                            <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                                <Image AlternateText="Edit" Url="../../../assests/images/Edit.png"></Image>

                                <Styles>
                                    <Style CssClass="pad"></Style>
                                </Styles>
                            </EditButton>
                            <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete" Styles-Style-CssClass="pad">
                                <Image AlternateText="Delete" Url="../../../assests/images/Delete.png"></Image>
                            </DeleteButton>
                            <UpdateButton Text="Save" ButtonType="Button" Styles-Style-CssClass="btn btn-primary btn-xs">
                                
                                <Styles>
                                    <Style CssClass="btn btn-primary "></Style>
                                </Styles>
                            </UpdateButton>
                            <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger btn-xs">
                                <Styles>
                                    <Style CssClass="btn btn-danger "></Style>
                                </Styles>
                            </CancelButton>
                        </SettingsCommandButton>
                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHeight="260px" PopupEditFormHorizontalAlign="Center"
                            PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="400px"
                            EditFormColumnCount="1" />
                        <SettingsText PopupEditFormCaption="Add/Modify GroupMaster" ConfirmDelete="Confirm Delete?" />
                       
                        <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowFilterRow="true" ShowFilterRowMenu="True" />


                        <StylesEditors>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </StylesEditors>
                        <%--                        <Styles>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </Styles>--%>
                        <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" AllowFocusedRow="false" />

                    </dxe:ASPxGridView>



                </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="PrincipleGroup" runat="server" 
            SelectCommand="SELECT gpm_id = CAST((CAST(gpm_id as INT)) as nvarchar(max)), gpm_Description as PrincipalGroup FROM tbl_master_groupMaster"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SelectGroupOwner" runat="server">
      
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SelectOwner" runat="server" 
            SelectCommand="Select_Owner" SelectCommandType="StoredProcedure">           
            <SelectParameters>
                <asp:Parameter Name="GroupType" Type="string" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:HiddenField ID="hdnbeforesave" runat="server" />
        <asp:HiddenField ID="aftersavecnt" runat="server" />
       <%-- code commented by Priti on 8-12-2016 to use store procedure instead on inline insert using InsertCommand--%>
           <%--<asp:SqlDataSource ID="GroupMaster" runat="server"
            SelectCommand="SELECT gpm_id AS Id, gpm_Description AS GroupName,gpm_code as GroupCode, gpm_Type AS GroupType, gpm_Owner AS GroupOwner,(select ISNULL(ltrim(rtrim(cnt_firstName)), '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')+' ['+case when cnt_internalId like 'CL%' then ltrim(rtrim(cnt_UCC)) else cnt_shortName end+']' as Name from tbl_master_contact where cnt_internalId=tbl_master_groupMaster.gpm_Owner ) AS GroupOwner1,gpm_MemberType AS MemberType, CAST(gpm_PrincipalGroup as nvarchar(max)) AS PrincipalGroup,CASE WHEN gpm_PrincipalGroup = 0 THEN 'None' ELSE (SELECT a.gpm_Description FROM tbl_master_groupMaster a WHERE a.gpm_id = tbl_master_groupMaster.gpm_PrincipalGroup) END AS PrincipalGroup1,gpm_emailID,gpm_ccemailID FROM tbl_master_groupMaster"
            ConflictDetection="CompareAllValues" InsertCommand="INSERT INTO [tbl_master_groupMaster] ([gpm_Description],[gpm_code], [gpm_Type], [gpm_Owner], [gpm_MemberType], [gpm_PrincipalGroup], [CreateDate], [CreateUser],[gpm_emailID],[gpm_ccemailID]) VALUES (@GroupName,@GroupCode, @GroupType, @GroupOwner, @MemberType, @PrincipalGroup, getdate(), @CreateUser,@gpm_emailID,@gpm_ccemailID)"
            OldValuesParameterFormatString="original_{0}" UpdateCommand="UPDATE [tbl_master_groupMaster] SET [gpm_Description] = @GroupName,[gpm_code] = @GroupCode, [gpm_Type] = @GroupType, [gpm_Owner] = @GroupOwner, [gpm_MemberType] = @MemberType, [gpm_PrincipalGroup] = @PrincipalGroup, [LastModifyDate] = getdate(), [LastModifyUser] = @CreateUser,gpm_emailID=@gpm_emailID,gpm_ccemailID=@gpm_ccemailID WHERE [gpm_id] = @Id"
            DeleteCommand="DELETE FROM [tbl_master_groupMaster] WHERE [gpm_id] = @original_Id" DeleteCommandType="Text">
            <DeleteParameters>
                <asp:Parameter Name="original_Id" Type="Decimal" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="GroupName" Type="String" />
                <asp:Parameter Name="GroupType" Type="String" />
                <asp:Parameter Name="GroupCode" Type="String" />
                <asp:Parameter Name="GroupOwner" Type="String" />
                <asp:Parameter Name="MemberType" Type="String" />
                <asp:Parameter Name="PrincipalGroup" Type="Decimal" />
                <asp:Parameter Name="CreateDate" Type="String" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
                <asp:Parameter Name="Id" Type="Decimal" />
                <asp:Parameter Name="gpm_emailID" Type="String" />
                <asp:Parameter Name="gpm_ccemailID" Type="String" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="GroupName" Type="String" />
                <asp:Parameter Name="GroupType" Type="String" />
                <asp:Parameter Name="GroupCode" Type="String" />
                <asp:Parameter Name="GroupOwner" Type="String" />
                <asp:Parameter Name="MemberType" Type="String" />
                <asp:Parameter Name="PrincipalGroup" Type="Decimal" />
                <asp:Parameter Name="CreateDate" Type="String" />
                <asp:Parameter Name="gpm_emailID" Type="String" />
                <asp:Parameter Name="gpm_ccemailID" Type="String" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
            </InsertParameters>
        </asp:SqlDataSource>--%>
         <asp:SqlDataSource ID="GroupMaster" runat="server"
            SelectCommand="SELECT gpm_id AS Id, gpm_Description AS GroupName,gpm_code as GroupCode, gpm_Type AS GroupType, gpm_Owner AS GroupOwner1,(select ISNULL(ltrim(rtrim(cnt_firstName)), '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')+' ['+case when cnt_internalId like 'CL%' then ltrim(rtrim(cnt_UCC)) else cnt_shortName end+']' as Name from tbl_master_contact where cnt_internalId=tbl_master_groupMaster.gpm_Owner ) AS GroupOwner,gpm_MemberType AS MemberType, CAST(gpm_PrincipalGroup as nvarchar(max)) AS PrincipalGroup,CASE WHEN gpm_PrincipalGroup = 0 THEN 'None' ELSE (SELECT a.gpm_Description FROM tbl_master_groupMaster a WHERE a.gpm_id = tbl_master_groupMaster.gpm_PrincipalGroup) END AS PrincipalGroup1,gpm_emailID,gpm_ccemailID FROM tbl_master_groupMaster"
            ConflictDetection="CompareAllValues" InsertCommand="prc_GroupMaster" InsertCommandType="StoredProcedure"
            OldValuesParameterFormatString="original_{0}" UpdateCommand="UPDATE [tbl_master_groupMaster] SET [gpm_Description] = @GroupName,[gpm_code] = @GroupCode, [gpm_Type] = @GroupType, [gpm_Owner] = @GroupOwner, [gpm_MemberType] = @MemberType, [gpm_PrincipalGroup] = @PrincipalGroup, [LastModifyDate] = getdate(), [LastModifyUser] = @CreateUser,gpm_emailID=@gpm_emailID,gpm_ccemailID=@gpm_ccemailID WHERE [gpm_id] = @Id"
            DeleteCommand="DELETE FROM [tbl_master_groupMaster] WHERE [gpm_id] = @original_Id" DeleteCommandType="Text">
            <DeleteParameters>
                <asp:Parameter Name="original_Id" Type="Decimal" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="GroupName" Type="String" />
                <asp:Parameter Name="GroupType" Type="String" />
                <asp:Parameter Name="GroupCode" Type="String" />
                <asp:Parameter Name="GroupOwner" Type="String" />
                <asp:Parameter Name="MemberType" Type="String" />
                <asp:Parameter Name="PrincipalGroup" Type="Decimal" />
                <asp:Parameter Name="CreateDate" Type="String" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
                <asp:Parameter Name="Id" Type="Decimal" />
                <asp:Parameter Name="gpm_emailID" Type="String" />
                <asp:Parameter Name="gpm_ccemailID" Type="String" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="GroupName" Type="String" />
                <asp:Parameter Name="GroupType" Type="String" />
                <asp:Parameter Name="GroupCode" Type="String" />
                <asp:Parameter Name="GroupOwner" Type="String" />
                <asp:Parameter Name="MemberType" Type="String" />
                <asp:Parameter Name="PrincipalGroup" Type="Decimal" />
               
                <asp:Parameter Name="gpm_emailID" Type="String" />
                <asp:Parameter Name="gpm_ccemailID" Type="String" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
            </InsertParameters>
        </asp:SqlDataSource>
        <%--.................end..............--%>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
        <br />
    </div>
    </div>
</asp:Content>

