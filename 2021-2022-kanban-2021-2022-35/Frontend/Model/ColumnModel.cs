using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    public class ColumnModel : NotifiableModelObject
    {


        public ObservableCollection<TaskModel> Tasks { get; set; }

        private string name;
        public string Name
        {
            get => name;
            set { name = value; }
        }

        private int columnIndex;
        public int ColumnIndex
        {
            get => columnIndex;
            set { columnIndex = value; }
        }

        private string Limit;
        public string limit
        {
            get => Limit;

        }



        private int boardId;
        public int BoardId
        {
            get => boardId;
            set { boardId = value; }
        }


        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                this._title = value;
            }
        }



        public ColumnModel(BackendController controller, int columnIndex, string name, int limit, BoardModel board) : base(controller)
        {
            this.Title = name;
            this.boardId = board.Id;
            this.name = name;
            this.Limit = "column Limit is " + limit;
            this.columnIndex = columnIndex;
            Tasks = new ObservableCollection<TaskModel>(controller.GetColumnTasks(board.Email, board.Id , columnIndex).
                Select((c) => new TaskModel(controller, c , this)).ToList());

        }

        public ColumnModel(BackendController controller, (int columnIndex, string name, int limit) column, BoardModel board) : this(controller, column.columnIndex ,column.name ,column.limit,  board) { }



    }
}
