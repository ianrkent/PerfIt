namespace PerfIt.BatchInstrumentation
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public abstract class InstrumentorBase<T>
    {
        private readonly List<IInstrumentationMetricHandler<T>> metricHandlers = new List<IInstrumentationMetricHandler<T>>();

        protected abstract T CreateAspectInstrumentationInfo();

        public void Instrument(Action<T> aspect, string instrumentationContext = null)
        {
            var aspectInstrumentationInfo = this.CreateAspectInstrumentationInfo();

            this.metricHandlers.ForEach(handler =>
            {
                handler.OnRequestStarting(aspectInstrumentationInfo);
            });

            aspect(aspectInstrumentationInfo);

            this.metricHandlers.ForEach(handler => handler.OnRequestEnding(aspectInstrumentationInfo));
        }

        public Task InstrumentAsync(
            Func<T, Task> asyncAspect,
            T aspectInstrumentationOutput,
            string instrumentationContext = null)
        {
            throw new NotImplementedException();
        }

        public void AddMetricHandler(IInstrumentationMetricHandler<T> handler)
        {
            this.metricHandlers.Add(handler);
        }
    }
}

