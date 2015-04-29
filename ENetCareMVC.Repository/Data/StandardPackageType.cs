using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENetCareMVC.Repository.Data
{
    [Serializable]
    public class StandardPackageType
    {
        public int PackageTypeId { get; set; }
        public string Description { get; set; }
        public int NumberOfMedications { get; set; }
        public ShelfLifeUnitType ShelfLifeUnitType { get; set; }
        public int ShelfLifeUnits { get; set; }
        public bool TemperatureSensitive { get; set; }
        public decimal Value { get; set; }

        public override string ToString()
        {
            return NumberOfMedications + " / " + Description;
        }

    }
}
