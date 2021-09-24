using System;
using System.Threading.Tasks;

namespace Discord_Channel_Importer.Utilities
{
	internal static class Web
	{
		/// <summary>
		/// Gets and returns all the text from the specified webpage
		/// </summary>
		public static async Task<string> GetTextFromUriAsync(Uri uri)
		{
			using var client = new System.Net.WebClient();	
			return await client.DownloadStringTaskAsync(uri);	
		}

		/// <summary>
		/// Creates and returns a Json object from a webpage's raw text
		/// </summary>
		public static async Task<object> GetJsonFromURIAsync(Uri uri, Type type)
		{
			string rawText = await Web.GetTextFromUriAsync(uri);
			return await Task.Run(() => { return Newtonsoft.Json.JsonConvert.DeserializeObject(rawText, type); });
		}
	}
}
