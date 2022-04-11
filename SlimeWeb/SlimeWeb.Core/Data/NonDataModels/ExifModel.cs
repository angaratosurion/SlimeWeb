using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Data.NonDataModels
{
   public class ExifModel
    {

        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Orientation { get; set; }
        public string Orientation_Rotation { get; set; }
        public double X_resolution { get; set; }
        public double Y_resolution { get; set; }
        public string Resolution_Unit { get; set; }
        public string Exposure_Time { get; set; }
        public string F_number { get; set; }
        public ushort Exposure_Program { get; set; }
        public string YCbCr_Positioning { get; set; }
        public string DateTaken { get; set; }
        public string Brightness { get; set; }
        public string GPSDateStamp { get; set; }
        public string GPSAltitude { get; set; }
        public string GPSLatitude { get; set; }
        public string GPSLongitude { get; set; }
        public string ImageWidth { get; set; }
        public string ImageLength { get; set; }
        public string Aperture { get; set; }




        /* public DateTime DateAndTime { get; set; }
        
         public string Compression { get; set; }

         */
    }
}
