using System;
using Unity.VisualScripting;
using UnityEngine;
using ZXing;

public class StandaloneEasyReaderSample : MonoBehaviour {

    public Transform boxTransform;

    [SerializeField]
    private string lastResult;
    [SerializeField]
    private bool logAvailableWebcams;
    [SerializeField]
    private int selectedWebcamIndex;
        // GUI.TextField(new Rect(10, 10, 256, 25), lastResult);

        public int rectX = 10;
        public int rectY = 10;
        public int rectWidth = 256;
        public int rectHeight = 25;


    private WebCamTexture camTexture;
    private Color32[] cameraColorData;
    private int width, height;
    public Rect screenRect;

    // create a reader with a custom luminance source
    private IBarcodeReader barcodeReader = new BarcodeReader {
        AutoRotate = false,
        Options = new ZXing.Common.DecodingOptions {
            TryHarder = false
        }
    };

    private Result result;

    private void Start() {
        LogWebcamDevices();
        SetupWebcamTexture();
        PlayWebcamTexture();

        lastResult = "http://www.google.com";

        cameraColorData = new Color32[width * height];
        screenRect = new Rect(0, 0, Screen.width/4, Screen.height/4);
    }

    private void OnEnable() {
        PlayWebcamTexture();
    }

    private void OnDisable() {
        if (camTexture != null) {
            camTexture.Pause();
        }
    }

    float qrcodeSize =  16.5f;// 18; // 5; // 5 cm
    float FOCAL_LENGTH = 570; // 2600; // 600; // i pixels
    public float distanceFromCam = 0.0f;
    float diagonal;
    public Transform boxTrans;
    public Vector3 boxPosOffset;
    private void Update() {
        if (camTexture.isPlaying) {
            // decoding from camera image
            camTexture.GetPixels32(cameraColorData); // -> performance heavy method 
            result = barcodeReader.Decode(cameraColorData, width, height); // -> performance heavy method
            if (result != null) {
                // Debug.Log("First Point: " + result.ResultPoints[0] + "\n Third point: " + result.ResultPoints[2]);
                diagonal = Mathf.Sqrt(Mathf.Pow(result.ResultPoints[0].X - result.ResultPoints[2].X, 2) + Mathf.Pow(result.ResultPoints[0].Y - result.ResultPoints[2].Y, 2));
                distanceFromCam = (qrcodeSize * FOCAL_LENGTH) / diagonal;
                // hvis man vil have en tredje dimesion med i spil, er det her man kan putte distanceFromCam ind i stedet for 0
                boxTrans.position = new Vector3(0, -result.ResultPoints[0].Y/2,result.ResultPoints[0].X/10 ) + boxPosOffset;
            }
        }
    }

    public int borderWidth = 40;
    public int borderRadius = 40;
    public float aspect = 0;
    private void OnGUI() {
        // show camera image on screen
        GUI.DrawTexture(screenRect, camTexture, ScaleMode.ScaleToFit);
        // GUI.DrawTexture(screenRect, camTexture, ScaleMode.ScaleAndCrop, true, 0, Color.white, 50, 10);

        // show decoded text on screen
        // GUI.TextField(new Rect(10, 10, 256, 25), lastResult);
        // GUI.TextField(new Rect(rectX, rectY, rectWidth, rectHeight), lastResult);
    }

    private void OnDestroy() {
        camTexture.Stop();
    }

    private void LogWebcamDevices() {
        if (logAvailableWebcams) {
            WebCamDevice[] devices = WebCamTexture.devices;
            for (int i = 0; i < devices.Length; i++) {
                Debug.Log(devices[i].name);
            }
        }
    }

    private void SetupWebcamTexture() {
        string selectedWebcamDeviceName = WebCamTexture.devices[selectedWebcamIndex].name;
        camTexture = new WebCamTexture(selectedWebcamDeviceName);
        camTexture.requestedHeight = Screen.height;
        camTexture.requestedWidth = Screen.width;
        Debug.Log("Screen Height: " + Screen.height + " Screen Width: " + Screen.width);
    }

