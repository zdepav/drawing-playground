using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using WeifenLuo.WinFormsUI.Docking;

namespace DrawingPlayground.Forms {

    public partial class HelpForm : DockContent {

        private Dictionary<string, TreeNode> types, constructors, properties, methods, pages;

        public HelpForm() {
            InitializeComponent();
            types = new Dictionary<string, TreeNode>();
            constructors = new Dictionary<string, TreeNode>();
            properties = new Dictionary<string, TreeNode>();
            methods = new Dictionary<string, TreeNode>();
            pages = new Dictionary<string, TreeNode>();
        }

        protected override string GetPersistString() => "Help";

        private void HelpForm_Load(object sender, EventArgs e) {
            var doc = new XmlDocument();
            doc.LoadXml(Properties.Resources.help_xml);
            treeView.ImageList.Images.Add(Properties.Resources.help_section);
            treeView.ImageList.Images.Add(Properties.Resources.help_page);
            treeView.ImageList.Images.Add(Properties.Resources.help_class);
            treeView.ImageList.Images.Add(Properties.Resources.help_constructor);
            treeView.ImageList.Images.Add(Properties.Resources.help_property);
            treeView.ImageList.Images.Add(Properties.Resources.help_method);
            foreach (var section in doc.DocumentElement.ChildNodes.OfType<XmlElement>()) {
                var sectionTn = new TreeNode(section.GetAttribute("title"), 0, 0) { Tag = "" };
                foreach (var subsection in section.ChildNodes.OfType<XmlElement>().Where(e => e.Name == "subsection")) {
                    var subsectionTn = new TreeNode(subsection.GetAttribute("title"), 0, 0) { Tag = "" };
                    sectionTn.Nodes.Add(subsectionTn);
                    foreach (var page in section.ChildNodes.OfType<XmlElement>().Where(e => e.Name == "page")) {
                        var pageTn = new TreeNode(page.GetAttribute("title"), 0, 0) {
                            Tag = BuildHelpPageHtml(page, page.GetAttribute("title"))
                        };
                        subsectionTn.Nodes.Add(pageTn);
                        if (page.HasAttribute("page-id")) {
                            pages.Add(page.GetAttribute("page-id"), pageTn);
                        }
                    }
                }
                foreach (var page in section.ChildNodes.OfType<XmlElement>().Where(e => e.Name == "page")) {

                }
                foreach (var type in section.ChildNodes.OfType<XmlElement>().Where(e => e.Name == "type")) {

                }
            }

        }

        private string BuildHelpPageHtml(XmlElement page, string title) {
            var html = new StringBuilder("<!DOCTYPE html><html><head><meta charset=\"utf-8\"/><title></title></head><body>");
            foreach (var node in page.ChildNodes.OfType<XmlNode>()) {
                AppendXmlNodeToHtml(node, html);
            }
            html.Append("</body></html>");
            return html.ToString();
        }

        private void AppendXmlNodeToHtml(XmlNode node, StringBuilder html) {
            switch (node) {
                case XmlText textNode:
                    html.Append(textNode.InnerText);
                    break;
                case XmlElement element:
                    AppendXmlElementToHtml(element, html);
                    break;
            }
        }

        private void AppendXmlElementToHtml(XmlElement element, StringBuilder html) {
            switch (element.Name) {
                case "b":
                    html.Append("<strong>");
                    foreach (var node in element.ChildNodes.OfType<XmlNode>()) {
                        AppendXmlNodeToHtml(node, html);
                    }
                    html.Append("</strong>");
                    break;
                case "i":
                    html.Append("<em>");
                    foreach (var node in element.ChildNodes.OfType<XmlNode>()) {
                        AppendXmlNodeToHtml(node, html);
                    }
                    html.Append("</em>");
                    break;
                case "u":
                    html.Append("<strong>");
                    foreach (var node in element.ChildNodes.OfType<XmlNode>()) {
                        AppendXmlNodeToHtml(node, html);
                    }
                    html.Append("</strong>");
                    break;
                case "s": {
                    var l = rtb.TextLength;
                    foreach (var node in element.ChildNodes.OfType<XmlNode>()) {
                        AppendXmlNodeToRtb(node, rtb);
                    }
                    rtb.Select(l, rtb.TextLength - l);
                    var font = rtb.SelectionFont;
                    rtb.SelectionFont = new Font(rtb.SelectionFont, font.Style | (
                        element.Name switch {
                            "b" => FontStyle.Bold,
                            "i" => FontStyle.Italic,
                            "u" => FontStyle.Underline,
                            "s" => FontStyle.Strikeout,
                            _ => FontStyle.Regular

                        }
                    ));
                    break;
                }
                case "a": {
                    var l = rtb.TextLength;
                    foreach (var node in element.ChildNodes.OfType<XmlNode>()) {
                        AppendXmlNodeToRtb(node, rtb);
                    }
                    rtb.Select(l, rtb.TextLength - l);
                    rtb.
                    break;
                }
                case "a":
                    break;
                case "a":
                    break;
                case "a":
                    break;
            }

        }
    }

}
