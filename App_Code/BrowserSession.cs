﻿using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web;
using HtmlAgilityPack;

public class BrowserSession
{
    private bool _isPost;
    private HtmlDocument _htmlDoc;
    private HttpWebResponse _response;
    private CookieCollection _cookies;
    private FormElementCollection _formElements;

    /// <summary>
    /// System.Net.CookieCollection. Provides a collection container for instances of Cookie class 
    /// </summary>
    public CookieCollection Cookies
    {
        get { return _cookies; }
        set { _cookies = value; }
    }

    /// <summary>
    /// Provide a key-value-pair collection of form elements 
    /// </summary>
    public FormElementCollection FormElements
    {
        get { return _formElements; }
        set { _formElements = value; }
    }

    /// <summary>
    /// Makes a HTTP GET request to the given URL
    /// </summary>
    public string Get(string url)
    {
        _isPost = false;
        CreateWebRequestObject().Load(url);
        return _htmlDoc.DocumentNode.InnerHtml;
    }

    /// <summary>
    /// Makes a HTTP POST request to the given URL
    /// </summary>
    public string Post(string url)
    {
        _isPost = true;
        CreateWebRequestObject().Load(url, "POST");
        _isPost = false;
        return _htmlDoc.DocumentNode.InnerHtml;
    }



    /// <summary>
    /// Creates the HtmlWeb object and initializes all event handlers. 
    /// </summary>
    public HtmlWeb CreateWebRequestObject()
    {
        HtmlWeb web = new HtmlWeb();
        web.UseCookies = true;
        web.PreRequest = new HtmlWeb.PreRequestHandler(OnPreRequest);
        web.PostResponse = new HtmlWeb.PostResponseHandler(OnAfterResponse);
        web.PreHandleDocument = new HtmlWeb.PreHandleDocumentHandler(OnPreHandleDocument);
        return web;
    }

    /// <summary>
    /// Event handler for HtmlWeb.PreRequestHandler. Occurs before an HTTP request is executed.
    /// </summary>
    protected bool OnPreRequest(HttpWebRequest request)
    {
        AddCookiesTo(request);               // Add cookies that were saved from previous requests
        if (_isPost) AddPostDataTo(request); // We only need to add post data on a POST request
        return true;
    }

    /// <summary>
    /// Event handler for HtmlWeb.PreRequestHandler. Occurs before an HTTP request is executed.
    /// </summary>
    public bool UpdateRequest(HttpWebRequest request)
    {
        AddCookiesTo(request);               // Add cookies that were saved from previous requests
        if (_isPost) AddPostDataTo(request); // We only need to add post data on a POST request
        return true;
    }

    /// <summary>
    /// Event handler for HtmlWeb.PostResponseHandler. Occurs after a HTTP response is received
    /// </summary>
    protected void OnAfterResponse(HttpWebRequest request, HttpWebResponse response)
    {
        SaveCookiesFrom(response); // Save cookies for subsequent requests
    }

    /// <summary>
    /// Event handler for HtmlWeb.PreHandleDocumentHandler. Occurs before a HTML document is handled
    /// </summary>
    protected void OnPreHandleDocument(HtmlDocument document)
    {
        SaveHtmlDocument(document);
    }

    /// <summary>
    /// Assembles the Post data and attaches to the request object
    /// </summary>
    private void AddPostDataTo(HttpWebRequest request)
    {
        string payload = FormElements.AssemblePostPayload();
        byte[] buff = Encoding.UTF8.GetBytes(payload.ToCharArray());
        request.ContentLength = buff.Length;
        request.ContentType = "application/x-www-form-urlencoded";
        System.IO.Stream reqStream = request.GetRequestStream();
        reqStream.Write(buff, 0, buff.Length);

        //Miracle delay (without delay between 2 requests an exeption "The server committed a protocol violation. Section=ResponseStatusLine" is thrown). AntonShtern
        System.Threading.Thread.Sleep(5000); 
    }

    /// <summary>
    /// Add cookies to the request object
    /// </summary>
    private void AddCookiesTo(HttpWebRequest request)
    {
        if (Cookies != null && Cookies.Count > 0)
        {
            if (request.CookieContainer == null)
            {
                request.CookieContainer = new CookieContainer();
            }
            request.CookieContainer.Add(Cookies);
        }
    }

    /// <summary>
    /// Saves cookies from the response object to the local CookieCollection object
    /// </summary>
    private void SaveCookiesFrom(HttpWebResponse response)
    {
        if (response.Cookies.Count > 0)
        {
            if (Cookies == null) Cookies = new CookieCollection();
            Cookies.Add(response.Cookies);
        }
    }

    /// <summary>
    /// Saves the form elements collection by parsing the HTML document
    /// </summary>
    private void SaveHtmlDocument(HtmlDocument document)
    {
        _htmlDoc = document;
        FormElements = new FormElementCollection(_htmlDoc);
    }
}

/// <summary>
/// Represents a combined list and collection of Form Elements.
/// </summary>
public class FormElementCollection : Dictionary<string, string>
{
    /// <summary>
    /// Constructor. Parses the HtmlDocument to get all form input elements. 
    /// </summary>
    public FormElementCollection(HtmlDocument htmlDoc)
    {
        IEnumerable<HtmlNode> inputs = htmlDoc.DocumentNode.Descendants("input");
        foreach (HtmlNode element in inputs)
        {
            string name = element.GetAttributeValue("name", "undefined");
            string value = element.GetAttributeValue("value", "");
            if (!name.Equals("undefined")) Add(name, value);
        }

    }

    /// <summary>
    /// Assembles all form elements and values to POST. Also html encodes the values.  
    /// </summary>
    public string AssemblePostPayload()
    {
        StringBuilder sb = new StringBuilder();
        foreach (KeyValuePair<string, string> element in this)
        {
            string value = HttpUtility.UrlEncode(element.Value);
            sb.Append("&" + element.Key + "=" + value);
        }
        return sb.ToString().Substring(1);
    }
}

