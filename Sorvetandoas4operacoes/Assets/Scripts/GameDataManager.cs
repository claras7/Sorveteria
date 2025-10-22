using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    [Header("Cronômetro e Acertos")]
    public float tempoTotal = 0f; // cronômetro contínuo
    public int acertosAdicao = 0;
    public int acertosSubtracao = 0;
    public int acertosMultiplicacao = 0;
    public int acertosDivisao = 0;

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
    /// Salva os acertos de cada fase.
    public void SalvarFase(string tipoFase, int acertos)
    {
        switch (tipoFase)
        {
            case "Adicao":
                acertosAdicao = acertos;
                break;
            case "Subtracao":
                acertosSubtracao = acertos;
                break;
            case "Multiplicacao":
                acertosMultiplicacao = acertos;
                break;
            case "Divisao":
                acertosDivisao = acertos;
                break;
            default:
                Debug.LogWarning("Tipo de fase inválido: " + tipoFase);
                break;
        }

        Debug.Log($"Fase {tipoFase} salva: {acertos} acertos | Tempo total: {tempoTotal:F1}s");
    }

    
    /// Reseta todos os dados do jogo para uma nova jogada.
    public void ResetarDados()
    {
        tempoTotal = 0f;
        acertosAdicao = 0;
        acertosSubtracao = 0;
        acertosMultiplicacao = 0;
        acertosDivisao = 0;
    }

    
    /// Salva resultados em arquivo.
    public void SalvarEmArquivo()
    {
        string caminho = Application.persistentDataPath + "/resultados.txt";
        string dataHora = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        string conteudo =
            $"Acertos:\n" +
            $"Adição: {acertosAdicao}/5\n" +
            $"Subtração: {acertosSubtracao}/5\n" +
            $"Multiplicação: {acertosMultiplicacao}/5\n" +
            $"Divisão: {acertosDivisao}/5\n" +
            $"Tempo total: {Mathf.RoundToInt(tempoTotal)} segundos\n\n" +
            $"Data: {dataHora}\n"+
            "-----------------------\n\n";

        System.IO.File.AppendAllText(caminho, conteudo);
        Debug.Log("Resultados salvos em arquivo: " + caminho);
    }
}
