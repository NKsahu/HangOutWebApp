using System.Drawing;
using ZXing;
using System.IO;
using System.Drawing.Imaging;
using System.Web;

namespace HangOut.Models.Common
{
    public class QrCode
    {
        public static string GenerateQr(System.Int64 QrId,string ShowLabel = "",string Path="")
        {
            string ImageName = "";
            var QCwriter = new BarcodeWriter();
            QCwriter.Format = BarcodeFormat.QR_CODE;
            var result = QCwriter.Write(QrId.ToString());
            string path =HttpContext.Current.Server.MapPath("~/"+Path+"/" + QrId.ToString()+".jpg");
            if (Directory.Exists(path))
            {
                return QrId.ToString() + ".jpg";
            }
            var barcodeBitmap = new Bitmap(result,100,100);
            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(path,
                   FileMode.Create, FileAccess.ReadWrite))
                {
                    barcodeBitmap.Save(memory, ImageFormat.Jpeg);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
                ImageName = QrId.ToString()+".jpg";
            }
            return ImageName;
        }

   

}
}