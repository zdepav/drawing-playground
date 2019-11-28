using DrawingPlayground.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            var doc = new XmlDocument { PreserveWhitespace = true };
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
                        var pageTn = new TreeNode(page.GetAttribute("title"), 1, 1) {
                            Tag = BuildHelpPageHtml(page)
                        };
                        subsectionTn.Nodes.Add(pageTn);
                        if (page.HasAttribute("page-id")) {
                            pages.Add(page.GetAttribute("page-id"), pageTn);
                        }
                    }
                }
                foreach (var page in section.ChildNodes.OfType<XmlElement>().Where(e => e.Name == "page")) {
                    var pageTn = new TreeNode(page.GetAttribute("title"), 1, 1) {
                        Tag = BuildHelpPageHtml(page)
                    };
                    sectionTn.Nodes.Add(pageTn);
                    if (page.HasAttribute("page-id")) {
                        pages.Add(page.GetAttribute("page-id"), pageTn);
                    }
                }
                foreach (var type in section.ChildNodes.OfType<XmlElement>().Where(e => e.Name == "type")) {
                    sectionTn.Nodes.Add(BuildTypeHelp(type));
                }
            }

        }

        private TreeNode BuildTypeHelp(XmlElement type) {
            var typeTn = new TreeNode(type.GetAttribute("name"));















        }

        private string BuildHelpPageHtml(XmlElement page) {
            var html = new StringBuilder("<!DOCTYPE html><html><head><meta charset=\"utf-8\"/><title>");
            html.Append(page.GetAttribute("title"));
            html.Append("</title></head><body>");
            foreach (var node in page.ChildNodes.OfType<XmlNode>()) {
                AppendXmlNodeToHtml(node, html);
            }
            html.Append("</body></html>");
            return html.ToString();
        }

        private static readonly Regex whitespaceRegex = new Regex(@"\s+");

        private void AppendXmlNodeToHtml(XmlNode node, StringBuilder html, bool insideCodeElement = false) {
            switch (node) {
                case XmlText textNode:
                    if (insideCodeElement) {
                        html.Append(textNode.InnerText);
                    } else {
                        html.Append(textNode.InnerText.Trim().Replace(whitespaceRegex, " "));
                    }
                    break;
                case XmlElement element:
                    if (insideCodeElement) {
                        // only <br/> tag is allowed inside the code tag
                        html.Append("<br/>");
                    } else {
                        AppendXmlElementToHtml(element, html);
                    }
                    break;
            }
        }

        private void AppendXmlElementToHtml(XmlElement element, StringBuilder html) {
            switch (element.Name) {
                case "b":
                    html.Append("<strong>");
                    AppendXmlElementChildrenToHtml(element, html);
                    html.Append("</strong>");
                    break;
                case "i":
                    html.Append("<em>");
                    AppendXmlElementChildrenToHtml(element, html);
                    html.Append("</em>");
                    break;
                case "u":
                    html.Append("<span style=\"text-decoration: underline\">");
                    AppendXmlElementChildrenToHtml(element, html);
                    html.Append("</span>");
                    break;
                case "s":
                    html.Append("<span style=\"text-decoration: line-through\">");
                    AppendXmlElementChildrenToHtml(element, html);
                    html.Append("</span>");
                    break;
                case "a":
                    html.Append("<a href=\"");
                    html.Append(element.GetAttribute("href"));
                    html.Append("\">");
                    AppendXmlElementChildrenToHtml(element, html);
                    html.Append("</a>");
                    break;
                case "p":
                    html.Append("<p>");
                    AppendXmlElementChildrenToHtml(element, html);
                    html.Append("</p>");
                    break;
                case "ul":
                case "ol":
                case "thead":
                case "tr":
                    html.Append('<');
                    html.Append(element.Name);
                    html.Append('>');
                    foreach (var childElement in element.ChildNodes.OfType<XmlElement>()) {
                        html.Append('<');
                        html.Append(childElement.Name);
                        html.Append('>');
                        AppendXmlElementChildrenToHtml(childElement, html);
                        html.Append("</");
                        html.Append(childElement.Name);
                        html.Append('>');
                    }
                    html.Append("</");
                    html.Append(element.Name);
                    html.Append('>');
                    break;
                case "code":
                    html.Append("<pre>");
                    foreach (var childNode in element.ChildNodes.OfType<XmlNode>()) {
                        AppendXmlNodeToHtml(childNode, html, true);
                    }
                    html.Append("</pre>");
                    break;
                case "br":
                    html.Append("<br/>");
                    break;
                case "table":
                    html.Append("<table>");
                    if (element.ChildNodes.OfType<XmlElement>().FirstOrDefault() is XmlElement elem && elem.Name == "thead") {
                        AppendXmlElementToHtml(elem, html);
                        html.Append("<tbody>");
                        foreach (var childElement in element.ChildNodes.OfType<XmlElement>().Skip(1)) {
                            AppendXmlElementToHtml(childElement, html);
                        }
                    } else {
                        html.Append("<tbody>");
                        AppendXmlElementChildrenToHtml(element, html);
                    }
                    html.Append("</tbody></table>");
                    break;
            }

        }
        private void AppendXmlElementChildrenToHtml(XmlElement element, StringBuilder html) {
            foreach (var node in element.ChildNodes.OfType<XmlNode>()) {
                AppendXmlNodeToHtml(node, html);
            }
        }
    }

}
