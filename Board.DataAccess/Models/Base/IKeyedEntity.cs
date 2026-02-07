namespace Board.DataAccess.Models.Base;

public interface IKeyedEntity<TKey> : IKeyedEntity
{
    TKey Id { get; init; }
}

public interface IKeyedEntity
{
}
