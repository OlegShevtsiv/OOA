using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.ViewModels.Home
{
    public class RateViewModel
    {
        public string UserId { get; set; }
        public string RatedEssenceId { get; set; }
        public decimal Value { get; set; }
    }
}
