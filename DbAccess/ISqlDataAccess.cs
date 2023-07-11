namespace scrapi.DbAccess
{
    public interface ISqlDataAccess
    {
        Task SaveData<T>(string storedProcedure, T parameters);
        Task<IEnumerable<T>> LoadData<T, U>(string storedProcedure, U parameters);
    }
}