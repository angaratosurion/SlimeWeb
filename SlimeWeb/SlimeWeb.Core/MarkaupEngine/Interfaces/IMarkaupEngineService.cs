using System;
using System.Collections.Generic;
using System.Text;

namespace SlimeWeb.Core.MarkaupEngine.Interfaces
{
    public interface IMarkaupEngineService
    {
        public  IEngine GetEnginebyName(string name);
        public void AddEngine(IEngine eng);
        public List<IEngine> GetEngines();
        public Boolean EngineExists(String name);
        public Boolean EngineExists(IEngine eng);

    }
}
