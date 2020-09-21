using BookNoteCommon.Helpers;
using MyBooknote.DataAccessLayer.EntityFramework;
using MyBooknote.Entities;
using MyBooknote.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBooknote.BusinessLayer
{
    public class BookNoteUserManager :ManagerBase<BooknoteUser>
    {
        public BusinessLayerResult<BooknoteUser> RegisterUser(RegisterViewModel data)
        {
            BooknoteUser user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<BooknoteUser> layerResult = new BusinessLayerResult<BooknoteUser>();

            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    layerResult.Errors.Add("Kullanıcı adı zaten kayıtlı!");
                }
                if (user.Email == data.Email)
                {
                    layerResult.Errors.Add("E poasta adresi zaten kayıtlı!");
                }
            }
            else
            {
                DateTime now = DateTime.Now;
                int dbResult = base.Insert(new BooknoteUser
                {
                    Username = data.Username,
                    Email = data.Email,
                    Password = data.Password,
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = false,
                    IsAdmin = false
                });

                if (dbResult > 0)
                {
                    layerResult.Result = Find(x => x.Username == data.Username && x.Email == data.Email);

                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/UserActivate/{layerResult.Result.ActivateGuid}";
                    string body = $"Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank'>tıklayınız</a>";
                    MailHelper.SendMail(body, layerResult.Result.Email, "Hesap aktifleştirme..");
                }
            }

            return layerResult;
        }

        public BusinessLayerResult<BooknoteUser> UpdateProfile(BooknoteUser data)
        {
            BooknoteUser db_user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<BooknoteUser> res = new BusinessLayerResult<BooknoteUser>();

            if (db_user != null && db_user.Id != data.Id)
            {
                if (db_user.Username == data.Username)
                {
                    res.Errors.Add("Kullanıcı adı zaten kayıtlı!");
                }
                if (db_user.Email == data.Email)
                {
                    res.Errors.Add("E poasta adresi zaten kayıtlı!");
                }
                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;
            Save();

            return res;
        }

        public BusinessLayerResult<BooknoteUser> RemoveUserById(int id)
        {
            BusinessLayerResult<BooknoteUser> res = new BusinessLayerResult<BooknoteUser>();
            BooknoteUser user = Find(x => x.Id == id);

            if (user != null)
            {
                if (Delete(user) == 0)
                {
                    res.Errors.Add("Kullanıcı silinemedi !");
                }
            }
            else
            {
                res.Errors.Add("Kullanıcı bulunamadı !");
            }
            Save();
            return res;
        }

        public BusinessLayerResult<BooknoteUser> GetUserById(int id)
        {
            BusinessLayerResult<BooknoteUser> res = new BusinessLayerResult<BooknoteUser>();
            res.Result = Find(x => x.Id == id);

            if (res.Result == null)
            {
                res.Errors.Add("Kullanıcı bulunamadı!");
            }

            return res;
        }

        public BusinessLayerResult<BooknoteUser> LoginUser(LoginViewModel data)
        {
            BusinessLayerResult<BooknoteUser> layerResult = new BusinessLayerResult<BooknoteUser>();
            layerResult.Result = Find(x => x.Username == data.Username && x.Password == data.Password);

            if (layerResult.Result != null)
            {
                if (!layerResult.Result.IsActive)
                {
                    layerResult.Errors.Add("Kullanıcı aktif değil. Lütfen Eposta adresinize gönderilen aktivasyon mailini kontrol ediniz.");
                }
            }
            else
            {
                layerResult.Errors.Add("Kullanıcı adı yada şifre uyuşmuyor. Lütfen tekrar deneyiniz.");
            }

            return layerResult;
        }

        public BusinessLayerResult<BooknoteUser> AktivateUser(Guid activateId)
        {
            BusinessLayerResult<BooknoteUser> res = new BusinessLayerResult<BooknoteUser>();
            res.Result = Find(x => x.ActivateGuid == activateId);

            if (res.Result != null)
            {
                if (res.Result.IsActive)
                {
                    res.Errors.Add("Kullanıcı zaten daha önce aktif edilmiş.");
                    return res;
                }

                res.Result.IsActive = true;
                Update(res.Result);
            }
            else
            {
                res.Errors.Add("Aktif edilecek kullanıcı bulunamadı.");
            }

            return res;
        }

        //method hiding..
        public new BusinessLayerResult<BooknoteUser> Insert(BooknoteUser data)
        {
            BooknoteUser user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<BooknoteUser> layerResult = new BusinessLayerResult<BooknoteUser>();

            layerResult.Result = data;

            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    layerResult.Errors.Add("Kullanıcı adı zaten kayıtlı!");
                }
                if (user.Email == data.Email)
                {
                    layerResult.Errors.Add("E poasta adresi zaten kayıtlı!");
                }
            }
            else
            {
                layerResult.Result.ActivateGuid = Guid.NewGuid();

                DateTime now = DateTime.Now;
                if (base.Insert(layerResult.Result) == 0)
                {
                    layerResult.Errors.Add("Kullanıcı kayıdı başarısız !");
                }
            }

            return layerResult;
        }

        public new BusinessLayerResult<BooknoteUser> Update(BooknoteUser data)
        {
            BooknoteUser db_user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<BooknoteUser> res = new BusinessLayerResult<BooknoteUser>();
            res.Result = data;

            if (db_user != null && db_user.Id != data.Id)
            {
                if (db_user.Username == data.Username)
                {
                    res.Errors.Add("Kullanıcı adı zaten kayıtlı!");
                }
                if (db_user.Email == data.Email)
                {
                    res.Errors.Add("E poasta adresi zaten kayıtlı!");
                }
                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;
            res.Result.IsActive = data.IsActive;
            res.Result.IsAdmin = data.IsAdmin;
            Save();

            return res;
        }

    }
}
