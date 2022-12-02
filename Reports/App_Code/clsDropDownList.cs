using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using DevExpress.Web;

/// <summary>
/// Summary description for clsDropDownList
/// </summary>


namespace Reports
{
    public class clsDropDownList
    {
        //public clsDropDownList()
        //{
        //}

        public void AddDataToDropDownList(string[,] listItems, DropDownList ComboBox)
        {
            ComboBox.Items.Clear();
            if (listItems[0, 0] != "n")
            {
                int length1 = listItems.GetLength(0);
                int lenght2 = listItems.GetLength(1);
                //string[,] item = new string[1, 2];
                //item[0, 0] = "Select";
                //item[0, 1] = string.Empty;
                //ComboBox.Items.Add(new ListItem(item[0, 0], item[0, 1]));
                for (int i = 0; i < length1; i++)
                {
                    ComboBox.Items.Add(new ListItem(listItems[i, 1], listItems[i, 0]));
                    //ComboBox.DataTextField =
                }
            }
        }

        public void AddDataToDropDownList(string[,] listItems, DropDownList ComboBox,
                                            Boolean ZeroPositionItem //__pass true/false if you want to place a 'Select' text with null value at start___//
                                         )
        {
            ComboBox.Items.Clear();
            if (listItems[0, 0] != "n")
            {
                int length1 = listItems.GetLength(0);
                int lenght2 = listItems.GetLength(1);
                if (ZeroPositionItem)
                {
                    string[,] item = new string[1, 2];
                    item[0, 0] = "Select";
                    item[0, 1] = string.Empty;
                    ComboBox.Items.Add(new ListItem(item[0, 0], item[0, 1]));
                }
                for (int i = 0; i < length1; i++)
                {
                    ComboBox.Items.Add(new ListItem(listItems[i, 1], listItems[i, 0]));
                    //ComboBox.DataTextField =
                }
            }
        }

        public void AddDataToDropDownList(string[,] listItems, DropDownList ComboBox,
                                            string ZeroPosition_All //__pass true/false if you want to place a 'All' text with 'All' value at start___//
                                         )
        {
            ComboBox.Items.Clear();
            if (listItems[0, 0] != "n")
            {
                int length1 = listItems.GetLength(0);
                int lenght2 = listItems.GetLength(1);
                if (ZeroPosition_All == "All")
                {
                    string[,] item = new string[1, 2];
                    item[0, 0] = "All";
                    item[0, 1] = "All";
                    ComboBox.Items.Add(new ListItem(item[0, 0], item[0, 1]));
                }
                for (int i = 0; i < length1; i++)
                {
                    ComboBox.Items.Add(new ListItem(listItems[i, 1], listItems[i, 0]));
                    //ComboBox.DataTextField =
                }
            }
        }

        public void AddDataToDropDownListToAspx(string[,] listItems, ASPxComboBox ComboBox1,
                                            Boolean ZeroPositionItem //__pass true/false if you want to place a 'Select' text with null value at start___//
                                         )
        {
            ComboBox1.Items.Clear();
            if (listItems[0, 0] != "n")
            {
                int length1 = listItems.GetLength(0);
                int lenght2 = listItems.GetLength(1);
                if (ZeroPositionItem)
                {
                    string[,] item = new string[1, 2];
                    item[0, 0] = "Select";
                    item[0, 1] = string.Empty;
                    ComboBox1.Items.Add(new ListEditItem(item[0, 0], item[0, 1]));
                }
                for (int i = 0; i < length1; i++)
                {
                    ComboBox1.Items.Add(new ListEditItem(listItems[i, 1], listItems[i, 0]));
                }
            }
        }




        public void AddDataToDropDownList(string[,] listItems, ASPxComboBox ComboBox,
                                            string ZeroPosition_All //__pass true/false if you want to place a 'All' text with 'All' value at start___//
                                         )
        {
            ComboBox.Items.Clear();
            if (listItems[0, 0] != "n")
            {
                int length1 = listItems.GetLength(0);
                int lenght2 = listItems.GetLength(1);
                if (ZeroPosition_All == "All")
                {
                    string[,] item = new string[1, 2];
                    item[0, 0] = "All";
                    item[0, 1] = "All";
                    ComboBox.Items.Add(new ListEditItem(item[0, 0], item[0, 1]));
                }
                for (int i = 0; i < length1; i++)
                {
                    ComboBox.Items.Add(new ListEditItem(listItems[i, 1], listItems[i, 0]));
                }
            }
        }


        public void AddDataToListBox(string[,] listItems, ListBox ListBx)
        {
            ListBx.Items.Clear();
            if (listItems[0, 0] != "n")
            {
                int length1 = listItems.GetLength(0);
                int lenght2 = listItems.GetLength(1);
                //string[,] item = new string[1, 2];
                //item[0, 0] = "Select";
                //item[0, 1] = string.Empty;
                //ComboBox.Items.Add(new ListItem(item[0, 0], item[0, 1]));
                for (int i = 0; i < length1; i++)
                {
                    ListBx.Items.Add(new ListItem(listItems[i, 1], listItems[i, 0]));
                    //ComboBox.DataTextField =
                }
            }
        }

        public void AddDataToListBox(string[,] listItems, ListBox ListBx, string All_Selection)
        {
            ListBx.Items.Clear();
            if (listItems[0, 0] != "n")
            {
                int length1 = listItems.GetLength(0);
                int lenght2 = listItems.GetLength(1);
                if (All_Selection == "All")
                {
                    ListBx.Items.Add(new ListItem("All", "All"));
                }
                for (int i = 0; i < length1; i++)
                {
                    ListBx.Items.Add(new ListItem(listItems[i, 1], listItems[i, 0]));
                    //ComboBox.DataTextField =
                }
            }
        }

        // Function Overloading for dropDown List----//
        public void AddDataToDropDownList(string[,] listItems, DropDownList ComboBox, int SelectedValue)
        {
            ComboBox.Items.Clear();
            int length1 = listItems.GetLength(0);
            int lenght2 = listItems.GetLength(1);
            //string[,] item = new string[1, 2];
            //item[0, 0] = "Select";
            //item[0, 1] = string.Empty;
            //ComboBox.Items.Add(new ListItem(item[0, 0], item[0, 1]));
            for (int i = 0; i < length1; i++)
            {
                if (Int32.Parse(listItems[i, 0].ToString()) == SelectedValue)
                {
                    ComboBox.Items.Add(new ListItem(listItems[i, 1], listItems[i, 0]));
                    ComboBox.SelectedValue = listItems[i, 0].ToString();
                }
                else
                {
                    ComboBox.Items.Add(new ListItem(listItems[i, 1], listItems[i, 0]));
                }
                //ComboBox.DataTextField =
            }
            //ComboBox.SelectedIndex = indexno-1;
        }



    }

}