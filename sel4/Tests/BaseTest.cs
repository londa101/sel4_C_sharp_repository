using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using sel4.Helpers;
using sel4.Pages;

namespace Sel4
{
    [TestFixture]
    public class BaseTest
    {
        private string bowserToStart = "C";
        protected IWebDriver driver;
        protected WebDriverWait wait;
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
                case "E":
                    driver = new EdgeDriver();
                    break;
            }

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        }

       

        public AdminLoginPage GoToLoginAdminPage(IWebDriver driver)
        {
            driver.Url = "http://localhost/litecart/admin/login.php";
            wait.Until(ExpectedConditions.TitleIs("My Store"));
            return new AdminLoginPage(driver);
        }

        public ShopPage GoToShopPage(IWebDriver driver)
        {
            driver.Url = "http://localhost/litecart/en/";
            wait.Until(ExpectedConditions.TitleIs("Online Store | My Store"));
            return new ShopPage(driver);
        }

       
        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
