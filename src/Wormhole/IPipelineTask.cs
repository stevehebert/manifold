namespace Wormhole
{
    public interface IPipelineTask<in TInput, out TOutput>
    {
        TOutput Execute(TInput input);
    }
}
