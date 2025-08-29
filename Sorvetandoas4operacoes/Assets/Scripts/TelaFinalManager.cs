using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TelaFinalManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textoMensagem;
    public TextMeshProUGUI textoAcertos;
    public TextMeshProUGUI textoTempo;

    void Start()
    {
        // Mensagem de conclusão
        if (textoMensagem != null)
            textoMensagem.text = "Parabéns, você concluiu todas as fases!";

        // Mostra acertos por fase
        if (textoAcertos != null)
        {
            textoAcertos.text =
                $"Adição: {GameDataManager.Instance.acertosAdicao}/5\n" +
                $"Subtração: {GameDataManager.Instance.acertosSubtracao}/5\n" +
                $"Multiplicação: {GameDataManager.Instance.acertosMultiplicacao}/5\n" +
                $"Divisão: {GameDataManager.Instance.acertosDivisao}/5";
        }

        // Mostra tempo total em mm:ss
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
        GameDataManager.Instance.ResetarDados();
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void SairDoJogo()
    {
        Application.Quit();
        Debug.Log("Jogo encerrado");
    }
}
