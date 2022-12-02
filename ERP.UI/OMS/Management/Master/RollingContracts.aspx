<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Master.management_master_RollingContracts" Codebehind="RollingContracts.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <style type="text/css">
	
	/* Big box with list of options */
	#ajax_listOfOptions{
		position:absolute;	/* Never change this one */
		width:50px;	/* Width of box */
		height:auto;	/* Height of box */
		overflow:auto;	/* Scrolling features */
		border:1px solid Blue;	/* Blue border */
		background-color:#FFF;	/* White background color */
		text-align:left;
		font-size:0.9em;
		z-index:32761;
	}
	#ajax_listOfOptions div{	/* General rule for both .optionDiv and .optionDivSelected */
		margin:1px;		
		padding:1px;
		cursor:pointer;
		font-size:0.9em;
	}
	#ajax_listOfOptions .optionDiv{	/* Div for each item in list */
		
	}
	#ajax_listOfOptions .optionDivSelected{ /* Selected item in the list */
		background-color:#DDECFE;
		color:Blue;
	}
	#ajax_listOfOptions_iframe{
		background-color:#F00;
		position:absolute;
		z-index:3000;
	}
	
	form{
		display:inline;
	}
    #txtIssuingBank
    {
        z-index:10000;
        
    }
    
    .bubblewrap{
    list-style-type:none;
    margin:0;
    padding:0;
    }

    .bubblewrap li{
    display:inline;
    width: 65px;
    height:60px;
    }

    .bubblewrap li img{
    width: 30px; /* width of each image.*/
    height: 35px; /* height of each image.*/
    border:0;
    margin-right: 12px; /*spacing between each image*/
    -webkit-transition:-webkit-transform 0.1s ease-in; /*animate transform property */
    -o-transition:-o-transform 0.1s ease-in; /*animate transform property in Opera */
    }

    .bubblewrap li img:hover{
    -moz-transform:scale(1.8); /*scale up image 1.8x*/
    -webkit-transform:scale(1.8);
    -o-transform:scale(1.8);
    
	</style>
	
    <script type="text/javascript" src="/assests/js/init.js"></script>
    <script type="text/javascript" src="/assests/js/ajaxList.js"></script>
    <script type="text/javascript" src="/assests/js/jquery-1.3.2.js"></script>
    
    

     <script language="javascript" type="text/javascript">
    //Global Variable
        FieldName = 'abc';
     //
       function ShowHeight(obj)
       {
            
           height();
       }
         function height()
        {
                        
            if(document.body.scrollHeight>250)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '250px';
            window.frameElement.widht = document.body.scrollWidht;
            
        }
        
        
        function OnMoreInfoClick(keyValue) 
        {
            var url='frmProductComDerivativeDetails.aspx?id='+keyValue;
            OnMoreInfoClick(url,"Show Items",'930px','550px',"Y")
         
        }
        function OnMoreInfo(keyValue,keyvalue1) 
        {
            var url='frmProductComDerivativeRates.aspx?id='+keyValue+','+keyvalue1;
            OnMoreInfoClick(url,"Show Rates",'930px','470px',"Y")
         
        }
        
        function ShowHideFilter(obj)
        {
           grid.PerformCallback(obj);
        }
         function callback()
        {
            //alert('tt');
            grid.PerformCallback();
        }
        function CallProductList(obj1,obj2,obj3)
        {
           var strQuery_Table = "Master_Products";
           var strQuery_FieldName = "Top 10 Ltrim(Rtrim(Products_Name))+' ['+Ltrim(Rtrim(Products_ShortName))+'] ' as TextValue,Cast(Products_ID as Varchar)+'^'+Ltrim(Rtrim(Products_Name))+'^'+Ltrim(Rtrim(Products_ShortName)) as DataValue";
           var strQuery_WhereClause = "Products_ProductTypeID=10 and (Products_Name like 'RequestLetter%' or Products_ShortName like 'RequestLetter%')";
           var strQuery_OrderBy='';
           var strQuery_GroupBy='';
           var CombinedQuery=new String(strQuery_Table+"$"+strQuery_FieldName+"$"+strQuery_WhereClause+"$"+strQuery_OrderBy+"$"+strQuery_GroupBy);
           ajax_showOptions(obj1,obj2,obj3,replaceChars(CombinedQuery)); 
        }
        function replaceChars(entry) {
            out = "+"; // replace this
            add = "--"; // with this
            temp = "" + entry; // temporary holder

            while (temp.indexOf(out)>-1) {
            pos= temp.indexOf(out);
            temp = "" + (temp.substring(0, pos) + add + 
            temp.substring((pos + out.length), temp.length));
            }
            return temp;
            
        }
       function AddItem(e,obj)
       {
      
        AddOrEdit=obj.split('~')[0];
//            FieldName='ASPxPopupControl1_ASPxCallbackPanel1_drpBranch';
//            Filter='N';
            RowID='';
            if(AddOrEdit=='Edit')
              {
                var data=obj.split('~');
                if(data.length>1)
                    RowID=data[1];
                    
                 
                document.getElementById('PopUpAddSpreadAccount_CbpAddSpreadAccounts_txtProducts_hidden').value=data[2];
//                document.getElementById('PopUpAddSpreadAccount_CbpAddSpreadAccounts_txtLinkedAccount_hidden').value=data[3];  
                }
         cPopUpAddSpreadAccount.Show();
         cCbpAddSpreadAccounts.PerformCallback(obj);
       }
       function ddlAssetSubType_ValueChange(SelectedValue)
       {
        if(SelectedValue=="34")
        {
            document.getElementById("Tr_Option_Exercise").style.display="none";
        }
        else
        {
            document.getElementById("Tr_Option_Exercise").style.display="inline";
        }
       }
       function btnSave_Click()
       {
//        var obj=document.getElementById(
         if(AddOrEdit=='Add')
             {
            cCbpAddSpreadAccounts.PerformCallback('Save~');
            }
           else
            {
                cCbpAddSpreadAccounts.PerformCallback('SaveUpdate~'+RowID);
            
            } 
       }
        function CbpAddSpreadAccounts_EndCallBack()
        {
           if(cCbpAddSpreadAccounts.cpIsInsert!=null)
           {
                alert(cCbpAddSpreadAccounts.cpIsInsert);
                if(cCbpAddSpreadAccounts.cpIsInsert=='Successfully Saved' || cCbpAddSpreadAccounts.cpIsInsert=='Successfully Updated') 
                {
                    cPopUpAddSpreadAccount.Hide();
                    grid.PerformCallback('RefreshGridView');
                }
           }
        }
        
        function GetProducts(obj1,obj2,obj3)
          {
//            var segname= document.getElementById('hiddenSegmentName').value;
//            var compname=document.getElementById('hiddenCompany').value;
           var strQuery_Table = "Master_Products";
           var strQuery_FieldName = " top 10 rtrim(ltrim(isnull(Products_Name,''))) +\' [ \'+rtrim(ltrim(isnull(Products_ShortName,'')))+\' ]\'  as Productname,rtrim(ltrim(cast(Products_ID as varchar))) as ProductsID";
           var strQuery_WhereClause = " (Products_Name like (\'%RequestLetter%\') or Products_ShortName like (\'%RequestLetter%\')) and  Products_ProductTypeID=11 and Products_ProductSubTypeID=34" ;
           var strQuery_OrderBy='';
           var strQuery_GroupBy='';
           var CombinedQuery=new String(strQuery_Table+"$"+strQuery_FieldName+"$"+strQuery_WhereClause+"$"+strQuery_OrderBy+"$"+strQuery_GroupBy);
           
           ajax_showOptions(obj1,obj2,obj3,replaceChars(CombinedQuery),'Main'); 
          }
         function replaceChars(entry) {
            out = "+"; // replace this
            add = "--"; // with this
            temp = "" + entry; // temporary holder

            while (temp.indexOf(out)>-1) {
            pos= temp.indexOf(out);
            temp = "" + (temp.substring(0, pos) + add + 
            temp.substring((pos + out.length), temp.length));
            }
            return temp;
            
        }
        
         function SetValueforclient(val1,val2)
            {
               
                document.getElementById('hiddenSegmentName').value=val1;
                document.getElementById('hiddenCompany').value=val2;           
                
            
            }
        
    </script>

    <style type="text/css">
	
	/* Big box with list of options */
	#ajax_listOfOptions{
		position:absolute;	/* Never change this one */
		width:50px;	/* Width of box */
		height:auto;	/* Height of box */
		overflow:auto;	/* Scrolling features */
		border:1px solid Blue;	/* Blue border */
		background-color:#FFF;	/* White background color */
		text-align:left;
		font-size:0.9em;
		z-index:32767;
	}
	#ajax_listOfOptions div{	/* General rule for both .optionDiv and .optionDivSelected */
		margin:1px;		
		padding:1px;
		cursor:pointer;
		font-size:0.9em;
	}
	#ajax_listOfOptions .optionDiv{	/* Div for each item in list */
		
	}
	#ajax_listOfOptions .optionDivSelected{ /* Selected item in the list */
		background-color:#DDECFE;
		color:Blue;
	}
	#ajax_listOfOptions_iframe{
		background-color:#F00;
		position:absolute;
		z-index:3000;
	}
	
	form{
		display:inline;
	}
	
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
           <div>
           <table class="TableMain100" border="0" width="100%">
                    <tr>
                            <td class="EHEADER" align="center" style="width: 100%">
                                    <strong><span id="SpanHeader" style="color: #000099">Rolling Contracts</span></strong>
                            </td>
                                    
                                
                        </tr>
                    </table>
            <table class="TableMain100">
                <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <td style="text-align: left; vertical-align: top">
                                    <table>
                                        <tr>
                                            <td id="ShowFilter">
                                                <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">
                                                    Show Filter</span></a>
                                            </td>
                                            <td id="Td1">
                                                <a href="javascript:ShowHideFilter('All');"><span style="color: #000099; text-decoration: underline">
                                                    All Records</span></a>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                </td>
                                <td class="gridcellright" style="text-align: right">
                                    <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true" BackColor="Navy"
                                        Font-Bold="False" ForeColor="White" ValueType="System.Int32" Width="130px" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged">
                                        <%--OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"--%>
                                        <Items>
                                            <dxe:ListEditItem Text="Select" Value="0" />
                                            <dxe:ListEditItem Text="PDF" Value="1" />
                                            <dxe:ListEditItem Text="XLS" Value="2" />
                                            <dxe:ListEditItem Text="RTF" Value="3" />
                                            <dxe:ListEditItem Text="CSV" Value="4" />
                                        </Items>
                                        <ButtonStyle BackColor="#C0C0FF" ForeColor="Black">
                                        </ButtonStyle>
                                        <ItemStyle BackColor="Navy" ForeColor="White">
                                            <HoverStyle BackColor="#8080FF" ForeColor="White">
                                            </HoverStyle>
                                        </ItemStyle>
                                        <Border BorderColor="White" />
                                        <DropDownButton Text="Export">
                                        </DropDownButton>
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <dxe:ASPxGridView ID="gridRollingContracts" runat="server" ClientInstanceName="grid"
                            Width="100%" KeyFieldName="RollingContracts_id" AutoGenerateColumns="False"  DataSourceID="sqlRollingContracts" OnCustomCallback="gridRollingContracts_CustomCallback"
                            OnCustomJSProperties="gridRollingContracts_CustomJSProperties" OnHtmlDataCellPrepared="gridRollingContracts_HtmlDataCellPrepared">
                            <ClientSideEvents EndCallback="function(s, e) {
	
	ShowHeight(s.cpHeight)
}" />
                            <Styles>
                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                </Header>
                                <LoadingPanel ImageSpacing="10px">
                                </LoadingPanel>
                            </Styles>
                            
                            <Columns>
                                <dxe:GridViewDataTextColumn Caption="ExchangeName" FieldName="Exchange_name" VisibleIndex="0">
                                    <CellStyle CssClass="gridcellleft">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="ProductsName" FieldName="Products_Name"
                                    VisibleIndex="1">
                                    <CellStyle CssClass="gridcellleft">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="ProductsShortName" FieldName="Products_ShortName"
                                    VisibleIndex="2">
                                    <CellStyle CssClass="gridcellleft">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                                         
                               
                                <dxe:GridViewDataTextColumn VisibleIndex="3">
                                    <HeaderTemplate>
                                        <a href="javascript:void(0);" style="color: #800000;font-weight:bold" onclick="AddItem(this,'Add~')">Add Items</a>
                                    </HeaderTemplate>
                                    <DataItemTemplate>
                                        <a href="javascript:void(0);" style="color: #800000" onclick="AddItem(this,'Edit~'+'<%# Container.KeyValue %>' + '~' + '<%# DataBinder.Eval(Container.DataItem, "RollingContracts_ProductID") %>')">
                                           Edit Items </a>
                                    </DataItemTemplate>
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <StylesEditors>
                                <ProgressBar Height="25px">
                                </ProgressBar>
                            </StylesEditors>
                            <Settings ShowGroupPanel="True" ShowFooter="True" ShowStatusBar="Visible" ShowTitlePanel="True" />
                            <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                                <FirstPageButton Visible="True">
                                </FirstPageButton>
                                <LastPageButton Visible="True">
                                </LastPageButton>
                            </SettingsPager>
                            <SettingsBehavior AllowFocusedRow="True" />
                            <Styles FocusedRow-BackColor="lightgray"></Styles>
                        </dxe:ASPxGridView>
                        <asp:SqlDataSource ID="sqlRollingContracts" runat="server"
                            SelectCommand="Select RollingContracts_id,RollingContracts_ProductID,Exchange_name,Products_Name,Products_ShortName from Master_RollingContracts,Master_Products,Master_Exchange Where  (Products_ProductTypeID=11 and
 Products_ProductSubTypeID in (34)) and RollingContracts_productId =Products_id and Exchange_id=RollingContracts_exchangeid">
                            
                        </asp:SqlDataSource>
                    </td>
                    <td>
                       
                        <dxe:ASPxPopupControl ID="PopUpAddSpreadAccount" ClientInstanceName="cPopUpAddSpreadAccount"
                            runat="server" AllowDragging="True" PopupHorizontalAlign="OutsideRight" HeaderText="Add Rolling Contracts"
                            EnableHotTrack="False" BackColor="#DDECFE" Width="100%" DisappearAfter="500"
                            CloseAction="CloseButton">
                            <ContentCollection>
                                <dxe:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                                    <dxe:ASPxCallbackPanel ID="CbpAddSpreadAccounts" runat="server" Width="100%" ClientInstanceName="cCbpAddSpreadAccounts"
                                        OnCallback="CbpAddSpreadAccounts_Callback">
                                        <PanelCollection>
                                            <dxe:PanelContent runat="server">
                                                <table style="width: 669px" border="1">
                                                    <tr>
                                                        <td style="width: 126px; font-weight: bold; color: #000099;" class="gridcellleft">
                                                            Exchange :</td>
                                                        <td style="width: 239px" class="gridcellleft">
                                                              <asp:DropDownList ID="drpExchange" runat="server"></asp:DropDownList>
                                                        </td>
                                                        
                                                    </tr>
                                                    <tr id="Tr_Option_Exercise">
                                                        <td style="width: 126px; font-weight: bold; color: #000099;" class="gridcellleft">
                                                            Product :</td>
                                                        <td style="width: 239px" class="gridcellleft">
                                                           <asp:TextBox ID="txtProducts" runat="server" Width="95%" onfocus="this.select()" onkeyup="GetProducts(this,'GenericAjaxList',event)"></asp:TextBox>
                                                        </td>
                                                        
                                                    </tr>
                                                    
                                                    <tr>
                                                        <td style="width: 126px" class="gridcellleft">
                                                            <asp:HiddenField ID="txtProducts_hidden" runat="server" /><asp:HiddenField ID="txtLinkedAccount_hidden" runat="server" />
                                                            <input id="btnSave" type="button" value="Save" class="btnUpdate" onclick="btnSave_Click()"
                                                                style="width: 60px" tabindex="0" />
                                                        </td>
                                                        <td style="width: 239px" class="gridcellleft">
                                                            <input id="btnCancel" type="button" value="Cancel" class="btnUpdate" onclick="cPopUpAddSpreadAccount.Hide();"
                                                                style="width: 60px" tabindex="0" />
                                                        </td>
                                                       
                                                    </tr>
                                                </table>
                                            </dxe:PanelContent>
                                        </PanelCollection>
                                        <ClientSideEvents EndCallback="function(s, e) {
	                                                    CbpAddSpreadAccounts_EndCallBack();
                                                    }" />
                                    </dxe:ASPxCallbackPanel>
                                </dxe:PopupControlContentControl>
                            </ContentCollection>
                            <HeaderStyle HorizontalAlign="Left">
                                <Paddings PaddingRight="6px" />
                            </HeaderStyle>
                            <SizeGripImage Height="16px" Width="16px" />
                            <CloseButtonImage Height="12px" Width="13px" />
                        </dxe:ASPxPopupControl>
                    </td>
                </tr>
                
            </table>
            <dxe:ASPxGridViewExporter ID="exporter" runat="server">
            </dxe:ASPxGridViewExporter>
            
            <asp:HiddenField ID="hiddenSegmentName" runat="Server" />
            <asp:HiddenField ID="hiddenCompany" runat="server" />
            <br />
        </div>
    
</asp:Content>
