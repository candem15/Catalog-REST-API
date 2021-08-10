namespace catalog.Settings
{
    public class MongoDbSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; } //We are using authentication with mongodb now. Theese properties must added to connection string. 
        public string Password { get; set; } //Password setted with .Net secret manager in terminal. This prevents security issues like leaking passwords.
        public string ConnectionString //Calculate connectionString that's needed in order to talk to MongoDB.
        { 
            get
            {
                string v = $"mongodb://{User}:{Password}@{Host}:{Port}"; //This is syntax type that MongoDb expecting from us.
                return v;
            } 
        }
    }
}
//"dotnet user-secrets init" for .Net secret manager to terminal.
//"dotnet user-secrets set MongoDbSettings:Password eray#Admin1" add password to MongoDbSettings with secret manager.