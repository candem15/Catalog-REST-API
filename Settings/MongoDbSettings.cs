namespace catalog.Settings
{
    public class MongoDbSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string ConnectionString //Calculate connectionString that's needed in order to talk to MongoDB.
        { 
            get
            {
                string v = $"mongodb://{Host}:{Port}";
                return v;
            } 
        }
    }
}