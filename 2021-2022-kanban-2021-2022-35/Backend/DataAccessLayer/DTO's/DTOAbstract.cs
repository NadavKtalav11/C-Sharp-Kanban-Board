using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    public abstract class DTOAbstract
    {
        protected DAOAbstract controller;

        protected DTOAbstract(DAOAbstract controller)
        {
            this.controller = controller;
        }



    }

}
