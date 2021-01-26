using System;

namespace Services.DTO
{
    public class AuthorDTO: IEquatable<AuthorDTO>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }

        public bool Equals(AuthorDTO other)
        {
            if (Object.ReferenceEquals(other, null)) return false;
            if (Object.ReferenceEquals(this, other)) return true;

            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            int hashAuthorName = Name == null ? 0 : Name.GetHashCode();
            int hashAuthorId = Id.GetHashCode();
            return hashAuthorName ^ hashAuthorId;
        }
    }
}
