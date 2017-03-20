
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

            var Shop = GoToShopPage(driver);
            var SectionList = Shop.GetSectionList();
            foreach (var section in SectionList)
            {
                var productList = Shop.GetProductsFromSection(section);
                foreach (var duck in productList)
                {
                    Assert.IsTrue(Shop.IsStickerExistFor(duck), $"Product '{Shop.GetProductId(duck)}' for '{Shop.GetSectionName(section)}' hasn't any sticker");
                }
            }
        
        }

        [Test]
        public void ProductShouldHaveCorrectDetails()
        {

            var Shop = GoToShopPage(driver);
            var duckInfoFromMainPage = Shop.GetInfo("Campaigns", 1);
            var Details =  Shop.SelectProduct("Campaigns", 1);
            var duckInfoFromDetails = Details.GetInfo();

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

        [Test]
        public void RegisterNewCustomer()
        {
            
            string Tail = StringHelper.GetRandomString(4);
            Customer userInfo = new Customer()
            {
                firstName = "FN_" + Tail,
                lastName = "LN_" + Tail,
                address1 = "address 1_" + Tail,
                postcode = StringHelper.GetRandomNumberString(5),
                city = "city_"+Tail,
                country = "United States",
                zone = "Kansas",
                email = $"email_{Tail}@mail.com",
                phone = $"+1{StringHelper.GetRandomNumberString(7)}",
                password = "test",
                confirmPassword = "test"
            };

            var Shop = GoToShopPage(driver);
            Shop.CreateNewUser(userInfo);
            Shop.Logout();
            Shop.CorrectLogin(userInfo);
            Shop.Logout();
        }

        [Test]
        [Ignore("not completed")]
        public void VerifyCart()
        {
            /*
             
            6) удалить все товары из корзины один за другим, после каждого удаления подождать, пока внизу обновится таблица
            */
            var Shop = GoToShopPage(driver);
            //add products to cart
            for (var i = 1; i <= 3; i++)
            {
               var detailPage=  Shop.SelectProduct("Most Popular", 1);
                if (detailPage.IsSizeControlExist())
                    detailPage.SelectSize("Small");
                detailPage.AddToCart();
                var QuantityElement = driver.FindElement(By.CssSelector(".quantity"));
                wait.Until(ExpectedConditions.TextToBePresentInElement(QuantityElement, i.ToString()));
                Shop = GoToShopPage(driver);
            }
            Shop.GoToCart();

            var a = 1;

        }

        private void ReturnToMainPage()
        {
            throw new NotImplementedException();
        }

        public bool IsRegularPriceGrey(string color)
        {
            return StyleHelper.IsGrey(color);
        }

        public bool IsCampaignPriceRed(string color)
        {
            return StyleHelper.IsRed(color);
        }
    }
}
