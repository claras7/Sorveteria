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

    void Start()
    {
        Time.timeScale = 1f;

        if (painelErro != null)
            painelErro.SetActive(false);

        acertosFase = 0;
        faseAtiva = true;

        IniciarFase();
    }

    void Update()
    {
        if (!faseAtiva || (painelErro != null && painelErro.activeSelf)) return;

        GameDataManager.Instance.tempoTotal += Time.deltaTime;

        if (textoTempo != null)
            textoTempo.text = Mathf.FloorToInt(GameDataManager.Instance.tempoTotal).ToString();
    }

    void IniciarFase()
    {
        if (faseAtual >= perguntas.Length)
        {
            GameDataManager.Instance.SalvarFase(tipoFase, acertosFase);

            if (string.IsNullOrEmpty(proximaCena))
                SceneManager.LoadScene("TelaFinal");
            else
                SceneManager.LoadScene(proximaCena);

            return;
        }

        faseAtiva = true;
        AtualizarUI();
    }

    // VERIFICA RESPOSTA
    public void VerificarResposta(int indiceEscolhido)
    {
        if (!faseAtiva) return;

        faseAtiva = false;

        if (indiceEscolhido == perguntas[faseAtual].indiceRespostaCorreta)
        {
            acertosFase++;
            AtualizarPontuacao();
            faseAtual++;
            IniciarFase();
        }
        else
        {
            MostrarErro();
            RegistrarRefeito();   // <-- ADICIONADO AQUI
        }
    }

    // MOSTRA O ERRO
    void MostrarErro()
    {
        if (painelErro != null)
            painelErro.SetActive(true);
    }

    // BOTÃO "Refazer"
    public void RefazerQuestao()
    {
        if (painelErro != null)
            painelErro.SetActive(false);

        faseAtiva = true;
        AtualizarUI();
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

            MostrarSorvetes(perguntaAtual.quantidadeSorveteEsquerda,
                            perguntaAtual.spriteSorveteEsquerdo,
                            grupoEsquerdo);

            MostrarSorvetes(perguntaAtual.quantidadeSorveteDireita,
                            perguntaAtual.spriteSorveteDireito,
                            grupoDireito);
        }
    }

    void MostrarSorvetes(int qtde, Sprite sprite, Transform grupo)
    {
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
    }

    void AtualizarPontuacao()
    {
        if (textoPontuacao != null)
            textoPontuacao.text = " " + acertosFase;
    }

    // -----------------------------
    // MÉTODO NOVO PARA CONTAR REFEITOS
    // -----------------------------
    void RegistrarRefeito()
    {
        switch (tipoFase)
        {
            case "Adicao":
                GameDataManager.Instance.refeitosPorFase[0]++;
                break;

            case "Subtracao":
                GameDataManager.Instance.refeitosPorFase[1]++;
                break;

            case "Multiplicacao":
                GameDataManager.Instance.refeitosPorFase[2]++;
                break;

            case "Divisao":
                GameDataManager.Instance.refeitosPorFase[3]++;
                break;
        }
    }
}
