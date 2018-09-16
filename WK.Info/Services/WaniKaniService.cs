﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WK.Info.Services
{
	public interface IWaniKaniService
	{
		Task<WaniKaniKanjiModel> GetKanjisAsync();

		Task<WaniKaniVocabModel> GetVocabsAsync();
	}

	public class WaniKaniService : IWaniKaniService
	{
		private const string ApiKeyEnvironmentVariable = "WK_INFO_API_KEY";

		private readonly string ApiUrl;
		private readonly string ApiVocabularyUrl;
		private readonly string ApiKanjiUrl;

		private readonly JsonSerializerSettings _jsonSerializerSettings;
		private readonly HttpClient _client;

		public WaniKaniService()
		{
			var apiKey = Environment.GetEnvironmentVariable(ApiKeyEnvironmentVariable);
			if (string.IsNullOrWhiteSpace(apiKey))
			{
				throw new ErrorException($"Api key not found. Please set the '{ApiKeyEnvironmentVariable}' environment variable to match your WK api key.");
			}

			ApiUrl = $"https://www.wanikani.com/api/user/{apiKey}";
			ApiVocabularyUrl = $"{ApiUrl}/vocabulary";
			ApiKanjiUrl = $"{ApiUrl}/kanji";

			var contractResolver = new DefaultContractResolver
			{
				NamingStrategy = new SnakeCaseNamingStrategy()
			};
			_jsonSerializerSettings = new JsonSerializerSettings { ContractResolver = contractResolver };

			_client = new HttpClient();
		}

		public async Task<WaniKaniKanjiModel> GetKanjisAsync()
		{
			var response = await _client.GetAsync(ApiVocabularyUrl);
			TryThrowRequestFailed(response);

			var responseText = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<WaniKaniKanjiModel>(responseText, _jsonSerializerSettings);
		}

		public async Task<WaniKaniVocabModel> GetVocabsAsync()
		{
			var response = await _client.GetAsync(ApiVocabularyUrl);
			TryThrowRequestFailed(response);

			var responseText = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<WaniKaniVocabModel>(responseText, _jsonSerializerSettings);
		}

		private void TryThrowRequestFailed(HttpResponseMessage response)
		{
			if (response.IsSuccessStatusCode)
			{
				return;
			}

			throw new ErrorException("Request failed.");
		}
	}
}
