using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rakon.Test.Core;

namespace Rakon.Test
{
    [TestClass]
    public class OperationTest
    {
        [TestMethod]
        public async Task TestRampingUp()
        {
            var operationService = new AsyncOperationService();
            var targetTemperature = new Value<Unit.Temperature>(50.0, Unit.Temperature.TemperatureInDegC);
            var rampRate = new Value<Unit.TemperatureRate>(10.0, Unit.TemperatureRate.TemperatureRateInDegCPerMinute);
            var minBand = new Value<Unit.Temperature>(targetTemperature.Number - 0.5, Unit.Temperature.TemperatureInDegC);
            var maxBand = new Value<Unit.Temperature>(targetTemperature.Number + 0.5, Unit.Temperature.TemperatureInDegC);
            var band = new ReadOnlyRange<Unit.Temperature>(minBand, maxBand);
            var time = TimeSpan.FromMinutes(5);
            await operationService.RampingUpAsync(targetTemperature, rampRate, band, time);
            var currentTemp = operationService.GetCurrentTemperature().Number;
            Assert.AreEqual(50.0, currentTemp);
        }

        [TestMethod]
        public async Task TestRampingDown()
        {
            var operationService = new AsyncOperationService();
            var targetTemperature = new Value<Unit.Temperature>(3.0, Unit.Temperature.TemperatureInDegC);
            var rampRate = new Value<Unit.TemperatureRate>(2.5, Unit.TemperatureRate.TemperatureRateInDegCPerMinute);
            var minBand = new Value<Unit.Temperature>(targetTemperature.Number - 1, Unit.Temperature.TemperatureInDegC);
            var maxBand = new Value<Unit.Temperature>(targetTemperature.Number + 1, Unit.Temperature.TemperatureInDegC);
            var band = new ReadOnlyRange<Unit.Temperature>(minBand, maxBand);
            var time = TimeSpan.FromMinutes(5);
            await operationService.RampingDownAsync(targetTemperature, rampRate, band, time);
            var currentTemp = operationService.GetCurrentTemperature().Number;
            Assert.AreEqual(3.0, currentTemp);
        }

        [TestMethod]
        public void TestCancelRamp()
        {
            var operationService = new AsyncOperationService();
            var targetTemperature = new Value<Unit.Temperature>(69.56, Unit.Temperature.TemperatureInDegC);
            var rampRate = new Value<Unit.TemperatureRate>(2.5, Unit.TemperatureRate.TemperatureRateInDegCPerMinute);
            var minBand = new Value<Unit.Temperature>(targetTemperature.Number - 3, Unit.Temperature.TemperatureInDegC);
            var maxBand = new Value<Unit.Temperature>(targetTemperature.Number + 3, Unit.Temperature.TemperatureInDegC);
            var band = new ReadOnlyRange<Unit.Temperature>(minBand, maxBand);
            var time = TimeSpan.FromMinutes(5);
            Task.Run(async () =>
            {
                await operationService.RampingDownAsync(targetTemperature, rampRate, band, time);
            });
            operationService.CancelOperation();
            Assert.IsFalse(operationService.GetOperationStatus());
            Assert.AreNotEqual(69.56, operationService.GetCurrentTemperature());
        }
    }
}
