using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACTPhotoIDViewer.helpers
{
    public class ConfigHelper
    {
        public static JObject LoadConfig(string FileName)
        {
            //IConfiguration configuration = new ConfigurationBuilder()
            //  .AddJsonFile("Config/reportsettings.json", true, true)
            //  .AddEnvironmentVariables()
            //  .Build();

            //JObject config = JObject.Parse(File.ReadAllText("Config/reportsettings.json"));


            // read JSON directly from a file

            JObject o2 = new JObject();

            using (StreamReader file = File.OpenText($"{FileName}"))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                o2 = (JObject)JToken.ReadFrom(reader);
            }

            return o2;
        }

        public static void SetConfig(JObject config, string FileName)
        {
            try
            {
                //string filename = "Config/reportsettings.json";
                //FileStream fs = null;
                //fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
                //using (StreamWriter file = new StreamWriter(fs))

                //using (StreamWriter file = File.CreateText("Config/reportsettings.json")) 
                //using (var writer = new JsonTextWriter(file))
                //{
                //   config.WriteTo(writer);
                //}

                var jsonString = JsonConvert.SerializeObject(config, formatting: Newtonsoft.Json.Formatting.Indented);

                File.WriteAllText(FileName, jsonString);

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
