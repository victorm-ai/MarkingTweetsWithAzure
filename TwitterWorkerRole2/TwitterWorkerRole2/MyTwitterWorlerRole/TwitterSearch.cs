using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTwitterWorlerRole
{
    public class TwitterSearch : TableEntity
    {
        public TwitterSearch(string Search)
        {
            PartitionKey = "PK";
            RowKey = Search.Replace("#", "");
            SearchTerm = Search;
        }

        public TwitterSearch() { }

        public string SearchTerm { get; set; }

        public long LastQueryId { get; set; }

        public int FavouritedTW { get; set; }
    }
}
