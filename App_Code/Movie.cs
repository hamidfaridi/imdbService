using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;

/// <summary>
/// Zusammenfassungsbeschreibung für Movie
/// </summary>
public class Movie
{
    public int No;
    public string Name;
    public string TitleType;
    public string Url;
    public string ResolutionDesc;
    public int Year;
    public int PersianDate;
    public int Date;
    public byte Disk;
    public string Format;
    public string Genre;
    public long SizeOnDisk;
    public string Creator;
    public string Description;
    public string Director;
    
    /// <summary>
    /// Rating
    /// </summary>
    public string AggregateRating;
    public string BestRating;
    public string WorstRating;
    public string RatingValue;
    //////////////
    
    public string Rate;
    public string Keywords;
    public string DatePublished;
    public string Duration;
    public string DurationDesc;
    public string Durations;
    public string Actor;
    public string Link;
    public string PosterData;
    public string PosterUrl { get; set; }
    public byte[] Cover;
}