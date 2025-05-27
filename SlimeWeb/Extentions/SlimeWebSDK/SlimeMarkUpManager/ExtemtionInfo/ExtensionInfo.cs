using SlimeWeb.Core.SDK.Interfaces;
using System.Reflection;

namespace SlimeMarkUpManager.ExtemtionInfo
{
    public class ExtensionInfo : IExtension
    {
        public string Name { get => " SlimeMarkUpManager"; }
        public string Description { get => "Markup Extention"; }
        public string Url { get => "https://github.com/angaratosurion/SlimeMarkUp"; }
        public string Version { get
            {
                string ap;
                ap = Assembly.GetEntryAssembly().GetName().Version.ToString();

                return ap;
            } }
        public string Authors { get
            {
                string ap;
                object[] attributes = Assembly.GetExecutingAssembly()
                    .GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);

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
