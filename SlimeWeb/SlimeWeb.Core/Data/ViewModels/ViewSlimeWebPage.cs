﻿using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Managers;
using SlimeWeb.Core.Managers.Interfaces;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Data.ViewModels
{
    public class ViewSlimeWebPage:SlimeWebPage
    {
        SlimeWebPageManager slimeWebPageManager = new SlimeWebPageManager();
        [DataType(DataType.Html)]
        public string HTMLcontent { get; set; }
        public ApplicationUser Author { get; set; }

        public void ImportFromModel(SlimeWebPage model)
        {
            try
            {
                this.Author = IDataManager.db.Users.First(x => x.UserName == model.Author);


                // this.Categories = model.Categories;

                this.Id = model.Id;
                this.Published = model.Published;
                //this.RowVersion = model.RowVersion;
                //this.Tags = model.Tags;
                this.Title = model.Title;
                this.content = model.content;
                this.Name = model.Name;
                this.TopPosition = model.TopPosition;
                this.BottomPosition = model.BottomPosition;
            }

            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                 
            }

        }
        public SlimeWebPage ToModel(string username)
        {
            try
            {
                SlimeWebPage ap = new SlimeWebPage();
                if (CommonTools.isEmpty(username))
                {
                    return null; ;
                }
                if (this.Author == null)
                {
                    Author = IDataManager.db.Users.First(x => x.UserName == username);
                }
                ap.Author = this.Author.UserName;


                ap.Id = Id;
                ap.Published = Published;
               // ap.RowVersion = RowVersion;
                ap.Title = Title;
                ap.content = content;
                ap.Name = Name;
                ap.TopPosition = this.TopPosition;
                ap.BottomPosition = this.BottomPosition;


                return ap;
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return null;
            }
        }
    }
}
