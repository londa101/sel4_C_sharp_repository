using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using sel4.Helpers;

namespace sel4.Pages
{
    public class CustomerPage : BasePage
    {
        private static readonly String Title = "My Store";
        

        public CustomerPage(IWebDriver driver) : base(driver)
        { }

        public void FillFieldsAndSave(Customer userInfo)
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

    }
}
