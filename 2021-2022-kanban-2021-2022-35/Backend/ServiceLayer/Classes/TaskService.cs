using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BussinessLayer;
using IntroSE.Kanban.Backend.ServiceLayer.Classes;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class TaskService
    {
        private BoardController boardController;
        private UserController usercontroller;



        public TaskService(BoardController boardController)
        {
            this.boardController =boardController; 
            usercontroller = boardController.userController;
        }

        /// <summary>
        /// This method move a task to the next column
        /// </summary>
        /// <param name="email">The user email address.</param>
        /// <param name="boardId">The board's ID in the system.</param>
        /// <param name="columnIndex">the column index  where the task is.</param>
        /// <param name="taskid">the task id</param>
        /// <returns> an empty Response , unless an error occurs </returns>
        /// <summary> 
        public string taskMove(string email , int boardId,int columnIndex, int taskid)
        {
            try
            {
                boardController.loginStatus(email);
                boardController.getBoard(boardId).taskMove(email,taskid, columnIndex);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }

        }


        /// <summary>
        /// This method returns all the user tasks the inprogress .
        /// </summary>
        /// <param name="email">The user email address.</param>
        /// <returns> a List od all inprogress tasks , unless an error occurs </returns>
        /// <summary> 
        public string inProgressTasks(string email)
        {
            try
            {
                boardController.loginStatus(email);
                List<Taskk> inProg = boardController.inProgressTasks(email);
                Response toReturn = new Response();
                toReturn.toResponse(inProg);
                return JsonSerializer.Serialize(toReturn);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }

        /// <summary>
        /// This method add a new task to the system.
        /// </summary>
        /// <param name="email">The user email address.</param>
        /// <param name="boardId">The board's ID in the system.</param>
        /// <param name="title">the task Title. </param>
        /// <param name="description">the task description</param>
        /// <param name="dueDate">the task dueDate </param>
        /// <returns> an empty Response , unless an error occurs </returns>
        /// <summary> 
        public string addNewTask(string email , int boardId, string title, string description, DateTime dueDate)
        {
            try
            {
                boardController.loginStatus(email);
                boardController.getBoard(boardId).addNewTask( title, description, dueDate);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }

        }

        /// <summary>
        /// This method returns a task.
        /// </summary>
        /// <param name="email">The user email address.</param>
        /// <param name="boardId">The board's ID in the system.</param>
        /// <param name="columnIndex">the column index  where the task is.</param>
        /// <param name="taskId">the task id</param>
        /// <returns> a Response  with the task, unless an error occurs </returns>
        /// <summary> 
        public string getTask(string email, int boardId, int columnIndex, int taskId)
        {
            try
            {
                boardController.loginStatus(email);
                Taskk task1=  boardController.getBoard(boardId).GetColumn(columnIndex).getTask(taskId);
                Response toReturn = new Response();
                toReturn.toResponse(task1);
                return JsonSerializer.Serialize(toReturn);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }

        }

        /// <summary>
        /// This method edit a specific task title.
        /// </summary>
        /// <param name="email">The user email address.</param>
        /// <param name="boardId">The board's ID in the system.</param>
        /// <param name="ColumnIndex">the column index  where the task is.</param>
        /// <param name="taskId">the task id</param>
        /// <param name="newTitle">the new title </param>
        /// <returns> an empty Response , unless an error occurs </returns>
        /// <summary> 
        public string taskEditTitle(string email , int boardId, int ColumnIndex, int taskId, string newTitle)
        {
            try
            {
                boardController.loginStatus(email);
                boardController.getBoard(boardId).taskEditTitle( email , taskId, ColumnIndex, newTitle);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }

        /// <summary>
        /// This method edit a specific task due date.
        /// </summary>
        /// <param name="email">The user email address.</param>
        /// <param name="boardId">The board's in the system.</param>
        /// <param name="ColumnIndex">the column index  where the task is.</param>
        /// <param name="taskId">the task id</param>
        /// <param name="newDueDate">the new due Date</param>
        /// <returns> an empty Response , unless an error occurs </returns>
        /// <summary> 
        public string taskEditDueDate(string email, int boardId, int ColumnIndex, int taskId, DateTime newDueDate)
        {
            try
            {
                boardController.loginStatus(email);
                boardController.getBoard(boardId).taskEditDueDate(email, taskId, ColumnIndex, newDueDate); 
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }

        /// <summary>
        /// This method edit a specific task description.
        /// </summary>
        /// <param name="email">The user email address.</param>
        /// <param name="BoardId">The board's ID in the system.</param>
        /// <param name="ColumnIndex">the column index  where the task is.</param>
        /// <param name="taskId">the task id</param>
        /// <param name="newDescription">the new description</param>
        /// <returns> an empty Response , unless an error occurs </returns>
        /// <summary> 

        public string taskEditDescription(string email, int BoardId, int ColumnIndex, int taskId, string newDescription)
        {
            try
            {
                boardController.loginStatus(email);
                boardController.getBoard(BoardId).taskEditDescription(email, taskId, ColumnIndex, newDescription);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }


        /// <summary>
        /// This method set a asignee to a task .
        /// </summary>
        /// <param name="email">The user email address.</param>
        /// <param name="BoardName">The board's name in the system.</param>
        /// <param name="ColumnIndex">the column index  where the task is.</param>
        /// <param name="taskId">the task id</param>
        /// <param name="newAssigneeEmail">the new assignee email</param>
        /// <returns> an empty Response , unless an error occurs </returns>
        /// <summary> 
        public string assignedTask(string email, string BoardName, int ColumnIndex, int taskId, string newAssigneeEmail)
        {
            try
            {
                boardController.loginStatus(email);
                boardController.assignedTask(email, BoardName,  ColumnIndex, taskId, newAssigneeEmail);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }
    }


}