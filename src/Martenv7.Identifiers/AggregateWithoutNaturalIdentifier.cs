using Marten;

namespace Martenv7.Identifiers;

[Collection("Marten collection")]
public class AggregateWithoutNaturalIdentifier(MartenFixture fixture)
{
    private IDocumentStore Store { get; } = fixture.Store;

    [Fact]
    public async Task ReplaysAsIntended()
    {
        var id = Guid.NewGuid();
        
        var session1 = Store.LightweightSession();
        session1.Events.StartStream<EntityWithoutNaturalId>(
            id,
            new EntityCreated("created"),
            new ContentAdded("more"));
        await session1.SaveChangesAsync();
        
        var session2 = Store.LightweightSession();

        var entity = await session2.Events.AggregateStreamAsync<EntityWithoutNaturalId>(id);
        
        Assert.NotNull(entity);
        Assert.Equal("created", entity.InitialContent);
        Assert.Equal("more", entity.MoreContent);
    }
}