using System.Collections.Generic;
using System.Linq;

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
        /// <param name="fileExtension">The file extension. Use the string ".*" to allow all file extensions.</param>
        /// <exception cref="ArgumentException">description is null or an empty string.</exception>
        /// <exception cref="ArgumentException">fileExtension is null, an empty string.</exception>
        public FileType(string description, string fileExtension) : this(description, new[] { fileExtension })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileType"/> class.
        /// </summary>
        /// <param name="description">The description of the file type.</param>
        /// <param name="fileExtensions">A list of file extensions.</param>
        /// <exception cref="ArgumentException">description is null or an empty string.</exception>
        /// <exception cref="ArgumentException">One or more of the file extension strings is null or empty.</exception>
        /// <exception cref="ArgumentNullException">fileExtensions is null.</exception>
        public FileType(string description, IEnumerable<string> fileExtensions)
        {
            if (string.IsNullOrEmpty(description)) throw new ArgumentException("The argument description must not be null or empty.", nameof(description));
            if (fileExtensions == null) throw new ArgumentNullException(nameof(fileExtensions));
            if (!fileExtensions.Any() || fileExtensions.Any(x => string.IsNullOrEmpty(x)))
            {
                throw new ArgumentException("The argument fileExtensions must have at least one item and all items must not be null or empty.", nameof(fileExtensions));
            }
            Description = description;
            FileExtensions = fileExtensions;
        }


        /// <summary>
        /// Gets the description of the file type.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the list of file extensions.
        /// </summary>
        public IEnumerable<string> FileExtensions { get; }
    }
}
