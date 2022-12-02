<%@ Control Language="C#" AutoEventWireup="true" Inherits="ERP.OMS.Control.Control_CallBackPager" Codebehind="CallBackPager.ascx.cs" %>
<table style="width: 100%">
    <tr>
        <td valign="top">
            Page</td>
        <td valign="top" >
            <b style="text-align:right" id="B_PageNo" runat="server"></b>
        </td>
        <td valign="top" >
         Of   
        </td>
        <td valign="top" >
            <b style="text-align:right" id="B_TotalPage" runat="server"></b>
        </td>
        <td valign="top" >
        ( <b style="text-align:right" id="B_TotalRows" runat="server"></b> items )
        </td>
        <td valign="top" >
            <table width="100%">
              <tr>
                    <td  valign="top" >
                        <asp:Image ID="ImgLNav" runat="server" />
                    </td>
                    <td  valign="top" >
                    <a runat="server" href="javascript:void(0);" onclick="#">
                                            1</a>
                    </td>
                    <td  valign="top" >
                    <a runat="server" href="javascript:void(0);" onclick="#">
                                            2</a>
                    </td>
                    <td  valign="top" >
                    <a  runat="server" href="javascript:void(0);" onclick="#">
                                            3</a>
                    </td>
                    <td  valign="top" >
                    <a  runat="server" href="javascript:void(0);" onclick="#">
                                            4</a>
                    </td>
                    <td  valign="top" >
                    <a  runat="server" href="javascript:void(0);" onclick="#">
                                            5</a>
                    </td>
                    <td  valign="top" >
                    <a  runat="server" href="javascript:void(0);" onclick="#">
                                            6</a>
                    </td>
                    <td  valign="top" >
                    <a  runat="server" href="javascript:void(0);" onclick="#">
                                            7</a>
                    </td>
                    <td  valign="top" >
                    <a runat="server" href="javascript:void(0);" onclick="#">
                                            8</a>
                    </td>
                    <td  valign="top" >
                    <a  runat="server" href="javascript:void(0);" onclick="#">
                                            9</a>
                    </td>
                    <td  valign="top" >
                    <a runat="server" href="javascript:void(0);" onclick="#">
                                            10</a>
                    </td>
                    <td style="text-align: right" valign="top" >
                         <asp:Image ID="ImgRNav" runat="server" />
                    </td>
                    
                </tr>
            </table>
        
        </td>
       
    </tr>
</table>
