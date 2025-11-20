using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TelaFinalManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textoMensagem;
    public TextMeshProUGUI textoTempo;

    void Start()
    {
        // 1️⃣ Mensagem de parabéns
        if (textoMensagem != null)
            textoMensagem.text = "Parabéns, você concluiu todas as fases!";

        // 2️⃣ Tempo total do jogo
        if (textoTempo != null)
        {
            int totalSegundos = Mathf.RoundToInt(GameDataManager.Instance.tempoTotal);
            int minutos = totalSegundos / 60;
            int segundos = totalSegundos % 60;

            textoTempo.text = $"Tempo total: {minutos:00}:{segundos:00}";
        }
    }

    public void JogarNovamente()
    {
        GameDataManager.Instance.SalvarEmArquivo();
        GameDataManager.Instance.ResetarDados();
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void SairDoJogo()
    {
        Debug.Log("Jogo encerrado");
        GameDataManager.Instance.SalvarEmArquivo();
        Application.Quit();
    }
}
