<%@ Page Title="Generate Stock Position" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.management_CreateStockPosition" CodeBehind="CreateStockPosition.aspx.cs" %>


<%--<%@ Register Assembly="DevExpress.Web.v9.1" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.1" Namespace= "DevExpress.Web"
    TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v9.1" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.1" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.1" Namespace="DevExpress.Web" TagPrefix="dxpc" %>

<%@ Register Assembly="DevExpress.Web.v9.1" Namespace="DevExpress.Web"
    TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v9.1" Namespace="DevExpress.Web"
    TagPrefix="dxp" %>
--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<link type="text/css" href="../CSS/PageStyle.css" rel="Stylesheet" />--%>
    
    <script type="text/javascript" src="/assests/js/jquery-1.3.2.min.js"></script>
    <script type="text/javascript" src="/assests/js/loaddata.js"></script>
    <script type="text/javascript" language="javascript">
        function DateChange(DateObj) {
            var Lck = '<%=Session["LCKBNK"] %>';
     var FYS = '<%=Session["FinYearStart"]%>';
     var FYE = '<%=Session["FinYearEnd"]%>';
     var LFY = '<%=Session["LastFinYear"]%>';
     DevE_CheckForLockDate(DateObj, Lck);
     DevE_CheckForFinYear(DateObj, FYS, FYE, LFY);
 }
 function DateCompare(DateobjFrm, DateobjTo) {
     var Msg = "To Date Can Not Be Less Than From Date!!!";
     DevE_CompareDateForMin(DateobjFrm, DateobjTo, Msg);
 }
 function OnTypeChange() {
     if (cForGenOrDel.GetValue() == "G") {

         cBtnGnDl.SetText("Generate");

     }

     if (cForGenOrDel.GetValue() == "D") {

         cBtnGnDl.SetText("Delete");
         Div_gv.style.display = "none";
         Div_For_Button.style.display = "none";
     }

 }
 function ShowButtonClick() {
     ShowLoading();
     var ComboValue = $.trim(cForGenOrDel.GetValue());
     cGvDateDtl.PerformCallback('Show~' + ComboValue);

 }
 function ReGenerate() {

     var ComboValue = $.trim(cForGenOrDel.GetValue());
     cGvDateDtl.PerformCallback('Generate~' + ComboValue);


 }

 //function SignOff() {
 //    window.parent.SignOff();
 //}
 //function height() {
 //    if (document.body.scrollHeight >= 500)
 //        window.frameElement.height = document.body.scrollHeight;
 //    else
 //        window.frameElement.height = '500px';
 //    window.frameElement.Width = document.body.scrollWidth;
 //}
 function GvDateDtl_EndCallBack() {
     if (cGvDateDtl.cpDivShowHide != null) {
         if (cGvDateDtl.cpDivShowHide == "T") {
             Div_gv.style.display = "inline";
             Div_For_Button.style.display = "inline";
         }
         else {
             Div_gv.style.display = "none";
             Div_For_Button.style.display = "none";
         }

     }

     if (cGvDateDtl.cpShowUpdateDeleteStatus != null) {
         alert(cGvDateDtl.cpShowUpdateDeleteStatus);
     }

     HideLoading();
 }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Generate Stock Position</h3>
        </div>

    </div>
     <div class="form_main">
       <%-- <div>
            <div id="Div_header" style="width: 89%">
                <div id="Div_headername" class="EHEADER" style="width: 97.5%; text-align: center">
                    <strong><span id="SpanHeader" style="color: #000099; font-size: small; font-family: Tahoma,Arial, Helvetica, sans-serif;">Generate Stock Position</span></strong>
                </div>
            </div>
            <div id="Div2" style="width: 98%; height: 2%">
            </div>

        </div>--%>
        <div id="Div1_Middle" class="div_Middle">
            <div id="Div1" style="width: 15%; height: 222%; float: left"></div>
            <div id="Div1_Middle1" class="tdmain" style="width: 7%; height: 22px;">
                From Date
            </div>
            <div id="Div1_Middle2_1" class="tdsecondary" style="width: 13%">
                <dxe:ASPxDateEdit ID="DtFrom" runat="server" ClientInstanceName="cDtFrom" DateOnError="Today"
                    EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" Width="87%">
                    <ClientSideEvents DateChanged="function(s,e){DateChange(cDtFrom);}" />
                    <ButtonStyle Width="3px">
                    </ButtonStyle>
                </dxe:ASPxDateEdit>
            </div>
            <div id="Div3" class="tdmain" style="width: 7%; height: 22px;">
                To Date
            </div>
            <div id="Div4" class="tdsecondary" style="width: 13%">
                <dxe:ASPxDateEdit ID="DtTo" runat="server" ClientInstanceName="cDtTo"
                    DateOnError="Today" EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True"
                    Width="89%">
                    <ClientSideEvents DateChanged="function(s,e){DateCompare(cDtFrom,cDtTo);}" />
                    <ButtonStyle Width="3px">
                    </ButtonStyle>
                </dxe:ASPxDateEdit>
            </div>
            <div id="Div_combo" class="tdsecondary" style="width: 12%">
                <dxe:ASPxComboBox runat="server" Width="76%" ID="ForGenOrDel" ClientInstanceName="cForGenOrDel" DropDownStyle="DropDown"
                    EnableSynchronization="False" EnableIncrementalFiltering="True" ValueType="System.String" SelectedIndex="0" Font-Size="Small">
                    <ClientSideEvents SelectedIndexChanged="OnTypeChange"></ClientSideEvents>
                    <Items>
                        <dxe:ListEditItem Text="Generate" Value="G"></dxe:ListEditItem>
                        <dxe:ListEditItem Text="Delete" Value="D"></dxe:ListEditItem>
                    </Items>
                </dxe:ASPxComboBox>
            </div>

            <div id="Divbtn_GnDl" style="width: 8%; margin: 2px; padding: 2px" align="center">
                <dxe:ASPxButton ID="BtnGnDl" CssClass="btn btn-primary" runat="server" Text="Generate" TabIndex="0"
                    ClientInstanceName="cBtnGnDl" AutoPostBack="False" Font-Size="Small"
                    OnClick="BtnGnDl_Click">
                    <ClientSideEvents Click="function (s, e) {ShowButtonClick();}" />
                </dxe:ASPxButton>


            </div>
            <asp:Label ID="lblMsg" runat="server"></asp:Label>
        </div>
        <br />
        <div id="Div_gv" style="width: 101%; display: none; float: right" align="center">

            <dxe:ASPxGridView ID="GvDateDtl" runat="server" AutoGenerateColumns="False" KeyFieldName="ID"
                ClientInstanceName="cGvDateDtl" OnCustomCallback="GvDateDtl_CustomCallback" Width="35%">
                <ClientSideEvents EndCallback="function (s, e) {GvDateDtl_EndCallBack();}" />
                <Templates>
                    <TitlePanel>
                        <table style="width: 100%">
                            <tr>
                                <td align="right">
                                    <table width="200px">
                                        <tr>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </TitlePanel>
                    <EditForm>
                    </EditForm>
                </Templates>
                <SettingsBehavior AllowFocusedRow="True" ColumnResizeMode="NextColumn" />
                <Styles>
                    <Header CssClass="gridheader" ImageSpacing="5px" SortingImageSpacing="5px">
                    </Header>
                    <FocusedGroupRow CssClass="gridselectrow">
                    </FocusedGroupRow>
                    <FocusedRow CssClass="gridselectrow">
                    </FocusedRow>
                    <LoadingPanel ImageSpacing="10px">
                    </LoadingPanel>
                </Styles>
                <SettingsPager AlwaysShowPager="True" NumericButtonCount="20" PageSize="20" ShowSeparators="True">
                    <FirstPageButton Visible="True">
                    </FirstPageButton>
                    <LastPageButton Visible="True">
                    </LastPageButton>
                </SettingsPager>

                <Columns>
                    <dxe:GridViewDataTextColumn Caption="Missing Date" FieldName="Date" VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Day" FieldName="Day" VisibleIndex="1"
                        Width="25%">
                        <CellStyle HorizontalAlign="Left ">
                        </CellStyle>
                    </dxe:GridViewDataTextColumn>
                </Columns>
                <SettingsText EmptyDataRow="No Data To Display" />
            </dxe:ASPxGridView>




        </div>
        <div id="space" style="width: 98%; height: 2%"></div>
        <div id="Div_For_Button" class="div_Middle" style="width: 98%; height: 6%; display: none" align="center">
            <div id="Div_For_Button_left" style="width: 51%; height: 222%; float: left" align="right">
                <dxe:ASPxButton ID="IgnoreAndGenerate_Btn" runat="server" Text="Ignore and Generate For Remaining Dates" TabIndex="0" ClientInstanceName="cIgnoreAndGenerate_Btn" AutoPostBack="False" Width="264px" Font-Size="Small">
                    <ClientSideEvents Click="function (s, e) {ReGenerate();}" />
                </dxe:ASPxButton>
            </div>
            <div id="Div_For_Button_right" style="width: 16%; height: 222%; float: left" align="center">
                <dxe:ASPxButton ID="CancelBtn" runat="server" Text="Cancel Process" TabIndex="0" ClientInstanceName="cA" AutoPostBack="False" Height="21px" OnClick="CancelBtn_Click" Font-Size="Small">
                </dxe:ASPxButton>
            </div>

        </div>
        <br />
        <br />
        <br />
    </div>
</asp:Content>
