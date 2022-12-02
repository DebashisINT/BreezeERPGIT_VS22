using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Others
{
    public partial class TilesCalculator : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable Unittable = new DataTable();
                Unittable.Columns.Add("ID", typeof(int));
                Unittable.Columns.Add("Value", typeof(string));
                Unittable.Columns.Add("Type", typeof(string));

                Unittable.Rows.Add(1, "inch","Unit");
                Unittable.Rows.Add(2, "feet", "Unit");
                Unittable.Rows.Add(3, "meter", "Unit");
                Unittable.Rows.Add(4, "cm", "Unit");
                Unittable.Rows.Add(5, "yard", "Unit");
                Unittable.Rows.Add(11, "square inch", "SquareUnit");
                Unittable.Rows.Add(22, "square feet", "SquareUnit");
                Unittable.Rows.Add(33, "square meter", "SquareUnit");
                Unittable.Rows.Add(44, "square centimeters", "SquareUnit");
                Unittable.Rows.Add(55, "square yard", "SquareUnit");

                DataRow[] dr1 = Unittable.Select("Type='Unit'");
                DataTable dt1 = dr1.CopyToDataTable();

                DataRow[] dr2 = Unittable.Select("Type='SquareUnit'");
                DataTable dt2 = dr2.CopyToDataTable();

                ddlTileWidthUnit.DataSource = dt1;
                ddlTileLongUnit.DataSource = dt1;
                ddlTotalAreaUnit.DataSource = dt2;
                ddlAreaWideUnit.DataSource = dt1;
                ddlAreaLongUnit.DataSource = dt1;
                ddlGapWidthUnit.DataSource = dt1;

                ddlTileWidthUnit.DataBind();
                ddlTileLongUnit.DataBind();
                ddlTotalAreaUnit.DataBind();
                ddlAreaWideUnit.DataBind();
                ddlAreaLongUnit.DataBind();
                ddlGapWidthUnit.DataBind();

                ddlTotalAreaUnit.SelectedValue = "22";
                ddlAreaWideUnit.SelectedValue="2";
                ddlAreaLongUnit.SelectedValue = "2";
            }
        }
        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            double TilesWidth = Convert.ToDouble(txtTileWidth.Value);
            double TilesLong = Convert.ToDouble(txtTileLong.Value);

            int TilesWidtUnit = Convert.ToInt32(ddlTileWidthUnit.SelectedValue);
            int TileLongUnit = Convert.ToInt32(ddlTileLongUnit.SelectedValue);
            int TotalAreaUnit = Convert.ToInt32(ddlTotalAreaUnit.SelectedValue);
            int GapWidthUnit = Convert.ToInt32(ddlGapWidthUnit.SelectedValue);

            double TotalArea = Convert.ToDouble(txtTotalArea.Value);
            double AreaWidth = Convert.ToDouble(txtAreaWidth.Value);
            double AreaLong = Convert.ToDouble(txtAreaLong.Value);
            if (TotalArea == 0) TotalArea = AreaWidth * AreaLong;

            double GapWidth = Convert.ToDouble(txtGapWidth.Value);

            int TotalTile = Tile(TilesWidth, TilesLong, GapWidth, TotalArea ,TilesWidtUnit, TileLongUnit,  GapWidthUnit,  TotalAreaUnit);
            int MinTotalTile = Convert.ToInt32(TotalTile * (1.05));
            int MaxTotalTile = Convert.ToInt32(TotalTile * (1.10));

            lblMessage.Text = "You will need <font color='green'><b>" + TotalTile + " tiles</b></font> to cover the area. We recommend you to purchase 5% - 10% more <font color='green'><b>(" + MinTotalTile + " tiles - " + MaxTotalTile + " tiles)</b></font> for cutting and possible future repairs.";
        }
        private int Tile(double tileLen, double tileWid, double gap, double sqft, int TileLongUnit, int TilesWidtUnit, int GapWidthUnit, int TotalAreaUnit)
        {
            return Tile(UnitChange(tileLen, TileLongUnit), UnitChange(tileWid, TilesWidtUnit), UnitChange(gap, GapWidthUnit), UnitChange(sqft, TotalAreaUnit));
        }

        private double UnitChange(double tileWid, int TilesWidtUnit) {
            switch (TilesWidtUnit)
            {
                case 1:
                    break;
                case 2: tileWid *= 12;
                    break;
                case 3: tileWid *= 39.3701;
                    break;
                case 4: tileWid /= 2.54;
                    break;
                case 5: tileWid *= 36;
                    break;
                case 11: tileWid /= 144;
                    break;
                case 22:
                    break;
                case 33: tileWid *= 10.7639;
                    break;
                case 44: tileWid /= 929.03;
                    break;
                case 55: tileWid *= 9;
                    break;
            }
            return tileWid;
        }

        private int Tile(double tileLen, double tileWid, double gap, double sqft)
        {
            //tileLen += gap;
            //tileWid += gap;
            //int i = 0;
            //double L = Math.Sqrt(sqft * 144);//inches
            //int LL = (int)(L + (1 - (L % 1)));
            //int TL = (int)((LL / tileLen));
            //int TW = (int)((LL / tileWid));
            //i = TL * TW;
            //if (L % 1 != 0)
            //{
            //    i += (int)(TL / 2);
            //}

            tileLen += gap;
            tileWid += gap;
            double i = 0;
            double LL = Math.Round((Math.Sqrt(sqft * 144)), 2);//inches
            double TL = ((LL / tileLen));
            double TW = ((LL / tileWid));
            i = TL * TW;

            return Convert.ToInt32(i);
        }
    }
}