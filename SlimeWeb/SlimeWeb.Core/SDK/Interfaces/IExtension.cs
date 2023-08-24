 
namespace SlimeWeb.Core.SDK.Interfaces
{
    public interface IExtension
    {
        public string Name { get;  }
        //Descriptio
        public string Description { get;  }

        //Url
        public string Url { get; }
        public string Version { get;  }
        public string  Authors { get;  }
    }
}
