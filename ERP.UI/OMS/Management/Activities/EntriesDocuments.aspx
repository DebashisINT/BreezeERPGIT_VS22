<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="EntriesDocuments.aspx.cs" Inherits="ERP.OMS.Management.Activities.EntriesDocuments" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <script type="text/javascript">
      

      <%--  function disp_prompt(name) {
            debugger;
            if (name == "tab0") {
               
                document.location.href = "SalesOrderAdd.aspx?key=<%=Session["key_QutId"]%>";
            }
            if (name == "tab1") {
               
            }
            else if (name == "tab2") {
               
                document.location.href = "SalesOrderAdd.aspx?key=<%=Session["key_QutId"]%>&tab=billship";
            }

    }--%>

    function Changestatus(obj) {



        var URL = "../../verify_documentremarks.aspx?id=" + obj;
        window.location.href = URL;
        //editwin = dhtmlmodal.open("Editbox", "iframe", URL, "Verify Remarks", "width=995px,height=300px,center=0,resize=1,top=-1", "recal");
        //editwin.onclose = function () {
        //    gridDocument.PerformCallback();
        //}
    }


    function Show() {
        var url = "../Master/frmAddDocuments.aspx?id=Contact_Document.aspx&id1=<%=Session["requesttype"]%>&AcType=Add";
        popup.SetContentUrl(url);
        //alert (url);
        popup.Show();
        popup.SetHeaderText('Add Document');
    }
    function PopulateGrid(obj) {
        gridDocument.PerformCallback(obj);
    }
    function Changestatus(obj) {
        var URL = "../verify_documentremarks.aspx?id=" + obj;
        window.location.href = URL;
        //editwin = dhtmlmodal.open("Editbox", "iframe", URL, "Verify Remarks", "width=995px,height=300px,center=0,resize=1,top=-1", "recal");
        //editwin.onclose = function () {
        //    gridDocument.PerformCallback();
        //}
    }
    function ShowEditForm(KeyValue) {
        debugger;
        var url = '../Master/frmAddDocuments.aspx?id=Contact_Document.aspx&id1=<%=Session["requesttype"]%>&AcType=Edit&docid=' + KeyValue;
        popup.SetContentUrl(url);
        //alert (url);
        popup.Show();
        popup.SetHeaderText('Modify Document');
    }

    function OnDocumentView(obj1, obj2) {

        var docid = obj1;
        var filename;
        var n;
        var chk = obj2.includes("~");
        if (chk) {
            n = d.getFullYear();
            filename = obj2.split('~')[1];
        }
        else {
            //Rev mantis 0025559
            n = obj2.split('~')[1];
              //Rev mantis 0025559 end
            filename = obj2.split('/')[2];
        }
        if (filename != '' && filename != null) {
            var d = new Date();
           // var n = d.getFullYear();//Rev mantis 0025559
            var url = '\\OMS\\Management\\Documents\\' + docid + '\\' + n + '\\' + filename;

            //window.open(url, '_blank');
            var seturl = '\\OMS\\Management\\DailyTask\\viewImage.aspx?id=' + url;
            // alert(seturl);
            $("#viewDocModal").modal("show");

            var extension = url.substr((url.lastIndexOf('.') + 1)).toLowerCase();
            switch (extension) {
                case 'pdf':
                    $('#pdfview').html("");
                    var htm = "";
                    htm += '<object data="' + url + '" type="application/pdf"  height="600"> </object>'
                    $('#pdfview').attr('data', url);
                    $('#imageview').hide();
                    $('#pdfview').html(htm);
                    break;
                case 'jpg':
                case 'png':
                case 'gif':
                case 'jpeg':
                    $('#imageview').attr('src', url);
                    $('#pdfview').hide();
                    $('#imageview').show();
                    break;
                default:
                    $('#viewDocModal').modal('hide');
                    window.open(
                       url,
                      '_blank'
                    );
            }
            

            

            //popup.contentUrl = url;
            //popup.Show();
            //popup.SetHeaderText('View Document');
        }
        else {
            jAlert('File not found.')
        }

        //var url = 'Copy of viewImage.aspx?id=' + keyValue;
        //popup.contentUrl = url;
        //popup.Show();

    }
    
    function DeleteRow(keyValue) {
        //doIt = confirm('Confirm delete?');
        //if (doIt) {
        //    gridDocument.PerformCallback('Delete~' + keyValue);
        //    height();
        //}
        //else {

        //}
        jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
            if (r == true) {
                gridDocument.PerformCallback('Delete~' + keyValue);
                //height();
            }
        });


    }
    </script>
    <style type="text/css">
        /*.dxpcLite_PlasticBlue .dxpc-contentWrapper, .dxdpLite_PlasticBlue .dxpc-contentWrapper {
            border-bottom:none !important;
        }*/
        input#txtFilepath {
            margin-bottom: 3px !important;
        }

        #LinkButton1 {
            right: 9px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">

        <div class="panel-title">           
            <h3>
                <asp:Label ID="lblheader" runat="server" Text="Documents of Product "></asp:Label></h3>
            <div class="crossBtn"><a href="/OMS/management/Activities/SalesOrderEntityList.aspx" id="lnkListPage" runat="server"><i class="fa fa-times"></i></a></div>
           
        </div>
    </div>
    <div class="form_main">
        <table width="100%">
            <tr>
                <td class="EHEADER" style="text-align: center">
                    <asp:Label ID="lblName" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <table class="TableMain100">
            <tr>
                <td>
                    <div class="row"> 

                        <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="1" Visible="true" ClientInstanceName="page">
                            <TabPages>

                                 <dxe:TabPage Name="General" Visible="true" Text="General">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>


                                <dxe:TabPage Text="Document" Visible="true" Name="[A]ttachment(s)">

                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">


                                              <% if (rights.CanAdd)
                                               { %>
                                            <div><a href="javascript:void(0);" onclick="Show();" class="btn btn-primary"><span>Add New</span> </a></div>
                                             <% } %>
                                            <dxe:ASPxGridView ID="EmployeeDocumentGrid" runat="server" AutoGenerateColumns="False"
                                                ClientInstanceName="gridDocument" KeyFieldName="Id" Width="100%" Font-Size="12px"
                                                OnCustomCallback="EmployeeDocumentGrid_CustomCallback" OnHtmlRowCreated="EmployeeDocumentGrid_HtmlRowCreated" OnRowCommand="EmployeeDocumentGrid_RowCommand">
                                                <SettingsSearchPanel Visible="True" />
                                                <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                                                <Columns>
                                                    <dxe:GridViewDataTextColumn FieldName="Id" ReadOnly="True" VisibleIndex="0" Visible="False">
                                                        <EditFormSettings Visible="False" />
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="Type" VisibleIndex="0" Caption="Doc.Type">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="FileName" VisibleIndex="1" Caption="Doc.Name">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="Src" VisibleIndex="2" Visible="False">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="Note1" VisibleIndex="3" Visible="true" Caption="Note 1">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="Note2" VisibleIndex="4" Visible="true" Caption="Note 2">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="Fileno" VisibleIndex="5" Visible="true">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="Bldng" VisibleIndex="6" Visible="true">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="FilePath" ReadOnly="True" VisibleIndex="7"
                                                        Caption="Location">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="createuser" ReadOnly="True" VisibleIndex="8"
                                                        Caption="Upload By">
                                                    </dxe:GridViewDataTextColumn>
                                                  
                                                    <dxe:GridViewDataTextColumn Caption="Verified By" FieldName="vrfy" ReadOnly="True"
                                                        VisibleIndex="9">
                                                        <EditFormSettings Visible="False" />
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="ReceiveDate" VisibleIndex="10" Visible="true">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="RenewDate" VisibleIndex="11" Visible="true">
                                                    </dxe:GridViewDataTextColumn>
                                                   
                                                    <dxe:GridViewDataTextColumn VisibleIndex="13" Width="8%" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center">
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <DataItemTemplate>
                                                            <a onclick="OnDocumentView('<%#Eval("doc") %>','<%#Eval("Src") %>')" style="text-decoration: none; cursor: pointer;" title="View Document" class="pad">
                                                                <img src="../../../assests/images/viewIcon.png" />
                                                            </a>
                                                           
                                                            <a href="javascript:void(0);" onclick="ShowEditForm('<%# Container.KeyValue %>');" style="text-decoration: none;" title="Edit" class="pad">
                                                                <img src="/assests/images/Edit.png" />
                                                            </a>
                                                            <a href="javascript:void(0);" onclick="DeleteRow('<%# Container.KeyValue %>')" style="text-decoration: none;" title="Delete">
                                                                <img src="/assests/images/Delete.png" />
                                                            </a>
                                                        </DataItemTemplate>
                                                        <HeaderTemplate>
                                                            Action
                                                    
                                                        </HeaderTemplate>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                  
                                                </Columns>
                                                <Settings ShowGroupPanel="True" ShowStatusBar="Visible" />
                                                <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center"
                                                    PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="700px"
                                                    EditFormColumnCount="1" />
                                                <Styles>
                                                    <LoadingPanel ImageSpacing="10px">
                                                    </LoadingPanel>
                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                    </Header>
                                                </Styles>
                                                <SettingsText PopupEditFormCaption="Add/Modify Documents" ConfirmDelete="Confirm delete?" />
                                                <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True">
                                                    <FirstPageButton Visible="True">
                                                    </FirstPageButton>
                                                    <LastPageButton Visible="True">
                                                    </LastPageButton>
                                                </SettingsPager>
                                                <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                                               
                                            </dxe:ASPxGridView>
                                            <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" ContentUrl="frmAddDocuments.aspx"
                                                CloseAction="CloseButton" Top="120" Left="300" ClientInstanceName="popup" Height="466px"
                                                Width="900px" HeaderText="Add/Modify Document" AllowResize="false" ContentStyle-Wrap="True" ResizingMode="Live" Modal="true">
                                                <ContentCollection>
                                                    <dxe:PopupControlContentControl runat="server">
                                                    </dxe:PopupControlContentControl>
                                                </ContentCollection>
                                                <FooterTemplate > 
                                                    <button id="printAttachment">Print</button>
                                                </FooterTemplate>
                                                <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
                                            </dxe:ASPxPopupControl>
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>

                                 <dxe:TabPage Name="[B]illing/Shipping" Visible="true" Text="[B]illing/Shipping">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                            </TabPages>
                            <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                            var activeTab   = page.GetActiveTab();	                                            
	                                            var Tab0 = page.GetTab(0);
                                                var Tab1 = page.GetTab(1);
                                                var Tab2 = page.GetTab(2);
                                              
	                                            if(activeTab == Tab0)
	                                            {
	                                                disp_prompt('tab0');
	                                            }
	                                            if(activeTab == Tab1)
	                                            {
	                                                disp_prompt('tab1');
	                                            }
	                                           else if(activeTab == Tab2)
	                                            {
	                                                disp_prompt('tab2');
	                                            }

	                                            }"></ClientSideEvents>
                            
                         
                        </dxe:ASPxPageControl>
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="txtID" runat="server" Visible="false"></asp:TextBox>
                </td>
            </tr>
        </table>
      
    </div>

    <!-- Modal -->
