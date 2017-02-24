namespace Unlimitedinf.Tools
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// To help with representing and performing operations on semantic versions <see href="http://semver.org/"/>.
    /// The implementation of the <see cref="IEquatable{T}"/> and <see cref="IComparable{T}"/> interfaces are done
    /// along C# guidelines and will check for object equality and comparability by also comparing the build version.
    /// Algebraic comparisons are performed using the semantic version specification (i.e. build version is ignored).
    /// </summary>
    /// <remarks>
    /// Portions of this implementation were referenced on 2016-11-09 from
    /// <see href="http://www.michaelfcollins3.me/blog/2013/01/23/semantic_versioning_dotnet.html"/>.
    /// Based on semver version 2.0.0
    /// </remarks>
    public class SemVer : IComparable<SemVer>, IEquatable<SemVer>
    {
        /// <summary>
        /// Major version.
        /// </summary>
        public int Major { get; private set; }
        /// <summary>
        /// Minor version.
        /// </summary>
        public int Minor { get; private set; }
        /// <summary>
        /// Patch version.
        /// </summary>
        public int Patch { get; private set; }
        /// <summary>
        /// Optional prerelease version.
        /// </summary>
        public string Prerelease { get; private set; }
        /// <summary>
        /// Optional build version.
        /// </summary>
        public string Build { get; private set; }

        // Valid prerelease and build regex
        private static readonly string PBRegexString = @"[A-Za-z0-9\-\.]+";

        // Full SemVer regex
        private static readonly Regex SemVerRegex = new Regex(
            $@"^(?<major>\d+)\.(?<minor>\d+)\.(?<patch>\d+)(-(?<prerelease>{PBRegexString}))?(\+(?<build>{PBRegexString}))?$",
            RegexOptions.Compiled | RegexOptions.Singleline);

        // Prerelease or build regex
        private static readonly Regex PBRegex = new Regex(
            $"^({PBRegexString})$",
            RegexOptions.Compiled | RegexOptions.Singleline);

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public SemVer(int major, int minor, int patch)
            : this(major, minor, patch, null, null)
        { }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public SemVer(int major, int minor, int patch, string prerelease, string build)
        {
            SetMajorMinorPatch(major, minor, patch);

            CheckValidPrereleaseOrBuildString(prerelease, nameof(prerelease));
            CheckValidPrereleaseOrBuildString(build, nameof(build));
            Prerelease = prerelease;
            Build = build;
        }

        /// <summary>
        /// Wrapper to set and validate all three.
        /// </summary>
        private void SetMajorMinorPatch(int major, int minor, int patch)
        {
            if (major < 0)
                throw new ArgumentOutOfRangeException($"{nameof(major)} cannot be <0 (is: {major})");
            if (minor < 0)
                throw new ArgumentOutOfRangeException($"{nameof(minor)} cannot be <0 (is: {minor})");
            if (patch < 0)
                throw new ArgumentOutOfRangeException($"{nameof(patch)} cannot be <0 (is: {patch})");

            Major = major;
            Minor = minor;
            Patch = patch;
        }

        /// <summary>
        /// Increase the major version number by 1 and reset the following.
        /// </summary>
        public SemVer IncrementMajor()
        {
            return new SemVer(Major + 1, 0, 0);
        }

        /// <summary>
        /// Increase the minor version number by 1 and reset the following.
        /// </summary>
        public SemVer IncrementMinor()
        {
            return new SemVer(Major, Minor + 1, 0);
        }

        /// <summary>
        /// Increase the patch version number by 1 and reset the following.
        /// </summary>
        public SemVer IncrementPatch()
        {
            return new SemVer(Major, Minor, Patch + 1);
        }

        private void CheckValidPrereleaseOrBuildString(string version, string paramName)
        {
            if (ReferenceEquals(null, version))
                return;

            if (string.IsNullOrWhiteSpace(version))
                throw new ArgumentException($"The {paramName} string cannot be null or whitespace.");

            Match match = PBRegex.Match(version);
            if (!match.Success)
                throw new ArgumentException($"The {paramName} string \"{version}\" is not valid semver.", paramName);
        }

        /// <summary>
        /// Set the prerelease stamp.
        /// </summary>
        /// <example>UpdatePrerelease("alpha")</example>
        /// <example>UpdatePrerelease("alpha.1")</example>
        /// <example>UpdatePrerelease("0.3.7")</example>
        /// <example>UpdatePrerelease("x.7.z.92")</example>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public SemVer SetPrerelease(string prerelease)
        {
            CheckValidPrereleaseOrBuildString(prerelease, nameof(prerelease));

            return new SemVer(Major, Minor, Patch, prerelease, null);
        }

        /// <summary>
        /// Set the build stamp.
        /// </summary>
        /// <example>UpdateBuild("001")</example>
        /// <example>UpdateBuild("20130313144700")</example>
        /// <example>UpdateBuild("exp.sha.5114f85")</example>
        /// <exception cref="ArgumentNullException">Throws when the parameter is null or whitespace.</exception>
        /// <exception cref="ArgumentException">Throws when the regular expression match fails.</exception>
        public SemVer SetBuild(string build)
        {
            CheckValidPrereleaseOrBuildString(build, nameof(build));

            return new SemVer(Major, Minor, Patch, Prerelease, build);
        }

        /// <summary>
        /// Parses a semantic version from a string.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FormatException"></exception>
        public static SemVer Parse(string version)
        {
            if (string.IsNullOrWhiteSpace(version))
                throw new ArgumentNullException($"{nameof(version)} cannot be null or empty.");

            Match match = SemVerRegex.Match(version);
            if (!match.Success)
                throw new ArgumentException($"The version number \"{version}\" is not a valid semantic version.", nameof(version));

            return new SemVer(
                int.Parse(match.Groups["major"].Value),
                int.Parse(match.Groups["minor"].Value),
                int.Parse(match.Groups["patch"].Value),
                match.Groups["prerelease"].Success ? match.Groups["prerelease"].Value : null,
                match.Groups["build"].Success ? match.Groups["build"].Value : null);
        }

        /// <summary>
        /// Try to parse a semantic version from a string.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FormatException"></exception>
        public static bool TryParse(string version, out SemVer result)
        {
            result = null;
            if (string.IsNullOrWhiteSpace(version))
                return false;

            Match match = SemVerRegex.Match(version);
            if (!match.Success)
                return false;

            int major, minor, patch;
            if (!int.TryParse(match.Groups["major"].Value, out major)
                || !int.TryParse(match.Groups["minor"].Value, out minor)
                || !int.TryParse(match.Groups["patch"].Value, out patch))
                return false;

            result = new SemVer(major, minor, patch,
                match.Groups["prerelease"].Success ? match.Groups["prerelease"].Value : null,
                match.Groups["build"].Success ? match.Groups["build"].Value : null);
            return true;
        }

        internal static int Compare(SemVer left, SemVer right)
        {
            if (ReferenceEquals(null, left))
                throw new ArgumentNullException(nameof(left));
            if (ReferenceEquals(null, right))
                throw new ArgumentNullException(nameof(right));

            if (ReferenceEquals(left, right))
                return 0;

            int result = left.Major.CompareTo(right.Major);
            if (result != 0)
                return result;

            result = left.Minor.CompareTo(right.Minor);
            if (result != 0)
                return result;

            result = left.Patch.CompareTo(right.Patch);
            if (result != 0)
                return result;

            result = ComparePrereleaseOrBuildVersionString(left.Prerelease, right.Prerelease);
            if (result != 0)
                return result;

            // Build metadata SHOULD be ignored when determining version precedence
            return 0;
        }

        internal static int ComparePrereleaseOrBuildVersionString(string left, string right)
        {
            bool existsLeft = !string.IsNullOrEmpty(left);
            bool existsRight = !string.IsNullOrEmpty(right);

            // When major, minor, and patch are equal, a pre-release/build version has lower precedence than a normal version
            if (existsLeft && !existsRight)
                return -1;
            if (!existsLeft && existsRight)
                return 1;
            if (!existsLeft && !existsRight)
                return 0;

            char[] dotDelimiter = new[] { '.' };
            string[] parts1 = left.Split(dotDelimiter, StringSplitOptions.RemoveEmptyEntries);
            string[] parts2 = right.Split(dotDelimiter, StringSplitOptions.RemoveEmptyEntries);

            int max = Math.Max(parts1.Length, parts2.Length);

            // Identifiers consisting of only digits are compared numerically and identifiers with letters or hyphens
            // are compared lexically in ASCII sort order.
            for (int i = 0; i < max; i++)
            {
                // A larger set of fields has a higher precedence than a smaller set,
                // if all of the preceding identifiers are equal
                if (i == parts1.Length)
                    return -1;
                if (i == parts2.Length)
                    return 1;

                string part1 = parts1[i];
                string part2 = parts2[i];
                int result = 0;
                int p1, p2;
                if (int.TryParse(part1, out p1) && int.TryParse(part2, out p2))
                    result = p1.CompareTo(p2);
                else
                    result = string.Compare(part1, part2, StringComparison.Ordinal);

                if (result != 0)
                    return result;
            }

            return 0;
        }

        //
        // Interface Implementations and Overrides
        //
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        public override string ToString()
        {
            return $"{Major}.{Minor}.{Patch}{(string.IsNullOrEmpty(Prerelease) ? "" : $"-{Prerelease}")}{(string.IsNullOrEmpty(Build) ? "" : $"+{Build}")}";
        }

        public override bool Equals(object obj)
        {
            SemVer other = obj as SemVer;
            if (other == null)
            {
                return false;
            }
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// Compare two semantic versions, including the build metadata. Build metadata precedence is determined the
        /// same way as prerelease version precedence.
        /// </summary>
        public int CompareTo(SemVer other)
        {
            if (ReferenceEquals(null, other))
                throw new ArgumentNullException(nameof(other));

            if (ReferenceEquals(this, other))
                return 0;

            int result = Compare(this, other);
            if (result != 0)
                return result;

            result = ComparePrereleaseOrBuildVersionString(Build, other.Build);
            if (result != 0)
                return result;

            // Objects are equal
            return 0;
        }

        public bool Equals(SemVer other)
        {
            if (ReferenceEquals(other, null))
                return false;

            return Major == other.Major
                && Minor == other.Minor
                && Patch == other.Patch
                && string.Equals(Prerelease, other.Prerelease)
                && string.Equals(Build, other.Build);
        }

        public static bool operator ==(SemVer left, SemVer right)
        {
            if (ReferenceEquals(left, null))
                return ReferenceEquals(right, null);
            if (ReferenceEquals(right, null))
                return false;

            return left.Major == right.Major
                && left.Minor == right.Minor
                && left.Patch == right.Patch
                && left.Prerelease == right.Prerelease;
            //// Build metadata SHOULD be ignored when determining version precedence.
            ////&& Build.Equals(other.Build);
        }

        public static bool operator !=(SemVer left, SemVer right)
        {
            return !(left == right);
        }

        public static bool operator <(SemVer left, SemVer right)
        {
            if (ReferenceEquals(null, left))
                throw new ArgumentNullException(nameof(left));
            if (ReferenceEquals(null, right))
                throw new ArgumentNullException(nameof(right));

            return Compare(left, right) < 0;
        }

        public static bool operator >=(SemVer left, SemVer right)
        {
            return !(left < right);
        }

        public static bool operator >(SemVer left, SemVer right)
        {
            if (ReferenceEquals(null, left))
                throw new ArgumentNullException(nameof(left));
            if (ReferenceEquals(null, right))
                throw new ArgumentNullException(nameof(right));

            return Compare(left, right) > 0;
        }

        public static bool operator <=(SemVer left, SemVer right)
        {
            return !(left > right);
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}