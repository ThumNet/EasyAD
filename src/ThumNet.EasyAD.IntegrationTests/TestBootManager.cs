using System;
using ThumNet.EasyAD.Startup;
using Umbraco.Core;

namespace ThumNet.EasyAD.IntegrationTests
{
    /// <summary>
    /// Extends the CoreBootManager for use in this Integration tests.
    /// </summary>
    internal class TestBootManager : CoreBootManager
    {
        public TestBootManager(UmbracoApplicationBase umbracoApplication, string baseDirectory)
            : base(umbracoApplication)

        {
            //This is only here to ensure references to the assemblies needed for the DataTypesResolver
            //otherwise they won't be loaded into the AppDomain.
            //var interfacesAssemblyName = typeof(IDataType).Assembly.FullName;
            //var editorControlsAssemblyName = typeof(uploadField).Assembly.FullName;
            var easyAdAssemblyName = typeof(ApplicationEvents).Assembly.FullName;

            base.InitializeApplicationRootPath(baseDirectory);
        }

        /// <summary>
        /// Can be used to initialize our own Application Events
        /// </summary>
        protected override void InitializeApplicationEventsResolver()
        {
            base.InitializeApplicationEventsResolver();
        }

        /// <summary>
        /// Can be used to add custom resolvers or overwrite existing resolvers once they are made public
        /// </summary>
        protected override void InitializeResolvers()
        {
            base.InitializeResolvers();
        }        
    }
}