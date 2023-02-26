using NUnit.Framework;
using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using IntroSE.Kanban.Backend.BussinessLayer;
using System.Text.Json;
using IntroSE.Kanban.Backend.ServiceLayer.Classes;

namespace UserServiceTests
{
    public class t4_TaskServiceTests
    {
        ServiceFactory factory = new ServiceFactory();
        BoardService boardService;
        ColumnService columnService;
        UserService user;
        Response goodResponse = new Response();
        TaskService taskService;
        BoardController boardController;
        GradingService gradingService;



        [OneTimeSetUp]
        public void setUp()
        {
            boardService = factory.boardService;
            columnService = factory.columnService;
            user = factory.userService;
            taskService = factory.taskService;
            boardService.DeleteData();
            boardController = factory.boardController;
            




            user.userRegister("Davidvolo@gmail.com", "34567cvbA");

            boardService.addBoard("Davidvolo@gmail.com", "columnTests");
            taskService.addNewTask("Davidvolo@gmail.com", 1 , "fortest", "hhh", new DateTime(2029, 5, 1, 8, 30, 52));
            taskService.addNewTask("Davidvolo@gmail.com" , 1, "fortestNeg", "legal", new DateTime(2029, 5, 1, 8, 30, 52));
            taskService.assignedTask("Davidvolo@gmail.com", "columnTests", 0, 1, "Davidvolo@gmail.com");


          
        }
        
        [Test, Order(1)]
        /// </summary>
        /// This function tests Requirement 20
        /// This function tests Requirement 21
        /// </summary>
        public void editTaskTitleTestPositive()
        {
            string b = taskService.taskEditTitle("Davidvolo@gmail.com", 1, 0, 1, "dfjlk");
            var a = JsonSerializer.Serialize(goodResponse);
            Assert.AreEqual(a, b);
        }

       
        [Test, Order(3)]
        /// </summary>
        /// This function tests Requirement 20
        /// </summary>
        public void editTaskTitleTestNegetive2()
        {
            string b = taskService.taskEditTitle("Davidvolo@gmail.com", 1, 0, 1, "sdfghjgkjhgkhgkjhkjjdhfdddddddddddddddddddddddddddddddddddghghfdsdfghhghfdshjkgjhddfhghfh");
            var a = JsonSerializer.Serialize(new Response("new title must be shorter then 50 chars and not empty"));
            Assert.AreEqual(a, b);
        }

        
        [Test, Order(4)]
        /// </summary>
        /// This function tests Requirement 20
        /// </summary>
        public void editTaskTitleTestNegetive3()
        {
            string b = taskService.taskEditTitle("Davidvolo@gmail.com", 1, 0, 1, "");
            var a = JsonSerializer.Serialize(new Response("new title must be shorter then 50 chars and not empty"));
            Assert.AreEqual(a, b);
        }

        
        [Test, Order(5)]
        /// </summary>
        /// This function tests Requirement 20
        /// </summary>
        public void editTaskTitleTestNegetive4()
        {
            string b = taskService.taskEditTitle("Davidvolo@gmail.com", 1, 0, 76, "");
            var a = JsonSerializer.Serialize(new Response("taskId is not in the column"));
            Assert.AreEqual(a, b);
        }

        
        [Test, Order(6)]
        /// </summary>
        /// This function tests Requirement 20
        /// This function tests Requirement 21
        /// </summary>
        public void editTaskDueDateTestPositive()
        {
            string b = taskService.taskEditDueDate("Davidvolo@gmail.com", 1, 0, 1, new DateTime(2030, 5, 1, 8, 30, 52));
            var a = JsonSerializer.Serialize(goodResponse);
            Assert.AreEqual(a, b);
        }

        
        [Test, Order(7)]
        /// </summary>
        /// This function tests Requirement 20
        /// </summary>
        public void editTaskDueDateTestNegative()
        {
            string b = taskService.taskEditDueDate("Davidvolo@gmail.com",1 , 0, 1, new DateTime(1900, 5, 1, 8, 30, 52));
            var a = JsonSerializer.Serialize(new Response("Error: due date can't be in the past"));
            Assert.AreEqual(a, b);
        }

        
        [Test, Order(9)]
        /// </summary>
        /// This function tests Requirement 20
        /// This function tests Requirement 21
        /// </summary>
        public void editTaskDescriptionTestPositive()
        {
            string b = taskService.taskEditDescription("Davidvolo@gmail.com", 1, 0, 1, "lhgkgkh");
            var a = JsonSerializer.Serialize(goodResponse);
            Assert.AreEqual(a, b);
        }


        
        [Test, Order(11)]
        /// </summary>
        /// This function tests Requirement 19
        /// </summary>
        public void moveTaskPositive()
        {
            string actual = taskService.taskMove("Davidvolo@gmail.com", 1, 0, 1);
            string expected = JsonSerializer.Serialize(goodResponse);
            Assert.AreEqual(actual, expected);
        }

        [Test, Order(12)]
        /// </summary>
        /// This function tests Requirement 19
        /// </summary>
        public void moveTaskNegative()
        {
            
            taskService.taskMove("Davidvolo@gmail.com", 1, 1, 1);
            string actual = taskService.taskMove("Davidvolo@gmail.com", 1, 2, 1);
            Response response = new Response("done tasks cant move on");
            string expected = JsonSerializer.Serialize(response);
            Assert.AreEqual(actual, expected);
        }


