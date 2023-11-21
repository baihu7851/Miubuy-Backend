using System.Collections.Generic;

namespace Miubuy.Models.TagApiModel
{
    public class TagsViewModel
    {
        public List<Tag> Tags { get; set; } = new List<Tag>();

        public class Tag
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Color { get; set; }
            public int RoomCount { get; set; }
        }
    }
}