from google import genai
from google.genai import types

import requests
import socket

HOST = '127.0.0.1'  # Standard loopback interface address (localhost)
PORT = 65432        # Port to listen on (non-privileged ports are > 1023)

with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    s.bind((HOST, PORT))
    s.listen()
    conn, addr = s.accept()
    with conn:
        print(f"Connected by {addr}")
        while True:
            data = conn.recv(1024)
            if not data:
                break
            print(f"Received from Unity: {data.decode('utf-8')}")
            conn.sendall(b"Hello from Python!")
            image_path = r"C:\Users\sammi\vscode-workspace\ar project\ConnectedXR-UI\ArScroll\Assets\ReferencesLibrary\Images" + data.decode('utf-8')
            with open(image_path, 'rb') as f: image_bytes = f.read()
            client = genai.Client()
            response = client.models.generate_content(
                model='gemini-2.5-flash',
                contents=[
                types.Part.from_bytes(
                    data=image_bytes,
                    mime_type='image/jpeg',
                ),
                'Caption this image.'
                ]
            )
            conn.sendall(response.text.encode('utf-8'))




# image_path = "https://goo.gle/instrument-img"
# # image_path = input("image url:")
# image_bytes = requests.get(image_path).content
# image = types.Part.from_bytes(
#   data=image_bytes, mime_type="image/jpeg"
# )

# client = genai.Client()

# response = client.models.generate_content(
#     model="gemini-2.5-flash",
#     contents=["What is this image?", image],
# )

# print(response.text)