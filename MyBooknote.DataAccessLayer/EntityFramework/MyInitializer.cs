using MyBooknote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBooknote.DataAccessLayer.EntityFramework
{
    public class MyInitializer : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            BooknoteUser admin = new BooknoteUser()
            {
                Name = "Serdar",
                Surname = "Filik",
                Email = "srdrflk@hotmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                Username = "srdrflk",
                Password = "1233",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "srdrflk"
            };

            BooknoteUser standartUser = new BooknoteUser()
            {
                Name = "Katya",
                Surname = "Filik",
                Email = "kate_64@inbox.ru",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                Username = "katya",
                Password = "1233",
                CreatedOn = DateTime.Now.AddHours(1),
                ModifiedOn = DateTime.Now.AddMinutes(65),
                ModifiedUsername = "srdrflk"
            };

            context.BookNoteUsers.Add(admin);
            context.BookNoteUsers.Add(standartUser);

            for (int i = 0; i < 8; i++)
            {
                BooknoteUser user = new BooknoteUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    Username = $"user{i}",
                    Password = "123",
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUsername = $"user{i}"
                };

                context.BookNoteUsers.Add(user);
            }

            context.SaveChanges();

            List<BooknoteUser> userList = context.BookNoteUsers.ToList();

            //Fake data ekleme
            for (int i = 0; i < 10; i++)
            {
                //kategori ekleme
                Category cat = new Category()
                {
                    Title = FakeData.NameData.GetFemaleFirstName(),
                    Description = FakeData.PlaceData.GetAddress(),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedUsername = "srdrflk"
                };
                context.Categories.Add(cat);

                for (int j = 0; j < FakeData.NumberData.GetNumber(5, 9); j++)
                {
                    BooknoteUser owner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                    //note ekleme
                    Note note = new Note()
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        Category = cat,
                        IsDraft = false,
                        LikeCount = FakeData.NumberData.GetNumber(1, 9),
                        Owner = owner,
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUsername = owner.Username,
                    };
                    cat.Notes.Add(note);

                    //comment ekleme
                    for (int k = 0; k < FakeData.NumberData.GetNumber(3, 5); k++)
                    {
                        BooknoteUser comment_owner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                        Comment comment = new Comment()
                        {
                            Text = FakeData.TextData.GetSentence(),
                            Note = note,
                            Owner = comment_owner,
                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedUsername = comment_owner.Username,
                        };

                        note.Comments.Add(comment);
                    }

                    //like ekleme


                    for (int m = 0; m < note.LikeCount; m++)
                    {
                        Liked liked = new Liked()
                        {
                            LikedUser = userList[m]
                        };

                        note.Likes.Add(liked);
                    }
                }
            }

            context.SaveChanges();
        }
    }
}
