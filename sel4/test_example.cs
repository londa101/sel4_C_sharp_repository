using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Sel4
{
    [TestFixture]
    public class MyFirstTest
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void start()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void FirstTest()
        {
            driver.Url = "https://wizzair.com/";
            var a = driver.Title;
            wait.Until(ExpectedConditions.TitleIs("Official Wizz Air website | Book direct for the cheapest prices"));
        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
