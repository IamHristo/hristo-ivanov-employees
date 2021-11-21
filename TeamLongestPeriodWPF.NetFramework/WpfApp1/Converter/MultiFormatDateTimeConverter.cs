// <copyright file="MultiFormatDateTimeConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WpfApp1.Converter
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    /// <summary>
    /// This class contains methods for converting date time to given format.
    /// </summary>
    public class MultiFormatDateTimeConverter : JsonConverter
    {
        private List<string> formats;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiFormatDateTimeConverter"/> class.
        /// </summary>
        /// <param name="formats">Formats for date time.</param>
        public MultiFormatDateTimeConverter(List<string> formats)
        {
            this.formats = formats;
        }

        /// <inheritdoc/>
        public override bool CanWrite
        {
            get { return false; }
        }

        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        /// <inheritdoc/>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string dateString = (string)reader.Value;

            if (dateString == null)
            {
                return DateTime.Now;
            }

            foreach (string format in this.formats)
            {
                if (DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    return date;
                }
            }

            throw new JsonException("Unable to parse \"" + dateString + "\" as a date.");
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
