<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/PopUp.Master" Inherits="ERP.OMS.Management.SettingsOptions.management_SettingsOptions_HeaderFooterDetails" ValidateRequest="false" CodeBehind="HeaderFooterDetails.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">
        //function SignOff() {
        //    window.parent.SignOff();
        //}
    </script>
    

    
    <style type="text/css">
        /* Big box with list of options */
        #ajax_listOfOptions {
            position: absolute; /* Never change this one */
            width: 50px; /* Width of box */
            height: auto; /* Height of box */
            overflow: auto; /* Scrolling features */
            border: 1px solid Blue; /* Blue border */
            background-color: #FFF; /* White background color */
            text-align: left;
            font-size: 0.9em;
            z-index: 32767;
        }

            #ajax_listOfOptions div { /* General rule for both .optionDiv and .optionDivSelected */
                margin: 1px;
                padding: 1px;
                cursor: pointer;
                font-size: 0.9em;
            }

            #ajax_listOfOptions .optionDiv { /* Div for each item in list */
            }

            #ajax_listOfOptions .optionDivSelected { /* Selected item in the list */
                background-color: #DDECFE;
                color: Blue;
            }

        #ajax_listOfOptions_iframe {
            background-color: #F00;
            position: absolute;
            z-index: 3000;
        }

        form {
            display: inline;
        }
    </style>
</asp:Content>
    

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100">
            <tr>
                <td class="gridcellleft" style="width: 100px">
                    <span style="color: #000099">Header/Footer<span style="color: #000000"> :</span></span>
                </td>
                <td class="gridcellleft" style="width: 791px">
                    <asp:DropDownList ID="ddlHeaderFooter" runat="server" Width="190px">
                        <asp:ListItem Value="H">Header</asp:ListItem>
                        <asp:ListItem Value="F">Footer</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="gridcellleft" style="width: 100px">
                    <span style="color: #000099">Heading<span style="color: #000000"> :</span></span>
                </td>
                <td class="gridcellleft" style="width: 791px">
                    <asp:TextBox ID="txtHeading" runat="server" Width="757px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="gridcellleft" style="width: 100px; height: 410px;">
                    <span style="color: #000099">Content<span style="color: #000000"> :</span></span>
                </td>
                <td class="gridcellleft" style="height: 410px; width: 791px;">
                    <%-- <asp:TextBox ID="txtContent" runat="server" Width="756px" Height="400px" TextMode="MultiLine"></asp:TextBox>--%>
                    <div>
                        <asp:PlaceHolder ID="FreeTextBoxPlaceHolder" runat="server" />
                    </div>
                </td>
            </tr>

            <tr>
                <td class="gridcellcenter" style="height: 34px">
                    <%--<input id="btnSave" type="button" value="Save" class="btnUpdate" onclick="btnSave_Click()"
                    style="width: 60px" tabindex="33" />
                <input id="btnCancel" type="button" value="Cancel" class="btnUpdate" onclick="btnCancel_Click()"
                    style="width: 60px" tabindex="34" />--%>
                </td>
                <td style="height: 34px; text-align: right; width: 791px;">
                    <dxe:ASPxButton ID="btnSave" runat="server" AutoPostBack="False" CssClass="btn btn-primary" Text="Save" CssFilePath="~/App_Themes/Office2003 Blue/{0}/styles.css"
                        CssPostfix="Office2003_Blue" Height="3px" Width="54px" OnClick="btnSave_Click">
                    </dxe:ASPxButton>
                    <asp:HiddenField ID="hdnID" runat="server" />
                </td>
            </tr>

        </table>
    </div>
</asp:Content>

