using Microsoft.EntityFrameworkCore;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Managers.Interfaces;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Managers
{
   public class SlimeWebPageManager:DataManager, ISlimeWebPageManager<SlimeWebPage>
    {
        BlogManager blmngr;
        public  SlimeWebPageManager()
        {
            blmngr = new BlogManager();
        }
        public async Task<List<SlimeWebPage>> List()
        {
            try
            {
                List<SlimeWebPage> ap = null;

                ap = await db.Pages.ToListAsync();
                return ap;

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return null;
            }

        }
      
        public async Task<List<SlimeWebPage>> ListByPublished()
        {
            try
            {
                List<SlimeWebPage> ap = null, SlimeWebPages;
                Blog blog = null;
              
                    ap = new List<SlimeWebPage>();
                    
                       

                        SlimeWebPages = (await this.List());
                        SlimeWebPages = SlimeWebPages.OrderByDescending(x => x.Published).ToList();
                        if (SlimeWebPages != null)
                        {
                            ap = SlimeWebPages;
                        }
                  


               

                return ap;

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return null;
            }

        }
        public async Task<List<SlimeWebPage>> ListByPublished(
            int page, int pagesize)
        {
            try
            {
                List<SlimeWebPage> ap = null, SlimeWebPages = null, tSlimeWebPages;
                Blog blog = null;
             
                    SlimeWebPages = await this.ListByPublished();

               

                if (pagesize > 0 && page > 0 && SlimeWebPages.Count > pagesize)
                {


                    if (SlimeWebPages != null)
                    {
                        ap = SlimeWebPages.Skip(page * pagesize).Take(pagesize).ToList();

                    }


                }
                else //if (pagesize <= 0)
                {
                    ap = await this.ListByPublished();
                }
                return ap;

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return null;
            }

        }
        public async Task<SlimeWebPage> Details(String name)
        {
            try
            {
                 SlimeWebPage ap = null;

                var pages = await this.List();
                if(pages != null) 
                {
                    ap=pages.FirstOrDefault(x=>x.Name== name);

                
                }

                
                return ap;

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return null;
            }
        }
        public async Task<SlimeWebPage> Details(int id)
        {
            try
            {
                SlimeWebPage ap = null;

                var pages = await this.List();
                if (pages != null)
                {
                    ap = pages.FirstOrDefault(x => x.Id == id);


                }


                return ap;

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return null;
            }
        }
        public async Task<bool> Exists(String name)
        {
            try
            {
                bool ap = false;
                var page = await this.Details(name);
                if(page != null)
                {
                    ap = true;
                }

                return ap;
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return false;
            }
        }
        public async Task<SlimeWebPage> Create(SlimeWebPage page, string user)
        {
            try
            {
                SlimeWebPage ap = null;

                if (page != null && user != null )
                {
                    if (!await this.Exists(page.Name))
                    {
                        ApplicationUser usr = (ApplicationUser)db.Users.First(m => m.UserName == user);
                        if (usr != null)
                        {
                            page.Author = usr.UserName;
                            await db.Pages.AddAsync(page);
                            db.SaveChanges();
                            ap = page;
                        }
                    }
                }
                return ap;

            }
            catch (Exception ex) {
                
                
                CommonTools.ErrorReporting(ex);
                return null;
            }

        }
        public async Task<SlimeWebPage> Edit(string name ,SlimeWebPage page)
        {
            try
            {
                SlimeWebPage vpage = null;
                if (page != null  )
                {
                    if (await this.Exists(name))
                    {
                        vpage = await this.Details(name);
                    }
                    if (vpage != null)
                    {


                        db.Entry(vpage).State = EntityState.Modified;

                        db.Entry(vpage).CurrentValues.SetValues(page);
                        // db.SlimeWebPage.Update(SlimeWebPage);
                        await db.SaveChangesAsync();
                    }
                }
                return page;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    if (entry.Entity is SlimeWebPage)
                    {
                        var proposedValues = entry.CurrentValues;
                        var databaseValues = entry.GetDatabaseValues();

                        foreach (var property in proposedValues.Properties)
                        {
                            var proposedValue = proposedValues[property];
                            var databaseValue = databaseValues[property];

                            // TODO: decide which value should be written to database
                            proposedValues[property] = proposedValue;
                        }

                        // Refresh original values to bypass next concurrency check
                        // entry.OriginalValues.SetValues(databaseValues);
                        entry.OriginalValues.SetValues(proposedValues);
                    }
                    else
                    {
                        throw new NotSupportedException(
                            "Don't know how to handle concurrency conflicts for "
                            + entry.Metadata.Name);
                    }
                }
                return null;

            }
            catch (Exception ex) { 
                CommonTools.ErrorReporting(ex); return null;
            }


        }
        public async Task Delete(string name)
        {
            try
            {
                if (name != null)
                {
                    SlimeWebPage page = await this.Details(name);
                    FileRecordManager fileRecordManager = new FileRecordManager();

                    bool deleted = await fileRecordManager.DeleteFromPages((int)page.Id);
                    // bool SlimeWebPagehasfiles = await fileRecordManager.SlimeWebPageHasFiles((int)id);
                    bool SlimeWebPagehasfiles = true;
                    if (deleted && SlimeWebPagehasfiles)
                    {
                        db.Pages.Remove(page);
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Pages.Remove(page);
                        db.SaveChanges();
                    }
                }

            }
            catch (Exception ex) { 
                CommonTools.ErrorReporting(ex); 
            }

        }

    }
}
