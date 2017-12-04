using System;
using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json;
using Rakon.Test.Core;
using Rakon.Test.Core.TemperatureControllers;

namespace Rakon.TCE.Controllers
{
    public class TemperatureController : Controller, ITemperatureController
    {
        public ReadOnlyRange<Unit.TemperatureRate> RampRateRange => new ReadOnlyRange<Unit.TemperatureRate>(
            new Value<Unit.TemperatureRate>(1.0, Unit.TemperatureRate.TemperatureRateInDegCPerMinute),
            new Value<Unit.TemperatureRate>(10.0, Unit.TemperatureRate.TemperatureRateInDegCPerMinute));

        public ReadOnlyRange<Unit.Temperature> TargetTemperatureRange => new ReadOnlyRange<Unit.Temperature>(
            new Value<Unit.Temperature>(-45.0, Unit.Temperature.TemperatureInDegC),
            new Value<Unit.Temperature>(+200.0, Unit.Temperature.TemperatureInDegC));

        private readonly AsyncOperationService _operationService = new AsyncOperationService();

        public ActionResult GetTemperature()
        {
            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(GetCurrentTemperature()),
                ContentType = "application/json"
            };
        }

        public ActionResult CancelRamping()
        {
            var result = "";
            try
            {
                _operationService.CancelOperation();
                result = "Ramping has been canceled";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                result = e.Message;
            }
            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(result),
                ContentType = "application/json"
            };
        }

        [System.Web.Http.HttpPost]
        public ActionResult StartRamping([FromBody] string targetTemp, string rampRate, string settlingBand,
            string settlingTime)
        {
            var result = "";
            try
            {
                var rate = new Value<Unit.TemperatureRate>(Convert.ToDouble(rampRate),
                    Unit.TemperatureRate.TemperatureRateInDegCPerMinute);
                if ((rate.Number < RampRateRange.Min) || (rate.Number > RampRateRange.Max))
                {
                    result = "Ramp rate is out of range";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                var target = new Value<Unit.Temperature>(Convert.ToDouble(targetTemp),
                    Unit.Temperature.TemperatureInDegC);
                if ((target < TargetTemperatureRange.Min) || (target > TargetTemperatureRange.Max))
                {
                    result = "Target temperature is out of range";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                var bandNumber = Convert.ToDouble(settlingBand);
                var minBand = new Value<Unit.Temperature>(target.Number - bandNumber, Unit.Temperature.TemperatureInDegC);
                var maxBand = new Value<Unit.Temperature>(target.Number + bandNumber, Unit.Temperature.TemperatureInDegC);
                var band = new ReadOnlyRange<Unit.Temperature>(minBand, maxBand);
                var time = TimeSpan.FromMinutes(Convert.ToDouble(settlingTime));
                var operation = BeginRampToTemperature(target, rate, band, time);
                result = !operation.GetOperationStatus()
                    ? "Ramping request accept"
                    : "Request reject, another operation has not finished";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                result = e.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public Value<Unit.Temperature> GetCurrentTemperature()
        {
            var result = _operationService.GetCurrentTemperature();
            return result;
        }

        public IAsyncOperationContext BeginRampToTemperature(Value<Unit.Temperature> targetTemperature,
            Value<Unit.TemperatureRate> rampRateAbs, ReadOnlyRange<Unit.Temperature> settlingBand,
            TimeSpan settlingTime)
        {
            if (_operationService.GetOperationStatus()) return _operationService;
            if (_operationService.GetCurrentTemperature() < targetTemperature)
            {
                var task = _operationService.RampingUpAsync(targetTemperature, rampRateAbs, settlingBand, settlingTime);
                try
                {
                    task.ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else if (_operationService.GetCurrentTemperature() > targetTemperature)
            {
                var task = _operationService.RampingDownAsync(targetTemperature, rampRateAbs, settlingBand, settlingTime);
                try
                {
                    task.ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return _operationService;
        }
    }
}