
using System;
using System.IO;
using socket4net;

namespace Pi.Framework
{
    /// <summary>
    ///     file loader interface
    /// </summary>
    public interface IFileLoader
    {
        /// <summary>
        ///     read file from specified path to string
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string Read(string path);
    }

    /// <summary>
    ///     windows file loader
    /// </summary>
    public class WindowsFileLoader : IFileLoader
    {
        public string Root { get; private set; }

        public WindowsFileLoader(string root = "Config/", bool useDomainLocation = false)
        {
            Root = useDomainLocation ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, root) : root;
        }

        public string Read(string path)
        {
            var composedPath = Root + path;
            var final = string.Empty;

            if (!File.Exists(composedPath))
                final = composedPath + ".csv";

            if (!File.Exists(final))
                final = composedPath + ".txt";

            if (!File.Exists(final))
            {
                Logger.Ins.Warn("{0} not exist!", composedPath);
                return string.Empty;
            }

            return FileExt.Read(final);
        }
    }
}
