﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Console
{
    public class JsonContent : HttpContent
    {
        public object SerializationTarget { get; private set; }
        public JsonContent(object serializationTarget)
        {
            SerializationTarget = serializationTarget;
            this.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }
        protected override async Task SerializeToStreamAsync(Stream stream,
                                                    TransportContext context)
        {
            using (StreamWriter writer = new StreamWriter(stream))
            using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
            {
                JsonSerializer ser = new JsonSerializer();
                ser.Serialize(jsonWriter, SerializationTarget);
            }

        }

        protected override bool TryComputeLength(out long length)
        {
            //we don't know. can't be computed up-front
            length = -1;
            return false;
        }
    }
}
