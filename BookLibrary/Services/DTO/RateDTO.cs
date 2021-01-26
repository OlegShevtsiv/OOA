using System;
using System.Collections.Generic;
using System.Text;

namespace Services.DTO
{
    public class RateDTO : IEquatable<RateDTO>
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string BookId { get; set; }
        public decimal Value { get; set; }

        public bool Equals(RateDTO other)
        {
            if (Object.ReferenceEquals(other, null)) return false;
            if (Object.ReferenceEquals(this, other)) return true;

            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            int hashBookId = BookId.GetHashCode();
            int hashUserId = UserId.GetHashCode();
            return hashBookId ^ hashUserId;
        }
    }
}
