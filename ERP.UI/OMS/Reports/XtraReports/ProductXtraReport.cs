using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting.BarCode;

namespace ERP.OMS.Reports.XtraReports
{
    public partial class ProductXtraReport : DevExpress.XtraReports.UI.XtraReport
    {
        public ProductXtraReport()
        {
            InitializeComponent();
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            if (GetCurrentColumnValue("sProduct_ImagePath").ToString() != "")
            {
                this.prodImage.ImageUrl = Convert.ToString(GetCurrentColumnValue("sProduct_ImagePath"));
                this.prodImage.Visible = true;
            }
            else
            {
                this.prodImage.Visible = false;
            }

            //Redirect property added here
            xrTableCell5.NavigateUrl = "~/OMS/management/store/Master/sProducts.aspx?DirectEdit=" + Convert.ToString(GetCurrentColumnValue("Id"));


          




        }

        private void xrBarCode1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRBarCode brCd = sender as XRBarCode;
            xrTableCell6.Visible = true;
            brCd.Visible = true;
            if (Convert.ToString(GetCurrentColumnValue("sProducts_barCode")) == "")
            {
                 brCd.Visible = false;
               // xrTableCell6.Visible = false;
            }
            else
            {
                brCd.Text =Convert.ToString( GetCurrentColumnValue("sProducts_barCode"));
                switch (Convert.ToString(GetCurrentColumnValue("sProducts_barCodeType")))
                {
                    case "1":
                        brCd.Symbology = new CodabarGenerator();
                        break;
                    case "2":
                        brCd.Symbology = new Code11Generator();
                        break;
                    case "3":
                        brCd.Symbology = new Code128Generator();
                        break;
                    case "4":
                        brCd.Symbology = new Code39Generator();
                        break;
                    case "5":
                        brCd.Symbology = new Code39ExtendedGenerator();
                        break;
                    case "6":
                        brCd.Symbology = new Code93Generator();
                        break;
                    case "7":
                        brCd.Symbology = new Code93ExtendedGenerator();
                        break;
                    case "8":
                        brCd.Symbology = new CodeMSIGenerator();
                        break;
                    case "9":
                        brCd.Symbology = new DataBarGenerator();
                        break;
                    case "10":
                        brCd.Symbology = new DataMatrixGenerator();
                        break;
                    case "11":
                        brCd.Symbology = new DataMatrixGS1Generator();
                        break;
                    case "12":
                        brCd.Symbology = new EAN128Generator();
                        break;
                    case "13":
                        brCd.Symbology = new EAN13Generator();
                        break;
                    case "14":
                        brCd.Symbology = new EAN8Generator();
                        break;
                    case "15":
                        brCd.Symbology = new Industrial2of5Generator();
                        break;
                    case "16":
                        brCd.Symbology = new IntelligentMailGenerator();
                        break;
                    case "17":
                        brCd.Symbology = new Interleaved2of5Generator();
                        break;
                    case "18":
                        brCd.Symbology = new ITF14Generator();
                        break;
                    case "19":
                        brCd.Symbology = new Matrix2of5Generator();
                        break;
                    case "20":
                        brCd.Symbology = new PDF417Generator();
                        break;
                    case "21":
                        brCd.Symbology = new PostNetGenerator();
                        break;
                    case "22":
                        brCd.Symbology = new QRCodeGenerator();
                        break;
                    case "23":
                        brCd.Symbology = new UPCAGenerator();
                        break;
                    case "24":
                        brCd.Symbology = new UPCE0Generator();
                        break;
                    case "25":
                        brCd.Symbology = new UPCE1Generator();
                        break;
                    case "26":
                        brCd.Symbology = new UPCSupplemental2Generator();
                        break;
                    case "27":
                        brCd.Symbology = new UPCSupplemental5Generator();
                        break;
                    default:
                        //this.xrTableCell6.Visible = false;
                      //  xrTableCell6.Visible = false;
                        break;
                };

            }
        }



    }
}
