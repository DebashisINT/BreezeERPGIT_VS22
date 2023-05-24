<%--================================================== Revision History =============================================
1.0   Pallab    V2.0.38      24-05-2023          0026208: Email Setup module design modification & check in small device
====================================================== Revision History =============================================--%>

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

    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    
    <style>
        select
        {
            z-index: 1;
            margin-bottom: 0;
        }

        /*#grid {
            max-width: 98% !important;
        }*/
        #FormDate , #toDate , #dtTDate , #dt_PLQuote , #dt_partyInvDt
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #dt_PLQuote_B-1 , #dt_partyInvDt_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #dt_PLQuote_B-1 #dt_PLQuote_B-1Img ,
        #dt_partyInvDt_B-1 #dt_partyInvDt_B-1Img
        {
            display: none;
        }

        /*select
        {
            -webkit-appearance: auto;
        }*/

        .calendar-icon
        {
                right: 18px;
                bottom: 6px;
        }
        .padTabtype2 > tbody > tr > td
        {
            vertical-align: bottom;
        }
        #rdl_Salesquotation
        {
            margin-top: 0px;
        }

        .lblmTop8>span, .lblmTop8>label
        {
            margin-top: 8px !important;
        }

        .col-md-2, .col-md-4 {
    margin-bottom: 5px;
}

        .simple-select::after
        {
                top: 34px;
            right: 13px;
        }

        .dxeErrorFrameWithoutError_PlasticBlue.dxeControlsCell_PlasticBlue
        {
            padding: 0;
        }

        .aspNetDisabled
        {
            background: #f3f3f3 !important;
        }

        .backSelect {
    background: #42b39e !important;
}

        #ddlInventory
        {
                -webkit-appearance: auto;
        }

        /*.wid-90
        {
            width: 100%;
        }
        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-content
        {
            width: 97%;
        }*/
        .newLbl
        {
                margin: 3px 0 !important;
        }

        .lblBot > span, .lblBot > label
        {
                margin-bottom: 3px !important;
        }

        .col-md-2 > label, .col-md-2 > span, .col-md-1 > label, .col-md-1 > span
        {
            margin-top: 8px !important;
            font-size: 14px;
        }

        .col-md-6 span
        {
            font-size: 14px;
        }

        #gridDEstination
        {
            width:99% !important;
        }

        #txtEntity , #txtCustName
        {
            width: 100%;
        }
        .col-md-6 span
        {
            margin-top: 8px !important;
        }

        .rds
        {
            margin-top: 10px !important;
        }

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue , select
        {
            height: 30px !important;
            
        }
        select
        {
            background-color: transparent;
                padding: 0 20px 0 5px !important;
        }

        .newLbl
        {
            font-size: 14px;
            margin: 3px 0 !important;
            font-weight: 500 !important;
            margin-bottom: 0 !important;
            line-height: 20px;
        }

        .crossBtn {
            top: 25px !important;
            right: 25px !important;
        }

        .wrapHolder
        {
            height: 60px;
        }
        #rdl_SaleInvoice
        {
            margin-top: 12px;
        }

        .dxeRoot_PlasticBlue
        {
            width: 100% !important;
        }

        @media only screen and (max-width: 1380px) and (min-width: 1300px)
        {
            .col-xs-1, .col-xs-2, .col-xs-3, .col-xs-4, .col-xs-5, .col-xs-6, .col-xs-7, .col-xs-8, .col-xs-9, .col-xs-10, .col-xs-11, .col-xs-12, .col-sm-1, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-sm-10, .col-sm-11, .col-sm-12, .col-md-1, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-md-10, .col-md-11, .col-md-12, .col-lg-1, .col-lg-2, .col-lg-3, .col-lg-4, .col-lg-5, .col-lg-6, .col-lg-7, .col-lg-8, .col-lg-9, .col-lg-10, .col-lg-11, .col-lg-12
            {
                 padding-right: 10px;
                 padding-left: 10px;
            }
            .simple-select::after
        {
                top: 34px;
            right: 8px;
        }
            .calendar-icon
        {
                right: 14px;
                bottom: 6px;
        }
        }

    </style>
    <%--Rev end 1.0--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading">
        <div id="td_contact1" class="panel-title">
            
            <h3>
                <span id="lblHeadTitle">Email Setup</span>
            </h3>
            <div class="crossBtn"><a href="SettingsOptions/EmailSetup.aspx"><i class="fa fa-times"></i></a></div>
        </div>
        
    </div>
        <div class="form_main">
        <div class="clearfix" style=" padding: 22px 0px; margin-bottom: 15px; border-radius: 4px; border: 1px solid rgb(204, 204, 204);">
            <%--Rev 1.0: "simple-select" class add --%>
            <div class="col-md-3 simple-select">
                <label>Company <span style="text-align: left; font-size: medium; color: Red; width: 8px;">*</span>:</label>
                <div>
                    <asp:DropDownList ID="cmbOrganization" runat="server" Width="100%" Font-Size="11px"
                        TabIndex="1">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvCompany" runat="server" ControlToValidate="cmbOrganization" ValidationGroup="EmailGroup" InitialValue="0"
                        SetFocusOnError="true" class="tp2 fa fa-exclamation-circle iconRed" ToolTip="Mandatory"></asp:RequiredFieldValidator>
                </div>
            </div>
            <%--Rev 1.0: "simple-select" class add --%>
            <div class="col-md-3 simple-select">
                <label>Segment <span style="text-align: left; font-size: medium; color: Red; width: 8px;">*</span>:</label>
                <div>
                    <asp:DropDownList ID="cmbSegment" runat="server" Width="100%" Font-Size="11px" TabIndex="2">
                    </asp:DropDownList>

                    <asp:RequiredFieldValidator ID="rfvSegment" runat="server" ControlToValidate="cmbSegment" ValidationGroup="EmailGroup" InitialValue="0"
                        SetFocusOnError="true" class="tp2 fa fa-exclamation-circle iconRed" ToolTip="Mandatory"></asp:RequiredFieldValidator>
                </div>
            </div>
            <%--Rev 1.0: "simple-select" class add --%>
            <div class="col-md-3 simple-select">
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
            <%--Rev 1.0: "simple-select" class add --%>
            <div class="col-md-3 simple-select">
                <label>SSL Mode:</label>
                <div>
                    <asp:DropDownList ID="cmbSSL" runat="server" Width="100%" Font-Size="11px" TabIndex="11">
                        <asp:ListItem Text="True" Value="true"></asp:ListItem>
                        <asp:ListItem Text="False" Value="false"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <%--Rev 1.0: "simple-select" class add --%>
            <div class="col-md-3 simple-select">
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
    </div>   
</asp:Content>
