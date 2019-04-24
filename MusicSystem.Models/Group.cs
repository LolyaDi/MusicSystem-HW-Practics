using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicSystem.Models
{
    public class Group: Entity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public ICollection<Song> Songs { get; set; }
    }
}
