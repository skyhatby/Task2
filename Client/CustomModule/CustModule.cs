using System;
using System.Collections.Concurrent;
using System.Web;

namespace Client.CustomModule
{
    public class CustModule : IHttpModule
    {
        private MyEventHandler _eventHandler;
        private static readonly ConcurrentDictionary<string, int> Hits = new ConcurrentDictionary<string, int>();
        public void Dispose()
        { }
        public delegate void MyEventHandler(Object s, EventArgs e);

        public void Init(HttpApplication context)
        {
            context.BeginRequest += OnBeginRequest;
        }
        public event MyEventHandler MyEvent
        {
            add { _eventHandler += value; }
            remove { _eventHandler -= value; }
        }
        public void OnBeginRequest(Object s, EventArgs e)
        {
            var app = s as HttpApplication;
            if (app != null)
            {
                int num;
                if (!Hits.TryGetValue(app.Request.RawUrl, out num))
                    Hits[app.Request.RawUrl] = 1;
                else
                    Hits[app.Request.RawUrl] += 1;
                app.Context.Response.AddHeader("Hits", Hits[app.Request.RawUrl].ToString());
            }
            if (_eventHandler != null)
                _eventHandler(this, null);
        }
    }
}