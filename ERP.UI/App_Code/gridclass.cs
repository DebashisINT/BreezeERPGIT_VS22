using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevExpress.Web;

/// <summary>
/// Summary description for gridclass
/// </summary>
public class gridclass
{
	public gridclass()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    //public void Grid_Hover(object sender, GridView e)
    //{
    //    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='gold';");
    //    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white';");
    //}

}


public class MyTemplate : ITemplate
{
    #region ITemplate Members

    public void InstantiateIn(Control container)
    {
        GridViewEditFormTemplateContainer formContainer = (GridViewEditFormTemplateContainer)container;
        if (formContainer == null) return;
        ASPxPageControl pageControl = new ASPxPageControl();
        container.Controls.Add(pageControl);

        foreach (GridViewColumn col in formContainer.Grid.VisibleColumns)
        {
            GridViewDataColumn dataCol = col as GridViewDataColumn;
            if (dataCol == null) continue;
            TabPage page = pageControl.TabPages.Add(dataCol.FieldName);
            ASPxGridViewTemplateReplacement replacement = new ASPxGridViewTemplateReplacement();
            replacement.ReplacementType = GridViewTemplateReplacementType.EditFormCellEditor;
            replacement.ColumnID = dataCol.FieldName;
            page.Controls.Add(replacement);
        }

    }
    #endregion
}

