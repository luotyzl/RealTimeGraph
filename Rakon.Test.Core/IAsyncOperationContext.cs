using System;
using System.Threading.Tasks;

namespace Rakon.Test.Core
{
    public interface IAsyncOperationContext
    {
        Task RampingUpAsync(Value<Unit.Temperature> targetTemperature,
            Value<Unit.TemperatureRate> rampRateAbs, ReadOnlyRange<Unit.Temperature> settlingBand,
            TimeSpan settlingTime);

        Task RampingDownAsync(Value<Unit.Temperature> targetTemperature,
            Value<Unit.TemperatureRate> rampRateAbs, ReadOnlyRange<Unit.Temperature> settlingBand,
            TimeSpan settlingTime);

        void CancelOperation();
        bool GetOperationStatus();

        Value<Unit.Temperature> GetCurrentTemperature();
    }
}