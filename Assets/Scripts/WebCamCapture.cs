namespace KrakJam2024
{
    using UnityEngine;
    using System.Collections;
    using System.IO;

    public class WebcamCapture : MonoBehaviour
    {
        WebCamTexture webcamTexture;
        public string fileName = "capturedImage.png";

        void Start()
        {
            Debug.Log("DATA:" + Application.persistentDataPath);
            // Start the webcam
            webcamTexture = new WebCamTexture();
            webcamTexture.Play();
        }

        void Update()
        {
            // Check for a specific condition to take a photo (like a button press)
            if (Input.GetKeyDown(KeyCode.M))
            {
                StartCoroutine(TakePhoto());
            }
        }

        IEnumerator TakePhoto()
        {
            // Wait till the end of the current frame
            yield return new WaitForEndOfFrame();

            Texture2D photo = new Texture2D(webcamTexture.width, webcamTexture.height);
            photo.SetPixels(webcamTexture.GetPixels());
            photo.Apply();

            // Encode the photo to a PNG
            byte[] bytes = photo.EncodeToPNG();
            Destroy(photo);

            // Write to a file
            File.WriteAllBytes(Application.persistentDataPath + "/" + fileName, bytes);
            Debug.Log("Photo saved to " + Application.persistentDataPath + "/" + fileName);
        }
    }
}