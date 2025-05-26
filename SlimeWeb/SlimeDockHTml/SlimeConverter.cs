using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Office2010.Word;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
// Κάνουμε alias το Wordprocessing Paragraph για να ξεχωρίζει
using WParagraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using WBreak = DocumentFormat.OpenXml.Wordprocessing.Break;
using WBreakValues = DocumentFormat.OpenXml.Wordprocessing.BreakValues;


namespace SlimeDockHTml
{
   public  class SlimeConverter
    {

        public static string ConvertToHtml(string file, string outputDirectory)
        {
            var fi = new FileInfo(file);
            //Console.WriteLine(fi.Name);
            //byte[] byteArray = File.ReadAllBytes(fi.FullName);
            //using (MemoryStream memoryStream = new MemoryStream())
            {
                //memoryStream.Write(byteArray, 0, byteArray.Length);
                using (WordprocessingDocument wDoc = OpenWordDocument(file))///WordprocessingDocument wDoc =
                    //WordprocessingDocument.Open(memoryStream, true))
                {
                    var destFileName = new FileInfo(fi.Name.Replace(".docx", ".html"));
                    if (outputDirectory != null && outputDirectory != string.Empty)
                    {
                        DirectoryInfo di = new DirectoryInfo(outputDirectory);
                        if (!di.Exists)
                        {
                            //throw new OpenXmlPowerToolsException("Output directory does not exist");
                            di.Create();
                        }
                        destFileName = new FileInfo(System.IO.Path.Combine(di.FullName, destFileName.Name));
                    }
                    var imageDirectoryName = destFileName.FullName.Substring(0, destFileName.FullName.Length 
                        - 5) + "_files";
                    int imageCounter = 0;

                    var pageTitle = fi.FullName;
                    var part = wDoc.CoreFilePropertiesPart;
                    if (part != null)
                    {
                        pageTitle = (string)part.GetXDocument().Descendants(DC.title)
                            .FirstOrDefault() ?? fi.FullName;
                    }

                    // TODO: Determine max-width from size of content area.
                    HtmlConverterSettings settings = new HtmlConverterSettings()
                    {
                        AdditionalCss = "body { margin: 1cm auto; max-width: 20cm; padding: 0; }",
                        PageTitle = pageTitle,
                        FabricateCssClasses = true,
                        CssClassPrefix = "pt-",
                        RestrictToSupportedLanguages = false,
                        RestrictToSupportedNumberingFormats = false,
                        ImageHandler = imageInfo =>
                        {
                            DirectoryInfo localDirInfo =
                            new DirectoryInfo(imageDirectoryName);
                            if (!localDirInfo.Exists)
                                localDirInfo.Create();
                            ++imageCounter;
                            string extension = imageInfo.ContentType.Split('/')[1].
                            ToLower();
                            ImageFormat imageFormat = null;
                            if (extension == "png")
                                imageFormat = ImageFormat.Png;
                            else if (extension == "gif")
                                imageFormat = ImageFormat.Gif;
                            else if (extension == "bmp")
                                imageFormat = ImageFormat.Bmp;
                            else if (extension == "jpeg")
                                imageFormat = ImageFormat.Jpeg;
                            else if (extension == "tiff")
                            {
                                // Convert tiff to gif.
                                extension = "gif";
                                imageFormat = ImageFormat.Gif;
                            }
                            else if (extension == "x-wmf")
                            {
                                extension = "wmf";
                                imageFormat = ImageFormat.Wmf;
                            }

                            // If the image format isn't one that we expect, ignore it,
                            // and don't return markup for the link.
                            if (imageFormat == null)
                                return null;

                            //string imageFileName = imageDirectoryName + "/image" +
                            //    imageCounter.ToString() + "." + extension;
                            string imgbase64;
                            MemoryStream imgstrm = new MemoryStream();
                            try
                            {
                                // imageInfo.Bitmap.Save(imageFileName, imageFormat);
                                imageInfo.Bitmap.Save(imgstrm,imageFormat);
                              imgbase64= "data:"+imageInfo.ContentType+ ";base64, " +
                                Convert.ToBase64String(imgstrm.ToArray());


                            }
                            catch (System.Runtime.InteropServices.ExternalException)
                            {
                                return null;
                            }
                            //string imageSource = localDirInfo.Name + "/image" +
                            //    imageCounter.ToString() + "." + extension;
                            string imageSource = imgbase64;

                            XElement img = new XElement(Xhtml.img,
                                new XAttribute(NoNamespace.src, imageSource),
                                imageInfo.ImgStyleAttribute,
                                imageInfo.AltText != null ?
                                    new XAttribute(NoNamespace.alt, imageInfo.AltText) : null);
                            return img;
                        }
                    };
                    XElement htmlElement = HtmlConverter.ConvertToHtml(wDoc, settings);

                    // Produce HTML document with <!DOCTYPE html > declaration to tell the browser
                    // we are using HTML5.
                    var html = new XDocument(
                        new XDocumentType("html", null, null, null),
                        htmlElement);

                    // Note: the xhtml returned by ConvertToHtmlTransform contains objects of type
                    // XEntity.  PtOpenXmlUtil.cs define the XEntity class.  See
                    // http://blogs.msdn.com/ericwhite/archive/2010/01/21/writing-entity-references-using-linq-to-xml.aspx
                    // for detailed explanation.
                    //
                    // If you further transform the XML tree returned by ConvertToHtmlTransform, you
                    // must do it correctly, or entities will not be serialized properly.

                    var htmlString = html.ToString(SaveOptions.DisableFormatting);
                    File.WriteAllText(destFileName.FullName, htmlString, Encoding.UTF8);
                    return destFileName.FullName;
                }
            }
        }
        public static string ConvertToHtml(string file )
        {
             var fi = new FileInfo(file);
            //Console.WriteLine(fi.Name);
            //byte[] byteArray = File.ReadAllBytes(fi.FullName);
           // using (MemoryStream memoryStream = new MemoryStream())
            {
                // memoryStream.Write(byteArray, 0, byteArray.Length);
                using (WordprocessingDocument wDoc = OpenWordDocument(file)) 
                  //  WordprocessingDocument.Open(memoryStream, true))
                {
                    var destFileName = new FileInfo(fi.Name.Replace(".docx", ".html"));
                     
                    var imageDirectoryName = destFileName.FullName.Substring(0, destFileName.FullName.Length - 5) + "_files";
                    int imageCounter = 0;

                    var pageTitle = fi.FullName;
                    var part = wDoc.CoreFilePropertiesPart;
                    if (part != null)
                    {
                        pageTitle = (string)part.GetXDocument().Descendants(DC.title)
                            .FirstOrDefault() ?? fi.FullName;
                    }

                    // TODO: Determine max-width from size of content area.
                    HtmlConverterSettings settings = new HtmlConverterSettings()
                    {
                        AdditionalCss = "body { margin: 1cm auto; max-width: 20cm; padding: 0; }",
                        PageTitle = pageTitle,
                        FabricateCssClasses = true,
                        CssClassPrefix = "pt-",
                        RestrictToSupportedLanguages = false,
                        RestrictToSupportedNumberingFormats = false,
                        ImageHandler = imageInfo =>
                        {
                            DirectoryInfo localDirInfo =
                            new DirectoryInfo(imageDirectoryName);
                            if (!localDirInfo.Exists)
                                localDirInfo.Create();
                            ++imageCounter;
                            string extension = imageInfo.ContentType.Split('/')[1].
                            ToLower();
                            ImageFormat imageFormat = null;
                            if (extension == "png")
                                imageFormat = ImageFormat.Png;
                            else if (extension == "gif")
                                imageFormat = ImageFormat.Gif;
                            else if (extension == "bmp")
                                imageFormat = ImageFormat.Bmp;
                            else if (extension == "jpeg")
                                imageFormat = ImageFormat.Jpeg;
                            else if (extension == "tiff")
                            {
                                // Convert tiff to gif.
                                extension = "gif";
                                imageFormat = ImageFormat.Gif;
                            }
                            else if (extension == "x-wmf")
                            {
                                extension = "wmf";
                                imageFormat = ImageFormat.Wmf;
                            }

                            // If the image format isn't one that we expect, ignore it,
                            // and don't return markup for the link.
                            if (imageFormat == null)
                                return null;

                            //string imageFileName = imageDirectoryName + "/image" +
                            //    imageCounter.ToString() + "." + extension;
                            string imgbase64;
                            MemoryStream imgstrm = new MemoryStream();
                            try
                            {
                                // imageInfo.Bitmap.Save(imageFileName, imageFormat);
                                imageInfo.Bitmap.Save(imgstrm, imageFormat);
                                imgbase64 = "data:" + imageInfo.ContentType + ";base64, " +
                                  Convert.ToBase64String(imgstrm.ToArray());


                            }
                            catch (System.Runtime.InteropServices.ExternalException)
                            {
                                return null;
                            }
                            //string imageSource = localDirInfo.Name + "/image" +
                            //    imageCounter.ToString() + "." + extension;
                            string imageSource = imgbase64;

                            XElement img = new XElement(Xhtml.img,
                                new XAttribute(NoNamespace.src, imageSource),
                                imageInfo.ImgStyleAttribute,
                                imageInfo.AltText != null ?
                                    new XAttribute(NoNamespace.alt, imageInfo.AltText) : null);
                            return img;
                        }
                    };
                    XElement htmlElement = HtmlConverter.ConvertToHtml(wDoc, settings);

                    // Produce HTML document with <!DOCTYPE html > declaration to tell the browser
                    // we are using HTML5.
                    var html = new XDocument(
                        new XDocumentType("html", null, null, null),
                        htmlElement);

                    // Note: the xhtml returned by ConvertToHtmlTransform contains objects of type
                    // XEntity.  PtOpenXmlUtil.cs define the XEntity class.  See
                    // http://blogs.msdn.com/ericwhite/archive/2010/01/21/writing-entity-references-using-linq-to-xml.aspx
                    // for detailed explanation.
                    //
                    // If you further transform the XML tree returned by ConvertToHtmlTransform, you
                    // must do it correctly, or entities will not be serialized properly.

                    var htmlString = html.ToString(SaveOptions.DisableFormatting);
                   // File.WriteAllText(destFileName.FullName, htmlString, Encoding.UTF8);
                    return htmlString;
                }
            }
        }
        public static Properties GetExtendedFileProperties( string file)
        {
            Properties properties =null;
            var document = OpenWordDocument(file);
            if (document != null )
            {
                if (document.ExtendedFilePropertiesPart is null)
                {
                    throw new ArgumentNullException("ExtendedFilePropertiesPart is null.");
                }

                var props = document.ExtendedFilePropertiesPart.Properties;
                 
               
                properties = props;
            }

            return properties;
             
        }
        public static PackageProperties GetPackagePropertiesProperties(string file)
        {
            PackageProperties properties = null; ;
            var document = OpenWordDocument(file);
            if (document != null)
            {
                if (document.PackageProperties is null)
                {
                    throw new ArgumentNullException("PackageProperties is null.");
                }

                var props = document.PackageProperties;
               
                properties = props;
            }

            return properties;

        }

