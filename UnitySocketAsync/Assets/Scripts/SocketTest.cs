using SocketAsync;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SocketTest : MonoBehaviour
{
    public Slider Progress;
    public Text TxtProgress;
    public Text TxtBytesFile;
    public Button BtnReadFile;
    public Button BtnSend;
    public Button BtnCancel;
    public InputField TxtFilePath;

    private float pctProgress = 0;
    private byte[] bytesToSend = null;

    public void Awake()
    {
        BtnSend.onClick.AddListener(() => sendBytes());
        BtnReadFile.onClick.AddListener(() => readFile());
    }

    private void Update()
    {
        Progress.value = pctProgress;
        TxtProgress.text = pctProgress.ToString("P1");
    }

    private void sendBytes()
    {
        updateProgress(0);

        if (bytesToSend != null)
        {
            SocketManager socket = new SocketManager(
                bytesToSend,
                () => done(),
                () => canceled(),
                (progress) => updateProgress(progress));
        }
    }

    private void done()
    {
        Debug.Log("Socket Done");
    }

    private void canceled()
    {
        Debug.Log("Socket Canceled");
    }

    private void updateProgress(float progress)
    {
        pctProgress = (float)(progress / ((bytesToSend != null) ? bytesToSend.Length : 0));
    }

    private void readFile()
    {
        bytesToSend = File.ReadAllBytes(TxtFilePath.text);
        TxtBytesFile.text = $"Bytes: {bytesToSend.Length}";
    }
}
