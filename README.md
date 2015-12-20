# README #

NMeasure helps you to tackle your needs with regard to working with measurements: Numbers with Units.
It will help you to 
* convert measurements to other Units.
* compactify your units (well, your calculations result in the unit Kilogram*Meter / Second.Squared() ? That's Newton)
* The unit system is not hard-coded. There is a good set of commonly used physical units by instantiating the _StandardUnitConfiguration_
* NMeasure knows the concept of Fundamental units (e.g. Length and Time). While it will let you add 1 gram to 1 ton (if it knows the conversion), it won't let you add a second to a Joule.
* You can define your own Units and your own conversions, the configuration has its own
  Fluent API for you to just that.

## Calculation ##

    using static NMeasure.U; //If you can use c#6, it imports common units.
    ...
    StandardUnitConfiguration.Use();
    ...
    var m1 = new Measure(2.0, Meter); // or 2m * Meter
    var m2 = new Measure(6.0, Second.Inverse());
    var m3 = m1*m2; // m3 is 12 meters / second;

Also use subtraction, addition and division.

## Conversion ##

    var m = new Measure(1.0, Kilometer);
    var m2 = m.ConvertTo(Millimeter);
    m2.Value.IsEqualTo(1000000);
    ...
    var t1 = 3 * Seconds;
    var t2 = 4 * Milliseconds;
    var t3 = t1 + t2 // Gives you 3.004 seconds, the first unit is carried over.

An example from the tests...

    AdHocConfig.Use(c=>
    {
        c.SetMeasurePrecision(2);
        c.Unit(Meter).IsPhysicalUnit(_LENGTH)
            .StartScale()
            .To(Kilometer, 1000);
        c.Unit(Second).IsPhysicalUnit(_TIME)
            .StartScale()
            .To(Hour, 3600);
    });
    var kmPerH = Kilometer / Hour;
    var mPerSec = Meter / Second;     
    var v1 = 100 * kmPerH;
    var v2 = v1.ConvertTo(mPerSec);
    v2.Unit.IsEqualTo(mPerSec);
    v2.Value.IsEqualTo(27.78m);

## Licensing ##

Copyright 2010 Frank-Leonardo Quednau ([realfiction.net](http://realfiction.net)) 
Licensed under the Apache License, Version 2.0 (the "License"); 
you may not use this solution except in compliance with the License. 
You may obtain a copy of the License at 

http://www.apache.org/licenses/LICENSE-2.0 

Unless required by applicable law or agreed to in writing, 
software distributed under the License is distributed on an "AS IS" 
BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
See the License for the specific language governing permissions and limitations under the License. 