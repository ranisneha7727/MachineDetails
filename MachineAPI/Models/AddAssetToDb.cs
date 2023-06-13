using MachineAPI.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MachineAPI.Models
{
    public class AddAssetToDb
    {
        public static bool AddAssetOrMachineToDB(Machine machineDetails)
        {
            try
            {
                if (machineDetails != null && machineDetails.Assets.Count > 0)
                {
                    var client = new MongoClient(Constants.ConnectionString);
                    var db = client.GetDatabase(Constants.DbName);
                    var collectionData = db.GetCollection<BsonDocument>(Constants.MachineCollectionName);
                    BsonDocument documnt = new BsonDocument
                {
                    { Constants.ColumnNameMachine, machineDetails.MachineName},
                    { Constants.ColumnNameAsset, machineDetails.Assets[0].AssetName},
                    { Constants.ColumnNameSeriesNo , machineDetails.Assets[0].SeriesNo}
                };
                    collectionData.InsertOneAsync(documnt);
                    return true;
                }
                else
                { return false; }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to Add Asset......." + ex.Message);
                return false;
            }
        }
    }
}
