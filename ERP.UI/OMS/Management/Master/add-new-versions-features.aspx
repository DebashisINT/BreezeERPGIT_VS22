<%@ Page Title="" Language="C#" MasterPageFile="" AutoEventWireup="true" CodeBehind="add-new-versions-features.aspx.cs" Inherits="ERP.OMS.Management.Master.add_new_versions_features" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">--%>
  <head>  
      
      <link href="/ckeditor/contents.css" rel="stylesheet" /> 
       <script type="text/javascript" src="https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.12.4.min.js"></script>
       <script src="/ckeditor/ckeditor.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var editor = CKEDITOR.instances['text_id'];
            if (editor) { editor.destroy(true); }
            CKEDITOR.replace('text_id');
            $("#spanverson").html($("#hiddnversion1").val());
            btnSave_Click("2");
        });
        function btnSave_Click(flag) {
            var cmb = CKEDITOR.instances['text_id'].getData();
            $.ajax({
                type: "POST",
                url: "/OMS/Management/Master/add-new-versions-features.aspx/addversionfeatures",
                data: JSON.stringify({ "VersionNumber": $.trim($("#hiddnversion1").val()), "FeaturesMarkup": cmb, "flag": flag }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                global: false,
                async: false,
                success: function (msg) {
                    if (msg.d) {
                        if (flag == "2") {
                            var editor = CKEDITOR.instances['text_id'];
                            if (editor) { editor.destroy(true); }
                            CKEDITOR.replace('text_id');
                            CKEDITOR.instances["text_id"].setData(msg.d);
                            return false;
                        }
                        else if (flag == "1") {
                            alert("added Successfully");
                            CKEDITOR.instances["text_id"].setData(msg.d);
                            return false;
                        }

                    }
                },
                error: function (response) {
                    console.log(response);
                }
            });
           
        }

    </script>

   
   


</head>

<body>
    <form runat="server">
<%--<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">--%>
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Update/View Features of Current Version</h3> 
            <asp:HiddenField ID="hiddnversion1" runat="server" />
        </div>
    </div>
    <div class="form_main" style="border: 1px solid #ccc; padding: 10px 15px;">

       
                    <table width="100%">
                        
                       
                        <tr id="TrInsert">
                            <td style="vertical-align: top; ">
                            
                                <div class="clear"></div>
                                
                                <div class="col-md-9" id="TrMessage">
                                    <label class="Ecoheadtxt">Version Number :</label><strong><span id="spanverson"></span></strong> <br />
                                     <label class="Ecoheadtxt">Body :</label>
                                    <div>
                                        <asp:TextBox ID="text_id" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                
                                <div class="clear"></div>
                                <br />
                                <div class="col-md-12" id="TrButton">

                                    <input id="btnSave" type="button" value="Save" class="btn btn-primary" onclick="btnSave_Click('1')"
                                        style="width: 64px" />
                                </div>
                            </td>
                        </tr>


                    </table>
               
    </div>
    </form>
</body>