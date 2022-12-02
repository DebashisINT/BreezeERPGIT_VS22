<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/PopUp.Master" Inherits="ERP.OMS.Management.ActivityManagement.management_activitymanagement_frm_AddLead" CodeBehind="frm_AddLead.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Untitled Page</title>
    <link href="../../CSS/style.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
<!--

    function chkclicked(obj, id1, name1, prodtype, prodid, phoneid, prdid, amt) {
        if (obj.checked == true) {
            var hidden1 = document.getElementById("hd1");
            if (hidden1.value != "") {
                hidden1.value = hidden1.value + "," + id1 + "|" + name1 + "|" + prodtype + "|" + prodid + "|" + phoneid + "|" + prdid + "|" + amt;
            }
            else {
                hidden1.value = id1 + "|" + name1 + "|" + prodtype + "|" + prodid + "|" + phoneid + "|" + prdid + "|" + amt;
            }
        }
        else {
            var hidden1 = document.getElementById("hd1");
            if (hidden1.value != "") {
                var obj12 = hidden1.value;
                varArray = obj12.split(",");
                var temp3 = "";
                for (i = 0; i < varArray.length; i++) {
                    var temp1 = varArray[i];
                    var temp2 = id1 + "|" + name1 + "|" + prodtype + "|" + prodid + "|" + phoneid + "|" + prdid + "|" + amt;
                    if (varArray[i] == temp2) {

                    }
                    else {
                        if (temp3 != "") {
                            temp3 = temp3 + "," + varArray[i];
                        }
                        else {
                            temp3 = varArray[i];
                        }
                    }
                }
                hidden1.value = temp3;
            }
        }


    }
    function CallParent() {

        //parent.document.getElementById("IFRAME_ForAllPages").contentWindow.SetDisable();
        
        parent.editwin.close();

            //parent.SetParent();
        //parent.close();
       
    }



    // -->
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100">
            <tr>
                <td style="width: 5%" class="lt">
                    <table>
                        <tr>
                            <td style="width: 75px">
                                <asp:Button ID="btnCondition" runat="server" Text="Address Search" CssClass="btnUpdate"
                                    OnClick="btnCondition_Click" Width="97px" Height="23px" />
                            </td>
                            <td>
                                <asp:Button ID="btnNameSearch" runat="server" Text="Name Search" CssClass="btnUpdate"
                                    OnClick="btnNameSearch_Click" Width="97px" Height="23px" />
                            </td>
                            <td>
                                <asp:Button ID="btnAddLead" runat="server" Text="Add Lead" CssClass="btnUpdate" OnClick="btnAddLead_Click"
                                    Width="69px" Height="23px" />
                            </td>
                            <td>
                                <asp:Button ID="btnAddPhoneCall" runat="server" Text="Add Phone Call" CssClass="btnUpdate"
                                    OnClick="btnAddPhoneCall_Click" Width="98px" Height="23px" />
                            </td>

                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="text-align: center">
                    <asp:HiddenField ID="hd1" runat="server" />
                    <asp:Label ID="lblTitle" Font-Size="X-Small" CssClass="mylabel1" runat="server" Font-Bold="True"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <table class="TableMain100">
                        <tr id="CheckBoxContainer" runat="server" visible="false">
                            <td class="mylabel1" visible="false">
                                <asp:CheckBox AutoPostBack="true" ID="chkLead" runat="server" Text="Lead Data" OnCheckedChanged="chkLead_CheckedChanged"
                                    Width="61px" />
                                <asp:CheckBox ID="chkPhoneCall" AutoPostBack="true" runat="server" Text="Phone Call"
                                    OnCheckedChanged="chkPhoneCall_CheckedChanged" />
                                <asp:CheckBox ID="chkSalesVisit" AutoPostBack="true" runat="server" Text="Sales Visit"
                                    OnCheckedChanged="chkSalesVisit_CheckedChanged" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="mylabel1">No. of Contact Alloted if no item select :<asp:TextBox ID="txtNoCont" Text="0" runat="server"
                    Width="33px"></asp:TextBox>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Please write integer only"
                        ControlToValidate="txtNoCont" Operator="DataTypeCheck" Type="Integer" Width="166px"></asp:CompareValidator></td>
            </tr>
            <tr>
                <td class="lt" visible="false">
                    <asp:Label ID="lblAdd" runat="server" Visible="false" Text="Address KeyWord"></asp:Label>
                </td>
            </tr>
            <tr>
                <td id="tdname" runat="server" class="lt" visible="false">
                    <asp:Label ID="lblSerachName" CssClass="mylabel1" Text="Name" runat="server" Visible="false"></asp:Label>
                    <asp:TextBox ID="txtNameSerach" runat="server" Visible="false"></asp:TextBox>
                    <asp:Button ID="btnGoSerach" Text="Submit" runat="server" Visible="false" CssClass="btnUpdate"
                        OnClick="btnGoSerach_Click" Height="23px" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Panel ID="pnlSearchCriteria" Width="100%" Visible="false" runat="server">
                        <table class="TableMain100">
                            <tr>
                                <td class="mylabel1" style="width: 2%">Address :</td>
                                <td style="width: 3%">
                                    <asp:TextBox ID="txtCon1" runat="server" Height="18px" Width="130px"></asp:TextBox></td>
                                <td>
                                    <asp:DropDownList ID="drpAndOr1" runat="server" Width="68px">
                                        <asp:ListItem Text="And" Value="And"></asp:ListItem>
                                        <asp:ListItem Text="Or" Value="Or"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtCon2" runat="server" Height="19px" Width="123px"></asp:TextBox></td>
                                <td style="width: 2%">
                                    <asp:DropDownList ID="drpAndOr2" runat="server" Width="105px">
                                        <asp:ListItem Text="And" Value="And"></asp:ListItem>
                                        <asp:ListItem Text="Or" Value="Or"></asp:ListItem>
                                    </asp:DropDownList></td>
                                <td colspan="2">&nbsp;<asp:TextBox ID="txtCon3" runat="server" Height="19px" Width="133px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="mylabel1">Created User :</td>
                                <td>
                                    <asp:DropDownList ID="drp_Cond_User" runat="server" Width="134px">
                                        <asp:ListItem Text="And" Value="And"></asp:ListItem>
                                        <asp:ListItem Text="Or" Value="Or"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 4%">
                                    <asp:DropDownList ID="drpUser" AppendDataBoundItems="true" runat="server" Width="199px">
                                        <asp:ListItem Text="None" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="mylabel1">Create Date :</td>
                                <td>
                                    <asp:DropDownList ID="drp_Cond_CrateDate" runat="server" Width="134px">
                                        <asp:ListItem Text="And" Value="And"></asp:ListItem>
                                        <asp:ListItem Text="Or" Value="Or"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td class="mylabel1" colspan="3" style="border: solid 1px white">
                                    <table>
                                        <tr>
                                            <td class="mylabel1">From :</td>
                                            <td valign="middle" style="height: 37px">
                                                <dxe:ASPxDateEdit ID="ASPxDateFrom" runat="server"
                                                    EditFormat="Custom"
                                                    UseMaskBehavior="True" Width="146px">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                            <td class="mylabel1" style="height: 37px">To :
                                            </td>
                                            <td valign="middle" style="height: 37px">
                                                <dxe:ASPxDateEdit ID="ASPxDateTo" runat="server"
                                                    EditFormat="Custom"
                                                    UseMaskBehavior="True" Width="152px">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="mylabel1" style="height: 24px">Source Type :
                                </td>
                                <td style="height: 24px">
                                    <asp:DropDownList ID="drp_Cond_SourceType" runat="server" Width="134px">
                                        <asp:ListItem Text="And" Value="And"></asp:ListItem>
                                        <asp:ListItem Text="Or" Value="Or"></asp:ListItem>
                                    </asp:DropDownList></td>
                                <td style="height: 24px">
                                    <asp:DropDownList AutoPostBack="true" AppendDataBoundItems="true" ID="drpSourceType"
                                        runat="server" OnSelectedIndexChanged="drpSourceType_SelectedIndexChanged" Width="196px">
                                        <asp:ListItem Text="No Preference" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td class="mylabel1" style="height: 24px">Source Name :</td>
                                <td style="width: 3%; height: 24px;">
                                    <asp:DropDownList ID="drpReferBy" runat="server" Width="220px">
                                    </asp:DropDownList></td>
                                <td style="width: 1%;">
                                    <asp:Button ID="btnGo" Text="Submit" runat="server" CssClass="btnUpdate" OnClick="btnGo_Click"
                                        Height="22px" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left">
                    <asp:Button ID="btnSubmit1" runat="server" Text="Save Data" CssClass="btnUpdate"
                        OnClick="btnSubmit1_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="grdLead" CssClass="lt" runat="server" AllowPaging="true" PageSize="10"
                        AutoGenerateColumns="false" Width="100%" CellPadding="4" ForeColor="#333333"
                        GridLines="None" BorderWidth="1px" BorderColor="#507CD1" OnPageIndexChanging="grdLead_PageIndexChanging"
                        OnRowDataBound="grdLead_RowDataBound">
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
                        <EditRowStyle BackColor="#2461BF" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle Font-Bold="false" ForeColor="black" CssClass="EHEADER" BorderColor="AliceBlue"
                            BorderWidth="1px" />
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkLead" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="LeadId" ControlStyle-CssClass="lt">
                                <ItemTemplate>
                                    <asp:Label ID="lblId" CssClass="lt" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Name">
                                <ItemTemplate>
                                    <asp:Label ID="lbl23" runat="server" Text='<%# Eval("name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Next Visit Date">
                                <ItemTemplate>
                                    <asp:Label ID="lbl2" runat="server" Text='<%# Eval("NextvisitDate") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Product Type" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblProdType" runat="server" Text='<%# Eval("ProdcuctType") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Address" ItemStyle-Width="250px">
                                <ItemTemplate>
                                    <asp:Label ID="lbl15" runat="server" Text='<%# Eval("LandMark") %>'></asp:Label>[
                                        <asp:Label ID="lbl1" runat="server" Text='<%# Eval("Address1") %>'></asp:Label>
                                    &nbsp;&nbsp;
                                        <asp:Label ID="lblAddress2" runat="server" Text='<%# Eval("Address2") %>'></asp:Label>
                                    &nbsp;&nbsp;
                                        <asp:Label ID="lblAddress3" runat="server" Text='<%# Eval("Address3") %>'></asp:Label>]
                                        <asp:Label ID="lbl134" runat="server" Text='<%# Eval("City") %>'></asp:Label>,
                                        <asp:Label ID="lbl14" runat="server" Text='<%# Eval("pin") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Product">
                                <ItemTemplate>
                                    <asp:Label ID="lblProdId" runat="server" Text='<%# Eval("Productid") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Phone Id" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lbl12" runat="server" Text='<%# Eval("phoneid") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Prod Ids" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblprdId" runat="server" Text='<%# Eval("prdid") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Amount">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmt" runat="server" Text='<%# Eval("Amount") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:GridView ID="grdSelectedCandidate" Visible="false" runat="server" AllowPaging="true"
                        PageSize="10" AutoGenerateColumns="false" Width="100%" CellPadding="4" ForeColor="#333333"
                        GridLines="None" BorderWidth="1px" BorderColor="#507CD1" OnRowDataBound="grdSelectedCandidate_RowDataBound">
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
                        <EditRowStyle BackColor="#2461BF" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle Font-Bold="false" ForeColor="black" CssClass="EHEADER" BorderColor="AliceBlue"
                            BorderWidth="1px" />
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:TemplateField HeaderText="Id">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelectedCandidate" Checked="true" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Ids">
                                <ItemTemplate>
                                    <asp:Label ID="lblCandidateId" runat="server" Text='<%# Eval("CandidateId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblCandidateName" runat="server" Text='<%# Eval("CandidateName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Product Type">
                                <ItemTemplate>
                                    <asp:Label ID="lblSelectedProdType" runat="server" Text='<%# Eval("ProdcuctType") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Product Id">
                                <ItemTemplate>
                                    <asp:Label ID="lblSelectedProdId" runat="server" Text='<%# Eval("Productid") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Phone Id">
                                <ItemTemplate>
                                    <asp:Label ID="lblSelectedPhoneid" runat="server" Text='<%# Eval("phoneid") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Prod Ids">
                                <ItemTemplate>
                                    <asp:Label ID="lblSelectedprdId" runat="server" Text='<%# Eval("prdid") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Prod Ids">
                                <ItemTemplate>
                                    <asp:Label ID="lblSelectedAmt" runat="server" Text='<%# Eval("Amt") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:Button ID="btnSubmit" runat="server" Text="Save Data" CssClass="btnUpdate" OnClick="btnSubmit_Click"
                        Visible="False" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
