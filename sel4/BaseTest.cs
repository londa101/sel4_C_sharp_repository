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
    public class BaseTest
    {
        private string bowserToStart = "FF_ESR";
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
               
            }

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        }
      
        


       


        public bool AreElementsPresent(By locator)
        {
            return driver.FindElements(locator).Count > 0;
        }

        public List<string> GetMenuLinks(By selector)
        {
            return driver.FindElements(selector).Select(element => element.GetAttribute("href")).ToList();
        }
        public List<string> GetMenuText(By selector)
        {
            return driver.FindElements(selector).Select(element => element.Text).ToList();
        }

        public void SelectMenuItemByLink(string link)
        {
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector($"[href='{link}']")));
            var currentItem = driver.FindElement(By.CssSelector($"[href='{link}']"));
            currentItem.Click();
        }

        public void SelectMenuItemByText(string text)
        {
            wait.Until(ExpectedConditions.ElementExists(By.LinkText(text)));
            var currentItem = driver.FindElement(By.LinkText(text));
            currentItem.Click();
        }

        public void GoToLoginAdminPage()
        {
            driver.Url = "http://localhost/litecart/admin/login.php";
            wait.Until(ExpectedConditions.TitleIs("My Store"));
        }

        public void GoToShopPage()
        {
            driver.Url = "http://localhost/litecart/en/";
            wait.Until(ExpectedConditions.TitleIs("Online Store | My Store"));
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
