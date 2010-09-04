using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace TreeViewPopulateOnDemand
{
    public class FileSystemBuilder : TreeLevelBuilder
    {
        public override IEnumerable<TreeNode> GetChildNodes(TreeNode parent)
        {
            var nodes = new List<TreeNode>();

            bool isFirst = true;
            string dirToListFrom = HttpContext.Current.Server.MapPath("~/");
            if (parent != null)
            {
                dirToListFrom = Path.Combine(dirToListFrom, GetFileSystemPathFromValuePath(parent.ValuePath));
                isFirst = false;
            }

            //  add sub-directories
            foreach (var dir in Directory.GetDirectories(dirToListFrom))
            {
                var dirInfo = new DirectoryInfo(dir);
                string value = dirInfo.Name;
                if (isFirst)
                {
                    value = "FS$" + dirInfo.Name;
                }
                nodes.Add(new TreeNode(dirInfo.Name, value)
                              {
                                  PopulateOnDemand = true,
                                  Expanded = false
                              });
            }

            //  add files
            foreach (var file in Directory.GetFiles(dirToListFrom))
            {
                var fileInfo = new FileInfo(file);
                var value = fileInfo.Name;
                if (isFirst)
                {
                    value = "FS$" + fileInfo;
                }
                nodes.Add(new TreeNode(fileInfo.Name, value)
                              {
                                  PopulateOnDemand = false,
                                  Expanded = false,
                                  // instead of an alert, this can be open the file in a frame
                                  // by setting the Target property to the name of the frame
                                  // and using the NavigateUrl to point to a viewer page
                                  // and passing the file to show in the url
                                  // ex. ~/Viewer.aspx?path=  UrlEncode(path)
                                  NavigateUrl = "javascript:alert(" + EncodeJsString("opening file:\n\n") + " + " + EncodeJsString(fileInfo.FullName) + ");"
                              });
            }

            return nodes;
        }

        /// <summary>
        /// Encodes a string to be represented as a string literal. The format
        /// is essentially a JSON string.
        /// 
        /// The string returned includes outer quotes 
        /// Example Output: "Hello \"Rick\"!\r\nRock on"
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static string EncodeJsString(string s)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\"");
            foreach (char c in s)
            {
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        int i = (int)c;
                        if (i < 32 || i > 127)
                        {
                            sb.AppendFormat("\\u{0:X04}", i);
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }
            sb.Append("\"");

            return sb.ToString();
        }

        private string GetFileSystemPathFromValuePath(string valuePath)
        {
            //  strips the FS$ prefix from the value
            var split = SplitValuePath(valuePath);
            string path = "";
            foreach (var item in split)
            {
                if (item.StartsWith("FS$") && path == "")
                {
                    path = Path.Combine(path, item.Substring(3));
                }
                else if (path != "")
                {
                    path = Path.Combine(path, path);
                }
            }
            return path;
        }

        public override bool ShouldRun(TreeNode parent)
        {
            //  only run for the root of the tree, or for parents
            //  that are also file system nodes (look for FS$ prefix)
            return parent == null || SplitValuePath(parent.ValuePath).Any(x => x.StartsWith("FS$"));
        }
    }
}