<%@ Page Title="Users" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Master.management_master_Root_AddUserCompany" 
    CodeBehind="Root_AddUserCompany.aspx.cs" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <script type="text/javascript">
        /*Code  Added  By Priti on 20122016 to use jquery Choosen*/
        $(document).ready(function () {
            ListBind();
            ChangeSource();

        });
        function ListBind() {

            var config = {
                '.chsn': {},
                '.chsn-deselect': { allow_single_deselect: true },
                '.chsn-no-single': { disable_search_threshold: 10 },
                '.chsn-no-results': { no_results_text: 'Oops, nothing found!' },
                '.chsn-width': { width: "100%" }
            }
            for (var selector in config) {
                $(selector).chosen(config[selector]);
            }

        }
        function lstContact() {

            $('#lstContact').fadeIn();

        }
        function setvalue() {
            document.getElementById("txtContact_hidden").value = document.getElementById("lstContact").value;

        }
        function Changeselectedvalue() {
            var lstContact = document.getElementById("lstContact");
            if (document.getElementById("txtContact_hidden").value != '') {
                for (var i = 0; i < lstContact.options.length; i++) {
                    if (lstContact.options[i].value == document.getElementById("txtReportTo_hidden").value) {
                        lstContact.options[i].selected = true;
                    }
                }
                $('#lstContact').trigger("chosen:updated");
            }

        }
        function ChangeSource() {
            var fname = "%";
            var lContact = $('select[id$=lstContact]');
            lContact.empty();
            

            $.ajax({
                type: "POST",
                url: "Root_AddUserCompany.aspx/ALLContact",
                data: JSON.stringify({ reqStr: fname }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    var listItems = [];
                    if (list.length > 0) {

                        for (var i = 0; i < list.length; i++) {
                            var id = '';
                            var name = '';
                            id = list[i].split('|')[1];
                            name = list[i].split('|')[0];

                            $('#lstContact').append($('<option>').text(name).val(id));

                        }

                        $(lContact).append(listItems.join(''));

                        lstContact();
                        $('#lstContact').trigger("chosen:updated");

                        Changeselectedvalue();

                    }
                    else {
                        //   alert("No records found");
                        //lstReferedBy();
                        $('#lstContact').trigger("chosen:updated");

                    }
                }
            });
            // }
        }
        //....end......
        function showOptions(obj1, obj2, obj3) {
            ajax_showOptions(obj1, obj2, obj3);
        }

        function CallGrid() {

            grid.PerformCallback();
        }

        function DeleteRow(keyValue) {
            //doIt = confirm('Confirm delete?');
            //if (doIt) {
            //    grid.PerformCallback(keyValue);
            //}
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    grid.PerformCallback(keyValue);
                }
            });
        }
        function Cancel_Click() {
            //parent.editwin.close();
            location.href = '/OMS/Management/Master/root_user.aspx';
        }
        FieldName = 'BtnSave';
    </script>
    <style>
          /*Code  Added  By Priti on 20122016 to use jquery Choosen*/
         .chosen-container.chosen-container-single {
            width:100% !important;
        }
        .chosen-choices {
            width:100% !important;
        }
        #lstContact {
            width:100%;
            display:none !important;
        }
       
        #display:none !important;_chosen{
            width:100% !important;
        }
        .dxtcLite_PlasticBlue > .dxtc-content {
            overflow:visible !important
        }
        /*...end....*/
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>

        <table class="TableMain100">
            <tr>
                <td style="vertical-align: top;">
                    <div class="crossBtn"><a href="/OMS/management/master/root_user.aspx"><i class="fa fa-times"></i></a></div>
                </td>
            </tr>

            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td>Select Company:
                                    </td>
                                  
                                    <td style="width:300px;">
                                        <%--<asp:TextBox ID="txtContact" runat="server" Width="300px"></asp:TextBox>--%>
                                          <asp:ListBox ID="lstContact" CssClass="chsn"   runat="server"  data-placeholder="Select..."></asp:ListBox>
                                        <asp:HiddenField ID="txtContact_hidden" runat="server" />
                                       
                                    </td>
                                    <td>
                                        <asp:Button ID="BtnSave" runat="server" Text="Add" CssClass="btnUpdate btn btn-primary"  OnClick="BtnSave_Click" OnClientClick="setvalue()" />
                                        <asp:Button ID="BtnCancel" runat="server" CssClass="btnUpdate btn btn-danger" Text="Cancel" OnClientClick="Cancel_Click()" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="text-align: left" visible="false">
                                        <asp:HiddenField ID="txtID" runat="server" />
                                        <br />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxGridView ID="GridName" runat="server" KeyFieldName="UserCompany_ID" AutoGenerateColumns="False"
                        DataSourceID="SelectName" ClientInstanceName="grid" Width="100%" OnCustomCallback="GridName_CustomCallback">
                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="Company" ReadOnly="True" VisibleIndex="0"
                                Width="80%">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="UserCompany_CompanyID" ReadOnly="True" Visible="false">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="8" Width="60px" Caption="Details" CellStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <DataItemTemplate>
                                    <a href="javascript:void(0);" onclick="DeleteRow('<%# Container.KeyValue %>')">

                                        <img src="/assests/images/Delete.png" />
                                    </a>
                                </DataItemTemplate>
                                <CellStyle Wrap="False">
                                </CellStyle>
                                <HeaderTemplate>
                                    Delete                               
                                </HeaderTemplate>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                        </Columns>
                        <Styles>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                        </Styles>
                        <SettingsText ConfirmDelete="Are You Sure To Delete This Record ???" />
                        <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="SelectName" runat="server"></asp:SqlDataSource>
    </div>
</asp:Content>
