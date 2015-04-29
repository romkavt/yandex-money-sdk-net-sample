using System;
using Microsoft.Phone.Controls;

namespace Yandex.Money.Api.Sdk.Sample.WP8
{
    public class Wrapper : IWebBrowser<string>
    {
        private readonly WebBrowser _browser;

        public Wrapper(WebBrowser browser)
        {
            if (browser == null)
                return;

            _browser = browser;
            _browser.Navigating += (s, e) => { if (NavigationEvent != null) NavigationEvent(s, e.Uri.Query); };
        }

        public void Navigate(string uri, byte[] postParams, string additionalHeaders)
        {
            if (_browser != null)
                _browser.Navigate(new Uri(uri), postParams, additionalHeaders);
        }

        public event EventHandler<string> NavigationEvent;
    }
}