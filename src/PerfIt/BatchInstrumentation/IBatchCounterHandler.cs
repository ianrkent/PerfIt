namespace PerfIt.BatchInstrumentation
{
    using System.Collections.Generic;

    public interface IBatchCounterHandler   
    {
        void OnRequestStarting(Dictionary<string, object> dictionary);

        void OnRequestEnding(BatchInformation batchInfo, Dictionary<string, object> theAspectsCallContext);
    }
}