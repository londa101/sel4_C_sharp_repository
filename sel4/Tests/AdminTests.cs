
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using sel4.Helpers;
using sel4.Pages;
using System;
using System.Linq;

namespace Sel4
{
    [TestFixture]
    public class AdminTests : BaseTest
    {
       
        [Test]
        public void LoginTest()
        {
           var LoginPage =  GoToLoginAdminPage(driver);
            LoginPage.CorrectLogin("admin", "admin");
        }

        [Test]
        public void LeftMenuTest()
        {
            By MainMenuLocator = By.CssSelector("ul#box-apps-menu li a");
            By SubMenuLocator = By.CssSelector("li[id^='doc'] a");
            By HeaderLocator = By.CssSelector("h1");

            List<string> errorMessage = new List<string>();

            var LoginPage = GoToLoginAdminPage(driver);
           var Home =  LoginPage.CorrectLogin("admin", "admin");
            var mainMenuList = Home.GetMenuText(MainMenuLocator);
            {
                foreach (var item in mainMenuList)
                {
                    Home.SelectMenuItemByText(item);
                    Assert.IsTrue(Home.AreElementsPresent(HeaderLocator), $"header for item {item} doesn't found");

                    var subMenu = Home.GetMenuText(SubMenuLocator);
                    foreach (var subItem in subMenu)
                    {
                        Home.SelectMenuItemByText(subItem);
                        Assert.IsTrue(Home.AreElementsPresent(HeaderLocator), $"header for item {subItem} doesn't found");
                    }
                }
            }
        }

        [Test]
        public void CountriesShouldBeSorted()
        {
           var LoginPage =  GoToLoginAdminPage(driver);
           var Home =  LoginPage.CorrectLogin("admin", "admin");
            Home.SelectMenuItemByText("Countries");
            var AdminCountries = new AdminCountriesPage(driver);
            var Countries = AdminCountries.GetCountriesList();
            Assert.That(Countries, Is.Ordered);
        }

        [Test]
        public void CountriesZonesShouldBeSorted()
        {

           var Login =  GoToLoginAdminPage(driver);
            var Home = Login.CorrectLogin("admin", "admin");
            Home.SelectMenuItemByText("Countries");
            var AdminCountries = new AdminCountriesPage(driver);

            var Countries = AdminCountries.GetCountriesList();
            Assert.Multiple(() =>
            {
                for (var i = 1; i <= Countries.Count; i++)
                {
                    if (AdminCountries.GetZoneCount(i) != "0")
                    {
                        AdminCountries.OpenCountry(Countries[i]);
                        var ZoneList = AdminCountries.GetZoneList();
                        AdminCountries.ReturnToCountriesList();
                        Assert.That(ZoneList, Is.Ordered);
                    }
                }
            });
        }

        [Test]
        public void GeoZonesShouldBeSorted()
        {

           var Login =  GoToLoginAdminPage(driver);
            var Home = Login.CorrectLogin("admin", "admin");
            Home.SelectMenuItemByText("Geo Zones");
            var AdminGeoZones = new AdminGeoZonesPage(driver);
            var Countries = AdminGeoZones.GetCountriesList();
            Assert.Multiple(() =>
            {
                for (var i = 0; i < Countries.Count; i++)
                {
                    AdminGeoZones.OpenCountry(Countries[i]);
                    var GeoZoneCounryList = AdminGeoZones.GetGeoZoneCountryList();
                    var GeoZoneList = AdminGeoZones.GetGeoZoneList();
                    AdminGeoZones.ReturnToCountriesList();
                    Assert.That(GeoZoneCounryList, Is.Ordered);
                    Assert.That(GeoZoneList, Is.Ordered);
                }
            });
            
        }


