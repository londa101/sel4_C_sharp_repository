
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;


namespace Sel4
{
    [TestFixture]
    public class ShopTests :BaseTest
    {
        


        [Test]
        public void AllProductsShouldHaveStickers()
        {
            List<string> errorMessage = new List<string>();

            GoToShopPage();
            var ProductList = GetProductElementList();
            foreach (var duck in ProductList)
            {
                Assert.IsTrue(IsStickerExistFor(duck), $" product {duck} hasn't any sticker");
            }

            
        }

        private bool IsStickerExistFor(IWebElement product)
        {
           // By StickerSelector = By.CssSelector("div.sticker");
            By StickerSelector = By.CssSelector(" strong.campaign-price");
            return product.FindElements(StickerSelector).Count ==1;
        }

        private List<IWebElement> GetProductElementList()
        {
            return driver.FindElements(By.CssSelector("li.product")).ToList();
        }
    }
}
