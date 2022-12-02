<%@ Page Language="C#" AutoEventWireup="true"   MasterPageFile="~/OMS/MasterPage/ERP.Master"  Inherits="ERP.OMS.Management.management_verify_documentremarks" CodeBehind="verify_documentremarks.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script lang="javascript" type="text/javascript">

        FieldName = 'abcd';
        //   function CallAjax(obj1,obj2,obj3)
        //         { 
        //        // alert ('1');
        //            ajax_showOptions(obj1,obj2,obj3);
        //         }
        // Fieldname = 'none'
        //function SignOff() {
        //    window.parent.SignOff();
        //}

        ////window.onload = autoStartTimer;
        //function height() {
        //    if (document.body.scrollHeight >= 300)
        //        window.frameElement.height = document.body.scrollHeight;
        //    else
        //        window.frameElement.height = '300px';
        //    window.frameElement.Width = document.body.scrollWidth;
        //}
        function Page_Load() {
            //document.getElementById('Div1').style.display="none";
        }
        function FillValues(obj) {
            parent.editwin.close(obj);
        }
        $(document).ready(function () {
           
            var prvurl = '<%=Session["PageRedirect"] %>';
            if (prvurl == '')
            {
                prvurl = '<%=ViewState["previousPageUrl"] %>';
                $('.crossBtn a').attr('href', prvurl)
            }
            else
            {
                $('.crossBtn a').attr('href', prvurl)
            }
        });
         
        <%--="<%=Session["PageRedirect"] %>"--%>

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Verify Remarks</h3>
            <div class="crossBtn"><a href=""><i class="fa fa-times"></i></a></div>
        </div>

    </div>
    <div class="form_main">
        <%--    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3600">
        </asp:ScriptManager>--%>
        <%-- <table class="TableMain100">
            <tr>
                <td class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099">Verify Remarks</span></strong>
                </td>
            </tr>
        </table>--%>

        <table style="border: solid 1px white; width: 400px; text-align: left;">

            <tr>
                <td>
                    <table>
                        <tr>
                            <%--<td style="width: 150px; text-align: right;">
                                                       <span id="Span1" class="Ecoheadtxt" style="text-align:right;">Content :</span>
                                                        </td>--%>
                            <td style="width: 60%" align="left">
                                <asp:TextBox ID="txtReportTo" runat="server" Width="600px" TextMode="MultiLine" Height="50px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <%--            <tr style="height: 10px;">
            </tr>--%>
            <tr>
                <td>
                    <asp:Button ID="btnYes" runat="server" CssClass="btn btn-primary" OnClientClick="f2()" Text="Save" OnClick="btnYes_Click" ValidationGroup="a" />
                    <asp:Button ID="btnNo" runat="server" CssClass="btn btn-danger" Text="Cancel" OnClick="btnNo_Click" />

                    <%--                    <table>
                        <tr>
                            <td style="width: 60px;"></td>
                            <td align="left" id="td_yes" runat="server">
                                <asp:Button ID="btnYes" runat="server" CssClass="btn btn-primary" OnClientClick="f2()" Text="Save" OnClick="btnYes_Click" ValidationGroup="a" />
                            </td>
                            <td style="width: 20px;"></td>
                            <td align="left" id="td_no" runat="server">
                                <asp:Button ID="btnNo" runat="server" CssClass="btn btn-danger" Text="Cancel" OnClick="btnNo_Click" />

                            </td>
                        </tr>
                    </table>--%>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>


