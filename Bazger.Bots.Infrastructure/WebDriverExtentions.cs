using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Bazger.Bots.Infrastructure
{
    public static class WebDriverExtentions
    {
        public static IWebElement TryFindElement(this ISearchContext driver, By by)
        {
            try
            {
                return driver.FindElement(by);
            }
            catch (NotFoundException ex)
            {
                return null;
            }
        }

        public static object TryExecuteScript(this RemoteWebDriver driver, string script, params object[] args)
        {
            try
            {
                return driver.ExecuteScript(script, args);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
