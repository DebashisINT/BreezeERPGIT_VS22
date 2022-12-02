<%@ Page Title="Rplacement Notes" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"
    CodeBehind="ReplacementstocksList.aspx.cs" Inherits="ERP.OMS.Management.Activities.ReplacementstocksList" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        strong label {
            font-weight: bold !important;
        }

        input[type="radio"] {
            webkit-transform: translateY(3px);
            -moz-transform: translateY(3px);
            transform: translateY(3px);
        }

        .blink {
            animation: blink-animation 1s steps(5, start) infinite;
            -webkit-animation: blink-animation 1s steps(5, start) infinite;
            cursor: pointer;
            color: #128AC9;
        }

        @keyframes blink-animation {
            to {
                visibility: hidden;
            }
        }

        @-webkit-keyframes blink-animation {
            to {
                visibility: hidden;
            }
        }
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

        function OnMoreInfoClick(keyValue,stock) {

            if (stock == 'In') {
                var url = 'ReplacementNotestockin.aspx?keyM=' + keyValue;
                window.location.href = url;

            }

            if (stock == 'Out') {
                var url = 'ReplacementNotestockout.aspx?keyM=' + keyValue;
                window.location.href = url;

            }
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


    </script>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-heading">
        <div class="panel-title">
            <h3>Replacement Note</h3>
        </div>
    </div>
    <div class="form_main">
        <div class="clearfix">
        
          

        </div>
    </div>


    <div class="GridViewArea">
        <dxe:ASPxGridView ID="GrdReplacement" runat="server" KeyFieldName="Replacement_Id" AutoGenerateColumns="False"
            Width="100%" ClientInstanceName="cGridreplacement" OnCustomCallback="GrdReplacement_CustomCallback" OnCustomColumnDisplayText="gridReplacement_CustomColumnDisplayText">
            <Columns>

                <%--   <dxe:GridViewDataCheckColumn Caption="#" VisibleIndex="0" > </dxe:GridViewDataCheckColumn>--%>


                <dxe:GridViewDataTextColumn Caption="Sl" UnboundType="String"
                    VisibleIndex="1" FixedStyle="Left" Width="4%">
                    <Settings AllowAutoFilter="False"
                        AllowSort="False" />
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>




                <dxe:GridViewDataTextColumn Caption="Date" FieldName="CreatedDate"
                    VisibleIndex="2" FixedStyle="Left" Width="10%">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>





                <dxe:GridViewDataTextColumn Caption="Replacement Note Number" FieldName="Replacement_Number"
                    VisibleIndex="3" FixedStyle="Left" Width="15%">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>



                <dxe:GridViewDataTextColumn Caption="Customer" FieldName="Customer"
                    VisibleIndex="4" FixedStyle="Left" Width="25%">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>



                <dxe:GridViewDataTextColumn Caption="Sales Invoice Number" FieldName="InvoiceNumber"
                    VisibleIndex="5" FixedStyle="Left" Width="30%">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Sales Invoice Date" FieldName="InvoiceDate"
                    VisibleIndex="6" FixedStyle="Left" Width="30%">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="UserCreate"
                    VisibleIndex="7" FixedStyle="Left" Width="10%">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>





                <dxe:GridViewDataTextColumn Caption="Last Updated By" FieldName="UserUpdate"
                    VisibleIndex="9" FixedStyle="Left" Width="10%">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>



                <dxe:GridViewDataTextColumn Caption="Last Updated On" FieldName="ModifiedDate"
                    VisibleIndex="10" FixedStyle="Left" Width="10%">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="11" Width="15%">
                    <DataItemTemplate>
                     
                        <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>',<%#keystokinorout %>)" class="pad" title="Edit">
                            <img src="../../../assests/images/info.png" /></a>


                     
                      
                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <HeaderTemplate><span>Actions</span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                </dxe:GridViewDataTextColumn>
            </Columns>
            <ClientSideEvents />



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
                        <label>Assign Branch:<span style="color: red">*</span></label>
                        <div>
                            <dxe:ASPxComboBox ID="CmbBranch" EnableIncrementalFiltering="True" ClientInstanceName="cCmbBranch"
                               
                                    runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                            </dxe:ASPxComboBox>
                            <span id="MandatoryAssignBranch" style="display: none">
                            <img id="gridHistory_DXPEForm_efnew_DXEFL_DXEditor1234_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                        </div>
                    </div>
                    <div>
                        <label style="margin-top:8px">Remark:</label>
                        <div><dxe:ASPxMemo ID="txtAssignBranchremark" runat="server" Width="100%" Height="50px" ClientInstanceName="CtxtAssignBranchremark"></dxe:ASPxMemo></div>
                    </div>
                    <div style="padding-top: 15px;text-align: center;">
                        <input id="btnAssignBranchSave" class="btn btn-primary" onclick="CallAssignBranch_save()" type="button" value="Assign"/>
                        <input id="btnAssignBranchCancel" class="btn btn-danger" onclick='cPopup_AssignBranch.Hide();' type="button" value="Cancel"/>
                    </div>
                    


                </div>

            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />


    </dxe:ASPxPopupControl>

    </div>



</asp:Content>


