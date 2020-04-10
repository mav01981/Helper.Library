using Moq;
using SharedTestData;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Xunit;

[Trait("Data", "ADO.Net Tests")]
public class ADONetContextTests
{
    private Mock<IDataReader> mockReader()
    {
        const string Column1 = "First";
        const string Column2 = "Second";
        const string ExpectedValue1 = "Value1";
        const string ExpectedValue2 = "Value1";

        var dataReader = new Mock<IDataReader>();

        dataReader.Setup(m => m.FieldCount).Returns(2);
        dataReader.Setup(m => m.GetName(0)).Returns(Column1);
        dataReader.Setup(m => m.GetName(1)).Returns(Column2);

        dataReader.Setup(m => m.GetFieldType(0)).Returns(typeof(string));
        dataReader.Setup(m => m.GetFieldType(1)).Returns(typeof(string));

        dataReader.Setup(m => m.GetOrdinal("First")).Returns(0);
        dataReader.Setup(m => m.GetValue(0)).Returns(ExpectedValue1);
        dataReader.Setup(m => m.GetValue(1)).Returns(ExpectedValue2);

        dataReader.SetupSequence(m => m.Read())
            .Returns(true)
            .Returns(true)
            .Returns(false);
        return dataReader;
    }

    private Mock<IADOConnectionFactory> adoConnectionFactory;
        
    private readonly ADONetContext AdoNet;

    public ADONetContextTests()
    {
        var sqlconnection = new Mock<SqlConnection>();
        sqlconnection.Setup(x => x.Open());
        adoConnectionFactory = new Mock<IADOConnectionFactory>();
        adoConnectionFactory.Setup(x => x.CreateConnection(It.IsAny<string>())).Returns(sqlconnection.Object);

        AdoNet = new ADONetContext(adoConnectionFactory.Object,
            "Server=myServerName,myPortNumber;Database=myDataBase;User Id=myUsername;Password=myPassword;");
    }

    [Theory(DisplayName = "Read a Single row to a simple class.")]
    [MemberData(nameof(PersonData.TestData), parameters: 1, MemberType = typeof(PersonData))]
    public async Task Create_SQL_Context(int id, string fullName, int age)
    {
        //Arrange
        var reader = mockReader();
        //Act
        var result = await AdoNet.ExecuteReaderSingleAsync<Person>("SELECT * FROM dbo.Person");
        //Assert
        Assert.Equal(result, new Person { Id = 1, FullName = "James Smith", Age = 21 });
    }
}

