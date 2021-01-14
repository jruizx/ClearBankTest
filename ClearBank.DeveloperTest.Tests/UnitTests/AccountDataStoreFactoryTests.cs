using ClearBank.DeveloperTest.Data;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.UnitTests
{
    public class AccountDataStoreFactoryTests
    {
        [Test]
        public void GetAccountDataStoreShouldReturnAccountDataStoreWhenTheDataStoreTypeIsNotBackup()
        {
            var result = new AccountDataStoreFactory().GetAccountDataStore();

            Assert.IsInstanceOf<AccountDataStore>(result);
        }
    }
}