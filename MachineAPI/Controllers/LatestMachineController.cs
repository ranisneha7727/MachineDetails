using MachineAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace MachineAPI.Controllers
{
    [Route("api/v1/machines/latest")]
    [ApiController]
    public class LatestMachineController : ControllerBase
    {
        public DataTable? DataFromDB = new DataTable();
        public DataTable? outSearchData;
        public DataTable? DataFromFile;
        public LatestMachineController()
        {
            try
            {
                DataFromDB = GetMachineDetailsFromSource.LoadDataFromDB();
                outSearchData = DataFromDB.Clone();
            }
            catch (Exception ex)
            {
                BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult LatestMachinetype()
        {
            try
            {
                string[] MachineNames;
                string[] AssetNames;
                int LatestCountCheck = 0;

                if (outSearchData != null)
                    outSearchData.Clear();
                if (DataFromDB != null)
                {
                    MachineNames = DataFromDB.DefaultView.ToTable(true, "MachineName").AsEnumerable().Select(r => r.Field<string>("MachineName")).ToArray();
                    AssetNames = DataFromDB.DefaultView.ToTable(true, "AssetName").AsEnumerable().Select(r => r.Field<string>("AssetName")).ToArray();
                    foreach (string machineName in MachineNames)
                    {
                        DataRow[] filteredAssetDetails = DataFromDB.Select("MachineName like '" + machineName + "'");
                        foreach (DataRow assetDetails in filteredAssetDetails)
                        {
                            string strLatestSeriesOfAsset = $"S{DataFromDB.Select("AssetName like '" + assetDetails["AssetName"].ToString() + "'").Max(r => Convert.ToInt64(r["SeriesNo"].ToString().Trim().Remove(0, 1)))}";
                            if (assetDetails["SeriesNo"].ToString().Trim().Equals(strLatestSeriesOfAsset, StringComparison.InvariantCultureIgnoreCase))
                                LatestCountCheck++;
                        }
                        if (LatestCountCheck == filteredAssetDetails.GetLength(0))
                        {
                            outSearchData = filteredAssetDetails.CopyToDataTable();
                        }
                        LatestCountCheck = 0;
                    }
                }
                else
                    return BadRequest();
                if (outSearchData != null)
                    return Ok(outSearchData);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
