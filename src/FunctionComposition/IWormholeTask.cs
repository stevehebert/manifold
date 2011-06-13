namespace Wormhole
{
    public interface IWormholeTask<in TInput, out TOutput>
    {
        TOutput Execute(TInput input);
    }

}
