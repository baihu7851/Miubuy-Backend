namespace Services.Models
{
    public class TagModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public bool Delete { get; set; }
        public int RoomCount { get; set; }
    }
}