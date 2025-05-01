using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjInfo : MonoBehaviour
{
    public TMP_Text width_label;
    public Slider width_slider;
    public TMP_Text height_label;
    public Slider height_slider;
    public TMP_Text info;
    public CustomObject obj;

    private void Awake()
    {
        width_slider.onValueChanged.AddListener(v =>
        {
            width_label.text = $"w: {v * 1000:f2}";
            obj.size = new(v * 1000, obj.size.y);
        });
        width_slider.value = obj.size.x / 1000;

        height_slider.onValueChanged.AddListener(v =>
        {
            height_label.text = $"h: {v * 1000:f2}";
            obj.size = new(obj.size.x, v * 1000);
        });
        height_slider.value = obj.size.y / 1000;
    }

    private void LateUpdate()
    {
        info.text =
$@"pos:   ({obj.position.x:f2}, {obj.position.y:f2})
vel:    ({obj.velocity.x:f2}, {obj.velocity.y:f2})
angle: {obj.angle * Mathf.Rad2Deg:f2}
a.vel:  {obj.angularVelocity * Mathf.Rad2Deg:f2}
";
    }
}
