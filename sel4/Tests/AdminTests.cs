
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
    public class AdminTests : BaseTest
    {
       
        [Test]
        public void LoginTest()
        {
            GoToLoginAdminPage();
            CorrectLogin("admin", "admin");
        }

        [Test]
        public void LeftMenuTest()
        {
            By MainMenuLocator = By.CssSelector("ul#box-apps-menu li a");
            By SubMenuLocator = By.CssSelector("li[id^='doc'] a");
            By HeaderLocator = By.CssSelector("h1");

            List<string> errorMessage = new List<string>();

            GoToLoginAdminPage();
            CorrectLogin("admin", "admin");
            var mainMenuList = GetMenuText(MainMenuLocator);
            {
                foreach (var item in mainMenuList)
                {
                    SelectMenuItemByText(item);
                    Assert.IsTrue(AreElementsPresent(HeaderLocator), $"header for item {item} doesn't found");

                    var subMenu = GetMenuText(SubMenuLocator);
                    foreach (var subItem in subMenu)
                    {
                        SelectMenuItemByText(subItem);
                        Assert.IsTrue(AreElementsPresent(HeaderLocator), $"header for item {subItem} doesn't found");
                    }
                }
            }
        }

        [Test]
        public void CountriesShouldBeSorted()
        {
            GoToLoginAdminPage();
            CorrectLogin("admin", "admin");
            SelectMenuItemByText("Countries");
            var Countries = GetCountriesList();
            Assert.That(Countries, Is.Ordered);
        }

        [Test]
        public void CountriesZonesShouldBeSorted()
        {

            GoToLoginAdminPage();
            CorrectLogin("admin", "admin");
            SelectMenuItemByText("Countries");
            var Countries = GetCountriesList();
            Assert.Multiple(() =>
            {
                for (var i = 1; i <= Countries.Count; i++)
                {
                    if (GetZoneCount(i) != "0")
                    {
                        OpenCountry(Countries[i]);
                        var ZoneList = GetZoneList();
                        ReturnToCountriesList();
                        Assert.That(ZoneList, Is.Ordered);
                    }
                }
            });
        }

        [Test]
        public void GeoZonesShouldBeSorted()
        {

            GoToLoginAdminPage();
            CorrectLogin("admin", "admin");
            SelectMenuItemByText("Geo Zones");
            var Countries = GetCountriesList();
            Assert.Multiple(() =>
            {
                for (var i = 0; i < Countries.Count; i++)
                {
                    OpenCountry(Countries[i]);
                    var GeoZoneCounryList = GetGeoZoneCountryList();
                    var GeoZoneList = GetGeoZoneList();
                    ReturnToCountriesList();
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


            GoToLoginAdminPage();
            CorrectLogin("admin", "admin");
            SelectMenuItemByText("Catalog");
            AddNewProduct(newDuckInfo);

            var isNewProductAppearInAdmin = AreElementsPresent(By.XPath($"//a[.='{newDuckInfo.Name}']"));

            GoToShopPage();
            var isNewProductAppearInShop = AreElementsPresent(By.XPath($"//*[@id='box-most-popular']//li//div[.='{newDuckInfo.Name}']"));
            var isNewProductFirstInLatestList = AreElementsPresent(By.XPath($"//*[@id='box-latest-products']//li[1]//div[.='{newDuckInfo.Name}']"));

            Assert.Multiple(() =>
            {
                Assert.IsTrue(isNewProductAppearInAdmin, $"{newDuckInfo.Name} should be appeared in product list on Admin page");
                Assert.IsTrue(isNewProductAppearInShop, $"{newDuckInfo.Name} should be appeared in product list on Shop page");
                Assert.IsTrue(isNewProductFirstInLatestList, $"{newDuckInfo.Name} should be first in Latest list on Shop page");
            });
           

        }

        private void AddNewProduct(Product newDuckInfo)
        {
            GoToNewProduct();
            GoTab("General");
            AddGeneralInfo(newDuckInfo);
            GoTab("Information");
            AddInformtion(newDuckInfo);
            GoTab("Prices");
            AddPrices(newDuckInfo);
            SaveProduct();
        }

        private void SaveProduct()
        {
            var SaveButton = driver.FindElement(By.CssSelector("button[name='save']"));
            SaveButton.Click();
        }

        private void AddPrices(Product product)
        {
            var PurchasePriceField = driver.FindElement(By.CssSelector("input[name='purchase_price']"));
            var PurchasePriceCurrencySelect = new SelectElement(driver.FindElement(By.CssSelector("select[name=purchase_price_currency_code]")));
            var PriceUSDField = driver.FindElement(By.CssSelector("input[name='prices[USD]']"));
            var PriceEURField = driver.FindElement(By.CssSelector("input[name='prices[EUR]']"));

            PurchasePriceField.TypeText(product.PurchasePrice);
            PurchasePriceCurrencySelect.SelectByText(product.Currency);
            PriceUSDField.TypeText(product.PriceUSD);
            PriceEURField.TypeText(product.PriceEUR);
        }

        private void AddInformtion(Product product)
        {
            var ManufacterSelect = new SelectElement(driver.FindElement(By.CssSelector("select[name=manufacturer_id]")));
            var KeywordsField = driver.FindElement(By.CssSelector("input[name='keywords']"));
            var ShortDescriptionField = driver.FindElement(By.CssSelector("input[name='short_description[en]']"));
            var DescriptionArea = driver.FindElement(By.CssSelector("div.trumbowyg-editor"));
            var HeadTitleField = driver.FindElement(By.CssSelector("input[name='head_title[en]']"));
            var MetaDescriptionField = driver.FindElement(By.CssSelector("input[name='meta_description[en]']"));

            
            ManufacterSelect.SelectByText(product.Manufacter);
            KeywordsField.TypeText(product.Keywords);
            ShortDescriptionField.TypeText(product.ShortDescription);
            DescriptionArea.SendKeys(product.Description);
            HeadTitleField.TypeText(product.HeadTitle);
            MetaDescriptionField.TypeText(product.MetaDescription);

        }

        private void AddGeneralInfo(Product product)
        {
            
            var NameField = driver.FindElement(By.CssSelector("input[name='name[en]']"));
            var CodeField = driver.FindElement(By.CssSelector("input[name='code']"));
            var QuantityField = driver.FindElement(By.CssSelector("input[name='quantity']"));
            var ImageField = driver.FindElement(By.CssSelector("input[name='new_images[]']"));
            var DataFromField = driver.FindElement(By.CssSelector("input[name='date_valid_from']"));
            var DataToField = driver.FindElement(By.CssSelector("input[name='date_valid_to']"));

           
            SetStatus(product.Status);
            NameField.TypeText(product.Name);
            CodeField.TypeText(product.Code);
            SetCategory(product.Category);
            SetProductGroup(product.ProductGroup); 
            QuantityField.TypeText(product.Quantity);
            ImageField.TypeText($"{SupportFilesPath}\\{product.Image}");
            DataFromField.SendKeys(product.DateFrom); 
            DataToField.SendKeys(product.DateTo); 
            
        }

        private void SetProductGroup(List<string> GroupList)
        {
            ClearAllCheckboxes(By.CssSelector("input[type='checkbox'][name='product_groups[]']"));
            foreach (var group in GroupList)
            {
                By GroupLocator = By.XPath($".//td[.='{group}']/..//input");
                var groupCheckbox = driver.FindElement(GroupLocator);
                groupCheckbox.SetCheckbox(true);
            }
        }

        private void SetStatus(bool status)
        {
            var value = status ? '1' : '0';
            By StatusLocator = By.CssSelector($"input[name='status'][value='{value}']");
            var Status = driver.FindElement(StatusLocator);
            Status.Click();
        }

        private void SetCategory(List<string> CategoryList)
        {
            ClearAllCheckboxes(By.CssSelector("input[type='checkbox'][name='categories[]']"));

            foreach (var category in CategoryList)
            {
                By CategoryLocator = By.CssSelector($"input[type='checkbox'][name='categories[]'][data-name='{category}']");
                var categoryCheckbox = driver.FindElement(CategoryLocator);
                categoryCheckbox.SetCheckbox(true);
            }
        }

        private void ClearAllCheckboxes(By GroupSelector)
        {
            var allCheckboxes = driver.FindElements(GroupSelector);
            foreach (var element in allCheckboxes)
            { element.SetCheckbox(false); }
        }

        private void GoTab(string tabName)
        {
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector($"div.tabs a[href='#tab-{StringHelper.ConvertToLinkPart(tabName)}']")));
            By TabLocator = By.CssSelector($"div.tabs a[href='#tab-{StringHelper.ConvertToLinkPart(tabName)}']");
            var tab = driver.FindElement(TabLocator);
            tab.Click();
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector($"div.tabs li.active a[href='#tab-{StringHelper.ConvertToLinkPart(tabName)}']")));
        }

        private void GoToNewProduct()
        {
            By addNewProductLocator = By.CssSelector("a.button[href*=edit_product]");
            wait.Until(ExpectedConditions.ElementIsVisible(addNewProductLocator));
            var button = driver.FindElement(addNewProductLocator);
            button.Click();
        }

        public void CorrectLogin(string user, string password)
        {
            var usernameInput = driver.FindElement(By.Name("username"));
            var passwordInput = driver.FindElement(By.Name("password"));
            var loginButton = driver.FindElement(By.Name("login"));

            usernameInput.SendKeys(user);
            passwordInput.SendKeys(password);
            loginButton.Click();
            wait.Until(ExpectedConditions.UrlToBe("http://localhost/litecart/admin/"));

        }

        private List<string> GetGeoZoneList()
        {
            List<string> result = new List<string>();
            By TableSelector = By.CssSelector("table.dataTable");
            var table = driver.FindElement(TableSelector);

            var i = GetColumnIndex(TableSelector, "Zones");

            By ColumnSelector = By.CssSelector($"td:nth-child({i}) select > option[selected]");
            result = driver.FindElements(ColumnSelector).Select(element => element.Text).ToList();
            return result;
        }

        private void ReturnToCountriesList()
        {
            By ButtonSelector = By.CssSelector("button[name=cancel]");
            var buttonCancel = driver.FindElement(ButtonSelector);
            buttonCancel.Click();

        }

        private List<string> GetZoneList()
        {
            List<string> result = new List<string>();
            result = GetColumnValues("Name");
            result.Remove(""); //delete las element
            return result;
        }

        private List<string> GetGeoZoneCountryList()
        {
            List<string> result = new List<string>();
            By TableSelector = By.CssSelector("table.dataTable");
            var table = driver.FindElement(TableSelector);

            var i = GetColumnIndex(TableSelector, "Country");

            By ColumnSelector = By.CssSelector($"td:nth-child({i}) select > option[selected]");
            result =driver.FindElements(ColumnSelector).Select(element => element.Text).ToList();
            return result;
        }

        private void OpenCountry(string country)
        {
            var locator = By.XPath($"//a[.='{country}']");
            var countryLink = driver.FindElement(locator);
            countryLink.Click();
        }

        private void OpenCountry(int index)
        {
            By TableSelector = By.CssSelector("table.dataTable");

            var locator = By.CssSelector($"tr:nth-child({index}) td:nth-child({GetColumnIndex(TableSelector, "Names")}) a");
            var countryLink = driver.FindElement(locator);
            countryLink.Click();
        }

        private string GetZoneCount(string country)
        {
            By TableSelector = By.CssSelector("table.dataTable");

            var locator = By.XPath($"//td[.='{country}']/../td[{GetColumnIndex(TableSelector, "Zones")}]");
            var zoneElemenent = driver.FindElement(locator);
            return zoneElemenent.Text;
        }

        private string GetZoneCount(int index)
        {
            By TableSelector = By.CssSelector("table.dataTable");
            var ZoneSelector = By.CssSelector($"tr:nth-child({index}) td:nth-child({GetColumnIndex(TableSelector, "Zones")})");
            var zoneElemenent = driver.FindElement(ZoneSelector);
            return zoneElemenent.Text;
        }  

        private List<string> GetCountriesList()
        {
            List<string> result = new List<string>();
            result = GetColumnValues("Name");

            return result;
        }

        private List<string> GetColumnValues(string columnName)
        {
            List<string> result = new List<string>();
            By TableSelector = By.CssSelector("table.dataTable");
            var table = driver.FindElement(TableSelector);

            var i = GetColumnIndex(TableSelector, columnName);
            By ColumnSelector = By.CssSelector($" td:nth-child({i})");
            result = table.FindElements(ColumnSelector).Select(element => element.Text).ToList();
            return result;
        }

        private int GetColumnIndex(By TableSelector, string columnName)
        {
            var headers = GetColumnNames(TableSelector);
            var i = headers.IndexOf(columnName) + 1;
            return i;
        }

        private List<string> GetColumnNames(By TableSelector)
        {
            var table = driver.FindElement(TableSelector);
            var headerList = table.FindElements(By.CssSelector("th")).Select(element => element.Text).ToList();
            return headerList;
        }
    }
}
