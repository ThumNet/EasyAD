using System.Threading;
using Umbraco.Core.Configuration;

namespace ThumNet.EasyAD.Configuration
{
    /// <summary>
    /// Provides extension methods for the <see cref="UmbracoConfig"/> class.
    /// </summary>
    public static class UmbracoConfigExtensions
    {
        private static Config _config;

        /// <summary>
        /// Gets the models builder configuration.
        /// </summary>
        /// <param name="umbracoConfig">The umbraco configuration.</param>
        /// <returns>The models builder configuration.</returns>
        /// <remarks>Getting the models builder configuration freezes its state,
        /// and any attempt at modifying the configuration using the Setup method
        /// will be ignored.</remarks>
        public static Config EasyAD(this UmbracoConfig umbracoConfig)
        {
            // capture the current Config2.Default value, cannot change anymore
            LazyInitializer.EnsureInitialized(ref _config, () => Config.Value);
            return _config;
        }
    }
}
