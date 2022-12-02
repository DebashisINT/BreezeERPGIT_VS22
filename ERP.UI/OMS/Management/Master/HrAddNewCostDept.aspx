<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_master_HrAddNewCostDept" CodeBehind="HrAddNewCostDept.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <!--JS Inline Method-->
    <script language="javascript" type="text/javascript">
        FieldName = "DontKnow";
        //function SignOff()
        //{
        //    window.parent.SignOff();
        //}
        //function height()
        //{        
        //   if(document.body.scrollHeight>=350)
        //    window.frameElement.height = document.body.scrollHeight;
        //   else
        //    window.frameElement.height = '350px';
        //    window.frameElement.Width = document.body.scrollWidth;
        //}
        function Message(obj) {
            if (obj == 1) {
                alert("Successfully Inserted");
                editwin.close();
            }
            else
                alert("There is Any Problem To Save Data!!!\n Please Try Again");
        }
        function Cancel() {
            alert('111')
            window.location.href = "HRCostCenter.aspx";

        }
    </script>
    <style>
        #RequiredFieldValidator1, #revEmailID {
            position:absolute;
            right: -5px;
            top: 7px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Add Cost Centers/Departments</h3>
            <div class="crossBtn"><a href="HRCostCenter.aspx"><i class="fa fa-times"></i></a></div>
        </div>

    </div>
    <div class="form_main">

        <div class="form_main" style="border: 1px solid #ccc">
            <table>
                <tr>
                    <td style="height: 277px">
                        <table class="pdtble" width="" style="" border="0">
                            <tr>
                                <td style="text-align: left; vertical-align: top; height: 11px;width:190px">Cost Center/Department Name<span style="color: red">*</span>
                                </td>
                                <td  style="position:relative;width:260px">
                                    <asp:TextBox ID="TxtCenter" runat="server" Width="250px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="" ToolTip="Mandatory" ForeColor="Red" CssClass="pullleftClass fa fa-exclamation-circle spl" ControlToValidate="TxtCenter" Display="Dynamic"></asp:RequiredFieldValidator>
                                </td>


                            </tr>
                            <tr>
                                <td style="text-align: left; height: 11px; vertical-align: top; width: 190px">Cost Center/Department Type</td>
                                <td style="height: 24px; ">
                                    <asp:DropDownList ID="DDLType" runat="server" Width="250px">
                                        <asp:ListItem>Department</asp:ListItem>
                                        <asp:ListItem>Employee</asp:ListItem>
                                        <asp:ListItem>Branch</asp:ListItem>
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="text-align: left; vertical-align: top; height: 11px;">Parent Cost Center/Department</td>
                                <td style="">
                                    <asp:DropDownList ID="DDLCostDept" runat="server" Width="250px">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="text-align: left; vertical-align: top; height: 11px;">Head Of Department</td>
                                <td style="">
                                    <asp:DropDownList ID="DDLHeadDept" runat="server" Width="250px">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtDptHead" runat="server" Width="250px" Style="display: none"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="text-align: left; vertical-align: top; height: 11px;">Email ID</td>
                                <td style="position:relative;width:260px">
                                    <asp:TextBox ID="TxtEmail" runat="server" Width="250px"></asp:TextBox>

                                    <asp:RegularExpressionValidator ID="revEmailID" runat="server"
                                        ControlToValidate="TxtEmail"  ToolTip="Invalid Email" ForeColor="Red" CssClass="pullleftClass fa fa-exclamation-circle spl"  Display="Dynamic"
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>

                                </td>
                            </tr>
                            <tr style="display:none;">
                                <td style="vertical-align: top; height: 11px; text-align: left"></td>
                                <td style=" padding: 0">
                                    <table style="width: 250px">
                                        <tr>
                                            <td style="width: 100px">
                                                <asp:CheckBox ID="ChkFund" runat="server" Text="Mutual Funds" Width="124px" />
                                            </td>
                                            <td style="width: 100px">
                                                <asp:CheckBox ID="ChkBrok" runat="server" Text="Broking " />
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="ChkInsu" runat="server" Text="Insurance" /></td>
                                            <td>
                                                <asp:CheckBox ID="ChkDepos" runat="server" Text="Depository" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; vertical-align: top; height: 11px;">&nbsp;</td>
                                <td style="">
                                    <asp:Label ID="lblmessage" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; height: 11px; text-align: left">&nbsp;</td>
                                <td style="">&nbsp;<asp:Button ID="BtnSave" runat="server" CssClass="btn btn-primary" Text="Save" CausesValidation="true" OnClick="BtnSave_Click" />
                                    <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-danger" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField ID="txtDptHead_hidden" runat="Server" />
</asp:Content>
