
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Linq;
using System;

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
                Assert.AreEqual(duckInfoFromMainPage.Name, duckInfoFromDetails.Name);
                Assert.AreEqual(duckInfoFromMainPage.RegularPrice, duckInfoFromDetails.RegularPrice);
                Assert.AreEqual(duckInfoFromMainPage.CampaignPrice, duckInfoFromDetails.CampaignPrice);

                Assert.IsTrue(duckInfoFromMainPage.IsCampaignPriceBold);
                Assert.IsTrue(duckInfoFromMainPage.IsRegularPriceCrossed);
                Assert.LessOrEqual(duckInfoFromMainPage.RegularPriceSize, duckInfoFromMainPage.CampaignPriceSize);

                Assert.IsTrue(duckInfoFromDetails.IsCampaignPriceBold);
                Assert.IsTrue(duckInfoFromDetails.IsRegularPriceCrossed);
                Assert.LessOrEqual(duckInfoFromDetails.RegularPriceSize, duckInfoFromDetails.CampaignPriceSize);


            });
           
            var a = 1;

        }

        private Duck GetInfoFromDetailsPage()
        {
            Duck result = new Duck();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='information']//*[@class='price' or @class='regular-price']")));

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
