<%--<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeFile="Report_StockMeasurement.aspx.cs" Inherits="ERP.OMS.Reports.Reports_Report_StockMeasurement" %>--%>
<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Reports.Reports_Report_StockMeasurement" Codebehind="Report_StockMeasurement.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxCallbackPanel"
    TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>--%>
    <link type="text/css" href="../CentralData/CSS/GenericCss.css" rel="Stylesheet" />
    <style type="text/css">
        #MainFull {
            padding: 5px;
            width: 995px;
        }

        #Container1 {
            width: 435px;
            padding: 5px;
        }

        .LableWidth {
            width: 110px;
        }

        .ContentWidth {
            width: 125px;
            height: 21px;
        }

        .labelCont {
            font-size: 13px;
            margin-top: 7px;
        }

        .btnRight {
            margin-right: 18px;
            float: right;
        }

        .txt_left {
            text-align: left;
        }

        .txt_right {
            text-align: right;
        }
    </style>
    <!--External Javascript-->

    <script type="text/javascript" src="../CentralData/JSScript/GenericJScript.js"></script>

    <!--Start For Ajax-->

    <script type="text/javascript" src="../CentralData/JSScript/init.js"></script>

    <script type="text/javascript" src="../CentralData/JSScript/GenericAjaxList.js"></script>

    <link type="text/css" href="../CentralData/CSS/GenericAjaxStyle.css" rel="Stylesheet" />
    <!--End For Ajax-->
    <script lang="javascript" type="text/javascript">
        function PageLoad()///Call Into Page Load
        {
            //           HideShow('C1_Row2_Col4','H');
            //           HideShow('C1_Row2_Col5','H');
            //           HideShow('Container2','H');                  
        }
        function NORECORD(obj) {
            alert('No Record Found !! ');
        }
        FieldName = '';
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="MainFull">
        <div id="Header" class="Header">
            Report For stock Measurement <span class="clear"></span>
        </div>
        <div id="Row0" class="Row">
            <div id="Container1" class="container">
                <div id="C1_Row1" class="Row">
                    <div id="C1_Row1_Col1" class="LFloat_Lable LableWidth">
                        <asp:Label ID="lblDate" runat="server" Text="Date : "></asp:Label>
                    </div>
                    <div id="C1_Row1_Col2" class="LFloat_Content ContentWidth">
                        <dxe:ASPxDateEdit ID="dtFrom" runat="server" ClientInstanceName="cdtFor" DateOnError="Today"
                            EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" Width="125px"
                            Font-Size="11px" TabIndex="0">
                            <DropDownButton Text="From">
                            </DropDownButton>
                            <%--<clientsideevents datechanged="function(s,e){DateChange(cdtFor);}"></clientsideevents>--%>
                        </dxe:ASPxDateEdit>
                    </div>
                    <div id="C1_Row1_Col3" class="LFloat_Content ContentWidth">
                        <dxe:ASPxDateEdit ID="dtTo" runat="server" ClientInstanceName="cdtTo" DateOnError="Today"
                            EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" Width="125px"
                            Font-Size="11px" TabIndex="0">
                            <DropDownButton Text="To">
                            </DropDownButton>
                            <%--<clientsideevents datechanged="function(s,e){DateChange(cdtFor);}"></clientsideevents>--%>
                        </dxe:ASPxDateEdit>
                    </div>
                    <br class="clear" />
                </div>
                <span class="clear"></span>
                <%--<div id="C1_Row2" class="Row">
                        <div id="C1_Row2_Col1" class="LFloat_Lable LableWidth">
                            <asp:Label ID="lblGroupBy" runat="server" Text="Group By : "></asp:Label>
                        </div>
                        <div class="left">
                            <div id="C1_Row2_Col2" class="LFloat_Content ContentWidth">
                                <dxe:ASPxComboBox ID="CmbGroupBy" runat="server" ValueType="System.String" ClientInstanceName="cCmbGroupBy"
                                    SelectedIndex="0" TabIndex="0" Width="125px">
                                    <items>
                                        <dxe:ListEditItem Text="Client" Value="C"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Branch" Value="B"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Group" Value="G"></dxe:ListEditItem>
                                    </items>
                                    <clientsideevents selectedindexchanged="function(s, e) {fn_GroupBy(s.GetValue());}" />
                                </dxe:ASPxComboBox>
                            </div>
                            <div class="left">
                                <div>
                                    <div id="C1_Row2_Col3" class="LFloat_Content ContentWidth">
                                        <dxe:ASPxRadioButtonList ID="RblClient" runat="server" SelectedIndex="0" ItemSpacing="15px"
                                            Paddings-PaddingTop="1px" RepeatDirection="Horizontal" TextWrap="False" TabIndex="0"
                                            ClientInstanceName="cRblClient">
                                            <items>
                                                <dxe:ListEditItem Text="All" Value="A" />
                                                <dxe:ListEditItem Text="Selected" Value="S" />                                               
                                            </items>
                                            <clientsideevents valuechanged="function(s, e) {fn_Client(s.GetValue());}" />
                                            <border borderwidth="0px" />
                                        </dxe:ASPxRadioButtonList>
                                    </div>
                                </div>
                                <div>
                                    <div id="C1_Row2_Col4" class="LFloat_Content ContentWidth">
                                        <dxe:ASPxRadioButtonList ID="RblBranch" runat="server" SelectedIndex="0" ItemSpacing="15px"
                                            Paddings-PaddingTop="1px" RepeatDirection="Horizontal" TextWrap="False" TabIndex="0"
                                            ClientInstanceName="cRblBranch">
                                            <items>
                                                <dxe:ListEditItem Text="All" Value="A" />
                                                <dxe:ListEditItem Text="Selected" Value="S" />                                                
                                            </items>
                                            <clientsideevents valuechanged="function(s, e) {fn_Branch(s.GetValue());}" />
                                            <border borderwidth="0px" />
                                        </dxe:ASPxRadioButtonList>
                                    </div>
                                    <span class="clear"></span>
                                </div>
                                <div id="C1_Row2_Col5">
                                    <div class="LFloat_Content ContentWidth">
                                        <dxe:ASPxComboBox ID="CmbGroupType" ClientInstanceName="cCmbGroupType" runat="server"
                                            Font-Size="11px" TabIndex="0" Width="125px" OnCallback="CmbGroupType_Callback">
                                            <clientsideevents valuechanged="function(s, e) {fn_CmbGroupType(s.GetValue());}"
                                                endcallback="CmbGroupType_EndCallback" />
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div id="C1_Row2_Col6" class="LFloat_Content ContentWidth" style="display: none;
                                        margin-top: 3px;">
                                        <dxe:ASPxRadioButtonList ID="RblGroup" runat="server" SelectedIndex="0" ItemSpacing="15px"
                                            Paddings-PaddingTop="1px" RepeatDirection="Horizontal" TextWrap="False" TabIndex="0"
                                            ClientInstanceName="cRblGroup">
                                            <items>
                                                <dxe:ListEditItem Text="All" Value="A" />
                                                <dxe:ListEditItem Text="Selected" Value="S" />                                               
                                            </items>
                                            <clientsideevents valuechanged="function(s, e) {fn_Group(s.GetValue());}" />
                                            <border borderwidth="0px" />
                                        </dxe:ASPxRadioButtonList>
                                    </div>
                                    <span class="clear"></span>
                                </div>
                                <span class="clear"></span>
                            </div>
                            <span class="clear"></span>
                        </div>
                        <span class="clear"></span>
                    </div>--%>
                <span class="clear"></span>
                <%-- <div id="C1_Row3" class="Row">
                        <div id="C1_Row3_Col1" class="LFloat_Lable LableWidth">
                            <asp:Label ID="Label1" runat="server" Text="Contract Type : "></asp:Label>
                        </div>
                        <div id="C1_Row3_Col2" class="LFloat_Content ContentWidth">
                            <dxe:ASPxComboBox ID="CmbContractType" runat="server" ValueType="System.String" ClientInstanceName="cCmbContractType"
                                SelectedIndex="0" TabIndex="0" Width="125px">
                                <items>
                                        <dxe:ListEditItem Text="Purchase" Value="P"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Sale" Value="S"></dxe:ListEditItem>                                       
                                    </items>
                            </dxe:ASPxComboBox>
                        </div>
                    </div>--%>
                <span class="clear"></span>
                <br class="clear" />
                <div id="C1_Row6" class="Row">
                    <dxe:ASPxButton ID="btnExport" runat="server" ClientInstanceName="cbtnExport" CssClass="btnUpdate"
                        AutoPostBack="False" Height="5px" Text="Export" Width="101px" OnClick="btnExport_Click">
                    </dxe:ASPxButton>
                </div>
            </div>
            <%--<div id="Container2" class="container">
                    <div id="C2_Row0" class="Row">
                        <div id="C2_Row0_Col1" class="LFloat_Content">
                            <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="320px" TabIndex="0"></asp:TextBox>
                        </div>
                        <div id="C2_Row0_Col2" class="LFloat_Lable">
                            <a href="javascript:void(0);" tabindex="0" onclick="btnAddToList_click()"><span class="lnkBtnAjax green">
                                Add to List</span></a>
                        </div>
                    </div>
                    <div id="C2_Row1" class="Row">
                        <div id="C2_Row1_Col1" class="LFloat_Content finalSelectedBox">
                            <asp:ListBox ID="lstSelection" runat="server" Font-Size="12px" Height="100px" Width="400px"
                                TabIndex="0"></asp:ListBox>
                        </div>
                    </div>
                    <div id="C2_Row2" class="Row">
                        <div id="C2_Row2_Col1" class="LFloat_Lable">
                            <a href="javascript:void(0);" tabindex="0" onclick="lnkBtnAddFinalSelection()"><span
                                class="lnkBtnAjax blue">Done</span></a>&nbsp;&nbsp; <a href="javascript:void(0);"
                                    tabindex="0" onclick="lnkBtnRemoveFromSelection()"><span class="lnkBtnAjax red">Remove</span></a>
                        </div>
                    </div>
                </div>--%>
        </div>
        <br />
        <%--<div style="display: none">
                <asp:TextBox ID="txtSelectionID_hidden" runat="server"></asp:TextBox>
                <asp:HiddenField ID="HiddenField_ClientBranchGroup" runat="server" />
            </div>--%>
    </div>
</asp:Content>
