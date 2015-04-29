using System.Threading.Tasks;
using Yandex.Money.Api.Sdk.Authorization;
using Yandex.Money.Api.Sdk.Interfaces;
using Yandex.Money.Api.Sdk.Requests;
using Yandex.Money.Api.Sdk.Responses;
using Yandex.Money.Api.Sdk.Utils;

namespace Yandex.Money.Api.Sdk.Sample.WP8
{
    public class AuthorizationBroker
    {
        private TaskCompletionSource<string> _tcs;

        public Task<string> AuthorizeAsync(IHttpClient client, IWebBrowser<string> browser, string uri, AuthorizationRequestParams prms)
        {
            if (browser == null)
                return null;

            _tcs = new TaskCompletionSource<string>();

            browser.NavigationEvent += async (s, e) =>
            {
                var d = Misc.QueryParamsToDictionary(e);

                if (!d.ContainsKey("code")) 
                    return;

                var tr = new TokenRequest<TokenResult>(client, new JsonSerializer<TokenResult>())
                {
                    Code = d["code"],
                    ClientId = prms.ClientId,
                    RedirectUri = prms.RedirectUri
                };

                var token = await tr.Perform();

                if (token == null)
                    return;

                if (_tcs != null)
                    _tcs.SetResult(token.Token);
            };

            browser.Navigate(uri, prms.PostBytes(), @"Content-Type: application/x-www-form-urlencoded/r/n");

            return _tcs.Task;
        }
    }
}