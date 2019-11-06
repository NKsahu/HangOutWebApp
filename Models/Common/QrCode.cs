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
            var QCwriter = new BarcodeWriter();
            QCwriter.Format = BarcodeFormat.QR_CODE;
            var result = QCwriter.Write(QrId.ToString());
            string path =HttpContext.Current.Server.MapPath("~/QRImgTorR/"+QrId.ToString()+".jpg");
            var barcodeBitmap = new Bitmap(result);
            using (var memoryStream = new MemoryStream())
            {
                
            }

            

        

            return false;
        }

   

}
}