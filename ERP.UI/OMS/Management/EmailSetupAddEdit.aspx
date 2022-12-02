<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.management_EmailSetupAddEdit" ValidateRequest="false" CodeBehind="EmailSetupAddEdit.aspx.cs" %>

<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script type="text/javascript">
        function ValidatePage() {


            if (document.getElementById("cmbOrganization").value == "0") {
                alert('Please Select Company!.');
                return false;
            }
            else if (document.getElementById("cmbSegment").value == "0") {
                alert('Please Select Segment!');
                return false;
            }
                //        else if(document.getElementById("txtReplyTo").value =='')
                //      {
                //       alert('Reply To Email Required!..');
                //       return false;
                //      }
            else if (document.getElementById("txtFromName").value == '') {
                alert('From Name Required!..');
                return false;
            }
                //        else if(document.getElementById("txtReplyToName").value =='')
                //      {
                //       alert('Reply To Name Required!..');
                //       return false;
                //      }




            else if (document.getElementById("txtRUserName").value == '') {
                alert('Regular Username  Required!..');
                return false;
            }
            else if (document.getElementById("txtRPassword").value == '') {
                alert('Regular Password  Required!.');
                return false;
            }
            else if (document.getElementById("txtOHost").value == '') {
                alert('Outgoing Host  Required!.');
                return false;
            }
            else if (document.getElementById("txtOPort").value == '') {
                alert('Outgoing Port  Required!.');
                return false;
            }
            else if (document.getElementById("txtIHost").value == '') {
                alert('Incoming Host  Required!.');
                return false;
            }
            else if (document.getElementById("txtIPort").value == '') {
                alert('Incoming Port  Required!.');
                return false;
            }



        }
        function Close() {
            parent.editwin.close();
        }

        function MaskMoney(evt) {
            if (!(evt.keyCode == 46 || (evt.keyCode >= 48 && evt.keyCode <= 57))) return false;
            var parts = evt.srcElement.value.split('.');
            if (parts.length > 2) return false;
            if (evt.keyCode == 46) return (parts.length == 1);
            if (parts[0].length >= 14) return false;
            if (parts.length == 2 && parts[1].length >= 2) return false;
        }
    </script>
    <script runat="server">
        //protected void Page_Load(Object Src, EventArgs E) {
        //    //if (!IsPostBack) {

        //        FreeTextBox FreeTextBox1 = new FreeTextBox();
        //        FreeTextBox1.ID = "FreeTextBox1";		
        //        FreeTextBoxPlaceHolder.Controls.Add(FreeTextBox1);

        //    //}
        //}
        //protected void SaveButton_Click(Object Src, EventArgs E)
        //{

        //    FreeTextBox FreeTextBox1 = FreeTextBoxPlaceHolder.FindControl("FreeTextBox1") as FreeTextBox;

        //    Output.Text = FreeTextBox1.Text;
        //}

    </script>
    <style type="text/css">
       #prepage {
           display:none;
       }
       .col-md-3 {
           position:relative;
       }
       .tp2 {
           position:absolute;
               right: -6px;
    top: 39px;
       }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div id="td_contact1" class="panel-title">
            
            <h3>
                <span id="lblHeadTitle">Email Setup</span>
            </h3>
            <div class="crossBtn"><a href="SettingsOptions/EmailSetup.aspx"><i class="fa fa-times"></i></a></div>
        </div>
        
    </div>
    <div class="form_main">
        <div class="clearfix" style="background: rgb(245, 244, 243); padding: 22px 0px; margin-bottom: 15px; border-radius: 4px; border: 1px solid rgb(204, 204, 204);">
            <div class="col-md-3">
                <label>Company <span style="text-align: left; font-size: medium; color: Red; width: 8px;">*</span>:</label>
                <div>
                    <asp:DropDownList ID="cmbOrganization" runat="server" Width="100%" Font-Size="11px"
                        TabIndex="1">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvCompany" runat="server" ControlToValidate="cmbOrganization" ValidationGroup="EmailGroup" InitialValue="0"
                        SetFocusOnError="true" class="tp2 fa fa-exclamation-circle iconRed" ToolTip="Mandatory"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="col-md-3">
                <label>Segment <span style="text-align: left; font-size: medium; color: Red; width: 8px;">*</span>:</label>
                <div>
                    <asp:DropDownList ID="cmbSegment" runat="server" Width="100%" Font-Size="11px" TabIndex="2">
                    </asp:DropDownList>

                    <asp:RequiredFieldValidator ID="rfvSegment" runat="server" ControlToValidate="cmbSegment" ValidationGroup="EmailGroup" InitialValue="0"
                        SetFocusOnError="true" class="tp2 fa fa-exclamation-circle iconRed" ToolTip="Mandatory"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="col-md-3">
                <label>Email Type <span style="text-align: left; font-size: medium; color: Red; width: 8px;">*</span>:</label>
                <div>
                    <asp:DropDownList ID="cmbType" runat="server" Width="100%" Font-Size="11px" TabIndex="3">
                        <asp:ListItem Text="Normal Email(N)" Value="N"></asp:ListItem>
                        <asp:ListItem Text="ECN Email(E)" Value="E"></asp:ListItem>
                        <asp:ListItem Text="Bulk Email(B)" Value="B"></asp:ListItem>
                        <asp:ListItem Text="Self Service Email(S)" Value="S"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-3">
                <label>From Name <span style="text-align: left; font-size: medium; color: Red; width: 8px;">*</span>:</label>
                <div>
                    <asp:TextBox ID="txtFromName" runat="server" Width="100%" Font-Size="11px" TabIndex="4" MaxLength="150"></asp:TextBox>

                    <asp:RequiredFieldValidator ID="rfvFromName" runat="server" ControlToValidate="txtFromName" ValidationGroup="EmailGroup"
                        SetFocusOnError="true" class="tp2 fa fa-exclamation-circle iconRed" ToolTip="Mandatory"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="col-md-3">
                <label>From Email <span style="text-align: left; font-size: medium; color: Red; width: 8px;">*</span>:</label>
                <div>
                    <asp:TextBox ID="txtRUserName" runat="server" Width="100%" Font-Size="11px" TabIndex="5" MaxLength="100"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtRUserName" SetFocusOnError="true" class="tp2 fa fa-exclamation-circle iconRed" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="EmailGroup" ToolTip="Enter Valid CC Email ID"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="rfvFromEmail" runat="server" ControlToValidate="txtRUserName" ValidationGroup="EmailGroup"
                        SetFocusOnError="true" class="tp2 fa fa-exclamation-circle iconRed" ToolTip="Mandatory"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="col-md-3">
                <label>From Email Password <span style="text-align: left; font-size: medium; color: Red; width: 8px;">*</span>:</label>
                <div>
                    <asp:TextBox ID="txtRPassword" runat="server" Width="100%" Font-Size="11px" TextMode="Password" MaxLength="20"
                        TabIndex="6"></asp:TextBox>

                    <asp:RequiredFieldValidator ID="rfvFromEmailPass" runat="server" ControlToValidate="txtRPassword" ValidationGroup="EmailGroup"
                        SetFocusOnError="true" class="tp2 fa fa-exclamation-circle iconRed" ToolTip="Mandatory"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="col-md-3">
                <label>Outgoing SMTP Host <span style="text-align: left; font-size: medium; color: Red; width: 8px;">*</span>:</label>
                <div>
                    <asp:TextBox ID="txtOHost" runat="server" Width="100%" Font-Size="11px" TabIndex="7" MaxLength="100"></asp:TextBox>

                    <asp:RequiredFieldValidator ID="rfvOutgoingSMTP" runat="server" ControlToValidate="txtOHost" ValidationGroup="EmailGroup"
                        SetFocusOnError="true" class="tp2 fa fa-exclamation-circle iconRed" ToolTip="Mandatory"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="col-md-3">
                <label>Outgoing Port <span style="text-align: left; font-size: medium; color: Red; width: 8px;">*</span>:</label>
                <div>
                    <asp:TextBox ID="txtOPort" runat="server" Width="100%" Font-Size="11px" TabIndex="8" MaxLength="10"></asp:TextBox>

                    <asp:RequiredFieldValidator ID="rfvOutgoingPort" runat="server" ControlToValidate="txtOPort" ValidationGroup="EmailGroup"
                        SetFocusOnError="true" class="tp2 fa fa-exclamation-circle iconRed" ToolTip="Mandatory"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="col-md-3">
                <label>Incoming SMTP Host <span style="text-align: left; font-size: medium; color: Red; width: 8px;">*</span>:</label>
                <div>
                    <asp:TextBox ID="txtIHost" runat="server" Width="100%" Font-Size="11px" TabIndex="9" MaxLength="100"></asp:TextBox>

                    <asp:RequiredFieldValidator ID="rfvIncomingSMTP" runat="server" ControlToValidate="txtIHost" ValidationGroup="EmailGroup"
                        SetFocusOnError="true" class="tp2 fa fa-exclamation-circle iconRed" ToolTip="Mandatory"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="col-md-3">
                <label>Incoming Port <span style="text-align: left; font-size: medium; color: Red; width: 8px;">*</span>:</label>
                <div>
                    <asp:TextBox ID="txtIPort" runat="server" Width="100%" Font-Size="11px" TabIndex="10" MaxLength="10"></asp:TextBox>

                    <asp:RequiredFieldValidator ID="rfvIncomingPort" runat="server" ControlToValidate="txtIPort" ValidationGroup="EmailGroup"
                        SetFocusOnError="true" class="tp2 fa fa-exclamation-circle iconRed" ToolTip="Mandatory"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="col-md-3">
                <label>SSL Mode:</label>
                <div>
                    <asp:DropDownList ID="cmbSSL" runat="server" Width="100%" Font-Size="11px" TabIndex="11">
                        <asp:ListItem Text="True" Value="true"></asp:ListItem>
                        <asp:ListItem Text="False" Value="false"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-3">
                <label>Is Active:</label>
                <div>
                    <asp:DropDownList ID="cmbStatus" runat="server" Width="100%" Font-Size="11px" TabIndex="12">
                        <asp:ListItem Text="Active" Value="Y"></asp:ListItem>
                        <asp:ListItem Text="Deactive" Value="N"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div><div class="clear"></div>
            <div class="col-md-3">
                <label>Reply To Name:</label>
                <div>
                    <asp:TextBox ID="txtReplyToName" runat="server" Width="100%" Font-Size="11px" MaxLength="150"
                        TabIndex="13"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-3">
                <label>Reply To Email:</label>
                <div>
                    <asp:TextBox ID="txtReplyTo" runat="server" Width="100%" Font-Size="11px" TabIndex="14" MaxLength="100"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="revReplyEmail" runat="server" ControlToValidate="txtReplyTo" SetFocusOnError="true" class="tp2 fa fa-exclamation-circle iconRed" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="EmailGroup" ToolTip="Enter Valid CC Email ID"></asp:RegularExpressionValidator>
                </div>
            </div>
            <div class="clear"></div>
            <div class="col-md-12">
                <label>Disclaimer:</label>
                <div>
                    <div>

                        <asp:PlaceHolder ID="FreeTextBoxPlaceHolder" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-md-12" style="padding-top:15px">
                
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click"
                    TabIndex="15" ValidationGroup="EmailGroup" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger" PostBackUrl="~/OMS/Management/SettingsOptions/EmailSetup.aspx"
                    TabIndex="16" />
          
            </div>
        </div>
    </div>
       
</asp:Content>
