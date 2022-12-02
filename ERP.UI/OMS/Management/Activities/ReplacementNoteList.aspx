<%@ Page Title="Rplacement Notes" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"
    CodeBehind="ReplacementNoteList.aspx.cs" Inherits="ERP.OMS.Management.Activities.ReplacementNoteList" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        
    </style>

    <script>
        function onPrintJv(id) {
            window.location.href = "../../reports/XtraReports/Viewer/PurchaseInvoiceReportViewer.aspx?id=" + id;
        }
        function OpenPopUPUserWiseQuotaion() {
            cgridUserWiseQuotation.PerformCallback();
            cPopupUserWiseQuotation.Show();
        }
        document.onkeydown = function (e) {
            if (event.keyCode == 18) isCtrl = true;
            if (event.keyCode == 65 && isCtrl == true) { //run code for Ctrl+S -- ie, Save & New  
                StopDefaultAction(e);
                OnAddButtonClick();
            }
        }

        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };

            e.returnValue = false;
            e.stopPropagation();
        }



        function OnClickDelete(keyValue) {
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    cGridreplacement.PerformCallback('Delete~' + keyValue);
                    cGridreplacement.PerformCallback('Display');
                }
            });
        }




        function OnClickStatus(keyValue) {
            GetObjectID('hiddenedit').value = keyValue;
            cGrdQuotation.PerformCallback('Edit~' + keyValue);
        }

        function OpenPopUPApprovalStatus() {
            cgridPendingApproval.PerformCallback();
            cpopupApproval.Show();
        }



        function grid_EndCallBack() {
            if (cGrdQuotation.cpEdit != null) {
                GetObjectID('hiddenedit').value = cGrdQuotation.cpEdit.split('~')[0];
                cProforma.SetText(cGrdQuotation.cpEdit.split('~')[1]);
                cCustomer.SetText(cGrdQuotation.cpEdit.split('~')[4]);
                var pro_status = cGrdQuotation.cpEdit.split('~')[2]
                //cGrdQuotation.cpEdit = null;
                if (pro_status != null) {
                    var radio = $("[id*=rbl_QuoteStatus] label:contains('" + pro_status + "')").closest("td").find("input");
                    radio.attr("checked", "checked");
                    //return false;
                    //$('#rbl_QuoteStatus[type=radio][value=' + pro_status + ']').prop('checked', true); 
                    cQuotationRemarks.SetText(cGrdQuotation.cpEdit.split('~')[3]);

                    cQuotationStatus.Show();
                }
            }
            if (cGrdQuotation.cpUpdate != null) {
                GetObjectID('hiddenedit').value = '';
                cProforma.SetText('');
                cCustomer.SetText('');
                cQuotationRemarks.SetText('');
                var pro_status = 2;
                if (pro_status != null) {
                    var radio = $("[id*=rbl_QuoteStatus] label:contains('" + pro_status + "')").closest("td").find("input");
                    radio.attr("checked", "checked");
                    cQuotationStatus.Hide();
                }
                jAlert(cGrdQuotation.cpUpdate);
            }
            if (cGrdQuotation.cpDelete != null) {
                jAlert(cGrdQuotation.cpDelete);
                cGrdQuotation.cpDelete = null;
                cGridreplacement.Refresh();
            }


        }


        function SavePrpformaStatus() {
            if (document.getElementById('hiddenedit').value == '') {
                cGrdQuotation.PerformCallback('save~');
            }
            else {
                var checked_radio = $("[id*=rbl_QuoteStatus] input:checked");
                var status = checked_radio.val();
                var remarks = cQuotationRemarks.GetText();
                cGrdQuotation.PerformCallback('update~' + GetObjectID('hiddenedit').value + '~' + status + '~' + remarks);
            }

        }

        function OnMoreInfoClick(keyValue) {

            var url = 'ReplacementNote.aspx?key=' + keyValue + '&type=REPLACE';
            window.location.href = url;


        }



        function OnclickViewAttachment(obj) {
            var URL = '/OMS/Management/Activities/PurchaseInvoice_Document.aspx?idbldng=' + obj + '&type=PurchaseInvoice';
            window.location.href = URL;
        }




        function OnAddButtonClick() {
            var url = 'ReplacementNote.aspx?key=' + 'ADD';
            window.location.href = url;
        }
        var keyval;


        function GetApprovedQuoteId(s, e, itemIndex) {
            var rowvalue = cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetApprovedRowValues);

            cgridPendingApproval.PerformCallback('Status~' + rowvalue);
            cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetApprovedRowValues);

        }
        function OnGetApprovedRowValues(obj) {
            uri = "PurchaseInvoice.aspx?key=" + obj + "&status=2";
            popup.SetContentUrl(uri);
            popup.Show();


        }

        function ch_fnApproved() {
        }


        function GetRejectedQuoteId(s, e, itemIndex) {
            debugger;
            cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetRejectedRowValues);

        }
        function OnGetRejectedRowValues(obj) {
            uri = "PurchaseInvoice.aspx?key=" + obj + "&status=3";
            popup.SetContentUrl(uri);
            popup.Show();
        }


        function OnApprovalEndCall(s, e) {
            $.ajax({
                type: "POST",
                url: "purchaseinvoicelist.aspx/GetPendingCase",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
             
                }
            });
            }



            function OnEditButtonClick(ReplacementId, current_status, assign_to_branch, receiver_remark, assignee_remark) {
                debugger;
                $("#hidden_replacementId").val(ReplacementId);
                $('#MandatoryAssignBranch').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');

                if (current_status == 0) {
                    cPopup_AssignBranch.Show();
                    CtxtAssignBranchremark.SetValue();
                    cCmbBranch.SetValue();

                    CtxtAssignBranchremark.SetEnabled(true);
                    cCmbBranch.SetEnabled(true);
                    $("#btnAssignBranchSave").attr("disabled", false);
                    $("#btnAssignBranchCancel").attr("disabled", false);
                }
                else if (current_status == 1 || current_status == 2) {
                    cPopup_AssignBranch.Show();
                    CtxtAssignBranchremark.SetValue(assignee_remark);
                    cCmbBranch.SetValue(assign_to_branch);

                    CtxtAssignBranchremark.SetEnabled(false);
                    cCmbBranch.SetEnabled(false);
                    $("#btnAssignBranchSave").attr("disabled", true);
                    $("#btnAssignBranchCancel").attr("disabled", true);
                }
                else {
                    //do nothing
                }
            }



            function CallAssignBranch_save() {
                var flag = true;
                var Remarks = CtxtAssignBranchremark.GetValue();
                var branch = cCmbBranch.GetValue();

                if (branch == "" || branch == null) {
                    $('#MandatoryAssignBranch').attr('style', 'display:block;position: absolute;right: 13px;top: 35px;');
                    flag = false;
                }
                else {
                    $('#MandatoryAssignBranch').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');
                    cPopup_AssignBranch.Hide();

                    var tbl_trans_oldunit = {
                        assign_to_branch: branch,
                        assignee_remark: Remarks,
                        Replacement_Id: $("#hidden_replacementId").val(),
                        current_status: 1
                    }

                    $.ajax({
                        type: "POST",
                        url: "ReplacementNoteList.aspx/SaveAssignedBranch",
                        data: JSON.stringify({ 'assignbranch': tbl_trans_oldunit }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            CtxtAssignBranchremark.SetValue();
                            cCmbBranch.SetValue()

                            jAlert(data.d);

                            cGridreplacement.PerformCallback('Display');

                        },
                        failure: function (response) {
                            jAlert("Error");
                        }
                    });
                }
                return flag;
            }
            function gridRowclick(s, e) {
                $('#GrdReplacement').find('tr').removeClass('rowActive');
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
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cGridreplacement.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cGridreplacement.SetWidth(cntWidth);
            }
            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cGridreplacement.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cGridreplacement.SetWidth(cntWidth);
                }

            });
        });
    </script>
    <link href="CSS/ReplacementNoteList.css" rel="stylesheet" />
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-heading">
        <div class="panel-title">
            <h3>Replacement Note</h3>
        </div>
    </div>
    <div class="form_main">
        <div class="clearfix">
              <% if (rights.CanAdd)
                                   { %>



            <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-success btn-radius">
                <span class="btn-icon"><i class="fa fa-plus" ></i></span>
                <span><u>A</u>dd New</span> </a>


                  <%} %>



                  <% if (rights.CanExport)
               { %>
            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>
             <% } %>


        </div>
    </div>


    <div class="GridViewArea relative">
        <dxe:ASPxGridView ID="GrdReplacement" runat="server" KeyFieldName="Replacement_Id" AutoGenerateColumns="False"
            Width="100%" ClientInstanceName="cGridreplacement" OnCustomCallback="GrdReplacement_CustomCallback"
             OnCustomColumnDisplayText="gridReplacement_CustomColumnDisplayText"
               OnDataBinding="gridReplacement_DataBinding" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto" 
            >
            <SettingsSearchPanel Visible="True" Delay="5000" />
            <Columns>


                <dxe:GridViewDataTextColumn Caption="Sl" UnboundType="String"
                    VisibleIndex="1" FixedStyle="Left" Width="4%">
                    <Settings AllowAutoFilter="False"
                        AllowSort="False" />
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>




                <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="CreatedDate"
                    VisibleIndex="2" FixedStyle="Left" Width="10%">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>





                <dxe:GridViewDataTextColumn Caption="Document Number" FieldName="Replacement_Number"
                    VisibleIndex="3" FixedStyle="Left" Width="15%">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>



                <dxe:GridViewDataTextColumn Caption="Customer" FieldName="Customer"
                    VisibleIndex="4" FixedStyle="Left" Width="20%">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>



                <dxe:GridViewDataTextColumn Caption="Sales Invoice Number" FieldName="InvoiceNumber"
                    VisibleIndex="5" FixedStyle="Left" Width="15%">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Sales Invoice Date" FieldName="InvoiceDate"
                    VisibleIndex="6" FixedStyle="Left" Width="15%">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="UserCreate"
                    VisibleIndex="7" FixedStyle="Left" Width="10%">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>





                <dxe:GridViewDataTextColumn Caption="Updated By" FieldName="UserUpdate"
                    VisibleIndex="9" FixedStyle="Left" Width="10%">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>



                <dxe:GridViewDataTextColumn Caption="Updated On" FieldName="ModifiedDate"
                    VisibleIndex="10" FixedStyle="Left" Width="10%">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn FixedStyle="Left"  VisibleIndex="11" Width="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <DataItemTemplate>

                        <div class='floatedBtnArea'>
                            <% if (rights.CanEdit)
                               { %>

                            <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')"  title="">
                                <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>
                            <%} %>

                            <% if (rights.CanDelete)
                               { %>
                            <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')"  title="">
                                <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>


                            <%} %>

                            <% if (rights.CanAssignbranch)
                               { %>
                            <a href="javascript:void(0);" onclick="OnEditButtonClick('<%# Container.KeyValue %>', '<%# Eval("current_status") %>', '<%# Eval("assign_to_branch") %>', '<%# Eval("receiver_remark") %>', '<%# Eval("assignee_remark") %>')" title="" >
                                <span class='ico ColorSix'><i class='fa fa-truck out'></i></span><span class='hidden-xs'>Assigned Branch</span>
                            </a>
                            </a><%} %>
                            <% if (rights.CanPrint)
                               { %>
                            <a href="javascript:void(0);" onclick="onPrintJv('<%# Container.KeyValue %>')" class="" title="" style="display: none;">
                                <span class='ico ColorSeven'><i class='fa fa-print'></i></span><span class='hidden-xs'>Print</span>
                            </a><%} %>
                        </div>
                    </DataItemTemplate>
                 
                    <HeaderTemplate><span>Actions</span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                </dxe:GridViewDataTextColumn>
            </Columns>
            <SettingsContextMenu Enabled="true"></SettingsContextMenu>
            <ClientSideEvents RowClick="gridRowclick" />



            <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                <FirstPageButton Visible="True">
                </FirstPageButton>
                <LastPageButton Visible="True">
                </LastPageButton>
            </SettingsPager>


            <SettingsSearchPanel Visible="True" />
            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsLoadingPanel Text="Please Wait..." />


        </dxe:ASPxGridView>

        <asp:HiddenField ID="hiddenedit" runat="server" />
        <asp:HiddenField ID="hidden_replacementId" runat="server" />

    </div>





    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" GridViewID="GrdQuotation" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>



    <div id="divAssigntobranch">

        <dxe:ASPxPopupControl ID="Popup_AssignBranch" runat="server" ClientInstanceName="cPopup_AssignBranch"
            Width="400px" HeaderText="Assign Branch" PopupHorizontalAlign="WindowCenter"
            BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <div class="Top clearfix">
                        <div>
                            <label>Assign Unit:<span style="color: red">*</span></label>
                            <div>
                                <dxe:ASPxComboBox ID="CmbBranch" EnableIncrementalFiltering="True" ClientInstanceName="cCmbBranch"
                                    runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                </dxe:ASPxComboBox>
                                <span id="MandatoryAssignBranch" style="display: none">
                                    <img id="gridHistory_DXPEForm_efnew_DXEFL_DXEditor1234_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                            </div>
                        </div>
                        <div>
                            <label style="margin-top: 8px">Remark:</label>
                            <div>
                                <dxe:ASPxMemo ID="txtAssignBranchremark" runat="server" Width="100%" Height="50px" ClientInstanceName="CtxtAssignBranchremark"></dxe:ASPxMemo>
                            </div>
                        </div>
                        <div style="padding-top: 15px; text-align: center;">
                            <input id="btnAssignBranchSave" class="btn btn-primary" onclick="CallAssignBranch_save()" type="button" value="Assign" />
                            <input id="btnAssignBranchCancel" class="btn btn-danger" onclick='cPopup_AssignBranch.Hide();' type="button" value="Cancel" />
                        </div>



                    </div>

                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />


        </dxe:ASPxPopupControl>

    </div>



</asp:Content>


