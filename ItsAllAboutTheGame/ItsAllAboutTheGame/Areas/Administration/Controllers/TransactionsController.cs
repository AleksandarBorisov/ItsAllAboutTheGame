using ItsAllAboutTheGame.Data.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItsAllAboutTheGame.Areas.Administration.Controllers
{
    //[Area(DataConstants.AdminArea)]
    [Authorize(Roles = DataConstants.AdminRole + "," + DataConstants.MasterAdminRole)]
    public class TransactionsController : Controller
    {

    }
}
