using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// HINWEIS: Mit dem Befehl "Umbenennen" im Menü "Umgestalten" können Sie den Klassennamen "IMDbService" sowohl im Code als auch in der SVC- und der Konfigurationsdatei ändern.
public class IMDbService : IIMDbService
{
    public Movie GetDetailByTitleCode(string url)
    {
        IMDb imdb = new IMDb(url);
        return imdb.ReadWebPage();
    }

    public string GetPosterBase64Data(string url)
    {
        IMDb imdb = new IMDb(url);
        string posterUrl = imdb.GetMoviePhotoUrl(url);

        if (!string.IsNullOrWhiteSpace(posterUrl))
        {
            return imdb.GetBase64PosterData(posterUrl);
        }

        return null;
    }

    public string GetPosterUrl(string url)
    {
        IMDb imdb = new IMDb(url);
        return imdb.GetMoviePhotoUrl(url);
    }
}
