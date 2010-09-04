using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace TreeViewPopulateOnDemand
{
    public class InfiniteNumbersBuilder : TreeLevelBuilder
    {
        public override IEnumerable<TreeNode> GetChildNodes(TreeNode parent)
        {
            string parentsStaticName = GetStaticNameFromValuePath(parent.ValuePath);

            //  if parent is a number, return nodes for the next 5 numbers
            int i;
            if (int.TryParse(parent.Value, out i))
            {
                return Enumerable.Range(1, 5).Select(x => new TreeNode(x.ToString() + " (" + parentsStaticName + ")", x.ToString())
                                                              {
                                                                  PopulateOnDemand = true,
                                                                  Expanded = false
                                                              });
            }

            // return an initial number 1
            return new[] {new TreeNode("1 (" + parentsStaticName + ")", "1")
                              {
                                  PopulateOnDemand = true,
                                  Expanded = false
                              }};
        }

        private string GetStaticNameFromValuePath(string valuePath)
        {
            return SplitValuePath(valuePath)
                .Where(x => x.StartsWith("NAME$"))
                .Select(x => x.Substring(5))
                .First();
        }

        public override bool ShouldRun(TreeNode parent)
        {
            //  run for any nodes under a static name node
            //  (look for the NAME$ any where in the valuepath)
            return parent != null && SplitValuePath(parent.ValuePath).Any(x => x.StartsWith("NAME$"));
        }
    }
}