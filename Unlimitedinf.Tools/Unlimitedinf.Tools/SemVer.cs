namespace Unlimitedinf.Tools
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// To help with representing and performing operations on semantic versions <see cref="http://semver.org/"/>.
    /// </summary>
    /// <remarks>
    /// Portions of this implementation were referenced on 2016-11-09 from
    /// <see cref="http://www.michaelfcollins3.me/blog/2013/01/23/semantic_versioning_dotnet.html"/>.
    /// Based on semver version 2.0.0
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Sem", Justification = "Proper name.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ver", Justification = "Proper name.")]
    public class SemVer : IComparable, IComparable<SemVer>, IEquatable<SemVer>
    {
        public int Major { get; private set; }
        public int Minor { get; private set; }
        public int Patch { get; private set; }
        public string Prerelease { get; private set; }
        public string Build { get; private set; }

        private static readonly string PBRegexString = @"[A-Za-z0-9\-\.]+";
        private static readonly Regex SemVerRegex = new Regex(
            $@"^(?<major>\d+)\.(?<minor>\d+)\.(?<patch>\d+)(-(?<prerelease>{PBRegexString}))?(\+(?<build>{PBRegexString}))?$",
            RegexOptions.Compiled | RegexOptions.Singleline);
        private static readonly Regex PBRegex = new Regex($"^({PBRegexString})$",
            RegexOptions.Compiled | RegexOptions.Singleline);

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="version"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public SemVer(string version)
        {
            if (string.IsNullOrWhiteSpace(version))
                throw new ArgumentNullException(nameof(version));

            Match match = SemVerRegex.Match(version);
            if (!match.Success)
                throw new ArgumentException($"The version number \"{version}\" is not valid semver.", nameof(version));

            this.SetMajorMinorPatch(
                int.Parse(match.Groups["major"].Value),
                int.Parse(match.Groups["minor"].Value),
                int.Parse(match.Groups["patch"].Value));

            this.Prerelease = match.Groups["prerelease"].Success ? match.Groups["prerelease"].Value : null;
            this.Build = match.Groups["build"].Success ? match.Groups["build"].Value : null;
        }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="major"></param>
        /// <param name="minor"></param>
        /// <param name="patch"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public SemVer(int major, int minor, int patch)
        {
            this.SetMajorMinorPatch(major, minor, patch);
        }

        /// <summary>
        /// Wrapper to set and validate all three.
        /// </summary>
        /// <param name="major"></param>
        /// <param name="minor"></param>
        /// <param name="patch"></param>
        private void SetMajorMinorPatch(int major, int minor, int patch)
        {
            if (major < 0)
                throw new ArgumentOutOfRangeException(nameof(major), $"{nameof(major)} cannot be <0 (is: {major})");
            if (minor < 0)
                throw new ArgumentOutOfRangeException(nameof(minor), $"{nameof(minor)} cannot be <0 (is: {minor})");
            if (patch < 0)
                throw new ArgumentOutOfRangeException(nameof(patch), $"{nameof(patch)} cannot be <0 (is: {patch})");

            this.Major = major;
            this.Minor = minor;
            this.Patch = patch;
        }

        /// <summary>
        /// Resets the version number based on which one is passed as true.
        /// Reset at all sets everything to default.
        /// Reset at major sets minor and all following to default.
        /// Etc.
        /// </summary>
        /// <param name="all"></param>
        /// <param name="major"></param>
        /// <param name="minor"></param>
        /// <param name="patch"></param>
        /// <param name="prerelease"></param>
        private void Reset(bool all = false, bool major = false, bool minor = false, bool patch = false, bool prerelease = false)
        {
            if (all)
            {
                this.Major = 0;
                this.Minor = 0;
                this.Patch = 0;
                this.Prerelease = null;
                this.Build = null;
            }
            else if (major)
            {
                this.Minor = 0;
                this.Patch = 0;
                this.Prerelease = null;
                this.Build = null;
            }
            else if (minor)
            {
                this.Patch = 0;
                this.Prerelease = null;
                this.Build = null;
            }
            else if (patch)
            {
                this.Prerelease = null;
                this.Build = null;
            }
            else if (prerelease)
            {
                this.Build = null;
            }
        }

        /// <summary>
        /// Increase the major version number by 1 and reset the following.
        /// </summary>
        public void IncrementMajor()
        {
            this.Major++;
            this.Reset(major: true);
        }

        /// <summary>
        /// Increase the minor version number by 1 and reset the following.
        /// </summary>
        public void IncrementMinor()
        {
            this.Minor++;
            this.Reset(minor: true);
        }

        /// <summary>
        /// Increase the patch version number by 1 and reset the following.
        /// </summary>
        public void IncrementPatch()
        {
            this.Patch++;
            this.Reset(patch: true);
        }

        /// <summary>
        /// Set the prerelease stamp.
        /// </summary>
        /// <param name="prerelease"></param>
        /// <example>"alpha"</example>
        /// <example>"alpha.1"</example>
        /// <example>"0.3.7"</example>
        /// <example>"x.7.z.92"</example>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void SetPrerelease(string prerelease)
        {
            if (string.IsNullOrWhiteSpace(prerelease))
                throw new ArgumentNullException(nameof(prerelease));

            Match match = PBRegex.Match(prerelease);
            if (!match.Success)
                throw new ArgumentException($"The prerelease string \"{prerelease}\" is not valid semver.", nameof(prerelease));

            this.Reset(prerelease: true);
            this.Prerelease = prerelease;
        }

        /// <summary>
        /// Set the build stamp.
        /// </summary>
        /// <param name="build"></param>
        /// <example>"001"</example>
        /// <example>"20130313144700"</example>
        /// <example>"exp.sha.5114f85"</example>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void SetBuild(string build)
        {
            if (string.IsNullOrWhiteSpace(build))
                throw new ArgumentNullException(nameof(build));

            Match match = PBRegex.Match(build);
            if (!match.Success)
                throw new ArgumentException($"The build string \"{build}\" is not valid semver.", nameof(build));

            this.Build = build;
        }

        #region Interface Implementations and Overrides

        public override string ToString()
        {
            return $"{this.Major}.{this.Minor}.{this.Patch}"
                + (string.IsNullOrEmpty(this.Prerelease) ? "" : $"-{this.Prerelease}")
                + (string.IsNullOrEmpty(this.Build) ? "" : $"+{this.Build}");
        }

        public static explicit operator SemVer(string version)
        {
            return new SemVer(version);
        }

        public int CompareTo(object obj)
        {
            SemVer other = obj as SemVer;
            if (other == null)
            {
                try
                {
                    other = (SemVer)(obj as string);
                }
                catch (Exception e) when (e is ArgumentException || e is ArgumentNullException || e is ArgumentOutOfRangeException)
                {
                    throw new ArgumentException($"{obj as string} is not a valid semver.", e);
                }
            }
            return this.CompareTo(other);
        }

        public override bool Equals(object obj)
        {
            SemVer other = obj as SemVer;
            if (other == null)
            {
                try
                {
                    other = (SemVer)(obj as string);
                }
                catch (Exception e) when (e is ArgumentException || e is ArgumentNullException || e is ArgumentOutOfRangeException)
                {
                    return false;
                }
            }
            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            string ver = this.ToString();
            return ver.Remove(ver.Length - this.Build.Length - 2).GetHashCode();
        }

        /// <summary>
        /// Compare two semver instances based off of the semver 2.0.0 spec.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(SemVer other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (ReferenceEquals(this, other))
                return 0;

            int result = this.Major.CompareTo(other.Major);
            if (result != 0)
                return result;

            result = this.Minor.CompareTo(other.Minor);
            if (result != 0)
                return result;

            result = this.Patch.CompareTo(other.Patch);
            if (result != 0)
                return result;

            result = SemVer.ComparePrereleaseVersions(this.Prerelease, other.Prerelease);
            if (result != 0)
                return result;

            // Build metadata SHOULD be ignored when determining version precedence
            return 0;
        }

        private static int ComparePrereleaseVersions(string left, string right)
        {
            bool existsLeft = !string.IsNullOrEmpty(left);
            bool existsRight = !string.IsNullOrEmpty(right);

            // When major, minor, and patch are equal, a pre-release version has lower precedence than a normal version
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

        /// <summary>
        /// Compare two semver objects for equality.
        /// Build metadata SHOULD be ignored when determining version precedence.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(SemVer other)
        {
            if (ReferenceEquals(other, null))
                return false;

            return this.Major == other.Major
                && this.Minor == other.Minor
                && this.Patch == other.Patch
                && this.Prerelease == other.Prerelease;
            //&& this.Build.Equals(other.Build);
        }

        public static bool operator ==(SemVer left, SemVer right)
        {
            if (ReferenceEquals(left, null))
                return ReferenceEquals(right, null);
            if (ReferenceEquals(right, null))
                return false;
            return left.Equals(right);
        }
        public static bool operator !=(SemVer left, SemVer right)
        {
            return !(left == right);
        }

        public static bool operator <(SemVer left, SemVer right)
        {
            if (ReferenceEquals(left, null))
                throw new ArgumentNullException(nameof(left));
            if (ReferenceEquals(right, null))
                throw new ArgumentNullException(nameof(right));
            return left.CompareTo(right) < 0;
        }
        public static bool operator >=(SemVer left, SemVer right)
        {
            return !(left < right);
        }

        public static bool operator >(SemVer left, SemVer right)
        {
            if (ReferenceEquals(left, null))
                throw new ArgumentNullException(nameof(left));
            if (ReferenceEquals(right, null))
                throw new ArgumentNullException(nameof(right));
            return left.CompareTo(right) > 0;
        }
        public static bool operator <=(SemVer left, SemVer right)
        {
            return !(left > right);
        }

        #endregion Interface Implementations and Overrides
    }
}
