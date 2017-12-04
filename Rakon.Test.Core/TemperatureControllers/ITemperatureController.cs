using System;

namespace Rakon.Test.Core.TemperatureControllers
{
    public interface ITemperatureController
    {
        /// <summary>
        ///     Returns the temperature controller minimum and maximum
        ///     supported ramp rate. E.g. [1oC/min, 10oC/min]
        /// </summary>
        ReadOnlyRange<Unit.TemperatureRate> RampRateRange { get; }

        /// <summary>
        ///     Returns the current temperature controller temperature
        ///     or null if the temperature is not available
        ///     (e.g. in case of an equipment communication error).
        /// </summary>
        /// <returns></returns>
        Value<Unit.Temperature> GetCurrentTemperature();

        /// <summary>
        ///     Instructs the temperature controller to start ramping.
        ///     If a ramp operation is already in progress, it is cancelled.
        ///     This method does not block and returns immediately.
        /// </summary>
        /// <param name="targetTemperature">The temperature to ramp to.</param>
        /// <param name="rampRateAbs">The absolute ramp rate.</param>
        /// <param name="settlingBand">
        ///     The temperature band relative to <see cref="targetTemperature" />
        ///     where the temperature is considered to be stable (stabilised).
        /// </param>
        /// <param name="settlingTime">
        ///     The wait (hold, or stabilisation) time after the temperature
        ///     has stabilised. The ramp must be stable for at least this period of time before
        ///     the ramp operation is deemed completed.
        /// </param>
        /// <returns>
        ///     Returns an async operation context -
        ///     you should define your own class here.
        /// </returns>
        IAsyncOperationContext BeginRampToTemperature(
            Value<Unit.Temperature> targetTemperature,
            Value<Unit.TemperatureRate> rampRateAbs,
            ReadOnlyRange<Unit.Temperature> settlingBand,
            TimeSpan settlingTime);

        /// <summary>
        ///     Returns the target temperature minimum and maximum
        /// </summary>
        ReadOnlyRange<Unit.Temperature> TargetTemperatureRange { get; }
    }
}