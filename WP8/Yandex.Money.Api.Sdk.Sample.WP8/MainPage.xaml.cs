using System;
using System.Globalization;
using System.Windows;
using Yandex.Money.Api.Sdk.Authorization;
using Yandex.Money.Api.Sdk.Net;
using Yandex.Money.Api.Sdk.Requests;
using Yandex.Money.Api.Sdk.Responses;
using Yandex.Money.Api.Sdk.Utils;

namespace Yandex.Money.Api.Sdk.Sample.WP8
{
    public partial class MainPage
    {
        /// <summary>
        /// http://tech.yandex.com/money/doc/dg/tasks/register-client.xml
        /// </summary>
        private const string ClientId = @"Your client Id";

        private const string RedirectUri = @"Your redirect uri";

        private static readonly Authenticator Authenticator = new Authenticator();

        private static readonly DefaultMobileHostsProvider Hosts = new DefaultMobileHostsProvider();

        private readonly DefaultHttpPostClient _httpClient = new DefaultHttpPostClient(Hosts, Authenticator);

        private static readonly AuthorizationRequestParams Params = new AuthorizationRequestParams
        {
            ClientId = ClientId,
            RedirectUri = RedirectUri,
            Scope = Scopes.Compose(new[] {Scopes.AccountInfo})
        };

        private readonly Wrapper _wrapper;

        public MainPage()
        {
            InitializeComponent();

            _wrapper = new Wrapper(Browser);
        }

        private async void GetAccountInfoClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(Authenticator.Token))
                {
                    var ab = new AuthorizationBroker();
                    var token = await ab.AuthorizeAsync(_httpClient, _wrapper, Hosts.AuthorizationdUri.ToString(), Params);
                    Authenticator.Token = token;
                }

                var accountInfoRequest = new AccountInfoRequest<AccountInfoResult>(_httpClient, new JsonSerializer<AccountInfoResult>());
                var accountInfoResult = await accountInfoRequest.Perform();

                Dispatcher.BeginInvoke(() =>
                {
                    Account.Text = accountInfoResult.Account;
                    Amount.Text = accountInfoResult.Balance.ToString(CultureInfo.InvariantCulture);
                    Browser.Visibility = Visibility.Collapsed;
                });

            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show(ex.Message));
            }
        }
    }
}