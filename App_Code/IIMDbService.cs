using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Script.Services;

// HINWEIS: Mit dem Befehl "Umbenennen" im Menü "Umgestalten" können Sie den Schnittstellennamen "IIMDbService" sowohl im Code als auch in der Konfigurationsdatei ändern.
[ServiceContract]
public interface IIMDbService
{
    [OperationContract]
    [WebInvoke(Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "/GetDetailByTitleCode/?url={url}",
        BodyStyle = WebMessageBodyStyle.Bare)]
    Movie GetDetailByTitleCode(string url);

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
}