using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyBooknote.Entities.ValueObjects
{
    public class LoginViewModel
    {
        [DisplayName("Kullanıcı adı"),Required(ErrorMessage ="{0} alanı boş olamaz..")]
        public string Username { get; set; }
        [DisplayName("Şifre"), Required(ErrorMessage = "{0} alanı boş olamaz.."),DataType(DataType.Password)]
        public string Password { get; set; }

    }
}