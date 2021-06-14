using System;

namespace API.DTOs
{
    public class AnimalDto
    {
        public string Type { get; set; }
        public bool Deleted { get; set; }
        public string Source { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Used { get; set; }
    }
}