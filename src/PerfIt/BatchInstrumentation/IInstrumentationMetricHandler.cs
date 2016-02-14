namespace PerfIt.BatchInstrumentation
{
    public interface IInstrumentationMetricHandler<in T>
    {
        void OnRequestStarting(T aspectInstrumentationInfo);

        void OnRequestEnding(T aspectInstrumentationInfo);
    }
}