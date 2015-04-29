using System;

namespace Yandex.Money.Api.Sdk.Sample.WP8
{
    public interface IWebBrowser<T>
    {
        void Navigate(string uri, byte[] postParams, string additionalHeaders);
        event EventHandler<T> NavigationEvent;
    }
}