using MachineAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MachineAPI.Models
{
    public class DeleteAsset
    {
        public static async Task DeleteAssetDetails(MachineDetails objDetails)
        {
            try
            {
                var client = new MongoClient(Constants.connectionString);
                var db = client.GetDatabase(Constants.dbName);
                var collectionData = db.GetCollection<BsonDocument>(Constants.collectionName);

                await collectionData.FindOneAndDeleteAsync(Builders<BsonDocument>.Filter.Eq("machineName", objDetails.MachineName) & Builders<BsonDocument>.Filter.Eq("assetName", objDetails.AssetName));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to Create Client......." + ex.Message);
            }
        }
    }
}
