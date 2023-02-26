using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer.Classes
{
    public class mydesirializeClass
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class BoardDTO
        {
            public int id { get; set; }
            public string name { get; set; }
            public string owner { get; set; }
        }

        public class Column
        {
            public int ColumnIndex { get; set; }
            public List<Task> tasks { get; set; }
            public int limit { get; set; }
            public TaskDAO taskDAO { get; set; }
        }

        public class JoinedMembers
        {
            [JsonProperty("nadavkt@gmail.com")]
            public NadavktGmailCom NadavktGmailCom { get; set; }
        }

        public class NadavktGmailCom
        {
            public string email { get; set; }
            public string password { get; set; }
            public bool loggedIn { get; set; }
        }

        public class ReturnValue
        {
            public List<Column> columns { get; set; }
            public string name { get; set; }
            public int boardId { get; set; }
            public string Owner { get; set; }
            public int counterTaskId { get; set; }
            public JoinedMembers joinedMembers { get; set; }
            public BoardDTO boardDTO { get; set; }
        }

        public class Root
        {
            public ReturnValue ReturnValue { get; set; }
        }

        public class Task
        {
            public int Id { get; set; }
            public DateTime CreationDate { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime DueDate { get; set; }
            public int boardId { get; set; }
        }

        public class TaskDAO
        {
        }
    }
}
