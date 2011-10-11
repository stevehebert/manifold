namespace Wormhole
{
    public interface IRoutedPipelineTask<in TInput, out TOutput> : IPipelineTask<TInput, TOutput>
    {
        bool CanExecute(TInput input);
    }
}