using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;


/// <summary>
/// Summary description for ImageConverter
/// </summary>
public class ImageConverter
{
    public ImageConverter()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    /// <summary>   
    /// Converts the specified image into a JPEG format   
    /// </summary>   
    /// <param name="fileName">The file path of the image to convert</param>   
    /// <returns>An Image with JPEG data if successful; otherwise null</returns>   
    public static Image[] ConvertToJpeg(string fileName)
    {
        Image[] retVal = null;
        using (FileStream fs = File.OpenRead( fileName))
        {
            retVal = ConvertToJpeg( fs);
            fs.Close();
        }

        return retVal;
    }

    /// <summary>   
    /// Converts the specified image into a JPEG format   
    /// </summary>   
    /// <param name="imgStream">The stream of the image to convert</param>   
    /// <returns>An Image with JPEG data if successful; otherwise null</returns>   
    public static Image[] ConvertToJpeg(Stream imgStream)
    {
        Image[] retVal = null;
        Image limage = null;
        Stream retStream = new MemoryStream();
        FrameDimension myDimension;
        int myPageCount = 0;

        using (Image img = Image.FromStream(imgStream, true, true))
        {
            myDimension = new FrameDimension(img.FrameDimensionsList[0]);
            myPageCount = img.GetFrameCount(myDimension);
            retVal = new Image[myPageCount];
            for (int i = 0; i < myPageCount; i++)
            {
                retStream = new MemoryStream();
                img.SelectActiveFrame(myDimension, i);
                img.Save(retStream, ImageFormat.Jpeg);
                retStream.Flush();
                limage= Image.FromStream(retStream, true, true);
                //limage.Tag = fileName;
                retVal[i] = limage;
            }
        }
        

        return retVal;
    }

}
