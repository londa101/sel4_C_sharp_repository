using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Sel4
{
    [TestFixture]
    public class MyFirstTest
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void start()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void LoginTest()
        {
            driver.Url = "http://localhost/litecart/admin/login.php";
            wait.Until(ExpectedConditions.TitleIs("My Store"));
            CorrectLogin("admin", "admin");

            //TODO verifications 
            //

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
