using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject mainMenu;

    public void IniciarJogo(string nomeCenaInicial)
    {
        SceneManager.LoadScene(nomeCenaInicial);
    }

    public void AbrirMenuPrincipal()
    {
        mainMenu.SetActive(true);
    }

    public void FecharMenuPrincipal()
    {
        mainMenu.SetActive(false);
    }

    public void SairDoJogo()
    {
        Application.Quit();
    }
}
