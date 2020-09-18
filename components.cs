using GemBox.Spreadsheet;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;


namespace demo.opencart
{
    class components
    {

        IWebDriver driver;
        WebDriverWait wait;
        public components(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromMinutes(1));
        }

     
       public string readConfig(string key)
        {
            StreamReader sr = new StreamReader("C:\\Users\\Ashok\\source\\repos\\demo.opencart C#\\demo.opencart\\config.properties");

            string word = "";
            
            while ((word=sr.ReadLine()) != null)
            {
                string Key = word.Split('=')[0];
                string value = word.Split('=')[1];
                if (Key == key)
                    return value;
            }
            sr.Close();
            return "";
        }

        public void LogOut()
        {
            driver.FindElement(By.XPath("//span[@class='caret']")).Click();
            driver.FindElement(By.XPath("(//a[contains(text(),'Logout')])[1]")).Click();
        }

        public Dictionary<string, string>  GetRegDetails(int id)
        {
            Dictionary<string, string> hash = new Dictionary<string, string>();
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            var workbook = ExcelFile.Load(@"C:\Users\Ashok\source\repos\demo.opencart C#\demo.opencart\Register.xlsx");
            var worksheet = workbook.Worksheets[0];
            var cell = worksheet.Cells[0, 0];
            for(int i = 1; i < worksheet.Rows.Count; i++)
            {
                Console.WriteLine(worksheet.Cells[i, 0].Value);
                if (worksheet.Cells[i, 0].Value.Equals(id))
                {
                    hash.Add("firstname", worksheet.Cells[i, 1].Value.ToString());
                    hash.Add("lastname", worksheet.Cells[i, 2].Value.ToString());
                    hash.Add("email", worksheet.Cells[i, 3].Value.ToString());
                    hash.Add("phone", worksheet.Cells[i, 4].Value.ToString());
                    hash.Add("password", worksheet.Cells[i, 5].Value.ToString());
                    hash.Add("confirmpassword", worksheet.Cells[i, 6].Value.ToString());
                    break;
                }
            }
         
            return hash;

        }

        public Dictionary<string, string> GetLogDetails(int id)
        {
            Dictionary<string, string> hash = new Dictionary<string, string>();
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            var workbook = ExcelFile.Load(@"C:\Users\Ashok\source\repos\demo.opencart C#\demo.opencart\Login.xlsx");
            var worksheet = workbook.Worksheets[0];
            var cell = worksheet.Cells[0, 0];
            for (int i = 1; i < worksheet.Rows.Count; i++)
            {
                Console.WriteLine(worksheet.Cells[i, 0].Value);
                if (worksheet.Cells[i, 0].Value.Equals(id))
                {
                    hash.Add("email", worksheet.Cells[i, 1].Value.ToString());
                    hash.Add("password", worksheet.Cells[i, 2].Value.ToString());
                    break;
                }
            }

            return hash;

        }

        public Dictionary<string, string> GetProdDetails(int id)
        {
            Dictionary<string, string> hash = new Dictionary<string, string>();
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            var workbook = ExcelFile.Load(@"C:\Users\Ashok\source\repos\demo.opencart C#\demo.opencart\Product details.xlsx");
            var worksheet = workbook.Worksheets[0];
            var cell = worksheet.Cells[0, 0];
            for (int i = 1; i < worksheet.Rows.Count; i++)
            {
                Console.WriteLine(worksheet.Cells[i, 0].Value);
                if (worksheet.Cells[i, 0].Value.Equals(id))
                {
                    for(int j=1;j<11;j++)
                        hash.Add(worksheet.Cells[0, j].Value.ToString(), worksheet.Cells[i, j].Value.ToString());
                    break;
                }
            }

            return hash;

        }

        public void writeResult(String text)
        {
            StreamWriter sr = new StreamWriter("C:\\Users\\Ashok\\source\\repos\\demo.opencart C#\\demo.opencart\\Result.txt",true);
            sr.WriteLine(text);
            sr.Close();
        }

        public void writeOutput(String text)
        {
            StreamWriter sr = new StreamWriter("C:\\Users\\Ashok\\source\\repos\\demo.opencart C#\\demo.opencart\\output data.txt", true);
            sr.WriteLine(text);
            sr.Close();
        }

        public void takeScreenshot(string name)
        {

            ITakesScreenshot screenshotDriver = driver as ITakesScreenshot;

            Screenshot screenshot = screenshotDriver.GetScreenshot();

            screenshot.SaveAsFile("C:\\Users\\Ashok\\source\\repos\\demo.opencart C#\\demo.opencart\\Screenshot\\"+ DateTime.Now.ToString("MMddyyyy-HHmm") +"_"+ name+".jpeg", ScreenshotImageFormat.Jpeg);
        }

       

        public void AddToCartRibbonClose()
        {
            Func<IWebDriver, bool> waitme = new Func<IWebDriver, bool>((IWebDriver driver) => {
                //div[@class="alert alert-success alert-dismissible"]
                if (driver.FindElement(By.XPath("//div[@class='alert alert-success alert-dismissible']")).Text.Contains("Success"))
                    return true;
                return false;
            });
            wait.Until(waitme);
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript("arguments[0].scrollIntoView()", driver.FindElement(By.XPath("//button[@class='close']")));
            Thread.Sleep(500);
            driver.FindElement(By.XPath("//button[@class='close']")).Click();
            
        }

        public void logout()
        {
            driver.FindElement(By.XPath("//div[@id='top-links']/ul/li[2]/a")).Click();
            driver.FindElement(By.LinkText("Logout")).Click();
        }

     

        public void waitUntilElementLocated(string xpath)
        {
            Func<IWebDriver, bool> waitme = new Func<IWebDriver, bool>((IWebDriver driver) => {
                if (driver.FindElements(By.XPath(xpath)).Count > 0)
                    return true;
                return false;
            });
            wait.Until(waitme);
        }

        public void waitUntilClickable(string xpath)
        {
            Func<IWebDriver, bool> waitme = new Func<IWebDriver, bool>((IWebDriver driver) => {
                if (driver.FindElement(By.XPath(xpath)).Enabled && driver.FindElement(By.XPath(xpath)).Displayed)
                    return true;
                return false;
            });
            wait.Until(waitme);
        }

        public void scrollIntoView(string xpath)
        {
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript("arguments[0].scrollIntoView()", driver.FindElement(By.XPath(xpath)));
        }

        public void waitUntilTextLocated(string xpath,string text)
        {
            Func<IWebDriver, bool> waitme = new Func<IWebDriver, bool>((IWebDriver driver) => {
                if (driver.FindElement(By.XPath(xpath)).Text.Contains(text))
                    return true;
                return false;
            });
            wait.Until(waitme);
        }

       

     

    }
}
