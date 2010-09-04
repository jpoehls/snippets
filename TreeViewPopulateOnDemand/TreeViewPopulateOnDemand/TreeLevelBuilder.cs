using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace TreeViewPopulateOnDemand
{
    public abstract class TreeLevelBuilder
    {
        /// <summary>
        /// Builds and returns the list of child nodes to add
        /// to the given parent node.
        /// </summary>
        public abstract IEnumerable<TreeNode> GetChildNodes(TreeNode parent);

        /// <summary>
        /// Returns true/false whether this level builder
        /// should run for the given parent node.
        /// </summary>
        public abstract bool ShouldRun(TreeNode parent);

        /// <summary>
        /// The path separator character used by the TreeView control
        /// </summary>
        public char PathSeparator { get; set; }

        /// <summary>
        /// Splits a TreeNode's ValuePath by the separator character.
        /// </summary>
        public IEnumerable<string> SplitValuePath(string valuePath)
        {
            //  the default TreeView.PathSeparator charactor is a /g
            //  but when you ask for TreeView.PathSeparator you get '\0'
            //  So it doesn't split by / like it should -- don't know why this is
            //  SO TO FIX THIS WE JUST REPLACE \0 WITH / HERE
            if (PathSeparator == '\0')
                PathSeparator = '/';

            return valuePath.Split(PathSeparator);
        }
    }
}