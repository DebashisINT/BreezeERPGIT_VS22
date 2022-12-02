<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="VersionUpload.aspx.cs" Inherits="ERP.OMS.Management.Master.VersionUpload" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
    .backBox {
        padding: 0 15px 15px;
        background: #f9f9f9;
        border: 1px solid #ccc;
    }
    
    .darkLabel {
        color: #191717;
    }

    
    .boxed {
        background: #f9f9f9;
        border: 1px solid #a3a4a5;
        padding: 20px;
        margin-bottom: 10px;
    }

    .rowBox {
        background: #f0f0e0;
        border: 1px solid #c3c3bd;
        padding: 6px 0;
        margin: 5px 0px;
    }
    .formBox {
        max-width: 500px;
        background: #eaeffb;
        padding: 22px;
        border-radius: 12px;
        box-shadow: 0px -4px 0px rgb(251, 151, 49), 0px 4px 0px #FF5722, 0px 4px 5px rgba(60, 62, 62, 0.22);
        position: relative;
        margin: 0 auto;
        z-index: 55;
    }

    .frmBox-Header {
        font-size: 17px;
        background: #1788bb;
        position: absolute;
        top: -37px;
        padding: 5px 18px;
        border-radius: 8px 18px 0 0;
        color: #fff;
    }

    .pdLeft5 {
        padding-left: 5px;
    }

    .uLabel {
        background: #ff8100;
        padding: 2px 3px;
        border-radius: 4px;
        color: #fff;
    }
    .crossBtn.stkCross {
        top: -40px;
        width: 30px;
        height: 30px;
        line-height: 28px !important;
    }
    .btn-radius {
        border-radius: 15px;
        padding: 5px 15px !important;
    }
    .mtop80 {
        margin-top: 13px;
    }
    .pageOverlay {
        position: fixed;
        width: 100%;
        top: 0;
        left: 0;
        bottom: 0;
        background: rgba(0,0,0,0.74);
        z-index: 44;
    }
    .crossBtn.pageTypepop {
        font-size: 14px !important;
        height: 25px;
        line-height: 25px !important;
        top: 14px;
        width: 25px;
    }
    .stLbl + div {
        text-align: center;
        padding: 0 10px;
    }
    .attachIcon {
        width: 35px;
        height: 35px;
        margin-top: 5px;
        display: block;
        background: #fbfbfb;
        font-size: 20px;
        border-radius: 50%;
        line-height: 36px;
        text-align: center;
        box-shadow: 1px 4px 5px rgb(39 37 37 / 15%);
    }
    .attachIcon:hover {
        background: #5a8dff;
        color: #fff;
        cursor:pointer
    }
    .tblWit {
        width:100%;
        border-collapse:separate; 
        border-spacing:0 5px;
    }
    .tblWit>tbody>tr>td {
        background:#fff;
        padding:8px;
    }
    .tblWit>tbody>tr{
        -webkit-transition:all 0.2s ease-in;
        transition:all 0.2s ease-in;
    }
    .tblWit>tbody>tr:hover{
        -webkit-transform:translateY(-2px);
        transform:translateY(-2px);
    }
    .tblWit>tbody>tr>td {
        border-style: solid none;
        border-color:#fff;
        padding: 10px;
    }
    .tblWit>tbody>tr>td:first-child {
        border-left-style: solid;
        border-top-left-radius: 10px; 
        border-bottom-left-radius: 10px;
    }
    .tblWit>tbody>tr>td:last-child {
        border-right-style: solid;
        border-bottom-right-radius: 10px; 
        border-top-right-radius: 10px; 
    }
