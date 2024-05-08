using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;



namespace VIEWBUFF
{
    public partial class Form1 : Form
    {
  
        private ChromeDriver driver;


        public Form1()
        {
            InitializeComponent();
            
        }

        private async void btnBuffView_Click(object sender, EventArgs e)
        {

            // Các hoạt động khác của bạn ở đây

            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            try
            {
                ChromeOptions options = new ChromeOptions();
            options.AddArgument("--window-size=300,400");
            // Thiết lập kích thước cửa sổ trình duyệt

            // Add extension vào ChromeOptions
            options.AddExtension(@"C:\Users\lavan\Desktop\KTEAM\VIEWBUFF\VIEWBUFF\AdBlock Max.crx");


                // Khởi tạo trình điều khiển Chrome với các tùy chọn đã thiết lập
                driver = new ChromeDriver(service, options);
                driver.Url = "https://www.google.com/";

            // Tìm phần tử input để nhập liệu
            IWebElement inputElement = driver.FindElement(By.XPath("//*[@id=\"APjFqb\"]"));

            await Task.Delay(3000); // hoặc Task.Delay(5000) nếu muốn chờ 5 giây

            // Gửi dữ liệu vào phần tử input
            inputElement.SendKeys("Zefoy");

            // Gửi phím Enter
            inputElement.SendKeys(OpenQA.Selenium.Keys.Enter);


            // Mở tab mới và điều hướng đến URL https://zefoy.com/
            OpenLinkJS(driver, "https://zefoy.com/");

            // Đợi 5 giây
            await Task.Delay(10000);

            RefreshPage(driver);

           SwitchToNewTab(driver);

            ImageCapture imageCapture = new ImageCapture();
            string xpath = "/html/body/div[5]/div[2]/form/div/div/img";
            string folderPath = @"C:\Users\lavan\Desktop\KTEAM\VIEWBUFF\VIEWBUFF\CaptchaIMG"; // Đường dẫn của thư mục CaptchaIMG
            imageCapture.CaptureAndSaveImage(driver, xpath, folderPath);

            // Đường dẫn của thư mục chứa ảnh captcha
            string imagePath = @"C:\Users\lavan\Desktop\KTEAM\VIEWBUFF\VIEWBUFF\CaptchaIMG";

            // Lấy đường dẫn của ảnh captcha mới nhất trong thư mục
            string latestImagePath = GetLatestImagePath(imagePath);

            // Khởi tạo đối tượng CaptchaSolver
            CaptchaSolver solver = new CaptchaSolver();

            try
            {
                // Giải mã captcha từ ảnh
                string captchaResult = await solver.SolveCaptcha(latestImagePath);

                // Nhập kết quả captcha vào trường input trên trình duyệt
                driver.FindElement(By.XPath("/html/body/div[5]/div[2]/form/div/div/div/input")).SendKeys(captchaResult);
            }
            catch (Exception ex)
            {
                    // Xử lý lỗi nếu cần
                    Console.WriteLine("Error: " + ex.Message);
            }
            
                driver.FindElement(By.XPath("/html/body/div[5]/div[2]/form/div/div/div/div/button")).Click();
              Thread.Sleep(1000);
              driver.FindElement(By.XPath("/html/body/div[6]/div/div[2]/div/div/div[5]/div/button")).Click();
              Thread.Sleep(1000);
              driver.FindElement(By.XPath("/html/body/div[10]/div/form/div/input")).SendKeys(txtLink.Text);
                driver.FindElement(By.XPath("/html/body/div[10]/div/form/div/div/button")).Click();
                Thread.Sleep(10000);
                driver.FindElement(By.XPath("/html/body/div[10]/div/form/div/div/button")).Click();
                driver.FindElement(By.XPath("//*[@id=\"c2VuZC9mb2xeb3dlcnNfdGlrdG9V\"]/div[1]/div/form/button")).Click();
            try
            {
                while (true)
                {
                    // Đợi 82 giây
                    Thread.Sleep(82000);

                    // Thực hiện click vào phần tử đầu tiên
                    driver.FindElement(By.XPath("/html/body/div[10]/div/form/div/div/button")).Click();

                    // Đợi 4 giây
                    Thread.Sleep(4000);

                    // Thực hiện click vào phần tử thứ hai
                    driver.FindElement(By.XPath("//*[@id=\"c2VuZC9mb2xeb3dlcnNfdGlrdG9V\"]/div[1]/div/form/button")).Click();

                    }
            }
          catch (Exception ex)
            {
                // Xử lý ngoại lệ ở đây (nếu cần)
            }
                


            }
            catch (Exception ex)
            {
                // Xử lý nếu có lỗi xảy ra
               Console.WriteLine("Error: " + ex.Message);
            }              

        }
        


        static void OpenLinkJS(IWebDriver driver, string link)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript($"window.open('{link}', '_blank')");
        }

        private void RefreshPage(IWebDriver driver)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("setTimeout(function(){ location.reload(); }, 10000);");
        }

        private void SwitchToNewTab(ChromeDriver driver)
        {
            // Lấy tất cả cửa sổ (tabs) đang mở
            List<string> windowHandles = driver.WindowHandles.ToList();

            // Chuyển qua tab mới (tab cuối cùng trong danh sách)
            driver.SwitchTo().Window(windowHandles.Last());
        }
        private string GetLatestImagePath(string folderPath)
        {
            // Lấy danh sách tất cả các file trong thư mục
            string[] files = Directory.GetFiles(folderPath);

            // Sắp xếp các file theo thời gian tạo giảm dần
            Array.Sort(files, (x, y) => new FileInfo(y).CreationTime.CompareTo(new FileInfo(x).CreationTime));

            // Lấy đường dẫn của file đầu tiên (ảnh mới nhất)
            return files.FirstOrDefault();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        
    }
}
