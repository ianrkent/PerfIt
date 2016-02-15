namespace PerfIt.Tests
{
    using System.Diagnostics;

    using PerfIt.BatchInstrumentation;

    using Xunit;

    public class InstrumentationExamples
    {
        [Fact]
        public void ExampleUsage()
        {
            var batchInstrumentor = CreateBatchProcessInstrumentor();
            batchInstrumentor.Instrument(
                aspectInstrumentationInfo =>
                {
                    // do some work, and then update the aspects Instrumentation Info
                    aspectInstrumentationInfo.TotalItemsInBatch = 10;
                    aspectInstrumentationInfo.FailedCount = 1;
                    aspectInstrumentationInfo.SuccessfullyProcessedCount = 9;
                });

        }

        private static BatchProcessInstrumentor CreateBatchProcessInstrumentor()
        {
            var instrumentor = new BatchProcessInstrumentor();
            instrumentor.AddMetricHandler(new ExampleDurationTracer<BatchInformation>());
            instrumentor.AddMetricHandler(new ExampleBatchItemRatePerformanceCounter());
            return instrumentor;
        }

        private static Instrumentor CreateStandardInstrumentor()
        {
            var instrumentor = new Instrumentor();
            instrumentor.AddMetricHandler(new ExampleDurationTracer());
            return instrumentor;
        }
    }


    internal class ExampleBatchItemRatePerformanceCounter : IInstrumentationMetricHandler<BatchInformation>
    {
        public void OnRequestStarting(BatchInformation aspectInstrumentationInfo)
        {
            // do any stuff that needs to be setup up before the aspect is run
        }

        public void OnRequestEnding(BatchInformation aspectInstrumentationInfo)
        {
            // Read the aspectInstrumentationInfo - which in this case is some batch information
            var totalItemsInBatch = aspectInstrumentationInfo.TotalItemsInBatch;

            // record as appropriate
        }
    }

    internal class ExampleDurationTracer : IInstrumentationMetricHandler
    {
        public void OnRequestStarting()
        {
            Trace.Write("Start a stop watch and add it into a context that should be passed to the OnRequestEnding method");
        }

        public void OnRequestEnding()
        {
            Trace.Write("Get the stop watch from the context, and write the duration to the Trace");
        }
    }

    internal class ExampleDurationTracer<T> : ExampleDurationTracer, IInstrumentationMetricHandler<T>
    {
        public void OnRequestStarting(T aspectInstrumentationInfo)
        {
            // We don't actually need any information from the aspect, so we can just delegate to the insrumentation method that does not need it
            this.OnRequestStarting();
        }

        public void OnRequestEnding(T aspectInstrumentationInfo)
        {
            // We don't actually need any information from the aspect, so we can just delegate to the insrumentation method that does not need it
            this.OnRequestStarting();
        }
    }
}
