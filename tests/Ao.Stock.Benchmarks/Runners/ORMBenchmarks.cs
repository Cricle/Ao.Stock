using Ao.Stock.Mirror;
using BenchmarkDotNet.Attributes;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Ao.Stock.Benchmarks.Runners
{
    [MemoryDiagnoser]
    public class ORMBenchmarks
    {
        protected SqlConnection connection;
        private string sql;
        [GlobalSetup]
        public void Init()
        {
            sql = "SELECT * FROM [Posts]";
            connection = new SqlConnection("Data Source=.;Initial Catalog=test;Integrated Security=True");
            connection.Open();

            HandCode().GetAwaiter().GetResult();
            ORM().GetAwaiter().GetResult();
            Dapper().GetAwaiter().GetResult();
        }
        [Benchmark(Baseline = true)]
        public async Task HandCode()
        {
            var posts = new List<Post>();
            using (var comm = connection.CreateCommand(sql))
            {
                using (var reader = await comm.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var post = new Post
                        {
                            Id = reader.GetInt32(nameof(Post.Id)),
                            Text = reader.GetString(nameof(Post.Text)),
                            CreationDate = reader.GetDateTime(nameof(Post.CreationDate)),
                            LastChangeDate = reader.GetDateTime(nameof(Post.LastChangeDate)),
                            Counter1 = reader.IsDBNull(nameof(Post.Counter1)) ? null : reader.GetInt32(nameof(Post.Counter1)),
                            Counter2 = reader.IsDBNull(nameof(Post.Counter2)) ? null : reader.GetInt32(nameof(Post.Counter2)),
                            Counter3 = reader.IsDBNull(nameof(Post.Counter3)) ? null : reader.GetInt32(nameof(Post.Counter3)),
                            Counter4 = reader.IsDBNull(nameof(Post.Counter4)) ? null : reader.GetInt32(nameof(Post.Counter4)),
                            Counter5 = reader.IsDBNull(nameof(Post.Counter5)) ? null : reader.GetInt32(nameof(Post.Counter5)),
                            Counter6 = reader.IsDBNull(nameof(Post.Counter6)) ? null : reader.GetInt32(nameof(Post.Counter6)),
                            Counter7 = reader.IsDBNull(nameof(Post.Counter7)) ? null : reader.GetInt32(nameof(Post.Counter7)),
                            Counter8 = reader.IsDBNull(nameof(Post.Counter8)) ? null : reader.GetInt32(nameof(Post.Counter8)),
                            Counter9 = reader.IsDBNull(nameof(Post.Counter9)) ? null : reader.GetInt32(nameof(Post.Counter9)),
                        };
                        posts.Add(post);
                    }
                }
            }
        }
        [Benchmark]
        public async Task ORM()
        {
            await connection.ExecuteReaderAsync<Post>(sql);
        }
        [Benchmark]
        public async Task Dapper()
        {
            await connection.QueryAsync<Post>(sql);
        }
        public class Post
        {
            public int Id { get; set; }
            public string? Text { get; set; }
            public DateTime CreationDate { get; set; }
            public DateTime LastChangeDate { get; set; }
            public int? Counter1 { get; set; }
            public int? Counter2 { get; set; }
            public int? Counter3 { get; set; }
            public int? Counter4 { get; set; }
            public int? Counter5 { get; set; }
            public int? Counter6 { get; set; }
            public int? Counter7 { get; set; }
            public int? Counter8 { get; set; }
            public int? Counter9 { get; set; }
        }
    }
}
