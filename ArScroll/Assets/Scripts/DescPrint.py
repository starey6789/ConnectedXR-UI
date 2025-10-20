from google import genai
from google.genai import types

import requests
import socket
import time
import json

HOST = '0.0.0.0'  # Standard loopback interface address (localhost)
PORT = 65432        # Port to listen on (non-privileged ports are > 1023)

prompt_list = [
    "provide a description for this and be concise, keep it around 100 words, do not include an introduction or conclusion",
    "provide just the process of drawing this piece, keep it under 2000 characters and do not include an introduction or conclusion",
    "describe the symbolism in this and be concise, keep it around 100 words, do not include an introduction or conclusion",
    "describe this art piece's history, if it doesn't have any just say 'No history regarding this piece.', be concise, keep it around 100 words, do not include an introduction or conclusion",
    "list related artworks with respective artists, if it doesn't have any just say 'No related works', do not include an introduction or conclusion or any text formatting"
]

section_list = [
    "description",
    "process",
    "symbolism",
    "history",
    "related_works"
]

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
            conn.sendall(b"Generating text")
            temp_list = []
            for i in range(len(section_list)):
                print(i)
                response = client.models.generate_content(
                    model='gemini-2.5-flash',
                    contents=[types.Part.from_bytes(data=image_bytes, mime_type='image/jpeg',),
                    prompt_list[i]
                    ]
                )
                conn.sendall(response.text.encode('utf-8'))
                # with open(r"Assets\Resources\GeneratedTexts\\" + name + "_" + section_list[i] + ".txt", "w", encoding="utf-8") as text_file:
                #     print(response.text, file=text_file)
            # json_array = json.dumps(temp_list) 
            conn.sendall(b"Generation done") 

# description prompt: provide a description for this and be concise, keep it around 100 words, do not include an introduction or conclusion
# process prompt: provide just the process of drawing this piece
# symbolism prompt: describe the symbolism in this and be concise, keep it around 100 words, do not include an introduction or conclusion
# history prompt: describe its history, if it doesn't have any just say "No history regarding this piece.", be concise, keep it around 100 words, do not include an introduction or conclusion
# related works: list related works with respective artists, do not include an introduction or conclusion


# image_path = "https://goo.gle/instrument-img"
# image_path = input("image url:")
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