using System;
namespace API.Entities
{
    public class Animal
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Used { get; set; }
    }
}