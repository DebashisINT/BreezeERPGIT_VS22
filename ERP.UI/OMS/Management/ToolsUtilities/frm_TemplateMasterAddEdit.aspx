<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.ToolsUtilities.management_utilities_frm_TemplateMasterAddEdit" ValidateRequest="false" CodeBehind="frm_TemplateMasterAddEdit.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <script type="text/javascript">
        function calldispose() {

            var type = document.getElementById("cmbType").value;
            var y = (screen.availHeight - 450) / 2;
            var x = (screen.availWidth - 900) / 2;
            var str = 'frm_TemplateReservedWord.aspx?Type=' + type;
            //  editwin=dhtmlmodal.open("Editbox", "iframe", str, "Bill For - "+type+"", "width=640px,height=350px,center=1,resize=1,top=500", "recal");    
            window.open(str, "Search_Conformation_Box", "height=350,width=700,top=" + y + ",left=" + x + ",location=no,directories=no,menubar=no,toolbar=no,status=yes,scrollbars=no,resizable=no,dependent=no");


        }
        function calback() {
            alert("123");
        }
        function Close() {
            parent.editwin.close();
        }
        function TypeSet(obj) {
            if (obj == 'ND') {
                document.getElementById("trFName").style.display = "none";
                document.getElementById("trMName").style.display = "none";
                document.getElementById("trLName").style.display = "none";
                document.getElementById("trCode").style.display = "inline";
                document.getElementById("trAdd1").style.display = "inline";
                document.getElementById("trAdd2").style.display = "inline";
                document.getElementById("trAdd3").style.display = "inline";
                document.getElementById("trCity").style.display = "inline";
                document.getElementById("trState").style.display = "none";
                document.getElementById("trCountry").style.display = "none";
                document.getElementById("trPIN").style.display = "inline";
                document.getElementById("trISD").style.display = "none";
                document.getElementById("trSTD").style.display = "none";
                document.getElementById("trPhone").style.display = "inline";
                document.getElementById("trMob").style.display = "none";
                document.getElementById("trPAN").style.display = "inline";
                document.getElementById("trDOB").style.display = "none";
                document.getElementById("trClientName").style.display = "inline";


                document.getElementById("trSalutaion").style.display = "none";

            }
            else if (obj == 'CD') {
                document.getElementById("trFName").style.display = "none";
                document.getElementById("trMName").style.display = "none";
                document.getElementById("trLName").style.display = "none";
                document.getElementById("trCode").style.display = "inline";
                document.getElementById("trAdd1").style.display = "inline";
                document.getElementById("trAdd2").style.display = "inline";
                document.getElementById("trAdd3").style.display = "inline";
                document.getElementById("trCity").style.display = "inline";
                document.getElementById("trState").style.display = "inline";
                document.getElementById("trCountry").style.display = "none";
                document.getElementById("trPIN").style.display = "inline";
                document.getElementById("trISD").style.display = "none";
                document.getElementById("trSTD").style.display = "none";
                document.getElementById("trPhone").style.display = "inline";
                document.getElementById("trMob").style.display = "none";
                document.getElementById("trPAN").style.display = "inline";
                document.getElementById("trDOB").style.display = "none";
                document.getElementById("trClientName").style.display = "inline";

                document.getElementById("trSalutaion").style.display = "none";
            }
            else {
                document.getElementById("trFName").style.display = "inline";
                document.getElementById("trMName").style.display = "inline";
                document.getElementById("trLName").style.display = "inline";
                document.getElementById("trCode").style.display = "inline";
                document.getElementById("trAdd1").style.display = "inline";
                document.getElementById("trAdd2").style.display = "inline";
                document.getElementById("trAdd3").style.display = "inline";
                document.getElementById("trCity").style.display = "inline";
                document.getElementById("trState").style.display = "inline";
                document.getElementById("trCountry").style.display = "inline";
                document.getElementById("trPIN").style.display = "inline";
                document.getElementById("trISD").style.display = "inline";
                document.getElementById("trSTD").style.display = "inline";
                document.getElementById("trPhone").style.display = "inline";
                document.getElementById("trMob").style.display = "inline";
                document.getElementById("trPAN").style.display = "inline";
                document.getElementById("trDOB").style.display = "inline";
                document.getElementById("trClientName").style.display = "none";

                document.getElementById("trSalutaion").style.display = "inline";
            }

        }
        function PostReservedWord(obj) {
            var body = $("#FreeTextBox1_designEditor").contents().find("body").html() + '<b>' + obj + '</b>';
            $("#FreeTextBox1_designEditor").contents().find("body").html(body);
        }
    </script>
    <style>
        .FreeTextBox1_OuterTable {
            width: 100% !important;
        }

        #myDiv span {
            margin-top: 20px;
            background: #0176c5;
            color: #fff !important;
            border: 1px solid #094b77 !important;
            margin-right: 2px;
            padding: 3px 5px;
            ;
        }

            #myDiv span:hover, #myDiv span:focus {
                background: #0a619c;
                box-shadow: none;
            }

        .pullleftClass {
            position: absolute;
            right: -5px;
            top: 28px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Template Master</h3>
        </div>
    </div>
    <div class="form_main" style="height: 800px;">
        <%--     <asp:ScriptManager ID="ScriptManager1" runat="server"  		#FAF8CC B7CEEC AsyncPostBackTimeout="3600">
            </asp:ScriptManager>--%>
        <table class="TableMain100">
            <tr>
                <td style="vertical-align: top; padding-left: 30px;">
                    <div class="crossBtn"><a href="frm_TemplateMaster.aspx"><i class="fa fa-times"></i></a></div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div>
                        <div class="col-md-3">
                            <label>Template Used For:</label>
                            <asp:DropDownList ID="cmbType" runat="server" Width="100%" Font-Size="11px" TabIndex="1"
                                onchange="TypeSet(this.value)">
                                <asp:ListItem Value="CL" Text="Customer"></asp:ListItem>
                                <asp:ListItem Value="ND" Text="NSDL Clients"></asp:ListItem>
                                <asp:ListItem Value="CD" Text="CDSL Clients"></asp:ListItem>
                                <asp:ListItem Value="EM" Text="Employee"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <label>Short Name:</label>
                            <asp:TextBox ID="txtShortName" runat="server" Width="100%" Font-Size="11px" TabIndex="2" MaxLength="250"></asp:TextBox>
                        </div>
                        <div class="clear"></div>
                        <div class="col-md-7">
                            <asp:PlaceHolder ID="FreeTextBoxPlaceHolder" runat="server" />
                        </div>
                        <div class="col-md-5">
                            <%--style="background-color: #F2F5A9; border: solid 1px black;" cellpadding="1"
                                cellspacing="1" border="1"--%>
                            <div id="myDiv">
                                <table>
                                    <tr id="trSalutaion" runat="server">
                                        <td>
                                            <asp:Label Text="#Salutation#" ID="Label19" runat="server" onclick="PostReservedWord('#Salutation#');"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trClientName" runat="server">
                                        <td>
                                            <asp:Label Text="#ClientName#" ID="Label18" runat="server" onclick="PostReservedWord('#ClientName#');"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trFName" runat="server">
                                        <td>
                                            <asp:Label Text="#FirstName#" ID="textTE" runat="server" onclick="PostReservedWord('#FirstName#');"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trMName" runat="server">
                                        <td>
                                            <asp:Label Text="#MiddleName#" ID="Label1" runat="server" onclick="PostReservedWord('#MiddleName#');"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trLName" runat="server">
                                        <td>
                                            <asp:Label Text="#LastName#" ID="Label2" runat="server" onclick="PostReservedWord('#LastName#');"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trCode" runat="server">
                                        <td>
                                            <asp:Label Text="#ClientID#" ID="Label3" runat="server" onclick="PostReservedWord('#ClientID#');"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trAdd1" runat="server">
                                        <td>
                                            <asp:Label Text="#Addres1#" ID="Label4" runat="server" onclick="PostReservedWord('#Addres1#');"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trAdd2" runat="server">
                                        <td>
                                            <asp:Label Text="#Addres2#" ID="Label5" runat="server" onclick="PostReservedWord('#Addres2#');"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trAdd3" runat="server">
                                        <td>
                                            <asp:Label Text="#Addres3#" ID="Label6" runat="server" onclick="PostReservedWord('#Addres3#');"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trCity" runat="server">
                                        <td>
                                            <asp:Label Text="#City#" ID="Label7" runat="server" onclick="PostReservedWord('#City#');"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trState" runat="server">
                                        <td>
                                            <asp:Label Text="#State#" ID="Label8" runat="server" onclick="PostReservedWord('#State#');"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trCountry" runat="server">
                                        <td>
                                            <asp:Label Text="#Country#" ID="Label9" runat="server" onclick="PostReservedWord('#Country#');"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trPIN" runat="server">
                                        <td>
                                            <asp:Label Text="#Pin#" ID="Label10" runat="server" onclick="PostReservedWord('#Pin#');"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trISD" runat="server">
                                        <td>
                                            <asp:Label Text="#ISDCode#" ID="Label11" runat="server" onclick="PostReservedWord('#ISDCode#');"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trSTD" runat="server">
                                        <td>
                                            <asp:Label Text="#STDCode#" ID="Label12" runat="server" onclick="PostReservedWord('#STDCode#');"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trPhone" runat="server">
                                        <td>
                                            <asp:Label Text="#TelephoneNumber#" ID="Label13" runat="server" onclick="PostReservedWord('#TelephoneNumber#');"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trMob" runat="server">
                                        <td>
                                            <asp:Label Text="#MobNumber#" ID="Label14" runat="server" onclick="PostReservedWord('#MobNumber#');"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trDOB" runat="server">
                                        <td>
                                            <asp:Label Text="#DateOfBirth#" ID="Label15" runat="server" onclick="PostReservedWord('#DateOfBirth#');"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trPAN" runat="server">
                                        <td>
                                            <asp:Label Text="#PANNumber#" ID="Label16" runat="server" onclick="PostReservedWord('#PANNumber#');"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trDate" runat="server">
                                        <td>
                                            <asp:Label Text="#CurrentDate#" ID="Label17" runat="server" onclick="PostReservedWord('#CurrentDate#');"></asp:Label>
                                        </td>
                                    </tr>

                                </table>
                            </div>
                        </div>
                        <div class="clear"></div>
                        <div class="col-md-12" style="padding-top: 15px;">
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary btnUpdate" OnClick="btnSave_Click"
                                TabIndex="3" ValidationGroup="a" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger btnUpdate" OnClick="btnCancel_Click"
                                TabIndex="4" />
                        </div>
                    </div>

                </td>
            </tr>
            <tr id="TrUploadFile">
                <td class="gridcellleft" valign="top">
                    <%--   <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>--%>

                    <%--         </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave"  EventName="Click"  />
                        </Triggers>
                    </asp:UpdatePanel>--%>
                </td>
                <td>
                    <div>
                    </div>
                </td>
            </tr>

        </table>
    </div>
</asp:Content>
