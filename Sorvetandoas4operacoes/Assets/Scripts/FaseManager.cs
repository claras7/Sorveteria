using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class NivelComOpcoes
{
    public string enunciado;
    public string[] opcoes = new string[4]; // 4 opções
    public int indiceRespostaCorreta; // Índice de 0 a 3
}

public class FaseManager : MonoBehaviour
{
    [Header("Referências da UI")]
    public TextMeshProUGUI textoEnunciado;
    public Button[] botoesResposta;               // Botões na mesma ordem das opções
    public TextMeshProUGUI[] textosBotoes;

    [Header("Configuração de níveis")]
    [SerializeField]
    public NivelComOpcoes[] niveis;

    private int nivelAtual = 0;

    void Start()
    {
        CarregarNivel(nivelAtual);
    }

    void CarregarNivel(int index)
    {
        if (index < niveis.Length)
        {
            NivelComOpcoes nivel = niveis[index];

            textoEnunciado.text = nivel.enunciado;

            for (int i = 0; i < botoesResposta.Length; i++)
            {
                textosBotoes[i].text = nivel.opcoes[i];

                int capturaIndice = i;
                botoesResposta[i].onClick.RemoveAllListeners();
                botoesResposta[i].onClick.AddListener(() => VerificarResposta(capturaIndice));
            }
        }
        else
        {
            Debug.Log("Você completou todos os níveis!");
        }
    }

    public void VerificarResposta(int indiceEscolhido)
    {
        if (indiceEscolhido == niveis[nivelAtual].indiceRespostaCorreta)
        {
            Debug.Log("Resposta correta!");
            nivelAtual++;
            CarregarNivel(nivelAtual);
        }
        else
        {
            Debug.Log("Resposta errada! Tente novamente.");
        }
    }
}
