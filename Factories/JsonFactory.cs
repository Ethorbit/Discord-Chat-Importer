using System;
using System.Threading.Tasks;

namespace Discord_Channel_Importer.Utilities
{
	public static class JsonFactory
	{
		/// <summary>
		/// Creates and returns a Json object from string
		/// </summary>
		public static async Task<string> CreateFromStringAsync(string txt)
		{
			return await Task.Run(() => 
			{ 
				return "Test"; 
			});
		}

		/// <summary>
		/// Creates and returns a Json object from a URL's raw text
		/// </summary>
		public static async Task CreateFromURL(string url)
		{
			string txt;

			// Download the text
			using (var client = new System.Net.WebClient())
			{
				txt = await client.DownloadStringTaskAsync(new System.Uri(url));
			}

			// Turn it into a Json object
			
		}
	}
}
