using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rakon.Test.Core
{
    public class AsyncOperationService : IAsyncOperationContext
    {
        private static bool _isBusy;

        private static Value<Unit.Temperature> _currentTemp = new Value<Unit.Temperature>(25.0,
            Unit.Temperature.TemperatureInDegC);

        private static bool _cancelFlg;

        public async Task RampingUpAsync(Value<Unit.Temperature> targetTemperature,
            Value<Unit.TemperatureRate> rampRateAbs, ReadOnlyRange<Unit.Temperature> settlingBand, TimeSpan settlingTime)
        {
            if (_isBusy) return;
            var startTime = SystemTime.Now;
            var maintainingTime = 0;
            await Task.Run(() =>
            {
                _isBusy = true;
                while (((_currentTemp < settlingBand.Min) || (maintainingTime < settlingTime.Minutes)) && !_cancelFlg)
                {
                    if ((SystemTime.Now - startTime).Minutes < 1)
                        Thread.Sleep(1000);
                    startTime = SystemTime.Now;
                    if (_currentTemp + rampRateAbs < settlingBand.Max)
                    {
                        _currentTemp.Number += rampRateAbs.Number;
                    }
                    else
                    {
                        _currentTemp = targetTemperature;
                        ++maintainingTime;
                    }
                }
                _isBusy = _cancelFlg = false;
            });
        }

        public async Task RampingDownAsync(Value<Unit.Temperature> targetTemperature,
            Value<Unit.TemperatureRate> rampRateAbs, ReadOnlyRange<Unit.Temperature> settlingBand, TimeSpan settlingTime)
        {
            if (_isBusy) return;
            var startTime = SystemTime.Now;
            var maintainingTime = 0;
            await Task.Run(() =>
            {
                _isBusy = true;
                while (((_currentTemp > settlingBand.Max) || (maintainingTime < settlingTime.Minutes)) && !_cancelFlg)
                {
                    if ((SystemTime.Now - startTime).Minutes < 1)
                        Thread.Sleep(1000);
                    startTime = SystemTime.Now;
                    if (_currentTemp - rampRateAbs > settlingBand.Max)
                    {
                        _currentTemp.Number -= rampRateAbs.Number;
                    }
                    else
                    {
                        _currentTemp = targetTemperature;
                        ++maintainingTime;
                    }
                }
                _isBusy = _cancelFlg = false;
            });
        }

        public void CancelOperation()
        {
            _cancelFlg = _isBusy;
        }

        public bool GetOperationStatus()
        {
            return _isBusy;
        }

        public Value<Unit.Temperature> GetCurrentTemperature()
        {
            return _currentTemp;
        }
    }
}
