using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

[System.Serializable]
public class Pergunta
{
    public string enunciado;
    public string[] respostas = new string[4];
    public int indiceRespostaCorreta;
}

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

    [Header("Configurações do jogo")]
    public float tempoLimite = 30f;
    private float tempoRestante;
    private float tempoTotalGasto = 0f;

    private int faseAtual = 0;
    private int pontuacao = 0;
    private bool faseAtiva = true;
    private Coroutine coroutineErro;

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
        {
            textoTempo.text = Mathf.CeilToInt(tempoRestante).ToString();
        }
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
            if (tempoGastoNaFase <= tempoLimite)
                pontuacao += 10;
            else
                pontuacao += 5;

            AtualizarPontuacao();

            faseAtual++;
            IniciarFase();
        }
        else
        {
            painelErro.SetActive(true);
            Time.timeScale = 0f;

            // Inicia contagem automática para pular fase
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
        yield return new WaitForSecondsRealtime(segundos); // Espera real mesmo com Time.timeScale = 0
        ContinuarDepoisErro();
    }

    void AtualizarUI()
    {
        if (faseAtual < perguntas.Length)
        {
            textoEnunciado.text = perguntas[faseAtual].enunciado;

            for (int i = 0; i < textosRespostas.Length; i++)
            {
                textosRespostas[i].text = perguntas[faseAtual].respostas[i];
            }
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
