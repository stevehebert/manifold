using System;

namespace Wormhole.Core
{
    public interface IStateMonad<TState, TValue>
    {
        Tuple<TState, TValue> Execute(Tuple<TState, TValue> stateValuePair);
    }
}
