using SlimeWeb.Core.SDK.Interfaces;

namespace HelloWorld.ExtemtionInfo
{
    public class ExtensionInfo : IExtension
    {
        public string Name { get => "Hello World"; }
        public string Description { get => "Test extention"; }
        public string Url { get => "www.google.gr"; }
        public string Version { get => "1.1.1.0"; }
        public string Authors { get => "dddd"; }
    }
}
