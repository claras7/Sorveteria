using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // se usar TextMeshPro

public class FaseManager : MonoBehaviour
{
    public float tempoLimite = 30f;
    private float tempoRestante;
    public TextMeshProUGUI textoTempo; // Ou Text se estiver usando UI > Text
    public GameObject painelTempoEsgotado;

    void Start()
    {
        tempoRestante = tempoLimite;
        painelTempoEsgotado.SetActive(false);
    }

    void Update()
    {
        tempoRestante -= Time.deltaTime;

        if (tempoRestante > 0)
        {
            textoTempo.text = "Tempo: " + Mathf.CeilToInt(tempoRestante).ToString();
        }
        else
        {
            textoTempo.text = "Tempo: 0";
            painelTempoEsgotado.SetActive(true);
            Time.timeScale = 0f; // pausa o jogo
        }
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

