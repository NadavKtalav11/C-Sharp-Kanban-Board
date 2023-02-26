using IntroSE.Kanban.Backend.DataAccessLayer.DAO;
using IntroSE.Kanban.Backend.DataAccessLayer.DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    public class TaskDTO : DTOAbstract
    {
        //fields


        public const string taskIdColumnName = "id";
        public const string taskTitleColumnName = "title";
        public const string taskdescriptionColumnName = "description";
        public const string taskdueDateColumnName = "dueDate";
        public const string taskCreationDateColumnName = "creationDate";
        public const string taskColumnColumnName = "column";
        public const string taskAssigneeColumnName = "assignee";
        public const string taskBoardColumnName = "board";

        private int _id;
        public int id { get => _id; set { _id = value; controller.Update(id, taskIdColumnName, value); } }
        private string _title;
        public string title { get => _title; set { _title = value; controller.Update(id, taskTitleColumnName, value); } }

        private string _description;
        public string description { get => _description; set { _description = value; controller.Update(id, taskdescriptionColumnName, value); } }

        private string _dueDate;
        public string dueDate { get => _dueDate; set { _dueDate = value; controller.Update(id, taskdueDateColumnName, value); } }
        private string _creationDate;
        public string creationDate { get => _creationDate; set { _creationDate = value; controller.Update(id, taskCreationDateColumnName, value); } }

        private int _column;
        public int column { get => _column; set { _column = value; controller.Update(id, taskColumnColumnName, value); } }

        private string _assignee;
        public string assignee { get => _assignee; set { _assignee = value; controller.Update(id, taskAssigneeColumnName, value); } }


        private int _boardId;
        public int boardId { get => _boardId; set { _boardId = value; controller.Update(id, taskAssigneeColumnName, value); } }



        //constractor
        public TaskDTO(int id, string title, string description, string dueDate, string creationDate, int column, string assignee, int boardId) : base(new TaskDAO())
        {
            this._id = id;
            this._title = title;
            this._description = description;
            this._dueDate = dueDate;
            this._creationDate = creationDate;
            this._column = column;
            this._assignee = assignee;
            this._boardId = boardId;
        }
        public TaskDTO (int id) : base(new TaskDAO())
        {
            this.id = id;
        }
    }

  
}




