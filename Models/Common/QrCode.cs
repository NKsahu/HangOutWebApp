using System.Drawing;
using ZXing;
using System.IO;
using System.Drawing.Imaging;
using System.Web;

namespace HangOut.Models.Common
{
    public class QrCode
    {
        public static string GenerateQr(string QrId,string ShowLabel = "",string Path="")
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
            RectangleF rectf = new RectangleF(100, 100, 200, 200);
            var barcodeBitmap = new Bitmap(result,400,400);
            Graphics g = Graphics.FromImage(barcodeBitmap);
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            g.DrawString(QrId.ToString(), new Font(FontFamily.GenericSerif,30), Brushes.Black, barcodeBitmap.Width/2,barcodeBitmap.Height/2,sf);
            g.Flush();
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