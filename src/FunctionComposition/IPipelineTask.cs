namespace Wormhole
{
    public interface IPipelineTask<TInput, TOutput>
    {
        TOutput Execute(TInput input);
    }

}
