namespace Manifold.Tests.SupportedContainers
{
    public interface ICommonModule
    {
        TType Resolve<TType>();
        void Build();
    }
}