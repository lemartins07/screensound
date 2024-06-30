using Microsoft.AspNetCore.Mvc;
using ScreenSound.API.Requests;
using ScreenSound.API.Response;
using ScreenSound.Banco;
using ScreenSound.Modelos;

namespace ScreenSound.API.EndPoints;

public static class ArtistasExtensions
{
    public static void AddEndPointsArtistas(this WebApplication app)
    {
        app.MapGet("/artistas", ([FromServices] DAL<Artista> dal) =>
        {
            var response = EntityListToResponseList(dal.Listar());

            return Results.Ok(response);
        });

        app.MapGet("/artistas/{nome}", ([FromServices] DAL<Artista> dal, string nome) =>
        {
            var artista = dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));

            if (artista is null)
            {
                return Results.NotFound();
            }

            var response = EntityToResponse(artista);

            return Results.Ok(response);
        });

        app.MapPost("/artistas", ([FromServices] DAL<Artista> dal, [FromBody] ArtistasRequest artistaRequest) =>
        {
            var artista = new Artista(artistaRequest.nome, artistaRequest.bio);            

            dal.Adicionar(artista);

            return Results.Ok();
        });

        app.MapDelete("/artistas/{id}", ([FromServices] DAL<Artista> dal, int id) =>
        {
            var artista = dal.RecuperarPor(a => a.Id == id);
            if (artista is null)
            {
                return Results.NotFound();
            }
            dal.Deletar(artista);

            return Results.NoContent();
        });

        app.MapPut("/artistas", ([FromServices] DAL<Artista> dal, [FromBody] ArtistasRequestEdit artistaEdit) =>
        {
            var artistaAAtualizar = dal.RecuperarPor(a => a.Id.Equals(artistaEdit.id));
            if (artistaAAtualizar is null)
            {
                return Results.NotFound();
            }

            artistaAAtualizar.Nome = artistaEdit.nome;
            artistaAAtualizar.Bio = artistaEdit.bio;
            artistaAAtualizar.FotoPerfil = artistaEdit.fotoPerfil;

            dal.Atualizar(artistaAAtualizar);

            return Results.Ok();
        });
    }

    private static ICollection<ArtistasResponse> EntityListToResponseList(IEnumerable<Artista> listaDeArtistas)
    {
        return listaDeArtistas.Select(a => EntityToResponse(a)).ToList();
    }

    private static ArtistasResponse EntityToResponse(Artista artista)
    {
        return new ArtistasResponse(artista.Id, artista.Nome, artista.Bio, artista.FotoPerfil);
    }
}
