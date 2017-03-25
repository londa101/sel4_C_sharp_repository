using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.Generic;
using sel4.Helpers;
using System;
using System.Linq;

namespace sel4.Pages
{
    public class ShopPage:BasePage
    {
        public ShopPage(IWebDriver driver)
    : base(driver)
        {
            PageFactory.InitElements(driver, this);
        }


        [FindsBy(How = How.CssSelector, Using = "form[name=login_form] a")]
        IWebElement newCustomerLink;

        public ProductDetailsPage SelectProduct(string listName, int item)
        {
            string box = listName.ToLower().Replace(' ', '-');
            By linkSelector = By.CssSelector($"#box-{box}  li.product:nth-child({item})>a.link");
            IWebElement productLink = driver.FindElement(linkSelector);
            productLink.Click();
            return new ProductDetailsPage(driver);

        }

        public CustomerPage GoToUserCreationPage()
        {
           // IWebElement newCustomerLink = driver.FindElement(By.CssSelector("form[name=login_form] a"));
            newCustomerLink.Click();
            return new CustomerPage(driver);
        }

        public CartPage GoToCart()
        {
            var checkoutLink = driver.FindElement(By.CssSelector("a.link[href$='checkout']"));
            checkoutLink.Click();
            return new CartPage(driver);
        }

        

        public void CorrectLogin(Customer userInfo)
        {
            var usernameInput = driver.FindElement(By.Name("email"));
            var passwordInput = driver.FindElement(By.Name("password"));
            var loginButton = driver.FindElement(By.Name("login"));

            usernameInput.SendKeys(userInfo.email);
            passwordInput.SendKeys(userInfo.password);
            loginButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#box-account a[href$=logout]")));
        }

        public void Logout()
        {
            var logoutLink = driver.FindElement(By.CssSelector("#box-account a[href$=logout]"));
            logoutLink.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.Name("login")));
        }

        public void CreateNewUser(Customer userInfo)
        {
           var customerPage =  GoToUserCreationPage();
            customerPage.FillFieldsAndSave(userInfo);
        }


        public Duck GetInfo(string listName, int index)
        {
            Duck result = new Duck();
            string box = StringHelper.ConvertToLinkPart(listName);
            By productSelector = By.CssSelector($"#box-{box}  li.product");
            IWebElement productElement = driver.FindElement(productSelector);
            IWebElement regularPriceElement = productElement.FindElement(By.XPath(".//*[@class='price' or @class='regular-price']"));
            IWebElement campaignPriceElement = productElement.FindElement(By.CssSelector(".campaign-price"));

            result.Name = productElement.FindElement(By.CssSelector(".name")).Text;
            result.RegularPrice = regularPriceElement.Text;
            result.RegularPriceColor = regularPriceElement.GetCssValue("color");
            result.RegularPriceSize = Convert.ToDouble(regularPriceElement.GetCssValue("font-size").Replace("px", ""));
            result.IsRegularPriceCrossed = regularPriceElement.TagName == "s";

            result.CampaignPriceColor = campaignPriceElement.GetCssValue("color");
            result.CampaignPrice = campaignPriceElement.Text;
            result.CampaignPriceSize = Convert.ToDouble(campaignPriceElement.GetCssValue("font-size").Replace("px", ""));
            result.IsCampaignPriceBold = campaignPriceElement.TagName == "strong";

            return result;

        }


        public List<IWebElement> GetProductsFromSection(IWebElement section)
        {
            return section.FindElements(By.CssSelector("li.product")).ToList();
        }

        public string GetSectionName(IWebElement section)
        {
            return section.FindElement(By.CssSelector("h3")).Text;
        }

        public bool IsStickerExistFor(IWebElement product)
        {
            By StickerSelector = By.CssSelector("div.sticker");
            return product.FindElements(StickerSelector).Count == 1;
        }

        public List<IWebElement> GetProducts()
        {
            return driver.FindElements(By.CssSelector("li.product")).ToList();
        }

        public string GetProductId(IWebElement product)
        {
            return product.FindElement(By.CssSelector(".name")).Text;
        }

        public List<IWebElement> GetSectionList()
        {
            return driver.FindElements(By.CssSelector(".middle>.content>.box")).ToList();
        }
    }


}