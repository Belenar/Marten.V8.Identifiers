namespace Martenv7.Identifiers;

public class EntityWithoutNaturalId
{
    public Guid OtherId { get; set; }
    public string? InitialContent { get; set; }
    public string? MoreContent { get; set; }
    
    public void Apply(EntityCreated @event)
    {
        InitialContent = @event.Content;
    }

    public void Apply(ContentAdded @event)
    {
        MoreContent = @event.Content;
    }
}