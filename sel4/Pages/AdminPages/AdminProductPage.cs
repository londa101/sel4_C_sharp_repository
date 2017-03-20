using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using sel4.Helpers;

namespace sel4.Pages
{
    public class AdminProductPage : AdminHomePage
    {


        public AdminProductPage(IWebDriver driver)
    : base(driver)
        { }

        public void SaveProduct()
        {
            var SaveButton = driver.FindElement(By.CssSelector("button[name='save']"));
            SaveButton.Click();
        }

        public void AddPrices(Product product)
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

        public void AddInformtion(Product product)
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

        public void AddGeneralInfo(Product product)
        {
            string SupportFilesPath = FileHelper.GetSupportFilesLocation();
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

        public void SetProductGroup(List<string> GroupList)
        {
            ClearAllCheckboxes(By.CssSelector("input[type='checkbox'][name='product_groups[]']"));
            foreach (var group in GroupList)
            {
                By GroupLocator = By.XPath($".//td[.='{group}']/..//input");
                var groupCheckbox = driver.FindElement(GroupLocator);
                groupCheckbox.SetCheckbox(true);
            }
        }

        public void SetStatus(bool status)
        {
            var value = status ? '1' : '0';
            By StatusLocator = By.CssSelector($"input[name='status'][value='{value}']");
            var Status = driver.FindElement(StatusLocator);
            Status.Click();
        }

        public void SetCategory(List<string> CategoryList)
        {
            ClearAllCheckboxes(By.CssSelector("input[type='checkbox'][name='categories[]']"));

            foreach (var category in CategoryList)
            {
                By CategoryLocator = By.CssSelector($"input[type='checkbox'][name='categories[]'][data-name='{category}']");
                var categoryCheckbox = driver.FindElement(CategoryLocator);
                categoryCheckbox.SetCheckbox(true);
            }
        }

        public void ClearAllCheckboxes(By GroupSelector)
        {
            var allCheckboxes = driver.FindElements(GroupSelector);
            foreach (var element in allCheckboxes)
            { element.SetCheckbox(false); }
        }

        public void GoTab(string tabName)
        {
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector($"div.tabs a[href='#tab-{StringHelper.ConvertToLinkPart(tabName)}']")));
            By TabLocator = By.CssSelector($"div.tabs a[href='#tab-{StringHelper.ConvertToLinkPart(tabName)}']");
            var tab = driver.FindElement(TabLocator);
            tab.Click();
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector($"div.tabs li.active a[href='#tab-{StringHelper.ConvertToLinkPart(tabName)}']")));
        }


    }
}