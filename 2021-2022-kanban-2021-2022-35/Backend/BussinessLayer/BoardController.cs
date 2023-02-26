using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;
using IntroSE.Kanban.Backend.DataAccessLayer.DAOs;
using IntroSE.Kanban.Backend.DataAccessLayer.DTO;
using IntroSE.Kanban.Backend.DataAccessLayer.DAO_s;
using IntroSE.Kanban.Backend.DataAccessLayer.DAO;
using IntroSE.Kanban.Backend.DataAccessLayer.DTO_s;

namespace IntroSE.Kanban.Backend.BussinessLayer
{
    public class BoardController
    {
        private int boardIdCounter; 
        public Dictionary<int, Board> boards { get; private set; }
        public UserController userController { get; private set; }
        private BoardDAO boardDAO;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        //constractor
        public BoardController()
        {
            this.boardDAO = new BoardDAO();
            this.boardIdCounter = 0;
            this.boards = new Dictionary<int, Board>();
            this.userController = new UserController();
        }

        /// <summary>
        /// This method loads all the data to the system. 
        /// <summary>
        public void loadData()
        {
            log.Info("try to load all boards");
            foreach (BoardDTO boardDTO in boardDAO.Select())
            {
                Board board = new Board(boardDTO);
                boards.Add(board.boardId, board);
                boardIdCounter = Math.Max(boardIdCounter, board.boardId); 
            }
            log.Info("all boards loaded seccussfully");
            boardIdCounter++;
            BoardsMembersDAO boardsMembersDAO = new BoardsMembersDAO();
            log.Info("try to load boards members");
            foreach (BoardsMembersDTO boardsMembersDTO in boardsMembersDAO.Select())
            {
                getBoard(boardsMembersDTO.boardId).joinAMemberLOadDAta(userController.getUser(boardsMembersDTO.email));
            }
            log.Info("all members loaded seccussfully");


        }
        public void assignedTask(string email, string boardName, int columnIndex, int taskId, string newAssignee)
        {
            log.Info("try to assign tasks");
            if (columnIndex == 2)
            {
                log.Error("done tasks cannot change");
                throw new Exception("done tasks cannot change");
            }
            Board board = getBoardByName(boardName, email);
            if (board == null)
            {
                log.Error("this email dont belong to this board name");
                throw new Exception("this email dont belong to this board name");
                
            }
            else
            {
                board.assignedTask(email, columnIndex, taskId, newAssignee);
                Taskk task = board.GetColumn(columnIndex).getTask(taskId);
                log.Info("the task assigned successfully");
            }
        }
        


        /// <summary>
        /// This method checks if user is logged in. 
        /// </summary>
        /// <param name="email">The user's thatis being checked.</param>
        /// <summary>
        public void loginStatus(string email)

        {
            log.Info("check if the user is logged in");
            bool loggedIn = userController.getUser(email).loggedIn;
            if (!loggedIn)
            {
                log.Error(" the user isnt logged in");
                throw new Exception("please log in first!");

            }
        }


        /// <summary>
        /// This method return a list of all the user in progress tasks. 
        /// </summary>
        /// <param name="email">The user's thatis being checked.</param>
        /// <summary>

        public List<Taskk> inProgressTasks(string email)
        {
            log.Info("try to return all user in progress tasks");
            List<Taskk> inProgList = new List<Taskk>();
            foreach (Board board in boards.Values)
            {
                if (board.membersContain(email))
                {
                    foreach (Taskk task in board.GetColumn(1).tasks)
                    {
                        if (task.assignee.Equals(email))
                        {
                            inProgList.Add(task);
                        }
                    }
                }
            }
            log.Info("user in progress tasks returned seccusseully");
            return inProgList;
        }

        /// <summary>
        /// This method removes a user from the system. 
        /// </summary>
        /// <param name="email">The user's that is being removed.</param>
        /// <summary>
        public void userRemove(string email)
        {
            log.Info("try to remove " + email );
            userController.userRemove(email);
            bool changed = false;
            foreach (Board board in boards.Values)
            {
                if (board.removeUser(email))
                    changed = true;
            }
            BoardsMembersDAO boardsMembersDAO = new BoardsMembersDAO();
            bool success = false;
            success = boardsMembersDAO.Delete(email);
            if (!success & changed)
            {

                throw new Exception("the deletion has failed");
            }
            log.Info(email + " removed successfully");
        }

