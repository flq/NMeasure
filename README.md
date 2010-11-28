# README #

NMeasure helps you tackle your needs in working with measurements: Numbers with Units.
It will help you to convert measurements to other Units.

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

## Calculation ##

    var m1 = new Measure(2.0, SingleUnit.Meter);
    var m2 = new Measure(6.0, Unit.Inverse(SingleUnit.Second));
    var m3 = m1*m2; // m3 is 12 meters / second;

## Conversion ##

    var m = new Measure(1.0, SingleUnit.Kilometer);
    var m2 = m.ConvertTo(SingleUnit.Millimeter);
    m2.Value.IsEqualTo(1000000);
