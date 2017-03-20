using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace sel4.Pages
{
    public class AdminLoginPage:BasePage
    {
        private static readonly String Title = "My Store";
        

        public AdminLoginPage(IWebDriver driver) : base(driver)
        { }

        private IWebElement usernameInput => driver.FindElement(By.Name("username"));
        private IWebElement passwordInput => driver.FindElement(By.Name("password"));
        private IWebElement loginButton => driver.FindElement(By.Name("login"));

        public AdminHomePage CorrectLogin(string user, string password)
        {
            usernameInput.SendKeys(user);
            passwordInput.SendKeys(password);
            loginButton.Click();
            wait.Until(ExpectedConditions.UrlToBe("http://localhost/litecart/admin/"));
            return new AdminHomePage(driver);
        }

    }
}
