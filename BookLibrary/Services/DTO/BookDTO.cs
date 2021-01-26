using System;

namespace Services.DTO
{
    public class BookDTO: IEquatable<BookDTO>
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string AuthorId { get; set; }

        public int Year { get; set; }

        public byte[] Image { get; set; }
        public byte[] FileBook { get; set; }
        public decimal Rate { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public uint RatesAmount { get; set; }

        public bool Equals(BookDTO other)
        {
            if (Object.ReferenceEquals(other, null)) return false;
            if (Object.ReferenceEquals(this, other)) return true;

            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            int hashBookTitle = Title == null ? 0 : Title.GetHashCode();
            int hashBookId = Id.GetHashCode();
            return hashBookTitle ^ hashBookId;
        }
    }
}
