using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DAOs;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTO
{
    public class BoardDTO : DTOAbstract
    {
        //fields

        public const string boardIDColumnName = "id";
        public const string boardNameColumnName = "name";
        public const string boardOwnerColumnName = "owner";

        private int _id;
        public int id { get => _id; set { _id = value; controller.Update(id, boardIDColumnName, value); } }
        private string _name;
        public string name { get => _name; set { _name = value; controller.Update(id, boardNameColumnName, value); } }

        private string _owner;
        public string owner { get => _owner; set { _owner = value; controller.Update(id, boardOwnerColumnName, value); } }
  




        //constractor
        public BoardDTO(int id, string name, string owner): base(new BoardDAO())
        {            
            this._id = id;
            this._name = name;
            this._owner = owner;
        }


    }
}

