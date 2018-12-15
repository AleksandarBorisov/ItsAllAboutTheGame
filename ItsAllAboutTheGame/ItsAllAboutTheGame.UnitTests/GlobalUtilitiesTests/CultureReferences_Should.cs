using ItsAllAboutTheGame.GlobalUtilities;
using ItsAllAboutTheGame.GlobalUtilities.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ItsAllAboutTheGame.UnitTests.GlobalUtilitiesTests
{
    [TestClass]
    public class CultureReferences_Should
    {
        [TestMethod]
        public void ReturnCorrectSymbol_WhenCurrencyIsPresentInDictionary()
        {
            //This test is designed to prevent adding an unknown currency to our Currency enums

            //Arrange
            var currencies= GlobalConstants.Currencies.Split(',');

            var cultures = CultureReferences.CurrencySymbols.Keys.ToArray();

            //Act and Assert
            CollectionAssert.IsSubsetOf(currencies, cultures);
        }
    }
}
