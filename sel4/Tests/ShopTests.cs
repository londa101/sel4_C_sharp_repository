
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
    public class ShopTests:BaseTest
    {
        
        [Test]
        public void AllProductsShouldHaveStickers()
        {

            GoToShopPage();
            var SectionList = GetSectionList();
            foreach (var section in SectionList)
            {
                var productList = GetProductsFromSection(section);
                foreach (var duck in productList)
                {
                    Assert.IsTrue(IsStickerExistFor(duck), $"Product '{GetProductId(duck)}' for '{GetSectionName(section)}' hasn't any sticker");
                }
            }
        
        }

        [Test]
        public void ProductShouldHaveCorrectDetails()
        {

            GoToShopPage();
            var duckInfoFromMainPage = GetInfoFromMainPage("Campaigns", 1);
            SelectProduct("Campaigns", 1);
            var duckInfoFromDetails = GetInfoFromDetailsPage();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(duckInfoFromMainPage.Name, duckInfoFromDetails.Name, $"Names are different. MainPage - '{duckInfoFromMainPage.Name}', detail page - '{duckInfoFromMainPage.Name}'");
                Assert.AreEqual(duckInfoFromMainPage.RegularPrice, duckInfoFromDetails.RegularPrice, $"Regular prices are different. MainPage - '{duckInfoFromMainPage.RegularPrice}', detail page - '{duckInfoFromMainPage.RegularPrice}'");
                Assert.AreEqual(duckInfoFromMainPage.CampaignPrice, duckInfoFromDetails.CampaignPrice, $"Campaign prices are different. MainPage - '{duckInfoFromMainPage.CampaignPrice}', detail page - '{duckInfoFromMainPage.CampaignPrice}'");

                Assert.IsTrue(duckInfoFromMainPage.IsCampaignPriceBold, "Campaign Price on main page should be bold");
                Assert.IsTrue(IsCampaignPriceRed(duckInfoFromMainPage.CampaignPriceColor), $"Campaign price on main page should be red. Acual - '{duckInfoFromMainPage.CampaignPriceColor}'");
                Assert.IsTrue(duckInfoFromMainPage.IsRegularPriceCrossed, "Regular Price on main page should be crossed");
                Assert.IsTrue(IsRegularPriceGrey(duckInfoFromMainPage.RegularPriceColor), $"Regular price on main page should be grey. Acual - '{duckInfoFromMainPage.RegularPriceColor}'");
                Assert.LessOrEqual(duckInfoFromMainPage.RegularPriceSize, duckInfoFromMainPage.CampaignPriceSize, $"Regular price on main page should be less than Campaign Price. Regular - '{duckInfoFromMainPage.RegularPriceSize}', Campaign - '{duckInfoFromMainPage.CampaignPriceSize}'");

                Assert.IsTrue(duckInfoFromDetails.IsCampaignPriceBold, "Campaign Price on detail page should be bold");
                Assert.IsTrue(IsCampaignPriceRed(duckInfoFromDetails.CampaignPriceColor), $"Campaign price on detail page should be red. Acual - '{duckInfoFromDetails.CampaignPriceColor}'");
                Assert.IsTrue(duckInfoFromDetails.IsRegularPriceCrossed, "Regular Price on detail page should be crossed");
                Assert.IsTrue(IsRegularPriceGrey(duckInfoFromDetails.RegularPriceColor), $"Regular price on detail page should be grey. Acual - '{duckInfoFromDetails.RegularPriceColor}'");
                Assert.LessOrEqual(duckInfoFromDetails.RegularPriceSize, duckInfoFromDetails.CampaignPriceSize, $"Regular price on detail page should be less than Campaign Price. Regular - '{duckInfoFromDetails.RegularPriceSize}', Campaign - '{duckInfoFromDetails.CampaignPriceSize}'");
            });
        }

        [Test]
        public void RegisterNewCustomer()
        {
            
            string Tail = StringHelper.GetRandomString(4);
            Customer userInfo = new Customer()
            {
                firstName = "FN_" + Tail,
                lastName = "LN_" + Tail,
                address1 = "address 1_" + Tail,
                postcode = StringHelper.GetRandomNumberString(5),
                city = "city_"+Tail,
                country = "United States",
                zone = "Kansas",
                email = $"email_{Tail}@mail.com",
                phone = $"+1{StringHelper.GetRandomNumberString(7)}",
                password = "test",
                confirmPassword = "test"
            };

            GoToShopPage();
            CreateNewUser(userInfo);
            Logout();
            CorrectLogin(userInfo);
            Logout();
        }

        private void CorrectLogin(Customer userInfo)
        {
            var usernameInput = driver.FindElement(By.Name("email"));
            var passwordInput = driver.FindElement(By.Name("password"));
            var loginButton = driver.FindElement(By.Name("login"));

            usernameInput.SendKeys(userInfo.email);
            passwordInput.SendKeys(userInfo.password);
            loginButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#box-account a[href$=logout]")));
        }

        private void Logout()
        {
            var logoutLink = driver.FindElement(By.CssSelector("#box-account a[href$=logout]"));
            logoutLink.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.Name("login")));
        }

        private void CreateNewUser(Customer userInfo)
        {
            GoToUserCreationPage();
            FillFieldsAndSave(userInfo); 
        }

        private void FillFieldsAndSave(Customer userInfo)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("h1")));

            var taxIdInput = driver.FindElement(By.Name("tax_id"));
            var companyInput = driver.FindElement(By.Name("company"));
            var firstNameInput = driver.FindElement(By.Name("firstname"));
            var lastNameInput = driver.FindElement(By.Name("lastname"));
            var address1Input = driver.FindElement(By.Name("address1"));
            var address2Input = driver.FindElement(By.Name("address2"));
            var postcodeInput = driver.FindElement(By.Name("postcode"));
            var cityInput = driver.FindElement(By.Name("city"));
            var countrySelect = new SelectElement(driver.FindElement(By.Name("country_code")));
            var emailInput = driver.FindElement(By.Name("email"));
            var phoneInput = driver.FindElement(By.Name("phone"));
            var newsletterCheckbox = driver.FindElement(By.Name("newsletter"));
            var passwordInput = driver.FindElement(By.Name("password"));
            var confirmPasswordInput = driver.FindElement(By.Name("confirmed_password"));
            var CreateAccountButton = driver.FindElement(By.Name("create_account"));

            taxIdInput.TypeText(userInfo.taxId);
            companyInput.TypeText(userInfo.company);
            firstNameInput.TypeText(userInfo.firstName);
            lastNameInput.TypeText(userInfo.lastName);
            address1Input.TypeText(userInfo.address1);
            address2Input.TypeText(userInfo.address2);
            postcodeInput.TypeText(userInfo.postcode);
            cityInput.TypeText(userInfo.city);
            countrySelect.SelectText(userInfo.country);
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("select[name=zone_code]")));
            var zoneSelect = new SelectElement(driver.FindElement(By.CssSelector("select[name=zone_code]")));

            zoneSelect.SelectText(userInfo.zone);
            emailInput.TypeText(userInfo.email);
            phoneInput.TypeText(userInfo.phone);
            newsletterCheckbox.SetCheckbox(userInfo.newsletter);
            passwordInput.TypeText(userInfo.password);
            confirmPasswordInput.TypeText(userInfo.confirmPassword);
            CreateAccountButton.Click();
        }

        private void GoToUserCreationPage()
        {
            IWebElement newCustomerLink = driver.FindElement(By.CssSelector("form[name=login_form] a"));
            newCustomerLink.Click();
        }

        private bool IsRegularPriceGrey(string color)
        {
            return StyleHelper.IsGrey(color);
        }

        private bool IsCampaignPriceRed(string color)
        {
            return StyleHelper.IsRed(color);
        }

        private Duck GetInfoFromDetailsPage()
        {
            Duck result = new Duck();
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("h1")));

            IWebElement regularPriceElement = driver.FindElement(By.XPath("//div[@class='information']//*[@class='price' or @class='regular-price']"));
            IWebElement campaignPriceElement = driver.FindElement(By.CssSelector(".campaign-price"));

            result.Name = driver.FindElement(By.CssSelector("h1.title")).Text;
            result.RegularPrice = regularPriceElement.Text;
            result.RegularPriceColor = regularPriceElement.GetCssValue("color");
            result.RegularPriceSize = Convert.ToInt32(regularPriceElement.GetCssValue("font-size").Replace("px", ""));
            result.IsRegularPriceCrossed = regularPriceElement.TagName == "s";

            result.CampaignPriceColor = campaignPriceElement.GetCssValue("color");
            result.CampaignPrice = campaignPriceElement.Text;
            result.CampaignPriceSize = Convert.ToInt32(campaignPriceElement.GetCssValue("font-size").Replace("px", ""));
            result.IsCampaignPriceBold = campaignPriceElement.TagName == "strong";

            return result;
        }

        private Duck GetInfoFromMainPage(string listName, int index)
        {
            Duck result = new Duck();
            string box = listName.ToLower().Replace(' ', '-');
            By productSelector = By.CssSelector($"#box-{box}  li.product");
            IWebElement productElement = driver.FindElement(productSelector);
            IWebElement regularPriceElement = productElement.FindElement(By.XPath(".//*[@class='price' or @class='regular-price']"));
            IWebElement campaignPriceElement = productElement.FindElement(By.CssSelector(".campaign-price"));

            result.Name = productElement.FindElement(By.CssSelector(".name")).Text;
            result.RegularPrice = regularPriceElement.Text;
            result.RegularPriceColor = regularPriceElement.GetCssValue("color");
            result.RegularPriceSize = Convert.ToDouble(regularPriceElement.GetCssValue("font-size").Replace("px", ""));
            result.IsRegularPriceCrossed = regularPriceElement.TagName == "s";

            result.CampaignPriceColor = campaignPriceElement.GetCssValue("color");
            result.CampaignPrice = campaignPriceElement.Text;
            result.CampaignPriceSize = Convert.ToDouble(campaignPriceElement.GetCssValue("font-size").Replace("px", ""));
            result.IsCampaignPriceBold = campaignPriceElement.TagName == "strong";

            return result;

        }

        private void SelectProduct(string listName, int item)
        {
            string box = listName.ToLower().Replace(' ', '-');
            By linkSelector = By.CssSelector($"#box-{box}  li.product:nth-child({item})>a.link");
            IWebElement productLink = driver.FindElement(linkSelector);
            productLink.Click();

        }

        private List<IWebElement> GetProductsFromSection(IWebElement section)
        {
            return section.FindElements(By.CssSelector("li.product")).ToList();
        }

        private string GetSectionName(IWebElement section)
        {
            return section.FindElement(By.CssSelector("h3")).Text;
        }

        private bool IsStickerExistFor(IWebElement product)
        {
            By StickerSelector = By.CssSelector("div.sticker");
            return product.FindElements(StickerSelector).Count ==1;
        }

        private List<IWebElement> GetProducts()
        {
            return driver.FindElements(By.CssSelector("li.product")).ToList();
        }

        private string GetProductId(IWebElement product)
        {
            return product.FindElement(By.CssSelector(".name")).Text;
        }

        private List<IWebElement> GetSectionList()
        {
            return driver.FindElements(By.CssSelector(".middle>.content>.box")).ToList();
        }
    }
}
