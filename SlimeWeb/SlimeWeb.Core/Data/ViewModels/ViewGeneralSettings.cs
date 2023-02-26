using Microsoft.AspNetCore.Mvc.Rendering;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Data.ViewModels
{
    public class ViewGeneralSettings : GeneralSettings
    {

        public List<SelectListItem> Cells { get; } = new List<SelectListItem>
    {
        new SelectListItem { Value = "Name", Text = "Name" },
        new SelectListItem { Value = "LastUpdate", Text = "Last Updated At" },
        new SelectListItem { Value = "Created", Text = "Created  At"  },
        new SelectListItem { Value = "Administrator", Text = "Administrator"  }
    };
        public List<SelectListItem> Directions { get; } = new List<SelectListItem>
    {
        new SelectListItem { Value = "Accending", Text = "Accending" },
        new SelectListItem { Value = "Decending", Text = "Decending" },

    };
        public void ImportFromModel(GeneralSettings model)
        {
            try
            {

                if (model == null)
                {
                    this.Id = model.Id;
                    this.WebSiteName = model.WebSiteName;
                    this.ItemsPerPage = model.ItemsPerPage;
                    this.OrderBy = model.OrderBy;
                    this.Direction = model.Direction;

                }
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);


            }

        }

        public GeneralSettings ToModel()
        {
            try
            {
                GeneralSettings ap = new GeneralSettings();

                ap.Id = this.Id;
                ap.WebSiteName = this.WebSiteName;
                ap.ItemsPerPage = this.ItemsPerPage;
                ap.OrderBy = this.OrderBy;
                ap.Direction = this.Direction;


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
