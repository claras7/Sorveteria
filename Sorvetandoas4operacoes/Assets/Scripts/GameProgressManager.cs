using UnityEngine;

public static class GameProgressManager
{
    private const string KeyFaseAtual = "FaseAtual";
    private const string KeyPontuacao = "Pontuacao";
    private const string KeyVidas = "Vidas";

    // Salva o progresso atual do jogador
    public static void SalvarProgresso(int faseAtual, int pontuacao, int vidas)
    {
        PlayerPrefs.SetInt(KeyFaseAtual, faseAtual);
        PlayerPrefs.SetInt(KeyPontuacao, pontuacao);
        PlayerPrefs.SetInt(KeyVidas, vidas);
        PlayerPrefs.Save();
    }

    // Carrega os dados salvos (se não houver, define valores padrão)
    public static void CarregarProgresso(out int faseAtual, out int pontuacao, out int vidas)
    {
        faseAtual = PlayerPrefs.GetInt(KeyFaseAtual, 0);   // padrão: fase 0
        pontuacao = PlayerPrefs.GetInt(KeyPontuacao, 0);   // padrão: 0 pontos
        vidas = PlayerPrefs.GetInt(KeyVidas, 3);           // padrão: 3 vidas
    }

    // Apaga os dados salvos para recomeçar do zero
    public static void ResetarProgresso()
    {
        PlayerPrefs.DeleteKey(KeyFaseAtual);
        PlayerPrefs.DeleteKey(KeyPontuacao);
        PlayerPrefs.DeleteKey(KeyVidas);
    }
}
