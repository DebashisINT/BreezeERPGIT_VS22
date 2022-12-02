using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

/// <summary>
/// Summary description for BSReport
/// </summary>
public class PrintPLBS : DevExpress.XtraReports.UI.XtraReport
{
    private DevExpress.XtraReports.UI.DetailBand Detail;
    public TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    public PrintableComponentContainer printableComponentContainer2;
    public PrintableComponentContainer printableComponentContainer1;
    private FormattingRule formattingRule1;
    private XRCrossBandBox xrCrossBandBox1;
    public XRLabel AddressLine1;
    public XRLabel txtCompanyName;
    public XRLabel AddressLine2;
    public XRLabel AsOn;
    public XRLabel TypeRight;
    public XRLabel TypeLeft;
    private PageFooterBand PageFooter;
    private XRPageInfo xrPageInfo1;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public PrintPLBS()
    {
        InitializeComponent();

        

        //this.PaperKind = System.Drawing.Printing.PaperKind.LegalExtra;
        //this.Landscape = true;
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        disposing = false;
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.printableComponentContainer1 = new DevExpress.XtraReports.UI.PrintableComponentContainer();
            this.printableComponentContainer2 = new DevExpress.XtraReports.UI.PrintableComponentContainer();
            this.TypeRight = new DevExpress.XtraReports.UI.XRLabel();
            this.TypeLeft = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.AsOn = new DevExpress.XtraReports.UI.XRLabel();
            this.AddressLine2 = new DevExpress.XtraReports.UI.XRLabel();
            this.AddressLine1 = new DevExpress.XtraReports.UI.XRLabel();
            this.txtCompanyName = new DevExpress.XtraReports.UI.XRLabel();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.formattingRule1 = new DevExpress.XtraReports.UI.FormattingRule();
            this.xrCrossBandBox1 = new DevExpress.XtraReports.UI.XRCrossBandBox();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.printableComponentContainer1,
            this.printableComponentContainer2});
            this.Detail.HeightF = 148.5417F;
            this.Detail.Name = "Detail";
            this.Detail.StylePriority.UsePadding = false;
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // printableComponentContainer1
            // 
            this.printableComponentContainer1.LocationFloat = new DevExpress.Utils.PointFloat(3.739899E-05F, 0F);
            this.printableComponentContainer1.Name = "printableComponentContainer1";
            this.printableComponentContainer1.Padding = new DevExpress.XtraPrinting.PaddingInfo(50, 0, 0, 0, 100F);
            this.printableComponentContainer1.SizeF = new System.Drawing.SizeF(624.5734F, 109.326F);
            this.printableComponentContainer1.StylePriority.UseBackColor = false;
            this.printableComponentContainer1.StylePriority.UseBorderColor = false;
            this.printableComponentContainer1.StylePriority.UseBorderDashStyle = false;
            this.printableComponentContainer1.StylePriority.UseBorders = false;
            this.printableComponentContainer1.StylePriority.UseBorderWidth = false;
            this.printableComponentContainer1.StylePriority.UseFont = false;
            this.printableComponentContainer1.StylePriority.UseForeColor = false;
            this.printableComponentContainer1.StylePriority.UsePadding = false;
            this.printableComponentContainer1.StylePriority.UseTextAlignment = false;
            // 
            // printableComponentContainer2
            // 
            this.printableComponentContainer2.LocationFloat = new DevExpress.Utils.PointFloat(624.5728F, 3.739899E-05F);
            this.printableComponentContainer2.Name = "printableComponentContainer2";
            this.printableComponentContainer2.Padding = new DevExpress.XtraPrinting.PaddingInfo(60, 0, 0, 0, 100F);
            this.printableComponentContainer2.SizeF = new System.Drawing.SizeF(642.4266F, 109.326F);
            this.printableComponentContainer2.StylePriority.UseBackColor = false;
            this.printableComponentContainer2.StylePriority.UseBorderColor = false;
            this.printableComponentContainer2.StylePriority.UseBorderDashStyle = false;
            this.printableComponentContainer2.StylePriority.UseBorders = false;
            this.printableComponentContainer2.StylePriority.UseBorderWidth = false;
            this.printableComponentContainer2.StylePriority.UseFont = false;
            this.printableComponentContainer2.StylePriority.UseForeColor = false;
            this.printableComponentContainer2.StylePriority.UsePadding = false;
            this.printableComponentContainer2.StylePriority.UseTextAlignment = false;
            // 
            // TypeRight
            // 
            this.TypeRight.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.TypeRight.LocationFloat = new DevExpress.Utils.PointFloat(624.5734F, 99F);
            this.TypeRight.Name = "TypeRight";
            this.TypeRight.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.TypeRight.SizeF = new System.Drawing.SizeF(642.4261F, 23.00001F);
            this.TypeRight.StylePriority.UseFont = false;
            this.TypeRight.StylePriority.UseTextAlignment = false;
            this.TypeRight.Text = "xrLabel1";
            this.TypeRight.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // TypeLeft
            // 
            this.TypeLeft.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.TypeLeft.LocationFloat = new DevExpress.Utils.PointFloat(0F, 98.87501F);
            this.TypeLeft.Name = "TypeLeft";
            this.TypeLeft.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.TypeLeft.SizeF = new System.Drawing.SizeF(624.5734F, 23F);
            this.TypeLeft.StylePriority.UseFont = false;
            this.TypeLeft.StylePriority.UseTextAlignment = false;
            this.TypeLeft.Text = "TypeLeft";
            this.TypeLeft.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // TopMargin
            // 
            this.TopMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.AsOn,
            this.AddressLine2,
            this.AddressLine1,
            this.txtCompanyName,
            this.TypeRight,
            this.TypeLeft});
            this.TopMargin.HeightF = 122F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // AsOn
            // 
            this.AsOn.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.AsOn.LocationFloat = new DevExpress.Utils.PointFloat(0F, 68.99998F);
            this.AsOn.Name = "AsOn";
            this.AsOn.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.AsOn.SizeF = new System.Drawing.SizeF(1267F, 23F);
            this.AsOn.StylePriority.UseFont = false;
            this.AsOn.StylePriority.UseTextAlignment = false;
            this.AsOn.Text = "AddressLine2";
            this.AsOn.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // AddressLine2
            // 
            this.AddressLine2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.AddressLine2.LocationFloat = new DevExpress.Utils.PointFloat(3.739899E-05F, 46F);
            this.AddressLine2.Name = "AddressLine2";
            this.AddressLine2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.AddressLine2.SizeF = new System.Drawing.SizeF(1267F, 22.99999F);
            this.AddressLine2.StylePriority.UseFont = false;
            this.AddressLine2.StylePriority.UseTextAlignment = false;
            this.AddressLine2.Text = "AddressLine2";
            this.AddressLine2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // AddressLine1
            // 
            this.AddressLine1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.AddressLine1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 23.00001F);
            this.AddressLine1.Name = "AddressLine1";
            this.AddressLine1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.AddressLine1.SizeF = new System.Drawing.SizeF(1267F, 23F);
            this.AddressLine1.StylePriority.UseFont = false;
            this.AddressLine1.StylePriority.UseTextAlignment = false;
            this.AddressLine1.Text = "AddressLine1";
            this.AddressLine1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // txtCompanyName
            // 
            this.txtCompanyName.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.txtCompanyName.ForeColor = System.Drawing.Color.Red;
            this.txtCompanyName.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.txtCompanyName.Name = "txtCompanyName";
            this.txtCompanyName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.txtCompanyName.SizeF = new System.Drawing.SizeF(1267F, 23F);
            this.txtCompanyName.StylePriority.UseFont = false;
            this.txtCompanyName.StylePriority.UseForeColor = false;
            this.txtCompanyName.StylePriority.UseTextAlignment = false;
            this.txtCompanyName.Text = "txtCompanyName";
            this.txtCompanyName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 100F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // formattingRule1
            // 
            this.formattingRule1.Name = "formattingRule1";
            // 
            // xrCrossBandBox1
            // 
            this.xrCrossBandBox1.BorderWidth = 2F;
            this.xrCrossBandBox1.EndBand = null;
            this.xrCrossBandBox1.EndPointFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrCrossBandBox1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrCrossBandBox1.Name = "xrCrossBandBox1";
            this.xrCrossBandBox1.StartBand = null;
            this.xrCrossBandBox1.StartPointFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrCrossBandBox1.WidthF = 50F;
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo1});
            this.PageFooter.FormattingRules.Add(this.formattingRule1);
            this.PageFooter.HeightF = 37.5F;
            this.PageFooter.Name = "PageFooter";
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.FormattingRules.Add(this.formattingRule1);
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(3.739899E-05F, 9.999968F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(1266.999F, 23F);
            // 
            // PrintPLBS
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageFooter});
            this.CrossBandControls.AddRange(new DevExpress.XtraReports.UI.XRCrossBandControl[] {
            this.xrCrossBandBox1});
            this.FormattingRuleSheet.AddRange(new DevExpress.XtraReports.UI.FormattingRule[] {
            this.formattingRule1});
            this.Margins = new System.Drawing.Printing.Margins(1, 0, 122, 100);
            this.Name = "PrintPLBS";
            this.PageHeight = 1752;
            this.PageWidth = 1268;
            this.PaperKind = System.Drawing.Printing.PaperKind.A3Extra;
            this.Version = "15.1";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
}