        public static WordprocessingDocument OpenWordDocument(string file)
        {
            WordprocessingDocument ap;
            var fi = new FileInfo(file);
            Console.WriteLine(fi.Name);
            byte[] byteArray = File.ReadAllBytes(fi.FullName);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.Write(byteArray, 0, byteArray.Length);

                WordprocessingDocument wDoc =
                    WordprocessingDocument.Open(memoryStream, true);
                ap = wDoc;

            }



                return ap;
        }
        public static List<List<WParagraph>> GetPages(string file)
        {
            var pages = new List<List<WParagraph>>();

            var doc = OpenWordDocument(file);
            var body = doc.MainDocumentPart.Document.Body;
            var paragraphs = body.Elements<WParagraph>().ToList();

            var currentPage = new List<WParagraph>();

            foreach (var para in paragraphs)
            {
                currentPage.Add(para);

                // Αν υπάρχει page break μέσα στην παράγραφο, κόβουμε σελίδα
                if (para.Descendants<WBreak>().Any(b => b.Type == WBreakValues.Page))
                {
                    pages.Add(currentPage);
                    currentPage = new List<WParagraph>();
                }
            }

            if (currentPage.Count > 0)
                pages.Add(currentPage);


            return pages;
        }
        public static List<WParagraph> GetPage(string file,int pagenum)
        {
            var page = new List<WParagraph>();
            var pages = GetPages(file);
            page = pages[pagenum];
            
            


            return page;
        }


    }
    }

