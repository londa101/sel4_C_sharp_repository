
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
