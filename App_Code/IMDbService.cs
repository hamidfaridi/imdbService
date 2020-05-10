public class IMDbService : IIMDbService
{
    /// <summary>
    /// Gets movie data by entering full imdb url
    /// </summary>
    /// <param name="url">full Url path
    /// for Example: "https://www.imdb.com/title/tt1371111/"</param>
    /// <returns>Return Movie Class in json data format</returns>
    public Movie GetDetailByUrl(string url)
    {
        IMDb imdb = new IMDb(url);
        return imdb.ReadWebPage();
    }

    /// <summary>
    /// Get movie by title value
    /// </summary>
    /// <param name="title">IMDb Title value
    /// for Example: "tt1371111" or "1371111" in located in url "https://www.imdb.com/title/tt1371111/"</param>
    /// <returns>Return Movie Class in json data format</returns>
    public Movie GetDetailByTitle(string title)
    {
        int titleNo = 0;
        string url = string.Empty;

        if (title.ToLower().Replace("\\", "").Replace("/", "").Replace(" ", "").Contains("tt")
            && title.ToLower().Replace("\\", "").Replace("/", "").Replace(" ", "").IndexOf("tt") == 0)
        {
            if (int.TryParse(title.ToLower().Replace("\\", "").Replace("/", "").Replace(" ", "").Substring(2), out titleNo))
            {
                url = string.Format("https://www.imdb.com/title/tt{0}", titleNo);
            }
        }
        else
        {
            if (int.TryParse(title.ToLower().Replace("\\", "").Replace("/", "").Replace(" ", ""), out titleNo))
            {
                url = string.Format("https://www.imdb.com/title/tt{0}", titleNo);
            }
        }

        if (!string.IsNullOrWhiteSpace(url))
        {
            IMDb imdb = new IMDb(url);
            return imdb.ReadWebPage();
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Get poster data in Base64 data format
    /// </summary>
    /// <param name="url">full Url path
    /// for Example: "https://www.imdb.com/title/tt1371111/"</param>
    /// <returns>Base64 data in json data format</returns>
    public string GetBase64PosterData(string url)
    {
        IMDb imdb = new IMDb(url);
        string posterUrl = imdb.GetMoviePosterUrl(url);

        if (!string.IsNullOrWhiteSpace(posterUrl))
        {
            return imdb.GetBase64Data(posterUrl);
        }

        return null;
    }

    /// <summary>
    /// Get thumbnail data in Base64 data format
    /// </summary>
    /// <param name="url">full Url path
    /// for Example: "https://www.imdb.com/title/tt1371111/"</param>
    /// <returns>Base64 data in json data format</returns>
    public string GetBase64ThumbnailData(string url)
    {
        IMDb imdb = new IMDb(url);
        string posterUrl = imdb.GetMoviePosterThumbnailUrl(url);

        if (!string.IsNullOrWhiteSpace(posterUrl))
        {
            return imdb.GetBase64Data(posterUrl);
        }

        return null;
    }

    /// <summary>
    /// Get poster url from requested IMDb title url
    /// </summary>
    /// <param name="url">full Url path
    /// for Example: "https://www.imdb.com/title/tt1371111/"</param>
    /// <returns>returns full photo url in json data format</returns>
    public string GetPosterUrl(string url)
    {
        IMDb imdb = new IMDb(url);
        return imdb.GetMoviePosterUrl(url);
    }

    /// <summary>
    /// Get thumbnail url from requested IMDb title url
    /// </summary>
    /// <param name="url">full Url path
    /// for Example: "https://www.imdb.com/title/tt1371111/"</param>
    /// <returns>returns full photo url in json data format</returns>
    public string GetThumbnailUrl(string url)
    {
        IMDb imdb = new IMDb(url);
        return imdb.GetMoviePosterThumbnailUrl(url);
    }

    /// <summary>
    /// Gets movie data by entering full imdb url
    /// </summary>
    /// <param name="url">full Url path
    /// for Example: "https://www.imdb.com/title/tt1371111/"</param>
    /// <returns>Return Movie Class in XML data format</returns>
    public Movie GetDetailByUrlXML(string url)
    {
        return GetDetailByUrl(url);
    }

    /// <summary>
    /// Get movie by title value
    /// </summary>
    /// <param name="title">IMDb Title value
    /// for Example: "tt1371111" or "1371111" in located in url "https://www.imdb.com/title/tt1371111/"</param>
    /// <returns>Return Movie Class in XML data format</returns>
    public Movie GetDetailByTitleXML(string title)
    {
        return GetDetailByTitle(title);
    }

    /// <summary>
    /// Get poster data in Base64 data format
    /// </summary>
    /// <param name="url">full Url path
    /// for Example: "https://www.imdb.com/title/tt1371111/"</param>
    /// <returns>Base64 data in XML data format</returns>
    public string GetPosterUrlXML(string url)
    {
        return GetPosterUrl(url);
    }

    /// <summary>
    /// Get thumbnail url from requested IMDb title url
    /// </summary>
    /// <param name="url">full Url path
    /// for Example: "https://www.imdb.com/title/tt1371111/"</param>
    /// <returns>returns full photo url in XML data format</returns>
    public string GetThumbnailUrlXML(string url)
    {
        return GetThumbnailUrl(url);
    }

    /// <summary>
    /// Get poster url from requested IMDb title url
    /// </summary>
    /// <param name="url">full Url path
    /// for Example: "https://www.imdb.com/title/tt1371111/"</param>
    /// <returns>returns full photo url in XML data format</returns>
    public string GetBase64PosterDataXML(string url)
    {
        return GetBase64PosterData(url);
    }

    /// <summary>
    /// Get thumbnail data in Base64 data format
    /// </summary>
    /// <param name="url">full Url path
    /// for Example: "https://www.imdb.com/title/tt1371111/"</param>
    /// <returns>Base64 data in XML data format</returns>
    public string GetBase64ThumbnailDataXML(string url)
    {
        return GetBase64ThumbnailData(url);
    }
}
