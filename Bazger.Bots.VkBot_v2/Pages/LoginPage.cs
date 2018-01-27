using Bazger.Bots.Infrastructure;
using OpenQA.Selenium.Remote;

namespace Bazger.Bots.VkBot_v2.Pages
{
    public class LoginPage : WebPage
    {
        private string _url = "https://vk.com/login";

        public LoginPage(RemoteWebDriver driver) : base(driver)
        {
        }

        public override string GetPageUrl()
        {
            return _url;
        }

        public void EnterTextToPasswordTextbox(string password)
        {
            _driver.FindElementById("pass").SendKeys(password);
        }

        public void EnterTextToLoginTextbox(string login)
        {
            _driver.FindElementById("email").SendKeys(login);
        }

        public void PressOnLoginButton()
        {
            _driver.FindElementById("login_button").Click();
        }
    }
}
