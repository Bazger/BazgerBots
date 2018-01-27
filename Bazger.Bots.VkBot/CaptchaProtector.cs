using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Bazger.Bots.VkBot
{
    public delegate void CaptchaProtectorAction();
    public class CaptchaProtector
    {
        public static void ProtectAction(RemoteWebDriver driver, CaptchaProtectorAction action)
        {
            try
            {
                action();
            }
            catch (NoSuchFrameException)
            {
                if (driver.PageSource.Contains("box_x_button"))
                {
                    ProtectAction(driver, action);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
