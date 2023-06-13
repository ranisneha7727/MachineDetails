using MachineAPI.Models;
using MachineAPI.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using MongoDB.Driver.Linq;
using System.Text.RegularExpressions;

namespace MachineAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MachinesController : ControllerBase
    {
        public MachinesController()
        {
        }

        [HttpGet]
        public IActionResult? Search(string? search)
        {
            try
            {
                var client = new MongoClient(Constants.ConnectionString);
                var db = client.GetDatabase(Constants.DbName);
                var machinecollectionData = db.GetCollection<BsonDocument>(Constants.MachineCollectionName);
                if (!string.IsNullOrEmpty(search))
                {
                    var queryExp = new BsonRegularExpression(new Regex(search, RegexOptions.IgnoreCase));
                    var builder = Builders<BsonDocument>.Filter;
                    var filter = builder.Regex(Constants.ColumnNameMachine, queryExp);
                    var documents = machinecollectionData.Find(filter).ToList();
                    if (documents.Any())
                        return Ok(documents.ConvertAll(BsonTypeMapper.MapToDotNetValue));
                    else
                    {
                        filter = builder.Regex(Constants.ColumnNameAsset, queryExp);
                        documents = machinecollectionData.Find(filter).ToList();
                        if (documents.Any())
                            return Ok(documents.ConvertAll(BsonTypeMapper.MapToDotNetValue));
                        else
                            return null;
                    }
                }
                else
                {
                    var documents = machinecollectionData.Find(new BsonDocument()).ToList();
                    if (documents .Any())
                    {
                        return Ok((documents.ConvertAll(BsonTypeMapper.MapToDotNetValue)));
                    }
                    else
                        return null;
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult AddAsset(Machine machine)
        {
            try
            {
                if (machine.MachineName != null && machine.Assets.Count > 0)
                {
                    bool ifCreated = AddAssetToDb.AddAssetOrMachineToDB(machine);
                    if (ifCreated)
                        return Ok();
                    else
                        return BadRequest();
                }
                else
                    return BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("{machine}/{asset}")]
        public async Task<IActionResult> DeleteAssetAsync(string machine, string asset)
        {
            try
            {
                if (!string.IsNullOrEmpty(machine) && !string.IsNullOrEmpty(asset))
                {
                    var deletedDoc = await DeleteAssetFromDb.RemoveAssetFromMachine(machine, asset);
                    if (deletedDoc != null)
                        return Ok(deletedDoc);
                    else
                        return NotFound();
                }
                else
                    return BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }

        [Route("latest")]
        [HttpGet]
        public async Task<IActionResult?> LatestMachine()
        {
            try
            {
                DataTable? dataFromDB = new();
                DataTable? outSearchData = new ();

                var client = new MongoClient(Constants.ConnectionString);
                var db = client.GetDatabase(Constants.DbName);
                var machinecollectionData = db.GetCollection<BsonDocument>(Constants.MachineCollectionName);
                var documents = machinecollectionData.Find(new BsonDocument()).ToList();
                
                dataFromDB = ConvertToDataTable(documents);

                if (dataFromDB != null && dataFromDB.Rows.Count>0)
                {
                    outSearchData = GetLatestMachine(dataFromDB);
                    if (outSearchData != null && outSearchData.Rows.Count > 0)
                        return Ok(outSearchData);
                    else
                        return BadRequest();
                }
                else
                   return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private DataTable? ConvertToDataTable(List<BsonDocument> documents)
        {
            try
            {
                DataTable? dt = new DataTable();
                foreach (BsonElement elem in documents[0].Elements)
                {
                    dt.Columns.Add(elem.Name);
                }
                for (int i=0; i< documents.Count; i++) 
                {
                    DataRow row = dt.NewRow();
                    foreach (BsonElement elem in documents[i])
                    {
                        row[elem.Name] = elem.Value;
                    }
                    dt.Rows.Add(row);
                }
                return dt;
            }
            catch
            {
                return null;
            }
        }

        private static DataTable? GetLatestMachine(DataTable ipTble)
        {
            int latestCountCheck = 0;
            string[] machineNames, assetNames;
            DataTable outData = new DataTable();

            if (ipTble != null)
            {
                machineNames = ipTble.DefaultView.ToTable(true, Constants.ColumnNameMachine).AsEnumerable().Select(r => r.Field<string>(Constants.ColumnNameMachine)).ToArray();
                assetNames = ipTble.DefaultView.ToTable(true, Constants.ColumnNameAsset).AsEnumerable().Select(r => r.Field<string>(Constants.ColumnNameAsset)).ToArray();
                foreach (string machineName in machineNames)
                {
                    DataRow[] filteredAssetDetails = ipTble.Select("MachineName like '" + machineName + "'");
                    foreach (DataRow assetDetails in filteredAssetDetails)
                    {
                        string strLatestSeriesOfAsset = $"S{ipTble.Select("AssetName like '" + assetDetails[Constants.ColumnNameAsset].ToString() + "'").Max(r => Convert.ToInt64(r[Constants.ColumnNameSeriesNo].ToString().Trim().Remove(0, 1)))}";
                        if (assetDetails[Constants.ColumnNameSeriesNo].ToString().Trim().Equals(strLatestSeriesOfAsset, StringComparison.InvariantCultureIgnoreCase))
                            latestCountCheck++;
                    }
                    if (latestCountCheck == filteredAssetDetails.GetLength(0))
                    {
                        outData = filteredAssetDetails.CopyToDataTable();
                    }
                    latestCountCheck = 0;
                }
                return outData;
            }
            else
                return null;
        }
    }
}