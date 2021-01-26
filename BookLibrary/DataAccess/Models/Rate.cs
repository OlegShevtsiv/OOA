
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class Rate
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string BookId { get; set; }
        public decimal Value { get; set; }
        public Rate() { }
        public Rate(string _id, string _userId, string _bookId, decimal _value)
        {
            Id = _id;
            UserId = _userId;
            BookId = _bookId;
            Value = _value;
        }
    }
}
