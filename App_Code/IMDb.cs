using System;
using System.Collections.Generic;
using System.IdentityModel;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.ServiceModel.Channels;
using System.Text;
using System.Web;

/// <summary>
/// Zusammenfassungsbeschreibung für IMDb
/// </summary>
public class IMDb
{
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
        string webPageUrl = string.Empty;
        string tempString = string.Empty;
        string director = string.Empty;
        string genre = string.Empty;
        string duration = string.Empty;
        string star = string.Empty;
        string rating = string.Empty;
        string rate = string.Empty;
        string posterUrl = string.Empty;
        string posterData = string.Empty;

        using (var client = new WebClient())
        {
            try
            {
                client.Encoding = Encoding.UTF8;
                webPageUrl = client.DownloadString(_url);
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

        if (string.IsNullOrWhiteSpace(webPageUrl))
        {
            return null;
        }

        tempString = webPageUrl;

        //Get Poster Url
        try
        {
            posterUrl = tempString.Substring(tempString.IndexOf(@"<div class=""poster"">") + tempString.Substring(tempString.IndexOf(@"<div class=""poster"">")).IndexOf("src=")).Remove(0, 5).Substring(0, tempString.Substring(tempString.IndexOf(@"<div class=""poster"">") + tempString.Substring(tempString.IndexOf(@"<div class=""poster"">")).IndexOf("src=")).Remove(0, 5).IndexOf(@""""));
        }
        catch (Exception)
        {
            posterUrl = "None";
        }

        //GetDirector(s) info
        index = tempString.IndexOf(@"Directors:");
            //tempString.IndexOf(@"<h4 class=""inline"">Director:</h4>");

        if (index > 0)
        {
            if (tempString.IndexOf(@"<h4 class=""inline"">Writer") > 0)
            {
                director = GetDirector(tempString.Substring(index, tempString.IndexOf(@"<h4 class=""inline"">Writer") - index));
            }
            else if (tempString.IndexOf(@"<h4 class=""inline"">Star") > 0)
            {
                director = GetDirector(tempString.Substring(index, tempString.IndexOf(@"<h4 class=""inline"">Star") - index));
            }
        }
        else
        {
            index = tempString.IndexOf(@"Director:");
            if (index > 0)
            {
                if (tempString.IndexOf(@"<h4 class=""inline"">Writer") > 0)
                {
                    director = GetDirector(tempString.Substring(index, tempString.IndexOf(@"<h4 class=""inline"">Writer") - index));
                }
                else if (tempString.IndexOf(@"<h4 class=""inline"">Star") > 0)
                {
                    director = GetDirector(tempString.Substring(index, tempString.IndexOf(@"<h4 class=""inline"">Star") - index));
                }
            }
        }

        //GetGenre(s) info
        index = 0;

        index = tempString.IndexOf(@"<span class=""itemprop"" itemprop=""genre"">");
        if (index > 0)
        {
            genre = GetGenre(tempString.Substring(index));
        }

        //GetDuration info
        index = 0;

        index = tempString.IndexOf(@"<h4 class=""inline"">Runtime:</h4>");
        if (index > 0)
        {
            duration = GetDuration(tempString.Substring(index));
        }
        else
        {
            if (tempString.IndexOf(@"<time itemprop=""duration"" datetime") > 0)
            {
                duration = ConvertDuration(tempString.Substring(tempString.IndexOf(@"<time itemprop=""duration"" datetime")));
            }
            else
            {
                duration = "0";
            }
        }

        //GetStar(s) info
        index = tempString.IndexOf(@"<h4 class=""inline"">Stars:</h4>");
        if (index > 0)
        {
            star = GetStars(tempString.Substring(index, tempString.Substring(index).IndexOf(@"<a href=""fullcredits")));
        }
        else if (tempString.IndexOf(@"<h4 class=""inline"">Star:</h4>") > 0)
        {
            index = tempString.IndexOf(@"<h4 class=""inline"">Star:</h4>");
            star = GetStar(tempString.Substring(index, tempString.Substring(index).IndexOf(@"</span>")));
        }

        //GetRating info
        index = 0;

        index = tempString.IndexOf(@"<span itemprop=""ratingValue"">");
        if (index > 0)
        {
            rating = GetRating(tempString.Substring(index));
        }

        //GetRate info
        index = 0;

        index = tempString.IndexOf(@"<meta itemprop=""contentRating"" content=""");
        if (index > 0)
        {
            rate = GetRate(tempString.Substring(index));
        }
        else
        {
            rate = "Not Rated";
        }

        posterData = GetBase64PosterData(posterUrl);

        Movie movie = new Movie()
        {
            Director = director,
            Genre = genre,
            Duration = duration,
            Star = star,
            Rating = rating,
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

    private string GetRate(string temp)
    {
        int index = 0;

        string rate = string.Empty;

        index = temp.IndexOf(@"<meta itemprop=""contentRating"" content=""");
        index += (@"<meta itemprop=""contentRating"" content=""").Length;

        return temp.Substring(index + temp.Substring(index).IndexOf(">") + 1, temp.Substring(index + temp.Substring(index).IndexOf(">") + 1).IndexOf("\n"));
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

    private string GetStar(string temp)
    {
        int index = 0;
        string star = string.Empty;

        index += temp.Substring(index).IndexOf(@"<span class=""itemprop"" itemprop=""name"">");
        index += (@"<span class=""itemprop"" itemprop=""name"">").Length;

        return temp.Substring(index);
    }

    private string GetStars(string temp)
    {
        int index = 0;
        int index2 = 0;
        string star = string.Empty;

        try
        {
            index += temp.Substring(index).IndexOf(@"?ref_=tt_ov_st_sm");

            while (index > 0)
            {
                index += (@"?ref_=tt_ov_st_sm").Length + 3;
                index2 = temp.Substring(index).IndexOf(@"</a>");
                star += ", " + temp.Substring(index, index2);

                temp = temp.Substring(index + index2);
                index = temp.IndexOf(@"?ref_=tt_ov_st_sm");
            }

            star = (star.Length > 2) ? star.Substring(2) : null;
        }
        catch (Exception ex)
        {
            star = string.Empty;
        }

        return star;
    }

    private string GetDuration(string temp)
    {
        int index = 0;
        int index2 = 0;
        string duration = string.Empty;

        index =  temp.IndexOf(@"<time datetime=""PT");
            //temp.IndexOf(@"<time itemprop=""duration"" datetime=");

        while (index > 0)
        {
            index = temp.IndexOf(@"<time datetime=""PT") + 18 + temp.Substring(temp.IndexOf(@"<time datetime=""PT") + 18).IndexOf("M\">") + 3;
            //index += (@"M"">").Length;
            index2 = temp.Substring(index).IndexOf(@"</time>");

            duration += " | " + temp.Substring(index, index2);
            temp = temp.Substring(temp.IndexOf(@"</time>") + (@"</time>").Length).Trim();

            if (temp.Substring(0, 6) != @"</div>" && temp.Substring(0, 1) == "(")
            {
                duration += " " + temp.Substring(0, temp.IndexOf("\n"));
            }

            index = temp.IndexOf(@"<time datetime=""PT");
        }

        if (duration.Length > 2)
        {
            return duration.Substring(2);
        }

        return duration;
    }

    private string GetGenre(string temp)
    {
        int index = 0;
        int index2 = 0;
        string genre = string.Empty;

        do
        {
            index += (@"<span class=""itemprop"" itemprop=""genre"">").Length;
            index2 = temp.Substring(index).IndexOf(@"</span>");
            genre += ", " + temp.Substring(index, index2);

            temp = temp.Substring(index + index2);
            index = temp.IndexOf(@"<span class=""itemprop"" itemprop=""genre"">");
        } while (index > 0);

        return genre.Substring(2);
    }

    private string GetDirector(string temp)
    {
        int index = 0;
        int index2 = 0;
        string director = string.Empty;

        index += temp.Substring(index).IndexOf(@"tt_ov_dr""");

        while (index > 0)
        {
            index += (@"tt_ov_dr").Length + 3;
            index2 = temp.Substring(index).IndexOf(@"</a>");
            director += ", " + temp.Substring(index, index2);

            temp = temp.Substring(index + index2);
            index = temp.IndexOf(@"tt_ov_dr""");
        }

        return director.Substring(2);
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