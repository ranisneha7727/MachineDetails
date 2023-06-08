using MongoDB.Bson;
using MongoDB.Driver;
using System.Data;

namespace MachineAPI.Models
{
    public class GetMachineDetailsFromSource
    {
        private static DataTable? inputdt;
        private static string connectionString = "mongodb://localhost:27017";
        private static string dbName = "asset";
        private static string collectionName = "MachineDetails";
        
        public static DataTable? LoadDataFromDB()
        {
            try
            {
                MongoClient client = new MongoClient(connectionString);
                MongoServer server = client.GetServer();
                MongoDatabase mongoDB = server.GetDatabase(dbName);
                var collectionData = mongoDB.GetCollection(collectionName);

                ConvertToDatatable(collectionData);
                return inputdt;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to Create Client.......");
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static void ConvertToDatatable(MongoCollection collectionData)
        {
            try
            {
                MongoCursor<BsonDocument> input = collectionData.FindAllAs<BsonDocument>();
                inputdt = new DataTable();
                inputdt.Columns.Add("MachineName");
                inputdt.Columns.Add("AssetName");
                inputdt.Columns.Add("SeriesNo");

                foreach (var item in input)
                {
                    inputdt.Rows.Add(item["machineName"], item["assetName"], item["seriesNo"]);
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Conversion failed....");
                Console.WriteLine(ex.Message);
            }
        }

        public static DataTable? LoadDataFromFile()
        {
            try
            {
                inputdt = new DataTable();
                var builder = new ConfigurationBuilder()
                               .SetBasePath(Directory.GetCurrentDirectory())
                               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                string inputTextFile = builder.Build().GetSection("Path").GetSection("InputFilePath").Value;

                if (File.Exists(inputTextFile))
                {
                    string[]? inputFileReader = File.ReadAllLines(inputTextFile);
                    if (inputFileReader != null)
                    {
                        inputdt.Columns.Add("MachineName");
                        inputdt.Columns.Add("AssetName");
                        inputdt.Columns.Add("SeriesNo");
                        foreach (string dtrow in inputFileReader)
                        {
                            inputdt.Rows.Add(dtrow.Split(','));
                        }
                        return inputdt;
                    }
                    else
                        return null;
                }
                else
                    throw new FileNotFoundException();
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("File not found.......");
                throw ex.InnerException;
            }
        }
    }
}
