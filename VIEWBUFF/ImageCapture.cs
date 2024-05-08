using System;
using System.IO;
using OpenQA.Selenium;

public class ImageCapture
{
    public void CaptureAndSaveImage(IWebDriver driver, string xpath, string folderPath)
    {
        // Kiểm tra nếu thư mục lưu ảnh không tồn tại thì tạo mới
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Tạo tên file ảnh dựa trên thời gian hiện tại để tránh trùng lặp
        string fileName = $"captured_image_{DateTime.Now:yyyyMMddHHmmssfff}.png";

        // Tìm phần tử để chụp ảnh
        IWebElement element = driver.FindElement(By.XPath(xpath));

        // Chụp ảnh của phần tử
        Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();

        // Lưu ảnh vào thư mục chỉ định
        string filePath = Path.Combine(folderPath, fileName);

        using (MemoryStream memoryStream = new MemoryStream(screenshot.AsByteArray))
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                memoryStream.WriteTo(fileStream);
            }
        }
    }
}