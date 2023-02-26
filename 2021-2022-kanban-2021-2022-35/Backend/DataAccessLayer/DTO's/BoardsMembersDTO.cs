using IntroSE.Kanban.Backend.DataAccessLayer.DAO_s;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTO_s
{
    internal class BoardsMembersDTO:DTOAbstract
    {
        //fields

        public const string boardMembersIDColumnName = "BoardId";
        public const string boardMembersEmailColumnName = "UserEmail";



        private string _email;
        public string email { get => _email; set { _email = value; controller.Update(_email, boardMembersEmailColumnName, value); } }
        private int _boardId;
        public int boardId { get => _boardId; set { _boardId = value; controller.Update(_email, boardMembersIDColumnName, value); } }





        //constractor
        public BoardsMembersDTO(int boardId, string email): base(new BoardsMembersDAO())
        {
            this._boardId = boardId;
            this._email = email;
        }
    }
}
