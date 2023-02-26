using Org.BouncyCastle.Asn1.X509;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Data.ViewModels;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Managers
{
    public class GeneralSettingsManager:DataManager
    {
        public ViewGeneralSettings Details()
        {
            try
            {
                ViewGeneralSettings ap = new ViewGeneralSettings();
                GeneralSettings tap = null; ;
                if (this.Exists())
                {
                    tap = db.GeneralSettings;
                    ap.ImportFromModel(tap);
                     

                }
                else
                {
                    tap = new  GeneralSettings();
                   
                }

                ap.ImportFromModel(tap);
                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }

        }
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
  
        public async Task Edit(GeneralSettings genset)
        {
            try
            {
                if( this.Exists())
                {

                    await this.ClearSettingsTable();

                }
                else
                {

                    db.Settings.Add(genset);
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
               // return null;
            }
        }
        public async Task ClearSettingsTable()
        {
            try
            {
                if (this.Exists())
                {

                    foreach (GeneralSettings g in db.Settings.ToList())
                    {
                        db.Settings.Remove(g);
                    }
                    db.SaveChanges();

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
