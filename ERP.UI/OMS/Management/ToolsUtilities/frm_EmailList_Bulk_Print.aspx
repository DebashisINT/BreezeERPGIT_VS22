<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/PopUp.Master"
    Inherits="ERP.OMS.Management.ToolsUtilities.management_toolsutilities_frm_EmailList_Bulk_Print" Codebehind="frm_EmailList_Bulk_Print.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
    function onPage()
        {
            window.focus();
            self.print();
        }
    </script>
    <style>
        #ASPxDataView1_ICell .dxdvItem_PlasticBlue {
            background:#073d63 !important;
            border-color:none !important;
            padding:5px 10px !important;
            border-radius: 9px;
        }
        #ASPxDataView1_ICell .dxdvItem_PlasticBlue label {
            line-height:28px;
            color:#e6e6e6;
            margin-right: 18px;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <dxe:ASPxDataView ID="ASPxDataView1" runat="server" CssFilePath="~/App_Themes/Office2003 Blue/{0}/styles.css" CssPostfix="Office2003_Blue" ImageFolder="~/App_Themes/Office2003 Blue/{0}/" PagerPanelSpacing="0px" RowPerPage="10">
            <ItemTemplate>
                <label><%# Eval("Email")%></label>
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
            <ItemStyle Height="5px" HorizontalAlign="Justify" >
                <Paddings Padding="0px" PaddingBottom="2px" PaddingLeft="0px" PaddingRight="0px"
                    PaddingTop="2px" />
                <Border BorderColor="Beige" BorderStyle="Solid" BorderWidth="1px" />
            </ItemStyle>
        </dxe:ASPxDataView>
    </div>
</asp:Content>