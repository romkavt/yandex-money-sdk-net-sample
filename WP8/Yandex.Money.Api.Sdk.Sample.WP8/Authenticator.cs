using Yandex.Money.Api.Sdk.Net;

namespace Yandex.Money.Api.Sdk.Sample.WP8
{
    public class Authenticator : DefaultAuthenticator
    {
        public override string Token { get; set; }
    }
}