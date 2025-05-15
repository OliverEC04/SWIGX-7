import cv2
import numpy as np
from pyzbar.pyzbar import decode

# Known real-world width of the QR code in mm
KNOWN_WIDTH = 165  # mm
# Known distance from the camera to the QR code when capturing this image
KNOWN_DISTANCE = 250  # mm

# Capture image from webcam
cap = cv2.VideoCapture(0)
ret, frame = cap.read()
cap.release()

if ret:
    # Detect QR codes in the frame
    qr_codes = decode(frame)

    if qr_codes:
        # Use the first detected QR code
        qr = qr_codes[0]
        # Get the bounding box
        points = qr.polygon

        if len(points) >= 4:
            # Convert points to (x, y)
            pts = [(point.x, point.y) for point in points]

            # Compute width in pixels between top-left and top-right points
            width_pixels = np.linalg.norm(np.array(pts[0]) - np.array(pts[1]))

            # Calculate focal length
            focal_length = (width_pixels * KNOWN_DISTANCE) / KNOWN_WIDTH
            print(f"Estimated Focal Length: {focal_length:.2f} pixels")

            # Optional: draw QR code box
            for i in range(len(pts)):
                cv2.line(frame, pts[i], pts[(i+1) % len(pts)], (0, 255, 0), 2)

            # Show the frame with QR overlay
            cv2.imshow("QR Code Detection", frame)
            cv2.waitKey(0)
            cv2.destroyAllWindows()
        else:
            print("QR code polygon has insufficient points.")
    else:
        print("No QR code detected. Make sure it's clearly visible and in frame.")
else:
    print("Failed to capture image from webcam.")
