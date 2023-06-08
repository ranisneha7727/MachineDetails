using MachineAPI.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MachineAPI.Models
{
    public class AddMachineDetails 
    {
        public static async Task AddMachineDetailsinDB(MachineDetails objDetails)
        {
            try
            {
                var client = new MongoClient(Constants.connectionString);
                var db = client.GetDatabase(Constants.dbName);
                var collectionData = db.GetCollection<BsonDocument>(Constants.collectionName);
                BsonDocument documnt = new BsonDocument
                {
                    { "machineName", objDetails.MachineName},
                    { "assetName", objDetails.AssetName},
                    { "seriesNo" , objDetails.SeriesNo}
                };
                await collectionData.InsertOneAsync(documnt);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to Create Client......." + ex.Message);
            }
        }
    }
}
