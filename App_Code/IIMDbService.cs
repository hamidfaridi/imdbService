﻿using System.ServiceModel;
using System.ServiceModel.Web;

[ServiceContract]
public interface IIMDbService
{
    #region json Data format

    [OperationContract]
    [WebInvoke(Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "/GetDetailByUrl/?url={url}",
        BodyStyle = WebMessageBodyStyle.Bare)]
    Movie GetDetailByUrl(string url);

    [OperationContract]
    [WebInvoke(Method = "GET",
    RequestFormat = WebMessageFormat.Json,
    ResponseFormat = WebMessageFormat.Json,
    UriTemplate = "/GetDetailByTitle/?title={title}",
    BodyStyle = WebMessageBodyStyle.Bare)]
    Movie GetDetailByTitle(string title);

    [OperationContract]
    [WebInvoke(Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "/GetPosterUrl/?url={url}",
        BodyStyle = WebMessageBodyStyle.Bare)]
    string GetPosterUrl(string url);

    [OperationContract]
    [WebInvoke(Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "/GetBase64PosterData/?url={url}",
        BodyStyle = WebMessageBodyStyle.Bare)]
    string GetPosterBase64Data(string url);

    #endregion

    #region XML Data format

    [OperationContract]
    [WebInvoke(Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Xml,
        UriTemplate = "/GetDetailByUrlXML/?url={url}",
        BodyStyle = WebMessageBodyStyle.Bare)]
    Movie GetDetailByUrlXML(string url);

    [OperationContract]
    [WebInvoke(Method = "GET",
    RequestFormat = WebMessageFormat.Json,
    ResponseFormat = WebMessageFormat.Xml,
    UriTemplate = "/GetDetailByTitleXML/?title={title}",
    BodyStyle = WebMessageBodyStyle.Bare)]
    Movie GetDetailByTitleXML(string title);

    [OperationContract]
    [WebInvoke(Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Xml,
        UriTemplate = "/GetPosterUrlXML/?url={url}",
        BodyStyle = WebMessageBodyStyle.Bare)]
    string GetPosterUrlXML(string url);

    [OperationContract]
    [WebInvoke(Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Xml,
        UriTemplate = "/GetBase64PosterDataXML/?url={url}",
        BodyStyle = WebMessageBodyStyle.Bare)]
    string GetPosterBase64DataXML(string url);

    #endregion
}