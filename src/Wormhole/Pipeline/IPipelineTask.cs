namespace Wormhole.Pipeline
{
    public interface IPipelineTask<TInput, TOutput>
    {
        TOutput Execute(TInput input);
    }

}
