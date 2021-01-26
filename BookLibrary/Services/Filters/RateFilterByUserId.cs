using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Filters
{
    public class RateFilterByUserId: IFilter
    {
        public string UserId { get; set; }
    }
}
