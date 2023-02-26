using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BussinessLayer;
using IntroSE.Kanban.Backend.ServiceLayer.Classes;
using log4net;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class UserService
    {
        private BoardController boardController;
        private UserController userController;

        public UserService(BoardController boardController , UserController userController)
        {
            this.boardController = boardController;
            this.userController = boardController.userController;

        }

 
        /// <summary>
        /// This method registers a new user to the system.
        /// </summary>
        /// <param name="Email">The user email address, used as the username for logging the system.</param>
        /// <param name="Password">The user password.</param>
        /// <returns>An empty response, unless an error occurs </returns>
        public string userRegister(string Email, string Password)
        {
            try
            {
                userController.userRegister(Email, Password);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));

            }
        }
        /// <summary>
        /// This method remove a registered user from the system.
        /// </summary>
        /// <param name="Email">The user email address, used as the username for logging the system.</param>
        /// <returns>An empty response, unless an error occurs </returns>
        public string userRemove(string Email)
        {
            try
            {
                boardController.loginStatus(Email);
                boardController.userRemove(Email);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }


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
        /// This method login a  user to the system.
        /// </summary>
        /// <param name="Email">The user email address, used as the username for logging the system.</param>
        /// <param name="Password">The user password.</param>
        /// <returns>A response with user email, unless an error occurs </returns>
        public string userLogin(string Email, string Password)
        {
            try
            {
                userController.login(Email, Password);
                Response toReturn = new Response();
                toReturn.toResponse(Email);
                return JsonSerializer.Serialize(toReturn);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }

        /// <summary>
        /// This method logout a user from the system.
        /// </summary>
        /// <param name="Email">The user email address, used as the username for logging the system.</param>
        /// <returns>An empty response, unless an error occurs </returns>
        public string userLogout(string Email)
        {
            try
            {
                userController.logOut(Email);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }

        /// <summary>
        /// This method edit the user's password .
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system.</param>
        /// <param name="password">The user password.</param>
        /// <param name="newPassword">The user new password.</param>
        /// <returns>An empty response, unless an error occurs </returns>
        public string userEditPassword(string email, string password, string newPassword)
        {
            try
            {
                userController.getUser(email).userEditPassword(password, newPassword);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }

        /// <summary>
        /// This method join a user to a specific board.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system.</param>
        /// <param name="boardId">The board id in the system.</param>
        /// <returns>An empty response, unless an error occurs </returns>
        /// <summary> 
        public string joinBoard(string email, int boardId)
        {
            try
            {
                boardController.joinMember(email , boardId); ;
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }

        /// <summary>
        /// in This method the user leaves a specific board.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system.</param>
        /// <param name="boardId">The board's ID in the system.</param>
        /// <returns>An empty response, unless an error occurs </returns>
        /// <summary> 
        public string leaveBoard(string email, int boardId)
        {
            try
            {
                
                boardController.leaveBoard(email, boardId);
                Response ToReturn = new Response();
                return JsonSerializer.Serialize(ToReturn);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }


        /// <summary>
        /// This method transfer ownership of a board from one user to another one. 
        /// </summary>
        /// <param name="email">The user email address of the ownership.</param>
        /// <param name="TransferTo">The user email adress of the new owner.</param>
        /// <param name="boardName">The board name in the system.</param>
        /// <returns>An empty response, unless an error occurs </returns>
        /// <summary> 
        public string transferOwnership(string email, string TransferTo, string boardName)
        {
            try
            {
                Response ToReturn = new Response();
                boardController.transferOwnership(email,TransferTo, boardName);
                return JsonSerializer.Serialize(ToReturn);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }

        /// <summary>
        /// This method return all the users the register to the system. 
        /// </summary>
        /// <returns>A response with List of all users, unless an error occurs </returns>
        /// <summary> 
        public string getAllUsers()
        {
            try
            {
                Response ToReturn = new Response();
                List<User> users = userController.getUsersList();
                ToReturn.toResponse(users);
                return JsonSerializer.Serialize(ToReturn);
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e.Message));
            }
        }


    }



    
}
