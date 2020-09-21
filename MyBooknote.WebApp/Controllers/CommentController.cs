using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBooknote.WebApp.Controllers
{
    public class CommentController : Controller
    {
        public ActionResult ShowNoteComments()
        {
            return PartialView("_PartialComment");
        }
    }
}