using MachineAPI.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics.Eventing.Reader;

namespace MachineAPI.Models
{
    public class DeleteAssetFromDb
    {
        public static async Task<BsonDocument?> RemoveAssetFromMachine(string machineName, string assetName)
        {
            try
            {
                var client = new MongoClient(Constants.ConnectionString);
                var db = client.GetDatabase(Constants.DbName);
                var collectionData = db.GetCollection<BsonDocument>(Constants.MachineCollectionName);
                var asset = await collectionData.FindOneAndDeleteAsync(Builders<BsonDocument>.Filter.Eq(Constants.ColumnNameMachine, machineName) & Builders<BsonDocument>.Filter.Eq(Constants.ColumnNameAsset, assetName));
                if (asset != null)
                {
                    return asset;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to Create Client......." + ex.Message);
                return null;
            }
        }
    }
}
