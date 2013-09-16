using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EditDistance.Models
{
    public class tag
    {
        public int MinDistance { get; set; }
        public string Name { get; set; }

        public tag()
        {
            MinDistance = 0;
            Name = "";
        }
    }
}