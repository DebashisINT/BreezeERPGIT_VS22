using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Browsing;
using BusinessLogicLayer;



namespace ERP.OMS.Management
{
    public partial class BrowseDirectory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TreeView1.Nodes.Clear();
                string[] achDrives = Directory.GetLogicalDrives();
                TreeNode onjParent;
                for (int i = 0; i < achDrives.Length; i++)
                {
                    onjParent = new TreeNode(achDrives[i].ToString(), achDrives[i].ToString());
                    onjParent.PopulateOnDemand = true;
                    TreeView1.Nodes.Add(onjParent);
                }



                TreeView1.CollapseAll();
            }
            error.Visible = false;
            TreeView1.TreeNodeExpanded += new TreeNodeEventHandler(TreeView1_TreeNodeExpanded);
            TreeView1.SelectedNodeChanged += new EventHandler(TreeView1_SelectedNodeChanged);
        }

        void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {
            _browseTextBox.Text = TreeView1.SelectedValue;
        }

        void TreeView1_TreeNodeCollapsed(object sender, TreeNodeEventArgs e)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        void TreeView1_TreeNodeExpanded(object sender, TreeNodeEventArgs e)
        {
            if (e.Node.Value.EndsWith("\\"))
            {
                AddNodes(e.Node.Value, e.Node);
            }


        }
        private TreeNode AddNodes(string path, TreeNode parentNode)
        {
            FileList objList = new FileList(path, "*.*");
            TreeNode node = new TreeNode(path, path);
            for (int index = 0; index < objList.Directories.Length; index++)
            {
                string directory = objList.Directories[index];
                TreeNode objChildNode = new TreeNode(directory, path + "\\" + directory + "\\");
                objChildNode.PopulateOnDemand = true;
                objChildNode.Target = "_blank";

                parentNode.ChildNodes.Add(objChildNode);
            }
            foreach (string file in objList.Files)
            {
                TreeNode objChildNode = new TreeNode(file, path + "\\" + file);
                parentNode.ChildNodes.Add(objChildNode);
            }
            return node;
        }


        protected void _browseButton_Click(object sender, ImageClickEventArgs e)
        {
            TreeView1.Nodes.Clear();
            if (UpdateBrowseTextBoxWithSlash())
            {

                TreeNode onjParent = new TreeNode(_browseTextBox.Text, _browseTextBox.Text);
                onjParent.PopulateOnDemand = true;
                TreeView1.Nodes.Add(onjParent);

                TreeView1.CollapseAll();
            }
            else
            {
                error.Visible = true;
                error.Text = "Please Enter valid path";
            }
        }

        private bool UpdateBrowseTextBoxWithSlash()
        {
            if (_browseTextBox.Text.Length != 0)
            {
                if (
                        -1 == _browseTextBox.Text.LastIndexOf(".") &&
                        !_browseTextBox.Text.Substring(_browseTextBox.Text.Length - 1, 1).Equals("/") &&
                        !_browseTextBox.Text.Substring(_browseTextBox.Text.Length - 1, 1).Equals("\\")
                    )
                {
                    if (_browseTextBox.Text.Substring(0, 1).Equals("\\") || -1 != _browseTextBox.Text.IndexOf(":\\"))
                        _browseTextBox.Text += "\\";
                    else
                        _browseTextBox.Text += "/";
                    return System.IO.Directory.Exists(_browseTextBox.Text);
                }
                else if (_browseTextBox.Text.LastIndexOf(".") > 0)
                {
                    return System.IO.File.Exists(_browseTextBox.Text);
                }
            }
            return true;
        }
    }
}