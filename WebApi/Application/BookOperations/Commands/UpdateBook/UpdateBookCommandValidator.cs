using FluentValidation;

namespace WebApi.Applicationi.BookOperations.Commands.UpdateBook
{
    //BUNLAR HEP FLUENT VALİDATİON KÜTÜPHANESİ ÖZELLİKLERİ
    public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
    {
        public UpdateBookCommandValidator()
        {
            RuleFor(command => command.BookId).GreaterThan(0);  //update de hem BookId var Hemde Model var valide edilmesi gereken
            RuleFor(command => command.Model.GenreId).GreaterThan(0);
            RuleFor(command => command.Model.Title).NotEmpty().MinimumLength(4);
        }

    }
}
