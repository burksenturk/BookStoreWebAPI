using FluentValidation;
using WebApi.Application.GenreOperations.Queries.GetGenreDetail;

namespace WebApi.Application.GenreOperations.Commands.CreateGenre
{

        public class CreateGenreCommandValidator : AbstractValidator<CreateGenreCommand>
        {
            //dışarıdan model aldık
            public CreateGenreCommandValidator()
            {
                 RuleFor(command => command.Model.Name).NotEmpty().MinimumLength(4);
            }
        }
    
}
