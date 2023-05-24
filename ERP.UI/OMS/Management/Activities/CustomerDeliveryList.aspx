<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                12-04-2023        2.0.37           Pallab              25988: Customer Delivery (Route) module design modification & check in small device
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"  CodeBehind="CustomerDeliveryList.aspx.cs" 
    Inherits="ERP.OMS.Management.Activities.CustomerDeliveryList"   %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server" >

    <%--<script type="text/javascript" src="../../../CentralData/JSScript/GenericJScript.js"></script>--%>
    
    
    <script lang="javascript" type="text/javascript">
        var DChallanid = 0;
        function onPrintJv(id) {
            DChallanid = id;
            cSelectPanel.cpSuccess = "";
            cDocumentsPopup.Show();
            //cSelectPanel.PerformCallback('BindDocumentsDetails');
            //CselectDuplicate.SetEnabled(false);
            //CselectTriplicate.SetEnabled(false);
            CselectOriginal.SetCheckState('UnChecked');
            CselectDuplicate.SetCheckState('UnChecked');
            CselectTriplicate.SetCheckState('UnChecked');
            cCmbDesignName.SetSelectedIndex(0);
            //cComponentPanel.PerformCallback();
            cSelectPanel.PerformCallback('Bindalldesignes');
            $('#btnOK').focus();
        }

        //function OrginalCheckChange(s, e) {
        //    debugger;
        //    if (s.GetCheckState() == 'Checked') {
        //        CselectDuplicate.SetEnabled(true);
        //    }
        //    else {
        //        CselectDuplicate.SetCheckState('UnChecked');
        //        CselectDuplicate.SetEnabled(false);
        //        CselectTriplicate.SetCheckState('UnChecked');
        //        CselectTriplicate.SetEnabled(false);
        //    }
        //}
        //function DuplicateCheckChange(s, e) {
        //    if (s.GetCheckState() == 'Checked') {
        //        CselectTriplicate.SetEnabled(true);
        //    }
        //    else {
        //        CselectTriplicate.SetCheckState('UnChecked');
        //        CselectTriplicate.SetEnabled(false);
        //    }
        //}

        function PerformCallToGridBind() {
            debugger;
            cSelectPanel.PerformCallback('Bindsingledesign');
            cDocumentsPopup.Hide();
            return false;
        }
        var isFirstTime = true;
        function AllControlInitilize() {
            //debugger;
            if (isFirstTime) {

                if (localStorage.getItem('FromDateBTO')) {
                    var fromdatearray = localStorage.getItem('FromDateBTO').split('-');
                    var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                    cFormDate.SetDate(fromdate);
                }

                if (localStorage.getItem('ToDateBTO')) {
                    var todatearray = localStorage.getItem('ToDateBTO').split('-');
                    var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                    ctoDate.SetDate(todate);
                }
                if (localStorage.getItem('BranchBTO')) {
                    if (ddlClientNameBranch.FindItemByValue(localStorage.getItem('BranchBTO'))) {
                        ddlClientNameBranch.SetValue(localStorage.getItem('BranchBTO'));
                    }

                }
                //updateGridByDate();
                isFirstTime = false;
            }
        }

        function updateGridByDate() {
            //debugger;
            if (cFormDate.GetDate() == null) {
                jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
            }
            else if (ctoDate.GetDate() == null) {
                jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
            }
            else if (ddlClientNameBranch.GetValue() == null) {
                jAlert('Please select Branch.', 'Alert', function () { ddlClientNameBranch.Focus(); });
            }
            else {
                localStorage.setItem("FromDateBTO", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("ToDateBTO", ctoDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("BranchBTO", ddlClientNameBranch.GetValue());
                $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                $("#hfBranchID").val(ddlClientNameBranch.GetValue());
                $("#hfIsFilter").val("Y");
                gridCustDelivList.Refresh();
                //gridCustDelivList.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ddlClientNameBranch.GetValue());

            }
        }

        function cSelectPanelEndCall(s, e) {
            debugger;
            if (cSelectPanel.cpSuccess != null) {
                var TotDocument = cSelectPanel.cpSuccess.split(',');
                var reportName = cCmbDesignName.GetValue();
                var module = 'RChallan';
                if (TotDocument.length > 0) {
                    for (var i = 0; i < TotDocument.length; i++) {
                        if (TotDocument[i] != "") {
                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + DChallanid + '&PrintOption=' + TotDocument[i], '_blank')
                        }
                    }
                }
            }
            //cSelectPanel.cpSuccess = null
            if (cSelectPanel.cpSuccess == "") {
                if (cSelectPanel.cpChecked != "") {
                    jAlert('Please check Original For Recipient and proceed.');
                }
                //CselectDuplicate.SetEnabled(false);
                //CselectTriplicate.SetEnabled(false);
                CselectOriginal.SetCheckState('UnChecked');
                CselectDuplicate.SetCheckState('UnChecked');
                CselectTriplicate.SetCheckState('UnChecked');
                cCmbDesignName.SetSelectedIndex(0);
            }
        }

        $(function () {
            var vAnotherKeyWasPressed = false;
            var ALT_CODE = 18;

            //When some key is pressed
            $(window).keydown(function (event) {
                var vKey = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
                vAnotherKeyWasPressed = vKey != ALT_CODE;
                if (event.altKey && (event.key == 's' || event.key == 'S')) {
                    //console.log('save not');
                    if (cPopup_Empcitys.IsVisible()) {
                        //console.log('save');
                        cbtnSave_citys.DoClick();
                    }
                    return false;
                }

                if (event.altKey && (event.key == 'a' || event.key == 'A')) {
                    if (!cPopup_Empcitys.IsVisible()) {
                        if (document.getElementById('AddBtn') != null) {
                            //console.log('new');
                            fn_PopOpen();
                            return false;
                        }

                    }

                }

                if (event.altKey && (event.key == 'c' || event.key == 'C')) {
                    //console.log('save not');
                    if (cPopup_Empcitys.IsVisible()) {
                        fn_btnCancel();
                    }
                    return false;
                }

            });

            //When some key is left
            $(window).keyup(function (event) {

                var vKey = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;

            });
        });


        function fn_AllowonlyNumeric(s, e) {
            var theEvent = e.htmlEvent || window.event;
            var key = theEvent.keyCode || theEvent.which;
            var keychar = String.fromCharCode(key);
            if (key == 9 || key == 37 || key == 38 || key == 39 || key == 40 || key == 8 || key == 46) { //tab/ Left / Up / Right / Down Arrow, Backspace, Delete keys
                return;
            }
            var regex = /[0-9\b]/;

            if (!regex.test(keychar)) {
                theEvent.returnValue = false;
                if (theEvent.preventDefault)
                    theEvent.preventDefault();
            }
        }
     
        function componentEndCallBack(s, e) {
            //console.log(e);
            // cPopup_Empcitys.Show();
        }

        var mainAccountInUse = [];
        function gridCustDelivList_EndCallBack() {
           
            if (gridCustDelivList.cpMainAccountInUse != null) {
                if (grid.cpMainAccountInUse != '') {
                    for (var mainCount = 0; mainCount < gridCustDelivList.cpMainAccountInUse.split('~').length; mainCount++) {
                        mainAccountInUse.push(gridCustDelivList.cpMainAccountInUse.split('~')[mainCount]);
                    }
                }
            }
            
            if (gridCustDelivList.cpDelete != null) {
                if (gridCustDelivList.cpDelete == 'Success')
                    jAlert('Data Deleted Successfully');
                else
                    jAlert("Error on deletion\n'Please Try again!!'");
            }
            

             
            if (gridCustDelivList.cpExists != null) {
                if (gridCustDelivList.cpExists == "Exists") {
                    jAlert('Record already Exists');
                    //cPopup_Empcitys.Hide();
                }
                else {
                    jAlert("Error on operation \n 'Please Try again!!'")
                    //cPopup_Empcitys.Hide();
                }
            }
        }

        $(document).ready(function () {
            $('.dxpc-closeBtn').click(function () {
                fn_btnCancel();
            });
        });
 
        <%-------  ### Added By : Samrat Roy - 22/05/17 #####--%>
        function OnAddButtonClick() {
            var url = 'CustomerDelivery.aspx?key=' + 'ADD';
            window.location.href = url;
        }

        function OnMoreInfoClick(keyValue) {
            debugger;
            var ActiveUser = '<%=Session["userid"]%>'
            if (ActiveUser != null) {
                $.ajax({
                    type: "POST",
                    url: "CustomerDeliveryList.aspx/GetEditablePermission",
                    data: "{'ActiveUser':'" + ActiveUser + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,//Added By:Subhabrata
                    success: function (msg) {
                        debugger;
                        var status = msg.d;
                        var url = 'CustomerDelivery.aspx?key=' + keyValue + '&Permission=' + status;
                        window.location.href = url;
                    }
                }); 
            }      
        }

        function OnViewClick(keyValue) {
            var url = 'CustomerDelivery.aspx?key=' + keyValue + '&req=V';
            window.location.href = url;
        }
        <%-------  ### END :: Added By : Samrat Roy - 22/05/17 :: END #####--%>

        function OnClickDelete(keyValue) {
            //alert("keyValue=" + keyValue + "  custID=" + custID + "  prodID=");
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    gridCustDelivList.PerformCallback('Delete~' + keyValue);
                }
            });
        }

        function LoadFilteredGrid(s, e) {
            gridCustDelivList.PerformCallback("LoadGridOnBranchFilter~" + ddlClientNameBranch.GetValue());

        }
        function gridRowclick(s, e) {
            $('#gridCustDelivList').find('tr').removeClass('rowActive');
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
                    ////console.log(index);
                    //console.log(value);
                    setTimeout(function () {
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }

    </script>
    <link href="CSS/CustomerDeliveryList.css" rel="stylesheet" />

    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    
    <style>
        #GrdOrder {
            max-width: 98% !important;
        }
        #FormDate, #toDate, #dtTDate, #dt_PLQuote, #dt_PlQuoteExpiry , #ASPxDateEditFrom , #ASPxDateEditTo {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        select
        {
            -webkit-appearance: auto;
        }

        /*.calendar-icon
        {
                right: 10px;
        }*/


        #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #dt_PLQuote_B-1 , #dt_partyInvDt_B-1, #ASPxDateEditFrom_B-1, #ASPxDateEditTo_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #dt_PLQuote_B-1 #dt_PLQuote_B-1Img ,
        #dt_partyInvDt_B-1 #dt_partyInvDt_B-1Img, #ASPxDateEditFrom_B-1 #ASPxDateEditFrom_B-1Img, #ASPxDateEditTo_B-1 #ASPxDateEditTo_B-1Img
        {
            display: none;
        }
    </style>
    <%--Rev end 1.0--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>Customer Delivery (Route)</h3>
        </div>
        <table class="rgtpad pull-right" style="margin-top:7px;">
                    <tr>
                        <td>
                            
                        </td>
                        <td>Unit</td>
                        <td>
                            <dxe:ASPxComboBox ID="ddlBranch" ClientInstanceName="ddlClientNameBranch" runat="server" ValueType="System.String" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                 <ClearButton DisplayMode="Always"></ClearButton>
                            </dxe:ASPxComboBox>
                        </td>
                        <td>From Date :</td>
                        <%--Rev 1.0: "for-cust-icon" class add --%>
                        <td width="150px" class="for-cust-icon">
                                <dxe:ASPxDateEdit ID="ASPxDateEditFrom" runat="server" ClientInstanceName="cFormDate" EditFormat="Custom" EditFormatString="dd-MM-yyyy"  UseMaskBehavior="True" TabIndex="23">
                                    <ButtonStyle Width="13px"></ButtonStyle>
                                </dxe:ASPxDateEdit>
                            <%--Rev 1.0--%>
                            <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                            <%--Rev end 1.0--%>
                        </td>
                        <td>To Date :</td>
                        <%--Rev 1.0: "for-cust-icon" class add --%>
                        <td width="150px" class="for-cust-icon">
                            <dxe:ASPxDateEdit ID="ASPxDateEditTo" runat="server" EditFormat="Custom" ClientInstanceName="ctoDate" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" TabIndex="23" >
                                <ButtonStyle Width="13px"></ButtonStyle>
                            </dxe:ASPxDateEdit>
                            <%--Rev 1.0--%>
                            <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                            <%--Rev end 1.0--%>
                        </td>
                        <td>
                           <%-- <dxe:ASPxButton ID="btnShow" ClientInstanceName="cinBtnShow" runat="server" AutoPostBack="False" Text="Show" CssClass="btn btn-primary"  >
                                    <ClientSideEvents Click="function(s, e) {LoadFilteredGrid(); e.processOnServer=false;}" />
                            </dxe:ASPxButton>--%>
                            <input type="button" value="Show" class="btn btn-success" onclick="updateGridByDate()" />
                        </td>
                    </tr>
                </table>
    </div>

        <div class="form_main">
        <div class="Main">
            <div class="clearfix">              
               <% if (rights.CanAdd)
                    { %>
                <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-info"><span><u>A</u>dd New</span> </a>
                <% } %>
                <% if (rights.CanExport)
                                { %>
                            <asp:DropDownList ID="ddlExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="ddlExport_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">PDF</asp:ListItem>
                            <asp:ListItem Value="2">XLS</asp:ListItem>
                            <asp:ListItem Value="3">RTF</asp:ListItem>
                            <asp:ListItem Value="4">CSV</asp:ListItem>
                        </asp:DropDownList>
                <% } %> 
              
            </div>

            <!-- NEED TO RECHEACK : START -->
            <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
                Width="600px" HeaderText="Add/Modify UDF" Modal="true" AllowResize="true" ResizingMode="Postponed">
                <headertemplate>
                <span>UDF</span>
                <dxe:ASPxImage ID="ASPxImage3" runat="server" ImageUrl="/assests/images/closePop.png"  Cursor="pointer" cssClass="popUpHeader" >
                <ClientSideEvents Click="function(s, e){ popup.Hide(); }" />
                </dxe:ASPxImage>
                </headertemplate>
                <contentcollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
                </contentcollection>
            </dxe:ASPxPopupControl>
            <asp:HiddenField runat="server" ID="IsUdfpresent" />
            <asp:HiddenField runat="server" ID="Keyval_internalId" />
            <!-- NEED TO RECHEACK : END -->

            <div class="GridViewArea">

                <dxe:ASPxGridView ID="gridCustDelivList" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridCustDelivList"
                    KeyFieldName="SlNo" Width="100%" DataSourceID="EntityServerModeDataSource"
                    OnHtmlRowCreated="gridCustDelivList_HtmlRowCreated"
                    OnHtmlEditFormCreated="gridCustDelivList_HtmlEditFormCreated"
                    OnCustomCallback="gridCustDelivList_CustomCallback" 
                    CssClass="pull-left" SettingsBehavior-AllowFocusedRow="true" OnSummaryDisplayText="ShowGrid_SummaryDisplayText"  SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" >
                    <settingssearchpanel visible="True" delay="5000" />
                    <settings showgrouppanel="True" showstatusbar="Hidden" showfilterrow="true" showfilterrowmenu="True" />
                    <columns>

                        <dxe:GridViewDataTextColumn Caption="Delievry_Id" FieldName="Delievry_Id" ReadOnly="True"
                        Visible="False" FixedStyle="Left" VisibleIndex="0">
                        <EditFormSettings Visible="True" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Document No." FieldName="Delievry_Number" ReadOnly="True"  Width="140"
                        Visible="True" FixedStyle="Left" VisibleIndex="1" Settings-ShowFilterRowMenu="True">
                        <EditFormSettings Visible="True" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="Delievry_CheckDate" ReadOnly="True"  Width="75"
                        Visible="True" FixedStyle="Left" VisibleIndex="2">
                        <EditFormSettings Visible="True" />
                        <Settings AutoFilterCondition="Contains" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>

                        <%--<dxe:GridViewDataTextColumn Caption="Customers" FieldName="CustomerNames" ReadOnly="True"
                        Visible="True" FixedStyle="Left" VisibleIndex="3">
                        <EditFormSettings Visible="True" />
                        <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>--%>

                         <dxe:GridViewDataTextColumn Caption="Unit Name" FieldName="BranchName" ReadOnly="True"
                            Visible="True" FixedStyle="Left" VisibleIndex="3" Settings-ShowFilterRowMenu="True" >
                        <EditFormSettings Visible="True" />
                             <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                         <dxe:GridViewDataTextColumn Caption="Area Name" FieldName="AreaName" ReadOnly="True"
                            Visible="True" FixedStyle="Left" VisibleIndex="4">
                        <EditFormSettings Visible="True" />
                             <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                         <dxe:GridViewDataTextColumn Caption="Vehicle No." FieldName="VehicleNumber" ReadOnly="True"
                            Visible="True" FixedStyle="Left" VisibleIndex="5">
                        <EditFormSettings Visible="True" />
                             <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                        
                         <dxe:GridViewDataTextColumn Caption="Driver Name" FieldName="TransporterName" ReadOnly="True"
                            Visible="True" FixedStyle="Left" VisibleIndex="6">
                        <EditFormSettings Visible="false" />
                             <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                         <dxe:GridViewDataTextColumn Caption="Phone" FieldName="DriverPhNo" ReadOnly="True"
                            Visible="True" FixedStyle="Left" VisibleIndex="7">
                        <EditFormSettings Visible="false" />
                             <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                         <dxe:GridViewDataTextColumn Caption="No. of Items" FieldName="NumProducts" ReadOnly="True"
                            Visible="True" FixedStyle="Left" VisibleIndex="8" Width="110px">
                        <EditFormSettings Visible="True" />
                             <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                         <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="EnteredBy" ReadOnly="True"
                        Visible="True" FixedStyle="Left" VisibleIndex="9">
                        <EditFormSettings Visible="True" />
                             <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                         <dxe:GridViewDataTextColumn Caption="Updated By" FieldName="UpdatedBy" ReadOnly="True"
                        Visible="True" FixedStyle="Left" VisibleIndex="10">
                        <EditFormSettings Visible="True" />
                             <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                         <dxe:GridViewDataTextColumn Caption="Last updated On" FieldName="LastUpdatedOn" ReadOnly="True"
                        Visible="True" FixedStyle="Left" VisibleIndex="11">
                        <EditFormSettings Visible="True" />
                             <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>


                        <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="12" Width="100">
                            <DataItemTemplate>
                                <div class='floatedBtnArea'>
                                 <% if (rights.CanView)
                                    { %>
                                <a href="javascript:void(0);" onclick="OnViewClick('<%#Eval("Delievry_Id")%>')" class="" title="">
                                    <span class='ico ColorFive'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span></a>
                                   <% } %>
                                <% if (rights.CanEdit)
                                   { %>
                                <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%#Eval("Delievry_Id")%>')" class="" title="">
                            
                                    <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>  <% } %>
                                  <% if (rights.CanDelete)
                                     { %>
                                <a href="javascript:void(0);" onclick="OnClickDelete('<%#Eval("Delievry_Id")%>')" class="" title="">
                                    <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a> 
                                  <% } %>
                                <a href="javascript:void(0);" onclick="OnClickCopy('<%#Eval("Delievry_Id")%>')" class="" title=" " style="display:none">
                                  <span class='ico ColorSix'><i class='fa fa-copy'></i></span><span class='hidden-xs'>Copy</span></a>
                                <a href="javascript:void(0);" onclick="OnClickStatus('<%#Eval("Delievry_Id")%>')" class="" title="" style="display:none">
                                    <span class='ico editColor'><i class='fa fa-check' aria-hidden='true'></i></span><span class='hidden-xs'>Status</span></a>
                                 <%--<% if (rights.CanView)
                                    { %>
                                <a href="javascript:void(0);" onclick="OnclickViewAttachment('<%# Container.KeyValue %>')" class="pad" title="View Attachment">
                                    <img src="../../../assests/images/attachment.png" />  </a>
                                 <% } %>--%>
                               <% if (rights.CanPrint)
                                   { %>
                                     <a href="javascript:void(0);" onclick="onPrintJv('<%#Eval("Delievry_Id")%>')" class="" title="">
                                     <span class='ico ColorSeven'><i class='fa fa-print'></i></span><span class='hidden-xs'>Print</span>
                                </a><%} %>
                                    </div>
                            </DataItemTemplate>
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <CellStyle HorizontalAlign="Center"></CellStyle>
                            <HeaderTemplate><span>Actions</span></HeaderTemplate>
                            <EditFormSettings Visible="False"></EditFormSettings>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                    </columns>
 <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                    <settingspager pagesize="10">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200"/>
                    </settingspager>
                    <settingsbehavior columnresizemode="NextColumn" />
                    <clientsideevents endcallback="function (s, e) {gridCustDelivList_EndCallBack();}" RowClick="gridRowclick" />
                    <Settings ShowGroupPanel="True"  ShowFooter="true"  ShowGroupFooter="VisibleIfExpanded" ShowStatusBar="Hidden" 
                        ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="false" />
                    <TotalSummary>
                                       <dxe:ASPxSummaryItem FieldName="NumProducts" SummaryType="Sum" /> 
                                 </TotalSummary>
                </dxe:ASPxGridView>
                <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                </dxe:ASPxGridViewExporter>

                <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                    ContextTypeName="ERPDataClassesDataContext" TableName="v_Entity_CustomerDeliveryRouteList" />
            </div>

            <div class="HiddenFieldArea" style="display: none;">
                <asp:HiddenField runat="server" ID="hiddenedit" />
            </div>

        </div>
    </div>
    </div>
     <%--DEBASHIS--%>
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="ASPxDocumentsPopup" runat="server" ClientInstanceName="cDocumentsPopup"
            Width="350px" HeaderText="Select Design(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <%--   <dxe:ASPxGridView runat="server" KeyFieldName="ID" ClientInstanceName="cgriddocuments" ID="grid_Documents"
                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" OnCustomCallback="cgridDocuments_CustomCallback" ClientSideEvents-EndCallback="cgridDocumentsEndCall"
                        Settings-ShowFooter="false" AutoGenerateColumns="False"
                        Settings-VerticalScrollableHeight="100" Settings-VerticalScrollBarMode="Hidden">
                                                      
                        <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                        <SettingsPager Visible="false"></SettingsPager>
                        <Columns>
                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="10" Caption=" "  />




                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="ID" Width="0" ReadOnly="true" Caption="No." CellStyle-CssClass="hide" HeaderStyle-CssClass="hide">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="NAME" Width="" ReadOnly="true" Caption="Design(s)">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                      
                    </dxe:ASPxGridView>--%>
                    <dxe:ASPxCallbackPanel runat="server" ID="SelectPanel" ClientInstanceName="cSelectPanel" OnCallback="SelectPanel_Callback" ClientSideEvents-EndCallback="cSelectPanelEndCall">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">



                                <%--   <dxe:ASPxCheckBox ID="selectOriginal" Text="Original" runat="server" ToolTip="Select Original"
                                    ClientInstanceName="CselectOriginal"  ClientSideEvents-CheckedChanged="function(s, e) { 
                                      grid.PerformCallback(s.GetChecked()+'^'+'stock'); }">
                                   </dxe:ASPxCheckBox>--%>

                               <%-- <dxe:ASPxCheckBox ID="selectOriginal" Text="Original For Recipient" runat="server" ToolTip="Select Original" ClientSideEvents-CheckedChanged="OrginalCheckChange"
                                    ClientInstanceName="CselectOriginal">
                                </dxe:ASPxCheckBox>

                                <dxe:ASPxCheckBox ID="selectDuplicate" Text="Duplicate For Transporter" runat="server" ToolTip="Select Duplicate" ClientSideEvents-CheckedChanged="DuplicateCheckChange"
                                    ClientInstanceName="CselectDuplicate">
                                </dxe:ASPxCheckBox>--%>
                                <dxe:ASPxCheckBox ID="selectOriginal" Text="Original For Recipient" runat="server" ToolTip="Select Original" 
                                    ClientInstanceName="CselectOriginal">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxCheckBox ID="selectDuplicate" Text="Duplicate For Transporter" runat="server" ToolTip="Select Duplicate" 
                                    ClientInstanceName="CselectDuplicate">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxCheckBox ID="selectTriplicate" Text="Triplicate For Supplier" runat="server" ToolTip="Select Triplicate"
                                    ClientInstanceName="CselectTriplicate">
                                </dxe:ASPxCheckBox>

                                <dxe:ASPxComboBox ID="CmbDesignName" ClientInstanceName="cCmbDesignName" runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                   <%-- <Items>
                                        <dxe:ListEditItem Selected="True" Text="Default" Value="1"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Tax_Invoice" Value="2"></dxe:ListEditItem>
                                    </Items>--%>
                                    <%-- <ClientSideEvents ValueChanged="function(s,e){OnCmbCountryName_ValueChange()}"></ClientSideEvents>--%>
                                </dxe:ASPxComboBox>

                                <div class="text-center pTop10">
                                    <dxe:ASPxButton ID="btnOK" ClientInstanceName="cbtnOK" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {return PerformCallToGridBind();}" />
                                    </dxe:ASPxButton>
                                </div>
                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
    </div>

    <div>
        <asp:HiddenField ID="hfIsFilter" runat="server" />
        <asp:HiddenField ID="hfFromDate" runat="server" />
        <asp:HiddenField ID="hfToDate" runat="server" />
        <asp:HiddenField ID="hfBranchID" runat="server" />
    </div>


    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>
</asp:Content>
