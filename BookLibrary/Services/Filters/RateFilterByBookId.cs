using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Filters
{
    public class RateFilterByBookId: IFilter
    {
        public string BookId { get; set; }
    }
}
