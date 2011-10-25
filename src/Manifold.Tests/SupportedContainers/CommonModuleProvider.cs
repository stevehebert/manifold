using System;
using System.IO;
using Manifold.Configuration;

namespace Manifold.Tests.SupportedContainers
{
    public class CommonModuleProvider
    {
        /// <summary>
        /// This retrieves the container associated with the provider type
        /// </summary>
        /// <param name="supportedProviderType">provider type</param>
        /// <param name="pipeCreator">pipe configuration</param>
        /// <returns>commond module</returns>
        public static ICommonModule Create(SupportedProviderType supportedProviderType, Action<IPipeCreator> pipeCreator)
        {
            switch(supportedProviderType)
            {
                case SupportedProviderType.Autofac:
                    return new AutofacModule(pipeCreator);

                case SupportedProviderType.Ninject:
                    return new NinjectModule(pipeCreator);

                default:
                    throw new InvalidDataException("SupportedProviderType was not mapped to an actual module type");
            }
        }
    }
}
