using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TreeViewPopulateOnDemand
{
    public partial class _Default : Page
    {
        private static readonly TreeFactory Factory = new TreeFactory();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateRootLevelNodes();
            }
        }

        private void PopulateRootLevelNodes()
        {
            Factory.BuildNodes(uxTree.Nodes);
        }

        protected void uxTree_TreeNodePopulate(object sender, TreeNodeEventArgs e)
        {
            Factory.PathSeparator = uxTree.PathSeparator;
            Factory.BuildNodes(e.Node);
        }
    }
}