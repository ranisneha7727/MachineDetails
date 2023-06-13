using System.ComponentModel.DataAnnotations;

namespace MachineAPI.Entities
{
    public class Machine
    {
        [Required(ErrorMessage = "You should provide machine name.")]
        [MaxLength(20)]
        public string MachineName { get; set; }
        public List<Assets?> Assets { get; set; }
    }
}