using System;
using System.Threading.Tasks;

namespace Discord_Channel_Importer.Utilities
{
	public static class JsonFactory
	{
		/// <summary>
		/// Creates and returns a Json object from a URL's raw text
		/// </summary>
		public static async Task<object> CreateFromURLAsync(string url, Type type)
		{
			string rawText;

			// Download the text
			using (var client = new System.Net.WebClient())
			{
				rawText = await client.DownloadStringTaskAsync(new System.Uri(url));
			}

			// Turn it into a Json object
			return await Task.Run(() =>
			{
				return Newtonsoft.Json.JsonConvert.DeserializeObject(rawText, type);
			});
		}
	}
}
