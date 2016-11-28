using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using System.Collections.ObjectModel;

namespace Products1.Services
{
    public class AzureClient
    {
        private IMobileServiceClient _client;
        private IMobileServiceSyncTable<DeviceUser> _table;
        private const string serviceUri = "http://xazureconcept.azurewebsites.net/";
        private const string dbPath = "productsDb";

        public AzureClient()
        {
            _client = new MobileServiceClient(serviceUri);
            var store = new MobileServiceSQLiteStore(dbPath);
            store.DefineTable<DeviceUser>();
            _client.SyncContext.InitializeAsync(store);
            _table = _client.GetSyncTable<DeviceUser>();
        }

        public async Task<IEnumerable<DeviceUser>> GetDeviceUsers()
        {
            if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
                await SyncAsync();

            return await _table.ToEnumerableAsync();
        }

        public async Task AddDeviceUser(DeviceUser user)
        {
            try
            {
                await _table.InsertAsync(user);
            }
            catch (Exception ex)
            {
                var messa = ex.Message;
                throw;
            }
        }

        public async Task SyncAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;
            try
            {
                await _client.SyncContext.PushAsync();
                await _table.PullAsync("allDeviceUsers", _table.CreateQuery());
            }
            catch (MobileServicePushFailedException pushEx)
            {
                if (pushEx.PushResult != null)
                    syncErrors = pushEx.PushResult.Errors;
            }
        }

        public async Task CleanData()
        {
            await _table.PurgeAsync("allDeviceUsers", _table.CreateQuery(), new System.Threading.CancellationToken());
        }
    }
}
