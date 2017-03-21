using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace sel4.Pages
{
    public class CartPage : BasePage
    {
        private static readonly String Title = "My Store";


        public CartPage(IWebDriver driver) : base(driver)
        { }

        public int GetProductsCount()
        {
            var ShortcutLocator = By.CssSelector("li.shortcut");
            return driver.FindElements(ShortcutLocator).Count;
        }

        public bool IsAnyProductExist()
        {
            var rowsLocator = By.XPath("//div[@id='order_confirmation-wrapper']//td[6]/.."); //we use 6th cell because total and subtotal rows don't have so many cells
            return driver.FindElements(rowsLocator).Count > 0;
        }

        public void DeleteFirstProduct()
        {

            var RemoveButtonLocator = By.CssSelector("li:nth-child(1) button[name='remove_cart_item']");
            wait.Until(ExpectedConditions.ElementToBeClickable(RemoveButtonLocator));
            driver.FindElement(RemoveButtonLocator).Click();
            wait.Until(ExpectedConditions.StalenessOf(driver.FindElement(RemoveButtonLocator)));
            //

        }

        public void SelectFirstShortcut()
        {
            var ShortcutLocator = By.CssSelector("li.shortcut");
            wait.Until(ExpectedConditions.ElementToBeClickable(ShortcutLocator));
            var shortcut = driver.FindElement(ShortcutLocator);

            driver.FindElement(ShortcutLocator).Click();
        }
    
    }
}
