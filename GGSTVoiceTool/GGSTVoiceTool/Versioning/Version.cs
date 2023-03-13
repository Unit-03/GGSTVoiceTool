using System;
using System.Runtime.InteropServices;

namespace GGSTVoiceTool
{
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public readonly struct Version : IEquatable<Version>, IComparable<Version>, ICloneable
    {
        #region Fields

		[FieldOffset(0)] public readonly byte Major;
        [FieldOffset(1)] public readonly byte Minor;
        [FieldOffset(2)] public readonly byte Patch;

        [FieldOffset(0)]
        private readonly int version;

		#endregion

		#region Constructors

		public Version(string ver)
        {
            string[] parts = ver.Trim().TrimStart('v').Split('.');

            if (parts.Length < 3)
                throw new ArgumentException($"Invalid version string '{ver}'");

            version = 0;

            Major = byte.Parse(parts[0]);
            Minor = byte.Parse(parts[1]);
            Patch = byte.Parse(parts[2]);
        }

        public Version(byte major, byte minor, byte patch)
        {
            version = 0;

            Major = major;
            Minor = minor;
            Patch = patch;
        }

        public Version(int ver)
        {
            Major = 0;
            Minor = 0;
            Patch = 0;

            version = ver & 0xFFFFFF;
        }

		#endregion

		#region Methods

		public override string ToString()
        {
            return $"{Major}.{Minor}.{Patch}";
        }

        public override bool Equals(object obj)
        {
            return obj is Version ver && version == ver.version;
        }

        public bool Equals(Version ver)
        {
            return version == ver.version;
        }

        public override int GetHashCode()
        {
            return version;
        }

        public int CompareTo(Version ver)
        {
            return version.CompareTo(ver.version);
        }

        public object Clone()
        {
            return new Version(version);
        }

        #endregion

        #region Operators

        public static bool operator ==(Version left, Version right) => left.version == right.version;
        public static bool operator !=(Version left, Version right) => left.version != right.version;
        public static bool operator  >(Version left, Version right) => left.version  > right.version;
        public static bool operator  <(Version left, Version right) => left.version  < right.version;
        public static bool operator >=(Version left, Version right) => left.version >= right.version;
        public static bool operator <=(Version left, Version right) => left.version <= right.version;

        #endregion
    }
}
