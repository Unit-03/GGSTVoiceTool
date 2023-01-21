namespace GGSTVoiceTool
{
    public readonly struct SemVersion
    {
		#region Constants

		public static SemVersion Current => new SemVersion(0, 2, 2);

		#endregion

		#region Fields

		public readonly byte Major;
        public readonly byte Minor;
        public readonly byte Patch;

		#endregion

		#region Constructors

		public SemVersion(string ver)
        {
            string[] parts = ver.TrimStart('v').Split('.');

            if (parts.Length < 3)
                throw new System.ArgumentException($"Invalid version string '{ver}'");

            Major = byte.Parse(parts[0]);
            Minor = byte.Parse(parts[1]);
            Patch = byte.Parse(parts[2]);
        }

        public SemVersion(byte major, byte minor, byte patch)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
        }

		#endregion

		#region Methods

		public override string ToString()
        {
            return $"v{Major}.{Minor}.{Patch}";
        }

        public override bool Equals(object obj)
        {
            return obj is SemVersion ver ? this == ver : false;
        }

        public override int GetHashCode()
        {
            return (Major << 16) &
                   (Minor << 8)  &
                   (Patch << 0);
        }

		#endregion

		#region Operators

		public static bool operator ==(SemVersion left, SemVersion right)
        {
            return left.Major == right.Major &&
                   left.Minor == right.Minor &&
                   left.Patch == right.Patch;
        }

        public static bool operator !=(SemVersion left, SemVersion right)
        {
            return left.Major != right.Major ||
                   left.Minor != right.Minor ||
                   left.Patch != right.Patch;
        }

        public static bool operator >(SemVersion left, SemVersion right)
        {
            return (left.Major > right.Major) ||
                   (left.Major == right.Major && left.Minor > right.Minor) ||
                   (left.Major == right.Major && left.Minor == right.Minor && left.Patch > right.Patch);
        }

        public static bool operator <(SemVersion left, SemVersion right)
        {
            return (left.Major < right.Major) ||
                   (left.Major == right.Major && left.Minor < right.Minor) ||
                   (left.Major == right.Major && left.Minor == right.Minor && left.Patch < right.Patch);
        }

		#endregion
	}
}
