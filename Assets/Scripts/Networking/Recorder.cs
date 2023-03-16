using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;

public class Recorder : MonoBehaviour
{
    RecorderController recorderController; // control interface for recording video
    RecorderControllerSettings controllerSettings;
    MovieRecorderSettings videoRecorder;
    public string directory = "videos";

    public void Init()
    {
        // Make setup for recording video
        controllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
        recorderController = new RecorderController(controllerSettings);
        videoRecorder = ScriptableObject.CreateInstance<MovieRecorderSettings>();
        videoRecorder.name = "Video recorder";
        videoRecorder.Enabled = true;
        controllerSettings.AddRecorderSettings(videoRecorder);
        // controllerSettings.SetRecordModeToManual(); // will stop when closing
        RecorderOptions.VerboseMode = false;
        videoRecorder.ImageInputSettings = new GameViewInputSettings()
        {
            OutputWidth = 3840,
            OutputHeight = 2160
        };
        videoRecorder.AudioInputSettings.PreserveAudio = true;
        controllerSettings.AddRecorderSettings(videoRecorder);
        controllerSettings.FrameRate = 60;

    }

    public void StartRecording(string videoName)
    {
        videoRecorder.OutputFile = Application.dataPath + "/" + directory + "/" + videoName;
        Debug.Log(videoRecorder.OutputFile);
        recorderController.PrepareRecording();
        recorderController.StartRecording();
    }

    public void StopRecording()
    {
        recorderController.StopRecording();
    }
}
