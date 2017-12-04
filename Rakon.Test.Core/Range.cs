
namespace Rakon.Test.Core
{
    public class ReadOnlyRange<TUnit>
        where TUnit : Unit
    {
        public Value<TUnit> Min { get; }

        public Value<TUnit> Max { get; }

        public ReadOnlyRange(Value<TUnit> min, Value<TUnit> max)
        {
            Min = min;
            Max = max;
        }
    }
}