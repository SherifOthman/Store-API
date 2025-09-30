namespace OnlineStore.Domain.Enums;

public enum RoleValue
{
    Customer = 1 << 0,
    Staff    = 1 << 1,
    Manager  = 1 << 2,
    Admin    = 1 << 3,
}
