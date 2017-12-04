using System;

namespace Rakon.Test.Core
{
    public abstract class Unit
    {
        public string Name { get; }

        internal Unit(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(name);
            }

            Name = name;
        }

        public class Temperature : Unit
        {
            public static readonly Temperature TemperatureInDegC 
                = new Temperature("oC");

            private Temperature(string name) : base(name) { }
        }

        public class TemperatureRate : Unit
        {
            public static readonly TemperatureRate TemperatureRateInDegCPerMinute 
                = new TemperatureRate("oC/min");

            private TemperatureRate(string name) : base(name) { }
        }
    }
}