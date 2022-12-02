<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RepxReportViewer.aspx.cs" EnableEventValidation="false" Inherits="Reports.Reports.REPXReports.RepxReportViewer" %>

<%@ Register Assembly="DevExpress.XtraReports.v15.1.Web, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%--<%@ Register Assembly="DevExpress.XtraCharts.v15.1.Web, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraCharts.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts" TagPrefix="dx" %>--%>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dxe" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">    
    <link rel="stylesheet" type="text/css" href="/assests/fonts/font-awesome/css/font-awesome.min.css" />  
    <%--<script type="text/javascript" src="https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.12.4.min.js"></script> --%>
    <script src="/assests/js/jquery-1.12.4.min.js"></script>
    <script src="../../assests/bootstrap/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="/assests/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" href="/assests/css/custom/main.css" /> 
    <link href="../GridReports/CSS/SearchPopup.css" rel="stylesheet" />
    <script src="../GridReports/JS/SearchPopup.js"></script>
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <link href="../../assests/pluggins/choosen/choosen.min.css" rel="stylesheet" />
      <style type="text/css">
        .chosen-container.chosen-container-multi {
            width: 100% !important;
        }

        .receiptIndex_0{
        width:362px;
        }

        .chosen-choices {
            width: 100% !important;
        }
        .modal
        {
           z-index: 99999;
        }
          @media screen and (min-width: 768px) 
          {
            .modal-dialog {
            right: auto; left: auto !important;
            }
          }       
         
    </style>

    <script type="text/javascript">

        function ValueSelected(e, indexName) {
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                if (indexName == "receiptIndex") {
                    var Id = e.target.parentElement.parentElement.cells[0].innerText;
                    var name = e.target.parentElement.parentElement.cells[1].children[0].value;
                    if (Id) {
                        SetRecipients(Id, name);
                    }
                    ctxtTo.Focus();
                }
            }
            else if (e.code == "ArrowDown") {
                thisindex = parseFloat(e.target.getAttribute(indexName));
                thisindex++;
                if (thisindex < 10)
                    $("input[" + indexName + "=" + thisindex + "]").focus();
                //ctxtTo.Focus();
            }
            else if (e.code == "ArrowUp") {
                thisindex = parseFloat(e.target.getAttribute(indexName));
                thisindex--;
                if (thisindex > -1)
                    $("input[" + indexName + "=" + thisindex + "]").focus();
                else {
                    $('#txtRecipientsSearch').focus();
                    //ctxtTo.Focus();
                }
            }
        }

        function itemClik(s, e) {

            if (e.item.name == "CustomEmail") {
                //DocViewer.SaveToWindow('pdf');                
                //cCallbackPanel.PerformCallback(e.item.name);               
                //ctxtTo.SetText("");
                //lstItems.SetText("");                              
                //ctxtFrom.SetText("");                
                //ctxtBody.SetText("");
                DocViewer.cpErrorResult = "";
                cPopup_EMail.Show();
                BindFromAddress();
                ctxtTo.SetText("");
                ctxtCc.SetText("");
                ctxtSubject.SetText("");
                document.getElementById("txtMailBody").value = '';
                ctxtTo.Focus();
            }
        }

        function BindFromAddress() {
            $.ajax({
                type: "POST",
                url: "RepxReportViewer.aspx/GetFromEmail",

                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    ctxtFrom.SetText(list);
                    
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        }

        function CallSend(s, e) {
            //var values = '';
            //var listBox = document.getElementById("lstItems");
            //for (var i = 0; i < listBox.options.length; i++) {
            //    if (listBox.options[i].selected) {
            //        values += listBox.options[i].innerHTML + " " + listBox.options[i].value + "\n";
            //    }
            //}
            //alert(values);

            //var ListValue = '';

            //var techGroups = document.getElementById("lstItems");
            //for (var i = 0; i < techGroups.options.length; i++) {
            //    if (techGroups.options[i].selected == true) {
            //        if (ListValue == '') {
            //            ListValue = techGroups.options[i].value;
            //        }
            //        else {
            //            ListValue = ListValue + ',' + techGroups.options[i].value;
            //        }
            //    }
            //}

            //document.getElementById("hndRecipients_hidden").value = ListValue;

            //if (ListValue == "") {
           
            if (validateEmail(ctxtCc.GetValue()) || ctxtCc.GetValue() == null) {
                  if (ctxtTo.GetValue() == null) {
                    alert('Please specify at least one recipient.');
                }
                else {
                      cCallbackPanel.PerformCallback();                     
                }
            }
            else
            {
                //ctxtCc.validateEmail();
                var CcAdd = ctxtCc.GetValue();
                alert('The address ' + CcAdd + ' in the Cc field was not recognized. Please make sure that all addresses are properly formed.');
            }
        }

        function SinglevalidateEmail(singleEmail) {
            var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            return regex.test(singleEmail);
        }

        function validateEmail(emails) {
            if (emails != null) {
                var res = emails.split(",");
                for (i = 0; i < res.length; i++)
                    if (res[i] != "")
                        if (!SinglevalidateEmail(res[i]))
                            return false;
                return true;
            }
            else {
                return true;
            }
        }

        function CallbackPanelEndCall(s, e) {

            if (DocViewer.cpErrorResult) {
                if (DocViewer.cpErrorResult != "") {
                    alert(DocViewer.cpErrorResult);
                    DocViewer.cpErrorResult = null;
                }
            }
            else {
                alert("Mail sent successfully.");
                cPopup_EMail.Hide();
            }
        }

        function closePopup(s, e) {
            e.cancel = false;
            ctxtFrom.SetValue("");
            ctxtTo.SetValue("");
            ctxtCc.SetValue("");
            ctxtSubject.SetValue("");
            document.getElementById("txtMailBody").value = '';
        }

        $(document).keydown(function (e) {
            if (e.keyCode == 27) {
                cPopup_EMail.Hide();
                //cShowGrid.Focus();
                DocViewer.Focus();
            }
        });

        function CancelSend() {
            cPopup_EMail.Hide();
        }
        
        //function ListAssignTo() {
        //    $('#lstItems').chosen();
        //    $('#lstItems').fadeIn();
        //}

        //function BindRecipients() {
        //    var lBox = $('select[id$=lstItems]');
        //    var listItems = [];
        //    lBox.empty();

        //    $.ajax({
        //        type: "POST",
        //        url: "RepxReportViewer.aspx/GetRecipients",
               
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: function (msg) {                    
        //            var list = msg.d;                    
        //            if (list.length > 0) {
        //                for (var i = 0; i < list.length; i++) {
        //                    //var id = '';
        //                    //var name = '';
        //                    //id = list[i].split('|')[1];
        //                    //name = list[i].split('|')[0];

        //                    //listItems.push('<option value="' +
        //                    //id + '">' + name
        //                    //+ '</option>');

        //                    var id = '';
        //                    var email = '';
        //                    id = list[i].split('|')[1];
        //                    email = list[i].split('|')[0];

        //                    listItems.push('<option value="' +
        //                    email + '">' + email
        //                    + '</option>');
        //                }

        //                $(lBox).append(listItems.join(''));


        //                //setWithFromBind();
        //                //ListBind();
        //                ListAssignTo();
        //                $('select.chsn').hide();
        //                $('#lstItems').trigger("chosen:updated");
        //                $('#lstItems').prop('disabled', false).trigger("chosen:updated");
        //            }
        //            else {
        //                lBox.empty();
        //                $('#lstItems').trigger("chosen:updated");
        //                $('#lstItems').prop('disabled', true).trigger("chosen:updated");

        //            }
        //        },
        //        error: function (XMLHttpRequest, textStatus, errorThrown) {
        //            alert(textStatus);
        //        }
        //    });
        //}

        function closeOnClick() {
            window.location.href = '/Reports/REPXReports/RepxReportMain.aspx?reportname=' + document.getElementById('HDRepornName').value;
        }

        //function OnClickEmail(s, e) {
        <%--if (e.item != null) {
                if (e.item.name == 'CustomPrn')
            {
                //alert(e.item.name);
                ASPxDocumentViewer1.Print();
            }
            else if (e.item.name == 'CustomExp')
                //ReportViewer1.SaveToWindow('pdf');
                ASPxCallback1.PerformCallback(e.item.name);
            }--%>
        //    alert('fire');
        //if (e.item.name == 'Email') {
        //    ReportViewer1.SaveToWindow('pdf');
        //    cbtnEmail.PerformCallback(e.item.name);
        // }
        //}
        //}

        //$(function () {
        //    $("#lstItems").focus(function () {
        //        BindRecipients();
        //    });
        //});
    </script>
    <%--For Recipients Single Selection--%>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#RecipientsModel').on('shown.bs.modal', function () {
                $('#txtRecipientsSearch').focus();
            })

            var ReportModuleName = document.getElementById('HDRepornName').value;
            if (ReportModuleName == "TRIALONNETBALSUMARY" || ReportModuleName == "STATEMENTOFACCOUNT") {
                $('.crossBtn').hide();
            }
            else {
                $('.crossBtn').show();
            }
        })
        function RecipientsButnClick(s, e) {
            $('#RecipientsModel').modal('show');
        }

        function Recipients_KeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                $('#RecipientsModel').modal('show');
            }
        }

        function Recipientskeydown(e) {
            var OtherDetails = {}

            if ($.trim($("#txtRecipientsSearch").val()) == "" || $.trim($("#txtRecipientsSearch").val()) == null) {
                return false;
            }
            OtherDetails.SearchKey = $("#txtRecipientsSearch").val();
            //OtherDetails.BranchID = hdnSelectedBranches.value;

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Email");

                if ($("#txtRecipientsSearch").val() != "") {
                    callonServer("../GridReports/Services/Master.asmx/GetRecipientsBind", OtherDetails, "RecipientsTable", HeaderCaption, "receiptIndex", "SetRecipients");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[receiptIndex=0]"))
                    $("input[receiptIndex=0]").focus();
            }
        }

        function SetRecipients(Id, Name) {
            var key = Name;
            if (key != null && key != '') {
                $('#RecipientsModel').modal('hide');
                ctxtTo.SetText(Name);
                //GetObjectID('hndSelectRecipients').value = key;
                document.getElementById("hndSelectRecipients").value = key;
            }
            else {
                ctxtTo.SetText('');
                //GetObjectID('hndSelectRecipients').value = '';
                document.getElementById("hndSelectRecipients").value = '';
            }
        }

        //Rev Debashis 0024342
        //function docprintinit(s, e) {
        //    var createFrameElement = s.viewer.printHelper.createFrameElement;
        //    s.viewer.printHelper.createFrameElement = function (name) {
        //        var frameElement = createFrameElement.call(this, name);
        //        if (ASPx.Browser.Chrome) {
        //            frameElement.addEventListener("load", function (e) {
        //                if (frameElement.contentDocument.contentType !== "text/html")
        //                    frameElement.contentWindow.print();
        //            });
        //        }
        //        return frameElement;
        //    }
        //    s.Print();
        //}

        function onPrintClick(s,e)
        {
            var createFrameElement = s.viewer.printHelper.createFrameElement;
            s.viewer.printHelper.createFrameElement = function (name) {
                var frameElement = createFrameElement.call(this, name);
                if (ASPx.Browser.Chrome) {
                    frameElement.addEventListener("load", function (e) {
                        if (frameElement.contentDocument.contentType !== "text/html")
                            frameElement.contentWindow.print();
                    });
                }
                return frameElement;
            }
            s.Print();
        }
        //End of Rev Debashis 0024342

    </Script>
    <%--For Recipients Single Selection--%>

       
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="crossBtn" style="top: 41px; color: #ccc"><a href="#" onclick="closeOnClick()"><i class="fa fa-times"></i></a></div>
        </div>
        <%--<dx:ASPxDocumentToolbar runat="server" ID="aspxDocumentToolbar1" ReportViewerID="ASPxDocumentViewer1"></dx:ASPxDocumentToolbar>--%>
        <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
            <PanelCollection>
                <dxe:PanelContent runat="server">
                    <dx:ASPxDocumentViewer ID="ASPxDocumentViewer1" runat="server" ClientInstanceName="DocViewer">
                        <%--Rev Debashis 0024342--%>
                        <%--<ClientSideEvents Init="function(s, e) { 
                               s.Print();
                           }" />--%>
                        <%--<ClientSideEvents Init="docprintinit"/>--%>
                        <%--End of Rev Debashis 0024342--%>
                        <ToolbarItems>
                            <dx:ReportToolbarButton ItemKind="Search" />
                            <dx:ReportToolbarSeparator />
                            <%--Rev Debashis 0024342--%>
                           <%-- <dx:ReportToolbarButton ItemKind="PrintReport" />
                            <dx:ReportToolbarButton ItemKind="PrintPage" />--%>
                            <dx:ReportToolbarButton Name="Print" Text="" ToolTip="Print" Enabled="true" IconID="actions_print_16x16devav"/>
                            <%--End of Rev Debashis 0024342--%>
                            <dx:ReportToolbarSeparator />
                            <dx:ReportToolbarButton Enabled="False" ItemKind="FirstPage" />
                            <dx:ReportToolbarButton Enabled="False" ItemKind="PreviousPage" />
                            <dx:ReportToolbarLabel ItemKind="PageLabel" />
                            <dx:ReportToolbarComboBox ItemKind="PageNumber" Width="65px">
                            </dx:ReportToolbarComboBox>
                            <dx:ReportToolbarLabel ItemKind="OfLabel" />
                            <dx:ReportToolbarTextBox IsReadOnly="True" ItemKind="PageCount" />
                            <dx:ReportToolbarButton ItemKind="NextPage" />
                            <dx:ReportToolbarButton ItemKind="LastPage" />
                            <dx:ReportToolbarSeparator />
                            <dx:ReportToolbarButton ItemKind="SaveToDisk" />
                            <dx:ReportToolbarButton ItemKind="SaveToWindow" />
                            <dx:ReportToolbarComboBox ItemKind="SaveFormat" Width="70px">
                                <Elements>
                                    <dx:ListElement Value="pdf" />
                                    <dx:ListElement Value="xls" />
                                    <dx:ListElement Value="xlsx" />
                                    <dx:ListElement Value="rtf" />
                                    <dx:ListElement Value="mht" />
                                    <dx:ListElement Value="html" />
                                    <dx:ListElement Value="txt" />
                                    <dx:ListElement Value="csv" />
                                    <dx:ListElement Value="png" />
                                </Elements>
                            </dx:ReportToolbarComboBox>

                            <%--<dx:ReportToolbarButton IconID="mail_mail_16x16" Name="Email" ToolTip="Email" />--%>
                            <dx:ReportToolbarButton ItemKind="Custom" Name="CustomEmail" ToolTip="Email" Enabled="true" IconID="mail_mail_16x16"></dx:ReportToolbarButton>
                            <%--Rev Debashis 0024342--%>
                            <%--<dx:ReportToolbarButton ItemKind="Custom" Name="CustomPrint" ToolTip="Print" Enabled="true" IconID="actions_print_16x16devav"></dx:ReportToolbarButton>--%>
                            <%--End of Rev Debashis 0024342--%>
                        </ToolbarItems>
                        <ClientSideEvents ToolbarItemClick="itemClik" />
                        <%--Rev Debashis 0024342--%>
                        <%--<ClientSideEvents ToolbarItemClick="onPrintClick" />--%>
                         <ClientSideEvents ToolbarItemClick="function(s, e) {
	                                            if(e.item.name == 'Print') {
                                                onPrintClick(s, e);
                                                }}" />
                        <%--End of Rev Debashis 0024342--%>
                    </dx:ASPxDocumentViewer>
                </dxe:PanelContent>
            </PanelCollection>
            <%--<ClientSideEvents EndCallback="CallbackPanelEndCall" />--%>
            <ClientSideEvents EndCallback="CallbackPanelEndCall"/>
        </dxe:ASPxCallbackPanel>

        <dxe:ASPxPopupControl ID="Popup_EMail" runat="server" ClientInstanceName="cPopup_EMail"
            Width="500px" HeaderText="Send Mail" PopupHorizontalAlign="WindowCenter"
            BackColor="white" Height="150px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True" ContentStyle-CssClass="pad">
            <ClientSideEvents Closing="function(s, e) {
	    closePopup(s, e);}" />
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <div class="Top clearfix">
                        <table style="width: 100%; margin: 0 auto; margin-top: 5px;">
                            <tr>
                                <td>
                                    <label>
                                        <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="From :">
                                        </dxe:ASPxLabel>
                                    </label>

                                    <dxe:ASPxTextBox ID="txtFrom" ClientInstanceName="ctxtFrom" runat="server" Width="100%" ClientEnabled="false">                                        
                                    </dxe:ASPxTextBox>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <label>
                                        <dxe:ASPxLabel ID="ASPxLabel12" runat="server" Text="To :">
                                        </dxe:ASPxLabel>
                                    </label>

                                    <%--<dxe:ASPxTextBox ID="txtTo" ClientInstanceName="ctxtTo" runat="server" Width="100%">
                                    </dxe:ASPxTextBox>--%>
                                    <div>                                       
                                        <%--<asp:ListBox ID="lstItems" runat="server" SelectionMode="Single" Font-Size="12px" Height="90px" Width="100%" CssClass="mb0 chsn " data-placeholder="Select ...">--%>
                                            <%--<asp:ListItem Text="CC" Value="3"></asp:ListItem>--%>
                                       <%-- </asp:ListBox>--%>
                                        <dxe:ASPxButtonEdit ID="txtTo" runat="server" ReadOnly="true" ClientInstanceName="ctxtTo" Width="100%" TabIndex="1">
                                            <Buttons>
                                                <dxe:EditButton>
                                                </dxe:EditButton>
                                            </Buttons>
                                            <ClientSideEvents ButtonClick="function(s,e){RecipientsButnClick();}" KeyDown="function(s,e){Recipients_KeyDown(s,e);}" />
                                        </dxe:ASPxButtonEdit>
                                        <asp:HiddenField ID="hndSelectRecipients" runat="server" />
                                    </div>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <label>
                                        <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="Cc :">
                                        </dxe:ASPxLabel>
                                    </label>
                                    <dxe:ASPxTextBox ID="txtCc" ClientInstanceName="ctxtCc" runat="server" Width="100%" AutoCompleteType="Email">
                                        <%--<ValidationSettings ErrorDisplayMode="Text" Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="true">
                                            <RegularExpression ErrorText="" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                        </ValidationSettings>--%>
                                    </dxe:ASPxTextBox>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <label style="margin-top: 6px">
                                        <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Subject :">
                                        </dxe:ASPxLabel>
                                    </label>
                                    <dxe:ASPxTextBox ID="txtSubject" ClientInstanceName="ctxtSubject" runat="server" Width="100%">
                                    </dxe:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label style="margin-top: 6px">
                                        <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Body :">
                                        </dxe:ASPxLabel>
                                    </label>
                                    <%--<dxe:ASPxTextBox ID="txtBody" ClientInstanceName="ctxtBody" runat="server" Width="100%" TextMode="Multiline">
                                    </dxe:ASPxTextBox>--%>

                                    <asp:TextBox ID="txtMailBody" runat="server" Width="100%" TextMode="MultiLine" Height="150">
                                    </asp:TextBox>

                                </td>
                            </tr>
                        </table>
                        <div style="margin-top: 10px;">
                            <input id="btnSend" class="btn btn-primary" onclick="CallSend()" type="button" value="Send"  />
                          
                            <input id="btnCancel" class="btn btn-danger" onclick="CancelSend()" type="button" value="Cancel" />
                        </div>

                    </div>

                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>   
        
        <!--Recipients Modal -->
        <div class="modal fade" id="RecipientsModel" role="dialog">
            <div class="modal-dialog">
                <!-- Recipients content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Recipients Search</h4>
                    </div>
                    <div class="modal-body">
                        <input type="text" onkeydown="Recipientskeydown(event)" id="txtRecipientsSearch" autofocus width="100%" placeholder="Search By Email" />
                        <div id="RecipientsTable">
                            <table border='1' width="100%" class="dynamicPopupTbl">
                                <tr class="HeaderStyle">
                                    <th class="hide">ID</th>
                                    <th>Email</th>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <!--Recipients Modal -->     

        <%-- <dx:ASPxButton ID="btnEmail" runat="server" OnCallback="btnEmail_Callback"></dx:ASPxButton>--%>

        <%-- <dxe:ASPxButton ID="btnEmail" ClientInstanceName="cbtnEmail" runat="server" AutoPostBack="false">
            <ClientSideEvents Click="function(s, e) {
                                    OnClickEmail();
                            }" >
                </ClientSideEvents>
        </dxe:ASPxButton>--%>
        <asp:HiddenField ID="StartDate" runat="server" />
        <asp:HiddenField ID="EndDate" runat="server" />
        <asp:HiddenField ID="HDRepornName" runat="server" />
    </form>
</body>
</html>
