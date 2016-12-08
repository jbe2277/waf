using System.Threading.Tasks;

namespace Jbe.NewsReader.Applications.Controllers
{
    /// <summary>
    /// Interface for an app controller which is responsible for the app lifecycle.
    /// </summary>
    public interface IAppController
    {
        /// <summary>
        /// Initializes the app controller.
        /// Do work here before the UI is shown.
        /// Consider to restore the application state here.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Run the app controller.
        /// Do show the UI now.
        /// </summary>
        void Run();

        /// <summary>
        /// Called when the app was suspended and is now resumed. During the suspended state no events are received
        /// (e.g. network status change). Therefore, the app should check the device status when it is resumed.
        /// </summary>
        void Resuming();

        /// <summary>
        /// The application transitions to Suspended state from some other state.
        /// Do save the application state.
        /// Do NOT perform long running operations within this method because it has a time limit (OS specific).
        /// </summary>
        /// <returns>A task that represents the async operation.</returns>
        Task SuspendingAsync();
    }
}
