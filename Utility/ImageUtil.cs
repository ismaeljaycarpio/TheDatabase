using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace DocGen.Utility
{
    public class ImageUtil
    {
        public static string SaveAsPNG(string imagePath, byte[] content)
        {
            string Message = "";
            try
            {
                Image img = Image.FromStream(new MemoryStream(content));
                img.Save(imagePath, ImageFormat.Png);
            }
            catch(Exception ex)
            {
                Message = ex.Message;
            }
            return Message;
        }
    }
}