using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace TreeViewPopulateOnDemand
{
    /// <summary>
    /// Tree Factory knows about all of the level builders
    /// and gives you an easy way to call all of them at once
    /// filtering out the ones that shouldn't be used.
    /// </summary>
    public class TreeFactory
    {
        private readonly List<TreeLevelBuilder> _levelBuilders;

        public TreeFactory()
        {
            _levelBuilders = new List<TreeLevelBuilder>();
            _levelBuilders.Add(new StaticNamesBuilder());
            _levelBuilders.Add(new InfiniteNumbersBuilder());
            _levelBuilders.Add(new FileSystemBuilder());
        }

        public char PathSeparator { get; set; }

        public void SetPathSeparator(char separator)
        {
            _levelBuilders.ForEach(x => x.PathSeparator = separator);
        }

        public void BuildNodes(TreeNodeCollection rootNodeCollection)
        {
            BuildNodes(null, rootNodeCollection);
        }

        public void BuildNodes(TreeNode parent)
        {
            BuildNodes(parent, parent.ChildNodes);
        }

        private void BuildNodes(TreeNode parent, TreeNodeCollection nodeCollectionToAddTo)
        {
            IEnumerable<TreeNode> nodes = _levelBuilders
                .Where(level => level.ShouldRun(parent))
                .SelectMany(level => level.GetChildNodes(parent));
            foreach (TreeNode node in nodes)
            {
                nodeCollectionToAddTo.Add(node);
            }
        }
    }
}