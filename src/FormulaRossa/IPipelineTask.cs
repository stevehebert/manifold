namespace FormulaRossa
{
    public interface IPipelineTask<in TInput, out TOutput>
    {
        TOutput Execute(TInput input);
    }

    public interface IRoutedPipelineTask<in TInput, out TOutput>
    {
        bool CanProcess(TInput input);
        TOutput Execute(TInput input);
    }
}
