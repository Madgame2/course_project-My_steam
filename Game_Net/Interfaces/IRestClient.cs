﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Net.Interfaces
{
    public interface IRestClient
    {
        Task<string> GetAsync(string endPoint, string token);
        Task<string> PostAsync(string endPoint, string token, string jsonData);

        Task<string> PostAsync(string endPoint, string token, FormUrlEncodedContent jsonData);
        Task<Stream> GetStreamAsync(string endPoint, string token);
        Task<Stream> GetStreamAsync(string endPoint, string token, Dictionary<string, string>? headers);
        Task<string> PostMultipartAsync(string endPoint, string token, MultipartFormDataContent content);
    }
}
