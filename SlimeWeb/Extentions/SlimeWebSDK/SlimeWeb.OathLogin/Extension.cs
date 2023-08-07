

using SlimeWeb.Core.SDK.Interfaces;
using System;
using System.Reflection;

namespace SlimeWeb.OathLogin
{
    public class Extension : IExtension
    {

        string IExtension.Name => "SlimeWeb.OathLogin";

        string IExtension.Description => "It offers external login ability";

        string IExtension.Url => "www.google.gr";

       public  string Version
        {
            get {
                

                string ap;
                ap = Assembly.GetEntryAssembly().GetName().Version.ToString();

                return ap;


            } 
        
        }

        public string Authors
        {

               get{ string ap;
                    object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);

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
 
