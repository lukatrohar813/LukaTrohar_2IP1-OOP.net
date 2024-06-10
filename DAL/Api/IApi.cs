namespace DAL.Api
{
    public interface IApi
    {
        public Task<T> GetData<T>(string endpoint);
    }
}
