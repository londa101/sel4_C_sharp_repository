using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace sel4.Pages
{
    public class BasePage
    {
        public IWebDriver driver;
        protected WebDriverWait wait;

        protected BasePage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

        }
        protected BasePage(IWebDriver driver, String Title)
        {
            this.driver = driver;

            if (!Title.Equals(this.driver.Title))
            {
                string msg = "Page with title : " + Title + " is not found";
                throw new NoSuchWindowException(msg);
            }
        }

        public bool AreElementsPresent(By locator)
        {
            return driver.FindElements(locator).Count > 0;
        }

       
    }
}
