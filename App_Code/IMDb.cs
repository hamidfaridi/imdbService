using System;
using System.IdentityModel;
using System.Net;
using System.Text;

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
        string creator = string.Empty;
        string name = string.Empty;
        string titleType = string.Empty;
        string description = string.Empty;
        string genre = string.Empty;
        string duration = string.Empty;
        string durationDesc = string.Empty;
        string actor = string.Empty;
        string rating = string.Empty;
        string rate = string.Empty;
        string url = string.Empty;
        string posterUrl = string.Empty;
        string posterData = string.Empty;

        string runTime = string.Empty;

        using (var client = new WebClient())
        {
            try
            {
                client.Encoding = Encoding.UTF8;
                webPageContent = client.DownloadString(_url);
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("The remote server returned an error: (403) Forbidden.") && !ex.Message.Contains("Unable to connect to the remote server"))
                {
                    throw new BadRequestException(string.Format("{0}", ex.Message), ex.InnerException);
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

                runTime = webPageContent.Substring(webPageContent.ToUpper().IndexOf("RUNTIME:"),
                                   webPageContent.Substring(webPageContent.ToUpper().IndexOf("RUNTIME:")).ToUpper().IndexOf("</DIV>"));

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
            posterUrl = "Poster url couldn't found.";
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
                throw new Exception("Cannot retrieve Genre");
            }
        }

        //GetDatePublished
        index = 0;

        index = jsonData.IndexOf("\"datePublished\":");
        if (index > 0)
        {
            datePublished = jsonData.Substring(index + "\"datePublished\":".Length, jsonData.Substring(index + "\"datePublished\":".Length).IndexOf(",")).Replace("\"", "").Replace("\n", "").Trim();
        }

        //GetUrl
        index = 0;

        index = jsonData.IndexOf("\"url\":");
        if (index > 0)
        {
            name = jsonData.Substring(index + "\"url\":".Length,
                   jsonData.Substring(index + "\"url\":".Length).IndexOf(","))
                           .Replace("\"", "").Trim();
        }

        //GetName
        index = 0;

        index = jsonData.IndexOf("\"name\":");
        if (index > 0)
        {
            name = jsonData.Substring(index + "\"name\":".Length,
                   jsonData.Substring(index + "\"name\":".Length).IndexOf(","))
                           .Replace("\"", "").Replace("\n", "").Trim();
        }

        //GetDescription
        index = 0;

        index = jsonData.IndexOf("\"description\":");
        if (index > 0)
        {
            description = jsonData.Substring(index + "\"description\":".Length,
                          jsonData.Substring(index + "\"description\":".Length).IndexOf("\","))
                                  .Replace("\"", "").Trim();
        }

        //GetType
        index = 0;

        index = jsonData.IndexOf("\"@type\":");
        if (index > 0)
        {
            titleType = jsonData.Substring(index + "\"@type\":".Length,
                          jsonData.Substring(index + "\"@type\":".Length).IndexOf("\","))
                                  .Replace("\"", "").Trim();
        }

        //GetKeywords
        index = 0;

        index = jsonData.IndexOf("\"keywords\":");
        if (index > 0)
        {
            description = jsonData.Substring(index + "\"keywords\":".Length,
                          jsonData.Substring(index + "\"keywords\":".Length).IndexOf("\","))
                                  .Replace("\"", "").Trim();
        }

        //GetRate info
        index = 0;

        index = jsonData.IndexOf("\"contentRating\":");
        if (index > 0)
        {
            rate = jsonData.Substring(index + "\"contentRating\":".Length,
                   jsonData.Substring(index + "\"contentRating\":".Length).IndexOf(","))
                           .Replace("\"", "").Replace("\n", "").Trim();
        }
        else
        {
            rate = "Not Rated";
        }

        //GetActor(s) info
        index = jsonData.IndexOf("\"actor\":");
        if (index > 0)
        {
            actor = GetNameList(jsonData.Substring(jsonData.IndexOf("\"actor\":") + "\"actor\":".Length,
                             jsonData.Substring(jsonData.IndexOf("\"actor\":") + "\"actor\":".Length).IndexOf("]") + 1)
                                     .Replace("[", "").Replace("]", "").Replace("\"", "").Replace("\n", "").Trim());
        }

        //GetDirector(s) info
        index = jsonData.IndexOf("\"director\":");
        if (index > 0)
        {
            director = GetNameList(jsonData.Substring(jsonData.IndexOf("\"director\":") + "\"director\":".Length,
                                   jsonData.Substring(jsonData.IndexOf("\"director\":") + "\"director\":".Length).IndexOf("]") + 1)
                                           .Replace("[", "").Replace("]", "").Replace("\"", "").Replace("\n", "").Trim());
        }

        //GetCreator(s)
        index = jsonData.IndexOf("\"creator\":");
        if (index > 0)
        {
            creator = GetNameList(jsonData.Substring(jsonData.IndexOf("\"creator\":") + "\"creator\":".Length,
                                  jsonData.Substring(jsonData.IndexOf("\"creator\":") + "\"creator\":".Length).IndexOf("]") + 1)
                                          .Replace("[", "").Replace("]", "").Replace("\"", "").Replace("\n", "").Trim());
        }

        //GetDuration info
        index = 0;

        index = jsonData.IndexOf("\"duration\":");
        if (index > 0)
        {
            duration = jsonData.Substring(index + "\"duration\":".Length,
                          jsonData.Substring(index + "\"duration\":".Length).IndexOf("\","))
                                  .Replace("\"", "").Trim();
            durationDesc = GetDuration(duration);
        }

        //runTime

        //GetRating info
        index = 0;

        index = tempString.IndexOf(@"<span itemprop=""ratingValue"">");
        if (index > 0)
        {
            rating = GetRating(tempString.Substring(index));
        }

        posterData = GetBase64PosterData(posterUrl);

        Movie movie = new Movie()
        {
            Name = name,
            Actor = actor,
            Creator = creator,
            DatePublished = datePublished,
            Description = description,
            Director = director,
            Duration = duration,
            DurationDesc = durationDesc,
            Genre = genre,
            AggregateRating = "",
            RatingValue = rating,
            TitleType = titleType,
            Url = url,
            BestRating = "",
            WorstRating = "",
            Rate = rate,
            PosterUrl = posterUrl
        };

        return movie;
    }

    public string GetMoviePhotoUrl(string url)
    {
        string photoUrl = string.Empty;

        try
        {
            string processedString = new WebClient().DownloadString(url);

            photoUrl = processedString.Substring(processedString.IndexOf(@"<div class=""poster"">")
                + processedString.Substring(processedString.IndexOf(@"<div class=""poster"">")).IndexOf("src="))
                .Remove(0, 5).Substring(0, processedString.Substring(processedString.IndexOf(@"<div class=""poster"">")
                + processedString.Substring(processedString.IndexOf(@"<div class=""poster"">"))
                .IndexOf("src=")).Remove(0, 5).IndexOf(@""""));

        }
        catch (Exception)
        {
            photoUrl = null;
        }

        return photoUrl;
    }

    private string ConvertDuration(string temp)
    {
        int intHour = 0;
        int intMin = 0;
        string strHour = "";
        string strMin = "";
        string strDuration = "";
        try
        {
            strDuration = temp.Substring(temp.IndexOf(@"M"">") + (@"M"">").Length).Trim().Substring(0, temp.Substring(temp.IndexOf(@"M"">") + (@"M"">").Length).Trim().IndexOf("\n"));
        }
        catch (Exception)
        {
        }

        if (strDuration.IndexOf("h") > 0)
        {
            strHour = strDuration.Substring(0, strDuration.IndexOf("h"));
        }

        if (strDuration.IndexOf("min") > 3 && (strDuration.IndexOf("h") > 0))
        {
            strMin = strDuration.Substring(strDuration.IndexOf("h") + 2, strDuration.IndexOf("min") - 3);
        }
        else if (strDuration.IndexOf("min") > 3 && (strDuration.IndexOf("h") == -1))
        {
            strMin = strDuration.Substring(0, strDuration.IndexOf("min") - 3);
        }

        int.TryParse(strHour, out intHour);
        int.TryParse(strMin, out intMin);

        return (intHour * 60 + intMin).ToString() + " min";
    }

    private string GetRating(string temp)
    {
        int index = 0;
        int index2 = 0;

        string rate = string.Empty;

        index = temp.IndexOf(@"<span itemprop=""ratingValue"">");
        index += (@"<span itemprop=""ratingValue"">").Length;

        if (int.TryParse(temp.Substring(index, 3).Replace(".", ""), out index2))
        {
            return temp.Substring(index, 3);
        }
        else return "0";
    }

    private string GetNameList(string temp)
    {
        int index = 0;
        int index2 = 0;
        string name = string.Empty;

        try
        {
            index += temp.IndexOf("name:") + "name:".Length;

            while (index > "name:".Length)
            {
                index2 = temp.Substring(temp.IndexOf("name:") + "name:".Length).IndexOf("}") - 1;
                name += string.Format(", {0}", temp.Substring(index, index2).Trim());
                temp = temp.Substring(index + index2);
                if (temp.Length > "name:".Length)
                {
                    index = temp.IndexOf("name:") + "name:".Length;
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
            name = string.Empty;
        }

        return name;
    }

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
            }
        }

        return duration;
    }

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

    public string GetBase64PosterData(string url)
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
            throw new BadRequestException(string.Format("Error occured while trying to download poster data."), ex.InnerException);
        }

        return posterData;
    }
}