namespace Waf.BookLibrary.Library.Applications.Services
{
    public interface IPresentationService
    {
        double VirtualScreenWidth { get; }

        double VirtualScreenHeight { get; }

        
        void InitializeCultures();
    }
}
