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
    public class BoardService

    {
        private BoardController boardController;

        public BoardService(BoardController boardController)
        {
            this.boardController=  boardController;

        }



        /// <summary>
        /// This method return a specific board . 
        /// </summary>
        /// <param name="email">The user email address of the ownership.</param>
        /// <param name="boardId">The board's Id in the system.</param>
        /// <returns>An Response with the board, unless an error occurs </returns>
        /// <summary> 
        public string getBoard( string email , int boardId)
        {
            try
            {
                boardController.loginStatus(email);
                Board board = boardController.getBoard(boardId);
                Response<Board> toReturn = new Response<Board>(board , null);
                //Response toReturn = new Response();
                return JsonController.Serialize<Response<Board>>(toReturn);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }


        /// <summary>
        /// This method return a all the users that are members of specific Board . 
        /// </summary>
        /// <param name="email">The user email address of the ownership.</param>
        /// <param name="BoardId">The board's Id in the system.</param>
        /// <returns>An Response with list of users emails, unless an error occurs </returns>
        /// <summary> 
        public string getAllMembers(string email , int BoardId)
        {
            try
            {
                boardController.loginStatus(email);
                List<string> BoardUsers = boardController.getBoard(BoardId).getJoinedMembers();
                Response toReturn = new Response();
                toReturn.toResponse(BoardUsers);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }




        /// <summary>
        /// This method add a board to the system. 
        /// </summary>
        /// <param name="email">The user email address of the ownership.</param>
        /// <param name="boardName">The board name in the system.</param>
        /// <returns>An empty Response, unless an error occurs </returns>
        /// <summary> 
        public string addBoard(string email, string boardName)
        {
            try
            {
                boardController.loginStatus(email);
                boardController.addBoard(boardName ,email);
                Response toReturn = new Response();
                return JsonSerializer.Serialize(toReturn);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }

        /// <summary>
        /// This method remove a board from the system. 
        /// </summary>
        /// <param name="boardId">The board's in the system.</param>
        /// <param name="removerEmail">The user email address of the ownership.</param>
        /// <returns>An empty Response ,  unless an error occurs </returns>
        /// <summary> 
        public string RemoveBoard( int boardId , string removerEmail)
        {
            try
            {
                boardController.loginStatus(removerEmail);
                boardController.removeBoard(boardId, removerEmail);
                Response toReturn = new Response();
                return JsonSerializer.Serialize(toReturn);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }

        /// <summary>
        /// This method edit a board name at the system. 
        /// </summary>
        /// <param name="email">The user email address of the ownership.</param>
        /// <param name="BoardId">The board's ID in the system.</param>
        /// <param name="newName">The board new name .</param>
        /// <returns>An empty Response ,unless an error occurs </returns>
        /// <summary> 
        public string boardEditName(string email ,int BoardId, string newName)
        {
            try
            {
                boardController.loginStatus(email);
                boardController.getBoard(BoardId).boardEditName(newName);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }


        }

        /// <summary>
        /// This method deletes all the data in the system
        /// <summary>
        /// <returns>An empty Response ,unless an error occurs </returns>
        /// <summary> 
        public string DeleteData()
        {
            try
            {
                
                Response ToReturn = new Response();
                boardController.deleteData();
                return JsonSerializer.Serialize(ToReturn);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }


        /// <summary>
        /// This method load all the data in the system
        /// <summary>
        /// <returns>An empty Response ,unless an error occurs </returns>
        /// <summary> 

        public string LoadData()
        {
            try
            {
                boardController.userController.loadData();
                boardController.loadData();
                Response ToReturn = new Response();
                return JsonSerializer.Serialize(ToReturn);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }



    }
}