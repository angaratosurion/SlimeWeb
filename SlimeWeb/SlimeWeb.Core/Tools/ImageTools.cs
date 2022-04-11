using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SlimeWeb.Core.Data.NonDataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Tools
{
   public class ImageTools
    {
        public ExifModel GetMetadata(string path)
        {
            try
            {
                ExifModel ap = null; ;
                if (path != null && File.Exists(path))
                {
                    using (Image image = Image.Load(path))
                    {
                        var exif = image.Metadata.ExifProfile;
                        if (exif != null)
                        {
                            ap = new ExifModel();

                            ap.Exposure_Time = this.RationalToString((IExifValue<Rational>)exif.GetValue(ExifTag.ExposureTime));
                            ap.F_number = this.RationalToString(exif.GetValue(ExifTag.FNumber));
                            if (exif.GetValue(ExifTag.ExposureProgram) != null)
                            {
                                ap.Exposure_Program = exif.GetValue(ExifTag.ExposureProgram).Value;
                            }
                            if (exif.GetValue(ExifTag.Model) != null)
                            {
                                ap.Model = exif.GetValue(ExifTag.Model).Value;
                            }
                            ap.Orientation = this.RationalToString(exif.GetValue(ExifTag.Orientation));
                            ap.Resolution_Unit = this.RationalToString(exif.GetValue(ExifTag.ResolutionUnit));
                            ap.X_resolution = RationalToDouble(exif.GetValue(ExifTag.XResolution));
                            ap.Y_resolution = RationalToDouble(exif.GetValue(ExifTag.YResolution));
                            ap.YCbCr_Positioning = this.RationalToString(exif.GetValue(ExifTag.YCbCrPositioning));
                            ap.DateTaken = CheckifStringValueisNull(exif.GetValue(ExifTag.DateTimeOriginal));
                            ap.Brightness = this.RationalToString(exif.GetValue(ExifTag.BrightnessValue));
                            ap.GPSDateStamp = CheckifStringValueisNull(exif.GetValue(ExifTag.GPSDateStamp));
                            ap.GPSAltitude = RationalToString(exif.GetValue(ExifTag.GPSAltitude));
                            ap.GPSLongitude = RationalToArStringAR(exif.GetValue(ExifTag.GPSLongitude));
                            ap.GPSLatitude = RationalToArStringAR(exif.GetValue(ExifTag.GPSLatitude));
                            ap.ImageWidth = this.RationalToString(exif.GetValue(ExifTag.ImageWidth));
                            ap.ImageLength = this.RationalToString(exif.GetValue(ExifTag.ImageLength));






                        }
                        image.Dispose();

                    }
                }
                return ap;

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex); ; return null;
            }
        }
        private string RationalToString(int numerator, int denominator)
        {
            try
            {
                return String.Format("{0}/{1}", numerator, denominator);

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex); return "";
            }
        }
        public string RationalToString(IExifValue<Number> exif)
        {
            try
            {
                string num = "";
                if (exif != null)
                {

                    num = exif.Value.ToString();

                }
                return String.Format("{0}", num);

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex); return "";
            }
        }
        public string CheckifStringValueisNull(IExifValue<string> exif)
        {
            try
            {
                string num = "";
                if (exif != null)
                {

                    num = exif.Value;

                }
                return String.Format("{0}", num);

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex); return "";
            }
        }
        public string RationalToString(IExifValue<ushort> exif)
        {
            try
            {
                string num = "";
                if (exif != null)
                {

                    num = exif.Value.ToString();

                }
                return String.Format("{0}", num);

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex); return "";
            }
        }
        public string RationalToString(IExifValue<Rational> exif)
        {
            try
            {
                int numerator = 0, denominator = 0;
                if (exif != null)
                {
                    numerator = (int)exif.Value.Numerator;
                    denominator = (int)exif.Value.Denominator;


                }
                return String.Format("{0}/{1}", numerator, denominator);

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex); return "";
            }
        }
        public double RationalToDouble(IExifValue<Rational> exif)
        {
            try
            {
                double num = 0;
                if (exif != null)
                {
                    num = exif.Value.ToDouble();


                }
                return num;

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex); return -1;
            }
        }
        public string RationalToString(IExifValue<SignedRational> exif)
        {
            try
            {
                int numerator = 0, denominator = 0;
                if (exif != null)
                {
                    numerator = (int)exif.Value.Numerator;
                    denominator = (int)exif.Value.Denominator;


                }
                return String.Format("{0}/{1}", numerator, denominator);

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex); return "";
            }
        }
        public string RationalToArStringAR(IExifValue<Rational[]> rationals)
        {
            try
            {
                string ap = null;

                if (rationals != null)
                {


                    foreach (var r in rationals.Value)
                    {

                        ap += " " + this.RationalToString((int)r.Numerator, (int)r.Denominator);
                    }
                }


                return ap;


            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex); return null;
            }
        }


    }
}
