namespace Countries.Models
{
    using System;
    using System.Collections.Generic;
    public class RegionalBloc
   {
        public string acronym { get; set; }
        public string name { get; set; }
        public List<object> otherAcronyms { get; set; }
        public List<string> otherNames { get; set; }
   }
}
