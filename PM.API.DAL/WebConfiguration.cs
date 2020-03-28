namespace PM.API.DAL
{
    public class WebConfiguration : IWebConfiguration
    {
        public string DefaultConnection { get; set; }

        public WebConfiguration(string connectionString)
        {
            DefaultConnection = connectionString;
        }
    }
}
