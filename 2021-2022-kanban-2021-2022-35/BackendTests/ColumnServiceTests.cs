using NUnit.Framework;
using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using IntroSE.Kanban.Backend.BussinessLayer;
using System.Text.Json;
using IntroSE.Kanban.Backend.ServiceLayer.Classes;

namespace UserServiceTests
{
    public class t3_ColumnServiceTests
    {

        Factory factory = new Factory();
        BoardService boardService;
        ColumnService columnService;
        UserService user;
        Response goodResponse = new Response();
        TaskService taskService;
        BoardController boardController;


        [OneTimeSetUp]
        public void setUp()
        {
            boardService = factory.boardService;    
            columnService = factory.columnService;
            user = factory.userService;
            taskService = factory.taskService;
            boardController = factory.boardController;
            boardService.DeleteData();
            
            

            user.userRegister("davidvolo@gmail.com", "34567cvbA");

            boardService.addBoard("davidvolo@gmail.com", "columnTests");
            taskService.addNewTask("davidvolo@gmail.com" ,1, "fortest", "hhh", new DateTime(2029, 5, 1, 8, 30, 52));
            taskService.addNewTask("davidvolo@gmail.com", 1, "fortestNeg", "illegal", new DateTime(2029, 5, 1, 8, 30, 52));



        }

         [Test, Order(33)]
        /// </summary>
        // This function tests Requirement 16
        /// </summary>
        public void getLimitPositvieTest()
        {
            string actual = columnService.getLimit("davidvolo@gmail.com", 1, 1);
            Response response = new Response();
            response.toResponse(-1);
            string expected = JsonSerializer.Serialize(response);
            Assert.AreEqual(expected, actual);
        }

        
        [Test, Order(34)]
        /// </summary>
        /// This function tests Requirement 16
        /// </summary>
        public void editLimitPositvieTest()
        { 
            string b = columnService.editLimit("davidvolo@gmail.com", 1,1,7);
            var a = JsonSerializer.Serialize(goodResponse);
            Assert.AreEqual(a, b);
        }

        
        [Test, Order(35)]
        /// </summary>
        /// This function tests Requirement 16
        /// </summary>
        public void editLimitNegetivetest()
        {
            string b = columnService.editLimit("davidvolo@gmail.com", 1, 1, -7);
            var a = JsonSerializer.Serialize(new Response("limit can't be smaller than zero"));
            Assert.AreEqual(a, b);
        }

        
        [Test, Order(36)]
        /// </summary>
        /// This function tests Requirement 16
        /// </summary>
        public void editLimitNegetiveAmounttest()
        {
            string b = columnService.editLimit("davidvolo@gmail.com", 1, 0, 1);
            var a = JsonSerializer.Serialize(new Response("the new limit, can't be smaller than the amount of tasks that are already in the column"));
            Assert.AreEqual(a, b);
        }

        [Test, Order(36)]
        /// </summary>
        /// This function tests Requirement 16
        /// </summary>
        public void InitalizeLimitTest()
        {
            boardService.addBoard("davidvolo@gmail.com", "columnLimitTests");
            int a = boardController.getBoard(1).GetColumn(0).limit;
            int b = -1;
          
            Assert.AreEqual(a, b);
        }


    }

}