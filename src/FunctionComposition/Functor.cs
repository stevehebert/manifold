namespace Wormhole
{
    public delegate TOutput Functor<in TInput, out TOutput>(TInput input);

    public delegate TOutput Functor<in TNameType, in TInput, out TOutput>(TNameType name, TInput input);
}
