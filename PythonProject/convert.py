from flask import Flask, request, jsonify
from flask_cors import CORS
from transformers import AutoModelForCausalLM, AutoTokenizer
import torch

app = Flask(__name__)
CORS(app)  # CORS hatalarını önlemek için

# Modeli ve Tokenizer'ı yükle
model_path = r"C:\Users\sumey\OneDrive\Masaüstü\New folder\DialoGPT-medium"  # Dosya yolu düzeltildi
tokenizer = AutoTokenizer.from_pretrained(model_path)
model = AutoModelForCausalLM.from_pretrained(model_path)

@app.route("/chat", methods=["POST"])
def chat():
    data = request.get_json()
    user_input = data.get("text", "").strip()

    if not user_input:
        return jsonify({"error": "Input text is required"}), 400

    # Kullanıcının girişini modele ver
    input_ids = tokenizer.encode(user_input + tokenizer.eos_token, return_tensors="pt")
    output_ids = model.generate(input_ids, max_length=1000, pad_token_id=tokenizer.eos_token_id)
    response_text = tokenizer.decode(output_ids[:, input_ids.shape[-1]:][0], skip_special_tokens=True)

    return jsonify({"response": response_text})

if __name__ == "__main__":
    app.run(host="0.0.0.0", port=5000, debug=True)  # debug=True ekledim
