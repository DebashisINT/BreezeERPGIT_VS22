<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="TransactionLockConfigouration.aspx.cs" Inherits="ERP.OMS.Management.Master.TransactionLockConfigouration" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="/assests/js/init.js"></script>
    <script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <script src="../Activities/JS/SearchMultiPopup.js"></script>
    <link href="../Activities/CSS/SearchPopup.css" rel="stylesheet" />
    <script type="text/javascript">
     

        var ProdArr = new Array();
        $(document).ready(function () {
            cGrdQuotation.Refresh();
            cGrdQuotation.Refresh();
            var ProdObj = new Object();
            ProdObj.Name = "ProductSource";
            ProdObj.ArraySource = ProdArr;
            arrMultiPopup.push(ProdObj);
        })

     //$(document).ready(function () {
     //    if (ccmbModule.GetValue() == "0" || ccmbModule.GetValue() == null) {
     //        $("#dvlblAdd").hide();
     //        $("#dvtxtAdd").hide();
        
     //        $("#dvlblEdit").hide();
     //        $("#dvtxtEdit").hide();
        
     //        $("#dvlbldelete").hide();
     //        $("#dvtxtDelete").hide();
     //    }
        
     //});
        
        function OnMoreInfoClick(ModuleId)
        {
            Modulechanged(ModuleId);
            cPopup_MopduleWise.Show();
        }

        function Modulechanged(ModuleId)
       {
            var ModCount = ModuleId;
            var Val = ModuleId;
            $("#hdncWiseProductId").val(ModuleId);
             //$.ajax({
             //    type: "POST",
             //    url: "TransactionLockConfigouration.aspx/LockValue",
             //    data: JSON.stringify({ Val: Val }),
             //    contentType: "application/json; charset=utf-8",
             //    dataType: "json",
             //    async: false,
             //    success: function (msg) {
             //        var status = msg.d;
             //        var Addval = status.split("~")[0];
             //        var EditVal = status.split("~")[1];
             //        var deleteval = status.split("~")[2];
             //        var CopyVal = status.split("~")[3];

                     //if (Addval == "True") {
                     //    $("#dvlblAdd").show();
                     //    $("#dvtxtAdd").show();
                     //}
                     //else {
                     //    $("#dvlblAdd").hide();
                     //    $("#dvtxtAdd").hide();
                     //}
                     //if (EditVal == "True") {
                     //    $("#dvlblEdit").show();
                     //    $("#dvtxtEdit").show();
                     //}
                     //else {
                     //    $("#dvlblEdit").hide();
                     //    $("#dvtxtEdit").hide();
                     //}
                     //if (deleteval == "True") {
                     //    $("#dvlbldelete").show();
                     //    $("#dvtxtDelete").show();
                     //}
                     //else {
                     //    $("#dvlbldelete").hide();
                     //    $("#dvtxtDelete").hide();
                     //}


             //    }
             //});
         //if (ctxtModule.GetText() != "" && ctxtModule.GetText() != null && $("#hdnCalcommitProductId").val() != "" && $("#hdnCalcommitProductId").val() != null && $("#hdnCalcommitProductId").val() != undefined)
            if (ModCount != 0 && ModCount != "" && ModCount != null && ModCount !="0")
            {
                 $.ajax({
                     type: "POST",
                     url: "TransactionLockConfigouration.aspx/ModuleDateValue",
                     data: JSON.stringify({ Val: Val }),
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     async: false,
                     success: function (msg) {

                         var modval = msg.d;
                         if (modval != "0") {
                             var addtype = modval.toString().split('~')[0];

                             var addfromdate = modval.toString().split('~')[1];
                             var addtodate = modval.toString().split('~')[2];
                             var edittype = modval.toString().split('~')[3];

                             var editfromdate = modval.toString().split('~')[4];
                             var edittodate = modval.toString().split('~')[5];
                             var deletetype = modval.toString().split('~')[6];

                             var deletefromdate = modval.toString().split('~')[7];
                             var deletetodate = modval.toString().split('~')[8];
                             var Addcheckbox = modval.toString().split('~')[9];
                             var Editcheckbox = modval.toString().split('~')[10];
                             var Deletecheckbox = modval.toString().split('~')[11];
                             if (addtype != "1")
                             {
                                 cAddFormDate.SetDate(new Date(addfromdate));
                                 cAddtoDate.SetDate(new Date(addtodate));
                             }
                             else if (addtype == "1") {
                                 cAddFormDate.Clear();
                                 cAddtoDate.Clear();
                             }
                             if (edittype != "2") {
                                 cEditFormDate.SetDate(new Date(editfromdate));
                                 cEditToDate.SetDate(new Date(edittodate));
                             }
                             else if (edittype == "2") {
                                 cEditFormDate.Clear();
                                 cEditToDate.Clear();
                             }
                             if (deletetype != "3") {
                                 cDeleteFormDate.SetDate(new Date(deletefromdate));
                                 cDeleteToDate.SetDate(new Date(deletetodate));
                             }
                            else if (deletetype == "3") {
                                cDeleteFormDate.Clear();
                                cDeleteToDate.Clear();
                            }
                             if(Addcheckbox=="True")
                             {
                                 cchkAdd.SetChecked(true)
                             }
                             else
                             {
                                 cchkAdd.SetChecked(false)
                             }
                             if (Editcheckbox == "True") {
                                 cchkEdit.SetChecked(true)
                             }
                             else {
                                 cchkEdit.SetChecked(false)
                             }
                             if (Deletecheckbox == "True") {
                                 cchkDelete.SetChecked(true)
                             }
                             else {
                                 cchkDelete.SetChecked(false)
                             }


                         }
                         else if(modval =="0")
                         {
                             cAddFormDate.Clear();
                             cAddtoDate.Clear();
                             cEditFormDate.Clear();
                             cEditToDate.Clear();
                             cDeleteFormDate.Clear();
                             cDeleteToDate.Clear();

                         }

                     }
                 });

                 //$.ajax({
                 //    type: "POST",
                 //    url: "TransactionLockConfigouration.aspx/ModuleCheckBoxValue",
                 //    //data: JSON.stringify(),
                 //    contentType: "application/json; charset=utf-8",
                 //    dataType: "json",
                 //    async: false,
                 //    success: function (msg) {
                 //        var d = msg.d;



                 //    }
                 //});

             }


     }


        function OpenEntryDate(s,e)
        {
            cPopup_AllFreeze.Show();
            if ($("#hdnMasterFromdate").val() != null && $("#hdnMasterFromdate").val() != "" && $("#hdnMasterFromdate").val() != undefined) {
                cAllFromDate.SetDate(new Date($("#hdnMasterFromdate").val()));
                cAllToDate.SetDate(new Date($("#hdnMasterToDate").val()));
            }
        }

        function OpenUnFreezeDate(s,e)
        {
            jConfirm('Do you want to unfreeze all. Confirm ?', 'Confirmation Dialog', function (r) {
                if(r==true)
                {

                    $.ajax({
                        type: "POST",
                        url: "TransactionLockConfigouration.aspx/DeleteAllFreezeData",
                        data: JSON.stringify(),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg) {
                            var d = msg.d;
                            cGrdQuotation.Refresh();
                            cGrdQuotation.Refresh();
                        }
                    });
                }
                //else
                //{

                //}
            });
        }
        function AllCloseData(s,e)
        {
            cPopup_AllFreeze.Hide();
        }
        function CloseData(s,e)
        {
            cPopup_MopduleWise.Hide();
        }

        function fn_AllbtnValidate(s,e)
        {
            var flag = true;
            if (cAllFromDate.GetDate() == null && cAllToDate.GetDate() != null)
            {
                flag = false;
            }
            $("#hdnCalcommitProductId").val("All");
           e.processOnServer = flag;
        }
     function  fn_btnValidate(s,e)
     {
         var flag = true;
        
       
         if (cAddFormDate.GetDate() == null && cAddtoDate.GetDate() != null)
         {
             flag = false;
         }
         if (cAddFormDate.GetDate() != null && cAddtoDate.GetDate() == null) {
             flag = false;
         }
         if (cEditFormDate.GetDate() == null && cEditToDate.GetDate() != null) {
             flag = false;
         }
         if (cEditFormDate.GetDate() != null && cEditToDate.GetDate() == null) {
             flag = false;
         }
         if (cDeleteFormDate.GetDate() == null && cDeleteToDate.GetDate() != null) {
             flag = false;
         }
         if (cDeleteFormDate.GetDate() != null && cDeleteToDate.GetDate() == null) {
             flag = false;
         }
         $("#hdnCalcommitProductId").val("Single");
            e.processOnServer = flag;
     }


     function ValidAllfrezeefromCheck(s,e)
     {
         cAllToDate.SetMinDate(cAllFromDate.GetDate());
         if (cAllToDate.GetDate() < cAllFromDate.GetDate()) {
             cAllToDate.Clear();
         }
     }

     function ValidDeletefromCheck(s,e)
     {
         cDeleteToDate.SetMinDate(cDeleteFormDate.GetDate());
         if (cDeleteToDate.GetDate() < cDeleteFormDate.GetDate()) {
             cDeleteToDate.Clear();
         }
     }
     function ValidEditfromCheck(s, e) {
         cEditToDate.SetMinDate(cEditFormDate.GetDate());
         if (cEditToDate.GetDate() < cEditFormDate.GetDate()) {
             cEditToDate.Clear();
         }
     }

     function ValidAddfromCheck(s, e) {
         cAddtoDate.SetMinDate(cAddFormDate.GetDate());
         if (cAddtoDate.GetDate() < cAddFormDate.GetDate()) {
             cAddtoDate.Clear();
         }
     }


     function ModuleButnClick(s, e) {
         $('#ProdModel').modal('show');
        // ShowModuleDetails();
     }

     function Module_KeyDown(s, e) {
         if (e.htmlEvent.key == "Enter") {
             $('#ProdModel').modal('show');
            // ShowModuleDetails();
         }
     }

     function Modulekeydown(e) {
         var OtherDetails = {}
         //if ($.trim($("#txtProdSearch").val()) == "" || $.trim($("#txtProdSearch").val()) == null) {
         //    return false;
         //}
         OtherDetails.SearchKey = $("#txtProdSearch").val();

         if (e.code == "Enter" || e.code == "NumpadEnter") {
             var HeaderCaption = [];

             HeaderCaption.push("Module Name");
           

            // if ($("#txtProdSearch").val() != "") {
                 callonServerM("TransactionLockConfigouration.aspx/GetModule", OtherDetails, "ProductTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "ProductSource");
             //}
         }
         else if (e.code == "ArrowDown") {
             if ($("input[dPropertyIndex=0]"))
                 $("input[dPropertyIndex=0]").focus();
         }
     }

     function SetSelectedValues(Id, Name, ArrName) {

         if (ArrName == 'ProductSource') {
             var key = Id;
             var Product_id = 0;
             var loopStock_id = 0;

             var adddat = key.split(',');
             var sproducts_id;
             var stock_id;
             if (key != null && key != '') {

                 for (var p = 0; p < adddat.length; p++) {
                     var Prodid = adddat[p].split("||@||");
                     Product_id = Prodid[0];
                     loopStock_id = Prodid[1];
                     if (p == 0) {
                         sproducts_id = Product_id
                         stock_id = loopStock_id;
                     }
                     else {
                         sproducts_id += "," + Product_id
                         stock_id += "," + loopStock_id
                     }
                 }


                 $('#ProdModel').modal('hide');
                 ctxtModule.SetText(Name);
                 GetObjectID('hdnCalcommitProductId').value = sproducts_id;
                 GetObjectID('hdncWiseProductId').value = sproducts_id;
                 var d = sproducts_id.split(',');
                 if(d.length==1)
                 {
                     Modulechanged();
                 }
                 else
                 {
                     cAddFormDate.Clear();
                     cAddtoDate.Clear();
                     cEditFormDate.Clear();
                     cEditToDate.Clear();
                     cDeleteFormDate.Clear();
                     cDeleteToDate.Clear();
                 }

             }
             else {
                 ctxtModule.SetText('');
                 GetObjectID('hdncWiseProductId').value = '';
                 GetObjectID('hdnCalcommitProductId').value = '';
             }
         }

     }

     function ShowModuleDetails() {
         var OtherDetails = {}
         //if ($.trim($("#txtProdSearch").val()) == "" || $.trim($("#txtProdSearch").val()) == null) {
         //    return false;
         //}
         OtherDetails.SearchKey = $("#txtProdSearch").val();

         var HeaderCaption = [];
         HeaderCaption.push("Module Name");
      

         callonServerM("TransactionLockConfigouration.aspx/GetModule", OtherDetails, "ProductTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "ProductSource");

     }

     function DataSaved()
     {
         cPopup_AllFreeze.Hide();
         cPopup_MopduleWise.Hide();
         jAlert("Data Inserted successfully.","Alert", function (){
             $("#hdncWiseProductId").val('');
        
             $("#hdnCalcommitProductId").val('');
             cPopup_AllFreeze.Hide();
             cPopup_MopduleWise.Hide();
             window.location.href = "/OMS/Management/Master/TransactionLockConfigouration.aspx";
         });
        
         
     }

     function DeleteModule()
     {
         $("#hdncWiseProductId").val('');
         ctxtModule.SetText("");
         $("#hdnCalcommitProductId").val('');
         cAddFormDate.Clear();
         cAddtoDate.Clear();
         cEditFormDate.Clear();
         cEditToDate.Clear();
         cDeleteFormDate.Clear();
         cDeleteToDate.Clear();
     }

     function GetCheckBoxValue(value) {
         //var value = s.GetChecked();
         if (value == true) {
             $("#hdncWiseProductId").val('All');
             ctxtModule.SetText("ALL");
             $("#hdnCalcommitProductId").val('All');
             ctxtModule.SetEnabled(false);

         } else {
             $("#hdncWiseProductId").val('');
             ctxtModule.SetText("");
             $("#hdnCalcommitProductId").val('');
             ctxtModule.SetEnabled(true);
         }
     }


     function grid_EndCallBack(s, e) {
       
     }
     function gridRowclick(s, e) {
         $('#GrdQuotation').find('tr').removeClass('rowActive');
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
    <style>
        .cntr{
            border: 1px solid #ccc;
            padding-bottom: 7px;
            margin: 0;
            margin-top: 10px;
            border-radius: 5px;
        }
        .cntr .hdr {
            background: #7093de;
            padding: 4px 0;
            margin: 0;
            border-radius: 2px 2px 0 0;
            margin-bottom: 3px;
            font-weight: 500;
            color: #fff;
            text-transform: uppercase;
        }
        .cntr .hdr.edit {
            background: #65bda5;
        }
        .cntr .hdr.delete {
            background: #ff6060;
        }
    </style>



</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-header clearfix">
        <div class="panel-title">
            <h3>
                <asp:Label ID="lblHeadTitle" runat="server">Data Freeze</asp:Label>
            </h3>
        </div>
    </div>
    <%--<div class="form_main">
        <div style="margin-top:10px;">
            <div class="row">
                <div class="col-md-2">
                <%--    <label>
                        <dxe:ASPxLabel ID="lblModSelect" runat="server" Text="Select Module: " CssClass="inline">
                        </dxe:ASPxLabel>
 
                    </label>--%>


<%--                      <div style="margin-top: 5px;">
                                   <dxe:ASPxLabel ID="dxelblProject" ClientInstanceName="cdxelblProject" runat="server" Text="Select Module: ">
                                    </dxe:ASPxLabel>

                            <dxe:ASPxCheckBox runat="server" ID="ChkAllProduct" ClientInstanceName="cChkAllProduct" ToolTip="All Select">
                                        <ClientSideEvents CheckedChanged="function(s, e) { 
                                                        GetCheckBoxValue(s.GetChecked()); 
                                                    }" />
                             </dxe:ASPxCheckBox>
                               <a href="#" style="left: -12px; top: 20px;"><%--onclick="AddcustomerClick()"--%>

                          <%--  <i id="I1" runat="server" class="fa fa-trash" aria-hidden="true" onclick="DeleteModule()"></i></a>--%>

                 <%--   </div>--%>
                 <%--   <dxe:ASPxComboBox ID="cmbModule" runat="server" Width="100%" ClientInstanceName="ccmbModule" Font-Size="12px">
                         <ClientSideEvents SelectedIndexChanged="Modulechanged" />
                    </dxe:ASPxComboBox>--%>
                   <%--   <dxe:ASPxButtonEdit ID="txtModule" runat="server" ReadOnly="true" ClientInstanceName="ctxtModule" Width="100%" TabIndex="5">
                            <Buttons>
                                <dxe:EditButton>
                                </dxe:EditButton>
                            </Buttons>
                            <ClientSideEvents ButtonClick="function(s,e){ModuleButnClick();}" KeyDown="function(s,e){Module_KeyDown(s,e);}" />
                     </dxe:ASPxButtonEdit>--%>
              <%--                            
                </div>
            </div>
            --%>
       <%--     <div class="row cntr" runat="server" id="dvtxtAdd">
            <%--    <div class="row mTop5 hdr" runat="server" id="dvlblAdd">
                    <div class="col-md-4">
                       <%-- <button class="btn btn-primary">Add</button>--%>
                    <%--    <asp:Label runat="server">Add</asp:Label>--%>
                   
                 <%--   </div>
                </div>--%>
               <%-- <div class="col-md-2">
                    <label>From Date</label>

                    <div>
                        <dxe:ASPxDateEdit ID="AddFormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cAddFormDate" Width="100%">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                             <ClientSideEvents DateChanged="ValidAddfromCheck" />
                             <ClientSideEvents GotFocus="function(s,e){cAddFormDate.ShowDropDown();}" />
                        </dxe:ASPxDateEdit>
                    </div>
                </div>--%>
               <%-- <div class="col-md-2">
                    <label>To Date</label>
                    <div>
                        <dxe:ASPxDateEdit ID="AddtoDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cAddtoDate" Width="100%">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                             <ClientSideEvents GotFocus="function(s,e){cAddtoDate.ShowDropDown();}" />
                        </dxe:ASPxDateEdit>
                    </div>
                </div>--%>
          <%--  </div>--%>
            
         <%--   <div class="row cntr" runat="server" id="dvtxtEdit">--%>
           <%--     <div class="row mTop5 hdr edit" runat="server" id="dvlblEdit">
                    <div class="col-md-4">
                       <%-- <button class="btn btn-primary">Edit</button>--%>
                         <%-- <asp:Label runat="server">Edit</asp:Label>--%>
                    <%--</div>
                </div>--%>
             <%--   <div class="col-md-2">
                   

                    <label>From Date</label>
                    <div>
                        <dxe:ASPxDateEdit ID="EditFormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cEditFormDate" Width="100%">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                            <ClientSideEvents DateChanged="ValidEditfromCheck" />
                             <ClientSideEvents GotFocus="function(s,e){cEditFormDate.ShowDropDown();}" />
                        </dxe:ASPxDateEdit>
                    </div>
                </div>--%>
               <%-- <div class="col-md-2">
                    <label>To Date</label>
                    <div>
                        <dxe:ASPxDateEdit ID="EditToDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cEditToDate" Width="100%">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                             <ClientSideEvents GotFocus="function(s,e){cEditToDate.ShowDropDown();}" />
                        </dxe:ASPxDateEdit>
                    </div>
                </div>--%>
          <%--  </div>--%>
            
      <%--      <div class="row cntr" runat="server" id="dvtxtDelete">
                <div class="row mTop5 hdr delete" runat="server" id="dvlbldelete">
                    <div class="col-md-4">
                       <%-- <button class="btn btn-primary">Delete</button>--%>
                    <%--    <asp:Label runat="server">Delete</asp:Label>
                    </div>
                </div>
                <div class="col-md-2">
                    <label>From Date</label>
                    <div>
                        <dxe:ASPxDateEdit ID="DeleteFormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cDeleteFormDate" Width="100%">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                               <ClientSideEvents DateChanged="ValidDeletefromCheck" />
                             <ClientSideEvents GotFocus="function(s,e){cDeleteFormDate.ShowDropDown();}" />
                        </dxe:ASPxDateEdit>
                    </div>
                </div>--%>
                <%--<div class="col-md-2">
                    <label>To Date</label>
                    <div>
                        <dxe:ASPxDateEdit ID="DeleteToDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cDeleteToDate" Width="100%">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                             <ClientSideEvents GotFocus="function(s,e){cDeleteToDate.ShowDropDown();}" />
                        </dxe:ASPxDateEdit>
                    </div>
                </div>--%>
        <%--    </div>
        </div>--%>
       <%-- <div  class="" style="padding-top:10px">
        <dxe:ASPxButton ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" ClientInstanceName="cbtnSave" AutoPostBack="false" UseSubmitBehavior="false" OnClick="Save_Click">
           
             <ClientSideEvents Click="fn_btnValidate" />
             <%--<ClientSideEvents Click="function(s,e){fn_btnValidate();}" />  --%>
     <%--   </dxe:ASPxButton>
            </div>--%>
    <%--</div>--%>

  <%--  <div class="modal fade" id="ProdModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Product Search</h4>
                </div>--%>
               <%-- <div class="modal-body">
                    <input type="text" onkeydown="Modulekeydown(event)" id="txtProdSearch" width="100%" placeholder="Search By Module Name" />
                    <div id="ProductTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">

                            <tr class="HeaderStyle" style="font-size: small">
                                <th>Select</th>
                                <th class="hide">Module_Id</th>
                                <th>Module Name</th>
                             
                            </tr>
                        </table>
                    </div>
                </div>--%>
             <%--   <div class="modal-footer">
                    <button type="button" class="btn btn-default" onclick="DeSelectAll('ProductSource')">Deselect All</button>
                    <button type="button" class="btn btn-default" data-dismiss="modal" onclick="OKPopup('ProductSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
              <%--  </div>
            </div>
        </div>
    </div>--%>
<%--      <asp:HiddenField ID="hdncWiseProductId" runat="server" />
    <asp:HiddenField ID="hdnCalcommitProductId" runat="server" />--%>
    <div class="form_main">
      <dxe:ASPxButton ID="btnFreeze" runat="server" Text="Freeze All" CssClass="btn btn-primary" ClientInstanceName="cbtnFreeze" AutoPostBack="false" UseSubmitBehavior="false">
             <ClientSideEvents Click="OpenEntryDate" />
      </dxe:ASPxButton>


      <dxe:ASPxButton ID="btnUnFreeze" runat="server" Text="UnFreeze All" CssClass="btn btn-danger" ClientInstanceName="cbtnUnFreeze" AutoPostBack="false" UseSubmitBehavior="false">
             <ClientSideEvents Click="OpenUnFreezeDate" />
      </dxe:ASPxButton>


     <div class="GridViewArea relative">
        <dxe:ASPxGridView ID="GrdQuotation" runat="server" KeyFieldName="Module_Id" AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true"
            Width="100%" ClientInstanceName="cGrdQuotation" 
            Settings-HorizontalScrollBarMode="Auto" SettingsBehavior-AutoExpandAllGroups="true" DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false" 
            SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="350" Settings-VerticalScrollBarMode="Auto">
            <SettingsSearchPanel Visible="True" Delay="5000" />
            <Columns>
                <dxe:GridViewDataTextColumn FieldName="Module_Id" Visible="false" Width="0">
                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Group" GroupIndex="0" FieldName="ParentName"
                    VisibleIndex="1" FixedStyle="Left" Width="250px" SortOrder="Ascending">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
              
                <dxe:GridViewDataTextColumn Caption="Module" FieldName="Module_Name"
                    VisibleIndex="2" Width="80%">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                   <dxe:GridViewDataTextColumn Caption="Add Freeze - From" FieldName="AddFromLock"
                    VisibleIndex="3" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                   <dxe:GridViewDataTextColumn Caption="Add Freeze - To" FieldName="AddToLock"
                    VisibleIndex="4" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                   <dxe:GridViewDataTextColumn Caption="Edit Freeze - From" FieldName="EditFromLock"
                    VisibleIndex="5" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                   <dxe:GridViewDataTextColumn Caption="Edit Freeze - To" FieldName="EditToLock"
                    VisibleIndex="6" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                   <dxe:GridViewDataTextColumn Caption="Delete Freeze - From" FieldName="DeleteFromLock"
                    VisibleIndex="7" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                   <dxe:GridViewDataTextColumn Caption="Delete Freeze - To" FieldName="DeleteToLock"
                    VisibleIndex="8" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                 <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="9" Width="20%">
                    <DataItemTemplate>
                         <div class=''>
                            <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%#Eval("Module_Id")%>')" class="pad" title="">
                             <img src="../../../assests/images/info.png" /></a>
                       </div>
                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <HeaderTemplate><span>Actions</span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                </dxe:GridViewDataTextColumn>

            </Columns>
           
            <SettingsContextMenu Enabled="true"></SettingsContextMenu>
            <ClientSideEvents EndCallback="grid_EndCallBack" RowClick="gridRowclick"  />
            <SettingsPager NumericButtonCount="100" PageSize="100" ShowSeparators="True" Mode="ShowPager">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="100,150,200" />
                <FirstPageButton Visible="True">
                </FirstPageButton>
                <LastPageButton Visible="True">
                </LastPageButton>
            </SettingsPager>
          

            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsLoadingPanel Text="Please Wait..." />
        </dxe:ASPxGridView>

        <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
            ContextTypeName="ERPDataClassesDataContext" TableName="v_LockModuleList" />
    
       
    </div>

    </div>
    <dxe:ASPxPopupControl ID="Popup_MopduleWise" runat="server" ClientInstanceName="cPopup_MopduleWise"
        Width="400px" HeaderText="Freeze Data" PopupHorizontalAlign="WindowCenter"
        BackColor="white" Height="150px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
    <div>
           <div class="row cntr" runat="server" id="dvtxtAdd">
                <div class="row mTop5 hdr" runat="server" id="dvlblAdd">
                    <div class="col-md-4">
                       <%-- <button class="btn btn-primary">Add</button>--%>
                        <asp:Label runat="server">Add</asp:Label>
                   
                    </div>
                </div>

               <div class="col-md-2">
                   <label>Freeze</label>

                    <dxe:ASPxCheckBox runat="server" ID="chkAdd" ClientInstanceName="cchkAdd" ToolTip="Select Add">
                                      <%--  <ClientSideEvents CheckedChanged="function(s, e) { 
                                                        GetCheckBoxValue(s.GetChecked()); 
                                                    }" />--%>
                </dxe:ASPxCheckBox>

               </div>

                <div class="col-md-5">
                    <label>From Date</label>

                    <div>
                        <dxe:ASPxDateEdit ID="AddFormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cAddFormDate" Width="100%">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                             <ClientSideEvents DateChanged="ValidAddfromCheck" />
                             <ClientSideEvents GotFocus="function(s,e){cAddFormDate.ShowDropDown();}" />
                        </dxe:ASPxDateEdit>
                    </div>
                </div>
              
                <div class="col-md-5">
                    <label>To Date</label>
                    <div>
                        <dxe:ASPxDateEdit ID="AddtoDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cAddtoDate" Width="100%">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                             <ClientSideEvents GotFocus="function(s,e){cAddtoDate.ShowDropDown();}" />
                        </dxe:ASPxDateEdit>
                    </div>
                </div>
            </div>
            
            <div class="row cntr" runat="server" id="dvtxtEdit">
                <div class="row mTop5 hdr edit" runat="server" id="dvlblEdit">
                    <div class="col-md-4">
                       <%-- <button class="btn btn-primary">Edit</button>--%>
                          <asp:Label runat="server">Edit</asp:Label>
                    </div>
                </div>

             <div class="col-md-2">
                   <label>Freeze</label>

                    <dxe:ASPxCheckBox runat="server" ID="chkEdit" ClientInstanceName="cchkEdit" ToolTip="Select Edit">
                                   <%--     <ClientSideEvents CheckedChanged="function(s, e) { 
                                                        GetCheckBoxValue(s.GetChecked()); 
                                                    }" />--%>
                </dxe:ASPxCheckBox>

               </div>

                <div class="col-md-5">
                   

                    <label>From Date</label>
                    <div>
                        <dxe:ASPxDateEdit ID="EditFormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cEditFormDate" Width="100%">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                            <ClientSideEvents DateChanged="ValidEditfromCheck" />
                             <ClientSideEvents GotFocus="function(s,e){cEditFormDate.ShowDropDown();}" />
                        </dxe:ASPxDateEdit>
                    </div>
                </div>
              
                <div class="col-md-5">
                    <label>To Date</label>
                    <div>
                        <dxe:ASPxDateEdit ID="EditToDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cEditToDate" Width="100%">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                             <ClientSideEvents GotFocus="function(s,e){cEditToDate.ShowDropDown();}" />
                        </dxe:ASPxDateEdit>
                    </div>
                </div>
            </div>
            
            <div class="row cntr" runat="server" id="dvtxtDelete">
                <div class="row mTop5 hdr delete" runat="server" id="dvlbldelete">
                    <div class="col-md-4">
                       <%-- <button class="btn btn-primary">Delete</button>--%>
                        <asp:Label runat="server">Delete</asp:Label>
                    </div>
                </div>

            <div class="col-md-2">
                   <label>Freeze</label>

                    <dxe:ASPxCheckBox runat="server" ID="chkDelete" ClientInstanceName="cchkDelete" ToolTip="Select DElete">
                                       <%-- <ClientSideEvents CheckedChanged="function(s, e) { 
                                                        GetCheckBoxValue(s.GetChecked()); 
                                                    }" />--%>
                </dxe:ASPxCheckBox>

               </div>

                <div class="col-md-5">
                    <label>From Date</label>
                    <div>
                        <dxe:ASPxDateEdit ID="DeleteFormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cDeleteFormDate" Width="100%">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                               <ClientSideEvents DateChanged="ValidDeletefromCheck" />
                             <ClientSideEvents GotFocus="function(s,e){cDeleteFormDate.ShowDropDown();}" />
                        </dxe:ASPxDateEdit>
                    </div>
                </div>
            

                <div class="col-md-5">
                    <label>To Date</label>
                    <div>
                        <dxe:ASPxDateEdit ID="DeleteToDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cDeleteToDate" Width="100%">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                             <ClientSideEvents GotFocus="function(s,e){cDeleteToDate.ShowDropDown();}" />
                        </dxe:ASPxDateEdit>
                    </div>
                </div>
            </div>
     </div>
        <div  class="" style="padding-top:10px">
            <dxe:ASPxButton ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" ClientInstanceName="cbtnSave" AutoPostBack="false" UseSubmitBehavior="false" OnClick="Save_Click">
                 <ClientSideEvents Click="fn_btnValidate" />
                <%--OnClick="Save_Click"--%>
           </dxe:ASPxButton>

            <dxe:ASPxButton ID="btnCancel" runat="server" Text="Close" CssClass="btn btn-danger" ClientInstanceName="cbtnCancel" AutoPostBack="false" UseSubmitBehavior="false">
                <ClientSideEvents Click="CloseData" />
             </dxe:ASPxButton>
         </div>
  

            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />


    </dxe:ASPxPopupControl>


    
    <dxe:ASPxPopupControl ID="Popup_AllFreeze" runat="server" ClientInstanceName="cPopup_AllFreeze"
        Width="400px" HeaderText="All Freeze Data" PopupHorizontalAlign="WindowCenter"
        BackColor="white" Height="150px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">

                 <div class="row" runat="server" id="Div1">
                
                <div class="col-md-6">
                    <label>From Date</label>
                    <div>
                        <dxe:ASPxDateEdit ID="AllFromDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cAllFromDate" Width="100%">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                               <ClientSideEvents DateChanged="ValidAllfrezeefromCheck" />
                             <ClientSideEvents GotFocus="function(s,e){cAllFromDate.ShowDropDown();}" />
                        </dxe:ASPxDateEdit>
                    </div>
                </div>
                <div class="col-md-6">
                    <label>To Date</label>
                    <div>
                        <dxe:ASPxDateEdit ID="AllToDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cAllToDate" Width="100%">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                             <ClientSideEvents GotFocus="function(s,e){cAllToDate.ShowDropDown();}" />
                        </dxe:ASPxDateEdit>
                    </div>
                </div>
            </div>

       <div  class="" style="padding-top:10px">
            <dxe:ASPxButton ID="AllbtnSave" runat="server" Text="Save" CssClass="btn btn-primary" ClientInstanceName="cAllbtnSave" AutoPostBack="false" UseSubmitBehavior="false" OnClick="AllSave_Click">
       
             <ClientSideEvents Click="fn_AllbtnValidate" />  <%--OnClick="AllSave_Click"--%>
            
           </dxe:ASPxButton>
            <dxe:ASPxButton ID="AllbtnCancel" runat="server" Text="Close" CssClass="btn btn-danger" ClientInstanceName="cAllbtnCancel" AutoPostBack="false" UseSubmitBehavior="false">
                <ClientSideEvents Click="AllCloseData" />
            
           </dxe:ASPxButton>
         </div>

            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />


    </dxe:ASPxPopupControl>

          <asp:HiddenField ID="hdncWiseProductId" runat="server" />
    <asp:HiddenField ID="hdnCalcommitProductId" runat="server" />
      <asp:HiddenField ID="hdnMasterFromdate" runat="server" />
    <asp:HiddenField ID="hdnMasterToDate" runat="server" />
</asp:Content>
