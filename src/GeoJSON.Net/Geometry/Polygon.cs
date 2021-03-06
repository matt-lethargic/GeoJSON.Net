﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Polygon.cs" company="Joerg Battermann">
//   Copyright © Joerg Battermann 2014
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net.Converters;
using Newtonsoft.Json;

namespace GeoJSON.Net.Geometry
{
    /// <summary>
    /// Defines the Polygon type.
    /// Coordinates of a Polygon are a list of linear rings coordinate arrays. The first element in 
    /// the array represents the exterior ring. Any subsequent elements represent interior rings (or holes).
    /// </summary>
    /// <remarks>
    /// See https://tools.ietf.org/html/rfc7946#section-3.1.6
    /// </remarks>
    public class Polygon : GeoJSONObject, IGeometryObject, IEqualityComparer<Polygon>, IEquatable<Polygon>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon" /> class.
        /// </summary>
        /// <param name="coordinates">
        /// The linear rings with the first element in the array representing the exterior ring. 
        /// Any subsequent elements represent interior rings (or holes).
        /// </param>
        public Polygon(List<LineString> coordinates)
        {
            if (coordinates == null)
            {
                throw new ArgumentNullException(nameof(coordinates));
            }

            if (coordinates.Any(linearRing => !linearRing.IsLinearRing()))
            {
                throw new ArgumentException("All elements must be closed LineStrings with 4 or more positions" +
                                            " (see GeoJSON spec at 'https://tools.ietf.org/html/rfc7946#section-3.1.6').", nameof(coordinates));
            }

            Coordinates = coordinates;
            Type = GeoJSONObjectType.Polygon;
        }

        /// <summary>
        /// Gets the list of points outlining this Polygon.
        /// </summary>
        [JsonProperty(PropertyName = "coordinates", Required = Required.Always)]
        [JsonConverter(typeof(PolygonConverter))]
        public List<LineString> Coordinates { get; private set; }

        #region IEqualityComparer, IEquatable

        /// <summary>
        /// Determines whether the specified object is equal to the current object
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(this, obj as Polygon);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object
        /// </summary>
        public bool Equals(Polygon other)
        {
            return Equals(this, other);
        }

        /// <summary>
        /// Determines whether the specified object instances are considered equal
        /// </summary>
        public bool Equals(Polygon left, Polygon right)
        {
            if (base.Equals(left, right))
            {
                return left.Coordinates.SequenceEqual(right.Coordinates);
            }
            return false;
        }

        /// <summary>
        /// Determines whether the specified object instances are considered equal
        /// </summary>
        public static bool operator ==(Polygon left, Polygon right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }
            if (ReferenceEquals(null, right))
            {
                return false;
            }
            return left != null && left.Equals(right);
        }

        /// <summary>
        /// Determines whether the specified object instances are not considered equal
        /// </summary>
        public static bool operator !=(Polygon left, Polygon right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Returns the hash code for this instance
        /// </summary>
        public override int GetHashCode()
        {
            int hash = base.GetHashCode();
            foreach (var item in Coordinates)
            {
                hash = (hash * 397) ^ item.GetHashCode();
            }
            return hash;
        }

        /// <summary>
        /// Returns the hash code for the specified object
        /// </summary>
        public int GetHashCode(Polygon other)
        {
            return other.GetHashCode();
        }

        #endregion
    }
}