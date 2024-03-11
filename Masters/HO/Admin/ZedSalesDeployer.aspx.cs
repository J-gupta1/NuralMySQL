using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using DevExpress.Web.ASPxTreeList;
using Ionic.Zip;
using System.Text;


public partial class Masters_HO_Admin_ZedSalesDeployer : System.Web.UI.Page
{
    DataTable dt = new DataTable();
    string[] strRows;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (dt.Columns.Count==0)
        {
            dt.Columns.Add("Name");
            dt.Columns.Add("Path");
        }
    }
    protected void ASPxTreeList1_VirtualModeCreateChildren(object sender, TreeListVirtualModeCreateChildrenEventArgs e)
    {
        string path = e.NodeObject == null ? Page.MapPath("~/") : e.NodeObject.ToString();
        List<string> children = new List<string>();
        if (Directory.Exists(path))
        {
            if (dt.Columns.Count == 0)
            {
                dt.Columns.Add("Name");
                dt.Columns.Add("Path");
            }
            string str=@"\";
            string[] stringSeparators = new string[] { str };
            string[] result;
            foreach (string name in Directory.GetDirectories(path))
            {
                //result = name.Split(stringSeparators, StringSplitOptions.None);
                //string FileName = result[result.Length - 1].ToString();
                //strRows = new string[2];
                //strRows[0] = FileName;
                //strRows[1] = name;
                //dt.Rows.Add(strRows);
                if (!IsSystemName(name))
                    children.Add(name);
            }
            foreach (string name in Directory.GetFiles(path))
            {
                result = name.Split(stringSeparators, StringSplitOptions.None);
                string FileName = result[result.Length - 1].ToString();
                strRows = new string[2];
                strRows[0] = FileName;
                strRows[1] = name;
                dt.Rows.Add(strRows);
                if (!IsSystemName(name))
                    children.Add(name);
            }
        }
        e.Children = children;
    }
    protected void ASPxTreeList1_VirtualModeNodeCreating(object sender, TreeListVirtualModeNodeCreatingEventArgs e)
    {
        string path = e.NodeObject.ToString();

        e.NodeKeyValue = GetNodeGuid(path);
        e.IsLeaf = !Directory.Exists(path);
        e.SetNodeValue("FileName", PopFileName(path));
    }
    Guid GetNodeGuid(string path)
    {
        if (!Map.ContainsKey(path))
            Map[path] = Guid.NewGuid();
        return Map[path];
    }
    Dictionary<string, Guid> Map
    {
        get
        {
            const string key = "DX_PATH_GUID_MAP";
            if (Session[key] == null)
                Session[key] = new Dictionary<string, Guid>();
            return Session[key] as Dictionary<string, Guid>;
        }
    }
    string PopFileName(string path)
    {
        return path.Substring(1 + path.LastIndexOf("\\"));
    }
    bool IsSystemName(string name)
    {
        name = PopFileName(name).ToLower();
        return name.StartsWith("app_") || name == "bin" || name.EndsWith(".aspx.cs") || name.EndsWith(".aspx.vb");
    }
    protected void btnZip_Click(object sender, EventArgs e)
    {
        if (dt.Columns.Count == 0)
        {
            dt.Columns.Add("Name");
            dt.Columns.Add("Path");
        }
        List<string> filesToInclude =new List<string>();
        foreach (TreeListNode node in ASPxTreeList1.GetSelectedNodes())
        {
            foreach (DataRow dtr in dt.Rows)
            {
                if (node.GetValue("FileName").ToString() == dtr["Name"].ToString())
                {
                    filesToInclude.Add(dtr["Path"].ToString());
                }
            }
        }
        if (filesToInclude.Count == 0)
        {
            //ErrorMessage.InnerHtml += "You did not select any files?<br/>\n";
        }
        else
        {
            Response.Clear();
            Response.BufferOutput = false;
            string archiveName = String.Format("archive-{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
            Response.ContentType = "application/zip";
            Response.AddHeader("content-disposition", "filename=" + archiveName);
            using (ZipFile zip = new ZipFile())
            {
                zip.AddFiles(filesToInclude.Distinct());
                zip.Save(Response.OutputStream);
            }
            Response.Close();
        }
    }
    protected void ASPxTreeList1_SelectionChanged(object sender, EventArgs e)
    {
        ASPxTreeList NewSender = (ASPxTreeList)sender;
        foreach (TreeListNode node in NewSender.GetSelectedNodes())
        {
            node.Expanded = true;
            Recursive(node);
        }
    }
    void Recursive(TreeListNode Node)
    {
        try
        {
            if (Node.HasChildren)
            {
                foreach (TreeListNode childnode in Node.ChildNodes)
                {
                    childnode.Selected = true;
                    childnode.Expanded = true;
                    Recursive(childnode);
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
}
