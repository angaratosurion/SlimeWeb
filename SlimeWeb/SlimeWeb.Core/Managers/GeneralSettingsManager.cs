using Org.BouncyCastle.Asn1.X509;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Managers
{
    public class GeneralSettingsManager:DataManager
    {
        public Boolean Exists()
        {
            try
            {
                if (db.GeneralSettings == null)
                {
                    return false;

                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return false;
            }

        }
        public GeneralSettings Details()
        {
			try
			{
                if(!this.Exists()) 
                {
                    GeneralSettings ap = new GeneralSettings();
                    db.Settings.Add(ap);
                    db.SaveChanges();

                }
                return db.GeneralSettings;
			}
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }

        }
        public async Task Edit(GeneralSettings bl)
        {
            try
            {
                if( this.Exists())
                {



                }

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
               // return null;
            }
        }
        }
}
