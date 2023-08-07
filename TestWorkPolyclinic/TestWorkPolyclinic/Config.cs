using System.Text.Json;

namespace TestWorkPolyclinic
{
    public class Config
    {
        public static Config config;
        public ConfigDB DB { set; get; }
        static Config()
        {
            try
            {
                using StreamReader sr = new StreamReader("config.json");
                config = JsonSerializer.Deserialize<Config>(sr.ReadToEnd());
            }
            catch
            {
                using StreamWriter sw = new StreamWriter("config.json", false, System.Text.Encoding.Default);
                config = new Config()
                {
                    DB = new ConfigDB()
                    {
                        Host = "localhost",
                        Port = 5432,
                        Database = "dbTestWorkPolyclinic",
                        Username = "postgres",
                        Password = "password"
                    }
                };
                sw.WriteLine(JsonSerializer.Serialize(config, new JsonSerializerOptions() { WriteIndented = true }));
            }
        }
    }

    public class ConfigDB
    {
        public string Host { set; get; }
        public int Port { set; get; }
        public string Database { set; get; }
        public string Username { set; get; }
        public string Password { set; get; }
    }
}
