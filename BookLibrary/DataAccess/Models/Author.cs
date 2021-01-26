namespace DataAccess.Models
{
    public class Author
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }

        public Author() { }
        public Author(string _id, string _name, string _surname, string _description, byte[] _image)
        {
            Id = _id;
            Name = _name;
            Surname = _surname;
            Description = _description;
            Image = _image;
        }
    }
}
