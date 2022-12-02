<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="true" Inherits="ERP.OMS.Management.management_cashbankPopup1" CodeBehind="cashbankPopup1.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register assembly="DevExpress.Web.v15.1" namespace="DevExpress.Web" tagprefix="dx" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />
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
            z-index: 100;
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
            z-index: 5;
        }

        form {
            display: inline;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function CallList(obj1,obj2,obj3)
   {
       ajax_showOptions(obj1,obj2,obj3);    
   }        
               function TypeSelect1()
               {
                   var obj=document.getElementById("cmbType").value;
                   if(obj=="C")
                   {
                       document.getElementById("tdmain").style.display="none";
                       document.getElementById("tdmain1").style.display="inline";
                       document.getElementById("tddate").style.display="none";
                       document.getElementById("tddate1").style.display="inline";
                       document.getElementById("tdpayee1").style.display="inline";
                       document.getElementById("tdpayee").style.display="none";
                       document.getElementById("tdcb").style.display = 'none';
                       document.getElementById("tdcb1").style.display = 'none';
                   }
                   else    
                   {
                       document.getElementById("tdmain").style.display="inline";
                       document.getElementById("tdmain1").style.display="none";
                       document.getElementById("tddate").style.display="inline";
                       document.getElementById("tddate1").style.display="none";
                       document.getElementById("tdpayee1").style.display="none";
                       document.getElementById("tdpayee").style.display="inline";
                       document.getElementById("tdcb").style.display = 'inline';
                       document.getElementById("tdcb1").style.display = 'inline';
                   }
               }
               function CallListBank(obj1,obj2,obj3)
               {
                   ajax_showOptions(obj1,obj2,obj3);    
               }
               function OnInstmentTypeChange()
               {
                   var objInstType=document.getElementById("cmbInstType").value;
                   if(objInstType=="D")
                   {
                       document.getElementById("trIssuingBank").style.display = 'inline';
                       document.getElementById("trCustomerBank").style.display = 'none';
                   }
                   else                        
                   {
                       document.getElementById("trCustomerBank").style.display = 'inline';
                       document.getElementById("trIssuingBank").style.display = 'none';
                       CmbClientBankCI.PerformCallback(objSubAc);  
                   }
               } 
               function OnCashBankReportSubAcChange()
               {
                   objSubAc=document.getElementById("cmbSubAc").value;
               }
               function checkTextAreaMaxLength(textBox,e, length)
               {
                
                   var mLen = textBox["MaxLength"];
                   if(null==mLen)
                       mLen=length;
                    
                   var maxLength = parseInt(mLen);
                   if(!checkSpecialKeys(e))
                   {
                       if(textBox.value.length > maxLength-1)
                       {
                           if(window.event)//IE
                               e.returnValue = false;
                           else//Firefox
                               e.preventDefault();
                       }
                   }   
               }
               function checkSpecialKeys(e)
               {
                   if(e.keyCode !=8 && e.keyCode!=46 && e.keyCode!=37 && e.keyCode!=38 && e.keyCode!=39 && e.keyCode!=40)
                       return false;
                   else
                       return true;
               } 
               function AllowNumericOnly(e)
               {
                   var keycode;
                   if (window.event) keycode = window.event.keyCode;
                   else if (event) keycode = event.keyCode;
                   else if (e) keycode = e.which;
                   else return true;
                   if( (keycode > 47 && keycode <= 57) || (keycode == 46) )
                   {
                       return true;
                   }
                   else
                   {
                       return false;
                   }
                   return true;

               }
               function testDetails()
               {
                   var txt = document.getElementById("txtPayment");
                   if(txt.value.indexOf(".") > 0)
                   {
                       var str = txt.value.substring(txt.value.indexOf(".") + 1);
                       if(str.length > 4)
                       {
                           alert("only four decimals are allowed");
                       }
                   }
                   else
                   {
                       if(txt.value.length <= 11)
                       {
                           return true;
                       }
                       else
                       {
                           alert("11 digits only allowed");
                           txt.value = txt.value.substring(0,11);
                       }
                   }
               } 
               function testDetails1()
               {
                   var txt = document.getElementById("txtReceipt");
                   if(txt.value.indexOf(".") > 0)
                   {
                       var str = txt.value.substring(txt.value.indexOf(".") + 1);
                       if(str.length > 4)
                       {
                           alert("only four decimals are allowed");
                       }
                   }
                   else
                   {
                       if(txt.value.length <= 11)
                       {
                           return true;
                       }
                       else
                       {
                           alert("11 digits only allowed");
                           txt.value = txt.value.substring(0,11);
                       }
                   }
               }
               function Clear()
               {
                   document.getElementById("cmbSubAc").value="Select";
                   document.getElementById("cmbInstType").value="D";
                   document.getElementById("cmbPayee").value="0";
                   document.getElementById("txtInstNo").value="";
                   document.getElementById("txtPayment").value="";
                   document.getElementById("txtReceipt").value="";
                   document.getElementById("txtLineNarration").value="";
                   document.getElementById("txtIssuingBank").value="";
                   document.getElementById("txtIssuingBank_hidden").value="";
               }
               FieldName = 'txtNarration';
               </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div>
        <table style="width: 100%; border-right: #3300ff thin solid; border-top: #3300ff thin solid; border-bottom-width: thin; border-bottom-color: #3300ff; border-left: #3300ff thin solid; border-bottom: #3300ff thin solid; background-color: #ddecfe;">
            <tr>
                <td>
                    <table style="width: 100%; background-color: #ddecfe; border-top-width: thin; border-left-width: thin; border-left-color: #3300ff; border-top-color: #3300ff;"
                        border="0" id="main"
                        cellspacing="0">
                        <tr id="">
                            <td style="text-align: left;">Type :</td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="cmbType" runat="server" Width="100px" Font-Size="12px" AutoPostBack="True" OnSelectedIndexChanged="cmbType_SelectedIndexChanged1">

                                    <asp:ListItem Value="P">Payment[P]</asp:ListItem>
                                    <asp:ListItem Value="R">Receipt[R]</asp:ListItem>
                                    <asp:ListItem Value="C">Contra[C]</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: left;">Date :
                            </td>
                            <td style="text-align: left;">
                                <dxe:ASPxDateEdit ID="dteDate" runat="server" EditFormat="Custom"
                                    EditFormatString="dd MMMM yyyy" UseMaskBehavior="True" Font-Size="12px" Width="100px" CssFilePath="~/App_Themes/Office2003 Blue/{0}/styles.css" CssPostfix="Office2003_Blue" ImageFolder="~/App_Themes/Office2003 Blue/{0}/">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </td>
                            <td style="text-align: left;">Company :</td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="cmbCompany" runat="server" DataSourceID="dsCompany" DataTextField="CashBank_CompanyName" DataValueField="CashBank_CompanyID" Width="200px" Font-Size="12px" AutoPostBack="True" OnSelectedIndexChanged="cmbCompany_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: left;">Segment :
                            </td>
                            <td style="text-align: left;">
                                <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="cmbSegment" runat="server" Width="100px" Font-Size="12px">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbCompany" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr id="trBankCashType">
                            <td style="text-align: left;" id="tdBankCashType">Vou. No :
                            </td>
                            <td style="text-align: left;" id="tdBankCashType1">
                                <asp:TextBox ID="txtVoucherNo" runat="server" Font-Size="12px" ReadOnly="true" Width="97px"></asp:TextBox>
                            </td>
                            <td style="text-align: left;" id="tdBankAccountNo">Branch :
                            </td>
                            <td style="text-align: left;" id="tdBankAccountNo1">
                                <asp:DropDownList ID="cmbBranch" runat="server" DataSourceID="dsBranch" DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100px" Font-Size="12px">
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: left;" id="tdcb">Cash/Bank A/c :
                            </td>
                            <td style="text-align: left;" id="tdcb1">
                                <asp:DropDownList ID="cmbCashBankAc" runat="server" DataSourceID="MainAccount" DataTextField="MainAccount_Name" DataValueField="CashBank_MainAccountID" Width="200px" Font-Size="12px">
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: left;" id="td3">Settelment No :
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtSettlementNo" runat="server" Width="100px" Font-Size="12px"></asp:TextBox>
                                <asp:HiddenField ID="txtSettlementNo_hidden" runat="server" />
                            </td>
                        </tr>
                        <tr id="tr6" style="background-color: #ddecfe;" align="center">
                            <td style="text-align: left;">Narration :</td>
                            <td colspan="3" style="text-align: left">
                                <asp:TextBox ID="txtNarration" MaxLength="500" TextMode="MultiLine" onkeyDown="checkTextAreaMaxLength(this,event,'500');" runat="server" Width="260px"></asp:TextBox>
                            </td>
                            <td style="text-align: left;">Curr. Bank Balan. :</td>
                            <td style="text-align: left;">
                                <asp:Label ID="ASPxLabel4" runat="server" Font-Size="12px" Width="100px"></asp:Label>
                            </td>
                            <td style="text-align: left;" id="td13">Curr. A/C Balan.:
                            </td>
                            <td style="text-align: left;" id="td14">
                                <asp:Label ID="ASPxLabel5" runat="server" Font-Size="12px" Width="100px"></asp:Label>
                            </td>
                        </tr>

                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 20px"></td>
            </tr>
            <tr style="background-color: #a4c6f8">
                <td>
                    <table style="width: 100%; border-top-width: thin; border-left-width: thin;" border="0" id="Table1"
                        cellspacing="0">
                        <tr>
                            <td style="text-align: left;" id="tdmain">Main A/C
                            </td>
                            <td style="text-align: left; display: none" id="tdmain1">Cash/Bank A/C
                            </td>
                            <td style="text-align: left;">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="cmbMainAc" runat="server" AutoPostBack="True" Width="200px" Font-Size="12px" OnSelectedIndexChanged="cmbMainAc_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbType" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                            <td style="text-align: left;">Sub A/C
                            </td>
                            <td style="text-align: left;">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="cmbSubAc" runat="server" Width="200px" Font-Size="12px">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbMainAc" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                            <td style="text-align: left;">Inst Type
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="cmbInstType" runat="server" Width="200px" Font-Size="12px">
                                    <asp:ListItem Value="D">Draft[D]</asp:ListItem>
                                    <asp:ListItem Value="C">Cheque[C]</asp:ListItem>
                                    <asp:ListItem Value="E">Electronic Transfer[E]</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td style="text-align: left;" id="tddate">Date
                            </td>
                            <td style="text-align: left; display: none" id="tddate1">Withdrawl
                            </td>
                            <td style="text-align: left;">
                                <dxe:ASPxDateEdit ID="dtDateWith" runat="server" EditFormat="Custom"
                                    EditFormatString="dd MMMM yyyy" UseMaskBehavior="True" Font-Size="12px" Width="200px" CssFilePath="~/App_Themes/Office2003 Blue/{0}/styles.css" CssPostfix="Office2003_Blue" ImageFolder="~/App_Themes/Office2003 Blue/{0}/">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>

                            </td>
                            <td style="text-align: left;" id="tdpayee">Payee
                            </td>
                            <td style="text-align: left; display: none;" id="tdpayee1">Deposit
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="cmbPayee" runat="server" Width="200px" Font-Size="12px">
                                    <asp:ListItem Value="0">Amit Kumar</asp:ListItem>
                                    <asp:ListItem Value="1">Mr. Asit</asp:ListItem>
                                    <asp:ListItem Value="2">Mr. Sitangshu</asp:ListItem>
                                    <asp:ListItem Value="3">Mr. Binay</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: left;">Instrument No
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtInstNo" runat="server" Width="197px" Font-Size="12px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">Payment
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtPayment" runat="server" Width="197px" Font-Size="12px" onkeyup="testDetails();" onkeypress="return AllowNumericOnly(this);"></asp:TextBox>
                            </td>
                            <td style="text-align: left;">Receipt
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtReceipt" runat="server" Width="197px" Font-Size="12px" onkeyup="testDetails1();" onkeypress="return AllowNumericOnly(this);"></asp:TextBox>
                            </td>
                            <td style="text-align: left;">Line Narration:
                            </td>
                            <td style="text-align: left;" colspan="3">
                                <asp:TextBox ID="txtLineNarration" MaxLength="500" Width="197px" TextMode="MultiLine" onkeyDown="checkTextAreaMaxLength(this,event,'500');" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trIssuingBank">
                            <td style="text-align: left;">Issuing Bank </td>
                            <td style="text-align: left;" colspan="7">
                                <asp:TextBox ID="txtIssuingBank" runat="server" Width="340px">
                                </asp:TextBox>
                                <asp:HiddenField ID="txtIssuingBank_hidden" runat="server" />
                            </td>
                        </tr>
                        <tr id="trCustomerBank" style="display: none">
                            <td style="text-align: left;" id="tdCustomerBank">Customer Bank 
                            </td>
                            <td style="text-align: left;" id="tdCustomerBank1" colspan="3">
                                <dxe:ASPxComboBox ID="CmbClientBank" runat="server" Width="250px" Height="20" DropDownWidth="550"
                                    DropDownStyle="DropDownList" DataSourceID="dsgrdClientbank" ValueField="cbd_id"
                                    CssFilePath="~/App_Themes/Office2003 Blue/{0}/styles.css" ValueType="System.String"
                                    TextFormatString="{0} -- {2}" EnableIncrementalFiltering="True" CallbackPageSize="30"
                                    ClientInstanceName="CmbClientBankCI" OnCallback="CmbClientBank_OnCallback" CssPostfix="Office2003_Blue">

                                    <Columns>
                                        <dxe:ListBoxColumn FieldName="bnk_bankName" Caption="Bank Name" Width="150px" ToolTip="Bank Name" />
                                        <dxe:ListBoxColumn FieldName="cbd_accountName" Caption="Account Holder Name" Width="200px"
                                            ToolTip="Account Holder Name" />
                                        <dxe:ListBoxColumn FieldName="cbd_accountNumber" Caption="Account Number" Width="120px"
                                            ToolTip="Account Number" />
                                        <dxe:ListBoxColumn FieldName="bnk_micrno" Caption="MICR Number" Width="80px" ToolTip="MICR Number" />
                                        <dxe:ListBoxColumn FieldName="cbd_Accountcategory" Caption="Account Type" Width="80px"
                                            ToolTip="MICR Number" />
                                    </Columns>
                                </dxe:ASPxComboBox>
                            </td>
                            <td style="text-align: left;">Auth. Letter Ref
                            </td>
                            <td style="text-align: left; padding-top: 10px;" colspan="2">
                                <asp:TextBox ID="txtAuthLetterRef" runat="server" Width="170px">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8" style="text-align: right;">
                                <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btnUpdate" Height="22px" Width="73px" OnClick="btnAdd_Click" />
                                <%--<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnUpdate" Height="22px" Width="73px"/>--%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="grdAdd" runat="server" DataKeyNames="CashReportID" CssClass="gridcellleft" CellPadding="4" ForeColor="#333333" GridLines="None" BorderWidth="1px" BorderColor="#507CD1" Width="100%" AutoGenerateColumns="False" OnRowEditing="grdAdd_RowEditing" OnRowDeleting="grdAdd_RowDeleting">
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
                                            <EditRowStyle BackColor="#2461BF" />
                                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                            <HeaderStyle Font-Bold="False" ForeColor="Black" CssClass="EHEADER" BorderColor="AliceBlue" BorderWidth="1px" />
                                            <AlternatingRowStyle BackColor="White" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="MainAccount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMainAccount" runat="server" Text='<%# Eval("MainAccount1") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="SubAccount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSubAccount" runat="server" Text='<%# Eval("SubAccount1") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Inst. Type">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInstType" runat="server" Text='<%# Eval("InstType1") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Inst No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInstNo" runat="server" Text='<%# Eval("CashBank_InstrumentNumber") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("CashBank_InstrumentDate1") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Payee">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPayee" runat="server" Text='<%# Eval("Payee1") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Payment">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblWithdraw" runat="server" Text='<%# Eval("CashBank_AmountWithdrawl") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Receipt">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblReceipt" runat="server" Text='<%# Eval("CashBank_AmountDeposit") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("CashReportID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete" ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete" OnClientClick="javascript:return confirm('Do You Want To Delete This Record ?')" Text="Delete">
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:CommandField HeaderText="Edit" ShowEditButton="True" />
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click"></asp:AsyncPostBackTrigger>
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <asp:SqlDataSource ID="dsCashBank" runat="server" ConflictDetection="CompareAllValues"
        SelectCommand=""
        InsertCommand="insert into table1 (temp123) values('11')" UpdateCommand="update table1 set temp123='123'"></asp:SqlDataSource>
    <asp:SqlDataSource ID="dsCompany" runat="server" 
        SelectCommand="SELECT COMP.CMP_INTERNALID AS CashBank_CompanyID , COMP.CMP_NAME AS CashBank_CompanyName  FROM TBL_MASTER_COMPANY AS COMP"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SelectSegment" runat="server" 
        SelectCommand="SELECT LTRIM(RTRIM(A.EXCH_INTERNALID)) AS CashBank_ExchangeSegmentID ,TME.EXH_ShortName + '--' + A.EXCH_SEGMENTID AS EXCHANGENAME FROM (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID=@COMPANYID) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID">
        <SelectParameters>
            <asp:Parameter Name="COMPANYID" Type="string" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="dsBranch" runat="server"
        SelectCommand="SELECT BRANCH_id AS BANKBRANCH_ID , BRANCH_DESCRIPTION AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH"></asp:SqlDataSource>
    <asp:SqlDataSource ID="MainAccount" runat="server" 
        SelectCommand="Select MainAccount_ReferenceID as CashBank_MainAccountID, MainAccount_Name as MainAccount_Name from Master_MainAccount where MainAccount_BankCashType='Bank' or MainAccount_BankCashType='Cash'"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SelectSubaccount" runat="server" 
        SelectCommand="Select SubAccount_ReferenceID as SubAccountID, SubAccount_Name as SubAccountName from Master_SubAccount where SubAccount_MainAcReferenceID=@ID">
        <SelectParameters>
            <asp:Parameter Name="ID" Type="string" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="dsgrdClientbank" runat="server" ConflictDetection="CompareAllValues"
        InsertCommand="insert into table1 (temp123) values('11')"
        SelectCommand="select A.* , MB.bnk_id,MB.bnk_bankName,MB.bnk_BranchName,MB.bnk_micrno from (Select TCBD.cbd_id,TCBD.cbd_cntId,TCBD.cbd_bankCode, TCBD.cbd_Accountcategory,TCBD.cbd_Accountcategory as AccountType,TCBD.cbd_accountNumber,TCBD.cbd_accountType,cbd_accountName from tbl_trans_contactBankDetails as  TCBD where TCBD.cbd_cntId=@SubAccountCode) as A inner  join tbl_master_Bank as MB on MB.bnk_id=a.cbd_id">
        <SelectParameters>
            <asp:Parameter Name="SubAccountCode" Type="string" />
        </SelectParameters>
    </asp:SqlDataSource>

</asp:Content>
