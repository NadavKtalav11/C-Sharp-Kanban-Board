using IntroSE.Kanban.Backend.BussinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    public class TaskModel : NotifiableModelObject
    {

        public string TaskTitle { get; set; }


        private string Description;
        public string description
        {
            get => Description;
        }

        private string dueDate;
        public string DueDate
    {
            get => dueDate;
        }
        private string CreationDate;
        public string creationDate
        {
            get => CreationDate;
        }
        private string Assignee;
        public string assignee
        {
            get => Assignee;
        }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                this._title = value;
                RaisePropertyChanged("Title");
            }
        }
        private string Id;
        public string id
        {
            get => Id;
            set
            {
                this.Id = value;
                RaisePropertyChanged("Body");
            }
        }


        private string UserEmail; //storing this user here is an hack becuase static & singletone are not allowed.
        public TaskModel(BackendController controller,int id,  string title, string description ,DateTime dueDate, DateTime creationDate ,string assignee , ColumnModel column) : base(controller)
        {
            this.id = "task ID- " + id;
            this.Description = "description- " +description;
            this.dueDate = "dueDate-" + dueDate.ToString();
            this.CreationDate = "creationDate- " +creationDate.ToString();
            this.Title = "title- " + title;
            if (assignee == null)
            {
                this.Assignee = "no assignee";
            }
            else
            {
                this.Assignee = "assignee- " + assignee;
            }
            
        }

        public TaskModel(BackendController controller, Taskk task,  ColumnModel column) : this(controller,task.Id , task.Title,task.Description,  task.DueDate,  task.CreationDate, task.assignee, column) { }



    }
}

