using System;
using System.ComponentModel.DataAnnotations;

namespace MusicSystem.Models
{
    public class Song: Entity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public string Lyrics { get; set; }

        [Required]
        public TimeSpan Length { get; set; }

        [Required]
        public int Rating { get; set; }
    }
}