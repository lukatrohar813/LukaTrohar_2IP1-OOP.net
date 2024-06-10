namespace DAL.Repos
{
    public static class RepositoryFactory
    {
        public static IFileRepository GetRepository()
        {
            return  FileRepository.Instance;
        }
    }
}
