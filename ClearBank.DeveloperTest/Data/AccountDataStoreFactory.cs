using System.Configuration;

namespace ClearBank.DeveloperTest.Data
{
    public class AccountDataStoreFactory
    {
        public IAccountDataStore GetAccountDataStore()
        {
            var dataStoreType = ConfigurationManager.AppSettings["DataStoreType"];

            return dataStoreType == "Backup"
                ? (IAccountDataStore) new BackupAccountDataStore()
                : new AccountDataStore();
        }
    }
}