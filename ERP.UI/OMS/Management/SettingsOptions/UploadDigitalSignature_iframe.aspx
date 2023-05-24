<%--================================================== Revision History =============================================
1.0   Pallab    V2.0.38      23-05-2023          0026206: Digital Signature module design modification & check in small device
====================================================== Revision History =============================================--%>

<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.SettingsOptions.management_SettingsOptions_UploadDigitalSignature_iframe" Codebehind="UploadDigitalSignature_iframe.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%--    

    

    <title>Untitled Page</title>--%>

    <script language="javascript" type="text/javascript">
    //function SignOff()
    //{
    //    window.parent.SignOff();
    //}
        function ClickOnMoreInfo(url, HeaderText, Width, Height, anyKey) //AnyKey will help to call back event to child page, if u have to fire more that one function 
        {
           
             OnMoreInfoClick(url,"Modify Lead Details",'940px','450px',"Y"); 
//             editwin=dhtmlmodal.open("Editbox", "iframe", url,HeaderText , "width="+Width+",height="+Height+",center=1,resize=1,scrolling=2,top=500", "recal")
//             document.getElementById('ctl00_ContentPlaceHolder1_Headermain1_cmbSegment').style.visibility='hidden';
//             editwin.onclose=function()
//             {
//                document.getElementById('ctl00_ContentPlaceHolder1_Headermain1_cmbSegment').style.visibility='visible';
//                 if(anyKey=='Y')
//                 {
//                    document.getElementById('IFRAME_ForAllPages').contentWindow.callback();
//                   
//                 }
//             }
        }
        function DhtmlClose()
        {
            parent.editwin.close();
            document.getElementById('IFRAME_ForAllPages').contentWindow.callback();
        }
       function InfoClick()
        {
        
            var URL='UploadDigitalSignature_add.aspx' ;
            //OnMoreInfoClick(URL, 'Add Digital Signature', '950px', '450px', 'Y');
            popup.SetContentUrl(URL);
            popup.Show();
        }
        
       function AuthUser(obj)
        {
        if(obj==null)
        {
        alert('Please Select a Record');
        }
        else
        {
             var URL='UploadDigitalSignature_AuthUser.aspx?id='+obj ;
            OnMoreInfoClick(URL,'Add Authorize User','950px','450px','Y');
            }
        }

        
        function callback()
        {
          grid.PerformCallback();
        }
        
        
        
        function ShowHideFilter(obj)
          {
           grid.PerformCallback(obj);
          }



    </script>

    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    
    <style>
        select
        {
            z-index: 1;
        }

        #gridAdvanceAdj {
            max-width: 99% !important;
        }
        #FormDate, #toDate, #dtTDate, #dt_PLQuote, #dt_PlQuoteExpiry {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        select
        {
            -webkit-appearance: none;
        }

        .calendar-icon
        {
            right: 20px;
        }

        .panel-title h3
        {
            padding-top: 0px !important;
        }

        .fakeInput
        {
                min-height: 30px;
    border-radius: 4px;
        }
        
    </style>
    <%--Rev end 1.0--%>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>Digital Signature</h3>
        </div>

    </div> 
        <div class="form_main">
        <table id="tblSummary" border="0" cellpadding="0" cellspacing="0" class="TableMain100">
            <tr>
                <td colspan="2" style="text-align: left; padding-bottom: 10px">
                    <table>
                        <tr>
                            <td >
                                <a href="javascript:void(0);" onclick="javascript:InfoClick();" class="btn btn-primary">Add New</a>
                               <%-- <span id="ShowFilter"><a href="javascript:ShowHideFilter('s');" class="btn btn-success">
                                    Show Filter</a></span>
                                <span id="Td1"><a href="javascript:ShowHideFilter('All');" class="btn btn-primary">
                                    All Records</a></span>--%>
                                <% if (rights.CanExport)
                                               { %>
                                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true"  >
                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                        <asp:ListItem Value="2">XLS</asp:ListItem>
                                        <asp:ListItem Value="3">RTF</asp:ListItem>
                                        <asp:ListItem Value="4">CSV</asp:ListItem>
                                </asp:DropDownList>
                                 <% } %>
                            </td>
                            
                        </tr>
                    </table>
                </td>
                <%--<td class="gridcellright pull-right" colspan="4" valign="top">
                    <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true" 
                        ClientInstanceName="exp" Font-Bold="False" ForeColor="black" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                        SelectedIndex="0" ValueType="System.Int32" Width="130px">
                        <Items>
                            <dxe:ListEditItem Text="Select" Value="0" />
                            <dxe:ListEditItem Text="PDF" Value="1" />
                            <dxe:ListEditItem Text="XLS" Value="2" />
                            <dxe:ListEditItem Text="RTF" Value="3" />
                            <dxe:ListEditItem Text="CSV" Value="4" />
                        </Items>
                        <ButtonStyle>
                        </ButtonStyle>
                        <ItemStyle >
                            <HoverStyle >
                            </HoverStyle>
                        </ItemStyle>
                        <Border BorderColor="black" />
                        <DropDownButton Text="Export">
                        </DropDownButton>
                    </dxe:ASPxComboBox>
                </td>--%>
            </tr>
            <tr>
                <td colspan="6" style="text-align: left">
                    <dxe:ASPxGridView ID="gridSign" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid"
                        KeyFieldName="DigitalSignature_ID" Width="100%" OnCustomCallback="gridSign_CustomCallback">
                        <settingsbehavior allowfocusedrow="True" />
                        <styles>
                        <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>

                        <FocusedRow CssClass="gridselectrow"></FocusedRow>
                        </styles>
                          <settingspager numericbuttoncount="20" pagesize="20" showseparators="True" alwaysshowpager="True">
                        <FirstPageButton Visible="True"></FirstPageButton>

                        <LastPageButton Visible="True"></LastPageButton>
                        </settingspager>
                        <columns>
                        <dxe:GridViewDataTextColumn FieldName="name" Caption="Name" VisibleIndex="0">
                        <Settings FilterMode="DisplayText" AutoFilterCondition="Contains"></Settings>

                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataDateColumn FieldName="DigitalSignature_ValidFrom" Caption="Valid From" VisibleIndex="1">
                        <PropertiesDateEdit DisplayFormatString="dd-MMM-yyyy" EditFormat="Custom"></PropertiesDateEdit>

                        <Settings AutoFilterCondition="Contains"></Settings>

                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        </dxe:GridViewDataDateColumn>
                        <dxe:GridViewDataDateColumn FieldName="DigitalSignature_ValidUntil" Caption="Valid To" VisibleIndex="2">
                        <PropertiesDateEdit DisplayFormatString="dd-MMM-yyyy" EditFormat="Custom"></PropertiesDateEdit>

                        <Settings AutoFilterCondition="Contains"></Settings>

                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        </dxe:GridViewDataDateColumn>
                        <dxe:GridViewDataDateColumn VisibleIndex="3"><DataItemTemplate>
                                    <a href="javascript:AuthUser('<%# Container.KeyValue %>');"><span style="color:Black; text-decoration: none;">
                                        Authorised User</span></a>
                                
                        </DataItemTemplate>

                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <HeaderTemplate>
                                    Actions
                                                        
                        </HeaderTemplate>
                        </dxe:GridViewDataDateColumn>
                        </columns>
                                                <settings showstatusbar="Visible" />
                                                <styleseditors>
                        <ProgressBar Height="25px"></ProgressBar>
                        </styleseditors>
                    </dxe:ASPxGridView>
                      <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" ContentUrl="UploadDigitalSignature_add.aspx"
                                            CloseAction="CloseButton" ClientInstanceName="popup" Height="466px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                            Width="900px" HeaderText="Add/Modify Digital signature" AllowResize="true" ResizingMode="Postponed" Modal="true">
                                            <ContentCollection>
                                                <dxe:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                                                </dxe:PopupControlContentControl>
                                            </ContentCollection>
                                            <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
                                        </dxe:ASPxPopupControl>
                     <dxe:ASPxGridViewExporter ID="exporter" runat="server">
        </dxe:ASPxGridViewExporter>
                </td>
            </tr>
        </table>
            </div>
    </div>
</asp:Content>
