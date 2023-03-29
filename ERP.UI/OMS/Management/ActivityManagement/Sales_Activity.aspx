<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                29-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="Sales Activity" Language="C#" AutoEventWireup="true" EnableEventValidation="false"  CodeBehind="Sales_Activity.aspx.cs" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.ActivityManagement.Sales_Activity" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

 
    <script language="javascript" type="text/javascript">

        function popUpNotRedirect(msg) {
            StopProgress();
            jAlert(msg, 'Sales Activity', function () {
                //  txtActivityName.focus();
            });

        }
        function popUpRedirect(msg, obj) {
            StopProgress();
            jAlert(msg, 'Sales Activity', function () {
                window.location.href = obj;
                //  txtActivityName.focus();
            });

        }
        $(function () {
            $(".MyradioButtonlist").find("label").click(function () {
                window.returnValue = false;
                return window.returnValue;

            });

        })
        function Startdate(s, e) {

            var t = s.GetDate();
            if (t == "")
            { $('#MandatorysDate').attr('style', 'display:block'); }
            else { $('#MandatorysDate').attr('style', 'display:none'); }
        }
        function Enddate(s, e) {

            var t = s.GetDate();
            if (t == "")
            { $('#MandatoryEDate').attr('style', 'display:block'); }
            else { $('#MandatoryEDate').attr('style', 'display:none'); }



            var sdate = tstartdate.GetValue();
            var edate = tenddate.GetValue();

            var startDate = new Date(sdate);
            var endDate = new Date(edate);

            if (startDate > endDate) {

                flag = false;
                $('#MandatoryEgSDate').attr('style', 'display:block');
            }
            else { $('#MandatoryEgSDate').attr('style', 'display:none'); }
        }
        document.onkeydown = function (e) {

            if (event.keyCode == 18) isCtrl = true;
            if (event.keyCode == 83 && isCtrl == true) {

                //run code for CTRL+S -- ie, save!            

                $('#<%=btnSubmit.ClientID %>').click();

                return false;

            }
            else if ((event.keyCode == 120 || event.keyCode == 88) && isCtrl == true) {

                //run code for CTRL+X -- ie, discard!           

                $('#<%=btnSubmitExit.ClientID %>').click();

                    return false;

                }



        }

        function OnTitleChange() {

            if (document.getElementById('taskList')) {
                var NoteTitle = document.getElementById('taskList').value;
                if (NoteTitle == 0) {
                    BindActivityType(null);
                } else {
                    BindActivityType(NoteTitle);
                }
            }

        }
        function FunctionProduct(s, e) {
            if (lbAvailable.cphdnProduct) {

                if (lbAvailable.cphdnProduct != '') {
                    document.getElementById('hdnProduct').value = lbAvailable.cphdnProduct;

                    lbAvailable.cphdnProduct = null;
                }
            }
            else {
                document.getElementById('hdnProduct').value = lbAvailable.cphdnProduct; lbAvailable.cphdnProduct = null;

            }

            if (($('#<%=hdnbtnexit.ClientID %>').val() != "" && $('#<%=hdnbtnexit.ClientID %>').val() != null) || ($('#<%=hdnbtn.ClientID %>').val() != "" && $('#<%=hdnbtn.ClientID %>').val() != null)) {

                if ($('#<%=hdnProduct.ClientID %>').val() == "" || $('#<%=hdnProduct.ClientID %>').val() == null) {

                    $('#MandatoryProduct').attr('style', 'display:block');
                }
                else {

                    $('#MandatoryProduct').attr('style', 'display:none');
                }
            }
        }
        function FunctionCustomer(s, e) {

            if (lbClientAvailable.cphdnCustomer)
                if (lbClientAvailable.cphdnCustomer != '') {

                    document.getElementById('hdnCustomerLead').value = lbClientAvailable.cphdnCustomer;
                    lbClientAvailable.cphdnCustomer = null;
                }
        }
        function OnNextClick(s, e) {
            var indexTab = (pageControl.GetActiveTab()).index;

            if ($('#<%=hdnAssign.ClientID %>').val() == "") {
                $('#MandatoryAssign').attr('style', 'display:block');
                return false;
            }
            else {
                $('#MandatoryAssign').attr('style', 'display:none');
                pageControl.SetActiveTab(pageControl.GetTab(indexTab + 1));
            }

            if ($('#<%=hdnSupervisor.ClientID %>').val() == "") {
                $('#MandatorySupervisorAssign').attr('style', 'display:block');
                return false;
            }
            else {
                $('#MandatorySupervisorAssign').attr('style', 'display:none');
                pageControl.SetActiveTab(pageControl.GetTab(indexTab + 1));
            }

        }
        function OnBackClick(s, e) {
            $('#MandatoryAssign').attr('style', 'display:none');
            $('#MandatorySupervisorAssign').attr('style', 'display:none');
            var indexTab = (pageControl.GetActiveTab()).index;
            pageControl.SetActiveTab(pageControl.GetTab(indexTab - 1));
        }


        $(document).ready(function () {


            if ($('#<%=hdnIndustry.ClientID %>').val() != "") {

                BindProductClassGroup($('#<%=hdnIndustry.ClientID %>').val());

              
            }
          
            if ($('#<%=hdnEditIndustry.ClientID %>').val() != "") {
               

                BindProductSelectedClassGroup($('#<%=hdnEditIndustry.ClientID %>').val(), $('#<%=hdnEditProductClassGroup.ClientID %>').val());
             
            }

            $('#<%=RdnType.ClientID %>').change(function () {
                               var typeParam = $("input[name='<%=RdnType.UniqueID%>']:radio:checked").val();
                RemoveSelectedValBasedOnType();
             
                return false;

            });
            //  DisableTab();
            $('#<%=txtActivityName.ClientID %>').focus();

            $("#btnSubmit").click(function () {

                var flag = true;
                $('#<%=hdnbtn.ClientID %>').val('1');

                var productID = $('#<%=hdnProduct.ClientID %>').val();

                var tParam = $("input[name='<%=RdnType.UniqueID%>']:radio:checked").val();
                var customerLeadID = $('#<%=hdnCustomerLead.ClientID %>').val();

                var activityTypeID = $('#<%=hdnActivityType.ClientID %>').val();

                var ProductClassID = $('#<%=hdnProductClass.ClientID %>').val();
                var assignID = $("#lstAssignTo").find("option:selected").text();

                var SuperassignID = $("#lstSupervisor").find("option:selected").text();
                var IndustryID = $('#<%=hdnIndustry.ClientID %>').val();
                var sdate = tstartdate.GetValue();
                var edate = tenddate.GetValue();

                var startDate = new Date(sdate);
                var endDate = new Date(edate);

                if (assignID == "") {
                    flag = false;
                    $('#MandatoryAssign').attr('style', 'display:block');
                }
                else { $('#MandatoryAssign').attr('style', 'display:none'); }

                if (SuperassignID == "") {
                    flag = false;
                    $('#MandatorySupervisorAssign').attr('style', 'display:block');
                }
                else { $('#MandatorySupervisorAssign').attr('style', 'display:none'); }

                if (customerLeadID == "") {
                    flag = false;
                    $('#MandatoryClient').attr('style', 'display:block');
                }
                else { $('#MandatoryClient').attr('style', 'display:none'); }


                if (IndustryID == "") {
                    flag = false;
                    $('#MandatoryIndustry').attr('style', 'display:block');
                }
                else { $('#MandatoryIndustry').attr('style', 'display:none'); }

                if (tParam == "3") {
                    if (ProductClassID == "") {
                        flag = false;
                        $('#MandatoryProductClass').attr('style', 'display:block');
                    }
                    else { $('#MandatoryProductClass').attr('style', 'display:none'); }
                }
                else { $('#MandatoryProductClass').attr('style', 'display:none'); }
                if (productID == "") {
                    flag = false;
                    $('#MandatoryProduct').attr('style', 'display:block');
                }
                else { $('#MandatoryProduct').attr('style', 'display:none'); }

                if (sdate == null || sdate == "") {
                    flag = false;
                    $('#MandatorysDate').attr('style', 'display:block');
                }
                else { $('#MandatorysDate').attr('style', 'display:none'); }
                if (edate == null || sdate == "") {
                    flag = false;
                    $('#MandatoryEDate').attr('style', 'display:block');
                }
                else {
                    $('#MandatoryEDate').attr('style', 'display:none');
                    if (startDate > endDate) {

                        flag = false;
                        $('#MandatoryEgSDate').attr('style', 'display:block');
                    }
                    else { $('#MandatoryEgSDate').attr('style', 'display:none'); }
                }



                if (activityTypeID == "") {
                    flag = false;
                    $('#MandatoryActivityType').attr('style', 'display:block');
                }
                else { $('#MandatoryActivityType').attr('style', 'display:none'); }

                if (flag) {
                    $("#loading").fadeIn();
                    return flag;
                }
                else { return flag; }

               
            });




            $("#btnSubmitExit").click(function () {
                var flag = true;

                $('#<%=hdnbtnexit.ClientID %>').val('1')
                var productID = $('#<%=hdnProduct.ClientID %>').val();
                var customerLeadID = $('#<%=hdnCustomerLead.ClientID %>').val();
                var activityTypeID = $('#<%=hdnActivityType.ClientID %>').val();
                var assignID = $("#lstAssignTo").find("option:selected").text();
                var SuperassignID = $("#lstSupervisor").find("option:selected").text();
                var IndustryID = $('#<%=hdnIndustry.ClientID %>').val();

                var tParam = $("input[name='<%=RdnType.UniqueID%>']:radio:checked").val();

                var ProductClassID = $('#<%=hdnProductClass.ClientID %>').val();
                var sdate = tstartdate.GetValue();
                var edate = tenddate.GetValue();

                var startDate = new Date(sdate);
                var endDate = new Date(edate);

                if (assignID == "") {
                    flag = false;
                    $('#MandatoryAssign').attr('style', 'display:block');
                }
                else { $('#MandatoryAssign').attr('style', 'display:none'); }

                if (SuperassignID == "") {
                    flag = false;
                    $('#MandatorySupervisorAssign').attr('style', 'display:block');
                }
                else { $('#MandatorySupervisorAssign').attr('style', 'display:none'); }

                if (IndustryID == "") {
                    flag = false;
                    $('#MandatoryIndustry').attr('style', 'display:block');
                }
                else { $('#MandatoryIndustry').attr('style', 'display:none'); }
                if (customerLeadID == "") {
                    flag = false;
                    $('#MandatoryClient').attr('style', 'display:block');
                }
                else { $('#MandatoryClient').attr('style', 'display:none'); }

                if (tParam == "3") {
                    if (ProductClassID == "") {
                        flag = false;
                        $('#MandatoryProductClass').attr('style', 'display:block');
                    }
                    else { $('#MandatoryProductClass').attr('style', 'display:none'); }
                }
                else { $('#MandatoryProductClass').attr('style', 'display:none'); }
                if (productID == "") {
                    flag = false;
                    $('#MandatoryProduct').attr('style', 'display:block');
                }
                else { $('#MandatoryProduct').attr('style', 'display:none'); }

                if (sdate == null || sdate == "") {
                    flag = false;
                    $('#MandatorysDate').attr('style', 'display:block');
                }
                else { $('#MandatorysDate').attr('style', 'display:none'); }
                if (edate == null || sdate == "") {
                    flag = false;
                    $('#MandatoryEDate').attr('style', 'display:block');
                }
                else {
                    $('#MandatoryEDate').attr('style', 'display:none');
                    if (startDate > endDate) {

                        flag = false;
                        $('#MandatoryEgSDate').attr('style', 'display:block');
                    }
                    else { $('#MandatoryEgSDate').attr('style', 'display:none'); }
                }



                if (activityTypeID == "") {
                    flag = false;
                    $('#MandatoryActivityType').attr('style', 'display:block');
                }
                else { $('#MandatoryActivityType').attr('style', 'display:none'); }

                if (flag) {
                    $("#loading").fadeIn();
                    return flag;
                }
                else { return flag; }

              
            });
            $('#taskList').change(function () {
                var task_id = $(this).val();
              
            });
        });





        function AtTheTimePageLoad() {
            FieldName = 'ASPxPageControl1_cmbLegalStatus';
           
            document.getElementById("txtReferedBy_hidden").style.display = 'none';
        }
        function DisableTab() {

            pageControl.tabs[0].SetEnabled(false);


            pageControl.tabs[1].SetEnabled(false);
            pageControl.tabs[2].SetEnabled(false);

           
            if ($('#<%=hdntab.ClientID %>').val() != "") {
                pageControl.SetActiveTab(pageControl.GetTab(1));
            }
            else { pageControl.SetActiveTab(pageControl.GetTab(0)); }
        }

        $(document).ready(function () {
           
            $("#lstAssignTo").chosen().change(function () {
                var assignId = $(this).val();
                $('#<%=hdnAssign.ClientID %>').val(assignId);
                $('#MandatoryAssign').attr('style', 'display:none');
            })

            $('#lblSalesActivity').text('Assign Sales Activity');
        })




    </script>

    <script>
        //refresh product
        function RefreshProduct() {
          
            var IndustryId = '';
            var hdnIndustry = document.getElementById("hdnIndustry").value;
            if (hdnIndustry != null && hdnIndustry != '') {
                IndustryId = hdnIndustry;
                lbAvailable.PerformCallback(IndustryId);
            }
            else {
              
            }

        }
        function RemoveLeadOrCustomerOnSelecType() {
            var typeParam = "4";
            lbClientAvailable.PerformCallback("Clientlist~" + "0" + "~" + "0" + "~" + typeParam);
        }

        function RemoveIndustryOnSelecType() {
            BindIndustry()
        }
        function RemoveProductClassOnSelecType() {
            lbAvailable.PerformCallback('0');
        }
        function RemoveProductOnSelecType() {
            BindProductClassGroup('0');
        }

        function BindLeadOrCustomerOnSelecType() {

            document.getElementById('divclientlist').innerHTML = '';


            var firstParam = $("input[name='<%=RadioButtonActivityList.UniqueID%>']:radio:checked").val();

           
            var typeParam = $("input[name='<%=RdnType.UniqueID%>']:radio:checked").val();
            $('#radioCustomerLeadbtn').val(firstParam);
            var industryId = $("#lstIndustry").find("option:selected").val();

            var EntityType = '';
            var firstProductParam = '';
           
            if (firstParam == '1')
            { EntityType = '2'; }
            else
            { EntityType = '3' }

            if (industryId) {

                lbClientAvailable.PerformCallback("Clientlist~" + industryId + "~" + EntityType + "~" + typeParam);
            }

        }



        //refresh lead/customer
        function RefreshLeadOrCustomer() {

            //var IndustryId = '0';
            var IndustryId = '';
            var hdnIndustry = document.getElementById("hdnIndustry").value;
            if (hdnIndustry != null && hdnIndustry != '') {
                IndustryId = hdnIndustry;
            }


            var EntityType = '';
            var typeParam = $("input[name='<%=RdnType.UniqueID%>']:radio:checked").val();

            var firstParam = $("input[name='<%=RadioButtonActivityList.UniqueID%>']:radio:checked").val();
            var lstProducts = $('select[id$=lbAvailable]');
            var firstProductParam = '';
            if ($('#radioCustomerLeadbtn').val() == '')
            { firstProductParam = 2 }
            else
            {
                firstProductParam = $('#radioCustomerLeadbtn').val();
            }

            if (firstParam == '1')
            { EntityType = '2'; }
            else
            { EntityType = '3' }

          
            if (industryId) {

                lbClientAvailable.PerformCallback("Clientlist~" + industryId + "~" + EntityType + "~" + typeParam);
            }




        }



        function StopProgress() {

            $("#loading").fadeOut();
        }
    </script>
     <style type="text/css">
        .mb0 {
            margin-bottom:0px !important;
        }
        .validclass{

            position:absolute;
            right:-17px;
            top:8px;

        }
        #lstIndustry + .chosen-container .chosen-results {
            max-height:150px !important;
        }

  
  #dvLoading
        {
            background: url(loader.gif) no-repeat center center;
            height: 100px;
            width: 100px;
            position: fixed;
            z-index: 1000;
            left: 50%;
            top: 50%;
            margin: -25px 0 0 -25px;
        }        
        .ui-widget-overlay
        {
            background: url(overlay.png) repeat;
            left: 0;
            opacity: 0.9;
            position: absolute;
            top: 0;
        }

    </style>
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    
   


     <script type="text/javascript">


         $(function () {



             //supervisor list and assign list is binded kaushik 21-11-2016
             BindAssign();
             BindSupervisor();
             var clientType = document.getElementById("hdnEditClientType").value;



             BindIndustry();

             BindActivityType(null);
           
             var firstParam = 'F';
             var radBtn = $("table.tbl input:radio");


             //Customer or Lead radio button is clicked kaushik 21-11-2016
             $('#<%=RadioButtonActivityList.ClientID %>').click(function () {
                 document.getElementById('divclientlist').innerHTML = '';
                 var check = document.getElementById('<%= chkAll.ClientID %>');
                 check.checked = false;
                 var firstParam = $("input[name='<%=RadioButtonActivityList.UniqueID%>']:radio:checked").val();


                 var typeParam = $("input[name='<%=RdnType.UniqueID%>']:radio:checked").val();
                 $('#radioCustomerLeadbtn').val(firstParam);
                 var industryId = $("#lstIndustry").find("option:selected").val();

                 var EntityType = '';
                 var firstProductParam = '';
               
                 if (firstParam == '1')
                 { EntityType = '3'; }
                 else
                 { EntityType = '2' }



                 if (industryId) {


                     lbClientAvailable.PerformCallback("Clientlist~" + industryId + "~" + EntityType + "~" + typeParam);
                 }



             });

             // DisableSalesActivity();

         });


         //Customer or Lead radio button is clicked kaushik 21-11-2016

         var radProductBtn = $("table.tblProduct input:radio");
         // var lProductBox = $('select[id$=ListBoxProduct]');


         function RemoveSelectedValBasedOnType() {
             BindIndustry();
             var typeParam = "4";
             BindProductClassGroup('0');
             lbAvailable.PerformCallback('RemoveProduct');
             lbClientAvailable.PerformCallback("Clientlist~" + "0" + "~" + "0" + "~" + typeParam);

             $('#<%=hdnIndustry.ClientID %>').val('');
             $('#<%=hdnProduct.ClientID %>').val('');

          
             $('#<%=hdnEditIndustry.ClientID %>').val('');
           
         }
         //preferred or all product radio button is clicked kaushik 21-11-2016

         function DisableSalesActivity() {

             $('#lstSupervisor').prop('disabled', true).trigger("chosen:updated");

             $('#lstAssignTo').prop('disabled', true).trigger("chosen:updated");

             $('#lstIndustry').prop('disabled', true).trigger("chosen:updated");



             $('#ListBoxActivityType').prop('disabled', true).trigger("chosen:updated");
             $('#ListBoxProductClass').prop('disabled', true).trigger("chosen:updated");
          

         }



         function BindIndustry() {
             var lBox = $('select[id$=lstIndustry]');
             var listItems = [];


             lBox.empty();
             var hdnEditCustomerLeadValue = document.getElementById("hdnEditCustomerLead").value;

             $.ajax({
                 type: "POST",
                 url: 'Sales_Activity.aspx/GetIndustryList',
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 data: "{'hdnEditCustomerLeadValue':'" + hdnEditCustomerLeadValue + "'}",
                 success: function (msg) {
                     var list = msg.d;

                     if (list.length > 0) {

                         for (var i = 0; i < list.length; i++) {

                             var id = '';
                             var name = '';
                             id = list[i].split('|')[1];
                             name = list[i].split('|')[0];

                             listItems.push('<option value="' +
                             id + '">' + name
                             + '</option>');
                         }


                         lBox.empty();
                         $(lBox).append(listItems.join(''));

                         ListIndustry();

                         $('#lstIndustry').trigger("chosen:updated");
                         $('#lstIndustry').prop('disabled', false).trigger("chosen:updated");
                         var hdnEditIndustry = document.getElementById("hdnEditIndustry").value;
                         if (hdnEditIndustry != null && hdnEditIndustry != '') {
                             setIndustry(hdnEditIndustry);
                            
                             BindCustomerProductEdit();

                             var hdnidslsact = document.getElementById("hdnidslsact").value;

                             if (hdnidslsact != "" && hdnidslsact != null)
                             { $('#lstIndustry').prop('disabled', true).trigger("chosen:updated"); }

                         }
                         else {
                            
                         }

                     }
                     else {
                         lBox.empty();
                       
                         $('#lstIndustry').trigger("chosen:updated");
                         $('#lstIndustry').prop('disabled', true).trigger("chosen:updated");

                     }
                 },
                 error: function (XMLHttpRequest, textStatus, errorThrown) {
                    
                 }
             });


         }


         function setIndustry(obj) {
             if (obj) {
                 var lstIndustry = document.getElementById("lstIndustry");

                 for (var i = 0; i < lstIndustry.options.length; i++) {
                     if (lstIndustry.options[i].value == obj) {
                         lstIndustry.options[i].selected = true;
                     }
                 }
                 $('#lstIndustry').trigger("chosen:updated");

             }
         }

         function BindAssign() {

             var lAssignTo = $('select[id$=lstAssignTo]');
           

             lAssignTo.empty();
           
             $.ajax({
                 type: "POST",
                 url: 'Sales_Activity.aspx/GetAllUserListBeforeSelect',
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (msg) {
                     var list = msg.d;
                     var listItems = [];
                     if (list.length > 0) {

                         for (var i = 0; i < list.length; i++) {

                             var id = '';
                             var name = '';
                             id = list[i].split('|')[1];
                             name = list[i].split('|')[0];

                             listItems.push('<option value="' +
                             id + '">' + name
                             + '</option>');
                         }

                         $(lAssignTo).append(listItems.join(''));
                        
                         //   ListSupervisor();
                         ListAssignTo();
                         setWithFromAssign();

                       
                         $('#lstAssignTo').trigger("chosen:updated");
                      
                     }
                     else {
                         $('#lstSupervisor').trigger("chosen:updated");
                         $('#lstSupervisor').prop('disabled', true).trigger("chosen:updated");
                        
                     }
                    
                 },
                 error: function (XMLHttpRequest, textStatus, errorThrown) {
                  
                 }
             });


         }



         function BindSupervisor() {

          
             var lSupervisor = $('select[id$=lstSupervisor]');

            
             lSupervisor.empty();


             $.ajax({
                 type: "POST",
                 url: 'Sales_Activity.aspx/GetAllSupervisorUserListBeforeSelect',
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (msg) {
                     var list = msg.d;
                     var listItems = [];
                     if (list.length > 0) {

                         for (var i = 0; i < list.length; i++) {

                             var id = '';
                             var name = '';
                             id = list[i].split('|')[1];
                             name = list[i].split('|')[0];

                             listItems.push('<option value="' +
                             id + '">' + name
                             + '</option>');
                         }

                        
                         $(lSupervisor).append(listItems.join(''));


                         ListSupervisor();
                       
                         setWithFromSupervisor();
                     
                         $('#lstSupervisor').trigger("chosen:updated");

                     }
                     else {
                         $('#lstSupervisor').trigger("chosen:updated");
                         $('#lstSupervisor').prop('disabled', true).trigger("chosen:updated");
                       
                     }
                   
                 },
                 error: function (XMLHttpRequest, textStatus, errorThrown) {
                 
                 }
             });


         }
         function setWithFromAssign() {
             var lstAssignTo = document.getElementById("lstAssignTo");
             var listValue = document.getElementById("hdnEditAssignTo").value;
             var str_array = listValue.split(',');

             for (var i = 0; i < lstAssignTo.options.length; i++) {
                 var selectedValue = lstAssignTo.options[i].value;

                 if (str_array.indexOf(selectedValue) > -1) {
                     lstAssignTo.options[i].selected = true;
                 }
                 else {
                     lstAssignTo.options[i].selected = false;
                 }
             }
             if (str_array != "") {

                 $('#lstAssignTo').prop('disabled', true).trigger("chosen:updated");
             }
         }


         function setWithFromSupervisor() {
             var lstSupervisor = document.getElementById("lstSupervisor");
             var listValue = document.getElementById("hdnEditSupervisor").value;
             var str_array = listValue.split(',');

             for (var i = 0; i < lstSupervisor.options.length; i++) {
                 var selectedValue = lstSupervisor.options[i].value;

                 if (str_array.indexOf(selectedValue) > -1) {
                     lstSupervisor.options[i].selected = true;
                 }
                 else {
                     lstSupervisor.options[i].selected = false;
                 }
             }

             if (str_array != "") {

                 $('#lstSupervisor').prop('disabled', true).trigger("chosen:updated");
             }
         }

         function BindActivityType(noteTilte) {
             var lBox = $('select[id$=ListBoxActivityType]');
             var listItems = [];
             var selectedNoteId = '';
             if (noteTilte) {

                 selectedNoteId = noteTilte;
             }
             lBox.empty();


             $.ajax({
                 type: "POST",
                 url: 'Sales_Activity.aspx/GetActivityTypeList',
                 contentType: "application/json; charset=utf-8",
                 data: JSON.stringify({ NoteId: selectedNoteId }),
                 dataType: "json",
                 success: function (msg) {
                     var list = msg.d;

                     if (list.length > 0) {

                         for (var i = 0; i < list.length; i++) {

                             var id = '';
                             var name = '';
                             id = list[i].split('|')[1];
                             name = list[i].split('|')[0];

                             listItems.push('<option value="' +
                             id + '">' + name
                             + '</option>');
                         }



                         $(lBox).append(listItems.join(''));
                         ListActivityType();

                         $('#ListBoxActivityType').trigger("chosen:updated");
                         $('#ListBoxActivityType').prop('disabled', false).trigger("chosen:updated");
                         setWithFromActivityType();


                     }
                     else {
                         lBox.empty();
                       
                         $('#ListBoxActivityType').trigger("chosen:updated");
                         $('#ListBoxActivityType').prop('disabled', true).trigger("chosen:updated");

                     }
                 },
                 error: function (XMLHttpRequest, textStatus, errorThrown) {
                    
                 }
             });


         }


         function setWithFromActivityType() {

             var lstActivityType = document.getElementById("ListBoxActivityType");
             var listValue = document.getElementById("hdnEditActivityType").value;
             var str_array = listValue.split(',');

             for (var i = 0; i < lstActivityType.options.length; i++) {
                 var selectedValue = lstActivityType.options[i].value;

                 if (str_array.indexOf(selectedValue) > -1) {
                     lstActivityType.options[i].selected = true;
                 }
                 else {
                     lstActivityType.options[i].selected = false;
                 }
             }

             if (str_array != "") {

                 $('#ListBoxActivityType').prop('disabled', true).trigger("chosen:updated");
             }
         }


         function setWithFromProductClassType(listValue) {



             //debugger;

             var lstProductClass = document.getElementById("ListBoxProductClass");
            
             var str_array = listValue.split(',');

             for (var i = 0; i < lstProductClass.options.length; i++) {
                 var selectedValue = lstProductClass.options[i].value;

                 if (str_array.indexOf(selectedValue) > -1) {
                     lstProductClass.options[i].selected = true;
                 }
                 else {
                     lstProductClass.options[i].selected = false;
                 }
             }

             if (str_array != "") {

                 $('#ListBoxProductClass').prop('disabled', true).trigger("chosen:updated");
             }
         }




         //Bind Product Class Group
         function BindProductClassGroup(IndustryID) {

             var lBox = $('select[id$=ListBoxProductClass]');
             var listItems = [];

             lBox.empty();


             $.ajax({
                 type: "POST",
                 url: 'Sales_Activity.aspx/GetProductClassList',
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 data: "{'IndustryID':'" + IndustryID + "'}",
                 success: function (msg) {
                     var list = msg.d;

                     if (list.length > 0) {

                         for (var i = 0; i < list.length; i++) {

                             var id = '';
                             var name = '';
                             id = list[i].split('|')[1];
                             name = list[i].split('|')[0];

                             listItems.push('<option value="' +
                             id + '">' + name
                             + '</option>');
                         }



                         $(lBox).append(listItems.join(''));
                         ListProductClass();

                         $('#ListBoxProductClass').trigger("chosen:updated");
                        $('#ListBoxActivityType').prop('disabled', false).trigger("chosen:updated");
                       
                     }
                     else {

                         lBox.empty();
                       
                         $('#ListBoxProductClass').trigger("chosen:updated");
                         $('#ListBoxActivityType').prop('disabled', true).trigger("chosen:updated");

                     }
                 },
                 error: function (XMLHttpRequest, textStatus, errorThrown) {
                    
                 }
             });


         }



         //Bind Product seelcted Class Group
         function BindProductSelectedClassGroup(IndustryID, ProductClassGroup) {
             debugger;
             var lBox = $('select[id$=ListBoxProductClass]');
             var listItems = [];

             lBox.empty();


             $.ajax({
                 type: "POST",
                 url: 'Sales_Activity.aspx/GetProductClassList',
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 data: "{'IndustryID':'" + IndustryID + "'}",
                 success: function (msg) {
                     var list = msg.d;

                     if (list.length > 0) {

                         for (var i = 0; i < list.length; i++) {

                             var id = '';
                             var name = '';
                             id = list[i].split('|')[1];
                             name = list[i].split('|')[0];

                             listItems.push('<option value="' +
                             id + '">' + name
                             + '</option>');
                         }



                         $(lBox).append(listItems.join(''));
                         ListProductClass();

                         $('#ListBoxProductClass').trigger("chosen:updated");
                         $('#ListBoxProductClass').prop('disabled', false).trigger("chosen:updated");
                      
                         setWithFromProductClassType(ProductClassGroup);

                     }
                     else {

                         lBox.empty();
                        
                         $('#ListBoxProductClass').trigger("chosen:updated");
                         $('#ListBoxProductClass').prop('disabled', true).trigger("chosen:updated");

                     }
                 },
                 error: function (XMLHttpRequest, textStatus, errorThrown) {
                   
                 }
             });


         }
    </script>
    <style>
        #pageControl, .dxtc-content {
            overflow: visible !important;
        }

        #MandatoryAssign {
            position: absolute;
            right: -17px;
            top: 6px;
        }

           #MandatorySupervisorAssign {
            position: absolute;
            right: 1px;
            top: 27px;
        }
        .chosen-container.chosen-container-multi,
        .chosen-container.chosen-container-single {
            width:100% !important;
        }
        .chosen-choices {
            width:100% !important;
        }
        #lstAssignTo, #lstSupervisor,#lstIndustry {
            width:200px;
        }
        .hide {
            display:none;
        }
        .dxtc-activeTab .dxtc-link  {
            color:#fff !important;
        }
    </style>
      <script type="text/javascript">
          var classGrpOldVal = [];
          $(document).ready(function () {
           
              //for choosen drop down kaushik  17-11-2016
              ListBind();
              ListProduct();
              ListAssignTo();
              ListSupervisor();

              $('#<%=hdnClassgroupoldval.ClientID %>').val('');



              $("#ListBoxActivityType").chosen().change(function () {
                  var Id = $(this).val();

                  $('#<%=hdnActivityType.ClientID %>').val(Id);

                  $('#MandatoryActivityType').attr('style', 'display:none');

              })


              //when ListBoxProductClassGroup Type drop down changed kaushik 27-12-2016
              $("#ListBoxProductClass").chosen().change(function () {
                  debugger;
                  var Id = $(this).val();

                  var status = 'checked';
                  $('#<%=hdnProductClass.ClientID %>').val(Id);

                  if (classGrpOldVal.length == 0) {  //check

                      classGrpOldVal = Id;

                      $('#<%=hdnProductClass.ClientID %>').val(classGrpOldVal);
                      status = 'checked';
                      lbAvailable.PerformCallback("ProductGroup~" + status);
                  }
                  else {
                      var newid = Id;
                      $('#<%=hdnOldProductClass.ClientID %>').val(classGrpOldVal);
                      $('#<%=hdnProductClass.ClientID %>').val(newid);
                      classGrpOldVal = newid;
                      status = 'Unchecked';

                      lbAvailable.PerformCallback("ProductGroupFinal~");
                  }
           

                  var tParam = $("input[name='<%=RdnType.UniqueID%>']:radio:checked").val();

                  //leadindustrypopulate
                  var firstParam = $("input[name='<%=RadioButtonActivityList.UniqueID%>']:radio:checked").val();
                  var EntityType = '';
                  var industryId = $("#lstIndustry").find("option:selected").val();
                  var typeParam = "3";
                  var lstProducts = $('select[id$=lbAvailable]');
                  var firstProductParam = '';
                  if ($('#radioCustomerLeadbtn').val() == '')
                  { firstProductParam = 2 }
                  else
                  {
                      firstProductParam = $('#radioCustomerLeadbtn').val();
                  }

                  if (firstParam == '1')
                  { EntityType = '2'; }
                  else
                  { EntityType = '3' }
                
                  if ($('#<%=hdnlead.ClientID %>').val() == "") {
                      document.getElementById('divclientlist').innerHTML = '';
                      if (tParam == "3") {
                        

                          if (industryId) {
                           

                              lbClientAvailable.PerformCallback("Clientlist~" + industryId + "~" + EntityType + "~" + "0" + "~" + Id);
                          }

                      }
                    
                  }
                  //kaushik 20/17/2016

              })


              //when assign to drop down in changed kaushik  17-11-2016
              $("#lstAssignTo").chosen().change(function () {
                  var assignId = $(this).val();


                  var lSupervisor = $('select[id$=lstSupervisor]');
                  $('#<%=hdnAssign.ClientID %>').val(assignId);
                  $.ajax({
                      type: "POST",
                      url: 'Sales_Activity.aspx/GetAllUserListAfterSelect',
                      data: "{'Uid':'" + assignId + "'}",
                      contentType: "application/json; charset=utf-8",
                      dataType: "json",
                      success: function (msg) {
                          var list = msg.d;
                          var listItems = [];
                          if (list.length > 0) {

                              for (var i = 0; i < list.length; i++) {

                                  var id = '';
                                  var name = '';
                                  id = list[i].split('|')[1];
                                  name = list[i].split('|')[0];

                                  listItems.push('<option value="' +
                                  id + '">' + name
                                  + '</option>');
                              }

                              lSupervisor.empty();
                              $(lSupervisor).append(listItems.join(''));
                              ListSupervisor();


                              $('#lstSupervisor').trigger("chosen:updated");

                          }
                          else {
                              $('#lstSupervisor').trigger("chosen:updated");
                              $('#lstSupervisor').prop('disabled', true).trigger("chosen:updated");
                             
                          }
                      },
                      error: function (XMLHttpRequest, textStatus, errorThrown) {
                          
                      }
                  });

              });
              //when assign to drop down in changed kaushik  17-11-2016

              //when supervisor drop down in changed kaushik  16-11-2016
              $("#lstSupervisor").chosen().change(function () {
                  debugger;
                  var supervisorId = $(this).val();
                  var lAssignTo = $('select[id$=lstAssignTo]');
                  $('#<%=hdnSupervisor.ClientID %>').val(supervisorId);
                  $('#MandatorySupervisorAssign').attr('style', 'display:none');
                  $.ajax({
                      type: "POST",
                      url: 'Sales_Activity.aspx/GetAllUserListAfterSelect',
                      data: "{'Uid':'" + supervisorId + "'}",
                      contentType: "application/json; charset=utf-8",
                      dataType: "json",
                      success: function (msg) {
                          var list = msg.d;
                          var listItems = [];
                          if (list.length > 0) {

                              for (var i = 0; i < list.length; i++) {

                                  var id = '';
                                  var name = '';
                                  id = list[i].split('|')[1];
                                  name = list[i].split('|')[0];

                                  listItems.push('<option value="' +
                                  id + '">' + name
                                  + '</option>');
                              }
                              lAssignTo.empty();
                              $('#lstAssignTo').trigger("chosen:updated");

                              $(lAssignTo).append(listItems.join(''));


                              ListAssignTo();



                              var listValue = document.getElementById("hdnAssign").value;
                              var str_array = listValue.split(',');
                              for (var i = 0; i < document.getElementById('lstAssignTo').options.length; i++) {
                                  var selectedValue = document.getElementById('lstAssignTo').options[i].value;

                                  if (selectedValue == listValue) {
                                      document.getElementById('lstAssignTo').options[i].selected = true;
                                  }
                                  else {
                                      document.getElementById('lstAssignTo').options[i].selected = false;
                                  }
                              }

                              $('#lstAssignTo').trigger("chosen:updated");


                          }
                          else {
                              $('#lstAssignTo').trigger("chosen:updated");
                              $('#lstAssignTo').prop('disabled', true).trigger("chosen:updated");

                          }
                        
                      },
                      error: function (XMLHttpRequest, textStatus, errorThrown) {
                          
                      }
                  });

              });
              //when supervisor drop down in changed kaushik  16-11-2016

              //when customer or lead drop down in changed kaushik  16-11-2016


         
              //when Industry drop down changes start kaushik  15-12-2016
              $("#lstIndustry").chosen().change(function () {
                  document.getElementById('divprodlist').innerHTML = '';

                  var IndustryId = $(this).val();
                  $('#<%=hdnIndustry.ClientID %>').val(IndustryId);

                  //mandatory show when submit button click
                  if (($('#<%=hdnbtnexit.ClientID %>').val() != "" && $('#<%=hdnbtnexit.ClientID %>').val() != null) || ($('#<%=hdnbtn.ClientID %>').val() != "" && $('#<%=hdnbtn.ClientID %>').val() != null)) {

                      if (IndustryId == "") {
                          $('#MandatoryIndustry').attr('style', 'display:block');
                      }
                      else { $('#MandatoryIndustry').attr('style', 'display:none'); }
                  }

                  
                  var EntityType = '';

                  var lstProducts = $('select[id$=lbAvailable]');

                  var firstParam = $("input[name='<%=RadioButtonActivityList.UniqueID%>']:radio:checked").val();
                  var firstProductParam = '';
                  if ($('#radioCustomerLeadbtn').val() == '')
                  { firstProductParam = 2 }
                  else
                  {
                      firstProductParam = $('#radioCustomerLeadbtn').val();
                  }

                  if (firstParam == '1')
                  { EntityType = '2'; }
                  else
                  { EntityType = '3' }
                  //populate product list


                  lbAvailable.PerformCallback(IndustryId);
                  //populate product class group
                  BindProductClassGroup(IndustryId);
                  var typeParam = $("input[name='<%=RdnType.UniqueID%>']:radio:checked").val();
                  if ($('#<%=hdnlead.ClientID %>').val() == "") {
                      document.getElementById('divclientlist').innerHTML = '';


                      lbClientAvailable.PerformCallback("Clientlist~" + IndustryId + "~" + EntityType + "~" + typeParam);
                  }
                  //kaushik 20/17/2016





              });

              //when Industry drop down changes end kaushik   15-12-2016
          });


          //kaushik 02-01-2016 bind customer product in edit mode

          function BindCustomerProductEdit() {


              var IndustryId = document.getElementById("hdnEditIndustry").value;
              $('#<%=hdnIndustry.ClientID %>').val(IndustryId)
              var lstItems = $('select[id$=lstItems]');
              var EntityType = '';

          
              var firstProductParam = document.getElementById("hdnEditClientType").value;

              if (firstProductParam == '1')
              { EntityType = '3'; }
              else
              { EntityType = '2' }


              //kaushik 20/17/2016

              //

              lstItems.empty();
              $.ajax({
                  type: "POST",
                  url: 'Sales_Activity.aspx/GetConsumerByIndustryIdList',
                  data: "{'IndustryId':'" + IndustryId + "','EntityTypeID':'" + EntityType + "'}",
                  contentType: "application/json; charset=utf-8",
                  dataType: "json",
                  success: function (msg) {
                      var list = msg.d;
                      var listItems = [];
                      if (list.length > 0) {

                          for (var i = 0; i < list.length; i++) {

                              var id = '';
                              var name = '';
                              id = list[i].split('|')[1];
                              name = list[i].split('|')[0];

                              listItems.push('<option value="' +
                              id + '">' + name
                              + '</option>');
                          }


                          $(lstItems).append(listItems.join(''));
                          ListBind();

                          setWithFromClient();
                          $('#lstItems').trigger("chosen:updated");
                          $('#lstItems').prop('disabled', true).trigger("chosen:updated");
                      }
                      else {
                          $('#lstItems').trigger("chosen:updated");
                          $('#lstItems').prop('disabled', true).trigger("chosen:updated");
                      }
                  },
                  error: function (XMLHttpRequest, textStatus, errorThrown) {
                   
                  }
              });
          }




          function setWithFromClient() {

              var lstItems = document.getElementById("lstItems");
              var listValue = document.getElementById("hdnEditCustomerLead").value;
              var str_array = listValue.split(',');

              for (var i = 0; i < lstItems.options.length; i++) {
                  var selectedValue = lstItems.options[i].value;

                  if (str_array.indexOf(selectedValue) > -1) {
                      lstItems.options[i].selected = true;
                  }
                  else {
                      lstItems.options[i].selected = false;
                  }
              }

          }

          //when customer or lead drop down in changed kaushik  16-11-2016
          function ListBind() {

              var config = {
                  '.chsn': {},
                  '.chsn-deselect': { allow_single_deselect: true },
                  '.chsn-no-single': { disable_search_threshold: 10 },
                  '.chsn-no-results': { no_results_text: 'Oops, nothing found!' },
                  '.chsn-width': { width: "100%" }
              }
              for (var selector in config) {
                  $(selector).chosen(config[selector]);
              }

          }





          function ListProduct() {

              var config = {
                  '.chsnProduct': {},
                  '.chsnProduct-deselect': { allow_single_deselect: true },
                  '.chsnProduct-no-single': { disable_search_threshold: 10 },
                  '.chsnProduct-no-results': { no_results_text: 'Oops, nothing found!' },
                  '.chsnProduct-width': { width: "100%" }
              }
              for (var selector in config) {
                  $(selector).chosen(config[selector]);
              }

          }

          function ListSupervisor() {

              $('#lstSupervisor').chosen();
              $('#lstSupervisor').fadeIn();
          }

          function ListAssignTo() {

              $('#lstAssignTo').chosen();
              $('#lstAssignTo').fadeIn();
          }


          function ListIndustry() {

              $('#lstIndustry').chosen();
              $('#lstIndustry').fadeIn();



          }

          function ListActivityType() {

              $('#ListBoxActivityType').chosen();
              $('#ListBoxActivityType').fadeIn();

              var config = {
                  '.chsnProduct': {},
                  '.chsnProduct-deselect': { allow_single_deselect: true },
                  '.chsnProduct-no-single': { disable_search_threshold: 10 },
                  '.chsnProduct-no-results': { no_results_text: 'Oops, nothing found!' },
                  '.chsnProduct-width': { width: "100%" }
              }
              for (var selector in config) {
                  $(selector).chosen(config[selector]);
              }
          }

          function ListProductClass() {

              $('#ListBoxProductClass').chosen();
              $('#ListBoxProductClass').fadeIn();

              var config = {
                  '.chsnProduct': {},
                  '.chsnProduct-deselect': { allow_single_deselect: true },
                  '.chsnProduct-no-single': { disable_search_threshold: 10 },
                  '.chsnProduct-no-results': { no_results_text: 'Oops, nothing found!' },
                  '.chsnProduct-width': { width: "100%" }
              }
              for (var selector in config) {
                  $(selector).chosen(config[selector]);
              }
          }
          function btnSubmit_clientClick() {

              //return false;
              var flag = true;

              var productID = $('#<%=hdnProduct.ClientID %>').val();
              var customerLeadID = $('#<%=hdnCustomerLead.ClientID %>').val();
              var activityTypeID = $('#<%=hdnActivityType.ClientID %>').val();
              var assignID = $("#lstAssignTo").find("option:selected").text();
              var SuperassignID = $("#lstSupervisor").find("option:selected").text();
              var IndustryID = $('#<%=hdnIndustry.ClientID %>').val();
              var sdate = tstartdate.GetValue();
              var edate = tenddate.GetValue();

              var startDate = new Date(sdate);
              var endDate = new Date(edate);

              if (assignID == "") {
                  flag = false;
                  $('#MandatoryAssign').attr('style', 'display:block');
              }
              else { $('#MandatoryAssign').attr('style', 'display:none'); }

              if (SuperassignID == "") {
                  flag = false;
                  $('#lstSupervisor').attr('style', 'display:block');
              }
              else { $('#lstSupervisor').attr('style', 'display:none'); }

              if (customerLeadID == "") {
                  flag = false;
                  $('#MandatoryClient').attr('style', 'display:block');
              }
              else { $('#MandatoryClient').attr('style', 'display:none'); }
              if (productID == "") {
                  flag = false;
                  $('#MandatoryProduct').attr('style', 'display:block');
              }
              else { $('#MandatoryProduct').attr('style', 'display:none'); }

              if (IndustryID == "") {
                  flag = false;
                  $('#MandatoryIndustry').attr('style', 'display:block');
              }
              else { $('#MandatoryIndustry').attr('style', 'display:none'); }

              if (sdate == null) {
                  flag = false;
                  $('#MandatorysDate').attr('style', 'display:block');
              }
              else { $('#MandatorysDate').attr('style', 'display:none'); }
              if (edate == null) {
                  flag = false;
                  $('#MandatoryEDate').attr('style', 'display:block');
              }
              else {
                  $('#MandatoryEDate').attr('style', 'display:none');
                  if (startDate > endDate) {

                      flag = false;
                      $('#MandatoryEgSDate').attr('style', 'display:block');
                  }
                  else { $('#MandatoryEgSDate').attr('style', 'display:none'); }
              }



              if (activityTypeID == "") {
                  flag = false;
                  $('#MandatoryActivityType').attr('style', 'display:block');
              }
              else { $('#MandatoryActivityType').attr('style', 'display:none'); }


              return flag;
          }
          function CheckBoxListProduct_Init(s, e) {
          
              var s9 = '';
              var s19 = '';
              var s1 = s.GetSelectedItems();
              var s2 = s1.length;
              var s3 = "";
              var s13 = "";
              if (e.isSelected == false) {
                  var CurItem = s.GetItem(e.index);

                  if (document.getElementById('divprodlist').innerHTML.indexOf(',') == -1)
                  { document.getElementById('divprodlist').innerHTML = document.getElementById('divprodlist').innerHTML.replace(CurItem.text, ''); }
                  else
                  {
                      document.getElementById('divprodlist').innerHTML = document.getElementById('divprodlist').innerHTML.replace(',' + CurItem.text, '');
                  }
              
                  if (document.getElementById('hdnProduct').value.indexOf(CurItem.value) == 0) {
                      $('#<%=hdnProduct.ClientID %>').val($('#<%=hdnProduct.ClientID %>').val().replace(CurItem.value, ''));
                  }
                  else {
                      $('#<%=hdnProduct.ClientID %>').val($('#<%=hdnProduct.ClientID %>').val().replace(',' + CurItem.value, ''));
                  }
              }
              else {


                  if (s2 != 0) {
                      for (var i = 0; i < s2; i++) {

                          s9 = s1[i].text;
                          s19 = s1[i].value;


                          var arrayproduct = $('#<%=hdnProduct.ClientID %>').val().split(',');

                       
                          if (arrayproduct.indexOf(s19) == -1) {

                              s3 += s9.replace(/-/g, ' ') + ",";
                              s13 += s19.replace(/-/g, ' ') + ",";
                             
                          }

                      }
                  }
                  if (s3 != null || s3 != ', ') {
                      s3 = s3.slice(0, s3.lastIndexOf(", "));
                      s13 = s13.slice(0, s13.lastIndexOf(", "));
                      if (document.getElementById('divprodlist').innerHTML.trim() == "")
                      { document.getElementById('divprodlist').innerHTML = s3; }
                      else { document.getElementById('divprodlist').innerHTML = document.getElementById('divprodlist').innerHTML + ',' + s3; }

                      $('#<%=hdnProduct.ClientID %>').val($('#<%=hdnProduct.ClientID %>').val() + ',' + s13);
                  }

              }
            
              if (($('#<%=hdnbtnexit.ClientID %>').val() != "" && $('#<%=hdnbtnexit.ClientID %>').val() != null) || ($('#<%=hdnbtn.ClientID %>').val() != "" && $('#<%=hdnbtn.ClientID %>').val() != null)) {
                  if ($('#<%=hdnProduct.ClientID %>').val() == "")
                  { $('#MandatoryProduct').attr('style', 'display:block'); }
                  else { $('#MandatoryProduct').attr('style', 'display:none'); }
              }
          }


          //Product Class Group





          //customer
          function CheckBoxListClient_Init(s, e) {
          
              var s9 = '';
              var s19 = '';
              var s1 = s.GetSelectedItems();
              var s2 = s1.length;
              var s3 = "";
              var s13 = "";
              if (e.isSelected == false) {

                  var CurItem = s.GetItem(e.index);

                  if (document.getElementById('divclientlist').innerHTML.indexOf(',') == -1)
                  { document.getElementById('divclientlist').innerHTML = document.getElementById('divclientlist').innerHTML.replace(CurItem.text, ''); }
                  else
                  {
                      document.getElementById('divclientlist').innerHTML = document.getElementById('divclientlist').innerHTML.replace(',' + CurItem.text, '');
                  }

                  if (document.getElementById('hdnCustomerLead').value.indexOf(CurItem.value) == 0) {
                      $('#<%=hdnCustomerLead.ClientID %>').val($('#<%=hdnCustomerLead.ClientID %>').val().replace(CurItem.value, ''));
                  }
                  else {
                      $('#<%=hdnCustomerLead.ClientID %>').val($('#<%=hdnCustomerLead.ClientID %>').val().replace(',' + CurItem.value, ''));
                  }

              }
              else {


                  if (s2 != 0) {
                      for (var i = 0; i < s2; i++) {

                          s9 = s1[i].text;
                          s19 = s1[i].value;



                          var arraycustomer = $('#<%=hdnCustomerLead.ClientID %>').val().split(',');

                          if (arraycustomer.indexOf(s19) == -1) {
                              s3 += s9.replace(/-/g, ' ') + ",";
                              s13 += s19.replace(/-/g, ' ') + ",";
                          }

                      }
                  }
                  if (s3 != null || s3 != ', ') {
                      s3 = s3.slice(0, s3.lastIndexOf(", "));
                      s13 = s13.slice(0, s13.lastIndexOf(", "));

                      if (document.getElementById('divclientlist').innerHTML.trim() == "")
                      { document.getElementById('divclientlist').innerHTML = s3; }
                      else { document.getElementById('divclientlist').innerHTML = document.getElementById('divclientlist').innerHTML + ',' + s3; }

                      $('#<%=hdnCustomerLead.ClientID %>').val($('#<%=hdnCustomerLead.ClientID %>').val() + ',' + s13);
                     }

                 }
                 if (($('#<%=hdnbtnexit.ClientID %>').val() != "" && $('#<%=hdnbtnexit.ClientID %>').val() != null) || ($('#<%=hdnbtn.ClientID %>').val() != "" && $('#<%=hdnbtn.ClientID %>').val() != null)) {

                  if ($('#<%=hdnCustomerLead.ClientID %>').val() == "")
                  { $('#MandatoryClient').attr('style', 'display:block'); }
                  else
                  { $('#MandatoryClient').attr('style', 'display:none'); }
              }
          }



          function ShowAvailable(name) {

              lbAvailable.PerformCallback("productlist~" + name.value);
              //  __doPostBack("txtAvailable", "TextChanged");
          }
          function ShowClientAvailable(name) {

              lbClientAvailable.PerformCallback("SearchClientlist~" + name.value);
              //  __doPostBack("txtAvailable", "TextChanged");
          }
          $(document).ready(function () {
              $("#drpPriority").change(function () {
                
                  var value = $(this).val();
                  if (value == '0') {
                      $(this).css({ 'background': '#f5dfc3', 'border-color': '#b8b8b8', 'color': '#555' }); // low
                  } else if (value == '1') {
                      $(this).css({ 'background': '#66c19b', 'border-color': '#56a886', 'color': '#fff' }); //normal
                  }
                  else if (value == '2') {
                      $(this).css({ 'background': '#35d667', 'border-color': '#2cc35b', 'color': '#555' }); //normal
                  }
                  else if (value == '3') {
                      $(this).css({ 'background': '#ff7c7c', 'border-color': '#f07171', 'color': '#fff' }); //normal
                  }
                  else if (value == '4') {
                      $(this).css({ 'background': '#f94747', 'border-color': '#f23c3c', 'color': '#fff' }); //normal
                  }
              });
          });
          $(window).load(function () {

              if ($('#drpPriority').val() == '0') {
                  $('#drpPriority').css({ 'background': '#f5dfc3', 'border-color': '#b8b8b8', 'color': '#555' }); // low
              } else if ($('#drpPriority').val() == '1') {
                  $('#drpPriority').css({ 'background': '#66c19b', 'border-color': '#56a886', 'color': '#fff' }); //normal
              }
              else if ($('#drpPriority').val() == '2') {
                  $('#drpPriority').css({ 'background': '#35d667', 'border-color': '#2cc35b', 'color': '#555' }); //normal
              }
              else if ($('#drpPriority').val() == '3') {
                  $('#drpPriority').css({ 'background': '#ff7c7c', 'border-color': '#f07171', 'color': '#fff' }); //normal
              }
              else if ($('#drpPriority').val() == '4') {
                  $('#drpPriority').css({ 'background': '#f94747', 'border-color': '#f23c3c', 'color': '#fff' }); //normal
              }
          });



          $(document).ready(function () {

              $('.chk').change(function () {

                  var check = document.getElementById('<%= chkAll.ClientID %>');
                  if (check.checked) {
                      lbClientAvailable.SelectAll();
                      // alert(lbClientAvailable.GetItemCount());
                      GetAllClientValues();

                  } else {
                      lbClientAvailable.UnselectAll();
                      RemoveAllClientValues();

                  }
              });

           
          });

          function GetAllClientValues() {
              var client = '';

             
              var j = lbClientAvailable.GetItemCount();
            
              for (var i = 0; i < j; i++) {

                  if (i == 0) {

                      client = lbClientAvailable.GetItem(i).value;

                  }
                  else {

                      client = client + ',' + lbClientAvailable.GetItem(i).value;

                  }
                
              }

              $('#<%=hdnCustomerLead.ClientID %>').val(client);
          }

          function RemoveAllClientValues() {

              $('#<%=hdnCustomerLead.ClientID %>').val('');
          }
    </script>
    <style>
        #pageControl_CC {
            min-height:248px;
        }
        .nextic  {
            position:relative;
            padding-right:15px;
        }
        #drpPriority {
            border-radius:3px;
        }
        .nextic:before {
            position:absolute;
            right: 11px;
            top: 8px;
        }
        .prvic {
            position:relative;
            padding-left:15px;
        }
        .prvic:before{
            position:absolute;
            left: 11px;
            top: 8px;
        }
        .auto-style2 {
            position: relative;

        }
        .auto-style3 {
        }
        #RadioButtonActivityList td label {
            margin-right:8px;
            -webkit-transform:translateY(-3px);
            -moz-transform:translateY(-3px);
            transform:translateY(-3px);
        }

        .dxeDisabled_PlasticBlue, .aspNetDisabled{opacity:0.8 !important}

        #RadioButtonActivityList{float:left;}
        #btnSubmit:first-letter {
            text-decoration:underline;
        }
        .transform {
            -webkit-transform: translateY(-4px);
            -moz-transform: translateY(-4px);
             transform: translateY(-4px);
        }
        .transfor>input[type="checkbox"] {
            -webkit-transform: translateY(-2px);
            -moz-transform: translateY(-2px);
             transform: translateY(-2px);
        }
    </style>
   
    <style>
        #loading {
            position: fixed;
            width:100%;
            top: 0;
              left: 0;
              bottom: 0;
              right: 0;
              z-index:99999;
              display:none;
        }
        #loadingcont {
            
              z-index: 999;
              height: 2em;
              width: 2em;
              overflow: show;
              margin: auto;
              margin-top:250px;
              
        }
        #RdnType>tbody>tr>td{
            padding-right:16px;
        }
    </style>

    <style>
        /*Rev 1.0*/

        select
        {
            height: 30px !important;
            border-radius: 4px;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
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
            top: 26px;
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
            z-index: 0;
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

        /*.TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #EmployeeGrid , .TableMain100 #GrdEmployee,
        .TableMain100 #SalesDetailsGrid
        {
            max-width: 96% !important;
        }*/

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
        /*Rev end 1.0*/
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>Assign Sales Activity </h3>
           <div id="btncross" class="crossBtn" style="margin-left:50px;"><a href="<%=Session["pCrossUrl"]  %>"><i class="fa fa-times"></i></a></div>
        </div>

    </div>
      
        <div class="form_main relative">
        <div id="loading" style="">
            <div id="loadingcont" >
                <p id="loadingspinr">
                    <img src="/assests/images/loader.gif" />
                </p>
            </div>
        </div>
        <div>
            <div class="row clearfix">
                <div class="col-md-12">
                    <div class="">
                        <section class="col-md-12" style="padding:0;">
                            <div class="clearfix" style="background: #def0eb; padding: 9px 0px; margin-bottom: 15px; border-radius: 4px; border: 1px solid #cccccc;">
                            <div class="col-md-4">
                                    <label style="padding-bottom:0px;padding-top:4px">Select  Type</label> 
                                    <div class="radio-btn">

                                       <asp:RadioButtonList ID="RdnType" class="tbl"  RepeatDirection="Horizontal" runat="server" AutoPostBack="false" >
                                          <asp:ListItem Text="Productwise" Value="1" Selected="True" />
                                         <asp:ListItem Text="Industrywise" Value="2"  />
                                      <asp:ListItem Text="Product Classwise" Value="3"  />
                                    </asp:RadioButtonList>
                                    </div>
                                </div>
                                 


                                <div class="col-md-2" style="display:none">
                                    <label>Activity Name</label>
                                    <div class="auto-style2">
                                        <asp:TextBox ID="txtActivityName" runat="server" Width="100%" MaxLength="100"></asp:TextBox>
                                       
                                    </div>
                                </div>
                                
                                 <div class="col-md-4">
                                    <label>Assign To<span style="color: red">*</span></label>
                                    <div class="auto-style2">
                                        <asp:ListBox ID="lstAssignTo"  CssClass="hide" runat="server" Font-Size="12px"   Height="90px" Width="100%"  data-placeholder="Select..."></asp:ListBox>
                                        <asp:Label ID="lblAssignTo" runat="server" Text=""></asp:Label>
                                        <asp:TextBox ID="txtAssign" runat="server" Width="100%" style="display:none"></asp:TextBox>
                                        <span id="MandatoryAssign" style="display:none"><img id="gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc"  title="Mandatory"></span>
                                        <asp:HiddenField ID="hdnAssign" runat="server" />
                                        <asp:HiddenField ID="hdnAssignText" runat="server" />
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <label >Supervisor<span style="color: red">*</span></label>
                                    <div class="auto-style3">
                                        <asp:ListBox ID="lstSupervisor" CssClass="hide" runat="server"   Font-Size="12px" Height="90px" Width="100%"  data-placeholder="Select..."></asp:ListBox>
                                              <asp:Label ID="lblSupervisor" runat="server" Text=""></asp:Label>       
                                        <asp:TextBox ID="txtSupervisor" runat="server" Width="100%"  style="display:none">
                                             
                                        </asp:TextBox>
                                        <span id="MandatorySupervisorAssign" style="display:none"><img id="gridHistory1_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc"  title="Mandatory"></span>
                                     
                                        <asp:HiddenField ID="hdnSupervisor" runat="server" />
                                    </div>
                                </div>
                                <div class="clear"></div>
                                <div class="col-md-4">
                                    <label style="padding-bottom:5px;padding-top:10px">Task</label> 
                                    <div>
                                        <asp:DropDownList ID="taskList" runat="server" Width="100%"   onchange="OnTitleChange()">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="taskDescList" runat="server" Width="100%" CssClass="hide" >
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div>

                                          <label style="padding-top:10px">Activity Type</label><span style="color: red">*</span>
                                                <div style="padding-top: 4px;"> </div>
                                                <div  class="relative">
                                                    <asp:ListBox ID="ListBoxActivityType"   runat="server" SelectionMode="Multiple"  Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsnProduct  hide" data-placeholder="Select ..."></asp:ListBox>
                   <asp:HiddenField ID="hdnActivityType" runat="server" />
                            <asp:HiddenField ID="hdnActivityTypeText" runat="server" />   
                                                    <span id="MandatoryActivityType" style="display:none" class="validclass"><img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                                               
                                                     </div>
                                    </div>
                                </div>
                                
                                <div class="col-md-4">
                                    <div><label style="padding-bottom:5px;padding-top:10px">Priority</label><br />
                                        <asp:DropDownList ID="drpPriority" runat="server" Width="100%"  >
                                            <asp:ListItem Value="0">Low</asp:ListItem>
                                            <asp:ListItem Value="1">Normal</asp:ListItem>
                                            <asp:ListItem Value="2" >High</asp:ListItem>
                                              <asp:ListItem Value="3" Selected="True">Urgent</asp:ListItem>
                                            <asp:ListItem Value="4">Immediate</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="clear"></div>
                                <div class="col-md-4">
                                    <label style="padding-bottom:5px;padding-top:10px">Activity Start Date</label><span style="color: red">*</span>
                                    <div class="relative">
                        
                                        <dxe:ASPxDateEdit ID="txtStartDate" runat="server"  Date="" Width="100%"   ClientInstanceName="tstartdate" >
                         
                                            <TimeSectionProperties>
                                                <TimeEditProperties EditFormatString="hh:mm tt" />

                                            </TimeSectionProperties>
                                             <ClientSideEvents DateChanged="Startdate" />
                                        </dxe:ASPxDateEdit>
                                        <span id="MandatorysDate" style="display:none" class="validclass"><img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                       
                                    </div>
                                </div>
                                <div class="col-md-4"><label style="padding-bottom:5px;padding-top:10px">Activity Completion Date</label><span style="color: red">*</span>
                                    <div class="relative">
                                        <dxe:ASPxDateEdit ID="txtEndDate"  runat="server"  Date="" Width="100%"  ClientInstanceName="tenddate">
                                            <TimeSectionProperties>
                                                <TimeEditProperties EditFormatString="hh:mm tt" />
                                            </TimeSectionProperties>
                                               <ClientSideEvents DateChanged="Enddate" />
                                        </dxe:ASPxDateEdit>
                                        <span id="MandatoryEDate" style="display:none" class="validclass"><img id="2gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                    <span id="MandatoryEgSDate" style="display:none" class="validclass"><img id="2gridHistory_DXPEForm_efnew_DXEFL_DXEditor12_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Completion date should be greater than or equal to start date"></span>
                                   </div>    
                                </div>
                                <div class="col-md-4">
                                    <div><label style="padding-bottom:5px;padding-top:10px">Additional Note </label>
                                     
                                        <dxe:ASPxMemo ID="txtInstNote" runat="server" Width="100%" Height="30px"></dxe:ASPxMemo>
                                    </div>
                                </div>
                                <div class="clear"></div>
                                
                        </div>
                        </section>
                        <section class="col-md-12 clearfix" style="background: rgb(245, 244, 243);padding:0px;padding-bottom:10px; margin-bottom: 15px; border-radius: 4px; border: 1px solid rgb(204, 204, 204);">
                         <div class="col-md-3" style="">
                                <label style="padding-top: 0px;">Select Industry<span style="color: red">*</span></label>
                                <div class="auto-style3 relative">
                                        <asp:ListBox ID="lstIndustry" CssClass="hide" runat="server" Font-Size="12px" Height="90px"   Width="100%"  data-placeholder="Select..."></asp:ListBox>
                                                   
                                            <span id="MandatoryIndustry" style="display:none" class="validclass"><img id="INgridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                       
                                            
                                    <asp:HiddenField ID="hdnIndustry" runat="server" />
                                </div>
                                 
                            </div>
                            <div class="clear"></div>
                            

                              <div class="col-md-6">
                                    <div>

                                       <label style="padding-top:10px"> Select Product Class</label>
                                                <div style="padding-top: 4px;"> </div>
                                                <div  class="relative">
                                                    <asp:ListBox ID="ListBoxProductClass"   runat="server" SelectionMode="Single"  Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsnProduct  hide" data-placeholder="Select Product Class/Group"></asp:ListBox>
                   <asp:HiddenField ID="hdnProductClass" runat="server" />
                                                      <asp:HiddenField ID="hdnOldProductClass" runat="server" />
                            <asp:HiddenField ID="hdnProductClassText" runat="server" />   
                                                   <span id="MandatoryProductClass" style="display:none" class="validclass"><img id="3gridHistory12_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                    
                                                               
                                                     </div>
                                    </div>

                                   <div class=" relative">
                           
                               <a href="javascript:void(0)" onclick="RefreshProduct()" style="position:absolute;   right: -17px;top: 10px;" title="Refresh"><i class="fa fa-refresh"></i></a>
                                <div style="padding-top: 4px;"> </div>
                                <div  class="relative">
                                       <div id="divprodlist" runat="server" clientidmode="Static" style="width: 100%;display:none; font-size: xx-small; font-family: Arial; color: #0000FF; font-weight: bold;">

                    </div>
                                      <asp:TextBox ID="txtAvailableProduct" runat="server" autocomplete="off"  OnBlur="ShowAvailable(this)"  OnKeyUp="ShowAvailable(this)" onChange="ShowAvailable(this)" Width="100%" placeholder="Type Product name to search"></asp:TextBox>
                    <dxe:ASPxListBox ID="lbAvailable" runat="server" ClientInstanceName="lbAvailable"  OnCallback="lbAvailable_Callback"
                        Height="247px" Width="100%" SelectionMode="CheckColumn" Caption="" EnableCallbackMode="true">
                        <ClientSideEvents SelectedIndexChanged="CheckBoxListProduct_Init" EndCallback="FunctionProduct" />

                    </dxe:ASPxListBox>
                                  
   <asp:HiddenField ID="hdnProduct" runat="server" />
            <asp:HiddenField ID="hdnProductText" runat="server" />   
                                    <span id="MandatoryProduct" style="display:none" class="validclass"><img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                                               
                                     </div>
                            </div>
                            <div style="margin-top:10px" class="checkbox-btn">
                                <asp:CheckBox ID="chkMail" runat="server"   /> 
                                <label for="chkMail"> Send Mail</label>
                            </div>
                          </div>
                           
                            <div class="col-md-6 relative" style="padding-top: 11px;">             
                                <div style="padding-top: 0px;" >
                                    <div class="radio-btn">
                                        <asp:RadioButtonList ID="RadioButtonActivityList" Width="145px" RepeatDirection="Horizontal" runat="server" CssClass="" >
                                            <asp:ListItem Text="Lead" Value="2"  />
                                            <asp:ListItem Text="Customer" Value="1"  Selected="True"/>
                                      
                                        </asp:RadioButtonList>
                                    </div>
                                    <label for="chkMail" class="transform"> Select All</label>
                                    <asp:CheckBox ID="chkAll" runat="server"  ClientMode="Static" class="chk transfor" /> 
                                   <span style="color: red; position:absolute;top:7px;left:155px;">*</span><div class="clear"></div>
                                    
                                     
                                    <a href="javascript:void(0)" onclick="RefreshLeadOrCustomer()" style="position:absolute;right:15px;top:0" title="Refresh"> <i class="fa fa-refresh"></i></a>
                                </div>
                                <div class="relative">

                                   <div id="divclientlist" runat="server" clientidmode="Static" style="width: 100%;display:none; font-size: xx-small; font-family: Arial; color: #0000FF; font-weight: bold;">

                    </div>
                                      <asp:TextBox ID="txtClient" runat="server" autocomplete="off"  OnBlur="ShowClientAvailable(this)"  OnKeyUp="ShowClientAvailable(this)" onChange="ShowClientAvailable(this)" Width="100%" placeholder="Type Lead/Customer name to search"></asp:TextBox>
                    <dxe:ASPxListBox ID="lbClientAvailable" runat="server" ClientInstanceName="lbClientAvailable"  OnCallback="lbClientAvailable_Callback"
                        Height="280px" Width="100%" SelectionMode="CheckColumn" Caption="" EnableCallbackMode="true" >
                        <ClientSideEvents SelectedIndexChanged="CheckBoxListClient_Init"  EndCallback="FunctionCustomer" />
                    </dxe:ASPxListBox>
                                    <span id="MandatoryClient" style="display:none" class="validclass"><img id="4gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                       
                                     <asp:HiddenField ID="hdnCustomerLead" runat="server" />

                                          <asp:HiddenField ID="hdnCustomerLeadText" runat="server" />   
                                       <asp:HiddenField ID="hdnlead" runat="server" />   
                                </div>
                            </div>

                           
                            </section>
                    </div>
                    <div class="clear"></div>
                   
                </div>
                
                
                
                
                
                <div class="clear"></div>
                <div class="col-md-12">     
                    
                      <asp:Button ID="btnSubmit" Text="S&#818;ave & New" runat="server" CssClass="btn btn-primary" style="padding: 5px 10px;"  OnClick="btnSubmit_Click" AccessKey="N" />  
                    <asp:Button ID="btnSubmitExit" Text="Save & Ex&#818;it" runat="server" CssClass="btn btn-primary" style="padding: 5px 10px;"  OnClick="btnSubmitExit_Click" />                               
                   </div>
                <asp:HiddenField ID="hdntab" runat="server" />
                  <asp:HiddenField ID="hdnEditAssignTo" runat="server" />
                  <asp:HiddenField ID="hdnEditSupervisor" runat="server" />
                  <asp:HiddenField ID="hdnEditCustomerLead" runat="server" />
                  <asp:HiddenField ID="hdnEditProduct" runat="server" />
                  <asp:HiddenField ID="hdnEditActivityType" runat="server" />
                  <asp:HiddenField ID="hdnEditIndustry" runat="server" />
                 <asp:HiddenField ID="hdnEditClientType" runat="server" />
                 <asp:HiddenField ID="hdnbtn" runat="server" />
                <asp:HiddenField ID="hdnbtnexit" runat="server" />
                <asp:HiddenField ID="hdnidslsact" runat="server" />
                 <asp:HiddenField ID="hdnClassgroupoldval" runat="server" />
                <asp:HiddenField ID="hdnType" runat="server" />

                  <asp:HiddenField ID="hdnEditProductClassGroup" runat="server" />
            </div>
        </div>
      
        <input id="radioCustomerLeadbtn" type="hidden" />
    </div>
    </div>
</asp:Content>

