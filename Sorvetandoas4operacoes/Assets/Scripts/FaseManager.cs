using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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
    private int faseAtual = 0;
    private int pontuacao = 0;
    private bool faseAtiva = true;

    void Start()
    {
        Time.timeScale = 1f;
        if (painelTempoEsgotado != null) painelTempoEsgotado.SetActive(false);
        if (painelErro != null) painelErro.SetActive(false);
        pontuacao = 0;
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
            textoTempo.text = "Tempo: " + Mathf.CeilToInt(tempoRestante).ToString();
        }
        else
        {
            textoTempo.text = "Tempo: 0";
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
            // Aqui você pode mostrar uma tela de fim de jogo, pontuação final etc.
            return;
        }

        tempoRestante = tempoLimite;
        faseAtiva = true;
        AtualizarUI();
    }

    public void VerificarResposta(int indiceEscolhido)
    {
        if (!faseAtiva) return;

        if (indiceEscolhido == perguntas[faseAtual].indiceRespostaCorreta)
        {
            pontuacao += 10; // adiciona 10 pontos por acerto
            AtualizarPontuacao();

            faseAtual++;
            IniciarFase();
        }
        else
        {
            // Errou, mostra painel de erro e pausa
            faseAtiva = false;
            painelErro.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void ContinuarDepoisErro()
    {
        painelErro.SetActive(false);
        Time.timeScale = 1f;
        faseAtual++;
        IniciarFase();
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
        textoPontuacao.text = "Pontuação: " + pontuacao;
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
