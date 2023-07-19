using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace ClientDB.AppFunctions
{
    public class ImageCompresor
    {
        public static void Compressimage(Stream sourcePath, string targetPath, String filename)
        {
            try
            {
                using (var image = System.Drawing.Image.FromStream(sourcePath))                
                {
                    float maxHeight = 300.0f;
                    float maxWidth = 300.0f;
                    int newWidth;
                    int newHeight;
                    string extension;
                    Bitmap originalBMP = new Bitmap(sourcePath);
                    int originalWidth = originalBMP.Width;
                    int originalHeight = originalBMP.Height;

                    if (originalWidth > maxWidth || originalHeight > maxHeight)
                    {

                        // To preserve the aspect ratio  
                        float ratioX = (float)maxWidth / (float)originalWidth;
                        float ratioY = (float)maxHeight / (float)originalHeight;
                        float ratio = Math.Min(ratioX, ratioY);
                        newWidth = (int)(originalWidth * ratio);
                        newHeight = (int)(originalHeight * ratio);
                    }
                    else
                    {
                        newWidth = (int)originalWidth;
                        newHeight = (int)originalHeight;

                    }
                    Bitmap InitialBmp = new Bitmap(originalBMP, newWidth, newHeight);
                    Bitmap GetCroppedImg = (Bitmap)Imagetest(InitialBmp);
                    Bitmap bitMAP1 = GetCroppedImg;
                    Graphics imgGraph = Graphics.FromImage(bitMAP1);
                    extension = Path.GetExtension(targetPath);
                    if (extension == ".png" || extension == ".gif")
                    {
                        imgGraph.SmoothingMode = SmoothingMode.AntiAlias;
                        imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        //imgGraph.DrawImage(originalBMP, 0, 0, newWidth, newHeight);


                        bitMAP1.Save(targetPath, image.RawFormat);

                        bitMAP1.Dispose();
                        imgGraph.Dispose();
                        originalBMP.Dispose();
                    }
                    else if (extension == ".jpg")
                    {

                        imgGraph.SmoothingMode = SmoothingMode.AntiAlias;
                        imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                        System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                        EncoderParameters myEncoderParameters = new EncoderParameters(1);
                        EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
                        myEncoderParameters.Param[0] = myEncoderParameter;
                        bitMAP1.Save(targetPath, jpgEncoder, myEncoderParameters);

                        bitMAP1.Dispose();
                        imgGraph.Dispose();
                        originalBMP.Dispose();

                    }


                }

            }
            catch (Exception)
            {
                throw;

            }
        }

        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public static string getimgpath(HttpPostedFileBase sourceFile) 
        {
            string getTempImgPath = HttpContext.Current.Server.MapPath("~\\Images");
            string setTempImgPath = getTempImgPath + "\\TempImages\\";
            if (!Directory.Exists(setTempImgPath))
            {
                Directory.CreateDirectory(setTempImgPath);
            }
            Stream strm = new System.IO.MemoryStream();
            string result = "";
            string imgdate = "";
            string ImgPath = "";

            if (sourceFile != null)
            {
                strm = sourceFile.InputStream;
                result = Path.GetFileNameWithoutExtension(sourceFile.FileName);
                imgdate = DateTime.Now.ToString("HH:mm:ss").ToString();
                imgdate = imgdate.Replace(":", "_");
                ImgPath = setTempImgPath + result + "_min_" + imgdate + ".png";
                Compressimage(strm, ImgPath, sourceFile.FileName);
            }
            return ImgPath;
        }

        public static System.Drawing.Image Imagetest(Bitmap getimg)
        {
            Bitmap sourceImage = getimg;
            
            int SrcIMgHeight = sourceImage.Height;
            int SrcIMgWidth = sourceImage.Width;

            if (SrcIMgWidth >= SrcIMgHeight)
            {
                Rectangle cropArea = new Rectangle(((SrcIMgWidth / 2) - (SrcIMgHeight / 2)), 0, SrcIMgHeight, SrcIMgHeight);
                Bitmap targetImage = sourceImage.Clone(cropArea, sourceImage.PixelFormat);
                return targetImage;
            }
            else
            {
                Rectangle cropArea = new Rectangle(0, ((SrcIMgHeight / 2) - (SrcIMgWidth / 2)), SrcIMgWidth, SrcIMgWidth);
                Bitmap targetImage = sourceImage.Clone(cropArea, sourceImage.PixelFormat);
                return targetImage;
            }
        }
    }
}