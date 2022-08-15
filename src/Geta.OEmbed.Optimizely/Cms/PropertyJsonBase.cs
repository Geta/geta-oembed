// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using EPiServer.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Geta.OEmbed.Optimizely.Cms
{
    public abstract class PropertyJsonBase<T> : PropertyLongString where T : class
    {
        private JsonSerializerSettings? _jsonOptions;
        private T? _deserializedValue;

        public virtual T? DeserializedValue => Value as T;

        public override object? Value
        {
            get
            {
                if (string.IsNullOrEmpty(LongString))
                {
                    _deserializedValue = null;
                }
                else if (_deserializedValue is null)
                {
                    Deserialize();
                }

                return _deserializedValue;
            }
            set
            {
                switch (value)
                {
                    case T typedValue:
                        Serialize(typedValue);
                        break;
                    case string longString:
                        LongString = longString;
                        break;
                    case null:
                        LongString = null;
                        break;
                    default:
                        throw new Exception($"Value must be of type {typeof(T).Name} or a JSON string that can be deserialized to {typeof(T).Name}");
                }
            }
        }

        public override PropertyDataType Type => PropertyDataType.LongString;

        public override Type PropertyValueType => typeof(T);

        public override object SaveData(PropertyDataCollection properties)
        {
            return LongString;
        }

        protected override void SetDefaultValue()
        {
            _deserializedValue = default;
        }

        protected virtual JsonSerializerSettings GetJsonOptions()
        {
            _jsonOptions ??= new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                TypeNameHandling = TypeNameHandling.Auto,
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                NullValueHandling = NullValueHandling.Ignore,
            };

            return _jsonOptions; 
        }

        protected virtual void Serialize(T value)
        {
            try
            {
                LongString = JsonConvert.SerializeObject(value, GetJsonOptions());
            }
            catch (Exception)
            {
                LongString = null;
            }
        }

        protected virtual void Deserialize()
        {
            try
            {
                _deserializedValue = JsonConvert.DeserializeObject<T>(LongString, GetJsonOptions());
            }
            catch (Exception)
            {
                _deserializedValue = null;
            }
        }
    }
}