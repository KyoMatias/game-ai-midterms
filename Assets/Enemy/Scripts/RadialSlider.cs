using UnityEngine;
using UnityEngine.UI;

public class RadialSlider : MonoBehaviour
{
    [SerializeField]
    private GameObject _detectionUI;

    [SerializeField]
    private Image fillImage;

    [Range(0f, 1f)]
    public float value;

    [SerializeField]
    private Camera _mainCamera;

    void Awake()
    {
        _mainCamera = Camera.main;
    }

    void OnEnable()
    {
        EventManager.ON_DETECTION += SetValue;
    }

    void OnDisable()
    {
        EventManager.ON_DETECTION -= SetValue;
    }

    void OnValidate() => SetValue(value, true);

    void LateUpdate()
    {
        FaceCamera();
    }

    void FaceCamera()
    {
        if (_mainCamera == null)
        {
            _mainCamera = Camera.main;
            if (_mainCamera == null)
                return;
        }

        transform.forward = _mainCamera.transform.forward;
    }

    public void SetValue(float v, bool b)
    {
        if (!_detectionUI)
            return;

        _detectionUI.SetActive(b);
        value = Mathf.Clamp01(v);
        fillImage.fillAmount = value;

        if (value <= 0.50f)
        {
            fillImage.color = Color.white;
        }
        else if (value < 0.75f)
        {
            fillImage.color = new Color(1f, 0.5f, 0f); // orange
        }
        else
        {
            fillImage.color = Color.red;
        }
    }
}
