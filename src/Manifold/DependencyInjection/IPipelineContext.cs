namespace Manifold.DependencyInjection
{
    public interface IPipelineContext
    {
        ITypeResolver TypeResolver { get; }

        object this[int id] { get; set; }
    }
}