</style>
    <script>

        $(document).ready(function () {

            $("#dtVersionDate").datepicker();
            $("#fileInput").change(function () {
                var allowedTypes = ['application/zip', 'application/ZIP', 'application/x-zip-compressed'];
                var file = this.files[0];
                var fileType = file.type;
                if (!allowedTypes.includes(fileType)) {
                    alert('Please select a valid zip file.');
                    $("#fileInput").val('');
                    return false;
                }
            });
        });



        function backToList() {
            window.location.href = "OMS/management/ProjectMainPage.aspx";
        }

        function SubmitForm()
        {
            var data = $('form').serialize();
            if ($("#txtName").val() == "") {
                jAlert('Version name is mandatory.', 'Alert');
                return false;
            }

            if ($("#txtWhatsNew").val() == "") {
                jAlert("Version What's new is mandatory.", 'Alert');
                return false;
            }

            if ($("#dtVersionDate").val() == "") {
                jAlert("Version date is mandatory.", 'Alert');
                return false;
            }
            if ($("#fileInput").val() == "") {
                jAlert("Version file is mandatory.", 'Alert');
                return false;
            }


            if (window.FormData !== undefined) {  
  
                var fileUpload = $("#fileInput").get(0);  
                var files = fileUpload.files;  
              
                // Create FormData object  
                var fileData = new FormData();  
  
                // Looping over all files and add it to FormData object  
                for (var i = 0; i < files.length; i++) {  
                    fileData.append(files[i].name, files[i]);  
                }  
              
                // Adding one more key to FormData object  

                fileData.append('name', $("#txtName").val());
                fileData.append('whatsnew', $("#txtWhatsNew").val());
                fileData.append('versiondate', $("#dtVersionDate").val());



            
                $.ajax({
                    xhr: function() {
                        var xhr = new window.XMLHttpRequest();
                        xhr.upload.addEventListener("progress", function(evt) {
                            if (evt.lengthComputable) {
                                var percentComplete = ((evt.loaded / evt.total) * 100);
                                $(".progress-bar").width(percentComplete + '%');
                                $(".progress-bar").html(percentComplete+'%');
                            }
                        }, false);
                        return xhr;
                    },
                    type: 'POST',
                    url: 'http://3.7.30.86:84/API/Upload/FileUpload',
                    data: fileData,
                    contentType: 'multipart/form-data',
                    cache: false,
                    processData:false,
                    beforeSend: function(){
                        $(".progress-bar").width('0%');
                        $('#uploadStatus').html('<img src="../../../assests/images/loading.gif"/>');
                    },
                    error:function(){
                        $('#uploadStatus').html('<p style="color:#EA4335;">File upload failed, please try again.</p>');
                    },
                    success: function(resp){
                        if(resp == 'ok'){
                            $('#uploadForm')[0].reset();
                            $('#uploadStatus').html('<p style="color:#28A74B;">File has uploaded successfully!</p>');
                        }else if(resp == 'err'){
                            $('#uploadStatus').html('<p style="color:#EA4335;">Please select a valid file to upload.</p>');
                        }
                    }
                });
            }
       
	
        // File type validation
        


        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pageOverlay"></div>


<div class="panel-heading">
    <div class="panel-title clearfix">
        <h3 class="pull-left"></h3>
    </div>
</div>
<div class="">
    <div class="clearfix formBox mtop80">
        <div id="ApprovalCross"  class="crossBtn pageTypepop" ><a href="javascript:void(null)" onclick="backToList()"><i class="fa fa-times"></i></a></div>
        <label class="pagePopLabl">Version Upload</label>
        <div class="row">
            <div class="col-md-12">
                <div class="progress" style="height:6px">
                  <div class="progress-bar progress-bar-success progress-bar-striped" role="progressbar" aria-valuenow="40" aria-valuemin="0" aria-valuemax="100" style="width: 40%">
                    <span class="sr-only"></span>
                  </div>
                </div>
            </div>
            <div id="uploadStatus"></div>
            <div class="col-md-12">
                <label>Version name</label>
                <div>
                   <input type="text" id="txtName" class="form-control" />
                </div>
            </div>
            <div class="col-md-12">
                <label>Whats new</label>
                <div>
                    <textarea class="form-control" id="txtWhatsNew" rows="5"></textarea>
                </div>
            </div>
            <div class="col-md-12">
                <label>Version Date</label>
                <div>
                   <input type="text" id="dtVersionDate" class="form-control" />
                </div>
            </div>
        </div>
        <div class="row" style="padding:5px 0">
            <div class="col-md-4">
                <span class="attachIcon"><i class="fa fa-paperclip"></i></span>
                <asp:FileUpload runat="server" AllowMultiple="false" ID="upUserCtrl" />
            </div>
            <div class="col-md-8 text-right">
                <asp:Button class="btn btn-success" Text="Submit" runat="server" style="margin-top: 12px;" OnClick="Unnamed_Click"></asp:Button>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <h5>Previous Versions</h5>
                <div>
                    <table class="tblWit">
                        <tr>
                            <td>Version Name</td>
                            <td>Size</td>
                            <td>Last Updated</td>
                            <td>Version Date</td>
                        </tr>
                        <tr>
                            <td>Version Name</td>
                            <td>Size</td>
                            <td>Last Updated</td>
                            <td>Version Date</td>
                        </tr>
                    </table>
                </div>
            </div>     
        </div>
    </div>
</div>
</asp:Content>
