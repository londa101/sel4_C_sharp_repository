using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

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


    }
}