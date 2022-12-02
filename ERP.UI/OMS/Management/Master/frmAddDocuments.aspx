<%@ Page Language="C#" AutoEventWireup="True"
    Inherits="ERP.OMS.Management.Master.management_Master_frmAddDocuments" MasterPageFile="~/OMS/MasterPage/PopUp.Master" CodeBehind="frmAddDocuments.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--    <style>
        .TableMain100 {
            background: silver;
        }

        .dxbButton {
            color: #000000;
            font-weight: normal;
            font-size: 9pt;
            font-family: Tahoma;
            vertical-align: middle;
            border: solid 1px #7F7F7F;
            background: #E0DFDF url('WebResource.axd?d=_spZ71SnQgyimh4Ik74-9HFcObMTcdvPomzoDw07BJ7yszl8Y6ulZ8IM…MPHZ5kNiogkXYXSknkreo_-rWyjQdokHUVEUqZw7VR1WskJUAwD0&t=635755229276311331') top;
            background-repeat: repeat-x;
            padding: 1px 1px 1px 1px;
            cursor: pointer;
        }

        .dxpcHeader {
            background-color: #ccc !important;
        }
        }
    </style>--%>


    <%--style="margin: 0px 0px 0px 0px; background-color: #DDECFE" onload="clearPreloadPage()">--%>
    <style>
        /*#LinkButton1 {
            position: absolute;
            right: 15px;
            top: 9px;
        }*/
        .relative {
            position:relative;
        }
        .pullrightClass {
             position: absolute;
    right: 47px;
    width: 15px;
    height: 15px;
    top: 9px;
    color: #DF3636;
    font-size: 15px;
        }
        .r59 {
            right:59px;
        }
        .mbot3 {
            margin-bottom:0px !important;
        }
        .mRight10 {
            margin-right:10px;
        }
         #MandatoryFileName, #MandatoryFileNo, #MandatoryFloor, #MandatoryFileSize,
        #MandatoryRoom, #MandatoryCell, #MandatoryBuilding,#MandatoryDocumentType{
            position: absolute;
            right: 46px;
            top: 9px;
        }
    </style>
    <script type="text/javascript">
      
        function OnCloseButtonClick(s, e) {
           
            var parentWindow = window.parent;
            parentWindow.popup.Hide();

        }

        $(document).ready(function () {
            if ($("#hdnIsServiceManagemnt").val() == "1") {
                $("#spnFileName").addClass('hide');
                $("#spnBuilding").addClass('hide');
                $("#spnNumber").addClass('hide');
            }
            else {
                $("#spnFileName").removeClass('hide');
                $("#spnBuilding").removeClass('hide');
                $("#spnNumber").removeClass('hide');
            }
        });


        function Validation() {
           
            //MandatoryBuilding
            var Building = $('#Building :selected').val();

            var DType = document.getElementById('DTYpe');
            var FName = document.getElementById('TxtName');
            var fileNo = document.getElementById('TxtfileNo');
            var cellNo = document.getElementById('TxtCellNo');
            var RoomNo = document.getElementById('TxtRoomNo');
            var FloorNo = document.getElementById('TxtFloorNo');
            
            if (DType.value == '') {
                $('#MandatoryDocumentType').attr('style', 'display:block');
                return false;
            }
            else { $('#MandatoryDocumentType').attr('style', 'display:none'); }
            //Rev Tanmoy not mandatory for service Management
            if ($("#hdnIsServiceManagemnt").val()!="1") {
                if (fileNo.value == '') {
                    $('#MandatoryFileNo').attr('style', 'display:block');
                    return false;
                }
                else {
                    $('#MandatoryFileNo').attr('style', 'display:none');
                }

                if (FName.value == '') {
                    $('#MandatoryFileName').attr('style', 'display:block');
                    return false;
                }
                else { $('#MandatoryFileName').attr('style', 'display:none'); }

                if (Building == "") {
                    $('#MandatoryBuilding').attr('style', 'display:block');
                    return false;
                }
                else { $('#MandatoryBuilding').attr('style', 'display:none'); }
            }
            
            if (FName.value != '' && fileNo != '') {
                var Upload_Image = document.getElementById('FileUpload1');
              
                if ($('#<%=hidFilename.ClientID %>').val() != "")
                { 
          
                
                
                }
                else{
                    if (Upload_Image == null || Upload_Image.value == '') {
                        if (FloorNo.value == '') {
                            $('#MandatoryFloor').attr('style', 'display:block');
                            return false;
                        }
                        else {
                            $('#MandatoryFloor').attr('style', 'display:none');

                        }
                        if (cellNo.value == '') {
                            $('#MandatoryCell').attr('style', 'display:block');
                            return false;
                        }
                        else {
                            $('#MandatoryCell').attr('style', 'display:none');
                        }
                        if (RoomNo.value == '') {
                            $('#MandatoryRoom').attr('style', 'display:block');
                            return false;
                        }
                        else { $('#MandatoryRoom').attr('style', 'display:none'); }
                    }
                    else {


                        var maxFileSize = '<%=fileSize %>'; // 2MB

                        if ($("#FileUpload1")[0].files[0].size < maxFileSize) {
                            $('#MandatoryFileSize').attr('style', 'display:none');
                        } else {
                            $('#MandatoryFileSize').attr('style', 'display:block');
                            $("#FileUpload1").val('');
                            return false;
                        }


                        var files = $('#FileUpload1')[0].files;
                        var len = $('#FileUpload1').get(0).files.length;

                        for (var i = 0; i < len; i++) {

                            f = files[i];

                            var ext = f.name.split('.').pop().toLowerCase();
                            if ($.inArray(ext, ['exe']) == 1) {
                                $('#MandatoryFileType').attr('style', 'display:block');
                                return false;
                            }
                            else { $('#MandatoryFileType').attr('style', 'display:none'); }
                        }
                    }
                }
            }
          

            
        }
        function ShowFileUpload() {
            //document.getElementById("txtFilepath").style.display = "none";

        }
        function CheckLengthNote1() {
            //var textbox = document.getElementById('txt_note1').value;
            //if (textbox.trim().length >=200) {
            //    return false;
            //}
            //else {
            //    return true;
            //}
        }
        function CheckLengthNote2() {
            ////var textbox = document.getElementById('txt_note2').value;
            ////if (textbox.trim().length >= 200) {
            ////    return false;
            ////}
            ////else {
            ////    return true;
            ////}
        }


        function OnDocumentView(obj1, obj2) { 
            var docid = obj1;
            var filename;
            var chk = obj2.includes("~");
            if (chk) {
                filename = obj2.split('~')[1];
            }
            else {
                filename = obj2.split('/')[2];
            }
            if (filename != '' && filename != null) {
                var d = new Date();
                var n = d.getFullYear();
                var url = '\\OMS\\Management\\Documents\\' + docid + '\\' + n + '\\' + filename;
                //window.open(url, '_blank');
                var seturl = '\\OMS\\Management\\DailyTask\\viewImage.aspx?id=' + url;
                popup.contentUrl = url;
                popup.Show();
            }
            else {
                alert('File not found.')
            }

        }
        
        function GetFileSize() {
            var maxFileSize = '<%=fileSize %>'; // 2MB
         
            if ($("#FileUpload1")[0].files[0].size < maxFileSize) {
                $('#MandatoryFileSize').attr('style', 'display:none');
            } else {
                $('#MandatoryFileSize').attr('style', 'display:block');
                $("#FileUpload1").val('');
                return false;
            }


            var files = $('#FileUpload1')[0].files;
            var len = $('#FileUpload1').get(0).files.length;

            for (var i = 0; i < len; i++) {

                f = files[i];

                var ext = f.name.split('.').pop().toLowerCase();
                if ($.inArray(ext, ['exe']) == 1) {
                    $('#MandatoryFileType').attr('style', 'display:block');
                    return false;
                }
                else
                { $('#MandatoryFileType').attr('style', 'display:none'); }
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="background-color: rgb(237,243,244);">
        <%-- <asp:ScriptManager ID="ScriptManager1" runat="server">
                        </asp:ScriptManager>
                        <asp:UpdatePanel ID="UpdatePanel154" runat="server">
                            <ContentTemplate>--%>
        <table style="background-color: rgb(237,243,244); width: 100%">
            <tr>
                <td width="120px">Document Type<span style="color: red;">*</span>
                </td>
                <td style="position:relative;">
                    <asp:DropDownList ID="DTYpe" runat="server" Width="250px" TabIndex="1">
                    </asp:DropDownList>
                     <span id="MandatoryDocumentType" style="display:none"><img id="gridHistory21_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                </td>
                <td>Number<span style="color: red;" id="spnNumber"> *</span>
                </td>
                <td class="relative">
                    <asp:TextBox ID="TxtfileNo" runat="server" MaxLength="20" Width="250px" TabIndex="2"> </asp:TextBox>

<span id="MandatoryFileNo" style="display:none"><img id="gridHistory1_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"</span>
                   <%-- <asp:RequiredFieldValidator Display="Dynamic" runat="server" ID="rfvComname" ControlToValidate="TxtfileNo" ToolTip="Mandatory"
                        SetFocusOnError="true" ErrorMessage="" ValidationGroup="doc" ForeColor="Red" CssClass="pullrightClass fa fa-exclamation-circle ">                                                        
                    </asp:RequiredFieldValidator>--%>
                </td>
            </tr>
            <tr>
                <td>File Name <span style="color: red;" id="spnFileName">*</span>
                </td>
                <td class="relative">
                    <asp:TextBox ID="TxtName" runat="server" MaxLength="50" Width="250px" TabIndex="3"></asp:TextBox>
                    <span id="MandatoryFileName" style="display:none"><img id="gridHistory2_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                  
                     <%-- <asp:RequiredFieldValidator Display="Dynamic" runat="server" ID="RequiredFieldValidator1" ControlToValidate="TxtName" ToolTip="Mandatory"
                        SetFocusOnError="true" ErrorMessage="" ValidationGroup="doc" ForeColor="Red" CssClass="pullrightClass fa fa-exclamation-circle r59">                                                        
                    </asp:RequiredFieldValidator>--%>
                </td>
                <td>Received Date:
                </td>
                <td class="relative">
                    <dxe:ASPxDateEdit ID="dtReceived" ClientInstanceName="dtReceived" runat="server"
                        EditFormat="Custom" UseMaskBehavior="True" Width="250px" TabIndex="5">
                        <DropDownButton>
                        </DropDownButton>
                    </dxe:ASPxDateEdit>
                </td>
                <%--<td>Cell/Cabinet No.<span style="color:red;"> *</span>
                </td>
                <td>
                    <asp:TextBox ID="TxtCellNo" MaxLength="20" runat="server" Width="250px" TabIndex="4"></asp:TextBox>
                </td>--%>
            </tr>
            <tr>
                <td>Document
                </td>
                <td style="position: relative">
                    <asp:FileUpload ID="FileUpload1" runat="server" Width="250px" TabIndex="9" onchange="GetFileSize()" />
 <span id="MandatoryFileSize" style="display:none"><img id="gridHistory22_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="File Size More Than <%=fileSizeinMB %> MB"></span>
                     <span id="MandatoryFileType" style="display:none"><img id="gridHistory42_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Invalid File Type"></span>
                    <div id="divfile" runat="server" visible="false" class="pull-left mRight10"> 
                        <a  onclick="OnDocumentView('<%=filedoc %>','<%=filesrc %>')" style="text-decoration: none; cursor: pointer;" title="View" class="pad btn btn-default btn-xs">
                                                           View file
                                                        </a>
                    </div>


                  <%--  <asp:TextBox ID="txtFilepath" runat="server" Visible="false" Width="250px" TabIndex="6" CssClass="mbot3"></asp:TextBox>--%>
                    
                   

                    <asp:LinkButton ID="LinkButton1" Text="Change File" runat="server" OnClick="LinkButton1_Click"
                        TabIndex="7" CssClass="pull-left btn btn-default btn-xs"></asp:LinkButton>
                    <div style="clear:both"></div>
                    <span style="color:blue; ">Accepts file size upto <%=fileSizeinMB %> MB.</span>
                    <asp:HiddenField ID="hidFilename" runat="server" />
                </td>
                <%--<td>Received Date:
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="dtReceived" ClientInstanceName="dtReceived" runat="server"
                        EditFormat="Custom" UseMaskBehavior="True" Width="250px" TabIndex="5">
                        <DropDownButton>
                        </DropDownButton>
                    </dxe:ASPxDateEdit>
                </td>--%>
                <td>Renewal Date:
                </td>
                <td class="relative">
                    <dxe:ASPxDateEdit ID="dtRenew" ClientInstanceName="dtRenew" runat="server" EditFormat="Custom"
                        UseMaskBehavior="True" Width="250px" TabIndex="6">
                        <DropDownButton>
                        </DropDownButton>
                    </dxe:ASPxDateEdit>
                </td>
            </tr>
            <tr>
                <td>Building<span style="color: red;" id="spnBuilding">*</span>
                </td>
                <td class="relative">
                    <asp:DropDownList ID="Building" runat="server" Width="250px" TabIndex="7">
                    </asp:DropDownList>
                     <span id="MandatoryBuilding" style="display:none"><img id="gridHistory13_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                </td>
                <td>Floor No. 
                </td>
                <td class="relative">
                    <asp:TextBox ID="TxtFloorNo" runat="server" MaxLength="3" Width="250px" TabIndex="8"></asp:TextBox>
                    <span id="MandatoryFloor" style="display:none"><img id="gridHistory3_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                </td>
            </tr>
            <tr>
                <td>Cell/Cabinet No.
                </td>
                <td class="relative">
                    <asp:TextBox ID="TxtCellNo" MaxLength="50" runat="server" Width="250px" TabIndex="4"></asp:TextBox>
                    <span id="MandatoryCell" style="display:none"><img id="gridHistory5_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                </td>
                <%--<td>Select File
                </td>
                <td>
                    <asp:FileUpload ID="FileUpload1" runat="server" Width="250px" TabIndex="9" />
                    <asp:TextBox ID="txtFilepath" runat="server" Visible="false" Width="250px" TabIndex="6"></asp:TextBox>
                    <asp:LinkButton ID="LinkButton1" Text="Change File" runat="server" OnClick="LinkButton1_Click"
                        TabIndex="7"></asp:LinkButton>
                </td>--%>
                <td>Room No.
                </td>
                <td class="relative">
                    <asp:TextBox ID="TxtRoomNo" runat="server" MaxLength="20" Width="250px" TabIndex="10"  ></asp:TextBox>
                    <span id="MandatoryRoom" style="display:none"><img id="gridHistory6_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                </td>
            </tr>

            <%-- </table>
        <table style="background-color: rgb(237,243,244);">--%>
            <tr>
                <td>Note 1 :
                </td>
                <td class="relative">
                    <asp:TextBox ID="txt_note1" runat="server" TextMode="MultiLine"  Width="250px"
                        Height="117px" TabIndex="11" ></asp:TextBox>
                </td>
                <%-- <td style="width: 68px;"></td>--%>
                <td>Note 2 :
                </td>
                <td class="relative">
                    <asp:TextBox ID="txt_note2" runat="server" TextMode="MultiLine"  Width="250px"
                        Height="117px" TabIndex="12"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table style="background-color: rgb(237,243,244);">

            <tr>
                <td></td>
                <td colspan="2" style="text-align: left; padding-left: 115px">
                    <asp:Button ID="Button1" runat="server" Text="Save" TabIndex="13" OnClick="Button1_Click" OnClientClick="javascript: return Validation()"
                        CssClass="btn btn-primary"  />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" TabIndex="14" CssClass="btn btn-danger" /><asp:HiddenField ID="IsFileUpload" runat="server" />
                </td>
            </tr>
        </table>
        <table><tr>
            <td style="font-size:11px;  color:blue;">

              Note: If the 'File Name' has been entered, Document has been selected in 'Document' and value entered in 'Number', then Document can be saved without any other field values. If No Document selected in 'Document' then you must have to entered values for the fields: 'Floor No.', 'Room No. ', 'Cell/Cabinet No'.
            </td>
               </tr></table>
          <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" ContentUrl="frmAddDocuments.aspx"
                                            CloseAction="CloseButton" Top="120" Left="300" ClientInstanceName="popup" Height="400px"
                                            Width="850px" HeaderText="Document" AllowResize="true" ResizingMode="Postponed" Modal="true">
                                            <ContentCollection>
                                                <dxe:PopupControlContentControl runat="server">
                                                </dxe:PopupControlContentControl>
                                            </ContentCollection>
                                            <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
                                        </dxe:ASPxPopupControl>
        <asp:SqlDataSource ID="SlectBuilding" runat="server" 
            SelectCommand="select bui_Name,bui_id from tbl_master_building"></asp:SqlDataSource>

    </div>
    <%--</ContentTemplate>
                        </asp:UpdatePanel>--%>
    <asp:HiddenField ID="hdnIsServiceManagemnt" runat="server" />
</asp:Content>

