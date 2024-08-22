namespace Scripts.Services.Interfaces
{
    public interface IPersistentService
    {
        public T Load<T>() where T : class;
        public bool Save<T>(T data) where T : class;
    }
}