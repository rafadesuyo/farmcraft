using UnityEngine;

public class MenuView : MonoBehaviour
{
    //Variables
    private Menu type;

    //Getters
    public virtual Menu Type => type;

    public void Open(MenuSetupOptions setupOptions = null)
    {
        gameObject.SetActive(true);
        Setup(setupOptions);
    }

    protected virtual void Setup(MenuSetupOptions setupOptions) { }

    public void Close()
    {
        OnClose();
        gameObject.SetActive(false);
    }

    protected void OpenSelf()
    {
        CanvasManager.Instance.OpenMenu(Type);
    }

    protected virtual void OnClose()
    { }

    public virtual void Initialize() { }
}
