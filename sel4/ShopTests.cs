
using NUnit.Framework;
using OpenQA.Selenium;
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
            });
            /*
               в) обычная цена серая и зачёркнутая, а акционная цена красная и жирная (это надо проверить на каждой странице независимо, при этом цвета на разных страницах могут не совпадать)
               г) акционная цена крупнее, чем обычная (это надо проверить на каждой странице независимо)
             */
            var a = 1;

        }

        private Duck GetInfoFromDetailsPage()
        {
            throw new NotImplementedException();
        }

        private Duck GetInfoFromMainPage(string listName, int index)
        {
            Duck result = new Duck();
            string box = listName.ToLower().Replace(' ', '-');
            By productSelector = By.CssSelector($"#box-{box}  li.product");
            IWebElement productElement = driver.FindElement(productSelector);

            result.Name = productElement.FindElement(By.CssSelector(".name")).Text;
            IWebElement regularPriceElement = productElement.FindElement(By.XPath(".//*[@class='price' or @class='regular-price']"));
            result.RegularPrice = regularPriceElement.Text;
            result.RegularPriceColor = regularPriceElement.GetCssValue("color");
            result.RegularPriceSize = regularPriceElement.Size;

            IWebElement campaignPriceElement = productElement.FindElement(By.CssSelector(".campaign-price"));
            result.CampaignPriceColor = campaignPriceElement.GetCssValue("color");
            result.CampaignPrice = campaignPriceElement.Text;
            result.CampaignPriceSize = campaignPriceElement.Size;
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
