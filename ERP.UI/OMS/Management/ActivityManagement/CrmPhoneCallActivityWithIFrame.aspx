<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.ActivityManagement.management_activitymanagement_CrmPhoneCallActivityWithIFrame" CodeBehind="CrmPhoneCallActivityWithIFrame.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <script language="javascript" type="text/javascript">


        $(document).ready(function () {
            $("#<%=btnCancel.ClientID %>").click(function () {
                var url = '<%= Session["CrmPhoneCallActivityUrl"] %>';
                window.location.href = url;
                return false;
            });
        });
        function callforms(val) {

            if (val == "AssignedTo") {
                globval = "AssignedTo"
                var frm = "UserList.aspx";
                var Title = "List Of User";

                // 13122016comment this line by sanjib due to value was coming null and giving error.

                //OnMoreInfoClick(frm, Title, '950px', '450px', "Y");
                //end
            }
            if (val == "AddLead") {
                // var val123 =document.getElementById("f1").contentWindow.GetUserList(); //document.getElementById("txtUserList").value;
                var val123 = document.getElementById("txtUserList");
                if (val123 != "") {
                    globval = "AddLead"
                    var frm = 'frm_AddLead.aspx?Call=PhoneCall&user=' + val123;
                    var Title = "Add Lead";
                    // 13122016comment this line by sanjib due to value was coming null and giving error.
                    // OnMoreInfoClick(frm, Title, '950px', '450px', "Y");
                    //end
                }
                else {
                    alert('Assign to Can Not Be Blank');
                }
            }
            if (val == "PostpondMeeting") {
                //  var obj = document.getElementById("f1").contentWindow.GetLeadId();
                var obj = document.getElementById("txtLeadId");
                if (obj.value != "") {

                    globval = "PostpondMeeting";
                    var Title = "Postpone Meeting";
                    var frm = 'frmupdateMeeting.aspx?id=' + obj.value;
                    OnMoreInfoClick(frm, Title, '950px', '450px', "Y");
                }
                else {
                    alert('Select a lead/customer')
                    var frm = "";
                }
            }
            if (val == "PostpondMeetingSales") {
                //var obj = document.getElementById("f1").contentWindow.GetLeadId1();
                var obj = document.getElementById("txtLeadId1");
                if (obj.value != "") {
                    globval = "AddLead";
                    var Title = "Postpone Meeting";
                    var frm = 'frmupdateMeeting.aspx?id=' + obj.value;
                    OnMoreInfoClick(frm, Title, '950px', '450px', "Y");
                }
                else {
                    alert('Select a lead/customer')
                    var frm = "";
                }
            }
            if (val == "CancelMeeting") {
                //var obj = document.getElementById("f1").contentWindow.GetLeadId();
                var obj = document.getElementById("txtLeadId");
                if (obj.value != "") {
                    globval = "CancelMeeting";
                    var Title = "Cancel Meeting";
                    var frm = 'frmCancelMeeting.aspx?id=' + obj.value;
                    OnMoreInfoClick(frm, Title, '950px', '450px', "Y");
                }
                else {
                    alert('Select a lead/customer')
                    var frm = "";
                }
            }
            if (val == "CancelMeetingSales") {
                // var obj = document.getElementById("f1").contentWindow.GetLeadId1();
                var obj = document.getElementById("txtLeadId1");
                if (obj.value != "") {
                    globval = "CancelMeetingSales";
                    var Title = "Cancel Meeting";
                    var frm = 'frmCancelMeeting.aspx?id=' + obj.value;
                    OnMoreInfoClick(frm, Title, '950px', '450px', "Y");
                }
                else {
                    alert('Select a lead/customer')
                    var frm = "";
                }
            }
            if (val == "Export") {
                globval = "Export";
                var frm = "frmmessage_history.aspx"
                var Title = "Export Files";
                OnMoreInfoClick(frm, Title, '950px', '450px', "Y");
            }
            if (val == "AllotSales") {
                //var st = document.getElementById("f1").contentWindow.GetLeadId();
                var st = document.getElementById("txtLeadId");
                if (st.value != "") {
                    globval = "AllotSales";
                    var Title = "Allotment";
                    frm = 'frmAllot_new.aspx?Calling=PhoneCall&id=' + st.value;
                    OnMoreInfoClick(frm, Title, '950px', '450px', "Y");
                }
                else {
                    alert('Select a lead/customer')
                    var frm = "";
                }
            }
            if (val == "Allot") {
                var st = document.getElementById("txtLeadId1");
                if (st.value != "") {
                    globval = "Allot";
                    var Title = "Allotment";
                    var frm = 'frmAllot_sales_new.aspx?Calling=PhoneCall&id=' + st.value;
                    OnMoreInfoClick(frm, Title, '950px', '450px', "Y");
                }
                else {
                    alert('Select a lead/customer')
                    var frm = "";
                }
            }

            if (frm != "") {
                editwin = dhtmlmodal.open("Editbox", "iframe", frm, Title, "width=950px,height=500px,center=1,resize=1,scrolling=2,top=500", "recal");
            }
            // added by sanjib on 13/12/2016 for enable the update button on phone call activity
            $("#btnSubmit").removeClass("aspNetDisabled");
            //end

            //  editwin.onclose=function()

        }

        function callback() {

            //document.getElementById("ctl00_ContentPlaceHolder1_Headermain1_cmbSegment").style.display ='inline';
            if (globval == "PostpondMeeting") {

                GetShow();
            }
            if (globval == "CancelMeeting") {

                GetShow();
            }
            if (globval == "CancelMeetingSales") {

                GetShowSales();
            }
            if (globval == "Allot") {

                GetShowSales();
            }
            if (globval == "AllotSales") {

                GetShow();
            }
            if (globval == "PostpondMeetingSales") {

                GetShowSales();
            }

        }

        function SetParent() {
            SetDisable();
        }
        function testparent() {

            var a = document.getElementById("txtUserList");
            return a;

        }

        function testparent1() {
            //            var b=document.getElementById("f1").contentWindow.GetHiddenUserList();
            //            return b;
            var b = document.getElementById("hd1UserList");
            return b;


        }

        //###############################################################################//
        function UserList() {
            callforms("AssignedTo");
        }
        function frmOpenNewWindow1(location, v_height, v_weight) {
            var x = (screen.availHeight - v_height) / 2;
            var y = (screen.availWidth - v_weight) / 2
            window.open(location, "Search_Conformation_Box", "height=" + v_height + ",width=" + v_weight + ",top=" + x + ",left=" + y + ",location=no,directories=no,menubar=no,toolbar=no,status=yes,scrollbars=yes,resizable=no,dependent=no'");
        }

        function fun123() {
            callforms("AddLead");

        }
        function checkButton() {
            document.getElementById("Button1").disabled = false;
        }
        function FillLeadId123(val) {
            var ob = document.getElementById("txtLeadId")
            ob.value = val;
        }
        function FillLeadId1234(val) {
            var ob = document.getElementById("txtLeadId1")
            ob.value = val;
        }
        function FillLeadId(obj, val) {
            var ob = document.getElementById("txtLeadId")
            if (ob.value == null) {
                if (obj.checked == true) {
                    ob.value = val + ','
                }
            }
            else {
                if (obj.checked == true) {
                    ob.value = ob.value + val + ','
                }
                else {
                    var st = ob.value.split(",")
                    //ob.value = null;
                    var tt = ''
                    ob.value = tt;
                    for (var i = 0; i < st.length; i++) {
                        if (st[i] == val) {

                        }
                        else {
                            if (st[i] == tt) {
                            }
                            else {
                                ob.value = ob.value + st[i] + ',';
                            }
                        }
                    }
                }
            }
        }

        function FillLeadId1(obj, val) {
            //    alert(obj+','+val);
            var ob = document.getElementById("txtLeadId1");
            //        alert('lead'+','+ob);
            if (ob.value == null) {
                if (obj.checked == true) {
                    ob.value = val + ','
                }
            }
            else {
                if (obj.checked == true) {
                    ob.value = ob.value + val + ','
                }
                else {
                    var st = ob.value.split(",")
                    //ob.value = null;
                    var tt = ''
                    ob.value = tt;
                    for (var i = 0; i < st.length; i++) {
                        if (st[i] == val) {

                        }
                        else {
                            if (st[i] == tt) {
                            }
                            else {
                                ob.value = ob.value + st[i] + ',';
                            }
                        }
                    }
                }
            }
        }


        function fun(obj, str) {
            document.getElementById("drpProduct").disabled = str;
        }
        function fun1(obj, str) {
            document.getElementById("drpSalesProduct").disabled = str;
        }
        function windowopenform() {

            callforms("AllotSales");
            //        var st = document.getElementById("txtLeadId")
            //        frmOpenNewWindow1('frmAlloat.aspx?id=' + st.value ,500,500);
        }
        function windowopenform123() {

            //        var st = document.getElementById("txtLeadId1")
            //        frmOpenNewWindow1('frmAllot_sales.aspx?id=' + st.value ,500,500);
            callforms("Allot");
        }
        function javascr() {
            var tdate = document.getElementById("txtToDate");
            var fdate = document.getElementById("txtFromDate");
            tdate.value = fdate.value;
            //        alert(tdate.value);
            //        alert(fdate.value);
        }
        function funCheckFunction() {
            callforms("PostpondMeeting");
        }
        function funCheckFunctionSales() {
            callforms("PostpondMeetingSales");
        }

        function GetLeadId() {
            var st = document.getElementById("txtLeadId");
            return st;
        }
        function GetLeadId1() {

            var st = document.getElementById("txtLeadId1");
            return st;
        }

        function funSaveClick() {
            callforms("CancelMeeting");

        }
        function CallExport() {
            callforms("Export");
        }
        function funSaveClick1() {
            callforms("CancelMeeting");
        }
        function funSaveClickSales() {
            callforms("CancelMeetingSales");
        }
        //    function frmOpenNewWindow_custom(location,v_height,v_weight,top,left)
        //    {   
        //       window.open(location,"Search_Conformation_Box","height="+ v_height +",width="+ v_weight +",top="+ top +",left="+ left +",location=no,directories=no,menubar=no,toolbar=no,status=no,scrollbars=yes,resizable=no,dependent=no'");       
        //    } 
        function TextVal() {
            var btn = document.getElementById("btnGenratedSalesVisit");
            btn.click();
            var btn1 = document.getElementById("btnshowGenratedSaleVisit");
            btn1.click();
        }
        //function height() {
        //    //         alert(document.body.scrollHeight);
        //    if (document.body.scrollHeight > 385) {
        //        window.frameElement.height = document.body.scrollHeight;
        //    }
        //    else {
        //        window.frameElement.height = "385";
        //    }
        //    window.frameElement.Width = document.body.scrollWidth;
        //}
        function GetUserList() {

            var ob = document.getElementById("txtUserList");
            return ob;

            //     var ob1=document.getElementById("hd1UserList");
        }
        function GetHiddenUserList() {
            var ob2 = document.getElementById("hd1UserList");
            return ob2;
        }

        function SetDisable() {
            document.getElementById('Button1').disabled = true;
            document.getElementById('txtUserList').disabled = true;
            document.getElementById('btnSubmit').disabled = false;
        }
        function GetShow() {

            document.getElementById("btnshowGenratedSaleVisit").click();
        }
        function GetShowSales() {
            document.getElementById("btnShowSales").click();
        }
        function InVisibilityForSales() {
            document.getElementById("Button5").style.display = 'none';
            document.getElementById("Button6").style.display = 'none';
        }
        function VisibilityForSales() {
            document.getElementById("Button5").style.display = 'inline';
            document.getElementById("Button6").style.display = 'inline';
        }
        function InVisibilityForSalesVisit() {
            document.getElementById("btnPostPendingMeetings").style.display = 'none';
            document.getElementById("btnCancelMeeting").style.display = 'none';
        }
        function VisibilityForSalesVisit() {
            document.getElementById("btnPostPendingMeetings").style.display = 'inline';
            document.getElementById("btnCancelMeeting").style.display = 'inline';
        }

        //function SignOff() {
        //    window.parent.SignOff()
        //}
        $(document).ready(function () {
            $('#<%=Label12.ClientID %>').html('Start Date And Time (Phone Calls):');
            $('#<%=Label13.ClientID %>').html('End Date And Time (Phone Calls):');


            $('#<%=drpActType.ClientID %>').change(function () {
                $('#<%=Label12.ClientID %>').html('Start Date And Time (' + $("#<%=drpActType.ClientID %> option:selected").text() + '):');
                $('#<%=Label13.ClientID %>').html('End Date And Time (' + $("#<%=drpActType.ClientID %> option:selected").text() + '):');
                return false;
            })

        });

        function btnSubmit_clientClick() {
          
            var flag = true;          
            
            var sdate = TxtStartDate.GetValue();
            var edate = TxtEndDate.GetValue();

            var startDate = new Date(sdate);
            var endDate = new Date(edate);


            if (sdate != null && sdate != '' && edate != null && edate != '') {
                if (startDate > endDate) {
                    flag = false;
                    $('#MandatoryEgSDate').attr('style', 'display:block');
                }
            }
            else { $('#MandatoryEgSDate').attr('style', 'display:none'); }

            return flag;
        }

    </script>
    <style>
        #MandatoryEgSDate {
            position: absolute;
            right: -20px;
            top: 8px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Add Sales Activity(<asp:Literal ID="ltrActivity" runat="server" Text=''></asp:Literal>)</h3>
            <div id="btncross" class="crossBtn" style="margin-left: 50px;"><a href="<%= Session["CrmPhoneCallActivityUrl"] %>"><i class="fa fa-times"></i></a></div>
        </div>

    </div>
    <div class="form_main" style="border: 1px solid #ccc; padding: 10px 15px;">
        <div class="pull-left">
            <%--<a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>--%>

            <% if (rights.CanExport)
                                               { %>
            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>

            </asp:DropDownList>
              <% } %>
        </div>

        <table class="TableMain100">
            <tr style="display: none">
                <td class="lt">
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnCreate" runat="server" Text="Create" CssClass="btn btn-primary" OnClick="btnCreate_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnModify" runat="server" Text="Modify" CssClass="btn btn-primary" OnClick="btnModify_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnGenratedSalesVisit" runat="server" Text="Generated Sales Visit"
                                    CssClass="btn btn-primary" OnClick="btnGenratedSalesVisit_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnGenratedSales" runat="server" Text="Genrated Sales" CssClass="btn btn-primary"
                                    OnClick="btnGenratedSales_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnCourtesyCall" runat="server" Text="Courtesy call" CssClass="btn btn-primary"
                                    OnClick="btnCourtesyCall_Click" />
                            </td>
                            <td style="width: 56px">
                                <asp:Button ID="btnCancel1" runat="server" Text="Cancel" CssClass="btn btn-danger" OnClick="btnCancel1_Click"
                                    Visible="False" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr runat="server" id="PnlBtn" visible="false">
                <td class="lt">
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnReassign" runat="server" Text="Reassign" CssClass="btn btn-primary"
                                    OnClick="btnReassign_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnReschedule" runat="server" Text="Reschedule" CssClass="btn btn-primary"
                                    OnClick="btnReschedule_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnShowDetail" runat="server" Text="Show Details" CssClass="btn btn-primary"
                                    OnClick="btnShowDetail_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnDelegate" runat="server" Text="Delegate To" CssClass="btn btn-primary"
                                    OnClick="btnDelegate_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

            <tr id="trgrid" runat="server">
                <td>
                    <asp:Panel ID="userInfo1" runat="server" Width="100%">
                        <%--  changed normal grid to dev grid by sanjib 13122016 --%>





                        <dxe:ASPxGridView ID="grdUserInfo" runat="server" ClientInstanceName="grid" KeyFieldName="UserId"
                            Width="100%" OnCustomCallback="grdUserInfo_CustomCallback"
                            AutoGenerateColumns="False">
                            <SettingsBehavior ConfirmDelete="True" AllowFocusedRow="false" />
                            <Styles>
                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                </Header>
                                <LoadingPanel ImageSpacing="10px">
                                </LoadingPanel>
                            </Styles>
                            <SettingsPager NumericButtonCount="20" Visible="true">
                            </SettingsPager>
                            <Columns>
                                <dxe:GridViewDataHyperLinkColumn Caption="User Id" FieldName="UserId" VisibleIndex="1" Width="15%" Visible="False">
                                    <DataItemTemplate>
                                        <a href='CrmPhoneCallActivityWithIFrame.aspx?id=<%#Eval("UserId") %>&amp;type=SW' style="cursor: pointer;"><%#Eval("User") %></a>
                                    </DataItemTemplate>
                                </dxe:GridViewDataHyperLinkColumn>
                                <dxe:GridViewDataHyperLinkColumn Caption="S.No" FieldName="SNo" VisibleIndex="2" Width="15%" Visible="False">
                                    <DataItemTemplate>

                                        <a href='CrmPhoneCallActivityWithIFrame.aspx?id=<%#Eval("UserId") %>&amp;type=SW' style="cursor: pointer;"><%#Eval("SNo") %></a>
                                    </DataItemTemplate>
                                </dxe:GridViewDataHyperLinkColumn>
                                <dxe:GridViewDataHyperLinkColumn Caption="User" FieldName="UserId" VisibleIndex="3" Width="15%">
                                    <DataItemTemplate>

                                        <a href='CrmPhoneCallActivityWithIFrame.aspx?id=<%#Eval("UserId") %>&amp;type=SW' style="cursor: pointer;"><%#Eval("User") %></a>
                                    </DataItemTemplate>
                                </dxe:GridViewDataHyperLinkColumn>
                                <dxe:GridViewDataHyperLinkColumn Caption="Pending Activity" FieldName="Pending Acttivity" VisibleIndex="4" Width="15%">
                                    <DataItemTemplate>

                                        <a href='CrmPhoneCallActivityWithIFrame.aspx?id=<%#Eval("UserId") %>&amp;type=SW' style="cursor: pointer;"><%#Eval("Pending Acttivity") %></a>
                                    </DataItemTemplate>
                                </dxe:GridViewDataHyperLinkColumn>
                                <dxe:GridViewDataHyperLinkColumn Caption="Scheduled End Date" FieldName="Scheduled End Date" VisibleIndex="5" Width="15%">
                                    <DataItemTemplate>

                                        <a href='CrmPhoneCallActivityWithIFrame.aspx?id=<%#Eval("UserId") %>&amp;type=SW' style="cursor: pointer;"><%#Eval("Scheduled End Date") %></a>
                                    </DataItemTemplate>
                                </dxe:GridViewDataHyperLinkColumn>
                                <dxe:GridViewDataHyperLinkColumn Caption="Expected End Date" FieldName="Expected End Date" VisibleIndex="6" Width="15%">
                                    <DataItemTemplate>

                                        <a href='CrmPhoneCallActivityWithIFrame.aspx?id=<%#Eval("UserId") %>&amp;type=SW' style="cursor: pointer;"><%#Eval("Expected End Date") %></a>
                                    </DataItemTemplate>
                                </dxe:GridViewDataHyperLinkColumn>
                                <dxe:GridViewDataHyperLinkColumn Caption="Pending Calls" FieldName="Pending Call" VisibleIndex="7" Width="15%">
                                    <DataItemTemplate>

                                        <a href='CrmPhoneCallActivityWithIFrame.aspx?id=<%#Eval("UserId") %>&amp;type=SW' style="cursor: pointer;"><%#Eval("Pending Call") %></a>
                                    </DataItemTemplate>
                                </dxe:GridViewDataHyperLinkColumn>
                                <dxe:GridViewDataHyperLinkColumn Caption="Call Back" FieldName="Call Back" VisibleIndex="8" Width="15%">
                                    <DataItemTemplate>

                                        <a href='CrmPhoneCallActivityWithIFrame.aspx?id=<%#Eval("UserId") %>&amp;type=SW' style="cursor: pointer;"><%#Eval("Call Back") %></a>
                                    </DataItemTemplate>
                                </dxe:GridViewDataHyperLinkColumn>
                                <dxe:GridViewDataHyperLinkColumn Caption="Non Contactable" FieldName="Non Contactable" VisibleIndex="9" Width="15%">
                                    <DataItemTemplate>

                                        <a href='CrmPhoneCallActivityWithIFrame.aspx?id=<%#Eval("UserId") %>&amp;type=SW' style="cursor: pointer;"><%#Eval("Non Contactable") %></a>
                                    </DataItemTemplate>
                                </dxe:GridViewDataHyperLinkColumn>
                                <dxe:GridViewDataHyperLinkColumn Caption="Non Usable" FieldName="Non Usable" VisibleIndex="10" Width="15%">
                                    <DataItemTemplate>

                                        <a href='CrmPhoneCallActivityWithIFrame.aspx?id=<%#Eval("UserId") %>&amp;type=SW' style="cursor: pointer;"><%#Eval("Non Usable") %></a>
                                    </DataItemTemplate>
                                </dxe:GridViewDataHyperLinkColumn>
                                <dxe:GridViewDataHyperLinkColumn Caption="Pipeline/Sales Visits" FieldName="Pipeline/Sales Visits" VisibleIndex="11" Width="15%">
                                    <DataItemTemplate>

                                        <a href='CrmPhoneCallActivityWithIFrame.aspx?id=<%#Eval("UserId") %>&amp;type=SW' style="cursor: pointer;"><%#Eval("Pipeline/Sales Visits") %></a>
                                    </DataItemTemplate>
                                </dxe:GridViewDataHyperLinkColumn>
                                <dxe:GridViewDataHyperLinkColumn Caption="Won/Confirm Sales" FieldName="Won/Confirm Sales" VisibleIndex="12" Width="15%">
                                    <DataItemTemplate>

                                        <a href='CrmPhoneCallActivityWithIFrame.aspx?id=<%#Eval("UserId") %>&amp;type=SW' style="cursor: pointer;"><%#Eval("Won/Confirm Sales") %></a>
                                    </DataItemTemplate>
                                </dxe:GridViewDataHyperLinkColumn>
                                <dxe:GridViewDataHyperLinkColumn Caption="Lost/Not Interested" FieldName="Lost/Not Interested" VisibleIndex="13" Width="15%">
                                    <DataItemTemplate>

                                        <a href='CrmPhoneCallActivityWithIFrame.aspx?id=<%#Eval("UserId") %>&amp;type=SW' style="cursor: pointer;"><%#Eval("Lost/Not Interested") %></a>
                                    </DataItemTemplate>
                                </dxe:GridViewDataHyperLinkColumn>
                                <%-- <dxe:GridViewCommandColumn VisibleIndex="9" Width="5%" ShowDeleteButton="True" />--%>
                            </Columns>
                            <%-- <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center" PopupEditFormHeight="600px"
                                PopupEditFormVerticalAlign="TopSides" PopupEditFormWidth="900px" />--%>
                            <%--<SettingsText PopupEditFormCaption="Add/Modify Cash/Bank" ConfirmDelete="Are you sure to Delete this Record!" />--%>

                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <StylesEditors>
                                <ProgressBar Height="25px">
                                </ProgressBar>
                            </StylesEditors>
                            <%-- <ClientSideEvents EndCallback="function(s, e) {
	  EndCall(s.cpEND);
}" />--%>
                        </dxe:ASPxGridView>

                        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                        </dxe:ASPxGridViewExporter>

                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblUserName" runat="server" Text="UserName" ForeColor="Red" Visible="False"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="txtUser" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr runat="server" id="pnlShowDetail">
                <td>
                    <%--converted asp grid to devexpress grid on 13122016 by sanjib--%>
                    <%-- <asp:GridView ID="grdDetail" runat="server" Width="100%" CssClass="gridcellleft"
                        CellPadding="4" ForeColor="#333333" GridLines="None" BorderWidth="1px" BorderColor="#507CD1"
                        OnRowDataBound="grdDetail_RowDataBound" AllowPaging="True" OnPageIndexChanging="grdDetail_PageIndexChanging">
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
                        <EditRowStyle BackColor="#2461BF" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle Font-Bold="False" ForeColor="" CssClass="EHEADER" BorderColor="AliceBlue"
                            BorderWidth="1px" />
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:TemplateField HeaderText="Activity">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkDetail" runat="server" />
                                    <asp:Label Visible="false" ID="lblActNo" runat="Server" Text='<%# Eval("Activity No") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="gridheader" />
                    </asp:GridView>--%>

                    <dxe:ASPxGridView ID="grdDetailNew" runat="server" ClientInstanceName="grdDetailNewgrid" KeyFieldName="UserId"
                        Width="100%" OnCustomCallback="grdDetailNew_CustomCallback"
                        AutoGenerateColumns="False">
                        <SettingsBehavior ConfirmDelete="True" AllowFocusedRow="false" />
                        <Styles>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </Styles>
                        <%--  <SettingsPager NumericButtonCount="20" Visible="true">
                        </SettingsPager>--%>
                        <Columns>
                            <%--<dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Caption="Activity NO" >
                            </dxe:GridViewCommandColumn>--%>

                            <dxe:GridViewDataTextColumn Caption="Activity" VisibleIndex="0" FieldName="Activity NO">
                                <DataItemTemplate>
                                    <dxe:ASPxCheckBox ID="chkDetail" runat="server">
                                    </dxe:ASPxCheckBox>
                                    <%--<dxe:ASPxCheckBox ID="chkDetail" runat="server" ClientSideEvents-CheckedChanged="checkcheckbox('<%# Eval("LeadId")) %>')" >
                                    </dxe:ASPxCheckBox>--%>
                                    <%--<dxe:ASPxLabel ID="lblActNo" runat="server"></dxe:ASPxLabel>--%>

                                    <dxe:ASPxTextBox ID="lblActNo" runat="server" Width="100%" Visible="false"
                                        NullText="0" Value='<%# Eval("Activity NO") %>'>
                                    </dxe:ASPxTextBox>
                                </DataItemTemplate>
                            </dxe:GridViewDataTextColumn>
                            <%--<dxe:GridViewDataCheckColumn  Visible="true" VisibleIndex="1" FieldName="Activity NO">
                               <EditFormSettings Visible="False" VisibleIndex="1" />
                           </dxe:GridViewDataCheckColumn>--%>

                            <dxe:GridViewDataTextColumn FieldName="SNO" VisibleIndex="2">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Activity NO" VisibleIndex="3">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Total Calls" VisibleIndex="4">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Won" VisibleIndex="5">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="In Process" VisibleIndex="6">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Pending" VisibleIndex="7">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Non Usable" VisibleIndex="8">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <%-- <dxe:GridViewCommandColumn VisibleIndex="9" Width="5%" ShowDeleteButton="True" />--%>
                        </Columns>
                        <%-- <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center" PopupEditFormHeight="600px"
                                PopupEditFormVerticalAlign="TopSides" PopupEditFormWidth="900px" />--%>
                        <%--<SettingsText PopupEditFormCaption="Add/Modify Cash/Bank" ConfirmDelete="Are you sure to Delete this Record!" />--%>

                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <StylesEditors>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </StylesEditors>
                        <%--<ClientSideEvents SelectionChanged="grdDetailNew_SelectionChanged" />--%>
                        <%-- <ClientSideEvents EndCallback="function(s, e) {
	  EndCall(s.cpEND);
}" />--%>
                    </dxe:ASPxGridView>


                    <asp:TextBox ID="txtId" runat="server" Visible="false"></asp:TextBox>
                </td>
            </tr>
            <tr id="frmShowCall" runat="Server">
                <td>
                    <asp:Panel ID="pnlCall" runat="server" Width="100%" Visible="false">
                        <div class="row">
                            <div class="col-md-3">
                                <label><asp:Label ID="Label8" runat="server" Text="Activity Type :" Width="83px"></asp:Label></label>
                                <div>
                                    <asp:DropDownList ID="drpActType" Enabled="true" AutoPostBack="false" runat="server"
                                        Width="100%">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <label><asp:Label ID="Label12" runat="server" Text="" ></asp:Label></label>
                                <div>
                                    <%--<asp:TextBox ID="TxtStartDate" runat="server"></asp:TextBox>
                     <asp:Image ID="ImgStartDate" runat="server" ImageUrl="~/images/calendar.jpg" />
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TxtStartDate"
                         ErrorMessage="Required" ValidationGroup="a"></asp:RequiredFieldValidator>--%>
                                    <dxe:ASPxDateEdit ID="TxtStartDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                        Width="100%" EditFormatString="dd-MM-yyyy hh:mm tt">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                    </dxe:ASPxDateEdit>
                                </div>
                            </div>
                            <div class="col-md-3" style="display: none">
                                <label  style="display: none">
                                    <asp:Label ID="Label9" runat="server" Text="Assigned To  " ></asp:Label></label>
                                <div style="display: none">
                                    <asp:TextBox ID="txtUserList" runat="server" Width="100%"></asp:TextBox>
                                    <asp:HiddenField ID="hd1UserList" runat="server" />
                                    <asp:DropDownList Visible="false" ID="drpUserWork" runat="server" Width="100%">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <label> <asp:Label ID="Label13" runat="server" Text=""></asp:Label></label>
                                <div class="relative">
                                    <%--<asp:TextBox ID="TxtEndDate" runat="server"></asp:TextBox>
                      <asp:Image ID="ImgEndDate" runat="server" ImageUrl="~/images/calendar.jpg" />
                      <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TxtEndDate"
                          ErrorMessage="Required" ValidationGroup="a"></asp:RequiredFieldValidator>--%>
                                    <dxe:ASPxDateEdit ID="TxtEndDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                        Width="100%" EditFormatString="dd-MM-yyyy hh:mm tt">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                    </dxe:ASPxDateEdit>
                                    <span id="MandatoryEgSDate" style="display: none" class="validclass">
                                        <img id="4gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="End date should be greater than or equal to start date"></span>

                                </div>
                            </div>
                            <div class="col-md-3">
                                <label><asp:Label ID="Label14" runat="server" Text="Priority "></asp:Label></label>
                                <div>
                                    <asp:DropDownList ID="drpPriority" runat="server" Width="100%">
                                        <asp:ListItem Text="Low" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Normal" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="High" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Urgent" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Immediate" Value="4"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <label>
                                    <asp:Label ID="Label10" runat="server" Text="Description"></asp:Label></label>
                                <div>
                                    <asp:TextBox ID="txtDesc" TextMode="MultiLine" Rows="2" runat="server" Width="100%"></asp:TextBox>
                                </div>
                                <div style="display: none">
                                    <input type="button" value="Add Lead" disabled="disabled" onclick="fun123()"
                                        id="Button1" class="btnUpdate btn btn-success btn-xs" /></div>
                            </div>
                            <div class="col-md-6">
                                <label>
                                    <asp:Label ID="Label11" runat="server" Text="Instrution Note "></asp:Label></label>
                                <div>
                                    <asp:TextBox ID="txtInstNote" runat="server" TextMode="MultiLine" Rows="2" Width="100%"></asp:TextBox>
                                </div>
                            </div>
                            <div class="clear"></div>
                            <div class="col-md-12">
                                <asp:Button ID="btnSubmit" Text="Save" runat="server"  CssClass="btnUpdate btn btn-primary"
                                        OnClick="btnSubmit_Click" Enabled="True" ValidationGroup="a" OnClientClick="javasript:return btnSubmit_clientClick()" />
                                    <asp:Button ID="btnCancel" Text="Cancel" runat="server"  CssClass="btnUpdate btn btn-danger"
                                        OnClick="btnCancel_Click" />
                            </div>
                            
                        </div>
                        <table class="TableMain100">
                            <tr>
                                

                            </tr>
                            <tr>
                                
                                
                                
                            </tr>
                            <tr>
                                
                                
                                
                            </tr>
                            <tr>
                                
                            </tr>
                            <tr>
                                <td></td>
                                <td align="" colspan="4">
                                    
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlActivityDetail" runat="server" Visible="false" Width="100%">
                        <asp:GridView ID="grdActivityDetail" runat="server" Width="100%" CssClass="gridcellleft"
                            CellPadding="4" ForeColor="#333333" GridLines="None" BorderWidth="1px" BorderColor="#507CD1"
                            OnPageIndexChanging="grdActivityDetail_PageIndexChanging">
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
                            <EditRowStyle BackColor="#2461BF" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <HeaderStyle Font-Bold="False" ForeColor="Black" CssClass="EHEADER" BorderColor="AliceBlue"
                                BorderWidth="1px" />
                            <AlternatingRowStyle BackColor="White" />
                        </asp:GridView>

                    </asp:Panel>
                </td>
            </tr>
            <tr id="frmShowGenratedSalesvisit" runat="Server" visible="False">
                <td>
                    <table class="TableMain100">
                        <tr>
                            <td>
                                <table class="TableMain100">
                                    <tr style="display: none">
                                        <td>
                                            <input type="text" runat="Server" id="txtLeadId" name="txtLeadId" style="background-color: #DDECFE; border-color: #DDECFE; border-style: none; color: #DDECFE"
                                                readonly="readOnly"
                                                visible="true" />
                                            <asp:Label ID="lblError" runat="server" ForeColor="#DDECFE" BorderStyle="none" BorderColor="#DDECFE"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="lt">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="Lrd" runat="server" GroupName="a" Checked="True" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label2" runat="server" CssClass="mylabel1" Text="From Lead Data" Font-Size="X-Small"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="Erd" runat="server" GroupName="a" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label3" runat="server" CssClass="mylabel1" Text="From Existing Customer Data"
                                                            Font-Size="X-Small"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <table class="TableMain100">
                                                <tr>
                                                    <td class="mylabel1" style="width: 20%">Select Date Range:
                                                    </td>
                                                    <td class="lt">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <dxe:ASPxDateEdit ID="FromDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                                        Width="115px">
                                                                        <ButtonStyle Width="13px">
                                                                        </ButtonStyle>
                                                                    </dxe:ASPxDateEdit>
                                                                </td>
                                                                <td class="mylabel1" style="width: 15%">To:
                                                                </td>
                                                                <td class="lt">
                                                                    <%--<asp:TextBox ID="ToDate" runat="server" Width="103px"></asp:TextBox>
                                                <asp:Image ID="ImgTo" runat="server" ImageUrl="~/images/calendar.jpg" />--%>
                                                                    <dxe:ASPxDateEdit ID="ToDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy hh:mm tt"
                                                                        UseMaskBehavior="True" Width="115px">
                                                                        <ButtonStyle Width="13px">
                                                                        </ButtonStyle>
                                                                    </dxe:ASPxDateEdit>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <%--<asp:TextBox ID="FromDate" runat="server" Width="104px"></asp:TextBox>
                                                <asp:Image ID="ImgFrom" runat="server" ImageUrl="~/images/calendar.jpg" />--%>
                                                    </td>
                                                    <td class="mylabel1" style="width: 3%;">Assigned/UnAssigned:
                                                    </td>
                                                    <td class="lt">
                                                        <asp:DropDownList ID="drpSelect" runat="server" Width="185px">
                                                            <asp:ListItem>All</asp:ListItem>
                                                            <asp:ListItem>Assigned</asp:ListItem>
                                                            <asp:ListItem>UnAssigned</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="mylabel1" style="width: 10%;">Product Type
                                                    </td>
                                                    <td>
                                                        <table style="border: solid 1px white">
                                                            <tr>
                                                                <td class="lt">
                                                                    <input type="radio" class="mylabel1" runat="Server" id="Radio1" name="rdr" value="All"
                                                                        checked="true" onclick="javascript: fun(this, true)" />
                                                                    All
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="lt" style="width: 1%">
                                                                    <input type="radio" class="mylabel1" runat="Server" id="Radio2" name="rdr" value="Select"
                                                                        onclick="javascript: fun(this, false)" />Selective
                                                                </td>
                                                                <td class="lt" style="width: 3%">
                                                                    <asp:DropDownList ID="drpProduct" runat="server" Enabled="false" Width="180px">
                                                                        <asp:ListItem Text="Broking And DP Account" Value="Broking And DP Account"></asp:ListItem>
                                                                        <asp:ListItem Text="Mutual Fund" Value="Mutual Fund"></asp:ListItem>
                                                                        <asp:ListItem Text="Insurance" Value="Insurance"></asp:ListItem>
                                                                        <asp:ListItem Text="Refreal Agent" Value="Refreal Agent"></asp:ListItem>
                                                                        <asp:ListItem Text="Sub Broker" Value="Sub Broker"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td valign="bottom" align="left">
                                                        <asp:Button ID="btnshowGenratedSaleVisit" runat="server" Text="Show" CssClass="btnUpdate btn btn-primary"
                                                            OnClick="btnshowGenratedSaleVisit_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnSelectAll" runat="server" Text="Select All" Visible="false" CssClass="btnUpdate btn btn-primary"
                                    OnClick="btnSelectAll_Click" />
                                <asp:Button ID="btnExport" runat="server" Text="Export" Visible="false" CssClass="btnUpdate btn btn-primary" />
                                <input type="button" id="btnPostPendingMeetings" name="btnPostPendingMeetings" value="PostPone Meeting"
                                    class="btnUpdate" onclick="funCheckFunction()" style="height: 21px" />
                                <input type="button" id="btnCancelMeeting" name="btnCancelMeeting" value="Cancel Meeting"
                                    class="btnUpdate" onclick="funSaveClick()" style="height: 21px" />
                                <asp:Label ID="lblTotalRecord" runat="server" ForeColor="Navy"></asp:Label>
                                <br />
                                <br />
                                <asp:GridView ID="grdGenratedSalesVisit" runat="server" AutoGenerateColumns="true"
                                    AllowPaging="True" PageSize="6" Width="100%" AllowSorting="True" CssClass="gridcellleft"
                                    CellPadding="4" ForeColor="#333333" GridLines="None" BorderWidth="1px" BorderColor="#507CD1"
                                    OnPageIndexChanging="grdGenratedSalesVisit_PageIndexChanging" OnRowDataBound="grdGenratedSalesVisit_RowDataBound"
                                    OnSorting="grdGenratedSalesVisit_Sorting">
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <HeaderStyle Font-Bold="False" ForeColor="Black" BorderColor="AliceBlue" BorderWidth="1px" />
                                    <AlternatingRowStyle BackColor="White" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sel">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSel" runat="server"></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="Server" Text='<%# Eval("LeadId") + "@@@@" +  Eval("ProductId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td id="frmAllot" runat="Server" visible="False">
                                <input type="button" id="btnAllot" name="btnAllot" value="Allot" onclick="windowopenform()"
                                    class="btnUpdate" style="height: 21px" />
                                <asp:Button ID="btnCancelGenratedSalesVisit" runat="server" Text="Cancel" CssClass="btnUpdate"
                                    OnClick="btnCancelGenratedSalesVisit_Click" Height="21px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="frmShowCourtesyCall" runat="Server" visible="false">
                <td>
                    <table class="TableMain100">
                        <tr>
                            <td>
                                <table class="TableMain100">
                                    <tr>
                                        <td colspan="7" id="tdcourtesy" runat="server" style="text-align: center">Show Courtesy Call
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table style="border: solid 1px black">
                                                <tr>
                                                    <td colspan="7" class="lt">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="CLrd" runat="server" GroupName="a" Checked="True" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label4" runat="server" CssClass="mylabel1" Text="From Lead Data" Font-Size="X-Small"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="CCrd" runat="server" GroupName="a" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label5" runat="server" CssClass="mylabel1" Text="From Existing Customer Data"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="mylabel1" style="width: 9%">Select Date:</td>
                                                    <td style="text-align: left; width: 21%;">
                                                        <%--<asp:TextBox ID="CourtesyStartCallDate" runat="server" Width="150px"></asp:TextBox>
                                       <asp:Image ID="ImgStartCall" runat="server" ImageUrl="~/images/calendar.jpg" />--%>
                                                        <dxe:ASPxDateEdit ID="CourtesyStartCallDate" runat="server" EditFormat="Custom" UseMaskBehavior="True">
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                        </dxe:ASPxDateEdit>
                                                    </td>
                                                    <td class="mylabel1" style="width: 2%">To</td>
                                                    <td class="lt" style="width: 20%">
                                                        <%--<asp:TextBox ID="CourtesyEndCallDate" runat="server" Width="150px"></asp:TextBox>
                                       <asp:Image ID="ImgEndCall" runat="server" ImageUrl="~/images/calendar.jpg" />--%>
                                                        <dxe:ASPxDateEdit ID="CourtesyEndCallDate" runat="server" EditFormat="Custom" UseMaskBehavior="True">
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                        </dxe:ASPxDateEdit>
                                                    </td>
                                                    <td class="mylabel1" style="width: 9%">Select User:</td>
                                                    <td style="text-align: left;">
                                                        <asp:DropDownList ID="drpSelectedUser" runat="server" AppendDataBoundItems="true"
                                                            Width="208px">
                                                            <asp:ListItem Value="0">All</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="lt">
                                                        <asp:Button ID="btnShowCourtesyCall" runat="Server" Text="Show" CssClass="btnUpdate"
                                                            OnClick="btnShowCourtesyCall_Click" Height="21px" /></td>
                                                    <td style="text-align: left">&nbsp;</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblTotalCourtestCall" runat="Server" ForeColor="red"></asp:Label>
                                <asp:GridView ID="grdCourtestCall" runat="Server" AutoGenerateColumns="true" AllowPaging="True"
                                    PageSize="6" Width="100%" AllowSorting="True" CssClass="gridcellleft" CellPadding="4"
                                    ForeColor="#333333" GridLines="None" BorderWidth="1px" BorderColor="#507CD1"
                                    OnPageIndexChanging="grdCourtestCall_PageIndexChanging" OnRowDataBound="grdCourtestCall_RowDataBound"
                                    OnSorting="grdCourtestCall_Sorting" OnSelectedIndexChanged="grdCourtestCall_SelectedIndexChanged">
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <HeaderStyle Font-Bold="False" ForeColor="" BorderColor="AliceBlue" BorderWidth="1px" />
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="FrmShowGenratedSales" runat="Server" visible="false">
                <td>
                    <table class="TableMain100">
                        <tr id="TdGeneratedSales" runat="server">
                            <td colspan="3" style="text-align: center">
                                <asp:Label ID="Label15" CssClass="mylabel1" runat="server" Text="Genrated Sales By Phone Follow Up"></asp:Label>
                                <input type="Hidden" runat="Server" id="txtLeadId1" name="txtLeadId1" style="background-color: #DDECFE; border-color: #DDECFE; border-style: none; color: #DDECFE" />
                                <asp:Label ID="Label1" runat="server" ForeColor="#DDECFE" BorderStyle="none"></asp:Label></td>
                        </tr>
                        <tr>
                        </tr>
                        <tr>
                            <td>
                                <table class="TableMain100">
                                    <tr>
                                        <td colspan="6">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="LGLrd" runat="server" GroupName="a" Checked="True" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label6" runat="server" Text="From Lead Data" CssClass="mylabel1"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="LGCrd" runat="server" GroupName="a" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label7" runat="server" Text="From Existing Customer Data" CssClass="mylabel1"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="mylabel1" style="width: 8%">Select Date Range :</td>
                                        <td style="width: 7%;" class="lt">
                                            <%-- <asp:TextBox ID="SalesStartDate" runat="server" Width="106px"></asp:TextBox>
                                       <asp:Image ID="ImgSalesStartDate" runat="server" ImageUrl="~/images/calendar.jpg" />--%>
                                            <dxe:ASPxDateEdit ID="SalesStartDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                Width="154px">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td class="mylabel1" style="width: 1%">To
                                        </td>
                                        <td style="width: 8%;" class="lt">
                                            <%--<asp:TextBox ID="SalesEndDate" runat="server" Width="127px"></asp:TextBox>
                                       <asp:Image ID="ImgSalesEndDate" runat="server" ImageUrl="~/images/calendar.jpg" />--%>
                                            <dxe:ASPxDateEdit ID="SalesEndDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                Width="157px">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td class="mylabel1" style="width: 3%">Assigned/UnAssigned
                                        </td>
                                        <td style="width: 8%" class="lt">
                                            <asp:DropDownList ID="drpSalesSelect" runat="server" Width="126px">
                                                <asp:ListItem>All</asp:ListItem>
                                                <asp:ListItem>Assigned</asp:ListItem>
                                                <asp:ListItem>UnAssigned</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="mylabel1" style="width: 7%">Product Type :</td>
                                        <td colspan="3" style="border: solid 1px white;">
                                            <table style="width: 431px">
                                                <tr>
                                                    <td>
                                                        <input type="radio" class="mylabel1" runat="Server" id="Radio3" name="rdr1" value="All"
                                                            onclick="javascript: fun1(this, true)" checked="True" />All:
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <input type="radio" class="mylabel1" runat="Server" id="Radio4" name="rdr1" value="Select"
                                                            onclick="javascript: fun1(this, false)" />Selective:</td>
                                                    <td>
                                                        <asp:DropDownList ID="drpSalesProduct" runat="Server" Enabled="false" Width="299px">
                                                            <asp:ListItem Text="Broking &amp; DP Account" Value="Broking &amp; DP Account"></asp:ListItem>
                                                            <asp:ListItem Text="Mutual Fund" Value="Mutual Fund"></asp:ListItem>
                                                            <asp:ListItem Text="Insurance" Value="Insurance"></asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td class="lt" valign="bottom" align="left">
                                            <asp:Button ID="btnShowSales" runat="Server" Text="Show" CssClass="btnUpdate" OnClick="btnShowSales_Click"
                                                Height="21px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                                <asp:Button ID="btnSelectAllSales" runat="server" Text="Select All" Visible="False"
                                    CssClass="btnUpdate" Height="21px" OnClick="btnSelectAllSales_Click" />
                                <asp:Button ID="btnSalesExport" runat="server" Text="Export" Visible="False" CssClass="btnUpdate"
                                    Height="21px" />
                                <input type="button" id="Button5" name="btnPostPendingMeetings" value="PostPone Meeting"
                                    onclick="funCheckFunctionSales()" class="btnUpdate" style="height: 21px" />
                                <input type="button" id="Button6" name="btnCancelMeeting" value="Cancel Meeting"
                                    onclick="funSaveClickSales()" class="btnUpdate" style="height: 21px" />
                                <asp:Label ID="lblSalesTotalRecord" runat="Server" ForeColor="red"></asp:Label>
                                <br />
                                <br />
                                <asp:GridView ID="grdSales" runat="Server" AutoGenerateColumns="true" AllowPaging="True"
                                    PageSize="6" Width="100%" CssClass="gridcellleft" CellPadding="4" ForeColor="#333333"
                                    GridLines="None" BorderWidth="1px" BorderColor="#507CD1" AllowSorting="True"
                                    OnPageIndexChanging="grdSales_PageIndexChanging" OnRowDataBound="grdSales_RowDataBound"
                                    OnSorting="grdSales_Sorting">
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <HeaderStyle Font-Bold="False" ForeColor="Black" BorderColor="AliceBlue" BorderWidth="1px" />
                                    <AlternatingRowStyle BackColor="White" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sel">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSel" runat="server"></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="Server" Text='<%# Eval("LeadId") + "@@@@" +  Eval("ProductId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" id="frmSalesAllot" runat="Server" visible="False" style="height: 23px">
                                <input type="button" id="btnSalesAllot" name="btnAllot" value="Allot" onclick="windowopenform123()"
                                    class="btnUpdate" style="height: 21px" />
                                <asp:Button ID="btnSalesCanCel" runat="server" Text="Cancel" CssClass="btnUpdate btn btn-danger"
                                    OnClick="btnSalesCanCel_Click" Height="21px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
