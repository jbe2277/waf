using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Waf.Applications.Services;

namespace System.Waf.UnitTesting.Mocks
{
    /// <summary>
    /// This mock class implements the IFileDialogService interface.
    /// </summary>
    [Export(typeof(IFileDialogService)), Export]
    public class MockFileDialogService : IFileDialogService
    {
        /// <summary>
        /// The file dialog result of the last dialog.
        /// </summary>
        public FileDialogResult? Result { get; set; }

        /// <summary>
        /// The owner window of the last dialog.
        /// </summary>
        public object? Owner { get; private set; }

        /// <summary>
        /// The file dialog type of the last dialog.
        /// </summary>
        public FileDialogType FileDialogType { get; private set; }

        /// <summary>
        /// The supported file types of the last dialog.
        /// </summary>
        public IEnumerable<FileType> FileTypes { get; private set; } = Array.Empty<FileType>();

        /// <summary>
        /// The default file type of the last dialog.
        /// </summary>
        public FileType? DefaultFileType { get; private set; }

        /// <summary>
        /// The default file name of the last dialog.
        /// </summary>
        public string? DefaultFileName { get; private set; }


        /// <summary>
        /// Shows the open file dialog box that allows a user to specify a file that should be opened.
        /// </summary>
        /// <param name="owner">The window that owns this OpenFileDialog.</param>
        /// <param name="fileTypes">The supported file types.</param>
        /// <param name="defaultFileType">Default file type.</param>
        /// <param name="defaultFileName">Default filename. The directory name is used as initial directory when it is specified.</param>
        /// <returns>A FileDialogResult object which contains the filename selected by the user.</returns>
        public FileDialogResult ShowOpenFileDialog(object? owner, IEnumerable<FileType> fileTypes, FileType? defaultFileType, string? defaultFileName)
        {
            FileDialogType = FileDialogType.OpenFileDialog;
            Owner = owner;
            FileTypes = fileTypes;
            DefaultFileType = defaultFileType;
            DefaultFileName = defaultFileName;
            return Result!;
        }

        /// <summary>
        /// Shows the save file dialog box that allows a user to specify a filename to save a file as.
        /// </summary>
        /// <param name="owner">The window that owns this SaveFileDialog.</param>
        /// <param name="fileTypes">The supported file types.</param>
        /// <param name="defaultFileType">Default file type.</param>
        /// <param name="defaultFileName">Default filename. The directory name is used as initial directory when it is specified.</param>
        /// <returns>A FileDialogResult object which contains the filename entered by the user.</returns>
        public FileDialogResult ShowSaveFileDialog(object? owner, IEnumerable<FileType> fileTypes, FileType? defaultFileType, string? defaultFileName)
        {
            FileDialogType = FileDialogType.SaveFileDialog;
            Owner = owner;
            FileTypes = fileTypes;
            DefaultFileType = defaultFileType;
            DefaultFileName = defaultFileName;
            return Result!;
        }

        /// <summary>
        /// Clears the last remembered dialog.
        /// </summary>
        public void Clear()
        {
            Owner = null;
            FileDialogType = FileDialogType.None;
            FileTypes = Array.Empty<FileType>();
            DefaultFileType = null;
            DefaultFileName = null;
        }
    }
}
