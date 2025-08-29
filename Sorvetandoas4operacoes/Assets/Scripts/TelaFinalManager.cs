using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TelaFinalManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textoPontuacao;
    public TextMeshProUGUI textoTempo;
    public TextMeshProUGUI textoAcertos;  // Mostra acertos por fase
    public TextMeshProUGUI textoArquivo;  // opcional: caminho do arquivo salvo

    void Start()
    {
        // Recupera os valores salvos
        int pontuacaoFinal = PlayerPrefs.GetInt("PontuacaoTotal", 0);
        float tempoTotal = PlayerPrefs.GetFloat("TempoTotal", 0f);

        int acertosAdicao = PlayerPrefs.GetInt("AcertosAdicao", 0);
        int acertosSubtracao = PlayerPrefs.GetInt("AcertosSubtracao", 0);
        int acertosMultiplicacao = PlayerPrefs.GetInt("AcertosMultiplicacao", 0);
        int acertosDivisao = PlayerPrefs.GetInt("AcertosDivisao", 0);

        // Mostra pontuação e tempo
        if (textoPontuacao != null)
            textoPontuacao.text = "Pontuação Final: " + pontuacaoFinal;

        if (textoTempo != null)
            textoTempo.text = "Tempo Total: " + Mathf.RoundToInt(tempoTotal) + " segundos";

        // Mostra acertos por fase
        if (textoAcertos != null)
        {
            textoAcertos.text = 
                $"Acertos por fase:\n" +
                $"Adição: {acertosAdicao}\n" +
                $"Subtração: {acertosSubtracao}\n" +
                $"Multiplicação: {acertosMultiplicacao}\n" +
                $"Divisão: {acertosDivisao}";
        }

        // Mostra o caminho do arquivo salvo (opcional)
        string caminho = Application.persistentDataPath + "/resultados.txt";
        if (textoArquivo != null)
            textoArquivo.text = "Resultados também salvos em:\n" + caminho;
    }

    // Botão "Jogar Novamente"
    public void JogarNovamente()
    {
        // Zera os valores para nova jogada
        PlayerPrefs.SetInt("PontuacaoTotal", 0);
        PlayerPrefs.SetFloat("TempoTotal", 0f);
        PlayerPrefs.SetInt("AcertosAdicao", 0);
        PlayerPrefs.SetInt("AcertosSubtracao", 0);
        PlayerPrefs.SetInt("AcertosMultiplicacao", 0);
        PlayerPrefs.SetInt("AcertosDivisao", 0);
        PlayerPrefs.Save();

        SceneManager.LoadScene("MenuPrincipal");
    }

    // Botão "Sair"
    public void SairDoJogo()
    {
        Application.Quit();
        Debug.Log("Jogo encerrado");
    }
}
