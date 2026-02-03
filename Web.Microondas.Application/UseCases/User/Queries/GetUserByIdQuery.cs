namespace Web.Microondas.Application.UseCases.User.Queries;

public class GetUserByIdQuery
{
    public Guid Id { get; set; }
    public GetUserByIdQuery(Guid id) => Id = id;
}
