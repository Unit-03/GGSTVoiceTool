using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Harmony
{
    public class IniFile
    {
        #region Constants

        private const int MAX_VALUE_SIZE = 255;

        #endregion

        #region Fields

        private readonly string filePath;
        private readonly string fileName;

        #endregion

        #region Constructors

        public IniFile() : this(null)
        {
        }

        public IniFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                path = Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, null);

            if (!Path.HasExtension(path))
                path = Path.ChangeExtension(path, "ini");

            filePath = path;
            fileName = Path.GetFileNameWithoutExtension(filePath);
        }

        #endregion

        #region Methods

        public string Read(string key, string group = null)
        {
            return Read(key, string.Empty, group);
        }

        public string Read(string key, string defaultValue, string group = null)
        {
            StringBuilder buffer = new(MAX_VALUE_SIZE);
            GetPrivateProfileString(group ?? fileName, key, null, buffer, MAX_VALUE_SIZE, filePath);

            string value = buffer.ToString();
            return string.IsNullOrEmpty(value) ? defaultValue : value;
        }

        public T Read<T>(string key, string group = null)
        {
            return Read<T>(key, default, group);
        }

        public T Read<T>(string key, T defaultValue, string group = null)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

            if (!converter.CanConvertFrom(typeof(string)))
                return defaultValue;

            string value = Read(key, group);
            object conversion = converter.ConvertFromString(value);

            if (conversion == null)
                return defaultValue;

            return (T)conversion;
        }

        public bool Write(string key, object value, string group = null)
        {
            return WritePrivateProfileString(group ?? fileName, key, value.ToString(), filePath);
        }

        public bool HasKey(string key, string group = null)
        {
            return !string.IsNullOrEmpty(Read(key, group));
        }

        public bool DeleteKey(string key, string group = null)
        {
            return Write(key, null, group);
        }

        public bool DeleteGroup(string group = null)
        {
            return Write(null, null, group);
        }

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool WritePrivateProfileString(string group, string key, string value, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern uint GetPrivateProfileString(string group, string key, string def, StringBuilder buffer, int size, string filePath);

        #endregion
    }
}
