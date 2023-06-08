using MachineAPI.Entities;
using MachineAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MachineAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MachinesController : ControllerBase
    {
        public DataTable? DataFromDB = new DataTable();
        public DataTable? outSearchData;
        public DataTable? DataFromFile;
        public MachinesController()
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
        public IActionResult Search()
        {
            try
            {
                if (outSearchData != null)
                    outSearchData.Clear();
                if (DataFromDB != null)
                {
                    outSearchData = DataFromDB;
                }
                return Ok(outSearchData);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("{search}")]
        public IActionResult SearchMachine(string search)
        {
            try
            {
                if (outSearchData != null)
                    outSearchData.Clear();
                if (DataFromDB != null && search != null && search.Trim() != "")
                {
                    foreach (DataRow row in DataFromDB.Rows)
                    {
                        foreach (string val in row.ItemArray.Cast<string>())
                        {
                            if (val.Trim().Equals(search.Trim(), StringComparison.CurrentCultureIgnoreCase))
                            {
                                outSearchData.Rows.Add(row.ItemArray);
                            }
                        }
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

        [HttpPost]
        public async Task<ActionResult<MachineDetails>> AddMachine(MachineDetails objAsset)
        {
            try
            {
                if (objAsset.MachineName != null && objAsset.AssetName != null & objAsset.SeriesNo != null)
                {
                    await AddMachineDetails.AddMachineDetailsinDB(objAsset);
                    return StatusCode(StatusCodes.Status201Created);
                }
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Adding Machine Details");
            }
        }

        [HttpDelete("{assetname},{machinename}")]
        public async Task<ActionResult<MachineDetails>> Delete(string assetname, string machinename)
        {
            try
            {
                MachineDetails objAsset = new MachineDetails();
                objAsset.AssetName = assetname;
                objAsset.MachineName = machinename;
                if (assetname != null && machinename != null)
                {
                    await DeleteAsset.DeleteAssetDetails(objAsset);
                    return StatusCode(StatusCodes.Status204NoContent);
                }
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Deleting Machine Details");
            }
        }
    }
}
