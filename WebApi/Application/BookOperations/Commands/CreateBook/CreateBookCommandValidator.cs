using FluentValidation;
using System;

namespace WebApi.Application.BookOperations.Commands.CreateBook
{
    //komple command ın inputlarını valide etmek istiyoruz CreateBookCommand a n tane modelde gelse  tüm modelleri valide edebilecek bir validasyon sınıfı oluşturduk
    public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>  // CreateBookCommandValidator classı CreateBookCommand ın nesnelerini objelerini valide eder 
    {
        public CreateBookCommandValidator()  //validasyon constructor aracılıgı ile çalısıyor..
        {
            RuleFor(command => command.Model.GenreId).GreaterThan(0);  //genreId 0 dan büyük girilebilir validasyonu koyduk
            RuleFor(command => command.Model.PageCount).GreaterThan(0);
            RuleFor(command => command.Model.PublishDate.Date).NotEmpty().LessThan(DateTime.Now.Date);  //boş olamaz ve bugünden küçük olmalı....     .date koyarak saati kırpttık 
            RuleFor(command => command.Model.Title).NotEmpty().MinimumLength(4);  //x,y,x vermemin sebebi hepsinde oluyor oldugunu göstermek..

        }
    }
}
