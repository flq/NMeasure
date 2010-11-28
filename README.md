NMeasure helps you tackle your needs in working with measurements: Numbers with Units.
It will help you to convert measurements to other Units.

## Calculation ##
var m1 = new Measure(2.0, SingleUnit.Meter);
var m2 = new Measure(6.0, Unit.Inverse(SingleUnit.Second));
var m3 = m1*m2; // m3 is 12 meters / second;

## Conversion ##
var m = new Measure(1.0, SingleUnit.Kilometer);
var m2 = m.ConvertTo(SingleUnit.Millimeter);
m2.Value.IsEqualTo(1000000);