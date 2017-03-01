
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;



namespace Sel4
{
    [TestFixture]
    public class AdminTests : BaseTest
    {
       
      
        [Test]
        public void LoginTest()
        {
            GoToLoginAdminPage();
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

            GoToLoginAdminPage();
            CorrectLogin("admin", "admin");
            var mainMenuList = GetMenuText(MainMenuLocator);
            {
                foreach (var item in mainMenuList)
                {
                    SelectMenuItemByText(item);
                    Assert.IsTrue(AreElementsPresent(HeaderLocator), $"header for item {item} doesn't found");

                    var subMenu = GetMenuText(SubMenuLocator);
                    foreach (var subItem in subMenu)
                    {
                        SelectMenuItemByText(subItem);
                        Assert.IsTrue(AreElementsPresent(HeaderLocator), $"header for item {subItem} doesn't found");
                    }
                }
            }
        }


        //bool AreElementsPresent(IWebDriver driver, By locator)
        //{
        //    return driver.FindElements(locator).Count > 0;
        //}

        //public List<string> GetMenuLinks(By selector)
        //{
        //    return driver.FindElements(selector).Select(element => element.GetAttribute("href")).ToList();
        //}
        //public List<string> GetMenuText(By selector)
        //{
        //    return driver.FindElements(selector).Select(element => element.Text).ToList();
        //}

        //public void SelectMenuItemByLink(string link)
        //{
        //    wait.Until(ExpectedConditions.ElementExists(By.CssSelector($"[href='{link}']")));
        //    var currentItem = driver.FindElement(By.CssSelector($"[href='{link}']"));
        //    currentItem.Click();
        //}

        //public void SelectMenuItemByText(string text)
        //{
        //    wait.Until(ExpectedConditions.ElementExists(By.LinkText(text)));
        //    var currentItem = driver.FindElement(By.LinkText(text));
        //    currentItem.Click();
        //}

        //public void GoToLoginPage()
        //{
        //    driver.Url = "http://localhost/litecart/admin/login.php";
        //    wait.Until(ExpectedConditions.TitleIs("My Store"));
        //}

        //public void CorrectLogin(string user, string password)
        //{
        //    var usernameInput = driver.FindElement(By.Name("username"));
        //    var passwordInput = driver.FindElement(By.Name("password"));
        //    var loginButton = driver.FindElement(By.Name("login"));

        //    usernameInput.SendKeys(user);
        //    passwordInput.SendKeys(password);
        //    loginButton.Click();
        //    wait.Until(ExpectedConditions.UrlToBe("http://localhost/litecart/admin/"));

        //}

    }
}
