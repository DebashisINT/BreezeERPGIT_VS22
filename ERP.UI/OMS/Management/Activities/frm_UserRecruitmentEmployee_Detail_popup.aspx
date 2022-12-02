<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/OMS/MasterPage/ERP.Master" 
    Inherits="ERP.OMS.Management.Activities.management_activities_frm_UserRecruitmentEmployee_Detail_popup" Codebehind="frm_UserRecruitmentEmployee_Detail_popup.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    

    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <asp:GridView ID="GridCandi1" runat="server" CellPadding="4" ForeColor="#333333"
            GridLines="None" AutoGenerateColumns="false" BorderWidth="1px" BorderColor="#507CD1"
            PageSize="20" EmptyDataText="No Data Found!">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#EFF3FB" />
            <EditRowStyle BackColor="#2461BF" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle Font-Bold="False" ForeColor="Black" CssClass="EHEADER" BorderColor="AliceBlue" />
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField HeaderText="Interview Date">
                    <ItemTemplate>
                        <asp:Label ID="lblLastInterview" runat="server" Text='<%# Eval("InterviewDate") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Interviewer">
                    <ItemTemplate>
                        <asp:Label ID="lblInterviewId1" runat="server" Text='<%# Eval("Interviewer") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Result">
                    <ItemTemplate>
                        <asp:Label ID="lblResult" runat="server" Text='<%# Eval("Result") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Next Interview Date">
                    <ItemTemplate>
                        <asp:Label ID="lblNextInterview" runat="server" Text='<%# Eval("NextInterviewDate") %> '></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Next Interviewer">
                    <ItemTemplate>
                        <asp:Label ID="lblInterviewId2" runat="server" Text='<%# Eval("NextInterviewer") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Next Interview Place">
                    <ItemTemplate>
                        <asp:Label ID="lblInterviewId3" runat="server" Text='<%# Eval("NextInterviewPlace") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <input id="Submit1" type="submit" value="Close" onclick="window.close();" />
  </asp:Content>
