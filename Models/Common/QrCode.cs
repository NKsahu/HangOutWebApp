using System.Drawing;
using ZXing;
using System.IO;
using System.Drawing.Imaging;
using System.Web;

namespace HangOut.Models.Common
{
    public class QrCode
    {
        public static bool GenerateQr(int QrId,string ShowLabel = "")
        {
            bool Returresult = false;
            var QCwriter = new BarcodeWriter();
            QCwriter.Format = BarcodeFormat.QR_CODE;
            var result = QCwriter.Write(QrId.ToString());
            string path =HttpContext.Current.Server.MapPath("~/QRImgTorR/"+QrId.ToString()+".jpg");
            var barcodeBitmap = new Bitmap(result);
            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(path,
                   FileMode.Create, FileAccess.ReadWrite))
                {
                    barcodeBitmap.Save(memory, ImageFormat.Jpeg);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
                Returresult = true;
            }
            return Returresult;
        }

   

}
}