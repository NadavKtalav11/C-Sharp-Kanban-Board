using NUnit.Framework;
using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using IntroSE.Kanban.Backend.BussinessLayer;
using System.Text.Json;
using IntroSE.Kanban.Backend.ServiceLayer.Classes;

namespace UserServiceTests
{
    public class t2_BoardServiceTests
    {

        ServiceFactory factory = new ServiceFactory();
        BoardService boardS; 
        ColumnService columnService;
        UserService userS;
        Response goodResponse = new Response();
        TaskService taskService;

        [OneTimeSetUp , Order(1)]
        public void setup()
        {
            
            boardS = factory.boardService;
            columnService = factory.columnService;
            userS = factory.userService;
            taskService = factory.taskService;
            boardS.DeleteData();

            
            userS.userRegister("nada@gmail.com", "daviD111");

 
        }


        [Test, Order(2)]
        /// </summary>
        /// This function tests Requirement 9
        /// </summary>
        public void userAddBoardPositive()
        {
            userS.userRegister("nadav12345@gmail.com", "NAdav123");

            string actual = boardS.addBoard("nadav12345@gmail.com", "Work");
            
            string expected = JsonSerializer.Serialize(goodResponse);
            Assert.AreEqual(expected, actual);
        }

        [Test, Order(3)]
        /// </summary>
        /// This function tests Requirement 9
        /// </summary>
        public void userAddBoardNegativeNotLogIn()
        {
            userS.userLogout("nadav12345@gmail.com");
            string actual = boardS.addBoard("nadav12345@gmail.com", "another board");
            string expected = JsonSerializer.Serialize(new Response("please log in first!"));
            Assert.AreEqual(expected, actual);
        }


        [Test, Order(4)]
        /// </summary>
        /// This function tests Requirement 9
        /// </summary>
        public void userAddBoardNegativeEmptyBoardName()
        {
            userS.userLogin("nadav12345@gmail.com", "NAdav123");
            string actual = boardS.addBoard("nadav12345@gmail.com", "");
            string expected = JsonSerializer.Serialize(new Response("board name cant be null or empty"));
            Assert.AreEqual(expected, actual);
        }

        [Test, Order(5)]
        /// </summary>
        /// This function tests Requirement 11
        /// </summary>
        public void userRemoveBoardNegativenotFound()
        {
            string actual = boardS.RemoveBoard(8 , "nadav12345@gmail.com");
            string expected = JsonSerializer.Serialize(new Response("this board dosent exist in the system"));
            Assert.AreEqual(expected, actual);
        }

        [Test, Order(6)]
        /// </summary>
        /// This function tests Requirement 11
        /// </summary>
        public void userRemoveBoardNegativeNotOwner()
        {
            userS.userRegister("avoda!@mail.com", "sisnmA11");
  
            userS.joinBoard("avoda!@mail.com", 1);
            string actual = boardS.RemoveBoard(1 , "avoda!@mail.com");
            string expected = JsonSerializer.Serialize(new Response("only the owner can delete his boards"));
            Assert.AreEqual(expected, actual);
        }
        [Test, Order(7)]
        /// </summary>
        /// This function tests Requirement 11
        /// </summary>
        public void userRemoveBoardPositive()
        {
            string actual = boardS.RemoveBoard(1 , "nadav12345@gmail.com");
            Response toReturn = new Response();
            string expected = JsonSerializer.Serialize(toReturn);
            Assert.AreEqual(expected, actual);
        }


        [Test, Order(8)]
        public void boardEditNamePositive()
        {
            boardS.addBoard("nada@gmail.com", "Board!1");
            string actual = boardS.boardEditName("nada@gmail.com", 2,  "newBoard1");
            string expected = JsonSerializer.Serialize(goodResponse);
            Assert.AreEqual(actual, expected);
        }

        /// </summary>
        /// This function tests Requirement 6
        /// </summary>
        
        [Test, Order(9)]
        public void boardEditNameNegativeSameName()
        {
            boardS.addBoard("nada@gmail.com", "Board!1");
            string actual = boardS.boardEditName("nada@gmail.com" , 2, "newBoard1");
            string expected = JsonSerializer.Serialize(new Response("cant change to the same name"));
            Assert.AreEqual(actual, expected);
        }

        [Test, Order(10)]
        public void boardEditNameNegativeEmptyName()
        {
            boardS.addBoard("nada@gmail.com", "Board!1");
            string actual = boardS.boardEditName("nada@gmail.com", 2 , "");
            string expected = JsonSerializer.Serialize(new Response("name cant be null or empty"));
            Assert.AreEqual(actual, expected);
        }


    }
}
        