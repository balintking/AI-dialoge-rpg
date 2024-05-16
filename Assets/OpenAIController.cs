using System;
using System.Collections;
using System.Collections.Generic;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class OpenAIController : MonoBehaviour
{
    public TMP_Text textField;
    public TMP_InputField inputField;
    public Button SendButton;
    
    public OpenAIAPI API;

    private List<ChatMessage> messages;
    
    // Start is called before the first frame update
    void Start()
    {
        API = new OpenAIAPI(Environment.GetEnvironmentVariable("OPENAI_API_KEY", EnvironmentVariableTarget.User));
        StartConversation();
        SendButton.clicked += GetResponse;
    }

    private void StartConversation()
    {
        messages = new List<ChatMessage>()
        {
            new ChatMessage(ChatMessageRole.System, "Say Hi!")
        };
        
        inputField.text = "";
    }

    private async void GetResponse()
    {
        if (inputField.text == "")
        {
            return;
        }
        
        SendButton.SetEnabled(false);
        
        var userMessage = new ChatMessage(ChatMessageRole.User, inputField.text);
        if (userMessage.TextContent.Length > 100)
        {
            //limit message length to 100 characters
            userMessage.TextContent = userMessage.TextContent[..100];
        }
        messages.Add(userMessage);
        Debug.Log($"{userMessage.Role}: {userMessage.TextContent}");
        
        textField.text = $"You: {userMessage.TextContent}";
        inputField.text = "";
        
        var chatResult = await API.Chat.CreateChatCompletionAsync(new ChatRequest()
        {
            Model = Model.ChatGPTTurbo,
            Temperature = 0.1,
            MaxTokens = 50,
            Messages = messages
        });
        
        var response = chatResult.Choices[0].Message;
        var responseMessage = new ChatMessage(response.Role, response.TextContent);
        messages.Add(responseMessage);
        Debug.Log($"{responseMessage.Role}: {responseMessage.TextContent}");
        
        textField.text = $"You: {userMessage.TextContent}\n\nGuard: {responseMessage.TextContent}";
        
        SendButton.SetEnabled(true);
    }

    // Update is called once per frame
    void Update() {}
}
