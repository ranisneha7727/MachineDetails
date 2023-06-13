using MachineAPI.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.RegularExpressions;

namespace MachineAPI.Models
{
    public class SearchInDb
    {
        //public static async Task<BsonDocument?> SearchInDB(string searchString)
        //{
        //    var client = new MongoClient(Constants.ConnectionString);
        //    var db = client.GetDatabase(Constants.DbName);
        //    var machinecollectionData = db.GetCollection<BsonDocument>(Constants.MachineCollectionName);
        //    var queryExp = new BsonRegularExpression(new Regex(search, RegexOptions.IgnoreCase));
        //    var builder = Builders<BsonDocument>.Filter;
        //    var filter = builder.Regex(Constants.ColumnNameMachine, queryExp);
        //    var documents = machinecollectionData.Find(filter).ToList();
        //    if (documents.Any())
        //        return Ok(documents.ConvertAll(BsonTypeMapper.MapToDotNetValue));
        //    else
        //    {
        //        filter = builder.Regex(Constants.ColumnNameAsset, queryExp);
        //        documents = machinecollectionData.Find(filter).ToList();
        //        if (documents.Any())
        //            return Ok(documents.ConvertAll(BsonTypeMapper.MapToDotNetValue));
        //        else
        //            return null;
        //    }
        //}

        //public static async Task<BsonDocument?> SearchAllInDB()
        //{
        //    var client = new MongoClient(Constants.ConnectionString);
        //    var db = client.GetDatabase(Constants.DbName);
        //    var machinecollectionData = db.GetCollection<BsonDocument>(Constants.MachineCollectionName);
        //    var documents = machinecollectionData.Find(new BsonDocument()).ToList();
        //    if (documents.Any())
        //    {
        //        return Ok((documents.ConvertAll(BsonTypeMapper.MapToDotNetValue)));
        //    }
        //    else
        //        return null;
        //}
    }
}
