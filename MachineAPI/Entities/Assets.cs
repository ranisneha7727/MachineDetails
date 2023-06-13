using System.ComponentModel.DataAnnotations;

namespace MachineAPI.Entities
{
    public class Assets
    {
        [MaxLength(40)]
        public string AssetName { get; set; }
        [MaxLength(10)]
        public string SeriesNo { get; set; }
    }
}
