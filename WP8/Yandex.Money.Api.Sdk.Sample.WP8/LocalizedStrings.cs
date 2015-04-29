using Yandex.Money.Api.Sdk.Sample.WP8.Resources;

namespace Yandex.Money.Api.Sdk.Sample.WP8
{
    /// <summary>
    /// Provides access to string resources.
    /// </summary>
    public class LocalizedStrings
    {
        private static AppResources _localizedResources = new AppResources();

        public AppResources LocalizedResources { get { return _localizedResources; } }
    }
}