using ItsAllAboutTheGame.GlobalUtilities.Enums;
using System;
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

        public const string DefaultUserSorting = "Username_asc";

        public const string DefaultTransactionSorting = "CreatedOn_desc";

        public const string UsersRolesCache = "UsersRoles";

        public static string DepositDescription = "Deposit with card ";

        public static string StakeDescription = "Stake on game ";

        public static string WinDescription = "Win on game ";

        public static string GameOneGrid = "4x3";

        public static string WithdrawDescription = "Withdraw with card ";

        public static string GameTwoGrid = "5x5";

        public static string GameThreeGrid = "8x5";

        public static HashSet<string> GameGrids = new HashSet<string>() { GameOneGrid, GameTwoGrid, GameThreeGrid };

        public static int MaxPageCount = 5;

        public const string BaseCurrency = "USD";

        public static string BaseCurrencySymbol = "$";

        public static string Currencies = string.Join(",", Enum.GetNames(typeof(Currency)));

    }
}
