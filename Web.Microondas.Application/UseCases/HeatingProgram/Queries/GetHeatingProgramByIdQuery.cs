namespace Web.Microondas.Application.UseCases.HeatingProgram.Queries;

public class GetHeatingProgramByIdQuery
{
    public Guid Id { get; set; }
    public GetHeatingProgramByIdQuery(Guid id) => Id = id;
}
