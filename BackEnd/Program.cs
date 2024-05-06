using System.Numerics;
using Microsoft.AspNetCore.Http.HttpResults;
using MySql.Data.MySqlClient;
using Mysqlx;

string connectionString = "server=localhost;uid=root;pwd=;database=inventorymanagement";
MySqlConnection conn = new MySqlConnection();
conn.ConnectionString = connectionString;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var products = new List<Product>(); 
var employees = new List<Employee>(); 
var customers = new List<Customer>();

app.MapGet("/list_emp", () => {
    conn.Open();
    string query = "SELECT * FROM EMPLOYEES";
    MySqlCommand cmd = new MySqlCommand(query, conn);
    MySqlDataReader reader = cmd.ExecuteReader();
    while(reader.Read()){
        Employee emp = new Employee((ulong)reader["EmpID"], (string)reader["EmpName"], (string)reader["phone"], (string)reader["password"]);
        employees.Add(emp);
    }
    conn.Close();
    return employees;
});

// ~~~~~~~~~~~~~~~~ CUSTOMER MANAGEMENT ~~~~~~~~~~~~~~~~

// Retrieve customer based on phone number
app.MapGet("/list_cust/{phone}", (string phone) => {
    customers.Clear();
    conn.Open();
    string query = "SELECT * FROM CUSTOMERS";
    MySqlCommand cmd = new MySqlCommand(query, conn);
    MySqlDataReader reader = cmd.ExecuteReader();
    while(reader.Read()){
        Customer cust = new Customer((ulong)reader["CustID"], (string)reader["custName"], (string)reader["phone"], (string)reader["email"], (string)reader["address"]);
        if (phone.Equals("all")){
            Console.WriteLine("all");
            customers.Add(cust);
        }
        else{
            if (cust.phone.Contains(phone)){
                Console.WriteLine(cust.phone + " " + phone);
                customers.Add(cust);
            }
        }
    }
    conn.Close();
    return customers;
});

// Add New Customer
app.MapPost("/add_cust", Results<Created<Customer>, NotFound<string>> (Customer cust) => {
    conn.Open();
    string query = $"INSERT INTO CUSTOMERS (custName, phone, email, address) values ('{cust.name}', '{cust.phone}', '{cust.mail}', '{cust.address}')";
    MySqlCommand cmd = new MySqlCommand(query, conn);
    int rowsAffected = cmd.ExecuteNonQuery();
    conn.Close();
    if (rowsAffected == 1){
        return TypedResults.Created("Customer created successfully!", cust);
    }
    else{
        return TypedResults.NotFound("Error occurred. Please try later.");
    }
});

// Delete Customer By ID
app.MapDelete("/del_cust/{id}", Results<NoContent, NotFound<string>> (int id) => {
    conn.Open();
    string query = $"DELETE FROM CUSTOMERS WHERE custID = {id}";
    MySqlCommand cmd = new MySqlCommand(query, conn);
    
    int rowsAffected = cmd.ExecuteNonQuery();
    conn.Close();
    if (rowsAffected >= 1){
        return TypedResults.NoContent();
    }
    else{
        return TypedResults.NotFound("Error occurred. Please try again later.");
    }
});

// ~~~~~~~~~~~~~~~~ CUSTOMER MANAGEMENT ~~~~~~~~~~~~~~~~

// ~~~~~~~~~~~~~~~~ PRODUCT MANAGEMENT ~~~~~~~~~~~~~~~~
// List All Products
app.MapGet("/list_prods", () => {
    products.Clear();
    conn.Open();
    string query = "SELECT * FROM PRODUCTS";
    MySqlCommand cmd = new MySqlCommand(query, conn);
    MySqlDataReader reader = cmd.ExecuteReader();
    while(reader.Read()){
        Product prod = new Product((ulong)reader["ProductID"], (string)reader["name"], (string)reader["Description"], Convert.ToDecimal(reader["UnitPrice"]), (int)reader["StockQuantity"], Convert.ToDecimal(reader["Discount"]));
        products.Add(prod);
    }
    conn.Close();
    return products;
});

// Add New Product
app.MapPost("/add_prod", Results<Created<Product>, NotFound<string>> (Product prod) => {
    conn.Open();
    string query = $"INSERT INTO PRODUCTS (Name, Description, UnitPrice, StockQuantity, Discount) values ('{prod.name}', '{prod.desc}', '{prod.unitPrice}', '{prod.qty}', '{prod.discount}')";
    MySqlCommand cmd = new MySqlCommand(query, conn);
    int rowsAffected = cmd.ExecuteNonQuery();
    conn.Close();
    if (rowsAffected == 1){
        return TypedResults.Created("Customer created successfully!", prod);
    }
    else{
        return TypedResults.NotFound("Error occurred. Please try later.");
    }
});

// Delete Product By ID
app.MapDelete("/del_prod/{id}", Results<NoContent, NotFound<string>> (int id) => {
    conn.Open();
    string query = $"DELETE FROM PRODUCTS WHERE ProductID = {id}";
    MySqlCommand cmd = new MySqlCommand(query, conn);
    int rowsAffected = cmd.ExecuteNonQuery();
    conn.Close();
    if (rowsAffected >= 1){
        return TypedResults.NoContent();
    }
    else{
        return TypedResults.NotFound("Error occurred. Please try again later.");
    }
});

// Inward Inventory
app.MapPost("/inflow/{id}/{qty}", Results<Created<string>, NotFound<string>>(ulong id, int qty) => {
    conn.Open();
    List<ulong> pids = new List<ulong>();
    string query = $"SELECT PRODUCTID FROM PRODUCTS";
    MySqlCommand cmd = new MySqlCommand(query, conn);
    MySqlDataReader reader = cmd.ExecuteReader();
    while(reader.Read()){
        pids.Add((ulong)reader["ProductID"]);
    }
    reader.Close();
    if (pids.Contains(id)){
        string query1 = $"INSERT INTO INWARD (ProductID, Qty) VALUES ({id}, {qty})";
        MySqlCommand cmd1 = new MySqlCommand(query1, conn);
        int rowsAffected = cmd1.ExecuteNonQuery();
        conn.Close();
        if (rowsAffected >= 1){
            return TypedResults.Created("Stock Updated Successfully", $"{id} {qty}");
        }
        else{
            return TypedResults.NotFound("Error occurred. Please try again later.");
        }    
    }
    else{
        conn.Close();
        return TypedResults.NotFound("Error occurred. Please try again later.");
    }
});

// ~~~~~~~~~~~~~~~~ PRODUCT MANAGEMENT ~~~~~~~~~~~~~~~~

app.Run();

public record Employee(ulong Id, string name, string phone, string pwd);

public record Customer(ulong Id, string name, string phone, string mail, string address);

public record Product(ulong Id, string name, string desc, Decimal unitPrice, int qty, Decimal discount);

public record InflowRecord(ulong Id, int qty);