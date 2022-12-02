<%@ Page Title="Port Details" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.frm_PortCode"
    CodeBehind="frm_PortCode.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .popUpHeader {
            float: right;
        }

        .stateDiv {
            height: 25px;
            width: 105px;
            float: left;
        }

        .dxpc-headerContent {
            color: white;
        }
    </style>
    <%-- <script type="text/javascript" src="../../CentralData/JSScript/GenericJScript.js"></script>--%>

    <link href="../Activities/CSS/SearchPopup.css" rel="stylesheet" />
    <script src="../Activities/JS/SearchPopup.js"></script>


    <style>
        .dxgvHeader {
            border: 1px solid #2c4182 !important;
            background-color: #415698 !important;
        }

            .dxgvHeader, .dxgvHeader table {
                color: #fff !important;
            }

        .pullleftClass {
            position: absolute;
            right: 133px;
            top: 15px;
        }
    </style>

    <script>


       



        function StateClick(s, e) {
           
            var txt = "<table border='1' width=\"100%\"><tr class=\"HeaderStyle\"><th>State Name</th><th>State Code</th></tr><table>";
            document.getElementById("PortEditTable").innerHTML = txt;
            // $('#PortEditModel').modal('show');
            if (ctxt_Country.GetText().trim() == "") {
                setTimeout(function () {
                    jAlert("Select a country.");
                }, 200);

            } else {
               
                cPortEditModel.Show();
                $('#txtStateSearch').focus();
            }
       }
       
        function StateSearchkeydown(e) {
            var obj = {};
            obj.countryId = ctxt_Country.GetValue();
            obj.SearchKey = $('#txtStateSearch').val();
            if (e.code == "Enter" || e.code == "NumpadEnter") {
             
                var HeaderCaption = [];
                HeaderCaption.push("State Name");
                HeaderCaption.push("State Code");
                if ($('#txtStateSearch').val() != "") {
              
                    callonServer("../Activities/Services/Master.asmx/GetStateListByCountryId", obj, "PortEditTable", HeaderCaption, "PortEditIndex", "SetState");


                }
            }

            else if (e.code == "ArrowDown") {
                if ($("input[PortEditIndex=0]"))
                    $("input[PortEditIndex=0]").focus();
            }
        }

        function SetState(Id, Name) {

            $('#hdStateId').val(Id);
            ctxt_State.SetText(Name);

            cPortEditModel.Hide();
        }
        function ValueSelected(e, indexName) {
            if (indexName == "PortEditIndex") {
                if (e.code == "Enter" || e.code == "NumpadEnter") {
                    var Code = e.target.parentElement.parentElement.cells[0].innerText;
                    var name = e.target.parentElement.parentElement.cells[1].children[0].value;

                    $("#hdStateId").val(Code);
                    ctxt_State.SetText(name);
                    cPortEditModel.Hide();
                } else if (e.code == "ArrowDown") {
                    thisindex = parseFloat(e.target.getAttribute(indexName));
                    thisindex++;
                    if (thisindex < 10)
                        $("input[" + indexName + "=" + thisindex + "]").focus();
                }

                else if (e.code == "ArrowUp") {
                    thisindex = parseFloat(e.target.getAttribute(indexName));
                    thisindex--;
                    if (thisindex > -1)
                        $("input[" + indexName + "=" + thisindex + "]").focus();
                    else {
                        $('#txtStateSearch').focus();
                    }
                }

            }
        }


        function StateKeydown(s, e) {
            if (e.htmlEvent.key == "Enter" || e.htmlEvent.key == "NumEnter") {
                s.OnButtonClick(0);
            }
        }

        function Country_change() {
            ctxt_State.SetText('');
            $('#hdStateId').val('');

            if (ctxt_Country.GetText == '') { 
                ctxt_State.SetState('');
               

            }
            else { 
            }


        }



        function isNumberKey(e) {
            {
                if (e.htmlEvent.keyCode != 192)
                { return true; }
                else { _aspxPreventEvent(e.htmlEvent); }

            }
        }
        function txtBillNo_TextChanged() {    // function 3
            var mode = ''
            var portcode = ctxt_PorctCode.GetText();
            if (portcode.trim() != '') {
                $.ajax({
                    type: "POST",
                    url: "frm_PortCode.aspx/CheckUniqueName",
                    data: JSON.stringify({ portcode: portcode }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg) {
                        var data = msg.d;

                        if (data == true) {
                            $("#DuplicateBillNo").show();
                            ctxt_PorctCode.SetText('');
                            ctxt_PorctCode.Focus();
                            //document.getElementById("portcode").value = '';
                            //document.getElementById("portcode").focus();
                        }
                        else {
                            $("#DuplicateBillNo").hide();
                            $("#valid").hide();
                        }
                    }
                });
            }
        }
        function gridRowclick(s, e) {
            $('#PortCode').find('tr').removeClass('rowActive');
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
                grid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                grid.SetWidth(cntWidth);
            }
            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    grid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    grid.SetWidth(cntWidth);
                }

            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
        <div class="panel-heading">
            <div class="panel-title">
                <h3>Port Details</h3>
            </div>
        </div>
        <div class="form_main">
            <div class="Main">
                <div class="SearchArea clearfix">
                    <div class="FilterSide">
                        <div style="float: left; padding-right: 5px;">
                            <% if (rights.CanAdd)
                               { %>
                            <a href="javascript:void(0);" onclick="fn_PopUpOpen()" class="btn btn-success btn-radius "><span class="btn-icon"><i class="fa fa-plus" ></i></span><span>Add New</span> </a>
                            <% } %>
                            <%--<a href="javascript:ShowHideFilter('s');" class="btn btn-primary"><span>Show Filter</span></a>--%>
                            <% if (rights.CanExport)
                               { %>
                            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius " OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                                <asp:ListItem Value="0">Export to</asp:ListItem>
                                <asp:ListItem Value="1">PDF</asp:ListItem>
                                <asp:ListItem Value="2">XLS</asp:ListItem>
                                <asp:ListItem Value="3">RTF</asp:ListItem>
                                <asp:ListItem Value="4">CSV</asp:ListItem>
                            </asp:DropDownList>
                            <% } %>
                        </div>

                    </div>

                </div>

                <div class="GridViewArea relative">
                    <dxe:ASPxGridView ID="PortCode" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid"
                        KeyFieldName="Port_Id" Width="100%" OnHtmlEditFormCreated="PortCode_HtmlEditFormCreated" Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Auto"
                        OnCustomCallback="PortCode_CustomCallback" SettingsBehavior-AllowFocusedRow="true" DataSourceID="EntityServerModeDataSource" >
                        <Columns>

                            <dxe:GridViewDataTextColumn Caption="Port Code" FieldName="Port_Code" VisibleIndex="1" Width="15%">
                                <EditFormSettings Visible="True" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Description" FieldName="Port_Description" VisibleIndex="3" Width="70%">
                                <EditFormSettings Visible="True" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" VisibleIndex="3" Width="0">
                                <HeaderTemplate>
                                    <span></span>
                                </HeaderTemplate>
                                <DataItemTemplate>
                                    <div class='floatedBtnArea'>
                                    <% if (rights.CanEdit)
                                       { %>
                                    <a href="javascript:void(0);" onclick="fn_EditPortCode('<%#Container.KeyValue %>')" class="" title="">
                                        <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>
                                    <% } %>
                                    <% if (rights.CanDelete)
                                       { %>
                                    <a href="javascript:void(0);" onclick="fn_DeletePortCode('<%#Container.KeyValue %>')" title="">
                                        <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                                    <% } %>
                                    </div>
                                </DataItemTemplate>
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />

                        <ClientSideEvents EndCallback="function (s, e) {grid_EndCallBack();}" RowClick="gridRowclick"  />
                    </dxe:ASPxGridView>
                    <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                        ContextTypeName="ERPDataClassesDataContext" TableName="tbl_master_portcode" />

                    <asp:SqlDataSource runat="server" ID="dsCountry" SelectCommand="SELECT [cou_id], [cou_country] Country_Name FROM [tbl_master_country]"></asp:SqlDataSource>

                </div>

                <div class="PopUpArea">
                    <dxe:ASPxPopupControl ID="PopupPort" runat="server" ClientInstanceName="cPopupPort"
                        Width="400px" Height="100px" HeaderText="Add Port Details" PopupHorizontalAlign="Windowcenter"
                        PopupVerticalAlign="WindowCenter" CloseAction="closeButton" Modal="true">
                        <ContentCollection>
                            <dxe:PopupControlContentControl ID="countryPopup" runat="server">
                                <div class="Top clearfix">
                                    <div style="padding-top: 5px;" class="col-md-12">
                                        <div class="PostDiv" style="padding-top: 5px;">Port Code:<span style="color: red;">*</span></div>
                                        <div style="padding-top: 5px;">
                                            <dxe:ASPxTextBox ID="txt_PorctCode" ClientInstanceName="ctxt_PorctCode" Enabled="true"
                                                runat="server" Width="300px" MaxLength="20">
                                                <ClientSideEvents LostFocus="txtBillNo_TextChanged" KeyPress="function(s, e) {  if(e.htmlEvent.keyCode != 126 ) {  return true; } else {  ASPxClientUtils.PreventEvent(e.htmlEvent); } }" />
                                            </dxe:ASPxTextBox>
                                            <div id="valid" style="display: none; position: absolute;    right: 37px;top: 32px;">
                                                <img id="grid_DXPEForm_DXEFL_DXEditor2_EI" title="Mandatory" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-YRohc" alt="Required" />
                                            </div>
                                            <span id="DuplicateBillNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Duplicate Port Code not allowed"></span>
                                        </div>
                                        <div class="CountryDiv" style="padding-top: 5px;">Country:<span style="color: red;">*</span></div>
                                        <div style="padding-top: 5px;">
                                            <dxe:ASPxComboBox ID="txt_Country" ClientInstanceName="ctxt_Country" Enabled="true"
                                                runat="server" Width="300px" MaxLength="20" DataSourceID="dsCountry" ValueField="Cou_id" TextField="Country_Name">
                                                <ClientSideEvents SelectedIndexChanged="Country_change" />
                                            </dxe:ASPxComboBox>
                                            <span id="badd1" style="display: none ;position: absolute;    right: 37px;top: 79px;" class="mandt" >
                                                <img id="grid_DXPEForm_DXEFL_DXEditor2_EiI" title="Mandatory" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-YRohc" alt="Required" />
                                            </span>
                                        </div>
                                        <asp:HiddenField ID="hdStateId" runat="server" />
                                        <div class="StateDiv" style="padding-top: 5px;">State: <span style="color: red;">*</span></div>
                                        <div style="padding-top: 5px;">
                                            <dxe:ASPxButtonEdit ID="txt_State" ClientInstanceName="ctxt_State"   ReadOnly="true"
                                                runat="server" Width="300px">
                                                <Buttons>
                                                    <dxe:EditButton Text="..." Width="20px">
                                                    </dxe:EditButton>
                                                </Buttons>
                                                <ClientSideEvents   ButtonClick="StateClick" KeyDown="StateKeydown"  />
                                            </dxe:ASPxButtonEdit>
                                              <span id="baddState" style="display: none; position: absolute;    right: 37px;top: 127px;" class="mandt">
                                                <img id="grid_DXPEForm_DXEFL_DXEditor2_EiI" title="Mandatory" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-YRohc" alt="Required" />
                                            </span>
                                        </div>
                                        <div class="DescDiv" style="padding-top: 5px;">Description:</div>
                                        <div style="padding-top: 5px;">
                                            <%--<dxe:ASPxTextBox ID="txt_PorctDesc" ClientInstanceName="ctxt_PorctDesc" ClientEnabled="true" 
                        runat="server" Width="236px" MaxLength="500">
                        </dxe:ASPxTextBox>--%>
                                            <dxe:ASPxMemo ID="txt_PorctDesc" runat="server" Width="300px" ClientInstanceName="ctxt_PorctDesc" ClientEnabled="true" Height="71px" MaxLength="500"></dxe:ASPxMemo>
                                            <%--<div id="valid" style="display: none; position: absolute; right: -4px; top: 30px;">
                        <img id="grid_DXPEForm_DXEFL_DXEditor2_EI" title="Mandatory" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-YRohc" alt="Required" /></div>--%>
                                        </div>
                                    </div>
                                </div>
                                <div class="ContentDiv">
                                    <div class="ScrollDiv"></div>
                                    <br style="clear: both;" />
                                    <div class="Footer" style="padding-left: 120px;">
                                        <div style="float: left;">
                                            <dxe:ASPxButton ID="btnSave_PortCode" ClientInstanceName="cbtnSave_States" runat="server"
                                                AutoPostBack="False" Text="Save" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                                <ClientSideEvents Click="function (s, e) {btnSave_PortCode();}" />
                                            </dxe:ASPxButton>
                                        </div>
                                        <div style="">
                                            <dxe:ASPxButton ID="btnCancel_Country" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger" UseSubmitBehavior="false">
                                                <ClientSideEvents Click="function (s, e) {fn_btnCancel();}" />
                                            </dxe:ASPxButton>
                                        </div>
                                        <br style="clear: both;" />
                                    </div>
                                    <br style="clear: both;" />
                                </div>
                            </dxe:PopupControlContentControl>
                        </ContentCollection>
                        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                    </dxe:ASPxPopupControl>
                    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                    </dxe:ASPxGridViewExporter>
                </div>
                <div class="HiddenFieldArea" style="display: none;">
                    <dxe:ASPxHiddenField runat="server" ClientInstanceName="chfID" ID="hfID">
                    </dxe:ASPxHiddenField>
                </div>
            </div>
        </div>


    <dxe:ASPxPopupControl runat="server" ID="PortEditModel" ClientInstanceName="cPortEditModel"
        Width="500px" Height="300px" HeaderText="Search State" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">

        <HeaderTemplate>
            <span>Search State </span>
            <dxe:ASPxImage ID="ASPxImage1" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader">
                <ClientSideEvents Click="function(s, e){ 
                                cPortEditModel.Hide();
                            }" />
            </dxe:ASPxImage>
        </HeaderTemplate>


        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                <div class="modal-body">
                    <input type="text" onkeydown="StateSearchkeydown(event)" id="txtStateSearch" autofocus width="100%" placeholder="Search by State Name" />

                    <div id="PortEditTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">

                                <th>State Name</th>
                                <th>State Code</th>
                            </tr>
                        </table>
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>
    <%--</dxe:ASPxPopupControl>
  <div class="modal fade" id="PortEditModel" role="dialog">
    <div class="modal-dialog">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal">&times;</button>
          <h4 class="modal-title">Select State</h4>
        </div>
        <div class="modal-body">
           <input type="text" onkeydown="StateSearchkeydown(event)"  id="txtStateSearch" autofocus width="100%" placeholder="Search By State Name."/>
             
            <div id="PortEditTable">
                <table border='1' width="100%" class="dynamicPopupTbl">
                    <tr class="HeaderStyle">
                   <th class="hide">Id</th> 
                        <th >State Name</th>
                    </tr>
                </table>
            </div>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
        </div>
      </div>
      
    </div>
  </div>--%>

    <script type="text/javascript">


        //function HideValidation()
        //{
        //    var portCode = ctxt_PorctCode.GetText();
        //    portCode = portCode.trim();
        //    alert(portCode.length)
        //    if(portCode.length>0)
        //    {
        //        $('#valid').attr('style', 'display:none;position: absolute;right: 133px;top: 15px;');
        //    }

        //}
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function fn_PopUpOpen() {

            $('#valid').attr('style', 'display:none;');
            $('#badd1').attr('style', 'display:none;');
            $('#baddState').attr('style', 'display:none;');
            chfID.Set("hfID", '');
            ctxt_PorctCode.SetText('');
            ctxt_PorctDesc.SetText('');
            ctxt_Country.SetText('');
            ctxt_State.SetText('');
            $('#hdStateId').val('');

            ctxt_PorctCode.SetEnabled(true);
            cPopupPort.SetHeaderText('Add Port Details');
            //ctxtNseCode.SetText('');
            //ctxtBseCode.SetText('');
            //ctxtMcxCode.SetText('');
            //ctxtMcsxCode.SetText('');
            //ctxtNcdexCode.SetText('');
            //ctxtCdslCode.SetText('');
            //ctxtNsdlCode.SetText('');
            //ctxtNdmlCode.SetText('');
            //ctxtDotexidCode.SetText('');
            //ctxtCvlidCode.SetText('');
            cPopupPort.Show();

        }
        function fn_EditPortCode(keyValue) {

            grid.PerformCallback('Edit~' + keyValue);
            ctxt_PorctCode.SetEnabled(false);
        }
        function fn_DeletePortCode(keyValue) {
            //var result=confirm('Confirm delete?');
            //if(result)
            //{
            //    grid.PerformCallback('Delete~' + keyValue);
            //}
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    grid.PerformCallback('Delete~' + keyValue);
                }
                else {
                    return false;
                }
            });


        }
        function fn_btnCancel() {
            cPopupPort.Hide();
        }
        function btnSave_PortCode() {
            var portCode = ctxt_PorctCode.GetText();
            var country = ctxt_Country.GetText();
            var state = ctxt_State.GetText();

            if (portCode.trim() == '')
                //if (ctxt_PorctDesc.GetText() == '')
            {
                $('#valid').attr('style', 'display: block; position: absolute;    right: 37px;top: 32px;');
                // alert('Please Enter Country Name');
                ctxt_PorctCode.Focus();

            }
            else {
                $('#valid').attr('style', 'display: none; position: absolute;    right: 37px;top: 32px;');
                if (portCode.indexOf('~') != -1) {
                    jAlert('Invalid Port Code. Char(~) not allowed)');
                    return;
                }
                if (country.trim() == '') {

                    $('#badd1').attr('style', 'display: block ;position: absolute;    right: 37px;top: 79px;');
                    ctxt_Country.Focus();
                    return;
                }
                else
                {
                    $('#badd1').attr('style', 'display: none ;position: absolute;    right: 37px;top: 79px;');
                    
                }
               
                if (state.trim() == '') {

                    $('#baddState').attr('style', 'display: block; position: absolute;    right: 37px;top: 127px;');
                    ctxt_State.Focus();
                    return;
                }
                else
                {
                    $('#baddState').attr('style', 'display: none; position: absolute;    right: 37px;top: 127px;');
                }
                var id = chfID.Get('hfID');
                if (id == '')

                    //grid.PerformCallback('savePortCode~' + ctxt_PorctDesc.GetText() + '~' + ctxtNseCode.GetText() + '~' + ctxtBseCode.GetText() + '~' + ctxtMcxCode.GetText() + '~' + ctxtMcsxCode.GetText() + '~' + ctxtNcdexCode.GetText() + '~' + ctxtCdslCode.GetText() + '~' + ctxtNsdlCode.GetText() + '~' + ctxtNdmlCode.GetText() + '~' + ctxtDotexidCode.GetText() + '~' + ctxtCvlidCode.GetText());
                    grid.PerformCallback('savePortCode~' + ctxt_PorctDesc.GetText());
                else
                    grid.PerformCallback('updatePortCode~' + chfID.Get('hfID'));






                //if(state.trim()=='')
                //{
                //    $('#valid').attr('style', 'display:block;position: absolute;right: 133px;top: 15px;');
                //    return false
                //    ctxt_State.Focus();
                //}
                //else {
                //    cbtnSave_States.Focus();
                //}




            }
        }


        function grid_EndCallBack() {
            if (grid.cpEdit != null) {
                ctxt_PorctCode.SetText(grid.cpEdit.split('~')[0]);
                ctxt_PorctDesc.SetText(grid.cpEdit.split('~')[1]);

                var hfid = grid.cpEdit.split('~')[2];

                $("#hdStateId").val(grid.cpEdit.split('~')[3]);
                ctxt_State.SetText(grid.cpEdit.split('~')[4]);
                ctxt_Country.SetValue(grid.cpEdit.split('~')[5]);

                cPopupPort.SetHeaderText('Modify Port Details');
                chfID.Set("hfID", hfid);
                cPopupPort.Show();
            }

            if (grid.cpinsert != null) {
                if (grid.cpinsert == 'Success') {
                    jAlert('Saved successfully');
                    cPopupPort.Hide();
                    grid.Refresh();
                }
                else {
                    jAlert("Error On Insertion\n'Please Try Again!!'");
                }
            }

            if (grid.cpExists != null) {
                if (grid.cpExists == 'Exists') {
                    jAlert('Duplicate value');
                    cPopupPort.Hide();
                }

            }

            if (grid.cpUpdate != null) {
                if (grid.cpUpdate == 'Success') {
                    jAlert('Updated successfully');
                    grid.cpUpdate = null;
                    cPopupPort.Hide();
                    grid.Refresh();
                }
                else {
                    jAlert("Error on Updation\n'Please Try again!!'")
                    grid.cpUpdate = null;
                }
            }


            if (grid.cpDelete != null) {
                if (grid.cpDelete == 'Successfully Deleted') {
                    jAlert(grid.cpDelete);
                    grid.cpDelete = null;
                    grid.Refresh();
                    //grid.PerformCallback();
                }
                else {
                    jAlert(grid.cpDelete)
                    grid.cpDelete = null;
                    grid.PerformCallback();
                }
            }
        }





    </script>
</asp:Content>

