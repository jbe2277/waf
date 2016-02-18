using System.Collections.Generic;

namespace System.Waf.Applications.Services
{
    /// <summary>
    /// Represents a file type.
    /// </summary>
    public class FileType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileType"/> class.
        /// </summary>
        /// <param name="description">The description of the file type.</param>
        /// <param name="fileExtension">The file extension. This string has to start with a '.' point. Use the string ".*" to allow all file extensions.</param>
        /// <exception cref="ArgumentException">description is null or an empty string.</exception>
        /// <exception cref="ArgumentException">fileExtension is null, an empty string or doesn't start with a '.' point character.</exception>
        public FileType(string description, string fileExtension)
        {
            if (string.IsNullOrEmpty(description)) { throw new ArgumentException("The argument description must not be null or empty.", nameof(description)); }
            if (string.IsNullOrEmpty(fileExtension)) { throw new ArgumentException("The argument fileExtension must not be null or empty.", nameof(fileExtension)); }
            if (fileExtension[0] != '.') { throw new ArgumentException("The argument fileExtension must start with the '.' character.", nameof(fileExtension)); }

            Description = description;
            FileExtension = fileExtension;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileType"/> class.
        /// </summary>
        /// <param name="description">The description of the file type.</param>
        /// <param name="fileExtensions">A list of file extensions. Every string has to start with a '.' point.</param>
        /// <exception cref="ArgumentException">description is null or an empty string.</exception>
        /// <exception cref="ArgumentException">One or more of the file extension strings doesn't start with a '.' point character.</exception>
        /// <exception cref="ArgumentNullException">fileExtensions is null.</exception>
        public FileType(string description, IEnumerable<string> fileExtensions)
            : this(description, string.Join(";*", CheckFileExtensions(fileExtensions)))
        {
        }


        /// <summary>
        /// Gets the description of the file type.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the file extension. This string starts with a '.' point. Multiple file extensions are concatenated with the string ";*" as separator.
        /// </summary>
        public string FileExtension { get; }


        private static IEnumerable<string> CheckFileExtensions(IEnumerable<string> fileExtensions)
        {
            if (fileExtensions == null) { throw new ArgumentNullException(nameof(fileExtensions)); }
            foreach (string fileExtension in fileExtensions)
            {
                if (string.IsNullOrEmpty(fileExtension) || fileExtension[0] != '.') 
                { 
                    throw new ArgumentException("All items of the argument fileExtensions must start with the '.' character.", nameof(fileExtensions)); 
                }
            }
            return fileExtensions;
        }
    }
}
