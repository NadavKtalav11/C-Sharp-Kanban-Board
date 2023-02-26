using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BussinessLayer;
using System.Text.Json;
using IntroSE.Kanban.Backend.ServiceLayer.Classes;
using Backend.Service;

namespace IntroSE.Kanban.Backend.ServiceLayer
{

    public class ColumnService
    {
        private BoardController boardController;

        public ColumnService(BoardController boardController )
        {
            this.boardController = boardController;

        }

        public ColumnService()
        {
            this.boardController = new BoardController();

        }


        /// <summary>
        /// This method return a specific column from the system. 
        /// </summary>
        /// <param name="email">The user email address.</param>
        /// <param name="boardId">The board's ID in the system.</param>
        /// <param name="columnindex">The column index .</param>
        /// <returns>An empty Response ,unless an error occurs </returns>
        /// <summary> 
        public string getColumn(string email, int boardId , int columnindex)
        {
            try
            {
                boardController.loginStatus(email);
                Column column = boardController.getBoard(boardId).GetColumn(columnindex);
                Response<Column> res = new Response<Column>(column, null);
                return JsonController.Serialize<Response<Column>>(res);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }


        }

        /// <summary>
        /// This method return all the task that are in the column. 
        /// </summary>
        /// <param name="email">The user email address.</param>
        /// <param name="boardId">The board's ID in the system.</param>
        /// <param name="columnindex">The column index .</param>
        /// <returns>An empty Response ,unless an error occurs </returns>
        /// <summary> 


        public string getColumnTasks(string email, int boardId, int columnindex)
        {
            try
            {
                boardController.loginStatus(email);
                Column column = boardController.getBoard(boardId).GetColumn(columnindex);
                List<Taskk> tasks = column.tasks;
                Response<List<Taskk>> res = new Response<List<Taskk>>(tasks, null);
                return JsonController.Serialize<Response<List<Taskk>>>(res);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        


        }




        /// <summary>
        /// This method edit a specific column's limit of the tasks in it. 
        /// </summary>
        /// <param name="email">The user email address.</param>
        /// <param name="boardId">The board's ID in the system.</param>
        /// <param name="columnIndex">the column index at the board .</param>
        /// <param name="newLimit"> the new tasks number limit at the column.</param>
        /// <returns>An empty Response ,unless an error occurs </returns>
        /// <summary> 
        public string editLimit(string email, int boardId, int columnIndex, int newLimit)
        {
            try
            {
                boardController.loginStatus(email);
                boardController.getBoard(boardId).GetColumn(columnIndex).editLimit(newLimit);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }

        }

        /// <summary>
        /// This method return a specific column limit of tasks in it. 
        /// </summary>
        /// <param name="email">The user email address.</param>
        /// <param name="boardId">The board's ID in the system.</param>
        /// <param name="columnIndex">the column index at the board .</param>
        /// <returns> a Response with the column limit,unless an error occurs </returns>
        /// <summary> 
        public string getLimit(string email, int boardId , int columnIndex)
        {
            try
            {
                boardController.loginStatus(email);
                int limit = boardController.getBoard(boardId).GetColumn(columnIndex).limit;
                Response toReturn = new Response();
                toReturn.toResponse(limit);
                return JsonSerializer.Serialize(toReturn);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }

        }

        /// <summary>
        /// This method return a List with all the task that are in a specific column. 
        /// </summary>
        /// <param name="email">The user email address.</param>
        /// <param name="boardId">The board's in the system.</param>
        /// <param name="columnIndex">the column index at the board .</param>
        /// <returns> a Response with List of the column tasks,unless an error occurs </returns>
        /// <summary> 
        public string getTasks(string email , int boardId, int columnIndex)
        {
            try
            {
                boardController.loginStatus(email);
                List<Taskk> tasks = boardController.getBoard(boardId).GetColumn(columnIndex).tasks;
                Response toReturn = new Response();
                toReturn.toResponse(tasks);
                return JsonSerializer.Serialize(toReturn);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }

        }
    }
}