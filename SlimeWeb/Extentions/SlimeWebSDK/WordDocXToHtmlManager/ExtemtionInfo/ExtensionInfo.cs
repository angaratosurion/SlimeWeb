using SlimeWeb.Core.SDK.Interfaces;
using System.Reflection;

namespace WordDocXToHtmlManager.ExtemtionInfo
{
    public class ExtensionInfo : IExtension
    {
        public string Name { get => " WordDocXToHtmlManager"; }
        public string Description { get => "Makes the CMS to use word documents  for storage " +
                "instead of the database "; }
        public string Url { get => "www.google.gr"; }
        public string Version { get
            {
                string ap;
                ap = Assembly.GetEntryAssembly().GetName().Version.ToString();

                return ap;
            } }
        public string Authors { get
            {
                string ap;
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);

                AssemblyCopyrightAttribute attribute = null;
                if (attributes.Length > 0)
                {
                    attribute = attributes[0] as AssemblyCopyrightAttribute;
                }
                ap = attribute.Copyright;

                return ap;
            }
        }
    }
}
