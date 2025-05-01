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
        screenRect = new Rect(0, 0, Screen.width, Screen.height);
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
                // hvis man vil have en tredje dimesion med i spil, kan man putte distanceFromCam ind i stedet for 0
                boxTrans.position = new Vector3(0, -result.ResultPoints[0].Y/2,result.ResultPoints[0].X/10 ) + boxPosOffset;
                // Debug.Log("Distance from camera: " + distanceFromCam + " cm");
                // lastResult = result.Text + " " + result.BarcodeFormat;
                // print(lastResult);
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
    }

    private void PlayWebcamTexture() {
        if (camTexture != null) {
            camTexture.Play();
            width = camTexture.width;
            height = camTexture.height;
        }
    }
}