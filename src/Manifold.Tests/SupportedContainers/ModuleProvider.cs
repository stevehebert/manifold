using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Manifold.Configuration;

namespace Manifold.Tests.SupportedContainers
{
    public class ModuleProvider
    {
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
