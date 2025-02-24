using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAIGPT;
using UnityEngine.UI;
using VoicevoxBridge;
using TMPro;

public class SendChat : MonoBehaviour
{
    // OpenAI APIキー
    [SerializeField]
    private string openAIApiKey;
    // ユーザーの入力を受け取るためのInputField
    [SerializeField]
    private InputField inputField;
    // APIの返答を受け取るためのInputField
    [SerializeField]
    private InputField outputField;
    // Text2Speech用Voicevox
    [SerializeField]
    private VOICEVOX voicevox;
    // 送信ボタンが押されたときに呼び出されるメソッド
    public void OnClick()
    {
        // InputFieldからテキストを取得
        var text = inputField.GetComponent<InputField>().text;
        // メッセージを送信
        sendmessage(text);
        // InputFieldをクリア
        inputField.GetComponent<InputField>().text = "";
    }

    // メッセージを送信し、応答を取得する非同期メソッド
    public async void sendmessage(string text)
    {
        // OpenAI GPTとの接続を初期化
        var chatGPTConnection = new ChatGPTConnection(openAIApiKey);
        // OpenAI GPTにリクエストを送信し、応答を待つ
        var response = await chatGPTConnection.RequestAsync(text);
        // 応答があれば処理を行う
        if (response.choices != null && response.choices.Length > 0)
        {
            var choice = response.choices[0];
            Debug.Log("ChatGPT Response: " + choice.message.content);
            // 返答内容をテキストフィールドに出力
            outputField.text = choice.message.content;
            PlayVoice(outputField.text);
        }
    }

    private async void PlayVoice(string text) {
        int speaker = 1; //ずんだもん
        string voice_text = text;

        await voicevox.PlayOneShot(speaker, text);
    }
}