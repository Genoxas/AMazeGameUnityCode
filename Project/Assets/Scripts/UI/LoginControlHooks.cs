using UnityEngine;
using UnityEngine.UI;

public class LoginControlHooks : MonoBehaviour
{
  public delegate void CanvasHook();

  public CanvasHook OnLoginHook;
  public CanvasHook OnRegisterHook;
  public CanvasHook OnExitHook;
  public CanvasHook OnGuestHook;

  public Text userNameInput;
  public Text passwordInput;
  public Text errorMessage;

  //Gets User name
  public string GetUserName()
  {
    return userNameInput.text;
  }
  
  //Get Password
  public string GetPassword()
  {
    return passwordInput.text;
  }

  //Log User in
  public void UILogin()
  {
    if(OnLoginHook != null)
      OnLoginHook.Invoke();
  }

  public void UIExit()
  {
    if(OnExitHook != null)
      OnExitHook.Invoke();
  }

  public void UIRegister()
  {
    if(OnRegisterHook != null)
      OnRegisterHook.Invoke();
  }

  public void UIGuest()
  {
    if(OnRegisterHook != null)
      OnGuestHook.Invoke();
  }

}

