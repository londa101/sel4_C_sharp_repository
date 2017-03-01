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
            By MainMenuLocator = By.CssSelector("ul#box-apps-menu li a");
            By SubMenuLocator = By.CssSelector("li[id^='doc'] a");
            By HeaderLocator = By.CssSelector("h1");

            List<string> errorMessage = new List<string>();

            GoToLoginPage();
            CorrectLogin("admin", "admin");
            var mainMenuList = GetMenuText(MainMenuLocator);
            {
                foreach (var item in mainMenuList)
                {
                    SelectMenuItemByText(item);
                    Assert.IsTrue(AreElementsPresent(driver, HeaderLocator), $"header for item {item} doesn't found");

                    var subMenu = GetMenuText(SubMenuLocator);
                    foreach (var subItem in subMenu)
                    {
                        SelectMenuItemByText(subItem);
                        Assert.IsTrue(AreElementsPresent(driver, HeaderLocator), $"header for item {subItem} doesn't found");
                    }
                }
            }
        }


        bool AreElementsPresent(IWebDriver driver, By locator)
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
