using SlimeWeb.Core.Data.NonDataModels.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Managers
{
  public class NavigationManager//:DataManager
    {
        SlimeWebPageManager _pageManager;
        static List<Menu> TopMenu = new List<Menu>();
        static List<Menu> BottomMenu = new List<Menu>();
        public NavigationManager(SlimeWebPageManager pageManager)
        {
            _pageManager = pageManager;
        }
        public Menu  FindParentInTopMenu(string name)
        {
            try
            {
                Menu ap = null;


                if( TopMenu!=null && TopMenu.Count>0)
                {

                    ap = TopMenu.FirstOrDefault(x => x.Name == name);
                }
                return ap;

            }
            catch (Exception)
            {

                throw;
            }

        }
        public void AddToTopMenu(Menu menu,string parent )
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
