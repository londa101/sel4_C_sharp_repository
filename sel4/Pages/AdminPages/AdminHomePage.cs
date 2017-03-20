using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Linq;

namespace sel4.Pages
{
    public class AdminHomePage:BasePage
    {


        public AdminHomePage(IWebDriver driver)
    : base(driver)
        { }

        public List<string> GetMenuLinks(By selector)
        {
            return driver.FindElements(selector).Select(element => element.GetAttribute("href")).ToList();
        }
        public List<string> GetMenuText(By selector)
        {
            return driver.FindElements(selector).Select(element => element.Text).ToList();
        }

        public void SelectMenuItemByLink(string link)
        {
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector($"[href='{link}']")));
            var currentItem = driver.FindElement(By.CssSelector($"[href='{link}']"));
            currentItem.Click();
        }
        public void SelectMenuItemByText(string text)
        {
            wait.Until(ExpectedConditions.ElementExists(By.LinkText(text)));
            var currentItem = driver.FindElement(By.LinkText(text));
            currentItem.Click();
        }

       
        protected List<string> GetColumnValues(string columnName)
        {
            List<string> result = new List<string>();
            By TableSelector = By.CssSelector("table.dataTable");
            var table = driver.FindElement(TableSelector);

            var i = GetColumnIndex(TableSelector, columnName);
            By ColumnSelector = By.CssSelector($" td:nth-child({i})");
            result = table.FindElements(ColumnSelector).Select(element => element.Text).ToList();
            return result;
        }

        protected int GetColumnIndex(By TableSelector, string columnName)
        {
            var headers = GetColumnNames(TableSelector);
            var i = headers.IndexOf(columnName) + 1;
            return i;
        }

        protected List<string> GetColumnNames(By TableSelector)
        {
            var table = driver.FindElement(TableSelector);
            var headerList = table.FindElements(By.CssSelector("th")).Select(element => element.Text).ToList();
            return headerList;
        }

       

        public void ReturnToCountriesList()
        {
            By ButtonSelector = By.CssSelector("button[name=cancel]");
            var buttonCancel = driver.FindElement(ButtonSelector);
            buttonCancel.Click();

        }
        public void OpenCountry(string country)
        {
            var locator = By.XPath($"//a[.='{country}']");
            var countryLink = driver.FindElement(locator);
            countryLink.Click();
        }






    }
}