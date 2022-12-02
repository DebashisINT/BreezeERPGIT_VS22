<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Master.Vehicle" CodeBehind="Vehicle.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    <script type="text/javascript" src="../../../CentralData/JSScript/GenericJScript.js"></script>
    <style>
       

        .myImage {
            max-height: 100px;
            max-width: 100px;
        }
    </style>
    <style type="text/css">
        .cityDiv {
            height: 25px;
        }

        .cityTextbox {
            height: 25px;
            width: 50px;
        }

        .Top {
            height: 90px;
            width: 100%;
            padding-top: 5px;
            valign: top;
        }

        .Footer {
            height: 30px;
            width: 400px;
            padding-top: 10px;
        }

        .ScrollDiv {
            height: 250px;
            width: 400px;
            overflow-x: hidden;
            overflow-y: scroll;
        }

        .ContentDiv {
            width: 100%;
            height: 300px;
            border: 2px;
        }



        .TitleArea {
            height: 20px;
            padding-left: 10px;
            padding-right: 3px;
            background-image: url( '../images/EHeaderBack.gif' );
            background-repeat: repeat-x;
            background-position: bottom;
            text-align: center;
        }

        .FilterSide {
            float: left;
            width: 50%;
        }

        .SearchArea {
            width: 100%;
            height: 30px;
            padding-top: 5px;
        }

        .newLbl {
            margin: 5px 0 !important;
            display: block;
        }
    </style>
    <style>
        .imageArea {
            width: 150px;
            height: 100px !important;
            overflow: hidden;
        }

        .popUpHeader {
            float: right;
        }

        .blll {
            margin: 0;
            padding: 0 !important;
            margin-top: 6px;
        }

        .dxeErrorCellSys.dxeNoBorderLeft {
            position: absolute;
        }
    </style>

    <script type="text/javascript">

        function OnAddButtonClick() {
            var url = 'VehicleAddEdit.aspx?id=' + 'ADD';
            window.location.href = url;
        }




        $(function () {
            var vAnotherKeyWasPressed = false;
            var ALT_CODE = 18;

            //When some key is pressed
            $(window).keydown(function (event) {
                var vKey = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
                vAnotherKeyWasPressed = vKey != ALT_CODE;
                if (event.altKey && (event.key == 's' || event.key == 'S')) {
                    console.log('save not');
                    if (cPopup_Empcitys.IsVisible()) {
                        console.log('save');
                        cbtnSave_citys.DoClick();
                    }
                    return false;
                }

                if (event.altKey && (event.key == 'a' || event.key == 'A')) {
                    if (!cPopup_Empcitys.IsVisible()) {
                        if (document.getElementById('AddBtn') != null) {
                            console.log('new');
                            fn_PopOpen();
                            return false;
                        }

                    }

                }

                if (event.altKey && (event.key == 'c' || event.key == 'C')) {
                    console.log('save not');
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


        function fn_PopOpen() {

        }
        function afterFileUpload() {
            if (document.getElementById('hiddenedit').value == '') {
                grid.PerformCallback('savecity~' + GetObjectID('fileName').value);
            }
            else {
                grid.PerformCallback('updatecity~' + GetObjectID('hiddenedit').value + "~" + GetObjectID('fileName').value);
            }

        }

        function btnSave_citys() {
            if (ccmbIsInventory.GetValue() == 0) {
                if ((ctxtPro_Code.GetText() != '') && (ctxtPro_Name.GetText().trim() != '')) {
                    if (upload1.GetText().trim() != '') {
                        upload1.Upload();
                    } else {
                        afterFileUpload();
                    }
                }
            } else {
                if ((ctxtPro_Code.GetText() != '') && (ctxtPro_Name.GetText().trim() != '') && (ctxtQuoteLot.GetText() != '') && (cCmbQuoteLotUnit.GetValue() != null) && (ctxtTradingLot.GetText() != '') && (ctxtDeliveryLot != '') && (cCmbTradingLotUnits.GetText().trim() != '') && (cCmbDeliveryLotUnit.GetText().trim() != '') && (ccmbStockUom.GetValue() != null)) {

                    if (validReorder()) {
                        if (upload1.GetText().trim() != '') {
                            upload1.Upload();
                        } else {
                            afterFileUpload();
                        }
                    }
                }
            }

        }

        function validReorder() {
            var retval = true;
            var minlvl = (ctxtMinLvl.GetValue() != null) ? ctxtMinLvl.GetValue() : "0";
            var reordLvl = (ctxtReorderLvl.GetValue() != null) ? ctxtReorderLvl.GetValue() : "0";
            if (reordLvl < minlvl) {
                $('#reOrderError').css({ 'display': 'block' });
                retval = false;
            }
            else {
                $('#reOrderError').css({ 'display': 'None' });
            }
            return retval;
        }

        function btnSave_citysOld() {

            var valiEmail = false;

            var validPhNo = false;

            var CheckUniqueCode = false;

            var reg = /^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$/;
            if (reg.test(ctxtMarkets_Email.GetText())) {
                valiEmail = true;
            }

            if (!isNaN(ctxtMarkets_Phones.GetText()) && ctxtMarkets_Phones.GetText().length == 10) {
                validPhNo = true;
            }


            //for unique code ajax call
            var MarketsCode = ctxtMarkets_Code.GetText();
            $.ajax({
                type: "POST",
                url: "sMarkets.aspx/CheckUniqueCode",
                data: "{'MarketsCode':'" + MarketsCode + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    //                    CheckUniqueCode = msg.d;

                    if (document.getElementById('hiddenedit').value == '') {
                        CheckUniqueCode = msg.d;
                    }
                    else {
                        CheckUniqueCode == false
                    }

                    if (CheckUniqueCode == false && ctxtMarkets_Code.GetText() != '' && ctxtMarkets_Name.GetText() != '' && (ctxtMarkets_Email.GetText() == '' || valiEmail == true) && (ctxtMarkets_Phones.GetText() == '' || validPhNo == true)) {
                        if (document.getElementById('hiddenedit').value == '') {
                            //alert("in add");
                            grid.PerformCallback('savecity~');
                        }
                        else {
                            //alert("in update");
                            grid.PerformCallback('updatecity~' + GetObjectID('hiddenedit').value);
                        }
                    }
                    else if (CheckUniqueCode == true) {
                        jAlert('Please enter unique market code');
                        ctxtMarkets_Code.Focus();
                    }
                    else if (ctxtMarkets_Code.GetText() == '') {
                        jAlert('Please Enter Markets Code');
                        ctxtMarkets_Code.Focus();
                    }
                    else if (ctxtMarkets_Name.GetText() == '') {
                        jAlert('Please Enter Markets Name');
                        ctxtMarkets_Name.Focus();
                    }
                    else if (!reg.test(ctxtMarkets_Email.GetText())) {
                        jAlert('Please enter valid email');
                        ctxtMarkets_Email.Focus();
                    }
                    else if (isNaN(ctxtMarkets_Phones.GetText()) || ctxtMarkets_Phones.GetText().length != 10) {
                        jAlert('Please enter valid Phone No');
                        ctxtMarkets_Phones.Focus();
                    }

                }

            });
        }


        function fn_btnCancel() {
            cPopup_Empcitys.Hide();
            $("#txtPro_Code_EC, #txtPro_Name_EC, #txtQuoteLot_EC, #txtTradingLot_EC, #txtDeliveryLot_EC").hide();
        }

        //080517-Kallol
        function fn_Editcity(keyValue) {
            //document.getElementById('btnUdf').disabled = false;
            //cPopup_Empcitys.SetHeaderText('Modify Products');
            //grid.PerformCallback('Edit~' + keyValue);
            //document.getElementById('Keyval_internalId').value = 'ProductMaster' + keyValue;
            var url = '/OMS/Management/Master/VehicleAddEdit.aspx?id=' + keyValue;
            window.location.href = url;
        }


        //080517-Kallol
        function fn_Deletecity(keyValue) {
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    //alert("keyValue : " + keyValue);
                    grid.PerformCallback('Delete~' + keyValue);
                }
            });
        }


        function componentEndCallBack(s, e) {
            console.log(e);
            // cPopup_Empcitys.Show();
        }

        var mainAccountInUse = [];
        function grid_EndCallBack() {
            if (grid.cpinsert != null) {
                jAlert('Saved Successfully');
                //alert('Saved Successfully');
                //................CODE  UPDATED BY sAM ON 18102016.................................................
                ctxtPro_Name.GetInputElement().readOnly = false;
                //................CODE ABOVE UPDATED BY sAM ON 18102016.................................................
                cPopup_Empcitys.Hide();
            }
            else if (grid.cpinsert == 'fail') {
                jAlert("Error On Insertion \n 'Please Try Again!!'")
            }
            else if (grid.cpinsert == 'UDFManddratory') {
                jAlert("UDF is set as Mandatory. Please enter values.", "Alert", function () { OpenUdf(); });

            }
            else {
                //jAlert(grid.cpinsert);
                //cPopup_Empcitys.Hide();
            }

            if (grid.cpMainAccountInUse != null) {
                if (grid.cpMainAccountInUse != '') {
                    for (var mainCount = 0; mainCount < grid.cpMainAccountInUse.split('~').length; mainCount++) {
                        mainAccountInUse.push(grid.cpMainAccountInUse.split('~')[mainCount]);
                    }
                }
            }

            if (grid.cpEdit != null) {
                var col = grid.cpEdit.split('~')[15];
                var size = grid.cpEdit.split('~')[16];

                //................. Code Added By Sam on 25102016....................
                var sizeapplicable = grid.cpEdit.split('~')[18];
                var colorapplicable = grid.cpEdit.split('~')[19];


                //................. Code Added By Sam on 25102016....................
                ctxtPro_Code.SetText(grid.cpEdit.split('~')[1]);
                ctxtPro_Name.SetText(grid.cpEdit.split('~')[2]);
                //ctxtPro_Name.GetInputElement().readOnly = true;
                ctxtPro_Description.SetText(grid.cpEdit.split('~')[3]);
                cCmbProType.SetValue(grid.cpEdit.split('~')[4]);
                cCmbProClassCode.SetValue(grid.cpEdit.split('~')[6]);
                ctxtGlobalCode.SetText(grid.cpEdit.split('~')[7]);
                GlobalCode = grid.cpEdit.split('~')[7];
                ctxtTradingLot.SetText(grid.cpEdit.split('~')[8]);
                cCmbTradingLotUnits.SetValue(grid.cpEdit.split('~')[9]);
                cCmbQuoteCurrency.SetValue(grid.cpEdit.split('~')[10]);
                ctxtQuoteLot.SetText(grid.cpEdit.split('~')[11]);
                cCmbQuoteLotUnit.SetValue(grid.cpEdit.split('~')[12]);
                ctxtDeliveryLot.SetText(grid.cpEdit.split('~')[13]);
                cCmbDeliveryLotUnit.SetValue(grid.cpEdit.split('~')[14]);
                if (col != '') {
                    cCmbProductColor.SetValue(grid.cpEdit.split('~')[15]);
                    ProdColor = grid.cpEdit.split('~')[15];
                }
                else {
                    cCmbProductColor.SetValue('0');
                    ProdColor = 0;
                }
                if (size != '') {
                    cCmbProductSize.SetValue(grid.cpEdit.split('~')[16]);
                    ProdSize = grid.cpEdit.split('~')[16];
                }
                else {
                    cCmbProductSize.SetValue('0');
                    ProdSize = 0;
                }


                if (sizeapplicable == 'True') {
                    Rrdblapp.SetSelectedIndex(0);
                    SizeApp = 0;
                }
                else {
                    Rrdblapp.SetSelectedIndex(1);
                    SizeApp = 1;
                }

                if (colorapplicable == 'True') {
                    RrdblappColor.SetSelectedIndex(0);
                    ColApp = 0;
                }
                else {
                    RrdblappColor.SetSelectedIndex(1);
                    ColApp = 1;
                }
                GetObjectID('hiddenedit').value = grid.cpEdit.split('~')[17];

                //Code Added by Debjyoti 30-12-2016
                if (grid.cpEdit.split('~')[20] != '0') {
                    cCmbBarCodeType.SetValue(grid.cpEdit.split('~')[20]);
                    barCodeType = grid.cpEdit.split('~')[20];
                }
                ctxtBarCodeNo.SetText(grid.cpEdit.split('~')[21]);
                BarCode = grid.cpEdit.split('~')[21];


                //Code added by debjyoti 04-01-2017
                if (grid.cpEdit.split('~')[22] == 'False')
                    ccmbIsInventory.SetValue('0');
                else
                    ccmbIsInventory.SetValue('1');

                cCmbStockValuation.SetValue(grid.cpEdit.split('~')[23].trim());
                if (grid.cpEdit.split('~')[24].trim() == '0')
                    ctxtSalePrice.SetText('');
                else
                    ctxtSalePrice.SetText(grid.cpEdit.split('~')[24].trim());

                if (grid.cpEdit.split('~')[25].trim() == '0')
                    ctxtMinSalePrice.SetText('');
                else
                    ctxtMinSalePrice.SetText(grid.cpEdit.split('~')[25].trim());

                if (grid.cpEdit.split('~')[26].trim() == '0')
                    ctxtPurPrice.SetText('');
                else
                    ctxtPurPrice.SetText(grid.cpEdit.split('~')[26].trim());

                if (grid.cpEdit.split('~')[27].trim() == '0')
                    ctxtMrp.SetText('');
                else
                    ctxtMrp.SetText(grid.cpEdit.split('~')[27].trim());

                ccmbStockUom.SetValue(grid.cpEdit.split('~')[28]);
                ctxtMinLvl.SetText(grid.cpEdit.split('~')[29]);
                ctxtReorderLvl.SetText(grid.cpEdit.split('~')[30]);
                ccmbNegativeStk.SetValue(grid.cpEdit.split('~')[31].trim());
                cCmbTaxCodeSale.SetValue(grid.cpEdit.split('~')[32].trim());
                taxCodeSale = grid.cpEdit.split('~')[32].trim();

                cCmbTaxCodePur.SetValue(grid.cpEdit.split('~')[33].trim());
                taxCodePur = grid.cpEdit.split('~')[33].trim();

                if (grid.cpEdit.split('~')[34].trim() == '') {
                    cCmbTaxScheme.SetValue(0);
                    taxScheme = 0;
                }
                else {
                    cCmbTaxScheme.SetValue(grid.cpEdit.split('~')[34].trim());
                    taxScheme = grid.cpEdit.split('~')[34].trim();
                }
                if (grid.cpEdit.split('~')[35].trim() == 'True') {
                    cChkAutoApply.SetChecked(true);
                    autoApply = true;
                    GetCheckBoxValue(true);
                } else {
                    cChkAutoApply.SetChecked(false);
                    autoApply = false;
                    GetCheckBoxValue(false);
                }

                cProdImage.SetImageUrl(grid.cpEdit.split('~')[36].trim());
                document.getElementById('fileName').value = grid.cpEdit.split('~')[36].trim();
                gridLookup.Clear();
                cComponentPanel.PerformCallback(grid.cpEdit.split('~')[37].trim());
                cCmbStatus.SetValue(grid.cpEdit.split('~')[38].trim());

                //  ctxtHsnCode.SetText(grid.cpEdit.split('~')[39].trim()); 
                cHsnLookUp.gridView.SelectItemsByKey(grid.cpEdit.split('~')[39].trim());
                cAspxServiceTax.SetValue(grid.cpEdit.split('~')[40].trim());
                // ccmbStatusad.SetValue(grid.cpEdit.split('~')[31].trim());
                //Debjyoti 31-01-2017
                //packing details
                ctxtPackingQty.SetValue(grid.cpEdit.split('~')[41].trim());
                ctxtpacking.SetValue(grid.cpEdit.split('~')[42].trim());
                ccmbPackingUom.SetValue(grid.cpEdit.split('~')[43].trim());
                //packing details End Here
                console.log(grid.cpEdit.split('~')[44]);
                if (grid.cpEdit.split('~')[44] == 'False')
                    caspxInstallation.SetValue('0');
                else
                    caspxInstallation.SetValue('1');
                ccmbBrand.SetValue(grid.cpEdit.split('~')[45].trim());

                if (grid.cpEdit.split('~')[46] == 'True')
                    ccmbIsCapitalGoods.SetValue('1');
                else
                    ccmbIsCapitalGoods.SetValue('0');
                //  ccmbIsCapitalGoods.SetValue(grid.cpEdit.split('~')[46].trim());

                cmb_tdstcs.SetValue(grid.cpEdit.split('~')[47]);

                if (grid.cpEdit.split('~')[48] == "True")
                    ccmbOldUnit.SetValue('1');
                else
                    ccmbOldUnit.SetValue('0');


                ccmbsalesInvoice.SetValue(grid.cpEdit.split('~')[49].trim());
                ccmbSalesReturn.SetValue(grid.cpEdit.split('~')[50].trim());
                ccmbPurInvoice.SetValue(grid.cpEdit.split('~')[51].trim());
                ccmbPurReturn.SetValue(grid.cpEdit.split('~')[52].trim());

                ctxtPro_Code.SetEnabled(false);
                ccmbIsInventory.SetEnabled(false);

                changeControlStateWithInventory();
                cPopup_Empcitys.Show();

                ctxtPro_Name.Focus();
            }
            if (grid.cpUpdate != null) {
                if (grid.cpUpdate == 'Success') {
                    jAlert('Saved Successfully');
                    cPopup_Empcitys.Hide();
                }
                else {
                    jAlert("Error on Updation\n'Please Try again!!'")
                    //cPopup_Empcitys.Hide();
                }
            }
            if (grid.cpUpdateValid != null) {
                if (grid.cpUpdateValid == "StateInvalid") {
                    jAlert("Please Select proper country state and city");
                    //cPopup_Empcitys.Show();
                    //cCmbState.Focus();
                    //alert(GetObjectID('<%#hiddenedit.ClientID%>').value);
                    //grid.PerformCallback('Edit~'+GetObjectID('<%#hiddenedit.ClientID%>').value);
                    //grid.cpUpdateValid=null;
                }
            }
            if (grid.cpDelete != null) {
                if (grid.cpDelete == 'Success')
                    jAlert('Data Deleted Successfully');
                else if (grid.cpDelete == 'refrenceExist')
                    jAlert('Used in other module. Cannot delete.');
                else
                    jAlert("Error on deletion\n'Please Try again!!'")
            }
            //debjyoti
            if (grid.cpErrormsg != null) {
                jAlert(grid.cpErrormsg);
                grid.cpErrormsg = null;
            }

            if (grid.cpExists != null) {
                if (grid.cpExists == "Exists") {
                    jAlert('Record already Exists');
                    //cPopup_Empcitys.Hide();
                }
                else {
                    jAlert("Error on operation \n 'Please Try again!!'")
                    //cPopup_Empcitys.Hide();
                }
            }
        }
        function OnCmbCountryName_ValueChange() {
            cCmbState.PerformCallback("BindState~" + cCmbCountryName.GetValue());
        }
        function CmbState_EndCallback() {
            cCmbState.SetSelectedIndex(0);
            cCmbState.Focus();
        }
        function OnCmbStateName_ValueChange() {
            cCmbCity.PerformCallback("BindCity~" + cCmbState.GetValue());
        }
        function CmbCity_EndCallback() {
            cCmbCity.SetSelectedIndex(0);
            cCmbCity.Focus();
        }
        $(document).ready(function () {
            $('.dxpc-closeBtn').click(function () {
                fn_btnCancel();
            });
        });
        function gridRowclick(s, e) {
            $('#cityGrid').find('tr').removeClass('rowActive');
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
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                grid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                grid.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
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
            <h3>Vehicle Master</h3>
        </div>
    </div>

    <div class="form_main">
        <div class="Main">
        <div class="clearfix">
             <% if (rights.CanAdd)
                { %>
            <a href="javascript:void(0);" onclick="OnAddButtonClick();" class="btn btn-success btn-radius">
                <span class="btn-icon"><i class="fa fa-plus" ></i></span><span><u>A</u>dd New</span> 

            </a>
            <% } %>
            
                <% if (rights.CanExport)
                   { %>
             <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius " OnChange="if(!AvailableExportOption()){return false;}" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
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

            <div class="GridViewArea relative">

                <dxe:ASPxGridView ID="cityGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid"
                    KeyFieldName="vehicle_Id" Width="100%"
                    OnHtmlRowCreated="cityGrid_HtmlRowCreated"
                    OnHtmlEditFormCreated="cityGrid_HtmlEditFormCreated" Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Auto"
                    OnCustomCallback="cityGrid_CustomCallback"  SettingsBehavior-AllowFocusedRow="true" Settings-HorizontalScrollBarMode="Visible" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"  >
                    <settingssearchpanel visible="True" delay="5000" />
                    <settings showgrouppanel="True" showstatusbar="Hidden" showfilterrow="true" showfilterrowmenu="True" />
                    <columns>

                              <dxe:GridViewDataTextColumn Caption="Vehicle No." FieldName="vehicle_regNo" ReadOnly="True"
                                Visible="True"  VisibleIndex="0" Width="180">
                                <EditFormSettings Visible="True" />
                                <Settings AutoFilterCondition="Contains" />
                                  <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Engine No." FieldName="vehicle_engineNo" ReadOnly="True"
                                Visible="True"  VisibleIndex="1">
                                <Settings AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="True" />
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Chassis No." FieldName="vehicle_ChassisNo" ReadOnly="True"
                                Visible="True"  VisibleIndex="2">
                                <Settings AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="True" />
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Vehicle Type" FieldName="vehicle_fuelType" ReadOnly="True"
                                Visible="True"  VisibleIndex="3">
                                <Settings AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="True" />
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Vehicle Maker" FieldName="vehicle_maker" ReadOnly="True"
                                Visible="True"  VisibleIndex="4">
                                <EditFormSettings Visible="True" />
                                <Settings AutoFilterCondition="Contains" />
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Model" FieldName="vehicle_model" ReadOnly="True"
                                Visible="True"  VisibleIndex="5">
                                <EditFormSettings Visible="True" />
                                <Settings AutoFilterCondition="Contains" />
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>
                                

                                 <dxe:GridViewDataTextColumn Caption="Vehicle Type" FieldName="vehicle_Type" ReadOnly="True"
                                Visible="True"  VisibleIndex="6">
                                <Settings AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="True" />
                                     <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>
                        
                                 <dxe:GridViewDataTextColumn Caption="Year of Registration" FieldName="vehicle_yearReg" ReadOnly="True"
                                Visible="True"  VisibleIndex="7">
                                <Settings AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="True" />
                                     <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>
                         <dxe:GridViewDataTextColumn Caption="Owner Type" FieldName="vehicle_vehOwnerType" ReadOnly="True"
                                Visible="True"  VisibleIndex="8">
                                <Settings AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="True" />
                             <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>
                      

                         <dxe:GridViewDataTextColumn Caption="Engine CC" FieldName="vehicle_engineCC" ReadOnly="True"
                                Visible="True"  VisibleIndex="9">
                                <Settings AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="True" />
                             <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Log Book Status" FieldName="vehicle_LogBookStatus" ReadOnly="True"
                                Visible="True"  VisibleIndex="10">
                                <Settings AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="True" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>

                          <dxe:GridViewDataTextColumn Caption="GPS Installed" FieldName="vehicle_isGPSInstalled" ReadOnly="True"
                                Visible="True"  VisibleIndex="11">
                                <Settings AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="True" />
                              <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Alloted To" FieldName="vehicle_AllotedTo" ReadOnly="True"
                                Visible="True"  VisibleIndex="12">
                                <Settings AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="True" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>


                        
                        <dxe:GridViewDataTextColumn Caption="Fleet Card Applied?" FieldName="vehicle_isFleetCardApplied" ReadOnly="True"
                                Visible="True" VisibleIndex="13" Width="120">
                                <Settings AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="True" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Fleet CardNumber" FieldName="vehicle_FleetCardNumber" ReadOnly="True"
                                Visible="True" VisibleIndex="14" Width="120">
                                <Settings AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="True" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>


                         <dxe:GridViewDataTextColumn Caption="Happy Card" FieldName="vehicle_HappyCard" ReadOnly="True"
                                Visible="True"  VisibleIndex="15">
                                <Settings AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="True" />
                             <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>
                                              
                         
                        <dxe:GridViewDataTextColumn Caption="Insurer Name" FieldName="vehicle_InsurerName" ReadOnly="True"
                                Visible="True"  VisibleIndex="16">
                                <Settings AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="True" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>

                          <dxe:GridViewDataTextColumn Caption="Policy No" FieldName="vehicle_PolicyNo" ReadOnly="True"
                                Visible="True"  VisibleIndex="17">
                                <Settings AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="True" />
                              <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>

                         <dxe:GridViewDataTextColumn Caption="Policy Valid Upto" FieldName="vehicle_PolicyValidUpto" ReadOnly="True"
                                Visible="True"  VisibleIndex="18">
                                <Settings AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="True" />
                             <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Insurance Given To" FieldName="vehicle_InsuranceGivenTo" ReadOnly="True"
                                Visible="True" VisibleIndex="19">
                                <Settings AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="True" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>

                        
                          <dxe:GridViewDataTextColumn Caption="TaxTokenNo" FieldName="vehicle_TaxTokenNo" ReadOnly="True"
                                Visible="True"  VisibleIndex="20">
                                <Settings AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="True" />
                              <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="TaxValidUpto" FieldName="vehicle_TaxValidUpto" ReadOnly="True"
                                Visible="True"  VisibleIndex="21">
                                <Settings AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="True" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>

                      
                          <dxe:GridViewDataTextColumn Caption="Pollution Case Dtl" FieldName="vehicle_PollutionCaseDtl" ReadOnly="True"
                                Visible="True"  VisibleIndex="22">
                                <Settings AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="True" />
                              <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>
                          <dxe:GridViewDataTextColumn Caption="PollutionCertValidUpto" FieldName="vehicle_PollutionCertValidUpto" ReadOnly="True"
                                Visible="True"  VisibleIndex="23">
                                <Settings AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="True" />
                              <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>
                          <dxe:GridViewDataTextColumn Caption="Auth Letter" FieldName="vehicle_isAuthLetter" ReadOnly="True"
                                Visible="True"  VisibleIndex="24">
                                <Settings AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="True" />
                              <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="AuthLetterValidUpto" FieldName="vehicle_AuthLetterValidUpto" ReadOnly="True"
                                Visible="True"  VisibleIndex="25">
                                <Settings AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="True" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Blue Book" FieldName="vehicle_BlueBook" ReadOnly="True"
                                Visible="True"  VisibleIndex="26">
                                <Settings AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="True" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="CF Case Details" FieldName="vehicle_CFDetails" ReadOnly="True"
                                Visible="True"  VisibleIndex="27">
                                <Settings AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="True" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="CF Paper Valid Upto" FieldName="vehicle_CFValidUpto" ReadOnly="True"
                                Visible="True"  VisibleIndex="28">
                                <Settings AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="True" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>
                       

                         <dxe:GridViewDataTextColumn Caption="Fuel Type" FieldName="vehicle_fuelType" ReadOnly="True"
                                Visible="True"  VisibleIndex="29">   
                                <Settings AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="True" />
                             <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Is Active" FieldName="vehicle_isActive" ReadOnly="True"
                                Visible="True"  VisibleIndex="30">   
                                <Settings AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="True" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>
                         <dxe:GridViewDataTextColumn Caption="By Hand" FieldName="ByHand" ReadOnly="True"
                                Visible="True"  VisibleIndex="30">   
                                <Settings AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="True" />
                             <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn ReadOnly="True" Width="0" CellStyle-HorizontalAlign="Center" VisibleIndex="31"  Visible="True">
                                <HeaderStyle HorizontalAlign="Center" />

                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                <HeaderTemplate>
                                
                                </HeaderTemplate>
                                <DataItemTemplate>
                                
                                    <div class='floatedBtnArea'>
                                        <%if (rights.CanEdit)
                                          { %>
                                        <a href="javascript:void(0);" onclick="fn_Editcity('<%# Container.KeyValue %>')" title="" class="">
                                            <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>
                                        <%} %>
                                        <%if (rights.CanDelete)
                                          { %>
                                        <a href="javascript:void(0);" onclick="fn_Deletecity('<%# Container.KeyValue %>')" title=""  class="">
                                            <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                                        <%} %>
                                    </div>
                                </DataItemTemplate>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>
                    
                    </columns>
                    <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                    <settingspager pagesize="10">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200"/>
                    </settingspager>
                    <settingsbehavior columnresizemode="NextColumn" />
                    <clientsideevents endcallback="function (s, e) {grid_EndCallBack();}" RowClick="gridRowclick"  />
                </dxe:ASPxGridView>
            </div>

            <div class="HiddenFieldArea" style="display: none;">
                <asp:HiddenField runat="server" ID="hiddenedit" />
            </div>

        </div>

        <dxe:ASPxPopupControl ID="AssignValuePopup" runat="server" ClientInstanceName="AssignValuePopup"
            Width="200px" HeaderText="Add / Edit Key Value" PopupHorizontalAlign="WindowCenter"
            BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <contentstyle verticalalign="Top" cssclass="pad">
            </contentstyle>
            <contentcollection>
             <dxe:PopupControlContentControl ID="AssignValuePopupContent" runat="server">
                <div id="generatedForm">
                </div>
                <div id="SubmitFrm">

                    <asp:TextBox ID="KeyField" runat="server" Style="display: none;"></asp:TextBox>
                    <asp:TextBox ID="ValueField" runat="server" Style="display: none;"></asp:TextBox>
                    <asp:TextBox ID="RexPageName" runat="server" Style="display: none;"></asp:TextBox>


                    <asp:Button ID="Button1" runat="server" Text="Save" CssClass="btn btn-primary" OnClientClick="return SaveDataToResource()" OnClick="BTNSave_clicked" Style="margin-left: 155px;" />

                </div>
             </dxe:PopupControlContentControl>
            </contentcollection>
            <headerstyle backcolor="LightGray" forecolor="Black" />
         </dxe:ASPxPopupControl>
                <asp:SqlDataSource ID="ProductDataSource" runat="server"  
            SelectCommand="SELECT 
                               [vehicle_Id]
                              ,[vehicle_regNo]
                              ,[vehicle_engineNo]
                              ,[vehicle_engineCC]
                              ,[vehicle_Type]
                              ,[vehicle_maker]
                              ,[vehicle_model]
                              ,[vehicle_yearReg]
                              ,[vehicle_fuelType]
                              ,[vehicle_isActive]
                              ,[CreateDate]
                              ,[CreateUser]
                              ,[LastModifyDate]
                              ,[LastModifyUser]
                              ,[vehicle_LogBookStatus]
                              ,[vehicle_AllotedTo]
                              ,[vehicle_isFleetCardApplied]
                              ,[vehicle_FleetCardNumber]
                              ,[vehicle_HappyCard]
                              ,[vehicle_InsurerName]
                              ,[vehicle_PolicyNo]
                              ,[vehicle_PolicyValidUpto]
                              ,[vehicle_InsuranceGivenTo]
                              ,[vehicle_TaxTokenNo]
                              ,[vehicle_TaxValidUpto]
                              ,[vehicle_PollutionCaseDtl]
                              ,[vehicle_PollutionCertValidUpto]
                              ,[vehicle_isAuthLetter]
                              ,[vehicle_AuthLetterValidUpto]
                              ,[vehicle_BlueBook]
                              ,[vehicle_CFDetails]
                              ,[vehicle_CFValidUpto]
                              ,[vehicle_vehOwnerType]
                              ,[vehicle_isGPSInstalled]
                              ,[vehicle_ChassisNo]
                              ,[vehicle_Pollution]
                          FROM [tbl_master_vehicle]"
            ></asp:SqlDataSource>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server"  Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true" >
        </dxe:ASPxGridViewExporter>
    </div>

</asp:Content>
