namespace Wormhole
{
    public interface IFunctionExecutor<TInput, TOutput>
    {
        TOutput Execute(TInput input);

        TOutput Execute(object name, TInput input);
    }
}
