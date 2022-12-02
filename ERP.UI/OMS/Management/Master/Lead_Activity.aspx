<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="true"
    Inherits="ERP.OMS.Management.Master.management_master_Lead_Activity" CodeBehind="Lead_Activity.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--choosen.min.js code Added By7 Priti on 13122016 for jquery choosen--%>
      <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            ListBind();
           // ChangeSource();

        });
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
        function lstCompany() {

            $('#lstCompany').fadeIn();
            
        }
        function setvalue() {
            document.getElementById("txtCompany_hidden").value = document.getElementById("lstCompany").value;
         
        }
        function Changeselectedvalue() {
            var lstCompany = document.getElementById("lstCompany");
            if (document.getElementById("txtCompany_hidden").value != '') {
                for (var i = 0; i < lstCompany.options.length; i++) {
                    if (lstCompany.options[i].value == document.getElementById("txtCompany_hidden").value) {
                        lstCompany.options[i].selected = true;
                    }
                }
                $('#lstCompany').trigger("chosen:updated");
            }

        }
        function ChangeSource() {
            var fname = "%";
            var lCompany = $('select[id$=lstCompany]');
            //var ddlCategory = $('[id$=cmbCategory]'); //document.getElementByText("cmbCategory");
            var obj4 = document.getElementById("cmbCategory");
            var obj5 = obj4.value;
           
            lCompany.empty();

            $.ajax({
                type: "POST",
                url: "Lead_Activity.aspx/GetCompany",
                data: JSON.stringify({ reqStr: fname, param: obj5 }),
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

                            $('#lstCompany').append($('<option>').text(name).val(id));
                           
                        }

                        $(lCompany).append(listItems.join(''));
                       
                        lstCompany();
                        $('#lstCompany').trigger("chosen:updated");
                       
                       // Changeselectedvalue();
                       
                    }
                    else {
                        //   alert("No records found");
                        //lstReferedBy();
                        $('#lstCompany').trigger("chosen:updated");
                       
                    }
                }
            });
            // }
        }
        //.........end..................
        function CallList(obj1, obj2, obj3) {
            //alert('rrr');
            var obj6 = document.getElementById("cmbCategory");
            if (obj6.value == 'HLO' || obj6.value == 'LAP' || obj6.value == 'PLO' || obj6.value == 'TLO' || obj6.value == 'BLO' || obj6.value == 'ELO' || obj6.value == 'ALO' || obj6.value == 'SLO' || obj6.value == 'LAS' || obj6.value == 'CRD') {
                ajax_showOptions(obj1, obj2, obj3, obj6.value);
            }
            else {
                var obj4 = document.getElementById("txtCompany_hidden");
                var obj5 = obj4.value;
                //alert(obj5);
                ajax_showOptions(obj1, obj2, obj3, obj5);
            }
        }
        function CallList1(obj1, obj2, obj3) {
            //alert('rrr');

            var obj4 = document.getElementById("cmbCategory");
            var obj5 = obj4.value;
            //alert(obj5);
            ajax_showOptions(obj1, obj2, obj3, obj5);
        }
        FieldName = 'BTNClose';
        function Validation() {
            var ddl = document.getElementById("cmbCategory");
            var txt = document.getElementById("txtProductAmmount");
            if (ddl.value == "Mutual Fund") {
                if (txt.value == "") {
                    alert('Product Amount Required');
                    return false;
                }
                else {
                    return true;
                }
            }
            if (ddl.value == "Insurance") {
                if (txt.value == "") {
                    alert('Product Amount Required');
                    return false;
                }
                else {
                    return true;
                }
            }
            else {
                return true;
            }
        }

        function AddProductCategory() {
            GridCategory.PerformCallback();
        }
        function VisibilityOnOff(obj) {

            if (obj == 'Broking & DP Account' || obj == 'Sub Broker' || obj == 'Relationship Partner') {
                document.getElementById("TDCompanyName").style.display = 'none';
                document.getElementById("TDCompanyVal").style.display = 'none';
                document.getElementById("TDProductName").style.display = 'none';
                document.getElementById("TDProductVal").style.display = 'none';
                document.getElementById("TDProductAmtName").style.display = 'none';
                document.getElementById("TDProductAmtVal").style.display = 'none';
                document.getElementById("TDADD").style.display = 'none';
                document.getElementById("TDGRID").style.display = 'none';
               

            }
            else {
                document.getElementById("TDCompanyName").style.display = 'inline';
                document.getElementById("TDCompanyVal").style.display = 'inline';
                document.getElementById("TDProductName").style.display = 'inline';
                document.getElementById("TDProductName").style.display = 'table-cell';
                document.getElementById("TDProductVal").style.display = 'inline';
                document.getElementById("TDProductAmtName").style.display = 'inline';
                document.getElementById("TDProductAmtVal").style.display = 'inline';
                document.getElementById("TDADD").style.display = 'inline';
                document.getElementById("TDGRID").style.display = 'inline';
                document.getElementById("TDGRID").style.display = 'table-cell';
                ChangeSource();
            }
           
        }
        function BranchOrClient(obj) {

            if (obj.id == 'rdClient') {
                document.getElementById("drpVisitPlace").style.display = 'inline';
                document.getElementById("TdVisitplace").style.display = 'inline';
                document.getElementById("TdBranch").style.display = 'none';
            }

            else if (obj.id == 'rdBranch') {
                document.getElementById("drpVisitPlace").style.display = 'none';
                document.getElementById("TdVisitplace").style.display = 'none';
                document.getElementById("TdBranch").style.display = 'inline';
                document.getElementById("TdBranch").style.display = 'table-cell';
            }
        }
        function SearchByBranchName(obj1, obj2, obj3) {
            ajax_showOptions(obj1, obj2, obj3);
        }
        FieldName = 'cmbAddressOfMeeting';
    </script>
    <style type="text/css">
        .auto-style1 {
            width: 157px;
        }
        .auto-style3 {
            width: 16%;
        }
        .auto-style4 {
            width: 15%;
        }
         /*Code  Added  By Priti on13122016 to use jquery Choosen*/
         .chosen-container.chosen-container-single {
            width:100% !important;
        }
        .chosen-choices {
            width:100% !important;
        }
        #lstCompany {
            width:200px;
        }
        #lstCompany {
            display:none !important;
            
        }
        #lstCompany_chosen{
            width:200px !important;
        }
        .dxtcLite_PlasticBlue > .dxtc-content {
            overflow:visible !important
        }

        
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <%-- <h3>Activity</h3>--%>
            <h4>
                <asp:Label ID="lblHeadTitle" runat="server"></asp:Label>
            </h4>
        </div>
        <div class="crossBtn">
            <%--<a href="Lead.aspx" id="goBackCrossBtn"><i class="fa fa-times"></i></a>--%>
            <asp:HyperLink
                ID="goBackCrossBtn"
                NavigateUrl="#"
                runat="server">
        <i class="fa fa-times" style="margin-top:6px"></i>
            </asp:HyperLink>
        </div>
    </div>
    <div class="form_main" style="border: 1px solid #ccc; padding-left: 15px">
        <table class="TableMain100">
            <tr>
                <td>
                    <table style="width: 98%">
                        <tr>
                            <td class="mylabel1" style="width: 15%;">

                                <asp:Label ID="Label2" runat="server" Text="Activity Type"></asp:Label></td>
                            <td style="width: 35%; text-align: left">
                                <asp:DropDownList ID="cmbActType" runat="server" Width="100%" AutoPostBack="true"
                                    OnSelectedIndexChanged="cmbActType_SelectedIndexChanged" TabIndex="1">
                                    <asp:ListItem Text="Phone Calls" Value="1" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Sales Visit" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="Sales" Value="6"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 50%">
                                <asp:Label ID="lblmessage1" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel runat="server" ID="Body" Width="100%">
                        <table style="width: 98%">
                            <tr>
                                <td class="mylabel1" style="width: 15%;">
                                    <asp:Label ID="Label3" runat="server" Text="Assign To"></asp:Label></td>
                                <td style="width: 35%">
                                    <asp:DropDownList ID="cmbAssignTo" runat="server" Width="100%" TabIndex="2">
                                    </asp:DropDownList></td>
                                <td class="mylabel1" style="width: 15%;padding-left:15px;">
                                    <asp:Label ID="Label1" runat="server" Text="Start Date/Time"></asp:Label></td>
                                <td style="width: 35%">
                                    <%--<asp:TextBox ID="txtStartDate" runat="server" TabIndex="3" Width="146px"></asp:TextBox>
                                    <asp:Image
                                        ID="imgStartDate" runat="server" ImageUrl="~/images/calendar.jpg" />--%>
                                    <%--     <dxe:ASPxDateEdit ID="txtStartDate" runat="server" EditFormat="Custom" EditFormatString="dd MMMM yyyy hh:mm tt"
                                        UseMaskBehavior="True" TabIndex="3">
                                        <timesectionproperties>
                                         <TimeEditProperties EditFormatString="hh:mm tt" />
                                      </timesectionproperties>
                                        <buttonstyle width="13px">
                                        </buttonstyle>
                                    </dxe:ASPxDateEdit>--%>


                                    <dxe:ASPxDateEdit id="txtStartDate" runat="server" editformat="Custom" date="2009-11-02 09:23" width="200">
                                        <timesectionproperties>
                                                                <TimeEditProperties EditFormatString="hh:mm tt" />
                                                      </timesectionproperties>
                                    </dxe:ASPxDateEdit>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtStartDate"
                                        Display="Dynamic" ErrorMessage="date required" ValidationGroup="b"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="width: 50%">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="cmbAssignTo"
                                        Display="Dynamic" ErrorMessage="Please Select user."></asp:RequiredFieldValidator></td>
                                <td class="mylabel1" style="width: 15%;padding-left:15px;">
                                    <asp:Label ID="Label4" runat="server" Text="End Date/Time"></asp:Label></td>
                                <td style="width: 35%">
                                    <%--<asp:TextBox ID="txtEndDate" runat="server" TabIndex="4" Width="146px"></asp:TextBox>
                                    <asp:Image
                                        ID="imgEndDate" runat="server" ImageUrl="~/images/calendar.jpg" />--%>
                                    <%--        <dxe:ASPxDateEdit ID="txtEndDate" runat="server" EditFormat="Custom" EditFormatString="dd MMMM yyyy hh:mm tt"
                                        UseMaskBehavior="True" TabIndex="4">
                                        <timesectionproperties>
                                             <TimeEditProperties EditFormatString="hh:mm tt" />
                                         </timesectionproperties>
                                        <buttonstyle width="13px">
                                        </buttonstyle>
                                    </dxe:ASPxDateEdit>--%>

                                    <dxe:ASPxDateEdit id="txtEndDate" runat="server" editformat="Custom" date="2009-11-02 09:23" width="200">
                                        <timesectionproperties>
                                                                <TimeEditProperties EditFormatString="hh:mm tt" />
                                                      </timesectionproperties>
                                    </dxe:ASPxDateEdit>
                                </td>
                            </tr>
                            <tr>
                                <td class="mylabel1" style="width: 15%;">
                                    <asp:Label ID="Label5" runat="server" Text="Description"></asp:Label></td>
                                <td style="width: 35%">
                                    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Width="100%"
                                        TabIndex="5"></asp:TextBox></td>
                                <td class="mylabel1" style="padding-left:15px;">
                                    <asp:Label ID="Label6" runat="server" Text="Priority"></asp:Label></td>
                                <td style="width: 35%">
                                    <asp:DropDownList ID="cmbPriority" runat="server" Width="200px" TabIndex="6">
                                        <asp:ListItem Text="Low" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Normal" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="High" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Urgent" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Immediate" Value="4"></asp:ListItem>
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td class="mylabel1" style="width: 15%;">
                                    <asp:Label ID="Label7" runat="server" Text="Instruction Notes"></asp:Label></td>
                                <td colspan="3" style="width: 85%">
                                    <asp:TextBox ID="txtInstructionNotes" runat="server" TextMode="MultiLine" Width="738px"
                                        TabIndex="7"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="width: 100%">
                                    <asp:Panel runat="server" ID="panelinside" Width="100%">
                                        <table style="width: 100%">
                                            <tr>
                                                <td colspan="4">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td class="auto-style4">
                                                                <asp:Label ID="Label8" runat="server" Text="Category"></asp:Label></td>
                                                            <td style="width: 35%">
                                                                <asp:DropDownList ID="cmbCategory" runat="server" Width="200px" TabIndex="8">
                                                                    <asp:ListItem Text="Broking &amp; DP Account" Value="Broking &amp; DP Account"></asp:ListItem>
                                                                    <asp:ListItem Text="Mutual Fund" Value="Mutual Fund"></asp:ListItem>
                                                                    <asp:ListItem Text="Insurance-Life" Value="Insurance-Life"></asp:ListItem>
                                                                    <asp:ListItem Text="Insurance-General" Value="Insurance-General"></asp:ListItem>
                                                                    <asp:ListItem Text="Relationship Partner" Value="Relationship Partner"></asp:ListItem>
                                                                    <asp:ListItem Text="Sub Broker" Value="Sub Broker"></asp:ListItem>
                                                                    <asp:ListItem Text="Housing Loan" Value="HLO"></asp:ListItem>
                                                                    <asp:ListItem Text="Loan Against Property" Value="LAP"></asp:ListItem>
                                                                    <asp:ListItem Text="Personal Loan" Value="PLO"></asp:ListItem>
                                                                    <asp:ListItem Text="Travel Loan" Value="TLO"></asp:ListItem>
                                                                    <asp:ListItem Text="Business Loan" Value="BLO"></asp:ListItem>
                                                                    <asp:ListItem Text="Education Loan" Value="ELO"></asp:ListItem>
                                                                    <asp:ListItem Text="Auto Loan" Value="ALO"></asp:ListItem>
                                                                    <asp:ListItem Text="SME Loan" Value="SLO"></asp:ListItem>
                                                                    <asp:ListItem Text="Loan Against Securities" Value="LAS"></asp:ListItem>
                                                                    <asp:ListItem Text="Credit Cards" Value="CRD"></asp:ListItem>
                                                                </asp:DropDownList></td>
                                                            <td  class="mylabel1" id="TDCompanyName">
                                                                <asp:Label ID="Label13" runat="server" Text="Company Name"></asp:Label>
                                                            </td>
                                                            <td  id="TDCompanyVal">
                                                                <asp:TextBox ID="txtCompany" runat="server" Width="200px" ValidationGroup="a" TabIndex="9"></asp:TextBox>
                                                                 <asp:ListBox ID="lstCompany" CssClass="chsn"   runat="server" Font-Size="12px" Width="253px"   data-placeholder="Select..."></asp:ListBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="auto-style4" id="TDProductName">
                                                                <asp:Label ID="Label9" runat="server" Text="Product"></asp:Label></td>
                                                            <td style="width: 37%" id="TDProductVal">
                                                                <asp:TextBox ID="txtProduct" runat="server" Width="200px" ValidationGroup="a" TabIndex="10"></asp:TextBox>
                                                            </td>
                                                            <td style="width: 16%; height: 38px;" class="mylabel1" id="TDProductAmtName">
                                                                <asp:Label ID="Label10" runat="server" Text="Product Amount"></asp:Label></td>
                                                            <td style="width: 35%; height: 38px;" colspan="2" id="TDProductAmtVal">
                                                                <asp:TextBox ID="txtProductAmmount" runat="server" ValidationGroup="a" Width="200px"
                                                                    TabIndex="11"></asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtProductAmmount"
                                                                    Display="Dynamic" ErrorMessage="Only Integer" ValidationExpression="^[0-9]+"
                                                                    SetFocusOnError="True"></asp:RegularExpressionValidator>
                                                                <asp:Label ID="lblAmountError" runat="server" ForeColor="Red" Text="Amount Required"
                                                                    Visible="False"></asp:Label></td>
                                                            <td id="TDADD">
                                                                <input id="Button1" type="button" runat="server" value="Add" class="btn btn-primary " onclick="AddProductCategory()" />
                                                                <%-- <asp:Button ID="BtnADD" runat="server" Text="Add" OnClientClick="AddProductCategory();" visible="false" ValidationGroup="a" CssClass="btnUpdate" TabIndex="12"  />--%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="TdProduct" runat="server">
                                                <td></td>
                                                <td style="width: 100%; text-align: left;" colspan="3" id="TDGRID">
                                                    <dxe:ASPxGridView ID="GridCategory" ClientInstanceName="GridCategory" runat="server"
                                                        Width="100%" OnCustomCallback="GridCategory_CustomCallback">
                                                        <styles>
                                                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                            </Header>
                                                            <LoadingPanel ImageSpacing="10px">
                                                            </LoadingPanel>
                                                        </styles>
                                                        <settings showgroupbuttons="False" showstatusbar="Hidden" />
                                                        <settingsbehavior allowdragdrop="False" allowgroup="False" allowsort="False" />
                                                        <settingspager mode="ShowAllRecords">
                                                        </settingspager>
                                                    </dxe:ASPxGridView>
                                                    <asp:Label ID="lblmessage" runat="server" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 15%" class="mylabel1">
                                                    <asp:Label ID="Label11" runat="server" Text="Next Visit Date"></asp:Label></td>
                                                <td style="width: 35%" colspan="1">
                                                    <%--<asp:TextBox ID="txtNextDate" runat="server" TabIndex="12" Width="146px"></asp:TextBox>
                                        <asp:Image
                                            ID="imgNextDate" runat="server" ImageUrl="~/images/calendar.jpg" />--%>
                                                    <%--  <dxe:ASPxDateEdit ID="txtNextDate" runat="server" EditFormat="Custom" EditFormatString="dd MMMM yyyy hh:mm tt"
                                                        UseMaskBehavior="True" TabIndex="13">
                                                        <timesectionproperties>
                                                             <TimeEditProperties EditFormatString="hh:mm tt" />
                                                        </timesectionproperties>
                                                        <buttonstyle width="13px">
                                                        </buttonstyle>
                                                    </dxe:ASPxDateEdit>--%>
                                                

                                                    <dxe:ASPxDateEdit id="txtNextDate" runat="server" editformat="Custom" date="2009-11-02 09:23" width="200">
                                                        <timesectionproperties>
                                                                <TimeEditProperties EditFormatString="hh:mm tt" />
                                                      </timesectionproperties>
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                                <td style="width: 50%" colspan="2">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtNextDate"
                                                        Display="Dynamic" ErrorMessage="Date Required!" SetFocusOnError="True" ValidationGroup="b"></asp:RequiredFieldValidator></td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <table style="border: solid 1px white">
                                                        <tr>
                                                            <td class="auto-style1">
                                                                <asp:Label ID="lblVisitPlace" runat="server" CssClass="mylabel1" Text="Next Visit Place :"></asp:Label></td>
                                                            <td>
                                                                <div class="radio-inline">
                                                                    <label>
                                                                        <asp:RadioButton ID="rdClient" runat="server" GroupName="k" Text="" Checked="true"
                                                                            CssClass="ColorOption" />
                                                                        <asp:Label CssClass="ColorOption" ID="Label12" runat="server" Text="Client Place"></asp:Label>
                                                                    </label>
                                                                </div>
                                                                <div class="radio-inline">
                                                                    <label>
                                                                        <asp:RadioButton ID="rdBranch" runat="server" GroupName="k" Text="" CssClass="ColorOption" />
                                                                        <asp:Label ID="Label14" CssClass="ColorOption" runat="server" Text="Branch Name"></asp:Label>
                                                                    </label>
                                                                </div>
                                                            </td>
                                                            <td></td>  

                                                            
                                                        </tr>
                                                        <tr>
                                                            <td class="auto-style1"></td>
                                                            <td id="TdVisitplace" style="display: inline">
                                                                <asp:DropDownList ID="drpVisitPlace" runat="server" Width="256px">
                                                                </asp:DropDownList></td>
                                                            <td id="TdBranch" style="display: none">
                                                                <asp:TextBox ID="txtbranch" runat="server" Width="253px"></asp:TextBox><asp:HiddenField
                                                                    ID="txtbranch_hidden" runat="server"></asp:HiddenField>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <%--<tr>
                                                    <td style="width: 15%;" class="mylabel1">
                                                        <asp:Label ID="Label12" runat="server" Text="Address Of Meeting"></asp:Label></td>
                                                    <td style="width: 50%; height: 2px;" colspan="2">
                                                        <asp:DropDownList ID="cmbAddressOfMeeting" runat="server" Width="100%" ValidationGroup="b"
                                                            TabIndex="14">
                                                        </asp:DropDownList></td>
                                                    <td style="width: 35%;">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="cmbAddressOfMeeting"
                                                            Display="Dynamic" ErrorMessage="Add Address!" ValidationGroup="b"></asp:RequiredFieldValidator></td>
                                                </tr>--%>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%; height: 30px;padding-left:160px" colspan="4">
                                    <asp:Button ID="BTNSave" runat="server" Text="Save" OnClick="BTNSave_click" CssClass="btn btn-primary"
                                        ValidationGroup="b" TabIndex="15" OnClientClick="setvalue()" />&nbsp; &nbsp;
                                    <asp:Button ID="BTNClose" runat="server"
                                        Text="Close" CssClass="btn btn-danger" TabIndex="15" OnClick="BTNClose_Click" /></td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:TextBox ID="txtCompany_hidden" runat="server" BackColor="Transparent" BorderColor="Transparent"
                                        BorderStyle="None" ForeColor="#DDECFE" Visible="false"></asp:TextBox>
                                    <asp:TextBox ID="txtProduct_hidden" runat="server" Visible="false"></asp:TextBox></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
