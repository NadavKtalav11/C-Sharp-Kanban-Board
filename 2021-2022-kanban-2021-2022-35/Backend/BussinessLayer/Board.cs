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
using IntroSE.Kanban.Backend.DataAccessLayer.DAOs;
using IntroSE.Kanban.Backend.DataAccessLayer.DAO;
using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DAO_s;
using IntroSE.Kanban.Backend.DataAccessLayer.DTO_s;
using System.Text.Json.Serialization;

namespace IntroSE.Kanban.Backend.BussinessLayer
{
    public class Board
    {
        //fields

        [JsonIgnore]
        public List<Column> columns { get;private set; }
        public string name { get;private set; }
        [JsonIgnore]
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
 
        public int boardId { get;private set; }
        public string Owner { get;private set; }
        [JsonIgnore] 
        public int counterTaskId { get; private set; }
        [JsonIgnore]
        public Dictionary<string , User> joinedMembers { get; private set; }
        [JsonIgnore]
        private ColumnDAO columnDAO;
        [JsonIgnore]
        private BoardDAO boardDAO;
        [JsonIgnore]
        public BoardDTO boardDTO { get; private set; }
        [JsonIgnore]
        private BoardsMembersDAO boardsMembersDAO;
        [JsonIgnore]
        private BoardsMembersDTO boardsMembersDTO;


        //constractor
        public Board(string name, User user, int Id) { 
        
            this.boardId = Id;
            columns = new List<Column>();
            Column backlog = new Column("Backlog" , -1 , 0 , boardId);
            Column inProgress = new Column("in Progress" , -1 ,1 ,boardId);
            Column done = new Column("Done" , -1 , 2 , boardId);
            columns.Add(backlog);
            columns.Add(inProgress);
            columns.Add(done);
            this.name = name;
            this.Owner = user.email;
            this.counterTaskId = 0;
            this.joinedMembers = new Dictionary<string, User>();
            joinedMembers.Add(user.email, user);
            columnDAO = new ColumnDAO();
            boardDAO = new BoardDAO();
            boardDTO = new BoardDTO(Id, name, user.email);
            bool boardSuccess;
            boardSuccess = boardDAO.Insert(boardDTO);
            if (!boardSuccess)
            {
                throw new Exception("the insert has failed");
            }
            bool membersSuccess;
            boardsMembersDAO = new BoardsMembersDAO();
            boardsMembersDTO = new BoardsMembersDTO(boardId, user.email);
            membersSuccess= boardsMembersDAO.Insert(boardsMembersDTO);
            if (!membersSuccess)
            {
                throw new Exception("the insertion has  failed");
            }
        }

        public Board (BoardDTO boardDTO)
        {
            this.boardId = boardDTO.id;
            this.name = boardDTO.name;
            this.Owner = boardDTO.owner;
            this.joinedMembers = new Dictionary<string, User>();
            this.columns = new List<Column>();
            ColumnDAO columnDAO = new ColumnDAO();
            columns.Add(new Column(columnDAO.Select(boardId, 0)));
            columns.Add(new Column(columnDAO.Select(boardId, 1)));
            columns.Add(new Column(columnDAO.Select(boardId, 2)));
            TaskDAO taskDAO= new TaskDAO();
            counterTaskId = taskDAO.SelectTasksNextId(boardId)+ 1;
            
        }

        [JsonConstructor]
        public Board(string name , int boardId , string Owner)
        {
            this.name= name;
            this.boardId = boardId;
            this.Owner = Owner;
        }




        /// <summary>
        /// This method removes user's membership from the board. 
        /// </summary>
        /// <param name="email">The user's email that is being removed.</param>
        /// <summary> 
        public bool removeUser(string email)
        {
            if (membersContain(email))
            {
                log.Info("remove the user from " +name + "the members list");
                joinedMembers.Remove(email);
                return true;
            }
            return false;
        }

        /// <summary>
        /// This method returns all the users that are members in the board. 
        /// <summary>
        public List<string> getJoinedMembers()
        {
            List<string> membersList = new List<string>();
            foreach (User user in joinedMembers.Values)
            {
                membersList.Add(user.email);
            }
            log.Info("get the board's columns");
            return membersList;
        }

        /// <summary>
        /// This method returns a specific column in the board. 
        /// </summary>
        /// <param name="columnIndex">The column's index that we want to return.</param>
        /// <summary> 
        public Column GetColumn(int columnIndex)
        {
            if (columnIndex < 0 | columnIndex > 2)
            {
                throw new Exception("task index must be between 0 and 2");
            }
            log.Warn("get the a specific column");
            return columns[columnIndex];
        }

 

