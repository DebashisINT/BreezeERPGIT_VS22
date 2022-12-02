<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Reports.Reports_Report_lead" CodeBehind="Report_lead.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--    <%@ Register Assembly="CrystalDecisions.Web, Version=10.2.3600.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
        Namespace="CrystalDecisions.Web" TagPrefix="CR" %>--%>
    <%--<%@ Register Assembly="DevExpress.Web.v10.2, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v10.2, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dxwtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v10.2.Export, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList.Export" TagPrefix="dxwtl" %>--%>

    <script language="javascript" type="text/javascript">


        function AtTheTimePageLoad() {
            FieldName = 'ASPxPageControl1_cmbLegalStatus';
            document.getElementById("txtName_hidden").style.display = 'none';
            document.getElementById("cmbArea").style.display = 'block';
            document.getElementById("cmbCity").style.display = 'block';
            document.getElementById("txtName").style.display = 'block';
        }
        function atpagecgange() {
            document.getElementById("txtName_hidden").style.display = 'none';
            FieldName = 'ASPxPageControl1_cmbLegalStatus';
        }
        function hidetextbox() {
            document.getElementById("txtName_hidden").style.display = 'none';
            FieldName = 'ASPxPageControl1_cmbLegalStatus';
        }
        function CallList(obj1, obj2, obj3) {
            FieldName = 'cmbCity';
            var obj4 = document.getElementById("cmbSourceType");
            var obj5 = obj4.value;
            ajax_showOptions(obj1, obj2, obj3, obj5);
        }
        function SourceTypeChange(obj) {
            if (obj == "All") {
                var txtName = document.getElementById("txtName");
                txtName.value = '';
                document.getElementById("txtName").style.display = 'none';
            }
            else {
                var txtName = document.getElementById("txtName");
                txtName.value = '';
                document.getElementById("txtName").style.display = 'inline';
            }
        }
        function StateChange(obj) {
            if (obj != 'All') {
                var sendData = 'City~' + obj;
                CallServer(sendData, "");
            }
            else {
                var cmbCity = document.getElementById("cmbCity");
                cmbCity.length = 0;
                document.getElementById("cmbCity").style.display = 'none';
                var cmbArea = document.getElementById("cmbArea");
                cmbArea.length = 0;
                document.getElementById("cmbArea").style.display = 'none';
            }
        }
        function CityChange(obj) {
            if (obj != 'All') {
                var sendData = 'Area~' + obj;
                CallServer(sendData, "");
            }
            else {
                var cmbArea = document.getElementById("cmbArea");
                cmbArea.length = 0;
                document.getElementById("cmbArea").style.display = 'none';
            }
        }
        function ReceiveServerData(rValue) {
            var DATA = rValue.split('~');
            //alert(rValue); 
            if (DATA[0] == 'City') {
                var cmbCity = document.getElementById("cmbCity");
                if (DATA[1] == 'Y') {
                    document.getElementById("cmbArea").style.display = 'none';
                    document.getElementById("cmbCity").style.display = 'inline';
                    cmbCity.length = 0;
                    var llistItem = DATA[2].split('!')
                    var i;
                    var opt1 = document.createElement("option");
                    opt1.text = 'All';
                    opt1.value = 'All';
                    cmbCity.options.add(opt1);
                    for (i = 0; i < llistItem.length; i++) {
                        var items = llistItem[i].split(',');
                        var opt = document.createElement("option");
                        opt.text = items[1];
                        opt.value = items[0];
                        cmbCity.options.add(opt);
                    }
                    document.getElementById("cmbCity").focus();
                    //CityChange(cmbCity.value);
                }
                else {
                    alert('There is no City Respect to the selected Satae!!');
                    cmbCity.length = 0;
                }
            }
            if (DATA[0] == 'Area') {
                var cmbArea = document.getElementById("cmbArea");
                if (DATA[1] == 'Y') {
                    document.getElementById("cmbArea").style.display = 'inline';
                    cmbArea.length = 0;
                    var llistItem = DATA[2].split('!')
                    var i;
                    var opt1 = document.createElement("option");
                    opt1.text = 'All';
                    opt1.value = 'All';
                    cmbArea.options.add(opt1);
                    for (i = 0; i < llistItem.length; i++) {
                        var items = llistItem[i].split(',');
                        var opt = document.createElement("option");
                        opt.text = items[1];
                        opt.value = items[0];
                        cmbArea.options.add(opt);
                    }
                    document.getElementById("cmbArea").focus();
                    //CityChange(cmbCity.value);
                }
                else {
                    alert('There is no City Respect to the selected Satae!!');
                    cmbCity.length = 0;
                }
            }
        }
    </script>
    <style>
        .col-md-2 {
            margin-bottom:10px;
        }
    </style>
    <%--    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Lead Report</h3>
        </div>
    </div>
     <div class="crossBtn"><a href="../Management/ProjectMainPage.aspx"><i class="fa fa-times"></i></a></div>
    <div class="form_main">
        <div class="row">
            <div class="col-md-2">
                <label>Report Type:</label>
                <asp:RadioButtonList ID="rdbReport" runat="server" RepeatDirection="Horizontal"
                    Width="131px">
                    <asp:ListItem Selected="True">Custom</asp:ListItem>
                    <asp:ListItem>Detail</asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div class="col-md-2">
                <label>Age Range:</label>
                <div>
                    Min: &nbsp;<asp:DropDownList ID="cmbStarYear" runat="server"></asp:DropDownList>
                    &nbsp; &nbsp;&nbsp; Max: &nbsp;<asp:DropDownList ID="cmbEndYear" runat="server"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2">
                <label>Profession:</label>
                <div>
                    <asp:DropDownList ID="cmbProfession" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2">
                <label>Marital Status:</label>
                <asp:DropDownList ID="cmbMaritalStatus" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
            <div class="col-md-2">
                <label>Legal Status:</label>
                <asp:DropDownList ID="cmbLegalStatus" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
            <div class="col-md-2">
                <label>Source Type:</label>
                <asp:DropDownList ID="cmbSourceType" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
            <div class="col-md-2">
                <label>
                    <span>Source Name:</span>
                </label>
                <div class="">
                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox><asp:TextBox ID="txtName_hidden"
                        runat="server" Width="12px" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <label>
                    <span>State:</span>
                </label>
                <div>
                    <asp:DropDownList ID="cmbState" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cmbState_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>

                </div>
            </div>
            <div class="col-md-2">
                <label>
                    <span>City:</span>
                </label>
                <div>
                    <asp:DropDownList ID="cmbCity" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cmbCity_SelectedIndexChanged" Width="100%"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2">
                <label>
                    <span>Area:</span>
                </label>
                <div>
                    <asp:DropDownList ID="cmbArea" runat="server" Width="100%"></asp:DropDownList>

                </div>
            </div>
            <div class="col-md-2">
                <label>
                    <span>From Date:</span>
                </label>
                <div>
                    <%--<asp:TextBox id="txtFromDate" tabIndex="19" runat="server" Width="146px"></asp:TextBox>&nbsp;<asp:Image id="imgFromDate" runat="server" ImageUrl="~/images/calendar.jpg"></asp:Image>--%>
                    <dxe:ASPxDateEdit ID="txtFromDate" runat="server" EditFormat="Custom" UseMaskBehavior="True" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                        <DropDownButton Text="From">
                        </DropDownButton>
                        <ValidationSettings ErrorText="Required." ErrorFrameStyle-CssClass="absolute">
                            <RequiredField IsRequired="True" />
                        </ValidationSettings>
                    </dxe:ASPxDateEdit>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFromDate"
                        Display="Dynamic" EnableTheming="True" ErrorMessage="Date required!"></asp:RequiredFieldValidator></div>
            </div>
            <div class="col-md-2">
                <label>
                    <span>To Date:</span>
                </label>
                <div>
                    <%-- <asp:TextBox id="txtToDate" tabIndex="19" runat="server" Width="146px"></asp:TextBox>&nbsp;<asp:Image id="imgToDate" runat="server" ImageUrl="~/images/calendar.jpg"></asp:Image>--%>
                    <dxe:ASPxDateEdit ID="txtToDate" runat="server" EditFormat="Custom" UseMaskBehavior="True" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                        <DropDownButton Text="To">
                        </DropDownButton>
                        <ValidationSettings ErrorText="Required." SetFocusOnError="True" ErrorFrameStyle-CssClass="absolute">
                            <RequiredField IsRequired="True" />
                        </ValidationSettings>
                    </dxe:ASPxDateEdit>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtToDate"
                        Display="Dynamic" EnableTheming="True" ErrorMessage="Date required!"></asp:RequiredFieldValidator></div>
            </div>
            <div class="clear"></div>
            <div class="col-md-12">
                <asp:Button ID="btnGetReport" runat="server" Text="Get Report" CssClass="btn btn-success" OnClick="btnGetReport_Click" />
            </div>
        </div>
        
    </div>
</asp:Content>

