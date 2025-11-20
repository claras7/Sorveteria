using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    [Header("Dados do Jogo")]
    public string nomeJogador = "";
    public float tempoTotal = 0f;

    public int acertosAdicao = 0;
    public int acertosSubtracao = 0;
    public int acertosMultiplicacao = 0;
    public int acertosDivisao = 0;

    // NOVO: quantidade de questões refeitas em cada fase
    public int[] refeitosPorFase = new int[4];

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

    public void ResetarDados()
    {
        tempoTotal = 0f;
        acertosAdicao = 0;
        acertosSubtracao = 0;
        acertosMultiplicacao = 0;
        acertosDivisao = 0;
        nomeJogador = "";

        refeitosPorFase = new int[4]; // <-- RESETA OS REFEITOS
    }

    public void SalvarEmArquivo()
    {
        string caminho = Application.persistentDataPath + "/resultados.txt";
        string dataHora = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

        string conteudo =
            $"Jogador: {nomeJogador}\n" +
            $"Acertos:\n" +
            $"Adição: {acertosAdicao}/5\n" +
            $"Subtração: {acertosSubtracao}/5\n" +
            $"Multiplicação: {acertosMultiplicacao}/5\n" +
            $"Divisão: {acertosDivisao}/5\n\n" +

            // NOVO BLOCO — REFEITOS
            $"Refações (erros):\n" +
            $"Adição: {refeitosPorFase[0]}\n" +
            $"Subtração: {refeitosPorFase[1]}\n" +
            $"Multiplicação: {refeitosPorFase[2]}\n" +
            $"Divisão: {refeitosPorFase[3]}\n\n" +

            $"Tempo total: {Mathf.RoundToInt(tempoTotal)} segundos\n" +
            $"Data: {dataHora}\n" +
            "-----------------------\n\n";

        System.IO.File.AppendAllText(caminho, conteudo);
        Debug.Log("Resultados salvos em arquivo: " + caminho);
    }

    public void SalvarEReiniciar()
    {
        SalvarEmArquivo();
        ResetarDados();
        UnityEngine.SceneManagement.SceneManager.LoadScene("CenaDoJogo");
    }
}
