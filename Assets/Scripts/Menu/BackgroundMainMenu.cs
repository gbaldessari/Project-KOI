using UnityEngine;
using UnityEngine.InputSystem;

public class BackgroundMainMenu : MonoBehaviour
{
    [SerializeField] float movementQuantity;

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<RectTransform>().position = new Vector2(
            (Mouse.current.position.ReadValue().x / Screen.width) * movementQuantity + (Screen.width / 2),
            (Mouse.current.position.ReadValue().y / Screen.height) * movementQuantity + (Screen.height / 2)
        );
    }
}
