<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.ToolsUtilities.management_toolsutilities_Frm_UploadCommonFolder" Codebehind="Frm_UploadCommonFolder.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    

    

    <script type="text/javascript" language="javascript">
    function dhtmlclose()
    {
   
    parent.editwin.close();
    }
    
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div>
            <table width="100%" style="border: solid 1px white;">
                <tr>
                    <td class="EHEADER" style="text-align: center;">
                        <strong><span style="color: #000099">Upload File into Common Folder</span></strong>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <table>
                            <tr>
                                <td class="gridcellright">
                                   <strong><span style="color: #000099"> Upload File: </span></strong>
                                </td>
                                <td>
                                    :</td>
                                <td class="gridcellleft">
                                    <asp:FileUpload ID="OFDSelectFile" runat="server" Width="372px" /></td>
                                <td align="LEFT">
                                    <asp:Button ID="btnAddForm" runat="Server" Text="Upload"   Width="100px" CssClass="btnUpdate"
                                        OnClick="btnAddForm_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
</asp:Content>
