<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.management_frmAddressPrint_popup" EnableEventValidation="false" CodeBehind="frmAddressPrint_popup.aspx.cs" %>

<%@ Import Namespace="System.Drawing" %>
<%@ Import Namespace="System.Drawing.Imaging" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.OleDb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxdv" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>--%>
    <script type="text/javascript">
        function onPage() {
            window.focus();
            self.print();
        }
        function height() {

            if (document.body.scrollHeight >= 600)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '700px';
            window.frameElement.Width = document.body.scrollWidth;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <dxe:ASPxDataView ID="ASPxDataView1" runat="server" PagerPanelSpacing="0px" ContentStyle-Font-Names="Times New Roman" ContentStyle-Font-Size="Medium" Layout="Table" RowPerPage="7" BackColor="#d4d0c8">
            <ItemTemplate>
                <%# Eval("Add1")%>
            </ItemTemplate>

            <PagerSettings ShowSeparators="True">

                <AllButton>
                    <Image Height="19px" Width="30px" />

                </AllButton>
                <FirstPageButton>
                    <Image Height="19px" Width="22px" />
                </FirstPageButton>
                <LastPageButton>
                    <Image Height="19px" Width="22px" />
                </LastPageButton>
                <NextPageButton>
                    <Image Height="19px" Width="20px" />
                </NextPageButton>
                <PrevPageButton>
                    <Image Height="19px" Width="20px" />
                </PrevPageButton>
            </PagerSettings>

            <ItemStyle Height="5px" HorizontalAlign="Justify">
                <Paddings Padding="0px" PaddingBottom="2px" PaddingLeft="0px" PaddingRight="0px"
                    PaddingTop="2px" />
                <Border BorderColor="Beige" BorderStyle="Solid" BorderWidth="1px" />
            </ItemStyle>
        </dxe:ASPxDataView>
    </div>
</asp:Content>

