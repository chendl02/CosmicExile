using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class NonLinearSlider : MonoBehaviour
{
    public Slider slider; // ��ק�󶨵Ļ�����
    public TMP_InputField inputField; // ��ʾ��������ֵ���ı�����ѡ��

    private int minValue = 10; // ����������Сֵ
    private int maxValue = 1000; // �����������ֵ
    public static int previousValidValue = Predict.initPredictDays;

    private Predict predict; // ���ڴ洢�������ϵ� Predict ʵ��
    void Awake()
    {
        previousValidValue = Predict.initPredictDays;
    }

    void Start()
    {

        predict = GetComponentInParent<Predict>();
        slider.minValue = 0; // ���û��������Է�Χ
        slider.maxValue = 1;
        float logMin = Mathf.Log10(minValue);
        float logMax = Mathf.Log10(maxValue);
        slider.value = Mathf.InverseLerp(logMin, logMax, Mathf.Log10(previousValidValue));
        slider.onValueChanged.AddListener(OnSliderValueChanged);


        inputField.text = previousValidValue.ToString();
        inputField.characterValidation = TMP_InputField.CharacterValidation.Integer;
        inputField.onValueChanged.AddListener(ValidateInput);
        inputField.onEndEdit.AddListener(EnsureValidRange);


        
    }

    // ������ʱ��֤����̬���£�
    private void ValidateInput(string input)
    {
        if (!int.TryParse(input, out int value))
            return;

        // ������ֵ��Χ�������Ҫ��̬���ƣ�
        if (value > maxValue)
            inputField.text = maxValue.ToString();
    }

    // ���������ʱ��֤������ȷ�ϣ�
    private void EnsureValidRange(string input)
    {
        if (int.TryParse(input, out int value))
        {
            // ������ֵ�� 10-1000 ��Χ��
            if (value < minValue)
                inputField.text = minValue.ToString();
            else if (value > maxValue)
                inputField.text = maxValue.ToString();
            if (previousValidValue.ToString() != inputField.text)
            {
                previousValidValue = int.Parse(inputField.text);
                // �� previousValidValue �Ķ���ֵӳ��� slider.value
                float logMin = Mathf.Log10(minValue);
                float logMax = Mathf.Log10(maxValue);
                slider.value = Mathf.InverseLerp(logMin, logMax, Mathf.Log10(previousValidValue));
                predict.setRenderer(previousValidValue);
            }
        }
        else
        {
            // ���������Ч�� ��ԭ
            inputField.text = previousValidValue.ToString();
        }
    }

    void OnSliderValueChanged(float linearValue)
    {
        EventSystem.current.SetSelectedGameObject(null);
        // ������ֵӳ��Ϊ������ֵ������ӳ�䣩
        float nonlinearValue = Mathf.Pow(10, Mathf.Lerp(
            Mathf.Log10(minValue),
            Mathf.Log10(maxValue),
            linearValue
        ));

        previousValidValue = Mathf.RoundToInt(nonlinearValue);
        inputField.text = previousValidValue.ToString();

        predict.setRenderer(previousValidValue);

        // ���������ֵ������̨�������ã�
        // Debug.Log("Non-linear Value: " + nonlinearValue);
    }
}
