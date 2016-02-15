namespace PerfIt.Tests
{
    using System;
    using System.Diagnostics;
    using System.Net;

    using FakeItEasy;

    using PerfIt.BatchInstrumentation;
    using Xunit;

    public class BatchProcessInstrumentorShould
    {
        private BatchProcessInstrumentor target;

        public BatchProcessInstrumentorShould()
        {
            this.target = new BatchProcessInstrumentor();
        }

        [Fact]
        public void CallTheAspect()
        {
            var fakeAction = A.Fake<Action<object>>();

            this.target.Instrument(fakeAction);

            A.CallTo(fakeAction).MustHaveHappened();
        }

        [Fact]
        public void AllowForSomeBatchInformationToBeProvidedByTheAspect()
        {
            this.target.Instrument(
                batchInformation =>
                    {
                        Assert.NotNull(batchInformation);
                        Assert.IsType<BatchInformation>(batchInformation);
                    });
        }

        [Fact]
        public void PassUpdatedBatchInformationToAPerformanceCounterHandler()
        {
            var counterHandler = A.Fake<IInstrumentationMetricHandler<BatchInformation>>();
            BatchInformation suppliedBatchInfo = null;
            this.target.AddMetricHandler(counterHandler);

            this.target.Instrument(
                batchInfo =>
                    {
                        suppliedBatchInfo = batchInfo;
                        suppliedBatchInfo.TotalItemsInBatch = 9;
                    });

            A.CallTo(() => counterHandler.OnRequestEnding(suppliedBatchInfo)).MustHaveHappened();
        }


        [Fact]
        public void CallAspectAndPerformanceCounterMethodsInCorrectOrder()
        {
            var counterHandler = A.Fake<IInstrumentationMetricHandler<BatchInformation>>();
            var aspect = A.Fake<Action<BatchInformation>>();
            this.target.AddMetricHandler(counterHandler);

            using (var scope = Fake.CreateScope())
            {
                this.target.Instrument(aspect);

                using (scope.OrderedAssertions())
                {
                    A.CallTo(() => counterHandler.OnRequestStarting(A<BatchInformation>._)).MustHaveHappened(Repeated.Exactly.Once);
                    A.CallTo(aspect).MustHaveHappened(Repeated.Exactly.Once);
                    A.CallTo(() => counterHandler.OnRequestEnding(A<BatchInformation>._)).MustHaveHappened(Repeated.Exactly.Once);
                }
            }
        }
    }


}