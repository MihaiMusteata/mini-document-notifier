using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniDocumentNotifier.Domain.Models;

namespace MiniDocumentNotifier.Domain.Tests.Models
{
    [TestClass]
    public class UserPreferencesTest
    {
        [TestMethod]
        public void CreateDefault_ReturnsExpectedDefaultValues()
        {
            var result = UserPreferences.CreateDefault();

            Assert.AreEqual("UploadDate", result.DefaultSortColumn);
            Assert.IsTrue(result.DefaultSortDirection);
            Assert.IsNull(result.LastUsername);
            Assert.IsNotNull(result.ColumnWidths);
            Assert.IsEmpty(result.ColumnWidths);
        }

        [TestMethod]
        public void CreateDefault_ReturnsNewInstanceEachTime()
        {
            var first = UserPreferences.CreateDefault();
            var second = UserPreferences.CreateDefault();

            Assert.AreNotSame(first, second);
        }
    }
}