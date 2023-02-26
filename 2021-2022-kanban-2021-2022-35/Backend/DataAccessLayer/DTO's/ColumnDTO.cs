using IntroSE.Kanban.Backend.DataAccessLayer.DAO;
using IntroSE.Kanban.Backend.DataAccessLayer.DAOs;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class ColumnDTO : DTOAbstract
    {
        //fields
        public const string columnKeyColumnName = "key";
        public const string columnNameColumnName = "name";
        public const string columnLimitColumnName = "limit";
        public const string columnBoardColumnName = "board";


        private int _index;
        public int index { get => _index; set { _index = value; controller.Update(index, columnKeyColumnName, value); } }
        private string _name;
        public string name { get => _name; set { _name = value; controller.Update(index, columnNameColumnName, value); } }

        private int _limit;
        public int limit { get => _limit; set { _limit = value; controller.Update(index, columnLimitColumnName, value); } }
        private int _boardId;
        public int boardId { get => _boardId; set { _boardId = value; controller.Update(index, columnBoardColumnName, value); } }


        //constractor
        public ColumnDTO(int key, string name, int limit, int boardId) : base(new ColumnDAO())
        {
            _index= key;
            _name = name;
            _limit = limit;
            _boardId = boardId;
        }



    }
}
