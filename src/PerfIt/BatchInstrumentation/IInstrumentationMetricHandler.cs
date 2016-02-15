namespace PerfIt.BatchInstrumentation
{
    public interface IInstrumentationMetricHandler
    {
        void OnRequestStarting();

        void OnRequestEnding();
    }

    public interface IInstrumentationMetricHandler<in T>
    {
        void OnRequestStarting(T aspectInstrumentationInfo);

        void OnRequestEnding(T aspectInstrumentationInfo);
    }
}