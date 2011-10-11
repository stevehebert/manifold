namespace Wormhole
{
    public interface IRoutingPipelineTask<in TInput, out TOutput> : IPipelineTask<TInput, TOutput>
    {
        bool CanExecute(TInput input);
    }
}