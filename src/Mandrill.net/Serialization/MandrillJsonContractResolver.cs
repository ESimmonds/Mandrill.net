﻿using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Mandrill.Serialization
{
    internal class MandrillJsonContractResolver : DefaultContractResolver
    {
        private static readonly Regex CamelCaseRegex = new Regex("^[A-Z][a-z]+(?:[A-Z][a-z]+)*$", RegexOptions.Compiled);


        public MandrillJsonContractResolver() : base(true)
        {
        }

        protected static string ConvertCamelCasePropertyNamesToLowerCaseUnderscoreStyle(string propertyName)
        {
            if (CamelCaseRegex.IsMatch(propertyName))
            {
                return Regex.Replace(
                    Regex.Replace(
                        Regex.Replace(propertyName, @"([A-Z]+)([A-Z][a-z])", "$1_$2"), @"([a-z\d])([A-Z])",
                        "$1_$2"), @"[-\s]", "_").ToLower();

            }
            return (propertyName).ToLower();
        }


        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var jsonProperty = base.CreateProperty(member, memberSerialization);

            var propertyType = jsonProperty.PropertyType;

            //don't serialize empty lists (unless required)
            if (propertyType.IsGenericType && propertyType.GenericTypeArguments.Length == 1)
            {
                if (member.GetCustomAttribute<RequiredAttribute>() == null)
                {
                    var t = propertyType.GenericTypeArguments[0];
                    if (typeof (IList<>).MakeGenericType(t).IsAssignableFrom(propertyType))
                    {
                        var prop = jsonProperty.DeclaringType.GetProperty(member.Name);
                        jsonProperty.ShouldSerialize = instance =>
                        {
                            var collection = prop.GetValue(instance) as ICollection;
                            if (collection != null)
                            {
                                return collection.Count > 0;
                            }
                            return false;
                        };
                    }
                }
            }


            //don't serialize empty dictionaries (unless required)
            if (typeof (IDictionary<string, string>).IsAssignableFrom(propertyType))
            {
                if (member.GetCustomAttribute<RequiredAttribute>() == null)
                {
                    var prop = jsonProperty.DeclaringType.GetProperty(member.Name);

                    jsonProperty.ShouldSerialize = instance =>
                    {
                        var dictionary = prop.GetValue(instance) as IDictionary<string, string>;
                        if (dictionary != null)
                        {
                            return dictionary.Count > 0;
                        }
                        return false;
                    };
                }
            }

            //leave the keys of a dictionary alone, otherwise convert to lowercase underscore
            if (!(typeof (IDictionary<string, string>).IsAssignableFrom(jsonProperty.DeclaringType)))
            {
                jsonProperty.PropertyName = ConvertCamelCasePropertyNamesToLowerCaseUnderscoreStyle(jsonProperty.PropertyName);
            }

            return jsonProperty;
        }
    }
}