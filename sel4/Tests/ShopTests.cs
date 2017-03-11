
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Linq;
using System;
using sel4.Helpers;

namespace Sel4
{
    [TestFixture]
    public class ShopTests:BaseTest
    {
        


        [Test]
        public void AllProductsShouldHaveStickers()
        {

            GoToShopPage();
            var SectionList = GetSectionList();
            foreach (var section in SectionList)
            {
                var productList = GetProductsFromSection(section);
                foreach (var duck in productList)
                {
                    Assert.IsTrue(IsStickerExistFor(duck), $"Product '{GetProductId(duck)}' for '{GetSectionName(section)}' hasn't any sticker");
                }
            }
        
        }

        [Test]
        public void ProductShouldHaveCorrectDetails()
        {

            GoToShopPage();
            var duckInfoFromMainPage = GetInfoFromMainPage("Campaigns", 1);
            SelectProduct("Campaigns", 1);
            var duckInfoFromDetails = GetInfoFromDetailsPage();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(duckInfoFromMainPage.Name, duckInfoFromDetails.Name, $"Names are different. MainPage - '{duckInfoFromMainPage.Name}', detail page - '{duckInfoFromMainPage.Name}'");
                Assert.AreEqual(duckInfoFromMainPage.RegularPrice, duckInfoFromDetails.RegularPrice, $"Regular prices are different. MainPage - '{duckInfoFromMainPage.RegularPrice}', detail page - '{duckInfoFromMainPage.RegularPrice}'");
                Assert.AreEqual(duckInfoFromMainPage.CampaignPrice, duckInfoFromDetails.CampaignPrice, $"Campaign prices are different. MainPage - '{duckInfoFromMainPage.CampaignPrice}', detail page - '{duckInfoFromMainPage.CampaignPrice}'");

                Assert.IsTrue(duckInfoFromMainPage.IsCampaignPriceBold, "Campaign Price on main page should be bold");
                Assert.IsTrue(IsCampaignPriceRed(duckInfoFromMainPage.CampaignPriceColor), $"Campaign price on main page should be red. Acual - '{duckInfoFromMainPage.CampaignPriceColor}'");
                Assert.IsTrue(duckInfoFromMainPage.IsRegularPriceCrossed, "Regular Price on main page should be crossed");
                Assert.IsTrue(IsRegularPriceGrey(duckInfoFromMainPage.RegularPriceColor), $"Regular price on main page should be grey. Acual - '{duckInfoFromMainPage.RegularPriceColor}'");
                Assert.LessOrEqual(duckInfoFromMainPage.RegularPriceSize, duckInfoFromMainPage.CampaignPriceSize, $"Regular price on main page should be less than Campaign Price. Regular - '{duckInfoFromMainPage.RegularPriceSize}', Campaign - '{duckInfoFromMainPage.CampaignPriceSize}'");

                Assert.IsTrue(duckInfoFromDetails.IsCampaignPriceBold, "Campaign Price on detail page should be bold");
                Assert.IsTrue(IsCampaignPriceRed(duckInfoFromDetails.CampaignPriceColor), $"Campaign price on detail page should be red. Acual - '{duckInfoFromDetails.CampaignPriceColor}'");
                Assert.IsTrue(duckInfoFromDetails.IsRegularPriceCrossed, "Regular Price on detail page should be crossed");
                Assert.IsTrue(IsRegularPriceGrey(duckInfoFromDetails.RegularPriceColor), $"Regular price on detail page should be grey. Acual - '{duckInfoFromDetails.RegularPriceColor}'");
                Assert.LessOrEqual(duckInfoFromDetails.RegularPriceSize, duckInfoFromDetails.CampaignPriceSize, $"Regular price on detail page should be less than Campaign Price. Regular - '{duckInfoFromDetails.RegularPriceSize}', Campaign - '{duckInfoFromDetails.CampaignPriceSize}'");
            });
        }

        private bool? IsRegularPriceGrey(string color)
        {
            return StyleHelper.IsGrey(color);
        }

        private bool IsCampaignPriceRed(string color)
        {
            return StyleHelper.IsRed(color);
        }

        private Duck GetInfoFromDetailsPage()
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

        private Duck GetInfoFromMainPage(string listName, int index)
        {
            Duck result = new Duck();
            string box = listName.ToLower().Replace(' ', '-');
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

        private void SelectProduct(string listName, int item)
        {
            string box = listName.ToLower().Replace(' ', '-');
            By linkSelector = By.CssSelector($"#box-{box}  li.product:nth-child({item})>a.link");
            IWebElement productLink = driver.FindElement(linkSelector);
            productLink.Click();

        }

        private List<IWebElement> GetProductsFromSection(IWebElement section)
        {
            return section.FindElements(By.CssSelector("li.product")).ToList();
        }

        private string GetSectionName(IWebElement section)
        {
            return section.FindElement(By.CssSelector("h3")).Text;
        }

        private bool IsStickerExistFor(IWebElement product)
        {
            By StickerSelector = By.CssSelector("div.sticker");
            return product.FindElements(StickerSelector).Count ==1;
        }

        private List<IWebElement> GetProducts()
        {
            return driver.FindElements(By.CssSelector("li.product")).ToList();
        }

        private string GetProductId(IWebElement product)
        {
            return product.FindElement(By.CssSelector(".name")).Text;
        }

        private List<IWebElement> GetSectionList()
        {
            return driver.FindElements(By.CssSelector(".middle>.content>.box")).ToList();
        }
    }
}
