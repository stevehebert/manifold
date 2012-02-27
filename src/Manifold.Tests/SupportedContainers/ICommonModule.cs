namespace Manifold.Tests.SupportedContainers
{
    public interface ICommonModule
    {
        TType Resolve<TType>();
        void Build();
        void Register<TType, TInterface>() where TType : TInterface;
    }
}