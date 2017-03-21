using OpenQA.Selenium;
using System.Collections.Generic;
using System;
using OpenQA.Selenium.Support.UI;

namespace sel4.Pages
{
    public class AdminCountriesPage : AdminHomePage
    {


        public AdminCountriesPage(IWebDriver driver)
    : base(driver)
        { }

        public List<string> GetCountriesList()
        {
            List<string> result = new List<string>();
            result = GetColumnValues("Name");

            return result;
        }
       

        public void OpenCountry(int index)
        {
            By TableSelector = By.CssSelector("table.dataTable");

            var locator = By.CssSelector($"tr:nth-child({index}) td:nth-child({GetColumnIndex(TableSelector, "Names")}) a");
            var countryLink = driver.FindElement(locator);
            countryLink.Click();
        }
        public List<string> GetZoneList()
        {
            List<string> result = new List<string>();
            result = GetColumnValues("Name");
            result.Remove(""); //delete las element
            return result;
        }
        public string GetZoneCount(string country)
        {
            By TableSelector = By.CssSelector("table.dataTable");

            var locator = By.XPath($"//td[.='{country}']/../td[{GetColumnIndex(TableSelector, "Zones")}]");
            var zoneElemenent = driver.FindElement(locator);
            return zoneElemenent.Text;
        }

        public string GetZoneCount(int index)
        {
            By TableSelector = By.CssSelector("table.dataTable");
            var ZoneSelector = By.CssSelector($"tr:nth-child({index}) td:nth-child({GetColumnIndex(TableSelector, "Zones")})");
            var zoneElemenent = driver.FindElement(ZoneSelector);
            return zoneElemenent.Text;
        }

        public CountryPage AddNewCountry()
        {
            By addNewCountryLocator = By.CssSelector("a.button[href*=edit_country]");
            wait.Until(ExpectedConditions.ElementIsVisible(addNewCountryLocator));
            var button = driver.FindElement(addNewCountryLocator);
            button.Click();
            return new CountryPage(driver);

        }
    }
}