<%@ Page Title="Branches" Language="C#" AutoEventWireup="True" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Master.management_master_Branch" CodeBehind="Branch.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        
    </style>

    <script language="javascript" type="text/javascript">

        function OnEditButtonClick(keyValue) {
            var url = 'BranchAddEdit.aspx?id=' + keyValue;
            //OnMoreInfoClick(url, "Add New Accout", '920px', '500px', "Y");
            window.location.href = url;
        }
        function EndCall(obj) {
            if (grid.cpDelmsg != null)
                jAlert(grid.cpDelmsg);
        }
        function OnAddButtonClick() {
            var url = 'BranchAddEdit.aspx?id=ADD';
            //OnMoreInfoClick(url, "Add New Accout", '920px', '500px', "Y");
            window.location.href = url;
        }






        function DeleteRow(keyValue) {

            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    grid.PerformCallback('Delete~' + keyValue);
                }
            });


            //doIt = confirm('Confirm delete?');
            //alert(doIt);
            //if (doIt) {
            //    grid.PerformCallback('Delete~' + keyValue);
            //}
        }


        function ShowHideFilter1(obj) {
            gridTerminal.PerformCallback(obj);
        }

        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function Page_Load() {
            document.getElementById("TdCombo").style.display = "none";
        }
        function callback() {
            grid.PerformCallback();
        }
        function gridRowclick(s, e) {
            $('#gridStatus').find('tr').removeClass('rowActive');
            $('.floatedBtnArea').removeClass('insideGrid');
            //$('.floatedBtnArea a .ico').css({ 'opacity': '0' });
            $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
            $(s.GetRow(e.visibleIndex)).addClass('rowActive');
            setTimeout(function () {
                //alert('delay');
                var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a .ico').css({'opacity': '1'});
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a').each(function (e) {
                //    setTimeout(function () {
                //        $(this).fadeIn();
                //    }, 100);
                //});    
                $.each(lists, function (index, value) {
                    //console.log(index);
                    //console.log(value);
                    setTimeout(function () {
                        console.log(value);
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }
    </script>

    
    <script>
        var Branchlist = []
        function ModelPushPop() {
            var BranchID = $("#hdnBranchID").val();
            let a = [];

            $(".statecheckall:checked").each(function () {
                a.push(this.value);
            });

            $(".statecheck:checked").each(function () {
                a.push(this.value);
            });
            var str1
            //  alert(a);

            str1 = { BranchID: BranchID, Modellist: a }
            $.ajax({
                type: "POST",
                url: "Branch.aspx/GetModelListSubmit",
                data: JSON.stringify(str1),
                contentType: "application/json; charset=utf-8",
                datatype: "json",
                success: function (responseFromServer) {
                    // alert(responseFromServer.d)
                    $("#myModal").modal('hide');
                    jAlert('Model assigned successfully');
                }
            });
        }

        function CheckParticular(v) {
            if (v == false) {
                $(".statecheckall").prop('checked', false);
            }
        }

        function CheckAll(id) {
            var ischecked = $(".statecheckall").is(':checked');
            if (ischecked == true) {
                $('input:checkbox.statecheck').each(function () {
                    $(this).prop('checked', true);
                });

            }
            else {
                $('input:checkbox.statecheck').each(function () {
                    $(this).prop('checked', false);
                });

            }


        }

        function fn_ReceiptChallanMap(BranchID) {
            $("#hdnBranchID").val(BranchID);
            var str
            str = { BranchID: BranchID }
            var html = "";
            // alert();
            $.ajax({
                type: "POST",
                url: "Branch.aspx/GetModuuleList",
                data: JSON.stringify(str),
                contentType: "application/json; charset=utf-8",
                datatype: "json",
                success: function (responseFromServer) {
                    for (i = 0; i < responseFromServer.d.length; i++) {
                        if (responseFromServer.d[i].IsChecked == true) {
                            html += "<li><input type='checkbox' id=" + responseFromServer.d[i].ModuleID + "  class='statecheck' onclick=CheckParticular($(this).is(':checked')) value=" + responseFromServer.d[i].ModuleID + " checked  /><a href='#'><label id='lblstatename' class='lblstate' for=" + responseFromServer.d[i].ModuleID + " >" + responseFromServer.d[i].Module + "</label></a></li>";
                        }
                        else {
                            html += "<li><input type='checkbox' id=" + responseFromServer.d[i].ModuleID + " class='statecheck'  onclick=CheckParticular($(this).is(':checked')) value=" + responseFromServer.d[i].ModuleID + "   /><a href='#'><label id='lblstatename' class='lblstate' for=" + responseFromServer.d[i].ModuleID + ">" + responseFromServer.d[i].Module + "</label></a></li>";
                        }
                    }
                    $("#divModalBody").html(html);
                    $("#myModal").modal('show');
                }
            });
        }

        // Mantis Issue 24142
        function upload_Signature(BranchID) {
            cImgSignature.SetImageUrl("");
            cupldSignature.ClearText;

            $("#hdnSigBranchID").val(BranchID);
            var str
            str = { BranchID: BranchID }

            $.ajax({
                type: "POST",
                url: "Branch.aspx/SetImage",
                data: JSON.stringify(str),
                contentType: "application/json; charset=utf-8",
                datatype: "json",
                success: function (responseFromServer) {
                    var filePath = responseFromServer.d;
                    if (filePath != "") {
                        cImgSignature.SetImageUrl(filePath);
                    }
                }
            });

            $("#ModalSignature").modal('show');
        }

        //function save_Signature(e) {
        //    var SignatureFileName = cImgSignature.GetImageUrl();
        //    var BranchID = $("#hdnSigBranchID").val();
        //    var param = { BranchID: BranchID, SignatureFileName: SignatureFileName }

        //    $.ajax({
        //        type: "POST",
        //        url: "Branch.aspx/save_Signature",
        //        data: JSON.stringify(param),
        //        contentType: "application/json; charset=utf-8",
        //        datatype: "json",
        //        success: function (responseFromServer) {
        //            var filePath = responseFromServer.d;
        //            if (filePath != "") {
                        
        //                cupldSignature.UploadedFiles[0].SaveAs(filePath);

        //            }
        //        }
        //    });

        //    $("#ModalSignature").modal('hide');
        //}

        function close_Signature(e)
        {
            $("#ModalSignature").modal('hide');
        }

        function Validate_Signature(e)
        {
            if (cupldSignature.GetText() == "") {
                jAlert("No signature selected.");
            }
        }
        // End of Mantis Issue 24142
    </script>
    <style>
        .pl5 {
            margin-left:8px;
        }
        /*Mantis Issue 24142*/
        .cityDiv>img, #ImgSignature {
            max-width:100%;
            margin-bottom:8px
        }
        /*End of Mantis Issue 24142*/
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Branches</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100" style="width: 100%">
            <tr>
                <td style="text-align: left; vertical-align: top">
                    <table>
                        <tr>
                            <td id="ShowFilter">
                                <% if (rights.CanAdd)
                                   { %>
                                <a href="javascript:void(0);" onclick="javascript:OnAddButtonClick();" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus"></i></span>Add New</a>
                                <% } %>
                                <% if (rights.CanExport)
                                   { %>
                                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                    <asp:ListItem Value="2">XLS</asp:ListItem>
                                    <asp:ListItem Value="3">RTF</asp:ListItem>
                                    <asp:ListItem Value="4">CSV</asp:ListItem>

                                </asp:DropDownList>
                                <% } %>
                                <%--<a href="javascript:ShowHideFilter('s');" class="btn btn-primary"><span>Show Filter</span></a>--%>
                            </td>
                            <td id="Td1">
                                <%--<a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>--%>
                            </td>
                        </tr>
                    </table>
                </td>
                <td class="gridcellright" style="float: right; vertical-align: top">
                    <%-- <div style="float: right">--%>
                    <%--<dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true"
                        Font-Bold="False" ForeColor="black" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                        ValueType="System.Int32" Width="130px">
                        <Items>
                            <dxe:ListEditItem Text="Select" Value="0" />
                            <dxe:ListEditItem Text="PDF" Value="1" />
                            <dxe:ListEditItem Text="XLS" Value="2" />
                            <dxe:ListEditItem Text="RTF" Value="3" />
                            <dxe:ListEditItem Text="CSV" Value="4" />
                        </Items>
                        <Border BorderColor="black" />
                        <DropDownButton Text="Export">
                        </DropDownButton>
                    </dxe:ASPxComboBox>--%>
                    <%-- </div>--%>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top; text-align: left" colspan="2" class="relative">
                    <dxe:ASPxGridView ID="gridStatus" ClientInstanceName="grid" Width="100%"
                        KeyFieldName="branch_id" DataSourceID="gridStatusDataSource" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" runat="server"
                        AutoGenerateColumns="False" OnCustomCallback="gridStatus_CustomCallback" SettingsBehavior-AllowFocusedRow="true">
                        <SettingsSearchPanel Visible="True" Delay="5000" />
                        <ClientSideEvents EndCallback="function(s, e) {
	  EndCall(s.cpEND);
}" />
                        <Columns>
                            <dxe:GridViewDataTextColumn Visible="False" FieldName="branch_id" Caption="ID">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="branch_code"
                                Caption="Short Name">
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="branch_description"
                                Caption="Branch Name">
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="ParentBranch"
                                Caption="Parent Branch">
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="branch_cpPhone"
                                Caption="Branch Phone" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="Country"
                                Caption="Country" Visible="true">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="State"
                                Caption="State" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="City"
                                Caption="City" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn VisibleIndex="10" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="0">
                                <%--<CellStyle CssClass="gridcellleft">
                                </CellStyle>--%>

                                <DataItemTemplate>
                                    <div class='floatedBtnArea'>
                                        <%-- <% if (rights.CanDelete)
                                       { %>--%>

                                        <%-- <% } %>--%>
                                        <% if (rights.CanEdit)
                                           { %>
                                        <a href="javascript:void(0);" onclick="OnEditButtonClick('<%# Container.KeyValue %>')" title="" class="">
                                            <span class='ico ColorSix'><i class='fa fa-pencil'></i></span><span class='hidden-xs'>Edit</span>
                                        </a>
                                        <% } %>
                                        <% if (rights.CanDelete)
                                           { %>
                                        <a href="javascript:void(0);" onclick="DeleteRow('<%# Container.KeyValue %>')" title="">
                                            <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                                        <% } %>

                                         <% if (SrvBranchMap && rights.CanAddUpdateDocuments)
                                           { %>
                                        <a href="javascript:void(0);" onclick="fn_ReceiptChallanMap('<%# Container.KeyValue %>')" title="">
                                            <span class='ico deleteColor'><i class='fa fa-sitemap' aria-hidden='true'></i></span><span class='hidden-xs'>Receipt Challan Mapping</span></a>
                                         <% } %>
                                        <%--Mantis Issue 24142--%>
                                        <a href="javascript:void(0);" onclick="upload_Signature('<%# Container.KeyValue %>')" title="">
                                            <span class='ico attachColor'><i class='fa fa-sitemap' aria-hidden='true'></i></span><span class='hidden-xs'>Add Signature</span></a>
                                        <%--End of Mantis Issue 24142--%>
                                    </div>
                                </DataItemTemplate>
                                <HeaderTemplate></HeaderTemplate>
                                <%--<HeaderTemplate>--%>
                                <%-- <a href="javascript:void(0);" onclick="javascript:OnAddButtonClick();"><span style="color: #000099; text-decoration: underline">Add New</span> </a>--%>
                                <%--</HeaderTemplate>--%>
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                        </Columns>
                        <ClientSideEvents RowClick="gridRowclick" />
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <SettingsText ConfirmDelete="Confirm delete?" />
                        <StylesEditors>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </StylesEditors>
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowFilterRow="true" ShowFilterRowMenu="True" />
                        <%--<Styles>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </Styles>--%>
                        <%--<SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>--%>
                        <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server">
        </dxe:ASPxGridViewExporter>
        <asp:SqlDataSource ID="gridStatusDataSource" runat="server"
            SelectCommand="">
            <%--  <SelectParameters>
                <asp:SessionParameter Name="userlist" SessionField="userchildHierarchy" Type="string" />
            </SelectParameters>--%>
        </asp:SqlDataSource>
    </div>

    
    <div id="myModal" class="modal fade" data-backdrop="static" role="dialog">
        <div class="modal-dialog" style="width: 450px;">

            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Receipt Challan List</h4>
                </div>
                <div class="modal-body">
                    <div>

                        <input type="text" id="myInput" onkeyup="myFunction()" placeholder="Search for Receipt Challan.">

                        <ul id="divModalBody" class="listStyle">
                            <%--<input type="checkbox" id="idstate" class="statecheck" /><label id="lblstatename" class="lblstate"></label>--%>
                        </ul>
                    </div>
                    <input type="button" id="btnsatesubmit" title="SUBMIT" value="SUBMIT" class="btn btn-primary" onclick="ModelPushPop()" />
                    <input type="hidden" id="hdnstatelist" class="btn btn-primary" />
                    <input type="hidden" id="hdnBranchID" class="btn btn-primary" />
                </div>
            </div>

        </div>
    </div>
    <%--Mantis Issue 24142--%>
    <div id="ModalSignature" class="modal fade pmsModal w40" data-backdrop="static" role="dialog">
        <div class="modal-dialog" style="width: 550px; ">

            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Select Signature</h4>
                </div>
                <div class="modal-body" style="border-radius: 0 0 5px 5px">
                    <div style="margin-right:5px">
                        <div class="cityDiv" style="height: auto;">
                            <dxe:ASPxImage ID="ImgSignature" runat="server"  ClientInstanceName="cImgSignature" CssClass="myImageSmall" ></dxe:ASPxImage>
                        </div>
                        <dxe:ASPxUploadControl ID="upldSignature" runat="server" ClientInstanceName="cupldSignature" 
                            ShowProgressPanel="True" CssClass="pull-left">
                            <ValidationSettings MaxFileSize="2194304" AllowedFileExtensions=".jpg, .jpeg, .gif, .png" ErrorStyle-CssClass="validationMessage" />
                                                
                        </dxe:ASPxUploadControl>
                          
                     </div>
                    
                    <div style="padding-left:10px" >
                        <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="upldSignature" CssClass="btn btn-success pl5" 
                            OnClientClick="Validate_Signature()" OnClick="save_Signature" Width="73px"   />
                        <input type="button" id="btnClose" title="Close" value="Close" class="btn btn-primary" onclick="close_Signature()" />
                        <asp:Button ID="btnSignatureDelete" runat="server" Text="Delete" ValidationGroup="btnSignatureDelete" CssClass="btn btn-danger" 
                            OnClick="btnSignatureDelete_Click" Width="73px" />
                        <%--<input type="button" id="btnSave" title="Save" value="Save" class="btn btn-primary" onclick="save_Signature()" />--%>
                    </div>
                    <div class="clear"></div>
                    <div style="padding-left:10px" >
                        
                    </div>
                </div>
            </div>

        </div>
    </div>

    <input type="hidden" id="hdnSigBranchID" class="btn btn-primary" runat="server" />

    <%--End of Mantis Issue 24142--%>

    <style>
        #myInput {
            background-image: url('/css/searchicon.png'); /* Add a search icon to input */
            background-position: 10px 12px; /* Position the search icon */
            background-repeat: no-repeat; /* Do not repeat the icon image */
            width: 100%; /* Full-width */
            font-size: 16px; /* Increase font-size */
            padding: 12px 20px 12px 40px; /* Add some padding */
            border: 1px solid #ddd; /* Add a grey border */
            margin-bottom: 12px; /* Add some space below the input */
        }

        #divModalBody {
            /* Remove default list styling */
            list-style-type: none;
            padding: 0;
            margin: 0;
            margin-bottom: 8px;
        }

            #divModalBody li {
                padding: 5px 10px;
            }

                #divModalBody li a {
                    margin-top: -1px; /* Prevent double borders */
                    padding: 0 12px; /* Add some padding */
                    text-decoration: none; /* Remove default text underline */
                    font-size: 14px; /* Increase the font-size */
                    color: black; /* Add a black text color */
                    display: inline-block; /* Make it into a block element to fill the whole list */
                    cursor: pointer;
                }
    </style>

    <script>
        function myFunction() {
            // Declare variables
            var input, filter, ul, li, a, i, txtValue;
            input = document.getElementById('myInput');
            filter = input.value.toUpperCase();
            ul = document.getElementById("divModalBody");
            li = ul.getElementsByTagName('li');

            // Loop through all list items, and hide those who don't match the search query
            for (i = 0; i < li.length; i++) {
                a = li[i].getElementsByTagName("a")[0];
                txtValue = a.textContent || a.innerText;

                if (txtValue.toUpperCase().indexOf(filter) > -1) {
                    li[i].style.display = "";
                } else {
                    li[i].style.display = "none";
                }

            }
        }
    </script>
</asp:Content>
