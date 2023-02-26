﻿using System;
using System.Linq;
using System.Collections.Generic;
using IntroSE.Kanban.Backend.BussinessLayer;
using System.Text.Json;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    /// <summary>
    /// A class for grading your work <b>ONLY</b>. The methods are not using good SE practices and you should <b>NOT</b> infer any insight on how to write the service layer/business layer. 
    /// <para>
    /// Each of the class' methods should return a JSON string with the following structure (see <see cref="System.Text.Json"/>):
    /// <code>
    /// {
    ///     "ErrorMessage": &lt;string&gt;,
    ///     "ReturnValue": &lt;object&gt;
    /// }
    /// </code>
    /// Where:
    /// <list type="bullet">
    ///     <item>
    ///         <term>ReturnValue</term>
    ///         <description>
    ///             The return value of the function.
    ///             <para>
    ///                 The value may be either a <paramref name="primitive"/>, a <paramref name="Task"/>, or an array of of them. See below for the definition of <paramref name="Task"/>.
    ///             </para>
    ///             <para>If the function does not return a value or an exception has occorred, then the field should be either null or undefined.</para>
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>ErrorMessage</term>
    ///         <description>If an exception has occorred, then this field will contain a string of the error message. Otherwise, the field will be null or undefined.</description>
    ///     </item>
    /// </list>
    /// </para>
    /// <para>
    /// An empty response is a response that both fields are either null or undefined.
    /// </para>
    /// <para>
    /// The structure of the JSON of a Task, is:
    /// <code>
    /// {
    ///     "Id": &lt;int&gt;,
    ///     "CreationTime": &lt;DateTime&gt;,
    ///     "Title": &lt;string&gt;,
    ///     "Description": &lt;string&gt;,
    ///     "DueDate": &lt;DateTime&gt;
    /// }
    /// </code>
    /// </para>
    /// </summary>
    public class GradingService
    {
        private BoardController boardController;


        public GradingService()
        {
            this.boardController =new BoardController();
        }


        /// <summary>
        /// This method registers a new user to the system.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string Register(string email, string password)
        {
            try
            {
                boardController.userController.userRegister(email, password);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }

        }


        /// <summary>
        ///  This method logs in an existing user.
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>A response with the user's email, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string Login(string email, string password)
        {
            try
            {
                boardController.userController.login(email, password);
                Response toReturn = new Response();
                toReturn.toResponse(email);
                return JsonSerializer.Serialize(toReturn, toReturn.options);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }


        /// <summary>
        /// This method logs out a logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string Logout(string email)
        {
            try
            {
                boardController.userController.logOut(email);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }

        /// <summary>
        /// This method limits the number of tasks in a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            try
            {
                boardController.loginStatus(email);
                boardController.getBoardByName(boardName , email).GetColumn(columnOrdinal).editLimit(limit);
                Response toReturn = new Response();

                return JsonSerializer.Serialize(toReturn);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }

        }

        /// <summary>
        /// This method gets the limit of a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with the column's limit, unless an error occurs (see <see cref="GradingService"/>)</returns>

        public string GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            try
            {
                boardController.loginStatus(email);
                int output = boardController.getBoardByName(boardName, email).GetColumn(columnOrdinal).limit;
                Response toReturn = new Response();
                toReturn.toResponse(output);
                return JsonSerializer.Serialize(toReturn, toReturn.options);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }


        /// <summary>
        /// This method gets the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with the column's name, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            try
            {
                boardController.loginStatus(email);
                string name = boardController.getBoardByName(boardName, email).GetColumn(columnOrdinal).name;
                Response toReturn = new Response();
                toReturn.toResponse(name);
                return JsonSerializer.Serialize(toReturn, toReturn.options);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }


        /// <summary>
        /// This method adds a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            try
            {
                boardController.loginStatus(email);
                boardController.AddTask( email, boardName, title, description, dueDate);
                Response toReturn = new Response();
                return JsonSerializer.Serialize(toReturn);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }

        /// <summary>
        /// This method updates the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            try
            {
                boardController.loginStatus(email);
                boardController.getBoardByName(boardName, email).taskEditDueDate( email , taskId, columnOrdinal, dueDate);
                Response toReturn = new Response();
                return JsonSerializer.Serialize(toReturn);

            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }


        /// <summary>
        /// This method updates task title.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {
            try
            {
                boardController.loginStatus(email);
                boardController.getBoardByName(boardName, email).taskEditTitle(email, taskId, columnOrdinal, title); ;
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }


        /// <summary>
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        {
            try
            {
                boardController.loginStatus(email);
                boardController.getBoardByName(boardName, email).taskEditDescription(email,taskId , columnOrdinal , description);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }


        /// <summary>
        /// This method advances a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string AdvanceTask(string email, string boardName, int columnOrdinal, int taskId)
        {
            try
            {
                boardController.loginStatus(email);
                boardController.getBoardByName(boardName, email).taskMove(email, taskId, columnOrdinal);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }


        /// <summary>
        /// This method returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with a list of the column's tasks, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetColumn(string email, string boardName, int columnOrdinal)
        {
            try
            {
                boardController.loginStatus(email);
                Column column = boardController.getBoardByName(boardName, email).GetColumn(columnOrdinal);
                List<Taskk> colTasks = column.tasks;
                Response toReturn = new Response();
                toReturn.toResponse(colTasks);
                var options = new JsonSerializerOptions {WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(toReturn, options);
                return jsonString;
               // return JsonSerializer.Serialize(toReturn);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }


        /// <summary>
        /// This method adds a board to the specific user.
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="name">The name of the new board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string AddBoard(string email, string name)
        {
            try
            {
                boardController.loginStatus(email);
                boardController.addBoard(name, email);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }


        /// <summary>
        /// This method deletes a board.
        /// </summary>
        /// <param name="email">Email of the user, must be logged in and an owner of the board.</param>
        /// <param name="name">The name of the board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string RemoveBoard(string email, string name)

        {
            try
            {
                boardController.loginStatus(email);
                boardController.removeBoard(email , name);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }


        /// <summary>
        /// This method returns all in-progress tasks of a user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>A response with a list of the in-progress tasks of the user, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string InProgressTasks(string email)
        {
            try
            {
                boardController.loginStatus(email);
                List<Taskk> inProgTasks =  boardController.inProgressTasks(email);
                Response toReturn = new Response();
                toReturn.toResponse(inProgTasks);
                return JsonSerializer.Serialize(toReturn, toReturn.options);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }

        /// <summary>
        /// This method returns a list of IDs of all user's boards.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>A response with a list of IDs of all user's boards, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetUserBoards(string email)
        {
            try
            {
                boardController.loginStatus(email);
                List<int> boardsIdList = boardController.getUserBoards(email);               
                Response toReturn = new Response();
                toReturn.toResponse(boardsIdList);
                return JsonSerializer.Serialize(toReturn, toReturn.options);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        
        }
        
        /// <summary>
        /// This method returns a board's name
        /// </summary>
        /// <param name="boardId">The board's ID</param>
        /// <returns>A response with the board's name, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetBoardName(int boardId)
        {
            try
            {
                string name = boardController.getBoard(boardId).name;
                Response toReturn = new Response();
                toReturn.toResponse(name);
                return JsonSerializer.Serialize(toReturn, toReturn.options);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }

        }

        /// <summary>
        /// This method adds a user as member to an existing board.
        /// </summary>
        /// <param name="email">The email of the user that joins the board. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string JoinBoard(string email, int boardID)
        {
            try
            {
                boardController.loginStatus(email);
                boardController.joinMember(email, boardID);
                Response toReturn = new Response();
                return JsonSerializer.Serialize(toReturn);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }

        }

        /// <summary>
        /// This method removes a user from the members list of a board.
        /// </summary>
        /// <param name="email">The email of the user. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string LeaveBoard(string email, int boardID)
        {
            try
            {
                boardController.loginStatus(email);
                boardController.leaveBoard(email , boardID) ;
                Response toReturn = new Response();
                return JsonSerializer.Serialize(toReturn);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }      
        }

        /// <summary>
        /// This method assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column number. The first column is 0, the number increases by 1 for each column</param>
        /// <param name="taskID">The task to be updated identified a task ID</param>        
        /// <param name="emailAssignee">Email of the asignee user</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            try
            {
                boardController.loginStatus(email);
                boardController.assignedTask(email , boardName , columnOrdinal , taskID , emailAssignee);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }


        ///<summary>This method loads all persisted data.
        ///<para>
        ///<b>IMPORTANT:</b> When starting the system via the GradingService - do not load the data automatically, only through this method. 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
        ///</para>
        /// </summary>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string LoadData()
        {
            try
            {
                boardController.userController.loadData();
                boardController.loadData();
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }


        ///<summary>This method deletes all persisted data.
        ///<para>
        ///<b>IMPORTANT:</b> 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
        ///</para>
        /// </summary>
        ///<returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string DeleteData()
        {
            try
            {
                boardController.deleteData();
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }

        /// <summary>
        /// This method transfers a board ownership.
        /// </summary>
        /// <param name="currentOwnerEmail">Email of the current owner. Must be logged in</param>
        /// <param name="newOwnerEmail">Email of the new owner</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            try
            {
                boardController.loginStatus(currentOwnerEmail);
                boardController.transferOwnership(currentOwnerEmail, newOwnerEmail, boardName);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }

        
    }
}
