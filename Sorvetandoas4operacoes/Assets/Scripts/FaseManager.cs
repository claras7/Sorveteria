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
    public TextMeshProUGUI textoTempo;       // tempo total aparecendo na tela
    public TextMeshProUGUI textoPontuacao;
    public GameObject painelErro;
    public TextMeshProUGUI textoOperador;

    [Header("Sorvetes Dinâmicos")]
    public GameObject prefabSorvete;
    public Transform grupoEsquerdo;
    public Transform grupoDireito;

    [Header("Configurações de fase")]
    public string proximaCena; 
    public string tipoFase;   

    private int faseAtual = 0;
    private bool faseAtiva = true;
    private int acertosFase = 0;
    private Coroutine coroutineErro;

    void Start()
    {
        Time.timeScale = 1f;
        if (painelErro != null) painelErro.SetActive(false);

        acertosFase = 0;
        faseAtiva = true;

        IniciarFase();
    }

    void Update()
    {
        if (!faseAtiva || painelErro.activeSelf) return;

        // Atualiza cronômetro 
        GameDataManager.Instance.tempoTotal += Time.deltaTime;

        // Exibe tempo em tela
        if (textoTempo != null)
            textoTempo.text = Mathf.FloorToInt(GameDataManager.Instance.tempoTotal).ToString();
    }

    void IniciarFase()
    {
        if (faseAtual >= perguntas.Length)
        {
            // Salva os acertos desta fase
            GameDataManager.Instance.SalvarFase(tipoFase, acertosFase);

            // Debug para conferir
            Debug.Log($"Fase {tipoFase} | Acertos: {acertosFase}");

            
            if (string.IsNullOrEmpty(proximaCena))
            {
                SceneManager.LoadScene("TelaFinal");
            }
            else
            {
                SceneManager.LoadScene(proximaCena);
            }
            return;
        }

        faseAtiva = true;
        AtualizarUI();
    }

    public void VerificarResposta(int indiceEscolhido)
    {
        if (!faseAtiva) return;

        faseAtiva = false;

        if (indiceEscolhido == perguntas[faseAtual].indiceRespostaCorreta)
        {
            acertosFase++;
            AtualizarPontuacao();
        }

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
    // Garante que o Grid Layout seja atualizado corretamente
    LayoutRebuilder.ForceRebuildLayoutImmediate(grupo.GetComponent<RectTransform>());

    foreach (Transform child in grupo)
        Destroy(child.gameObject);

    for (int i = 0; i < qtde; i++)
    {
        GameObject novo = Instantiate(prefabSorvete, grupo);
        Image img = novo.GetComponent<Image>();
        img.sprite = sprite;

        TextMeshProUGUI txt = novo.GetComponentInChildren<TextMeshProUGUI>();
        if (txt != null)
            txt.gameObject.SetActive(false);
    }

    // Força o Unity a reposicionar os novos itens no grid
    LayoutRebuilder.ForceRebuildLayoutImmediate(grupo.GetComponent<RectTransform>());
}


    void AtualizarPontuacao()
    {
        if (textoPontuacao != null)
            textoPontuacao.text = " " + acertosFase;
    }
}
