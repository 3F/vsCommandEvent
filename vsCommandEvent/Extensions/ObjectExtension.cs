﻿/*
 * Copyright (c) 2013-2015  Denis Kuzmin (reg) <entry.reg@gmail.com>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace net.r_eg.vsCE.Extensions
{
    public static class ObjectExtension
    {
        /// <summary>
        /// Deep copy of public properties through Reflection (slowest)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="destination"></param>
        public static void CloneByReflectionInto<T>(this T obj, T destination)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();

            foreach(PropertyInfo property in properties) {
                property.SetValue(destination, property.GetValue(obj, null), null);
            }
        }

        /// <summary>
        /// Deep copy through serialization (binary format) - Object should be Serializable.
        /// Note: also not so good.. see IL variants
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">Object for cloning</param>
        /// <returns>Cloned</returns>
        public static T CloneBySerializationBinary<T>(this T obj)
        {
            using(MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter fmt = new BinaryFormatter();
                fmt.Serialize(ms, obj);
                ms.Position = 0;
                return (T)fmt.Deserialize(ms);
            }
        }

        /// <summary>
        /// Deep copy through serialization.
        /// Alias to CloneBySerializationWithType.
        /// </summary>
        /// <typeparam name="T">Base type of object.</typeparam>
        /// <param name="obj">Object for cloning.</param>
        /// <returns>The new object.</returns>
        public static T CloneBySerialization<T>(this T obj)
        {
            return obj.CloneBySerializationWithType<T, T>();
        }

        /// <summary>
        /// Deep copy with changing type via serialization (text format).
        /// Supports a lot of objects by default than binary variant above.
        /// The T + T2 useful if needed casting of object, e.g. Parent to Child
        /// </summary>
        /// <typeparam name="T">Base type of object.</typeparam>
        /// <typeparam name="T2">Specific type for new object.</typeparam>
        /// <param name="obj">Object for cloning.</param>
        /// <returns>The new object with specific type.</returns>
        public static T2 CloneBySerializationWithType<T, T2>(this T obj)
        {
            if(obj == null) {
                return default(T2);
            }
            return JsonConvert.DeserializeObject<T2>(
                                JsonConvert.SerializeObject(
                                        obj, 
                                        Formatting.None, 
                                        new JsonSerializerSettings()
                                        {
                                            NullValueHandling   = NullValueHandling.Include,
                                            Formatting          = Formatting.None,
                                            TypeNameHandling    = TypeNameHandling.All,
                                        }
                                ),
                                new JsonSerializerSettings() {
                                    Binder = new JsonSerializationBinder(),
                                });
        }

        /// <summary>
        /// Comparing objects and array of objects.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>true value if the objects are considered equal.</returns>
        public static bool EqualsMixedObjects(this object left, object right)
        {
            if(left == null && right == null) {
                return true;
            }

            if(left == null || right == null) {
                return false;
            }

            if(Object.ReferenceEquals(left, right)) {
                return true;
            }

            if(left.GetType().IsArray && right.GetType().IsArray) {
                return Enumerable.SequenceEqual((object[])left, (object[])right);
            }

            if(!left.GetType().IsArray && !right.GetType().IsArray) {
                return Object.Equals(left, right);
            }

            return false;
        }

        /// <summary>
        /// Checks object on null value and empty string if it's string.
        /// </summary>
        /// <param name="obj">Object for checking.</param>
        /// <returns>true if null or empty string, otherwise false.</returns>
        public static bool IsNullOrEmptyString(this object obj)
        {
            return (obj == null || obj is string && (string)obj == String.Empty);
        }

        /// <summary>
        /// Convert complex object to system.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static object ToSystemObject(this object data)
        {
            if(data is Newtonsoft.Json.Linq.JArray) {
                return (object)((Newtonsoft.Json.Linq.JArray)data).ToObject(typeof(object[]));
            }
            return data;
        }

        /// <summary>
        /// Calculate MD5 hash from object.
        /// </summary>
        /// <typeparam name="T">Base type of object.</typeparam>
        /// <param name="obj">Object for calculating.</param>
        /// <returns>MD5 Hash code.</returns>
        public static string MD5Hash<T>(this T obj)
        {
            using(MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(
                                        Encoding.UTF8.GetBytes(
                                            JsonConvert.SerializeObject(
                                                obj, 
                                                Formatting.None, 
                                                new JsonSerializerSettings()
                                                {
                                                    NullValueHandling   = NullValueHandling.Include,
                                                    Formatting          = Formatting.None,
                                                    TypeNameHandling    = TypeNameHandling.All,
                                                }
                                        )));
                
                return hash.BytesToHexView();
            }
        }
    }
}