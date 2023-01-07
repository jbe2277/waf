﻿using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.ComponentModel;

namespace System.Waf.Applications
{
    /// <summary>This class encapsulates the logic for a recent file list.</summary>
    /// <remarks>
    /// This class can be used in a Settings file to store and load the recent file list as user settings. In Visual Studio you might need
    /// to enter the full class name "System.Waf.Applications.RecentFileList" in the "Select a type" dialog.
    /// </remarks>
    public sealed class RecentFileList : IXmlSerializable
    {
        private readonly ObservableCollection<RecentFile> recentFiles;
        private int maxFilesNumber = 8;
        
        /// <summary>Initializes a new instance of the <see cref="RecentFileList"/> class.</summary>
        public RecentFileList()
        {
            recentFiles = new ObservableCollection<RecentFile>();
            RecentFiles = new ReadOnlyObservableCollection<RecentFile>(recentFiles);
        }

        /// <summary>Gets the list of recent files.</summary>
        public ReadOnlyObservableCollection<RecentFile> RecentFiles { get; }

        /// <summary>Gets or sets the maximum number of recent files in the list.</summary>
        /// <remarks>If the set number is lower than the list count then the recent file list is truncated to the number.</remarks>
        /// <exception cref="ArgumentException">The value must be equal or larger than 1.</exception>
        public int MaxFilesNumber
        {
            get => maxFilesNumber;
            set
            {
                if (maxFilesNumber == value) return;
                if (value <= 0) throw new ArgumentException("The value must be equal or larger than 1.");
                maxFilesNumber = value;
                if (recentFiles.Count - maxFilesNumber >= 1) RemoveRange(maxFilesNumber, recentFiles.Count - maxFilesNumber);
            }
        }

        private int PinCount => recentFiles.Count(r => r.IsPinned);

        /// <summary>
        /// Loads the specified collection into the recent file list. Use this method when you need to initialize the RecentFileList 
        /// manually. This can be useful when you are using an own persistence implementation.
        /// </summary>
        /// <remarks>Recent file items that exist before the Load method is called are removed.</remarks>
        /// <param name="recentFiles">The recent files.</param>
        /// <exception cref="ArgumentNullException">The argument recentFiles must not be null.</exception>
        public void Load(IEnumerable<RecentFile> recentFiles)
        {
            if (recentFiles == null) throw new ArgumentNullException(nameof(recentFiles));
            Clear();
            AddRange(recentFiles.Take(maxFilesNumber));
        }

        /// <summary>Adds a file to the recent file list. If the file already exists in the list then it only changes the position in the list.</summary>
        /// <param name="fileName">The path of the recent file.</param>
        /// <exception cref="ArgumentException">The argument fileName must not be null or empty.</exception>
        public void AddFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentException("The argument fileName must not be null or empty.", nameof(fileName));
            var recentFile = recentFiles.FirstOrDefault(r => r.Path == fileName);
            if (recentFile != null)
            {
                int oldIndex = recentFiles.IndexOf(recentFile);
                int newIndex = recentFile.IsPinned ? 0 : PinCount;
                if (oldIndex != newIndex) recentFiles.Move(oldIndex, newIndex);
            }
            else
            {
                if (PinCount < maxFilesNumber)
                {
                    if (recentFiles.Count >= maxFilesNumber) RemoveAt(recentFiles.Count - 1);
                    Insert(PinCount, new RecentFile(fileName));
                }
            }
        }

        /// <summary>Removes the specified recent file.</summary>
        /// <param name="recentFile">The recent file to remove.</param>
        /// <exception cref="ArgumentNullException">The argument recentFile must not be null.</exception>
        /// <exception cref="ArgumentException">The argument recentFile was not found in the recent files list.</exception>
        public void Remove(RecentFile recentFile)
        {
            if (recentFile == null) throw new ArgumentNullException(nameof(recentFile));
            if (!recentFiles.Remove(recentFile)) throw new ArgumentException("The passed recentFile was not found in the recent files list.", nameof(recentFile));
            recentFile.PropertyChanged -= RecentFilePropertyChanged;
        }

        XmlSchema IXmlSerializable.GetSchema() => null!;

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            reader.ReadToDescendant("RecentFile");
            while (reader.MoveToContent() == XmlNodeType.Element && reader.LocalName == "RecentFile")
            {
                var recentFile = new RecentFile();
                ((IXmlSerializable)recentFile).ReadXml(reader);
                Add(recentFile);
            }
            if (!reader.IsEmptyElement) reader.ReadEndElement();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));
            foreach (RecentFile recentFile in recentFiles)
            {
                writer.WriteStartElement("RecentFile");
                ((IXmlSerializable)recentFile).WriteXml(writer);
                writer.WriteEndElement();
            }
        }

        private void Insert(int index, RecentFile recentFile)
        {
            recentFile.PropertyChanged += RecentFilePropertyChanged;
            recentFiles.Insert(index, recentFile);
        }
        
        private void Add(RecentFile recentFile)
        {
            recentFile.PropertyChanged += RecentFilePropertyChanged;
            recentFiles.Add(recentFile);
        }

        private void AddRange(IEnumerable<RecentFile> recentFilesToAdd) { foreach (RecentFile x in recentFilesToAdd) Add(x); }

        private void RemoveAt(int index)
        {
            recentFiles[index].PropertyChanged -= RecentFilePropertyChanged;
            recentFiles.RemoveAt(index);
        }

        private void RemoveRange(int index, int count) { for (int i = 0; i < count; i++) RemoveAt(index); }

        private void Clear()
        {
            foreach (var x in recentFiles) x.PropertyChanged -= RecentFilePropertyChanged;
            recentFiles.Clear();
        }

        private void RecentFilePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(RecentFile.IsPinned))
            {
                var recentFile = (RecentFile)sender!;
                int oldIndex = recentFiles.IndexOf(recentFile);
                if (recentFile.IsPinned)
                {
                    recentFiles.Move(oldIndex, 0);
                }
                else
                {
                    int newIndex = PinCount;
                    if (oldIndex != newIndex) recentFiles.Move(oldIndex, newIndex);
                }
            }
        }
    }
}
