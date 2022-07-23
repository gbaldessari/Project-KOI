using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarController : MonoBehaviour
{
    private Slider slider;
    private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        slider = GetComponent<Slider>();
        animator = GetComponent<Animator>();
    }

    public void SetValue(float value)
    {
        slider.value = value;
    }

    public void UpdateEnabled(bool enablingMode)
    {
        animator.SetBool("IsOpen", enablingMode);
    }

    public bool GetCurrentStatus()
    {
        return animator.GetBool("IsOpen");
    }
}
