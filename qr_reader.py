import cv2
from pyzbar.pyzbar import decode

# Open webcam
cap = cv2.VideoCapture(0)

while True:
    ret, frame = cap.read()
    if not ret:
        break

    print("----NEW FRAME----")
    qr_codes = decode(frame)
    for qr in qr_codes:
        qr_data = qr.data.decode('utf-8')
        # print(f"QR Code Data: {qr_data}")

        try:
            # print(f"\nqr.polygon: {qr.polygon}\n")
            pts = qr.polygon
            if len(pts) == 4:
                pts = [(p.x, p.y) for p in pts]
                # print(f"\npts: {pts}\n")
                colours = [(0, 0, 255), (0, 255, 0), (255, 0, 0), (255, 0, 255)]
                for pt in pts:
                    cv2.circle(frame, pt, radius=6, color=colours[pts.index(pt)], thickness=-1)
                # ----------------- sidelængde -----------------
                # cv2.line(frame, pts[0], pts[1], color=(255, 255, 0), thickness=2)
                # p2p = (pts[0][0] - pts[0][1]) # ustabil, da det skifter hvad der er punkt 1 og 3
                # print(f"afstand mellem punkt 1 og 3: {p2p}")
                # ------------------------------------------------

                # ----------------- diagonal -----------------
                cv2.line(frame, pts[0], pts[2], color=(255, 0, 0), thickness=2)
                diagonal = ((pts[0][0] - pts[1][0])**2 + (pts[0][1] - pts[1][1])**2)**0.5

                print(f"længde af diagonalen for kode {qr_codes.index(qr)}: {diagonal}")
                # ------------------------------------------------
                # cv2.polylines(frame, [pts], isClosed=True, color=(0, 255, 0), thickness=2)
            else:
                print("der var ikke 4 punkter")
        except:
            print("ku ikk tegne noget")
        # Put the decoded text on the screen
        x, y, w, h = qr.rect
        # cv2.putText(frame, qr_data, (x, y - 10), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 255, 0), 2)
        cv2.putText(frame, f"{diagonal:.2f}", (x, y - 10), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 0, 0), 1)
        # Vi skal kende bredden af QR koden i "real-world units" (centimeter)
        KNOWN_WIDTH = 5.0  # bredden af QR koden (er rimelig tæt på 5 cm)
        FOCAL_LENGTH = 400 # 200 # 500 # 600 # 700  #  focal length af kameraet

        # Calculate the diagonal from the camera to the QR code
        distance_from_cam = (KNOWN_WIDTH * FOCAL_LENGTH) / diagonal
        cv2.putText(frame, f"Dist: {distance_from_cam:.2f} cm", (x, y - 30), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 255, 0), 2)
    print("----END OF FRAME----")
    

    # Show the video feed
    cv2.imshow('QR Code Scanner', frame)

    # Press 'q' to exit
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

# Release resources
cap.release()
cv2.destroyAllWindows()
