using MongoDB.Driver;
using CsvHelper;
using System.Formats.Asn1;
using System.Globalization;

class BankOperations
{
    private readonly IMongoCollection<BankAccount> _accountCollection;
    private readonly IMongoCollection<Customer> _customerCollection;
    private BankAccount _currentAccount;

    public BankOperations()
    {
        var client = new MongoClient("mongodb://localhost:27017");
        var database = client.GetDatabase("bankingapp");
        _accountCollection = database.GetCollection<BankAccount>("accounts");
        _customerCollection = database.GetCollection<Customer>("customers");
    }

    // Create a new customer
    public Customer CreateCustomer(string customerName)
    {
        var customer = new Customer
        {
            CustomerName = customerName
        };

        _customerCollection.InsertOne(customer);
        return customer;
    }

    // Set the current account
    public void SetCurrentAccount(BankAccount account)
    {
        _currentAccount = account;
    }

    // Get the current account
    public BankAccount GetCurrentAccount()
    {
        return _currentAccount;
    }

    // Perform deposit operation
    public void Deposit(decimal amount)
    {
        if (_currentAccount != null)
        {
            _currentAccount.Balance += amount;
            UpdateAccount(_currentAccount);
        }
    }

    // Perform withdrawal operation
    public void Withdraw(decimal amount)
    {
        if (_currentAccount != null)
        {
            if (amount <= _currentAccount.Balance)
            {
                _currentAccount.Balance -= amount;
                UpdateAccount(_currentAccount);
            }
        }
    }
    public void ExportAccountsToCsv(string filePath)
    {
        var accounts = _accountCollection.Find(FilterDefinition<BankAccount>.Empty).ToList();

        using (var writer = new StreamWriter(filePath))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(accounts);
        }
    }

    // Import accounts from a CSV file
    public void ImportAccountsFromCsv(string filePath)
    {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var accounts = csv.GetRecords<BankAccount>().ToList();
            _accountCollection.InsertMany(accounts);
        }
    }

    // Update an account
    private void UpdateAccount(BankAccount account)
    {
        _accountCollection.ReplaceOne(Builders<BankAccount>.Filter.Eq(a => a.Id, account.Id), account);
    }
}
