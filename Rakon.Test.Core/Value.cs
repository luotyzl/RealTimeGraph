using System;

namespace Rakon.Test.Core
{
    public class Value<TUnit>
        where TUnit : Unit
    {
        public double Number { get; set; }

        public TUnit Unit { get; }

        public Value(TUnit unit)
        {
            if (unit == null)
            {
                throw new ArgumentNullException(nameof(unit));
            }

            Unit = unit;
        }

        public Value(double number, TUnit unit)
            : this(unit)
        {
            Number = number;
        }

        public static implicit operator double(Value<TUnit> value)
        {
            return value.Number;
        }
    }
}