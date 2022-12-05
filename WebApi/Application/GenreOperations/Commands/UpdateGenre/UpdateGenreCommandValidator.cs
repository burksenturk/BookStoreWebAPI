using FluentValidation;
using WebApi.Application.GenreOperations.Commands.CreateGenre;


namespace WebApi.Application.GenreOperations.Commands.UpdateGenre
{
    public class UpdateGenreCommandValidator : AbstractValidator<UpdateGenreCommand>
    {
        //dışarıdan model aldık
        public UpdateGenreCommandValidator()
        {
            //minlength 4 olsun ama boş gelmezse 4 olsun .. aşağıda minlength kuralını when li koşula bağladık
            RuleFor(command => command.Model.Name).MinimumLength(4).When(x => x.Model.Name != string.Empty);
        }
    }
}
