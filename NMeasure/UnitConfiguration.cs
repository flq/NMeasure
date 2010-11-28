using System;
using System.Collections.Generic;
using System.Linq;

namespace NMeasure
{
    public class UnitConfiguration
    {
        private static UnitConfiguration unitSystem;
        
        public static UnitConfiguration UnitSystem
        {
            get { return unitSystem ?? (unitSystem = new UnitConfiguration()); }
            private set { unitSystem = value; }
        }

        private readonly Dictionary<Unit,UnitMeta> metadata = new Dictionary<Unit, UnitMeta>();
        private readonly UnitGraph unitGraph = new UnitGraph();

        public UnitConfiguration()
        {
            UnitSystem = this;
            Precision = 10;
        }

        public UnitMeta this[Unit unit]
        {
            get
            {
                UnitMeta meta;
                metadata.TryGetValue(unit, out meta);
                return meta;
            }
        }

        /// <summary>
        /// Default precision: 10
        /// </summary>
        public int Precision { get; private set; }

        public UnitGraph UnitGraph { get { return unitGraph; } }

        public IUnitMetaConfig Unit(SingleUnit unit)
        {
            return Unit(NMeasure.Unit.From(unit));
        }

        public IUnitMetaConfig Unit(Unit unit)
        {
            return getOrAdd(unit);
        }

        private UnitMeta getOrAdd(Unit unit)
        {
            if (!metadata.ContainsKey(unit))
                metadata.Add(unit, new UnitMeta(unit, this));
            return metadata[unit];
        }

        public void SetMeasurePrecision(int precision)
        {
            Precision = precision;
        }
    }

    public interface IUnitMetaConfig
    {
        IUnitMetaConfig BelongsToTypeSystem(params UnitSystem[] unitSystem);
        IUnitMetaConfig IsPhysicalUnit(SingleUnit singleUnit);
        IUnitMetaConfig ConvertibleTo(SingleUnit second, Func<double, double> firstToSecond, Func<double, double> secondToFirst);
        IUnitMetaConfig ConvertibleTo(Unit second, Func<double, double> firstToSecond, Func<double, double> secondToFirst);
        IUnitScale StartScale();
    }

    public interface IUnitScale
    {
        IUnitScale To(SingleUnit singleUnit, int scale);
    }

    public class UnitMeta : IUnitMetaConfig
    {
        private readonly Unit unit;
        private readonly UnitConfiguration config;

        internal UnitMeta(Unit unit, UnitConfiguration config)
        {
            this.unit = unit;
            this.config = config;
            config.UnitGraph.AddUnit(unit);
        }

        public Unit Unit
        {
            get { return unit; }
        }

        public SingleUnit PhysicalUnit { get; private set; }
        public UnitSystem[] AssociatedUnitSystems { get; private set; }
        public UnitGraphNode ConversionInfo { get { return config.UnitGraph[unit]; } }

        public bool IsMemberOfUnitSystem(UnitSystem unitSystem)
        {
            return AssociatedUnitSystems.Any(us=> us.Equals(unitSystem));
        }

        IUnitMetaConfig IUnitMetaConfig.BelongsToTypeSystem(params UnitSystem[] unitSystem)
        {
            AssociatedUnitSystems = unitSystem;
            return this;
        }

        IUnitMetaConfig IUnitMetaConfig.IsPhysicalUnit(SingleUnit singleUnit)
        {
            if (!singleUnit.ToString().StartsWith("_"))
                throw new InvalidOperationException("Only physical units (marked with beginning underscore) are valid as input to this method");
            PhysicalUnit = singleUnit;
            return this;
        }

        IUnitMetaConfig IUnitMetaConfig.ConvertibleTo(Unit second, Func<double, double> firstToSecond, Func<double, double> secondToFirst)
        {
            if (PhysicalUnit == SingleUnit.Dimensionless)
                throw new InvalidOperationException("You must define physical unit of the left-hand side");
            var unitMeta = second.GetUnitData();
            if (unitMeta == null || unitMeta.PhysicalUnit == SingleUnit.Dimensionless)
            {
                unitMeta = (UnitMeta)config.Unit(second).IsPhysicalUnit(PhysicalUnit);
            }
            if (unitMeta.PhysicalUnit != PhysicalUnit)
                throw new InvalidOperationException("You can only define conversions between units that are compatible as physical units");
            var node = config.UnitGraph.AddUnit(second);
            ConversionInfo.AddConversion(node, firstToSecond);
            node.AddConversion(ConversionInfo, secondToFirst);
            return this;
        }

        IUnitMetaConfig IUnitMetaConfig.ConvertibleTo(SingleUnit second, Func<double, double> firstToSecond, Func<double, double> secondToFirst)
        {
            return ((IUnitMetaConfig) this).ConvertibleTo(Unit.From(second), firstToSecond, secondToFirst);
        }

        IUnitScale IUnitMetaConfig.StartScale()
        {
            return new ScaleBuilder(config, this);
        }
    }

    internal class ScaleBuilder : IUnitScale
    {
        private readonly UnitConfiguration config;
        private UnitMeta precedingUnit;


        public ScaleBuilder(UnitConfiguration config, UnitMeta rootUnit)
        {
            this.config = config;
            precedingUnit = rootUnit;
            if (rootUnit.PhysicalUnit == SingleUnit.Dimensionless)
                throw new InvalidOperationException("The unit you start with should be associated with a physical unit to start a scale.");
            
        }

        IUnitScale IUnitScale.To(SingleUnit singleUnit, int scale)
        {
            var newUnit = config.Unit(singleUnit)
                .IsPhysicalUnit(precedingUnit.PhysicalUnit)
                .ConvertibleTo(precedingUnit.Unit, v => v*scale, v => v/scale);
            ((IUnitMetaConfig)precedingUnit).ConvertibleTo(singleUnit, v => v / scale, v => v * scale);
            precedingUnit = (UnitMeta) newUnit;
            return this;
        }
    }
}