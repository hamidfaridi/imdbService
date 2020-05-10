using System;
using System.IdentityModel;
using System.IO;
using System.Net;
using System.Text;
using System.Web.UI;
using System.Xml;

/// <summary>
/// Zusammenfassungsbeschreibung für IMDb
/// </summary>
public class IMDb
{
    string jsonData = string.Empty;

    private string _url = string.Empty;

    /// <summary>
    /// Default Constrauctor for IMDB class
    /// </summary>
    /// <param name="url">An IMDB title url</param>
    public IMDb(string url)
    {
        _url = url;
    }

    public Movie ReadWebPage()
    {
        if (string.IsNullOrWhiteSpace(_url))
        {
            return null;
        }

        int index = 0;
        string webPageContent = string.Empty;
        string tempString = string.Empty;
        string datePublished = string.Empty;
        string director = string.Empty;
        string writers = string.Empty;
        string name = string.Empty;
        string titleType = string.Empty;
        string description = string.Empty;
        string genre = string.Empty;
        string duration = string.Empty;
        string durations = string.Empty;
        string durationDesc = string.Empty;
        string actor = string.Empty;
        string keywords = string.Empty;
        string aggregateRating = string.Empty;
        string ratingCount = string.Empty;
        string bestRating = string.Empty;
        string worstRating = string.Empty;
        string ratingValue = string.Empty;

        string rate = string.Empty;
        string url = string.Empty;
        string posterUrl = string.Empty;
        string posterThumbnail = string.Empty;
        string posterData = string.Empty;

        string runTime = string.Empty;

        using (var client = new WebClient())
        {
            try
            {
                client.Encoding = Encoding.UTF8;
                webPageContent = client.DownloadString(_url);

                //GetPosterThumbnail
                posterThumbnail = webPageContent.Substring(webPageContent.IndexOf(@"<div class=""poster"">")
                + webPageContent.Substring(webPageContent.IndexOf(@"<div class=""poster"">")).IndexOf("src="))
                .Remove(0, 5).Substring(0, webPageContent.Substring(webPageContent.IndexOf(@"<div class=""poster"">")
                + webPageContent.Substring(webPageContent.IndexOf(@"<div class=""poster"">"))
                .IndexOf("src=")).Remove(0, 5).IndexOf(@""""));
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("The remote server returned an error: (403) Forbidden.") && !ex.Message.Contains("Unable to connect to the remote server"))
                {
                    //throw new BadRequestException(string.Format("{0}", ex.Message), ex.InnerException);
                }
                return null;
            }
        }

        if (string.IsNullOrWhiteSpace(webPageContent))
        {
            return null;
        }
        else
        {
            if (webPageContent.IndexOf("<script type=\"application/ld+json\">") > 0)
            {
                jsonData =
                    webPageContent.Substring(webPageContent.IndexOf("<script type=\"application/ld+json\">")
                                               + "<script type=\"application/ld+json\">".Length,
                                             webPageContent.Substring(webPageContent.IndexOf("<script type=\"application/ld+json\">")
                                               + "<script type=\"application/ld+json\">".Length).IndexOf("</script"));

                runTime = webPageContent.Substring(webPageContent.ToLower().IndexOf("runtime:")).Substring(webPageContent.Substring(webPageContent.ToLower().IndexOf("runtime:")).ToLower().IndexOf("<time datetime="));
                runTime = "<div>" + runTime.Substring(0, runTime.IndexOf("</div>")) + "</div>";
                webPageContent = string.Empty;
            }
        }

        if (string.IsNullOrWhiteSpace(jsonData))
        {
            return null;
        }

        //Get Poster Url
        try
        {
            posterUrl = jsonData.Substring(jsonData.IndexOf("\"image\":") + "\"image\":".Length, jsonData.Substring(jsonData.IndexOf("\"image\":") + "\"image\":".Length).IndexOf("\",")).Replace("\"", "").Trim();
        }
        catch (Exception ex)
        {
            posterUrl = "Poster url couldn't found. (Error Occured))";
        }

        //GetGenre(s) info
        index = 0;

        index = jsonData.IndexOf("\"genre\":");

        if (index > 0)
        {
            try
            {
                genre = GetList(jsonData.Substring(jsonData.IndexOf("\"genre\":") + "\"genre\":".Length, jsonData.Substring(jsonData.IndexOf("\"genre\":") + "\"genre\":".Length).IndexOf("]") + 1).Replace("[", "").Replace("]", "").Replace("\"", "").Replace("\n", "").Trim());
            }
            catch (Exception ex)
            {
                throw new Exception("Genre data couldn't found. (Error Occured))");
            }
        }

        //GetDatePublished
        index = 0;

        index = jsonData.IndexOf("\"datePublished\":");
        if (index > 0)
        {
            try
            {
                datePublished = jsonData.Substring(index + "\"datePublished\":".Length, jsonData.Substring(index + "\"datePublished\":".Length).IndexOf(",")).Replace("\"", "").Replace("\n", "").Trim();
            }
            catch (Exception ex)
            {
                throw new Exception("Genre data couldn't found. (Error Occured))");
            }
        }

        //GetUrl
        index = 0;

        index = jsonData.IndexOf("\"url\":");
        if (index > 0)
        {
            try
            {
                url = jsonData.Substring(index + "\"url\":".Length,
                   jsonData.Substring(index + "\"url\":".Length).IndexOf(","))
                           .Replace("\"", "").Trim();
            }
            catch (Exception ex)
            {
                throw new Exception("Url data couldn't found. (Error Occured))");
            }
        }

        //GetName
        index = 0;

        index = jsonData.IndexOf("\"name\":");
        if (index > 0)
        {
            try
            {
                name = jsonData.Substring(index + "\"name\":".Length,
                       jsonData.Substring(index + "\"name\":".Length).IndexOf(","))
                               .Replace("\"", "").Replace("\n", "").Trim();
            }
            catch (Exception ex)
            {
                throw new Exception("Name data couldn't found. (Error Occured))");
            }
        }

        //GetDescription
        index = 0;

        index = jsonData.IndexOf("\"description\":");
        if (index > 0)
        {
            try
            {
                description = jsonData.Substring(index + "\"description\":".Length,
                          jsonData.Substring(index + "\"description\":".Length).IndexOf("\","))
                                  .Replace("\"", "").Trim();
            }
            catch (Exception ex)
            {
                throw new Exception("Description data couldn't found. (Error Occured))");
            }
        }

        //GetType
        index = 0;

        index = jsonData.IndexOf("\"@type\":");
        if (index > 0)
        {
            try
            {
                titleType = jsonData.Substring(index + "\"@type\":".Length,
                                              jsonData.Substring(index + "\"@type\":".Length).IndexOf("\","))
                                                      .Replace("\"", "").Trim();
            }
            catch (Exception ex)
            {
                throw new Exception("TitleType data couldn't found. (Error Occured))");
            }
        }

        //GetKeywords
        index = 0;

        index = jsonData.IndexOf("\"keywords\":");
        if (index > 0)
        {
            try
            {
                keywords = jsonData.Substring(index + "\"keywords\":".Length,
                           jsonData.Substring(index + "\"keywords\":".Length).IndexOf("\","))
                                   .Replace("\"", "").Trim();
            }
            catch (Exception ex)
            {
                throw new Exception("Keywords data couldn't found. (Error Occured))");
            }
        }

        //GetRate info
        index = 0;

        index = jsonData.IndexOf("\"contentRating\":");
        if (index > 0)
        {
            try
            {
                rate = jsonData.Substring(index + "\"contentRating\":".Length,
                       jsonData.Substring(index + "\"contentRating\":".Length).IndexOf(","))
                               .Replace("\"", "").Replace("\n", "").Trim();
            }
            catch (Exception ex)
            {
                throw new Exception("Rate data couldn't found. (Error Occured))");
            }
        }
        else
        {
            rate = "Not Rated";
        }

        //GetActor(s) info
        index = jsonData.IndexOf("\"actor\":");
        if (index > 0)
        {
            try
            {
                actor = GetNameList(jsonData.Substring(jsonData.IndexOf("\"actor\":") + "\"actor\":".Length,
                                 jsonData.Substring(jsonData.IndexOf("\"actor\":") + "\"actor\":".Length).IndexOf("]") + 1)
                                         .Replace("[", "").Replace("]", "").Replace("\"", "").Replace("\n", "").Trim());
            }
            catch (Exception ex)
            {
                throw new Exception("Actor data couldn't found. (Error Occured))");
            }
        }

        //GetDirector(s) info
        index = jsonData.IndexOf("\"director\":");
        if (index > 0)
        {
            try
            {
                director = GetNameList(jsonData.Substring(jsonData.IndexOf("\"director\":") + "\"director\":".Length,
                                                   jsonData.Substring(jsonData.IndexOf("\"director\":") + "\"director\":".Length).IndexOf("]") + 1)
                                                           .Replace("[", "").Replace("]", "").Replace("\"", "").Replace("\n", "").Trim());
            }
            catch (Exception ex)
            {
                throw new Exception("Director data couldn't found. (Error Occured))");
            }
        }

        //GetCreator(s)
        index = jsonData.IndexOf("\"creator\":");
        if (index > 0)
        {
            try
            {
                writers = GetNameList(jsonData.Substring(jsonData.IndexOf("\"creator\":") + "\"creator\":".Length,
                                      jsonData.Substring(jsonData.IndexOf("\"creator\":") + "\"creator\":".Length).IndexOf("]") + 1)
                                              .Replace("[", "").Replace("]", "").Replace("\"", "").Replace("\n", "").Trim());
            }
            catch (Exception ex)
            {
                throw new Exception("Creator data couldn't found. (Error Occured))");
            }
        }

        //GetDuration info
        index = 0;

        index = jsonData.IndexOf("\"duration\":");
        if (index > 0)
        {
            try
            {
                duration = jsonData.Substring(index + "\"duration\":".Length,
                              jsonData.Substring(index + "\"duration\":".Length).IndexOf("\","))
                                      .Replace("\"", "").Trim();
                durationDesc = GetDuration(duration);
            }
            catch (Exception ex)
            {
                throw new Exception("Duration data couldn't found. (Error Occured))");
            }
        }

        //Calculate Durations
        if (!string.IsNullOrWhiteSpace(runTime))
        {
            durations = GetRuntime(runTime);
        }

        //GetRating info
        index = 0;
        index = jsonData.IndexOf("\"aggregateRating\":");
        if (index > 0)
        {
            try
            {
                aggregateRating = jsonData.Substring(index, jsonData.Substring(index).IndexOf("},")).Trim();

                try
                {
                    ratingCount = aggregateRating.Substring(aggregateRating.ToLower().IndexOf("ratingcount\":") + "ratingcount\":".Length, aggregateRating.Substring(aggregateRating.ToLower().IndexOf("ratingcount\":") + "ratingcount\":".Length).IndexOf(",")).Replace("\"", "").Trim();
                }
                catch (Exception ex)
                {
                    ratingCount = "RatingCount data couldn't found. (Error Occured))";
                }

                try
                {
                    bestRating = aggregateRating.Substring(aggregateRating.ToLower().IndexOf("bestrating\":") + "bestrating\":".Length, aggregateRating.Substring(aggregateRating.ToLower().IndexOf("bestrating\":") + "bestrating\":".Length).IndexOf(",")).Replace("\"", "").Trim();
                }
                catch (Exception ex)
                {
                    bestRating = "BestRating data couldn't found. (Error Occured))";
                }

                try
                {
                    worstRating = aggregateRating.Substring(aggregateRating.ToLower().IndexOf("worstrating\":") + "worstrating\":".Length, aggregateRating.Substring(aggregateRating.ToLower().IndexOf("worstrating\":") + "worstrating\":".Length).IndexOf(",")).Replace("\"", "").Trim();
                }
                catch (Exception ex)
                {
                    worstRating = "WorstRating data couldn't found. (Error Occured))";
                }

                try
                {
                    ratingValue = aggregateRating.Substring(aggregateRating.ToLower().IndexOf("ratingvalue\":") + "ratingvalue\":".Length).Replace("\"", "").Trim();
                }
                catch (Exception ex)
                {
                    ratingValue = "RatingValue data couldn't found. (Error Occured))";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Rating data couldn't found. (Error Occured))");
            }
        }

        try
        {
            posterData = string.Format("You can user for getting poster data in Base64format http://imdbservice.hamidfaridi.com/IMDbService.svc/GetBase64PosterData/?url={0}", _url);
            //GetBase64PosterData(posterUrl);
        }
        catch (Exception ex)
        {
            throw new Exception("Poster data couldn't found. (Error Occured))");
        }

        Movie movie = new Movie()
        {
            Url = _url,
            TitleType = titleType,
            Name = name,
            Actor = actor,
            Writer = writers,
            DatePublished = datePublished,
            Description = description,
            Director = director,
            Genre = genre,
            Duration = duration,
            DurationDesc = durationDesc,
            Durations = durations,
            Keywords = keywords,
            Rate = rate,
            RatingCount = ratingCount,
            RatingValue = ratingValue,
            BestRating = bestRating,
            WorstRating = worstRating,
            PosterUrl = posterUrl,
            PosterThumbnail = posterThumbnail,
            PosterData = posterData
        };

        return movie;
    }

    /// <summary>
    /// Gets runtime(s)
    /// </summary>
    /// <param name="runTime">Runtime text block</param>
    /// <returns>Runtime(s) devided by '|'</returns>
    private string GetRuntime(string runTime)
    {
        string formattedDuration = string.Empty;
        XmlDocument document = new XmlDocument();

        try
        {
            document.LoadXml(runTime.Replace("\n", ""));

            try
            {
                for (int i = 0; i < document.InnerText.Split('|').Length; i++)
                {
                    formattedDuration += " |";

                    for (int j = 0; j < document.InnerText.Split('|')[i].Split(' ').Length; j++)
                    {
                        if (!string.IsNullOrWhiteSpace(document.InnerText.Split('|')[i].Split(' ')[j].Trim()))
                        {
                            formattedDuration += " " + document.InnerText.Split('|')[i].Split(' ')[j].Trim();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot format durations.");
            }

            if (formattedDuration.Length > 2)
            {
                return formattedDuration.Substring(3);
            }
            else
            {
                return formattedDuration;
            }
        }
        catch (Exception ex)
        {
            return "Can not format Durations. (Error occured)";
        }
    }

    /// <summary>
    /// Gets Poster thumbnail file if available
    /// </summary>
    /// <param name="url">Imdb full path url</param>
    /// <returns>Full thumbnail url</returns>
    public string GetMoviePosterThumbnailUrl(string url)
    {
        string thumbnailUrl = string.Empty;

        try
        {
            string processedString = new WebClient().DownloadString(url);

            thumbnailUrl = processedString.Substring(processedString.IndexOf(@"<div class=""poster"">")
                + processedString.Substring(processedString.IndexOf(@"<div class=""poster"">")).IndexOf("src="))
                .Remove(0, 5).Substring(0, processedString.Substring(processedString.IndexOf(@"<div class=""poster"">")
                + processedString.Substring(processedString.IndexOf(@"<div class=""poster"">"))
                .IndexOf("src=")).Remove(0, 5).IndexOf(@""""));
        }
        catch (Exception)
        {
            thumbnailUrl = "Cannot get PhotoUrl (Error occured)";
        }

        return thumbnailUrl;
    }

    /// <summary>
    /// Gets Poster large file if available
    /// </summary>
    /// <param name="url">Imdb full path url</param>
    /// <returns>Full poster url</returns>
    public string GetMoviePosterUrl(string url)
    {
        string posterUrl = string.Empty;

        try
        {
            string processedString = new WebClient().DownloadString(url);
            posterUrl = processedString.Substring(processedString.IndexOf("\"image\":") + "\"image\":".Length,
                processedString.Substring(processedString.IndexOf("\"image\":") + "\"image\":".Length).IndexOf("\",")).Replace("\"", "").Trim();
        }
        catch (Exception ex)
        {
            return "Poster url couldn't found. (Error Occured))";
        }

        return posterUrl;
    }

    /// <summary>
    /// Gets name(s) from listed names format
    /// </summary>
    /// <param name="nameList">Name list</param>
    /// <returns>name string devided by comma</returns>
    private string GetNameList(string nameList)
    {
        int index = 0;
        int index2 = 0;
        string name = string.Empty;

        try
        {
            index += nameList.IndexOf("name:") + "name:".Length;

            while (index > "name:".Length)
            {
                index2 = nameList.Substring(nameList.IndexOf("name:") + "name:".Length).IndexOf("}") - 1;
                name += string.Format(", {0}", nameList.Substring(index, index2).Trim());
                nameList = nameList.Substring(index + index2);
                if (nameList.Length > "name:".Length)
                {
                    index = nameList.IndexOf("name:") + "name:".Length;
                }
                else
                {
                    index = 0;
                }
            }

            name = (name.Length > 2) ? name.Substring(2) : null;
        }
        catch (Exception ex)
        {
            name = "Cannot get Actor name(s). (Error Occured)";
        }

        return name;
    }

    /// <summary>
    /// Gets coded format of duration
    /// </summary>
    /// <param name="formattedDuration">Coded Duration</param>
    /// <returns>Duration in minutes</returns>
    private string GetDuration(string formattedDuration)
    {
        string duration = string.Empty;
        if (formattedDuration.Contains("PT"))
        {
            int hour = 0;
            int minute = 0;

            try
            {
                if (int.TryParse(formattedDuration.ToUpper().Replace("PT", "").Split('H')[0], out hour))
                    if (int.TryParse(formattedDuration.ToUpper().Replace("PT", "").Split('H')[1].Split('M')[0], out minute))
                        duration = string.Format("{0} minutes", hour * 60 + minute);
            }
            catch (Exception ex)
            {
                duration = "Cannot get Durationdata. (Error occured)";
            }
        }

        return duration;
    }

    /// <summary>
    /// Gets text list with comma seperated
    /// </summary>
    /// <param name="listString">List text</param>
    /// <returns>List's names string seperated by comma</returns>
    private string GetList(string listString)
    {
        string list = "";

        for (int i = 0; i < listString.Split(',').Length; i++)
        {
            list += ", " + listString.Split(',')[i].Trim();
        }

        if (list.Length > 2)
        {
            return list.Substring(2);
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Downloads given photo url
    /// </summary>
    /// <param name="url">Photo url</param>
    /// <returns>Base64 data</returns>
    public string GetBase64Data(string url)
    {
        string posterData = string.Empty;

        try
        {
            byte[] posterDownloadedData = new WebClient().DownloadData(url);
            if (posterDownloadedData != null && posterDownloadedData.Length > 0)
            {
                posterData = Convert.ToBase64String(posterDownloadedData);
            }
        }
        catch (Exception ex)
        {
            return "The requested url not found!";
        }

        return posterData;
    }
}