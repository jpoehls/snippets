using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace TreeViewPopulateOnDemand
{
    /// <summary>
    /// Just adds some hard-coded nodes to the tree but includes
    /// a prefix on the value that lets the InfiniteNumbersBuilder
    /// know where it should work.
    /// </summary>
    public class StaticNamesBuilder : TreeLevelBuilder
    {
        public override IEnumerable<TreeNode> GetChildNodes(TreeNode parent)
        {
            const string prefix = "NAME$";
            var staticNames = new[] { "Joshua", "Kristina" };
            return staticNames.Select(x =>
                                      new TreeNode(x, prefix + x)
                                          {
                                              PopulateOnDemand = true,
                                              Expanded = false
                                          });
        }

        public override bool ShouldRun(TreeNode parent)
        {
            //  this builder should only add nodes to the root of the tree
            return parent == null;
        }
    }
}