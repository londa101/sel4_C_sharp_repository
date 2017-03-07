
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
            var duckInfoFromMainPage = GetInfoFromMainPage("Campaign", 1);
            SelectProduct("Campaign", 1);
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

        private Duck GetInfoFromMainPage(string list, int index)
        {

            throw new NotImplementedException();
        }

        private void SelectProduct(string listName, int item)
        {
            throw new NotImplementedException();
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
