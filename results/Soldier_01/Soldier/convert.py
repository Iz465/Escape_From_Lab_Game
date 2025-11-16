import torch

path = "C:\\Users\\Dean Winchester\\Desktop\\Escape_From_Lab_Game\\results\\Soldier_01\\Soldier\\Soldier-90380.pt"
checkpoint = torch.load(path, map_location="cpu")

print("Keys in checkpoint:", checkpoint.keys())

policy = checkpoint["Policy"]

# If it's a dict-like object, try to access its state_dict
if isinstance(policy, dict) and "state_dict" in policy:
    state_dict = policy["state_dict"]
else:
    state_dict = policy.state_dict() if hasattr(policy, "state_dict") else policy

# Create a dummy neural network structure similar to the one ML-Agents uses
import torch.nn as nn

class SoldierPolicy(nn.Module):
    def __init__(self):
        super().__init__()
        self.net = nn.Sequential(
            nn.Linear(10, 128),
            nn.ReLU(),
            nn.Linear(128, 128),
            nn.ReLU(),
            nn.Linear(128, 2),
            nn.Tanh()
        )

    def forward(self, x):
        return self.net(x)

model = SoldierPolicy()
model.load_state_dict(state_dict, strict=False)

# Test forward pass
dummy_input = torch.randn(1, 10)
output = model(dummy_input)
print("Output shape:", output.shape)

# Export ONNX
torch.onnx.export(
    model,
    dummy_input,
    "Soldier.onnx",
    export_params=True,
    opset_version=13,
    do_constant_folding=True,
    input_names=["obs"],
    output_names=["actions"]
)

print("✅ Exported ONNX model successfully!")
