namespace DAL.Api
{
    public static class ApiFactory
    {
      
        public static IApi GetApi()
        {
            return ApiService.Instance;
        }
    }
}