<div class="modal pmsModal fade" id="viewDocModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLongTitle" aria-hidden="true">
  <div class="modal-dialog" role="document" style="padding: 0;">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="exampleModalLongTitle">Document of Purchase GRN</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
          <div style="max-height:70vh;overflow-y:auto">
            <div class="pdfViewerWrap">
                <div id="pdfview"></div>
                
            </div>
                
            <div class="imageViewerWrap text-center">
                <img src="" alt="Image no found"  id="imageview" style="max-width:100%">
            </div>
          </div>
      </div>
      <div class="modal-footer">
        
         <button type="button" class="btn btn-success" id="toPrintBtn">Print</button>
          <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
      </div>
    </div>
  </div>
</div>
<script>
    $(document).ready(function () {
        $("body").on("click", "#toPrintBtn", function () {
            var url = $("#imageview").attr("src");
            var extension = url.substr((url.lastIndexOf('.') + 1)).toLowerCase();
            switch (extension) {
                case 'pdf':

                    break;
                case 'jpg':
                case 'png':
                case 'gif':
                case 'jpeg':
                    PrintImage(url);
                    break;
                default:

            }
            
        })
    })
    function ImagetoPrint(source) {
        return "<html><head><script>function step1(){\n" +
                "setTimeout('step2()', 10);}\n" +
                "function step2(){window.print();window.close()}\n" +
                "</scri" + "pt></head><body onload='step1()'>\n" +
                "<img src='" + source + "' style='max-width:100%' /></body></html>";
    }
    function PrintImage(source) {
        Pagelink = "about:blank";
        var pwa = window.open(Pagelink, "_new");
        pwa.document.open();
        pwa.document.write(ImagetoPrint(source));
        pwa.document.close();
    }
</script>
</asp:Content>
