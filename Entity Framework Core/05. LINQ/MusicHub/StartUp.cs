namespace MusicHub
{
    using System;
    using System.Linq;
    using System.Text;
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Console.WriteLine(ExportAlbumsInfo(context, 9));
            //Console.WriteLine(ExportSongsAboveDuration(context, 4));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albumsInfo = context.Producers
                .Find(producerId)
                .Albums
                .Select(x => new
                {
                    x.Name,
                    x.ReleaseDate,
                    ProducerName = x.Producer.Name,
                    Songs = x.Songs.Select(x => new
                    {
                        x.Name,
                        x.Price,
                        WriterName = x.Writer.Name
                    })
                    .OrderByDescending(x => x.Name)
                    .ThenBy(x => x.WriterName),
                    x.Price
                })
                .OrderByDescending(x => x.Price)
                .ToList();

            var sb = new StringBuilder();
            foreach (var album in albumsInfo)
            {
                sb.AppendLine($"-AlbumName: {album.Name}");
                sb.AppendLine($"-ReleaseDate: {album.ReleaseDate:MM/dd/yyyy}");
                sb.AppendLine($"-ProducerName: {album.ProducerName}");
                sb.AppendLine($"-Songs:");

                int songNumber = 1;
                foreach (var song in album.Songs)
                {
                    sb.AppendLine($"---#{songNumber++}");
                    sb.AppendLine($"---SongName: {song.Name}");
                    sb.AppendLine($"---Price: {song.Price:f2}");
                    sb.AppendLine($"---Writer: {song.WriterName}");
                }

                sb.AppendLine($"-AlbumPrice: {album.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context.Songs
                .Where(x => x.Duration > TimeSpan.FromSeconds(duration))
                .Select(x => new
                {
                    x.Name,
                    PerformerName = x.SongPerformers
                        .Select(x => x.Performer.FirstName + " " + x.Performer.LastName)
                        .FirstOrDefault(),
                    WriterName = x.Writer.Name,
                    AlbumProducerName = x.Album.Producer.Name,
                    x.Duration
                })
                .OrderBy(x => x.Name)
                .ThenBy(x => x.WriterName)
                .ThenBy(x => x.PerformerName)
                .ToList();

            var sb = new StringBuilder();
            int songNumber = 1;
            foreach (var song in songs)
            {
                sb.AppendLine($"-Song #{songNumber++}");
                sb.AppendLine($"---SongName: {song.Name}");
                sb.AppendLine($"---Writer: {song.WriterName}");
                sb.AppendLine($"---Performer: {song.PerformerName}");
                sb.AppendLine($"---AlbumProducer: {song.AlbumProducerName}");
                sb.AppendLine($"---Duration: {song.Duration:c}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
