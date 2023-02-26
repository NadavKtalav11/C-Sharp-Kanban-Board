using NUnit.Framework;
using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using IntroSE.Kanban.Backend.BussinessLayer;
using System.Text.Json;
using IntroSE.Kanban.Backend.ServiceLayer.Classes;
using IntroSE.Kanban.Backend.DataAccessLayer.DAOs;

namespace UserServiceTests
{
    public class t1_UserServiceTests
    {

        Factory factory = new Factory();
        BoardService boardService;
        ColumnService columnService;
        UserService user;
        TaskService taskService;
        Response goodResponse = new Response();
        BoardController boardController;




        [Test, Order(0)]
        public void deleteData()
        {

            boardService = factory.boardService;
            columnService = factory.columnService;
            user = factory.userService;
            taskService = factory.taskService;



            boardController = factory.boardController;


        }
        [Test, Order(1)]
        //This function tests Requirement 7
        public void LoadData()
        {

            BoardDAO boardDAO = new BoardDAO();
            int count = boardDAO.Select().Count;
            boardService.LoadData();
            int afterCount = boardController.boardNum();
            boardService.DeleteData();
            Assert.AreEqual(count, afterCount);


        }

        [Test, Order(1)]
        /// </summary>
        /// This function tests Requirement 7
        /// This function tests Requirement 2
        /// </summary>
        public void userRegisteTest_NegativePassword()
        {
            string actual = user.userRegister("David@gmail.com", "657");
            string expected = JsonSerializer.Serialize(new Response("the password needs to be of size 6-20 charcters"));
            Assert.AreEqual(actual, expected);
        }
        [Test, Order(2)]

        /// </summary>
        /// This function tests Requirement 7
        /// </summary>
        public void userRegisteTest_Positive()
        {
            string b = user.userRegister("David@gmail.com", "Abbbd657");
            var a = JsonSerializer.Serialize(new Response());
            Assert.AreEqual(a, b);
        }

        [Test, Order(3)]
        /// </summary>
        /// This function tests Requirement 7
        /// This function tests Requirement 3
        /// </summary>
        public void userRegisteTest_NegativeSameEmail()
        {
            string b = user.userRegister("David@gmail.com", "Abb2157");
            string a = JsonSerializer.Serialize(new Response("Error: this email is already assigned to a user, please try to reset password"));
            Assert.AreEqual(a, b);
        }

        [Test, Order(4)]
        /// </summary>
        /// This function tests Requirement 7
        /// </summary>
        public void userRegisteTest_NegativeNull()
        {
            string b = user.userRegister(null, "Abb2157");
            string a = JsonSerializer.Serialize(new Response("Email is null"));
            Assert.AreEqual(a, b);
        }

        [Test, Order(5)]
        /// </summary>
        /// This function tests Requirement 8
        /// </summary>
        public void userloginTest_Positive()
        {
            user.userRegister("nadav@gmail.com", "Abbbd657");
            user.userLogout("nadav@gmail.com");
            string actual = user.userLogin("nadav@gmail.com", "Abbbd657");
            Response email = new Response();
            email.ReturnValue = "nadav@gmail.com";
            string expected = JsonSerializer.Serialize(email);
            Assert.AreEqual(expected, actual);
        }

        [Test, Order(6)]
        /// </summary>
        /// This function tests Requirement 8
        /// </summary>
        public void userloginTest_negativeAlredy()
        {
            string actual = user.userLogin("nadav@gmail.com", "Abbbd657");
            string expected = JsonSerializer.Serialize(new Response("user already looged in"));
            Assert.AreEqual(actual, expected);
        }

        [Test, Order(7)]
        /// </summary>
        /// This function tests Requirement 8
        /// </summary>
        public void userloginTest_negativeNotRegistered()
        {
            string actual = user.userLogin("nadav123@gmail.com", "Abbbd657");
            string expected = JsonSerializer.Serialize(new Response("user is not registerd"));
            Assert.AreEqual(actual, expected);
        }

        [Test, Order(8)]
        /// </summary>
        /// This function tests Requirement 8
        /// </summary>
        public void userlogOutTest_Positive()
        {
            user.userRegister("nadav1234@gmail.com", "Abbbd657");

            string actual = user.userLogout("nadav1234@gmail.com");
            string expected = JsonSerializer.Serialize(goodResponse);
            Assert.AreEqual(actual, expected);
        }
        [Test, Order(9)]
        /// </summary>
        /// This function tests Requirement 8
        /// </summary>
        public void userlogOutTest_NegativeNotlogedin()
        {
            user.userRegister("nadav123456789@gmail.com", "Abbbd657");
            user.userLogout("nadav123456789@gmail.com");
            string actual = user.userLogout("nadav123456789@gmail.com");
            string expected = JsonSerializer.Serialize(new Response("you are alredy logged out"));
            Assert.AreEqual(expected, actual);
        }

        [Test, Order(9)]
        /// </summary>
        /// This function tests Requirement 10
        /// </summary>
        public void userNumofBOards() {
            user.userRegister("nadav12343221@gmail.com", "Abbbd657");
            int actual =boardController.getBoardNum("nadav12343221@gmail.com");
            int expected = 0;
            Assert.AreEqual(expected, actual);
        }
    

        [Test, Order(10)]
        public void userRemoveTest_Positive()
        {
            user.userLogin("nadav123456789@gmail.com", "Abbbd657");
            string actual = user.userRemove("nadav123456789@gmail.com");
            string expected = JsonSerializer.Serialize(goodResponse);
            Assert.AreEqual(expected, actual);
        }
        [Test, Order(11)]
        public void userRemoveTest_NegativeNotRegisterd()
        {
            string actual = user.userRemove("mail@mail.com");
            string expected = JsonSerializer.Serialize(new Response("user is not existed"));
            Assert.AreEqual(expected, actual);
        }

