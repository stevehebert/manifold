using System;

namespace FormulaRossa
{
    public delegate object Injector(Type type);

    public interface IOperation
    {
        Func<Injector, object, object> GetClosure();
    }
}
