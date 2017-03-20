using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;

namespace sel4.Pages
{
    public class AdminGeoZonesPage : AdminHomePage
    {


        public AdminGeoZonesPage(IWebDriver driver)
    : base(driver)
        { }

        public List<string> GetCountriesList()
        {
            List<string> result = new List<string>();
            result = GetColumnValues("Name");

            return result;
        }
        public List<string> GetGeoZoneList()
        {
            List<string> result = new List<string>();
            By TableSelector = By.CssSelector("table.dataTable");
            var table = driver.FindElement(TableSelector);

            var i = GetColumnIndex(TableSelector, "Zones");

            By ColumnSelector = By.CssSelector($"td:nth-child({i}) select > option[selected]");
            result = driver.FindElements(ColumnSelector).Select(element => element.Text).ToList();
            return result;
        }

        

        public List<string> GetGeoZoneCountryList()
        {
            List<string> result = new List<string>();
            By TableSelector = By.CssSelector("table.dataTable");
            var table = driver.FindElement(TableSelector);

            var i = GetColumnIndex(TableSelector, "Country");

            By ColumnSelector = By.CssSelector($"td:nth-child({i}) select > option[selected]");
            result = driver.FindElements(ColumnSelector).Select(element => element.Text).ToList();
            return result;
        }

       
    }
}