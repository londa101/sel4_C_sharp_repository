using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Linq;

namespace sel4.Pages
{
    public class AdminCatalogPage : AdminHomePage
    {


        public AdminCatalogPage(IWebDriver driver)
    : base(driver)
        { }


        public void AddNewProduct(Product newDuckInfo)
        {
           var newProductPage =  GoToNewProduct(driver);
            newProductPage.GoTab("General");
            newProductPage.AddGeneralInfo(newDuckInfo);
            newProductPage.GoTab("Information");
            newProductPage.AddInformtion(newDuckInfo);
            newProductPage.GoTab("Prices");
            newProductPage.AddPrices(newDuckInfo);
            newProductPage.SaveProduct();
        }

       

        private AdminProductPage GoToNewProduct(IWebDriver driver)
        {
            By addNewProductLocator = By.CssSelector("a.button[href*=edit_product]");
            wait.Until(ExpectedConditions.ElementIsVisible(addNewProductLocator));
            var button = driver.FindElement(addNewProductLocator);
            button.Click();
            return new AdminProductPage(driver);
        }

        public List<string> GetProductList()
        {
            var ListLocator = By.CssSelector(".dataTable a[href*='product']:not([title])");
            return driver.FindElements(ListLocator).Select(element => element.Text).ToList();
        }

        internal void SelectCategory(string category)
        {
            var categoryElement = driver.FindElement(By.XPath($"//a[.='{category}']"));
            categoryElement.Click();
        }

        internal AdminProductPage SelectProduct(string product)
        {
            var productElement = driver.FindElement(By.XPath($"//a[.='{product}']"));
            productElement.Click();
            return new AdminProductPage(driver);
        }
    }
}