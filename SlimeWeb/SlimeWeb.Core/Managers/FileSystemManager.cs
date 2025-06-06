﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using SlimeWeb.Core.App_Start;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Managers
{
    public class FileSystemManager
    {
        CommonTools cmtools = new CommonTools();
        //  static HttpServerUtilityBase util;
        //const string   filesdir="files",AppDataDir="App_Data";
        public const string AppDataDir = "AppData",TempDir="Temp";
       //static  IWebHostEnvironment webHostEnvironment;

        //[DllImport("kernel32.dll")]
        //static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, int dwFlags);

        //static int SYMLINK_FLAG_DIRECTORY = 1;
        //public FileSystemManager(IWebHostEnvironment twebHostEnvironment)
        //{
        //    webHostEnvironment = twebHostEnvironment;
        //}

        public FileSystemManager()
        {
        }

        #region Common
        public static string GetAppRootDataFolderAbsolutePath()
        {
            try
            {
                string ap = "";

                string pathwithextention = Path.Combine(FileSystemManager.GetAppRootBinaryFolderAbsolutePath(),
                    "wwwroot"); ;//System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                string path;//= System.IO.Path.GetDirectoryName(pathwithextention).Replace("file:\\", "");
                //path = pathwithextention.Replace("file:\\", "");
                ap = Path.Combine(pathwithextention, AppDataDir);



                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return "";
            }
        }
        public static string GetAppRootTempFolderAbsolutePath()
        {
            try
            {
                string ap = "";

                string pathwithextention = Path.Combine(FileSystemManager.
                    GetAppRootBinaryFolderAbsolutePath(), "wwwroot"); ;//System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                string path;//= System.IO.Path.GetDirectoryName(pathwithextention).Replace("file:\\", "");
                //path = pathwithextention.Replace("file:\\", "");
                ap = Path.Combine(pathwithextention, TempDir);



                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return "";
            }
        }
        public static string GetAppRootTempFolderAbsolutePath(string folder)
        {
            try
            {
                string ap = "";

                string pathwithextention = FileSystemManager.GetAppRootTempFolderAbsolutePath() ;//System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                
                ap = Path.Combine(pathwithextention, TempDir);
                CreateDirectory(ap);



                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return "";
            }
        }
        public static string GetAppRootFolderAbsolutePath()
        {
            try
            {
                string ap = "";

                string pathwithextention;
                
                    pathwithextention = Path.Combine(GetAppRootBinaryFolderAbsolutePath(), "wwwroot");               
                
            
                
                ap = pathwithextention;//.Replace("file:\\", "");
                //ap = Path.Combine(path, AppDataDir);



                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return "";
            }
        }
        public static string GetAppRootBinaryFolderAbsolutePath()
        {
            try
            {
                string ap = "";

                string pathwithextention;


                pathwithextention = AppContext.BaseDirectory;


                ;//System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                string path;//= System.IO.Path.GetDirectoryName(pathwithextention).Replace("file:\\", "");
                //ap = pathwithextention.Replace("file:\\", "");
                path = AppContext.BaseDirectory;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                  ap= pathwithextention.Replace("file:\\", "");
                }
                else
                {
                  ap= pathwithextention.Replace("file:", "");
                }



                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return "";
            }
        }
        public static string GetAppRootDataFolderRelativePath()
        {
            try
            {
                string ap = "";

                // string wkrotfold = setmngr.blogRootFolderName();
                bool isonsubfolder = AppSettingsManager.GetHostedInSubFolderSetting();
                if (isonsubfolder)
                {
                   // ap = "../../" + AppDataDir;
				   string pathbase;
					pathbase = AppSettingsManager.GetPathBase();
				   ap="."+pathbase+"/"+AppDataDir;


                }
                else
                {
                    ap = AppDataDir;
                }
                //"~/" + AppDataDir ;;




                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public static string GetBlogRootDataFolderRelativePath(string blogname)
        {
            try
            {
                string ap = "";
                if (CommonTools.isEmpty(blogname) == false)
                {
                    
                    ap =GetAppRootDataFolderRelativePath()+"/" + blogname;
                }


                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public static string GetBlogRootDataFolderAbsolutePath(string blogname)
        {
            try
            {
                string ap = "";
                if (CommonTools.isEmpty(blogname) == false)
                {

                    ap = Path.Combine(GetAppRootDataFolderAbsolutePath(), blogname);
                }


                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public static string GetPagesRootDataFolderRelativePath(string pagename)
        {
            try
            {
                string ap = "";
                if (CommonTools.isEmpty(pagename) == false)
                {

                    ap = GetAppRootDataFolderRelativePath() + "/" + pagename;
                }


                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public static string GetPagesRootDataFolderAbsolutePath(string pagename)
        {
            try
            {
                string ap = "";
                if (CommonTools.isEmpty(pagename) == false)
                {

                    ap = Path.Combine(GetAppRootDataFolderAbsolutePath(), pagename);
                }


                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        #endregion
        #region Directory
        public static Boolean DirectoryExists(String path)
        {
            try
            {
                Boolean ap = false;
                
                //path = path = Path.Combine(GetAppRootDataFolderAbsolutePath(), path);
                if (!CommonTools.isEmpty(path) && Directory.Exists(path))
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
        public static Boolean CreateDirectory(string abspath)
        {
            try
            {
                Boolean ap = false;

                if (DirectoryExists(abspath) == false)
                {
                    // string t =  Path.Combine(GetAppRootDataFolderAbsolutePath(), relpath);
                    //Directory.CreateDirectory(t);

                    Directory.CreateDirectory(abspath);
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
        public static Boolean DeleteDirectory(string relpath)
        {
            try
            {
                Boolean ap = false;
                if (!CommonTools.isEmpty(relpath) && DirectoryExists(relpath))
                {
                    string t = Path.Combine(GetAppRootDataFolderAbsolutePath(), relpath);
                    Directory.Delete(t, true);
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
        public static Boolean MoveDirectory(string relsrc, string reltrg)
        {
            try
            {
                Boolean ap = false;

                if (!CommonTools.isEmpty(relsrc) && !CommonTools.isEmpty(reltrg)
                    && DirectoryExists(relsrc))//&&  Exists(trg))
                {
                    relsrc = Path.Combine(GetAppRootDataFolderAbsolutePath(), relsrc);
                    reltrg = Path.Combine(GetAppRootDataFolderAbsolutePath(), reltrg);
                    Directory.Move(relsrc, reltrg);
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
     
        #endregion
        #region files
        //public static Boolean FileExists(String relpath)
        //{
        //    try
        //    {
        //        Boolean ap = false;
        //        String path = Path.Combine(GetAppRootFolderAbsolutePath(), relpath);

        //        if (CommonTools.isEmpty(path) != true && File.Exists(path) == true)
        //        {
        //            ap = true;
        //        }
        //        return ap;

        //    }
        //    catch (Exception ex)
        //    {
        //        CommonTools.ErrorReporting(ex);

        //        return false;
        //    }
        //}
        public static Boolean FileExists(String tpath,bool absolupath)
        {
            try
            {
                Boolean ap = false;
                String path = "";

                if (absolupath)
                {
                    path = tpath;
                }
                else
                {

                    Path.Combine(GetAppRootFolderAbsolutePath(), tpath);
                }

                if (CommonTools.isEmpty(path) != true && File.Exists(path) == true)
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
        public static async Task<Boolean> CreateFile(string relpath, List<IFormFile> files)
        {
            try
            {
                long size = files.Sum(f => f.Length);

                var filePaths = new List<string>();
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        // full path to file in temp location
                        var filePath = Path.GetTempFileName();
                        filePaths.Add(filePath);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                            await stream.FlushAsync();
                             stream.Close();
                        }
                    }
                }

                // process uploaded files
                // Don't rely on or trust the FileName property without validation.
              
                return true;
              

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);


                return false;
            }
        }
        public static async Task<string> CreateFile(string abspath, IFormFile file)
        {
            try
            {
                string ap = null;
              

                
                
                    if (file.Length > 0 && (CommonTools.isEmpty(abspath)==false))
                    {
                    // full path to file in temp location
                    var filePath = abspath;
                    string extension = Path.GetExtension(file.FileName);
                    string fileid = Guid.NewGuid().ToString();
                    fileid = Path.ChangeExtension(fileid, extension);
                    filePath = Path.Combine(abspath, fileid);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                            await stream.FlushAsync();
                            stream.Close();
                        }
                    ap = filePath;
                    }
                

                // process uploaded files
                // Don't rely on or trust the FileName property without validation.

                return ap;


            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);


                return null; ;
            }
        }
        public static Boolean DeleteFile(string relpath)
        {
            try
            {
                string path;
                Boolean ap = false;
                if (CommonTools.isEmpty(relpath) != true && FileExists(relpath,true)== true)
                {
                    path = relpath;
                    //Path.Combine(GetAppRootFolderAbsolutePath(), relpath);

                    //MapPath(relpath);
                    File.Delete(path);
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
        public static Boolean CopyFile(string relsrc, string reltrg)
        {
            try
            {
                Boolean ap = false;
                string src = relsrc, trg = reltrg;

                if (CommonTools.isEmpty(src) == false && CommonTools.isEmpty(trg) == false
                   && FileExists(src,true))//&&  Exists(trg))
                {
                    src = Path.Combine(GetAppRootDataFolderAbsolutePath(), relsrc);
                    trg = Path.Combine(GetAppRootDataFolderAbsolutePath(), reltrg);
                    File.Copy(src, trg, true);
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
        public static Boolean MoveFile(string relsrc, string reltrg)
        {
            try
            {
                Boolean ap = false;
                string src = relsrc, trg = reltrg;

                if (CommonTools.isEmpty(src) == false && CommonTools.isEmpty(trg) == false
                   && FileExists(src,true))//&&  Exists(trg))
                {
                    src = Path.Combine(GetAppRootDataFolderAbsolutePath(), relsrc);
                    trg = Path.Combine(GetAppRootDataFolderAbsolutePath(), reltrg);
                    File.Move(src, trg);
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
        public static long getFileSize(string aboultpath)
        {
            try
            {
                long ap =0;
                if( !CommonTools.isEmpty(aboultpath) && File.Exists(aboultpath)) 
                {
                
                    var info = new FileInfo(aboultpath);
                    ap = info.Length;
                
                }
                //

                return ap;
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);


                return -1;
            }

        }
        public static long getDirecorySize(string aboultpath)
        {
            try
            {
                long ap = 0;
                if (!CommonTools.isEmpty(aboultpath) && DirectoryExists(aboultpath))
                {

                    foreach(var file in Directory.GetFiles(aboultpath))
                    {
                        ap += getFileSize(file);
                    }

                }
                //

                return ap;
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);


                return -1;
            }

        }

        #endregion
    }
}
