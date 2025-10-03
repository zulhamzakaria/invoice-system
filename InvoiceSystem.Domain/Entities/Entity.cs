namespace InvoiceSystem.Domain.Entities;

public abstract class Entity
{
    public Guid Id { get; protected set; }
    protected Entity()
    {
        Id = Guid.NewGuid();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity entity) return false;
        if (ReferenceEquals(this, entity)) return true;
        if (GetType() != entity.GetType()) return false;

        return Id == entity.Id && Id != Guid.Empty;
    }

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(Entity left, Entity right)
    {
        if(ReferenceEquals(left, null))
        return ReferenceEquals(right,null);

        return left.Equals(right);
    }

    public static bool operator !=(Entity left, Entity right)
    {
        return !(left == right);
    }

}