        [Test]
        public void VerifyAddNewProduct()
        {
            string Tail = StringHelper.GetRandomString(4);
            Product newDuckInfo = new Product()
            {
                Name = "Donut Duck " + Tail,
                Status = true,
                Code = StringHelper.GetRandomNumberString(5),
                Category = new List<string>() { "Rubber Ducks" },
                ProductGroup = new List<string>() { "Unisex" },
                Quantity = "8",
                Image = "donut.jpg",
                DateFrom = DateHelper.DaysBeforeToday(2),
                DateTo = DateHelper.DaysAfterToday(2),
                Manufacter = "ACME Corp.",
                Keywords = Tail,
                ShortDescription = Tail,
                Description = $"test {Tail}",
                HeadTitle = Tail,
                MetaDescription = Tail,
                PurchasePrice = "15",
                Currency = "US Dollars",
                PriceUSD = "17",
                PriceEUR= "19"
            };


            var Login = GoToLoginAdminPage(driver);
            var Home = Login.CorrectLogin("admin", "admin");
            Home.SelectMenuItemByText("Catalog");
            var Catalog = new AdminCatalogPage(driver);
            Catalog.AddNewProduct(newDuckInfo);

            var isNewProductAppearInAdmin = Catalog.AreElementsPresent(By.XPath($"//a[.='{newDuckInfo.Name}']"));

            var Shop = GoToShopPage(driver);
            var isNewProductAppearInShop = Shop.AreElementsPresent(By.XPath($"//*[@id='box-most-popular']//li//div[.='{newDuckInfo.Name}']"));
            var isNewProductFirstInLatestList = Shop.AreElementsPresent(By.XPath($"//*[@id='box-latest-products']//li[1]//div[.='{newDuckInfo.Name}']"));

            Assert.Multiple(() =>
            {
                Assert.IsTrue(isNewProductAppearInAdmin, $"{newDuckInfo.Name} should be appeared in product list on Admin page");
                Assert.IsTrue(isNewProductAppearInShop, $"{newDuckInfo.Name} should be appeared in product list on Shop page");
                Assert.IsTrue(isNewProductFirstInLatestList, $"{newDuckInfo.Name} should be first in Latest list on Shop page");
            });
        }

        [Test]
        [Ignore("Not complleted yet")]
        public void VerifyNewWindows()
        {
            /*
             * 1) зайти в админку
               2) открыть пункт меню Countries (или страницу http://localhost/litecart/admin/?app=countries&doc=countries)
               3) открыть на редактирование какую-нибудь страну или начать создание новой
               4) возле некоторых полей есть ссылки с иконкой в виде квадратика со стрелкой -- они ведут на внешние страницы и открываются 
                   в новом окне, именно это и нужно проверить.

Конечно, можно просто убедиться в том, что у ссылки есть атрибут target="_blank". Но в этом упражнении требуется именно 
кликнуть по ссылке, чтобы она открылась в новом окне, потом переключиться в новое окно, закрыть его, вернуться обратно,
и повторить эти действия для всех таких ссылок.
             */
            var LoginPage = GoToLoginAdminPage(driver);
            var Home = LoginPage.CorrectLogin("admin", "admin");
            Home.SelectMenuItemByText("Countries");
            var AdminCountries = new AdminCountriesPage(driver);
            var newCountry = AdminCountries.AddNewCountry();
            var ExternalLinks = driver.FindElements(By.CssSelector("[class='fa fa-external-link']"));

            string mainWindow = driver.CurrentWindowHandle;
            ICollection<string> oldWindows = driver.WindowHandles;
           
            foreach (var link in ExternalLinks)
            {
                link.Click();
                ICollection<string> newWindows = driver.WindowHandles;
                List<string> newWindowList = newWindows.ToList<string>();
                newWindowList.Remove(mainWindow); 
                string newWindow = newWindowList[0];  //= wait.Until(ThereIsWindowOtherThan(oldWindows));
                driver.SwitchTo().Window(newWindow);
                // ...
                driver.Close();
                driver.SwitchTo().Window(mainWindow);

            }
            var Countries = AdminCountries.GetCountriesList();
            Assert.That(Countries, Is.Ordered);
        }

        private Func<IWebDriver, string> ThereIsWindowOtherThan(ICollection<string> oldWindows)
        {
            throw new NotImplementedException();
        }
    }
}
