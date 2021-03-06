using Microsoft.VisualStudio.TestTools.UnitTesting;
using DemoNewsApplication.Model;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DemoNewsApplicationTest
{
    [TestClass]
    public class HackerNewsAPI
    {
        Dictionary<string,string> _Configuration = new Dictionary<string, string> { { "baseUrl", "https://hacker-news.firebaseio.com/v0/" } };
        Dictionary<string, string> _InvalidConfiguration = new Dictionary<string, string> { { "baseUrl", "https://hacker-news.firebaseio.com/v0/invalid/endpoint" } };

        [TestMethod]
        public void GetNewStories_Success()
        {
          IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(_Configuration).Build();
          ApiResponse<int[]> response =   new HackerNews(configuration).getNewStories();
          Assert.IsTrue(response.isSuccessful);
        }

        [TestMethod]
        public void GetNewStories_Fail()
        {
          IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(_InvalidConfiguration).Build();
          ApiResponse<int[]> response = new HackerNews(configuration).getNewStories();
          Assert.IsFalse(response.isSuccessful);
        }

        [TestMethod]
        public void GetNewStory_Success()
        {
            int id = 22997417;
            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(_Configuration).Build();
            ApiResponse<Article> response = new HackerNews(configuration).getStory(id);
            Assert.IsTrue(response.data.@by != null);
        }

        [TestMethod]
        public void GetNewStory_Fail()
        {
            int id = 0;
            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(_InvalidConfiguration).Build();
            ApiResponse<Article> response = new HackerNews(configuration).getStory(id);
            Assert.IsFalse(response.data.@by != null);
        }
    }
}