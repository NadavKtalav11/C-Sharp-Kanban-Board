using IntroSE.Kanban.Backend.BussinessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DAO;
using IntroSE.Kanban.Backend.DataAccessLayer.DTO;
using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BackendTests
{
    internal class Program
    {
        static void Main(string[] args)
        {

            GradingService gradingService = new GradingService();
            Console.WriteLine(gradingService.LoadData());
            Console.WriteLine(gradingService.Login("Nada@gmail.com", "sismaHadasha11"));
            Console.WriteLine(gradingService.AddBoard("Nada@gmail.com", "boardnew"));
            Console.WriteLine(gradingService.AddTask("Nada@gmail.com", "Board!1", "title  new", "   rr  ", DateTime.Now.AddDays(7)));




            //gradingService.Login("ehad@gmail.com", "1231231WWe");
            //gradingService.AddBoard("ehad@gmail.com", "boardme");
            //gradingService.AddTask("ehad@gmail.com" , "boardme" ,"newtit" , "new dess" ,DateTime.Now.AddDays(6));


           }
        }
    }