        /// <summary>
        /// This method returns a list with all the names of the board that are in the system. 
        /// </summary>
        /// <summary>
        public List<string> getAllBoardNames()
        {
            log.Info("try to return all system boards name");
            List<string> boardNames = new List<string>();
            foreach(Board board in boards.Values)
            {
                boardNames.Add(board.name);
            }
            log.Info("board names returned suuccessfully");
            return boardNames;
        }

        /// <summary>
        /// This method return all the boards that a user is member of. 
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <summary>
        public List<int> getUserBoards(string email)
        {
            log.Info("try to return all " + email + " boards name");
            List<int> userBoards = new List<int>(); 
            foreach (Board board in boards.Values)
            {
                if (board.membersContain(email))
                {
                    userBoards.Add(board.boardId);
                }
            }
            log.Info("all " + email + " boards name returned successfully");
            return userBoards;
        }

        /// <summary>
        /// This method return a board. 
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="boardName">The board's name that we want to return.</param>
        /// <summary>
        internal Board getBoardByName(string boardName, string email)
        {
            log.Info("try to return all " + email + " boards with the name " + boardName);
            foreach (Board board in boards.Values)
            {
                if (board.name.Equals(boardName) && board.membersContain(email))
                {
                    return board;
                    
                }
            }
            log.Info("the specific board has found and returned");
            return null;

        }

        public void AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            getBoardByName(boardName, email).addNewTask(title, description, dueDate);
        }

        /// <summary>
        /// This method return a board. 
        /// </summary>
        /// <param name="Id"> The board's ID.</param>
        /// <summary>
        public Board getBoard(int Id)
        {
            log.Info("try to found the board with id ="  + Id);
            Board curr = boards.GetValueOrDefault(Id);
            if (curr == null)
            {
                throw new Exception(" this Board Id dosent exist");
            }
            log.Info("the specific board has found and returned");
            return curr;
        }


        /// <summary>
        /// This method adds a new board. 
        /// </summary>
        /// <param name="userEmail">The user's email.</param>
        /// <param name="boardName">The new board name .</param>
        /// <summary>
        internal Board addBoard(string boardName, string userEmail)
        {
            log.Info("try to add new board");
            if (boardName==null )
            {
                log.Error("board name was null");
                throw new Exception("board name cant be null or empty");
            }
            boardName = boardName.Trim();
            if (boardName.Length==0)
            {
                log.Error("board name was empty");
                throw new Exception("board name cant be null or empty");
            }
            if (getBoardByName(boardName , userEmail) != null)
            {
                log.Error("this user alredy have board with the same name");
                throw new Exception("user cant have 2 boards with same name");
            }
            User user = userController.getUser(userEmail);
            Board newBoard = new Board(boardName, user , boardIdCounter);
            boards.Add(boardIdCounter, newBoard);

            boardIdCounter++;
            log.Info("board name was added");
            return newBoard;
            
        }

        /// <summary>
        /// This method adds a user to board's members. 
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="boardId">The board's Id .</param>
        /// <summary>
        public void joinMember(string email , int boardId)
        {
            log.Info("try to join " + email + "to board number " + boardId);
            Board board = boards.GetValueOrDefault(boardId);
            if (board == null)
            {
                log.Error("this board dosent exist");
                throw new Exception("this board Id doent exist");
            }
            if (getBoardByName(board.name , email )!= null)
            {
                log.Error(email + " alredy has board with this name");
                throw new Exception("this user alredy have a board with the same name");
                
            }
            board.joinAMember(userController.getUser(email));
            log.Info( email + "was added as a member to board number " + boardId);
        }

        /// <summary>
        /// This method removing a user from a board's members. 
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="boardID">The board's Id .</param>
        /// <summary>
        public void leaveBoard(string email ,int boardID)
        {
            log.Info(email + " try to leave board number " + boardID);
            Board board = boards.GetValueOrDefault(boardID);
            if (board== null) 
            {
                log.Error("board number " + boardID + " dosent exist in the system");
                throw new Exception("this board id dosent exist in the system");
            }
            if (board.Owner == email)
            {
                log.Error( email + " is not the assignee");
                throw new Exception("board owner cant leave his board");
            }
            board.leaveBoard(email);
            log.Info(email + " leaved board number " + boardID);

        }

        /// <summary>
        /// This method return the num of boards the is member. 
        /// </summary>
        /// <param name="email">The user's email.</param>

