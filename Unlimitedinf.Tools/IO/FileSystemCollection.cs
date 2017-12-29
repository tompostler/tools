using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Unlimitedinf.Tools.IO
{
    /// <summary>
    /// A way to get enumerables for the filesystem in a Directory.EnumerateFiles fashion.
    /// </summary>
    /// <remarks>
    /// a la http://stackoverflow.com/a/13130054
    /// </remarks>
    public class FileSystemCollection : IEnumerable<string>
    {
        private readonly string _root;
        private readonly IList<string> _patterns;
        private readonly SearchOption _option;

        /// <summary>
        /// Ctor for one pattern.
        /// </summary>
        /// <param name="root">The root directory to start searching from.</param>
        /// <param name="pattern">
        ///     See <see cref="Directory.EnumerateDirectories(string, string, SearchOption)"/> or
        ///     <see cref="Directory.EnumerateFiles(string, string, SearchOption)"/> for a better description of
        ///     searchPattern.
        /// </param>
        /// <param name="option">Specify to search the top directory only or all recursive directories.</param>
        public FileSystemCollection(string root, string pattern = "*", SearchOption option = SearchOption.AllDirectories)
        {
            _root = root;
            _patterns = new List<string> { pattern };
            _option = option;
        }

        /// <summary>
        /// Ctor for multiple search patterns.
        /// </summary>
        /// <param name="root">The root directory to start searching from.</param>
        /// <param name="patterns">
        ///     See <see cref="Directory.EnumerateDirectories(string, string, SearchOption)"/> or
        ///     <see cref="Directory.EnumerateFiles(string, string, SearchOption)"/> for a better description of
        ///     searchPattern.
        /// </param>
        /// <param name="option">Specify to search the top directory only or all recursive directories.</param>
        public FileSystemCollection(string root, IList<string> patterns, SearchOption option)
        {
            _root = root;
            _patterns = patterns;
            _option = option;
        }

        /// <inheritdoc />
        public IEnumerator<string> GetEnumerator()
        {
            if (_root == null || !Directory.Exists(_root)) yield break;

            IEnumerable<string> matches = new List<string>();
            try
            {
                foreach (string pattern in _patterns)
                {
                    matches = matches.Concat(Directory.EnumerateDirectories(_root, pattern, SearchOption.TopDirectoryOnly))
                                     .Concat(Directory.EnumerateFiles(_root, pattern, SearchOption.TopDirectoryOnly));
                }
            }
            catch (UnauthorizedAccessException)
            {
                yield break;
            }
            catch (PathTooLongException)
            {
                yield break;
            }

            foreach (var match in matches)
            {
                yield return match;
            }

            if (_option == SearchOption.AllDirectories)
            {
                foreach (var dir in Directory.EnumerateDirectories(_root, "*", SearchOption.TopDirectoryOnly))
                {
                    var fse = new FileSystemCollection(dir, _patterns, _option);
                    foreach (var match in fse)
                    {
                        yield return match;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
