using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 

public class GameManager : MonoBehaviour
{
    public GameObject mainMenu;
    public TMP_InputField inputNomeJogador; 

    public void IniciarJogo(string nomeCenaInicial)
    {
       
        if (!string.IsNullOrEmpty(inputNomeJogador.text))
        {
            GameDataManager.Instance.nomeJogador = inputNomeJogador.text;
        }
        else
        {
            GameDataManager.Instance.nomeJogador = "Jogador"; 
        }

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
