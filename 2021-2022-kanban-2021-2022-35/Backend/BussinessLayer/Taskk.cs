using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;
using IntroSE.Kanban.Backend.DataAccessLayer.DAO;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System.Text.Json.Serialization;

namespace IntroSE.Kanban.Backend.BussinessLayer
{
    public class Taskk
    {
        //fields
        
        public int Id { get; private set; }
        public DateTime CreationDate { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }

        public DateTime DueDate { get; private set; }
        [JsonIgnore]
        public string status { get;private set; }
        [JsonIgnore]
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        
        public string assignee { get; private set; }
        [JsonIgnore]
        private TaskDAO taskDAO;
        [JsonIgnore]
        public TaskDTO taskDTO { get; private set; }
        [JsonIgnore]
        public int boardId { get; private set; }
        [JsonIgnore]
        public int columnIndex { get; private set; }


        //constracor
        public Taskk(string title, string description, DateTime dueDate, int taskId , int columnordinal , int boardId )
        {
            this.columnIndex = columnordinal;
            this.boardId = boardId;
            this.Title = title;
            this.Description = description;
            this.DueDate = dueDate;
            this.CreationDate = DateTime.Now;
            this.Id = taskId;
            this.assignee = null;
            taskDAO = new TaskDAO();
            taskDTO = new TaskDTO(taskId, title, description, dueDate.ToString(), CreationDate.ToString(), columnordinal, assignee, boardId);
            bool success = false;
            success = taskDAO.Insert(taskDTO);
            if (!success)
            {
                throw new Exception("the insertion has Failed");
            }
            
        }

        
        public Taskk(TaskDTO taskDTO)
        {
            this.columnIndex = taskDTO.column;
            this.boardId = taskDTO.boardId;
            this.Title = taskDTO.title;
            this.Description = taskDTO.description;
            this.DueDate = DateTime.Parse(taskDTO.dueDate);
            this.CreationDate = DateTime.Parse(taskDTO.creationDate);
            this.Id = taskDTO.id;

        }

        [JsonConstructor]
        public Taskk (int id , DateTime creationDate , string title , string description , DateTime dueDate , string assignee)
        {
            this.assignee = assignee;
            this.Id = id;
            this.CreationDate = creationDate;
            this.Title = title;
            this.Description = description;
            this.DueDate = dueDate;
        }

        /// <summary>
        /// This method returns the task's status. 
        /// <summary>
        public void move()
        {
            this.columnIndex++;
        }


        /// <summary>
        /// This method sets new assigne to the task. 
        /// </summary>
        /// <param name="NewAsignee">The new assigned to the task.</param>
        /// <summary>
        public void setAssignee(string NewAsignee)
        {
            log.Info("try to set new assigne");
            bool success = false;
            success =taskDAO.Update(Id, "assignee", NewAsignee);
            if (!success)
            {
                throw new Exception("the update has failed");
            }
            this.assignee = NewAsignee;
            log.Debug("the assignee chacnged seccussfully");
        }
 

        /// <summary>
        /// This method edits the task's title. 
        /// </summary>
        /// <param name="newTitle">The new title to the task.</param>
        /// <summary>
        public string taskEditTitle(string newTitle)
        {

            if(newTitle == null)
            {
                throw new Exception("new title cant be null");
            }
            log.Info("edit the task's title");
            string output = "";

            if (newTitle.Length > 50 || newTitle.Length==0)
            {
                log.Error("new title must be shorter then 50 chars and not empty");
                throw new Exception("new title must be shorter then 50 chars and not empty");
            }
            Title = newTitle;
            bool success = false;
            success = taskDAO.Update(taskDTO, "title", newTitle);
            if (!success)
            {
                throw new Exception("the update has failed");
            }
            log.Info("the task's title was editted successfully");
            return output;
        }

        /// <summary>
        /// This method edits the task's description. 
        /// </summary>
        /// <param name="newDescription">The new description to the task.</param>
        /// <summary>
        public string taskEditDescription(string newDescription)
        {
            if (newDescription == null)
            {
                throw new Exception("new title cant be null");
            }
            log.Info("edit the task's description");
            string output = "{}";
            if (Description.Equals(newDescription)) {
                log.Error("Error: can't change to the same value");
                throw new Exception("Error: can't change to the same value");
            }
            if (newDescription.Length > 300)
            {
                log.Error("the description must be shorter then 300 chars");
                throw new Exception("the description must be shorter then 300 chars");
            }
            Description = newDescription;
            bool success = false;
            success = taskDAO.Update(taskDTO, "description", newDescription);
            if (!success)
            {
                throw new Exception("the update has Failed");
            }
            log.Info("the task's description was editted successfully");
            return output;
        }

        /// <summary>
        /// This method edits the task's due date. 
        /// </summary>
        /// <param name="newDueDate">The new due date to the task.</param>
        /// <summary>
        public string taskEditDueDate(DateTime newDueDate)
        {
            log.Info("edit the task's description");
            string output = "{}";

            if (newDueDate.CompareTo(DateTime.Now) < 0)
            {
                log.Error("Error: due date can't be in the past");
                throw new Exception("Error: due date can't be in the past");
            }
            DueDate = newDueDate;
            bool success = false;
            success = taskDAO.Update(taskDTO, "dueDate", newDueDate.ToString());
            if (!success)
            {
                throw new Exception("the update has failed");
            }
            log.Info("the task's due date was editted successfully");
            return output;
        }


    }
}
