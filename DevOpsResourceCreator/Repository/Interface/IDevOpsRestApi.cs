namespace DevOpsResourceCreator.Repository
{
    public interface IDevOpsRestApi
    {
        public string GetAll(string url, string token);
        public string Get(string url, string token);
        public string Create(string url, string content, string token);
        public string Update(string url, string content, string token);
        public string Delete(string url, string token);
    }
}