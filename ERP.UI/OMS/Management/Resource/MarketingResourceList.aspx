<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="MarketingResourceList.aspx.cs" Inherits="ERP.OMS.Management.Resource.MarketingResourceList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script>

        $(document).ready(function () {

            document.onkeydown = function (e) {

                if (event.altKey == true) {

                    switch (event.keyCode) {

                        case 97:
                            StopDefaultAction(e);


                            var url = 'Document-Attachment.aspx?key=' + 'Add';
                            window.location.href = url;


                            break;
                        case 65:
                            StopDefaultAction(e);

                            var url = 'Document-Attachment.aspx?key=' + 'Add';
                            window.location.href = url;

                            break;


                        case 83:

                            StopDefaultAction(e);
                            gridDocumentAttachment.PerformCallback();

                            break;

                        case 115:

                            StopDefaultAction(e);
                            gridDocumentAttachment.PerformCallback();

                            break;
                    }

                }
            }
        });

        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };

            e.returnValue = false;
            e.stopPropagation();
        }
        function getUrlVars() {
            var vars = [], hash;
            var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < hashes.length; i++) {
                hash = hashes[i].split('=');
                vars.push(hash[0]);
                vars[hash[0]] = hash[1];
            }
            return vars;
        }


        function OnAddButtonClick() {
            var url = 'MarketingResouce.aspx?key=' + 'Add';
            window.location.href = url;
        }



        function DeleteAttachment(keyValue) {

            var prodcode = $("#hdnprodID").val();
            jConfirm('Do you want to delete?', 'Confirmation Dialog', function (r) {

                if (r == true) {
                    $.ajax({
                        type: "POST",
                        url: "Document-Attachmentlist.aspx/DeleteAttachment",
                        data: "{'AttchmentID':'" + keyValue + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",

                        success: function (msg) {

                            gridDocumentAttachment.PerformCallback();
                        }
                    });
                }


            });
        }

        function BindTemplate() {
            gridDocumentAttachment.PerformCallback();


        }
        function OnViewClick(keyValue) {
            var url = 'Document-Attachment.aspx?key=' + keyValue + '&req=V';
            window.location.href = url;
        }

        function DownloadImage(keyValue) {

            //$.ajax({
            //    type: "POST",
            //    url: "Document-Attachment.aspx/DownloadAttachment",
            //    data: JSON.stringify({ Id: keyValue }),
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    success: function (data) {
            //    }
            //});

            window.open("../Import/DownloadAttachment.aspx?D=" + keyValue);

        }

        function Callback_BeginCallback() {
            $("#drdExport").val(0);
        }
        function gridRowclick(s, e) {
            $('#gridAttachment').find('tr').removeClass('rowActive');
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
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                gridDocumentAttachment.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                gridDocumentAttachment.SetWidth(cntWidth);
            }
            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    gridDocumentAttachment.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    gridDocumentAttachment.SetWidth(cntWidth);
                }

            });
        });
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-heading">
        <div class="panel-title">
            <h3>Marketing Resources </h3>
        </div>

    </div>
    <div class="form_main">
        <div class="clearfix">

            <% if (rights.CanAdd)
               { %>
            <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span><span><u>A</u>dd Resources</span> </a>

            <%} %>


            <% if (rights.CanExport)
               { %>

            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius"
                OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>

            <%} %>
                
                
                <span style="float: right;">
                      <input type="button" value="Show" class="btn btn-primary" onclick="BindTemplate()" />
                </span>


        </div>

    </div>
    <div class="relative">


        <dxe:ASPxGridView ID="gridAttachment" ClientInstanceName="gridDocumentAttachment" runat="server" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Auto"
            KeyFieldName="DocID" Width="100%" OnCustomCallback="Grdattachment_CustomCallback" OnDataBinding="gridAttachment_DataBinding"  ClientSideEvents-BeginCallback="Callback_BeginCallback"
            SettingsBehavior-AllowFocusedRow="true">

            <columns>



                <dxe:GridViewDataTextColumn FieldName="Docnumber" VisibleIndex="2" Caption="Document ">
                     <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

               <dxe:GridViewDataTextColumn FieldName="Vendor" VisibleIndex="3" Caption="Entity">
                     <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn FieldName="uploaded_on" VisibleIndex="4" Caption="Uploaded On" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" >
                     <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn FieldName="Username" VisibleIndex="5" Caption="Uploaded By"  >
                     <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>



                <dxe:GridViewDataTextColumn FieldName="Contenttype" VisibleIndex="6" Caption="Document Type">
                     <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>



                <dxe:GridViewDataTextColumn VisibleIndex="7" HeaderStyle-HorizontalAlign="Center" Caption="Action" CellStyle-HorizontalAlign="Center" Width="0" >
                    <DataItemTemplate>
                        <div class='floatedBtnArea'>
                      <%--  <% if (rights.CanView)
                            { %>--%>
                        <a href="javascript:void(0);" onclick="OnViewClick('<%#Eval("DocID") %>')" class="" title="" >
                            <span class='ico ColorFive'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span></a>
                          <%-- <% } %>--%>

                        <a href="javascript:void(0);" onclick="DownloadImage('<%#Eval("DocID") %>')" title="" class="">
                            <span class='ico ColorSeven'><i class='fa fa-download'></i></span><span class='hidden-xs'>Download</span></a>
                         
                        
                          <% if (rights.CanDelete)
                              { %>

                        <a href="javascript:void(0);" onclick="DeleteAttachment('<%#Eval("DocID") %>')" title="" class="">
                            <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>

                          <%} %>
                        </div>

                    </DataItemTemplate>
                </dxe:GridViewDataTextColumn>



            </columns>


            <settingsbehavior confirmdelete="true" enablecustomizationwindow="true" enablerowhottrack="true" />
            <settings showfooter="true" showgrouppanel="true" showgroupfooter="VisibleIfExpanded" />

            <settingsediting mode="EditForm" />
            <settingscontextmenu enabled="true" />
            <settingsbehavior autoexpandallgroups="true" columnresizemode="Control" />
            <settings showgrouppanel="True" showstatusbar="Visible" showfilterrow="true" />


            <settingspager pagesize="10" Position="Bottom">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                </settingspager>
            <ClientSideEvents RowClick="gridRowclick" />

            <settings showfilterrow="True" showstatusbar="Visible" usefixedtablelayout="true" />


        </dxe:ASPxGridView>



    </div>

    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" GridViewID="gridAttachment" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>


</asp:Content>
