using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Data.ViewModels;
using SlimeWeb.Core.Managers.Interfaces;
using SlimeWeb.Core.Tools;

namespace SlimeWeb.Core.Managers
{
    public class GeneralSettingsManager:  IGeneralSettingsManager<ViewGeneralSettings,
        GeneralSettings>
    {
        public ViewGeneralSettings Details()
        {
            try
            {
                ViewGeneralSettings ap = new ViewGeneralSettings();
                GeneralSettings tap = null; ;
                if (this.Exists())
                {
                    tap =  IDataManager.db.GeneralSettings;
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
                if ( IDataManager.db.GeneralSettings == null)
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
  
        public virtual async  Task Edit(GeneralSettings genset)
        {
            try
            {
                if( this.Exists())
                {

                    //await this.ClearSettingsTable();

                }
                else
                {

                     IDataManager.db.Settings.Add(genset);
                     IDataManager.db.SaveChanges();
                }

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
               // return null;
            }
        }
        public virtual async  Task ClearSettings()
        {
            try
            {
                if (this.Exists())
                {

                    foreach (GeneralSettings g in  IDataManager.db.Settings.ToList())
                    {
                         IDataManager.db.Settings.Remove(g);
                    }
                     IDataManager.db.SaveChanges();

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
