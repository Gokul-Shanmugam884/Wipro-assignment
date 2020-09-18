using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading;
using java.awt;
using java.awt.datatransfer;
using java.awt.@event;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace demo.opencart
{
    [TestClass]
    public class UnitTest1
    {
        static IWebDriver driver;
        static components reuse;
        static WebDriverWait wait;
        [ClassInitialize]
        public static void setup(TestContext context)
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            reuse = new components(driver);
            wait = new WebDriverWait(driver, TimeSpan.FromMinutes(1));
        }

        [TestInitialize]
        public void Init()
        {
            driver.Url = reuse.readConfig("url");
        }


        [ClassCleanup]
        public static void clean()
        {
            driver.Close();
        }

        [TestMethod]
        public void TC01_Registration()
        {
            driver.FindElement(By.XPath("//span[@class='caret']")).Click();
            driver.FindElement(By.XPath("//a[contains(text(),'Register')]")).Click();
            Dictionary<string, string> details = reuse.GetRegDetails(5);
            driver.FindElement(By.Id("input-firstname")).SendKeys(details["firstname"]);
            driver.FindElement(By.Id("input-lastname")).SendKeys(details["lastname"]);
            driver.FindElement(By.Id("input-email")).SendKeys(details["email"]);
            driver.FindElement(By.Id("input-telephone")).SendKeys(details["phone"]);
            driver.FindElement(By.Id("input-password")).SendKeys(details["password"]);
            driver.FindElement(By.Id("input-confirm")).SendKeys(details["confirmpassword"]);
            reuse.writeResult("Checkbox clicked : " + driver.FindElement(By.Name("agree")).Selected);
            driver.FindElement(By.Name("agree")).Click();
            driver.FindElement(By.XPath("//input[@type='submit']")).Click();
            reuse.writeResult("Register : " + driver.FindElement(By.XPath("//div[@id='content']/h1")).Text);
            reuse.takeScreenshot("Registration");
            reuse.logout();
            reuse.writeResult("Logout : " + driver.FindElement(By.XPath("//div[@id='content']")).Text);
        }

        [TestMethod]
        public void TC02_Login_and_Edit_Account_information()
        {
            Dictionary<string, string> details = reuse.GetLogDetails(2);
            driver.FindElement(By.XPath("//span[@class='caret']")).Click();
            driver.FindElement(By.XPath("//a[contains(text(),'Login')]")).Click();
            driver.FindElement(By.Id("input-email")).SendKeys(details["email"]);
            driver.FindElement(By.Id("input-password")).SendKeys(details["password"]);
            driver.FindElement(By.XPath("//input[@type='submit']")).Click();
            reuse.takeScreenshot("Login");
            driver.FindElement(By.XPath("//a[contains(text(),'Edit your account')]")).Click();
            driver.FindElement(By.Id("input-telephone")).Clear();
            driver.FindElement(By.Id("input-telephone")).SendKeys("986532147");
            driver.FindElement(By.XPath("//input[@type='submit']")).Click();
            reuse.takeScreenshot("Telephone update");
            reuse.logout();
            reuse.writeResult("Logout : " + driver.FindElement(By.XPath("//div[@id='content']")).Text);
        }

        [TestMethod]
        public void TC03_Add_product_to_cart_and_make_payment()
        {
            Dictionary<string, string> details = reuse.GetLogDetails(2);
            driver.FindElement(By.XPath("//span[@class='caret']")).Click();
            driver.FindElement(By.XPath("//a[contains(text(),'Login')]")).Click();
            driver.FindElement(By.Id("input-email")).SendKeys(details["email"]);
            driver.FindElement(By.Id("input-password")).SendKeys(details["password"]);
            driver.FindElement(By.XPath("//input[@type='submit']")).Click();
            reuse.takeScreenshot("Login");
            driver.FindElement(By.XPath("(//a[contains(text(),'Components')])[1]")).Click();
            driver.FindElement(By.XPath("//a[contains(text(),'Monitors')]")).Click();
            reuse.takeScreenshot("Monitors");
            string name = driver.FindElement(By.XPath("(//div[@class='product-thumb'])[1]/div[2]/div[1]/h4")).Text;
            string prize = driver.FindElement(By.XPath("(//div[@class='product-thumb'])[1]/div[2]/div[1]/p[2]/span[1]")).Text;
            reuse.writeOutput(name + " : " + prize);
            driver.FindElement(By.XPath("(//div[@class='product-thumb'])[1]/div[2]/div[2]/button[1]")).Click();
            Dictionary<string, string> Proddetails = reuse.GetProdDetails(1);
            if (Proddetails["Check box 3"] == "Yes")
                driver.FindElement(By.XPath("//div[@id='input-option223']/div[1]/label/input")).Click();
            if (Proddetails["Check box 4"] == "Yes")
                driver.FindElement(By.XPath("//div[@id='input-option223']/div[2]/label/input")).Click();
            driver.FindElement(By.Id("input-option208")).Clear();
            driver.FindElement(By.Id("input-option208")).SendKeys(Proddetails["Text"]);

            driver.FindElement(By.Id("input-option217")).Click();
            driver.FindElement(By.XPath("//option[contains(text(),'" + Proddetails["Color"] + "')]")).Click();

            driver.FindElement(By.Id("input-option209")).SendKeys(Proddetails["Text area"]);
            driver.FindElement(By.Id("button-upload222")).Click();

            StringSelection strSelection = new StringSelection(Proddetails["File"]);
            Clipboard clipboard = Toolkit.getDefaultToolkit().getSystemClipboard();
            clipboard.setContents(strSelection, null);

            Robot robot = new Robot();

            robot.delay(300);

            robot.keyPress(KeyEvent.VK_CONTROL);
            robot.keyPress(KeyEvent.VK_V);
            robot.keyRelease(KeyEvent.VK_V);
            robot.keyRelease(KeyEvent.VK_CONTROL);

            Thread.Sleep(2000);

            robot.keyPress(KeyEvent.VK_TAB);
            robot.keyRelease(KeyEvent.VK_TAB);
            robot.keyPress(KeyEvent.VK_TAB);
            robot.keyRelease(KeyEvent.VK_TAB);

            robot.keyPress(KeyEvent.VK_SPACE);
            robot.keyRelease(KeyEvent.VK_SPACE);
            reuse.waitUntilElementLocated("//div[@class='text-danger']");


            driver.FindElement(By.Id("input-option219")).Clear();
            driver.FindElement(By.Id("input-option219")).SendKeys(Proddetails["Date"]);
            driver.FindElement(By.Id("input-option221")).Clear();
            driver.FindElement(By.Id("input-option221")).SendKeys(Proddetails["Time"]);
            driver.FindElement(By.Id("input-option220")).Clear();
            driver.FindElement(By.Id("input-option220")).SendKeys(Proddetails["Date & Time"]);
            driver.FindElement(By.Id("input-quantity")).Clear();
            driver.FindElement(By.Id("input-quantity")).SendKeys(Proddetails["Qty"]);
            driver.FindElement(By.Id("button-cart")).Click();

            //Further feature not working in application
            reuse.logout();
            reuse.writeResult("Logout : " + driver.FindElement(By.XPath("//div[@id='content']")).Text);
        }

        [TestMethod]
        public void TC04_Validate_menus_and_count_links()
        {
            Dictionary<string, string> details = reuse.GetLogDetails(2);
            driver.FindElement(By.XPath("//span[@class='caret']")).Click();
            driver.FindElement(By.XPath("//a[contains(text(),'Login')]")).Click();
            driver.FindElement(By.Id("input-email")).SendKeys(details["email"]);
            driver.FindElement(By.Id("input-password")).SendKeys(details["password"]);
            driver.FindElement(By.XPath("//input[@type='submit']")).Click();
            reuse.takeScreenshot("Login");

            reuse.writeResult("Number of menu links = " + driver.FindElements(By.XPath("//ul[@class='nav navbar-nav']/li")).Count);

            IList<IWebElement> menu = driver.FindElements(By.XPath("//ul[@class='nav navbar-nav']/li/a"));
            List<string> menuitems = new List<String>();
            foreach (IWebElement elem in menu)
            {
                menuitems.Add(elem.Text);
                Console.WriteLine( elem.Text);
            }
            foreach(string itm in menuitems)
            {
                if(driver.FindElements(By.XPath("//ul[@class='nav navbar-nav']/li/a[contains(text(),'" + itm + "')]")).Count>0)
                    driver.FindElement(By.XPath("//ul[@class='nav navbar-nav']/li/a[contains(text(),'"+itm+"')]")).Click();
                //  if (driver.FindElements(By.XPath("//ul[@class='nav navbar-nav']/li/a[contains(text(),'" + itm + "')]//parent::li/div/a[contains(text(),'Show All')]")).Count > 0)
                try
                {

                    driver.FindElement(By.XPath("//ul[@class='nav navbar-nav']/li/a[contains(text(),'" + itm + "')]//parent::li/div/a[contains(text(),'Show All')]")).Click();
                }
                catch(Exception e)
                {

                }
                    reuse.takeScreenshot("Menu_"+itm);
            }
            
            reuse.logout();
            reuse.writeResult("Logout : " + driver.FindElement(By.XPath("//div[@id='content']")).Text);

        }

        [TestMethod]
        public void TC05_Navigate_through_Brands()
        {

            Dictionary<string, string> details = reuse.GetLogDetails(2);
            driver.FindElement(By.XPath("//span[@class='caret']")).Click();
            driver.FindElement(By.XPath("//a[contains(text(),'Login')]")).Click();
            driver.FindElement(By.Id("input-email")).SendKeys(details["email"]);
            driver.FindElement(By.Id("input-password")).SendKeys(details["password"]);
            driver.FindElement(By.XPath("//input[@type='submit']")).Click();
            reuse.takeScreenshot("Login");
            driver.FindElement(By.LinkText("Brands")).Click();
            IList<IWebElement> brands = driver.FindElements(By.XPath("//div[@id='content']/div/div/a"));
            string brand;
            for(int i = 0; i < brands.Count; i++)
            {
                brand = brands[i].Text;
                brands[i].Click();
                reuse.takeScreenshot("Brand_"+brand);
                driver.FindElement(By.LinkText("Brands")).Click();
                brands = driver.FindElements(By.XPath("//div[@id='content']/div/div/a"));
            }

            reuse.logout();
            reuse.writeResult("Logout : " + driver.FindElement(By.XPath("//div[@id='content']")).Text);
        }
        }
}
