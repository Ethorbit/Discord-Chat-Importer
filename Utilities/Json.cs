
using System;
using System.Threading.Tasks;

namespace Discord_Channel_Importer.Utilities
{
	internal static class Json
	{
		/// <summary>
		/// Creates and returns a Json object from a webpage's raw text
		/// </summary>
		/// <exception cref="InvalidJsonException"></exception>
		public static async Task<object> GetJsonObjectFromURIAsync(Uri uri, Type type)
		{
			string rawText = await Web.GetTextFromUriAsync(uri);

			if (IsValidJson(rawText))
				return await Task.Run(() => { return Newtonsoft.Json.JsonConvert.DeserializeObject(rawText, type); });		
			else
				throw new InvalidJsonException();
		}

		public static bool IsValidJson(string json)
		{
			return ((json.StartsWith("{") && json.EndsWith("}")) || (json.StartsWith("[") && json.EndsWith("]")));
		}
	}

	internal class InvalidJsonException : Exception 
	{
		public override string Message
		{
			get
			{
				return "The string is not valid .json";
			}
		}
	}
}
