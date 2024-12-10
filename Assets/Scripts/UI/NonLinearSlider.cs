using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class NonLinearSlider : MonoBehaviour
{
    public Slider slider; // 拖拽绑定的滑动条
    public TMP_InputField inputField; // 显示滑动条数值的文本（可选）

    private int minValue = 10; // 滑动条的最小值
    private int maxValue = 1000; // 滑动条的最大值
    private int previousValidValue = Predict.initPredictDays;

    private Predict predict; // 用于存储父对象上的 Predict 实例


    void Start()
    {

        predict = GetComponentInParent<Predict>();
        slider.minValue = 0; // 设置滑动条线性范围
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

    // 在输入时验证（动态更新）
    private void ValidateInput(string input)
    {
        if (!int.TryParse(input, out int value))
            return;

        // 限制数值范围（如果需要动态限制）
        if (value > maxValue)
            inputField.text = maxValue.ToString();
    }

    // 在输入结束时验证（最终确认）
    private void EnsureValidRange(string input)
    {
        if (int.TryParse(input, out int value))
        {
            // 限制数值在 10-1000 范围内
            if (value < minValue)
                inputField.text = minValue.ToString();
            else if (value > maxValue)
                inputField.text = maxValue.ToString();
            if (previousValidValue.ToString() != inputField.text)
            {
                previousValidValue = int.Parse(inputField.text);
                // 将 previousValidValue 的对数值映射回 slider.value
                float logMin = Mathf.Log10(minValue);
                float logMax = Mathf.Log10(maxValue);
                slider.value = Mathf.InverseLerp(logMin, logMax, Mathf.Log10(previousValidValue));
                predict.setRenderer(previousValidValue);
            }
        }
        else
        {
            // 如果输入无效， 还原
            inputField.text = previousValidValue.ToString();
        }
    }

    void OnSliderValueChanged(float linearValue)
    {
        EventSystem.current.SetSelectedGameObject(null);
        // 将线性值映射为非线性值（对数映射）
        float nonlinearValue = Mathf.Pow(10, Mathf.Lerp(
            Mathf.Log10(minValue),
            Mathf.Log10(maxValue),
            linearValue
        ));

        previousValidValue = Mathf.RoundToInt(nonlinearValue);
        inputField.text = previousValidValue.ToString();

        predict.setRenderer(previousValidValue);

        // 输出非线性值到控制台（测试用）
        // Debug.Log("Non-linear Value: " + nonlinearValue);
    }
}
