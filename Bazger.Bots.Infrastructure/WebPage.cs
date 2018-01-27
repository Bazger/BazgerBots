using System;
using System.Threading;
using OpenQA.Selenium.Remote;

namespace Bazger.Bots.Infrastructure
{
    public abstract class WebPage
    {
        protected readonly RemoteWebDriver _driver;

        public abstract string GetPageUrl();

        protected WebPage(RemoteWebDriver driver)
        {
            _driver = driver;
        }

        public void GoToPageUrl()
        {
            _driver.Navigate().GoToUrl(GetPageUrl());
        }

        public void ScrollDownToPageEnd()
        {
            bool isBottom;
            do
            {
                _driver.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
                Thread.Sleep(300);
                var isBottomFuncResult = _driver
                    .ExecuteScript("if(window.pageYOffset + window.innerHeight >= document.body.offsetHeight){return true;}else{return false;}")
                    .ToString().ToLower();
                isBottom = bool.Parse(isBottomFuncResult);
            } while (!isBottom);
        }

        public void JQueryScrollDownToPageEnd()
        {
            bool isBottom;
            do
            {
                _driver.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
                Thread.Sleep(300);
                var isBottomFuncResult = _driver
                    .ExecuteScript(
                        "if($(window).scrollTop() + $(window).height() >= $(document).height()) { return true; } else{ return false;}")
                    .ToString().ToLower();
                isBottom = bool.Parse(isBottomFuncResult);
            } while (!isBottom);
        }

        public void ScrollDownUntil(Func<int> elementState, int stopCount)
        {
            var resultsCount = elementState();
            var isBottom = false;
            while (resultsCount < stopCount && !isBottom)
            {

                _driver.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
                resultsCount = elementState();
                //TODO: DRIVER WAIT
                Thread.Sleep(450);
                var isBottomFuncResult = _driver
                    .ExecuteScript(
                        "if(window.pageYOffset + window.innerHeight >= document.body.offsetHeight){return true;}else{return false;}")
                    .ToString().ToLower();
                isBottom = bool.Parse(isBottomFuncResult);
            }
        }

        public void ScrollTo(int locationX, int locationY)
        {
            _driver.ExecuteScript($"window.scroll({locationX},{locationY})");
        }

        public void LoadJQuery()
        {
            _driver.ExecuteScript(
                "scriptElt = document.createElement('script');scriptElt.type = 'text/javascript';scriptElt.src = \"https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js\";document.getElementsByTagName('head')[0].appendChild(scriptElt);");
        }
    }
}
