using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnvSettings : MonoBehaviour
{
    public static EnvSettings Instance { get; private set; }

    public TMP_Text width_label;
    public Slider width_slider;
    public TMP_Text height_label;
    public Slider height_slider;
    public TMP_InputField g_input;
    public TMP_Text e_label;
    public Slider e_slider;

    public RectTransform border;

    public float Width => border.sizeDelta.x;
    public float Height => border.sizeDelta.y;
    public float G => float.Parse(g_input.text);
    public float E => e_slider.value;
    public Vector2 GravityAcceleration => new(0, -G);
    public Rect Border => new(-Width / 2, -Height / 2, Width, Height);

    private void Awake()
    {
        width_slider.onValueChanged.AddListener(v =>
        {
            width_label.text = $"w: {v * 1000:f2}";
            border.sizeDelta = new Vector2(v * 1000, Height);
        });
        width_slider.value = Width / 1000;

        height_slider.onValueChanged.AddListener(v =>
        {
            height_label.text = $"h: {v * 1000:f2}";
            border.sizeDelta = new Vector2(Width, v * 1000);
        });
        height_slider.value = Height / 1000;
        
        e_slider.onValueChanged.AddListener(v =>
        {
            e_label.text = $"e: {v:f1}";
        });
        e_slider.value = 0.5f;

        Instance = this;
    }
}
