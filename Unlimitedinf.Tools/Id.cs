﻿namespace Unlimitedinf.Tools
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A lighter-weight id tracker to replace the heftiness of Guid. 4 billion unique Ids, with 2 billion before
    /// statistically likely collisions built on a backing datatype of 4 bytes.
    /// </summary>
    /// <remarks>
    /// Little or big endianess does not matter. The value is always serialized to hex, and is viewed/consumed as such.
    /// Comparisons on a machine as little or big endian don't mean anything because we're not relying on the bit order
    /// while in memory as long as they're all consistent.
    /// </remarks>
    [JsonConverter(typeof(IdConverter))]
    public struct Id : IComparable<Id>, IEquatable<Id>
    {
        private uint _data;

        /// <summary>
        /// Ctor.
        /// </summary>
#pragma warning disable CS3001 // Argument type is not CLS-compliant
        public Id(uint id)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
        {
            this._data = id;
        }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public Id(byte[] id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            if (id.Length != 4)
                throw new ArgumentException("Invalid length array", nameof(id));

            if (BitConverter.IsLittleEndian)
            {
                this._data = (uint)id[3] << 24;
                this._data += (uint)id[2] << 16;
                this._data += (uint)id[1] << 8;
                this._data += (uint)id[0];
            }
            else
            {
                this._data = (uint)id[0] << 24;
                this._data += (uint)id[1] << 16;
                this._data += (uint)id[2] << 8;
                this._data += (uint)id[3];
            }
        }

        /// <summary>
        /// Deserialize ctor.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="OverflowException"></exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public Id(string id)
        {
            id = id.TrimStart('0');
            if (!uint.TryParse(id, System.Globalization.NumberStyles.HexNumber, null, out this._data)
                && !string.IsNullOrWhiteSpace(id))
                this._data = uint.Parse(id);
        }

        /// <summary>
        /// Eight lower case characters.
        /// </summary>
        public override string ToString()
        {
            return ((uint)this).ToString("X8");
        }

        /// <summary>
        /// Make it easier to work with an Id's formatting.
        /// </summary>
#pragma warning disable CS3002 // Return type is not CLS-compliant
        public static explicit operator uint(Id id)
#pragma warning restore CS3002 // Return type is not CLS-compliant
        {
            return id._data;
        }

        /// <summary>
        /// Convert a string to Id. Needed for newtonsoft.
        /// </summary>
        public static explicit operator Id(string id)
        {
            return new Id(id);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Id"/> structure. Not guaranteed to be unique.
        /// </summary>
        public static Id NewId()
        {
            return new Id(GenerateRandom.Bytes(4));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Id"/> structure. Will try up to 10 times to be unique from the
        /// given set. If not unique, throws.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException">Thrown if a unique value could not be generated in 10 attempts.</exception>
        public static Id NewId(HashSet<Id> ids)
        {
            return Id.NewId(ids, 10);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Id"/> structure. Will try up to retryCount times to be unique
        /// from the given set. If not unique, throws.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException">Thrown if a unique value could not be generated.</exception>
        public static Id NewId(HashSet<Id> ids, int retryCount)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));

            Id id = Id.NewId();
            while (ids.Contains(id) && retryCount-- > 0)
                id = Id.NewId();

            if (ids.Contains(id))
                throw new ArgumentException("Could not generate a unique id.");

            return id;
        }

        /// <summary>
        /// A read-only instance of the <see cref="Id"/> structure whose value is all zeroes.
        /// </summary>
        public static readonly Id Empty = new Id(0);

        /// <summary>
        /// See <see cref="IComparable{T}.CompareTo(T)"/>.
        /// </summary>
        public int CompareTo(Id other)
        {
            return ((uint)this).CompareTo((uint)other);
        }

        /// <summary>
        /// See <see cref="object.Equals(object)"/>.
        /// </summary>
        public override bool Equals(object obj)
        {
            Id other = (Id)obj;
            if (ReferenceEquals(null, other))
                return false;

            return this.Equals(other);
        }

        /// <summary>
        /// See <see cref="IEquatable{T}.Equals(T)"/>.
        /// </summary>
        public bool Equals(Id other)
        {
            return this._data == other._data;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static bool operator ==(Id left, Id right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(Id left, Id right)
        {
            return !left.Equals(right);
        }
        public static bool operator <(Id left, Id right)
        {
            return left.CompareTo(right) < 0;
        }
        public static bool operator >(Id left, Id right)
        {
            return left.CompareTo(right) > 0;
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// See <see cref="object.GetHashCode"/>.
        /// </summary>
        public override int GetHashCode()
        {
            return ((uint)this).GetHashCode();
        }
    }

    #region IdConverter

    /// <summary>
    /// Enable Json.NET serialization and deserialization.
    /// </summary>
    public sealed class IdConverter : JsonConverter
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Id);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "3")]
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return new Id(serializer.Deserialize<string>(reader));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "2")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToString());
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

    #endregion
}
