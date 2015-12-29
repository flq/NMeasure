using System;
using System.Linq;
using NMeasure.Converting;

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

        private readonly UnitIndex<UnitMeta> metadata = new UnitIndex<UnitMeta>();
        private readonly UnitIndex<Unit> compactions = new UnitIndex<Unit>();
        private readonly UnitGraph unitGraph = new UnitGraph();
        private int _defaultPrecision;

        internal UnitConfiguration()
        {
            UnitSystem = this;
            _defaultPrecision = 10;
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
        public int Precision(Unit unit)
        {
            var unitMeta = this[unit];
            return unitMeta != null && unitMeta.UnitPrecision.HasValue ? unitMeta.UnitPrecision.Value : _defaultPrecision;
        }

        internal UnitGraph UnitGraph { get { return unitGraph; } }

        public IUnitMetaConfig Unit(Unit unit)
        {
            return GetOrAdd(unit);
        }

        public void SetMeasurePrecision(int precision)
        {
            _defaultPrecision = precision;
        }

        public void AddCompaction(Unit unit, Unit compactionOfUnit)
        {
            compactions[unit] = compactionOfUnit;
        }

        public void Extend<T>() where T : IUnitConfigurationExtension, new()
        {
            Extend(new T());
        }

        public void Extend(params IUnitConfigurationExtension[] extension)
        {
            foreach (var x in extension)
              x.Extend(this);
        }

        internal Unit GetEquivalent(Unit unit)
        {
            Unit compaction;
            compactions.TryGetValue(unit, out compaction);
            return compaction;
        }

        private UnitMeta GetOrAdd(Unit unit)
        {
            if (!metadata.ContainsKey(unit))
                metadata.Add(unit, new UnitMeta(unit, this));
            return metadata[unit];
        }
    }

    public interface IUnitConfigurationExtension
    {
        void Extend(UnitConfiguration c);
    }

    public interface IUnitMetaConfig
    {
        IUnitMetaConfig BelongsToTypeSystem(params UnitSystem[] unitSystem);
        IUnitMetaConfig IsPhysicalUnit(Unit unit);
        IUnitMetaConfig EquivalentTo(Unit unit);
        IUnitMetaConfig ConvertValueBased(Unit second, Func<decimal, decimal> firstToSecond, Func<decimal, decimal> secondToFirst);
        IUnitMetaConfig ConvertibleTo(Unit second, Func<Measure, Measure> firstToSecond, Func<Measure, Measure> secondToFirst);
        IUnitScale StartScale();
        IUnitMetaConfig HasPrecision(int precision);
    }

    public interface IUnitScale
    {
        IUnitScale To(Unit singleUnit, int scale);
    }

    public class UnitMeta : IUnitMetaConfig
    {
        private readonly Unit unit;
        private readonly UnitConfiguration config;

        public static readonly string MultiplicationSign = "*"; //'\u00D7'.ToString();
        public static readonly string DivisionSign = '/'.ToString();


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

        public Unit PhysicalUnit { get; private set; }
        public UnitSystem[] AssociatedUnitSystems { get; private set; }
        internal UnitGraphNode ConversionInfo { get { return config.UnitGraph[unit]; } }

        public bool IsMemberOfUnitSystem(UnitSystem unitSystem)
        {
            return AssociatedUnitSystems.Any(us=> us.Equals(unitSystem));
        }

        IUnitMetaConfig IUnitMetaConfig.BelongsToTypeSystem(params UnitSystem[] unitSystem)
        {
            AssociatedUnitSystems = unitSystem;
            return this;
        }

        IUnitMetaConfig IUnitMetaConfig.IsPhysicalUnit(Unit unit)
        {
            if (!unit.IsFundamental)
                throw new InvalidOperationException("Only a combination of fundamental units (marked with underscore) are valid as Physical unit");
            PhysicalUnit = unit;
            return this;
        }

        IUnitMetaConfig IUnitMetaConfig.EquivalentTo(Unit unit)
        {
            config.AddCompaction(unit, this.unit);
            return this;
        }

        public IUnitMetaConfig ConvertValueBased(Unit second, Func<decimal, decimal> firstToSecond, Func<decimal, decimal> secondToFirst)
        {
            if (PhysicalUnit == null)
                PhysicalUnit = unit.DerivePhysicalUnitsFromConstituentParts();
            if (PhysicalUnit.IsDimensionless())
                throw new InvalidOperationException("You must define physical unit of the left-hand side");
            
            var unitMeta = second.GetUnitData();
            
            if (unitMeta == null || unitMeta.PhysicalUnit.IsDimensionless())
                // If right-hand side of conversion has no physical unit, we'll assume that it has the same physical unit as left-hand.
                unitMeta = (UnitMeta) config.Unit(second).IsPhysicalUnit(PhysicalUnit);

            if (unitMeta.PhysicalUnit != PhysicalUnit)
                throw new InvalidOperationException("You can only define conversions between units that are compatible as physical units");
            
            var node = config.UnitGraph.AddUnit(second);
            ConversionInfo.AddConversion(node, firstToSecond);
            node.AddConversion(ConversionInfo, secondToFirst);
            return this;
        }

        IUnitMetaConfig IUnitMetaConfig.ConvertibleTo(Unit second, Func<Measure, Measure> firstToSecond, Func<Measure, Measure> secondToFirst)
        {
            var node = config.UnitGraph.AddUnit(second);
            ConversionInfo.AddConversion(node, firstToSecond);
            node.AddConversion(ConversionInfo, secondToFirst);
            return this;
        }

        IUnitMetaConfig IUnitMetaConfig.HasPrecision(int precision)
        {
            UnitPrecision = precision;
            return this;
        }

        public int? UnitPrecision { get; private set; }

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
            if (rootUnit.PhysicalUnit.IsDimensionless())
                throw new InvalidOperationException("The unit you start with should be associated with a physical unit to start a scale.");
            
        }

        IUnitScale IUnitScale.To(Unit singleUnit, int scale)
        {
            var newUnit = config.Unit(singleUnit)
                .IsPhysicalUnit(precedingUnit.PhysicalUnit)
                .ConvertValueBased(precedingUnit.Unit, v => v*scale, v => v/scale);
            ((IUnitMetaConfig)precedingUnit).ConvertValueBased(singleUnit, v => v / scale, v => v * scale);
            precedingUnit = (UnitMeta) newUnit;
            return this;
        }
    }
}