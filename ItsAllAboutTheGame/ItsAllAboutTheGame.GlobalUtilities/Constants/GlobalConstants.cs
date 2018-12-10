using System.Collections.Generic;

namespace ItsAllAboutTheGame.GlobalUtilities.Constants
{
    public class GlobalConstants
    {
        public const string AdminArea = "Administration";

        public const string AdminRole = "Administrator";

        public const string AdminEmail = "administrator@adminsite.com";

        public const string MasterAdminRole = "MasterAdministrator";

        public const string MasterAdminEmail = "masteradministrator@adminsite.com";

        public const int DefultPageSize = 5;

        public const string DefultUserSorting = "Username_asc";

        public const string DefultTransactionSorting = "CreatedOn_asc";

        public const string UsersRolesCache = "UsersRoles";

        public static string DepositDescription = "Deposit with card ";

        public static string StakeDescription = "Stake on game ";

        public static string WinDescription = "Win on game ";

        public static string GameOneGrid = "4x3";

        public static string WithdrawDescription = "Withdrawal of ";

        public static string GameTwoGrid = "5x5";

        public static string GameThreeGrid = "8x5";

        public static HashSet<string> GameGrids = new HashSet<string>() { GameOneGrid, GameTwoGrid, GameThreeGrid };

    }
}
