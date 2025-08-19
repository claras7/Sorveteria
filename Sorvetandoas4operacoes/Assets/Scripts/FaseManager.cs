using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class FaseManager : MonoBehaviour
{
    [Header("Perguntas")]
    public Pergunta[] perguntas;

    [Header("UI")]
    public TextMeshProUGUI textoEnunciado;
    public TextMeshProUGUI[] textosRespostas;
    public TextMeshProUGUI textoTempo;
    public TextMeshProUGUI textoPontuacao;
    public GameObject painelTempoEsgotado;
    public GameObject painelErro;
    public TextMeshProUGUI textoOperador; // operador no meio da tela

    [Header("Configurações do jogo")]
    public float tempoLimite = 30f;
    private float tempoRestante;
    private float tempoTotalGasto = 0f;

    private int faseAtual = 0;
    private int pontuacao = 0;
    private bool faseAtiva = true;
    private Coroutine coroutineErro;

    [Header("Sorvetes Dinâmicos")]
    public GameObject prefabSorvete;    // Prefab do sorvete (Image + Text)
    public Transform grupoEsquerdo;     // Painel do grupo esquerdo
    public Transform grupoDireito;      // Painel do grupo direito
    public int maxLinhas = 3;           // Máximo de linhas antes de usar "xN"

    void Start()
    {
        Time.timeScale = 1f;
        if (painelTempoEsgotado != null) painelTempoEsgotado.SetActive(false);
        if (painelErro != null) painelErro.SetActive(false);

        pontuacao = 0;
        tempoTotalGasto = 0f;
        IniciarFase();
        AtualizarPontuacao();
    }

    void Update()
    {
        if (!faseAtiva || painelTempoEsgotado.activeSelf || painelErro.activeSelf)
            return;

        tempoRestante -= Time.deltaTime;

        if (tempoRestante > 0)
            textoTempo.text = Mathf.CeilToInt(tempoRestante).ToString();
        else
        {
            textoTempo.text = "0";
            faseAtiva = false;
            painelTempoEsgotado.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    void IniciarFase()
    {
        Time.timeScale = 1f;
        painelTempoEsgotado.SetActive(false);
        painelErro.SetActive(false);

        if (faseAtual >= perguntas.Length)
        {
            Debug.Log("Parabéns! Você terminou todas as fases.");
            Debug.Log("Pontuação final: " + pontuacao);
            Debug.Log("Tempo total: " + Mathf.RoundToInt(tempoTotalGasto) + " segundos");
            return;
        }

        tempoRestante = tempoLimite;
        faseAtiva = true;
        AtualizarUI();
    }

    public void VerificarResposta(int indiceEscolhido)
    {
        if (!faseAtiva) return;

        faseAtiva = false;
        float tempoGastoNaFase = tempoLimite - tempoRestante;
        tempoTotalGasto += tempoGastoNaFase;

        if (indiceEscolhido == perguntas[faseAtual].indiceRespostaCorreta)
        {
            pontuacao += tempoGastoNaFase <= tempoLimite ? 10 : 5;
            AtualizarPontuacao();
            faseAtual++;
            IniciarFase();
        }
        else
        {
            painelErro.SetActive(true);
            Time.timeScale = 0f;
            coroutineErro = StartCoroutine(AutoContinuarDepoisErro(3f));
        }
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

    IEnumerator AutoContinuarDepoisErro(float segundos)
    {
        yield return new WaitForSecondsRealtime(segundos);
        ContinuarDepoisErro();
    }

    void AtualizarUI()
    {
        if (faseAtual < perguntas.Length)
        {
            Pergunta perguntaAtual = perguntas[faseAtual];

            textoEnunciado.text = perguntaAtual.enunciado;

            for (int i = 0; i < textosRespostas.Length; i++)
                textosRespostas[i].text = perguntaAtual.respostas[i];

            // Atualiza o operador no meio
            if (textoOperador != null)
                textoOperador.text = perguntaAtual.operador;

            // Cria sorvetes de cada lado
            MostrarSorvetes(perguntaAtual.quantidadeSorveteEsquerda, perguntaAtual.spriteSorveteEsquerdo, grupoEsquerdo);
            MostrarSorvetes(perguntaAtual.quantidadeSorveteDireita, perguntaAtual.spriteSorveteDireito, grupoDireito);
        }
    }

    void MostrarSorvetes(int qtde, Sprite sprite, Transform grupo)
    {
        // Limpa antigos
        foreach (Transform child in grupo)
            Destroy(child.gameObject);

        RectTransform rtGrupo = grupo.GetComponent<RectTransform>();
        float larguraPainel = rtGrupo.rect.width;
        float alturaPainel = rtGrupo.rect.height;

        RectTransform rtPrefab = prefabSorvete.GetComponent<RectTransform>();
        float larguraSorvete = rtPrefab.rect.width;
        float alturaSorvete = rtPrefab.rect.height;

        // Calcula colunas e linhas necessárias
        int colunas = Mathf.FloorToInt(larguraPainel / larguraSorvete);
        if (colunas < 1) colunas = 1;
        int linhas = Mathf.CeilToInt((float)qtde / colunas);

        // Se linhas excederem maxLinhas, mostra apenas "xN"
        if (linhas > maxLinhas)
    {
    GameObject novo = Instantiate(prefabSorvete, grupo);
    Image img = novo.GetComponent<Image>();
    img.sprite = sprite;

    RectTransform rt = novo.GetComponent<RectTransform>();
    rt.localScale = Vector3.one; // 👈 força tamanho normal (sem encolher)

    TextMeshProUGUI txt = novo.GetComponentInChildren<TextMeshProUGUI>();
    if (txt != null)
    {
        txt.gameObject.SetActive(true);
        txt.text = "x" + qtde;
    }
    return;
    }


        // Ajusta escala para caber verticalmente
        float escalaY = 1f;
        float alturaNecessaria = linhas * alturaSorvete;
        if (alturaNecessaria > alturaPainel)
            escalaY = alturaPainel / alturaNecessaria;

        // Instancia sorvetes em grid
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

            // Desativa texto "xN"
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
