<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.management_master_AuthoSignatory" CodeBehind="AuthoSignatory.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">
        function CallList(obj1, obj2, obj3) {

            ajax_showOptions(obj1, obj2, obj3);
            FieldName = 'Label3';

        }
        function ShowMessage(obj) {
            if (obj == 'Duplicate') {
                alert('Duplicate entry not allowed');
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table class="TableMain99">
        <tr>
            <td style="width: 90%">
                <dxe:ASPxGridView ID="gridSignatory" runat="server" Width="100%" ClientInstanceName="gridSignatory"
                    AutoGenerateColumns="False" KeyFieldName="AuthorizedSignatory_ID" DataSourceID="SqlSignatory"
                    OnHtmlEditFormCreated="gridSignatory_HtmlEditFormCreated" OnRowInserting="gridSignatory_RowInserting1"
                    OnRowDeleting="gridSignatory_RowDeleting" OnCustomJSProperties="gridSignatory_CustomJSProperties" OnRowUpdating="gridSignatory_RowUpdating">
                    <%--    OnStartRowEditing="gridSignatory_StartRowEditing" OnRowUpdating="gridSignatory_RowUpdating"--%>
                    <Templates>
                        <EditForm>
                            <table class="TableMain100">
                                <tr>
                                    <td style="border: solid 1px blue; width: 80%">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label1" runat="server" Text="Employee:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TxtEmployee" runat="server" Text='<%#Bind("name") %>'></asp:TextBox>
                                                    <%-- <asp:TextBox ID="TxtEmployee" runat="server" Text='<%#Bind("AuthorizedSignatory_EmployeeID") %>'></asp:TextBox>--%>
                                                    <asp:HiddenField ID="TxtEmployee_hidden" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label4" runat="server" Text="From Date:"></asp:Label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxDateEdit ID="ASPxStartDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                        Width="171px" EditFormatString="dd-MM-yyyy" Text='<%#Bind("AuthorizedSignatory_DateFrom") %>'>
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label2" runat="server" Text="To Date"></asp:Label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxDateEdit ID="ASPxTodate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                        Width="171px" EditFormatString="dd-MM-yyyy" Text='<%#Bind("AuthorizedSignatory_DateTo") %>'>
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                                <td>
                                                    <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                        runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                    <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                        runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                    <asp:HiddenField ID="hdid" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </EditForm>
                    </Templates>
                    <Styles>
                        <Header SortingImageSpacing="5px" ImageSpacing="5px">
                        </Header>
                        <LoadingPanel ImageSpacing="10px">
                        </LoadingPanel>
                    </Styles>
                    <Settings ShowGroupPanel="True" ShowTitlePanel="True" ShowFooter="True" ShowStatusBar="Visible"></Settings>
                    <SettingsBehavior ConfirmDelete="True" AllowFocusedRow="True"></SettingsBehavior>
                    <SettingsPager AlwaysShowPager="True" PageSize="20" NumericButtonCount="20" ShowSeparators="True">
                        <FirstPageButton Visible="True">
                        </FirstPageButton>
                        <LastPageButton Visible="True">
                        </LastPageButton>
                    </SettingsPager>
                    <Columns>
                        <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="0" FieldName="AuthorizedSignatory_ID">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="0" FieldName="AuthorizedSignatory_CompanyID">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataComboBoxColumn FieldName="AuthorizedSignatory_SegmentID" VisibleIndex="0"
                            Visible="false">
                        </dxe:GridViewDataComboBoxColumn>
                        <dxe:GridViewDataTextColumn Visible="false" FieldName="AuthorizedSignatory_EmployeeID"
                            Width="50%" Caption="Employee">
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                            <EditFormCaptionStyle Wrap="False" HorizontalAlign="Left">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="True"></EditFormSettings>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="name" Width="30%" Caption="Employee">
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                            <EditFormCaptionStyle Wrap="False" HorizontalAlign="Left">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="True"></EditFormSettings>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="AuthorizedSignatory_DateFrom"
                            Width="500px" Caption="From Date">
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                            <EditFormCaptionStyle Wrap="False" HorizontalAlign="Left">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="True"></EditFormSettings>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="AuthorizedSignatory_DateTo"
                            Caption="To Date" Width="500px">
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                            <EditFormCaptionStyle Wrap="False" HorizontalAlign="Left">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="True"></EditFormSettings>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewCommandColumn VisibleIndex="4" ShowDeleteButton="True" ShowEditButton="True">
                            <HeaderTemplate>
                                <a href="javascript:void(0);" onclick="gridSignatory.AddNewRow();">
                                    <span style="color: #000099; text-decoration: underline">Add New</span>
                                </a>
                            </HeaderTemplate>
                        </dxe:GridViewCommandColumn>
                    </Columns>
                    <ClientSideEvents EndCallback="function(s,e){ShowMessage(s.cpValue)}" />
                </dxe:ASPxGridView>
                <asp:Label ID="Label3" runat="server" Text="Label" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <input id="Button1" type="button" value="Cancel" onclick="parent.editwin1.close()" />
            </td>
        </tr>
        <asp:SqlDataSource ID="SqlSignatory" runat="server"
            SelectCommand="select AuthorizedSignatory_ID,AuthorizedSignatory_CompanyID,AuthorizedSignatory_SegmentID,AuthorizedSignatory_EmployeeID,convert(varchar(11),AuthorizedSignatory_DateFrom,105)as AuthorizedSignatory_DateFrom, case AuthorizedSignatory_DateTo when '1900-01-01 00:00:00.000' then '' else (convert(varchar(11),AuthorizedSignatory_DateTo,105)) end as AuthorizedSignatory_DateTo ,isnull(cnt_firstname,'')+' '+isnull(cnt_middlename,'')+' '+isnull(cnt_lastname,'') as name from tbl_master_contact as a inner join Master_AuthorizedSignatory as b on a.cnt_internalid=b.AuthorizedSignatory_EmployeeID where b.AuthorizedSignatory_SegmentID=@AuthorizedSignatory_SegmentID ORDER BY AuthorizedSignatory_ID"
            InsertCommand="insert into table1 values(1)" DeleteCommand="delete from table1"
            UpdateCommand="UPDATE [Master_AuthorizedSignatory] SET [AuthorizedSignatory_EmployeeID] = @AuthorizedSignatory_EmployeeID,
            [AuthorizedSignatory_DateFrom]=@AuthorizedSignatory_DateFrom,[AuthorizedSignatory_DateTo]=@AuthorizedSignatory_DateTo,
            [AuthorizedSignatory_ModifyUser]=@AuthorizedSignatory_ModifyUser,[AuthorizedSignatory_ModifyDateTime]=@AuthorizedSignatory_ModifyDateTime 
            WHERE [AuthorizedSignatory_ID] = @AuthorizedSignatory_ID">
            <SelectParameters>
                <%-- <asp:SessionParameter Name="AuthorizedSignatory_SegmentID" SessionField="KeyVal" Type="String" />--%>
                <asp:SessionParameter Name="AuthorizedSignatory_SegmentID" SessionField="KeyVal_id" Type="String" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="AuthorizedSignatory_EmployeeID" Type="String" />
                <asp:Parameter Name="AuthorizedSignatory_DateFrom" Type="DateTime" />
                <asp:Parameter Name="AuthorizedSignatory_DateTo" Type="DateTime" />
                <asp:Parameter Name="AuthorizedSignatory_ModifyUser" Type="String" />
                <asp:Parameter Name="AuthorizedSignatory_ModifyDateTime" Type="DateTime" />
                <asp:Parameter Name="AuthorizedSignatory_ID" Type="String" />
            </UpdateParameters>
        </asp:SqlDataSource>
        <%-- UpdateCommand="update table1 set temp123=1"--%>
    </table>
</asp:Content>

