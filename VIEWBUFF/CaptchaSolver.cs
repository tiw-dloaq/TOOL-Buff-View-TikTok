using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace VIEWBUFF
{
    public class CaptchaSolver
    {
        public async Task<string> SolveCaptcha(string imagePath)
        {
            // Đọc dữ liệu của ảnh vào một mảng byte
            byte[] imageData = File.ReadAllBytes(imagePath);

            // Khóa API của bạn
            string apiKey = "K81829961788957";

            // URL endpoint của API OCR.Space
            string ocrApiUrl = "https://api.ocr.space/parse/image";

            // Tạo một instance của HttpClient
            using (HttpClient client = new HttpClient())
            {
                // Tạo multipart form data
                MultipartFormDataContent form = new MultipartFormDataContent();

                // Thêm ảnh vào form data
                form.Add(new ByteArrayContent(imageData), "file", Path.GetFileName(imagePath));

                // Thêm key apikey vào header
                client.DefaultRequestHeaders.Add("apikey", apiKey);

                // Gửi yêu cầu POST đến API OCR.Space
                HttpResponseMessage response = await client.PostAsync(ocrApiUrl, form);

                // Đọc nội dung phản hồi
                string responseBody = await response.Content.ReadAsStringAsync();

                // Phân tích phản hồi JSON
                JObject jsonResponse = JObject.Parse(responseBody);

                // Lấy chuỗi "ParsedText" từ phản hồi JSON
                string parsedText = (string)jsonResponse["ParsedResults"][0]["ParsedText"];

                // Tách chuỗi thành các từ
                string[] words = parsedText.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                // Lấy từ thứ hai (index 1) trong mảng từ
                string desiredWord = words.Length > 1 ? words[1] : "";

                // Trả về từ mong muốn
                return desiredWord;
            }
        }
    }
}