    private void PlayWebcamTexture() {
        if (camTexture != null) {
            camTexture.Play();
            width = camTexture.width;
            height = camTexture.height;
        }
    }
}


// INTRODUCTION
// The goal of our project was to create an XR experience by using QR codes in the real world to
// control an object rendered in unity, by scanning the QR code with a camera.

// Our initial idea was to use the camera on the IGX car to scan for QR codes in the real world, that
// would then make it possible to render objects in the real world and create an AR racing game.
// However it became clear that the QR codes were difficult to detect, and needed to be quite big in order
// for the camera to detect them. Therefore we decided to use a laptop webcam instead, and use the QR code
// to control the position of a box in unity, which gave us the idea to create a more arcade-like XR game.


// Initial position determination
// To get a feel for how difficult it was to scan for QR codes and translate it to some form of coordinates or distance,
// we prototyped a distance-to-object scanner using a python script to scan for QR codes in a video stream from the webcam.
// It would check every frame for QR codes, and mark the edges of the QR code to determine the size of the diagonal.
// By knowing the size of the QR code beforehand, we could calculate the distance from the camera to the QR code from size of the diagonal.
// The python script was able to detect the QR code, and we were able to get a distance from the camera to the QR code,
// but it was a bit shaky and unstable.
// The first testing and benchmarking was done with holding a QR code on our phones in front of the webcam to 
// test the QR code detection capabilities of the webcam and the python script. 
// We determined the QR code had to be bigger, and printed it on a piece of A4 paper, which the webcam on the laptop was
// better at picking up.

// QR Detection in Unity
// In the Unity project, we used a QR code scanner to detect the QR code and get the distance from the camera to the QR code.
// We then used this distance to control the position of a box in unity.
// In the first draft, we controlled all three dimensions of the "bird-box" (the player), where the x and y
// position was determined by the position QR codes position (up-down left-right), and the z position, or the depth,
// was determined by the distance from the camera to the QR code, so by moving the code closer to- or further from
// the laptop, the player would move the bird-box back and forth. The sensitivity had to be tweaked a bit, as the
// distance from the camera to the QR code was quite small, which meant that when player moved the QR code
// it would move the bird-box quite a lot. We fixed this by dividing the distance from the camera to the QR code
// with a constant, which made the movement of the bird-box more manageable. We also added a small offset to the
// position of the QR code, to place it in range of the pipes on the course.
// We unfortunately ended up having to scrap the idea of using the z position, as the player was able
// to maneuver the bird-box all the way out of the pipes way, and we thought that adding random generation for new 
// horizontal pipes it would increase the scope a bit too much for the project.

// One of the libraries used to scan QR codes in the Unity project was  the ZXing library,
// where we modified some of the examples that showed how to scan and decode the QR code from the webcam.
// Sicne we had done some calibrations and calculations in the python script, it was quite easy to
// get the distance from the camera and position of the QR code, and use that to set the position of 
// an in-game object's transform.

// The Game
// The game itself is a simple arcade game, where the player controls a bird-box that has to avoid pipes.
// This is done by moving the QR code up or down in front of the webcam, which moves the bird-box up or down.
// The pipes are randomly generated from a set of pre-determined spawnpoints, on a floor that is constantly moving
// towards the player. Once the floor has moved a certain distance, it is destroyed and a new one is instanciated.

// Every loop the speed of the bird increases by an acceleration factor, to increase the difficulty.
// The score is increased by a function called with InvokeRepeating, which calls it every second.
// Pipe prefabs are procedurally spawned in, and has a collider marked as a triggerbox.
// If the player enters the pipe trigger, it triggers the OnTriggerEnter event, which causes a 
// game is over. When this happens, the text "YOU DIED" is activated and showed on the screem,
// and cancels the invokation of the "IncreaseScore" function so the score no longer increases.