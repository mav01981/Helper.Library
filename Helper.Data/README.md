# Helper.Data

## **ADO .Net**

**Implementation in Startup**

```c#
services.AddScoped<IADOConnectionFactory, ADOConnectionFactory>();
services.AddScoped<IADONetContext>(x => new ADONetContext(x.GetRequiredService<IADOConnectionFactory>(), "Server = myServerName, myPortNumber; Database = myDataBase; User Id = myUsername; Password = myPassword;"));
```
**Implement in Repository** 

```c#
public class DataRepository : IDataRepository
{
    private readonly IADONetContext _adoNetContext;

    public DataRepository(IADONetContext adoNetContext)
    {
        _adoNetContext = adoNetContext;
    }
}
```

# **EF ContextFactory**

```c#
using(var context = factory.Create(DataSource.Sql, $"Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;"))
{
    //TODO: Execute actions.
}
```
# **EF Generic Repository and Unit of work**