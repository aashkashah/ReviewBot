using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBot
{
    public class UserProfile
    {
        public string Name { get; set; }
        public int Age { get; set; }

        // The list of companies the user wants to review.
        //public List<string> CompaniesToReview { get; set; } = new List<string>();

        public string MovieSelcted { get; set; }

        public string MovieReview { get; set; }
    }
}
