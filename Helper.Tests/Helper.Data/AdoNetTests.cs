using Moq;
using SharedTestData;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Xunit;

[Trait("Data", "ADO.Net Tests")]
public class ADONetContextTests
{
    private Mock<IADOConnectionFactory> adoConnectionFactory;

    private ADONetContext AdoNet;

    private Mock<IDataReader> MockReader(Person person, int rows)
    {
        const string Column1 = "Age";
        const string Column2 = "FullName";
        const string Column3 = "Id";

        var dataReader = new Mock<IDataReader>();

        dataReader.Setup(m => m.FieldCount).Returns(3);
        dataReader.Setup(m => m.GetName(0)).Returns(Column1);
        dataReader.Setup(m => m.GetName(1)).Returns(Column2);
        dataReader.Setup(m => m.GetName(2)).Returns(Column3);

        dataReader.Setup(m => m.GetFieldType(0)).Returns(typeof(int));
        dataReader.Setup(m => m.GetFieldType(1)).Returns(typeof(string));
        dataReader.Setup(m => m.GetFieldType(2)).Returns(typeof(int));

        dataReader.Setup(m => m.GetOrdinal("First")).Returns(0);
        dataReader.Setup(m => m.GetValue(0)).Returns(person.Age);
        dataReader.Setup(m => m.GetValue(1)).Returns(person.FullName);
        dataReader.Setup(m => m.GetValue(2)).Returns(person.Id);

        dataReader.SetupSequence(m => m.Read())
            .Returns(true)
            .Returns(rows >= 2 ? true : false)
            .Returns(rows >= 4 ? true : false);

        return dataReader;
    }

    private void SetupMock(DBCommand dBCommand, Person person = null, int rows = 1)
    {
        var mock = new Mock<IDbCommand>();
        switch (dBCommand)
        {
            case DBCommand.Reader:
                mock.Setup(m => m.ExecuteReader()).Returns(MockReader(person, rows).Object).Verifiable();
                break;
            case DBCommand.ExecuteNoneQuery:
                mock.Setup(m => m.ExecuteNonQuery()).Returns(1).Verifiable();
                break;
            default:
                break;
        }

        var connectionMock = new Mock<IDbConnection>();
        connectionMock.Setup(m => m.CreateCommand()).Returns(mock.Object);

        adoConnectionFactory = new Mock<IADOConnectionFactory>();
        adoConnectionFactory.Setup(x => x.CreateConnection(It.IsAny<string>())).Returns(connectionMock.Object);

        AdoNet = new ADONetContext(adoConnectionFactory.Object, "Server=myServerName,myPortNumber;Database=myDataBase;User Id=myUsername;Password=myPassword;");
    }

    [Fact(DisplayName = "Execute non query with row/s effected.")]
    public async Task ADO_ExecuteNonQueryAsync_ReturnsTrue()
    {
        SetupMock(DBCommand.ExecuteNoneQuery);

        var result = await AdoNet.ExecuteNonQueryAsync<Person>("Insert into dbo.Person set ");

        Assert.True(result);
    }

    public static IEnumerable<object[]> Data =>
        new List<Person[]>()
        {
            new Person[] {  new Person { Id = 1, FullName = "James Smith", Age = 21 } },
        };

    [Theory(DisplayName = "Read a Single row and map to class.")]
    [MemberData(nameof(Data))]
    public async Task ADO_ExecuteReaderSingleAsync_ReturnObject(Person person)
    {
        SetupMock(DBCommand.Reader, person,1);

        var result = await AdoNet.ExecuteReaderSingleAsync<Person>("SELECT TOP 1 * FROM dbo.Person");

        Assert.Equal(person, result);
    }

    private List<Person> Expected => new List<Person>() {
            new Person {  Id = 1, FullName = "James Smith", Age = 21 },
            new Person {  Id = 1, FullName = "James Smith", Age = 21 } };

    [Theory(DisplayName = "Read multiple rows and map to class collection.")]
    [MemberData(nameof(Data))]
    public async Task ADO_ExecuteReaderCollectionAsync_ReturnsCollection(Person person)
    {
        SetupMock(DBCommand.Reader, person, 2);

        var result = await AdoNet.ExecuteReaderCollectionAsync<Person>("SELECT * FROM dbo.Person");

        Assert.Equal(Expected, result);
    }
}

