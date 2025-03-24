﻿using catch_up_backend.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace catch_up_backend.Services
{
    public class AIService : IAIService
    {
        private readonly string _geminiApiUrl;

        public AIService(IConfiguration configuration)
        {
            _geminiApiUrl = configuration["GeminiApi:Url"] + configuration["GeminiApi:Key"] ?? throw new ArgumentNullException("GeminiApi Key or Url not found in configuration.");
        }


        public async Task<string> GenerateAIChatResponse(string message)
        {
            if (string.IsNullOrEmpty(_geminiApiUrl))
            {
                return "AI integration is not configured ask admin for help.";
            }

            try
            {
                using (var client = new HttpClient())
                {
                    var additionalPrompt = "Generate response in the same language as request. In response use cool young people language. The request: ";
                    var requestText = additionalPrompt + message;

                    var requestBody = new
                    {
                        contents = new[]
                        {
                            new
                            {
                                parts = new[] { new { text = requestText } }
                            }
                        }
                    };
                    var json = JsonConvert.SerializeObject(requestBody);

                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(_geminiApiUrl, content);

                    var responseString = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(responseString);

                    string generatedText = result.candidates[0].content.parts[0].text.ToString();
                    return generatedText;
                }
            }
            catch (Exception ex)
            {
                return "AI generating response error please try again later.";
            }
        }
    }
}
