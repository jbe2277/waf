namespace Waf.NewsReader.Applications.Services
{
    public interface IDataService
    {
        T Load<T>() where T : class;

        void Save(object data);
    }
}
