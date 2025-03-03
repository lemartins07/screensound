﻿using Microsoft.AspNetCore.Mvc;
using ScreenSound.API.Requests;
using ScreenSound.API.Response;
using ScreenSound.Banco;
using ScreenSound.Modelos;

namespace ScreenSound.API.EndPoints;

public static class MusicasExtensions
{
    public static void AddEndPointsMusicas(this WebApplication app)
    {
        app.MapGet("/musicas", ([FromServices] DAL<Musica> dal) =>
        {
            return Results.Ok(dal.Listar());
        });

        app.MapGet("/musicas/{nome}", ([FromServices] DAL<Musica> dal, string nome) =>
        {
            var musica = dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));
            if (musica is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(musica);

        });

        app.MapPost("/musicas", ([FromServices] DAL<Musica> dal, [FromBody] MusicasRequest musicasRequest) =>
        {
            var musica = new Musica(musicasRequest.nome)
            {
                AnoLancamento = musicasRequest.anoLancamento
            };

            dal.Adicionar(musica);

            return Results.Ok();
        });

        app.MapPut("/musicas", ([FromServices] DAL<Musica> dal, [FromBody] MusicasRequestEdit musicaRequestEdit) =>
        {
            var musicaAAtualizar = dal.RecuperarPor(a => a.Id.Equals(musicaRequestEdit.id));
            if (musicaAAtualizar is null)
            {
                return Results.NotFound();
            }

            musicaAAtualizar.Nome = musicaRequestEdit.nome;
            musicaAAtualizar.AnoLancamento = musicaRequestEdit.anoLancamento;

            dal.Atualizar(musicaAAtualizar);

            return Results.Ok();
        });

        app.MapDelete("/musicas/{id}", ([FromServices] DAL<Musica> dal, int id) =>
        {
            var musica = dal.RecuperarPor(m => m.Id == id);

            if (musica is null)
            {
                return Results.NotFound();
            }

            dal.Deletar(musica);

            return Results.NoContent();
        });
    }

    private static ICollection<MusicasResponse> EntityListToResponseList(IEnumerable<Musica> musicaList)
    {
        return musicaList.Select(a => EntityToResponse(a)).ToList();
    }

    private static MusicasResponse EntityToResponse(Musica musica)
    {
        return new MusicasResponse(musica.Id, musica.Nome!, musica.Artista!.Id, musica.Artista.Nome);
    }
}
