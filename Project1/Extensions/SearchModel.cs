using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.Extensions
{
    public class SearchModel
    {
        public List<SearchField> SearchFields { get; set; } = new List<SearchField>();
        public string OrderBy { get; set; }
        public string OrderType { get; set; }
        public int skip { get; set; } = 0;
        public int take { get; set; } = 10;

    }
    public class SearchField
    {
        public string FieldName { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }

    }
}
