using Microsoft.Extensions.Logging.Abstractions;
using SlimeWeb.Core.Data.NonDataModels.Navigation;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SlimeWeb.Core.Managers
{
  public static class NavigationManager//:DataManager
    {
       static SlimeWebPageManager _pageManager= new SlimeWebPageManager();
        static List<SlimeWebMenu> TopMenu = new List<SlimeWebMenu>();
        static List<SlimeWebMenu> BottomMenu = new List<SlimeWebMenu>();
     
        //public NavigationManager(SlimeWebPageManager pageManager)
        //{
        //    _pageManager = pageManager;
        //}
        public static SlimeWebMenu  FindParentInTopMenu(string name)
        {
            try
            {
                SlimeWebMenu ap = null;


                if (TopMenu != null && TopMenu.Count > 0 && !CommonTools.isEmpty(name))
                {

                    ap = TopMenu.FirstOrDefault(x => x.Name == name);
                }
                return ap;

            }
            catch (Exception ex) { CommonTools.ErrorReporting(ex);
                return null; }

        }
        public static SlimeWebMenu FindParentInBottomMenu(string name)
        {
            try
            {
                SlimeWebMenu ap = null;


                if (BottomMenu != null && BottomMenu.Count > 0 && !CommonTools.isEmpty(name))
                {

                    ap = BottomMenu.FirstOrDefault(x => x.Name == name);
                }
                return ap;

            }
            catch (Exception ex)  {CommonTools.ErrorReporting(ex); return null; }

        }
        public static void AddToTopMenu(SlimeWebMenu menu,string parent )
        {
            try
            {
                SlimeWebMenu parMenu = FindParentInTopMenu(parent);
                if (!CommonTools.isEmpty(parent))
                {
                    if (parMenu != null && menu != null)
                    {
                        TopMenu.Add(menu);
                    }
                }
                else
                {
                    if ( menu != null)
                    {
                        TopMenu.Add(menu);
                    }
                }

            }
            catch (Exception ex)  {CommonTools.ErrorReporting(ex); }
        }
        public static void AddToBottomMenu(SlimeWebMenu menu, string parent)
        {
            try
            {
                SlimeWebMenu parMenu = FindParentInBottomMenu(parent);
                if (!CommonTools.isEmpty(parent))
                {
                    if (parMenu != null)
                    {
                        BottomMenu.Add(menu);
                    }
                }
                else
                {
                    if ( menu != null)
                    {
                        BottomMenu.Add(menu);
                    }
                }
            }
            
            catch (Exception ex)  {CommonTools.ErrorReporting(ex); }
        }
        public static List<SlimeWebMenu> GetTopMenu() { 
        
                       
                return TopMenu;
            
        
        }
        public static List<SlimeWebMenu> GetBottomMenu()
        {   return BottomMenu;   }
        public static void AddDefaultMenusOnTopMenu()
        {
            try
            {
                SlimeWebMenu aboutmenu = new SlimeWebMenu();
                SlimeWebMenu AboutVersion= new SlimeWebMenu();
                SlimeWebMenu aboutsite= new SlimeWebMenu();
                SlimeWebMenu contactmenu= new SlimeWebMenu();
                contactmenu.Name = "Contact";
                contactmenu.Title = "Contact";
                contactmenu.ControllerName = "Pages";
                contactmenu.ActionName = "View";

                contactmenu.Path = "?Name=" + contactmenu.Name;

                AddToTopMenu(contactmenu, "");
                aboutmenu.Title = "About";
                aboutmenu.Name = "About";
                AboutVersion.Title = "About Software";
                AboutVersion.Name= "AboutSoftware";
                AboutVersion.ControllerName = "Home";
                AboutVersion.ActionName = "AboutSoftware";
               AddToTopMenu(aboutmenu, "");
                var menu = FindParentInTopMenu("About");
                if (menu.MenuItems == null)
                {
                    menu.MenuItems= new List<SlimeWebMenu>();
                }
                menu.MenuItems.Add(AboutVersion);
               aboutsite.Title = "About Web Site";
                aboutsite.Name = "AboutWebSite";
                aboutsite.ControllerName = "Pages";
                aboutsite.ActionName = "View";

                aboutsite.Path = "?Name=" + aboutsite.Name;


                menu.MenuItems.Add(aboutsite);
            


            }
            catch (Exception ex)  {CommonTools.ErrorReporting(ex); }

        }
        public static void AddDefaultMenusOnBottomMenu()
        {
            try
            {
              
                SlimeWebMenu contactmenu = new SlimeWebMenu();
             
                contactmenu.Name = "Contact";
                contactmenu.Title = "Contact";
                contactmenu.ControllerName = "Pages";
                contactmenu.ActionName = "View";

                contactmenu.Path = "?Name=" + contactmenu.Name;
                AddToBottomMenu(contactmenu, "");


            }
            catch (Exception ex)  {CommonTools.ErrorReporting(ex);  }

        }
        //public static void AddPagestMenusOnTopMenu()
        //{
        //    try
        //    {
        //        SlimeWebMenu aboutmenu = new SlimeWebMenu();
        //        SlimeWebMenu AboutVersion = new SlimeWebMenu();
        //        aboutmenu.Title = "About";
        //        aboutmenu.Name = "About";
        //        AboutVersion.Title = "About Software";
        //        AboutVersion.Name = "AboutSoftware";
        //        AboutVersion.ControllerName = "Home";
        //        AboutVersion.ActionName = "AboutSoftware";
        //        AddToTopMenu(aboutmenu, "");
        //        var menu = FindParentInTopMenu("About");
        //        if (menu.MenuItems == null)
        //        {
        //            menu.MenuItems = new List<SlimeWebMenu>();
        //        }
        //        menu.MenuItems.Add(AboutVersion);


        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}
    }
}

