using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;
using IntroSE.Kanban.Backend.DataAccessLayer.DTO;
using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DAO;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System.Text.Json.Serialization;

namespace IntroSE.Kanban.Backend.BussinessLayer
{
    public class Column
    {
        //fields
        public int ColumnIndex { get; private set; }
        [JsonIgnore]
        public List<Taskk> tasks { get; private set; }
        
        public int limit { get; private set ;}
        public string name { get; private set; }
        [JsonIgnore]
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        [JsonIgnore]
        private ColumnDTO ColumnDTO;
        [JsonIgnore]
        public TaskDAO taskDAO { get; private set; }
        [JsonIgnore]
        private ColumnDAO ColumnDAO;

        //constracor for limit input
        public Column(string name, int limit, int index, int boardId)
        {
            this.ColumnIndex = index;
            tasks = new List<Taskk>();
            this.name = name;
            this.limit = limit;
            taskDAO = new TaskDAO();
            ColumnDAO = new ColumnDAO();
            ColumnDTO = new ColumnDTO(index, name, limit, boardId);
            bool success;
            success = ColumnDAO.Insert(ColumnDTO); 
            if (!success)
            {
                throw new Exception("the insertion has failed");
            }
    }


        internal Column(ColumnDTO columnDTO)
        {

            this.ColumnDTO = columnDTO;
            this.ColumnIndex = ColumnDTO.index;
            this.limit = columnDTO.limit;
            this.name =columnDTO.name;
            this.ColumnDTO = columnDTO;
            tasks = new List<Taskk>();
            TaskDAO taskDAO = new TaskDAO();
            foreach (TaskDTO taskDTO in taskDAO.Select(columnDTO.boardId, columnDTO.index)) {
                Taskk task = new Taskk(taskDTO);
                tasks.Add(task);
            }

        }

        [JsonConstructor]
        public Column(int ColumnIndex, int limit , string name )
        {
            
            this.name = name;
            this.ColumnIndex = ColumnIndex;
            this.limit = limit;
        }



        /// <summary>
        /// This method return the number of tasks in the column. 
        /// </summary>

        public int getTasksNumber()
        {
            return this.tasks.Count;
        }



        /// <summary>
        /// This method edit's the column's limit. 
        /// </summary>
        /// <param name="newLimit">The new column's limit.</param>
        /// <summary>
        public void editLimit(int newLimit)
        {
            log.Info("edit the column's limit");
            if (newLimit == -1)
            {
                log.Debug("the new limit is maxValue");
                limit = int.MaxValue;
                bool success = false;
                success = ColumnDAO.Update(ColumnDTO, "limit");
                if (!success)
                {
                    throw new Exception("ths update has failed");
                }
            }
            else if(newLimit < 0)
            {
                log.Error("limit can't be smaller than zero");
                throw new Exception("limit can't be smaller than zero");
            }
            else if (tasks.Count() > newLimit)
            {
                log.Error("the new limit, can't be smaller than the amount of tasks that are already in the column");
                throw new Exception("the new limit, can't be smaller than the amount of tasks that are already in the column");
            }
            else {

                limit = newLimit;
                bool success = false;
                success =ColumnDAO.Update(ColumnDTO, "limit", newLimit);
                if (!success)
                {
                    throw new Exception("ths update has failed");
                }
            }
            log.Info("edit the column's limit was done successfully");
        }






        /// <summary>
        /// This method return a task by Id. 
        /// </summary>
        /// <param name="taskId">The task's ID.</param>
        /// <summary>
        public Taskk getTask(int taskId)
        {
            log.Info("getting the task" + taskId + "from the colums");
            foreach (Taskk task in tasks)
            {
                if (task.Id == taskId)
                {
                    return task;
                }
            }
            log.Error("taskId is not in the column");
            throw new Exception("taskId is not in the column");
            
        }

        /// <summary>
        /// This method adds new task. 
        /// </summary>
        /// <param name="task">The new task.</param>
        /// <summary>
        public string taskAdd(Taskk task)
        {

            if (task == null)
            {
                throw new Exception("task cant be null");
            }
            log.Info("adding" + task + "to the column");
            string output = "";
            tasks.Add(task);
            
            log.Info(task + "was added successfully to the column");
            return output;
        }

        /// <summary>
        /// This method removing task. 
        /// </summary>
        /// <param name="taskId">The ID of the task is being removed.</param>
        /// <summary>
        public string taskRemove(int taskId)
        {
            log.Info("removing number" + taskId + "from the column");
            string output = "";
            bool found = false;
            for (int TaskIndex=0; TaskIndex< tasks.Count() & !found; TaskIndex++)
            {
                if (tasks.ElementAt(TaskIndex).Id== taskId)
                {
                    tasks.Remove(tasks.ElementAt(TaskIndex));
                    log.Info(taskId + "was removed successfully from the column");
                }
            }
            
            return output;

        }


    }

}
