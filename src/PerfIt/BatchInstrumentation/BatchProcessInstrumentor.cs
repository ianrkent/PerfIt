namespace PerfIt.BatchInstrumentation
{
    public class BatchProcessInstrumentor : InstrumentorBase<BatchInformation>
    {
        protected override BatchInformation CreateAspectInstrumentationInfo()
        {
            return new BatchInformation();
        }
    }
}