        /// <summary>
        /// This method sets an owener to the board. 
        /// <summary> 
        public void setOwner(string email)
        {
            
            log.Info("set new owner to the board");
            Owner = email;
            
            bool succes = boardDAO.Update(boardId, "owner", email);
            if (!succes)
            {
                throw new Exception("the update has failed");
            }
        }

        /// <summary>
        /// This method edits the board's name. 
        /// </summary>
        /// <param name="newName">The new name to board after the edit.</param>
        /// <summary> 
        public void boardEditName(string newName)
        {
            if (newName == null || newName.Equals(""))
            {
                throw new Exception("name cant be null or empty");
            }
            log.Warn("edit the board's name");
            if (newName.Equals(name))
            {
                log.Error(newName + "equals the old name");
                throw new Exception("cant change to the same name");
            }

            name = newName;
            bool success;
            success = boardDAO.Update(this.boardId, "name", newName);
            if (!success)
            {
                throw new Exception("the update has failed");
            }
            log.Debug("edit the board's name was done successfully");

        }


        /// <summary>
        /// This method chekcs if a user is in the boards list of members. 
        /// </summary>
        /// <param name="email">The email of the user that we check.</param>
        /// <summary>
        public bool membersContain(string email)
        {
            User user = joinedMembers.GetValueOrDefault(email);
            if (user != null)
            {
                return true;
            }
            log.Info("this user in not member in " + name);
            return false;
        }

        /// <summary>
        /// This method adds a new task. 
        /// </summary>
        /// <param name="title">The task's title.</param>
        /// <param name="description">The task's description.</param>
        /// <param name="dueDate">The task's due date.</param>
        /// <summary> 
        public void addNewTask(string title, string description, DateTime dueDate)
        {
            title = title.Trim();
            description = description.Trim();
            log.Info("adding new task to the board");
            log.Warn("checks if the backlog colums is not full and task's name is not null");
            if(GetColumn(0).limit == -1 ||
                GetColumn(0).limit > GetColumn(0).tasks.Count())
            {
                if (title == null || description == null)
                {
                    throw new Exception("cant insert null");
                }
                if (title.Length == 0 || title.Length > 50)
                {
                    throw new Exception("title length must be between 1-50");
                }
                if (description.Length > 300)
                {
                    throw new Exception("description must be shorter then 300 chars");
                }
                if (dueDate.CompareTo(DateTime.Now) < 0) 
                {
                    throw new Exception("due date cannot be in the past");
                }
                int Backlog = 0;
                Taskk newTaskk = new Taskk(title, description, dueDate, counterTaskId , Backlog, boardId );
                counterTaskId++;
                GetColumn(0).taskAdd(newTaskk);
            }
            else 
            {
                log.Error("the column is full");
                throw new Exception("the backlog column is full");
            }
        }



        /// <summary>
        /// This method adding user to the board's members. 
        /// </summary>
        /// <param name="user">The user that is being added.</param>
        /// <summary> 
        public void joinAMember(User user)
        {
            
            joinedMembers.Add(user.email,user );
            boardsMembersDTO = new BoardsMembersDTO(boardId, user.email);
            bool success;
            boardsMembersDAO=new BoardsMembersDAO();
            success = boardsMembersDAO.Insert(boardsMembersDTO);
  
            if (!success)
            {
                throw new Exception("the insert has failed");
            }
            log.Info("the user is now a member in " + name);
        }
        /// <summary>
        /// This method adding user to the board's members in load Data. 
        /// </summary>
        /// <param name="user">The user that is being added.</param>
        /// <summary> 

        public void joinAMemberLOadDAta(User user)
        {
            joinedMembers.Add(user.email, user);
        }

            /// <summary>
            /// This method edits the task's tile. 
            /// </summary>
            /// <param name="email">The user's email.</param>
            /// <param name="taskId">The Id of task.</param>
            /// <param name="columnIndex">The index of the colums that the task is in it.</param>
            /// <param name="newTitle">The new title to the task.</param>
            /// <summary> 
        public void taskEditTitle(string email , int taskId, int columnIndex, string newTitle)
        {
            if (columnIndex == 2)
            {
                log.Error("done tasks cant be change");
                throw new Exception("done tasks cant change");
            }
            string assignee = GetColumn(columnIndex).getTask(taskId).assignee;
            if (assignee == null | assignee!=email )
            {
                log.Error(email + "is not the assignee");
                throw new Exception("only the assignee can edit tasks");
            }
            GetColumn(columnIndex).getTask(taskId).taskEditTitle(newTitle);
        }

