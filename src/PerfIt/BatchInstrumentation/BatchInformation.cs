namespace PerfIt.BatchInstrumentation
{
    public class BatchInformation   
    {
        public int TotalItemsInBatch { get; set; }

        public int SuccessfullyProcessedCount { get; set; }

        public int FailedCount { get; set; }
    }
}