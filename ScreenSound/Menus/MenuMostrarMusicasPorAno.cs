using ScreenSound.Banco;
using ScreenSound.Modelos;

namespace ScreenSound.Menus;

internal class MenuMostrarMusicasPorAno : Menu
{
    public override void Executar(DAL<Artista> artistaDAL)
    {
        base.Executar(artistaDAL);
        ExibirTituloDaOpcao("Exibir musicas por ano");
        Console.Write("Digite o nome ano de lançamento: ");
        
        if (int.TryParse(Console.ReadLine(), out int anoLancamento)) 
        {
            var musicaDAL = new DAL<Musica>(new ScreenSoundContext());

            var listaMusicas = musicaDAL.ListaPor(m => m.AnoLancamento.Equals(anoLancamento));
            if (listaMusicas.Any())
            {
                Console.WriteLine("\nExibindo músicas:");

                foreach (var musica in listaMusicas)
                {
                    Console.WriteLine($"- {musica.Nome}");
                }
                
                Console.WriteLine("\nDigite uma tecla para voltar ao menu principal");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                Console.WriteLine($"\nNenhuma música lançada em {anoLancamento} foi encontrada!");
                Console.WriteLine("Digite uma tecla para voltar ao menu principal");
                Console.ReadKey();
                Console.Clear();
            }
        } else
        {
            Console.WriteLine($"\nformato inválido");
            Console.WriteLine("Digite uma tecla para voltar ao menu principal");
            Console.ReadKey();
            Console.Clear();
        }        
    }
}