        /// <summary>
        /// This method edits the task's due date. 
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="taskId">The Id of task.</param>
        /// <param name="columnIndex">The index of the colums that the task is in it.</param>
        /// <param name="newDueDate">The new due date to the task.</param>
        /// <summary> 
        public void taskEditDueDate(string email, int taskId, int columnIndex, DateTime newDueDate)
        {
            log.Error("done tasks cant be change");
            if (columnIndex == 2)
            {
                throw new Exception("done tasks cant change");
            }
            string assignee = GetColumn(columnIndex).getTask(taskId).assignee;
            if (assignee == null | assignee != email)
            {
                log.Error(email + "is not the assignee");
                throw new Exception("only the assignee can edit tasks");
            }
            GetColumn(columnIndex).getTask(taskId).taskEditDueDate(newDueDate);
        }

        /// <summary>
        /// This method edits the task's description. 
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="taskId">The Id of task.</param>
        /// <param name="columnIndex">The index of the colums that the task is in it.</param>
        /// <param name="newDescription">The new description to the task.</param>
        /// <summary>
        public void taskEditDescription(string email, int taskId, int columnIndex, string newDescription)
        {

            if (columnIndex == 2)
            {
                log.Error("done tasks cant be change");
                throw new Exception("done tasks cant change");
            }
            string assignee = GetColumn(columnIndex).getTask(taskId).assignee;
            if (assignee == null | assignee != email)
            {
                log.Error(email + "is not the assignee");
                throw new Exception("only the assignee can edit tasks");
            }
            GetColumn(columnIndex).getTask(taskId).taskEditDescription(newDescription);
        }

        /// <summary>
        /// This method moves task to the next colums if its possibe. 
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="taskId">The Id of task.</param>
        /// <param name="columnIndex">The index of the colums that the task is in it.</param>
        /// <summary>
        public void taskMove(string email, int taskId, int columnIndex)
        {
            
            string assignee = GetColumn(columnIndex).getTask(taskId).assignee;
            if (assignee == null | assignee != email)
            {
                throw new Exception("only the assignee can move his tasks");
            }
            if (columnIndex == 2)
            {
                log.Error("done tasks cant be change");
                throw new Exception("done tasks cant move on");
            }
            if (GetColumn(columnIndex + 1).limit <= GetColumn(columnIndex).getTasksNumber() & GetColumn(columnIndex + 1).limit!=-1)
            {
                log.Error("column number " + columnIndex+1 + "is full");
                throw new Exception("cant move task because of the next column limit");
            }
            Taskk task = GetColumn(columnIndex).getTask(taskId);
            
            GetColumn(columnIndex).taskRemove(taskId);
            GetColumn(columnIndex + 1).taskAdd(task);
            task.move();
            TaskDAO taskDAO = new TaskDAO();
            bool success;
            success =taskDAO.Update(taskId, boardId, "column", columnIndex + 1);
            if (!success)
            {
                throw new Exception("the update has failed");
            }

        }

        /// <summary>
        /// This method removing the user from the board's members. 
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <summary>
        public void leaveBoard(string email)
        {
            if (!membersContain(email))
            {
                throw new Exception("this user hasnt joined the board");
            }
            this.joinedMembers.Remove(email);
            for (int currColumnIndex = 0; currColumnIndex < 3; currColumnIndex++)
            {
                foreach (Taskk task in this.GetColumn(currColumnIndex).tasks)
                {
                    if (task.assignee != null && task.assignee.Equals(email))
                    {
                        task.setAssignee(null);

                    }
                }
            }
            bool success;
            success= boardsMembersDAO.Delete( email, boardId);
            if (!success)
            {
                throw new Exception("the delete has failed");
            }
            
        }

        /// <summary>
        /// This method return a user from the board's members. 
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <summary>
        public User getUser(string email)
        {
            
            User user = joinedMembers.GetValueOrDefault(email);
            if (user == null)
            {
                log.Error("this user is not a member in this board");
                throw new Exception("this user is not a member in this board");

            }
            return user;
        }

        /// <summary>
        /// This method set an assigne to a task. 
        /// </summary>
        /// <param name="email">The user's that points on the new assinged.</param>
        /// <param name="taskId">The Id of task.</param>
        /// <param name="columnIndex">The index of the colums that the task is in it.</param>
        /// <param name="newAsignee">The new assigned user.</param>
        /// <summary>
        public void assignedTask(string email, int columnIndex, int taskId, string newAsignee)
        {
            string currAssignee = GetColumn(columnIndex).getTask(taskId).assignee;
            if (!membersContain(newAsignee))
            {
                throw new Exception("only board members can set as assignee");
            }
            if (currAssignee == null)
            {
                GetColumn(columnIndex).getTask(taskId).setAssignee(newAsignee);
            }
            else
            {
                if (!currAssignee.Equals(email))
                {
                    throw new Exception("only the assignee can assign his tasks");
                }
                GetColumn(columnIndex).getTask(taskId).setAssignee(newAsignee);
            }
        }
            
     
    }


}

