using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

public class RecaptchaValidator
{
    private static readonly string SecretKey =
        System.Configuration.ConfigurationManager.AppSettings["RecaptchaSecretKey"];

    public static bool IsValidWithDebug(string recaptchaResponse, out string debugInfo) {
        if (string.IsNullOrEmpty(recaptchaResponse)) {
            debugInfo = "Token ว่างเปล่า - form ไม่ได้ส่ง g-recaptcha-response มา";
            return false;
        }

        try {
            // ========== แก้ปัญหา TLS 1.2 ==========
            // .NET Framework 4.5 default ใช้ TLS 1.0 ซึ่ง Google ไม่รองรับแล้ว
            // ต้องบังคับ TLS 1.2 ก่อนเรียก HTTPS request ทุกครั้ง
            System.Net.ServicePointManager.SecurityProtocol =
                (System.Net.SecurityProtocolType)3072; // = Tls12 (ค่า enum แบบตัวเลข เผื่อ enum Tls12 ไม่มีใน .NET 4.5)
            // =======================================

            using (var client = new System.Net.WebClient()) {
                var values = new System.Collections.Specialized.NameValueCollection {
                    { "secret", SecretKey ?? "" },
                    { "response", recaptchaResponse }
                };

                var result = client.UploadValues(
                    "https://www.google.com/recaptcha/api/siteverify", values);

                var jsonResponse = System.Text.Encoding.UTF8.GetString(result);
                var obj = Newtonsoft.Json.Linq.JObject.Parse(jsonResponse);
                bool success = obj["success"] != null && (bool)obj["success"];

                debugInfo = string.Format("SecretKeySet={0} | GoogleResponse={1}",
                    !string.IsNullOrEmpty(SecretKey), jsonResponse);
                return success;
            }
        }
        catch (Exception ex) {
            debugInfo = string.Format("Exception เรียก Google API ไม่ได้: {0} | InnerException: {1}",
                ex.Message, ex.InnerException != null ? ex.InnerException.Message : "none");
            return false;
        }
    }
}