using catch_up_backend.Dtos;
using catch_up_backend.Interfaces;
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


        public async Task<string> GenerateAIChatResponse(AIChatDto aiChatDto)
        {
            if (string.IsNullOrEmpty(_geminiApiUrl))
            {
                return "AI integration is not configured ask Admin for help.";
            }

            try
            {
                using (var client = new HttpClient())
                {
                    var requestText = $"Generate response in the same language as request. " +
                        $"Additional prompt preferences: {aiChatDto.AdditionalPromptPreferences}. " +
                        $"The request to process: '{aiChatDto.Message}'";

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
