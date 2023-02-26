using IntroSE.Kanban.Backend.ServiceLayer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BackendTests
{
    internal class t5_GratingServiceTests
    {


        GradingService gradingService = new GradingService();
        Response goodResponse = new Response();





        [Test, Order(1)]
        /// </summary>
        /// This function tests Requirement 7
        /// </summary>
        public void LoadData()
        {

            gradingService.DeleteData();



        }

        [Test, Order(1)]
        /// </summary>
        /// This function tests Requirement 7
        /// </summary>
        
        public void userRegisteTest_NegativePassword()
        {
            string actual = gradingService.Register("david@gmail.com", "657");
            string expected = JsonSerializer.Serialize(new Response("the password needs to be of size 6-20 charcters"));
            Assert.AreEqual(actual, expected);
        }

        [Test, Order(2)]
        /// </summary>
        /// This function tests Requirement 7
        /// </summary>
        public void userRegisteTest_Positive()
        {
            string b = gradingService.Register("david@gmail.com", "Abbbd657");
            var a = JsonSerializer.Serialize(new Response());
            Assert.AreEqual(a, b);
        }

        [Test, Order(3)]
        /// </summary>
        /// This function tests Requirement 7
        /// </summary>
        public void userRegisteTest_NegativeSameEmail()
        {
            string b = gradingService.Register("david@gmail.com", "Abb2157");
            string a = JsonSerializer.Serialize(new Response("Error: this email is already assigned to a user, please try to reset password"));
            Assert.AreEqual(a, b);
        }

        [Test, Order(4)]
        /// </summary>
        /// This function tests Requirement 7
        /// </summary>
        public void userRegisteTest_NegativeNull()
        {
            string b = gradingService.Register(null, "Abb2157");
            string a = JsonSerializer.Serialize(new Response("Email is null"));
            Assert.AreEqual(a, b);
        }

        [Test, Order(5)]
        /// </summary>
        /// This function tests Requirement 8
        /// </summary>
        public void userloginTest_Positive()
        {
            string actual = gradingService.Register("nadav@gmail.com", "Abbbd657");
            Response email = new Response();

            string expected = JsonSerializer.Serialize(email);
            Assert.AreEqual(expected, actual);
        }

        [Test, Order(6)]
        /// </summary>
        /// This function tests Requirement 8
        /// </summary>
        public void userloginTest_negativeAlredy()
        {
            string actual = gradingService.Login("nadav@gmail.com", "Abbbd657");
            string expected = JsonSerializer.Serialize(new Response("user already looged in"));
            Assert.AreEqual(actual, expected);
        }

        [Test, Order(7)]
        //// </summary>
        /// This function tests Requirement 8
        /// </summary>
        public void userloginTest_negativeNotRegistered()
        {
            string actual = gradingService.Login("nadav123@gmail.com", "Abbbd657");
            string expected = JsonSerializer.Serialize(new Response("user is not registerd"));
            Assert.AreEqual(actual, expected);
        }

        [Test, Order(8)]
        /// </summary>
        /// This function tests Requirement 8
        /// </summary>
        public void userlogOutTest_Positive()
        {
            gradingService.Register("nadav1234@gmail.com", "Abbbd657");
            gradingService.Login("nadav1234@gmail.com", "Abbbd657");
            string actual = gradingService.Logout("nadav1234@gmail.com");
            string expected = JsonSerializer.Serialize(goodResponse);
            Assert.AreEqual(actual, expected);
        }
        [Test, Order(9)]
        /// </summary>
        /// This function tests Requirement 8
        /// </summary>
        public void userlogOutTest_NegativeNotlogedin()
        {
            gradingService.Register("nadav123456789@gmail.com", "Abbbd657");
            string actual = gradingService.Logout("nadav123456789@gmail.com");
            string expected = JsonSerializer.Serialize(new Response());
            Assert.AreEqual(expected, actual);
        }


        [Test, Order(16)]
        /// </summary>
        /// This function tests Requirement 8
        /// </summary>
        public void userJoinBoardPositive()
        {
            gradingService.Register("nadavnadav1@gmail.com", "ABC123de");
            gradingService.Login("nadavnadav1@gmail.com", "ABC123de");
            gradingService.AddBoard("nadavnadav1@gmail.com", "toJoin");
            gradingService.Register("nadav12345678@gmail.com", "1!sKfsfs");
            gradingService.Login("nadav12345678@gmail.com", "1!sKfsfs");
            string actual = gradingService.JoinBoard("nadav12345678@gmail.com", 1);
            Response toReturn = new Response();
            string expected = JsonSerializer.Serialize(toReturn);
            Assert.AreEqual(expected, actual);
        }

        [Test, Order(17)]
        /// </summary>
        /// This function tests Requirements 6 and 12
        /// </summary>
        public void userJoinBoardNgativeSameName()
        {
            gradingService.Register("nadavnadav12@gmail.com", "ABC123de");
            gradingService.Login("nadavnadav12@gmail.com", "ABC123de");
            gradingService.AddBoard("nadavnadav12@gmail.com", "toJoin");
            string actual = gradingService.JoinBoard("nadav12345678@gmail.com", 2);
            string expected = JsonSerializer.Serialize(new Response("this user alredy have a board with the same name"));
            Assert.AreEqual(expected, actual);
        }


        [Test, Order(18)]
        /// </summary>
        /// This function tests Requirement 12
        /// </summary>
        public void userLeaveBoardNegative()
        {
            gradingService.Register("nadavnadav@gmail.com", "ABC123de");
            gradingService.Login("nadavnadav@gmail.com", "ABC123de");
            string actual = gradingService.LeaveBoard("nadavnadav@gmail.com", 2);
            string expected = JsonSerializer.Serialize(new Response("this user hasnt joined the board"));
            Assert.AreEqual(expected, actual);
        }


        [Test, Order(19)]
        /// </summary>
        /// This function tests Requirement 12
        /// </summary>
        public void userLeaveBoardPositive()
        {
            string actual = gradingService.LeaveBoard("nadav12345678@gmail.com", 1);
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
            gradingService.AddBoard("nadavnadav12@gmail.com", "transfer");
            gradingService.JoinBoard("nadavnadav@gmail.com", 3);
            string actual = gradingService.TransferOwnership("nadavnadav12@gmail.com", "nadavnadav@gmail.com", "transfer");
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
            gradingService.Register("nadavNotOwner@gmail.com", "notOwner11");
            gradingService.Login("nadavNotOwner@gmail.com", "notOwner11");
            gradingService.AddBoard("nadavNotOwner@gmail.com", "notOwner");
            gradingService.JoinBoard("nadavnadav@gmail.com", 1);
            string actual = gradingService.TransferOwnership("nadavnadav@gmail.com", "nadavNotOwner@gmail.com", "toJoin");
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

            string actual = gradingService.TransferOwnership("nadavNotOwner@gmail.com", "nadavnadav@gmail.com", "notOwner");
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

            string actual = gradingService.LeaveBoard("nadavnadav12@gmail.com", 2);
            string expected = JsonSerializer.Serialize(new Response("board owner cant leave his board"));
            Assert.AreEqual(expected, actual);
        }

        [Test, Order(24)]
        /// </summary>
        /// This function tests Requirement 15
        /// </summary>
        public void userLeaveBoardPositiveAsigned()
        {
            string actual = gradingService.LeaveBoard("nadavnadav@gmail.com", 1);
            string expected = JsonSerializer.Serialize(new Response());
            Assert.AreEqual(expected, actual);

        }


    }
}