        [Test, Order(12)]
        public void userRemoveTest_NegativeNotlogedin()
        {
            user.userRegister("Email@mail.com", "ABC1112x");
            user.userLogout("Email@mail.com");
            string actual = user.userRemove("Email@mail.com");
            string expected = JsonSerializer.Serialize(new Response("please log in first!"));
            Assert.AreEqual(expected, actual);
        }

        [Test, Order(13)]
        public void userEditpasswordpositive()
        {
            user.userRegister("nadav12345678@gmail.com", "Abbbd6578");

            string actual = user.userEditPassword("nadav12345678@gmail.com", "Abbbd6578", "NAdav1234");
            Response R = new Response();
            string expected = JsonSerializer.Serialize(R);
            Assert.AreEqual(expected, actual);
        }

        [Test, Order(14)]
        public void userEditpasswordNegativeUnvalidPassword()
        {
            string actual = user.userEditPassword("nadav12345678@gmail.com", "NAdav1234", "NN1234");
            string expected = JsonSerializer.Serialize(new Response("the password must contain at least one lower, one upper case letter and one number "));
            Assert.AreEqual(expected, actual);
        }

        [Test, Order(15)]
        public void userEditpasswordNegativeSamePassword()
        {
            string actual = user.userEditPassword("nadav12345678@gmail.com", "NAdav1234", "NAdav1234");
            string expected = JsonSerializer.Serialize(new Response("Error: you can't change the password to your old password"));
            Assert.AreEqual(expected, actual);
        }


        [Test, Order(16)]
        /// </summary>
        /// This function tests Requirement 12
        /// </summary>
        public void userJoinBoardPositive()
        {
            user.userRegister("nadavnadav1@gmail.com", "ABC123de");

            boardService.addBoard("nadavnadav1@gmail.com", "toJoin");
            string actual = user.joinBoard("nadav12345678@gmail.com", 1);
            Response toReturn = new Response();
            string expected = JsonSerializer.Serialize(toReturn);
            Assert.AreEqual(expected, actual);
        }

        [Test, Order(17)]
        /// </summary>
        /// This function tests Requirement 12
        /// </summary>
        public void userJoinBoardNgativeSameName()
        {
            user.userRegister("nadavnadav12@gmail.com", "ABC123de");

            boardService.addBoard("nadavnadav12@gmail.com", "toJoin");
            string actual = user.joinBoard("nadav12345678@gmail.com", 2);
            string expected = JsonSerializer.Serialize(new Response("this user alredy have a board with the same name"));
            Assert.AreEqual(expected, actual);
        }


        [Test, Order(18)]
        /// </summary>
        /// This function tests Requirement 12
        /// </summary>
        public void userLeaveBoardNegative()
        {
            user.userRegister("nadavnadav@gmail.com", "ABC123de");

            string actual = user.leaveBoard("nadavnadav@gmail.com", 2);
            string expected = JsonSerializer.Serialize(new Response("this user hasnt joined the board"));
            Assert.AreEqual(expected, actual);
        }


        [Test, Order(19)]
        /// </summary>
        /// This function tests Requirement 12
        /// </summary>
        public void userLeaveBoardPositive()
        {
            string actual = user.leaveBoard("nadav12345678@gmail.com", 1);
            Response toReturn = new Response();
            string expected = JsonSerializer.Serialize(toReturn);
            Assert.AreEqual(expected, actual);
        }

        
        [Test, Order(20)]
        /// </summary>
        /// This function tests Requirement 13
        /// </summary>
        public void userTransferOwnershipPositive()
        {
            boardService.addBoard("nadavnadav12@gmail.com", "transfer");
            user.joinBoard("nadavnadav@gmail.com", 3);
            string actual = user.transferOwnership("nadavnadav12@gmail.com", "nadavnadav@gmail.com", "transfer");
            Response toReturn = new Response();
            string expected = JsonSerializer.Serialize(toReturn);
            Assert.AreEqual(expected, actual);
        }

        
        [Test, Order(21)]
        /// </summary>
        /// This function tests Requirement 13
        /// </summary>
        public void userTransferOwnershipNegativeNotOwner()
        {
            user.userRegister("nadavNotOwner@gmail.com", "notOwner11");
            boardService.addBoard("nadavNotOwner@gmail.com", "notOwner");
            user.joinBoard("nadavnadav@gmail.com", 1);
            string actual = user.transferOwnership("nadavnadav@gmail.com", "nadavNotOwner@gmail.com", "toJoin");
            Response toReturn = new Response("only the owner can transfer the ownership");
            string expected = JsonSerializer.Serialize(toReturn);
            Assert.AreEqual(expected, actual);
        }

        [Test, Order(22)]
        /// </summary>
        /// This function tests Requirement 13
        /// </summary>
        public void userTransferOwnershipNegativeNotExist()
        {

            string actual = user.transferOwnership("nadavNotOwner@gmail.com", "nadavnadavqq@gmail.com", "notOwner");
            Response toReturn = new Response("the new owner must join the board first");
            string expected = JsonSerializer.Serialize(toReturn);
            Assert.AreEqual(expected, actual);
        }

        [Test, Order(23)]
        /// </summary>
        /// This function tests Requirement 14
        /// </summary>
        public void userLeaveBoardNegativeOwner()
        {

            string actual = user.leaveBoard("nadavnadav12@gmail.com", 2);
            string expected = JsonSerializer.Serialize(new Response("board owner cant leave his board"));
            Assert.AreEqual(expected, actual);
        }





    }
}