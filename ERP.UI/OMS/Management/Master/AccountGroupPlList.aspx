<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                22-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="AccountGroupPlList.aspx.cs" Inherits="ERP.OMS.Management.Master.AccountGroupPlList" %>

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
            var url = 'LayoutDetailsAddPl.aspx?key=' + key +'&Type=Edit';
            window.location.href = url;
        }
        function ViewForm(key) {
            var url = 'LayoutDetailsAddPl.aspx?key=' + key + '&Type=View';
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

        /*Rev 1.0*/
        .outer-div-main {
            background: #ffffff;
            padding: 10px;
            border-radius: 10px;
            box-shadow: 1px 1px 10px #11111154;
        }

        /*.form_main {
            overflow: hidden;
        }*/

        label , .mylabel1, .clsTo, .dxeBase_PlasticBlue
        {
            color: #141414 !important;
            font-size: 14px !important;
                font-weight: 500 !important;
                margin-bottom: 0 !important;
                    line-height: 20px;
        }

        #GrpSelLbl .dxeBase_PlasticBlue
        {
                line-height: 20px !important;
        }

        /*select
        {
            height: 30px !important;
            border-radius: 4px;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }*/

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

        .calendar-icon {
            position: absolute;
            bottom: 6px;
            right: 5px;
            z-index: 0;
            cursor: pointer;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate , #txtDOB , #txtAnniversary , #txtcstVdate , #txtLocalVdate ,
        #txtCINVdate , #txtincorporateDate , #txtErpValidFrom , #txtErpValidUpto , #txtESICValidFrom , #txtESICValidUpto , #FormDate , #toDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1 , #txtDOB_B-1 , #txtAnniversary_B-1 , #txtcstVdate_B-1 ,
        #txtLocalVdate_B-1 , #txtCINVdate_B-1 , #txtincorporateDate_B-1 , #txtErpValidFrom_B-1 , #txtErpValidUpto_B-1 , #txtESICValidFrom_B-1 ,
        #txtESICValidUpto_B-1 , #FormDate_B-1 , #toDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img ,
        #txtDOB_B-1 #txtDOB_B-1Img ,
        #txtAnniversary_B-1 #txtAnniversary_B-1Img ,
        #txtcstVdate_B-1 #txtcstVdate_B-1Img ,
        #txtLocalVdate_B-1 #txtLocalVdate_B-1Img , #txtCINVdate_B-1 #txtCINVdate_B-1Img , #txtincorporateDate_B-1 #txtincorporateDate_B-1Img ,
        #txtErpValidFrom_B-1 #txtErpValidFrom_B-1Img , #txtErpValidUpto_B-1 #txtErpValidUpto_B-1Img , #txtESICValidFrom_B-1 #txtESICValidFrom_B-1Img ,
        #txtESICValidUpto_B-1 #txtESICValidUpto_B-1Img , #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img
        {
            display: none;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
        }

        .simple-select::after {
            /*content: '<';*/
            content: url(../../../assests/images/left-arw.png);
            position: absolute;
            top: 6px;
            right: -2px;
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
            z-index: 0;
        }
        .simple-select {
            position: relative;
        }
        .simple-select:disabled::after
        {
            background: #1111113b;
        }
        /*select.btn
        {
            padding-right: 10px !important;
        }*/

        .panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }

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

        .styled-checkbox {
        position: absolute;
        opacity: 0;
        z-index: 1;
    }

        .styled-checkbox + label {
            position: relative;
            /*cursor: pointer;*/
            padding: 0;
            margin-bottom: 0 !important;
        }

            .styled-checkbox + label:before {
                content: "";
                margin-right: 6px;
                display: inline-block;
                vertical-align: text-top;
                width: 16px;
                height: 16px;
                /*background: #d7d7d7;*/
                margin-top: 2px;
                border-radius: 2px;
                border: 1px solid #c5c5c5;
            }

        .styled-checkbox:hover + label:before {
            background: #094e8c;
        }


        .styled-checkbox:checked + label:before {
            background: #094e8c;
        }

        .styled-checkbox:disabled + label {
            color: #b8b8b8;
            cursor: auto;
        }

            .styled-checkbox:disabled + label:before {
                box-shadow: none;
                background: #ddd;
            }

        .styled-checkbox:checked + label:after {
            content: "";
            position: absolute;
            left: 3px;
            top: 9px;
            background: white;
            width: 2px;
            height: 2px;
            box-shadow: 2px 0 0 white, 4px 0 0 white, 4px -2px 0 white, 4px -4px 0 white, 4px -6px 0 white, 4px -8px 0 white;
            transform: rotate(45deg);
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
        .TableMain100 #GrdHolidays , #AccountGroup
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

        .for-cust-icon {
            position: relative;
            z-index: 1;
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

        .mt-24
        {
            margin-top: 24px;
        }

        .col-md-3
        {
            margin-top: 8px;
        }

        #upldBigLogo , #upldSmallLogo
        {
            width: 100%;
        }

        .chosen-container-single .chosen-single span
        {
            min-height: 30px;
            line-height: 30px;
        }

        .chosen-container-single .chosen-single div {
        background: #094e8c;
        color: #fff;
        border-radius: 4px;
        height: 26px;
        top: 1px;
        right: 1px;
        /*position:relative;*/
    }

        .chosen-container-single .chosen-single div b {
            display: none;
        }

        .chosen-container-single .chosen-single div::after {
            /*content: '<';*/
            content: url(../../../assests/images/left-arw.png);
            position: absolute;
            top: 2px;
            right: 5px;
            font-size: 18px;
            transform: rotate(269deg);
            font-weight: 500;
        }

    .chosen-container-active.chosen-with-drop .chosen-single div {
        background: #094e8c;
        color: #fff;
    }

        .chosen-container-active.chosen-with-drop .chosen-single div::after {
            transform: rotate(90deg);
            right: 5px;
        }

        #DivSetAsDefault
        {
            margin-top: 25px;
        }

        /*.dxeDisabled_PlasticBlue, .aspNetDisabled {
            opacity: 0.4 !important;
            color: #ffffff !important;
        }*/
                /*.padTopbutton {
            padding-top: 27px;
        }*/
        /*#lookup_project
        {
            max-width: 100% !important;
        }*/
        /*Rev end 1.0*/
    </style>
    <script>
        function gridRowclick(s, e) {
            $('#AccountGroupLayout').find('tr').removeClass('rowActive');
            $('.floatedBtnArea').removeClass('insideGrid');
            //$('.floatedBtnArea a .ico').css({ 'opacity': '0' });
            $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
            $(s.GetRow(e.visibleIndex)).addClass('rowActive');
            setTimeout(function () {
                //alert('delay');
                var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a .ico').css({'opacity': '1'});
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a').each(function (e) {
                //    setTimeout(function () {
                //        $(this).fadeIn();
                //    }, 100);
                //});    
                $.each(lists, function (index, value) {
                    //console.log(index);
                    //console.log(value);
                    setTimeout(function () {
                        console.log(value);
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>Profit and Loss Layout</h3>
        </div>
    </div>
        <div class="form_main">
        <div class="SearchArea">
            <div class="FilterSide mb-10">
                <div style="float: left;  padding-right: 5px;">
                    <% if (true)
                       { %>
                    <a href="javascript:void(0);" onclick="AddNewRow()" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span><span>Add New</span> </a>

                    <% } %>
                </div>

                <div class="pull-left">
                    <% if (true)
                       { %>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnChange="if(!AvailableExportOption()){return false;}" AutoPostBack="true">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>

                    </asp:DropDownList>
                    <% } %>
                </div>
                <div class="clearfix"></div>
            </div>

        </div>
        <div class="relative">
            <dxe:ASPxGridView ID="AccountGroupLayout" ClientSideEvents-RowClick="gridRowclick" runat="server" ClientInstanceName="cAccountGroupLayout" AutoGenerateColumns="False" Width="100%" KeyFieldName="LAYOUT_ID" DataSourceID="LayoutDbSource">
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

                    <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="0">
                        <DataItemTemplate>
                            <div class='floatedBtnArea'>
                           
                              
                          <a class="edit-button" href="javascript:void(0);" onclick="EditForm('<%# Container.KeyValue %>');"><span style="font-size: 13px;margin-top: 8px;display: inline-block;margin-bottom: 0; <%#Eval("EditDeleteDispaly")%>" class="label label-info">Define Layout</span></a>

                            <%--<a href="javascript:void(0);" onclick="EditForm('<%# Container.KeyValue %>');" class="pad" title="Edit">
                                <span>Define Layout</span></a>--%>
                            
                             <%  
                            if (rights.CanView)
                           { %>
                            <a class="edit-button" href="javascript:void(0);" onclick="ViewForm('<%# Container.KeyValue %>');"><span style="font-size: 13px;margin-top: 8px;display: inline-block;margin-bottom: 0; " class="label label-info">View Layout</span></a>

                            <%--<a href="javascript:void(0);" onclick="EditForm('<%# Container.KeyValue %>');" class="pad" title="Edit">
                                <span>Define Layout</span>   <%#Eval("ViewDispaly")%>"</a>--%>
                            <% } %>

                            <% if (false)
                               { %>
                            <a href="javascript:void(0);"  onclick="ActivateForm('<%# Container.KeyValue %>');" class="pad" title="Activate" >
                               <span> Layout</span> </a>
                            <% } %>

                            <% if (rights.CanDelete)
                               { %>
                            <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="" title=""  >
                                <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a><%} %>
                            </div>
                        </DataItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <CellStyle HorizontalAlign="Center"></CellStyle>
                        <HeaderTemplate><span></span></HeaderTemplate>
                        <EditFormSettings Visible="False"></EditFormSettings>
                    </dxe:GridViewDataTextColumn>

                </Columns>


            </dxe:ASPxGridView>
        </div>
        <asp:SqlDataSource ID="LayoutDbSource" runat="server" ConflictDetection="CompareAllValues" 
            SelectCommand="SELECT LAYOUT_ID,LAYOUT_NAME,LAYOUT_DESCRIPTION,TBBU.user_name LAYOUT_CREATEDBY,LAYOUT_CREATEDON,TBBU.user_name LAYOUT_MODIFIEDBY,LAYOUT_MODIFIEDON,LAYOUT_ISACTIVE
,case when isnull(LAYOUT_ISACTIVE,0)=1 then 'visibility: visible;' else 'visibility:visible;' end EditDeleteDispaly,case when isnull(LAYOUT_ISACTIVE,0)=0 then 'visibility: visible;' else 'visibility:visible;' end ViewDispaly
 FROM [TBL_TRANS_LAYOUT] LEFT JOIN TBL_MASTER_USER TBU ON TBU.user_id=LAYOUT_CREATEDBY LEFT JOIN TBL_MASTER_USER TBBU ON TBBU.user_id=LAYOUT_MODIFIEDBY  where  LAYOUT_FOR='PL'"></asp:SqlDataSource>

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
    </div>
</asp:Content>
