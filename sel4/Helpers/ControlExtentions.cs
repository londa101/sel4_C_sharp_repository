using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace sel4.Helpers
{
    static class  ControlExtentions
    {
        public static void SetCheckbox(this IWebElement control, bool value)
        {
            var isChecked = control.GetAttribute("checked") == "true";
            if (value != isChecked)
                control.Click();
        }

        public static void SelectText(this SelectElement control, string textValue)
        {
            control.SelectByText(textValue);
        }

        public static void TypeText(this IWebElement control, string value)
        {

            if (!string.IsNullOrEmpty(value))
            {
                control.Clear();
                control.SendKeys(value);
            }
        }
    }
}
