using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using TweetSharp;

namespace MyTwitterWorlerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        private string PartitionKey { get; set; }
        private string TableName { get; set; }
        private List<TwitterSearch> Tweets { get; set; }
        public TwitterService TwitterInstance { get; set; }
        private CloudTable MetadataTable { get; set; }

        public WorkerRole()
        {
            this.PartitionKey = "PK";
            this.TableName = "TableTweets";

            this.Tweets = new List<TwitterSearch>()
            {
                new TwitterSearch("#MyAzure"),
            };
        }

        private void PersistQueryTable(IEnumerable<TwitterSearch> queries)
        {
            var BatchOperation = new TableBatchOperation();

            foreach (var query in queries)
                BatchOperation.InsertOrReplace(query);

            MetadataTable.ExecuteBatch(BatchOperation);
        }
        private List<TwitterSearch> GetTweets()
        {
            var StorageAccount =
            CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString"));

            var TableClient =
            StorageAccount.CreateCloudTableClient();

            var Table =
            TableClient.GetTableReference(TableName);

            MetadataTable = Table;

            if (!Table.Exists())
            {
                Table.CreateIfNotExists();
                return Tweets;
            }

            var Query =
            new TableQuery<TwitterSearch>().Where(TableQuery.GenerateFilterCondition("PartitionKey",
                                                  QueryComparisons.Equal, PartitionKey));

            return Table.ExecuteQuery(Query).ToList();
        }
        public void ProcessQuery(TwitterSearch Query)
        {
            var SearchResults = TwitterInstance.Search(new SearchOptions
            {
                Count = 9,
                Q = Query.SearchTerm,
                SinceId = Query.LastQueryId,
                Resulttype = TwitterSearchResultType.Popular,
            });

            if (!SearchResults.Statuses.Any()) return;

            foreach (var Tweet in SearchResults.Statuses)
            {
                TwitterInstance.FavoriteTweet(new FavoriteTweetOptions { Id = Tweet.Id });
                Query.FavouritedTW++;
            }

            Query.LastQueryId = SearchResults.Statuses.Max(x => x.Id);
        }

        public override void Run()
        {
            MyTwitter MyTwitterAccount = new MyTwitter();

            TwitterInstance =
            new TwitterService(MyTwitterAccount.API_Key, MyTwitterAccount.API_Secret);

            TwitterInstance.AuthenticateWith(MyTwitterAccount.AccessToken,
                                                    MyTwitterAccount.TokenSecret);

            Tweets = GetTweets();

            while (true)
            {
                Thread.Sleep(1000 * 10);

                foreach (var TweetFound in Tweets)
                {
                    try
                    {
                        ProcessQuery(TweetFound);
                        Trace.TraceError("Success: " + TweetFound.RowKey);
                    }
                    catch
                    {
                        Trace.TraceError("Fail: " + TweetFound.RowKey);
                    }
                }

                PersistQueryTable(Tweets);
            }
        }

        public override bool OnStart()
        {
            // Establecer el número máximo de conexiones simultáneas
            ServicePointManager.DefaultConnectionLimit = 12;

            // Para obtener información sobre cómo administrar los cambios de configuración
            // consulte el tema de MSDN en http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("MyTwitterWorlerRole has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("MyTwitterWorlerRole is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("MyTwitterWorlerRole has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Reemplazar lo siguiente por su propia lógica.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000);
            }
        }

       
    }
}
