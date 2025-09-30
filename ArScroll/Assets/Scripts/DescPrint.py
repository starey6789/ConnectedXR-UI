from google import genai
from google.genai import types

import requests
import socket
import time

HOST = '0.0.0.0'  # Standard loopback interface address (localhost)
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
            
            name = data.decode('utf-8')
            image_path = r"Assets\ReferencesLibrary\Images\\" + name + ".png"
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
            with open(r"Assets\Resources\\" + name + "_desc.txt", "w", encoding="utf-8") as text_file:
                print(response.text, file=text_file)
            conn.sendall(b"Wrote to file")



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