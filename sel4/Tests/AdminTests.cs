
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

using System.Collections.Generic;
using System.Linq;



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
