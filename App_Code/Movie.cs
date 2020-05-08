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
    public string ResolutionDesc;
    public int Year;
    public int PersianDate;
    public int Date;
    public byte Disk;
    public string Format;
    public string Genre;
    public long SizeOnDisk;
    public string Director;
    public string Rating;
    public string Rate;
    public string Duration;
    public string Durations;
    public string Star;
    public string Link;
    public string PosterData;
    public string PosterUrl { get; set; }
    public byte[] Cover;
}