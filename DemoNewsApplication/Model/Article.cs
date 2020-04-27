using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoNewsApplication.Model
{
    public class Article
    {
      public string id { get; set; }
      public bool deleted { get; set; }
      public string type { get; set; }
      public string by { get; set; }
      public long time { get; set; }
      public string friendlytime { get; set; }
      public string text { get; set; }
      public bool dead { get; set; }
      public int parent { get; set; }
      public string poll { get; set; }
      public int[] kids { get; set; }
      public string url { get; set; }
      public string score {get;set;}
      public string title { get; set; }
      public int descendants { get; set; }
    }
}