        /// <summary>
        public int getBoardNum(string email)
        {
            log.Info( " try to return the number of " + email  + " boards" );
            int i = 0;
            foreach (Board board in boards.Values)
            {
                if (board.membersContain(email))
                {
                    i = i + 1;
                }
            }
            log.Info("the number of boards returned seccusfully");
            return i;
        }

        /// <summary>
        /// This method removing a board. 
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="boardName">The board's name .</param>
        /// <summary>
        public void removeBoard(string email , string boardName)
        {
            removeBoard(getBoardByName(boardName, email).boardId, email);
        }
        /// <summary>
        /// This method removing a board. 
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="id">The board's id .</param>
        /// <summary>
        public void removeBoard(int id , string email)
        {
            log.Info(" try to remove board number " + id);
            Board board =boards.GetValueOrDefault(id);
            if (board == null)
            {
                log.Error("board number " + id  +  " is not in the system") ;
                throw new Exception("this board dosent exist in the system");
            }
            if (!board.Owner.Equals(email))
            {
                log.Error(email + " is not the owner");
                throw new Exception("only the owner can delete his boards");
            }
            
            boards.Remove(id);
            bool Board_success =boardDAO.Delete(id);
            ColumnDAO columnDAO = new ColumnDAO();
            bool Column_success = columnDAO.Delete(id);
            TaskDAO taskDAO = new TaskDAO();
            bool TaskSuccess = true;
            BoardsMembersDAO boardsMembersDAO = new BoardsMembersDAO();
            boardsMembersDAO.Delete(id);

            if (board.GetColumn(0).tasks.Count + board.GetColumn(1).tasks.Count + board.GetColumn(2).tasks.Count != 0)
            {
                TaskSuccess = taskDAO.Delete(id);
            }
            if (!Board_success | !Column_success | !TaskSuccess)
            {
                
                throw new Exception("the deletion has failed");
            }
            log.Info("the board has removed");

        }

        /// <summary>
        /// This method removing all the data from the system. 
        /// </summary>
        public void deleteData()
        {
            log.Info( "try to delete all system and DataBase data");
            BoardDAO boardDAO= new BoardDAO();
            bool successBoard;
            successBoard  =boardDAO.Clear();
            UserDAO userDAO = new UserDAO();
            bool successUser;
            successUser = userDAO.Clear();
            BoardsMembersDAO boardsMembersDAO = new BoardsMembersDAO();
            bool successMembers;
            successMembers = boardsMembersDAO.Clear();
            ColumnDAO columnDAO = new ColumnDAO();
            bool successColumn;
            successColumn =columnDAO.Clear();
            TaskDAO taskDAO = new TaskDAO();
            bool succesTask;
            succesTask= taskDAO.Clear();
            if (!succesTask | !successColumn | !successMembers | !successBoard | !successUser)
            {
                throw new Exception("data clearnce has failed");
            }
            userController.deleteData();
            this.boards.Clear();
            this.boardIdCounter = 1;
            log.Info("all the data removed");
        }

        /// <summary>
        /// This method transfer the owenrship of a board from one user to another. 
        /// </summary>
        /// <param name="currentOwnerEmail">The current owener's email.</param>
        /// <param name="boardName">The board's name .</param>
        /// <param name="newOwnerEmail">The new owener's email.</param>
        /// <summary>
        public void transferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            log.Info("try to transfer " +boardName +" ownership from " + currentOwnerEmail +" to " + newOwnerEmail);
            Board board = getBoardByName( boardName , currentOwnerEmail);
            if (board== null)
            {
                log.Error(currentOwnerEmail + " is not a member in the board");
                throw new Exception("please join the board first");
            }
            if (!board.Owner.Equals(currentOwnerEmail))
            {
                log.Error(currentOwnerEmail + " is not the owner the board");
                throw new Exception("only the owner can transfer the ownership");
            } 
            if (!board.membersContain(newOwnerEmail))
            {
                log.Error(newOwnerEmail + " is not member of the board");
                throw new Exception("the new owner must join the board first");
            }
            board.setOwner(newOwnerEmail);
            log.Info( "ownership transfered from " + currentOwnerEmail + " to " + newOwnerEmail);
        }

        /// <summary>
        /// This method returns the amount of the boards in the system. 
        /// <summary>
        public int boardNum()
        {
            log.Info("return number of all the boards in the system ");
            return boards.Count();
        }

    }
}
