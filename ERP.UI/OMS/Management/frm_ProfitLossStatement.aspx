<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"
    Inherits="ERP.OMS.Management.management_frm_ProfitLossStatement" CodeBehind="frm_ProfitLossStatement.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>--%>
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_wofolder.js"></script>

    <style type="text/css">
        /* Big box with list of options */
        #ajax_listOfOptions {
            position: absolute; /* Never change this one */
            width: 50px; /* Width of box */
            height: auto; /* Height of box */
            overflow: auto; /* Scrolling features */
            border: 1px solid Blue; /* Blue border */
            background-color: #FFF; /* White background color */
            text-align: left;
            font-size: 0.9em;
            z-index: 32767;
        }

            #ajax_listOfOptions div { /* General rule for both .optionDiv and .optionDivSelected */
                margin: 1px;
                padding: 1px;
                cursor: pointer;
                font-size: 0.9em;
            }

            #ajax_listOfOptions .optionDiv { /* Div for each item in list */
            }

            #ajax_listOfOptions .optionDivSelected { /* Selected item in the list */
                background-color: #DDECFE;
                color: Blue;
            }

        #ajax_listOfOptions_iframe {
            background-color: #F00;
            position: absolute;
            z-index: 3000;
        }

        form {
            display: inline;
        }
    </style>

    <script language="javascript" type="text/javascript">
        function btnAddsubscriptionlist_click() {
       var userid = document.getElementById('txtsegselected');
       if (userid.value != '') {
           var ids = document.getElementById('txtsegselected_hidden');
           var listBox = document.getElementById('lstSegment');
           var tLength = listBox.length;
           var no = new Option();
           no.value = ids.value;
           no.text = userid.value;
           listBox[tLength] = no;
           var recipient = document.getElementById('txtsegselected');
           recipient.value = '';
       }
       else
           alert('Please search name and then Add!')
       var s = document.getElementById('txtsegselected');
       s.focus();
       s.select();
   }

               function clientselectionfinal() {
                   var listBoxSubs = document.getElementById('lstSegment');
                   var cmb = document.getElementById('cmbsearchOption');
                   var listIDs = '';
                   var i;
                   if (listBoxSubs.length > 0) {
                       for (i = 0; i < listBoxSubs.length; i++) {
                           if (listIDs == '')
                               listIDs = listBoxSubs.options[i].value + ';' + listBoxSubs.options[i].text;
                           else
                               listIDs += ',' + listBoxSubs.options[i].value + ';' + listBoxSubs.options[i].text;
                       }
                       var sendData = cmb.value + '~' + listIDs;
                       CallServer(sendData, "");
                   }
                   var i;
                   for (i = listBoxSubs.options.length - 1; i >= 0; i--) {
                       listBoxSubs.remove(i);
                   }
                   document.getElementById('TdFilter').style.visibility = 'visible';
                   document.getElementById('TdSelect').style.visibility = 'visible';
                   //	        document.getElementById('rdAll').checked='true';
               }

               function btnRemovefromsubscriptionlist_click() {

                   var listBox = document.getElementById('lstSegment');
                   var tLength = listBox.length;

                   var arrTbox = new Array();
                   var arrLookup = new Array();
                   var i;
                   var j = 0;
                   for (i = 0; i < listBox.options.length; i++) {
                       if (listBox.options[i].selected && listBox.options[i].value != "") {

                       }
                       else {
                           arrLookup[listBox.options[i].text] = listBox.options[i].value;
                           arrTbox[j] = listBox.options[i].text;
                           j++;
                       }
                   }
                   listBox.length = 0;
                   for (i = 0; i < j; i++) {
                       var no = new Option();
                       no.value = arrLookup[arrTbox[i]];
                       no.text = arrTbox[i];
                       listBox[i] = no;
                   }
               }
               function MainAll(obj1, obj2) {
                   document.getElementById('cmbsearchOption').value = obj1;
                   if (obj2 == 'all') {

                       document.getElementById('TdFilter').style.visibility = 'hidden';
                       document.getElementById('TdSelect').style.visibility = 'hidden';
                   }
                   else {
                       document.getElementById('TdFilter').style.visibility = 'visible';
                       document.getElementById('TdSelect').style.visibility = 'visible';
                   }
               }
               function MainAllBranch(obj) {
                   if (obj == 'all') {

                       document.getElementById('TdFilterBranch').style.visibility = 'hidden';
                       document.getElementById('TdSelectBranch').style.visibility = 'hidden';
                   }
                   else {
                       document.getElementById('TdFilterBranch').style.visibility = 'visible';
                       document.getElementById('TdSelectBranch').style.visibility = 'visible';
                   }
               }
               function height() {
                   if (document.body.scrollHeight >= 500)
                       window.frameElement.height = document.body.scrollHeight;
                   else
                       window.frameElement.height = '500px';
                   window.frameElement.Width = document.body.scrollWidth;
               }
               function ShowMainAccountName(obj1, obj2, obj3) {
                   var cmb = document.getElementById("cmbsearchOption");
                   var obj4 = cmb.value;
                   ajax_showOptions(obj1, obj2, obj3, obj4);

               }

               function ShowBranchName(obj1, obj2, obj3) {
                   ajax_showOptions(obj1, obj2, obj3, 'Branch');
               }
               FieldName = 'lstSegment';
               function SegSelected(obj) {
                   //    document.getElementById('showFilter').style.display='inline';
                   document.getElementById('cmbsearchOption').value = obj;
                   //    FocusFiter();
               }
               </script>

    <script type="text/ecmascript">

        function ReceiveServerData(rValue) {

         var Data = rValue.split('~');
         var NoItems = Data[1].split(';');
         var val = '';
         for (i = 0; i < NoItems.length / 2; i++) {
             if (val == '') {
                 val = '(' + NoItems[i];
             }
             else {
                 val += ',' + NoItems[i];
             }
         }
         val = val + ')';
         var combo = document.getElementById('litSegment');
         combo.innerText = val;
     }

               </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="For the Period:" CssClass="mylabel1"
                    Width="103px"></asp:Label>
            </td>
            <td colspan="3">
                <table>
                    <tr>
                        <td>
                            <dxe:ASPxDateEdit ID="AspxFromDate" runat="server" EditFormat="custom" UseMaskBehavior="True">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxDateEdit>
                        </td>
                        <td style="width: 10%" class="rt">
                            <asp:Label ID="Label2" runat="server" Text="To:" CssClass="mylabel1"></asp:Label>
                        </td>
                        <td>
                            <dxe:ASPxDateEdit ID="AspxTodate" runat="server" EditFormat="custom" UseMaskBehavior="True">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxDateEdit>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label3" runat="server" Text="Segment:" CssClass="mylabel1"></asp:Label></td>
            <td colspan="3">
                <table>
                    <tr>
                        <td>
                            <asp:RadioButton ID="rdAll" runat="server" GroupName="k" Height="1px" Width="1px"
                                Checked="true" /></td>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="All" CssClass="mylabel1"></asp:Label></td>
                        <td>
                            <asp:RadioButton ID="rdselected" runat="server" GroupName="k" /></td>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="Selected" CssClass="mylabel1"></asp:Label>
                        </td>
                        <td id="TdFilter" style="visibility: hidden">
                            <asp:TextBox ID="txtsegselected" runat="server" Width="200px"></asp:TextBox>
                            <asp:HiddenField ID="txtsegselected_hidden" runat="server" />
                            <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a>
                            <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="85px"
                                Enabled="false">
                                <asp:ListItem>Branch</asp:ListItem>
                                <asp:ListItem>Segment</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td id="TdSelect" style="visibility: hidden">
                            <table>
                                <tr>
                                    <td>
                                        <asp:ListBox ID="lstSegment" runat="server" Font-Size="12px" Height="90px" Width="253px"></asp:ListBox></td>
                                    <td>
                                        <a id="A2" href="javascript:void(0);" onclick="clientselectionfinal()"><span style="color: #000099; text-decoration: underline; font-size: 10pt;">Done</span></a>
                                    </td>
                                    <td>
                                        <a id="A1" href="javascript:void(0);" onclick="btnRemovefromsubscriptionlist_click()">
                                            <span style="color: #cc3300; text-decoration: underline; font-size: 8pt;">Remove</span></a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr visible="false">
            <td colspan="4">
                <span id="litSegment" runat="server" style="color: Maroon" visible="false"></span>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label6" runat="server" Text="Branch:" CssClass="mylabel1"></asp:Label>
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:RadioButton ID="rdAllBranch" runat="server" GroupName="R" Height="1px" Width="1px"
                                Checked="true" /></td>
                        <td>
                            <asp:Label ID="Label7" runat="server" Text="All" CssClass="mylabel1"></asp:Label></td>
                        <td>
                            <asp:RadioButton ID="rdSelectedBranch" runat="server" GroupName="R" /></td>
                        <td>
                            <asp:Label ID="Label8" runat="server" Text="Selected" CssClass="mylabel1"></asp:Label>
                        </td>
                        <td></td>
                    </tr>
                </table>
            </td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="btnUpdate" OnClick="btnShow_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <div id="MainContainer" runat="server" class="TableMain100" style="border: solid 1px blue; text-align: center"
                    visible="true">
                </div>
            </td>
        </tr>
    </table>
</asp:Content>


