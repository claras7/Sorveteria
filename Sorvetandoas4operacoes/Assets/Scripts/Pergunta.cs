using UnityEngine;

[System.Serializable]
public class Pergunta
{
    public string enunciado;                  // Texto da pergunta
    public string[] respostas;                // Alternativas
    public int indiceRespostaCorreta;         // Índice da resposta correta (0, 1, 2, 3)

    public int quantidadeSorveteEsquerda;     // Quantidade de sorvetes do lado esquerdo
    public int quantidadeSorveteDireita;      // Quantidade de sorvetes do lado direito

    public Sprite spriteSorveteEsquerdo;      // Sprite do sorvete do grupo esquerdo
    public Sprite spriteSorveteDireito;       // Sprite do sorvete do grupo direito

    public string operador;                   // Símbolo da operação: "+", "-", "×", "÷"
}


