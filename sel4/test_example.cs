using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Linq;


namespace Sel4
{
    [TestFixture]
    public class MyFirstTest
    {
        private string bowserToStart = "C";
        private IWebDriver driver;
        private WebDriverWait wait;
        FirefoxOptions options = new FirefoxOptions();

        [SetUp]
        public void start()
        {
            switch (bowserToStart)
            {
                case "C":
                    driver = new ChromeDriver();
                    break;
                case "F":
                    driver = new FirefoxDriver();
                    break;
                case "FF_ESR":
                    options.BrowserExecutableLocation = @"c:\Program Files\FF\ESR\firefox.exe";
                    driver = new FirefoxDriver(options);
                    break;
                case "FF_nightly":
                    options.BrowserExecutableLocation = @"C:\Program Files (x86)\Nightly\firefox.exe";
                    driver = new FirefoxDriver(options);
                    break;
                case "IE":
                    driver = new InternetExplorerDriver();
                    break;
               
            }

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }
      
        [Test]
        public void LoginTest()
        {
            GoToLoginPage();
            CorrectLogin("admin", "admin");

            //TODO verifications 
            //

        }


        [Test]
        public void LeftMenuTest()
        {
            GoToLoginPage();
            CorrectLogin("admin", "admin");
            var mainMenuList = driver.FindElements(By.CssSelector("ul#box-apps-menu li  a")).Select(element => element.Text).ToList();
            foreach (var item in mainMenuList)
            {
                var currentItem = driver.FindElement(By.LinkText(item));
                currentItem.Click();
                Console.WriteLine($"-{item}");

               
                var subMenu =  driver.FindElements(By.CssSelector("li[id^='doc'] a")).Select(element => element.Text).ToList();
                
                foreach (var subItem in subMenu)
                {
                    currentItem = driver.FindElement(By.LinkText(subItem));
                    currentItem.Click();
                    Console.WriteLine($"----{subItem}"); 
                }


            }
            var a = 1;
            //TODO verifications 
            //

        }

        public void GoToLoginPage()
        {
            driver.Url = "http://localhost/litecart/admin/login.php";
            wait.Until(ExpectedConditions.TitleIs("My Store"));
        }

        public void CorrectLogin(string user, string password)
        {
            var usernameInput = driver.FindElement(By.Name("username"));
            var passwordInput = driver.FindElement(By.Name("password"));
            var loginButton = driver.FindElement(By.Name("login"));

            usernameInput.SendKeys(user);
            passwordInput.SendKeys(password);
            loginButton.Click();
            wait.Until(ExpectedConditions.UrlToBe("http://localhost/litecart/admin/"));

        }
        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
