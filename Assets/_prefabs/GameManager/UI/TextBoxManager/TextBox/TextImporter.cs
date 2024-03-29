﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextImporter : MonoBehaviour
{

    private TextAsset textFile;
    private Conversation conversation;
    private ConversationSnippet snippet;
    private List<ConversationSnippet> snippets;
    private string conversationId;
    private int snippetId = 0;
    private bool conversationFound = false;

    private void Start()
    {
        textFile = Resources.Load<TextAsset>("_TextBoxContent/text/TextBoxLines");
    }

    public Conversation textFileToConversation(string conversationId)
    {
        string[] textLines;
        if (textFile != null)
        {
            // instatiate entities
            snippets = new List<ConversationSnippet>();
            conversationFound = false;
            snippetId = 0;

            // convert CSV file to text lines
            textLines = conversationTextFileToStringArray(textFile);

            // convert text lines to conversation
            foreach (string sLine in textLines)
            {
                string[] csvRow = conversationSnippetTextLineToStringArry(sLine);

                // extract conversation snippets from CSV file
                if (conversationId == csvRow[0])
                {
                    if (!conversationFound)
                    {
                        conversationFound = true;
                    }

                    // prepare next snippet
                    snippet = new ConversationSnippet();
                    snippet.id = snippetId;
                    snippet.text = csvRow[2];
                    snippet.talkerName = csvRow[1];
                    snippet.vocals = Resources.Load<AudioClip>("_TextBoxContent/vocals/" + conversationId + "/" + snippet.id);
                    snippet.image = Resources.Load<Sprite>("_TextBoxContent/images/" + conversationId + "/" + snippet.id);
                    snippets.Add(snippet);
                    snippetId++;
                } else if (conversationFound)
                {
                    break;
                }
            }
            // wrap up snippets to one conversation
            return new Conversation(conversationId, snippets.ToArray());
        }
        Debug.LogWarning("File is null");
        return null;
    }

    private string[] conversationSnippetTextLineToStringArry(string sLine)
    {
        string[] naiveResult = sLine.Split(',');
        if (naiveResult.Length == 3)
            return naiveResult;

        string[] content = sLine.Split('\"');
        if (content.Length > 1)
        {
            string actualContent = content[1];

            string[] result = new string[3];
            result[0] = naiveResult[0];
            result[1] = naiveResult[1];
            result[2] = actualContent;
            return result;
        }
        return content;
    }

    private string[] conversationTextFileToStringArray(TextAsset textFile)
    {
        string[] result = textFile.text.Replace("\r\n", "\n").Split('\n');
        return result;
    }

    private static void DebugDialouge(List<ConversationSnippet> snippets)
    {
        int iSnippet = 0;
        foreach (ConversationSnippet snippet in snippets)
        {
            Debug.Log("---> Snippet " + iSnippet + ":" + snippet.text);
            Debug.Log("--->" + snippet.talkerName);
            Debug.Log("--->" + snippet.vocals);
            Debug.Log("--->" + snippet.image);
            iSnippet++;
        }
    }

    private string removePrefix(string content)
    {
        return content.Substring(3, content.Length - 3);
    }

}