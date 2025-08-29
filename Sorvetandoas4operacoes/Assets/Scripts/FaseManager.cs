using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class FaseManager : MonoBehaviour
{
    [Header("Perguntas")]
    public Pergunta[] perguntas;

    [Header("UI")]
    public TextMeshProUGUI textoEnunciado;
    public TextMeshProUGUI[] textosRespostas;
    public TextMeshProUGUI textoTempo;       // tempo da fase atual
    public TextMeshProUGUI textoTempoTotal;  // tempo acumulado total
    public TextMeshProUGUI textoPontuacao;
    public GameObject painelErro;
    public TextMeshProUGUI textoOperador;    // operador no meio da tela

    [Header("Configurações do jogo")]
    private float tempoDecorrido = 0f;       // tempo da fase atual
    private float tempoTotalGasto = 0f;      // tempo total acumulado

    private int faseAtual = 0;
    private int pontuacao = 0;
    private bool faseAtiva = true;
    private Coroutine coroutineErro;

    [Header("Sorvetes Dinâmicos")]
    public GameObject prefabSorvete;    
    public Transform grupoEsquerdo;     
    public Transform grupoDireito;      
    public int maxLinhas = 3;           

    [Header("Configurações de fases")]
    public string proximaCena; // aqui você coloca o nome da próxima cena da fase

    void Start()
    {
        Time.timeScale = 1f;
        if (painelErro != null) painelErro.SetActive(false);

        // Inicializa valores do jogo (sempre do zero)
        pontuacao = 0;
        tempoTotalGasto = 0f;

        IniciarFase();
        AtualizarPontuacao();
    }

    void Update()
    {
        if (!faseAtiva || painelErro.activeSelf)
            return;

        // Atualiza tempo da fase atual
        tempoDecorrido += Time.deltaTime;
        if (textoTempo != null)
            textoTempo.text = Mathf.FloorToInt(tempoDecorrido).ToString();

        // Atualiza tempo total acumulado
        if (textoTempoTotal != null)
        {
            int tempoTotalInt = Mathf.FloorToInt(tempoTotalGasto + tempoDecorrido);
            int minutos = tempoTotalInt / 60;
            int segundos = tempoTotalInt % 60;
            textoTempoTotal.text = string.Format("{0:00}:{1:00}", minutos, segundos);
        }
    }

    void IniciarFase()
    {
        Time.timeScale = 1f;
        painelErro.SetActive(false);

        if (faseAtual >= perguntas.Length)
        {
            // Fim da fase → ir para a próxima cena
            Debug.Log("Fim da fase atual.");
            SalvarResultados();
            if (!string.IsNullOrEmpty(proximaCena))
                SceneManager.LoadScene(proximaCena);
            return;
        }

        faseAtiva = true;
        tempoDecorrido = 0f;
        AtualizarUI();
    }

    void SalvarResultados()
    {
        // Salva em arquivo
        string caminho = Application.persistentDataPath + "/resultados.txt";
        string conteudo = $"Pontuação atual: {pontuacao}\nTempo total: {Mathf.RoundToInt(tempoTotalGasto + tempoDecorrido)} segundos\nData: {System.DateTime.Now}\n----------------\n";
        File.AppendAllText(caminho, conteudo);
        Debug.Log("Resultados salvos em: " + caminho);

        // Salva PlayerPrefs
        PlayerPrefs.SetInt("PontuacaoTotal", pontuacao);
        PlayerPrefs.SetFloat("TempoTotal", tempoTotalGasto + tempoDecorrido);
        PlayerPrefs.Save();
    }

    public void VerificarResposta(int indiceEscolhido)
    {
        if (!faseAtiva) return;

        faseAtiva = false;

        float tempoGastoNaFase = tempoDecorrido;
        tempoTotalGasto += tempoGastoNaFase;

        if (indiceEscolhido == perguntas[faseAtual].indiceRespostaCorreta)
        {
            pontuacao += 1; // pontuação fixa por acerto
            AtualizarPontuacao();
            faseAtual++;
            IniciarFase();
        }
        else
        {
            painelErro.SetActive(true);
            Time.timeScale = 0f;
            coroutineErro = StartCoroutine(AutoContinuarDepoisErro(2f)); // continua automaticamente
        }
    }

    IEnumerator AutoContinuarDepoisErro(float segundos)
    {
        yield return new WaitForSecondsRealtime(segundos);
        ContinuarDepoisErro();
    }

    public void ContinuarDepoisErro()
    {
        if (coroutineErro != null)
        {
            StopCoroutine(coroutineErro);
            coroutineErro = null;
        }

        painelErro.SetActive(false);
        Time.timeScale = 1f;
        faseAtual++;
        IniciarFase();
    }

    void AtualizarUI()
    {
        if (faseAtual < perguntas.Length)
        {
            Pergunta perguntaAtual = perguntas[faseAtual];

            textoEnunciado.text = perguntaAtual.enunciado;

            for (int i = 0; i < textosRespostas.Length; i++)
                textosRespostas[i].text = perguntaAtual.respostas[i];

            if (textoOperador != null)
                textoOperador.text = perguntaAtual.operador;

            MostrarSorvetes(perguntaAtual.quantidadeSorveteEsquerda, perguntaAtual.spriteSorveteEsquerdo, grupoEsquerdo);
            MostrarSorvetes(perguntaAtual.quantidadeSorveteDireita, perguntaAtual.spriteSorveteDireito, grupoDireito);
        }
    }

    void MostrarSorvetes(int qtde, Sprite sprite, Transform grupo)
    {
        foreach (Transform child in grupo)
            Destroy(child.gameObject);

        RectTransform rtGrupo = grupo.GetComponent<RectTransform>();
        float larguraPainel = rtGrupo.rect.width;
        float alturaPainel = rtGrupo.rect.height;

        RectTransform rtPrefab = prefabSorvete.GetComponent<RectTransform>();
        float larguraSorvete = rtPrefab.rect.width;
        float alturaSorvete = rtPrefab.rect.height;

        int colunas = Mathf.FloorToInt(larguraPainel / larguraSorvete);
        if (colunas < 1) colunas = 1;
        int linhas = Mathf.CeilToInt((float)qtde / colunas);

        if (linhas > maxLinhas)
        {
            GameObject novo = Instantiate(prefabSorvete, grupo);
            Image img = novo.GetComponent<Image>();
            img.sprite = sprite;

            RectTransform rt = novo.GetComponent<RectTransform>();
            rt.localScale = Vector3.one;

            TextMeshProUGUI txt = novo.GetComponentInChildren<TextMeshProUGUI>();
            if (txt != null)
            {
                txt.gameObject.SetActive(true);
                txt.text = "x" + qtde;
            }
            return;
        }

        float escalaY = 1f;
        float alturaNecessaria = linhas * alturaSorvete;
        if (alturaNecessaria > alturaPainel)
            escalaY = alturaPainel / alturaNecessaria;

        for (int i = 0; i < qtde; i++)
        {
            int linha = i / colunas;
            int coluna = i % colunas;

            GameObject novo = Instantiate(prefabSorvete, grupo);
            Image img = novo.GetComponent<Image>();
            img.sprite = sprite;

            RectTransform rt = novo.GetComponent<RectTransform>();
            rt.localScale = new Vector3(escalaY, escalaY, 1f);

            float x = coluna * larguraSorvete * escalaY;
            float y = -linha * alturaSorvete * escalaY;
            rt.anchoredPosition = new Vector2(x, y);

            TextMeshProUGUI txt = novo.GetComponentInChildren<TextMeshProUGUI>();
            if (txt != null)
                txt.gameObject.SetActive(false);
        }
    }

    void AtualizarPontuacao()
    {
        textoPontuacao.text = " " + pontuacao;
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