        [Test, Order(12)]
        /// </summary>
        /// This function tests Requirement 18
        /// This function tests Requirement 15
        /// </summary>
        public void addNewTaskPositive()
        {
            user.userRegister("Nada@gmail.com", "sismaHadasha11");
            user.userRegister("nadavvvXX@gmail.com", "sismaHadasha11");
            user.joinBoard("nadavvvXX@gmail.com", 1);
            user.userLogout("nadavvvXX@gmail.com");
            user.joinBoard("Nada@gmail.com", 1);
            
            
            taskService.assignedTask("Nada@gmail.com", "columnTests", 1, 1, "nadavvvXX@gmail.com");
            user.userLogin("nadavvvXX@gmail.com", "sismaHadasha11");
            taskService.assignedTask("nadavvvXX@gmail.com", "columnTests", 1, 1, "Nada@gmail.com");
            user.leaveBoard("Nada@gmail.com",1 );
            taskService.assignedTask("nadavvvXX@gmail.com", "columnTests", 1, 1, "nadavvvXX@gmail.com");


            string id = boardService.addBoard("nada@gmail.com", "Board!1");
            string actual = taskService.addNewTask("nada@gmail.com", 1, "ntAsk", "des", DateTime.Now.AddDays(50));
            string expected = JsonSerializer.Serialize(goodResponse);
            Assert.AreEqual(actual, expected);
        }

        
        [Test, Order(13)]
        /// </summary>
        /// This function tests Requirement 18
        /// </summary>
        public void addNewTaskNegativeWrongDate()
        {
            string strId = boardService.addBoard("nada@gmail.com", "Board!2");
            string actual = taskService.addNewTask("nada@gmail.com", 1, "ntAsk", "des", DateTime.Now.AddDays(-1));
            string expected = JsonSerializer.Serialize(new Response("due date cannot be in the past"));
            Assert.AreEqual(actual, expected);
        }

        
        [Test, Order(14)]
        /// </summary>
        /// This function tests Requirement 18
        /// </summary>
        public void addNewTaskNegativeWrong()
        {
            string actual = taskService.addNewTask("nada@gmail.com", 1, "ntAsk", "deskdlvksc;lkds;vlk;vlkds;vkd;lvk;dlkv;sdkv;ldkv;lkdv;ldks;ksc;lkds;vlk;vlkds;vkd;lvk;dlkv;sdkv;ldkv;lkdv;ldks;ksc;lkds;vlk;vlkds;vkd;lvk;dlkv;sdkv;ldkv;lkdv;ldks;ksc;lkds;vlk;vlkds;vkd;lvk;dlkv;sdkv;ldkv;lkdv;ldks;ksc;lkds;vlk;vlkds;vkd;lvk;dlkv;sdkv;ldkv;lkdv;ldks;ksc;lkds;vlk;vlkds;vkd;lvk;dlkv;sdkv;ldkv;lkdv;ldks;ksc;lkds;vlk;vlkds;vkd;lvk;dlkv;sdkv;ldkv;lkdv;ldks;vlkfds;vlkdf;vlksd;vkds;vlks;dfkv;dskfv;sdkfv;ldkf;vdlkv;dlv;s", DateTime.Now.AddDays(4));
            string expected = JsonSerializer.Serialize(new Response("description must be shorter then 300 chars"));
            Assert.AreEqual(actual, expected);
        }

        
        [Test, Order(15)]
        /// </summary>
        /// This function tests Requirement 18
        /// </summary>
        public void addNewTaskNegativeWrongTitle()
        {
            string actual = taskService.addNewTask("nada@gmail.com", 1, "", "des", DateTime.Now.AddDays(33393));
            string expected = JsonSerializer.Serialize(new Response("title length must be between 1-50"));
            Assert.AreEqual(actual, expected);
        }

        
        [Test, Order(18)]
        /// </summary>
        /// This function tests Requirement 23
        /// </summary>
        public void assignedTasksPositive()
        {
            user.userRegister("nadaaa@gmail.com", "sismA111");

            user.joinBoard("nadaaa@gmail.com", 1);
            user.userRegister("nadav@gmail.com", "sismA111");

            user.joinBoard("nadav@gmail.com", 1);
            string actual = taskService.assignedTask("nadaaa@gmail.com", "columnTests", 0, 2, "nadav@gmail.com");
            string expected = JsonSerializer.Serialize(new Response());
            Assert.AreEqual(actual, expected);
        }
        [Test, Order(28)]
        /// </summary>
        /// This function tests Requirement 23
        /// </summary>
        public void removeBOardWithRasksPositive()
        {

            string actual = "";
            //string actual = boardService.RemoveBoard(1, "Davidvolo@gmail.com" );
            string expected = JsonSerializer.Serialize(new Response());
            Assert.AreEqual(actual, expected);
        }

        [Test, Order(29)]
        /// </summary>
        /// This function tests Requirement 23
        /// </summary>
        public void assignedTasksNegative()
        {

            string actual = taskService.assignedTask("nadaaarr@gmail.com", "columnTests", 0, 2, "nadav@gmail.com");
            string expected = JsonSerializer.Serialize(new Response("user is not existed"));
            Assert.AreEqual(actual, expected);
        }

        [Test, Order(99)]
        /// </summary>
        /// This function tests Requirement 22
        /// </summary>
        public void GetinProg()
        {

            string actual = taskService.inProgressTasks("nadavvvXX@gmail.com");
            Response response = new Response();
            response.toResponse(new System.Collections.Generic.List<Taskk>());
            string expected = JsonSerializer.Serialize(response);
            Assert.AreEqual(expected, actual);
        }

        

    }
}

