using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SlimeWeb.Core.Data.NonDataModels;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SlimeWeb.Core.Managers
{
    public class SlimeLogManager
    {
		public async Task<String> GetLogFileName()
		{
			try
			{
				return Path.Combine(FileSystemManager.GetAppRootDataFolderAbsolutePath(),"logs","nlog.log");

			}
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public async Task<String> GetLogDirecotry()
        {
            try
            {
                return Path.Combine(FileSystemManager.GetAppRootDataFolderAbsolutePath(), "logs");

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public async Task<String> GetLogArchiveDirecotry()
        {
            try
            {
                return Path.Combine(await this.GetLogDirecotry(), "Archives");

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public async Task<bool> LogFileExists()
        {
            try
            {
                return File.Exists(await this.GetLogFileName());

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return false;
            }
        }
            public List<ExceptionModel> List()
        {
			try
			{
				List<ExceptionModel> list = new	List<ExceptionModel>();
                string logfile =  this.GetLogFileName().Result;
                if( LogFileExists().Result && !CommonTools.isEmpty(logfile))
                {
                    FileStream fileStr = File.Open(logfile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    var fileStream = new StreamReader(fileStr);
                 
                    // var t=JsonConvert.DeserializeObject<List<ExceptionModel>>(json);
                    var jsonReader = new JsonTextReader(fileStream);
                    jsonReader.SupportMultipleContent = true;
                    while(jsonReader.Read() )
                    {

                        var jsonob= JObject.Load(jsonReader);
                         var des = JsonConvert.DeserializeObject<ExceptionModel>(jsonob.ToString());
                        list.Add(des);
                        
                    }
                    

                }




				return list;
			}
			catch (Exception ex)
			{

				CommonTools.ErrorReporting(ex);
				return null;
			}
        }

        public   List<ExceptionModel> ListByLevel(string level)
        {
            try
            {
                List<ExceptionModel> list = null; ;
                if (!CommonTools.isEmpty(level))
                {
                    List<ExceptionModel> tlist = this.List();
                    list = tlist.FindAll(x => x.Level == level);

                }


                return list;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public  ExceptionModel  Details(string timestamp)
        {
            try
            {
                 ExceptionModel  list = null; ;
                DateTime dateTime;
               if (!CommonTools.isEmpty(timestamp))
                
                {
                    
                    List<ExceptionModel> tlist = this.List();
                    list = tlist.Find (x => x.TimeStamp.ToString()== timestamp);

                }


                return list;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public async void DeleteLog()
        {
            try
            {
                string logile=await this.GetLogFileName();  
                if(await this.LogFileExists())
                {
                    File.Delete(logile);
                }
                 
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                
            }
        }
        public async void DeleteLogArchive()
        {
            try
            {
                string logile = await this.GetLogFileName();
                string logarch = await this.GetLogArchiveDirecotry();
                if (Directory.Exists(logarch))

                {
                    if(Directory.GetFiles(logarch).Length > 0)
                    {
                        foreach(var file in Directory.GetFiles(logarch))
                        {
                            File.Delete(file);
                        }
                    }
                   
                }

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);

            }
        }
    }
}

