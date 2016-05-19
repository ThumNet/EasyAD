using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThumNet.EasyAD.Configuration
{
    /// <summary>
    /// Represents the EasyAD configuration
    /// </summary>
    public class Config
    {
        private static Config _value;

        /// <summary>
        /// Gets the configuration - internal so that the UmbracoConfig extension
        /// can get the value to initialize its own value. Either a value has
        /// been provided via the Setup method, or a new instance is created, which
        /// will load settings from the config file.
        /// </summary>
        internal static Config Value => _value ?? new Config();


        internal const int DefaultSyncInterval = 15; // Interval in minutes for AD group Synchronisation

        /// <summary>
        /// Initializes a new instance of the <see cref="Config"/> class.
        /// </summary>
        private Config()
        {
            const string prefix = "ThumNet.EasyAD.";

            // giant kill switch, default: false
            // must be explicitely set to true for anything else to happen
            Enable = ConfigurationManager.AppSettings["owin:appStartup"] == typeof(Startup.UmbracoEasyADOwinStartup).FullName;

            // ensure defaults are initialized for tests
            SyncInterval = DefaultSyncInterval;

            // stop here, everything is false
            if (!Enable) return;

            // default: initialized above with DefaultSyncInterval const
            var value = ConfigurationManager.AppSettings[prefix + "SyncInterval"];
            var interval = 0;
            if (!string.IsNullOrWhiteSpace(value) && int.TryParse(value, out interval))
                SyncInterval = interval;
        }

        /// <summary>
        /// Gets a value indicating whether the EasyAD is enabled.
        /// </summary>
        /// <remarks>
        ///     <para>If this is false then absolutely nothing happens.</para>
        ///     <para>Default value is <c>false</c> which means that unless we have this setting, nothing happens.</para>
        /// </remarks>
        public bool Enable { get; }

        /// <summary>
        /// Get a value indicating the AD group synchronisation interval in minutes
        /// </summary>
        public int SyncInterval { get; }
    }
}
