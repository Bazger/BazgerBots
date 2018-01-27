using System.Security.Authentication;
using System.Threading;
using Bazger.Bots.Core;
using Bazger.Bots.VkBot_v2.Config;
using Bazger.Bots.VkBot_v2.Pages;

namespace Bazger.Bots.VkBot_v2.Components
{
    public class ConnectorComponent : BotComponent
    {
        private readonly string Login;
        private readonly string Password;
        private LoginPage _loginPage;

        public ConnectorComponent(string login, string password, bool toRun = true) : base(toRun)
        {
            Login = login;
            Password = password;
        }

        public override void Prepare(BotStateBuilder botState)
        {
            _loginPage = new LoginPage(botState.Driver);
        }

        public override void Process(BotStateBuilder botState)
        {
            _loginPage.GoToPageUrl();
            _loginPage.EnterTextToLoginTextbox(Login);
            _loginPage.EnterTextToPasswordTextbox(Password);
            _loginPage.PressOnLoginButton();
            Thread.Sleep(1000);
            if (botState.Driver.Url.Contains(_loginPage.GetPageUrl()))
            {
                throw new AuthenticationException("Check your login or password, they may be incorrect");
            }
            Configs.Logger.Info($"Login to {Login} succeed!");
        }
    }
}
