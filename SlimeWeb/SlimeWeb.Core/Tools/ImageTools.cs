using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SlimeWeb.Core.Data.NonDataModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Image = SixLabors.ImageSharp.Image;

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
                        var exif = image.Metadata.ExifProfile.Values;
                        if (exif != null)
                        {
                            ap = new ExifModel();

                            var exposuretime = exif.FirstOrDefault(x => x.Tag == ExifTag.ExposureTime).GetValue();
                           ap.Exposure_Time = this.RationalToString((IExifValue<Rational>)exposuretime);
                            ap.F_number = this.RationalToString((IExifValue<Rational>)exif.FirstOrDefault(x => x.Tag == ExifTag.FNumber).GetValue());

                            if (exif.FirstOrDefault(x => x.Tag == ExifTag.ExposureProgram).GetValue() != null)
                            {
                                var exposureprogram = (IExifValue<ushort>)exif.FirstOrDefault(x => x.Tag == ExifTag.ExposureProgram).GetValue();
                                ap.Exposure_Program = exposureprogram.Value;
                            }
                            if (exif.FirstOrDefault(x => x.Tag == ExifTag.Model).GetValue() != null)
                            {
                                var model = (ExifTag<string>)exif.FirstOrDefault(x => x.Tag == ExifTag.Model).GetValue();
                                ap.Model = model.ToString();
                            }
                            ap.Orientation = Convert.ToString(((IExifValue<ushort>)exif.FirstOrDefault(x => x.Tag == ExifTag.Orientation)
                                .GetValue()).Value);
                            ap.Resolution_Unit = Convert.ToString(((IExifValue<ushort>)exif.FirstOrDefault(x => x.Tag == ExifTag.ResolutionUnit)
                                .GetValue()));
                            ap.X_resolution = RationalToDouble((IExifValue<Rational>)exif.FirstOrDefault(x => x.Tag == ExifTag.XResolution)
                                .GetValue());
                            ap.Y_resolution = RationalToDouble((IExifValue<Rational>)exif.FirstOrDefault(x => x.Tag == ExifTag.YResolution)
                                .GetValue());
                            ap.YCbCr_Positioning = this.RationalToString(((IExifValue<ushort>)exif.FirstOrDefault(x => x.Tag == ExifTag.YCbCrPositioning)
                                .GetValue()));
                           ap.DateTaken = CheckifStringValueisNull((IExifValue<string>)exif.FirstOrDefault(x => x.Tag == ExifTag.DateTimeOriginal)
                               .GetValue());
                            ap.Brightness = this.RationalToString((IExifValue<SignedRational>)exif.FirstOrDefault(x => x.Tag == ExifTag.BrightnessValue)
                               .GetValue());
                            ap.GPSDateStamp = CheckifStringValueisNull((IExifValue<string>)exif.FirstOrDefault(x => x.Tag == ExifTag.GPSDateStamp)
                               .GetValue());
                            ap.GPSAltitude = RationalToString((IExifValue<Rational>)exif.FirstOrDefault(x => x.Tag == ExifTag.GPSAltitude)
                                .GetValue());
                            ap.GPSLongitude = RationalToString((IExifValue<Rational>)exif.FirstOrDefault(x => x.Tag == ExifTag.GPSLongitude)
                                .GetValue());
                            ap.GPSLatitude = RationalToString((IExifValue<Rational>)exif.FirstOrDefault(x => x.Tag == ExifTag.GPSLatitude)
                                .GetValue());
                            ap.ImageWidth = this.RationalToString((IExifValue<Rational>)exif.FirstOrDefault(x => x.Tag == ExifTag.ImageWidth)
                                .GetValue());
                            ap.ImageLength = this.RationalToString((IExifValue<Rational>)exif.FirstOrDefault(x => x.Tag == ExifTag.ImageLength)
                                .GetValue());






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
