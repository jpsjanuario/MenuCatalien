﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

public class scr_Ranking : MonoBehaviour
{

    #region - Propriedades -
    private List<Pontuacao> _ListaRanking;
    public InputField[] _inputNomes;
    public InputField[] _inputPontos;
    public int _pontuacao;
    private int _idText = -1;
    #endregion

    #region - Start -
    void Start()
    {
        _pontuacao = (int)(UnityEngine.Random.Range(3000, 5000));
        CarregarRanking();
        ExibirRanking();

    }
    #endregion

    #region - Ranking -
    private void CarregarRanking()
    {
        _ListaRanking = RankingXML.Load(Path.Combine(Application.persistentDataPath, "ranking.xml")).Ranking;
    }

    private void ExibirRanking()
    {
        _ListaRanking.Add(new Pontuacao { Nome = "???", Pontos = _pontuacao });
        _ListaRanking = _ListaRanking.OrderByDescending(x => x.Pontos).ToList();


        for (int i = 0; i < 3; i++)
        {
            var nome = _inputNomes[i];
            var pontos = _inputPontos[i];

            nome.text = _ListaRanking[i].Nome;
            pontos.text = _ListaRanking[i].Pontos.ToString();

            if (nome.text == "???")
            {
                _idText = i; //Encontra qual o text que tem o nome em branco, ou seja, onde o jogador vai inserir seu nome
                nome.interactable = true;
            }
        }
    }

    private void SalvarRanking()
    {
        try
        {
            if(_idText >= 0)
            {
                var novoNome = _inputNomes[_idText].text;

                var novoRecorde = new Pontuacao
                {
                    Nome = novoNome == "???" ? "---" : novoNome,
                    Pontos = _pontuacao,
                    Data = DateTime.Now
                };

                var substituir = _ListaRanking.RemoveAll(x => x.Nome == "???");

                _ListaRanking.Add(novoRecorde);
            }

            var ranking = new RankingXML();
            ranking.Save(_ListaRanking, Application.persistentDataPath +  "/ranking.xml");
            Debug.Log("Ranking.xml salvo em " + Application.persistentDataPath);
        }
        catch (Exception ex)
        {
            Debug.Log("Erro ao salvar ranking");
        }
    }
    #endregion


    #region - Menus -
    public void AvancarFase(int idFase)
    {
        SalvarRanking();
    }

    public void AbrirMenu()
    {
        SalvarRanking();
        Application.LoadLevel("sce_Home");
    }

    public void ReiniciarFase()
    {
        SalvarRanking();
    }
    #endregion

    #region - Update -
    void Update()
    {

    }
    #endregion
}