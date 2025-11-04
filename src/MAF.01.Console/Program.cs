using Azure;
using Azure.AI.OpenAI;
using dotenv.net;
using Microsoft.Agents.AI;
using OpenAI;

DotEnv.Load();

var endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT") ?? throw new InvalidOperationException("AZURE_OPENAI_ENDPOINT is not set.");  
var apiKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY") ?? throw new InvalidOperationException("AZURE_OPENAI_API_KEY is not set.");  
var deploymentName = Environment.GetEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT_NAME") ?? "gpt-4.1-mini";

var instructions = """  
                    You are LearnBuddy, a friendly and knowledgeable study coach.                   
                    Your goal is to help the user understand complex topics clearly and progressively. 
                  """;
   
AIAgent agent = new AzureOpenAIClient(  
        new Uri(endpoint),   
        new AzureKeyCredential(apiKey))  
    .GetChatClient(deploymentName)  
    .CreateAIAgent(instructions: instructions, name: "LearnBuddy");      

await foreach (var update in agent.RunStreamingAsync("Explain what embeddings are in AI."))  
{  
    Console.Write(update);  
}