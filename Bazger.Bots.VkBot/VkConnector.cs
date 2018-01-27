using System;
using Bazger.Bots.VkBot.Config;
using OpenQA.Selenium.Remote;

namespace Bazger.Bots.VkBot
{
    public class VkConnector
    {

        public bool Connect(RemoteWebDriver driver)
        {
            try
            {
                driver.Navigate().GoToUrl("https://vk.com/login");
                EnterTextToLoginTextbox(driver, VkConfigs.Login);
                EnterTextToPasswordTextbox(driver, VkConfigs.Password);
                PressOnLoginButton(driver);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void EnterTextToPasswordTextbox(RemoteWebDriver driver, string password)
        {
            driver.FindElementById("pass").SendKeys(password);
        }

        public void EnterTextToLoginTextbox(RemoteWebDriver driver, string login)
        {
            driver.FindElementById("email").SendKeys(login);
        }

        public void PressOnLoginButton(RemoteWebDriver driver)
        {
            driver.FindElementById("login_button").Click();
        }
    }
}
