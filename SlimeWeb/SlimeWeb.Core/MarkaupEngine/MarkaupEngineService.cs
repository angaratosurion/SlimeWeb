using SlimeWeb.Core.MarkaupEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlimeWeb.Core.MarkaupEngine
{
    public class MarkaupEngineService : IMarkaupEngineService
    {
        List<IEngine> engines = new List<IEngine>();
        public void AddEngine(IEngine eng)
        {
            try
            {
                if(eng!=null && this.EngineExists(eng.Name))
                {
                    engines.Add(eng);
                }
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
            }
        }

        public bool EngineExists(string name)
        {
            try
            {
                bool ap = false;
                if (String.IsNullOrEmpty(name)==false)
                {
                   var en= engines.Find(x => x.Name == name);
                    if( en!=null)
                    {
                        ap = true;
                    }
                }
                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return false;
            }
        }

        public bool EngineExists(IEngine eng)
        {

            try
            {
                bool ap = false;
                if (eng !=null)
                {
                    var en = engines.Find(x => x.Name == eng.Name);
                    if (en != null)
                    {
                        ap = true;
                    }
                }
                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return false;
            }
        }

        public IEngine GetEnginebyName(string name)
        {
            try
            {
                IEngine ap = null;
                if (String.IsNullOrEmpty(name) == false)
                {
                    var en = engines.Find(x => x.Name == name);
                    if (en != null)
                    {
                        ap = en;
                    }
                }
                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }

        public List<IEngine> GetEngines()
        {
            try
            {
                IEngine ap = null;

                return this.engines;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
    }
}
