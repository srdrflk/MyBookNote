using BookNoteCommon;
using MyBooknote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBooknote.WebApp.Init
{
    public class WebCommon : ICommon
    {
        public string GetCurrentUserName()
        {
            if (HttpContext.Current.Session["login"] != null)
            {
                BooknoteUser user = HttpContext.Current.Session["login"] as BooknoteUser;
                return user.Username;
            }
            return "system";
        }
    }
}