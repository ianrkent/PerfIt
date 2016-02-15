namespace PerfIt.BatchInstrumentation
{
    using System;
    using System.Collections.Generic;

    public class Instrumentor
    {
        protected readonly List<IInstrumentationMetricHandler> metricHandlers = new List<IInstrumentationMetricHandler>();

        public void Instrument(Action aspect, string instrumentationContext = null)
        {
            this.metricHandlers.ForEach(handler =>
            {
                handler.OnRequestStarting();
            });

            aspect();

            this.metricHandlers.ForEach(handler => handler.OnRequestEnding());
        }

        public void AddMetricHandler(IInstrumentationMetricHandler handler)
        {
            this.metricHandlers.Add(handler);
        }
    }

    /// <summary>
    /// Generic version of InstrumentorBase, that allows the user to sepcifiy the Type that is to be used as the aspect information
    /// </summary>
    /// <typeparam name="TAspectInstrumentationInfo">Type that holds information that the aspect can record for use by the instrumentation</typeparam>
    public abstract class InstrumentorBase<TAspectInstrumentationInfo> : Instrumentor
    {
        private readonly List<IInstrumentationMetricHandler<TAspectInstrumentationInfo>> genericMetricHandlers = new List<IInstrumentationMetricHandler<TAspectInstrumentationInfo>>();

        protected abstract TAspectInstrumentationInfo CreateAspectInstrumentationInfo();

        public void Instrument(Action<TAspectInstrumentationInfo> aspect, string instrumentationContext = null)
        {
            var aspectInstrumentationInfo = this.CreateAspectInstrumentationInfo();

            this.genericMetricHandlers.ForEach(handler =>
            {
                handler.OnRequestStarting(aspectInstrumentationInfo);
            });

            aspect(aspectInstrumentationInfo);

            this.genericMetricHandlers.ForEach(handler => handler.OnRequestEnding(aspectInstrumentationInfo));
        }

        public void AddMetricHandler(IInstrumentationMetricHandler<TAspectInstrumentationInfo> handler)
        {
            this.genericMetricHandlers.Add(handler);
        }
    }
}

