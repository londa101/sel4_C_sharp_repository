using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.PageObjects;

namespace sel4.Pages
{
    public class ProductDetailsPage : BasePage
    {
        private static readonly String Title = "My Store";
        

        public ProductDetailsPage(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.XPath, Using = "//div[@class='information']//*[@class='price' or @class='regular-price']")]
        IWebElement regularPriceElement;

        [FindsBy(How = How.CssSelector, Using = ".campaign-price")]
        IWebElement campaignPriceElement;

        [FindsBy(How = How.CssSelector, Using = "button[name='add_cart_product']")]
        IWebElement AddButton;

        [FindsBy(How = How.CssSelector, Using = ".quantity")]
        public IWebElement QuantityElement;

        public Duck GetInfo()
        {
            Duck result = new Duck();
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("h1")));

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
            AddButton.Click();
        }

    }
}
