<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.management_frm_attendance_employeeWise_iframe" CodeBehind="frm_attendance_employeeWise_iframe.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/assests/js/init.js"></script>
    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>
    <script type="text/javascript" src="/assests/js/jquery-1.3.1.js"></script>
    <script type="text/javascript" src="/assests/js/jquery.timeentry.js"></script>
    <script type="text/javascript">
        $(function () {
            var i = 0;
            //alert('aaa');
            for (i = 0; i < Noofrows; i++) {
                if (i < 8) {
                    var no = i + 2;
                    //ID1='#ctl00_ContentPlaceHolder3_grdUserAttendace_ctl0'+no+'_txtINtime';
                    ID1 = '#grdUserAttendace_ctl0' + no + '_txtINtime';
                    functionBind(ID1);
                    ID2 = '#grdUserAttendace_ctl0' + no + '_txtOUTtime';
                    functionBind(ID2);
                }
                else {
                    var no = i + 2;
                    ID1 = '#grdUserAttendace_ctl' + no + '_txtINtime';
                    functionBind(ID1);
                    ID2 = '#grdUserAttendace_ctl' + no + '_txtOUTtime';
                    functionBind(ID2);
                }

            }
            //	    $('#ctl00_ContentPlaceHolder3_grdUserAttendace_ctl04_txtINtime').timeEntry();
            //	    $('#ctl00_ContentPlaceHolder3_grdUserAttendace_ctl04_txtOUTtime').timeEntry();

        });
        function functionBind(obj) {
            $(obj).timeEntry();
        }
    </script>

    <script type="text/javascript" language="javascript">
        function PageLoad(obj) {
            document.getElementById('txtName_hidden').style.display = "none";
            Noofrows = obj;
            height();
        }
        function height() {
            if (document.body.scrollHeight > 300)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = 300;
            window.frameElement.widht = document.body.scrollWidht;
            parent.height();
        }
        function NoOfRows(obj) {
            //alert(obj);
            Noofrows = obj;

            document.getElementById('txtName_hidden').style.display = "none";
        }

        function show(obj1) {
            //alert(obj1);
            document.getElementById(obj1).style.display = 'inline';
        }
        function hide(obj1) {
            //alert(obj1);
            document.getElementById(obj1).style.display = 'none';
        }
        FieldName = 'btnShow'
        function aftersave(obj1, obj2) {
            ShowEmployeeFilterForm(obj1);
        }
        function CallAjax(obj1, obj2, obj3) {
            var cmbcompany = document.getElementById('cmbCompany_I');
            //alert(cmbcompany.value);
            var cmbbranch = document.getElementById('cmbBranch_I');
            var obj4 = cmbcompany.value + '~' + cmbbranch.value
            //alert(obj4);
            ajax_showOptions(obj1, obj2, obj3, obj4);
        }
    </script>

    <script type="text/javascript" language="javascript">
        var ajaxBox_offsetX = 0;
        var ajaxBox_offsetY = 0;
        //var ajax_list_externalFile = '../management/ajax_list.aspx'; // Path to external file //Comment On 30.07.2015
        var ajax_list_externalFile = '../ajax_list.aspx'; // Path to external file
        var minimumLettersBeforeLookup = 1;	// Number of letters entered before a lookup is performed.

        var ajax_list_objects = new Array();
        var ajax_list_cachedLists = new Array();
        var ajax_list_activeInput = false;
        var ajax_list_activeItem;
        var ajax_list_optionDivFirstItem = false;
        var ajax_list_currentLetters = new Array();
        var ajax_optionDiv = false;
        var ajax_optionDiv_iframe = false;

        var ajax_list_MSIE = false;
        if (navigator.userAgent.indexOf('MSIE') >= 0 && navigator.userAgent.indexOf('Opera') < 0) ajax_list_MSIE = true;

        var currentListIndex = 0;


        function ajax_getTopPos(inputObj) {

            var returnValue = inputObj.offsetTop;
            while ((inputObj = inputObj.offsetParent) != null) {
                returnValue += inputObj.offsetTop;
            }
            return returnValue;
        }
        function ajax_list_cancelEvent() {
            return false;
        }

        function ajax_getLeftPos(inputObj) {
            var returnValue = inputObj.offsetLeft;
            while ((inputObj = inputObj.offsetParent) != null) returnValue += inputObj.offsetLeft;

            return returnValue;
        }

        function ajax_option_setValue(e, inputObj) {
            //______This if to hide cdropdown list if FieldName is assigned__//
            if (document.getElementById(FieldName)) document.getElementById(FieldName).style.display = '';

            if (!inputObj) inputObj = this;
            var tmpValue = inputObj.innerHTML;
            if (ajax_list_MSIE) tmpValue = inputObj.innerText; else tmpValue = inputObj.textContent;
            if (!tmpValue) tmpValue = inputObj.innerHTML;
            ajax_list_activeInput.value = tmpValue;
            //document.getElementById(search_param).value=inputObj.id;

            if (document.getElementById(ajax_list_activeInput.name + '_hidden')) document.getElementById(ajax_list_activeInput.name + '_hidden').value = inputObj.id;

            ajax_options_hide();
        }

        function ajax_options_hide() {

            if (document.getElementById(FieldName)) document.getElementById(FieldName).style.display = '';

            if (ajax_optionDiv) ajax_optionDiv.style.display = 'none';
            if (ajax_optionDiv_iframe) ajax_optionDiv_iframe.style.display = 'none';
        }

        function ajax_options_rollOverActiveItem(item, fromKeyBoard) {
            if (ajax_list_activeItem) ajax_list_activeItem.className = 'optionDiv';
            item.className = 'optionDivSelected';
            ajax_list_activeItem = item;

            if (fromKeyBoard) {
                if (ajax_list_activeItem.offsetTop > ajax_optionDiv.offsetHeight) {
                    ajax_optionDiv.scrollTop = ajax_list_activeItem.offsetTop - ajax_optionDiv.offsetHeight + ajax_list_activeItem.offsetHeight + 2;
                }
                if (ajax_list_activeItem.offsetTop < ajax_optionDiv.scrollTop) {
                    ajax_optionDiv.scrollTop = 0;
                }
            }
        }

        function ajax_option_list_buildList(letters, paramToExternalFile) {

            ajax_optionDiv.innerHTML = '';
            ajax_list_activeItem = false;
            if (ajax_list_cachedLists[paramToExternalFile][letters.toLowerCase()].length <= 1) {
                ajax_options_hide();
                return;
            }



            ajax_list_optionDivFirstItem = false;
            var optionsAdded = false;
            for (var no = 0; no < ajax_list_cachedLists[paramToExternalFile][letters.toLowerCase()].length; no++) {
                if (ajax_list_cachedLists[paramToExternalFile][letters.toLowerCase()][no].length == 0) continue;
                optionsAdded = true;
                var div = document.createElement('DIV');
                var items = ajax_list_cachedLists[paramToExternalFile][letters.toLowerCase()][no].split(/###/gi);
                if (ajax_list_cachedLists[paramToExternalFile][letters.toLowerCase()].length == 1 && ajax_list_activeInput.value == items[0]) {
                    ajax_options_hide();
                    return;
                }


                div.innerHTML = items[items.length - 1];
                div.id = items[0];
                //alert(div.innerHTML);
                //alert(items[0]);

                div.className = 'optionDiv';
                div.onmouseover = function () { ajax_options_rollOverActiveItem(this, false) }
                div.onclick = ajax_option_setValue;
                if (!ajax_list_optionDivFirstItem) ajax_list_optionDivFirstItem = div;
                ajax_optionDiv.appendChild(div);
            }
            if (optionsAdded) {
                ajax_optionDiv.style.display = 'block';
                if (ajax_optionDiv_iframe) ajax_optionDiv_iframe.style.display = '';
                ajax_options_rollOverActiveItem(ajax_list_optionDivFirstItem, true);
            }

            if (document.getElementById(FieldName)) document.getElementById(FieldName).style.display = 'none';

        }

        function ajax_option_list_showContent(ajaxIndex, inputObj, paramToExternalFile, whichIndex) {
            if (whichIndex != currentListIndex) return;
            var letters = inputObj.value;
            var content = ajax_list_objects[ajaxIndex].response;
            var elements = content.split('|');
            ajax_list_cachedLists[paramToExternalFile][letters.toLowerCase()] = elements;
            ajax_option_list_buildList(letters, paramToExternalFile);

        }

        function ajax_option_resize(inputObj) {
            ajax_optionDiv.style.top = (ajax_getTopPos(inputObj) + inputObj.offsetHeight + ajaxBox_offsetY) + 'px';
            ajax_optionDiv.style.left = (ajax_getLeftPos(inputObj) + ajaxBox_offsetX) + 'px';
            if (ajax_optionDiv_iframe) {
                ajax_optionDiv_iframe.style.left = ajax_optionDiv.style.left;
                ajax_optionDiv_iframe.style.top = ajax_optionDiv.style.top;
            }

        }

        function ajax_showOptions(inputObj, paramToExternalFile, e, obj_search, searchStart) {

            var search_par
            search_par = obj_search; //document.getElementsByName(obj_search).value;


            if (e.keyCode == 13 || e.keyCode == 9) return;
            if (ajax_list_currentLetters[inputObj.name] == inputObj.value) return;
            if (!ajax_list_cachedLists[paramToExternalFile]) ajax_list_cachedLists[paramToExternalFile] = new Array();
            ajax_list_currentLetters[inputObj.name] = inputObj.value;
            if (!ajax_optionDiv) {
                ajax_optionDiv = document.createElement('DIV');
                ajax_optionDiv.id = 'ajax_listOfOptions';
                document.body.appendChild(ajax_optionDiv);
                //alert('currentListIndex');
                if (ajax_list_MSIE) {
                    ajax_optionDiv_iframe = document.createElement('IFRAME');
                    ajax_optionDiv_iframe.border = '0';
                    ajax_optionDiv_iframe.style.width = ajax_optionDiv.clientWidth + 'px';
                    //ajax_optionDiv_iframe.style.width = '500px';
                    ajax_optionDiv_iframe.style.height = ajax_optionDiv.clientHeight + 'px';
                    ajax_optionDiv_iframe.id = 'ajax_listOfOptions_iframe';
                    document.body.appendChild(ajax_optionDiv_iframe);
                }

                var allInputs = document.getElementsByTagName('INPUT');
                for (var no = 0; no < allInputs.length; no++) {
                    if (!allInputs[no].onkeyup) allInputs[no].onfocus = ajax_options_hide;
                }
                var allSelects = document.getElementsByTagName('SELECT');
                for (var no = 0; no < allSelects.length; no++) {
                    allSelects[no].onfocus = ajax_options_hide;
                }

                var oldonkeydown = document.body.onkeydown;
                if (typeof oldonkeydown != 'function') {
                    document.body.onkeydown = ajax_option_keyNavigation;
                } else {
                    document.body.onkeydown = function () {
                        oldonkeydown();
                        ajax_option_keyNavigation();
                    }
                }
                var oldonresize = document.body.onresize;
                if (typeof oldonresize != 'function') {
                    document.body.onresize = function () { ajax_option_resize(inputObj); };
                } else {
                    document.body.onresize = function () {
                        oldonresize();
                        ajax_option_resize(inputObj);
                    }
                }

            }

            if (inputObj.value.length < minimumLettersBeforeLookup) {
                ajax_options_hide();
                //alert('currentListIndex');
                return;
            }


            ajax_optionDiv.style.top = (ajax_getTopPos(inputObj) + inputObj.offsetHeight + ajaxBox_offsetY) + 'px';
            ajax_optionDiv.style.left = (ajax_getLeftPos(inputObj) + ajaxBox_offsetX) + 'px';
            ajax_optionDiv.style.width = '50%';
            ajax_optionDiv.style.backgroundcolor = "transparent";
            if (ajax_optionDiv_iframe) {
                ajax_optionDiv_iframe.style.left = ajax_optionDiv.style.left;
                ajax_optionDiv_iframe.style.top = ajax_optionDiv.style.top;
            }

            ajax_list_activeInput = inputObj;
            ajax_optionDiv.onselectstart = ajax_list_cancelEvent;
            currentListIndex++;

            var tmpIndex = currentListIndex / 1;
            ajax_optionDiv.innerHTML = '';
            var ajaxIndex = ajax_list_objects.length;
            ajax_list_objects[ajaxIndex] = new sack();
            //alert('url1');
            //var abc =new Date 		
            //ajax_list_externalFile=ajax_list_externalFile+'&abc='+abc

            var url = ajax_list_externalFile + '?' + paramToExternalFile + '=1&search_param=' + search_par + '&letters=' + inputObj.value.replace(" ", "+");
            ajax_list_objects[ajaxIndex].requestFile = url;	// Specifying which file to get
            ajax_list_objects[ajaxIndex].onCompletion = function () { ajax_option_list_showContent(ajaxIndex, inputObj, paramToExternalFile, tmpIndex); };	// Specify function that will be executed after file has been found
            ajax_list_objects[ajaxIndex].runAJAX();		// Execute AJAX function		

        }

        function ajax_option_keyNavigation(e) {
            if (document.all) e = event;

            if (!ajax_optionDiv) return;
            if (ajax_optionDiv.style.display == 'none') return;

            if (e.keyCode == 38) {	// Up arrow
                if (!ajax_list_activeItem) return;
                if (ajax_list_activeItem && !ajax_list_activeItem.previousSibling) return;
                ajax_options_rollOverActiveItem(ajax_list_activeItem.previousSibling, true);
            }

            if (e.keyCode == 40) {	// Down arrow
                if (!ajax_list_activeItem) {
                    ajax_options_rollOverActiveItem(ajax_list_optionDivFirstItem, true);
                } else {
                    if (!ajax_list_activeItem.nextSibling) return;
                    ajax_options_rollOverActiveItem(ajax_list_activeItem.nextSibling, true);
                }
            }

            if (e.keyCode == 13 || e.keyCode == 9) {	// Enter key or tab key
                if (ajax_list_activeItem && ajax_list_activeItem.className == 'optionDivSelected') ajax_option_setValue(false, ajax_list_activeItem);
                if (e.keyCode == 13) return false; else return true;
            }
            if (e.keyCode == 27) {	// Escape key
                ajax_options_hide();
            }
        }


        document.documentElement.onclick = autoHideList;

        function autoHideList(e) {

            if (document.getElementById(FieldName)) document.getElementById(FieldName).style.display = '';

            if (document.all) e = event;

            if (e.target) source = e.target;
            else if (e.srcElement) source = e.srcElement;
            if (source.nodeType == 3) // defeat Safari bug
                source = source.parentNode;
            if (source.tagName.toLowerCase() != 'input' && source.tagName.toLowerCase() != 'textarea') ajax_options_hide();

        }
    </script>

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
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100">
            <tr>
                <td>
                    <dxe:ASPxComboBox ID="cmbCompany" runat="server" CssFilePath="~/App_Themes/Office2003 Blue/{0}/styles.css"
                        CssPostfix="Office2003_Blue" Font-Size="12px" ImageFolder="~/App_Themes/Office2003 Blue/{0}/"
                        ValueType="System.String" Font-Bold="False" EnableIncrementalFiltering="true"
                        ClientInstanceName="company">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                        <DropDownButton Text="Company">
                        </DropDownButton>
                    </dxe:ASPxComboBox>
                </td>
                <td class="gridcellleft" style="width: 50px">
                    <dxe:ASPxComboBox ID="cmbBranch" runat="server" CssFilePath="~/App_Themes/Office2003 Blue/{0}/styles.css"
                        CssPostfix="Office2003_Blue" ImageFolder="~/App_Themes/Office2003 Blue/{0}/"
                        ValueType="System.String" Width="130px" Font-Size="12px" EnableIncrementalFiltering="True"
                        EnableTheming="False" Height="10px" Font-Overline="False" ClientInstanceName="branch">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                        <DropDownButton Text="Branch" Width="40px">
                        </DropDownButton>
                    </dxe:ASPxComboBox>
                </td>
                <td style="width: 81px; text-align: left">
                    <dxe:ASPxDateEdit ID="cmbFromdate" runat="server" CssFilePath="~/App_Themes/Office2003 Blue/{0}/styles.css"
                        CssPostfix="Office2003_Blue" DateOnError="Today" EditFormat="Custom" EditFormatString="dd MMMM yyyy"
                        Font-Size="12px" ImageFolder="~/App_Themes/Office2003 Blue/{0}/" Width="131px"
                        UseMaskBehavior="True" NullText="From Date">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                        <DropDownButton ImagePosition="Top" Text="From" Width="30px">
                        </DropDownButton>
                    </dxe:ASPxDateEdit>
                </td>
                <td style="width: 81px; text-align: left">
                    <dxe:ASPxDateEdit ID="cmbDate" runat="server" CssFilePath="~/App_Themes/Office2003 Blue/{0}/styles.css"
                        CssPostfix="Office2003_Blue" DateOnError="Today" EditFormat="Custom" EditFormatString="dd MMMM yyyy"
                        Font-Size="12px" ImageFolder="~/App_Themes/Office2003 Blue/{0}/" Width="131px"
                        UseMaskBehavior="True" NullText="To Date">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                        <DropDownButton ImagePosition="Top" Text="To" Width="30px">
                        </DropDownButton>
                    </dxe:ASPxDateEdit>
                </td>
                <td style="width: 27px; text-align: right; padding-left: 5px" id="tdname">
                    <span class="Ecoheadtxt" style="color: Blue"><strong>Name:</strong></span>
                </td>
                <td style="padding-right: 5px; text-align: left;" id="tdtxtname">
                    <asp:TextBox ID="txtName" runat="server" Width="252px" Font-Size="11px"></asp:TextBox><asp:TextBox
                        ID="txtName_hidden" runat="server" Width="14px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                        Display="Dynamic" ErrorMessage="Please select name from list!" Font-Bold="True"></asp:RequiredFieldValidator>
                </td>
                <td class="gridcellright" style="width: 94px">
                    <%--<dxe:ASPxButton ID="btnShow" runat="server" Text="Show" CssFilePath="~/App_Themes/Office2003 Blue/{0}/styles.css"
                        CssPostfix="Office2003_Blue" OnClick="btnShow_Click">
                    </dxe:ASPxButton>--%>
                    <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" CssClass="btnUpdate" />
                </td>
            </tr>
            <tr>
                <td colspan="9" style="text-align: left">
                    <strong>Status</strong>: <strong style="color: Maroon">P</strong> – present ;<strong
                        style="color: Maroon">OV</strong> – Official Visit ;<strong style="color: maroon">OD</strong>-Official
                    Delay;<strong style="color: maroon">PD</strong>-Personal Delay;<strong style="color: maroon">A</strong>
                    – Absent or Leave without Pay; <strong>PL</strong>– Privilege Leave; <strong style="color: Maroon">CL</strong> – Casual Leave; <strong style="color: Maroon">SL</strong> – Sick
                    Leave; <strong style="color: Maroon">HC </strong>– Half day(Casual);<strong style="color: Maroon">HS
                    </strong>– Half day(Sick); <strong style="color: Maroon">WO</strong> – weekly Off;
                    <strong style="color: Maroon">PH</strong>– Paid holiday; <strong style="color: Maroon">CO</strong>– Compensatory off.
                </td>
            </tr>
            <tr>
                <td colspan="9">
                    <asp:GridView ID="grdUserAttendace" runat="Server" AutoGenerateColumns="False" BorderColor="CornflowerBlue"
                        BackColor="#DDECFE" BorderStyle="Solid" BorderWidth="2px" CellPadding="4" OnRowDataBound="grdUserAttendace_RowDataBound"
                        ForeColor="#0000C0" PageSize="200" OnSorting="grdUserAttendace_Sorting" AllowSorting="True">
                        <RowStyle BackColor="#DDECFE" ForeColor="#330099" BorderColor="#E6E8F3" BorderStyle="Double"
                            BorderWidth="1px"></RowStyle>
                        <SelectedRowStyle BackColor="#E6E8F3" ForeColor="SlateBlue" Font-Bold="True"></SelectedRowStyle>
                        <PagerStyle BackColor="#E6E8F3" ForeColor="SlateBlue" HorizontalAlign="Center"></PagerStyle>
                        <HeaderStyle BackColor="LightSteelBlue" ForeColor="Black" CssClass="EHEADER" Font-Bold="True"></HeaderStyle>
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblId" runat="server" Text='<%# Eval("datetime")%>' Visible="false"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="1px" />
                                <HeaderStyle Width="1px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="date" HeaderText="Date" SortExpression="datetime">
                                <ItemStyle HorizontalAlign="Left" Width="150px" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Status">
                                <ItemStyle HorizontalAlign="Center" Width="40px" />
                                <ItemTemplate>
                                    <asp:DropDownList ID="cmbStatus" runat="server" AppendDataBoundItems="True" Width="60px"
                                        Font-Size="12px">
                                        <asp:ListItem Value="P">P</asp:ListItem>
                                        <asp:ListItem Value="OV">OV</asp:ListItem>
                                        <asp:ListItem Value="OD">OD</asp:ListItem>
                                        <asp:ListItem Value="PD">PD</asp:ListItem>
                                        <asp:ListItem Value="A">A</asp:ListItem>
                                        <asp:ListItem Value="PL">PL</asp:ListItem>
                                        <asp:ListItem Value="CL">CL</asp:ListItem>
                                        <asp:ListItem Value="SL">SL</asp:ListItem>
                                        <asp:ListItem Value="HC">HC</asp:ListItem>
                                        <asp:ListItem Value="HS">HS</asp:ListItem>
                                        <asp:ListItem Value="WO">WO</asp:ListItem>
                                        <asp:ListItem Value="PH">PH</asp:ListItem>
                                        <asp:ListItem Value="CO">CO</asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="In Time">
                                <ItemStyle HorizontalAlign="Center" Width="70px" />
                                <ItemTemplate>
                                    <asp:TextBox ID="txtINtime" runat="server" Width="60px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Out Time">
                                <ItemStyle HorizontalAlign="Center" Width="70px" />
                                <ItemTemplate>
                                    <asp:TextBox ID="txtOUTtime" runat="server" Width="60px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="9">
                    <dxe:ASPxButton ID="btnSave" runat="server" Text="Mark Attendance" CssFilePath="~/App_Themes/Office2003 Blue/{0}/styles.css"
                        CssPostfix="Office2003_Blue" OnClick="btnSave_Click">
                    </dxe:ASPxButton>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
