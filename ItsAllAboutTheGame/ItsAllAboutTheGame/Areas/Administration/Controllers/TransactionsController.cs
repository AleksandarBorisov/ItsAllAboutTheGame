using ItsAllAboutTheGame.GlobalUtilities.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItsAllAboutTheGame.Areas.Administration.Controllers
{
    //[Area(DataConstants.AdminArea)]
    [Authorize(Roles = GlobalConstants.AdminRole + "," + GlobalConstants.MasterAdminRole)]
    public class TransactionsController : Controller
    {

    }
}
