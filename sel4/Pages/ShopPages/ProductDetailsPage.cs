using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace sel4.Pages
{
    public class ProductDetailsPage : BasePage
    {
        private static readonly String Title = "My Store";
        

        public ProductDetailsPage(IWebDriver driver) : base(driver)
        { }

        public Duck GetInfo()
        {
            Duck result = new Duck();
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("h1")));

            IWebElement regularPriceElement = driver.FindElement(By.XPath("//div[@class='information']//*[@class='price' or @class='regular-price']"));
            IWebElement campaignPriceElement = driver.FindElement(By.CssSelector(".campaign-price"));

            result.Name = driver.FindElement(By.CssSelector("h1.title")).Text;
            result.RegularPrice = regularPriceElement.Text;
            result.RegularPriceColor = regularPriceElement.GetCssValue("color");
            result.RegularPriceSize = Convert.ToInt32(regularPriceElement.GetCssValue("font-size").Replace("px", ""));
            result.IsRegularPriceCrossed = regularPriceElement.TagName == "s";

            result.CampaignPriceColor = campaignPriceElement.GetCssValue("color");
            result.CampaignPrice = campaignPriceElement.Text;
            result.CampaignPriceSize = Convert.ToInt32(campaignPriceElement.GetCssValue("font-size").Replace("px", ""));
            result.IsCampaignPriceBold = campaignPriceElement.TagName == "strong";

            return result;
        }

        public void SelectSize(string v)
        {
            var SizeSelect = new SelectElement(driver.FindElement(By.CssSelector("select[name='options[Size]']")));
            SizeSelect.SelectByText(v);
        }

        public bool IsSizeControlExist()
        {
            return AreElementsPresent(By.CssSelector("select[name='options[Size]']"));
        }



        public void AddToCart()
        {
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("button[name='add_cart_product']")));

            var AddButton = driver.FindElement(By.CssSelector("button[name='add_cart_product']"));
            AddButton.Click();
        }

    }
}
