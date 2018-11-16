using System;
using System.Collections.Generic;
using System.Text;

namespace ItsAllAboutTheGame.Data.Models.Abstract
{
    public interface IDeletable
    {
        DateTime? DeletedOn { get; set; }

        bool IsDeleted { get; set; }
    }
}
